using EntityFrameworkExtras.EF6;
using HeartValve.Models;
using HeartValve.Shared.Data;
using HeartValve.Shared.Models;
using HeartValve.Shared.StoredProcedures;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System;
using Kendo.Mvc.Extensions;
using System.IO;

namespace HeartValve.Controllers
{
    [Authorize(Roles = "Admin,Provider")]
    public class SurveyController : ApplicationController
    {
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Index(int surveyId)
        {
            ViewData["imagePath"] = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images/");

            var vm = new SurveyViewModel();
            var session = db.GetCurrentSurveySession(surveyId, UserId).SingleOrDefault();

            if (session == null || session.EndDate.HasValue)
            {
                session = db.AddSurveySession(surveyId, UserId).Select(x => new GetCurrentSurveySession_Result()
                {
                    SessionId = x.SessionId,
                    SurveyId = x.SurveyId,
                    UserId = x.UserId,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate
                }).SingleOrDefault();
            }

            var questions = db.GetSurveyQuestions(surveyId).ToList();
            var answers = db.GetSurveyAnswers(surveyId, UserId, session.SessionId, null, null).ToList();

            GetSurveyQuestions_Result nextQuestion;

            if (answers.Any())
            {
                var lastAnswer = answers.OrderByDescending(x => x.AnswerDate).FirstOrDefault();
                var lastQuestion = questions.SingleOrDefault(x => x.QuestionId == lastAnswer.QuestionId);

                nextQuestion = this.GetNextSurveyQuestion(lastQuestion, questions, new LogicEvaluator(UserId, db));

                if (nextQuestion != null && nextQuestion.QuestionId == lastQuestion.QuestionId)
                {
                    nextQuestion = questions.OrderBy(x => x.QuestionOrder).FirstOrDefault(x => x.QuestionOrder > nextQuestion.QuestionOrder);
                }
            }
            else
            {
                var firstQuestion = questions.SingleOrDefault(x => x.QuestionOrder == 1);

                nextQuestion = this.GetNextSurveyQuestion(firstQuestion, questions, new LogicEvaluator(UserId, db));
            }

            if (nextQuestion == null)
            {
                db.CompleteSurveySession(session.SessionId);
            }
            else
            {
                if (nextQuestion.QuestionTypeId == 1)
                {
                    vm.Options.Add(new GetSurveyQuestionOptions_Result
                    {
                        OptionText = "Yes",
                        OptionOrder = 1
                    });

                    vm.Options.Add(new GetSurveyQuestionOptions_Result
                    {
                        OptionText = "No",
                        OptionOrder = 1
                    });
                }
                else if (nextQuestion.QuestionTypeId == 2)
                {
                    vm.Options.Add(new GetSurveyQuestionOptions_Result
                    {
                        OptionText = "True",
                        OptionOrder = 1
                    });

                    vm.Options.Add(new GetSurveyQuestionOptions_Result
                    {
                        OptionText = "False",
                        OptionOrder = 1
                    });
                }
                else
                {
                    vm.Options = db.GetSurveyQuestionOptions(nextQuestion.QuestionId, null).ToList();
                }
            }

            vm.SurveyId = surveyId;
            vm.Question = nextQuestion;

            return View(vm);
        }

        [HttpPost]
        public ActionResult SubmitSurveyAnswers(int surveyId, List<HeartValve.Shared.Models.SurveyAnswer> answers)
        {
            var session = db.GetCurrentSurveySession(surveyId, UserId).SingleOrDefault();

            var sp = new AddSurveyAnswers()
            {
                SurveyId = surveyId,
                UserId = UserId,
                SessionId = session.SessionId,
                Answers = answers
            };

            db.Database.ExecuteStoredProcedure(sp);

            return null;
        }

        private GetSurveyQuestions_Result GetNextSurveyQuestion(GetSurveyQuestions_Result question, List<GetSurveyQuestions_Result> questions, LogicEvaluator evaluator)
        {
            var logic = db.GetSurveyQuestionLogic(question.QuestionId).ToList();

            if (logic.Any())
            {
                foreach (var line in logic)
                {
                    if (evaluator.Evaluate(line.Expression))
                    {
                        if (line.ActionId == 1)
                        {
                            int order = questions.SingleOrDefault(x => x.QuestionId == question.QuestionId).QuestionOrder;

                            question = this.GetNextSurveyQuestion(questions.OrderBy(x => x.QuestionOrder).FirstOrDefault(x => x.QuestionOrder > order), questions, evaluator);
                        }
                        else if (line.ActionId == 2)
                        {
                            question = this.GetNextSurveyQuestion(questions.SingleOrDefault(x => x.QuestionId == line.ActionQuestionId), questions, evaluator);
                        }
                        else if (line.ActionId == 3)
                        {
                            question = null;
                        }

                        break;
                    }
                }
            }

            return question;
        }
    }
}
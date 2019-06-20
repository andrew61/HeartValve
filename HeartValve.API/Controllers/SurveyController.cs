using EntityFrameworkExtras.EF6;
using HeartValve.Shared.Data;
using HeartValve.Shared.Models;
using HeartValve.Shared.StoredProcedures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;

namespace HeartValve.API.Controllers
{
    [RoutePrefix("api")]
    public class SurveyController : ApplicationController
    {
        [Route("Survey/{surveyId}")]
        public IHttpActionResult GetSurvey(int surveyId)
        {
            var survey = db.GetSurvey(surveyId).SingleOrDefault();

            return Ok(survey);
        }

        [Route("SurveyQuestion/{surveyId}")]
        public IHttpActionResult GetSurveyQuestion(int surveyId)
        {
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

            var questions = db.GetSurveyQuestions(surveyId).OrderBy(x => x.QuestionOrder).ToList();
            var answers = db.GetSurveyAnswers(surveyId, UserId, session.SessionId, null, null).ToList();
            var evaluator = new LogicEvaluator(UserId, db);
            GetSurveyQuestions_Result question = null;

            if (answers.Any())
            {
                var lastAnswer = answers.OrderByDescending(x => x.AnswerDate).FirstOrDefault();
                var lastQuestion = questions.SingleOrDefault(x => x.QuestionId == lastAnswer.QuestionId);
                var logic = db.GetSurveyQuestionLogic(lastQuestion.QuestionId).ToList();

                question = lastQuestion;

                foreach (var line in logic)
                {
                    if (line.ActionId == 2)
                    {
                        if (evaluator.Evaluate(line.Expression))
                        {
                            question = questions.SingleOrDefault(x => x.QuestionId == line.ActionQuestionId);
                            break;
                        }
                    }
                    else if (line.ActionId == 3)
                    {
                        if (evaluator.Evaluate(line.Expression))
                        {
                            question = null;
                            break;
                        }
                    }
                }

                if (question != null && question.Equals(lastQuestion))
                {
                    question = questions.FirstOrDefault(x => x.QuestionOrder > question.QuestionOrder);
                }
            }
            else
            {
                question = questions.SingleOrDefault(x => x.QuestionOrder == 1);
            }

            if (question != null)
            {
                var logic = db.GetSurveyQuestionLogic(question.QuestionId).ToList();

                while (logic.Any())
                {
                    var lastQuestion = question;

                    foreach (var line in logic)
                    {
                        if (line.ActionId == 1)
                        {
                            if (evaluator.Evaluate(line.Expression))
                            {
                                question = questions.FirstOrDefault(x => x.QuestionOrder > question.QuestionOrder);
                                break;
                            }
                        }
                    }

                    if (question != null && !question.Equals(lastQuestion))
                    {
                        logic = db.GetSurveyQuestionLogic(question.QuestionId).ToList();
                    }
                    else
                    {
                        logic.Clear();
                    }
                }
            }

            if (question == null)
            {
                db.CompleteSurveySession(session.SessionId);
            }

            return Ok(question);
        }

        [Route("SurveyQuestionOptions/{questionId}")]
        public IHttpActionResult GetSurveyQuestionOptions(int questionId)
        {
            var options = db.GetSurveyQuestionOptions(questionId, null);

            return Ok(options);
        }

        [HttpPost]
        [Route("SurveyAnswers/{surveyId}/{questionId}")]
        public IHttpActionResult PostSurveyAnswers(int surveyId, int questionId, List<HeartValve.Shared.Models.SurveyAnswer> answers)
        {
            var session = db.GetCurrentSurveySession(surveyId, UserId).SingleOrDefault();

            var sp = new AddSurveyAnswers()
            {
                SurveyId = surveyId,
                UserId = UserId,
                SessionId = session.SessionId,
                QuestionId = questionId,
                Answers = answers
            };

            db.Database.ExecuteStoredProcedure(sp);

            return Ok();
        }

        [HttpPost]
        [Route("SurveyImage/{surveyId}/{questionId}")]
        public async Task<HttpResponseMessage> PostSurveyImage(int surveyId, int questionId)
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var session = db.GetCurrentSurveySession(surveyId, UserId).SingleOrDefault();
            var answers = new List<HeartValve.Shared.Models.SurveyAnswer>();
            string root = HttpContext.Current.Server.MapPath("~/App_Data");
            var provider = new MultipartFormDataStreamProvider(root);

            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }

            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);

                string localPath = HostingEnvironment.MapPath("~/Images");

                if (!Directory.Exists(localPath))
                {
                    Directory.CreateDirectory(localPath);
                }

                foreach (var file in provider.FileData)
                {
                    string fileName = string.Format("{0}_{1}.jpg", Guid.NewGuid().ToString().Replace("-", string.Empty), DateTime.Now.ToString("MMddyyyy"));
                    File.Move(file.LocalFileName, Path.Combine(localPath, fileName));
                    answers.Add(new HeartValve.Shared.Models.SurveyAnswer() { AnswerText = fileName });
                }

                var sp = new AddSurveyAnswers()
                {
                    SurveyId = surveyId,
                    UserId = UserId,
                    SessionId = session.SessionId,
                    QuestionId = questionId,
                    Answers = answers
                };

                db.Database.ExecuteStoredProcedure(sp);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }
    }
}
using HeartValve.Shared.Data;
using NCalc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HeartValve.Shared.Models
{
    public class LogicEvaluator
    {
        private string userId;
        private HeartValveEntities db;

        public LogicEvaluator(string userId, HeartValveEntities db)
        {
            this.userId = userId;
            this.db = db;
        }

        public bool Evaluate(string expression)
        {
            Expression e = new Expression(expression);
            e.EvaluateFunction += e_EvaluateFunction;
            return (bool)e.Evaluate();
        }

        private void e_EvaluateFunction(string name, FunctionArgs args)
        {
            switch (name)
            {
                case "AVERAGE":
                    {
                        args.Result = args.EvaluateParameters().Where(x => x.ToString() != "").Select(x => decimal.Parse(x.ToString())).Average();
                        break;
                    }
                case "SUM":
                    {
                        args.Result = args.EvaluateParameters().Where(x => x.ToString() != "").Select(x => decimal.Parse(x.ToString())).Sum();
                        break;
                    }
                case "IN":
                    {
                        var parameters = args.EvaluateParameters();
                        var value = (int)parameters[0];
                        var arguments = parameters[1].ToString().Split(',');
                        bool result = false;
                        foreach (var argument in arguments)
                        {
                            int arg;
                            if (int.TryParse(argument, out arg))
                            {
                                if (arg == value)
                                {
                                    result = true;
                                    break;
                                }
                            }
                        }
                        args.Result = result;
                        break;
                    }
                case "SURVEYANSWER":
                    {
                        args.Result = "";

                        var parameters = args.EvaluateParameters();
                        var surveyId = (int)parameters[0];
                        var questionId = (int)parameters[1];
                        var categoryId = parameters.Length > 2 ? (int)parameters[2] : (int?)null;
                        var session = db.GetCurrentSurveySession(surveyId, userId).SingleOrDefault();

                        if (session != null)
                        {
                            var answer = db.GetSurveyAnswers(surveyId, this.userId, session.SessionId, questionId, categoryId).OrderByDescending(x => x.AnswerDate).FirstOrDefault();

                            if (answer != null)
                            {
                                switch (answer.QuestionTypeId)
                                {
                                    case 1:
                                        args.Result = answer.OptionId.GetValueOrDefault();
                                        break;
                                    case 2:
                                        args.Result = answer.OptionId.GetValueOrDefault();
                                        break;
                                    case 3:
                                        args.Result = answer.OptionId.GetValueOrDefault();
                                        break;
                                    case 4:
                                        args.Result = answer.AnswerText;
                                        break;
                                    case 5:
                                        args.Result = decimal.Parse(answer.AnswerText);
                                        break;
                                    case 6:
                                        args.Result = answer.OptionId.GetValueOrDefault();
                                        break;
                                    case 7:
                                        args.Result = answer.OptionId.GetValueOrDefault();
                                        break;
                                    case 9:
                                        args.Result = DateTime.Parse(answer.AnswerText);
                                        break;
                                    case 10:
                                        args.Result = DateTime.Parse(answer.AnswerText);
                                        break;
                                    case 11:
                                        args.Result = answer.OptionId.GetValueOrDefault();
                                        break;
                                }
                            }
                        }

                        break;
                    }
                case "SURVEYANSWERVALUE":
                    {
                        args.Result = "";

                        var parameters = args.EvaluateParameters();
                        var surveyId = (int)parameters[0];
                        var questionId = (int)parameters[1];
                        var session = db.GetCurrentSurveySession(surveyId, userId).SingleOrDefault();

                        if (session != null)
                        {
                            var answer = db.GetSurveyAnswers(surveyId, this.userId, session.SessionId, questionId, null).OrderByDescending(x => x.AnswerDate).FirstOrDefault();

                            if (answer != null)
                            {
                                args.Result = answer.OptionValue;
                            }
                        }

                        break;
                    }
                case "SURVEYANSWERS":
                    {
                        var parameters = args.EvaluateParameters();
                        var surveyId = (int)parameters[0];
                        var questionId = (int)parameters[1];
                        var session = db.GetCurrentSurveySession(surveyId, userId).SingleOrDefault();
                        var answers = string.Empty;
                        if (session != null)
                        {
                            db.GetSurveyAnswers(surveyId, userId, session.SessionId, questionId, null).ToList().ForEach(x =>
                            {
                                answers = answers + x.OptionId.GetValueOrDefault() + ",";
                            });
                        }
                        args.Result = answers.TrimEnd(',');
                        break;
                    }
                case "SURVEYOPTION":
                    {
                        args.Result = (int)args.EvaluateParameters()[0];
                        break;
                    }
                case "SURVEYANSWERCOUNT":
                    {
                        var parameters = args.EvaluateParameters();
                        var surveyId = (int)parameters[0];
                        var questionId = (int)parameters[1];
                        var optionId = (int)parameters[2];
                        var count = (int)parameters[3];
                        var groups = db.GetSurveyAnswers(surveyId, userId, null, questionId, null).ToList()
                            .FindConsecutiveGroups<GetSurveyAnswers_Result>(x => x.OptionId == optionId, count);
                        args.Result = groups.Any();
                        break;
                    }
                default:
                    break;
            }
        }
    }

    public static class Extensions
    {
        public static IEnumerable<IEnumerable<T>> FindConsecutiveGroups<T>(this IEnumerable<T> sequence, Predicate<T> predicate, int count)
        {
            IEnumerable<T> current = sequence;

            while (current.Count() > count)
            {
                IEnumerable<T> window = current.Take(count);

                if (window.Where(x => predicate(x)).Count() >= count)
                    yield return window;

                current = current.Skip(1);
            }
        }
    }
}
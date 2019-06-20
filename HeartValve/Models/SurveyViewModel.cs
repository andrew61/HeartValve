using HeartValve.Shared.Data;
using System.Collections.Generic;

namespace HeartValve.Models
{
    public class SurveyViewModel
    {
        public int SurveyId { get; set; }
        public GetSurveyQuestions_Result Question { get; set; }
        public List<GetSurveyQuestionOptions_Result> Options { get; set; }

        public SurveyViewModel()
        {
            this.Options = new List<GetSurveyQuestionOptions_Result>();
        }
    }
}
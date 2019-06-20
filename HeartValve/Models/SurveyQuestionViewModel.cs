using System.Collections.Generic;

namespace HeartValve.Models
{
    public class SurveyQuestionViewModel
    {
        public int QuestionId { get; set; }
        public int SurveyId { get; set; }
        public int QuestionTypeId { get; set; }
        public string Name { get; set; }
        public string QuestionText { get; set; }
        public int QuestionOrder { get; set; }
        public string ImagePath { get; set; }
        public bool Deleted { get; set; }
        public List<SurveyQuestionOptionViewModel> Options { get; set; }
    }
}
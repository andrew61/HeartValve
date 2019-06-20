namespace HeartValve.Models
{
    public class SurveyAnswerViewModel
    {
        public int QuestionId { get; set; }

        public int? CategoryId { get; set; }

        public int? OptionId { get; set; }

        public string AnswerText { get; set; }
    }
}
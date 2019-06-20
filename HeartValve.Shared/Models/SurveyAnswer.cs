using EntityFrameworkExtras.EF6;

namespace HeartValve.Shared.Models
{
    [UserDefinedTableType("SurveyAnswerType")]
    public class SurveyAnswer
    {
        [UserDefinedTableTypeColumn(1)]
        public int? CategoryId { get; set; }

        [UserDefinedTableTypeColumn(2)]
        public int? OptionId { get; set; }

        [UserDefinedTableTypeColumn(3)]
        public string AnswerText { get; set; }
    }
}
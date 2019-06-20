using System;

namespace HeartValve.Models
{
    public class SurveyQuestionOptionViewModel
    {
        public int OptionId { get; set; }
        public int QuestionId { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public string OptionText { get; set; }
        public int OptionOrder { get; set; }
        public Nullable<int> ShapeType { get; set; }
        public string Coordinates { get; set; }
        public string ImagePath { get; set; }
        public bool Deleted { get; set; }
        public string CategoryName { get; set; }
        public Nullable<int> CategoryOrder { get; set; }
    }
}
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HeartValve.Shared.Data
{
    using System;
    
    public partial class GetSurveyReport_Result
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string QuestionText { get; set; }
        public int QuestionId { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string AnswerText { get; set; }
        public System.DateTime AnswerDate { get; set; }
        public Nullable<int> OptionValue { get; set; }
        public Nullable<bool> HasCompleted { get; set; }
    }
}

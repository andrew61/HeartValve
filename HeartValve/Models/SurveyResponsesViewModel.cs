using System;
using System.Web.Mvc;
namespace HeartValve.Models
{
    public class SurveyResponsesViewModel
    {
        public string UserId { get; set; }
        public SelectList UserSelectList { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
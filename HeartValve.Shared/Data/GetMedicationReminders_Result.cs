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
    
    public partial class GetMedicationReminders_Result
    {
        public string UserId { get; set; }
        public Nullable<int> UserMedicationId { get; set; }
        public string BrandName { get; set; }
        public string GenericName { get; set; }
        public string Strength { get; set; }
        public string Unit { get; set; }
        public string Indication { get; set; }
        public string Instructions { get; set; }
        public string Dose { get; set; }
        public Nullable<int> ScheduleId { get; set; }
        public Nullable<System.DateTime> ScheduleDate { get; set; }
        public int TokenTypeId { get; set; }
        public string Token { get; set; }
    }
}

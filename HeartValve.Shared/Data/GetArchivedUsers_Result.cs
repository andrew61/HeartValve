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
    
    public partial class GetArchivedUsers_Result
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public Nullable<System.DateTime> EnrollmentDate { get; set; }
        public Nullable<int> SystolicMinThreshold { get; set; }
        public Nullable<int> SystolicMaxThreshold { get; set; }
        public Nullable<int> DiastolicMinThreshold { get; set; }
        public Nullable<int> DiastolicMaxThreshold { get; set; }
        public Nullable<int> HeartRateMinThreshold { get; set; }
        public Nullable<int> HeartRateMaxThreshold { get; set; }
        public Nullable<int> SpO2MinThreshold { get; set; }
        public Nullable<int> SpO2MaxThreshold { get; set; }
        public Nullable<decimal> WeightMinThreshold { get; set; }
        public Nullable<decimal> WeightMaxThreshold { get; set; }
        public Nullable<int> MRN { get; set; }
        public Nullable<bool> IsInStudy { get; set; }
        public bool IsActive { get; set; }
        public string Id { get; set; }
        public string Email { get; set; }
    }
}

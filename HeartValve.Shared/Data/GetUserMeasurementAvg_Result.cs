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
    
    public partial class GetUserMeasurementAvg_Result
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }
        public Nullable<int> SBPAvg { get; set; }
        public Nullable<int> DBPAvg { get; set; }
        public Nullable<int> PulseAvg { get; set; }
        public Nullable<int> MapAvg { get; set; }
        public Nullable<decimal> WeightAvg { get; set; }
        public Nullable<decimal> SpO2Avg { get; set; }
        public Nullable<bool> DiastolicBreached { get; set; }
        public Nullable<bool> SystolicBreached { get; set; }
        public Nullable<bool> HeartRateBreached { get; set; }
        public Nullable<bool> WeightBreached { get; set; }
        public Nullable<bool> SpO2Breached { get; set; }
        public Nullable<int> DiastolicMinThreshold { get; set; }
        public Nullable<int> DiastolicMaxThreshold { get; set; }
        public Nullable<int> SystolicMinThreshold { get; set; }
        public Nullable<int> SystolicMaxThreshold { get; set; }
        public Nullable<int> HeartRateMinThreshold { get; set; }
        public Nullable<int> HeartRateMaxThreshold { get; set; }
        public Nullable<decimal> WeightMinThreshold { get; set; }
        public Nullable<decimal> WeightMaxThreshold { get; set; }
        public Nullable<int> SpO2MinThreshold { get; set; }
        public Nullable<int> SpO2MaxThreshold { get; set; }
    }
}

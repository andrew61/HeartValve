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
    
    public partial class GetEquipmentItem_Result
    {
        public int EquipTypeID { get; set; }
        public string EquipDescription { get; set; }
        public string Manufacturer { get; set; }
        public string SerialNo { get; set; }
        public string ModelNo { get; set; }
        public string MacAddr { get; set; }
        public Nullable<System.DateTime> AcqDate { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<System.DateTime> RetiredDate { get; set; }
    }
}

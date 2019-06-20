
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HeartValve.Models
{
    public class RequestCheckModel
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        public string EquipmentRequester { get; set; }

        [DataType(DataType.DateTime)]
        public Nullable<DateTime> DeliveryDate { get; set; }

        public string DeliveryLocation { get; set; }
        [DataType(DataType.DateTime)]
        public Nullable<DateTime> EnrollmentDate { get; set; }
        public string BpmSize { get; set; }

        public string PatientEmail { get; set; }

        public string DeliveredBy { get; set; }
        
        public Nullable<DateTime> DeliveredDate { get; set; }

        public Nullable<bool> IsDelivered { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.Data;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
namespace HeartValve.Models
{
    public class ThresholdModel
    {
        public int? SystolicMin { get; set; }
        public int? SystolicMax { get; set; }
        public int? DiastolicMin { get; set; }
        public int? DiastolicMax { get; set; }
        public int? WeightMin { get; set; }
        public int? WeightMax { get; set; }
        public int? Sp02Min { get; set; }
        public int? Sp02Max { get; set; }
        public int? HeartRateMin { get; set; }
        public int? HeartRateMax { get; set; }
    }
}
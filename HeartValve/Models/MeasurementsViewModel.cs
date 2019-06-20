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
    public class MeasurementsViewModel
    {
        public string UserId { get; set; }
        public int WeightId { get; set; }
        public SelectList UserSelectList { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; } 
        public DataTable WeightTable { get; set; }
        public IEnumerable<decimal> WeightSeries { get; set; }
        public DataTable BloodPressureTable { get; set; }
        public IEnumerable<int> SBPSeries { get; set; }
        public IEnumerable<int> DBPSeries { get; set; }
        public IEnumerable<int> HRSeries { get; set; }
        public DataTable OxygenSaturationTable { get; set; }        
        public virtual  IEnumerable<decimal> Spo2Series { get; set; }
        public IEnumerable<int> HeartRateSeries { get; set; }
        public Random Random { get; set; }
        public List<string> Colors { get; set; }
        public ThresholdModel Thresholds { get; set; }
        //public IEnumerable<DateTime> ReadingDate { get; set; }
        public IEnumerable<String> ReadingDate { get; set; }



        public MeasurementsViewModel()
        {
            WeightTable = new DataTable();
            BloodPressureTable = new DataTable();
            OxygenSaturationTable = new DataTable();
            Random = new Random();
            Colors = new List<string>();
            Thresholds = new ThresholdModel();
        }        

        public string GetRandomColor()
        {
            string color = string.Format("#{0:X6}", Random.Next(0x1000000));

            while (Colors.Contains(color))
            {
                color = string.Format("#{0:X6}", Random.Next(0x1000000));
            }
            Colors.Add(color);

            return color;
        }
    }
    public class WeightViewModel {


        public int WeightId { get; set; }
        public string UserId { get; set; }
        public Decimal Weight { get; set; }
        public DateTime ReadingDate { get; set; }

    }
    public class BloodPressureViewModel
    {
        public int BloodPressureId { get; set; }
        public string UserId { get; set; }
        public int Systolic { get; set; }
        public int Diastolic { get; set; }
        public int Map { get; set; }
        public int HeartRate { get; set; }

        public DateTime ReadingDate { get; set; }

    }
    public class OxygenSaturationViewModel
    {
        public int OxygenSaturationId { get; set; }

        public string UserId { get; set; }
        public Decimal SpO2 { get; set; }
        public DateTime ReadingDate { get; set; }

    }
}
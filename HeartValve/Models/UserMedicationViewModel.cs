using System.ComponentModel.DataAnnotations;

namespace HeartValve.Models
{
    public class UserMedicationViewModel
    {
        public int UserMedicationId { get; set; }
        public string UserId { get; set; }
        public int MedicationId { get; set; }
        [UIHint("Medications")]
        public MedicationViewModel Medication { get; set; }
        public string Name { get; set; }
        public decimal? Quantity { get; set; }
        [Required]
        public string Strength { get; set; }
        public string Unit { get; set; }
        public string DosageForm { get; set; }
        public string Route { get; set; }
        public string Dose { get; set; }
        public int Frequency { get; set; }
        public int MedicationScheduleTypeId { get; set; }
        public string Indication { get; set; }
        public string Instructions { get; set; }
        public string DisplayName
        {
            get { return string.Format("{0} {1} {2}", this.Name, this.Strength, this.Unit); }
        }
    }
}
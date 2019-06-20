namespace HeartValve.Models
{
    public class MedicationViewModel
    {
        public int MedicationId { get; set; }
        public string BrandName { get; set; }
        public string GenericName { get; set; }
        public string Strength { get; set; }
        public string Unit { get; set; }
        public string DosageForm { get; set; }
        public string Route { get; set; }
        public string DisplayName
        {
            get { return string.Format("{0} {1} {2}", this.BrandName, this.Strength, this.Unit); }
        }
    }
}
using System;

namespace HeartValve.Shared.Models
{
    [ModelName("PillCapInstance_Custom")]
    public partial class PillCapInstance
    {
        public int PillCapInstanceId { get; set; }
        public string UserId { get; set; }
        public int PillCapId { get; set; }
        public string PillCapName { get; set; }
        public DateTime StartDate { get; set; }
        public Nullable<DateTime> EndDate { get; set; }

        public string SerialNumber { get; set; }
    }
}
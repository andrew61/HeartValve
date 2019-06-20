using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HeartValve.API.Models
{
    public class LoginInformationViewModel
    {

        public string UserName { get; set; }
        public DateTime Time { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string Model { get; set; }
        public string OS { get; set; }
        public string Network { get; set; }
        public string PhoneType { get; set; }
        public string AppVersion { get; set; }
    }
}
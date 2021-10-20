using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class country
    {
    
        public string name { get; set; }
        public string [] topLevelDomain { get; set; }
        public string alpha2Code { get; set; }
        public string alpha3Code { get; set; }
        public string [] callingCodes { get; set; }
        public string capital { get; set; }
        public string region { get; set; }
        public string subregion { get; set; }
        public long population { get; set; }
        public decimal area { get; set; }
        public string [] timezones { get; set; }
        public string nativeName { get; set; }
        public string flag { get; set; }
        public dynamic latlng { get; set; }
        public string [] altSpellings { get; set; }
        public string demonym { get; set; }
        public string [] borders { get; set; }
        public string numericCode { get; set; }
        public dynamic currencies { get; set; }
        public dynamic languages { get; set; }
        public decimal gini { get; set; }
        public dynamic translations { get; set; }
        public dynamic regionalBlocs { get; set; }
        public dynamic cioc { get; set; }



    }
}
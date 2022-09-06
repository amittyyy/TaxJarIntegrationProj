using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AWPurchase.Business.Models.TaxJar
{
    public class TaxJurisdictions
    {
        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("county")]
        public string County { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }
    }
}

using AW.Common.Api.ApiResponses;
using System;
using System.Collections.Generic;
using System.Text;

namespace AWPurchase.Business.Models.Output
{
    public class AwApiSalesRateResponse : AwApiResponse
    {
        public AwApiSalesRateResponse(string rate, bool hasNexus, string exemptType)
        {
            Rate = rate;
            HasNexus = hasNexus;
            ExemptionType = exemptType;
        }
       
        public string Rate { get; set; }
        public bool HasNexus { get; set; }
        public string ExemptionType { get; set; }
    }
}

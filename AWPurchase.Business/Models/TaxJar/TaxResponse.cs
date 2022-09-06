using AW.Common.Api.ApiResponses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AWPurchase.Business.Models.TaxJar
{
    public class TaxResponse
    {
        [JsonProperty("tax")]
        public TaxResponseAttributes Tax { get; set; }
    }

    public class TaxResponseAttributes : AwApiResponse
    {
        [JsonProperty("order_total_amount")]
        public decimal OrderTotalAmount { get; set; }

        [JsonProperty("shipping")]
        public decimal Shipping { get; set; }

        [JsonProperty("taxable_amount")]
        public decimal TaxableAmount { get; set; }

        [JsonProperty("amount_to_collect")]
        public decimal AmountToCollect { get; set; }

        [JsonProperty("rate")]
        public decimal Rate { get; set; }

        [JsonProperty("has_nexus")]
        public bool HasNexus { get; set; }

        [JsonProperty("freight_taxable")]
        public bool FreightTaxable { get; set; }

        [JsonProperty("tax_source")]
        public string TaxSource { get; set; }

        [JsonProperty("exemption_type")]
        public string ExemptionType { get; set; }

        [JsonProperty("jurisdictions")]
        public TaxJurisdictions Jurisdictions { get; set; }

        [JsonProperty("breakdown")]
        public TaxBreakdown Breakdown { get; set; }
    }
}

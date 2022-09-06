using AW.Common.Api.ApiResponses;
using AWPurchase.Business.Models.TaxJar;
using System.Collections.Generic;

namespace AWPurchase.Business.Models.Output
{
    public class AwApiAddressValidationResponse : AwApiResponse
    {
        public AwApiAddressValidationResponse(List<Address> addresses)
        {
            Addresses = addresses;
        }

        public List<Address> Addresses { get; set; }
    }
}

using AW.Common.Api.ApiResponses;
using System.Threading.Tasks;

namespace AWPurchase.Business.Interfaces
{
    public interface ISalesTaxRateService
    {
        Task<AwApiResponse> GetSalesTaxRateByLocation(string apiToken, object parameters);        
        Task<AwApiResponse> GetSalesRates(string apiToken, string zipCode, object parameters);
        Task<AwApiResponse> GetSalesRatesBySelfService(string apiToken);
        Task<AwApiResponse> GetSalesRatebyLocationSelfService(string apiToken, object parameters);

        Task<AwApiResponse> CreateTransactionOrder(string apiToken, object parameters);
        Task<AwApiResponse> CreateTransactionOrderSelfService(string apiToken, object parameters);
        Task<AwApiResponse> CreateRefundOrderSelfService(string apiToken, object Parameters);

        Task<AwApiResponse> DeleteOrderSelfService(string apiToken, string transaction_id);
        Task<AwApiResponse> DeleteRefundSelfService(string apiToken, string transaction_id);
        Task<AwApiResponse> ValidateAddressSelfService(string apiToken, object Parameters);
    }
}

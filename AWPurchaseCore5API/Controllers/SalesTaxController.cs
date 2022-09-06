using AW.Common.Api.ApiResponses;
using AWPurchase.Business.Interfaces;
using AWPurchase.Business.Models.Input;
using AWPurchase.Business.Models.TaxJar;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AWPurchaseCore5API.Controllers
{
    [Route("api/salesTaxRate")]
    [ApiController]
    public class SalesTaxController : Controller
    {
        private readonly ILogger<SalesTaxController> _logger;
        private readonly ISalesTaxRateService _saleTaxService;
        private readonly string apiToken;
        public SalesTaxController(ILogger<SalesTaxController> logger, ISalesTaxRateService saleTaxService)
        {
            _logger = logger;
            _saleTaxService = saleTaxService;

            _logger.LogInformation("Start SalesTaxController");

            apiToken = TaxJarApiToken.taxJarApiToken;

        }

        #region SandBox

        /// <summary>
        /// AT. 07042021 :- API will get result from taxjar api. 
        /// Set up TaxJar’s official .NET client, authenticate with your API token,
        /// start calculating sales tax right away. Now let’s go!
        /// </summary>
        /// <returns></returns>
        [HttpGet("getSalesTaxbyLocation")]
        public async Task<ActionResult<AwApiResponse>> GetsalesTaxByLocation(string state, string city, string zip, string productCode = "81162000A9000")
        {

            object param = new
            {
                from_country = "US",
                from_state = "NY",
                from_zip = "11788",
                from_city = "hauppauge",
                from_street = "140 Fell Ct",

                to_country = "US",
                to_state = state,
                to_city = city,
                to_zip = zip,
                shipping = 0,

                line_items = new[] {
                    new {
                        quantity = 1,
                        unit_price = 1,
                        product_tax_code = productCode
                    }
                 }

            };

            AwApiResponse awApiresponse = await _saleTaxService.GetSalesTaxRateByLocation(apiToken, param);
            return Ok(awApiresponse);
        }


        [HttpPost("createOrder")]
        public async Task<ActionResult<AwApiResponse>> CreateTransactionOrder([FromBody] TransactionInput input)
        {

            _logger.LogTrace($"Line Number 131, method CreateTransactionOrderSelfService. TransactionId {input.transaction_id} and Zip {input.to_zip}");

            object param = new
            {
                //AT. Required - The 'transaction_id' should only include alphanumeric characters, underscores, and dashes.
                transaction_id = input.transaction_id,  //"EG. 072020211249",        //Should be unique, Unique identifier of the given order transaction. 

                //The `transaction_date` may be a date '2015-05-25', an ISO UTC date/time '2015-05-25T13:05:45', or an ISO date/time with zone offset '2015-05-25T13:05:45-05:00'.
                transaction_date = input.transaction_date,     // EG."2021/07/20",        //The date/time the transaction was originally recorded. 

                provider = "api",                       // Source of where the transaction was originally recorded. Defaults to “api”.

                //Optional and static Value
                from_country = "US",
                from_state = "NY",
                from_zip = "11788",
                from_city = "hauppauge",
                from_street = "140 Fell Ct",

                //Required and client address
                to_country = "US",                  // Two-letter ISO country code of the country where the order shipped to.
                to_state = input.to_state,          // Two-letter ISO state code where the order shipped to.
                to_city = input.to_city,
                to_zip = input.to_zip,              // Postal code where the order shipped to (5-Digit ZIP or ZIP+4).

                //AT. Reuired
                amount = input.amount + input.shipping, // Total amount of the order with shipping, excluding sales tax in dollars.
                shipping = input.shipping,              // Total amount of shipping for the order in dollars.
                sales_tax = input.sales_tax,            // Total amount of sales tax collected for the order in dollars.


                line_items = new[] {
                    new {
                        quantity = 1,
                        product_identifier = input.product_identifier != "" ? input.product_identifier : "82",   // Product identifier for the item.
                        description = input.description,
                        product_tax_code = input.product_tax_code != "" ? input.product_tax_code  : "81162000A9000", // Product tax code for the item. If omitted, the item will remain fully taxable.
                        unit_price = input.amount,
                        sales_tax = input.sales_tax
                    }
                }

            };

            _logger.LogTrace($"Param record set {param}");

            AwApiResponse awApiresponse = await _saleTaxService.CreateTransactionOrder(apiToken, param);

            _logger.LogTrace($"response awApiresponse {awApiresponse} and success {awApiresponse.Success}");
            return Ok(awApiresponse);
        }

        #endregion

        #region Sales Tax Rate
        /// <summary>
        /// AT. 07142021 Returns sales rates by passing zip code and address paramters in optional.
        /// </summary>
        /// <returns></returns>
        [HttpGet("getSalesRatesByZipCode")]        
        public async Task<ActionResult<AwApiResponse>> GetSalesRates(string zip)
        {           
            AwApiResponse awApiresponse = await _saleTaxService.GetSalesRates(apiToken, zip, new
            {
                 from_country = "US",
                    from_state = "NY",
                    from_zip = "11788",
                    from_city = "hauppauge",
                    from_street = "140 Fell Ct",

                    to_country = "US", 
                    
                });
            return Ok(awApiresponse);
        }


        /// <summary>
        /// AT. 07142021 Self Servicec API implements taxjar nuget package and return sales rate by zip codes.
        /// </summary>
        /// <returns></returns>
        [HttpGet("getSalesTaxbyBulkZipsSelfService")]       
        public async Task<ActionResult<AwApiResponse>> GetSalesRatesBySelfService()
        {
            AwApiResponse awApiresponse = await _saleTaxService.GetSalesRatesBySelfService(apiToken);
            return Ok(awApiresponse);
        }

        /// <summary>
        /// AT. 07142021 SelfService API (Using Their DLL), Get Sales Rate by ZipCode and Company Product Code.
        /// </summary>
        /// <param name="state"></param>
        /// <param name="city"></param>
        /// <param name="zip"></param>
        /// <param name="productCode"></param>
        /// <returns></returns>
        [HttpGet("getSalesRatebyLocationSelfService")]
        public async Task<ActionResult<AwApiResponse>> GetSalesRatebyLocationSelfService(string state, string city, string zip, string productCode = "81162000A9000")
        {
            //AT. 12022021 Added explicitly to make sure PC will be Sass Business. 
            productCode = "81162000A9000";

            _logger.LogTrace($"GetSalesRatebyLocationSelfServic, Parameters:- state {state}, zip {zip}, productCode {productCode}");            

           object param = new
            {
                from_country = "US",
                from_state = "NY",
                from_zip = "11788",
                from_city = "hauppauge",
                from_street = "140 Fell Ct",

                to_country = "US",
                to_state = state,
                to_city = city,
                to_zip = zip,
                shipping = 0,

                line_items = new[] {
                    new {
                        quantity = 1,
                        unit_price = 1,
                        product_tax_code = productCode
                    }
                 }

            };

            _logger.LogTrace($"Param record set, Line Number 201 {param}");
            AwApiResponse awApiresponse = await _saleTaxService.GetSalesRatebyLocationSelfService(apiToken, param);

            _logger.LogTrace($"response awApiresponse 204 {awApiresponse} and success {awApiresponse.Success}");
            return Ok(awApiresponse);
        }

        #endregion

        #region Transactions
        /// <summary>
        /// AT. 07142021 SelfService API (Using Their DLL), Post Transaction in to TaxJar Account.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("createOrderSelfService")]
        public async Task<ActionResult<AwApiResponse>> CreateTransactionOrderSelfService([FromBody] TransactionInput input)
        {

            _logger.LogTrace($"Line Number 131, method CreateTransactionOrderSelfService. TransactionId {input.transaction_id} and Zip {input.to_zip}");

            object param = new
            {
                                                                        //AT. Required - The 'transaction_id' should only include alphanumeric characters, underscores, and dashes.
                transaction_id = input.transaction_id,                  //"EG. 072020211249",        //Should be unique, Unique identifier of the given order transaction. 

                                                                        //The `transaction_date` may be a date '2015-05-25', an ISO UTC date/time '2015-05-25T13:05:45', or an ISO date/time with zone offset '2015-05-25T13:05:45-05:00'.
                transaction_date = input.transaction_date,              // EG."2021/07/20",        //The date/time the transaction was originally recorded. 

                provider = "api",                                       // Source of where the transaction was originally recorded. Defaults to “api”.

                                                                        //Optional and static Value
                from_country = "US",
                from_state = "NY",
                from_zip = "11788",
                from_city = "hauppauge",
                from_street = "140 Fell Ct",

                                                            //Required and client address
                to_country = "US",                          // Two-letter ISO country code of the country where the order shipped to.
                to_state = input.to_state,                  // Two-letter ISO state code where the order shipped to.
                to_city = input.to_city,
                to_zip = input.to_zip,                      // Postal code where the order shipped to (5-Digit ZIP or ZIP+4).

                                                            //AT. Reuired
                amount = input.amount + input.shipping,     // Total amount of the order with shipping, excluding sales tax in dollars.
                shipping = input.shipping,                  // Total amount of shipping for the order in dollars.
                sales_tax = input.sales_tax,                // Total amount of sales tax collected for the order in dollars.


                line_items = new[] {
                    new {
                        quantity = 1,
                        product_identifier = input.product_identifier != "" ? input.product_identifier : "82",   // Product identifier for the item.
                        description = input.description,
                        product_tax_code = input.product_tax_code != "" ? input.product_tax_code  : "81162000A9000", // Product tax code for the item. If omitted, the item will remain fully taxable.
                        unit_price = input.amount,
                        sales_tax = input.sales_tax
                    }
                }

            };

            _logger.LogTrace($"Param record set {param}");

            AwApiResponse awApiresponse = await _saleTaxService.CreateTransactionOrderSelfService(apiToken, param);

            _logger.LogTrace($"response awApiresponse {awApiresponse} and success {awApiresponse.Success}");
            return Ok(awApiresponse);
        }

        /// <summary>
        /// AT. 07142021 Delete Transaction and only deletes which created by API.
        /// </summary>
        /// <param name="TransactionId"></param>
        /// <returns></returns>
        [HttpDelete("DeleteOrderSelfService")]
        public async Task<ActionResult<AwApiResponse>> DeleteOrderSelfService(string TransactionId)
        {
            _logger.LogTrace($"TransactionId record set, Line Number 277 {TransactionId}");

            AwApiResponse awApiresponse = await _saleTaxService.DeleteOrderSelfService(apiToken, TransactionId);

            _logger.LogTrace($"response awApiresponse 280 {awApiresponse} and success {awApiresponse.Success}");

            return Ok(awApiresponse);
        }
        #endregion

        #region Refund Transactions

        /// <summary>
        /// AT> 08192021 SelfService API to Get Transaction Refund Back.
        /// transaction_reference_id :- Unique identifier of the corresponding order transaction for the refund.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("RefundOrderSelfService")]
        public async Task<ActionResult<AwApiResponse>> RefundOrderSelfService([FromBody] RefundTransactionInput input)
        {
            _logger.LogTrace($"Line Number 302, method RefundOrderSelfService. TransactionId {input.transaction_id} and Zip {input.transaction_reference_id}");

            object param = new
            {
                                                                            //AT. Required - The 'transaction_id' should only include alphanumeric characters, underscores, and dashes.
                transaction_id = input.transaction_id,                      //"EG. 072020211249",        //Should be unique, Unique identifier of the given order transaction. 

                transaction_reference_id = input.transaction_reference_id,  // Required: Unique identifier of the corresponding order transaction for the refund.

                                                                            //The `transaction_date` may be a date '2015-05-25', an ISO UTC date/time '2015-05-25T13:05:45', or an ISO date/time with zone offset '2015-05-25T13:05:45-05:00'.
                transaction_date = input.transaction_date,                  // EG."2021/07/20",        //The date/time the transaction was originally recorded. 

                provider = "api",                                           // Source of where the transaction was originally recorded. Defaults to “api”.

                                                                            //Optional and static Value
                from_country = "US",
                from_state = "NY",
                from_zip = "11788",
                from_city = "hauppauge",
                from_street = "140 Fell Ct",

                                                                    // Required and client address
                to_country = "US",                                  // Two-letter ISO country code of the country where the order shipped to.
                to_state = input.to_state,                          // Two-letter ISO state code where the order shipped to.
                to_city = input.to_city,
                to_zip = input.to_zip,                              // Postal code where the order shipped to (5-Digit ZIP or ZIP+4).

                                                                    // AT. Reuired
                amount = input.amount + input.shipping,             // Total amount of the order with shipping, excluding sales tax in dollars.
                shipping = input.shipping,                          // Total amount of shipping for the order in dollars.
                sales_tax = input.sales_tax,                        // Total amount of sales tax collected for the order in dollars.


                line_items = new[] {
                    new {
                        quantity = 1,
                        product_identifier = input.product_identifier != "" ? input.product_identifier : "82",   // Product identifier for the item.
                        description = input.description,
                        product_tax_code = input.product_tax_code != "" ? input.product_tax_code  : "81162000A9000", // Product tax code for the item. If omitted, the item will remain fully taxable.
                        unit_price = input.amount,
                        sales_tax = input.sales_tax
                    }
                }

            };

            _logger.LogTrace($"Param record set Line 342 {param}");

            AwApiResponse awApiresponse = await _saleTaxService.CreateRefundOrderSelfService(apiToken, param);

            _logger.LogTrace($"response awApiresponse {awApiresponse} and success {awApiresponse.Success}");
            return Ok(awApiresponse);
        }

        [HttpDelete("DeleteRefundSelfService")]
        public async Task<ActionResult<AwApiResponse>> DeleteRefundSelfService(string TransactionId)
        {
            _logger.LogTrace($"Refund TransactionId record set, Line Number 355 {TransactionId}");

            AwApiResponse awApiresponse = await _saleTaxService.DeleteRefundSelfService(apiToken, TransactionId);

            _logger.LogTrace($"response awApiresponse 359 {awApiresponse} and success {awApiresponse.Success}");

            return Ok(awApiresponse);
        }

        #endregion

        #region Validate Address
        [HttpGet("validateAddressSelfService")]
        /// <summary>
        /// AT. 009012021 SelfService API (Using Their DLL), Validate Address.
        public async Task<ActionResult<AwApiResponse>> ValidateAddressSelfService(string state, string city, string zip, string street)
        {
            _logger.LogTrace($"Validate Address SelfService, Line Number 374 zip {zip}, state {state}, city {city}, street {street} ");
            object param = new
            {
                country = "US",
                state = state,
                zip = zip,
                city = city,
                street = street

            };
            AwApiResponse awApiresponse = await _saleTaxService.ValidateAddressSelfService(apiToken, param);

            _logger.LogTrace($"response awApiresponse 388 {awApiresponse} and success {awApiresponse.Success}");

            return Ok(awApiresponse);
        }
        #endregion

    }
}

using AWPurchase.Business.Models.TaxJar;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace AWPurchase.Business.HelperClasses
{
    [Serializable]
    public class TaxjarException : ApplicationException
    {
        public HttpStatusCode HttpStatusCode { get; set; }
        public TaxjarError TaxjarError { get; set; }

        public TaxjarException()
        {
        }

        public TaxjarException(HttpStatusCode statusCode, TaxjarError taxjarError, string message) : base(message)
        {
            HttpStatusCode = statusCode;
            TaxjarError = taxjarError;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWPurchase.Business.Models.Input
{
    public class RefundTransactionInput
    {
        public string transaction_id { get; set; }
        public string transaction_reference_id { get; set; }
        public DateTime transaction_date { get; set; }
        public string to_state { get; set; }
        public string to_city { get; set; }
        public string to_zip { get; set; }

        public decimal amount { get; set; }
        public decimal shipping { get; set; } = 0;
        public decimal sales_tax { get; set; }
        public string product_tax_code { get; set; } = "81162000A9000";
        public string description { get; set; }

        public string product_identifier { get; set; } = "82";
    }
}

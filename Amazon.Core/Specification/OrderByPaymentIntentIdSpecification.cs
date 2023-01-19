using Amazon.Core.Entities.Order_Aggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amazon.Core.Specification
{
    public class OrderByPaymentIntentIdSpecification :BaseSpecification<Order>
    {
        public OrderByPaymentIntentIdSpecification(string PaymentintentId)
            :base(O => O.PaymentIntenId == PaymentintentId)
        {

        }
    }
}

using Amazon.Core.Entities;
using Amazon.Core.Entities.Order_Aggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amazon.Core.Services
{
    public interface IPaymentService
    {
        Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketId);

        Task<Order> UpdatePaymentIntentSucceededOrFailed(string paymentIntentId, bool Issuccess);
    }
}

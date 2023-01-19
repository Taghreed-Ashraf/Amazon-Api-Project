using Amazon.Core.Entities.Order_Aggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amazon.Core.Specification
{
    
    public class OrderWithItemsAndDeliveyMethodSpecification : BaseSpecification<Order>
    {
        // This Constructor is Used for get all the Orders for a Specific User
        public OrderWithItemsAndDeliveyMethodSpecification(string buyerEmail)
            : base(O => O.BuyerEmail == buyerEmail)
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);
            AddOrderByDesending(O => O.OrderDate);
        }

        // This Constructor is Used for get Specfifc Order By Id for a Specific User
        public OrderWithItemsAndDeliveyMethodSpecification(int OrderId, string buyerEmail)
            : base(O => O.BuyerEmail == buyerEmail && O.Id == OrderId)
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);
        }
    }
}

using Amazon.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amazon.Core.Specification
{
    public class ProductWithFilterForCountSpecifction : BaseSpecification<Product>
    {
        public ProductWithFilterForCountSpecifction(ProductSpecParams productParams)
           : base(P =>
                   (string.IsNullOrEmpty(productParams.Search) || P.Name.ToLower().Contains(productParams.Search)) &&
                   (!productParams.BrandId.HasValue || P.ProductBrandId == productParams.BrandId.Value) &&
                   (!productParams.TypeId.HasValue || P.ProductTypeId == productParams.TypeId.Value)
                )
        {
        }
    }
}

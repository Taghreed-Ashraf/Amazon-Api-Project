using Amazon.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amazon.Core.Specification
{
    public class ProductWithBrandAndTypeSpecification : BaseSpecification<Product>
    {
        public ProductWithBrandAndTypeSpecification(ProductSpecParams productParams)
            : base(P =>
                    (string.IsNullOrEmpty(productParams.Search) || P.Name.ToLower().Contains(productParams.Search))&&
                    (!productParams.BrandId.HasValue || P.ProductBrandId == productParams.BrandId.Value) &&
                    (!productParams.TypeId.HasValue || P.ProductTypeId == productParams.TypeId.Value) 
                 )
        {
            AddIncludes(p => p.ProductBrand);
            AddIncludes(p => p.ProductType);

            ApplyPagintion(productParams.PageSize * (productParams.PageIndex -1) , productParams.PageSize);

            AddOrderBy(P => P.Name);

            if (productParams.Sort != null)
            {
                switch (productParams.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(P => P.Price);
                        break;

                    case "priceDesc":
                        AddOrderByDesending(P => P.Price);
                        break;
                    default:
                        AddOrderBy(P => P.Name);
                        break;
                }
            }
        }

        public ProductWithBrandAndTypeSpecification(int id) : base(p => p.Id == id)
        {
            AddIncludes(p => p.ProductBrand);
            AddIncludes(p => p.ProductType);
        }

    }
}

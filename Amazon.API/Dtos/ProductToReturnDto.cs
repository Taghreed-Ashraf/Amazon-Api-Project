using Amazon.Core.Entities;

namespace Amazon.API.Dtos
{
    public class ProductToReturnDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string PictureUrl { get; set; }

        public int ProductTypeId { get; set; } 

        public string ProductType { get; set; } 

        public int ProductBrandId { get; set; } // int : Not Allow null

        //public ProductBrand ProductBrand { get; set; } // Navgtional Property [One]
        public string ProductBrand { get; set; } 
    }
}

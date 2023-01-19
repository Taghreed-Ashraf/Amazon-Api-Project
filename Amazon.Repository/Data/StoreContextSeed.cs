using Amazon.Core.Entities;
using Amazon.Core.Entities.Order_Aggregate;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Amazon.Repository.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync (StoreContext context , ILoggerFactory logfact)
        {
            try
            {
                if(!context.productBrands.Any())
                {
                    var brandsData = File.ReadAllText("../Amazon.Repository/Data/DataSeed/brands.json");
                    var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
                    foreach (var brand in brands)
                        context.Set<ProductBrand>().Add(brand);
                }

                if (!context.ProductTypes.Any())
                {
                    var typesData = File.ReadAllText("../Amazon.Repository/Data/DataSeed/types.json");
                    var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
                    foreach (var type in types)
                        context.Set<ProductType>().Add(type);
                }

                if (!context.Products.Any())
                {
                    var productsData = File.ReadAllText("../Amazon.Repository/Data/DataSeed/products.json");
                    var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                    foreach (var product in products)
                        context.Set<Product>().Add(product); 
                }

                if (!context.DeliveryMethods.Any())
                {
                    var DeliveryMethodsData = File.ReadAllText("../Amazon.Repository/Data/DataSeed/delivery.json");
                    var DeliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliveryMethodsData);
                    foreach (var deleveryMethod in DeliveryMethods)
                        context.Set<DeliveryMethod>().Add(deleveryMethod);
                }

                await context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                var logger = logfact.CreateLogger<StoreContextSeed>();
                logger.LogError(ex, ex.Message);
            }
           
        }

    }
}

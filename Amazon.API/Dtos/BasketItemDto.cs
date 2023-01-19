using System.ComponentModel.DataAnnotations;

namespace Amazon.API.Dtos
{
    public class BasketItemDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string productName { get; set; }
        [Required]
        public string PictureUrl { get; set; }
        [Required]
        public string Brand { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        [Range(1 , double.MaxValue , ErrorMessage = "Price Must Be Gretaer Than Zero")]
        public decimal Price { get; set; }
        [Required]
        [Range(1 , int.MaxValue , ErrorMessage ="Quaintity Must be One Item at least!!")]
        public int Quantity { get; set; }
    }
}
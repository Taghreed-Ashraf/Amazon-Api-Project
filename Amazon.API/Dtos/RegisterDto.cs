using System.ComponentModel.DataAnnotations;

namespace Amazon.API.Dtos
{
    public class RegisterDto
    {
        [Required]
        public string DisplayName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Phone]
        public string phoneNumber { get; set; }
        [Required]
        [RegularExpression("(?=^.{6,10}$)(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&amp;*()_+{}&quot;:;'?/&gt;.&lt;.])(?!.*\\s).*$" ,
            ErrorMessage = "Password Must be Have 1 Uppercase , 1 Lowercase , 1 Number , 1 non alpanumeric and at least 6 charcters")]
        public string Password { get; set; }
    }
}

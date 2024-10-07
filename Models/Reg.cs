using System.ComponentModel.DataAnnotations;

namespace WebApplication9Municipal_Billing_System.Models
{
     public class Reg
    {
            [Key]
            public int UserId { get; set; }  // Auto-generated primary key

            [Required(ErrorMessage = "Name is required")]
            public string Name { get; set; } = string.Empty;

            [Required(ErrorMessage = "Surname is required")]
            public string Surname { get; set; } = string.Empty;

            [Required(ErrorMessage = "Email is required")]
            [EmailAddress(ErrorMessage = "Invalid Email Address")]
            public string Email { get; set; } = string.Empty;

            [Required(ErrorMessage = "Password is required")]
            [DataType(DataType.Password)]
            [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
            public string Password { get; set; } = string.Empty;




            [Required(ErrorMessage = "Please confirm your password")]
            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; } = string.Empty;  // Ensure this matches Password

            [Required(ErrorMessage = "Id Number is required")]
            [StringLength(13, MinimumLength = 13, ErrorMessage = "Id Number must be exactly 13 characters long")]
            [RegularExpression(@"^\d{13}$", ErrorMessage = "Id Number must be numeric and 13 digits long")]
            public string IdNumber { get; set; } = string.Empty;

        //public enum UserTypeList
        //{
        //    Admin, User
        //}

        //[Required(ErrorMessage = "User type is required")]
        //public UserTypeList UserType { get; set; }

    }

    public class LoginModel
    {
        [System.ComponentModel.DataAnnotations.Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? Password { get; set; }
         
    
    }
}

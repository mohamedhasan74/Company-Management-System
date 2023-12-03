using System.ComponentModel.DataAnnotations;

namespace PL.ViewModels
{
    public class SignUpViewModel
    {
        [Required(ErrorMessage ="First Name Is Required")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last Name Is Required")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "UserName Is Required")]
        [MinLength(5,ErrorMessage ="UserName At Least 5 Char")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invaild Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confrim Password is required")]
        [Compare("Password", ErrorMessage = "Confirm Password does not match Password")]
        [DataType(DataType.Password)]

        public string ConfirmPassword { get; set; }
        public bool IsAgree { get; set; }

    }
}

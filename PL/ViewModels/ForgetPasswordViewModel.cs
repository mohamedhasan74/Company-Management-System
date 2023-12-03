using System.ComponentModel.DataAnnotations;

namespace PL.ViewModels
{
    public class ForgetPasswordViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invaild Email")]
        public string Email { get; set; }
    }
}

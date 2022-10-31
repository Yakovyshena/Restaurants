using System.ComponentModel.DataAnnotations;

namespace RestaurantWebApplication.ViewModel
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "EMAIL")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "PASSWORD")]
        public string Password { get; set; }

        [Display(Name = "REMEMBER YOU ?")]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }

    }
}


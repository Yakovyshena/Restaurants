using System.ComponentModel.DataAnnotations;

namespace RestaurantWebApplication.ViewModel
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "EMAIL")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [Display(Name = "BIRTH YEAR")]
        public int Year { get; set; }

        [Required]
        [Display(Name = "PASSWORD")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "PASSWORDS ARE NOT SIMULAR")]
        [Display(Name = "CONFIRM PASSWORD")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}

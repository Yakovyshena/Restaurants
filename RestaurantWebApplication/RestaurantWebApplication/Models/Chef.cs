using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantWebApplication
{
    public partial class Chef
    {
        public Chef()
        {
            ChefsCpecialisations = new HashSet<ChefsCpecialisation>();
            Menus = new HashSet<Menu>();
            Restaurants = new HashSet<Restaurant>();
        }

        public int ChefId { get; set; }
        [Required(ErrorMessage = "This field should not be empty")]
        [Display(Name = "FIRST NAME")]
        public string FirstName { get; set; } = null!;
        [Display(Name = "MIDDLE NAME")]
        public string? MiddleName { get; set; }
        [Display(Name = "LAST NAME")]
        public string? LastName { get; set; }
        [Required(ErrorMessage = "This field should not be empty")]
        [Display(Name = "CITY")]
        public int BirthCityId { get; set; }
        
        [NotMapped]
        public string FullName
        {
            get 
            { 
                return FirstName + " " + MiddleName + " " + LastName; 
            }
        }

        public virtual City BirthCity { get; set; } = null!;
        public virtual ICollection<ChefsCpecialisation> ChefsCpecialisations { get; set; }
        public virtual ICollection<Menu> Menus { get; set; }
        public virtual ICollection<Restaurant> Restaurants { get; set; }
    }
}

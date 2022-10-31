using System.ComponentModel.DataAnnotations;

namespace RestaurantWebApplication
{
    public partial class Specialisation
    {
        public Specialisation()
        {
            ChefsCpecialisations = new HashSet<ChefsCpecialisation>();
        }

        public int SpecialisationId { get; set; }
        [Display(Name = "NAME")]
        [Required(ErrorMessage = "This field should not be empty")]
        public string Name { get; set; } = null!;

        public virtual ICollection<ChefsCpecialisation> ChefsCpecialisations { get; set; }
    }
}

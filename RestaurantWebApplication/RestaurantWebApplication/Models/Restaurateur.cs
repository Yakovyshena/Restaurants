namespace RestaurantWebApplication
{
    public partial class Restaurateur
    {
        public Restaurateur()
        {
            RestaurantsRestaurateurs = new HashSet<RestaurantsRestaurateur>();
        }

        public int RestaurateurId { get; set; }
        public string FirstName { get; set; } = null!;
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public int BirthCityId { get; set; }

        public virtual City BirthCity { get; set; } = null!;
        public virtual ICollection<RestaurantsRestaurateur> RestaurantsRestaurateurs { get; set; }
    }
}

namespace RestaurantWebApplication
{
    public partial class Menu
    {
        public Menu()
        {
            MenusDishes = new HashSet<MenusDish>();
            Restaurants = new HashSet<Restaurant>();
        }

        public int MenuId { get; set; }
        public int? BrandChefId { get; set; }

        public virtual Chef? BrandChef { get; set; }
        public virtual ICollection<MenusDish> MenusDishes { get; set; }
        public virtual ICollection<Restaurant> Restaurants { get; set; }
    }
}

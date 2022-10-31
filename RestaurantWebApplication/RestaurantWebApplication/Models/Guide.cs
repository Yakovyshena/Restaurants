namespace RestaurantWebApplication
{
    public partial class Guide
    {
        public Guide()
        {
            GuidesRestaurants = new HashSet<GuidesRestaurant>();
        }

        public int GuideId { get; set; }
        public string Name { get; set; } = null!;
        public string? Info { get; set; }

        public virtual ICollection<GuidesRestaurant> GuidesRestaurants { get; set; }
    }
}

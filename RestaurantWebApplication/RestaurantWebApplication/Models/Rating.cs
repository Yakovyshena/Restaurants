namespace RestaurantWebApplication
{
    public partial class Rating
    {
        public Rating()
        {
            RatingsRestaurants = new HashSet<RatingsRestaurant>();
        }

        public int RatingId { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<RatingsRestaurant> RatingsRestaurants { get; set; }
    }
}

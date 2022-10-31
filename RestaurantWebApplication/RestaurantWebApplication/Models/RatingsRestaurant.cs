namespace RestaurantWebApplication
{
    public partial class RatingsRestaurant
    {
        public int PairId { get; set; }
        public int RatingId { get; set; }
        public int RestaurantId { get; set; }
        public int RestaurantPosition { get; set; }
        public string? PositionArgument { get; set; }

        public virtual Rating Rating { get; set; } = null!;
        public virtual Restaurant Restaurant { get; set; } = null!;
    }
}

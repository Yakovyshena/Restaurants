namespace RestaurantWebApplication
{
    public partial class RestaurantsRestaurateur
    {
        public int PairId { get; set; }
        public int RestaurateurId { get; set; }
        public int RestaurantId { get; set; }
        public DateTime CoOwnerSince { get; set; }
        public DateTime? CoOwnerTill { get; set; }

        public virtual Restaurant Restaurant { get; set; } = null!;
        public virtual Restaurateur Restaurateur { get; set; } = null!;
    }
}

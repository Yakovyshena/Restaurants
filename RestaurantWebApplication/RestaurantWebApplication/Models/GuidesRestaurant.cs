namespace RestaurantWebApplication
{
    public partial class GuidesRestaurant
    {
        public int PairId { get; set; }
        public int GuideId { get; set; }
        public int RestaurantId { get; set; }
        public string Mark { get; set; } = null!;
        public string? MarkArgument { get; set; }

        public virtual Guide Guide { get; set; } = null!;
        public virtual Restaurant Restaurant { get; set; } = null!;
    }
}

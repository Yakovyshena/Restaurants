namespace RestaurantWebApplication
{
    public partial class MenusDish
    {
        public int PairId { get; set; }
        public int MenuId { get; set; }
        public int DishId { get; set; }

        public virtual Dish Dish { get; set; } = null!;
        public virtual Menu Menu { get; set; } = null!;
    }
}

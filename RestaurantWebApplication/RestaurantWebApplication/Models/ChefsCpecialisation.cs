namespace RestaurantWebApplication
{
    public partial class ChefsCpecialisation
    {
        public int PairId { get; set; }
        public int ChefId { get; set; }
        public int SpecialisationId { get; set; }

        public virtual Chef Chef { get; set; } = null!;
        public virtual Specialisation Specialisation { get; set; } = null!;
    }
}

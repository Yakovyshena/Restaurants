using Microsoft.EntityFrameworkCore;

namespace RestaurantWebApplication
{
    public partial class DBRestaurantsContext : DbContext
    {
        public DBRestaurantsContext()
        {
        }

        public DBRestaurantsContext(DbContextOptions<DBRestaurantsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Chef> Chefs { get; set; } = null!;
        public virtual DbSet<ChefsCpecialisation> ChefsCpecialisations { get; set; } = null!;
        public virtual DbSet<City> Cities { get; set; } = null!;
        public virtual DbSet<Country> Countries { get; set; } = null!;
        public virtual DbSet<Dish> Dishes { get; set; } = null!;
        public virtual DbSet<DishesProduct> DishesProducts { get; set; } = null!;
        public virtual DbSet<Guide> Guides { get; set; } = null!;
        public virtual DbSet<GuidesRestaurant> GuidesRestaurants { get; set; } = null!;
        public virtual DbSet<Menu> Menus { get; set; } = null!;
        public virtual DbSet<MenusDish> MenusDishes { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<ProductType> ProductTypes { get; set; } = null!;
        public virtual DbSet<Rating> Ratings { get; set; } = null!;
        public virtual DbSet<RatingsRestaurant> RatingsRestaurants { get; set; } = null!;
        public virtual DbSet<Restaurant> Restaurants { get; set; } = null!;
        public virtual DbSet<RestaurantsRestaurateur> RestaurantsRestaurateurs { get; set; } = null!;
        public virtual DbSet<Restaurateur> Restaurateurs { get; set; } = null!;
        public virtual DbSet<Specialisation> Specialisations { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server = YAKOVYSHENA\\SQLEXPRESS; Database = DBRestaurants; Trusted_Connection = True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Chef>(entity =>
            {
                entity.Property(e => e.ChefId).HasColumnName("ChefID");

                entity.Property(e => e.BirthCityId).HasColumnName("BirthCityID");

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.MiddleName).HasMaxLength(50);

                entity.HasOne(d => d.BirthCity)
                    .WithMany(p => p.Chefs)
                    .HasForeignKey(d => d.BirthCityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Chefs_Cities");
            });

            modelBuilder.Entity<ChefsCpecialisation>(entity =>
            {
                entity.HasKey(e => e.PairId);

                entity.ToTable("Chefs_Cpecialisations");

                entity.Property(e => e.PairId).HasColumnName("PairID");

                entity.Property(e => e.ChefId).HasColumnName("ChefID");

                entity.Property(e => e.SpecialisationId).HasColumnName("SpecialisationID");

                entity.HasOne(d => d.Chef)
                    .WithMany(p => p.ChefsCpecialisations)
                    .HasForeignKey(d => d.ChefId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Chefs_Cpecialisations_Chefs");

                entity.HasOne(d => d.Specialisation)
                    .WithMany(p => p.ChefsCpecialisations)
                    .HasForeignKey(d => d.SpecialisationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Chefs_Cpecialisations_Specialisations");
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.Property(e => e.CityId).HasColumnName("CityID");

                entity.Property(e => e.CountryId).HasColumnName("CountryID");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.Cities)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Cities_Countries");
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.Property(e => e.CountryId).HasColumnName("CountryID");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Dish>(entity =>
            {
                entity.Property(e => e.DishId).HasColumnName("DishID");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<DishesProduct>(entity =>
            {
                entity.HasKey(e => e.PairId);

                entity.ToTable("Dishes_Products");

                entity.Property(e => e.PairId).HasColumnName("PairID");

                entity.Property(e => e.Amount).HasMaxLength(50);

                entity.Property(e => e.DishId).HasColumnName("DishID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.HasOne(d => d.Dish)
                    .WithMany(p => p.DishesProducts)
                    .HasForeignKey(d => d.DishId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Dishes_Products_Dishes");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.DishesProducts)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Dishes_Products_Products");
            });

            modelBuilder.Entity<Guide>(entity =>
            {
                entity.Property(e => e.GuideId).HasColumnName("GuideID");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<GuidesRestaurant>(entity =>
            {
                entity.HasKey(e => e.PairId);

                entity.ToTable("Guides_Restaurants");

                entity.Property(e => e.PairId).HasColumnName("PairID");

                entity.Property(e => e.GuideId).HasColumnName("GuideID");

                entity.Property(e => e.Mark)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.MarkArgument)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.RestaurantId).HasColumnName("RestaurantID");

                entity.HasOne(d => d.Guide)
                    .WithMany(p => p.GuidesRestaurants)
                    .HasForeignKey(d => d.GuideId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Guides_Restaurants_Guides");

                entity.HasOne(d => d.Restaurant)
                    .WithMany(p => p.GuidesRestaurants)
                    .HasForeignKey(d => d.RestaurantId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Guides_Restaurants_Restaurants");
            });

            modelBuilder.Entity<Menu>(entity =>
            {
                entity.Property(e => e.MenuId).HasColumnName("MenuID");

                entity.Property(e => e.BrandChefId).HasColumnName("BrandChefID");

                entity.HasOne(d => d.BrandChef)
                    .WithMany(p => p.Menus)
                    .HasForeignKey(d => d.BrandChefId)
                    .HasConstraintName("FK_Menus_Chefs");
            });

            modelBuilder.Entity<MenusDish>(entity =>
            {
                entity.HasKey(e => e.PairId);

                entity.ToTable("Menus_Dishes");

                entity.Property(e => e.PairId).HasColumnName("PairID");

                entity.Property(e => e.DishId).HasColumnName("DishID");

                entity.Property(e => e.MenuId).HasColumnName("MenuID");

                entity.HasOne(d => d.Dish)
                    .WithMany(p => p.MenusDishes)
                    .HasForeignKey(d => d.DishId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Menus_Dishes_Dishes");

                entity.HasOne(d => d.Menu)
                    .WithMany(p => p.MenusDishes)
                    .HasForeignKey(d => d.MenuId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Menus_Dishes_Menus");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.ProductTypeId).HasColumnName("ProductTypeID");

                entity.HasOne(d => d.ProductType)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.ProductTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Products_ProductTypes");
            });

            modelBuilder.Entity<ProductType>(entity =>
            {
                entity.Property(e => e.ProductTypeId).HasColumnName("ProductTypeID");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Rating>(entity =>
            {
                entity.Property(e => e.RatingId).HasColumnName("RatingID");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<RatingsRestaurant>(entity =>
            {
                entity.HasKey(e => e.PairId);

                entity.ToTable("Ratings_Restaurants");

                entity.Property(e => e.PairId).HasColumnName("PairID");

                entity.Property(e => e.PositionArgument).HasMaxLength(50);

                entity.Property(e => e.RatingId).HasColumnName("RatingID");

                entity.Property(e => e.RestaurantId).HasColumnName("RestaurantID");

                entity.HasOne(d => d.Rating)
                    .WithMany(p => p.RatingsRestaurants)
                    .HasForeignKey(d => d.RatingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ratings_Restaurants_Ratings");

                entity.HasOne(d => d.Restaurant)
                    .WithMany(p => p.RatingsRestaurants)
                    .HasForeignKey(d => d.RestaurantId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ratings_Restaurants_Restaurants");
            });

            modelBuilder.Entity<Restaurant>(entity =>
            {
                entity.Property(e => e.RestaurantId).HasColumnName("RestaurantID");

                entity.Property(e => e.ChefId).HasColumnName("ChefID");

                entity.Property(e => e.CityId).HasColumnName("CityID");

                entity.Property(e => e.IconicDishId).HasColumnName("IconicDishID");

                entity.Property(e => e.MenuId).HasColumnName("MenuID");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.HasOne(d => d.Chef)
                    .WithMany(p => p.Restaurants)
                    .HasForeignKey(d => d.ChefId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Restaurants_Chefs");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.Restaurants)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Restaurants_Cities");

                entity.HasOne(d => d.IconicDish)
                    .WithMany(p => p.Restaurants)
                    .HasForeignKey(d => d.IconicDishId)
                    .HasConstraintName("FK_Restaurants_Dishes");

                entity.HasOne(d => d.Menu)
                    .WithMany(p => p.Restaurants)
                    .HasForeignKey(d => d.MenuId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Restaurants_Menus");
            });

            modelBuilder.Entity<RestaurantsRestaurateur>(entity =>
            {
                entity.HasKey(e => e.PairId);

                entity.ToTable("Restaurants_Restaurateurs");

                entity.Property(e => e.PairId).HasColumnName("PairID");

                entity.Property(e => e.CoOwnerSince).HasColumnType("date");

                entity.Property(e => e.CoOwnerTill).HasColumnType("date");

                entity.Property(e => e.RestaurantId).HasColumnName("RestaurantID");

                entity.Property(e => e.RestaurateurId).HasColumnName("RestaurateurID");

                entity.HasOne(d => d.Restaurant)
                    .WithMany(p => p.RestaurantsRestaurateurs)
                    .HasForeignKey(d => d.RestaurantId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Restaurants_Restaurateurs_Restaurants");

                entity.HasOne(d => d.Restaurateur)
                    .WithMany(p => p.RestaurantsRestaurateurs)
                    .HasForeignKey(d => d.RestaurateurId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Restaurants_Restaurateurs_Restaurateurs");
            });

            modelBuilder.Entity<Restaurateur>(entity =>
            {
                entity.Property(e => e.RestaurateurId).HasColumnName("RestaurateurID");

                entity.Property(e => e.BirthCityId).HasColumnName("BirthCityID");

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.MiddleName).HasMaxLength(50);

                entity.HasOne(d => d.BirthCity)
                    .WithMany(p => p.Restaurateurs)
                    .HasForeignKey(d => d.BirthCityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Restaurateurs_Cities");
            });

            modelBuilder.Entity<Specialisation>(entity =>
            {
                entity.Property(e => e.SpecialisationId).HasColumnName("SpecialisationID");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

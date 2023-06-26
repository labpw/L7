using Microsoft.EntityFrameworkCore;
using P06Shop.Shared.Shop;
using P07Shop.DataSeeder;

namespace P05Shop.API.Models
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories{ get; set; }
        public DbSet<Supplier> Suppliers{ get; set; }
        public DbSet<ProductSuppliers> ProductSuppliers{ get; set; }

        public DbSet<ProductDetails> ProductDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // fluent api 
            // product 
            modelBuilder.Entity<Product>()
                .Property(p => p.Barcode)
                .IsRequired()
                .HasMaxLength(12);

            modelBuilder.Entity<Product>()
                .Property(p => p.Title)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Product>()
             .Property(p => p.Price)
             .HasColumnType("decimal(8,2)");

            modelBuilder.Entity<Product>()
                 .HasOne(p => p.Category)
                 .WithMany(c => c.Products);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.ProductSuppliers)
                .WithOne(ps => ps.Product)
                .HasForeignKey(ps => ps.ProductId);

            modelBuilder.Entity<Product>()
            .HasOne(p => p.ProductDetails)
            .WithOne(d => d.Product)
            .HasForeignKey<ProductDetails>(d => d.ProductId);

            // category 
            modelBuilder.Entity<Category>()
                .HasMany(c => c.Products)
                .WithOne(p => p.Category);

            // supplier 
            modelBuilder.Entity<Supplier>()
                .HasMany(s => s.ProductSuppliers)
                .WithOne(ps => ps.Supplier)
                .HasForeignKey(ps => ps.SupplierId);

            // productsupplier
            modelBuilder.Entity<ProductSuppliers>()
                .HasKey(ps => new { ps.ProductId, ps.SupplierId });


            // productdetails
            modelBuilder.Entity<ProductDetails>()
                .HasKey(pd =>  pd.ProductId);

            // data seed 

            modelBuilder.Entity<Product>().HasData(ProductSeeder.GenerateProductData());
        }
    }
}
// instalacja dotnet ef 
//dotnet tool install --global dotnet-ef

// aktualizacja 
//dotnet tool update --global dotnet-ef

// dotnet ef migrations add [nazwa_migracji]
// dotnet ef database update 


// cofniecie migraji niezaplikowanych 
//dotnet ef migrations remove
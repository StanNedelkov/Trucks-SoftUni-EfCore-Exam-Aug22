namespace Trucks.Data
{
    using Microsoft.EntityFrameworkCore;
    using Trucks.Data.Models;

    public class TrucksContext : DbContext
    {
        public TrucksContext() { }

        public TrucksContext(DbContextOptions options)
            : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(Configuration.ConnectionString);
            }
        }
        public DbSet<Client> Clients { get; set; }
        public DbSet<ClientTruck> ClientsTrucks { get; set; }
        public DbSet<Despatcher> Despatchers { get; set; }
        public DbSet<Truck> Trucks { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ClientTruck>(ct =>
            {
                ct.HasKey(k => new { k.ClientId, k.TruckId });
            });
        }
    }
}

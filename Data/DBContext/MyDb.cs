using Data.Models;

using Microsoft.EntityFrameworkCore;
namespace Data.DBContext
{
    public class MyDb : DbContext
    {
        public MyDb(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<AcessToken> AcessTokens { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<Aircraft> Aircraft { get; set; }
        public DbSet<Airport> Airports { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<Routes> Routes { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Flight>()
                .HasOne(f => f.Route)
                .WithMany()
                .HasForeignKey(f => f.RouteId);

            modelBuilder.Entity<Flight>()
                .HasOne(f => f.Aircraft)
                .WithMany()
                .HasForeignKey(f => f.AircraftId);


            modelBuilder.Entity<Routes>()
                .HasOne(r => r.DepartureAirport)
                .WithMany()
                .HasForeignKey(r => r.DepartureAirportId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Routes>()
                .HasOne(r => r.ArrivalAirport)
                .WithMany()
                .HasForeignKey(r => r.ArrivalAirportId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Ticket>()
               .HasOne(f => f.Flight)
               .WithMany()
               .HasForeignKey(f => f.FlightId);

            modelBuilder.Entity<Flight>()
                 .Property(p => p.BusinessFlexiblePrice)
                 .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Flight>()
                 .Property(p => p.EconomyFlexiblePrice)
                 .HasColumnType("decimal(18,2)");


            modelBuilder.Entity<Ticket>()
                .Property(p => p.AmountTicket)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Ticket>()
                .Property(p => p.AmountTotal)
                .HasColumnType("decimal(18,2)");

        }
    }
}

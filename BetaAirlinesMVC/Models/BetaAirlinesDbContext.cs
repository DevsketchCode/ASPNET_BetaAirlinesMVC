using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BetaAirlinesMVC.Models
{
    public class BetaAirlinesDbContext : DbContext
    {

        // Each table in the database that we want to use
        public DbSet<Airport> Airports { get; set; }
        public DbSet<BookedFlight> BookedFlights { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        // Set Foreign Key for the Destination and ArrivalAirport for the Flights table
        // Since both were linked to the same table (Airports), it had to be linked this way to disable CascadeOnDelete
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Fluent API configurations for the Flight table
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Flight>()
                .HasRequired(m => m.DepartureAirport)
                .WithMany(m => m.DepartingAirports)
                .HasForeignKey(m => m.DepartureAirportId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Flight>()
                .HasRequired(m => m.ArrivalAirport)
                .WithMany(m => m.ArrivingAirports)
                .HasForeignKey(m => m.ArrivalAirportId)
                .WillCascadeOnDelete(false);
        }

        public System.Data.Entity.DbSet<BetaAirlinesMVC.ViewModel.BookAFlightViewModel> BookAFlightViewModels { get; set; }
    }
}
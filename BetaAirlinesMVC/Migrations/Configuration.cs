// To create this migration
// NuGet Package Manager, type Enable-Migrations

// If make a change to the model and need to update the database
// NuGet Package Manager, Package Manager Console, Add-Migration, then provide Name for the change (is attached to end of filename)
// Then apply it by typing Update-Database

namespace BetaAirlinesMVC.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BetaAirlinesMVC.Models.BetaAirlinesDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "BetaAirlinesMVC.Models.BetaAirlinesDbContext";
        }

        protected override void Seed(BetaAirlinesMVC.Models.BetaAirlinesDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}

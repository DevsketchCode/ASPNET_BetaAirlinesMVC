namespace BetaAirlinesMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Airports",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false, maxLength: 80),
                    ICAO = c.String(nullable: false, maxLength: 6),
                    City = c.String(nullable: false, maxLength: 50),
                    State = c.String(nullable: false, maxLength: 50),
                    Country = c.String(nullable: false, maxLength: 50),
                    Active = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Flights",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    DepartureDate = c.DateTime(nullable: false),
                    DepartureAirportId = c.Int(nullable: false),
                    ArrivalAirportId = c.Int(nullable: false),
                    FlightLengthInMinutes = c.Int(nullable: false),
                    Active = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Airports", t => t.ArrivalAirportId)
                .ForeignKey("dbo.Airports", t => t.DepartureAirportId)
                .Index(t => t.DepartureAirportId)
                .Index(t => t.ArrivalAirportId);

            CreateTable(
                "dbo.BookedFlights",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    DateBooked = c.DateTime(nullable: false),
                    Active = c.Int(nullable: false),
                    UserId = c.Int(nullable: false),
                    FlightId = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Flights", t => t.FlightId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.FlightId);

            CreateTable(
                "dbo.Users",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    FirstName = c.String(nullable: false, maxLength: 30),
                    LastName = c.String(nullable: false, maxLength: 30),
                    Username = c.String(nullable: false, maxLength: 30),
                    Password = c.String(nullable: false),
                    RegisteredDate = c.DateTime(nullable: false),
                    Active = c.Int(nullable: false),
                    UserRoleID = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserRoles", t => t.UserRoleID, cascadeDelete: true)
                .Index(t => t.UserRoleID);

            CreateTable(
                "dbo.UserRoles",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Role = c.String(nullable: false, maxLength: 30),
                    Description = c.String(maxLength: 150),
                    Active = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Contact",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false),
                    Email = c.String(nullable: false),
                    Subject = c.String(nullable: false),
                    Message = c.String(nullable: false),
                    Date = c.DateTime(nullable: false),
                    Active = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id);

        }

        public override void Down()
        {
            DropForeignKey("dbo.BookedFlights", "FlightId", "dbo.Flights");
            DropForeignKey("dbo.BookedFlights", "UserId", "dbo.Users");
            DropForeignKey("dbo.Users", "UserRoleID", "dbo.UserRoles");
            DropForeignKey("dbo.Flights", "DepartureAirportId", "dbo.Airports");
            DropForeignKey("dbo.Flights", "ArrivalAirportId", "dbo.Airports");
            DropIndex("dbo.Users", new[] { "UserRoleID" });
            DropIndex("dbo.BookedFlights", new[] { "FlightId" });
            DropIndex("dbo.BookedFlights", new[] { "UserId" });
            DropIndex("dbo.Flights", new[] { "ArrivalAirportId" });
            DropIndex("dbo.Flights", new[] { "DepartureAirportId" });
            DropTable("dbo.Contact");
            DropTable("dbo.UserRoles");
            DropTable("dbo.Users");
            DropTable("dbo.BookedFlights");
            DropTable("dbo.Flights");
            DropTable("dbo.Airports");
        }
    }
}

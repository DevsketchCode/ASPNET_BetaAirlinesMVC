namespace BetaAirlinesMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class undoUserRoleBool : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UserRoles", "Active", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserRoles", "Active", c => c.Boolean(nullable: false));
        }
    }
}

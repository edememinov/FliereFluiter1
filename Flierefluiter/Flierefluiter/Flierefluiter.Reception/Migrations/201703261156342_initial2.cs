namespace Flierefluiter.Reception.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "ConfirmationToken");
            DropColumn("dbo.AspNetUsers", "IsConfirmed");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "IsConfirmed", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "ConfirmationToken", c => c.String());
        }
    }
}

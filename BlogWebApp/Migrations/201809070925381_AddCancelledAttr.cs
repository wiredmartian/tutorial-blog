namespace BlogWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCancelledAttr : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Posts", "Cancelled", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Posts", "Cancelled");
        }
    }
}

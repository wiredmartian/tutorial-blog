namespace BlogWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLastUpdatedAttr : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Posts", "LastUpdated", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Posts", "LastUpdated");
        }
    }
}

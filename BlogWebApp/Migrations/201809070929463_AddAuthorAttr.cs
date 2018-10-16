namespace BlogWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAuthorAttr : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Posts", "Author", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Posts", "Author");
        }
    }
}

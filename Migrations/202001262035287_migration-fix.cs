namespace Journal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrationfix : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Posts", "PublisherId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Posts", "PublisherId");
        }
    }
}

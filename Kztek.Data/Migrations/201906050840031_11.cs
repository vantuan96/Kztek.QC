namespace Kztek.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _11 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.News", "DetailPath", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.News", "DetailPath");
        }
    }
}

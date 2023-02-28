namespace Kztek.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _8 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NewsCategory", "Type", c => c.String());
            DropColumn("dbo.ProductCategory", "Type");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ProductCategory", "Type", c => c.String());
            DropColumn("dbo.NewsCategory", "Type");
        }
    }
}

namespace Kztek.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WebInfo", "Phone", c => c.String());
            AlterColumn("dbo.Contact", "Description", c => c.String(maxLength: 1000));
            AlterColumn("dbo.CustomerComment", "Description", c => c.String(maxLength: 1000));
            AlterColumn("dbo.Customer", "Description", c => c.String(maxLength: 1000));
            AlterColumn("dbo.MainMenu", "Description", c => c.String(maxLength: 1000, unicode: false));
            AlterColumn("dbo.Media", "Description", c => c.String(maxLength: 1000));
            AlterColumn("dbo.NewsCategory", "Description", c => c.String(maxLength: 500));
            AlterColumn("dbo.News", "Description", c => c.String(maxLength: 1000));
            AlterColumn("dbo.ProductCategory", "Description", c => c.String(maxLength: 1000));
            AlterColumn("dbo.Role", "Description", c => c.String(maxLength: 250));
            DropColumn("dbo.NewsCategory", "MetaKeywork");
        }
        
        public override void Down()
        {
            AddColumn("dbo.NewsCategory", "MetaKeywork", c => c.String(maxLength: 500));
            AlterColumn("dbo.Role", "Description", c => c.String(maxLength: 1000));
            AlterColumn("dbo.ProductCategory", "Description", c => c.String());
            AlterColumn("dbo.News", "Description", c => c.String());
            AlterColumn("dbo.NewsCategory", "Description", c => c.String());
            AlterColumn("dbo.Media", "Description", c => c.String(maxLength: 4000));
            AlterColumn("dbo.MainMenu", "Description", c => c.String(maxLength: 8000, unicode: false));
            AlterColumn("dbo.Customer", "Description", c => c.String());
            AlterColumn("dbo.CustomerComment", "Description", c => c.String());
            AlterColumn("dbo.Contact", "Description", c => c.String());
            DropColumn("dbo.WebInfo", "Phone");
        }
    }
}

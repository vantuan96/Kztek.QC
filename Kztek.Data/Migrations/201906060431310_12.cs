namespace Kztek.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _12 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.NewsCategory", "MetaTitle", c => c.String(maxLength: 500));
            AlterColumn("dbo.NewsCategory", "MetaDesc", c => c.String(maxLength: 1000));
            AlterColumn("dbo.News", "MetaTitle", c => c.String(maxLength: 500));
            AlterColumn("dbo.News", "MetaDesc", c => c.String(maxLength: 1000));
            AlterColumn("dbo.News", "MetaKeywork", c => c.String(maxLength: 500));
            AlterColumn("dbo.ProductCategory", "MetaTitle", c => c.String(maxLength: 500));
            AlterColumn("dbo.ProductCategory", "MetaDesc", c => c.String(maxLength: 1000));
            AlterColumn("dbo.ProductCategory", "MetaKeywork", c => c.String(maxLength: 500));
            AlterColumn("dbo.Product", "MetaTitle", c => c.String(maxLength: 500));
            AlterColumn("dbo.Product", "MetaDesc", c => c.String(maxLength: 1000));
            AlterColumn("dbo.Product", "MetaKeywork", c => c.String(maxLength: 500));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Product", "MetaKeywork", c => c.String(maxLength: 500, unicode: false));
            AlterColumn("dbo.Product", "MetaDesc", c => c.String(maxLength: 1000, unicode: false));
            AlterColumn("dbo.Product", "MetaTitle", c => c.String(maxLength: 500, unicode: false));
            AlterColumn("dbo.ProductCategory", "MetaKeywork", c => c.String(maxLength: 500, unicode: false));
            AlterColumn("dbo.ProductCategory", "MetaDesc", c => c.String(maxLength: 1000, unicode: false));
            AlterColumn("dbo.ProductCategory", "MetaTitle", c => c.String(maxLength: 500, unicode: false));
            AlterColumn("dbo.News", "MetaKeywork", c => c.String(maxLength: 500, unicode: false));
            AlterColumn("dbo.News", "MetaDesc", c => c.String(maxLength: 1000, unicode: false));
            AlterColumn("dbo.News", "MetaTitle", c => c.String(maxLength: 500, unicode: false));
            AlterColumn("dbo.NewsCategory", "MetaDesc", c => c.String(maxLength: 1000, unicode: false));
            AlterColumn("dbo.NewsCategory", "MetaTitle", c => c.String(maxLength: 500, unicode: false));
        }
    }
}

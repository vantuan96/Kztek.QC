namespace Kztek.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Contact",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128, unicode: false),
                        CustomerId = c.String(maxLength: 128, unicode: false),
                        Description = c.String(maxLength: 1000),
                        IPCustomer = c.String(nullable: false, maxLength: 50),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CustomerComment",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128, unicode: false),
                        FullName = c.String(nullable: false, maxLength: 500),
                        Avartar = c.String(maxLength: 500),
                        Summary = c.String(maxLength: 1000),
                        Description = c.String(maxLength: 1000),
                        Ordering = c.Int(),
                        DateCreated = c.DateTime(nullable: false),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Customer",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128, unicode: false),
                        FullName = c.String(nullable: false, maxLength: 500),
                        Email = c.String(maxLength: 255, unicode: false),
                        Mobile = c.String(maxLength: 50, unicode: false),
                        Phone = c.String(maxLength: 50, unicode: false),
                        Address = c.String(maxLength: 1000),
                        Gender = c.Boolean(nullable: false),
                        Website = c.String(maxLength: 255, unicode: false),
                        Avartar = c.String(maxLength: 1000),
                        Country = c.String(maxLength: 50),
                        Description = c.String(maxLength: 1000),
                        DateCreated = c.DateTime(nullable: false),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MainMenu",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128, unicode: false),
                        Name = c.String(nullable: false, maxLength: 500),
                        ParentId = c.String(maxLength: 128, unicode: false),
                        Depth = c.Int(),
                        BreadCrumb = c.String(maxLength: 500, unicode: false),
                        NameUrl = c.String(maxLength: 500, unicode: false),
                        Url = c.String(maxLength: 1000, unicode: false),
                        IconPath = c.String(maxLength: 1000, unicode: false),
                        CoverPath = c.String(maxLength: 1000, unicode: false),
                        Description = c.String(maxLength: 1000, unicode: false),
                        Ordering = c.Int(nullable: false),
                        Page = c.String(maxLength: 10, unicode: false),
                        Position = c.String(maxLength: 10, unicode: false),
                        Target = c.String(maxLength: 10, unicode: false),
                        DateCreated = c.DateTime(nullable: false),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Media",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128, unicode: false),
                        Name = c.String(nullable: false, maxLength: 500),
                        Description = c.String(maxLength: 1000),
                        Alt = c.String(maxLength: 1000, unicode: false),
                        Url = c.String(maxLength: 1000, unicode: false),
                        Path = c.String(maxLength: 1000, unicode: false),
                        Page = c.String(maxLength: 50, unicode: false),
                        Position = c.String(maxLength: 50, unicode: false),
                        MediaType = c.String(maxLength: 50, unicode: false),
                        TotalView = c.Int(),
                        Ordering = c.Int(),
                        DateCreated = c.DateTime(nullable: false),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.NewsCategory",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128, unicode: false),
                        Name = c.String(nullable: false, maxLength: 500),
                        ParentId = c.String(maxLength: 128, unicode: false),
                        Depth = c.Int(),
                        BreadCrumb = c.String(maxLength: 500),
                        NameUrl = c.String(maxLength: 500, unicode: false),
                        MetaTitle = c.String(maxLength: 500, unicode: false),
                        MetaDesc = c.String(maxLength: 1000, unicode: false),
                        Description = c.String(maxLength: 500),
                        IconPath = c.String(maxLength: 1000),
                        CoverPath = c.String(maxLength: 1000),
                        Ordering = c.Int(),
                        DateCreated = c.DateTime(nullable: false),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.News",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128, unicode: false),
                        NewsCategoryId = c.String(maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 500),
                        NameUrl = c.String(maxLength: 1000),
                        Summary = c.String(maxLength: 1000),
                        Description = c.String(maxLength: 1000),
                        MetaTitle = c.String(maxLength: 500, unicode: false),
                        MetaDesc = c.String(maxLength: 1000, unicode: false),
                        MetaKeywork = c.String(maxLength: 500, unicode: false),
                        CoverPath = c.String(maxLength: 1000),
                        TotalView = c.Int(),
                        NewsType = c.Int(),
                        DateCreated = c.DateTime(nullable: false),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProductCategory",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128, unicode: false),
                        Name = c.String(nullable: false, maxLength: 500),
                        NameUrl = c.String(maxLength: 1000),
                        ParentId = c.String(maxLength: 128),
                        Depth = c.Int(),
                        BreadCrumb = c.String(maxLength: 500),
                        MetaTitle = c.String(maxLength: 500, unicode: false),
                        MetaDesc = c.String(maxLength: 1000, unicode: false),
                        MetaKeywork = c.String(maxLength: 500, unicode: false),
                        Description = c.String(maxLength: 1000),
                        IconPath = c.String(maxLength: 1000),
                        CorverPath = c.String(maxLength: 1000),
                        Ordering = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Product",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128, unicode: false),
                        Name = c.String(nullable: false, maxLength: 500),
                        Barcode = c.String(maxLength: 128, unicode: false),
                        ProductCategoryId = c.String(maxLength: 128, unicode: false),
                        Summary = c.String(maxLength: 1000),
                        Description = c.String(),
                        NameUrl = c.String(maxLength: 1000),
                        MetaTitle = c.String(maxLength: 500, unicode: false),
                        MetaDesc = c.String(maxLength: 1000, unicode: false),
                        MetaKeywork = c.String(maxLength: 500, unicode: false),
                        CorverPath = c.String(maxLength: 1000),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PricePromotion = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Quantity = c.Int(),
                        Ordering = c.Int(),
                        DateCreated = c.DateTime(nullable: false),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Product");
            DropTable("dbo.ProductCategory");
            DropTable("dbo.News");
            DropTable("dbo.NewsCategory");
            DropTable("dbo.Media");
            DropTable("dbo.MainMenu");
            DropTable("dbo.Customer");
            DropTable("dbo.CustomerComment");
            DropTable("dbo.Contact");
        }
    }
}

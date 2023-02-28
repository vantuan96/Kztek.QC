namespace Kztek.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Log",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        TableName = c.String(maxLength: 150),
                        ColumnId = c.String(maxLength: 150),
                        Action = c.String(maxLength: 50),
                        isSuccess = c.Boolean(nullable: false),
                        Message = c.String(),
                        UserId = c.String(maxLength: 50),
                        UserName = c.String(maxLength: 50),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MenuFunction",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        MenuName = c.String(nullable: false),
                        ControllerName = c.String(maxLength: 150),
                        MenuType = c.String(maxLength: 10),
                        ActionName = c.String(maxLength: 150),
                        Url = c.String(maxLength: 1000),
                        Icon = c.String(maxLength: 100),
                        ParentId = c.String(maxLength: 100),
                        Active = c.Boolean(nullable: false),
                        OrderNumber = c.Int(),
                        Breadcrumb = c.String(),
                        Dept = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RoleMenu",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        MenuId = c.String(maxLength: 150, unicode: false),
                        RoleId = c.String(maxLength: 150, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Role",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        RoleName = c.String(maxLength: 150),
                        Description = c.String(maxLength: 250),
                        Active = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Trash",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        TableName = c.String(),
                        ColumnId = c.String(),
                        LogId = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserConfig",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(maxLength: 50, unicode: false),
                        StationDefaultId = c.String(maxLength: 50, unicode: false),
                        DateCreated = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserRole",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(maxLength: 150, unicode: false),
                        RoleId = c.String(maxLength: 150, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Email = c.String(maxLength: 250),
                        ImagePath = c.String(maxLength: 500, unicode: false),
                        Username = c.String(nullable: false, maxLength: 100),
                        Password = c.String(maxLength: 500, unicode: false),
                        PasswordSalat = c.String(maxLength: 500, unicode: false),
                        Address = c.String(maxLength: 500),
                        Phone = c.String(maxLength: 150, unicode: false),
                        Admin = c.Boolean(nullable: false),
                        Active = c.Boolean(nullable: false),
                        UserAvatar = c.String(),
                        DateCreated = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.WebInfo",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        WebsiteName = c.String(maxLength: 500),
                        MetaTitle = c.String(maxLength: 500),
                        MetaDesc = c.String(maxLength: 1000),
                        MetaKeywork = c.String(maxLength: 1000),
                        PagingFontEnd = c.Int(nullable: false),
                        PagingBackEnd = c.Int(nullable: false),
                        EmailSystem = c.String(maxLength: 255),
                        EmailPassSystem = c.String(maxLength: 255),
                        EmailBcc = c.String(),
                        EmailCC = c.String(maxLength: 1000),
                        LogoPath = c.String(maxLength: 1000),
                        LogoUrl = c.String(maxLength: 1000),
                        MasterToolCode = c.String(maxLength: 1000),
                        AnalyticsCode = c.String(maxLength: 1000),
                        LinkGoogleMap = c.String(maxLength: 1000),
                        FaceBookCode = c.String(maxLength: 1000),
                        FanpageUrl = c.String(maxLength: 1000),
                        CompanyInfo = c.String(storeType: "ntext"),
                        ChatCode = c.String(maxLength: 1000),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.WebInfo");
            DropTable("dbo.User");
            DropTable("dbo.UserRole");
            DropTable("dbo.UserConfig");
            DropTable("dbo.Trash");
            DropTable("dbo.Role");
            DropTable("dbo.RoleMenu");
            DropTable("dbo.MenuFunction");
            DropTable("dbo.Log");
        }
    }
}

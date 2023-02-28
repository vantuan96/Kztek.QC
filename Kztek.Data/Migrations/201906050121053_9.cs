namespace Kztek.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _9 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomerComment", "Type", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CustomerComment", "Type");
        }
    }
}

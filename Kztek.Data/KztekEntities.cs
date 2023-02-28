using Kztek.Model.Models;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;


namespace Kztek.Data
{
    public class KztekEntities : DbContext
    {
        public KztekEntities()
            : base("KztekEntities")
        {
            Database.SetInitializer<KztekEntities>(null);
            this.Database.CommandTimeout = 180;
        }

        public KztekEntities(string conn) : base(conn)
        {
            Database.SetInitializer<KztekEntities>(null);
        }

        //Hệ thống
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<MenuFunction> MenuFunctions { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<RoleMenu> RoleMenus { get; set; }
        public DbSet<UserConfig> UserConfigs { get; set; }
        public DbSet<WebInfo> WebInfos { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Trash> Trashs { get; set; }

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerComment> CustomerComments { get; set; }
        public DbSet<MainMenu> MainMenus { get; set; }
        public DbSet<Media> Medias { get; set; }
        public DbSet<News> Newss { get; set; }
        public DbSet<NewsCategory> NewsCategorys { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategorys { get; set; }
        public virtual void Commit()
        {
            base.SaveChanges();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<IncludeMetadataConvention>();
        }
    }
}

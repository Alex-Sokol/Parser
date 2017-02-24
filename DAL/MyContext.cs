using System.Data.Entity;
using DAL.Entities;

namespace DAL
{
    public class MyContext : DbContext
    {
        static MyContext()
        {
            Database.SetInitializer(new DbInitializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<MyContext>(null);
            base.OnModelCreating(modelBuilder);
        }

        public MyContext(string name) : base(name)
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<MyContext>());
        }

        public DbSet<Site> Sites { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<CssFile> CssFiles { get; set; }
        public DbSet<SiteForParsing> SiteForParsing { get; set; }
    }
}
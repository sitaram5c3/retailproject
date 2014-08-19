using System.Data.Entity;

namespace LocateSense.Models
{
    public class LocateSenseContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, add the following
        // code to the Application_Start method in your Global.asax file.
        // Note: this will destroy and re-create your database with every model change.
        // 
        // System.Data.Entity.Database.SetInitializer(new System.Data.Entity.DropCreateDatabaseIfModelChanges<LocateSense.Models.LocateSenseContext>());

        public LocateSenseContext() : base("name=LocateSenseContext")
        {
        }

        public DbSet<user> users { get; set; }
        public DbSet<offer> offers { get; set; }
        public DbSet<product> products { get; set; }
        public DbSet<locate> locate { get; set; }

    }
}

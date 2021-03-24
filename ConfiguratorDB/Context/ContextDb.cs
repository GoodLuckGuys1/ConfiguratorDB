using ConfiguratorDB.Models;
using System.Data.Entity;

namespace ConfiguratorDB.Context
{
    public class ContextDb : DbContext
    {
        public ContextDb(string connectionString) : base(connectionString) { Database.CreateIfNotExists(); }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Customer> Customers { get; set; }
    }
}

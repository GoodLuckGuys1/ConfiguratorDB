using ConfiguratorDB.Context;
using ConfiguratorDB.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace ConfiguratorDB.Repositories
{
    public class CustomerRepository
    {
        ContextDb context;
        public CustomerRepository(ContextDb context)
        {
            this.context = context;
        }
        public async Task AddCustomer(Customer customer)
        {
            context.Customers.Add(customer);
            await context.SaveChangesAsync();
        }

        public async Task<List<Customer>> GetCustomers()
        {
            return await context.Customers.ToListAsync();
        }
    }
}

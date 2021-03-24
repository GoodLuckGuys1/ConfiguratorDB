using ConfiguratorDB.Context;
using ConfiguratorDB.Models;
using System.Threading.Tasks;

namespace ConfiguratorDB.Repositories
{
    public class OrderRepository
    {
        ContextDb context;
        public OrderRepository(ContextDb context)
        {
            this.context = context;
        }
        public async Task AddOrder(Order order)
        {
            context.Orders.Add(order);
            await context.SaveChangesAsync();
        }
    }
}

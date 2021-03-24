using System;
using System.ComponentModel.DataAnnotations;

namespace ConfiguratorDB.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public Customer Customer { get; set; }
        public DateTime OrderDate { get; set; }

        public decimal Price { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace ConfiguratorDB.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(64)]
        public string FirstName { get; set; }

        [MaxLength(64)]
        public string LastName { get; set; }

        [MaxLength(64)]
        public string MiddleName { get; set; }

        [MaxLength(32)]
        public string Sex { get; set; }
        public DateTime BirthDate { get; set; }

        public DateTime RegistrationDate { get; set; }
    }
}

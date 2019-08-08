using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Orders.Model.Entities
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        // Simplified for the test project
        // TODO: Should be split to another entity ("Customer") with authorization
        [Required]
        public string FirstName { get; set; }
        
        public string MiddleName { get; set; }

        [Required]
        public string LastName { get; set; }

        // Simplified for the test project
        // The "Fruit" is designed for being selected via radio button
        // TODO: Should be split to another entity ("Product")
        public Fruit Fruit { get; set; }

        [Range(1,int.MaxValue)]
        public int Amount { get; set; }

        public bool IsCallbackRequired { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public DateTimeOffset ETA { get; set; }
    }
}

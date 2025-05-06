using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Domain.Models
{
    [Table("tblClient", Schema = "public")]
    public class Client
    {
        public Client() {
            Orders = new List<Order>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("IdClient")]
        public int IdClient { get; set; }

        [Column("FirstName")]
        public string FirstName { get; set; }

        [Column("LastName")]
        public string LastName { get; set; }

        [Column("Address")]
        public string Address { get; set; }

        // Relación 1:N (Un cliente tiene muchas órdenes)
        public virtual List<Order> Orders { get; set; }
    }
}

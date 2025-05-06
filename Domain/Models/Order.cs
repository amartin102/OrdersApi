using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Domain.Models
{
    [Table("tblOrder", Schema = "public")]
    public class Order
    {
        public Order()
        {
            OrderItems = new List<OrderItem>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("IdOrder")]
        public int IdOrder { get; set; }

        [Column("SubTotal")]
        public decimal SubTotal { get; set; }

        [Column("Iva")]
        public decimal Iva { get; set; }

        [Column("Total")]
        public decimal Total { get; set; }

        [Column("IdClient")]
        public int IdClient { get; set; }

        [Column("Status")]
        public int Status { get; set; }

        [Column("CreationDate", TypeName = "timestamp")]      
        public DateTime CreationDate { get; set; }

        [Column("ModificationDate", TypeName = "timestamp")]
        public DateTime ModificationDate { get; set; }

        public virtual Client Client { get; set; }
                
        public virtual List<OrderItem> OrderItems { get; set; }
    }
}

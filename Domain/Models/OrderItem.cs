using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Domain.Models
{
    [Table("tblOrderItem", Schema = "public")]
    public class OrderItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("IdOrderItem")]
        public int IdOrderItem { get; set; }

        [Column("IdOrder")]
        public int IdOrder { get; set; }

        [Column("IdItem")]
        public int IdItem { get; set; }

        [Column("Ammount")]
        public int Ammount { get; set; }

        [Column("Active")]
        public bool Active { get; set; }

        public virtual Order Order { get; set; }                
        public virtual Item Item { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;

namespace Domain.Models
{
    [Table("tblItem", Schema = "public")]
    public class Item
    {
        public Item()
        {
           Items = new List<OrderItem>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("IdItem")]
        public int IdItem { get; set; }

        [Column("Description")]
        public string Description { get; set; }

        [Column("PriceUnit")]
        public decimal PriceUnit { get; set; }

        [Column("IdRecipe")]
        public int IdRecipe { get; set; }

        [JsonIgnore]
        public virtual List<OrderItem> Items { get; set; }
    }
}

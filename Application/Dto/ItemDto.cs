using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto
{
    public class ItemDto
    {
        public int IdItem { get; set; }

        public string Description { get; set; }

        public decimal PriceUnit { get; set; }

        public int Ammount { get; set; }

        public int RecipeId { get; set; }
    }
}

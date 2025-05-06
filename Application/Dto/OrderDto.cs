using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto
{
    public class OrderDto
    {
        public int IdOrder { get; set; }
        public string ClientName { get; set; }
        public string Address { get; set; }
        public string SubTotal { get; set; }
        public string Iva { get; set; }
        public decimal Total { get; set; }
        public int Status { get; set; }
        public List<ItemDto> items { get; set; }

        public OrderDto()
        {
            items = new List<ItemDto>();
        }
    }
}

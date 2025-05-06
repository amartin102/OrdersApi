using Domain.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Dto
{
    public class ProcessOrderDto
    {
        public int IdOrder { get; set; }

        public int IdClient { get; set; }
               
        public List<ItemsDto> items { get; set; }
    }
}

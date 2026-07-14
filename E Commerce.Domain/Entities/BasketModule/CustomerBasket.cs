using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Entities.BasketModule
{
    public class CustomerBasket
    {
        public int Id { get; set; } = default!; // Guid From Client Side
        public ICollection<BasketItem> Items { get; set; } = [];

    }
}

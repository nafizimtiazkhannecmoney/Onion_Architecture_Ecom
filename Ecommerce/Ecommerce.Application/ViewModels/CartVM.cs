using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.ViewModels
{
    public class CartVM
    {
        public OrderProduct OrderProduct { get; set; }
        public List<Cart> ListCart { get; set; }
    }
}

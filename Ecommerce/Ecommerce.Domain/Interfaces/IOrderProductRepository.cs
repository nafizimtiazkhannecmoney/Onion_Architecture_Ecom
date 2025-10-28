using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Domain.Interfaces
{
    public interface IOrderProductRepository : IRepository<OrderProduct>
    {
        void UpdateStatus(int id, string orderStatus, string? paymentStatus = null);
    }
}

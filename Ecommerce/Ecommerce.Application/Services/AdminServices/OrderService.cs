using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.Application.ViewModels;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Interfaces;

namespace Ecommerce.Application.Services.AdminServices
{
    public class OrderService
    {
        public readonly IUnitOfWork _unitOfWork;
        public OrderVM OrderVM { get; set; }

        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<OrderProduct> GetAll() 
        {
            var orderList = _unitOfWork.OrderProduct.GetAll(x => x.OrderStatus != "Delivered");
            return orderList;
        }

        public OrderVM Details(int id)
        {
            OrderVM = new OrderVM()
            {
                OrderProduct = _unitOfWork.OrderProduct.GetFirstOrDefault(
                    o => o.Id == id, includeProperties: "AppUser"),
                OrderDetails = _unitOfWork.OrderDetails.GetAll(
                    od => od.OrderProductId == id, includeProperties: "Product")
            };
            return OrderVM;
        }

        public OrderVM Delivered(OrderVM orderVM)
        {
            var orderProduct = _unitOfWork.OrderProduct.GetFirstOrDefault(op => op.Id == orderVM.OrderProduct.Id);
            orderProduct.OrderStatus = "Delivered";
            _unitOfWork.OrderProduct.Update(orderProduct);
            _unitOfWork.Save();

            return orderVM;
        }
    }
}

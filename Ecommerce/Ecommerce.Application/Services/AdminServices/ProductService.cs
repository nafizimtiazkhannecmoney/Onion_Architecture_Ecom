using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.Application.ViewModels;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ecommerce.Application.Services.AdminServices
{
    public class ProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private string _wwwRootPath;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            IEnumerable<Product> productList = _unitOfWork.Product.GetAll();
            return productList;
        }

        public ProductVM GetProductVM(int? id) 
        {
            var productVM = new ProductVM()
            {
                Product = new Product(),
                CategoryList = _unitOfWork.Category.GetAll()
                .Select(l => new SelectListItem
                {
                    Text  = l.Name,
                    Value = l.Id.ToString()
                })
            };
            

            if (id.HasValue && id > 0)
            {
                productVM.Product = _unitOfWork.Product.GetFirstOrDefault(p => p.Id == id);
            }

            return productVM;
        }

        public void UpsertProduct(ProductVM productVM, IFormFile file)
        {
            if (file != null)
            {
                string fileName = Guid.NewGuid().ToString();
                var uploadRoot = Path.Combine(_wwwRootPath, "img", "products");
                var extension = Path.GetExtension(file.FileName);

                if (!string.IsNullOrEmpty(productVM.Product.Picture))
                {
                    var oldPicPath = Path.Combine(_wwwRootPath, productVM.Product.Picture);
                    if (File.Exists(oldPicPath))
                    {
                        File.Delete(oldPicPath);
                    }
                }
                using (var fileSteram = new FileStream(Path.Combine(uploadRoot, fileName + extension), FileMode.Create))
                {
                    file.CopyTo(fileSteram);
                }

                productVM.Product.Picture = Path.Combine("img", "products", fileName + extension);
            }

            if (productVM.Product.Id <= 0)
            {
                _unitOfWork.Product.Add(productVM.Product);
            }
            else
            {
                _unitOfWork.Product.Update(productVM.Product);
            }
            _unitOfWork.Save();
        }

        public void DeleteProduct(int? id)
        {
            var productToBeDeleted = _unitOfWork.Product.GetFirstOrDefault(d => d.Id == id);
            _unitOfWork.Product.Remove(productToBeDeleted);
            _unitOfWork.Save();
        }

    }
}

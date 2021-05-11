using Business.Abstract;
using DataAccess.Abstract;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {
        private IProductDal _productDal;
        public ProductManager(IProductDal productDal)
        {
            _productDal = productDal;
        }
        public void Add(Product product)
        {
            _productDal.Add(product);
        }

        public void Delete(Product product)
        {
            _productDal.Delete(product);
        }

        public List<Product> GetAll()
        {
           return  _productDal.GetAll();
        }

        public List<Product> GetByCategoryId(int categoryId)
        {
           return  _productDal.GetAll(p => p.CategoryId == categoryId);
        }

        public Product GetById(int id)
        {
            return _productDal.Get(p => p.Id == id);
        }

        public List<Product> GetByUserId(int userId)
        {
            return _productDal.GetAll(p => p.UserId == userId);
        }

        //public List<Product> GetByUserOrderId(int userOrderId)
        //{
        //   return  _productDal.GetAll(p => p.UserOrderId == userOrderId);
        //}

        public void Update(Product product)
        {
            _productDal.Update(product);
        }

        public List<Product> ViewAllProducts(int userId)
        {
            List<Product> products = new List<Product>();
           var allProduct =  _productDal.GetAll();

            foreach (var item in allProduct)
            {
                if (item.UserId != userId)
                {
                    products.Add(item);
                }
            }
            return products;
        }
    }
}

using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IProductService
    {
        List<Product> GetAll(); // userId yapmalısın.
        Product GetById(int id);
        void Add(Product product);
        void Update(Product product);
        void Delete(Product product);
        List<Product> GetByCategoryId(int categoryId);
        List<Product> GetByUserId(int userId);
        //List<Product> GetByUserOrderId(int userOrderId);
        List<Product> ViewAllProducts(int userId);

        
    }

}

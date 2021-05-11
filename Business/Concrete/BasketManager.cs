using Business.Abstract;
using DataAccess.Abstract;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class BasketManager : IBasketService
    {
        IBasketDal _basketDal;
        IProductService _productService;
        public BasketManager(IBasketDal basketDal , IProductService productService)
        {
            _basketDal = basketDal;
            _productService = productService;
        }

        public void Add(Basket basket)
        {
           
            _basketDal.Add(basket);
        }

        public List<Basket> Baskets(int userId)
        {
            //List<Product> products = new List<Product>();
           var result =  _basketDal.GetAll(b => b.UserID == userId);
            return result;
        }

        public void Delete(Basket basket)
        {
            _basketDal.Delete(basket);
        }

        public Basket GetById(int basketId)
        {
           return  _basketDal.Get(b => b.Id == basketId);
        }
    }
}

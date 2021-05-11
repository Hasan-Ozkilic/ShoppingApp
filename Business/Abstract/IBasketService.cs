using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IBasketService
    {
        List<Basket> Baskets(int userId);
        void Add(Basket basket);
        void Delete(Basket basket);
        Basket GetById(int basketId);
    }
}

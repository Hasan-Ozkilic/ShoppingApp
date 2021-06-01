using Business.Abstract;
using DataAccess.Abstract;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class OrderManager:IOrderService
    {
        private IOrderDal _orderDal;
        public OrderManager(IOrderDal orderDal )
        {
            _orderDal = orderDal;
        }

        public void Add(Order order)
        {
            _orderDal.Add(order);
        }

        public void Delete(Order order)
        {
            _orderDal.Delete(order);
        }

        public Order GetById(int id)
        {
            return _orderDal.Get(o => o.Id == id);

        }

        public Order GetByUserId(int userId)
        {
            return _orderDal.Get(o => o.UserId == userId);
        }
    }
}

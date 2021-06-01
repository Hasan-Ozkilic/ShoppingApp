using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IOrderService
    {
        void Add(Order order);
        void Delete(Order order);
        Order GetByUserId(int userId);
        Order GetById(int id);

    }
}

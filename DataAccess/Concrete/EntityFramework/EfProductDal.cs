using DataAccess.Abstract;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfProductDal :EfEntityRepositoryBase<Product,ShoppingContext>, IProductDal
    {
      
    }
}

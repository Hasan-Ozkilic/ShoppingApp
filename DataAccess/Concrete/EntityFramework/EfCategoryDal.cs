using DataAccess.Abstract;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Concrete.EntityFramework
{
   public  class EfCategoryDal:EfEntityRepositoryBase<Category,ShoppingContext>,ICategoryDal
    {
    }
}

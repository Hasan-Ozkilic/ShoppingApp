using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
   public  interface ICategoryService
    {
        List<Category> GetAll();            
        Category GetById(int id);
        void Add(Category category);
        void Update(Category category);
        void Delete(Category category);
    }
}

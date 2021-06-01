using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
   public  interface IFavorilerService
    {
        void Add(Favoriler favoriler);

        void Delete(Favoriler favoriler);
        List<Favoriler> GetAll(int userId);
        List<Favoriler> GetByCategoryId(int userId, int categoryId);
        Favoriler GetById(int favoriId);
        void Delete(int productId);
    }
}

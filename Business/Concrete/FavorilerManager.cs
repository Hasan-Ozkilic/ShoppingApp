
using Business.Abstract;
using DataAccess.Abstract;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Concrete
{
    public class FavorilerManager:IFavorilerService
    {
        IFavorilerDal _favorilerDal;
        IProductService _productService;
        public FavorilerManager(IFavorilerDal favorilerDal , IProductService productService)
        {

            _favorilerDal = favorilerDal;
            _productService = productService;
        }

        public void Add(Favoriler favoriler)
        {
            _favorilerDal.Add(favoriler);
        }

        public void Delete(Favoriler favoriler)
        {
            _favorilerDal.Delete(favoriler);
        }

        public List<Favoriler> GetAll(int userId)
        {
         var favoriler=    _favorilerDal.GetAll();
          return   favoriler.Where(f => f.UserId == userId).ToList();

            
       
        }

        public List<Favoriler> GetByCategoryId(int userId, int categoryId)
        {
            List<Favoriler> favorilers = new List<Favoriler>();
           var listFavoriler =  GetAll(userId);
            foreach (var item in listFavoriler)
            {
                var product = _productService.GetById(item.ProductId);
                if (product.CategoryId == categoryId)
                {
                    favorilers.Add(item);
                }
            }
            return favorilers;
        }

        public Favoriler GetById(int favoriId)
        {
           return  _favorilerDal.Get(f => f.Id == favoriId);
        }
    }
}

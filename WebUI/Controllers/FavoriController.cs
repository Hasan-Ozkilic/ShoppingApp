using Business.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class FavoriController : Controller
    {
        IFavorilerService _favorilerService;
        IUserService _userService;
        IProductService _productService;
        public FavoriController( IFavorilerService favorilerService , IUserService userService ,IProductService productService)
        {
            _favorilerService = favorilerService;
            _userService = userService;
            _productService = productService;
        }
        public IActionResult GetAll()
        {

            if (!(Convert.ToBoolean(HttpContext.Session.GetString("Active"))))
            {
                return RedirectToAction("Login", "IO");
            }
            string tempId = HttpContext.Session.GetString("id");
           int userId =  int.Parse(tempId);
           var favoriler=  _favorilerService.GetAll(userId);

            List<FavoriGetAllModel> favoriGetAllModels = new List<FavoriGetAllModel>();
            foreach (var item in favoriler)
            {
               var product =  _productService.GetById(item.ProductId);
                FavoriGetAllModel favoriGetAllModel = new FavoriGetAllModel();
                favoriGetAllModel.Product = product;
                favoriGetAllModel.FavoriId = item.Id;
                favoriGetAllModels.Add(favoriGetAllModel);
            }
            //ViewBag.Favorite = "true";

            return View(favoriGetAllModels);
        }

        public IActionResult Add(int id)
        {

            if (!(Convert.ToBoolean(HttpContext.Session.GetString("Active"))))
            {
                return RedirectToAction("Login", "IO");
            }

            string tempId = HttpContext.Session.GetString("id");
            int userId = int.Parse(tempId);
            var userFavorites = _favorilerService.GetAll(userId);
            foreach (var item in userFavorites)
            {
                if ((item.UserId == userId) &&  (item.ProductId==id)) // aynı kullanıcı aynı ürünü 1 den fazla favoriler tablosuna ekleyemez.
                {
                    return RedirectToAction("GetAll", "Favori");
                }
            }


            _favorilerService.Add(new Entities.Favoriler() { ProductId = id , UserId= userId});
            return RedirectToAction("GetAll", "Favori");
        }
        public IActionResult Delete(int id)
        {

            if (!(Convert.ToBoolean(HttpContext.Session.GetString("Active"))))
            {
                return RedirectToAction("Login", "IO");
            }
            var result =  _favorilerService.GetById(id);
            _favorilerService.Delete(result);
          


          
            return RedirectToAction("GetAll", "Favori");
        }
        public IActionResult GetByCategoryId(int id) // category id gelir.
        {


            if (!(Convert.ToBoolean(HttpContext.Session.GetString("Active"))))
            {
                return RedirectToAction("Login", "IO");
            }

            string tempId = HttpContext.Session.GetString("id");
            int userId = int.Parse(tempId);

           var result =  _favorilerService.GetByCategoryId(userId, id);

            List<FavoriGetAllModel> favoriGetAllModels = new List<FavoriGetAllModel>();
            foreach (var item in result)
            {
                var product = _productService.GetById(item.ProductId);
                FavoriGetAllModel favoriGetAllModel = new FavoriGetAllModel();
                favoriGetAllModel.Product = product;
                favoriGetAllModel.FavoriId = item.Id;
                favoriGetAllModels.Add(favoriGetAllModel);
            }
            return View("GetAll", favoriGetAllModels);
        }
    }
}

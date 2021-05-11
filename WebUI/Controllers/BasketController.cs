using Business.Abstract;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class BasketController : Controller
    {
        IProductService _productService;
        IBasketService _basketService;
        IUserService _userService;

        public BasketController(IProductService productService, IBasketService basketService, IUserService userService)
        {
            _productService = productService;
            _basketService = basketService;
            _userService = userService;

        }
        public IActionResult Add(int id)
        {
           
            if (!(Convert.ToBoolean(HttpContext.Session.GetString("Active"))))
            {
                return RedirectToAction("Login", "IO");
            }
            //_productService.GetById
            //if (true)
            //{
            //}
            string tempId = HttpContext.Session.GetString("id");
            int userId = int.Parse(tempId);
            var basketProducts = _basketService.Baskets(userId);
            Basket basket = new Basket();
            if (basketProducts.Count==0)
            {
                basket.ProductId = id;
                basket.UserID = userId;
                _basketService.Add(basket);

                return RedirectToAction("Listele", "Basket");
            }
        

          
            foreach (var item in basketProducts)
            {
                if (item.ProductId== id  ) // 2 tane aynı ürün eklenmesi izin verilmez siparişlere.
                {
                    return RedirectToAction("Listele", "Basket");
                  
                }
            }
            basket.ProductId = id;
            basket.UserID = userId;
            _basketService.Add(basket);

            return RedirectToAction("Listele", "Basket");





        }
        public IActionResult Listele()
        {

            if (!(Convert.ToBoolean(HttpContext.Session.GetString("Active"))))
            {
                return RedirectToAction("Login", "IO");
            }
            string tempId = HttpContext.Session.GetString("id");
            int userId = int.Parse(tempId);
           var basketProducts =  _basketService.Baskets(userId);

            List<BasketListele> basketListele = new List<BasketListele>();

            foreach (var item in basketProducts)
            {

                BasketListele basketList = new BasketListele();
                var product =   _productService.GetById(item.ProductId);
                if (product==null)
                {
        var basketToDeleted = _basketService.GetById(item.Id);
                    _basketService.Delete(basketToDeleted); // eğer kullanıcın sepetindeki bir ürün daha sonra alınırsa bu ürün kullanıcının sepetinden de otomatik 
                                                            //silinmektedir.
                    continue;
                }
                int basketId = item.Id;
                basketList.Products = product;
                basketList.BasketId = basketId;
                basketListele.Add(basketList);
              

            }

            
            return View(basketListele);

        }
       public  IActionResult Delete(int id) // ilgili basket id gelir 
        {
           var basket =  _basketService.GetById(id);
            _basketService.Delete(basket);

            return RedirectToAction("Listele", "Basket");
        }
        public IActionResult Odeme()
        {

            if (!(Convert.ToBoolean(HttpContext.Session.GetString("Active"))))
            {
                return RedirectToAction("Login", "IO");
            }
            string tempId = HttpContext.Session.GetString("id");
            int userId = int.Parse(tempId);
            var basketProducts = _basketService.Baskets(userId);
            foreach (var item in basketProducts)
            {
              var productToDeleted  = _productService.GetById(item.ProductId); // tam sepette ödeme ye tıklarken ürün satılmışsa buradaki product
                // null olabilir bu durumu sonra kontrol etmelisin.
                var product = _productService.GetById(item.ProductId); // ilgili ürün bulunur. 

                int productPrice = product.ProductPrice; // ürünün fiyatı bulunur.
                int saticiId = product.UserId;
               User satici =  _userService.GetById(saticiId);
                satici.Balance += productPrice;
                _userService.Update(satici); // saticinin bakiyesi güncellenir.
               
                
                if (productToDeleted.UnitsInStock == 1)
                {
                  

                    _productService.Delete(productToDeleted); // Stokta 1 tane varsa ilgili ürün silinir.

                }
                if (productToDeleted.UnitsInStock > 1)
                { 
                    productToDeleted.UnitsInStock -= 1;
                    _productService.Update(productToDeleted);

                }

                _basketService.Delete(item); // ilgili sipariş silinir.

                
            }

            return View();
        }
    }
}

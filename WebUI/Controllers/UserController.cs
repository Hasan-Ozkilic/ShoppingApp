using Business.Abstract;
using Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

namespace WebUI.Controllers
{
   
    public class UserController : Controller
    {
        IUserService _userService;
        IProductService _productService;

        public Yardımcı yardımcı = new Yardımcı();
       
        
        public UserController(IUserService userService , IProductService productService )
        {
            _userService = userService;
            _productService = productService;
         
        }

     [HttpGet] // post veya get yap burayı çöz
        public IActionResult Index(/*LoginCheck loginCheck , string onayla*/ ) // id li user gelir.
        {
            //if (TempData["id"]==null)
            //{
            //    return RedirectToAction("Login", "IO");
            //}
          
            if (!(Convert.ToBoolean(HttpContext.Session.GetString("Active"))))
            {
                return RedirectToAction("Login", "IO");
            }
            string yardımcı2 =  HttpContext.Session.GetString("yardımcı");
            if (yardımcı2==null)
            {
                return RedirectToAction("Login", "IO");
            }
           int id =  int.Parse(yardımcı2);

         
            UserIndexModel userIndexModel = new UserIndexModel();

            userIndexModel.user = _userService.GetById(id);

            userIndexModel.products = _productService.GetByUserId(userIndexModel.user.Id);
           
            // eğer kullanıcıda hiç ürün yok ise bu kullanıcı alıcıdır. Ve bakiye 
                                                                    // görüntülemeye izin verilmez. Çünkü anlık ödeme simüle edilir.
            if (userIndexModel.products.Count == 0 && userIndexModel.user.Balance==0) // eğer en az bir ürün bile satılmış olursa balance 0 dan büyük
                                                                                      // bir sayi olacaktir. Bu durumda bu bloğa girmez.

            {
                ViewBag.isBalanceView = "no";
            }


            HttpContext.Session.SetString("id", userIndexModel.user.Id.ToString());
            HttpContext.Session.SetString("UserName", userIndexModel.user.UserName);
            


            return View(userIndexModel);
        }
     
       
        
        public IActionResult Profile()
        {
         
            string tempId = HttpContext.Session.GetString("id");
            string tempUserName = HttpContext.Session.GetString("UserName");
            string tempActive = HttpContext.Session.GetString("Active");
            bool isActive=Convert.ToBoolean(tempActive);
            //UserProfileModel userProfileModel = new UserProfileModel();
            if (isActive == false)
            {
                return RedirectToAction("Login", "IO");
            }
            if (tempId==null || tempUserName==null  )
            {
                
                return RedirectToAction("Login", "IO");
             
            }
           
           
           
            //userProfileModel.Id = Convert.ToInt32(tempId);
            //userProfileModel.UserName = tempUserName;
            return View(/*userProfileModel*/);
        }
        public IActionResult Exit()
        {
            HttpContext.Session.SetString("Active", "false");
            return RedirectToAction("Index", "Home");
        }
        public IActionResult HesapBilgilerim()
        {
           
            string tempUserName = HttpContext.Session.GetString("UserName");
         
            if (tempUserName==null)
            {
                return RedirectToAction("Login", "IO");
            }
            if (!(Convert.ToBoolean(HttpContext.Session.GetString("Active"))))
            {
                return RedirectToAction("Login", "IO");
            }
          


            User user = _userService.GetByUserName(tempUserName);
            UserHesapBilgilerimModel userHesapBilgilerimModel = new UserHesapBilgilerimModel();
            userHesapBilgilerimModel.User = user;

            return View(userHesapBilgilerimModel);
        }
        public IActionResult Bakiyem()
        {
            string tempUserName = HttpContext.Session.GetString("UserName");
            if (tempUserName == null)
            {
                return RedirectToAction("Login", "IO");
            }
            if (!(Convert.ToBoolean(HttpContext.Session.GetString("Active"))))
            {
                return RedirectToAction("Login", "IO");
            }


            string userName=HttpContext.Session.GetString("UserName");
            User user=_userService.GetByUserName(userName);
            UserBakiyemModel userBakiyemModel = new UserBakiyemModel();
            userBakiyemModel.UserInfo = user;
        
            return View(userBakiyemModel);
        }
    }

  
}

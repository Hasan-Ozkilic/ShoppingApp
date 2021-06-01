using Business.Abstract;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class ProductController : Controller
    {
      
        IProductService _productService;
        ICategoryService _categoryService;
        IUserService _userService;
        IFavorilerService _favorilerService;
     
        public ProductController(IProductService productService ,
            ICategoryService categoryService , IUserService userService , IFavorilerService favorilerService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _userService = userService;
            _favorilerService = favorilerService;
           
        }
        [HttpPost]
        public async Task<IActionResult> Add(Product product , IFormFile formFile)
        {
            if (formFile==null)
            {
                return View("ErrorMessage");

            }
            if (product.UnitsInStock == 0 )
            {
                product.UnitsInStock = 1;
            }
            if (product.Description == null)
            {
                product.Description = String.Empty;
            }
            string tempId = HttpContext.Session.GetString("id");
           product.UserId= int.Parse(tempId);

            var extension = Path.GetExtension(formFile.FileName); // .jpg , .png
            var fileName = string.Format($"{Guid.NewGuid()}{extension}");
            var path=Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img",fileName);
            product.ImageUrl = fileName;
            using (var stream = new FileStream(path,FileMode.Create))
            {
                await formFile.CopyToAsync(stream);
            }
            _productService.Add(product);
            LoginCheck loginCheck = new LoginCheck();
            loginCheck.i = product.UserId;
            User user=_userService.GetById(loginCheck.i);
            loginCheck.l = user.UserName;

            //return View("Message" , loginCheck); // post view çagırmamalı redirect to action daha öamtıklı olacaktır.
            return RedirectToAction("Message", "Product");
        }
        public IActionResult Message()
        {
            return View(); // sonra güzelleştirilebilir.
        }
        public IActionResult Telefon() // silineebilir.
        {
            int categoryId = 0;
            var result=_categoryService.GetAll();
            foreach (var item in result)
            {
                if (item.CategoryName=="Telefon")
                {
                    categoryId = item.Id;
                    break;
                }
            }
            var productList=_productService.GetByCategoryId(categoryId);
            return View(productList);
        }
      
        public IActionResult  Delete(int id) 
        {
            if (!(Convert.ToBoolean(HttpContext.Session.GetString("Active"))))
            {
                return RedirectToAction("Login", "IO");
            }

            Product productToDelete=  _productService.GetById(id);
            _productService.Delete(productToDelete);
            _favorilerService.Delete(productToDelete.Id); //favorilerden de silinir ve null hatası oluşması engellenir.
            return RedirectToAction("Index", "User");
        }
        public IActionResult Update(int id )
        {
            if (!(Convert.ToBoolean(HttpContext.Session.GetString("Active"))))
            {
                return RedirectToAction("Login", "IO");
            }
            Product productToUpdate = _productService.GetById(id);

            //ViewBag.formFile = productToUpdate.ImageUrl;

            TempData["fileName"] = productToUpdate.ImageUrl; // Hata oluşursa Http fonksiyonu kullanınız.
            TempData["userIdProvider"] = productToUpdate.UserId;

            return View(productToUpdate);
        }
        [HttpPost]
        public async Task<IActionResult> Update(Product product  , IFormFile formFile)
        {
            if (formFile ==null)
            {
                product.ImageUrl=(string)TempData["fileName"];
             
            }
            else
            {
                var extension = Path.GetExtension(formFile.FileName); // .jpg , .png
                var fileName = string.Format($"{Guid.NewGuid()}{extension}");
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", fileName);
                product.ImageUrl = fileName;
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await formFile.CopyToAsync(stream);
                }
            }


            product.UserId = (int)TempData["userIdProvider"];


            _productService.Update(product);

            return RedirectToAction("Index", "User");
        }

        public IActionResult GetAll() 
        {

            if (!(Convert.ToBoolean(HttpContext.Session.GetString("Active"))))
            {
                return RedirectToAction("Login", "IO");
            }
           
            string tempId = HttpContext.Session.GetString("id");
            int userId = int.Parse(tempId);
           var viewAllProducts= _productService.ViewAllProducts(userId); // user ın ürünleri hariç tüm ürünler getirildi.

            return View(viewAllProducts);
        }
        public IActionResult Detaylar(int id)
        {
            if (!(Convert.ToBoolean(HttpContext.Session.GetString("Active"))))
            {
                return RedirectToAction("Login", "IO");
            }
            Product productDetails = _productService.GetById(id);
            return View(productDetails);
        }
     
       
       
    }
   
}

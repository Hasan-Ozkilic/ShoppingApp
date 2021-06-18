using Business.Abstract;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace WebUI.Controllers
{
    public class CategoryController : Controller
    {
        // Dependency Injection
        private ICategoryService _categoryService;
        IProductService _productService;
        public CategoryController(ICategoryService categoryService, IProductService productService)
        {
            _categoryService = categoryService;
            _productService = productService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Bilgisayar()
        {

            if (!(Convert.ToBoolean(HttpContext.Session.GetString("Active"))))
            {
                return RedirectToAction("Login", "IO");
            }
            // bu kod satırları tekrar etmektedir. Bu kodlar ileri ki  bir zamanda refactoring edilebilir.
            string tempId = HttpContext.Session.GetString("id");
            int userId = int.Parse(tempId);
            var viewAllProducts = _productService.ViewAllProducts(userId); // user ın ürünleri hariç tüm ürünler getirildi.
           var computerProducts=  viewAllProducts.Where(p => p.CategoryId == 1).ToList();

            

            return View(computerProducts);
        }
        public IActionResult Telefon()
        {

            if (!(Convert.ToBoolean(HttpContext.Session.GetString("Active"))))
            {
                return RedirectToAction("Login", "IO");
            }
            string tempId = HttpContext.Session.GetString("id");
            int userId = int.Parse(tempId);
            var viewAllProducts = _productService.ViewAllProducts(userId); // user ın ürünleri hariç tüm ürünler getirildi.
            var phoneProducts = viewAllProducts.Where(p => p.CategoryId == 2).ToList();


            return View(phoneProducts);
        }
        public IActionResult ErkekGiyim()
        {

            if (!(Convert.ToBoolean(HttpContext.Session.GetString("Active"))))
            {
                return RedirectToAction("Login", "IO");
            }
            string tempId = HttpContext.Session.GetString("id");
            int userId = int.Parse(tempId);
            var viewAllProducts = _productService.ViewAllProducts(userId); // user ın ürünleri hariç tüm ürünler getirildi.
            var clothingProducts = viewAllProducts.Where(p => p.CategoryId == 3).ToList();


            return View(clothingProducts);
        }
        public IActionResult KadinGiyim()
        {

            if (!(Convert.ToBoolean(HttpContext.Session.GetString("Active"))))
            {
                return RedirectToAction("Login", "IO");
            }
            string tempId = HttpContext.Session.GetString("id");
            int userId = int.Parse(tempId);
            var viewAllProducts = _productService.ViewAllProducts(userId); // user ın ürünleri hariç tüm ürünler getirildi.
            var footProducts = viewAllProducts.Where(p => p.CategoryId == 4).ToList();

           
            return View(footProducts);
        }
        public IActionResult CocukGiyim()
        {

            if (!(Convert.ToBoolean(HttpContext.Session.GetString("Active"))))
            {
                return RedirectToAction("Login", "IO");
            }
            string tempId = HttpContext.Session.GetString("id");
            int userId = int.Parse(tempId);
            var viewAllProducts = _productService.ViewAllProducts(userId); // user ın ürünleri hariç tüm ürünler getirildi.
            var footProducts = viewAllProducts.Where(p => p.CategoryId == 5).ToList();


            return View(footProducts);
        }

        public IActionResult TakiVeMucevher()
        {

            if (!(Convert.ToBoolean(HttpContext.Session.GetString("Active"))))
            {
                return RedirectToAction("Login", "IO");
            }
            string tempId = HttpContext.Session.GetString("id");
            int userId = int.Parse(tempId);
            var viewAllProducts = _productService.ViewAllProducts(userId); // user ın ürünleri hariç tüm ürünler getirildi.
            var takiVeMucevherler = viewAllProducts.Where(p => p.CategoryId == 6).ToList();


            return View(takiVeMucevherler);
        }
        public IActionResult Saat()
        {

            if (!(Convert.ToBoolean(HttpContext.Session.GetString("Active"))))
            {
                return RedirectToAction("Login", "IO");
            }
            string tempId = HttpContext.Session.GetString("id");
            int userId = int.Parse(tempId);
            var viewAllProducts = _productService.ViewAllProducts(userId); // user ın ürünleri hariç tüm ürünler getirildi.
            var saatler = viewAllProducts.Where(p => p.CategoryId == 7).ToList();


            return View(saatler);
        }
        public IActionResult ErkekAyakkabisi()
        {

            if (!(Convert.ToBoolean(HttpContext.Session.GetString("Active"))))
            {
                return RedirectToAction("Login", "IO");
            }
            string tempId = HttpContext.Session.GetString("id");
            int userId = int.Parse(tempId);
            var viewAllProducts = _productService.ViewAllProducts(userId); // user ın ürünleri hariç tüm ürünler getirildi.
            var erkekAyakkabilari = viewAllProducts.Where(p => p.CategoryId == 8).ToList();


            return View(erkekAyakkabilari);
        }
        public IActionResult KadinAyakkabisi()
        {

            if (!(Convert.ToBoolean(HttpContext.Session.GetString("Active"))))
            {
                return RedirectToAction("Login", "IO");
            }
            string tempId = HttpContext.Session.GetString("id");
            int userId = int.Parse(tempId);
            var viewAllProducts = _productService.ViewAllProducts(userId); // user ın ürünleri hariç tüm ürünler getirildi.
            var kadinAyakkabilari = viewAllProducts.Where(p => p.CategoryId == 9).ToList();


            return View(kadinAyakkabilari);
        }
        public IActionResult KadinCantalari()
        {

            if (!(Convert.ToBoolean(HttpContext.Session.GetString("Active"))))
            {
                return RedirectToAction("Login", "IO");
            }
            string tempId = HttpContext.Session.GetString("id");
            int userId = int.Parse(tempId);
            var viewAllProducts = _productService.ViewAllProducts(userId); // user ın ürünleri hariç tüm ürünler getirildi.
            var cantalar = viewAllProducts.Where(p => p.CategoryId == 10).ToList();


            return View(cantalar);
        }
        public IActionResult ErkekCantalari()
        {

            if (!(Convert.ToBoolean(HttpContext.Session.GetString("Active"))))
            {
                return RedirectToAction("Login", "IO");
            }
            string tempId = HttpContext.Session.GetString("id");
            int userId = int.Parse(tempId);
            var viewAllProducts = _productService.ViewAllProducts(userId); // user ın ürünleri hariç tüm ürünler getirildi.
            var cocukCantalari = viewAllProducts.Where(p => p.CategoryId == 11).ToList();


            return View(cocukCantalari);
        }
        public IActionResult CocukCantalari() 
        {

            if (!(Convert.ToBoolean(HttpContext.Session.GetString("Active"))))
            {
                return RedirectToAction("Login", "IO");
            }
            string tempId = HttpContext.Session.GetString("id");
            int userId = int.Parse(tempId);
            var viewAllProducts = _productService.ViewAllProducts(userId); // user ın ürünleri hariç tüm ürünler getirildi.
            var cocukCantalari = viewAllProducts.Where(p => p.CategoryId == 12).ToList();


            return View(cocukCantalari);
        }
    }
}

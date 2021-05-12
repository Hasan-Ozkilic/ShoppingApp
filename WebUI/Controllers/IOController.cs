using Business.Abstract;
using Business.Concrete;
using DataAccess.Concrete.EntityFramework;
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
    public class IOController : Controller
    {
        IUserService _userService;
        // Kayıt Olma
        public IOController(IUserService userService)
        {
            _userService = userService;

        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateUser(User user ,string confirm)
        {
            foreach (var  item in _userService.GetAll())
            {
                if (item.UserName==user.UserName)
                {
                    return View("Index", user);
                    
                
                }
                if (user.Password != confirm)
                {
                    return View("Index", user);
                }

            }
            _userService.Add(user);


            LoginCheck loginCheck = new LoginCheck();
            TempData["check"] =user.UserName;
            TempData["id"]= user.Id;
          
            return RedirectToAction("Check" ,"IO");








        }

        // Giriş Yapma 
        public IActionResult Login()
        {
            LoginViewModel loginViewModel = new LoginViewModel();

            return View(loginViewModel);
        }
        [HttpPost]
        public IActionResult Login(LoginViewModel loginViewModel)
        {
            foreach (var item in _userService.GetAll())
            {
                if (item.UserName==loginViewModel.LoginModel.UserName && item.Password==loginViewModel.LoginModel.Password)
                {
                    TempData["id"] = item.Id;
                    TempData["check"] = loginViewModel.LoginModel.UserName;

                    return RedirectToAction("Check" , "IO");
                }
                if ("admin"==loginViewModel.LoginModel.UserName && loginViewModel.LoginModel.Password=="adminsifresi")
                {
                    return RedirectToAction("Index", "Admin");
                }
            }
            LoginViewModel _loginViewModel = new LoginViewModel();
            var result = _loginViewModel.Validate = true;
          


            return View("Login",_loginViewModel);

          
        }
     
        public IActionResult Check()
        {
            LoginCheck loginCheck = new LoginCheck();
            if (TempData["id"]==null)
            {
                return RedirectToAction("Login" ,"IO");
            }
            loginCheck.i = (int)TempData["id"];
            HttpContext.Session.SetString("yardımcı", loginCheck.i.ToString());
            HttpContext.Session.SetString("Active", "true");
            return RedirectToAction("Index", "User");
            //loginCheck.l= (string)TempData["check"];
            //(string loginViewModel, int id) tuple = ((string)result,(int)id);
            //return View(loginCheck);
        }
    }
}

using Business.Abstract;
using Business.Concrete;
using DataAccess.Concrete.EntityFramework;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
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
                if ("admin"==loginViewModel.LoginModel.UserName && loginViewModel.LoginModel.Password=="FlyEmirates7++++")
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

        public IActionResult PasswordReset()
        {
            return View();
        }


        [HttpPost]
        public IActionResult PasswordReset(string UserName) //PasswordReset passwordReset
        {
          User user =   _userService.GetByUserName(UserName);
            int userId = user.Id;
                
            

            #region Mail Yollama
            SmtpClient client = new SmtpClient();
            MailAddress from = new MailAddress("deneme232323232323@gmail.com", "ShoopingApp");
            string eMail = user.EMail;
            MailAddress to = new MailAddress(eMail);//bizim mail adresi
            MailMessage msg = new MailMessage(from, to);
            msg.IsBodyHtml = true;
            msg.Subject = "Şifre Sıfırlama";
            msg.Body = "Şifre sıfırlama bağlantısı : "+"https://localhost:44323/IO/PasswordVerify";


            NetworkCredential info = new NetworkCredential("deneme232323232323@gmail.com", "FlyEmirates7+1a0424c3");
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.Credentials = info;
            client.Send(msg);
            #endregion
            return RedirectToAction("Result", "IO", new { userId = userId});
        }
        public IActionResult Result(int userId)
        {


            HttpContext.Session.SetString("UserPasswordReset", userId.ToString()); 
            return View();
        }
       public IActionResult PasswordVerify()
        {
            return View();
        }

        [HttpPost]
        public IActionResult PasswordVerify(string password)
        {
            string userId = HttpContext.Session.GetString("UserPasswordReset");
            int userIdPasswordChange = int.Parse(userId);
           User user =  _userService.GetById(userIdPasswordChange);
            user.Password = password;
            _userService.Update(user);


            return RedirectToAction("Login", "IO");
        }

       
    }
}

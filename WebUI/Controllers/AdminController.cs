using Business.Abstract;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Controllers
{
    public class AdminController : Controller
    {
        IUserService _userService;
        public AdminController(IUserService userService)
        {
            _userService = userService;
        }
        public IActionResult Index()
        {

            return View();
        }
        public IActionResult List()
        {
            //  <a class="nav-link btn btn-danger text-white" asp-controller="Admin" asp-action="List">User List</a>
            var result = _userService.GetAll();

            return View(result);
        }
    }
}

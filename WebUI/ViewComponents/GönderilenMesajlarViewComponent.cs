using Business.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.ViewComponents
{
    public class GönderilenMesajlarViewComponent:ViewComponent
    {
        IMessageService _messageService;
        public GönderilenMesajlarViewComponent(IMessageService messageService)
        {
            _messageService = messageService;
        }
        public IViewComponentResult Invoke()
        {
            string tempId = HttpContext.Session.GetString("id");
            int userId = int.Parse(tempId); // bakalım olacak mı ? 
            var result = _messageService.GetByYollayanId(userId);
            var messageCount = result.Count;
            return View(messageCount);
        }

    }
}

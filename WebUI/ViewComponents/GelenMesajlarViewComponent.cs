using Business.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.ViewComponents
{
  
    public class GelenMesajlarViewComponent:ViewComponent
    {
        //IMessageService _messageService;
        //public GelenMesajlarViewComponent(IMessageService messageService)
        //{
        //    _messageService = messageService;
        //}

        IFakeMessageService _fakeMessageService;
        public GelenMesajlarViewComponent(IFakeMessageService fakeMessageService )
        {
            _fakeMessageService = fakeMessageService;
        }
        public IViewComponentResult Invoke()
        {

          

            string tempId = HttpContext.Session.GetString("id");
            int userId = int.Parse(tempId); //yollayanId
            var messages = _fakeMessageService.GetByAlanId(userId);
            int messageCount = messages.Count;
            return View(messageCount);
        }
    }
}

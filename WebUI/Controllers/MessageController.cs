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
    public class MessageController : Controller
    {
        IMessageService _messageService;
        IUserService _userService;
        IFakeMessageService _fakeMessageService;

        public MessageController(IMessageService messageService, IUserService userService, IFakeMessageService fakeMessageService)
        {
            _messageService = messageService;
            _userService = userService;
            _fakeMessageService = fakeMessageService;

        }
        public IActionResult Add(int id) // satıcı id gelir. alanId
        {
            if (!(Convert.ToBoolean(HttpContext.Session.GetString("Active"))))
            {
                return RedirectToAction("Login", "IO");
            }

            string tempId = HttpContext.Session.GetString("id");
            int userId = int.Parse(tempId); //yollayanId
            var alanKullanıcı = _userService.GetById(id);
            var yollayanKullanıcı = _userService.GetById(userId);
            MessageAddModel messageAddModel = new MessageAddModel();
            messageAddModel.AlanId = id;
            messageAddModel.YollayanId = userId;
            messageAddModel.Alan = alanKullanıcı;
            messageAddModel.Yollayan = yollayanKullanıcı;

            return View(messageAddModel);
        }
        [HttpPost]
        public IActionResult Add(int id, string message) // satıcı id gelir. alanId
        {
            if (!(Convert.ToBoolean(HttpContext.Session.GetString("Active"))))
            {
                return RedirectToAction("Login", "IO");
            }

            string tempId = HttpContext.Session.GetString("id");
            int userId = int.Parse(tempId); //yollayanId
            Message mesaj = new Message()
            {
                AlanId = id,
                YollayanId = userId,
                Mesaj = message,
                MesajTarihi = DateTime.Now
            };
            _messageService.Add(mesaj);

            FakeMessage fakeMessage = new FakeMessage() // fake mesaj veritabanına ürün eklendi
            {
                RealMessageId = mesaj.Id,
                AlanId = id,
                Mesaj = message,
                MesajTarihi = mesaj.MesajTarihi,
                YollayanId = userId
            };

            _fakeMessageService.Add(fakeMessage);


            return RedirectToAction("GetAll", "Product");
        }

        public IActionResult GetAllByAlanId()
        {
            if (!(Convert.ToBoolean(HttpContext.Session.GetString("Active"))))
            {
                return RedirectToAction("Login", "IO");
            }

            string tempId = HttpContext.Session.GetString("id");
            int userId = int.Parse(tempId); //yollayanId
            List<MessageGetAllModel> messageGetAllModels = new List<MessageGetAllModel>(); // normal mesajlasmanın ihtiyac duyulur silinebilir.
            var messages = _fakeMessageService.GetByAlanId(userId); // fakeservice tarafından çalıştırıldı.







            #region Profosyonel Mesajlasma
            // algorithm kaç farklı yollayan kullanıcı sayısını belirler.
            List<int> yollayanlarIdKumesi = new List<int>();

            foreach (var item in messages)
            {
                if (!(yollayanlarIdKumesi.Any(y => y == item.YollayanId)))
                {
                    yollayanlarIdKumesi.Add(item.YollayanId);
                }

            }

            List<TestModel> testModels = new List<TestModel>();
            

            foreach (var id in yollayanlarIdKumesi)
            {
                List<MessageGetAllModel> kullanıcıMesajlari= new List<MessageGetAllModel>();
                string userName = String.Empty;
                foreach (var mesaj in messages)
                {
                    if (mesaj.YollayanId == id)
                    {
                        MessageGetAllModel messageGetAllModel = new MessageGetAllModel();
                        var alanKullanıcı = _userService.GetById(mesaj.AlanId);
                        var yollayanKullanıcı = _userService.GetById(mesaj.YollayanId);
                        messageGetAllModel.Alan = alanKullanıcı;
                        messageGetAllModel.Yollayan = yollayanKullanıcı;
                        messageGetAllModel.AlanId = mesaj.AlanId;
                        messageGetAllModel.YollayanId = mesaj.YollayanId;
                        messageGetAllModel.Message = mesaj.Mesaj;
                        messageGetAllModel.MessageId = mesaj.Id;
                        messageGetAllModel.Time = mesaj.MesajTarihi;
                        messageGetAllModel.UserName = yollayanKullanıcı.UserName;
                        kullanıcıMesajlari.Add(messageGetAllModel);


                    }
                  
                }
                testModels.Add(new TestModel() {  MesajBox = kullanıcıMesajlari  });
            }


            // test models eleman sayısı farklı kullanıcıların sayısına baglıdır.

            #endregion












            #region Normal Mesajlasma

            // normal mesajlasma
            foreach (var item in messages)
            {
                MessageGetAllModel messageGetAllModel = new MessageGetAllModel();
                var alanKullanıcı = _userService.GetById(item.AlanId);
                var yollayanKullanıcı = _userService.GetById(item.YollayanId);
                messageGetAllModel.Alan = alanKullanıcı;
                messageGetAllModel.Yollayan = yollayanKullanıcı;
                messageGetAllModel.AlanId = item.AlanId;
                messageGetAllModel.YollayanId = item.YollayanId;
                messageGetAllModel.Message = item.Mesaj;
                messageGetAllModel.MessageId = item.Id;
                messageGetAllModel.Time = item.MesajTarihi;
                messageGetAllModels.Add(messageGetAllModel);


            }

            #endregion Normal Mesajlasma

            return View(testModels); // profosyonel mesajlasmadaki tests model kullanilir. 

        }

        public IActionResult Delete(int id) // id li mesaj gelir.  
        {
            var message = _fakeMessageService.GetById(id);

            _fakeMessageService.Delete(message);

            // mesajı alan kişi kendi fake tablosundan siler fakat gerçek veritabanındaki mesajlardan silemez.
            return RedirectToAction("GetAllByAlanId", "Message");
        }
        public IActionResult DeletePost(int id) // id li mesaj gelir.
        {
            
            var fakeMessage = _fakeMessageService.GetByMessageId(id);

            if (fakeMessage!=null)
            {
                _fakeMessageService.Delete(fakeMessage);
                var message = _messageService.GetById(id);
                _messageService.Delete(message);
            }
            if (fakeMessage==null)
            {
                var message = _messageService.GetById(id);
                _messageService.Delete(message);
            }
           

           
           
            // mesaj yollayan kullanıcı hem fake den hemde gerçek mesajlardan siler.

            return RedirectToAction("GetAllByYollayanId", "Message");
        }
        public IActionResult Update(int id) //realId gelir //_send den gelir ki mantıklı olan budur yollayan günceller
        {
            // FakeService de burası için bir fonksiyon oluşturulabilir. Parametresi realMesssageId
            // olan ve geriye sadece bir tane fake message döndüren bir fonksiyon. f=>f.Id ==realMessageId
            HttpContext.Session.SetString("realMessageId", id.ToString());

            //return RedirectToAction("GetAllByYollayanId", "Message");
            return View();
        }

        [HttpPost]
        public IActionResult Update(string message)
        {
            string realMessageId = HttpContext.Session.GetString("realMessageId");
            int messageId = int.Parse(realMessageId);
            var realMessage = _messageService.GetById(messageId);
            var fakeMessage = _fakeMessageService.GetByMessageId(messageId);
            realMessage.Mesaj = message;
            realMessage.MesajTarihi = DateTime.Now;

            _messageService.Update(realMessage);
            var realMessage2 = _messageService.GetById(messageId);

            fakeMessage.Mesaj = message;
            fakeMessage.MesajTarihi = realMessage2.MesajTarihi;
            _fakeMessageService.Update(fakeMessage);


            return RedirectToAction("GetAllByYollayanId", "Message");
        }


        public IActionResult GetAllByYollayanId()
        {
            if (!(Convert.ToBoolean(HttpContext.Session.GetString("Active"))))
            {
                return RedirectToAction("Login", "IO");
            }

            string tempId = HttpContext.Session.GetString("id");
            int userId = int.Parse(tempId); //yollayanId
            List<MessageGetAllModel> messageGetAllModels = new List<MessageGetAllModel>(); // silinebilir.
            var messages = _messageService.GetByYollayanId(userId);
            foreach (var item in messages)
            {
                MessageGetAllModel messageGetAllModel = new MessageGetAllModel();
                var alanKullanıcı = _userService.GetById(item.AlanId);
                var yollayanKullanıcı = _userService.GetById(item.YollayanId);
                messageGetAllModel.Alan = alanKullanıcı;
                messageGetAllModel.Yollayan = yollayanKullanıcı;
                messageGetAllModel.AlanId = item.AlanId;
                messageGetAllModel.YollayanId = item.YollayanId;
                messageGetAllModel.Message = item.Mesaj;
                messageGetAllModel.MessageId = item.Id;
                messageGetAllModel.Time = item.MesajTarihi;
                messageGetAllModels.Add(messageGetAllModel); // buda yapılabilir , eğer olmazsa.


            }

            #region Profosyonel Mesajlasma
            // algorithm kaç farklı yollayan kullanıcı sayısını belirler.
            List<int> yollayanlarIdKumesi = new List<int>();

            foreach (var item in messages)
            {
                if (!(yollayanlarIdKumesi.Any(y => y == item.AlanId)))
                {
                    yollayanlarIdKumesi.Add(item.AlanId); // yollayanlarda alana , alanlarda yollayan idlere bakılır, çünkü farklılık bunlarda oluşur.
                }

            }

            List<TestModel> testModels = new List<TestModel>();


            foreach (var id in yollayanlarIdKumesi)
            {
                List<MessageGetAllModel> kullanıcıMesajlari = new List<MessageGetAllModel>();
                string userName = String.Empty;
                foreach (var mesaj in messages)
                {
                    if (mesaj.AlanId == id)
                    {
                        MessageGetAllModel messageGetAllModel = new MessageGetAllModel();
                        var alanKullanıcı = _userService.GetById(mesaj.AlanId);
                        var yollayanKullanıcı = _userService.GetById(mesaj.YollayanId);
                        messageGetAllModel.Alan = alanKullanıcı;
                        messageGetAllModel.Yollayan = yollayanKullanıcı;
                        messageGetAllModel.AlanId = mesaj.AlanId;
                        messageGetAllModel.YollayanId = mesaj.YollayanId;
                        messageGetAllModel.Message = mesaj.Mesaj;
                        messageGetAllModel.MessageId = mesaj.Id;
                        messageGetAllModel.Time = mesaj.MesajTarihi;
                        messageGetAllModel.UserName = alanKullanıcı.UserName;
                        kullanıcıMesajlari.Add(messageGetAllModel);


                    }

                }
                testModels.Add(new TestModel() { MesajBox = kullanıcıMesajlari });
            }


            // test models eleman sayısı farklı kullanıcıların sayısına baglıdır.

            #endregion
         

            return View(testModels);
        }


        public IActionResult Deneme( int id)
        {
            string tempId = HttpContext.Session.GetString("id");
            int userId = int.Parse(tempId); //yollayanId
            //List<MessageGetAllModel> messageGetAllModels = new List<MessageGetAllModel>();
            var messages = _fakeMessageService.GetByAlanId(userId); // fakeservice tarafından çalıştırıldı.

            #region Profosyonel Mesajlasma
            // algorithm kaç farklı yollayan kullanıcı sayısını belirler.
            List<int> yollayanlarIdKumesi = new List<int>();

            foreach (var item in messages)
            {
                if (!(yollayanlarIdKumesi.Any(y => y == item.YollayanId)))
                {
                    yollayanlarIdKumesi.Add(item.YollayanId);
                }

            }

            List<TestModel> testModels = new List<TestModel>();


            foreach (var idd in yollayanlarIdKumesi)
            {
                List<MessageGetAllModel> kullanıcıMesajlari = new List<MessageGetAllModel>();
                string userName = String.Empty;
                foreach (var mesaj in messages)
                {
                    if (mesaj.YollayanId == idd)
                    {
                        MessageGetAllModel messageGetAllModel = new MessageGetAllModel();
                        var alanKullanıcı = _userService.GetById(mesaj.AlanId);
                        var yollayanKullanıcı = _userService.GetById(mesaj.YollayanId);
                        messageGetAllModel.Alan = alanKullanıcı;
                        messageGetAllModel.Yollayan = yollayanKullanıcı;
                        messageGetAllModel.AlanId = mesaj.AlanId;
                        messageGetAllModel.YollayanId = mesaj.YollayanId;
                        messageGetAllModel.Message = mesaj.Mesaj;
                        messageGetAllModel.MessageId = mesaj.Id;
                        messageGetAllModel.Time = mesaj.MesajTarihi;
                        messageGetAllModel.UserName = yollayanKullanıcı.UserName;
                        kullanıcıMesajlari.Add(messageGetAllModel);


                    }

                }
                testModels.Add(new TestModel() { MesajBox = kullanıcıMesajlari });
            }


            // test models eleman sayısı farklı kullanıcıların sayısına baglıdır.
            List<TestModel> tmodels = new List<TestModel>();

            foreach (var item in testModels) //testModels kaç farklı kullanıcının olduğunun tespit, eder.
            {
                foreach (var i in item.MesajBox)
                {
                    if (id == i.YollayanId)
                    {
                        tmodels.Add(item);
                        break; // istenilen kullanıcı bulundugu takdirde döngü sonlanır ve yollayan kullanıcıya ait tüm mesajlar gelir.
                    }
                }
            }

            #endregion
            return View(tmodels);
        }

        public IActionResult YollayanDeneme(int id)
        {
            string tempId = HttpContext.Session.GetString("id");
            int userId = int.Parse(tempId); //yollayanId
            //List<MessageGetAllModel> messageGetAllModels = new List<MessageGetAllModel>();
            var messages = _messageService.GetByYollayanId(userId); // message service tarafından çalıştırıldı.
            
            #region Profosyonel Mesajlasma
            // algorithm kaç farklı yollayan kullanıcı sayısını belirler.
            List<int> yollayanlarIdKumesi = new List<int>();

            foreach (var item in messages)
            {
                if (!(yollayanlarIdKumesi.Any(y => y == item.AlanId)))
                {
                    yollayanlarIdKumesi.Add(item.AlanId);
                }

            }

            List<TestModel> testModels = new List<TestModel>();


            foreach (var idd in yollayanlarIdKumesi)
            {
                List<MessageGetAllModel> kullanıcıMesajlari = new List<MessageGetAllModel>();
                string userName = String.Empty;
                foreach (var mesaj in messages)
                {
                    if (mesaj.AlanId == idd)
                    {
                        MessageGetAllModel messageGetAllModel = new MessageGetAllModel();
                        var alanKullanıcı = _userService.GetById(mesaj.AlanId);
                        var yollayanKullanıcı = _userService.GetById(mesaj.YollayanId);
                        messageGetAllModel.Alan = alanKullanıcı;
                        messageGetAllModel.Yollayan = yollayanKullanıcı;
                        messageGetAllModel.AlanId = mesaj.AlanId;
                        messageGetAllModel.YollayanId = mesaj.YollayanId;
                        messageGetAllModel.Message = mesaj.Mesaj;
                        messageGetAllModel.MessageId = mesaj.Id;
                        messageGetAllModel.Time = mesaj.MesajTarihi;
                        messageGetAllModel.UserName = alanKullanıcı.UserName;
                        kullanıcıMesajlari.Add(messageGetAllModel);


                    }

                }
                testModels.Add(new TestModel() { MesajBox = kullanıcıMesajlari });
            }


            // test models eleman sayısı farklı kullanıcıların sayısına baglıdır.
            List<TestModel> tmodels = new List<TestModel>();

            foreach (var item in testModels) //testModels kaç farklı kullanıcının olduğunun tespit, eder.
            {
                foreach (var i in item.MesajBox)
                {
                    if (id == i.AlanId)
                    {
                        tmodels.Add(item);
                        break; // istenilen kullanıcı bulundugu takdirde döngü sonlanır ve yollayan kullanıcıya ait tüm mesajlar gelir.
                    }
                }
            }

            #endregion
            return View(tmodels);

        }
    }
}

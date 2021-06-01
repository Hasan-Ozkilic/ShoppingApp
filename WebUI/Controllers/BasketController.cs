using Business.Abstract;
using Entities;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using WebUI.Models;


namespace WebUI.Controllers
{
    public class BasketController : Controller
    {
        IProductService _productService;
        IBasketService _basketService;
        IUserService _userService;
        IOrderService _orderService;
        ICardService _cardService; //araştır
        IFavorilerService _favorilerService;

        public BasketController
            (IProductService productService, IBasketService basketService,
            IUserService userService, IOrderService orderService, ICardService cardService, IFavorilerService favorilerService)
        {
            _productService = productService;
            _basketService = basketService;
            _userService = userService;
            _orderService = orderService;
            _cardService = cardService;
            _favorilerService = favorilerService;

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
            if (basketProducts.Count == 0)
            {
                basket.ProductId = id;
                basket.UserID = userId;
                _basketService.Add(basket);

                return RedirectToAction("Listele", "Basket");
            }



            foreach (var item in basketProducts)
            {
                if (item.ProductId == id) // 2 tane aynı ürün eklenmesi izin verilmez siparişlere.
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
            var basketProducts = _basketService.Baskets(userId);

            List<BasketListele> basketListele = new List<BasketListele>();

            foreach (var item in basketProducts)
            {

                BasketListele basketList = new BasketListele();
                var product = _productService.GetById(item.ProductId);
                if (product == null)
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
        public IActionResult Delete(int id) // ilgili basket id gelir 
        {
            var basket = _basketService.GetById(id);
            _basketService.Delete(basket);

            return RedirectToAction("Listele", "Basket");
        }

        public IActionResult Odeme(string firstName, string lastName, string city, string phone,
           string cardName, long cardNumber, int cvv, int expirationMonth, int expirationYear) // sipariş faturası gösterilecektir.
        {
            string orderId = HttpContext.Session.GetString("getByOrderId");

            Order order = _orderService.GetById(int.Parse(orderId));


            string tempId = HttpContext.Session.GetString("id");
            int userId = int.Parse(tempId);

            var basketProducts = _basketService.Baskets(userId);
            List<BasketListele> basketListele = new List<BasketListele>();

            foreach (var item in basketProducts)
            {

                BasketListele basketList = new BasketListele();
                var product = _productService.GetById(item.ProductId);

                int basketId = item.Id;
                basketList.Products = product;
                basketList.BasketId = basketId;
                basketListele.Add(basketList);


            }
            Card card = new Card()
            {
                CardName = cardName,
                CardNumber = cardNumber,
                Cvv = cvv,
                ExpirationMonth = expirationMonth,
                ExpirationYear = expirationYear,
                UserId = userId
            };
            BasketOrderModel basketOrderModelDeneme = new BasketOrderModel() //silinebilir. Veritabanınadan order bilgileri çekilebilir.
            {
                CardDetails = card,
                City = city,
                FirstName = firstName,
                LastName = lastName,
                Phone = phone,
                OrderList = order,
                BasketLists = basketListele
            };


            return View(basketOrderModelDeneme);

        }
       

        public IActionResult OrderDetails(string id) // tüm idler gelir. basketidler
        {

            HttpContext.Session.SetString("BasketIds", id);
            string tempId = HttpContext.Session.GetString("id");
            int userId = int.Parse(tempId);
            var basketProducts = _basketService.Baskets(userId);

            List<BasketListele> basketListele = new List<BasketListele>();

            foreach (var item in basketProducts) // silinebilir , çünkü post edildiğinde bu veriler silinmektedir.
            {

                BasketListele basketList = new BasketListele();
                var product = _productService.GetById(item.ProductId);
                if (product == null)
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
            Card userCard = _cardService.GetByUserId(userId);
            BasketOrderModel basketOrderModel = new BasketOrderModel();
            basketOrderModel.BasketLists = basketListele; // silinebilir , çünkü post edildiğinde bu veriler silinmektedir.
            if (userCard == null) //eğer kullanıcının sistemde eklenmiş kartı yok ise  bu kod parçası çalışacaktır.
            {
                basketOrderModel.CardDetails = new Card()
                {
                    CardName = "",
                    CardNumber = 0,
                    Cvv = 0,
                    ExpirationMonth = 0,
                    ExpirationYear = 0,
                    UserId = userId

                };
            }
            if (userCard != null)
            {
                basketOrderModel.CardDetails = new Card()
                {
                    CardName = userCard.CardName,
                    CardNumber = userCard.CardNumber,
                    Cvv = userCard.Cvv,
                    ExpirationMonth = userCard.ExpirationMonth,
                    ExpirationYear = userCard.ExpirationYear,
                    UserId = userCard.UserId

                };
            }

            basketOrderModel.City = "";
            basketOrderModel.FirstName = "";
            basketOrderModel.LastName = "";
            basketOrderModel.OrderList = new Order() { Address = "", UserId = 0, EMail = "", BasketIds = id, };
            //basketOrderModel.Phone = 0;


            return View(basketOrderModel);

        }
        [HttpPost]
        [Obsolete]
        public IActionResult OrderDetails(BasketOrderModel basketOrderModel)
        {

            string tempId = HttpContext.Session.GetString("id");
            int userId = int.Parse(tempId);

            var basketProducts = _basketService.Baskets(userId);
            List<BasketListele> basketListele = new List<BasketListele>();

            foreach (var item in basketProducts)
            {

                BasketListele basketList = new BasketListele();
                var product = _productService.GetById(item.ProductId);
                if (product == null)
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
            basketOrderModel.BasketLists = basketListele; //silinebilir.
            basketOrderModel.OrderList.BasketIds = HttpContext.Session.GetString("BasketIds");
            basketOrderModel.OrderList.OrderTime = DateTime.Now;
            basketOrderModel.OrderList.UserId = userId;
            basketOrderModel.CardDetails.UserId = userId;

            Order order = new Order() //aynı kullanıcıya ait birden fazla kayıt veritabanında tutulur . Bu daha sonra düzenlenebilir.
            {
                Address = basketOrderModel.OrderList.Address,
                BasketIds = basketOrderModel.OrderList.BasketIds,
                EMail = basketOrderModel.OrderList.EMail,
                OrderTime = basketOrderModel.OrderList.OrderTime,
                UserId = basketOrderModel.OrderList.UserId
            };
            _orderService.Add(order);
            int orderId = order.Id; // o anki order ıd alınır . sipariş faturasına eklenmesi için
            string orderIdNow = orderId.ToString();

            HttpContext.Session.SetString("getByOrderId", orderIdNow);


            Card card = new Card()
            {
                CardName = basketOrderModel.CardDetails.CardName,
                CardNumber = basketOrderModel.CardDetails.CardNumber,
                Cvv = basketOrderModel.CardDetails.Cvv,
                ExpirationMonth = basketOrderModel.CardDetails.ExpirationMonth,
                ExpirationYear = basketOrderModel.CardDetails.ExpirationYear,
                UserId = userId
            };
            Card userCard = _cardService.GetByUserId(userId);
            if (userCard == null) // ilk kez kayıt yapan kullanıcı veritabanına eklenir. Çünkü 1 kullanıcının sistemde sadece 1 kayıtı olacaktır.
            {
                _cardService.Add(card);
            }
            // eğer kullanıcı kendi kartı dışında bir kart kullanır ise bu kart sisteme eklenmez. Daha sonra yapılacak olan 
            //tüm alışverişlerde sistemde ilk kayıt yapılmış olan kart kullanıcıya gösterilecektir.
            // Daha sonra istenirse kaydedilmiş olan kartı güncelleyebilir veya silebilir.



            //BasketOrderModel basketOrderModelDeneme = new BasketOrderModel() //silinebilir. Veritabanınadan order bilgileri çekilebilir.
            //{
            //    CardDetails = card,
            //    City = basketOrderModel.City,
            //    FirstName = basketOrderModel.FirstName,
            //    LastName = basketOrderModel.LastName,
            //    Phone = basketOrderModel.Phone,
            //    OrderList = order,
            //    BasketLists = basketOrderModel.BasketLists
            //};
            #region Mail Yollama
            SmtpClient client = new SmtpClient();
            MailAddress from = new MailAddress("deneme232323232323@gmail.com", "ShoopingApp");
            MailAddress to = new MailAddress("xkanal306@gmail.com");//bizim mail adresi
            MailMessage msg = new MailMessage(from, to);
            msg.IsBodyHtml = true;
            msg.Subject = "Alışveriş Faturası";
            #region StringBuilder ile Mail düzenleme


            StringBuilder builder = new StringBuilder();
            builder.Append("<head>" );
            builder.Append(" <link rel=\"stylesheet\"  href=\"https://cdn.jsdelivr.net/npm/bootstrap@4.6.0/dist/css/bootstrap.min.css \" " +" "+ "integrity=\"sha384-B0vP5xmATw1+K9KRQjQERJvTumQW0nPEzvF6L/Z6nronJ3oUOFUFpCjEUQouq2+l\" "+" "+ "crossorigin=\"anonymous\" />");
            builder.Append("</head>");
            builder.Append("<body>");
           
            builder.Append("<h4>Sayın " + basketOrderModel.FirstName+" "+ basketOrderModel.LastName+" Sipariş Faturanız</h4>");
            builder.Append("<table class=\"table table-striped\">");
            builder.Append("  <tr>");
            builder.Append("<th>Ürün Resmi </th><th>Ürün İsmi </th><th>Ürün Fiyatı</th><th>Açıklama </th> ");
            builder.Append("</tr>");
            foreach (var item in basketListele)
            {
                builder.Append("  <tr>");
                string imagePath = "wwwroot\\"+"img\\"+item.Products.ImageUrl;
          
                //var imageFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, imagePath);
                 builder.Append("<td>  <img src="+"\""+imagePath +"\""+" " +  "width=\"140\" height=\"140\" /> </td><td>"+item.Products.ProductName+ "</td><td>"+item.Products.ProductPrice+"</td><td>"+item.Products.Description+" </td> ");
              
                builder.Append("</tr>");

        


            }
           


            builder.Append("</table>");
            builder.Append("<div style=\" text-align:center;margin-top:40px; font-family: Arial, Helvetica, sans - serif; \">");

            builder.Append("<ul>");
            builder.Append("<li>");
            builder.Append(" Telefon Numaranız :  "+basketOrderModel.Phone+ " </ul>");


            builder.Append("</li>");
            builder.Append("<li>");

            builder.Append(" Şehriniz :  " + basketOrderModel.City + " </ul>");
            builder.Append("</li>");



            builder.Append("</ul>");
            builder.Append("<pre>");
            builder.Append("<br><br><h4 style=\"color:red;\">-- Kart Bilgileriniz --</h4>"+ "<br>");
            builder.Append(" Kart İsminiz : "  + basketOrderModel.CardDetails.CardName +"<br>" );
            builder.Append(" Kart Numaranız : " + basketOrderModel.CardDetails.CardNumber+"<br>");
            builder.Append(" Son kullanma tarihi : " + basketOrderModel.CardDetails.ExpirationMonth +"\\"+basketOrderModel.CardDetails.ExpirationYear + "<br>");
            int toplam = 0;
            foreach (var item in basketListele)
            {
               toplam+= item.Products.ProductPrice;
            }
            builder.Append("<br><br> Sipariş Tutarı : " + toplam + "<br>");
            builder.Append(" Sipariş Adresiniz : " + basketOrderModel.OrderList.Address + "<br>");
            builder.Append(" E-Mail Adresiniz : " + basketOrderModel.OrderList.EMail + "<br>");
            builder.Append("</pre>");

            builder.Append("</div>");
            builder.Append("</body>");

            #region Pdf 
            StringReader sr = new StringReader(builder.ToString());

            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);

            #region Türkçe karakter sorunu için yazılması gereken kod bloğu.
            /*  FontFactory.Register(Path.Combine("C:\\Windows\\Fonts\\Arial.ttf"), "Garamond"); */// kendi türkçe karakter desteği olan fontunuzu da girebilirsiniz.
            StyleSheet css = new StyleSheet();
            css.LoadTagStyle("body", "face", "Garamond");
            css.LoadTagStyle("body", "encoding", "Identity-H");
            css.LoadTagStyle("body", "size", "12pt");
            htmlparser.SetStyleSheet(css);
            #endregion

            using (MemoryStream memoryStream = new MemoryStream())
            {
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
               
                pdfDoc.Open();
                htmlparser.Parse(sr);
                pdfDoc.Close();
                byte[] bytes = memoryStream.ToArray();
                memoryStream.Close();




                // normal text halinde yollar ama biz pdf yolluyoruz ve buna gerek kalmaz.
               // msg.Body += "Bizleri tercih ettiğiniz için teşekkür ederiz" + builder; //burada başında gönderen kişinin mail adresi geliyor
                                                                                       //msg.CC.Add(from);//herkes görür
                msg.Attachments.Add(new Attachment(new MemoryStream(bytes), "SiparisFaturasi.pdf"));
             //   msg.Attachments.Add(new Attachment("images/hp.jpg"));

                NetworkCredential info = new NetworkCredential("deneme232323232323@gmail.com", "sifreniz");
                client.Port = 587;
                client.Host = "smtp.gmail.com";
                client.EnableSsl = true;
                client.Credentials = info;
                client.Send(msg);
            }

            #endregion

            #endregion
            #endregion



            #region Odeme 

            // Odeme actiondan buraya taşınmıştır. Çünkü  Post methotda olması daha uygun olacaktır.

            //foreach (var item in basketProducts)
            //{
            //    var productToDeleted = _productService.GetById(item.ProductId); // tam sepette ödeme ye tıklarken ürün satılmışsa buradaki product
            //    // null olabilir bu durumu sonra kontrol etmelisin.
            //    var product = _productService.GetById(item.ProductId); // ilgili ürün bulunur. 

            //    int productPrice = product.ProductPrice; // ürünün fiyatı bulunur.
            //    int saticiId = product.UserId;
            //    User satici = _userService.GetById(saticiId);
            //    satici.Balance += productPrice;
            //    _userService.Update(satici); // saticinin bakiyesi güncellenir.


            //    if (productToDeleted.UnitsInStock == 1)
            //    {


            //        _productService.Delete(productToDeleted); // Stokta 1 tane varsa ilgili ürün silinir.
            //        _favorilerService.Delete(item.ProductId); // favorilerden de silinir.

            //    }
            //    if (productToDeleted.UnitsInStock > 1)
            //    {
            //        productToDeleted.UnitsInStock -= 1;
            //        _productService.Update(productToDeleted);

            //    }

            //    _basketService.Delete(item); // ilgili sipariş silinir.


            //}
            #endregion


            // return View(basketOrderModelDeneme); //sipariş faturası // ödeme kısmında productlar veritabanından silinir. 
            // ama kullanıcı siparişten vazgeçebilir. fake den silinse daha güzel olur . Araştır bunu
            //kullanıcı sipariş verdikten sonra iptal edemez bu yüzden fake e ihtiyeç yoktur. Bu durum Daha sonra düzenlenebilir.
            return RedirectToAction("Odeme", "Basket", new
            {
                city = basketOrderModel.City,
                firstName = basketOrderModel.FirstName,
                lastName = basketOrderModel.LastName,
                phone = basketOrderModel.Phone,
                cardName = basketOrderModel.CardDetails.CardName,
                cardNumber = basketOrderModel.CardDetails.CardNumber,
                cvv = basketOrderModel.CardDetails.Cvv,
                expirationMonth = basketOrderModel.CardDetails.ExpirationMonth,
                expirationYear = basketOrderModel.CardDetails.ExpirationYear

            });
        }


        [NonAction]
        public List<int> GetBasketIds(string id)
        {
            List<int> basketId = new List<int>(); // kullanıcının tüm basketId numaraları buraya eklenir.
            string[] basketIds = id.Split(',');
            for (int i = 0; i < basketIds.Length; i++)
            {
                if (i == basketIds.Length - 1)
                {
                    break;
                }
                basketId.Add(int.Parse(basketIds[i]));
            }
            return basketId;
        }

    }
}

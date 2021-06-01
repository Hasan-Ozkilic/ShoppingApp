using Business.Abstract;
using Business.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddSingleton<IProductService, ProductManager>();
            services.AddSingleton<IProductDal, EfProductDal>();

            services.AddSingleton<ICategoryService, CategoryManager>();
            services.AddSingleton<ICategoryDal, EfCategoryDal>();

            services.AddSingleton<IUserService, UserManager>();
            services.AddSingleton<IUserDal, EfUserDal>();
            services.AddSingleton<IBasketService, BasketManager>();
            services.AddSingleton<IBasketDal, EfBasketDal>();

            services.AddSingleton<IFavorilerService, FavorilerManager>();
            services.AddSingleton<IFavorilerDal, EfFavorilerDal>();

            services.AddSingleton<IMessageService, MessageManager>();
            services.AddSingleton<IMessageDal, EfMessageDal>();


            //silinebilir ins silinmez
            services.AddSingleton<IFakeMessageService, FakeMessageManager>();
            services.AddSingleton<IFakeMessageDal, EfFakeMessageDal>();

            services.AddSingleton<IOrderService,OrderManager>();
            services.AddSingleton<IOrderDal, EfOrderDal>();

            services.AddSingleton<ICardService, CardManager>();
            services.AddSingleton<ICardDal, EfCardDal>();







            services.AddSession();
      

           

        }

        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseSession();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            
         
            app.UseEndpoints(endpoints =>
            {
                
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

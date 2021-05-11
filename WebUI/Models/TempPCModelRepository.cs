using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Models
{
    public static class TempPCModelRepository
    {
        public static List<TempPCModel> tempPCModels = new List<TempPCModel>()
       {
           new TempPCModel()
           {
                LaptopName="HP Laptop" , ImageUrl="hp.jpg" , LaptopPrice =5000,

           },
           new TempPCModel()
           {
                LaptopName="Asus Laptop" , ImageUrl="asus.jpg" , LaptopPrice =6000,

           },
           new TempPCModel()
           {
                LaptopName="Acer Laptop" , ImageUrl="acer.jpg" , LaptopPrice =8000,

           }

       };
       
    }
}

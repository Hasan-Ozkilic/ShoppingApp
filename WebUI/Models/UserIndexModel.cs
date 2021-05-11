using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Models
{
    public class UserIndexModel
    {
        public User user { get; set; }
      public  List<Product> products { get; set; }

    }
}

using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Models
{
    public class UserProfileModel
    {
        public Product ProductInfo { get; set; }
        public int Id { get; set; }
        public string UserName { get; set; }
        public bool Active { get; set; }
    }
}

using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Models
{
    public class UserHesapBilgilerimModel
    {
        public User User { get; set; }
        public bool Active { get; set; }
        public Card CardInfos { get; set; }
    }
}

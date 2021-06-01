using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Models
{
    public class BasketOrderModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Order OrderList { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public Card CardDetails { get; set; }
        public List<BasketListele> BasketLists { get; set; }
    }
}

using Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class Order:IEntity
    {
        public int Id { get; set; }
       
        public string BasketIds { get; set; }
        public int UserId { get; set; }
        public string EMail { get; set; }
        public string Address { get; set; }
        public DateTime OrderTime { get; set; }
        

    }
}

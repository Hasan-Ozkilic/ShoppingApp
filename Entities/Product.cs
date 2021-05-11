using Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class Product:IEntity
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int UserId { get; set; }
        public string ProductName { get; set; } 
        public string ImageUrl { get; set; }
        public int ProductPrice { get; set; }
        public string Description { get; set; }
        public int UserOrderId { get; set; } // şimdilik kalsın ama sonradan silinmesi gerek.....
        public int UnitsInStock { get; set; }
    }
}

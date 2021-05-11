using Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class ProductDetailDto:IDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int ProductPrice { get; set; }
        public string CategoryName { get; set; }

    }
}

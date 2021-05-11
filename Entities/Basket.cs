using Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class Basket:IEntity
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int UserID { get; set; }
    }
}

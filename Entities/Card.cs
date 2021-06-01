using Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
   public  class Card:IEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string CardName { get; set; }
        public long CardNumber { get; set; }
        public int ExpirationMonth { get; set; }
        public int ExpirationYear { get; set; }
        public int Cvv { get; set; }
    }
}

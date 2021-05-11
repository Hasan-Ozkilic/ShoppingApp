using Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class Message:IEntity
    {
        public long Id { get; set; }
        public int YollayanId { get; set; }

        public int AlanId { get; set; }
        public DateTime MesajTarihi { get; set; }
        public string Mesaj { get; set; }
    }
}

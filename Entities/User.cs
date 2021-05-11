using Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class User:IEntity
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string EMail { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public long Balance { get; set; }
    }
}

using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Models
{
    public class MessageGetAllModel
    {
        public long MessageId { get; set; }
        public int AlanId { get; set; }
        public int YollayanId { get; set; }
        public User Alan { get; set; }
        public User Yollayan { get; set; }
        public string Message { get; set; }
        public DateTime Time { get; set; }
        public string UserName { get; set; }

    }
}

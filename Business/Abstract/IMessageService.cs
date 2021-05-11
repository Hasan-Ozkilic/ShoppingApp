using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IMessageService
    {
        void Add(Message message);
        void Delete(Message message);
        void Update(Message message);
        List<Message> GetByAlanId(int alanId);
        Message GetById(int messageId);
        List<Message> GetAll();
        List<Message> GetByYollayanId(int yollayanId);
  
    }
}

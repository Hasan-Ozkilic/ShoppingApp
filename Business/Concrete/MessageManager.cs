using Business.Abstract;
using DataAccess.Abstract;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class  MessageManager:IMessageService
    {
        IMessageDal _messageDal;
        public MessageManager(IMessageDal messageDal)
        {
            _messageDal = messageDal;
        }

        public void Add(Message message)
        {
            _messageDal.Add(message);
        }

        public void Delete(Message message)
        {
            _messageDal.Delete(message);
        }

        public List<Message> GetAll()
        {
           return _messageDal.GetAll();
        }

        public List<Message> GetByAlanId(int alanId)
        {
            return _messageDal.GetAll(m => m.AlanId == alanId);
        }

       

        public Message GetById(int messageId)
        {
         return   _messageDal.Get(m => m.Id == messageId);
        }

        public List<Message> GetByYollayanId(int yollayanId)
        {
            return _messageDal.GetAll(m => m.YollayanId == yollayanId);
        }

        public void Update(Message message)
        {
            _messageDal.Update(message);
        }
    }
}

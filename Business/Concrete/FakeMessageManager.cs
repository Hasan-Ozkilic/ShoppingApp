using Business.Abstract;
using DataAccess.Abstract;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class FakeMessageManager :IFakeMessageService
    {
        IFakeMessageDal _fakeMessageDal;
        public FakeMessageManager(IFakeMessageDal fakeMessageDal)
        {
            _fakeMessageDal = fakeMessageDal;
        }

        public void Add(FakeMessage message)
        {

            _fakeMessageDal.Add(message);
        }

        public void Delete(FakeMessage message)
        {
            _fakeMessageDal.Delete(message);
        }

        public List<FakeMessage> GetAll()
        {
           return  _fakeMessageDal.GetAll();
        }

        public List<FakeMessage> GetByAlanId(int alanId)
        {
          return   _fakeMessageDal.GetAll(f => f.AlanId == alanId);
        }

        public FakeMessage GetById(long id)
        {
            return _fakeMessageDal.Get(f => f.Id == id);
        }

        public FakeMessage GetByMessageId(long realMessageID)
        {
            return _fakeMessageDal.Get(f => f.RealMessageId == realMessageID);
        }

        public void Update(FakeMessage message)
        {
            _fakeMessageDal.Update(message);
        }
    }
}

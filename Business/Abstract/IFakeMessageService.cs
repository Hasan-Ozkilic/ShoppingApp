using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IFakeMessageService
    {
        void Add(FakeMessage message);
        void Delete(FakeMessage message);
        void Update(FakeMessage message);
        FakeMessage GetById(long id);
        FakeMessage GetByMessageId(long realMessageID);
        List<FakeMessage> GetAll();
        List<FakeMessage> GetByAlanId(int alanId);
    }
}

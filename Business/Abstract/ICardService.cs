using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface ICardService
    {
        void Add(Card card);
        void Delete(Card card);
        Card GetByUserId(int userId);
        Card GeyById(int id);
        void Update(Card card);
    }
}

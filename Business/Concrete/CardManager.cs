using Business.Abstract;
using DataAccess.Abstract;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
   public class CardManager:ICardService
    {
        private ICardDal _cardDal;
        public CardManager(ICardDal cardDal)
        {
            _cardDal = cardDal;
        }

        public void Add(Card card)
        {
            _cardDal.Add(card);
        }

        public void Delete(Card card)
        {
            _cardDal.Delete(card);
        }

        public Card GetByUserId(int userId)
        {
            return _cardDal.Get(c => c.UserId == userId);
        }

        public Card GeyById(int id)
        {
            return _cardDal.Get(c => c.Id == id);
        }

        public void Update(Card card)
        {
            _cardDal.Update(card);
        }
    }
}

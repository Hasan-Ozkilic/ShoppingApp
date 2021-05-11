using Business.Abstract;
using DataAccess.Abstract;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class UserManager : IUserService
    {
        IUserDal _userDal;
        public UserManager(IUserDal userDal)
        {
            _userDal = userDal;
        }
        public void Add(User entity)
        {
            // Validate         
                _userDal.Add(entity);          
        }

        public void Delete(User entity)
        {
            _userDal.Delete(entity);
        }

        public List<User> GetAll()
        {
            return _userDal.GetAll();
        }

        public User GetById(int id)
        {
            return _userDal.Get(u => u.Id == id);
        }

        public User GetByUserName(string userName)
        {
            return _userDal.Get(u => u.UserName == userName);
        }

        public void Update(User entity)
        {
            _userDal.Update(entity);
        }
        
       
     
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.Context;
using BLL.Models.Game;

namespace BLL.Repository
{
    public class UserRepository : IUserRepository
    {
        private StoryContext _context;

        public UserRepository(StoryContext context)
        {
            _context = context;
        }

        public User FindUser(UInt64 id)
        {
            var user = (from u in _context.Users
                       where u.Facebook_id == id
                       select u).FirstOrDefault();
            return user;
        }

        public User CreateUser(UInt64 fb_id, string name)
        {
            var user = new User { Facebook_id = fb_id , Name = name, Stories = null};

            user = _context.Users.Add(user).Entity;
            _context.SaveChanges();
            return user;
        }
    }
}

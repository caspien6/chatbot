using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Context;
using BLL.Models.Game;
using BLL.StateMachine;
using Microsoft.EntityFrameworkCore;

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
                        .Include(u=>u.Stories)
                       where u.Facebook_id == id
                       select u).FirstOrDefault();
            return user;
        }

        public User UpdateUser(User u)
        {
            var user = _context.Users.Update(u).Entity;
            _context.SaveChanges();
            return user;
        }

        public void DeleteUser(UInt64 id)
        {
            var user = (from u in _context.Users
                        where u.Facebook_id == id
                        select u).FirstOrDefault();
            _context.Remove(user);
            _context.SaveChanges();
        }

        public User CreateUser(UInt64 fb_id, string name)
        {
            var user = new User { Facebook_id = fb_id , Name = name, Stories = null, SavedState = MainMenuStateMachine.State.MainMenu};
            CreateEngineForUser(user);
            user = _context.Users.Add(user).Entity;
            _context.SaveChanges();
            return user;
        }

        private void CreateEngineForUser(User user)
        {
           

        }
    }
}

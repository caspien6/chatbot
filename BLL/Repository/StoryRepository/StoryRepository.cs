using BLL.Context;
using BLL.Models.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Repository.StoryRepository
{
    public class StoryRepository : IStoryRepository
    {
        private StoryContext _context;

        public StoryRepository(StoryContext context)
        {
            _context = context;
        }

        public ICollection<StoryPool> FindAllStory()
        {
            return _context.StoryPools.ToList();
        }

        public Story CreateChosenStory(User _user, int index)
        {
            var user = _context.Users.Where(u => u.Id == _user.Id).FirstOrDefault();
            if (user != null)
            {

                var storedStory = _context.StoryPools.ToList().ElementAt(index);
                var character = new Character { Items = new List<Item> { new Item { Name = "Axe" } }, Health = 200, Mana = 100 };
                var storyProcedureState = new StoryProcedureState { Station = storedStory.StarterStation, Character = character };
                var newStory = new Story { Title = storedStory.Title, State = storyProcedureState, IsActive = true };

                foreach (var item in user.Stories.ToList())
                {
                    item.IsActive = false;
                }

                user.Stories.Add(newStory);


                _context.Stories.Add(newStory);
                _context.SaveChanges();
                return newStory;
            }
            return null;
            

        }

        
    }
}

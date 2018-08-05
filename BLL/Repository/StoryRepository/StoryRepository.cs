using BLL.Context;
using BLL.Models.Game;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Repository.StoryRepository
{
    public enum AttributeType
    {
        Strength,
        Agility,
        Intelligence
    }

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

        public void SetName(User us, string name)
        {
            var story = PickSelectedStory(us);
            story.State.Character.Name = name;
            _context.Update(story);
            _context.SaveChanges();

        }
        

        public void SetPrimaryAttribute(User us, AttributeType attribType)
        {
            var story = PickSelectedStory(us);
            switch (attribType)
            {
                case AttributeType.Strength:
                    story.State.Character.Health *= 4;
                    break;
                case AttributeType.Agility:
                    story.State.Character.Health *= 4;
                    story.State.Character.Mana *= 2;
                    break;
                case AttributeType.Intelligence:
                    story.State.Character.Mana *= 4;
                    break;
                default:
                    throw new ApplicationException("Error while choosing primary attribute!");
            }

            _context.Update(story);
            _context.SaveChanges();

        }


        private Story PickSelectedStory(User us)
        {
            var user = (from s in _context.Users
                        .Include(u => u.Stories)
                        where s.Facebook_id == us.Facebook_id
                        select s).FirstOrDefault();

            var queryStoryId = us.Stories.Where(s => s.IsActive).Select(s => s.Id).FirstOrDefault();

            var selectedStory = (from s in _context.Stories
                                .Include(s => s.State)
                                .ThenInclude(sp => sp.Character)
                                 where s.Id == queryStoryId
                                 select s).FirstOrDefault();
            return selectedStory;
        }

    }
}

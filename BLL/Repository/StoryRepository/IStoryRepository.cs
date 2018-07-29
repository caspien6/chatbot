using BLL.Models.Game;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Repository.StoryRepository
{
    public interface IStoryRepository
    {
        ICollection<StoryPool> FindAllStory();
        Story CreateChosenStory(User _user, int index);

    }
}

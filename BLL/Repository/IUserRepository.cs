using BLL.Models.Game;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Repository
{
    public interface IUserRepository
    {
        User FindUser(UInt64 id);
        User CreateUser(UInt64 fb_id, string name);

    }
}

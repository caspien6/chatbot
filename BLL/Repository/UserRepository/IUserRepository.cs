using BLL.Models.Game;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Repository
{
    public interface IUserRepository
    {
        User FindUser(UInt64 id);
        User CreateUser(UInt64 fb_id, string name);
        void DeleteUser(UInt64 id);

    }
}

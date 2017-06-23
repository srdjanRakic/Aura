using System.Collections.Generic;
using Aura.Core.Entities;
using Aura.Core.Models;

namespace Aura.Core.Interfaces
{
    public interface IUser
    {
        List<User> GetUsers();

        User GetUserById(int id);

        User CreateUser(UserCreateDto user);

        User UpdateUser(int id, UserUpdateDto user);

        void DeleteUser(int id);
    }
}

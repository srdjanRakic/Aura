using System;
using System.Collections.Generic;
using Aura.Core.Entities;
using Aura.Core.Interfaces;
using System.Linq;
using Aura.Core.Models;

namespace Aura.Core.Services
{
    public class UserService : IUser
    {
        private readonly IRepository<User> _userRepository;

        public UserService(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public List<User> GetUsers()
        {
            return _userRepository.All().Where(u => u.DeletedOn == null).ToList();
        }

        public User GetUserById(int id)
        {
            return _userRepository.GetById(id);
        }

        public User CreateUser(UserCreateDto userDto)
        {
             User user = new User
             {
                 Guid = new Guid(),
                 CreatedOn = DateTime.UtcNow,
                 Email = userDto.Email,
                 Name = userDto.Name
             };

            _userRepository.Add(user);

            return user;
        }

        public User UpdateUser(int id, UserUpdateDto userDto)
        {
            User user = _userRepository.All().FirstOrDefault(u => u.Id == id && u.DeletedOn == null);

            if (user != null)
            {
                user.Name = userDto.Name;
                user.Email = userDto.Email;
            }

            return user;
        }

        public void DeleteUser(int id)
        {
            User user = _userRepository.All().FirstOrDefault(u => u.Id == id && u.DeletedOn == null);

            if (user != null)
            {
                user.DeletedOn = DateTime.UtcNow;
            }
            
            _userRepository.SaveChanges();
        }
    }
}
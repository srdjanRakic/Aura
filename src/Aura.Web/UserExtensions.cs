using System;
using System.Collections.Generic;
using System.Linq;
using Aura.Core.Entities;
using Aura.Infrastructure.Data;

namespace Aura.Web
{
    public static class UserExtensions
    {
        public static void EnsureSeedDataForContext(this AppDbContext context)
        {
            if (context.Users.Any())
            {
                return;
            }

            // init seed data
            var users = new List<User>()
            {
                new User()
                {
                    Guid = Guid.NewGuid(),
                    Name = "Srdjan Rakic",
                    Email = "srdjan_srdjan@hotmail.com",
                    CreatedOn = DateTime.UtcNow
                },
                new User()
                {
                    Guid = Guid.NewGuid(),
                    Name = "Stefan Stanoeski",
                    Email = "stefan.stefan@yahoo.com",
                    CreatedOn = DateTime.UtcNow
                },
                new User()
                {
                    Guid = Guid.NewGuid(),
                    Name = "Marko Nikolov",
                    Email = "marko.stefan@gmail.com",
                    CreatedOn = DateTime.UtcNow
                },
            };

            context.Users.AddRange(users);
            context.SaveChanges();
        }
    }
}

using System;
using System.Linq;
using Aura.Core.Entities;
using Aura.Core.Interfaces;
using Aura.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace Aura.Tests.Integration.Data
{
    public class EfRepositoryAddShould
    {
        private AppDbContext _dbContext;

        private static DbContextOptions<AppDbContext> CreateNewContextOptions()
        {
            // Create a fresh service provider, and therefore a fresh 
            // InMemory database instance.
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            // Create a new options instance telling the context to use an
            // InMemory database and the new service provider.
            var builder = new DbContextOptionsBuilder<AppDbContext>();
            builder.UseInMemoryDatabase()
                .UseInternalServiceProvider(serviceProvider);

            return builder.Options;
        }

        [Fact]
        public void AddItemAndSetId()
        {
            var repository = GetRepository();
            var item = new User();

            repository.Add(item);

            var newItem = repository.All().FirstOrDefault();

            Assert.Equal(item, newItem);
            Assert.True(newItem.Id > 0);
        }

        [Fact]
        public void UpdateItemAfterAddingIt()
        {
            // add an item
            var repository = GetRepository();
            var initialTitle = Guid.NewGuid().ToString();
            var item = new User
            {
                Name = initialTitle
            };
            repository.Add(item);

            // detach the item so we get a different instance
            _dbContext.Entry(item).State = EntityState.Detached;

            // fetch the item and update its title
            var newItem = repository.All()
                .FirstOrDefault(i => i.Name == initialTitle);
            Assert.NotSame(item, newItem);
            var newTitle = Guid.NewGuid().ToString();
            newItem.Name = newTitle;

            // Update the item
            repository.Update(newItem);
            var updatedItem = repository.All()
                .FirstOrDefault(i => i.Name == newTitle);

            Assert.NotEqual(item.Name, updatedItem.Name);
            Assert.Equal(newItem.Id, updatedItem.Id);
        }

        [Fact]
        public void DeleteItemAfterAddingIt()
        {
            // add an item
            var repository = GetRepository();
            var initialTitle = Guid.NewGuid().ToString();
            var item = new User
            {
                Name = initialTitle
            };
            repository.Add(item);

            // delete the item
            repository.Delete(item);

            // verify it's no longer there
            Assert.DoesNotContain(repository.All(),
                i => i.Name == initialTitle);
        }


        private EfRepository<User> GetRepository()
        {
            var options = CreateNewContextOptions();
            var mockDispatcher = new Mock<IDomainEventDispatcher>();

            _dbContext = new AppDbContext(options, mockDispatcher.Object);
            return new EfRepository<User>(_dbContext);
        }
    }
}
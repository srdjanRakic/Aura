using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Aura.Core.Entities;
using Aura.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using Xunit;

namespace Aura.Tests.Integration.Web
{
    public class ApiToDoItemsControllerListShould
    {
        private readonly HttpClient _client;

        public ApiToDoItemsControllerListShould()
        {
            _client = GetClient();
        }

        protected HttpClient GetClient()
        {
            var builder = new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .UseEnvironment("Testing"); // ensure ConfigureTesting is called in Startup

            var server = new TestServer(builder);
            var client = server.CreateClient();

            // client always expects json results
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }


        [Fact]
        public async Task ReturnTwoItems()
        {
            var response = await _client.GetAsync("/api/todoitems");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<IEnumerable<User>>(stringResponse).ToList();

            Assert.Equal(2, result.Count());
            Assert.Equal(1, result.Count(a => a.Name == "Test Item 1"));
            Assert.Equal(1, result.Count(a => a.Name == "Test Item 2"));
        }
    }
}
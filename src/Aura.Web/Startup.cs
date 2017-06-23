using System;
using Aura.Core.Entities;
using Aura.Core.Interfaces;
using Aura.Core.Services;
using Aura.Core.SharedKernel;
using Aura.Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using StructureMap;
using Swashbuckle.AspNetCore.Swagger;

namespace Aura.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Add framework services.

            // TODO: Add DbContext and IOC
            //services.AddDbContext<AppDbContext>(options =>
            //options.UseInMemoryDatabase(Guid.NewGuid().ToString()));

            // TODO: Add safer connection string for production (system environment variable)
            var connectionString = Configuration.GetConnectionString("UserDbConnectionString");
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

            // Add application services
            services.AddTransient<IRepository<User>, EfRepository<User>>();
            services.AddTransient<IUser, UserService>();

            services.AddMvc();
            services.AddMemoryCache();

            services.AddMvcCore().AddApiExplorer();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Aura Users API", Version = "v1" });
            });

            var container = new Container();

            container.Configure(config =>
            {
                config.Scan(_ =>
                {
                    _.AssemblyContainingType(typeof(Startup)); // Web
                    _.AssemblyContainingType(typeof(BaseEntity)); // Core
                    _.Assembly("Aura.Infrastructure"); // Infrastructure
                    _.WithDefaultConventions();
                    _.ConnectImplementationsToTypesClosing(typeof(IHandle<>));
                });
                
                // TODO: Add Registry Classes to eliminate reference to Infrastructure

                // TODO: Move to Infrastucture Registry
                config.For(typeof(IRepository<>)).Add(typeof(EfRepository<>));

                //Populate the container using the service collection
                config.Populate(services);
            });

            return container.GetInstance<IServiceProvider>();
      
        }

#region Testing Environment
        //public void ConfigureTesting(IApplicationBuilder app,
        //    IHostingEnvironment env,
        //    ILoggerFactory loggerFactory)
        //{
        //    this.Configure(app, env, loggerFactory);
        //    PopulateTestData(app);
        //}

        //private void PopulateTestData(IApplicationBuilder app)
        //{
        //    var dbContext = app.ApplicationServices.GetService<AppDbContext>();
        //    var users = dbContext.Users;

        //    foreach (var item in users)
        //    {
        //        dbContext.Remove(item);
        //    }

        //    dbContext.SaveChanges();
        //    dbContext.Users.Add(new User()
        //    {
        //        Name = "Srdjan",
        //        Email = "srg_ian@hotmail.com",
        //        CreatedOn = DateTime.Now
        //    });
        //    dbContext.Users.Add(new User()
        //    {
        //        Name = "Stefan Stanoeski",
        //        Email = "stefan.stanoeski@yahoo.com",
        //        CreatedOn = DateTime.Now
        //    });

        //    dbContext.SaveChanges();
        //}
#endregion

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, AppDbContext appDbContext)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));

            loggerFactory.AddDebug();

            loggerFactory.AddNLog();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            appDbContext.EnsureSeedDataForContext();

            app.UseStaticFiles();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Aura Users API V1");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

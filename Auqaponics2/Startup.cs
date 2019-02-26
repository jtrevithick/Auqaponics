using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Auqaponics2.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Auqaponics2.Hubs;
using Microsoft.AspNetCore.SignalR;
using Auqaponics2.Models;

namespace Auqaponics2
{
    public class Startup
    {
       

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddRoleManager<RoleManager<IdentityRole>>()
                .AddDefaultUI()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<ApplicationDbContext>();



            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSignalR();
            services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseSignalR(routes =>
            {
                routes.MapHub<ChatHub>("/chatHub");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            CreateUsersAndRoles(serviceProvider).Wait();

        }
        private async Task CreateUsersAndRoles(IServiceProvider serviceProvider)
        {
            //Get reference to RoleManager and UserManager from serviceProvider through dependency injection
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            //Check if admin role exists and if not add it.
            var roleExist = await RoleManager.RoleExistsAsync("Admin");
            if (!roleExist)
            {
                //create the roles and seed them to the database: Question 1  
                await RoleManager.CreateAsync(new IdentityRole("Admin"));
            }

            //Check if admin user exists and if not add it
            IdentityUser user = await UserManager.FindByEmailAsync("admin@aserver.net");

            if (user == null)
            {
                user = new IdentityUser()
                {
                    UserName = "admin@aserver.net",
                    Email = "admin@aserver.net",
                };
                await UserManager.CreateAsync(user, "P@ssword12");

                //Add user to admin role
                await UserManager.AddToRoleAsync(user, "Admin");
            }
        }

    }
}

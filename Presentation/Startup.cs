using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DataAccess.Context;
using Domain.Interfaces;
using DataAccess.Repositories;
using Application.Interfaces;
using Application.Services;
using Domain.Models;
using System.IO;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;

namespace Presentation
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
            //To Build: Ctrl + shift + B
            //To get a namespace of a class (which you are sure it exists and you have written the name correctly):
            //  Ctrl + .  (after placing the cursor on the class name itself)

            services.AddDbContext<BloggingContext>(options =>
              options.UseSqlServer(
                  Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<CustomUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;

             /*   options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.AllowedForNewUsers = true;
             */

                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 7;
            }).AddEntityFrameworkStores<BloggingContext>()
               .AddDefaultUI()
                 .AddDefaultTokenProviders();
      

            services.AddControllersWithViews();
            services.AddRazorPages();

            //builder.Services.Configure<IdentityOptions>(options =>
            //{
            //    // Default Lockout settings.
            //    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            //    options.Lockout.MaxFailedAccessAttempts = 5;
            //    options.Lockout.AllowedForNewUsers = true;
            //});

            services.AddAuthentication()
                       .AddGoogle(options =>
                       {
                           IConfigurationSection googleAuthNSection =
                               Configuration.GetSection("Authentication:Google");

                           options.ClientId = googleAuthNSection["ClientId"];
                           options.ClientSecret = googleAuthNSection["ClientSecret"];
                       });


            //we are informing the injector class what must be initialized when it comes across
            //a request for example for IBlogsService, IBlogsRepository
            services.AddScoped<IBlogsRepository, BlogsRepository>();
            
            services.AddScoped<IBlogsService, BlogsService>();
            services.AddScoped<ICategoriesRepository, CategoriesRepository>();
            services.AddScoped<ICategoriesService, CategoriesService>();

           


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,  ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            //app.UseStaticFiles(new StaticFileOptions
            //{
            //    FileProvider = new PhysicalFileProvider(
            //Path.Combine(env.ContentRootPath, "Files")),
            //    RequestPath = "/Files"
            //}   );
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            loggerFactory.AddFile("Logs/mylog-{Date}.txt");
        }
    }
}

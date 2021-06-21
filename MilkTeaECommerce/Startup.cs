using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MilkTeaECommerce.Data;
using MilkTeaECommerce.DataAccess.Repository;
using MilkTeaECommerce.DataAccess.Repository.IRepository;
using MilkTeaECommerce.Models;
using MilkTeaECommerce.Utility;
using Microsoft.AspNetCore.Http;
using System;

namespace MilkTeaECommerce
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
            services.AddCors(options =>
            {
                options.AddPolicy("MyPlolicy",
                    builder => builder.WithOrigins("http://localhost:51151"));
            });


            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<ApplicationUser>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireNonAlphanumeric = true;
            })
            .AddUserManager<UserManager<ApplicationUser>>()
            .AddSignInManager<SignInManager<ApplicationUser>>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();
            services.Configure<EmailOptions>(Configuration.GetSection("EmailOptions"));
            services.AddSingleton<IEmailSender, EmailSender>();


            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Identity/Login";
                options.LogoutPath = $"/Identity/Logout";
                options.AccessDeniedPath = $"/Identity/AccessDenied";
                options.Cookie.SameSite = SameSiteMode.Strict;
            });
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(1000);
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = SameSiteMode.Strict;
                options.Cookie.IsEssential = true;
            });
            services.AddRazorPages().AddRazorRuntimeCompilation();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

            //fix error X-Frame-Options Header Not Set
            // X-frame-options header set same origin 
            //(X-FRAME-OPTIONS : SAMEORIGIN) it mean:  The page can be framed as long as the domain framing it is the same. This is good if you are using frames yourself.

            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
                await next();
            });


            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                await next();
            });
            app.UseCors("MyPlolicy");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {

                endpoints.MapAreaControllerRoute(
                   name: "Seller",
                   areaName: "Seller",
                   pattern: "Seller/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapAreaControllerRoute(
                   name: "Admin",
                   areaName: "Admin",
                   pattern: "Admin/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapAreaControllerRoute(
                  name: "Shipper",
                  areaName: "Shipper",
                  pattern: "Shipper/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapDefaultControllerRoute();



            });
        }
    }
}

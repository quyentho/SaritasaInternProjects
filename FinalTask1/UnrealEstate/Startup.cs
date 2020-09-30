using System;
using System.Reflection;
using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UnrealEstate.Business.Authentication;
using UnrealEstate.Business.Authentication.Interface;
using UnrealEstate.Business.Comment;
using UnrealEstate.Business.Comment.Repository;
using UnrealEstate.Business.Comment.Service;
using UnrealEstate.Business.Email;
using UnrealEstate.Business.Email.Service;
using UnrealEstate.Business.Listing;
using UnrealEstate.Business.Listing.Repository;
using UnrealEstate.Business.Listing.Service;
using UnrealEstate.Business.Listing.ViewModel;
using UnrealEstate.Business.MappingConfig;
using UnrealEstate.Business.User;
using UnrealEstate.Business.User.Service;
using UnrealEstate.Infrastructure;
using UnrealEstate.Infrastructure.ModelConfigs;
using UnrealEstate.Infrastructure.Models;

namespace UnrealEstate
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
            services.AddDbContext<UnrealEstateDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    options.User.AllowedUserNameCharacters =
                        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";

                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredLength = 6;
                    options.Password.RequiredUniqueChars = 0;

                    // Lockout settings.
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                    options.Lockout.MaxFailedAccessAttempts = 5;
                    options.Lockout.AllowedForNewUsers = true;

                    options.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<UnrealEstateDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    options.ClientId = "276236464048-s6dhn7otoiedo6j0hltv6r3ke4ir8kkj.apps.googleusercontent.com";
                    options.ClientSecret = "HAz2HReoehA9DKcAEN6l5c88";
                });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

                options.LoginPath = "/Users/Login";
                options.SlidingExpiration = true;
            });

            services.AddTransient<IListingRepository, ListingRepository>();
            services.AddTransient<IListingService, ListingService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IAuthenticationService, AuthenticationService>();
            services.AddTransient<ICommentRepository, CommentRepository>();
            services.AddTransient<ICommentService, CommentService>();
            services.AddTransient<IEmailSenderService, EmailSenderService>();
            services.AddTransient<EmailConfiguration>();

            var emailConfig = Configuration.GetSection("EmailConfiguration")
                .Get<EmailConfiguration>();
            services.AddSingleton(emailConfig);

            services.AddAutoMapper(Assembly.GetAssembly(typeof(AutoMapperProfile)));

            services.AddAuthorization();

            services.AddControllersWithViews()
                .AddFluentValidation(fv =>
                {
                    fv.RegisterValidatorsFromAssembly(Assembly.GetAssembly(typeof(ListingValidator)));
                    fv.ImplicitlyValidateChildProperties = true;
                });
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<UnrealEstateDbContext>();

                context.Database.Migrate();

                UserManager<ApplicationUser> userManager = (UserManager<ApplicationUser>)serviceScope.ServiceProvider.GetService(typeof(UserManager<ApplicationUser>));
                RoleManager<IdentityRole> roleManager = (RoleManager<IdentityRole>) serviceScope.ServiceProvider.GetService(typeof(RoleManager<IdentityRole>));
                DatabaseInitializer.InitializeSeedData(context, userManager, roleManager);
            }


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


            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapRazorPages();
            });
        }
    }
}
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using UnrealEstate.Business.Authentication;
using UnrealEstate.Business.Authentication.Interface;
using UnrealEstate.Business.Comment;
using UnrealEstate.Business.Comment.Interface;
using UnrealEstate.Business.Comment.Repository;
using UnrealEstate.Business.Comment.Repository.Interface;
using UnrealEstate.Business.Email;
using UnrealEstate.Business.Email.Interface;
using UnrealEstate.Business.Listing;
using UnrealEstate.Business.Listing.Interface;
using UnrealEstate.Business.Listing.Repository;
using UnrealEstate.Business.Listing.Repository.Interface;
using UnrealEstate.Business.Listing.ViewModel.Request.Validator;
using UnrealEstate.Business.MappingConfig;
using UnrealEstate.Business.User;
using UnrealEstate.Business.User.Interface;
using UnrealEstate.Infrastructure;
using UnrealEstate.Infrastructure.ModelConfigs;
using UnrealEstate.Infrastructure.Models;
using UnrealEstateApi.Configurations;

namespace UnrealEstateApi
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
            services.AddDbContext<UnrealEstateDbContext>();

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    options.User.AllowedUserNameCharacters =
                        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";

                    options.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<UnrealEstateDbContext>()
                .AddDefaultTokenProviders();


            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidAudience = Configuration["JWT:ValidAudience"],
                        ValidIssuer = Configuration["JWT:ValidIssuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]))
                    };
                });

            var emailConfig = Configuration.GetSection("EmailConfiguration")
                .Get<EmailConfiguration>();
            services.AddSingleton(emailConfig);

            // TODO: this should be redefined for security purpose;
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;
            });

            services.AddControllers()
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore)
                .AddFluentValidation(fv =>
                {
                    fv.RegisterValidatorsFromAssembly(Assembly.GetAssembly(typeof(ListingValidator)));
                    fv.ImplicitlyValidateChildProperties = true;
                });

            services.AddTransient<IListingRepository, ListingRepository>();
            services.AddTransient<IListingService, ListingService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IAuthenticationService, AuthenticationService>();
            services.AddTransient<ICommentRepository, CommentRepository>();
            services.AddTransient<ICommentService, CommentService>();
            services.AddTransient<IEmailSenderService, EmailSenderService>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "Unreal Estate API", Version = "v1"});

                //First we define the security scheme
                c.AddSecurityDefinition("Bearer", //Name the security scheme
                    new OpenApiSecurityScheme
                    {
                        Description = "JWT Authorization header using the Bearer scheme.",
                        Type = SecuritySchemeType
                            .Http, //We set the scheme type to http since we're using bearer authentication
                        Scheme =
                            "bearer" //The name of the HTTP Authorization scheme to be used in the Authorization header. In this case "bearer".
                    });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer", //The name of the previously defined security scheme.
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        new List<string>()
                    }
                });
            });

            services.AddAutoMapper(Assembly.GetAssembly(typeof(AutoMapperProfile)));

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<UnrealEstateDbContext>();

                context.Database.Migrate();

                UserManager<ApplicationUser> userManager = (UserManager<ApplicationUser>)serviceScope.ServiceProvider.GetService(typeof(UserManager<ApplicationUser>));
                RoleManager<IdentityRole> roleManager = (RoleManager<IdentityRole>)serviceScope.ServiceProvider.GetService(typeof(RoleManager<IdentityRole>));
                DatabaseInitializer.InitializeSeedData(context, userManager, roleManager);
            }

            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            var swaggerOptions = Configuration.GetSection("SwaggerOptions").Get<SwaggerOptions>();

            app.UseSwagger(option => { option.RouteTemplate = swaggerOptions.JsonRoute; });
            app.UseSwaggerUI(option =>
            {
                option.SwaggerEndpoint(swaggerOptions.UIEndpoint, swaggerOptions.Description);
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    "default",
                    "{controller=Listing}/{action=GetListing}/{id?}");
            });
        }
    }
}
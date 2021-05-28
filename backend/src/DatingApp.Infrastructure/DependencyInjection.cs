﻿
using System;
using System.Text;
using CloudinaryDotNet;
using DatingApp.Core.Entities;
using DatingApp.Core.Interfaces;
using DatingApp.Core.Interfaces.Database;
using DatingApp.Core.Interfaces.Database.Repositories;
using DatingApp.Core.Interfaces.Services;
using DatingApp.Core.Services;
using DatingApp.Infrastructure.Database;
using DatingApp.Infrastructure.Database.Repositories;
using DatingApp.Infrastructure.Image;
using DatingApp.Infrastructure.Mapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.DI
{
    public static class DependencyInjection
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            AddIdentityCore(services);
            AddJwtAuthentication(services, configuration);
            AddDbContext(services, configuration);
            AddAutoMapper(services);
            AddCloudinary(services, configuration);
            AddServices(services);
        }

        private static void AddJwtAuthentication(IServiceCollection services, IConfiguration configuration)
        {
            var appSecret = configuration["AppSecret"];
            if (string.IsNullOrWhiteSpace(appSecret))
                throw new ArgumentNullException(nameof(appSecret));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(appSecret)),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                    };
                });
        }

        private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IUserPhotoRepository, UserPhotoRepository>();
            services.AddTransient<ILikeRepository, LikeRepository>();
            services.AddTransient<IMessageRepository, MessageRepository>();

            services.AddTransient<IUnitOfWork, UnitOfWork>();

            var connection = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrWhiteSpace(connection))
                throw new ArgumentNullException(nameof(connection));

            services.AddDbContext<DatabaseContext>(option =>
            {
                option.UseLazyLoadingProxies(); // TODO: Remove Lazy Loading
                option.UseSqlServer(connection);
            });
        }

        private static void AddIdentityCore(IServiceCollection services)
        {
            var builder = services.AddIdentityCore<User>(opt =>
            {
                opt.Password.RequireDigit = false;
                opt.Password.RequiredLength = 4;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;
            });

            builder = new IdentityBuilder(builder.UserType, typeof(Role), builder.Services);
            builder.AddEntityFrameworkStores<DatabaseContext>();
            builder.AddRoleValidator<RoleValidator<Role>>();
            builder.AddRoleManager<RoleManager<Role>>();
            builder.AddSignInManager<SignInManager<User>>();
        }

        private static void AddCloudinary(IServiceCollection services, IConfiguration configuration)
        {
            var cloudName = configuration["CloudinarySettings:CloudName"];
            if (string.IsNullOrWhiteSpace(cloudName))
                throw new ArgumentNullException(nameof(cloudName));

            var apiKey = configuration["CloudinarySettings:ApiKey"];
            if (string.IsNullOrWhiteSpace(apiKey))
                throw new ArgumentNullException(nameof(apiKey));

            var apiSecret = configuration["CloudinarySettings:ApiSecret"];
            if (string.IsNullOrWhiteSpace(apiSecret))
                throw new ArgumentNullException(nameof(apiSecret));

            var account = new Account(cloudName, apiKey, apiSecret);

            services.AddSingleton(new Cloudinary(account));
        }

        private static void AddAutoMapper(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(DependencyInjection));
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddScoped<IClassMapper, ClassMapper>();
            services.AddScoped<IImageUploader, ImageUploader>();

            services.AddScoped<ILikeService, LikeService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IUserPhotoService, UserPhotoService>();
            services.AddScoped<IUserRolesService, UserRolesService>();
            services.AddScoped<IUserService, UserService>();
        }
    }
}

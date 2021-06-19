using DatingApp.Api.Helpers;
using DatingApp.Api.Middlewares;
using DatingApp.Core.Constants;
using DatingApp.DI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace DatingApp.Api
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
            services.AddInfrastructure(Configuration);

            this.AddAuthorizationPolicy(services);

            this.AddControllers(services);

            services.AddCors();

            // to log any user activity
            services.AddScoped<LogUserActivity>();

            // swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DatingApp.Api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // swagger 
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DatingApp.Api v1"));
            }

            // app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            // setup cors
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseMiddleware<ErrorHandlerMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public void AddAuthorizationPolicy(IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(Authorizations.AdminRole, policy => policy.RequireRole(Roles.Admin));
                options.AddPolicy(Authorizations.ModeratorRole, policy => policy.RequireRole(Roles.Admin, Roles.Moderator));
            });
        }

        public void AddControllers(IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            })
            .AddNewtonsoftJson(opt =>
            {
                // to fix error
                // Self referencing loop detected for property 'xxx' with type 'xxx'. Path 'xxx'
                opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });
        }
    }
}

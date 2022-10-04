using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using WebApi.Repositories;
using WebApi.Repositories.Queries;

namespace WebApi
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
            #region Repositories
            services.AddScoped(typeof(ICommandText<>), typeof(CommandText<>));
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            #endregion


            services.AddControllers();
            AddSwagger(services);
        }

        private void AddSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                string groupName = "v1";
                options.SwaggerDoc(groupName, new OpenApiInfo
                {
                    Title = "Redarbor - WebApi",
                    Version = groupName,
                    Description = "Web Api that accesses a database where it stores employee content.",
                    Contact = new OpenApiContact
                    {
                        Name = "README WEB API - Redarbor",
                        Email = string.Empty,
                        Url = new Uri("https://github.com/Yac-Mc/Redarbor-WebApi"),
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseCors(c => c.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Redarbor - WebApi V1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

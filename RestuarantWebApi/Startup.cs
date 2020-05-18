using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using RestuarantWebApi.HubConfig;
using RestuarantWebApi.Repository;
using RestuarantWebApi.SqlService;
using RestuarantWebApi.Utils;
using System;

namespace RestuarantWebApi
{
    public class Startup
    {
        private readonly string _corname = "AllowSpecificOrigins";

        private readonly string _urlAccess = "http://localhost:4200";

        private string _urlonProduction = "http://localhost:4200";

        public IConfiguration Configuration { get; }

        /// <summary>
        /// Construtor to add 
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            // provide Dependency for Controller.
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title="Restuarant Api",Version="v1" });
                c.OperationFilter<RequestHeader>();
            });
            // Cross Origin Request Sharing.
            services.AddCors(options =>
            {
                options.AddPolicy(name: _corname, builder =>
                {
                    builder.WithOrigins(_urlAccess);
                    //builder.AllowAnyOrigin();
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                    builder.AllowCredentials();
                });
            });

            // Repository Pattern for Web Api.
            services.AddScoped<IUserRepository, UserService>();
            // Add SignalR.
            services.AddSignalR((huboptions) =>
            {

                huboptions.EnableDetailedErrors = true;
                huboptions.KeepAliveInterval = TimeSpan.FromMinutes(1);

            });
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();   
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            // Enable Cross Origin Resource Sharing.

            app.UseCors(this._corname);


            app.UseMvc();
            //app.UseAuthentication();
            // Add Swagger so that one can See list of Api.
            app.UseSwagger(c => {
                c.RouteTemplate = "Restuarantapi/swagger/{documentName}/swagger.json";
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/Restuarantapi/swagger/v1/swagger.json", "Restuarantapi");
                c.RoutePrefix = string.Empty;
            });
            
          
            app.UseSignalR(routes => {
                routes.MapHub<LoginHub>("/api/login");
            });
        }
    }
}

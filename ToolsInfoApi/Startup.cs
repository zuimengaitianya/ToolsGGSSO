using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using ToolsInfoApi.Models;
using ToolsInfoApi.Services;
using IdentityModel;
using System.IdentityModel.Tokens.Jwt;
using System.IO;

namespace ToolsInfoApi
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
            services.AddDbContext<TodoContext>(opt => opt.UseInMemoryDatabase("TodoList"));
            services.AddScoped<BookService>();

            // Add framework services.
            //services.AddApplicationInsightsTelemetry(Configuration);


            //JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();


            services.AddMvcCore()
                .AddAuthorization()
                .AddJsonFormatters();

            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = Configuration["BearerIssus:DefaultBearer"]; //"http://localhost:5000";
                    options.RequireHttpsMetadata = false;
                    options.Audience = "api1";
                });

            #region 跨域
            //配置cors。这将允许http://localhost:5002到http://localhost:5001进行Ajax跨域调用.
            services.AddCors(options =>
            {
                options.AddPolicy("default", policy =>
                {
                    policy.WithOrigins(Configuration["defaultCors:DefaultCors"] )//"http://localhost:5002"
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });
            #endregion
            
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info
                {
                    Version = "Version 1.0",
                    Title = "ToolsInfoApi Document",
                    Description = "Alone"
                });

                var basePath = AppContext.BaseDirectory;
                var xmlPath = Path.Combine(basePath, "ToolsInfoApi.xml");
                options.IncludeXmlComments(xmlPath);

            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //跨域访问
            app.UseCors("default");

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseAuthentication();
            
            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "OrgAPI");
                //c.RoutePrefix = string.Empty;
                c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
            });
        }
    }
}

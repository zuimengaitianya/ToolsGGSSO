using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ToolsInfoGGSSO
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
            // Adds IdentityServer
            var builder = services.AddIdentityServer(option=>option.IssuerUri= Configuration["IssuerUri:DefaultUri"])
                     /*
                      * AddDeveloperSigningCredential
                      * 
                      * Ids4会在项目目录中生成一个tempkey.rsa证书文件；
                      * ids4会先判断tempkey.rsa证书文件是否存在，如果不存在的话，就创建一个新的tempkey.rsa证书文件，
                      * 如果存在的话，就使用此证书文件。
                      */
                     .AddDeveloperSigningCredential()
                     
                     .AddInMemoryIdentityResources(Config.GetIdentityResources())
                     .AddInMemoryApiResources(Config.GetApiResources())
                     .AddInMemoryClients(Config.GetClients())
                     .AddTestUsers(Config.GetUsers())
                     .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
                     //.AddProfileService<ProfileService>()
                     ;
            services.AddAuthentication("Bearer")
                .AddCookie("Cookies")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = "http://localhost:5000";
                    options.RequireHttpsMetadata = false;
                    options.Audience = "api1";
                });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            
            
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseIdentityServer();

            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

    }
}

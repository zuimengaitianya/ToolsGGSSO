using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ToolsGGSSO.MvcClient
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

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = "Cookies";
                    options.DefaultChallengeScheme = "oidc";
                })
                .AddCookie("Cookies",options=> {
                    options.AccessDeniedPath = "/Authroization/Index";
                })
                .AddOpenIdConnect("oidc", options =>
                {
                    #region Implicit Model
                    //options.SignInScheme = "Cookies";

                    //options.Authority = "http://localhost:5000";
                    //options.RequireHttpsMetadata = false;

                    //options.ClientId = "mvc1";
                    //options.SaveTokens = true;
                    #endregion

                    #region Hybrid Model
                    options.SignInScheme = "Cookies";

                    options.Authority = "http://localhost:5000";
                    options.RequireHttpsMetadata = false;

                    options.ClientId = "mvc";
                    options.ClientSecret = "secret";
                    options.ResponseType = "code id_token";

                    //是否将Tokens保存到AuthenticationProperties中,最终到浏览器cookie中
                    options.SaveTokens = true;
                    //是否从UserInfoEndpoint获取Claims
                    options.GetClaimsFromUserInfoEndpoint = true;

                    options.Scope.Clear();

                    options.Scope.Add("email");
                    options.Scope.Add("roles");
                    options.Scope.Add("openid");
                    options.Scope.Add("profile");


                    options.ClaimActions.DeleteClaim("email");

                    //添加资源范围,访问api
                    options.Scope.Add("api1");
                    //离线访问,此范围值请求发出OAuth 2.0刷新令牌，该令牌可用于获取访问令牌,
                    //该令牌授予对最终用户的UserInfo端点的访问权，即使最终用户不存在(未登录)。
                    options.Scope.Add("offline_access");
                    //收集Claims
                    options.ClaimActions.MapJsonKey("website", "website");
                    options.ClaimActions.MapUniqueJsonKey("role", "role");
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        NameClaimType = JwtClaimTypes.GivenName,
                        RoleClaimType = JwtClaimTypes.Role
                    };
                    #endregion

                });

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
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
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

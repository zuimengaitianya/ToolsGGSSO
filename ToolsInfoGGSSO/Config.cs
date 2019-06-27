using IdentityModel;
using IdentityServer4;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Test;
using IdentityServer4.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ToolsInfoGGSSO
{
    public class Config
    {

        public static string baseUrl = "http://localhost:5002/";
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResource("roles","roles",new List<string>{ "role"})
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                //new ApiResource("api1", "My API")
                new ApiResource("api1", "My API"){ UserClaims = new List<string>{"role"} },//增加 role claim
            };
        }

        // 可以访问的客户端
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "client",
                    //没有交互性用户，使用clientid/secret实现认证。
                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ClientCredentials,//客户端认证方式(即只要请求头的client_id与client_secret设置对了就可以获取到token)
                    //用于认证的密码
                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    //客户端有权访问的范围(scopes)
                    // scopes that client has access to //必须要添加后面的参数，否则报forbidden错误
                    AllowedScopes = { "api1",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile}
                },
                new Client
                {
                    ClientId = "ro.client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,//密码认证方式（需要用户提供账号和密码才可以获取到token）
                    AllowOfflineAccess = true,
                    AccessTokenLifetime = 3600*6,//6小时
                    SlidingRefreshTokenLifetime = 1296000,//15天
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "api1",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile }
                },
                // OpenID Connect implicit flow client (MVC)
                new Client
                {
                    ClientId = "mvc1",
                    ClientName = "MVC Client",
                    AllowedGrantTypes = GrantTypes.Implicit,

                    // 登录成功回调处理地址，处理回调返回的数据
                    RedirectUris = { baseUrl+"signin-oidc" },

                    // where to redirect to after logout
                    PostLogoutRedirectUris = { baseUrl+"signout-callback-oidc" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    }
                },
                new Client
                {
                    ClientId = "mvc",
                    ClientName = "MVC Client",
                    AllowedGrantTypes = GrantTypes.Hybrid,

                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    RedirectUris           = { baseUrl+"signin-oidc" },
                    PostLogoutRedirectUris = { baseUrl+"signout-callback-oidc" },
                    RequireConsent=false,//禁用Consent页面确认
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "roles",
                        "api1"
                    },

                    AllowOfflineAccess = true
                },
                // JavaScript Client
                new Client
                {
                    ClientId = "js",
                    ClientName = "JavaScript Client",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,
                    AllowAccessTokensViaBrowser=true,

                    RedirectUris =           { baseUrl+"html/callback.html" },
                    PostLogoutRedirectUris = { baseUrl+"html/index.html" },
                    AllowedCorsOrigins =     { baseUrl },
                    RequireConsent=false,//禁用Consent页面确认
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api1"
                    }
                }
            };
        }

        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>()
            {
                new TestUser
                {
                    SubjectId="1",
                    Username="alice",
                    Password="123456",
                    Claims=new Claim[]
                            {
                                new Claim("UserId",1.ToString()),
                                new Claim(JwtClaimTypes.Name,"alice"),
                                new Claim(JwtClaimTypes.GivenName,"alice"),
                                new Claim(JwtClaimTypes.FamilyName,"globetools"),
                                new Claim(JwtClaimTypes.Email,"565009871@qq.com"),
                                new Claim(JwtClaimTypes.Role,"normal")
                            }
                },
                new TestUser
                {
                    SubjectId="2",
                    Username="alone",
                    Password="123456",
                    Claims=new Claim[]
                            {
                                new Claim("UserId",1.ToString()),
                                new Claim(JwtClaimTypes.Name,"anlong"),
                                new Claim(JwtClaimTypes.GivenName,"alone"),
                                new Claim(JwtClaimTypes.FamilyName,"globetools"),
                                new Claim(JwtClaimTypes.Email,"565009871@qq.com"),
                                new Claim(JwtClaimTypes.Role,"admin")
                            }
                }
            };
        }

    }

    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly TestUserStore _userService;
        public ResourceOwnerPasswordValidator(TestUserStore userService)
        {
            _userService = userService;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            if (_userService.ValidateCredentials(context.UserName, context.Password))
            {
                TestUser user = _userService.FindByUsername(context.UserName);
                context.Result = new GrantValidationResult(
                    subject: user.SubjectId,
                    authenticationMethod: "custom"
                    //claims: user.Claims
                    );
            }
            else
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "invalid custom credential");
            }
        }

    }

    public class ProfileService : IProfileService
    {
        private readonly TestUserStore _userService;

        public ProfileService(TestUserStore userService)
        {
            _userService = userService;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            try
            {
                //depending on the scope accessing the user data.
                var claims = context.Subject.Claims.ToList();

                //set issued claims to return
                context.IssuedClaims = claims.ToList();

                //var user = _userService.FindBySubjectId(context.Subject.GetSubjectId());
                //if (user != null)
                //{
                //    //context.AddRequestedClaims(user.Claims);
                //    context.IssuedClaims.AddRange(user.Claims);
                //}

            }
            catch (Exception ex)
            {

            }
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = true;
        }
    }

}

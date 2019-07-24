using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Refit;
using ToolsGG.Cat.Models;

namespace ToolsGG.Cat.Services
{
    public interface IXlinkApi
    {
        /// <summary>
        /// 登录与认证
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Headers("Content-Type: application/json")]
        [Post("/v2/user_auth")]
        Task<UserAuthResponse> postUserAuth([Body] UserAuthRequest request);

        /// <summary>
        /// 刷新凭证
        /// </summary>
        /// <param name="request"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [Headers("Content-Type: application/json")]
        [Post("/v2/user/token/refresh")]
        Task<TokenRefreshResponse> postUserTokenRefresh([Body] TokenRefreshRequest request, [Header("Access-Token")] string token);
    }
}

using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ToolsGG.Cat.Models;

namespace ToolsGG.Cat.Services
{
    public class RestApiCaller
    {
        private readonly IXlinkApi _xlinkApi;
        UserRepository userRepository;
        public RestApiCaller()
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(""),
                Timeout = TimeSpan.FromSeconds(15),
            };
            _xlinkApi = RestService.For<IXlinkApi>(client);
        }

        protected async Task<string> GetAccessToken()
        {

            if (userRepository != null)
            {
                if (userRepository.LastLogin.AddSeconds(userRepository.TokenTime) > DateTime.Now.AddSeconds(120))
                {
                    return userRepository.AccessToken;
                }
                else
                {
                    TokenRefreshRequest tokenRefreshRequest = new TokenRefreshRequest
                    {
                        refreshToken = userRepository.RefreshToken,
                    };
                    var res = await _xlinkApi.postUserTokenRefresh(tokenRefreshRequest, userRepository.AccessToken);
                    userRepository.AccessToken = res.accessToken;
                    userRepository.TokenTime = int.Parse(res.expireIn);
                    userRepository.LastLogin = DateTime.Now;
                    userRepository.RefreshToken = res.refreshToken;

                    return userRepository.AccessToken;
                }
            }
            else
            {
                UserAuthRequest userAuthRequest = new UserAuthRequest
                {

                };
                var res = await _xlinkApi.postUserAuth(userAuthRequest);

                userRepository = new UserRepository
                {
                    AccessToken = res.accessToken,
                    TokenTime = res.expireIn,
                    LastLogin = DateTime.Now,
                    RefreshToken = res.refreshToken,
                };

                return userRepository.AccessToken;

            }


        }

        public async void GetUserInfo()
        {
            string accessToken = await GetAccessToken();
        }

    }
}

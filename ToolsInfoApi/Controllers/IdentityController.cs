using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ToolsInfoApi.Controllers
{

    
    
    public class IdentityController : ControllerBase
    {
        [Route("identity")]
        [Authorize]
        [HttpGet]
        public IActionResult Get()
        {
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }

        [Route("identity/Get2")]
        [Authorize(Roles ="admin")]
        [HttpGet]
        public IActionResult Get2()
        {
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
            //return Ok();
        }

        [Route("identity/Get3")]
        [Authorize(Roles ="admin,normal")]
        [HttpGet]
        public IActionResult Get3()
        {
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
            //return Ok();
        }

        public async Task<TokenResponse> GetToken(string clientId, string clientSecret, string grantType, string userName, string password, string scope)
        {
            var client = new DiscoveryClient($"http://localhost:5001");
            client.Policy.RequireHttps = false;
            var disco = await client.GetAsync();
            var tokenClient = new TokenClient(disco.TokenEndpoint, clientId, clientSecret);
            return await tokenClient.RequestResourceOwnerPasswordAsync(userName, password, scope);
        }

        public async Task<TokenResponse> GetRefreshToken(string clientId, string clientSecret, string grantType, string refreshToken)
        {
            var client = new DiscoveryClient($"http://localhost:5001");
            client.Policy.RequireHttps = false;
            var disco = await client.GetAsync();
            var tokenClient = new TokenClient(disco.TokenEndpoint, clientId, clientSecret);
            return await tokenClient.RequestRefreshTokenAsync(refreshToken);
        }

    }
}
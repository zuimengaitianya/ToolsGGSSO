using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ToolsWebClient
{
    public static class Constants
    {
        public const string BaseAddress = "http://localhost:5000";

        public const string AuthorizeEndpoint = BaseAddress + "/connect/authorize";
        public const string LogoutEndpoint = BaseAddress + "/connect/endsession";
        public const string TokenEndpoint = BaseAddress + "/connect/token";
        public const string UserInfoEndpoint = BaseAddress + "/connect/userinfo";
        public const string IdentityTokenValidationEndpoint = BaseAddress + "/connect/identitytokenvalidation";
        public const string TokenRevocationEndpoint = BaseAddress + "/connect/revocation";

        public const string AspNetWebApiSampleApi = "http://localhost:5001";
        public const string AspNetWebApiSampleApiUsingPoP = "http://localhost:46613/";
    }
}
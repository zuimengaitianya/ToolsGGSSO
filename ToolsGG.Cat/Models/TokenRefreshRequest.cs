using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToolsGG.Cat.Models
{
    /// <summary>
    /// 刷新凭证 请求
    /// </summary>
    public class TokenRefreshRequest
    {
        /// <summary>
        /// 刷新凭证
        /// </summary>
        [JsonProperty(PropertyName = "refresh_token")]
        public string refreshToken { get; set; }
    }

    /// <summary>
    /// 刷新凭证 响应
    /// </summary>
    public class TokenRefreshResponse
    {
        /// <summary>
        /// 新的调用凭证
        /// </summary>
        [JsonProperty(PropertyName = "access_token")]
        public string accessToken { get; set; }
        /// <summary>
        /// 新的刷新凭证
        /// </summary>
        [JsonProperty(PropertyName = "refresh_token")]
        public string refreshToken { get; set; }
        /// <summary>
        /// 有效期
        /// </summary>
        [JsonProperty(PropertyName = "expire_in")]
        public string expireIn { get; set; }
    }

}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToolsGG.Cat.Models
{
    /// <summary>
    /// 登录与认证 请求
    /// </summary>
    public class UserAuthRequest
    {
        /// <summary>
        /// 企业ID
        /// </summary>
        [JsonProperty(PropertyName = "corp_id")]
        public string corpId { get; set; }
        /// <summary>
        /// 手机号 与邮箱2选1
        /// </summary>
        public string phone { get; set; }
        /// <summary>
        /// 邮箱 与手机号2选1
        /// </summary>
        public string email { get; set; }
        /// <summary>
        /// 手机区号，不填则默认中国:+86
        /// </summary>
        [JsonProperty(PropertyName = "phone_zone")]
        public string phoneZone { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string password { get; set; }
        /// <summary>
        /// 登录源，用户可以在登录时指定登录源，不同登录源可同时登录，长度在0~16个字符之间。
        /// </summary>
        public string resource { get; set; }
    }

    /// <summary>
    /// 登录与认证 响应
    /// </summary>
    public class UserAuthResponse {
        /// <summary>
        /// 用户ID
        /// </summary>
        [JsonProperty(PropertyName = "user_id")]
        public int userId { get; set; }
        /// <summary>
        /// 调用凭证
        /// </summary>
        [JsonProperty(PropertyName = "access_token")]
        public string accessToken { get; set; }
        /// <summary>
        /// 刷新凭证
        /// </summary>
        [JsonProperty(PropertyName = "refresh_token")]
        public string refreshToken { get; set; }
        /// <summary>
        /// 有效期（秒）
        /// </summary>
        [JsonProperty(PropertyName = "expire_in")]
        public int expireIn { get; set; }
        /// <summary>
        /// APP登录云端认证码
        /// </summary>
        public string authorize { get; set; }

    }
}

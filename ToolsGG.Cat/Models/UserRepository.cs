using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToolsGG.Cat.Models
{
    public class UserRepository
    {
        public string Name { get; set; }
        public string AccessToken { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public string AuthString { get; set; }
        public string CorpId { get; set; }
        public int TokenTime { get; set; }
        public DateTime LastLogin { get; set; }
        public int Uid { get; set; }
        public string RefreshToken { get; set; }
        public bool IsLoggedIn { get; set; }
        public string CurrentEnvironment { get; set; }
    }
}

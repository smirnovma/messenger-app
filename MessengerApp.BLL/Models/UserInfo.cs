using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerApp.BLL.Models
{
    public class UserInfo
    {
        [JsonProperty("Email")]
        public string Name { get; set; }
    }
}

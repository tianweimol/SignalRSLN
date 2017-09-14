using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace SignalR.Server.MVC.Models
{
    [Serializable]
    public class UserModel
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Gender { get; set; } = "男";
        public string GroupName { get; set; }
        public string UserName { get; set; }
        public string ConnectionId { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
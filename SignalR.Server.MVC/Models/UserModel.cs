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
        public int Gender { get; set; } = 0;
        public string GroupName { get; set; }
        public string UserName { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
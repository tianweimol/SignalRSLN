using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace SignalR.Server.MVC.Models
{
    /// <summary>
    /// 用户对象
    /// </summary>
    [Serializable]
    public class UserModel
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        /// <summary>
        /// 性别，默认为男
        /// </summary>
        public string Gender { get; set; } = "男";
        /// <summary>
        /// 组名
        /// </summary>
        public string GroupName { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 唯一标识用户连接的ID。每个连接都会对应一个唯一的ConnectionId，这个connectionId看起来和GUID很像
        /// </summary>
        public string ConnectionId { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
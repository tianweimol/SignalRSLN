using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using SignalR.Server.MVC.Common;
using Microsoft.AspNet.SignalR.Hubs;

namespace SignalR.Server.MVC
{
    public class MolHub : Hub
    {
        private static RedisHelper helper = new RedisHelper();
        public void Hello()
        {
            Clients.All.hello();
        }

        public void SendMessage(string name, string msg)
        {
            Clients.All.onMessage($"{name}和大家说：{msg}");
        }

        public override Task OnConnected()
        {
            helper.insertUser(Context.ConnectionId, Context.QueryString["userName"], Context.QueryString["groupName"], Context.QueryString["gender"]);
            return base.OnConnected();
        }
        public override Task OnDisconnected(bool stopCalled)
        {
            helper.RemoveUser(Context.ConnectionId, Context.QueryString["groupName"], Context.QueryString["gender"]);
            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            return base.OnReconnected();
        }
    }
}
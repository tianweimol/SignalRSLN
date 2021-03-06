﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using SignalR.Server.MVC.Common;
using SignalR.Server.MVC.Models;
using Newtonsoft.Json;

namespace SignalR.Server.MVC
{
    public class MolHub : Hub
    {
        private static RedisHelper helper = new RedisHelper();

        public void SendMessage(string name, string msg)
        {
            Clients.All.onMessage($"{name}和大家说：{msg}");
        }



        public void SendToClientByName(string Connectionid, string msg)
        {
            Clients.Client(Connectionid).onMessage($"管理员对你说：{msg}");
        }

        public void SendToClientByGroup(string group, string msg)
        {
            Clients.Group(group).onMessage($"管理员对组{group}说：{msg}");
        }

        public async Task SendToClientByNameAsync(string Connectionid, string msg)
        {
            await Task.Run(()=> Clients.Client(Connectionid).onMessage($"管理员对你说：{msg}") );
        }

        public async Task SendToClientByGroupAsync(string group, string msg)
        {
            await Task.Run(()=> {
                Clients.Group(group).onMessage($"管理员对组{group}说：{msg}");
            });
        }

        public async Task SendToClientByGroupsAsync(List<string> groups, string msg)
        {
            await Clients.Groups(groups).onMessage($"管理员对组们{groups}说：{msg}");
        }

        public override async Task OnConnected()
        {
            await helper.insertUserAsync(Context.ConnectionId, Context.QueryString["userName"], Context.QueryString["groupName"], Context.QueryString["gender"]);
            UserModel user = new UserModel()
            {
                ConnectionId = Context.ConnectionId,
                UserName = Context.QueryString["userName"],
                GroupName = Context.QueryString["groupName"],
                Gender = Context.QueryString["gender"]
            };
            await Groups.Add(Context.ConnectionId, Context.QueryString["groupName"]);
            await Clients.Group("admin组").showClients(user);
            await base.OnConnected();
        }
        public override async Task OnDisconnected(bool stopCalled)
        {
            await helper.RemoveUserAsync(Context.ConnectionId, Context.QueryString["groupName"], Context.QueryString["gender"]);
            await base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            return base.OnReconnected();
        }
    }
}
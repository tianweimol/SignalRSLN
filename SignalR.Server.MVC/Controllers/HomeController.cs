using Newtonsoft.Json;
using SignalR.Server.MVC.Common;
using SignalR.Server.MVC.Models;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SignalR.Server.MVC.Controllers
{
    public class HomeController : Controller
    {
        private static int currentGroup = 0;
        private static string currentGender = "男";
        private static int nameFlag = 0;
        private static RedisHelper helper = new RedisHelper();
        // GET: Home
public async Task<ActionResult> Index()
{
    return await Task.Run(() =>
    {
        currentGender = currentGender.Equals("男") ? "女" : "男";
        if (currentGroup < 3)
        {
            currentGroup++;
        }
        else
        {
            currentGroup = 0;
        }
        UserModel model = new UserModel()
        {
            GroupName = $"组{currentGroup}",
            Gender = currentGender,
            UserName = $"姓名{nameFlag++}"
        };
        return View(model);
    });
}


        public async Task<ActionResult> AdminIndex()
        {
            return await Task.Run(() =>
            {
                currentGender = currentGender.Equals("男") ? "女" : "男";
                if (currentGroup < 3)
                {
                    currentGroup++;
                }
                else
                {
                    currentGroup = 0;
                }
                UserModel model = new UserModel()
                {
                    GroupName = $"admin组",
                    Gender = "你猜",
                    UserName = $"我是管理员"
                };
                return View(model);
            });
        }
       
    }
}
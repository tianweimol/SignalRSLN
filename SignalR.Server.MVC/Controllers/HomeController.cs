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
        private static int currentGender = 0;
        private static int nameFlag = 0;
        private static RedisHelper helper = new RedisHelper();
        // GET: Home
        public async Task<ActionResult> Index()
        {
            currentGender = currentGender == 0 ? 1 : 0;
            if (currentGroup < 2)
            {
                currentGroup++;
            }
            else
            {
                currentGroup = 0;
            }
            UserModel model = new UserModel()
            {
                GroupName = $"组{currentGender}",
                Gender = currentGender,
                UserName = $"姓名{nameFlag++}"
            };
            return View(model);
        }
        /// <summary>
        /// 将客户端写入到redis中
        /// </summary>
        /// <param name="connectionid"></param>
        /// <param name="name"></param>
        /// <param name="groupName"></param>
        /// <param name="gender"></param>
        /// <returns></returns>
        public async Task<ActionResult> Connection(string connectionid, string userName, string groupName, string gender)
        {
            helper.insertUser(connectionid, userName, groupName,gender);
            return Content("OK");
        }
       
    }
}
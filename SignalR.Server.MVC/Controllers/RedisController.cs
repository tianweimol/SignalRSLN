using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SignalR.Server.MVC.Controllers
{
    public class RedisController : Controller
    {
        private string _redisServerIP = string.Empty;
        private string redisServiceIp
        {
            get
            {
                if (string.IsNullOrEmpty(_redisServerIP))
                {
                    _redisServerIP = System.Configuration.ConfigurationManager.AppSettings["redisServerIP"];
                }
                return _redisServerIP;
            }
        }
        private string _redisPort = string.Empty;
        private string redisPort
        {
            get
            {
                if (string.IsNullOrEmpty(_redisPort))
                {
                    _redisPort = System.Configuration.ConfigurationManager.AppSettings["redisPort"];
                }
                return _redisPort;
            }

        }
        private string _conStr = string.Empty;
        private string conStr
        {
            get
            {
                if (string.IsNullOrEmpty(_conStr))
                {
                    _conStr = $"{redisServiceIp}:{redisPort}";
                }
                return _conStr;
            }
        }
        public ActionResult Index()
        {
            using (ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(conStr))
            {
                IDatabase db = redis.GetDatabase(1);
                db.StringSet("testKey", "testValue");
            }
            return Content("Complete");
        }


        public async Task<ActionResult> GetStr()
        {
            using (ConnectionMultiplexer redis = await ConnectionMultiplexer.ConnectAsync(conStr))
            {
                IDatabase db = redis.GetDatabase(1);
                return Content(await db.StringGetAsync("testKey"));
            }
        }
    }
}
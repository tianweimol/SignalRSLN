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
        public ActionResult Index()
        {
            using (ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost:6379"))
            {
                IDatabase db = redis.GetDatabase(1);
                db.StringSet("testKey","testValue");
            }
            return Content("Complete");
        }


        public async Task<ActionResult> GetStr()
        {
            using (ConnectionMultiplexer redis = await ConnectionMultiplexer.ConnectAsync("localhost:6379"))
            {
                IDatabase db = redis.GetDatabase(1);
                return Content(await db.StringGetAsync("testKey"));                
            }
        }
    }
}
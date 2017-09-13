using Microsoft.Owin;
using Owin;
[assembly: OwinStartup(typeof(SignalR.Server.MVC.Startup))]
namespace SignalR.Server.MVC
{    
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
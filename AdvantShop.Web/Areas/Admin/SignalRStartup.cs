using AdvantShop.Web.Admin;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(SignalRStartup))]
namespace AdvantShop.Web.Admin
{
    public class SignalRStartup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR(new HubConfiguration());
        }
    }
}
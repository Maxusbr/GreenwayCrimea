//using System.Globalization;
//using System.Threading;
//using System.Threading.Tasks;
//using AdvantShop.Admin.Hubs;
//using Microsoft.AspNet.SignalR;
//using Microsoft.Owin;
//using Owin;

//[assembly: OwinStartup(typeof(SignalRStartup))]

//namespace AdvantShop.Admin.Hubs
//{
//    public class SignalRStartup
//    {
//        public void Configuration(IAppBuilder app)
//        {
//            var task = Task.Run(() => app.MapSignalR());
//            task.Wait(300);
//            if (task.IsCanceled)
//                Task.Run(() => app.MapSignalR()).Wait(300);
//            //app.MapSignalR();
//        }
//    }
//}
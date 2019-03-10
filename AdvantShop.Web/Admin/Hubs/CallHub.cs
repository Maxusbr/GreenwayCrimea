using System.Threading.Tasks;
using AdvantShop.Customers;
using AdvantShop.Security;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace AdvantShop.Admin.Hubs
{
    [HubName("callHub")]
    public class CallHub : Hub
    {
        public override Task OnConnected()
        {
            //RoleAccess.Check(CustomerContext.CustomerId)
            //AddGroups();
            return base.OnConnected();
        }

        public override Task OnReconnected()
        {
            //AddGroups();
            return base.OnReconnected();
        }

        public Task OnDisconnected()
        {
            //RemoveGroups();
            return base.OnDisconnected(true);
        }

        //public void AddGroups()
        //{
        //    //add 1st group
        //    Groups.Add(Context.ConnectionId, "foo");
        //}

        //public void RemoveGroups()
        //{
        //    //Groups.Remove(Context.ConnectionId, "");
        //}
    }
}
using System.Collections.Generic;
namespace AdvantShop.Core.Modules.Interfaces
{
    public interface IAdminSearch
    {
        List<string> SearchCustomers(string q);

        List<string> SearchOrders(string q);

        List<string> SearchProducts(string q);
    }
}

//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;

namespace AdvantShop.Orders
{
    public interface IOrderCustomer
    {
        int OrderID { get; set; }
        Guid CustomerID { get; set; }
        string CustomerIP { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string Patronymic { get; set; }
        string Email { get; set; }
        string Phone { get; set; }
        long? StandardPhone { get; set; }
    }
}
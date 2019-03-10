//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using AdvantShop.Core.Services.Crm.BusinessProcesses;

namespace AdvantShop.Customers
{
    public interface ICustomer : IBizObject
    {
        Guid Id { get; set; }

        int CustomerGroupId { get; set; }

        string Password { get; set; }

        string FirstName { get; set; }

        string LastName { get; set; }

        string Patronymic { get; set; }

        string Phone { get; set; }

        long? StandardPhone { get; set; }

        DateTime RegistrationDateTime { get; set; }

        string EMail { get; set; }

        bool SubscribedForNews { get; set; }

        long? BonusCardNumber { get; set; }

        string AdminComment { get; set; }

        int Rating { get; set; }

        Role CustomerRole { get; set; }
    }
}
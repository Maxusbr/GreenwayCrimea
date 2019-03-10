//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using AdvantShop.Core.Services.Mails;
using AdvantShop.Customers;

namespace AdvantShop.Module.UniSender.Domain
{
    public class UniSenderListMember : ISubscriber
    {
        public string Person_Id { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }
        
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public EMailRecipientType CustomerType { get; set; }
    }
}
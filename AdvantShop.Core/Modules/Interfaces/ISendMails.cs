//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using AdvantShop.Customers;

namespace AdvantShop.Core.Modules.Interfaces
{ 

    public interface ISendMails : IModule
    {
        bool SendMails(string title, string message, Services.Mails.EMailRecipientType recipientType);

        void SubscribeEmail(ISubscriber subscriber);

        void UnsubscribeEmail(string email);
    }
}
using System;

namespace AdvantShop.Core.Services.Mails
{
    /// <summary>
    /// Mail recipient type
    /// </summary>
    [Flags]
    public enum EMailRecipientType
    {
        None = 0,
        Subscriber = 1,
        OrderCustomer = 2
    }
}

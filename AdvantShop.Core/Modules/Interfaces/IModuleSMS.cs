//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Web;

namespace AdvantShop.Core.Modules.Interfaces
{
    public interface IModuleSms : IModule
    {
        void SendSms(Guid customerId, long phoneNumber, string text);

        /// <summary>
        /// for old admin area
        /// </summary>
        string RenderSendSmsButton(Guid customerId, long phoneNumber);

        /// <summary>
        /// for new admin area
        /// </summary>
        IHtmlString GetSendSmsButton(Guid customerId, long phoneNumber);
    }
}
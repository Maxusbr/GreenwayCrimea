//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;

namespace AdvantShop.Notifications
{
    public class AdminMessageBox
    {
        public List<AdminMessage> Items { get; set; }
        public string Message { get; set; }

        public AdminMessageBox()
        {
            Items = new List<AdminMessage>();
            Message = string.Empty;
        }

        public AdminMessageBox(string message)
        {
            Items = new List<AdminMessage>();
            Message = message;
        }
    }
}
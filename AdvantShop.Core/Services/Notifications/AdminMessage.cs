//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;

namespace AdvantShop.Notifications
{
    public class AdminMessage
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public int MessageType { get; set; }
        public string MessageTypeString { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime DateChange { get; set; }
        public bool Enabled { get; set; }
        public bool Viewed { get; set; }
    }
}
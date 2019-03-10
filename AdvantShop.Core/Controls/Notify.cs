//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

namespace AdvantShop.Core.Controls
{
    public enum NotifyType
    {
        Error = 0,
        Notice = 1,
        Success = 3
    }

    public class Notify
    {
        public NotifyType NotifyType { get; set; }
        public string Message { get; set; }

        public Notify(NotifyType notifyType, string message)
        {
            NotifyType = notifyType;
            Message = message;
        }

        public static string FormatMessage(NotifyType notifyType, string message)
        {
            return string.Format("<div class=\"notify-item type-{0}\">{1}<div class=\"close\"></div></div>", notifyType.ToString().ToLower(), message);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvantShop.Module.SimaLand.Service
{
    public class StatusMessage
    {
        public enum Status
        {
            Error = 0,
            Success = 1,
            Info = 2
        }

        public string Message { get; set; }
        public string BgColor { get; set; }
        public string BorderColor { get; set; }

        public StatusMessage()
        {
            Message = "";
            BgColor = "";
            BorderColor = "";
        }

        public StatusMessage(string message)
        {
            Message = message;
            BgColor = SetBgColor(Status.Info);
            BorderColor = SetBorderColor(Status.Info);
        }

        public StatusMessage(string message, Status status)
        {
            Message = message;
            BgColor = SetBgColor(status);
            BorderColor = SetBorderColor(status);
        }

        string SetBgColor(Status status)
        {
            switch (status)
            {
                case Status.Error: return "rgba(200, 0, 0, 0.3)";
                case Status.Success: return "rgba(0, 200, 0, 0.3)";
                case Status.Info: return "rgba(0, 184, 217, 0.3)";
                default:return "rgba(200, 0, 0, 0.3)";
            }
        }

        string SetBorderColor(Status status)
        {
            switch (status)
            {
                case Status.Error: return "crimson";
                case Status.Success: return "green";
                case Status.Info: return "cadetblue";
                default: return "crimson";
            }
        }
    }
}

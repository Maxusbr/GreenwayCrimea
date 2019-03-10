using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvantShop.Module.OrderNow.Models
{
    public class SaveSettings
    {
        public string text { get; set; }
        public string timeout { get; set; }
        public bool weekends { get; set; }
        public bool checkAvailability { get; set; }
        public bool showInCart { get; set; }
        public bool showInOrderConfirm { get; set; }
        public string ShowAt { get; set; }
        public string IconHeight { get; set; }
        public string ShowStart { get; set; }
        public string ShowFinish { get; set; }
        public string Ndays { get; set; }
        public bool ShowMobile { get; set; }
        public string TimeoutMessage { get; set; }
    }
}

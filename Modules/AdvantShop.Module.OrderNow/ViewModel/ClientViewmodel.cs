using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdvantShop.Module.OrderNow.Service;

namespace AdvantShop.Module.OrderNow.ViewModel
{
    public class ClientViewmodel
    {
        public string Message
        {
            get
            {
                if (Enabled)
                    return ModuleService.PrepareMessage(productid:ProductId);
                else if (!Enabled && !string.IsNullOrWhiteSpace(ModuleSettings.TimeoutMessage))
                {
                    Enabled = true;
                    return ModuleService.PrepareMessage(true,ProductId);
                }
                return string.Empty;
            }
        }

        public int ProductId { get; set; }
        public bool Enabled { get; set; }
        public string ShowAt { get; set; }
    }
}

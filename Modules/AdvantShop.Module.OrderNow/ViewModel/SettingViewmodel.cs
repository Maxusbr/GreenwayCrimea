using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdvantShop.Module.OrderNow.Service;

namespace AdvantShop.Module.OrderNow.ViewModel
{
    public class SettingViewmodel
    {
        public string OutputText
        {
            get
            {
                return ModuleSettings.Message;
            }
        }
        public bool IncludeWeekends
        {
            get
            {
                return ModuleSettings.IncludeWeekend;
            }
        }
        public bool CheckAvailability
        {
            get
            {
                return ModuleSettings.LookForAvailability;
            }
        }
        public string TimeoutTime
        {
            get
            {
                return ModuleSettings.TimeoutTime;
            }
        }
        public string Date
        {
            get
            {
                return ModuleService.GetTimeleft();
            }
        }
        public bool ShowInCart
        {
            get
            {
                return ModuleSettings.ShowInCart;
            }
        }

        public bool ShowInOrderConfirm
        {
            get
            {
                return ModuleSettings.ShowInOrderConfirm;
            }
        }
        public string ShowAt
        {
            get
            {
                return ModuleSettings.ShowAt;
            }
        }
        public bool IconUsed
        {
            get
            {
                return ModuleSettings.IconUsed;
            }
        }
        public int IconHeight
        {
            get
            {
                return ModuleSettings.IconHeight;
            }
        }

        public string ShowStart
        {
            get
            {
                return ModuleSettings.ShowStart;
            }
        }

        public string ShowFinish
        {
            get
            {
                return ModuleSettings.ShowFinish;
            }
        }

        public int Ndays
        {
            get
            {
                return ModuleSettings.Ndays;
            }
        }

        public bool ShowMobile
        {
            get
            {
                return ModuleSettings.ShowInMobile;
            }
        }

        public string OutputTimeoutText
        {
            get
            {
                return ModuleSettings.TimeoutMessage;
            }
        }
    }
}

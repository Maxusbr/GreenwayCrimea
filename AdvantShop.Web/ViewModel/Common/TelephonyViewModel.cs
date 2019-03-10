using System.Collections.Generic;
using System.Web.Mvc;
using AdvantShop.Core.Services.IPTelephony.CallBack;
using AdvantShop.Models;
using AdvantShop.Repository.Currencies;

namespace AdvantShop.ViewModel.Common
{
    public partial class TelephonyViewModel : BaseModel
    {
        public bool IsWorkTime { get; set; }
        public int TimeInterval { get; set; }
        public ECallBackShowMode ShowMode { get; set; }
    }
}
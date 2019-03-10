﻿using AdvantShop.Core.Services.Catalog;
using AdvantShop.Module.SimaLand.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvantShop.Module.SimaLand.ViewModel
{
    public class LabelViewModel
    {
        public Tag Tag { get; set; }
        public string Color {
            get
            {
                return Tag.UrlPath == "three-pay-two" ? PSLModuleSettings.ColorThreePayTwo : PSLModuleSettings.ColorMTGift;
            }
        }
    }
}
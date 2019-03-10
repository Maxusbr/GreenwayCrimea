using System;
using System.Linq;
using System.Web.Mvc;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Customers;
using AdvantShop.Diagnostics;
using AdvantShop.Module.RetailCRM;
using AdvantShop.Orders;
using AdvantShop.Web.Infrastructure.Controllers;
using System.Collections.Generic;
using AdvantShop.Core.Modules;
using AdvantShop.Modules;

namespace AdvantShop.Module.RetailCRM.Controllers
{
    public class RetailCRMController : ModuleController
    {
        public ActionResult GetScript()
        {

            var customer = CustomerContext.CurrentCustomer;
            var collectorKey = ModuleSettingsProvider.GetSettingValue<string>("CollectorKey", RetailCRMModule.ModuleStringId);
            var str = "<script type='text/javascript'> " +
                      "(function(_, r, e, t, a, i, l){ _['retailCRMObject'] = a; _[a] = _[a] || function(){ (_[a].q = _[a].q ||[]).push(arguments)}; " +
                      "_[a].l = 1 * new Date(); l = r.getElementsByTagName(e)[0]; i = r.createElement(e); i.async = !0; i.src = t; l.parentNode.insertBefore(i, l)})" +
                      "(window, document,'script','https://collector.retailcrm.pro/w.js','_rc');" +
                      "_rc('create', '" + collectorKey + "', {" +
                      (customer != null && customer.RegistredUser ? "'customerId': '" + customer.Id.ToString() + "'" : string.Empty) + "});" +
                      "_rc('send', 'pageView');" +
                      "</script> ";

            return Content(str);//, userInfo));
        }
    }
}

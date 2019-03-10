using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Filters;
using System;
using System.Web.Mvc;
using AdvantShop.Module.ReturnCustomer.Service;
using AdvantShop.Module.ReturnCustomer.Models;

namespace AdvantShop.Module.ReturnCustomer.Controllers
{
    [Module(Type = "ReturnCustomer")]
    public class RCClientController : ModuleController
    {
        string ModuleID = ReturnCustomer.ModuleStringId;

        public ActionResult Index()
        {
            return PartialView("~/modules/" + ModuleID + "/Views/Client/Index.cshtml");
        }

        public ActionResult UserAction()
        {
            var customer = Customers.CustomerContext.CurrentCustomer;

            if (customer == null || customer.CustomerRole == Customers.Role.Guest || customer.CustomerRole == Customers.Role.Administrator ||
                customer.CustomerRole == Customers.Role.Moderator || RCSettings.DisabledMailsList.Contains(customer.EMail))
            {
                return Content("");
            }

            var returnCustomerRecord = RCService.GetReturnCustomerRecord(customer.Id);
            var dateTimeNow = DateTime.Now;
            if (returnCustomerRecord == null)
            {
                returnCustomerRecord = new ReturnCustomerRecord
                {
                    CustomerID = customer.Id,
                    LastActionDate = dateTimeNow,
                    ExpirationDate = dateTimeNow.AddMinutes(RCService.timeExpiration),
                    IsNotNeedChecked = false,
                    WaitingVisit = false
                };

                RCService.AddReturnCustomerRecord(returnCustomerRecord);
            }
            else
            {
                if (returnCustomerRecord.WaitingVisit)
                {
                    returnCustomerRecord.WaitingVisit = false;
                    returnCustomerRecord.IsNotNeedChecked = false;
                }

                /*if (returnCustomerRecord.IsNotNeedChecked.HasValue && returnCustomerRecord.IsNotNeedChecked.Value)
                {
                    return Content("");
                }*/

                returnCustomerRecord.LastActionDate = dateTimeNow;

                if(returnCustomerRecord.ExpirationDate <= DateTime.Now)
                {
                    returnCustomerRecord.ExpirationDate = dateTimeNow.AddMinutes(RCService.timeExpiration);
                }

                RCService.UpdateReturnCustomerRecord(returnCustomerRecord);
            }

            return Content("");
        }
    }
}

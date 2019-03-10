using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Customers;

namespace AdvantShop.Web.Admin.ViewModels.Settings
{
    public class UsersSettingsViewModel
    {
        public UsersSettingsViewModel()
        {
            ManagersOrderConstraintList = new List<SelectListItem>();
            foreach (ManagersOrderConstraint value in Enum.GetValues(typeof(ManagersOrderConstraint)))
            {
                ManagersOrderConstraintList.Add(new SelectListItem() { Text = value.Localize(), Value = ((int)value).ToString()});
            }

            ManagersLeadConstraintList = new List<SelectListItem>();
            foreach (ManagersLeadConstraint value in Enum.GetValues(typeof(ManagersLeadConstraint)))
            {
                ManagersLeadConstraintList.Add(new SelectListItem() { Text = value.Localize(), Value = ((int)value).ToString() });
            }
        }

        public UsersViewModel UsersViewModel { get; set; }

        public ManagersOrderConstraint ManagersOrderConstraint { get; set; }
        public List<SelectListItem> ManagersOrderConstraintList { get; set; }


        public ManagersLeadConstraint ManagersLeadConstraint { get; set; }
        public List<SelectListItem> ManagersLeadConstraintList { get; set; }
    }
}

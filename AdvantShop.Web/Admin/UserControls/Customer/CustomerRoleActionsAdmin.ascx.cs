//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Linq;
using System.Web.UI.WebControls;
//using AdvantShop.Core.Caching;
using AdvantShop.Customers;
using AdvantShop.Helpers;
using System.Collections.Generic;
using AdvantShop.Saas;

namespace Admin.UserControls
{
    public partial class CustomerRoleActions : System.Web.UI.UserControl
    {
        protected string _category;

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!Visible) return;

            //_category = "";

            if(SaasDataService.IsSaasEnabled && !SaasDataService.CurrentSaasData.RoleActions)
            {
                divNotAvailableFeature.Visible = true;
                return;
            }

            Guid customerId = new Guid(Request["customerid"]);
            //var roleActionList = RoleActionService.GetRoleActions();
            var customerRoleKeysList = RoleActionService.GetCustomerRoleActionsByCustomerId(customerId);

            var dictionaryRoleActions = new Dictionary<RoleAction, bool>();
        
            foreach (RoleAction roleAction in Enum.GetValues(typeof(RoleAction)))
            {
                dictionaryRoleActions.Add(roleAction, customerRoleKeysList.Any(item => item.Role == roleAction));
            }


            //foreach (RoleActionKey key in customerRoleKeysList)
            //{
            //    var role = roleActionList.Find(r => r.Key == key);
            //    if (role != null)
            //        role.Enabled = true;
            //}

            //rprAccessSettigs.DataSource = roleActionList;
            rprAccessSettigs.DataSource = dictionaryRoleActions;
            rprAccessSettigs.DataBind();

        }

        public void SaveRole()
        {
            Guid customerId = new Guid(Request["customerid"]);

            foreach (RepeaterItem item in rprAccessSettigs.Items)
            {
                string roleActionKey = ((HiddenField)item.FindControl("hfRoleActionKey")).Value;
                bool enabled = SQLDataHelper.GetBoolean(((CheckBox)item.FindControl("chkRoleAction")).Checked);

                RoleActionService.UpdateOrInsertCustomerRoleAction(customerId, roleActionKey, enabled);
            }
        }

        //protected string RenderCategory(string category)
        //{
        //    if (category != _category)
        //    {
        //        _category = category;
        //        return String.Format("<tr><td colspan='2' style=\"font-weight: bold; padding:15px 0 5px 0;\">{0}</td></tr>", category);
        //    }

        //    return "";
        //}
    }
}
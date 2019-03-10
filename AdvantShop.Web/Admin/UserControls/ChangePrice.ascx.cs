using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using AdvantShop.Admin.UserControls;
using AdvantShop.Catalog;
using AdvantShop.Diagnostics;
using Resources;

namespace Admin.UserControls
{
    public partial class ChangePrice : System.Web.UI.UserControl
    {
        enum eAction
        {
            Decrement = 0,
            Increment = 1,
            IncBySupply = 2
        }

        private List<int> SelectedCategories
        {
            get { return (List<int>)(ViewState["SelectedCategories"] ?? (ViewState["SelectedCategories"] = new List<int>())); }
            set { ViewState["SelectedCategories"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlAction.Items.Add(new ListItem(Resource.Admin_ChangePrice_Increment, eAction.Increment.ToString()));
                ddlAction.Items.Add(new ListItem(Resource.Admin_ChangePrice_Decrement, eAction.Decrement.ToString()));
                ddlAction.Items.Add(new ListItem(Resource.Admin_ChangePrice_IncrementBySupply, eAction.IncBySupply.ToString()));
            }
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            tvCategories.tree_NodeSelected(sender, e);
            lblMessage.Visible = true;
            lblMessage.ForeColor = System.Drawing.Color.FromName("#0000ff");

            try
            {
                float val;
                if (!float.TryParse(txtValue.Text, out val))
                {
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Text = Resource.Admin_ChangePrice_Error;
                    return;
                }

                if (!SelectedCategories.Any())
                {
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Text = Resource.Admin_ChangePrice_NoCategories;
                    return;
                }

                if (ddlAction.SelectedValue == eAction.Decrement.ToString())
                {
                    ProductService.DecrementProductsPrice(val, false, SelectedCategories);
                    lblMessage.Text = string.Format(Resource.Admin_ChangePrice_Decreased, val);
                }

                if (ddlAction.SelectedValue == eAction.Increment.ToString())
                {
                    ProductService.IncrementProductsPrice(val, false, SelectedCategories);
                    lblMessage.Text = string.Format(Resource.Admin_ChangePrice_Increased, val);
                }

                if (ddlAction.SelectedValue == eAction.IncBySupply.ToString())
                {
                    ProductService.IncrementProductsPrice(val, true, SelectedCategories);
                    lblMessage.Text = string.Format(Resource.Admin_ChangePrice_IncreasedBySupply, val);
                }
                ProductService.PreCalcProductParamsMass();
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = ex.Message;
            }
        }

        protected void treeNode_Selected(object sender, TreeViewCategoryMultiSelect.TreeNodeSelectedArgs args)
        {
            SelectedCategories = args.SelectedValues.Select(int.Parse).ToList();
        }
    }
}
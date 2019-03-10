//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.SQL;
using AdvantShop.Customers;

namespace Admin.UserControls
{
    public partial class FindCustomers : System.Web.UI.UserControl
    {
        protected SqlPaging _paging;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                LoadCustomers(Request["page"].TryParseInt(1));
            }
            else
            {
                _paging = (SqlPaging)(ViewState["Paging"]);
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (CustomerContext.CurrentCustomer.CustomerRole == Role.Moderator)
            {
                _paging.Fields["CustomerRole"].Filter = new EqualFieldFilter() { ParamName = "@customerRole", Value = ((int)Role.User).ToString() };
            }

            ViewState["Paging"] = _paging;

            pageNumberer.PageCount = _paging.PageCount;
            pageNumberer.CurrentPageIndex = _paging.CurrentPageIndex;

            lvCustomers.DataSource = _paging.PageItems;
            lvCustomers.DataBind();
        }

        protected void LoadCustomers(int page)
        {
            _paging = new SqlPaging { TableName = "[Customers].[Customer]" };

            _paging.AddFieldsRange(new List<Field>
                {
                    new Field {Name = "CustomerID as ID", IsDistinct = true},
                    new Field {Name = "EMail"},
                    new Field {Name = "FirstName"},
                    new Field {Name = "LastName"},
                    new Field {Name = "CustomerRole"},
                    new Field {Name = "Phone"},
                    new Field {Name = "RegistrationDateTime"},
                    new Field {Name = "(Select ISNULL(SUM([Sum]),0) From  [Order].[Order] INNER JOIN [Order].[OrderCustomer] ON [Order].[OrderID] = [OrderCustomer].[OrderID] WHERE [CustomerId] = [Customer].[CustomerId]) as OrdersSum"}

                });

            _paging.ItemsPerPage = 8;
            _paging.CurrentPageIndex = page;

            
            ViewState["Paging"] = _paging;
        }

        protected void btnFindCustomer_Click(object sender, EventArgs e)
        {
            _paging.Fields["EMail"].Filter = !string.IsNullOrEmpty(txtSEmail.Text) ? new EqualFieldFilter() { ParamName = "@email", Value = txtSEmail.Text } : null;
            _paging.Fields["FirstName"].Filter = !string.IsNullOrEmpty(txtSFirstName.Text) ? new EqualFieldFilter() { ParamName = "@firstName", Value = txtSFirstName.Text } : null;
            _paging.Fields["LastName"].Filter = !string.IsNullOrEmpty(txtSLastName.Text) ? new EqualFieldFilter() { ParamName = "@lastName", Value = txtSLastName.Text } : null;
            _paging.Fields["Phone"].Filter = !string.IsNullOrEmpty(txtSPhone.Text) ? new EqualFieldFilter() { ParamName = "@phone", Value = txtSPhone.Text } : null;
            _paging.CurrentPageIndex = 1;
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtSEmail.Text = string.Empty;
            txtSFirstName.Text = string.Empty;
            txtSLastName.Text = string.Empty;
            txtSPhone.Text = string.Empty;
            btnFindCustomer_Click(sender, e);
        }

        protected void pn_SelectedPageChanged(object sender, EventArgs e)
        {
            _paging.CurrentPageIndex = pageNumberer.CurrentPageIndex;
        }
    }
}
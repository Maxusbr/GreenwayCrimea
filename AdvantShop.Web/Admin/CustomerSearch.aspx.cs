//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Core.SQL;
using AdvantShop.Customers;
using AdvantShop.Helpers;
using Resources;

namespace Admin
{
    public partial class CustomerSearch : AdvantShopAdminPage
    {
        #region private

        private bool _inverseSelection;
        private SqlPaging _paging;
        private InSetFieldFilter _selectionFilter;

        private List<Pair> _customerGroupList;
        private IEnumerable<Pair> CustomerGroupList
        {
            get
            {
                if (_customerGroupList != null) return _customerGroupList;
                _customerGroupList = new List<Pair>();
                foreach (var group in CustomerGroupService.GetCustomerGroupList())
                {
                    _customerGroupList.Add(new Pair(string.Format("{0} - {1}%", @group.GroupName, @group.GroupDiscount), @group.CustomerGroupId.ToString()));
                }

                return _customerGroupList;
            }
        }

        private List<ListItem> _roles;

        private List<ListItem> Roles
        {
            get
            {
                return _roles ?? (_roles = new List<ListItem>
                {
                    new ListItem(Resource.Admin_ViewCustomer_CustomerRole_User, ((int) Role.User).ToString()),
                    new ListItem(Resource.Admin_ViewCustomer_CustomerRole_Moderator,((int) Role.Moderator).ToString()),
                    new ListItem(Resource.Admin_ViewCustomer_CustomerRole_Administrator,((int) Role.Administrator).ToString()),
                    new ListItem(Resource.Admin_ViewCustomer_CustomerRoler_Guest, ((int) Role.Guest).ToString())
                });
            }
        }


        private void MsgErr(bool clean)
        {
            if (clean)
            {
                Message.Visible = false;
                Message.Text = "";
            }
            else
            {
                Message.Visible = false;
            }
        }

        private void MsgErr(string messageText)
        {
            Message.Visible = true;
            Message.Text = "<br/>" + messageText;
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            MsgErr(true);

            if (!IsPostBack)
            {
                if (Request["search"].IsNotEmpty())
                {
                    var customer = CustomerService.GetCustomerByEmail(Request["search"]);
                    if (customer != null)
                    {
                        Response.Redirect("ViewCustomer.aspx?customerID=" + customer.Id);
                        return;
                    }

                    var customerByCode = ClientCodeService.GetCustomerByCode(Request["search"], Guid.Empty);
                    if (customerByCode != null && customerByCode.Id != Guid.Empty)
                    {
                        Response.Redirect(string.Format("ViewCustomer.aspx?customerID={0}&code={1}",
                            customerByCode.Id, Request["search"]));
                        return;
                    }
                }

                if (Request["DeleteManagerForced"].IsNotEmpty())
                {
                    var managerId = Request["DeleteManagerForced"].TryParseInt();

                    var manager = ManagerService.GetManager(managerId);
                    if (manager != null)
                    {
                        ManagerService.ForcedDeleteManager(manager);
                        CustomerService.DeleteCustomer(manager.CustomerId);
                    }
                }


                _paging = new SqlPaging
                {
                    TableName = "[Customers].[Customer] " +
                                "LEFT JOIN [Customers].[Managers] ON [Customer].[ManagerId] = [Managers].[ManagerId] ",
                    ItemsPerPage = 10
                };


                _paging.AddFieldsRange(
                    new Field { Name = "[Customer].CustomerID as ID", IsDistinct = true },
                    new Field { Name = "[Customer].Email as Email" },
                    new Field { Name = "[Customer].Phone as Phone" },
                    new Field { Name = "[Customer].ManagerId" },
                    new Field { Name = "[Customer].Rating" },
                    new Field { Name = "[Customer].CustomerGroupId"},
                    new Field { Name = "(Select [Customer].Firstname + ' ' + [Customer].Lastname) as Name" },
                    new Field
                    {
                        Name = "(Select Top(1) [Order].OrderId From [Order].[Order] LEFT JOIN [Order].[OrderCustomer] ON [Order].[OrderId] = [OrderCustomer].[OrderId] " +
                        "WHERE [CustomerId] = [Customer].[CustomerId] Order by [OrderDate] Desc) as LastOrder"
                    },
                    new Field
                    {
                        Name = "(Select Top(1) [Order].Number From [Order].[Order] LEFT JOIN [Order].[OrderCustomer] ON [Order].[OrderId] = [OrderCustomer].[OrderId] " +
                        "WHERE [CustomerId] = [Customer].[CustomerId] Order by [OrderDate] Desc) as LastOrderNumber"
                    },
                    new Field
                    {
                        Name = "(Select ISNULL(SUM([Sum]),0) From  [Order].[Order] LEFT JOIN [Order].[OrderCustomer] ON [Order].[OrderID] = [OrderCustomer].[OrderId] " +
                        "WHERE [OrderCustomer].[CustomerId] = [Customer].[CustomerId] and [Order].[PaymentDate] is not null) as OrdersSum"
                    },
                    new Field
                    {
                        Name = "(Select COUNT([Order].[OrderId]) From  [Order].[Order] LEFT JOIN [Order].[OrderCustomer] ON [Order].[OrderID] = [OrderCustomer].[OrderId] " +
                        "WHERE [OrderCustomer].[CustomerId] = [Customer].[CustomerId] and [Order].[PaymentDate] is not null) as OrdersCount"
                    },
                    new Field { Name = "(Select Top(1) [City] From [Customers].[Contact] Where [Contact].[CustomerID] = [Customer].[CustomerID] ) as Location" },
                    new Field { Name = "[Customer].RegistrationDateTime as RegistrationDateTime", Sorting = SortDirection.Descending }
                    );

                if (!string.IsNullOrEmpty(Request["role"]))
                {
                    //_paging.AddField(new Field { Name = "[Customer].CustomerRole"});
                    //var fieldName = "CustomerRole = ";
                    _paging.AddField(new Field("[Customer].[CustomerRole]")
                    {
                        NotInQuery = true,
                        Filter = new EqualFieldFilter() { ParamName = "@CustomerRole", Value = ((int)Request["role"].TryParseEnum<Role>()).ToString() }
                    });

                    switch (Request["role"].TryParseEnum<Role>())
                    {
                        case Role.User:
                            lblHead.Text = Resource.Admin_MasterPageAdmin_Buyers;
                            lblSubHead.Text = Resource.Admin_CustomerSearch_SubHeader;
                            break;
                        case Role.Moderator:
                            lblHead.Text = Resource.Admin_MasterPageAdmin_Moderators;
                            lblSubHead.Text = Resource.Admin_MasterPageAdmin_Moderators;
                            break;
                        case Role.Administrator:
                            lblHead.Text = Resource.Admin_MasterPageAdmin_Administrators;
                            lblSubHead.Text = Resource.Admin_MasterPageAdmin_Administrators;
                            break;
                    }
                }

                if (!string.IsNullOrEmpty(Request["group"]))
                {
                    var group = CustomerGroupService.GetCustomerGroup(Request["group"].TryParseInt());
                    if (group != null)
                    {
                        _paging.Fields["[Customer].CustomerGroupId"].Filter = new EqualFieldFilter()
                        {
                            ParamName = "@CustomerGroupId",
                            Value = group.CustomerGroupId.ToString()
                        };
                        lblHead.Text += ", " + Resource.Admin_Customer_CustomerGroup + " " + group.GroupName;
                    }
                }

                if (SettingsCheckout.EnableManagersModule)
                {

                    _paging.AddField(new Field
                    {
                        Name =
                            "(Select [FirstName] + ' ' + [LastName] From  [Customers].[Customer]  WHERE [CustomerId] = [Managers].[CustomerId]) as ManagerName"
                    });

                    for (int i = 0; i < advCustomers.Columns.Count; i++)
                    {
                        if (string.Equals(advCustomers.Columns[i].AccessibleHeaderText, "ManagerName"))
                        {
                            advCustomers.Columns[i].Visible = true;
                            break;
                        }
                    }
                    tdManager.Visible = true;
                }
                else
                {
                    _paging.AddField(new Field("'' as ManagerName"));
                }

                if (CustomerContext.CurrentCustomer.CustomerRole == Role.Moderator)
                {
                    // we cant create filter by 2 fields
                    // result: WHERE CustomerRole = 0 or CustomerId = @rCustomerId
                    var fieldName = "CustomerRole = " + ((int)Role.User).ToString() + " or Managers.CustomerId";
                    _paging.AddField(new Field(fieldName)
                    {
                        NotInQuery = true,
                        Filter = new EqualFieldFilter() { ParamName = "@rCustomerId", Value = CustomerContext.CurrentCustomer.Id.ToString() }
                    });
                }

                advCustomers.ChangeHeaderImageUrl("arrowRegistrationDateTime", "images/arrowdown.gif");

                _paging.ItemsPerPage = 10;

                pageNumberer.CurrentPageIndex = 1;
                _paging.CurrentPageIndex = 1;
                ViewState["Paging"] = _paging;

                if (Request["search"].IsNotEmpty())
                {
                    if (Request["search"].Contains("@"))
                    {
                        txtSearchEmail.Text = Request["search"];
                    }
                    else if(Request["search"].Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", "").TryParseLong() != 0)
                    {
                        txtSearchPhone.Text = Request["search"];
                    }
                    else
                    {
                        txtSearchName.Text = Request["search"];
                    }

                    btnFilter_Click(null, null);
                }

            }
            else
            {
                _paging = (SqlPaging)(ViewState["Paging"]);
                _paging.ItemsPerPage = SQLDataHelper.GetInt(ddRowsPerPage.SelectedValue);

                if (_paging == null)
                {
                    throw (new Exception("Paging lost"));
                }

                var strIds = Request.Form["SelectedIds"];

                if (!string.IsNullOrEmpty(strIds))
                {
                    strIds = strIds.Trim();
                    var arrids = strIds.Split(' ');

                    var ids = new string[arrids.Length];
                    _selectionFilter = new InSetFieldFilter { IncludeValues = true };
                    for (int idx = 0; idx <= ids.Length - 1; idx++)
                    {
                        var t = arrids[idx];
                        if (t != "-1")
                        {
                            ids[idx] = t;
                        }
                        else
                        {
                            _selectionFilter.IncludeValues = false;
                            _inverseSelection = true;
                        }
                    }

                    _selectionFilter.Values = ids;
                }
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (advCustomers.UpdatedRow != null)
            {
                var customer = CustomerService.GetCustomer(new Guid(advCustomers.UpdatedRow["ID"]));
                if (customer != null)
                {
                    customer.CustomerGroupId = SQLDataHelper.GetInt(advCustomers.UpdatedRow["CustomerGroup"]);
                    CustomerService.UpdateCustomer(customer);
                }
            }

            DataTable data = _paging.PageItems;
            while (data.Rows.Count < 1 && _paging.CurrentPageIndex > 1)
            {
                _paging.CurrentPageIndex--;
                data = _paging.PageItems;
            }

            var clmn = new DataColumn("IsSelected", typeof(bool)) { DefaultValue = _inverseSelection };
            data.Columns.Add(clmn);
            if ((_selectionFilter != null) && (_selectionFilter.Values != null))
            {
                for (int i = 0; i <= data.Rows.Count - 1; i++)
                {
                    var intIndex = i;
                    if (Array.Exists(_selectionFilter.Values, c => c == data.Rows[intIndex]["ID"].ToString()))
                    {
                        data.Rows[i]["IsSelected"] = !_inverseSelection;
                    }
                }
            }
            if (data.Rows.Count < 1)
            {
                goToPage.Visible = false;
            }
            advCustomers.DataSource = data;
            advCustomers.DataBind();
            pageNumberer.PageCount = _paging.PageCount;
            lblFound.Text = _paging.TotalRowsCount.ToString();

            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_CustomerSearch_SubHeader));
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            if (ddSelect.SelectedIndex != 0)
            {
                if (ddSelect.SelectedIndex == 2)
                {
                    if (_selectionFilter != null)
                    {
                        _selectionFilter.IncludeValues = !_selectionFilter.IncludeValues;
                    }
                    else
                    {
                        _selectionFilter = null;
                    }
                }
                _paging.Fields["ID"].Filter = _selectionFilter;
            }
            else
            {
                _paging.Fields["ID"].Filter = null;
            }

            //----Firstname filter
            if (!string.IsNullOrEmpty(txtSearchName.Text))
            {
                var nfilter = new CompareFieldFilter { Expression = txtSearchName.Text, ParamName = "@Name" };
                _paging.Fields["Name"].Filter = nfilter;
            }
            else
            {
                _paging.Fields["Name"].Filter = null;
            }

            //----lastname filter
            if (!string.IsNullOrEmpty(txtSearchPhone.Text))
            {
                var nfilter = new CompareFieldFilter { Expression = txtSearchPhone.Text, ParamName = "@Phone" };
                _paging.Fields["Phone"].Filter = nfilter;
            }
            else
            {
                _paging.Fields["Phone"].Filter = null;
            }

            //----email filter
            if (!string.IsNullOrEmpty(txtSearchEmail.Text))
            {
                var nfilter = new CompareFieldFilter { Expression = txtSearchEmail.Text, ParamName = "@email" };
                _paging.Fields["Email"].Filter = nfilter;
            }
            else
            {
                _paging.Fields["Email"].Filter = null;
            }

            if (!string.IsNullOrEmpty(txtSearchLastOrder.Text))
            {
                var nfilter = new CompareFieldFilter { Expression = txtSearchLastOrder.Text, ParamName = "@LastOrderNumber" };
                _paging.Fields["LastOrderNumber"].Filter = nfilter;
            }
            else
            {
                _paging.Fields["LastOrderNumber"].Filter = null;
            }

            if (!string.IsNullOrEmpty(txtSearchOrdersCount.Text))
            {
                var nfilter = new CompareFieldFilter { Expression = txtSearchOrdersCount.Text, ParamName = "@OrdersCount" };
                _paging.Fields["OrdersCount"].Filter = nfilter;
            }
            else
            {
                _paging.Fields["OrdersCount"].Filter = null;
            }

            if (!string.IsNullOrWhiteSpace(txtSearchLocation.Text))
            {
                var nfilter = new CompareFieldFilter { Expression = txtSearchLocation.Text, ParamName = "@Location" };
                _paging.Fields["Location"].Filter = nfilter;
            }
            else
            {
                _paging.Fields["Location"].Filter = null;
            }

            if (!string.IsNullOrWhiteSpace(txtManager.Text))
            {
                var nfilter = new CompareFieldFilter { Expression = txtManager.Text, ParamName = "@Manager" };
                _paging.Fields["ManagerName"].Filter = nfilter;
            }
            else
            {
                _paging.Fields["ManagerName"].Filter = null;
            }


            if (!string.IsNullOrEmpty(txtSearchOrdersSum.Text))
            {
                var nfilter = new CompareFieldFilter { Expression = txtSearchOrdersSum.Text, ParamName = "@OrdersSum" };
                _paging.Fields["OrdersSum"].Filter = nfilter;
            }
            else
            {
                _paging.Fields["OrdersSum"].Filter = null;
            }

            //----RegDate filter
            if (!string.IsNullOrEmpty(txtDateFrom.Text) || !string.IsNullOrEmpty(txtDateTo.Text))
            {
                var nfilter = new DateTimeRangeFieldFilter();
                try
                {
                    nfilter.From = DateTime.Parse(txtDateFrom.Text);
                }
                catch (Exception)
                {
                    nfilter.From = DateTime.Parse("01.01.1900");
                }

                try
                {
                    nfilter.To = DateTime.Parse(txtDateTo.Text);
                }
                catch (Exception)
                {
                    nfilter.To = DateTime.MaxValue;
                }
                nfilter.ParamName = "@RegistrationDateTime";
                _paging.Fields["RegistrationDateTime"].Filter = nfilter;
            }
            else
            {
                _paging.Fields["RegistrationDateTime"].Filter = null;
            }


            pageNumberer.CurrentPageIndex = 1;
            _paging.CurrentPageIndex = 1;
        }

        protected void agv_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteCustomer")
            {
                DeleteCustomer(Guid.Parse(e.CommandArgument.ToString()));
            }
        }

        protected void lbDeleteSelected1_Click(object sender, EventArgs e)
        {
            if (!_inverseSelection)
            {
                if (_selectionFilter != null)
                {
                    foreach (var customerID in _selectionFilter.Values)
                    {
                        DeleteCustomer(customerID.TryParseGuid());
                    }
                }
            }
            else
            {
                var itemsIds = _paging.ItemsIds<string>("Convert(nvarchar(250), CustomerID) as ID");
                foreach (var customerID in itemsIds.Where(customerId => !_selectionFilter.Values.Contains(customerId.ToString())))
                {
                    var customer = CustomerService.GetCustomer(Guid.Parse(customerID));
                    if (customer != null && !customer.IsAdmin)
                    {
                        CustomerService.DeleteCustomer(Guid.Parse(customerID));
                    }
                }
            }
        }

        protected void linkGO_Click(object sender, EventArgs e)
        {
            int pagen;
            try
            {
                pagen = int.Parse(txtPageNum.Text);
            }
            catch (Exception)
            {
                pagen = -1;
            }
            if (pagen < 1 || pagen > _paging.PageCount)
                return;
            pageNumberer.CurrentPageIndex = pagen;
            _paging.CurrentPageIndex = pagen;
        }

        protected void advCustomers_Sorting(object sender, GridViewSortEventArgs e)
        {
            var arrows = new Dictionary<string, string>
                {
                    {"Name", "arrowName"},
                    {"Email", "arrowEmail"},
                    {"Phone", "arrowPhone"},
                    {"Location", "arrowLocation"},
                    {"OrdersCount", "arrowOrdersCount"},
                    {"LastOrder", "arrowLastOrder"},
                    {"OrdersSum", "arrowOrdersSum"},
                    {"ManagerName", "arrowManager"},
                    {"RegistrationDateTime", "arrowRegistrationDateTime"}
                };

            const string urlArrowUp = "images/arrowup.gif";
            const string urlArrowDown = "images/arrowdown.gif";
            const string urlArrowGray = "images/arrowdownh.gif";

            var csf = (from Field f in _paging.Fields.Values where f.Sorting.HasValue select f).First();
            var nsf = _paging.Fields[e.SortExpression];

            if (nsf.Name.Equals(csf.Name))
            {
                csf.Sorting = csf.Sorting == SortDirection.Ascending ? SortDirection.Descending : SortDirection.Ascending;
                advCustomers.ChangeHeaderImageUrl(arrows[csf.Name],
                                                  (csf.Sorting == SortDirection.Ascending ? urlArrowUp : urlArrowDown));
            }
            else
            {
                csf.Sorting = null;

                advCustomers.ChangeHeaderImageUrl(arrows[csf.Name], urlArrowGray);

                nsf.Sorting = SortDirection.Ascending;
                advCustomers.ChangeHeaderImageUrl(arrows[nsf.Name], urlArrowUp);
            }

            pageNumberer.CurrentPageIndex = 1;
            _paging.CurrentPageIndex = 1;
            UpdatePanel2.Update();
        }

        protected void pn_SelectedPageChanged(object sender, EventArgs e)
        {
            _paging.CurrentPageIndex = pageNumberer.CurrentPageIndex;
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            btnFilter_Click(sender, e);
            advCustomers.ChangeHeaderImageUrl(null, null);
        }

        protected void btnCreateCustomer_Click(object sender, EventArgs e)
        {
            Response.Redirect("CreateCustomer.aspx" + (string.IsNullOrEmpty(Request["role"]) ? string.Empty : "?role=" + Request["role"]));
        }

        protected void grid_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected bool CanDelete(Guid customerId)
        {
            var customer = CustomerService.GetCustomer(customerId);

            return customer.Id != CustomerContext.CustomerId && !customer.IsAdmin;
        }

        private void DeleteCustomer(Guid customerid)
        {
            var message = string.Empty;
            var customer = CustomerService.GetCustomer(customerid);
            var manager = ManagerService.GetManager(customer.Id);

            var canDelete = CanDelete(customerid) &&
                            (!customer.IsManager ||
                             (manager != null && ManagerService.CanDelete(manager.ManagerId, out message)));

            if (!canDelete)
            {
                if (Message.Visible)
                {
                    Message.Text += @"<br />" + message;
                }
                else
                {
                    MsgErr(message);
                }
            }
            else
            {
                CustomerService.DeleteCustomer(customerid);
            }
        }
    }
}
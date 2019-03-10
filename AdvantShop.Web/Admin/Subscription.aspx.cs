//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using AdvantShop.Configuration;
using AdvantShop.Core.Controls;
using AdvantShop.Core.SQL;
using AdvantShop.Helpers;
using AdvantShop.Customers;
using Resources;

namespace Admin
{
    public partial class SubscriptionPage : AdvantShopAdminPage
    {
        private bool _inverseSelection;
        private SqlPaging _paging;
        private InSetFieldFilter _selectionFilter;

        public SubscriptionPage()
        {
            _inverseSelection = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_Subscription_Header));

            if (!IsPostBack)
            {
                _paging = new SqlPaging { TableName = "[Customers].[Subscription]", ItemsPerPage = 10 };

                var f = new Field { Name = "Id", IsDistinct = true, Filter = _selectionFilter };
                _paging.AddField(f);


                f = new Field { Name = "Email" };
                _paging.AddField(f);

                f = new Field { Name = "Subscribe" };
                _paging.AddField(f);

                f = new Field { Name = "SubscribeDate", Sorting = SortDirection.Descending };
                _paging.AddField(f);

                f = new Field { Name = "UnsubscribeDate" };
                _paging.AddField(f);

                f = new Field { Name = "UnsubscribeReason" };
                _paging.AddField(f);

                grid.ChangeHeaderImageUrl("arrowSubscribeDate", "images/arrowup.gif");

                _paging.ItemsPerPage = 10;

                pageNumberer.CurrentPageIndex = 1;
                _paging.CurrentPageIndex = 1;
                ViewState["Paging"] = _paging;
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
                    string[] arrids = strIds.Split(' ');

                    var ids = new string[arrids.Length ];
                    _selectionFilter = new InSetFieldFilter { IncludeValues = true };
                    for (int idx = 0; idx <= ids.Length - 1; idx++)
                    {
                        int t = int.Parse(arrids[idx]);
                        if (t != -1)
                        {
                            ids[idx] = t.ToString(CultureInfo.InvariantCulture);
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

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            //-----Selection filter
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
                _paging.Fields["Id"].Filter = _selectionFilter;
            }
            else
            {
                _paging.Fields["Id"].Filter = _selectionFilter;
            }


            //----Email filter
            if (!string.IsNullOrEmpty(txtEmail.Text))
            {
                var nfilter = new CompareFieldFilter { Expression = txtEmail.Text, ParamName = "@Email" };
                _paging.Fields["Email"].Filter = nfilter;
            }
            else
            {
                _paging.Fields["Email"].Filter = null;
            }

            //---AddDate filter
            var dfilter1 = new DateTimeRangeFieldFilter { ParamName = "@SubscribeDate" };

            if (!string.IsNullOrEmpty(txtSubscribeDateFrom.Text))
            {
                DateTime? d = null;
                try
                {
                    d = DateTime.Parse(txtSubscribeDateFrom.Text);
                }
                catch (Exception)
                {
                }
                if (d.HasValue)
                {
                    var dt = new DateTime(d.Value.Year, d.Value.Month, d.Value.Day, 0, 0, 0, 0);
                    dfilter1.From = dt;
                }
            }

            if (!string.IsNullOrEmpty(txtSubscribeDateTo.Text))
            {
                DateTime? d = null;
                try
                {
                    d = DateTime.Parse(txtSubscribeDateTo.Text);
                }
                catch (Exception)
                {
                }
                if (d.HasValue)
                {
                    var dt = new DateTime(d.Value.Year, d.Value.Month, d.Value.Day, 23, 59, 59, 99);
                    dfilter1.To = dt;
                }
            }

            if (dfilter1.From.HasValue || dfilter1.To.HasValue)
            {
                _paging.Fields["SubscribeDate"].Filter = dfilter1;
            }
            else
            {
                _paging.Fields["SubscribeDate"].Filter = null;
            }

            //---AddDate filter
            var dfilter2 = new DateTimeRangeFieldFilter { ParamName = "@UnsubscribeDate" };

            if (!string.IsNullOrEmpty(txtUnsubscribeDateFrom.Text))
            {
                DateTime? d = null;
                try
                {
                    d = DateTime.Parse(txtUnsubscribeDateFrom.Text);
                }
                catch (Exception)
                {
                }
                if (d.HasValue)
                {
                    var dt = new DateTime(d.Value.Year, d.Value.Month, d.Value.Day, 0, 0, 0, 0);
                    dfilter2.From = dt;
                }
            }

            if (!string.IsNullOrEmpty(txtUnsubscribeDateTo.Text))
            {
                DateTime? d = null;
                try
                {
                    d = DateTime.Parse(txtUnsubscribeDateTo.Text);
                }
                catch (Exception)
                {
                }
                if (d.HasValue)
                {
                    var dt = new DateTime(d.Value.Year, d.Value.Month, d.Value.Day, 23, 59, 59, 99);
                    dfilter2.To = dt;
                }
            }

            if (dfilter2.From.HasValue || dfilter2.To.HasValue)
            {
                _paging.Fields["UnsubscribeDate"].Filter = dfilter2;
            }
            else
            {
                _paging.Fields["UnsubscribeDate"].Filter = null;
            }


            //----Enable filter
            if (ddSubscribe.SelectedIndex != 0)
            {
                var enableFilter = new EqualFieldFilter { ParamName = "@Subscribe" };
                if (ddSubscribe.SelectedIndex == 1)
                {
                    enableFilter.Value = "1";
                }
                if (ddSubscribe.SelectedIndex == 2)
                {
                    enableFilter.Value = "0";
                }
                _paging.Fields["Subscribe"].Filter = enableFilter;
            }
            else
            {
                _paging.Fields["Subscribe"].Filter = null;
            }

            pageNumberer.CurrentPageIndex = 1;
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            btnFilter_Click(sender, e);
            grid.ChangeHeaderImageUrl(null, null);
        }

        protected void pn_SelectedPageChanged(object sender, EventArgs e)
        {
            _paging.CurrentPageIndex = pageNumberer.CurrentPageIndex;
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
            if (pagen >= 1 && pagen <= _paging.PageCount)
            {
                pageNumberer.CurrentPageIndex = pagen;
                _paging.CurrentPageIndex = pagen;
            }
        }

        protected void lbDeleteSelected_Click(object sender, EventArgs e)
        {
            if ((_selectionFilter != null) && (_selectionFilter.Values != null))
            {
                if (!_inverseSelection)
                {
                    foreach (var id in _selectionFilter.Values)
                    {
                        SubscriptionService.DeleteSubscription(SQLDataHelper.GetInt(id));
                    }
                }
                else
                {
                    var itemsIds = _paging.ItemsIds<int>("ID");
                    foreach (int id in itemsIds.Where(iId => !_selectionFilter.Values.Contains(iId.ToString(CultureInfo.InvariantCulture))))
                    {
                        SubscriptionService.DeleteSubscription(id);
                    }
                }
            }
        }

        protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteUser")
            {
                SubscriptionService.DeleteSubscription((int.Parse(e.CommandArgument.ToString())));
            }
        }

        protected void grid_Sorting(object sender, GridViewSortEventArgs e)
        {
            var arrows = new Dictionary<string, string>
                {
                    {"ID", "arrowID"},
                    {"Email", "arrowEmail"},
                    {"Subscribe", "arrowSubscribe"},
                    {"SubscribeDate", "arrowSubscribeDate"},
                    {"UnsubscribeDate", "arrowUnsubscribeDate"},
                    {"UnsubscribeReason", "arrowUnsubscribeReason"}
                };
            const string urlArrowUp = "images/arrowup.gif";
            const string urlArrowDown = "images/arrowdown.gif";
            const string urlArrowGray = "images/arrowdownh.gif";


            Field csf = (from Field f in _paging.Fields.Values where f.Sorting.HasValue select f).First();
            Field nsf = _paging.Fields[e.SortExpression];

            if (nsf.Name.Equals(csf.Name))
            {
                csf.Sorting = csf.Sorting == SortDirection.Ascending ? SortDirection.Descending : SortDirection.Ascending;
                grid.ChangeHeaderImageUrl(arrows[csf.Name],
                                          (csf.Sorting == SortDirection.Ascending ? urlArrowUp : urlArrowDown));
            }
            else
            {
                csf.Sorting = null;
                grid.ChangeHeaderImageUrl(arrows[csf.Name], urlArrowGray);

                nsf.Sorting = SortDirection.Ascending;
                grid.ChangeHeaderImageUrl(arrows[nsf.Name], urlArrowUp);
            }

            pageNumberer.CurrentPageIndex = 1;
            _paging.CurrentPageIndex = 1;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (grid.UpdatedRow != null)
            {
                var subscribe = false;
                var id = 0;

                if (bool.TryParse(grid.UpdatedRow["Subscribe"], out subscribe)
                    && int.TryParse(grid.UpdatedRow["ID"], out id))
                {
                    if (subscribe)
                    {
                        SubscriptionService.Subscribe(id);
                    }
                    else
                    {
                        SubscriptionService.Unsubscribe(id);
                    }
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
                    int intIndex = i;
                    if (Array.Exists(_selectionFilter.Values, c => c == data.Rows[intIndex]["Id"].ToString()))
                    {
                        data.Rows[i]["IsSelected"] = !_inverseSelection;
                    }
                }
            }

            if (data.Rows.Count < 1)
            {
                goToPage.Visible = false;
            }

            grid.DataSource = data;
            grid.DataBind();


            pageNumberer.PageCount = _paging.PageCount;
            lblFound.Text = _paging.TotalRowsCount.ToString(CultureInfo.InvariantCulture);
        }

        protected void btnAdmin_SubscriptionReg_Click(object sender, EventArgs e)
        {
            Response.Redirect("Subscription.aspx");
        }

        protected void btnAdmin_Subscription_DeactivateReason_Click(object sender, EventArgs e)
        {
            Response.Redirect("Subscription_DeactivateReason.aspx");
        }
    }
}
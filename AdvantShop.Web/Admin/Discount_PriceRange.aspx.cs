//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using AdvantShop.Configuration;
using AdvantShop.Core.Caching;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Core.SQL;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;
using Resources;

namespace Admin
{
    partial class DiscountsPriceRange : AdvantShopAdminPage
    {
        private SqlPaging _paging;
        private InSetFieldFilter _selectionFilter;
        private bool _inverseSelection = false;


        public DiscountsPriceRange()
        {
            PreRender += Page_PreRender;
            Load += Page_Load;
        }


        protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "DeleteDiscount")
                {
                    DeleteOrderPriceDiscount(SQLDataHelper.GetInt(e.CommandArgument));
                }
                if (e.CommandName == "AddRange")
                {
                    SQLDataAccess.ExecuteNonQuery("INSERT INTO [Order].[OrderPriceDiscount] (PriceRange, PercentDiscount) VALUES (@Range, @Discount)",
                                                  CommandType.Text,
                                                  new SqlParameter("@Range", ((TextBox)grid.FooterRow.FindControl("txtNewPriceRange")).Text.TryParseDecimal()),
                                                  new SqlParameter("@Discount", ((TextBox)grid.FooterRow.FindControl("txtNewPercentDiscount")).Text.TryParseDecimal())
                        );
                    grid.ShowFooter = false;
                    grid.DataBind();

                }
                if (e.CommandName == "CancelAdd")
                {
                    grid.ShowFooter = false;
                }
                CacheManager.Remove(CacheNames.GetOrderPriceDiscountCacheObjectName());
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
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
            Message.Text = @"<br/>" + messageText;

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Message.Visible = false;
            MsgErr(true);

            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_Discount_PriceRange_SubHeader));

            if (!IsPostBack)
            {
                chkModuleEnabled.Checked = SettingsCheckout.EnableDiscountModule;
            }


            if (!IsPostBack)
            {
                _paging = new SqlPaging { TableName = "[Order].[OrderPriceDiscount]", ItemsPerPage = 10 };

                var f = new Field { Name = "OrderPriceDiscountID as ID" };

                //f.IsDistinct = True
                _paging.AddField(f);

                f = new Field { Name = "PriceRange", Sorting = SortDirection.Ascending };
                _paging.AddField(f);

                f = new Field { Name = "PercentDiscount" };
                _paging.AddField(f);

                grid.ChangeHeaderImageUrl("arrowPriceRange", "images/arrowup.gif");

                _paging.ItemsPerPage = 10;

                pageNumberer.CurrentPageIndex = 1;
                _paging.CurrentPageIndex = 1;
                ViewState["Paging"] = _paging;
            }
            else
            {
                _paging = (SqlPaging)ViewState["Paging"];
                _paging.ItemsPerPage = SQLDataHelper.GetInt(ddRowsPerPage.SelectedValue);

                if (_paging == null)
                {
                    throw new Exception("Paging lost");
                }

                string strIds = Request.Form["SelectedIds"];

                if (!string.IsNullOrEmpty(strIds))
                {
                    strIds = strIds.Trim();
                    string[] arrids = strIds.Split(' ');

                    var ids = new string[arrids.Length];
                    _selectionFilter = new InSetFieldFilter { IncludeValues = true };
                    for (int idx = 0; idx <= ids.Length - 1; idx++)
                    {
                        string t = arrids[idx];
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


        protected void btnReset_Click(object sender, EventArgs e)
        {
            btnFilter_Click(sender, e);
            grid.ChangeHeaderImageUrl(null, null);

        }
        protected void grid_Sorting(object sender, GridViewSortEventArgs e)
        {
            var arrows = new Dictionary<string, string>
                {
                    {"PriceRange", "arrowPriceRange"},
                    {"PercentDiscount", "arrowPercentDiscount"}
                };

            const string urlArrowUp = "images/arrowup.gif";
            const string urlArrowDown = "images/arrowdown.gif";
            const string urlArrowGray = "images/arrowdownh.gif";

            var csf = (from Field f in _paging.Fields.Values where f.Sorting.HasValue select f).First();
            var nsf = new Field();
            nsf = _paging.Fields[e.SortExpression];
            if (nsf.Name.Equals(csf.Name))
            {
                csf.Sorting = csf.Sorting == SortDirection.Ascending ? SortDirection.Descending : SortDirection.Ascending;
                grid.ChangeHeaderImageUrl(arrows[csf.Name], csf.Sorting == SortDirection.Ascending ? urlArrowUp : urlArrowDown);
            }
            else
            {
                csf.Sorting = null;
                //If Not csf.Name.Contains("SortOrder") Then
                grid.ChangeHeaderImageUrl(arrows[csf.Name], urlArrowGray);
                //End If

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
                var discount = grid.UpdatedRow["PercentDiscount"].TryParseDecimal();

                if (grid.UpdatedRow["PriceRange"].IsDecimal() && discount >= 0 && discount < 100)
                {
                    SQLDataAccess.ExecuteNonQuery("UPDATE [Order].[OrderPriceDiscount] SET PriceRange = @PriceRange, PercentDiscount = @PercentDiscount WHERE (OrderPriceDiscountID = @OrderPriceDiscountID)",
                                                  CommandType.Text,
                                                  new SqlParameter("@PriceRange", grid.UpdatedRow["PriceRange"].TryParseDecimal()),
                                                  new SqlParameter("@PercentDiscount", grid.UpdatedRow["PercentDiscount"].TryParseDecimal()),
                                                  new SqlParameter("@OrderPriceDiscountID", grid.UpdatedRow["ID"]));
                }
                CacheManager.Remove(CacheNames.GetOrderPriceDiscountCacheObjectName());
            }

            DataTable data = _paging.PageItems;
            while (data.Rows.Count < 1 & _paging.CurrentPageIndex > 1)
            {
                _paging.CurrentPageIndex -= 1;
                data = _paging.PageItems;
            }

            var clmn = new DataColumn("IsSelected", typeof(bool)) { DefaultValue = _inverseSelection };
            data.Columns.Add(clmn);
            if (_selectionFilter != null && _selectionFilter.Values != null)
            {
                for (int i = 0; i <= data.Rows.Count - 1; i++)
                {
                    Int32 intIndex = i;
                    if (Array.Exists<string>(_selectionFilter.Values, (string c) => c == data.Rows[intIndex]["ID"].ToString()))
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
            lblFound.Text = _paging.TotalRowsCount.ToString();
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
                        //New InSetFieldFilter()
                        //_SelectionFilter.IncludeValues = True
                    }
                }
                _paging.Fields["ID"].Filter = _selectionFilter;
            }
            else
            {
                _paging.Fields["ID"].Filter = null;
            }

            //----UserName filter 
            if (!string.IsNullOrEmpty(txtUserName.Text))
            {
                var userfilter = new CompareFieldFilter
                    {
                        ParamName = "@PriceRange",
                        Expression = txtUserName.Text
                    };
                _paging.Fields["PriceRange"].Filter = userfilter;
            }
            else
            {
                _paging.Fields["PriceRange"].Filter = null;
            }

            //----Text filter 
            if (!string.IsNullOrEmpty(txtText.Text))
            {
                var textFilter = new CompareFieldFilter
                    {
                        ParamName = "@PercentDiscount",
                        Expression = txtText.Text
                    };
                _paging.Fields["PercentDiscount"].Filter = textFilter;
            }
            else
            {
                _paging.Fields["PercentDiscount"].Filter = null;
            }

            pageNumberer.CurrentPageIndex = 1;
            _paging.CurrentPageIndex = 1;

        }
        protected void pn_SelectedPageChanged(object sender, EventArgs e)
        {
            _paging.CurrentPageIndex = pageNumberer.CurrentPageIndex;
        }

        protected void lbDeleteSelected_Click(object sender, EventArgs e)
        {
            if ((_selectionFilter != null) && (_selectionFilter.Values != null))
            {
                if (!_inverseSelection)
                {
                    foreach (var id in _selectionFilter.Values)
                    {
                        DeleteOrderPriceDiscount(SQLDataHelper.GetInt(id));
                    }
                }
                else
                {
                    var itemsIds = _paging.ItemsIds<int>("OrderPriceDiscountID as ID");
                    foreach (int id in itemsIds.Where(cId => !_selectionFilter.Values.Contains(cId.ToString(CultureInfo.InvariantCulture))))
                    {
                        DeleteOrderPriceDiscount(id);
                    }
                }
            }
        }

        private void DeleteOrderPriceDiscount(int id)
        {
            SQLDataAccess.ExecuteNonQuery("DELETE FROM [Order].[OrderPriceDiscount] WHERE (OrderPriceDiscountID = @OrderPriceDiscountID)",
                                          CommandType.Text, new SqlParameter("@OrderPriceDiscountID", id));

            CacheManager.Remove(CacheNames.GetOrderPriceDiscountCacheObjectName());
        }
 
        protected void chkModuleEnabled_CheckedChanged(object sender, EventArgs e)
        {
            SettingsCheckout.EnableDiscountModule = chkModuleEnabled.Checked;
            CacheManager.Remove(CacheNames.GetOrderPriceDiscountCacheObjectName());
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            grid.ShowFooter = true;
        }
    }
}
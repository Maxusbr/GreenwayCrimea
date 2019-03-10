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
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Controls;
using AdvantShop.Core.SQL;
using AdvantShop.Helpers;
using Resources;

namespace Admin
{
    public partial class Coupons : AdvantShopAdminPage
    {
        private bool _inverseSelection;
        private SqlPaging _paging;
        private InSetFieldFilter _selectionFilter;

        protected void Page_Load(object sender, EventArgs e)
        {
            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_Coupons_Header));

            if (!IsPostBack)
            {
                _paging = new SqlPaging
                    {
                        TableName = "[Catalog].[Coupon]",
                        ItemsPerPage = 10
                    };

                _paging.AddFieldsRange(new List<Field>
                    {
                        new Field
                            {
                                Name = "CouponID as ID",
                                IsDistinct = true
                            },
                        new Field {Name = "Code"},
                        new Field {Name = "Type"},
                        new Field {Name = "Value"},
                        new Field {Name = "AddingDate", Sorting = SortDirection.Descending},
                        new Field {Name = "ExpirationDate"},
                        new Field {Name = "PossibleUses"},
                        new Field {Name = "ActualUses"},
                        new Field {Name = "Enabled"},
                        new Field {Name = "MinimalOrderPrice"},
                    });

                grid.ChangeHeaderImageUrl("arrowAddingDate", "images/arrowdown.gif");

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

                string strIds = Request.Form["SelectedIds"];

                if (!string.IsNullOrEmpty(strIds))
                {
                    strIds = strIds.Trim();
                    string[] arrids = strIds.Split(' ');

                    _selectionFilter = new InSetFieldFilter();
                    if (arrids.Contains("-1"))
                    {
                        _selectionFilter.IncludeValues = false;
                        _inverseSelection = true;
                    }
                    else
                    {
                        _selectionFilter.IncludeValues = true;
                    }
                    _selectionFilter.Values = arrids.Where(id => id != "-1").ToArray();
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
                }
                _paging.Fields["ID"].Filter = _selectionFilter;
            }
            else
            {
                _paging.Fields["ID"].Filter = null;
            }

            //----Name filter
            _paging.Fields["Code"].Filter = !string.IsNullOrEmpty(txtCode.Text) ? new CompareFieldFilter { Expression = txtCode.Text, ParamName = "@txtCode" } : null;

            _paging.Fields["Type"].Filter = (ddlTypeFilter.SelectedValue != "any")
                                                ? new EqualFieldFilter { ParamName = "@Type", Value = ddlTypeFilter.SelectedValue } : null;

            _paging.Fields["Value"].Filter = !string.IsNullOrEmpty(txtValue.Text)
                                                 ? new EqualFieldFilter { ParamName = "@Value", Value = txtValue.Text } : null;

            _paging.Fields["Enabled"].Filter = (ddlEnabled.SelectedValue != "any")
                                                   ? new EqualFieldFilter { ParamName = "@Enabled", Value = ddlEnabled.SelectedValue } : null;

            _paging.Fields["MinimalOrderPrice"].Filter = !string.IsNullOrEmpty(txtMinimalPrice.Text)
                                                             ? new EqualFieldFilter { ParamName = "@MinimalOrderPrice", Value = txtMinimalPrice.Text } : null;

            pageNumberer.CurrentPageIndex = 1;
            _paging.CurrentPageIndex = 1;
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
                        CouponService.DeleteCoupon(SQLDataHelper.GetInt(id));
                    }
                }
                else
                {
                    var itemsIds = _paging.ItemsIds<int>("CouponID as ID");
                    foreach (int couponeId in itemsIds.Where(cId => !_selectionFilter.Values.Contains(cId.ToString(CultureInfo.InvariantCulture))))
                    {
                        CouponService.DeleteCoupon(couponeId);
                    }
                }
            }
        }

        protected void lbSetActive_Click(object sender, EventArgs e)
        {
            if ((_selectionFilter != null) && (_selectionFilter.Values != null))
            {
                if (!_inverseSelection)
                {
                    foreach (var couponeId in _selectionFilter.Values)
                    {
                        CouponService.SetCouponActivity(SQLDataHelper.GetInt(couponeId), true);
                    }
                }
                else
                {
                    var itemsIds = _paging.ItemsIds<int>("CouponID as ID");
                    foreach (int couponeId in itemsIds.Where(cId => !_selectionFilter.Values.Contains(cId.ToString(CultureInfo.InvariantCulture))))
                    {
                        CouponService.SetCouponActivity(couponeId, true);
                    }
                }
            }
        }
    
        protected void lbSetDeactive_Click(object sender, EventArgs e)
        {
            if ((_selectionFilter != null) && (_selectionFilter.Values != null))
            {
                if (!_inverseSelection)
                {
                    foreach (var couponeId in _selectionFilter.Values)
                    {
                        CouponService.SetCouponActivity(SQLDataHelper.GetInt(couponeId), false);
                    }
                }
                else
                {
                    var itemsIds = _paging.ItemsIds<int>("CouponID as ID");
                    foreach (int couponeId in itemsIds.Where(cId => !_selectionFilter.Values.Contains(cId.ToString(CultureInfo.InvariantCulture))))
                    {
                        CouponService.SetCouponActivity(couponeId, false);
                    }
                }
            }
        }

        protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteCoupon")
            {
                CouponService.DeleteCoupon(SQLDataHelper.GetInt(e.CommandArgument));
            }
        }

        protected void grid_Sorting(object sender, GridViewSortEventArgs e)
        {
            var arrows = new Dictionary<string, string>
                {
                    {"Code", "arrowCode"},
                    {"Type", "arrowType"},
                    {"Value", "arrowValue"},
                    {"AddingDate", "arrowAddingDate"},
                    {"ExpirationDate", "arrowExpirationDate"},
                    {"Enabled", "arrowEnabled"},
                    {"MinimalOrderPrice", "arrowMinimalOrderPrice"}
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
                var type = (CouponType)Enum.Parse(typeof(CouponType), grid.UpdatedRow["Type"]);
                float value = 0;
                float minimalPrice = 0;
                if (!(!float.TryParse(grid.UpdatedRow["Value"], out value) || (type == CouponType.Percent && value > 100) || value<=0 || !float.TryParse(grid.UpdatedRow["MinimalOrderPrice"], out minimalPrice)))
                {
  
                    Coupon coupon = CouponService.GetCoupon(SQLDataHelper.GetInt(grid.UpdatedRow["ID"]));
                    coupon.Code = grid.UpdatedRow["Code"];
                    coupon.Type = type;
                    coupon.Enabled = SQLDataHelper.GetBoolean(grid.UpdatedRow["Enabled"]);
                    coupon.Value = value;
                    coupon.MinimalOrderPrice = minimalPrice;
                    CouponService.UpdateCoupon(coupon);
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
                    if (Array.Exists(_selectionFilter.Values, c => c == (data.Rows[intIndex]["ID"]).ToString()))
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

        protected void ddl_DataBound(object sender, EventArgs e)
        {
            ((DropDownList)sender).Items.Insert(0, new ListItem(Resource.Admin_Catalog_Any, "any"));
        }

        protected void grid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
                ((DropDownList)e.Row.FindControl("ddlType")).SelectedValue =
                    ((DataRowView)e.Row.DataItem)["Type"].ToString();
        }
    }
}
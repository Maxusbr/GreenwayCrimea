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
using AdvantShop;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Core.SQL;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;
using AdvantShop.Taxes;

namespace Admin
{
    public partial class Taxes : AdvantShopAdminPage
    {
        SqlPaging _paging;
        InSetFieldFilter _selectionFilter;
        bool _inverseSelection;

        public Taxes()
        {
            _inverseSelection = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SetMeta(string.Format("{0} - {1}", AdvantShop.Configuration.SettingsMain.ShopName, Resources.Resource.Admin_Taxes_Header));

            if (!IsPostBack)
            {
                _paging = new SqlPaging { TableName = "[Catalog].[Tax]", ItemsPerPage = 10 };

                var f = new Field { Name = "TaxID as ID", IsDistinct = true };
                _paging.AddField(f);
                
                f = new Field { Name = "Name", Sorting = SortDirection.Ascending };
                _paging.AddField(f);

                f = new Field { Name = "TaxType" };
                _paging.AddField(f);

                f = new Field { Name = "Enabled" };
                _paging.AddField(f);
                
                //f = new Field { Name = "ShowInPrice" };
                //_paging.AddField(f);

                f = new Field { Name = "Rate" };
                _paging.AddField(f);


                grid.ChangeHeaderImageUrl("arrowName", "images/arrowup.gif");

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

                    var ids = new string[arrids.Length];
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
            if (String.CompareOrdinal(ddSelect.SelectedIndex.ToString(CultureInfo.InvariantCulture), "0") != 0)
            {

                if (String.CompareOrdinal(ddSelect.SelectedIndex.ToString(CultureInfo.InvariantCulture), "2") == 0)
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

            //----Name filter
            if (!string.IsNullOrEmpty(txtName.Text))
            {
                var nfilter = new CompareFieldFilter { Expression = txtName.Text, ParamName = "@Name" };
                _paging.Fields["Name"].Filter = nfilter;
            }
            else
            {
                _paging.Fields["Name"].Filter = null;
            }

            //----Enabled filter
            if (ddlEnabled.SelectedIndex != 0)
            {

                var efilter = new EqualFieldFilter { ParamName = "@Enabled" };
                if (ddlEnabled.SelectedIndex == 1)
                {
                    efilter.Value = "1";
                }
                if (ddlEnabled.SelectedIndex == 2)
                {
                    efilter.Value = "0";
                }
                _paging.Fields["Enabled"].Filter = efilter;
            }
            else
            {
                _paging.Fields["Enabled"].Filter = null;
            }


            //----ShowInPrice filter
            if (ddlEnabled.SelectedIndex != 0)
            {

                var efilter = new EqualFieldFilter { ParamName = "@ShowInPrice" };
                if (ddlShowInPrice.SelectedIndex == 1)
                {
                    efilter.Value = "1";
                }
                if (ddlShowInPrice.SelectedIndex == 2)
                {
                    efilter.Value = "0";
                }
                _paging.Fields["ShowInPrice"].Filter = efilter;
            }
            else
            {
                _paging.Fields["ShowInPrice"].Filter = null;
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


        //Delete!!
        protected void lbDeleteSelected_Click(object sender, EventArgs e)
        {
            if ((_selectionFilter != null) && (_selectionFilter.Values != null))
            {
                if (!_inverseSelection)
                {
                    foreach (var id in _selectionFilter.Values)
                    {
                        TaxService.DeleteTax(SQLDataHelper.GetInt(id));
                    }
                }
                else
                {
                    var itemsIds = _paging.ItemsIds<int>("TaxID as ID");
                    foreach (int tax in itemsIds.Where(tax => !_selectionFilter.Values.Contains(tax.ToString(CultureInfo.InvariantCulture))))
                    {
                        TaxService.DeleteTax(tax);
                    }
                }
            }
        }

        protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                TaxService.DeleteTax(int.Parse((string)e.CommandArgument));
            }
            if (e.CommandName == "AddTax")
            {
                GridViewRow footer = grid.FooterRow;
                string name = ((TextBox)footer.FindControl("txtNewName")).Text;
                float rate = ((TextBox)footer.FindControl("txtNewRate")).Text.TryParseFloat();
                bool enabled = SQLDataHelper.GetBoolean(((DropDownList)footer.FindControl("ddlNewEnabled")).SelectedValue);
                var type = (TaxType)SQLDataHelper.GetInt(((DropDownList) footer.FindControl("ddlNewTaxType")).SelectedValue);
                //bool showInPrice = SQLDataHelper.GetBoolean(((DropDownList)footer.FindControl("ddlNewShowInPrice")).SelectedValue);

                if (name.Trim().Length != 0)
                {
                    var tax = new TaxElement
                    {
                        Enabled = enabled,
                        Rate = rate,
                        Name = name,
                        ShowInPrice = true,
                        TaxType = type
                    };
                    TaxService.AddTax(tax);
                }
                grid.ShowFooter = false;
            }
        }

        protected void grid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var ddl = (DropDownList)e.Row.FindControl("ddlTaxType");

                ddl.DataSource =
                    Enum.GetValues(typeof(TaxType))
                        .Cast<TaxType>()
                        .Select(x => new {Text = x.Localize(), Value = ((int) x).ToString()}).ToList();
                ddl.DataBind();
                ddl.SelectedValue = ((DataRowView) e.Row.DataItem)["TaxType"].ToString();
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                var ddl = (DropDownList) e.Row.FindControl("ddlNewTaxType");
                if (ddl != null)
                {
                    ddl.DataSource =
                        Enum.GetValues(typeof(TaxType))
                            .Cast<TaxType>()
                            .Select(x => new {Text = x.Localize(), Value = ((int) x).ToString()}).ToList();
                    ddl.DataBind();
                }
            }
        }

        protected void grid_Sorting(object sender, GridViewSortEventArgs e)
        {
            var arrows = new Dictionary<string, string>
                {
                    {"Name", "arrowName"},
                    {"Enabled", "arrowEnabled"},
                    {"ShowInPrice", "arrowShowInPrice"},
                    {"Rate", "arrowRate"}
                };
            const string urlArrowUp = "images/arrowup.gif";
            const string urlArrowDown = "images/arrowdown.gif";
            const string urlArrowGray = "images/arrowdownh.gif";


            Field csf = (from Field f in _paging.Fields.Values where f.Sorting.HasValue select f).First();
            Field nsf = _paging.Fields[e.SortExpression];

            if (nsf.Name.Equals(csf.Name))
            {
                csf.Sorting = csf.Sorting == SortDirection.Ascending ? SortDirection.Descending : SortDirection.Ascending;
                grid.ChangeHeaderImageUrl(arrows[csf.Name], (csf.Sorting == SortDirection.Ascending ? urlArrowUp : urlArrowDown));
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
                TaxElement tax = TaxService.GetTax(grid.UpdatedRow["ID"].TryParseInt());
                if (tax != null)
                {
                    tax.Name = grid.UpdatedRow["Name"];
                    tax.Enabled = grid.UpdatedRow["Enable"].TryParseBool();
                    tax.TaxType = (TaxType) grid.UpdatedRow["TaxType"].TryParseInt();
                    //tax.ShowInPrice = grid.UpdatedRow["DoShowInPrice"].TryParseBool();
                    tax.Rate = grid.UpdatedRow["Rate"].TryParseFloat();

                    TaxService.UpdateTax(tax);

                    var isDefault = grid.UpdatedRow["Default"].TryParseBool();
                    if (isDefault)
                        SettingsCatalog.DefaultTaxId = tax.TaxId;
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

            grid.DataSource = data;
            grid.DataBind();

            pageNumberer.PageCount = _paging.PageCount;
            lblFound.Text = _paging.TotalRowsCount.ToString();
        }

        protected void grid_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
        }

        protected void btnAddTax_Click(object sender, EventArgs e)
        {
            grid.ShowFooter = true;
        }

        protected string GetTaxType(int type)
        {
            return ((TaxType) type).Localize();
        }

        protected bool IsDefault(int taxId)
        {
            return taxId == SettingsCatalog.DefaultTaxId;
        }

    }
}

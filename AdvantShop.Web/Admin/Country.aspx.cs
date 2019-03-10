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
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Core.SQL;
using AdvantShop.Helpers;
using AdvantShop.Repository;

namespace Admin
{
    public partial class Countries : AdvantShopAdminPage
    {
        SqlPaging _paging;
        InSetFieldFilter _selectionFilter;
        bool _inverseSelection;

        public Countries()
        {
            _inverseSelection = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SetMeta(string.Format("{0} - {1}", AdvantShop.Configuration.SettingsMain.ShopName, Resources.Resource.Admin_Country_SubHeader));

            if (!IsPostBack)
            {
                _paging = new SqlPaging { TableName = "[Customers].[Country]", ItemsPerPage = 20 };

                _paging.AddFieldsRange(
                    new List<Field>
                    {
                        new Field {Name = "CountryID as ID", IsDistinct = true},
                        new Field {Name = "SortOrder", Sorting = SortDirection.Descending},
                        new Field {Name = "CountryName", Sorting = SortDirection.Ascending},
                        new Field {Name = "CountryISO2"},
                        new Field {Name = "CountryISO3"},
                        new Field {Name = "DisplayInPopup"},
                        new Field {Name = "DialCode"},

                    });

                grid.ChangeHeaderImageUrl("arrowSortOrder", "images/arrowdown.gif");
            
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
                    int t;
                    _selectionFilter = new InSetFieldFilter { IncludeValues = true };
                    for (int idx = 0; idx <= ids.Length - 1; idx++)
                    {
                        t = int.Parse(arrids[idx]);
                        if (t != -1)
                        {
                            ids[idx] = t.ToString();
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
                _paging.Fields["CountryName"].Filter = nfilter;
            }
            else
            {
                _paging.Fields["CountryName"].Filter = null;
            }
            //---ISO2 filter
            if (!string.IsNullOrEmpty(txtRegNumber.Text))
            {
                var nfilter = new CompareFieldFilter
                    {
                        Expression = txtRegNumber.Text,
                        ParamName = "@CountryISO2"
                    };
                _paging.Fields["CountryISO2"].Filter = nfilter;
            }
            else
            {
                _paging.Fields["CountryISO2"].Filter = null;
            }

            //---ISO3 filter
            if (!string.IsNullOrEmpty(txtRegNumber.Text))
            {
                var nfilter = new CompareFieldFilter
                    {
                        Expression = txtRegNumber.Text,
                        ParamName = "@CountryISO3"
                    };
                _paging.Fields["CountryISO3"].Filter = nfilter;
            }
            else
            {
                _paging.Fields["CountryISO3"].Filter = null;
            }


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
            var pagen = txtPageNum.Text.TryParseInt(-1);
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
                        CountryService.Delete(SQLDataHelper.GetInt(id));
                    }
                }
                else
                {
                    foreach (var id in _paging.ItemsIds<int>("CountryID as ID").Where(id => !_selectionFilter.Values.Contains(id.ToString(CultureInfo.InvariantCulture))))
                    {
                        CountryService.Delete(id);
                    }
                }
            }
        }

        protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteCountry")
            {
                CountryService.Delete(SQLDataHelper.GetInt(e.CommandArgument));
            }

            if (e.CommandName == "AddCountry")
            {
                var footer = grid.FooterRow;
                if (
                    string.IsNullOrEmpty(((TextBox)footer.FindControl("txtNewName")).Text)
                    || string.IsNullOrEmpty(((TextBox)footer.FindControl("txtNewISO2")).Text)
                    || string.IsNullOrEmpty(((TextBox)footer.FindControl("txtNewISO3")).Text)
                    )
                {
                    grid.FooterStyle.BackColor = System.Drawing.Color.FromName("#ffcccc");
                    return;
                }

                CountryService.Add(new Country
                {
                    Name = ((TextBox) footer.FindControl("txtNewName")).Text,
                    Iso2 = ((TextBox) footer.FindControl("txtNewISO2")).Text,
                    Iso3 = ((TextBox) footer.FindControl("txtNewISO3")).Text,
                    DisplayInPopup = ((CheckBox) footer.FindControl("chkNewDisplayInPopup")).Checked,
                    SortOrder = ((TextBox)footer.FindControl("txtNewSortOrder")).Text.TryParseInt(),
                    DialCode = ((TextBox)footer.FindControl("txtNewDialCode")).Text.TryParseInt(true),
                });

                grid.ShowFooter = false;
            }
            if (e.CommandName == "CancelAdd")
            {
                grid.FooterStyle.BackColor = System.Drawing.Color.FromName("#ccffcc");
                grid.ShowFooter = false;
            }
        }

        protected void grid_Sorting(object sender, GridViewSortEventArgs e)
        {
            var arrows = new Dictionary<string, string>
                {
                    {"CountryName", "arrowCountryName"},
                    {"CountryISO2", "arrowISO2"},
                    {"CountryISO3", "arrowISO3"},
                    {"DisplayInPopup", "arrowDisplayInPopup"},
                    {"SortOrder", "arrowSortOrder"},
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
                CountryService.Update(new Country
                {
                    CountryId = SQLDataHelper.GetInt(grid.UpdatedRow["ID"]),
                    Name = grid.UpdatedRow["CountryName"],
                    Iso2 = grid.UpdatedRow["CountryISO2"],
                    Iso3 = grid.UpdatedRow["CountryISO3"],
                    DisplayInPopup = grid.UpdatedRow["DisplayInPopup"].TryParseBool(),
                    SortOrder = grid.UpdatedRow["SortOrder"].TryParseInt(),
                    DialCode = grid.UpdatedRow["DialCode"].TryParseInt(true)
                });
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

        protected void grid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["element_id"] = ((DataRowView)e.Row.DataItem)["Id"].ToString();
                e.Row.Attributes["rowType"] = "country";
            }
        }

        protected void btnAddCountry_Click(object sender, EventArgs e)
        {
            grid.ShowFooter = true;
            grid.FooterStyle.BackColor = System.Drawing.Color.FromName("#ccffcc");
            grid.DataBound += grid_DataBound;
        }

        void grid_DataBound(object sender, EventArgs e)
        {
            if (grid.ShowFooter)
            {
                grid.FooterRow.FindControl("txtNewName").Focus();
            }
        }
    }
}
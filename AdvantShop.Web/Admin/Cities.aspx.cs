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
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Core.SQL;
using AdvantShop.Helpers;
using AdvantShop.Repository;

namespace Admin
{
    public partial class Cities : AdvantShopAdminPage
    {
        #region Fields

        SqlPaging _paging;
        InSetFieldFilter _selectionFilter;
        bool _inverseSelection;

        private int _regionId;
        protected int RegionId
        {
            get { return _regionId != 0 ? _regionId : (_regionId = Request["regionid"].TryParseInt()); }
        }

        private int _countryId;
        protected int CountryId
        {
            get { return _countryId != 0 ? _countryId : (_countryId = Request["countryid"].TryParseInt()); }
        }

        #endregion

        public Cities()
        {
            _inverseSelection = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SetMeta(string.Format("{0} - {1}", AdvantShop.Configuration.SettingsMain.ShopName, Resources.Resource.Admin_MasterPageAdmin_Sities));

            if (RegionId < 0 && CountryId < 0)
                Response.Redirect("Country.aspx");

            if (RegionId != 0)
            {
                var region = RegionService.GetRegion(RegionId);
                if (region != null)
                {
                    lblHead.Text = region.Name;
                    hlBack.NavigateUrl = "Regions.aspx?CountryId=" + region.CountryId;
                    hlBack.Text = Resources.Resource.Admin_Cities_BackToRegions;

                    hlBack2.NavigateUrl = "Country.aspx";
                    hlBack2.Text = Resources.Resource.Admin_Cities_BackToCoutries;
                }
            }

            if (CountryId != 0)
            {
                var country = CountryService.GetCountry(CountryId);
                if (country != null)
                {
                    lblHead.Text = country.Name;
                    hlBack.NavigateUrl = "Regions.aspx?CountryId=" + country.CountryId;
                    hlBack.Text = Resources.Resource.Admin_Cities_BackToRegions;

                    hlBack2.NavigateUrl = "Country.aspx";
                    hlBack2.Text = Resources.Resource.Admin_Cities_BackToCoutries;
                }
                btnAddCity.Visible = false;
            }



            if (!IsPostBack)
            {
                _paging = new SqlPaging
                {
                    TableName = "[Customers].[City] Left Join Customers.Region On Region.RegionId=City.RegionId",
                    ItemsPerPage = 20
                };

                _paging.AddFieldsRange(
                    new List<Field>
                    {
                        new Field {Name = "CityID as ID", IsDistinct = true},
                        new Field {Name = "CityName", Sorting = SortDirection.Ascending},
                        new Field {Name = "CitySort"},
                        new Field {Name = "City.RegionID"},
                        new Field {Name = "City.DisplayInPopup"},
                        new Field {Name = "PhoneNumber"},
                        new Field {Name = "MobilePhoneNumber"},
                        new Field {Name = "Region.CountryId", NotInQuery = true},
                    });

                if (RegionId != 0)
                {
                    _paging.Fields["City.RegionID"].Filter = new EqualFieldFilter
                    {
                        ParamName = "@RegionID",
                        Value = RegionId.ToString()
                    };
                }

                if (CountryId != 0)
                {
                    _paging.Fields["Region.CountryId"].Filter = new EqualFieldFilter
                    {
                        ParamName = "@CountryId",
                        Value = CountryId.ToString()
                    };
                }

                grid.ChangeHeaderImageUrl("arrowCityName", "images/arrowup.gif");

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
            if (string.Compare(ddSelect.SelectedIndex.ToString(), "0") != 0)
            {

                if (string.Compare(ddSelect.SelectedIndex.ToString(), "2") == 0)
                {
                    if (_selectionFilter != null)
                    {
                        _selectionFilter.IncludeValues = !_selectionFilter.IncludeValues;
                    }
                    else
                    {
                        _selectionFilter = null; //New InSetFieldFilter()
                        //_SelectionFilter.IncludeValues = True
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
                _paging.Fields["CityName"].Filter = nfilter;
            }
            else
            {
                _paging.Fields["CityName"].Filter = null;
            }


            //----Name filter
            if (!string.IsNullOrEmpty(txtPhoneNumberFilter.Text))
            {
                var nfilter = new CompareFieldFilter { Expression = txtPhoneNumberFilter.Text, ParamName = "@PhoneNumber" };
                _paging.Fields["PhoneNumber"].Filter = nfilter;
            }
            else
            {
                _paging.Fields["PhoneNumber"].Filter = null;
            }


            //----Name filter
            if (!string.IsNullOrEmpty(txtPhoneNumberFilter.Text))
            {
                var nfilter = new CompareFieldFilter { Expression = txtPhoneNumberFilter.Text, ParamName = "@MobilePhoneNumber" };
                _paging.Fields["MobilePhoneNumber"].Filter = nfilter;
            }
            else
            {
                _paging.Fields["MobilePhoneNumber"].Filter = null;
            }


            //---RegionSort filter
            if (!string.IsNullOrEmpty(txtSort.Text))
            {
                var nfilter = new CompareFieldFilter
                    {
                        Expression = txtSort.Text,
                        ParamName = "@CitySort"
                    };
                _paging.Fields["CitySort"].Filter = nfilter;
            }
            else
            {
                _paging.Fields["CitySort"].Filter = null;
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
                        CityService.Delete(SQLDataHelper.GetInt(id));
                    }
                }
                else
                {
                    var itemsIds = _paging.ItemsIds<int>("CityID as ID");
                    foreach (var id in itemsIds.Where(id => !_selectionFilter.Values.Contains(id.ToString(CultureInfo.InvariantCulture))))
                    {
                        CityService.Delete(id);
                    }
                }
            }
        }

        protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                CityService.Delete(SQLDataHelper.GetInt(e.CommandArgument));
            }
            if (e.CommandName == "AddCity")
            {
                var footer = grid.FooterRow;

                if (string.IsNullOrEmpty(((TextBox)footer.FindControl("txtNewName")).Text))
                {
                    grid.FooterStyle.BackColor = System.Drawing.Color.FromName("#ffcccc");
                    return;
                }

                CityService.Add(new City
                {
                    Name = ((TextBox) footer.FindControl("txtNewName")).Text,
                    CitySort = ((TextBox)footer.FindControl("txtNewSort")).Text.TryParseInt(),
                    RegionId = RegionId,
                    DisplayInPopup = ((CheckBox)footer.FindControl("chkNewDisplayInPopup")).Checked,
                    PhoneNumber = ((TextBox)footer.FindControl("txtNewPhoneNumber")).Text,
                    MobilePhoneNumber = ((TextBox)footer.FindControl("txtNewMobilePhoneNumber")).Text
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
                    {"CityName", "arrowCityName"},
                    {"CityCode", "arrowCityCode"},
                    {"CitySort", "arrowCitySort"},
                    {"PhoneNumber", "arrowPhoneNumber"},
                    {"MobilePhoneNumber", "arrowMobilePhoneNumber"},
                    {"City.DisplayInPopup", "arrowDisplayInPopup"}
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
                CityService.Update(new City
                {
                    CityId = SQLDataHelper.GetInt(grid.UpdatedRow["ID"]),
                    Name = grid.UpdatedRow["CityName"],
                    RegionId = grid.UpdatedRow["RegionId"].TryParseInt(),
                    CitySort = grid.UpdatedRow["CitySort"].TryParseInt(),
                    DisplayInPopup = grid.UpdatedRow["DisplayInPopup"].TryParseBool(),
                    PhoneNumber = grid.UpdatedRow["PhoneNumber"],
                    MobilePhoneNumber = grid.UpdatedRow["MobilePhoneNumber"]
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
                    if (Array.Exists(_selectionFilter.Values, c => c == data.Rows[intIndex]["RegionID"].ToString()))
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

        protected void grid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
            }
        }

        protected void btnAddCity_Click(object sender, EventArgs e)
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
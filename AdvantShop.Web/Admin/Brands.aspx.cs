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

namespace Admin
{
    public partial class Brands : AdvantShopAdminPage
    {
        private bool _inverseSelection;
        private SqlPaging _paging;
        private InSetFieldFilter _selectionFilter;

        protected bool GetImageVisible(object o)
        {
            return !String.IsNullOrEmpty(o.ToString());
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, Resources.Resource.Admin_Brands_Header));

            if (!IsPostBack)
            {
                _paging = new SqlPaging
                    {
                        TableName = "[Catalog].[Brand] left join Catalog.Photo on Photo.ObjId = Brand.BrandID and Type = @Type ",
                        ItemsPerPage = 10
                    };

                _paging.AddFieldsRange(new List<Field>
                    {
                        new Field
                            {
                                Name = "BrandID as ID",
                                IsDistinct = true
                            },
                        new Field {Name = "ProductsCount", SelectExpression="(Select Count(ProductID) from Catalog.Product Where Product.BrandID=Brand.BrandID) as ProductsCount"},
                        new Field {Name = "PhotoName as BrandLogo"},
                        new Field {Name = "Enabled"},
                        new Field {Name = "SortOrder", Sorting = SortDirection.Ascending},
                        new Field {Name = "BrandName", Sorting=SortDirection.Ascending}
                    });
                _paging.AddParam(new SqlParam { Value = PhotoType.Brand.ToString(), ParameterName = "@Type" });
                grid.ChangeHeaderImageUrl("arrowSortOrder", "images/arrowup.gif");

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

            _paging.Fields["BrandLogo"].Filter = (ddlLogo.SelectedValue != "any")
                                                     ? new NullFieldFilter() { ParamName = "@BrandLogo", Null = ddlLogo.SelectedValue == "0" } : null;

            //----Name filter
            _paging.Fields["BrandName"].Filter = !string.IsNullOrEmpty(txtTitle.Text)
                                                     ? new CompareFieldFilter { Expression = txtTitle.Text, ParamName = "@BrandName" }
                                                     : null;


            _paging.Fields["Enabled"].Filter = (ddlShowOnMainPage.SelectedValue != "any")
                                                   ? new EqualFieldFilter { ParamName = "@Enabled", Value = ddlShowOnMainPage.SelectedValue } : null;

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
                        BrandService.DeleteBrand(SQLDataHelper.GetInt(id));
                    }
                }
                else
                {
                    var itemsIds = _paging.ItemsIds<int>("BrandID as ID");
                    foreach (var brandId in itemsIds.Where(brandId => !_selectionFilter.Values.Contains(brandId.ToString(CultureInfo.InvariantCulture))))
                    {
                        BrandService.DeleteBrand(brandId);
                    }
                }
            }
        }

        protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteBrand")
            {
                BrandService.DeleteBrand(SQLDataHelper.GetInt(e.CommandArgument));
            }
        }

        protected void grid_Sorting(object sender, GridViewSortEventArgs e)
        {
            var arrows = new Dictionary<string, string>
                {
                    {"BrandName", "arrowBrandName"},
                    {"ProductsCount", "arrowProductsCount"},
                    {"ShowOnMainPage", "arrowShowOnMainPage"},
                    {"Enabled", "arrowEnabled"},
                    {"SortOrder", "arrowSortOrder"}
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
                int sort = 0;
                int.TryParse(grid.UpdatedRow["SortOrder"], out sort);
                Brand brand = BrandService.GetBrandById(SQLDataHelper.GetInt(grid.UpdatedRow["ID"]));
                brand.Name = grid.UpdatedRow["BrandName"];
                brand.Enabled = SQLDataHelper.GetBoolean(grid.UpdatedRow["Enabled"]);
                brand.SortOrder = sort;
                BrandService.UpdateBrand(brand);
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

        protected void grid_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void grid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        }

    }
}
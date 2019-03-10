//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using AdvantShop.Catalog;
using AdvantShop;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.SQL;
using AdvantShop.Helpers;

namespace Admin.UserControls
{
    public partial class PopupGridBrand : UserControl
    {
        private SqlPaging _paging;
        public int SelectBrandId
        {
            get { return SQLDataHelper.GetInt(ViewState["BrandID"]); }
            set { ViewState["BrandID"] = value; }
        }

        public int ProductId { get; set; }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            agvBrand.DataSource = _paging.PageItems;
            agvBrand.DataBind();
            pageNumberer.PageCount = _paging.PageCount;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                _paging = new SqlPaging { TableName = "[Catalog].[Brand]", ItemsPerPage = 10 };

                var f = new Field { Name = "BrandID", IsDistinct = true };

                _paging.AddField(f);

                f = new Field { Name = "BrandName", Sorting = SortDirection.Ascending };

                _paging.AddField(f);

                f = new Field { Name = "(Select Count(ProductID) from Catalog.Product Where Product.BrandID=Brand.BrandID) as ProductsCount" };
                _paging.AddField(f);

                agvBrand.ChangeHeaderImageUrl("arrowBrandName", "../images/arrowup.gif");

                _paging.ItemsPerPage = 10;

                pageNumberer.CurrentPageIndex = 1;
                _paging.CurrentPageIndex = 1;
                ViewState["Paging"] = _paging;
                ViewState["Show"] = false;
            }
            else
            {
                _paging = (SqlPaging)(ViewState["Paging"]);

                if (_paging == null)
                {
                    throw (new Exception("Paging lost"));
                }
            }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(txtSearchName.Text))
            {
                var sfilter = new CompareFieldFilter { Expression = txtSearchName.Text, ParamName = "@Name" };
                _paging.Fields["BrandName"].Filter = sfilter;
            }
            else
            {
                _paging.Fields["BrandName"].Filter = null;
            }

            if (!string.IsNullOrEmpty(txtSearchProducts_Count.Text))
            {
                var sfilter = new EqualFieldFilter { Value = txtSearchProducts_Count.Text, ParamName = "@ProductsCount" };
                _paging.Fields["ProductsCount"].Filter = sfilter;
            }
            else
            {
                _paging.Fields["ProductsCount"].Filter = null;
            }


            ViewState["Show"] = true;
            pageNumberer.CurrentPageIndex = 1;
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtSearchName.Text = string.Empty;
            txtSearchProducts_Count.Text = string.Empty;
            btnFilter_Click(sender, e);
            agvBrand.DataBind();
            ViewState["Show"] = true;
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
            if (pagen < 1 || pagen > _paging.PageCount) return;
            pageNumberer.CurrentPageIndex = pagen;
            _paging.CurrentPageIndex = pagen;
            ViewState["Show"] = true;
        }

        protected void pn_SelectedPageChanged(object sender, EventArgs e)
        {
            _paging.CurrentPageIndex = pageNumberer.CurrentPageIndex;
            ViewState["Show"] = true;
        }

        protected void agv_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                SelectBrandId = SQLDataHelper.GetInt(e.CommandArgument);
                ViewState["Show"] = false;

                if (Request["ProductID"].TryParseInt() != 0)
                {
                    ProductService.SetBrand(Request["ProductID"].TryParseInt(), SelectBrandId);
                }

            }
        }

        protected void agvBrand_Sorting(object sender, GridViewSortEventArgs e)
        {
            var arrows = new Dictionary<string, string>
                {
                    {"BrandName", "arrowBrandName"},
                    {"ProductsCount", "arrowProductsCount"},                             
                };

            const string urlArrowUp = "../images/arrowup.gif";
            const string urlArrowDown = "../images/arrowdown.gif";
            const string urlArrowGray = "../images/arrowdownh.gif";

            Field csf = (from Field f in _paging.Fields.Values where f.Sorting.HasValue select f).First();
            Field nsf = _paging.Fields[e.SortExpression];

            if (nsf.Name.Equals(csf.Name))
            {
                csf.Sorting = csf.Sorting == SortDirection.Ascending ? SortDirection.Descending : SortDirection.Ascending;
                agvBrand.ChangeHeaderImageUrl(arrows[csf.Name],
                                              (csf.Sorting == SortDirection.Ascending ? urlArrowUp : urlArrowDown));
            }
            else
            {
                csf.Sorting = null;
                //If Not csf.Name.Contains("SortOrder") Then
                agvBrand.ChangeHeaderImageUrl(arrows[csf.Name], urlArrowGray);
                //End If

                nsf.Sorting = SortDirection.Ascending;
                agvBrand.ChangeHeaderImageUrl(arrows[nsf.Name], urlArrowUp);
            }

            pageNumberer.CurrentPageIndex = 1;
            _paging.CurrentPageIndex = 1;
            ViewState["Show"] = true;
            //UpdatePanel2.Update();
        }
    }
}
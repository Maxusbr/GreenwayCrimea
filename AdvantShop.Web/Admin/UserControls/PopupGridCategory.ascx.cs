//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using AdvantShop.Core.SQL;
using AdvantShop.Helpers;

namespace Admin.UserControls
{
    public partial class PopupGridCategory : UserControl
    {
        private SqlPaging _paging;
        public int SelectCategoryId = 0;

        protected void Page_PreRender(object sender, EventArgs e)
        {
            agvCategory.DataSource = _paging.PageItems;
            agvCategory.DataBind();
            pageNumberer.PageCount = _paging.PageCount;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // load modalpopup for contact
                _paging = new SqlPaging { TableName = "[Catalog].[Category]", ItemsPerPage = 10 };

                var f = new Field { Name = "CategoryID", IsDistinct = true };

                _paging.AddField(f);

                f = new Field { Name = "Name", Sorting=SortDirection.Ascending };

                _paging.AddField(f);

                f = new Field { Name = "Products_Count" };
                _paging.AddField(f);

                agvCategory.ChangeHeaderImageUrl("arrowName", "../images/arrowup.gif");

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
            //----Login filter
            if (!string.IsNullOrEmpty(txtSearchName.Text))
            {
                var sfilter = new CompareFieldFilter { Expression = txtSearchName.Text, ParamName = "@Name" };
                _paging.Fields["Name"].Filter = sfilter;
            }
            else
            {
                _paging.Fields["Name"].Filter = null;
            }

            //----Firstname filter
            if (!string.IsNullOrEmpty(txtSearchProducts_Count.Text))
            {
                var nfilter = new CompareFieldFilter { Expression = txtSearchProducts_Count.Text, ParamName = "@Products_Count" };
                _paging.Fields["Products_Count"].Filter = nfilter;
            }
            else
            {
                _paging.Fields["Products_Count"].Filter = null;
            }
            ViewState["Show"] = true;
            pageNumberer.CurrentPageIndex = 1;
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtSearchName.Text = string.Empty;
            txtSearchProducts_Count.Text = string.Empty;
            btnFilter_Click(sender, e);
            agvCategory.DataBind();
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
                SelectCategoryId = SQLDataHelper.GetInt(e.CommandArgument);
                ViewState["Show"] = false;
            }
        }

        protected void agvCategory_Sorting(object sender, GridViewSortEventArgs e)
        {
            var arrows = new Dictionary<string, string>
                {
                    {"Name", "arrowName"},
                    {"Products_Count", "arrowProducts_Count"},                             
                };

            const string urlArrowUp = "../images/arrowup.gif";
            const string urlArrowDown = "../images/arrowdown.gif";
            const string urlArrowGray = "../images/arrowdownh.gif";

            Field csf = (from Field f in _paging.Fields.Values where f.Sorting.HasValue select f).First();
            Field nsf = _paging.Fields[e.SortExpression];

            if (nsf.Name.Equals(csf.Name))
            {
                csf.Sorting = csf.Sorting == SortDirection.Ascending ? SortDirection.Descending : SortDirection.Ascending;
                agvCategory.ChangeHeaderImageUrl(arrows[csf.Name],
                                                 (csf.Sorting == SortDirection.Ascending ? urlArrowUp : urlArrowDown));
            }
            else
            {
                csf.Sorting = null;
                //If Not csf.Name.Contains("SortOrder") Then
                agvCategory.ChangeHeaderImageUrl(arrows[csf.Name], urlArrowGray);
                //End If

                nsf.Sorting = SortDirection.Ascending;
                agvCategory.ChangeHeaderImageUrl(arrows[nsf.Name], urlArrowUp);
            }

            pageNumberer.CurrentPageIndex = 1;
            _paging.CurrentPageIndex = 1;
            ViewState["Show"] = true;
            //UpdatePanel2.Update();
        }
    }
}
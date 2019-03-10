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
    public partial class PopupGridNews : UserControl
    {
        private SqlPaging _paging;
        public int SelectNewsId = 0;

        protected void Page_PreRender(object sender, EventArgs e)
        {
            agvNews.DataSource = _paging.PageItems;
            agvNews.DataBind();
            pageNumberer.PageCount = _paging.PageCount;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // load modalpopup for contact
                _paging = new SqlPaging { TableName = "[Settings].[News]", ItemsPerPage = 10 };

                var f = new Field { Name = "NewsID", IsDistinct = true };

                _paging.AddField(f);

                f = new Field { Name = "Title" };

                _paging.AddField(f);

                f = new Field { Name = "AddingDate", Sorting = SortDirection.Descending };
                _paging.AddField(f);

                agvNews.ChangeHeaderImageUrl("arrowAddingDate", "../images/arrowdown.gif");

                _paging.ItemsPerPage = 10;

                pageNumberer.CurrentPageIndex = 1;
                _paging.CurrentPageIndex = 1;
                ViewState["Paging"] = _paging;
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
            if (!string.IsNullOrEmpty(txtSearchTitle.Text))
            {
                var sfilter = new CompareFieldFilter { Expression = txtSearchTitle.Text, ParamName = "@artNo" };
                _paging.Fields["Title"].Filter = sfilter;
            }
            else
            {
                _paging.Fields["Title"].Filter = null;
            }

            //----Firstname filter
            if (!string.IsNullOrEmpty(txtSearchAddingDate.Text))
            {
                var nfilter = new CompareFieldFilter { Expression = txtSearchAddingDate.Text, ParamName = "@AddingDate" };
                _paging.Fields["AddingDate"].Filter = nfilter;
            }
            else
            {
                _paging.Fields["AddingDate"].Filter = null;
            }

            pageNumberer.CurrentPageIndex = 1;
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtSearchTitle.Text = string.Empty;
            txtSearchAddingDate.Text = string.Empty;
            btnFilter_Click(sender, e);
            agvNews.DataBind();
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
        }

        protected void pn_SelectedPageChanged(object sender, EventArgs e)
        {
            _paging.CurrentPageIndex = pageNumberer.CurrentPageIndex;
        }

        protected void agv_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                SelectNewsId = SQLDataHelper.GetInt(e.CommandArgument);
            }
        }

        protected void agvNews_Sorting(object sender, GridViewSortEventArgs e)
        {
            var arrows = new Dictionary<string, string>
                {
                    {"Title", "arrowTitle"},
                    {"AddingDate", "arrowAddingDate"},                             
                };

            const string urlArrowUp = "../images/arrowup.gif";
            const string urlArrowDown = "../images/arrowdown.gif";
            const string urlArrowGray = "../images/arrowdownh.gif";

            Field csf = (from Field f in _paging.Fields.Values where f.Sorting.HasValue select f).First();
            Field nsf = _paging.Fields[e.SortExpression];

            if (nsf.Name.Equals(csf.Name))
            {
                csf.Sorting = csf.Sorting == SortDirection.Ascending ? SortDirection.Descending : SortDirection.Ascending;
                agvNews.ChangeHeaderImageUrl(arrows[csf.Name],
                                             (csf.Sorting == SortDirection.Ascending ? urlArrowUp : urlArrowDown));
            }
            else
            {
                csf.Sorting = null;

                agvNews.ChangeHeaderImageUrl(arrows[csf.Name], urlArrowGray);

                nsf.Sorting = SortDirection.Ascending;
                agvNews.ChangeHeaderImageUrl(arrows[nsf.Name], urlArrowUp);
            }

            pageNumberer.CurrentPageIndex = 1;
            _paging.CurrentPageIndex = 1;
        }

    }
}
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
    public partial class PopupGridAux : UserControl
    {
        private SqlPaging _paging;
        public int SelectAuxId = 0;

        protected void Page_PreRender(object sender, EventArgs e)
        {
            agvAux.DataSource = _paging.PageItems;
            agvAux.DataBind();
            pageNumberer.PageCount = _paging.PageCount;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        
            if (!IsPostBack)
            {
                // load modalpopup for contact
                _paging = new SqlPaging { TableName = "[CMS].[StaticPage]", ItemsPerPage = 10 };

                var f = new Field { Name = "StaticPageID", IsDistinct = true };

                _paging.AddField(f);

                f = new Field { Name = "PageName" };

                _paging.AddField(f);

                f = new Field { Name = "SortOrder", Sorting=SortDirection.Ascending };
                _paging.AddField(f);

                agvAux.ChangeHeaderImageUrl("arrowPageName", "../images/arrowup.gif");

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
            if (!string.IsNullOrEmpty(txtSearchPageName.Text))
            {
                var sfilter = new CompareFieldFilter { Expression = txtSearchPageName.Text, ParamName = "@PageName" };
                _paging.Fields["PageName"].Filter = sfilter;
            }
            else
            {
                _paging.Fields["PageName"].Filter = null;
            }

            //----Firstname filter
            if (!string.IsNullOrEmpty(txtSearchSortOrder.Text))
            {
                var nfilter = new CompareFieldFilter { Expression = txtSearchSortOrder.Text, ParamName = "@SortOrder" };
                _paging.Fields["SortOrder"].Filter = nfilter;
            }
            else
            {
                _paging.Fields["SortOrder"].Filter = null;
            }        

            pageNumberer.CurrentPageIndex = 1;
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtSearchPageName.Text = string.Empty;
            txtSearchSortOrder.Text = string.Empty;        
            btnFilter_Click(sender, e);
            agvAux.DataBind();
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
                SelectAuxId = SQLDataHelper.GetInt(e.CommandArgument);
            }
        }

        protected void agvAux_Sorting(object sender, GridViewSortEventArgs e)
        {
            var arrows = new Dictionary<string, string>
                {
                    {"PageName", "arrowPageName"},
                    {"SortOrder", "arrowSortOrder"},                             
                };

            const string urlArrowUp = "../images/arrowup.gif";
            const string urlArrowDown = "../images/arrowdown.gif";
            const string urlArrowGray = "../images/arrowdownh.gif";

            Field csf = (from Field f in _paging.Fields.Values where f.Sorting.HasValue select f).First();
            Field nsf = _paging.Fields[e.SortExpression];

            if (nsf.Name.Equals(csf.Name))
            {
                csf.Sorting = csf.Sorting == SortDirection.Ascending ? SortDirection.Descending : SortDirection.Ascending;
                agvAux.ChangeHeaderImageUrl(arrows[csf.Name],
                                            (csf.Sorting == SortDirection.Ascending ? urlArrowUp : urlArrowDown));
            }
            else
            {
                csf.Sorting = null;
                //If Not csf.Name.Contains("SortOrder") Then
                agvAux.ChangeHeaderImageUrl(arrows[csf.Name], urlArrowGray);
                //End If

                nsf.Sorting = SortDirection.Ascending;
                agvAux.ChangeHeaderImageUrl(arrows[nsf.Name], urlArrowUp);
            }

            pageNumberer.CurrentPageIndex = 1;
            _paging.CurrentPageIndex = 1;
            //UpdatePanel2.Update();
        }
   
    }
}
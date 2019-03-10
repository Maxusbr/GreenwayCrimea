using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using AdvantShop.Catalog;
using AdvantShop.Core.SQL;
using AdvantShop.Helpers;

namespace Admin.UserControls.Products
{
    public partial class ProductVideos : System.Web.UI.UserControl
    {

        private bool _inverseSelection;
        private SqlPaging _paging;
        private InSetFieldFilter _selectionFilter;
        public int ProductID { set; get; }

    
        public class VideoMessageEventArgs
        {
            public string Message { get; private set; }
            public VideoMessageEventArgs(string message)
            {
                Message = message;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            btnAdd.OnClientClick = "javascript:open_window('m_ProductVideos.aspx?ProductID=" + ProductID + "',780,600);return false;";

            if (!IsPostBack)
            {
                _paging = new SqlPaging { TableName = "[Catalog].[ProductVideo]", ItemsPerPage = 10 };

                var f = new Field { Name = "ProductVideoID", IsDistinct = true, Filter = _selectionFilter };
                _paging.AddField(f);

                f = new Field
                    {
                        Name = "ProductID",
                        Filter = new EqualFieldFilter
                            {
                                ParamName = "@productID",
                                Value = ProductID.ToString()
                            }
                    };
                _paging.AddField(f);


                f = new Field { Name = "Name" };
                _paging.AddField(f);
            
                f = new Field { Name = "VideoSortOrder", Sorting = SortDirection.Ascending };
                _paging.AddField(f);

                grid_video.ChangeHeaderImageUrl("arrowVideoSortOrder", "~/admin/images/arrowup.gif");

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

                string strIds = Request.Form["SelectedIdsVideo"];


                if (!string.IsNullOrEmpty(strIds))
                {
                    strIds = strIds.Trim();
                    string[] arrids = strIds.Split(' ');

                    var ids = new string[arrids.Length ];
                    _selectionFilter = new InSetFieldFilter { IncludeValues = true };
                    for (int idx = 0; idx <= ids.Length - 1; idx++)
                    {
                        int t = -1;
                        int.TryParse(arrids[idx], out t);
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
                    //_InverseSelection = If(ids(0) = -1, True, False)
                }
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (ProductID == 0)
                return;

            if (grid_video.UpdatedRow != null)
            {
                int sortOrder = 0;
                if (int.TryParse(grid_video.UpdatedRow["VideoSortOrder"], out sortOrder))
                {
                    ProductVideoService.UpdateProductVideo(SQLDataHelper.GetInt(grid_video.UpdatedRow["ProductVideoID"]), grid_video.UpdatedRow["Name"], sortOrder);
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
                    if (Array.Exists(_selectionFilter.Values, c => c == data.Rows[intIndex]["ProductVideoID"].ToString()))
                    {
                        data.Rows[i]["IsSelected"] = !_inverseSelection;
                    }
                }
            }

            if (data.Rows.Count < 1)
            {
                goToPage.Visible = false;
            }

            grid_video.DataSource = data;
            grid_video.DataBind();

            pageNumberer.PageCount = _paging.PageCount;
            //lblFound.Text = _paging.TotalRowsCount.ToString();
        }

        protected void lbDeleteSelected_Click(object sender, EventArgs e)
        {
            if ((_selectionFilter != null) && (_selectionFilter.Values != null))
            {
                if (!_inverseSelection)
                {
                    foreach (string id in _selectionFilter.Values)
                    {
                        ProductVideoService.DeleteProductVideo(SQLDataHelper.GetInt(id));
                    }
                }
                else
                {
                    IEnumerable<ProductVideo> videos = ProductVideoService.GetProductVideos(ProductID);
                    foreach (ProductVideo pv in videos.Where(pv => !_selectionFilter.Values.Contains(pv.ProductVideoId.ToString())))
                    {
                        ProductVideoService.DeleteProductVideo(pv.ProductVideoId);
                    }
                }
            }
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

        protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                ProductVideoService.DeleteProductVideo(SQLDataHelper.GetInt(e.CommandArgument));
            }
        }

        protected void grid_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
        }

        protected void grid_Sorting(object sender, GridViewSortEventArgs e)
        {
            var arrows = new Dictionary<string, string>
                {
                    {"Name", "arrowName"},
                    {"VideoSortOrder", "arrowVideoSortOrder"}
                };
            const string urlArrowUp = "~/admin/images/arrowup.gif";
            const string urlArrowDown = "~/admin/images/arrowdown.gif";
            const string urlArrowGray = "~/admin/images/arrowdownh.gif";


            Field csf = (from Field f in _paging.Fields.Values where f.Sorting.HasValue select f).First();
            Field nsf = _paging.Fields[e.SortExpression];

            if (nsf.Name.Equals(csf.Name))
            {
                csf.Sorting = csf.Sorting == SortDirection.Ascending ? SortDirection.Descending : SortDirection.Ascending;
                grid_video.ChangeHeaderImageUrl(arrows[csf.Name],
                                                (csf.Sorting == SortDirection.Ascending ? urlArrowUp : urlArrowDown));
            }
            else
            {
                csf.Sorting = null;
                grid_video.ChangeHeaderImageUrl(arrows[csf.Name], urlArrowGray);

                nsf.Sorting = SortDirection.Ascending;
                grid_video.ChangeHeaderImageUrl(arrows[nsf.Name], urlArrowUp);
            }

            pageNumberer.CurrentPageIndex = 1;
            _paging.CurrentPageIndex = 1;
        }

        protected void pn_SelectedPageChanged(object sender, EventArgs e)
        {
            _paging.CurrentPageIndex = pageNumberer.CurrentPageIndex;
        }


        protected void btnFilter_Click(object sender, EventArgs e)
        {
            pageNumberer.CurrentPageIndex = 1;
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            btnFilter_Click(sender, e);
            grid_video.ChangeHeaderImageUrl(null, null);
        }

        protected void lnkUpdateVideo_Click(object sender, EventArgs e)
        {
            //OnMainPhotoUpdate(e);
        }
    }
}
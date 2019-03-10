using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using AdvantShop;
using AdvantShop.Catalog;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.SQL;
using AdvantShop.Helpers;

namespace Admin.UserControls.Products
{
    public partial class ProductPhotos : System.Web.UI.UserControl
    {

        private bool _inverseSelection;
        private SqlPaging _paging;
        private InSetFieldFilter _selectionFilter;
        public int ProductID { set; get; }

        public string ProductName { get; set; }

        public event Action<object, EventArgs> MainPhotoUpdate;
        protected virtual void OnMainPhotoUpdate(EventArgs e)
        {
            if (MainPhotoUpdate != null) MainPhotoUpdate(this, e);
        }

        public class PhotoMessageEventArgs
        {
            public string Message { get; private set; }
            public PhotoMessageEventArgs(string message)
            {
                Message = message;
            }
        }
        //public event Action<object, PhotoMessageEventArgs> PhotoMessage;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                _paging = new SqlPaging { TableName = "[Catalog].[Photo]", ItemsPerPage = 10 };

                var f = new Field { Name = "PhotoID as ID", IsDistinct = true, Filter = _selectionFilter };
                _paging.AddField(f);

                f = new Field
                    {
                        Name = "ObjId",
                        Filter = new EqualFieldFilter
                            {
                                ParamName = "@ObjId",
                                Value = ProductID.ToString()
                            }
                    };
                _paging.AddField(f);


                f = new Field { Name = "PhotoName" };
                _paging.AddField(f);


                f = new Field { Name = "Description" };
                _paging.AddField(f);

                f = new Field { Name = "PhotoSortOrder", Sorting = SortDirection.Ascending };
                _paging.AddField(f);

                f = new Field { Name = "Main" };
                _paging.AddField(f);

                f = new Field { Name = "convert(nvarchar, isnull(ColorID, 0)) as ColorID" };
                _paging.AddField(f);

                f = new Field { Name = "Type", NotInQuery = true, Filter = new EqualFieldFilter { ParamName = "@Type", Value = PhotoType.Product.ToString() } };
                _paging.AddField(f);

                grid.ChangeHeaderImageUrl("arrowPhotoSortOrder", "~/admin/images/arrowup.gif");

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

            if (grid.UpdatedRow != null)
            {
                int sortOrder = 0;
                int? colorID = grid.UpdatedRow["ColorID"].TryParseInt(true);
                if (colorID == 0)
                    colorID = null;

                if (int.TryParse(grid.UpdatedRow["PhotoSortOrder"], out sortOrder))
                {
                    var ph = new Photo(SQLDataHelper.GetInt(grid.UpdatedRow["ID"]), ProductID, PhotoType.Product)
                        {
                            Description = grid.UpdatedRow["Description"],
                            PhotoSortOrder = sortOrder,
                            ColorID = colorID
                        };

                    PhotoService.UpdatePhoto(ph);
                    if (grid.UpdatedRow["Main"] == "True")
                    {
                        PhotoService.SetProductMainPhoto(SQLDataHelper.GetInt(grid.UpdatedRow["ID"]));
                        MainPhotoUpdate(this, new EventArgs());
                    }
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

        protected void lbDeleteSelected_Click(object sender, EventArgs e)
        {
            if ((_selectionFilter != null) && (_selectionFilter.Values != null))
            {

                if (!_inverseSelection)
                {
                    foreach (var id in _selectionFilter.Values)
                    {
                        PhotoService.DeleteProductPhoto(SQLDataHelper.GetInt(id));
                    }
                }
                else
                {
                    var itemsIds = _paging.ItemsIds<int>("PhotoID as ID");
                    foreach (var id in itemsIds.Where(phId => !_selectionFilter.Values.Contains(phId.ToString())))
                    {
                        PhotoService.DeleteProductPhoto(id);
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
                //var main = false;
                //var photo = PhotoService.GetProductPhoto(SQLDataHelper.GetInt(e.CommandArgument));
                //if (photo != null)
                //{
                //    main = PhotoService.GetProductPhoto(SQLDataHelper.GetInt(e.CommandArgument)).Main;
                //}
                PhotoService.DeleteProductPhoto(SQLDataHelper.GetInt(e.CommandArgument));
                //if (main) 
                OnMainPhotoUpdate(EventArgs.Empty);
            }
        }

        protected void grid_Sorting(object sender, GridViewSortEventArgs e)
        {
            var arrows = new Dictionary<string, string>
                {
                    {"Description", "arrowDescription"},
                    {"PhotoSortOrder", "arrowPhotoSortOrder"},
                    {"Main", "arrowMain"},
                    {"ColorID", "arrowColorID"},
                };
            const string urlArrowUp = "~/admin/images/arrowup.gif";
            const string urlArrowDown = "~/admin/images/arrowdown.gif";
            const string urlArrowGray = "~/admin/images/arrowdownh.gif";


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

        protected void pn_SelectedPageChanged(object sender, EventArgs e)
        {
            _paging.CurrentPageIndex = pageNumberer.CurrentPageIndex;
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

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            pageNumberer.CurrentPageIndex = 1;
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            btnFilter_Click(sender, e);
            grid.ChangeHeaderImageUrl(null, null);
        }

        protected void lnkUpdatePhoto_Click(object sender, EventArgs e)
        {
            OnMainPhotoUpdate(e);
        }


        protected void sds_Init(object sender, EventArgs e)
        {
            ((SqlDataSource)sender).ConnectionString = AdvantShop.Connection.GetConnectionString();
        }
    }
}
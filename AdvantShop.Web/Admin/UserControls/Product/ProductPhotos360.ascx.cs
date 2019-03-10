using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.SQL;
using AdvantShop.FilePath;
using AdvantShop.Helpers;

namespace Admin.UserControls.Products
{
    public partial class ProductPhotos360 : System.Web.UI.UserControl
    {
        private SqlPaging _paging;

        public int ProductID { set; get; }

        public bool ActiveView360
        {
            set { ckbActiveView360.Checked = value; }
            get { return ckbActiveView360.Checked; }
        }

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
            //if (!IsPostBack)
            {
                _paging = new SqlPaging { TableName = "[Catalog].[Photo]", ItemsPerPage = 10 };
                
                var f = new Field
                    {
                        Name = "ObjId",
                        Filter = new EqualFieldFilter
                            {
                                ParamName = "@ObjId",
                                Value = ProductID.ToString()
                            },
                        IsDistinct = true
                    };
                _paging.AddField(f);

                f = new Field { Name = "convert(nvarchar, isnull(ColorID, 0)) as ColorID" };
                _paging.AddField(f);

                f = new Field { Name = "Type", NotInQuery = true, Filter = new EqualFieldFilter { ParamName = "@Type", Value = PhotoType.Product360.ToString() } };
                _paging.AddField(f);

                //gridphotos360.ChangeHeaderImageUrl("arrowPhotoSortOrder", "~/admin/images/arrowup.gif");

                _paging.ItemsPerPage = 100;

                ViewState["Paging"] = _paging;
            }
            //else
            //{
            //    _paging = (SqlPaging)(ViewState["Paging"]);
            //    if (_paging == null)
            //    {
            //        throw (new Exception("Paging lost"));
            //    }
            //}
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {


            if (ProductID == 0)
            {
                return;
            }

            ckbActiveView360.Checked = ActiveView360;

            if (gridphotos360.UpdatedRow != null)
            {
                int sortOrder = 0;
                int? colorID = gridphotos360.UpdatedRow["ColorID"].TryParseInt(true);
                if (colorID == 0)
                    colorID = null;

                if (int.TryParse(gridphotos360.UpdatedRow["PhotoSortOrder"], out sortOrder))
                {
                    //var ph = new Photo(SQLDataHelper.GetInt(gridphotos360.UpdatedRow["ID"]), ProductID, PhotoType.Product)
                    //    {
                    //        Description = gridphotos360.UpdatedRow["Description"],
                    //        PhotoSortOrder = sortOrder,
                    //        //ColorID = colorID
                    //    };

                    //PhotoService.UpdatePhoto(ph);
                    //if (gridphotos360.UpdatedRow["Main"] == "True")
                    //{
                    //    PhotoService.SetProductMainPhoto(SQLDataHelper.GetInt(gridphotos360.UpdatedRow["ID"]));
                    //    MainPhotoUpdate(this, new EventArgs());
                    //}
                }
            }

            //var listOfViews360 = new List<string>();

            //if (Directory.Exists(SettingsGeneral.AbsolutePath + "pictures/product/rotate/" + ProductID))
            //{
            //    foreach (var directory in Directory.GetDirectories(SettingsGeneral.AbsolutePath + "pictures/product/rotate/" + ProductID))
            //    {
            //        var firestPicture = Directory.GetFiles(directory, "*.jpg | *.jpeg | *.png | *.gif | *.tiff").FirstOrDefault();
            //        if (string.IsNullOrEmpty(firestPicture))
            //        {

            //        }
            //    }
            //}
            //return;

            DataTable data = _paging.PageItems;

            gridphotos360.DataSource = data;
            //gridphotos360.DataSource = PhotoService.GetPhotos(ProductID, PhotoType.Product360);

            gridphotos360.DataBind();

            if (!ActiveView360)
            {
                pnlPhotoView360.Attributes.Add("style", "display:none;");
            }
            else
            {
                pnlPhotoView360.Attributes.Remove("style");
            }
        }

        protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                PhotoService.DeletePhotoByOwnerIdAndTypeAndColor(ProductID, PhotoType.Product360,
                    SQLDataHelper.GetInt(e.CommandArgument) == 0 ? (int?)null : SQLDataHelper.GetInt(e.CommandArgument));

                var filePath = FoldersHelper.GetImageProductPathAbsolut(ProductImageType.Rotate, string.Empty) + ProductID;

                FileHelpers.DeleteDirectory(filePath);
            }
        }

        protected void grid_Sorting(object sender, GridViewSortEventArgs e)
        {
            var arrows = new Dictionary<string, string>
                {
                    {"PhotoSortOrder", "arrowPhotoSortOrder"},
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
                gridphotos360.ChangeHeaderImageUrl(arrows[csf.Name],
                                          (csf.Sorting == SortDirection.Ascending ? urlArrowUp : urlArrowDown));
            }
            else
            {
                csf.Sorting = null;
                gridphotos360.ChangeHeaderImageUrl(arrows[csf.Name], urlArrowGray);

                nsf.Sorting = SortDirection.Ascending;
                gridphotos360.ChangeHeaderImageUrl(arrows[nsf.Name], urlArrowUp);
            }

            _paging.CurrentPageIndex = 1;
        }

        protected void pn_SelectedPageChanged(object sender, EventArgs e)
        {
            //_paging.CurrentPageIndex = pageNumberer.CurrentPageIndex;
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
            //pageNumberer.CurrentPageIndex = 1;
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            btnFilter_Click(sender, e);
            gridphotos360.ChangeHeaderImageUrl(null, null);
        }

        protected void lnkUpdatePhoto_Click(object sender, EventArgs e)
        {
            //OnMainPhotoUpdate(e);
        }


        protected void sds_Init(object sender, EventArgs e)
        {
            ((SqlDataSource)sender).ConnectionString = AdvantShop.Connection.GetConnectionString();
        }


        protected string RenderPhotoName(string colorId)
        {
            var filePath = FoldersHelper.GetImageProductPathAbsolut(ProductImageType.Rotate, string.Empty) + ProductID;
            if (!Directory.Exists(filePath))
                return string.Empty;

            var firstPicture = Directory.GetFiles(filePath).FirstOrDefault();
            if (firstPicture == null)
                return string.Empty;

            return string.Format("<img src=\"{0}/pictures/product/rotate/{1}/{2}\"  style=\"width: 200px;\" />",
                SettingsGeneral.AbsoluteUrlPath, ProductID, Path.GetFileName(firstPicture));
        }

        protected string RenderPhotoCount(string colorId)
        {
            var filePath = FoldersHelper.GetImageProductPathAbsolut(ProductImageType.Rotate, string.Empty) + ProductID;

            return Directory.Exists(filePath) ? Directory.GetFiles(filePath).Count().ToString() : string.Empty;
        }
    }
}
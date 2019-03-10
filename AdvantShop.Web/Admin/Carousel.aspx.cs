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
using AdvantShop.Core.Caching;
using AdvantShop.Core.Controls;
using AdvantShop.Core.Services.CMS;
using AdvantShop.Core.SQL;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Diagnostics;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.Trial;
using Resources;

namespace Admin
{
    public partial class Carouseles : AdvantShopAdminPage
    {
        private bool _inverseSelection;
        private SqlPaging _paging;
        private InSetFieldFilter _selectionFilter;

        public Carouseles()
        {
            _inverseSelection = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_Carousels_Header));

            if (!IsPostBack)
            {
                _paging = new SqlPaging { TableName = "[CMS].[Carousel] left join Catalog.Photo on Photo.ObjId=Carousel.CarouselID and Type=@Type", ItemsPerPage = 10 };

                var f = new Field { Name = "CarouselID as ID", IsDistinct = true, Filter = _selectionFilter };
                _paging.AddField(f);

                f = new Field { Name = "URL" };
                _paging.AddField(f);

                f = new Field { Name = "SortOrder", Sorting = SortDirection.Ascending };
                _paging.AddField(f);


                f = new Field { Name = "Enabled" };
                _paging.AddField(f);

                f = new Field { Name = "PhotoName" };
                _paging.AddField(f);

                f = new Field { Name = "Description" };
                _paging.AddField(f);

                f = new Field { Name = "DisplayInOneColumn" };
                _paging.AddField(f);

                f = new Field { Name = "DisplayInTwoColumns" };
                _paging.AddField(f);

                f = new Field { Name = "DisplayInMobile" };
                _paging.AddField(f);


                _paging.AddParam(new SqlParam { ParameterName = "@Type", Value = PhotoType.Carousel.ToString() });

                grid.ChangeHeaderImageUrl("arrowSortOrder", "images/arrowup.gif");

                _paging.ItemsPerPage = 10;

                pageNumberer.CurrentPageIndex = 1;
                _paging.CurrentPageIndex = 1;
                ViewState["Paging"] = _paging;

                txtSortedCarousel.Text = CarouselService.GetMaxSortOrder().ToString(CultureInfo.InvariantCulture);

                //ddlMainPageModeAdd.Items.Clear();
                //ddlMainPageModeAdd.Items.Add(new ListItem(Resource.Admin_TemplateSettings_MainPageModeCommon, ECarouselPageMode.Common.ToString()));
                //ddlMainPageModeAdd.Items.Add(new ListItem(Resource.Admin_TemplateSettings_MainPageModeDefault, ECarouselPageMode.Default.ToString()));
                //ddlMainPageModeAdd.Items.Add(new ListItem(Resource.Admin_TemplateSettings_MainPageModeTwoColumns, ECarouselPageMode.TwoColumns.ToString()));
                //ddlMainPageModeAdd.Items.Add(new ListItem(Resource.Admin_TemplateSettings_MainPageModeThreeColumns, ECarouselPageMode.ThreeColumns.ToString()));
                //ddlMainPageModeAdd.SelectedIndex = 0;
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
                    else
                    {
                        _selectionFilter = null;
                    }
                }
                _paging.Fields["ID"].Filter = _selectionFilter;
            }
            else
            {
                _paging.Fields["ID"].Filter = _selectionFilter;
            }


            //----Name filter
            if (!string.IsNullOrEmpty(txtUrlFilter.Text))
            {
                var nfilter = new CompareFieldFilter { Expression = txtUrlFilter.Text, ParamName = "@URL" };
                _paging.Fields["URL"].Filter = nfilter;
            }
            else
            {
                _paging.Fields["URL"].Filter = null;
            }

            //----CurrencyValue filter
            if (!string.IsNullOrEmpty(txtSortOrder.Text))
            {
                var nfilter = new CompareFieldFilter { Expression = txtSortOrder.Text, ParamName = "@SortOrder" };
                _paging.Fields["SortOrder"].Filter = nfilter;
            }
            else
            {
                _paging.Fields["SortOrder"].Filter = null;
            }

            if (ddlEnabled.SelectedIndex != 0)
            {
                var eFilter = new EqualFieldFilter { ParamName = "@Enabled" };
                if (ddlEnabled.SelectedIndex == 1)
                {
                    eFilter.Value = "1";
                }
                if (ddlEnabled.SelectedIndex == 2)
                {
                    eFilter.Value = "0";
                }
                _paging.Fields["Enabled"].Filter = eFilter;
            }
            else
            {
                _paging.Fields["Enabled"].Filter = null;
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
                CacheManager.Remove("Carousel");
                if (!_inverseSelection)
                {
                    foreach (var id in _selectionFilter.Values)
                    {
                        CarouselService.DeleteCarousel(SQLDataHelper.GetInt(id));
                    }
                }
                else
                {
                    var itemsIds = _paging.ItemsIds<int>("CarouselID as ID");
                    foreach (int id in itemsIds.Where(id => !_selectionFilter.Values.Contains(id.ToString(CultureInfo.InvariantCulture))))
                    {
                        CarouselService.DeleteCarousel(id);
                    }
                }
            }
        }

        protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteCarousel")
            {
                CacheManager.Remove("Carousel");
                CarouselService.DeleteCarousel(SQLDataHelper.GetInt(e.CommandArgument));
            }
        }

        protected void grid_Sorting(object sender, GridViewSortEventArgs e)
        {
            var arrows = new Dictionary<string, string>
                {
                    {"URL", "arrowUrl"},
                    {"Description", "arrowDescription"},
                    {"SortOrder", "arrowSortOrder"},
                    {"Enabled", "arrowEnabled"},
                    {"DisplayInOneColumn", "arrowDisplayInOneColumn"},
                    {"DisplayInTwoColumns", "arrowDisplayInTwoColumns"},
                    {"DisplayInMobile", "arrowDisplayInMobile"},
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
                CacheManager.Remove("Carousel");
                int sortOrder = 0;
                if (int.TryParse(grid.UpdatedRow["SortOrder"], out sortOrder))
                {
                    var car = new Carousel()
                    {
                        CarouselId = SQLDataHelper.GetInt(grid.UpdatedRow["ID"]),
                        Url = grid.UpdatedRow["URL"],
                        SortOrder = sortOrder,
                        Enabled = SQLDataHelper.GetBoolean(grid.UpdatedRow["Enabled"]),
                        DisplayInOneColumn = SQLDataHelper.GetBoolean(grid.UpdatedRow["DisplayInOneColumn"]),
                        DisplayInTwoColumns = SQLDataHelper.GetBoolean(grid.UpdatedRow["DisplayInTwoColumns"]),
                        DisplayInMobile = SQLDataHelper.GetBoolean(grid.UpdatedRow["DisplayInMobile"]),
                    };
                    CarouselService.UpdateCarousel(car);

                    var photo = PhotoService.GetPhotoByObjId(car.CarouselId, PhotoType.Carousel);
                    photo.Description = grid.UpdatedRow["Description"];
                    PhotoService.UpdatePhoto(photo);
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


        protected void bthAddCarousel_Click(object sender, EventArgs e)
        {
            CacheManager.Remove("Carousel");
            lblErrorImage.Text = string.Empty;
            int sort;

            if (!CarouselLoad.HasFile)
            {
                ulValidationFailed.Visible = true;
                ulValidationFailed.InnerHtml += string.Format("<li>{0}</li>", Resource.Client_Feedback_WrongData);
                return;
            }

            if (CarouselLoad.HasFile && !FileHelpers.CheckFileExtension(CarouselLoad.FileName, EAdvantShopFileTypes.Image))
            {
                lblErrorImage.Text = Resource.Admin_ErrorMessage_WrongImageExtension;
                return;
            }

            int.TryParse(txtSortedCarousel.Text, out sort);

            if (string.IsNullOrEmpty(txtURL.Text))
                txtURL.Text = "#";

            var carousel = new Carousel
            {
                Url = txtURL.Text,
                SortOrder = sort,
                Enabled = true,
                DisplayInOneColumn = true,
                DisplayInTwoColumns = true,
                DisplayInMobile = true
            };
            int id = CarouselService.AddCarousel(carousel);

            try
            {
                if (CarouselLoad.HasFile)
                {
                    var tempName = PhotoService.AddPhoto(new Photo(0, id, PhotoType.Carousel) { OriginName = CarouselLoad.FileName });
                    if (!string.IsNullOrWhiteSpace(tempName))
                    {
                        using (System.Drawing.Image image = System.Drawing.Image.FromStream(CarouselLoad.PostedFile.InputStream))
                        {
                            FileHelpers.SaveResizePhotoFile(FoldersHelper.GetPathAbsolut(FolderType.Carousel, tempName), SettingsPictureSize.CarouselBigWidth, SettingsPictureSize.CarouselBigHeight, image);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error( "Attempt to load not image file", ex);
            }

            txtURL.Text = string.Empty;
            txtSortedCarousel.Text = string.Empty;
            TrialService.TrackEvent(TrialEvents.AddCarousel, "");
            Response.Redirect(UrlService.GetAdminAbsoluteLink("Carousel.aspx"));
        }

        protected void lbSetActive_Click(object sender, EventArgs e)
        {
            if ((_selectionFilter != null) && (_selectionFilter.Values != null))
            {
                CacheManager.Remove("Carousel");
                if (!_inverseSelection)
                {
                    foreach (var id in _selectionFilter.Values)
                    {
                        CarouselService.SetActive(SQLDataHelper.GetInt(id), true);
                    }
                }
                else
                {
                    var itemsIds = _paging.ItemsIds<int>("CarouselID as ID");
                    foreach (int id in itemsIds.Where(oId => !_selectionFilter.Values.Contains(oId.ToString())))
                    {
                        CarouselService.SetActive(SQLDataHelper.GetInt(id), true);
                    }
                }
            }
        }

        protected void lbSetNotActive_Click(object sender, EventArgs e)
        {
            if ((_selectionFilter != null) && (_selectionFilter.Values != null))
            {
                CacheManager.Remove("Carousel");
                if (!_inverseSelection)
                {
                    foreach (var id in _selectionFilter.Values)
                    {
                        CarouselService.SetActive(SQLDataHelper.GetInt(id), false);
                    }
                }
                else
                {
                    var itemsIds = _paging.ItemsIds<int>("CarouselID as ID");
                    foreach (int id in itemsIds.Where(oId => !_selectionFilter.Values.Contains(oId.ToString())))
                    {
                        CarouselService.SetActive(SQLDataHelper.GetInt(id), false);
                    }
                }
            }
        }
    }
}
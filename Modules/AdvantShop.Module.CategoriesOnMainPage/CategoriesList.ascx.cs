using System;
using System.Web.UI;
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
using AdvantShop.Module.CategoriesOnMainPage.Models;
using AdvantShop.Module.CategoriesOnMainPage.Service;
using System.Web;
using System.Web.Hosting;
using System.IO;

namespace AdvantShop.Module.CategoriesOnMainPage
{
    public partial class CategoriesList : UserControl
    {
        protected global::AdvantShop.Core.Controls.AdvGridView grid;
        protected global::AdvantShop.Core.Controls.PageNumberer pageNumberer;
        private bool _inverseSelection;
        private SqlPaging _paging;
        private InSetFieldFilter _selectionFilter;
        protected static List<ListItem> Categories;
        public string sn;
        protected void Page_Load(object sender, EventArgs e)
        {
            _inverseSelection = false;

            if (!IsPostBack)
            {
                _paging = new SqlPaging { TableName = "[Module].[CategoriesOnMainPage] INNER JOIN [Catalog].[Category] on [Category].[CategoryID] = [CategoriesOnMainPage].[CategoryId]", ItemsPerPage = 20 };

                var f = new Field { Name = "[CategoriesOnMainPage].[CategoryId] as ID", IsDistinct = true, Filter = _selectionFilter };
                _paging.AddField(f);

                f = new Field { Name = "[CategoriesOnMainPage].[Name]" };
                _paging.AddField(f);

                f = new Field { Name = "[CategoriesOnMainPage].[ImageUrl]" };
                _paging.AddField(f);

                f = new Field { Name = "[CategoriesOnMainPage].[URL]" };
                _paging.AddField(f);

                f = new Field { Name = "[CategoriesOnMainPage].[SortOrder]", Sorting = SortDirection.Ascending };
                _paging.AddField(f);
                
                //grid.ChangeHeaderImageUrl("arrowSortOrder", "images/arrowup.gif");

                _paging.ItemsPerPage = 20;

                pageNumberer.CurrentPageIndex = 1;
                _paging.CurrentPageIndex = 1;
                ViewState["Paging"] = _paging;

                var allCategoriesOnMainPageIds = COMPService.GetCategories().Select(category => category.CategoryId).ToList();
                txtSortedCategory.Text = allCategoriesOnMainPageIds.Count > 0 ? COMPService.GetMaxSortOrder().ToString(CultureInfo.InvariantCulture) : "10";

                var allCategories = COMPService.GetAllCategories();
                Categories = new List<ListItem>() { new ListItem("Не выбрана", "-1") };

                LoadAllCategories(allCategories, Categories, 0, "");

                Categories = Categories.Where(item => !allCategoriesOnMainPageIds.Contains(int.Parse(item.Value))).ToList();

                ddlAllCategories.DataSource = Categories;
                ddlAllCategories.DataBind();

                var locations = new List<ListItem>();
                locations.Add(new ListItem("Над каруселью", "false"));
                locations.Add(new ListItem("Под каруселью", "true"));

                ddlLocation.DataSource = locations;
                ddlLocation.DataBind();

                ddlLocation.SelectedValue = COMPSettings.UnderCarousel.ToString().ToLower();

                var effects = new List<ListItem>();
                effects.Add(new ListItem("Без эффекта", COMPSettings.Effects.none.ToString()));
                effects.Add(new ListItem("Увеличение", COMPSettings.Effects.scale.ToString()));
                effects.Add(new ListItem("Затемнение", COMPSettings.Effects.blackout.ToString()));
                effects.Add(new ListItem("Контраст", COMPSettings.Effects.contrast.ToString()));

                ddlEffects.DataSource = effects;
                ddlEffects.DataBind();

                ddlEffects.SelectedValue = COMPSettings.Effect;
            }
            else
            {
                _paging = (SqlPaging)(ViewState["Paging"]);
                _paging.ItemsPerPage = 20;
                //_paging.ItemsPerPage = SQLDataHelper.GetInt(ddRowsPerPage.SelectedValue);

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

        /*protected void btnFilter_Click(object sender, EventArgs e)
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
            
            pageNumberer.CurrentPageIndex = 1;
            _paging.CurrentPageIndex = 1;
        }*/

        /*protected void btnReset_Click(object sender, EventArgs e)
        {
            btnFilter_Click(sender, e);
            grid.ChangeHeaderImageUrl(null, null);
        }*/

        protected void pn_SelectedPageChanged(object sender, EventArgs e)
        {
            _paging.CurrentPageIndex = pageNumberer.CurrentPageIndex;

        }

        /*protected void linkGO_Click(object sender, EventArgs e)
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
        }*/

        protected void lbDeleteSelected_Click(object sender, EventArgs e)
        {
            if ((_selectionFilter != null) && (_selectionFilter.Values != null))
            {
                CacheManager.Remove("CategoriesOnMainPage");
                if (!_inverseSelection)
                {
                    foreach (var id in _selectionFilter.Values)
                    {
                        COMPService.DeleteCategory(SQLDataHelper.GetInt(id));
                    }
                }
                else
                {
                    var itemsIds = _paging.ItemsIds<int>("[CategoryId] as ID");
                    foreach (int id in itemsIds.Where(id => !_selectionFilter.Values.Contains(id.ToString(CultureInfo.InvariantCulture))))
                    {
                        COMPService.DeleteCategory(id);
                    }
                }
            }


        }

        protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteCategory")
            {
                CacheManager.Remove("CategoriesOnMainPage");
                COMPService.DeleteCategory(SQLDataHelper.GetInt(e.CommandArgument));
            }
        }

        protected void grid_Sorting(object sender, GridViewSortEventArgs e)
        {
            var arrows = new Dictionary<string, string>
                {
                    //{"URL", "arrowUrl"},
                    //{"SortOrder", "arrowSortOrder"},
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
                CacheManager.Remove("CategoriesOnMainPage");
                int sortOrder = 0;
                if (int.TryParse(grid.UpdatedRow["SortOrder"], out sortOrder))
                {
                    var categoryId = SQLDataHelper.GetInt(grid.UpdatedRow["ID"]);
                    //var imageUrl = string.Format("Category_{0}.jpg", categoryId);
                    /*var categoryOnMainPage = new CategoryOnMainPage
                    {
                        CategoryId = categoryId,
                        URL = grid.UpdatedRow["URL"],
                        SortOrder = sortOrder
                    };*/

                    var categoryOnMainPage = COMPService.GetCategory(categoryId);
                    
                    if(categoryOnMainPage != null)
                    {
                        categoryOnMainPage.URL = grid.UpdatedRow["URL"];
                        categoryOnMainPage.SortOrder = sortOrder;
                        categoryOnMainPage.Name = grid.UpdatedRow["Name"];
                        COMPService.UpdateCategory(categoryOnMainPage);
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

            /*if (data.Rows.Count < 1)
            {
                goToPage.Visible = false;
            }*/

            grid.DataSource = data;
            grid.DataBind();

            pageNumberer.PageCount = _paging.PageCount;
            lblFound.Text = _paging.TotalRowsCount.ToString();

            txtPicturesQuantityInLine.Text = COMPSettings.PicturesQuantityInLine.ToString();
            txtImageWidth.Text = COMPSettings.ImageWidth.ToString();
            txtImageHeight.Text = COMPSettings.ImageHeight.ToString();
            chbNewWindow.Checked = COMPSettings.NewWindow;
            chbNoShowCategoryName.Checked = COMPSettings.NoShowCategoryName;
            chbNoShowBorder.Checked = COMPSettings.NoShowBorder;

            var allCategoriesOnMainPageIds = COMPService.GetCategories().Select(category => category.CategoryId).ToList();
            txtSortedCategory.Text = allCategoriesOnMainPageIds.Count > 0 ? COMPService.GetMaxSortOrder().ToString(CultureInfo.InvariantCulture) : "10";
        }


        protected void bthAddCategory_Click(object sender, EventArgs e)
        {
            CacheManager.Remove("CategoriesOnMainPage");
            lblErrorImage.Text = string.Empty;
            int sort;

            if (!CategoryPictureLoad.HasFile)
            {
                ulValidationFailed.Visible = true;
                ulValidationFailed.InnerHtml += string.Format("<li>{0}</li>", "Некорректные данные");
                return;
            }

            if (CategoryPictureLoad.HasFile && !FileHelpers.CheckFileExtension(CategoryPictureLoad.FileName, EAdvantShopFileTypes.Image))
            {
                lblErrorImage.Text = "Несоответствующее расширение файла.";
                return;
            }

            int.TryParse(txtSortedCategory.Text, out sort);

            var categoryId = 0;

            int.TryParse(ddlAllCategories.SelectedValue, out categoryId);
            if(categoryId <= 0)
            {
                lblErrorImage.Text = "Неправильный выбор категории.";
                return;
            }

            if (string.IsNullOrEmpty(txtURL.Text))
            {
                var url = COMPService.GetCategoryUrlById(categoryId);
                txtURL.Text = !string.IsNullOrEmpty(url) ? string.Format("categories/{0}", url) : "#";
            }

            //получаем название категории
            var oldCategory = COMPService.GetCategory(categoryId);
            if (oldCategory == null)
            {
                var categoryName = COMPService.GetCategoryNameByCategoryId(categoryId);
                var categoryOnMainPage = new CategoryOnMainPage
                {
                    CategoryId = categoryId,
                    Name = categoryName,
                    URL = txtURL.Text,
                    SortOrder = sort,
                };

                COMPService.AddCategory(categoryOnMainPage);
            }

            var imageUrl = oldCategory == null ? string.Empty : oldCategory.ImageUrl;
            if(!string.IsNullOrEmpty(imageUrl))
            {
                CategoryImageService.RemovePicture(categoryId, true);
            }

            try
            {
                if (CategoryPictureLoad.HasFile)
                {
                    imageUrl = !string.IsNullOrEmpty(CategoryPictureLoad.FileName) ? CategoryPictureLoad.FileName : string.Format("Category{0}{1}", categoryId, Path.GetExtension(CategoryPictureLoad.FileName));
                    if (!string.IsNullOrWhiteSpace(imageUrl))
                    {
                        using (System.Drawing.Image image = System.Drawing.Image.FromStream(CategoryPictureLoad.FileContent))
                        {
                            var filePath = COMPService.GetPath("modules/CategoriesOnMainPage/Pictures/");
                            COMPService.GetPath("modules/CategoriesOnMainPage/Pictures/Original/");

                            if (!Directory.Exists(filePath))
                            {
                                Directory.CreateDirectory(filePath);
                            }

                            if (!Directory.Exists(filePath + "/Original"))
                            {
                                Directory.CreateDirectory(filePath + "/Original");
                            }

                            var totalFileName = imageUrl;
                            while (System.IO.File.Exists(filePath + totalFileName))
                            {
                                var lastIndexDot = totalFileName.LastIndexOf('.');
                                totalFileName = totalFileName.Insert(lastIndexDot, "n");
                            }

                            FileHelpers.SaveResizePhotoFile(filePath + totalFileName, COMPSettings.ImageWidth, COMPSettings.ImageHeight, image);
                            FileHelpers.SaveResizePhotoFile(filePath + "/Original/" + totalFileName, 0, 0, image);

                            CategoryImageService.AddImage(categoryId, totalFileName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error("Attempt to load not image file", ex);
                SetMessage("Не удалось загрузить изображение", System.Drawing.Color.Red, true);
            }

            txtURL.Text = string.Empty;
            txtSortedCategory.Text = string.Empty;
        }
        
        private void LoadAllCategories(List<Category> categories, List<ListItem> list, int categoryId, string offset)
        {
            foreach (var category in categories.Where(c => c.ParentCategoryId == categoryId).OrderBy(c => c.SortOrder).ToList())
            {
                list.Add(new ListItem(HttpUtility.HtmlDecode(offset + category.Name), category.CategoryId.ToString()));

                if (categories.Any(c => c.ParentCategoryId == category.CategoryId))
                {
                    LoadAllCategories(categories, list, category.CategoryId, offset + "&nbsp;&nbsp;");
                }
            }
        }


        protected bool Validate()
        {
            int picturesQuantityInLine = 0;
            int imageWidth = 0;
            int imageHeight = 0;

            var valid = int.TryParse(txtPicturesQuantityInLine.Text, out picturesQuantityInLine) && picturesQuantityInLine > 0;
            if (!valid) { SetMessage("Некорректное количество изображений в строке", System.Drawing.Color.Red, true); return false; }

            valid &= int.TryParse(txtImageWidth.Text, out imageWidth) && imageWidth > 0 && imageWidth <= 1000;
            if (!valid) { SetMessage("Некорректная ширина изображения", System.Drawing.Color.Red, true); return false; }

            valid &= int.TryParse(txtImageHeight.Text, out imageHeight) && imageHeight > 0 && imageHeight <= 1000;
            if (!valid) { SetMessage("Некорректная высота изображения", System.Drawing.Color.Red, true); return false; }
            
            return valid;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Validate())
            {
                return;
            }

            COMPSettings.PicturesQuantityInLine = int.Parse(txtPicturesQuantityInLine.Text);
            COMPSettings.ImageWidth = int.Parse(txtImageWidth.Text);
            COMPSettings.ImageHeight = int.Parse(txtImageHeight.Text);
            COMPSettings.NewWindow = chbNewWindow.Checked;
            COMPSettings.NoShowCategoryName = chbNoShowCategoryName.Checked;
            COMPSettings.NoShowBorder = chbNoShowBorder.Checked;

            COMPSettings.UnderCarousel = Convert.ToBoolean(ddlLocation.SelectedValue);
            COMPSettings.Effect = ddlEffects.SelectedValue;

            SetMessage("Изменения сохранены", System.Drawing.Color.Blue, true);
        }

        private void SetMessage(string message, System.Drawing.Color color, bool visible)
        {
            lblMessage.Text = message;
            lblMessage.ForeColor = color;
            lblMessage.Visible = visible;
        }

        protected void btnResizeImages_Click(object sender, EventArgs e)
        {
            try
            {
                var categories = COMPService.GetCategories();

                foreach (var category in categories)
                {
                    if (string.IsNullOrEmpty(category.ImageUrl)) { continue; }

                    var originalFilePath = COMPService.GetPath("modules/CategoriesOnMainPage/Pictures/Original/" + category.ImageUrl);
                    var originalFile = File.Open(originalFilePath, FileMode.Open);

                    try
                    {
                        using (System.Drawing.Image image = System.Drawing.Image.FromStream(originalFile))
                        {
                            var filePath = COMPService.GetPath("modules/CategoriesOnMainPage/Pictures/");

                            if (!Directory.Exists(filePath))
                            {
                                Directory.CreateDirectory(filePath);
                            }

                            FileHelpers.SaveResizePhotoFile(filePath + category.ImageUrl, COMPSettings.ImageWidth, COMPSettings.ImageHeight, image);
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.Log.Error(string.Format("Не удалось загрузить изображение для категории: {0}. Причина: {1}", category.Name, ex.Message));
                    }
                }

                SetMessage("Изображения пережаты.", System.Drawing.Color.Blue, true);
            }
            catch(Exception ex)
            {
                Debug.Log.Error(ex.Message);
                SetMessage("Пережатие сейчас недоступно, попробуйте позже.", System.Drawing.Color.Blue, true);
            }
        }
    }
}
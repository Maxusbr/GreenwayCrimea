using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.FilePath;
using AdvantShop.Module.Journal.Domain;
using AdvantShop.Module.Journal.Services;

namespace AdvantShop.Module.Journal
{
    public partial class JournalSettings : System.Web.UI.UserControl
    {
        private const string PdfPath = "~/Modules/Journal/Pdfs";

        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Visible = false;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            lblNotice.Text = "Максимальное количество страниц для одного журнала: " + JournalService.MaxPageCount;
            chkShowJournalCover.Checked = JournalModuleSetting.ShowCover;
            
            var coverType = JournalModuleSetting.CoverType;
            if (ddlCoverType.Items.FindByValue(coverType.ToString()) != null)
            {
                ddlCoverType.SelectedValue = coverType.ToString();
            }

            var logo = !string.IsNullOrEmpty(SettingsMain.LogoImageName)
                                ? string.Format("<img src=\"{0}\" width=\"height:55px\" />", FoldersHelper.GetPath(FolderType.Pictures, SettingsMain.LogoImageName, false))
                                : string.Empty;

            var coverTop = JournalModuleSetting.CoverTop;
            if (string.IsNullOrWhiteSpace(coverTop))
            {
                coverTop = string.Format("<div> <div class=\"cover-top-left\">{0}</div><div class=\"cover-top-right\">demo.advantshop.net<br> +7 (495) 800 200 10</div> </div>", logo);
            }
            ckeCoverTop.Text = coverTop;

            var coverMiddle = JournalModuleSetting.CoverMiddle;
            if (string.IsNullOrWhiteSpace(coverMiddle))
            {
                coverMiddle = "<div style=\"font-size: 44px;font-weight: bold;\">Каталог</div> интернет-магазина<br/> demo.advantshop.net";
            }
            ckeCoverMiddle.Text = coverMiddle;

            var coverBottom = JournalModuleSetting.CoverBottom;
            if (string.IsNullOrWhiteSpace(coverBottom))
            {
                coverBottom = "Март 2015";
            }
            ckeCoverBottom.Text = coverBottom;

            var productType = JournalModuleSetting.ViewMode;
            if (ddlCoverProductType.Items.FindByValue(productType.ToString()) != null)
            {
                ddlCoverProductType.SelectedValue = productType.ToString();
            }

            var headText = JournalModuleSetting.CatalogPageHead;
            if (string.IsNullOrWhiteSpace(headText))
            {
                
                headText = string.Format("<div> <div class=\"header-left\">{0}</div><div class=\"header-right\">demo.advantshop.net</div> </div>", logo);
            }

            ckeCatalogPageHead.Text = headText;
            txtCatalogPageBottomLeft.Text = JournalModuleSetting.CatalogPageBottomLeft ?? "Подвал на странице каталога слева";
            txtCatalogPageBottomRight.Text = JournalModuleSetting.CatalogPageBottomRight ?? "Подвал на странице каталога справа";

            ddlCategories.DataSource = FillCategories(0);
            ddlCategories.DataBind();

            FillCategoriesLabel();
            GetJournalPdfs();

            chkShowArtNo.Checked = JournalModuleSetting.ShowArtNo;
            chkShowOnlyAvailable.Checked = JournalModuleSetting.ShowOnlyAvalible;
            chkMoveNotAvaliableToEnd.Checked = JournalModuleSetting.MoveNotAvaliableToEnd;

            //if (TrialService.IsTrialEnabled)
            //{
            //    btnMakeMagic.Attributes.Add("disabled", "true");
            //}
        }

        protected void SaveSettings()
        {
            JournalModuleSetting.ShowCover = chkShowJournalCover.Checked;
            JournalModuleSetting.CoverType = ddlCoverType.SelectedValue.TryParseInt();
            JournalModuleSetting.CoverTop = ckeCoverTop.Text;
            JournalModuleSetting.CoverMiddle = ckeCoverMiddle.Text;
            JournalModuleSetting.CoverBottom = ckeCoverBottom.Text;


            JournalModuleSetting.ViewMode = ddlCoverProductType.SelectedValue.TryParseInt();

            JournalModuleSetting.CatalogPageHead = ckeCatalogPageHead.Text;
            JournalModuleSetting.CatalogPageBottomLeft = txtCatalogPageBottomLeft.Text;
            JournalModuleSetting.CatalogPageBottomRight = txtCatalogPageBottomRight.Text;

            JournalModuleSetting.ItemsPerPage = (JournalViewMode) JournalModuleSetting.ViewMode == JournalViewMode.List
                                                    ? 3
                                                    : 6;

            JournalModuleSetting.ShowArtNo = chkShowArtNo.Checked;
            JournalModuleSetting.ShowOnlyAvalible = chkShowOnlyAvailable.Checked;
            JournalModuleSetting.MoveNotAvaliableToEnd = chkMoveNotAvaliableToEnd.Checked;
            
            lblMessage.Text = (string)GetLocalResourceObject("ChangesSaved");
            lblMessage.Visible = true;
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }

        protected void btnMakeMagic_Click(object sender, EventArgs e)
        {
            if (!IsValidToExport())
                return;

            SaveSettings();

            var fileName = string.Format(PdfPath + "/catalog_{0}.pdf", DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss"));
            var filePath = HttpContext.Current.Server.MapPath(fileName);
            if (File.Exists(filePath))
                File.Delete(filePath);

            var isSuccess = JournalService.ExportToPdf(filePath);
            if (!isSuccess || !File.Exists(filePath))
            {
                ShowErrors("Не удалось экспортировать в pdf");
            }
        }

        protected void btnMakeMagicPreview_Click(object sender, EventArgs e)
        {
            if (!IsValidToExport())
                return;

            SaveSettings();

            var fileName = string.Format(PdfPath + "/Preview/catalog_preview_{0}.pdf", DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss"));
            var filePath = HttpContext.Current.Server.MapPath(fileName);
            if (File.Exists(filePath))
                File.Delete(filePath);

            var isSuccess = JournalService.ExportToPdf(filePath, 5);

            if (!isSuccess || !File.Exists(filePath))
            {
                ShowErrors("Не удалось экспортировать в pdf");
                return;
            }

            var fileInfo = new FileInfo(filePath);
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpContext.Current.Server.UrlPathEncode(fileInfo.Name));
            HttpContext.Current.Response.AddHeader("Content-Length", fileInfo.Length.ToString());
            HttpContext.Current.Response.ContentType = "application/octet-stream";
            HttpContext.Current.Response.TransmitFile(fileInfo.FullName);
        }
        

        private List<ListItem> FillCategories(int categoryId, string offset = "")
        {
            var list = new List<ListItem>();

            var subCategories = CategoryService.GetChildCategoriesByCategoryId(categoryId, false);
            foreach (var category in subCategories)
            {
                list.Add(new ListItem(offset + category.Name, category.CategoryId.ToString()));
                list.AddRange(FillCategories(category.CategoryId, offset + "---"));
            }

            return list;
        }

        private void FillCategoriesLabel()
        {
            var export = JournalModuleSetting.JournalExport;
            var categories = export.CategoryIds.Select(CategoryService.GetCategory).Where(c => c != null).ToList();

            lvCategories.DataSource = categories;
            lvCategories.DataBind();

            hlPreviewHtml.Visible = false;

            if (categories.Count > 0)
            {
                var cat = categories.FirstOrDefault(x => JournalService.CategoryHasProducts(x.CategoryId));
                if (cat != null)
                {
                    hlPreviewHtml.Visible = true;
                    hlPreviewHtml.NavigateUrl =
                        UrlService.GetUrl(
                            string.Format("journal/pdf?page=1&categoryId={0}&isLeft=true&pageIndex=1&categoryName={1}",
                                cat.CategoryId, cat.Name));
                }
            }
        }

        private void GetJournalPdfs()
        {
            var directory = new DirectoryInfo(HttpContext.Current.Server.MapPath(PdfPath));
            lvPdfs.DataSource =
                directory.GetFiles("*.pdf")
                    .OrderByDescending(x => x.LastWriteTime)
                    .Take(5)
                    .Select(file => new ListItem(file.Name))
                    .ToList();

            lvPdfs.DataBind();
        }
        
        protected void lbAddCategory_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ddlCategories.SelectedValue))
                return;

            var export = JournalModuleSetting.JournalExport ?? new JournalExport();

            var id = ddlCategories.SelectedValue.TryParseInt();
            if (id != 0)
            {
                var category = CategoryService.GetCategory(id);
                if (category != null && !export.CategoryIds.Contains(category.CategoryId))
                {
                    export.CategoryIds.Add(category.CategoryId);
                }
            }

            JournalModuleSetting.JournalExport = export;
            FillCategoriesLabel();
        }

        protected void lvCategories_OnItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "RemoveCategory")
            {
                var categoryId = e.CommandArgument.ToString().TryParseInt();
                if (categoryId != 0)
                {
                    var export = JournalModuleSetting.JournalExport;
                    if (export.CategoryIds.Contains(categoryId))
                        export.CategoryIds.Remove(categoryId);

                    JournalModuleSetting.JournalExport = export;
                }
            }
        }

        private bool IsValidToExport()
        {
            HideErrors();

            var categoryIds = JournalModuleSetting.JournalExport.CategoryIds;
            if (categoryIds.Count == 0)
            {
                ShowErrors("Ошибка: Не выбрано ни одной категории");
                return false;
            }

            return true;
        }

        private void ShowErrors(string error)
        {
            errorDiv.Visible = true;
            lblError.Text = error;
        }


        private void HideErrors()
        {
            errorDiv.Visible = false;
            lblError.Text = "";
        }
    }
}
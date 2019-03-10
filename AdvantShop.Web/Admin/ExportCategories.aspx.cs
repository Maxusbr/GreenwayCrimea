//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.ExportImport;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.Statistic;
using Resources;

namespace Admin
{
    public partial class ExportCategories : AdvantShopAdminPage
    {
        protected readonly string fileName = "export_categories";
        protected List<CategoryFields> FieldMapping = new List<CategoryFields>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FieldMapping = ExportFeedCsvCategorySettings.FieldMapping ?? new List<CategoryFields>();
            }

            LoadFirstCategory();
            LoadFirstCategoryTable();
            
            if (!IsPostBack)
            {
                LoadSettings();
            }

            linkCancel.Visible = CommonStatistic.IsRun;
        }


        protected void Page_PreRender(object sender, EventArgs e)
        {
            LoadSelectedValues();

            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_ExportCategories_ExportCategories));
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            if (lError.Visible) return;
            if (CommonStatistic.IsRun) return;

            //delete old
            foreach (var item in Directory.GetFiles(FoldersHelper.GetPathAbsolut(FolderType.PriceTemp)).Where(f => f.Contains(fileName)))
            {
                FileHelpers.DeleteFile(item);
            }

            divAction.Visible = false;
            divbtnAction.Visible = false;
            choseDiv.Visible = false;
            linkCancel.Visible = true;
            OutDiv.Visible = true;
            btnDownload.Visible = false;

            GetFieldMapping();

            ExportFeedCsvCategorySettings.CsvEnconing = ddlEncoding.SelectedValue;
            ExportFeedCsvCategorySettings.CsvSeparator = ddlSeparators.SelectedValue == SeparatorsEnum.Custom.StrName() ? txtCustomSeparator.Text : ddlSeparators.SelectedValue;
            ExportFeedCsvCategorySettings.FieldMapping = FieldMapping;

            var exportFeedCsvOptions = new ExportFeedCsvOptions()
            {
                FileName = FoldersHelper.GetPathRelative(FolderType.PriceTemp, fileName, false),
                FileExtention = "csv",
                CsvEnconing = ExportFeedCsvCategorySettings.CsvEnconing,
                CsvSeparator = ExportFeedCsvCategorySettings.CsvSeparator,
            };
            

            CommonStatistic.Init();
            CommonStatistic.CurrentProcess = Request.Url.PathAndQuery;
            CommonStatistic.CurrentProcessName = "Export categories";
            CommonStatistic.FileName = "../" + fileName;

            CsvExportCategories.Factory(
                ExportFeedCsvCategoryService.GetCsvCategories(FieldMapping),
                exportFeedCsvOptions,
                FieldMapping,
                ExportFeedCsvCategoryService.GetCsvCategoriesCount()
                ).Process().Wait();
        }

        protected void linkCancel_Click(object sender, EventArgs e)
        {
            CommonStatistic.IsRun = false;
            CommonStatistic.Init();
            linkCancel.Visible = false;
        }


        #region Private methods

        private void LoadFirstCategoryTable()
        {
            var tbl = new Table { ID = "tblValues", CssClass = "export-tb table-values" };
            var ddlRow = new TableHeaderRow();
            ddlRow.Cells.Add(new TableHeaderCell { Text = Resource.Admin_ExportCsv_Column });
            ddlRow.Cells.Add(new TableHeaderCell { Text = Resource.Admin_ExportCsv_SampleOfData });
            tbl.Rows.Add(ddlRow);
            
            foreach (CategoryFields item in Enum.GetValues(typeof(CategoryFields)))
            {
                if (item == CategoryFields.None)
                    continue;

                var enumIntStringTemp = item.ConvertIntString();

                var cell = new TableCell { ID = "cell" + enumIntStringTemp };
                var ddl = new DropDownList { ID = "ddlType" + ((int)item), Width = 150 };
                ddl.Items.AddRange(
                    (from CategoryFields eItem in Enum.GetValues(typeof (CategoryFields))
                        select new ListItem(eItem.Localize(), eItem.ConvertIntString())).ToArray());
                
                cell.Controls.Add(ddl);

                var cellLbl = new TableCell { ID = "cellLbl" + enumIntStringTemp };
                var lb = new Label {ID = "lbProduct" + ((int) item), Text = ""};
                cellLbl.Controls.Add(lb);

                tbl.Rows.Add(new TableRow() { Cells = { cell, cellLbl } });
            }

            choseDiv.Controls.Add(tbl);
        }

        private void LoadFirstCategory()
        {
            var fields = Enum.GetValues(typeof (CategoryFields)).Cast<CategoryFields>().ToList();

            var category = ExportFeedCsvCategoryService.GetCsvCategories(fields).Where(x => x.CategoryId != "0").Take(1).FirstOrDefault();
            if (category == null)
                return;

            foreach (CategoryFields item in fields)
            {
                var itemText = string.Empty;

                if (item == CategoryFields.CategoryId)
                    itemText = category.CategoryId;

                else if (item == CategoryFields.Name)
                    itemText = category.Name;

                else if (item == CategoryFields.Slug)
                    itemText = category.Slug;

                else if (item == CategoryFields.ParentCategory)
                    itemText = category.ParentCategory;

                else if (item == CategoryFields.SortOrder)
                    itemText = category.SortOrder;

                else if (item == CategoryFields.Enabled)
                    itemText = category.Enabled;

                else if (item == CategoryFields.Hidden)
                    itemText = category.Hidden;

                else if (item == CategoryFields.BriefDescription)
                    itemText = category.BriefDescription;

                else if (item == CategoryFields.Description)
                    itemText = category.Description;

                else if (item == CategoryFields.DisplayStyle)
                    itemText = category.DisplayStyle;

                else if (item == CategoryFields.Sorting)
                    itemText = category.Sorting;

                else if (item == CategoryFields.DisplayBrandsInMenu)
                    itemText = category.DisplayBrandsInMenu;

                else if (item == CategoryFields.DisplaySubCategoriesInMenu)
                    itemText = category.DisplaySubCategoriesInMenu;

                else if (item == CategoryFields.Tags)
                    itemText = category.Tags;

                else if (item == CategoryFields.Picture)
                    itemText = category.Picture;

                else if (item == CategoryFields.MiniPicture)
                    itemText = category.MiniPicture;

                else if (item == CategoryFields.Icon)
                    itemText = category.Icon;

                else if (item == CategoryFields.Title)
                    itemText = category.Title;

                else if (item == CategoryFields.H1)
                    itemText = category.H1;

                else if (item == CategoryFields.MetaKeywords)
                    itemText = category.MetaKeywords;

                else if (item == CategoryFields.MetaDescription)
                    itemText = category.MetaDescription;

                else if (item == CategoryFields.PropertyGroups)
                    itemText = category.PropertyGroups;

                ddlProduct.Items.Add(new ListItem(itemText ?? "", item.ConvertIntString()));
            }
        }


        private void GetFieldMapping()
        {
            var table = (Table)choseDiv.FindControl("tblValues");

            if (table == null)
                return;

            foreach (TableRow item in table.Rows)
            {
                var element = item.Cells[0].Controls.OfType<DropDownList>().FirstOrDefault();
                if (element == null) continue;

                if (element.SelectedValue == CategoryFields.None.ConvertIntString())
                    continue;

                CategoryFields currentField;
                if (Enum.TryParse(element.SelectedValue, out currentField))
                {
                    if (!FieldMapping.Contains(currentField))
                    {
                        FieldMapping.Add(currentField);
                    }
                }
            }



            if (FieldMapping.Count == 0 && IsPostBack)
            {
                //MsgErr(Resource.Admin_ExportCsv_ListEmpty);
                return;
            }
        }

        private void LoadSelectedValues()
        {
            var table = (Table)choseDiv.FindControl("tblValues");

            if (table == null)
                return;

            var i = 0;
            foreach (TableRow item in table.Rows)
            {
                var element = item.Cells[0].Controls.OfType<DropDownList>().FirstOrDefault();
                var label = item.Cells[1].Controls.OfType<Label>().FirstOrDefault();
                if (element == null) continue;

                if (i < FieldMapping.Count && element.Items.FindByValue(FieldMapping[i].ConvertIntString()) != null)
                {
                    element.SelectedValue = FieldMapping[i].ConvertIntString();
                }

                if (label != null && element.SelectedValue != "0" && ddlProduct.Items.Count > 0)
                {
                    label.Text = ddlProduct.Items.FindByValue(element.SelectedValue).Text;
                }

                i++;
            }
        }

        private void LoadSettings()
        {
            ddlEncoding.Items.Clear();
            foreach (EncodingsEnum enumItem in Enum.GetValues(typeof(EncodingsEnum)))
            {
                ddlEncoding.Items.Add(new ListItem(enumItem.ToString(), enumItem.StrName()));
            }

            ddlEncoding.SelectedValue = !string.IsNullOrEmpty(ExportFeedCsvCategorySettings.CsvEnconing) &&
                                        ddlEncoding.Items.FindByValue(ExportFeedCsvCategorySettings.CsvEnconing) != null
                ? ExportFeedCsvCategorySettings.CsvEnconing
                : EncodingsEnum.Utf8.StrName();

            ddlSeparators.Items.Clear();
            foreach (var enumItem in (SeparatorsEnum[])Enum.GetValues(typeof(SeparatorsEnum)))
            {
                ddlSeparators.Items.Add(new ListItem(enumItem.Localize(), enumItem.StrName()));
            }

            if (!string.IsNullOrEmpty(ExportFeedCsvCategorySettings.CsvSeparator))
            {
                foreach (SeparatorsEnum enumItem in Enum.GetValues(typeof(SeparatorsEnum)))
                {
                    if (ExportFeedCsvCategorySettings.CsvSeparator == enumItem.StrName())
                        ddlSeparators.SelectedValue = enumItem.StrName();

                    if (ExportFeedCsvCategorySettings.CsvSeparator == SeparatorsEnum.Custom.StrName())
                        txtCustomSeparator.Text = ExportFeedCsvCategorySettings.CsvSeparator;
                }
            }
            else
            {
                ddlSeparators.SelectedValue = SeparatorsEnum.SemicolonSeparated.StrName();
            }
        }

        #endregion
    }
}
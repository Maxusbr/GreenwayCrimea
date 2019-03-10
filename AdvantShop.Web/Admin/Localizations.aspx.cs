//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web.UI.WebControls;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Core.SQL;
using AdvantShop.Diagnostics;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using CsvHelper.Configuration;
using Resources;

namespace Admin
{
    public partial class Localizations : AdvantShopAdminPage
    {
        #region Fields

        private bool _inverseSelection;
        private SqlPaging _paging;
        private InSetFieldFilter _selectionFilter;

        private int LanguageId
        {
            get
            {
                var languageId = 0;
                var langs = LanguageService.GetList();

                if (Request["lang"] != null)
                {
                    var langId = Request["lang"].TryParseInt();

                    var lang = langs.FirstOrDefault(x => x.LanguageId == langId);
                    if (lang != null)
                        languageId = lang.LanguageId;
                }

                if (languageId == 0)
                {
                    var lang = langs.FirstOrDefault(x => x.LanguageCode == Thread.CurrentThread.CurrentUICulture.Name);
                    languageId = lang != null ? lang.LanguageId : langs.FirstOrDefault().LanguageId;
                }
                return languageId;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            lblError.Visible = false;
            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_Localizations_Header));

            if (!IsPostBack)
            {
                _paging = new SqlPaging
                {
                    TableName = "[Settings].[Localization]",
                    ItemsPerPage = 50,
                    CurrentPageIndex = 1,
                };

                _paging.AddFieldsRange(new[]
                {
                    new Field {Name = "LanguageId"},
                    new Field {Name = "ResourceKey"},
                    new Field {Name = "ResourceValue"},
                });

                _paging.Fields["LanguageId"].Filter = new EqualFieldFilter() { ParamName = "@LanguageId", Value = LanguageId.ToString() };
                
                //grid.ChangeHeaderImageUrl("arrowResourceKey", "images/arrowup.gif");
                pageNumberer.CurrentPageIndex = 1;
                ViewState["Paging"] = _paging;
            }
            else
            {
                _paging = (SqlPaging)(ViewState["Paging"]);
                _paging.ItemsPerPage = SQLDataHelper.GetInt(ddRowsPerPage.SelectedValue);

                if (_paging == null)
                    throw (new Exception("Paging lost"));

                string strIds = Request.Form["SelectedIds"];
                if (!string.IsNullOrEmpty(strIds))
                {
                    strIds = strIds.Trim();

                    var arrids = strIds.Split(' ');
                    var ids = new string[arrids.Length];

                    _selectionFilter = new InSetFieldFilter { IncludeValues = true };
                    for (int idx = 0; idx <= ids.Length - 1; idx++)
                    {
                        int t = int.Parse(arrids[idx]);
                        if (t != -1)
                        {
                            ids[idx] = t.ToString(CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            _selectionFilter.IncludeValues = false;
                            _inverseSelection = true;
                        }
                    }
                    _selectionFilter.Values = ids;
                }
            }

            _paging.ExtensionWhere = !chkShowAllResources.Checked
                                        ? " and ResourceKey not like 'Admin.%' and ResourceKey not like 'Core.%' "
                                        : "";
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            ddlLangs.DataSource = LanguageService.GetList();
            ddlLangs.DataBind();

            if (ddlLangs.Items.FindByValue(LanguageId.ToString()) != null)
            {
                ddlLangs.SelectedValue = LanguageId.ToString();
            }


            if (grid.UpdatedRow != null)
            {
                var languageId = SQLDataHelper.GetInt(grid.UpdatedRow["LanguageId"]);
                var resourceKey = SQLDataHelper.GetString(grid.UpdatedRow["ResourceKey"]);
                var resourceValue = SQLDataHelper.GetString(grid.UpdatedRow["ResourceValue"]);

                LocalizationService.AddOrUpdateResource(languageId, resourceKey, resourceValue);

                if (resourceKey.ToLower().StartsWith("js."))
                    LocalizationService.GenerateJsResourcesFile();
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
                    if (Array.Exists(_selectionFilter.Values, c => c == (data.Rows[intIndex]["ID"]).ToString()))
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
            lblFound.Text = _paging.TotalRowsCount.ToString(CultureInfo.InvariantCulture);
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            _paging.Fields["ResourceKey"].Filter = !string.IsNullOrEmpty(txtKey.Text)
                ? new CompareFieldFilter() {Expression = txtKey.Text, ParamName = "@key"}
                : null;

            _paging.Fields["ResourceValue"].Filter = !string.IsNullOrEmpty(txtValue.Text)
                ? new CompareFieldFilter() { Expression = txtValue.Text, ParamName = "@value" }
                : null;

            pageNumberer.CurrentPageIndex = 1;
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
        }


        protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "AddResource")
            {

                GridViewRow footer = grid.FooterRow;

                var key = ((TextBox) footer.FindControl("txtNewKey")).Text;
                var value = ((TextBox) footer.FindControl("txtNewValue")).Text;

                if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
                {
                    grid.FooterStyle.BackColor = System.Drawing.Color.FromName("#ffcccc");
                    return;
                }
                
                LocalizationService.AddOrUpdateResource(
                    LanguageId,
                    key.Trim(),
                    value.Trim());

                if (key.ToLower().StartsWith("js."))
                    LocalizationService.GenerateJsResourcesFile();

                grid.ShowFooter = false;
            }


            if (e.CommandName == "CancelAdd")
            {
                grid.FooterStyle.BackColor = System.Drawing.Color.FromName("#ccffcc");
                grid.ShowFooter = false;
            }
        }

        protected void grid_Sorting(object sender, GridViewSortEventArgs e)
        {
            var arrows = new Dictionary<string, string>
            {
                {"ResourceKey", "arrowResourceKey"},
                {"ResourceValue", "arrowResourceValue"},
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

        private void grid_DataBound(object sender, EventArgs e)
        {
            if (grid.ShowFooter)
            {
                grid.FooterRow.FindControl("txtNewKey").Focus();
            }
        }

        protected void btnAddResource_Click(object sender, EventArgs e)
        {
            grid.ShowFooter = true;
            grid.FooterStyle.BackColor = System.Drawing.Color.FromName("#ccffcc");
            grid.DataBound += grid_DataBound;
        }

        private void MsgErr(string messageText)
        {
            lblError.Visible = true;
            lblError.Text = @"<br/>" + messageText;
        }

        protected void ddlLangs_SelectedIndexChanged(object sender, EventArgs e)
        {
            Response.Redirect("Localizations.aspx?lang=" + ddlLangs.SelectedValue);
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            var language = LanguageService.GetLanguage(ddlLangs.SelectedValue.TryParseInt());
            var fileName = "localization.csv".FileNamePlusDate();
            var fileDirectory = FoldersHelper.GetPathAbsolut(FolderType.PriceTemp);
            
            if (!Directory.Exists(fileDirectory))
                Directory.CreateDirectory(fileDirectory);

            using (var csvWriter = new CsvHelper.CsvWriter(new StreamWriter(fileDirectory + fileName, false, Encoding.UTF8), new CsvConfiguration(){Delimiter = ";"}))
            {
                foreach (var item in new[] { "ResourceKey", "ResourceValue", "LanguageCode" })
                    csvWriter.WriteField(item);

                csvWriter.NextRecord();

                foreach (var resource in LocalizationService.GetResources(language.LanguageCode))
                {
                    csvWriter.WriteField(resource.Key);
                    csvWriter.WriteField(resource.Value);
                    csvWriter.WriteField(language.LanguageCode);

                    csvWriter.NextRecord();
                }
            }

            CommonHelper.WriteResponseFile(fileDirectory + fileName, fileName);
        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            if (!fuImportFile.HasFile)
                return;

            var filePath = FoldersHelper.GetPathAbsolut(FolderType.PriceTemp);
            var fileName = "localizationImport.csv";
            var fullFileName = filePath + fileName.FileNamePlusDate();

            try
            {
                FileHelpers.CreateDirectory(filePath);

                fuImportFile.SaveAs(fullFileName);

                using (var csvReader = new CsvHelper.CsvReader(new StreamReader(fullFileName), new CsvConfiguration() { Delimiter = ";" }))
                {
                    while (csvReader.Read())
                    {
                        var cultureName = csvReader.GetField<string>("LanguageCode");
                        var languageId = 0;

                        if (!string.IsNullOrEmpty(cultureName))
                        {
                            var language = LanguageService.GetLanguage(cultureName);
                            if (language != null)
                            {
                                languageId = language.LanguageId;
                            }
                            else
                            {
                                try
                                {
                                    var ci = CultureInfo.GetCultureInfo(cultureName);
                                    languageId = LanguageService.Add(new Language() { Name = ci.DisplayName, LanguageCode = ci.Name });
                                }
                                catch (Exception ex)
                                {
                                    Debug.Log.Error(ex);
                                }
                            }
                        }

                        if (languageId == 0)
                            continue;
                        
                        LocalizationService.AddOrUpdateResource(
                            languageId,
                            csvReader.GetField<string>("ResourceKey"),
                            csvReader.GetField<string>("ResourceValue") ?? "");
                    }
                }

                FileHelpers.DeleteFile(fullFileName);
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Core.SQL;
using AdvantShop.Diagnostics;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.SEO;

//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------
using CsvHelper.Configuration;

namespace Admin
{
    public partial class Redirects : AdvantShopAdminPage
    {
        SqlPaging _paging;
        InSetFieldFilter _selectionFilter;
        bool _inverseSelection;

        public Redirects()
        {
            _inverseSelection = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblMes.Text = string.Empty;
            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, Resources.Resource.Admin_301Redirects_Header));

            if (!IsPostBack)
            {
                chbEnabled301Redirect.Checked = SettingsSEO.Enabled301Redirects;

                _paging = new SqlPaging { TableName = "[Settings].[Redirect]", ItemsPerPage = 20 };

                var f = new Field { Name = "ID", IsDistinct = false, Sorting = SortDirection.Ascending };
                _paging.AddField(f);


                f = new Field { Name = "RedirectFrom" };
                _paging.AddField(f);

                f = new Field { Name = "RedirectTo" };
                _paging.AddField(f);

                f = new Field { Name = "ProductArtNo" };
                _paging.AddField(f);

                grid.ChangeHeaderImageUrl("arrowID", "images/arrowup.gif");

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

        protected void btnFilter_Click(object sender, EventArgs e)
        {

            //-----Selection filter
            if (String.CompareOrdinal(ddSelect.SelectedIndex.ToString(CultureInfo.InvariantCulture), "0") != 0)
            {

                if (String.CompareOrdinal(ddSelect.SelectedIndex.ToString(CultureInfo.InvariantCulture), "2") == 0)
                {
                    if (_selectionFilter != null)
                    {
                        _selectionFilter.IncludeValues = !_selectionFilter.IncludeValues;
                    }
                    else
                    {
                        _selectionFilter = null; //New InSetFieldFilter()
                        //_SelectionFilter.IncludeValues = True
                    }
                }
                _paging.Fields["ID"].Filter = _selectionFilter;
            }
            else
            {
                _paging.Fields["ID"].Filter = null;
            }

            //----Id filter
            if (!string.IsNullOrEmpty(txtId.Text))
            {
                var nfilter = new CompareFieldFilter { Expression = txtId.Text, ParamName = "@id" };
                _paging.Fields["ID"].Filter = nfilter;
            }
            else
            {
                _paging.Fields["ID"].Filter = null;
            }

            //---RedirectFrom filter
            if (!string.IsNullOrEmpty(txtRedirectFrom.Text))
            {
                var nfilter = new CompareFieldFilter { Expression = txtRedirectFrom.Text, ParamName = "@RedirectFrom" };
                _paging.Fields["RedirectFrom"].Filter = nfilter;
            }
            else
            {
                _paging.Fields["RedirectFrom"].Filter = null;
            }

            //---RedirectTo filter
            if (!string.IsNullOrEmpty(txtRedirectTo.Text))
            {
                var nfilter = new CompareFieldFilter { Expression = txtRedirectTo.Text, ParamName = "@RedirectTo" };
                _paging.Fields["RedirectTo"].Filter = nfilter;
            }
            else
            {
                _paging.Fields["RedirectTo"].Filter = null;
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

                if (!_inverseSelection)
                {
                    foreach (var id in _selectionFilter.Values)
                    {
                        RedirectSeoService.DeleteRedirectSeo(SQLDataHelper.GetInt(id));
                    }
                }
                else
                {
                    var itemsIds = _paging.ItemsIds<int>("ID");
                    //  List<int> ids = CountryService.GetAllCountryID();
                    foreach (int id in itemsIds.Where(id => !_selectionFilter.Values.Contains(id.ToString(CultureInfo.InvariantCulture))))
                    {
                        RedirectSeoService.DeleteRedirectSeo(id);
                    }
                }
            }
        }

        protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteRedirectSeo")
            {
                RedirectSeoService.DeleteRedirectSeo(SQLDataHelper.GetInt(e.CommandArgument));
            }
            if (e.CommandName == "AddRedirectSeo")
            {
                GridViewRow footer = grid.FooterRow;
                var txtNexRedirectFrom = ((TextBox)footer.FindControl("txtNexRedirectFrom")).Text.Trim().ToLower();
                var txtNewRedirectTo = ((TextBox)footer.FindControl("txtNewRedirectTo")).Text.Trim().ToLower();
                var txtNewProductArtNo = ((TextBox)footer.FindControl("txtNewProductArtNo")).Text.Trim();

                if (string.IsNullOrEmpty(txtNexRedirectFrom) || txtNexRedirectFrom == "*" || (string.IsNullOrEmpty(txtNewRedirectTo) && string.IsNullOrEmpty(txtNewProductArtNo)))
                {
                    grid.FooterStyle.BackColor = System.Drawing.Color.FromName("#ffcccc");
                    return;
                }

                if (RedirectSeoService.GetRedirectsSeoByRedirectFrom(txtNexRedirectFrom) != null)
                {
                    grid.FooterStyle.BackColor = System.Drawing.Color.FromName("#ffcccc");
                    return;
                }

                var redirectSeo = new RedirectSeo
                {
                    RedirectFrom = txtNexRedirectFrom,
                    RedirectTo = txtNewRedirectTo,
                    ProductArtNo = txtNewProductArtNo
                };

                RedirectSeoService.AddRedirectSeo(redirectSeo);
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
                    {"ID", "arrowId"},
                    {"RedirectFrom", "arrowRedirectFrom"},
                    {"RedirectTo", "arrowRedirectTo"}
                };
            const string urlArrowUp = "images/arrowup.gif";
            const string urlArrowDown = "images/arrowdown.gif";
            const string urlArrowGray = "images/arrowdownh.gif";


            Field csf = (from Field f in _paging.Fields.Values where f.Sorting.HasValue select f).First();
            Field nsf = _paging.Fields[e.SortExpression];

            if (nsf.Name.Equals(csf.Name))
            {
                csf.Sorting = csf.Sorting == SortDirection.Ascending ? SortDirection.Descending : SortDirection.Ascending;
                grid.ChangeHeaderImageUrl(arrows[csf.Name], (csf.Sorting == SortDirection.Ascending ? urlArrowUp : urlArrowDown));
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
                var redirectSeo = new RedirectSeo
                {
                    ID = SQLDataHelper.GetInt(grid.UpdatedRow["ID"]),
                    RedirectFrom = grid.UpdatedRow["RedirectFrom"].Trim().ToLower(),
                    RedirectTo = grid.UpdatedRow["RedirectTo"].Trim().ToLower(),
                    ProductArtNo = grid.UpdatedRow["ProductArtNo"]
                };

                if (redirectSeo.RedirectFrom.IsNotEmpty() && redirectSeo.RedirectFrom != "*")
                {
                    RedirectSeoService.UpdateRedirectSeo(redirectSeo);
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

        protected void grid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
            }
        }
        protected void btnAddRedirectSeo_Click(object sender, EventArgs e)
        {
            grid.ShowFooter = true;
            grid.FooterStyle.BackColor = System.Drawing.Color.FromName("#ccffcc");
            grid.DataBound += grid_DataBound;
        }

        void grid_DataBound(object sender, EventArgs e)
        {
            if (grid.ShowFooter)
            {
                grid.FooterRow.FindControl("txtNexRedirectFrom").Focus();
            }
        }
        protected void chbEnabled301Redirect_CheckedChanged(object sender, EventArgs e)
        {
            SettingsSEO.Enabled301Redirects = chbEnabled301Redirect.Checked;
        }


        protected void btnExport_Click(object sender, EventArgs e)
        {
            var fileName = "redirects.csv".FileNamePlusDate();
            var fileDirectory = FoldersHelper.GetPathAbsolut(FolderType.PriceTemp);

            if (!Directory.Exists(fileDirectory))
            {
                Directory.CreateDirectory(fileDirectory);
            }

            using (var csvWriter = new CsvHelper.CsvWriter(new StreamWriter(fileDirectory + fileName), new CsvConfiguration() { Delimiter = ";", SkipEmptyRecords = false }))
            {

                foreach (var item in new[] { "RedirectFrom", "RedirectTo", "ProductArtNo" })
                    csvWriter.WriteField(item);
                csvWriter.NextRecord();

                foreach (var redirect in RedirectSeoService.GetRedirectsSeo())
                {
                    csvWriter.WriteField(redirect.RedirectFrom);
                    csvWriter.WriteField(redirect.RedirectTo);
                    csvWriter.WriteField(redirect.ProductArtNo);

                    csvWriter.NextRecord();
                }
            }

            CommonHelper.WriteResponseFile(fileDirectory + fileName, fileName);
        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            if (!fuImportFile.HasFile)
            {
                return;
            }

            var filePath = FoldersHelper.GetPathAbsolut(FolderType.PriceTemp);
            var fileName = "redirectsImport.csv";
            var fullFileName = filePath + fileName.FileNamePlusDate();

            try
            {
                FileHelpers.CreateDirectory(filePath);

                fuImportFile.SaveAs(fullFileName);

                using (var csvReader = new CsvHelper.CsvReader(new StreamReader(fullFileName), new CsvConfiguration() { Delimiter = ";" }))
                {
                    while (csvReader.Read())
                    {
                        var currentRecord = new RedirectSeo
                        {
                            RedirectFrom = csvReader.GetField<string>("RedirectFrom").ToLower(),
                            RedirectTo = csvReader.GetField<string>("RedirectTo").ToLower(),
                            ProductArtNo = csvReader.GetField<string>("ProductArtNo")
                        };

                        if (string.IsNullOrWhiteSpace(currentRecord.RedirectFrom) || currentRecord.RedirectFrom == "*")
                            continue;

                        var redirect = RedirectSeoService.GetRedirectsSeoByRedirectFrom(currentRecord.RedirectFrom);
                        if (redirect == null)
                        {
                            RedirectSeoService.AddRedirectSeo(currentRecord);
                        }
                        else
                        {
                            currentRecord.ID = redirect.ID;
                            RedirectSeoService.UpdateRedirectSeo(currentRecord);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }
        }
    }
}
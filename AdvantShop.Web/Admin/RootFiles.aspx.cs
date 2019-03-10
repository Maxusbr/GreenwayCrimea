//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Helpers;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Diagnostics;
using Resources;
using System.Linq;

namespace Admin
{
    public partial class RootFiles : AdvantShopAdminPage
    {
        private int _currentPageIndex;
        private int _itemsPerPage;
        private int _totalRowCount;

        protected void Page_Load(object sender, EventArgs e)
        {
            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_RootFiles_Header));

            lblError.Text = string.Empty;
            lblError.Visible = false;

            if (!IsPostBack)
            {
                pageNumberer.CurrentPageIndex = 1;
                _currentPageIndex = 1;
                _itemsPerPage = 10;
            }
            else
            {
                _itemsPerPage = SQLDataHelper.GetInt(ddRowsPerPage.SelectedValue);
                _currentPageIndex = pageNumberer.CurrentPageIndex;
            }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            _currentPageIndex = 1;
            pageNumberer.CurrentPageIndex = 1;
        }

        protected void pn_SelectedPageChanged(object sender, EventArgs e)
        {
            _currentPageIndex = pageNumberer.CurrentPageIndex;
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
            if (pagen >= 1 && pagen <= pageNumberer.PageCount)
            {
                _currentPageIndex = pagen;
                pageNumberer.CurrentPageIndex = pagen;
            }
        }

        protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteFile")
            {
                FileHelpers.DeleteFile(SettingsGeneral.AbsolutePath + e.CommandArgument);
                upCounts.Update();
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            _totalRowCount = TotalFilesCount();
            var data = FilesTable();
            if (data.Rows.Count < 1 && _currentPageIndex > 1)
            {
                _currentPageIndex--;
                data = FilesTable();
            }

            if (data.Rows.Count < 1)
            {
                goToPage.Visible = false;
            }

            grid.DataSource = data;
            grid.DataBind();

            pageNumberer.CurrentPageIndex = _currentPageIndex;
            pageNumberer.PageCount = (int)(Math.Ceiling((double)_totalRowCount / _itemsPerPage));
            lblFound.Text = _totalRowCount.ToString();
        }

        protected void grid_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
        }

        private static int TotalFilesCount()
        {
            return GetFiles().Count;
        }

        private DataTable FilesTable()
        {
            var tbl = new DataTable();
            var col = new DataColumn("FileName", typeof(string)) { Caption = "FileName" };
            tbl.Columns.Add(col);

            var fileNames = GetFiles();

            // for paging
            var start = _totalRowCount > _itemsPerPage
                            ? _itemsPerPage * _currentPageIndex - _itemsPerPage : 0;
            var end = (_itemsPerPage * _currentPageIndex) > _totalRowCount
                          ? _totalRowCount : _itemsPerPage * _currentPageIndex;
            for (int i = start; i < end; i++)
            {
                var row = tbl.NewRow();
                row["FileName"] = fileNames[i];
                tbl.Rows.Add(row);
            }

            return tbl;
        }

        private static List<string> GetFiles()
        {
            return FileHelpers.GetFilesInRootDirectory().Select(file=> file.Name).ToList();
        }

        protected void bthAddFile_Click(object sender, EventArgs e)
        {
            lblErrorFile.Text = string.Empty;

            if (!FileLoad.HasFile && FileLoad.FileName.IsNullOrEmpty())
            {
                return;
            }

            try
            {
                if (!FileHelpers.CheckFileExtension(FileLoad.FileName, EAdvantShopFileTypes.FileInRootFolder))
                {
                    lblErrorFile.Text = Resource.Admin_RootFiles_ErrorWrongFileExtesion;
                    return;
                }
                using (var file = FileLoad.PostedFile.InputStream)
                {
                    FileHelpers.SaveFile(SettingsGeneral.AbsolutePath + FileLoad.FileName, file);
                }
            }
            catch (Exception ex)
            {
                lblErrorFile.Text = ex.Message;
                Debug.Log.Error("at loading file in root directory", ex);
                return;
            }

            Response.Redirect(UrlService.GetAdminAbsoluteLink("RootFiles.aspx"));
        }

        protected string RenderFileLink(string fileName)
        {
            return string.Format("~/Admin/HttpHandlers/DownloadFile.ashx?file={0}", fileName);
        }
    }
}
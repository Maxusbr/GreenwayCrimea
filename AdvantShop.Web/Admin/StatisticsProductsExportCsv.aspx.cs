//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Core.SQL;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Diagnostics;
using AdvantShop.ExportImport;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.Saas;
using AdvantShop.Statistic;

using CsvHelper;
using Resources;
using System.Data;

namespace Admin
{
    public partial class StatisticsProductsExportCsv : AdvantShopAdminPage
    {
        private readonly string _strFilePath;
        private string _strFullPath;
        private const string StrFileName = "StatisticsProducts";
        private const string StrFileExt = ".csv";
        protected string ExtStrFileName;
        private string _csvEnconing;
        private string _csvSeparator;
        private string _csvArtNo;
        private int? _csvCategoryId;
        private DateTime? _csvDateFrom;
        private DateTime? _csvDateTo;

        public StatisticsProductsExportCsv()
        {
            _strFilePath = FoldersHelper.GetPathAbsolut(FolderType.PriceTemp);
            FileHelpers.CreateDirectory(_strFilePath);
            var firstFile = Directory.GetFiles(_strFilePath).FirstOrDefault(f => f.Contains(StrFileName));
            _strFullPath = firstFile;
            ExtStrFileName = Path.GetFileName(firstFile);

            if (!string.IsNullOrWhiteSpace(_strFullPath)) return;
            ExtStrFileName = (StrFileName + StrFileExt).FileNamePlusDate();
            _strFullPath = _strFilePath + ExtStrFileName;
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            CommonHelper.DisableBrowserCache();
            divAction.Visible = !CommonStatistic.IsRun;
            divbtnAction.Visible = !CommonStatistic.IsRun;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (SaasDataService.IsSaasEnabled && !SaasDataService.CurrentSaasData.HaveExcel)
            {
                mainDiv.Visible = false;
                notInTariff.Visible = true;
            }

            //hrefAgaint.Visible = false;
            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_ExportExcel_Title));

            if (!IsPostBack)
            {
                ddlEncoding.Items.Clear();
                foreach (var enumItem in (EncodingsEnum[])Enum.GetValues(typeof(EncodingsEnum)))
                {
                    ddlEncoding.Items.Add(new ListItem(enumItem.ToString(), enumItem.StrName()));
                }

                ddlSeparetors.Items.Clear();
                foreach (var enumItem in (SeparatorsEnum[])Enum.GetValues(typeof(SeparatorsEnum)))
                {
                    ddlSeparetors.Items.Add(new ListItem(enumItem.Localize(), enumItem.StrName()));
                }
                ddlSeparetors.SelectedValue = SeparatorsEnum.SemicolonSeparated.StrName();

                var node = new TreeNode { Text = Resource.Admin_m_Category_Root, Value = @"0", Selected = true };
                tree.Nodes.Add(node);

                LoadChildCategories(tree.Nodes[0]);
            }

            MsgErr(true);
            OutDiv.Visible = CommonStatistic.IsRun;
            linkCancel.Visible = CommonStatistic.IsRun;
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            if (lError.Visible) return;
            if (CommonStatistic.IsRun) return;

            //delete old
            foreach (var item in Directory.GetFiles(_strFilePath).Where(f => f.Contains(StrFileName)))
            {
                FileHelpers.DeleteFile(item);
            }

            divAction.Visible = false;
            divbtnAction.Visible = false;

            _csvEnconing = ddlEncoding.SelectedValue;
            _csvSeparator = ddlSeparetors.SelectedValue == SeparatorsEnum.Custom.StrName() ? txtCustomSeparator.Text : ddlSeparetors.SelectedValue;

            _csvCategoryId = hfCatId.Value.TryParseInt(true);

            _csvArtNo = txtArtNo.Text;

            if (!string.IsNullOrEmpty(txtDateFrom.Text))
            {
                DateTime? d = null;
                try
                {
                    d = DateTime.Parse(txtDateFrom.Text);
                }
                catch (Exception)
                {
                }
                if (d.HasValue)
                {
                    var dt = new DateTime(d.Value.Year, d.Value.Month, d.Value.Day, 0, 0, 0, 0);
                    _csvDateFrom = dt;
                }
            }

            if (!string.IsNullOrEmpty(txtDateTo.Text))
            {
                DateTime? d = null;
                try
                {
                    d = DateTime.Parse(txtDateTo.Text);
                }
                catch (Exception)
                {
                }
                if (d.HasValue)
                {
                    var dt = new DateTime(d.Value.Year, d.Value.Month, d.Value.Day, 23, 59, 59, 99);
                    _csvDateTo = dt;
                }
            }

            CommonStatistic.Init();
            CommonStatistic.IsRun = true;
            CommonStatistic.CurrentProcess = Request.Url.PathAndQuery;
            CommonStatistic.CurrentProcessName = lblSubHead.Text;
            linkCancel.Visible = true;
            OutDiv.Visible = true;
            btnDownload.Visible = false;


            ExtStrFileName = (StrFileName + StrFileExt).FileNamePlusDate();
            _strFullPath = _strFilePath + ExtStrFileName;
            FileHelpers.CreateDirectory(_strFilePath);

            CommonStatistic.StartNew(() =>
            {
                try
                {
                    SaveCustomersStatisticToCsv(_strFullPath, _csvEnconing, _csvSeparator, _csvArtNo, _csvCategoryId, _csvDateFrom, _csvDateTo);
                }
                catch (Exception ex)
                {
                    Debug.Log.Error(ex);
                    CommonStatistic.WriteLog(ex.Message);
                }
                CommonStatistic.IsRun = false;
            });
        }

        private void SaveCustomersStatisticToCsv(string strFullPath, string csvEnconing, string csvSeparator, string csvArtNo, int? csvCategoryId, DateTime? csvDateFrom, DateTime? csvDateTo)
        {
            var sqlParameters = new List<SqlParameter>();

            var cmd = string.Format("SELECT [ProductId], [ArtNo], [Name], " +
                                    "(SELECT ISNULL(SUM([Amount]),0) FROM [Order].[OrderItems] INNER JOIN [Order].[Order] ON [OrderItems].[OrderID] = [Order].[OrderID] WHERE [PaymentDate] IS NOT NULL{0}{1} AND [OrderItems].[ProductID]=[Product].[ProductId]) AS Count, " +
                                    "(SELECT ISNULL(SUM([Amount]*[Price]),0) FROM [Order].[OrderItems] INNER JOIN [Order].[Order] ON [OrderItems].[OrderID] = [Order].[OrderID] WHERE [PaymentDate] IS NOT NULL{0}{1} AND [OrderItems].[ProductID]=[Product].[ProductId]) AS Sum " +
                                    "FROM [Catalog].[Product]{2}{3}{4}",
                csvDateFrom.HasValue ? " AND [OrderDate] >= @DateFrom" : "",
                csvDateTo.HasValue ? " AND [OrderDate] <= @DateTo" : "",
                csvArtNo.IsNotEmpty() || csvCategoryId.HasValue ? " WHERE " : "",
                csvArtNo.IsNotEmpty() ? " [ArtNo]=@ArtNo " : "",
                csvCategoryId.HasValue
                    ? (csvArtNo.IsNotEmpty() ? " AND " : "") +
                      " Exists( select 1 from [Catalog].[ProductCategories] INNER JOIN [Settings].[GetChildCategoryByParent](@CategoryId) AS hCat ON hCat.id = [ProductCategories].[CategoryID] and  ProductCategories.ProductId = [Product].[ProductID])"
                    : ""
                );

            if (csvArtNo.IsNotEmpty())
                sqlParameters.Add(new SqlParameter("@ArtNo", csvArtNo));
            if (csvCategoryId.HasValue)
                sqlParameters.Add(new SqlParameter("@CategoryId", csvCategoryId.Value));
            if (csvDateFrom.HasValue)
                sqlParameters.Add(new SqlParameter("@DateFrom", csvDateFrom.Value));
            if (csvDateTo.HasValue)
                sqlParameters.Add(new SqlParameter("@DateTo", csvDateTo.Value));

            var data = SQLDataAccess.ExecuteTable(cmd, CommandType.Text, sqlParameters.ToArray());

            CommonStatistic.TotalRow = data.Rows.Count;

            using (var writer = InitWriter(strFullPath, csvEnconing, csvSeparator))
            {
                var columns = new[]
                {
                    Resource.Admin_ExportField_ArtNo,
                    Resource.Admin_ExportField_ProductName,
                    Resource.Admin_ExportField_ProductSoldAmount,
                    Resource.Admin_ExportField_Sum,
                };

                foreach (var item in columns)
                    writer.WriteField(item);

                writer.NextRecord();

                for (int row = 0; row < data.Rows.Count; row++)
                {
                    if (!CommonStatistic.IsRun) return;

                    writer.WriteField(data.Rows[row]["ArtNo"]);
                    writer.WriteField(data.Rows[row]["Name"]);
                    writer.WriteField(SQLDataHelper.GetInt(data.Rows[row]["Count"]).ToString());
                    writer.WriteField(SQLDataHelper.GetFloat(data.Rows[row]["Sum"]).ToString("F2"));

                    writer.NextRecord();

                    CommonStatistic.RowPosition++;
                }
            }
        }
        private CsvWriter InitWriter(string strFullPath, string csvEnconing, string csvSeparator)
        {
            var writer = new CsvWriter(new StreamWriter(strFullPath, false, Encoding.GetEncoding(csvEnconing)));
            writer.Configuration.Delimiter = csvSeparator;
            return writer;
        }

        protected void linkCancel_Click(object sender, EventArgs e)
        {
            CommonStatistic.IsRun = false;
            CommonStatistic.Init();
            linkCancel.Visible = false;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (CommonStatistic.IsRun)
            {
                ltLink.Text = string.Empty;
                return;
            }
            if (File.Exists(_strFullPath))
            {
                var f = new FileInfo(_strFullPath);
                double sizeM = (double)f.Length / 1048576; //1024 * 1024

                string sizeMesage;
                if ((int)sizeM > 0)
                {
                    sizeMesage = ((int)sizeM) + " MB";
                }
                else
                {
                    double sizeK = (double)f.Length / 1024;
                    if ((int)sizeK > 0)
                    {
                        sizeMesage = ((int)sizeK) + " KB";
                    }
                    else
                    {
                        sizeMesage = (f.Length) + " B";
                    }
                }

                var temp = @"<a href='" + UrlService.GetAbsoluteLink("content/price_temp/" + ExtStrFileName) + @"' {0}>" +
                           Resource.Admin_ExportExcel_DownloadFile + @"</a>";

                //spanMessage.Text
                var t = @"<span> " + Resource.Admin_ExportExcel_FileSize + @": " + sizeMesage + @"</span>" + @"<span>, " + AdvantShop.Localization.Culture.ConvertDate(File.GetLastWriteTime(_strFullPath)) + @"</span>";
                ltLink.Text = string.Format(temp, "") + t;
            }
            else
            {
                ltLink.Text = @"<span>" + Resource.Admin_ExportExcel_NotExistDownloadFile + @"</span>";
            }
        }
        protected void lbParentChange_Click(object sender, EventArgs e)
        {
            mpeTree.Show();
        }

        protected void Select_change(object sender, EventArgs e)
        {
            mpeTree.Show();
            //lParent.Text = CategoryService.GetCategory(tree.SelectedValue.TryParseInt()).Name;
        }

        protected void btnUpdateParent_Click(object sender, EventArgs e)
        {
            mpeTree.Hide();
            hfCatId.Value = tree.SelectedValue;
            lParent.Text = CategoryService.GetCategory(tree.SelectedValue.TryParseInt()).Name;
        }
        public void PopulateNode(object sender, TreeNodeEventArgs e)
        {
            LoadChildCategories(e.Node);
        }

        private void LoadChildCategories(TreeNode node)
        {
            foreach (Category c in CategoryService.GetChildCategoriesByCategoryId(node.Value.TryParseInt(), false))
            {
                var newNode = new TreeNode
                {
                    Text = string.Format("{0} ({1})", c.Name, c.ProductsCount),
                    Value = c.CategoryId.ToString()

                };
                if (c.HasChild)
                {
                    newNode.Expanded = false;
                    newNode.PopulateOnDemand = true;
                }
                else
                {
                    newNode.Expanded = true;
                    newNode.PopulateOnDemand = false;
                }
                node.ChildNodes.Add(newNode);
            }
        }

        private void MsgErr(bool clean)
        {
            if (clean)
            {
                lError.Visible = false;
                lError.Text = string.Empty;
            }
            else
            {
                lError.Visible = false;
            }
        }

        private void MsgErr(string messageText)
        {
            lError.Visible = true;
            lError.Text = @"<br/>" + messageText;
        }
    }
}
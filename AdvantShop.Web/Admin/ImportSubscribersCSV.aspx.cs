//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Text;
using AdvantShop;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Customers;
using AdvantShop.Diagnostics;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.Saas;
using AdvantShop.Statistic;
using Resources;
using System;
using System.IO;
using System.Linq;

namespace Admin
{
    public partial class ImportSubscribersCSV : AdvantShopAdminPage
    {
        private readonly string _filePath;
        private readonly string _fullPath;
        private const string StrFileName = "importCSV";
        private const string StrFileExt = ".csv";
        private const bool _hasHeader = true;

        protected ImportSubscribersCSV()
        {
            _filePath = FoldersHelper.GetPathAbsolut(FolderType.PriceTemp);
            FileHelpers.CreateDirectory(_filePath);

            _fullPath = Directory.GetFiles(_filePath).Where(f => f.Contains(StrFileName)).OrderByDescending(x => x).FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(_fullPath)) return;
            _fullPath = _filePath + (StrFileName + StrFileExt).FileNamePlusDate();
        }

        private void MsgErr(bool clean)
        {
            if (clean)
            {
                lblError.Visible = false;
                lblError.Text = string.Empty;
            }
            else
            {
                lblError.Visible = false;
            }
        }

        private void MsgErr(string messageText)
        {
            lblError.Visible = true;
            lblError.Text = @"<br/>" + messageText;
        }

        protected void btnAction_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(lblError.Text))
                return;

            if (!FileUpload.HasFile) return;

            FileUpload.SaveAs(_fullPath);

            if (!File.Exists(_fullPath)) return;

            if (CommonStatistic.IsRun) return;
            CommonStatistic.Init();
            CommonStatistic.CurrentProcess = Request.Url.PathAndQuery;
            CommonStatistic.CurrentProcessName = Resource.Admin_ImportSubscribersCSV_Name;
            linkCancel.Visible = true;
            MsgErr(true);
            lblRes.Text = string.Empty;

            CommonStatistic.IsRun = true;
            CommonStatistic.TotalRow = GetRowCount(_fullPath);

            CommonStatistic.StartNew(() =>
            {
                try
                {
                    ImportSubscribers(_fullPath);
                }
                catch (Exception ex)
                {
                    Debug.Log.Error(ex);
                    CommonStatistic.WriteLog(ex.Message);
                }
                CommonStatistic.IsRun = false;
            });

            //CsvImport.Factory(_fullPath, true).Process();
            pUpload.Visible = false;
            OutDiv.Visible = true;
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            MsgErr(true);

            OutDiv.Visible = CommonStatistic.IsRun;
            linkCancel.Visible = CommonStatistic.IsRun;

            if (CommonStatistic.IsRun || !File.Exists(_fullPath)) return;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_ImportXLS_Title));
        }

        protected void linkCancel_Click(object sender, EventArgs e)
        {
            CommonStatistic.IsRun = false;
            hlDownloadImportLog.Attributes.CssStyle["display"] = "inline";
        }

        private void ImportSubscribers(string filePath)
        {
            bool flagHeader = true;
            using (var streamReader = new StreamReader(filePath, Encoding.GetEncoding("utf-8")))
            {
                while (streamReader.Peek() >= 0)
                {
                    if (flagHeader)
                    {
                        streamReader.ReadLine();
                        flagHeader = false;
                        continue;
                    }

                    var subscriptionParams = streamReader.ReadLine().Split(new[] { ';' });
                    var subscription = SubscriptionService.GetSubscription(subscriptionParams[0]);
                    if (subscription != null)
                    {
                        subscription.Email = subscriptionParams[0];
                        subscription.Subscribe = subscriptionParams[1] == Resource.Admin_Yes;
                        subscription.UnsubscribeReason = subscriptionParams[4];
                        SubscriptionService.UpdateSubscription(subscription);
                        CommonStatistic.TotalUpdateRow++;
                    }
                    else
                    {
                        SubscriptionService.AddSubscription(new Subscription
                            {
                                Email = subscriptionParams[0],
                                Subscribe = subscriptionParams[1] == Resource.Admin_Yes,
                                UnsubscribeReason = subscriptionParams.Length == 5 ? subscriptionParams[4] : string.Empty
                        });
                        CommonStatistic.TotalAddRow++;
                    }
                    
                    CommonStatistic.RowPosition++;
                }
            }
            CommonStatistic.IsRun = false;
            FileHelpers.DeleteFile(_fullPath);
        }

        private int GetRowCount(string filePath)
        {
            var result = 0;
            using (var streamReader = new StreamReader(filePath, Encoding.UTF8))
            {
                while (streamReader.Peek() >= 0)
                {
                    streamReader.ReadLine();
                    result++;
                }
            }
            return result - (_hasHeader ? 1 : 0);
        }
    }
}
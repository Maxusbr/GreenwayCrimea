//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AdvantShop.Configuration;
using AdvantShop.Core.Controls;
using AdvantShop.Diagnostics;
using Resources;
using System.Web;

namespace Admin
{
    public partial class LogViewer : AdvantShopAdminPage
    {
        protected ErrType CurrentView = ErrType.ErrHttp;
    
        protected void Page_Load(object sender, EventArgs e)
        {
            lblErr.Text = string.Empty;
            if (!string.IsNullOrEmpty(Request["errtype"]))
            {
                Enum.TryParse(Request["errtype"], true, out CurrentView);
            }

            //hlErr404.NavigateUrl = "LogViewer.aspx?ErrType=" + Debug.ErrType.Err404.ToString();
            //if (CurrentView == Debug.ErrType.Err404)
            //    hlErr404.Font.Bold = true;

            hlErr500.NavigateUrl = "LogViewer.aspx?ErrType=" + ErrType.Err500.ToString();
            if (CurrentView == ErrType.Err500)
                hlErr500.Font.Bold = true;

            hlErrHttp.NavigateUrl = "LogViewer.aspx?ErrType=" + ErrType.ErrHttp.ToString();
            if (CurrentView == ErrType.ErrHttp)
                hlErrHttp.Font.Bold = true;

            hlInfo.NavigateUrl = "LogViewer.aspx?ErrType=" + ErrType.Info.ToString();
            if (CurrentView == ErrType.Info)
                hlInfo.Font.Bold = true;

            string str = string.Empty;
            switch (CurrentView)
            {
                case ErrType.Err500:
                    str = Resource.Admin_MasterPageAdmin_BugTracker_Internal;
                    break;
                case ErrType.ErrHttp:
                    str = Resource.Admin_MasterPageAdmin_BugTracker_Other;
                    break;
                case ErrType.Info:
                    str = "Info";
                    break;
            }

            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, str));
            LoadData();
        }

        private bool LoadData()
        {
            var listLog = new List<LogEntry>();
            try
            {
                if (File.Exists(Debug.GetErrFileName(CurrentView)))
                {
                    using (var csv = new CsvHelper.CsvReader(new StreamReader(Debug.GetErrFileName(CurrentView), Encoding.UTF8, true)))
                    {
                        csv.Configuration.Delimiter = Debug.CharSeparate;
                        csv.Configuration.HasHeaderRecord = false;
                        while (csv.Read())
                        {
                            var r = new List<string>(csv.CurrentRecord);
                            if (r.Where(x => !string.IsNullOrWhiteSpace(x)).Count() >= 4)
                            {
                                var item = new LogEntry
                                {
                                    TimeStamp = r[0],
                                    Level = r[1],
                                    Message = HttpUtility.HtmlEncode(r[2]),
                                    ErrorMessage = HttpUtility.HtmlEncode(r[3])
                                };
                                listLog.Add(item);
                            }
                        }
                    }
                    listLog.Reverse();
                    grid.DataSource = listLog;
                    grid.DataBind();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                lblErr.Text = ex.Message;
                return false;
            }
        }
    }
}

//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.IO;
using System.Text;
using System.Web;
using AdvantShop;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Diagnostics;
using Resources;

namespace Admin
{
    public partial class LogViewerDetailed : AdvantShopAdminPage
    {
        private ErrType _currentView = ErrType.None;
        private AdvException _curError;
        private string _atTime;
        protected readonly StringBuilder OutHtml = new StringBuilder();
        protected bool ValidRequst;

        protected void Page_Load(object sender, EventArgs e)
        {
            lblErr.Text = string.Empty;
            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_MasterPageAdmin_BugTracker_HeaderDetails));
            hfBack.NavigateUrl = "LogViewer.aspx";
            if (string.IsNullOrWhiteSpace(Request["attime"]) || string.IsNullOrWhiteSpace(Request["errtype"]))
            {
                lblErr.Text = @"Request wrong";
                return;
            }

            _atTime = HttpUtility.UrlDecode(Request["attime"]);

            Enum.TryParse(Request["errtype"], true, out _currentView);
            if (_currentView == ErrType.None)
            {
                lblErr.Text = @"Wrong ErrType";
                return;
            }
            hfBack.NavigateUrl = "LogViewer.aspx?ErrType=" + _currentView.ToString();

            _curError = GetError(_currentView, _atTime);
            if (_curError != null)
            {
                ValidRequst = true;
                //common
                OutHtml.Append("<div class=\'tab-content\'>");
                OutHtml.Append("<table>");
                OutHtml.AppendFormat("<tr><td><b>{0}</b></td><td>{1}</td></tr>", "Date", _atTime);
                OutHtml.Append("<tr><td colspan='2'><hr/></td></td></tr>");
                OutHtml.AppendFormat("<tr><td><b>{0}</b></td><td>{1}</td></tr>", "Message", _curError.ExceptionData.ManualMessage.IsNullOrEmpty() ? "none" : HttpUtility.HtmlEncode(_curError.ExceptionData.ManualMessage));
                OutHtml.Append("<tr><td colspan='2'><hr/></td></td></tr>");
                OutHtml.AppendFormat("<tr><td><b>{0}</b></td><td>{1}</td></tr>", "ExceptionMessage", HttpUtility.HtmlEncode(_curError.ExceptionData.ExceptionMessage));
                OutHtml.Append("<tr><td colspan='2'><hr/></td></td></tr>");
                OutHtml.AppendFormat("<tr><td><b>{0}</b></td><td>{1}</td></tr>", "ExceptionStackTrace", _curError.ExceptionData.ExceptionStackTrace.IsNullOrEmpty() ? "none" : _curError.ExceptionData.ExceptionStackTrace);
                OutHtml.Append("<tr><td colspan='2'><hr/></td></td></tr>");
                OutHtml.AppendFormat("<tr><td><b>{0}</b></td><td>{1}</td></tr>", "InnerExceptionMessage", _curError.ExceptionData.InnerExceptionMessage.IsNullOrEmpty() ? "none" : _curError.ExceptionData.InnerExceptionMessage);
                OutHtml.Append("<tr><td colspan='2'><hr/></td></td></tr>");
                OutHtml.AppendFormat("<tr><td><b>{0}</b></td><td>{1}</td></tr>", "InnerExceptionStackTrace", _curError.ExceptionData.InnerExceptionStackTrace.IsNullOrEmpty() ? "none" : _curError.ExceptionData.InnerExceptionStackTrace);

                OutHtml.Append("<tr><td colspan='2'><hr/></td></td></tr>");
                foreach (var key in _curError.ExceptionData.Parameters.Keys)
                    OutHtml.AppendFormat("<tr><td><b>{0}</b></td><td>{1}</td></tr>", key, _curError.ExceptionData.Parameters[key]);
            
                OutHtml.Append("</table>");
                OutHtml.Append("</div>");

                //request
                OutHtml.Append("<div class=\'tab-content\'>");
                if (_curError.RequestData != null)
                {
                    OutHtml.Append("<table>");

                    foreach (var key in _curError.RequestData.ColectionData.Keys)
                        OutHtml.AppendFormat("<tr><td><b>{0}</b></td><td>{1}</td></tr>", key, _curError.RequestData.ColectionData[key]);
                    OutHtml.Append("</table>");
                }
                OutHtml.Append("</div>");

                //browser
                OutHtml.Append("<div class=\'tab-content\'>");
                if (_curError.BrowserData != null)
                {
                    OutHtml.Append("<table>");
                    foreach (var key in _curError.BrowserData.ColectionData.Keys)
                        OutHtml.AppendFormat("<tr><td><b>{0}</b></td><td>{1}</td></tr>", key, _curError.BrowserData.ColectionData[key]);
                    OutHtml.Append("</table>");
                }
                OutHtml.Append("</div>");

                //session
                OutHtml.Append("<div class=\'tab-content\'>");
                if (_curError.SessionData != null)
                {
                    OutHtml.Append("<table>");
                    foreach (var key in _curError.SessionData.ColectionData.Keys)
                        OutHtml.AppendFormat("<tr><td><b>{0}</b></td><td>{1}</td></tr>", key, _curError.SessionData.ColectionData[key]);
                    OutHtml.Append("</table>");
                }
                OutHtml.Append("</div>");
            }
        }

        private AdvException GetError(ErrType errType, string atTime)
        {
            try
            {
                if (File.Exists(Debug.GetErrFileName(errType)))
                {
                    using (var csv = new CsvHelper.CsvReader(new StreamReader(Debug.GetErrFileName(errType), Encoding.UTF8, true)))
                    {
                        csv.Configuration.Delimiter = Debug.CharSeparate;
                        csv.Configuration.HasHeaderRecord = false;
                        while (csv.Read())
                        {
                            string[] r = csv.CurrentRecord;
                            if (atTime == r[0] && r.Length >= 5)
                            {
                                var ex1 = new AdvException();
                                try
                                {
                                    if (r[4] != "none")
                                        ex1 = AdvException.GetFromJsonString(r[4]);
                                }
                                catch (Exception ex)
                                {
                                    lblErr.Text = ex.Message;
                                }

                                ex1.ExceptionData.ManualMessage = r[2];

                                return ex1;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblErr.Text = ex.Message;
            }
            return null;
        }
    }
}
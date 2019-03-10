//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Web;
using AdvantShop.Core.Controls;
using AdvantShop.Core.Services.Localization;

namespace AdvantShop.Payment
{
    public enum FormMethod
    {
        POST,
        GET
    }
    /// <summary>
    /// Summary description for PaymentFormHandler
    /// </summary>
    public class PaymentFormHandler
    {
        public const string SameKey = "#";
        public PaymentFormHandler()
        {
            Method = FormMethod.POST;
            FormName = "Pay";
        }
        public string Url { get; set; }
        public FormMethod Method { get; set; }
        public string FormName { get; set; }
        public PageWithPaymentButton Page { get; set; }
        private Dictionary<string, string> _inputValues = new Dictionary<string, string>();
        public Dictionary<string, string> InputValues
        {
            get { return _inputValues; }
            set { _inputValues = value; }
        }
        public void Add(string key, string value)
        {
            InputValues.Add(key, value);
        }
        public void AddRange(IEnumerable<KeyValuePair<string, string>> values, bool overwrite)
        {
            foreach (KeyValuePair<string, string> keyValuePair in values)
            {
                if (!InputValues.ContainsKey(keyValuePair.Key))
                    InputValues.Add(keyValuePair.Key, keyValuePair.Value);
                else if (overwrite)
                    InputValues[keyValuePair.Key] = keyValuePair.Value;
            }
        }
        public void AddRange(IEnumerable<KeyValuePair<string, string>> values)
        {
            AddRange(values, false);
        }
        public void AddRangeOverwrite(IEnumerable<KeyValuePair<string, string>> values)
        {
            AddRange(values, true);
        }

        public string ProcessRequest()
        {
            return ProcessRequest(false);
        }

        public string ProcessRequest(bool useWindows1251, bool useSingleQuote = false)
        {
            var context = HttpContext.Current;
            context.Response.Clear();

            string str = string.Empty;

            str += (string.Format("<form name=\"{0}\" method=\"{1}\" action=\"{2}\" accept-charset=\"{3}\">", FormName, Method, Url, useWindows1251 ? "windows-1251" : "utf-8"));
            foreach (var inputValue in InputValues)
            {
                if (inputValue.Value == null)
                    continue;

                var key = inputValue.Key;
                if (key.Contains(SameKey))
                {
                    key = key.Split(new[] { SameKey }, StringSplitOptions.RemoveEmptyEntries)[0];
                }

                if (useSingleQuote)
                {
                    str += (string.Format("<input name='{0}' type='hidden' value='{1}' />", key, inputValue.Value));
                }
                else
                {
                    str += (string.Format("<input name=\"{0}\" type=\"hidden\" value=\"{1}\" />", HttpUtility.HtmlEncode(key), HttpUtility.HtmlEncode(inputValue.Value)));
                }

            }
            str += "</form>";

            if (Page == PageWithPaymentButton.myaccount)
            {
                str += Button.RenderHtml(LocalizationService.GetResource("Core.Payment.PaymentButtonText"),
                                         eType.Confirm,
                                         eSize.Middle,
                                         onClientClick: string.Format("document.{0}.submit();", FormName));
            }
            else if (Page == PageWithPaymentButton.orderconfirmation)
            {
                str += Button.RenderHtml(LocalizationService.GetResource("Core.Payment.PaymentButtonText"),
                                                     eType.Submit,
                                                     eSize.Middle,
                                                     onClientClick: string.Format("document.{0}.submit();", FormName));
            }
            return str;
        }

        public void Post()
        {
            Post(false);
        }

        public void Post(bool useWindows1251)
        {
            var context = HttpContext.Current;
            context.Response.Clear();

            //var rgAnsii = new System.Text.RegularExpressions.Regex("[à-ÿÀ-ß¹]");

            //if (InputValues.Keys.Any(inputKey => rgAnsii.IsMatch(InputValues[inputKey])))
            //{
            //    context.Response.ContentType = "text/html; windows-1251";
            //    context.Response.Charset = "Windows-1251";
            //    context.Response.ContentEncoding = System.Text.Encoding.GetEncoding("Windows-1251");
            //}
            if (useWindows1251)
            {
                context.Response.ContentType = "text/html; windows-1251";
                context.Response.Charset = "Windows-1251";
                context.Response.ContentEncoding = System.Text.Encoding.GetEncoding("Windows-1251");
            }
            else
            {
                context.Response.ContentType = "text/html; utf-8";
                context.Response.Charset = "Utf-8";
                context.Response.ContentEncoding = System.Text.Encoding.UTF8;
            }
            //default
            //context.Response.ContentType = "text/html; utf-8";
            //context.Response.Charset = "Utf-8";
            //context.Response.ContentEncoding = System.Text.Encoding.UTF8;

            context.Response.Write(string.Format("<html><head><title>Connecting to payment server...</title><script src='https://ajax.googleapis.com/ajax/libs/jquery/2.1.3/jquery.min.js'></script></head><body>"));
            //accept-charset=\"utf-8\"
            context.Response.Write(string.Format("<form name=\"{0}\" method=\"{1}\" action=\"{2}\" >", FormName, Method, Url));
            foreach (var inputValue in InputValues)
            {
                if (inputValue.Value == null)
                    continue;

                context.Response.Write(string.Format("<input name=\"{0}\" type=\"hidden\" value=\"{1}\">", HttpUtility.HtmlEncode(inputValue.Key), HttpUtility.HtmlEncode(inputValue.Value)));
            }
            context.Response.Write(string.Format("</form><script>$(document).ready(function(){{document.{0}.submit();}});</script>", FormName));
            context.Response.Write("<center><br /><b>Please wait...</b><br />Connecting to payment server...</center></body></html>");
            context.Response.End();
        }
    }
}
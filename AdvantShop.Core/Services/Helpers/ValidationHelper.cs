//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AdvantShop.Core.Common.Extensions;

namespace AdvantShop.Helpers
{
    public class ValidationHelper
    {
        public static bool IsValidEmail(string strEmail)
        {

            if (string.IsNullOrEmpty(strEmail))
                return false;

            var r = new Regex(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z",
                              RegexOptions.IgnoreCase | RegexOptions.Singleline);

            return r.IsMatch(strEmail);
        }

        public static bool IsValidPositiveIntNumber(string str)
        {

            if (string.IsNullOrEmpty(str))
                return false;

            var r = new Regex("^[0-9]+$", RegexOptions.IgnoreCase | RegexOptions.Singleline);

            return r.IsMatch(str) && str.Length <= int.MaxValue.ToString().Length - 1;

        }

        public static string DeleteSigns(string strValue)
        {

            if (string.IsNullOrEmpty(strValue))
                return string.Empty;

            const string strProhibitedChars = "~`!@#$%^&*()+-\\/|\"\'{}[];:<>,.?=";

            foreach (var curChar in strProhibitedChars)
            {
                var v = new string(curChar, 1);
                strValue = strValue.Replace(v, "");
            }

            return strValue;
        }

        public static bool Required(Control control)
        {
            bool isValid = false;
            var type = control.GetType();
            switch (type.Name)
            {
                case "TextBox":
                    var _control = (TextBox)control;
                    isValid = _control.Text.IsNotEmpty();
                    break;
            }
            return isValid;
        }

        public static bool Url(string url)
        {
            if (string.IsNullOrEmpty(url))
                return false;

            var r = new Regex(@"^http\://[a-zA-Z0-9à-ÿÀ-ß\-\.]+\.[a-zA-Zà-ÿÀ-ß]{2,3}(/\s*)?$", RegexOptions.IgnoreCase | RegexOptions.Singleline);

            return r.IsMatch(url);
        }

        public static bool Money(string str)
        {
            if (!str.IsNotEmpty())
                return false;

            float isParse;
            return StringHelper.GetMoneyFromString(str, out isParse);
        }

        public static bool Validate(ValidElement validElement)
        {

            bool isValid = Validate(validElement.Control, validElement.ValidType);

            if (isValid) return true;

            if (validElement.ErrContent != null)
            {
                var type = validElement.ErrContent.GetType();

                switch (type.Name)
                {
                    case "Label":
                        var lbl = (Label)validElement.ErrContent;
                        lbl.Text = "<div>" + validElement.Message + "</div>";
                        break;
                    case "HtmlGenericControl":
                        var html = (HtmlGenericControl)validElement.ErrContent;
                        html.InnerHtml += "<div>" + validElement.Message + "</div>";
                        break;
                }
            }
            return validElement.Valid;
        }

        public static bool Validate(List<ValidElement> validElementList)
        {
            bool isValid = true;

            if (validElementList.Count == 0) return true;

            for (int index = 0; index < validElementList.Count; index++)
            {
                if (!Validate(validElementList[index]))
                    isValid = false;

            }
            return isValid;
        }

        private static bool Validate(Control control, ValidType validType)
        {
            bool isValid = false;

            if (validType == ValidType.Required)
            {
                isValid = Required(control);
            }
            else
            {
                var _control = (TextBox)control;
                switch (validType)
                {
                    case ValidType.Email:
                        isValid = IsValidEmail(_control.Text);
                        break;
                    case ValidType.Number:
                        isValid = IsValidPositiveIntNumber(_control.Text);
                        break;
                    case ValidType.Url:
                        isValid = Url(_control.Text);
                        break;
                    case ValidType.Money:
                        isValid = Money(_control.Text);
                        break;
                }
            }

            return isValid;
        }

    }
}
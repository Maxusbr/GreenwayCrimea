//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Localization;

namespace AdvantShop.Core.Controls
{
    public class ParametersControl : UserControl
    {
        protected bool _valid = false;
        public virtual Dictionary<string, string> Parameters { get; set; }
        protected bool ValidateFormData(IEnumerable<TextBox> required, IEnumerable<TextBox> dec)
        {
            return ValidateFormData(required, dec, null);
        }
        protected bool ValidateFormData(IEnumerable<TextBox> required)
        {
            return ValidateFormData(required, null, null);
        }
        protected bool ValidateFormData(IEnumerable<TextBox> required, IEnumerable<TextBox> dec, IEnumerable<TextBox> integer)
        {
            Func<TextBox, Label> getMsg = tb => (Label)FindControl("msg" + tb.ID.Substring(3));
            _valid = true;

            if (required != null)
            {
                ValidateTextBoxes(required, tb => string.IsNullOrEmpty(tb.Text), getMsg, LocalizationService.GetResource("Admin.Core.ParametersControl.EnterValue"));
            }
            if (dec != null)
            {
                ValidateTextBoxes(dec, tb => !tb.Text.IsDecimal(), getMsg, LocalizationService.GetResource("Admin.Core.ParametersControl.IsDecimal"));
            }
            if (integer != null)
            {
                ValidateTextBoxes(integer, tb => !tb.Text.IsInt(), getMsg, LocalizationService.GetResource("Admin.Core.ParametersControl.IsInt"));
            }

            return _valid;
        }

        private void ValidateTextBoxes(IEnumerable<TextBox> textBoxes, Func<TextBox, bool> pred, Func<TextBox, Label> messageAccessor, string message)
        {
            foreach (var textBox in textBoxes)
            {
                var lbl = messageAccessor(textBox);
                if (pred(textBox))
                {
                    MsgErr(lbl, message);
                }
                else
                {
                    ClearMsgErr(lbl);
                }
            }
        }

        protected void ClearMsgErr(Label lbl)
        {
            if (lbl == null) return;
            lbl.Text = "";
            lbl.Visible = false;
        }

        protected void MsgErr(Label lbl, string message)
        {
            if (lbl != null)
            {
                lbl.Visible = true;
                lbl.Text = message;
            }
            _valid = false;
        }

        public virtual void SaveOtherData()
        {
            
        }
    }
}
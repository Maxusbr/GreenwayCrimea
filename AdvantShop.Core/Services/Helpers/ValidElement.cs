//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;
using System.Web.UI;
using AdvantShop.Core.Services.Localization;

namespace AdvantShop.Helpers
{
    public class ValidElement
    {
        public Control Control { get; set; }
        public ValidType ValidType { get; set; }
        
        private string _message = "";
        public string Message
        {
            get
            {
                if (string.IsNullOrEmpty(_message))
                    _message = errorMessage[ValidType];
                return _message;
            }
            set { _message = value; }
        }

        public Control ErrContent { get; set; }
        public bool Valid { get; set; }

        private Dictionary<ValidType, string> errorMessage = new Dictionary<ValidType, string>();

        public ValidElement()
        {
            errorMessage.Add(ValidType.Required, LocalizationService.GetResource("Core.Helpers.ValidElement.Required"));
            errorMessage.Add(ValidType.Email, LocalizationService.GetResource("Core.Helpers.ValidElement.IncorrectEmail"));
            errorMessage.Add(ValidType.Number, LocalizationService.GetResource("Core.Helpers.ValidElement.IncorrectNumber"));
            errorMessage.Add(ValidType.Money, LocalizationService.GetResource("Core.Helpers.ValidElement.IncorrectMoneyFromat"));
            errorMessage.Add(ValidType.Url, LocalizationService.GetResource("Core.Helpers.ValidElement.IncorrectUrl"));
        }
    }

    public enum ValidType
    {
        Required,
        Email,
        Number,
        Money,
        Url
    }
}
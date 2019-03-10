using System;
using System.Linq;
using System.Web;
using AdvantShop;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Customers;
using AdvantShop.Helpers;

namespace UserControls
{
    public partial class Captcha : System.Web.UI.UserControl
    {
        private const int MiniSymbol = 4;
        private readonly char[] _validchars = "123456789qwertyupasdfghjkzxcvbnm".ToCharArray();
        private Random rand = new Random((int)DateTime.Now.TimeOfDay.TotalMilliseconds);

        private Guid CustomerId
        {
            get
            {
                return CustomerContext.CurrentCustomer != null ? CustomerContext.CurrentCustomer.Id : Guid.NewGuid();
            }
        }

        public string DefaultButtonID { get; set; }
        public string TextBoxCss { get; set; }
        public string ValidationGroup { get; set; }
        protected string Base64Text { set; get; }
        protected string MD5Text { set; get; }
        public int MaxLetterCount { get; set; }
        public bool DisplayAnyWay { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            InitCaptcha();
        }

        public void InitCaptcha()
        {
            if (this.Visible || SettingsMain.EnableCaptcha || DisplayAnyWay)
            {
                txtValidCode.DefaultButtonId = DefaultButtonID;
                txtValidCode.CssClass += " " + TextBoxCss;
                txtValidCode.ValidationGroup = ValidationGroup;
                CommonHelper.DisableBrowserCache();
                if (string.IsNullOrEmpty(hfBase64.Value))
                {
                    TryNew();
                }
            }
        }

        public void TryNew()
        {
            string captcha = string.Empty;
            int letterCount = MaxLetterCount > MiniSymbol ? MaxLetterCount : MiniSymbol;

            for (int i = 0; i <= letterCount - 1; i++)
            {
                char newChar;
                do
                {
                    newChar = char.ToUpper(_validchars[rand.Next(0, _validchars.Count())]);
                }
                while (captcha.Contains(newChar));

                captcha += newChar;
            }

            Base64Text = Convert.ToBase64String(SecurityHelper.EncryptString(captcha));
            hfBase64.Value = Base64Text;
            MD5Text = (captcha + CustomerId).Md5();
            hfSource.Value = HttpUtility.UrlEncode(MD5Text);
            txtValidCode.Text = "";
        }

        public bool IsValid()
        {
            if (this.Visible || SettingsMain.EnableCaptcha || DisplayAnyWay)
            {
                string enteredText = HttpUtility.UrlEncode((txtValidCode.Text.ToUpper() + CustomerId).Md5());
                bool result = enteredText == hfSource.Value;
                if (!result)
                    TryNew();
                return result;
            }
            else
            {
                return true;
            }
        }
    }
}
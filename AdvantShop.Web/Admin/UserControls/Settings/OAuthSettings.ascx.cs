using System;
using AdvantShop.Configuration;

namespace Admin.UserControls.Settings
{
    public partial class OAuthSettings : System.Web.UI.UserControl
    {
        public string ErrMessage = Resources.Resource.Admin_CommonSettings_InvalidOAuth;

        protected void Page_Load(object sender, EventArgs e)
        {
            var providers = AdvantShop.Core.AdvantshopConfigService.GetActivityAuthProviders();
            tableFacebook.Visible = !providers.ContainsKey("facebook") || providers["facebook"];

            //tableTwitter.Visible = !providers.ContainsKey("twitter") || providers["twitter"];

            tableVk.Visible = !providers.ContainsKey("vkontakte") || providers["vkontakte"];
            tableMailru.Visible = !providers.ContainsKey("mail.ru") || providers["mail.ru"];
            tableGoogle.Visible = !providers.ContainsKey("google") || providers["google"];
            tableYandex.Visible = !providers.ContainsKey("yandex") || providers["yandex"];
            tableOdnoklassniki.Visible = !providers.ContainsKey("odnoklassniki") || providers["odnoklassniki"];

            if (!IsPostBack)
                LoadData();
        }

        private void LoadData()
        {
            ckbGoogleActive.Checked = tableGoogle.Visible && SettingsOAuth.GoogleActive;
            ckbMailActive.Checked = tableMailru.Visible && SettingsOAuth.MailActive;
            ckbYandexActive.Checked = tableYandex.Visible && SettingsOAuth.YandexActive;
            ckbVKActive.Checked = tableVk.Visible && SettingsOAuth.VkontakteActive;
            ckbFacebookActive.Checked = tableFacebook.Visible && SettingsOAuth.FacebookActive;
            //ckbTwitterActive.Checked = tableTwitter.Visible && SettingsOAuth.TwitterActive;
            ckbOdnoklassnikiActive.Checked = tableOdnoklassniki.Visible && SettingsOAuth.OdnoklassnikiActive;

            txtGoogleClientId.Text = SettingsOAuth.GoogleClientId;
            txtGoogleClientSecret.Text = SettingsOAuth.GoogleClientSecret;

            txtVKAppId.Text = SettingsOAuth.VkontakeClientId;
            txtVKSecret.Text = SettingsOAuth.VkontakeSecret;

            txtFacebookClientId.Text = SettingsOAuth.FacebookClientId;
            txtFacebookApplicationSecret.Text = SettingsOAuth.FacebookApplicationSecret;

            txtOdnoklassnikiClientId.Text = SettingsOAuth.OdnoklassnikiClientId;
            txtOdnoklassnikiPublicApiKey.Text = SettingsOAuth.OdnoklassnikiPublicApiKey;
            txtOdnoklassnikiSecret.Text = SettingsOAuth.OdnoklassnikiSecret;

            txtMailAppId.Text = SettingsOAuth.MailAppId;
            txtMailClientId.Text = SettingsOAuth.MailClientId;
            txtMailClientSecret.Text = SettingsOAuth.MailClientSecret;

            txtYandexClientId.Text = SettingsOAuth.YandexClientId;
            txtYandexClientSecret.Text = SettingsOAuth.YandexClientSecret;
            //txtTwitterConsumerKey.Text = SettingsOAuth.TwitterConsumerKey;
            //txtTwitterConsumerSecret.Text = SettingsOAuth.TwitterConsumerSecret;
            //txtTwitterAccessToken.Text = SettingsOAuth.TwitterAccessToken;
            //txtTwitterAccessTokenSecret.Text = SettingsOAuth.TwitterAccessTokenSecret;

        }
        public bool SaveData()
        {
            //active
            SettingsOAuth.GoogleActive = ckbGoogleActive.Checked;
            SettingsOAuth.MailActive = ckbMailActive.Checked;
            SettingsOAuth.YandexActive = ckbYandexActive.Checked;
            SettingsOAuth.VkontakteActive = ckbVKActive.Checked;
            SettingsOAuth.FacebookActive = ckbFacebookActive.Checked;
            SettingsOAuth.OdnoklassnikiActive = ckbOdnoklassnikiActive.Checked;

            //google
            if (!string.IsNullOrEmpty(txtGoogleClientId.Text))
            {
                SettingsOAuth.GoogleClientId = txtGoogleClientId.Text;
            }
            if (!string.IsNullOrEmpty(txtGoogleClientSecret.Text))
            {
                SettingsOAuth.GoogleClientSecret = txtGoogleClientSecret.Text;
            }
            //vk
            SettingsOAuth.VkontakeClientId = txtVKAppId.Text;
            SettingsOAuth.VkontakeSecret = txtVKSecret.Text;
            //odnoklassniki
            SettingsOAuth.OdnoklassnikiClientId = txtOdnoklassnikiClientId.Text;
            SettingsOAuth.OdnoklassnikiSecret = txtOdnoklassnikiSecret.Text;
            SettingsOAuth.OdnoklassnikiPublicApiKey = txtOdnoklassnikiPublicApiKey.Text;
            //facebook
            SettingsOAuth.FacebookClientId = txtFacebookClientId.Text;
            SettingsOAuth.FacebookApplicationSecret = txtFacebookApplicationSecret.Text;
            //mail
            SettingsOAuth.MailAppId = txtMailAppId.Text;
            SettingsOAuth.MailClientId = txtMailClientId.Text;
            SettingsOAuth.MailClientSecret = txtMailClientSecret.Text;
            //yandex
            SettingsOAuth.YandexClientId = txtYandexClientId.Text;
            SettingsOAuth.YandexClientSecret = txtYandexClientSecret.Text;

            //twitter
            //SettingsOAuth.TwitterActive = ckbTwitterActive.Checked;
            //SettingsOAuth.TwitterConsumerKey = txtTwitterConsumerKey.Text;
            //SettingsOAuth.TwitterConsumerSecret = txtTwitterConsumerSecret.Text;
            //SettingsOAuth.TwitterAccessToken = txtTwitterAccessToken.Text;
            //SettingsOAuth.TwitterAccessTokenSecret = txtTwitterAccessTokenSecret.Text;

            LoadData();

            return true;
        }
    }
}
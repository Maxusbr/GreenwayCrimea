using System;
using System.Drawing;
using System.Text;
using System.Web.UI;
using AdvantShop.Core.Modules;

namespace AdvantShop.Module.Snowfall
{
    public partial class Admin_SnowfallModule : UserControl
    {
        private const string _moduleName = "Snowfall";

        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Visible = false;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            txtSnowfallColor.Attributes["data-plugin-jpicker-options"] = "{clientPath:'" +
                                                                         Request.Url.GetLeftPart(UriPartial.Authority) +
                                                                         Request.ApplicationPath +
                                                                         (!Request.ApplicationPath.EndsWith("/")
                                                                              ? "/"
                                                                              : string.Empty) +
                                                                         "js/plugins/jpicker/images/" + "'}";

            if (!IsPostBack)
            {
                //ckbEnableSnowfall.Checked = ModuleSettingsProvider.GetSettingValue<bool>("SnowfallEnabled", _moduleName);
                var snowfallColor = ModuleSettingsProvider.GetSettingValue<string>("SnowfallColor", _moduleName);
                if (!string.IsNullOrEmpty(snowfallColor))
                {
                    txtSnowfallColor.Text = snowfallColor.TrimStart('#');
                }
                txtSnowfallMaxSize.Text = ModuleSettingsProvider.GetSettingValue<string>("SnowfallMaxSize", _moduleName);
                txtSnowfallMinSize.Text = ModuleSettingsProvider.GetSettingValue<string>("SnowfallMinSize", _moduleName);
                txtSnowfallNewOn.Text = ModuleSettingsProvider.GetSettingValue<string>("SnowfallNewOn", _moduleName);
            }
        }

        protected void Save()
        {
            var errMessage = new StringBuilder();
            const string tpl = "<li>{0}</li>";
            bool isExistErr = false;

            errMessage.Append("<ul style=\"padding:0; margin:0;\">");

            string color = Server.HtmlEncode(txtSnowfallColor.Text);
            if (string.IsNullOrEmpty(color))
            {
                isExistErr = true;
                errMessage.AppendFormat(tpl, GetLocalResourceObject("SnowfallValid_Color"));
            }

            int maxSize;
            if (!int.TryParse(txtSnowfallMaxSize.Text, out maxSize))
            {
                isExistErr = true;
                errMessage.AppendFormat(tpl, GetLocalResourceObject("SnowfallValid_MaxSize"));
            }

            int minSize;
            if (!int.TryParse(txtSnowfallMinSize.Text, out minSize))
            {
                isExistErr = true;
                errMessage.AppendFormat(tpl, GetLocalResourceObject("SnowfallValid_MinSize"));
            }

            int newOn;
            if (!int.TryParse(txtSnowfallNewOn.Text, out newOn))
            {
                isExistErr = true;
                errMessage.AppendFormat(tpl, GetLocalResourceObject("SnowfallValid_NewOn"));
            }

            errMessage.Append("</ul>");

            if (isExistErr)
            {
                lblErr.Text = errMessage.ToString();
                lblErr.ForeColor = Color.Red;
                lblErr.Visible = true;
                return;
            }

            //ModuleSettingsProvider.SetSettingValue("SnowfallEnabled", ckbEnableSnowfall.Checked, _moduleName);
            ModuleSettingsProvider.SetSettingValue("SnowfallColor", "#" + color, _moduleName);
            ModuleSettingsProvider.SetSettingValue("SnowfallMaxSize", maxSize, _moduleName);
            ModuleSettingsProvider.SetSettingValue("SnowfallMinSize", minSize, _moduleName);
            ModuleSettingsProvider.SetSettingValue("SnowfallNewOn", txtSnowfallNewOn.Text, _moduleName);

            lblMessage.Text = (String) GetLocalResourceObject("Snowfall_Message");
            lblMessage.ForeColor = Color.Blue;
            lblMessage.Visible = true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Save();
        }
    }
}
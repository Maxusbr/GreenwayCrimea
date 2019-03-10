//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

namespace AdvantShop.Configuration
{

    public class SettingsBank
    {
        public static string INN
        {
            get { return SettingProvider.Items["INN"]; }
            set { SettingProvider.Items["INN"] = value; }
        }
        public static string RS
        {
            get { return SettingProvider.Items["RS"]; }
            set { SettingProvider.Items["RS"] = value; }
        }
        public static string Director
        {
            get { return SettingProvider.Items["Director"]; }
            set { SettingProvider.Items["Director"] = value; }
        }
        public static string Manager
        {
            get { return SettingProvider.Items["Manager"]; }
            set { SettingProvider.Items["Manager"] = value; }
        }
        public static string Accountant
        {
            get { return SettingProvider.Items["Accountant"]; }
            set { SettingProvider.Items["Accountant"] = value; }
        }

        public static string StampImageName
        {
            get { return SettingProvider.Items["StampImage"]; }
            set { SettingProvider.Items["StampImage"] = value; }
        }

        public static string CompanyName
        {
            get { return SettingProvider.Items["CompanyName"]; }
            set { SettingProvider.Items["CompanyName"] = value; }
        }
        public static string KPP
        {
            get { return SettingProvider.Items["KPP"]; }
            set { SettingProvider.Items["KPP"] = value; }
        }
        public static string BankName
        {
            get { return SettingProvider.Items["BankName"]; }
            set { SettingProvider.Items["BankName"] = value; }
        }
        public static string KS
        {
            get { return SettingProvider.Items["KS"]; }
            set { SettingProvider.Items["KS"] = value; }
        }
        public static string BIK
        {
            get { return SettingProvider.Items["BIK"]; }
            set { SettingProvider.Items["BIK"] = value; }
        }
        public static string Address
        {
            get { return SettingProvider.Items["Address"]; }
            set { SettingProvider.Items["Address"] = value; }
        }
    }
}

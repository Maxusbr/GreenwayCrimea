//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Globalization;
using AdvantShop.Core.Common.Extensions;

namespace AdvantShop.Configuration
{
    public class SettingsMail
    {

        public const string SIX_STARS = "******";

        /// <summary>
        /// Get or sets the name of the SMTP server.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string SMTP
        {
            get { return SettingProvider.Items["EmailSettingSMTP"]; }
            set { SettingProvider.Items["EmailSettingSMTP"] = value; }
        }

        /// <summary>
        /// Get or sets the port that SMTP clients use to connect to an SMTP mail server. The default value is 25.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public static int Port
        {
            get { return SettingProvider.Items["EmailSettingPort"].TryParseInt(); }
            set { SettingProvider.Items["EmailSettingPort"] = value.ToString(); }
        }

        /// <summary>
        /// Get or sets the user password to use to connect to SMTP mail server.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string Password
        {
            get { return SettingProvider.Items["EmailSettingPassword"]; }
            set { SettingProvider.Items["EmailSettingPassword"] = value; }
        }

        public static string InternalDataL
        {
            get { return SettingProvider.Items["InternalDataL"]; }
            set { SettingProvider.Items["InternalDataL"] = value; }
        }

        public static string InternalDataP
        {
            get { return SettingProvider.Items["InternalDataP"]; }
            set { SettingProvider.Items["InternalDataP"] = value; }
        }




        /// <summary>
        /// Get or sets the login to use to connect to an SMTP mail server.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string Login
        {
            get { return SettingProvider.Items["EmailSettingLogin"]; }
            set { SettingProvider.Items["EmailSettingLogin"] = value; }
        }

        /// <summary>
        /// Get or sets the default value that indicates who the email message is from.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string From
        {
            get { return SettingProvider.Items["EmailSettingFrom"]; }
            set { SettingProvider.Items["EmailSettingFrom"] = value; }
        }


        public static string SenderName
        {
            get { return SettingProvider.Items["EmailSettingSenderName"]; }
            set { SettingProvider.Items["EmailSettingSenderName"] = value; }
        }

        /// <summary>
        /// Get or sets the default value that indicates SLL option
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool SSL
        {
            get { return bool.Parse(SettingProvider.Items["EmailSettingSSL"]); }
            set { SettingProvider.Items["EmailSettingSSL"] = value.ToString(); }
        }

        public static string EmailForRegReport
        {
            get { return SettingProvider.Items["Email_4_RegReport"]; }
            set { SettingProvider.Items["Email_4_RegReport"] = value; }
        }
        public static string EmailForOrders
        {
            get { return SettingProvider.Items["Email_4_orders"]; }
            set { SettingProvider.Items["Email_4_orders"] = value; }
        }
        public static string EmailForProductDiscuss
        {
            get { return SettingProvider.Items["Email_4_ProductDiscuss"]; }
            set { SettingProvider.Items["Email_4_ProductDiscuss"] = value; }
        }

        public static string EmailForFeedback
        {
            get { return SettingProvider.Items["Email_4_Feedback"]; }
            set { SettingProvider.Items["Email_4_Feedback"] = value; }
        }

        public static string EmailForLeads
        {
            get { return SettingProvider.Items["Email_NewLead"]; }
            set { SettingProvider.Items["Email_NewLead"] = value; }
        }

        public static int MailErrorCurrentCount
        {
            get { return int.Parse(SettingProvider.Items["MailErrorCurrentCount"]); }
            set { SettingProvider.Items["MailErrorCurrentCount"] = value.ToString(CultureInfo.InvariantCulture); }
        }

        public static DateTime MailErrorLastSend
        {
            get { return DateTime.Parse(SettingProvider.Items["MailErrorLastSend"], CultureInfo.InvariantCulture); }
            set { SettingProvider.Items["MailErrorLastSend"] = value.ToString(CultureInfo.InvariantCulture); }
        }




        public static string ImapHost
        {
            get { return SettingProvider.Items["EmailSettingImapHost"]; }
            set { SettingProvider.Items["EmailSettingImapHost"] = value; }
        }

       public static int ImapPort
        {
            get { return SettingProvider.Items["EmailSettingImapPort"].TryParseInt(); }
            set { SettingProvider.Items["EmailSettingImapPort"] = value.ToString(); }
        }
    }
}
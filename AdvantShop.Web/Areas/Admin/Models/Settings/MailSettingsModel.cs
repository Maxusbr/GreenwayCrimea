
namespace AdvantShop.Web.Admin.Models.Settings
{
    public class MailSettingsModel
    {
        public string EmailForOrders { get; set; }
        public string EmailForProductDiscuss { get; set; }
        public string EmailForRegReport { get; set; }
        public string EmailForFeedback { get; set; }
        public string EmailForLeads { get; set; }

        public string SMTP { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        /// <summary>
        /// Пароль с которым сравнивается Password, чтобы узнать что он изменился
        /// </summary>
        public string PasswordCompare { get; set; }

        public string From { get; set; }
        public string SenderName { get; set; }
        public bool SSL { get; set; }
        public int Port { get; set; }


        public string ImapHost { get; set; }
        public int ImapPort { get; set; }
    }
}



//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using AdvantShop.Core.Services.Mails;
using AdvantShop.Customers;

namespace AdvantShop.Modules
{
    public class MailChimpListMembers
    {
        public int Total { get; set; }
        public System.Collections.Generic.List<MailChimpListMember> Data { get; set; }
    }

    public class MailChimpListMember
    {
        //Member email address date timestamp timestamp of their associated status date (subscribed, unsubscribed, cleaned, or updated) in GMT
        public string email { get; set; }
        public string id { get; set; }
        public string euid { get; set; }
        public string email_type { get; set; }
        public string ip_signup { get; set; }
        public string timestamp_signup { get; set; }
        public string ip_opt { get; set; }
        public string timestamp_opt { get; set; }
        public string member_rating { get; set; }
        public string info_changed { get; set; }
        public string web_id { get; set; }
        public string leid { get; set; }
        public string language { get; set; }
        public string list_name { get; set; }
        public MailChimpListMemberMerges merges { get; set; }
        public string status { get; set; }
        public string timestamp { get; set; }
        public string is_gmonkey { get; set; }

            //"lists":[],
            //"geo":[],
            //"clients":[],
            //"static_segments":[],
            //"notes":[]

        ////For unsubscribes only - the reason collected for the unsubscribe. If populated, one of 'NORMAL','NOSIGNUP','INAPPROPRIATE','SPAM','OTHER'
        //public string Reason;

        ////For unsubscribes only - if the reason is OTHER, the text entered.
        //public string ReasonText;
    }

    public class MailChimpListMemberMerges
    {
        public string Email { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
    }


    public class MailChimpListSubscriber : ISubscriber
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public EMailRecipientType CustomerType { get; set; }
    }
}
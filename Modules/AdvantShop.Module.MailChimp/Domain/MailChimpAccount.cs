//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;

namespace AdvantShop.Modules
{
    public class MailChimpAccount
    {
        public string username { get; set; }
        public string user_id { get; set; }
        public bool is_trial { get; set; }
        public bool is_approved { get; set; }
        public bool has_activated { get; set; }
        public string timezone { get; set; }
        public string plan_type { get; set; }
        public int plan_low { get; set; }
        public int plan_high { get; set; }
        public string plan_start_date { get; set; }
        public int emails_left { get; set; }
        public bool pending_monthly { get; set; }
        public string first_payment { get; set; }
        public string last_payment { get; set; }
        public int times_logged_in { get; set; }
        public string last_login { get; set; }
        public string affiliate_link { get; set; }
        public string industry { get; set; }
        public MailChimpAccountContact contact { get; set; }
        public List<MailChimpAccountModule> modules { get; set; }
        public List<MailChimpAccountOrder> orders { get; set; }
        //array rewards
        //array integrations
    }

    public class MailChimpAccountContact
    {
        public string fname { get; set; }
        public string lname { get; set; }
        public string email { get; set; }
        public string company { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string country { get; set; }
        public string url { get; set; }
        public string phone { get; set; }
        public string fax { get; set; }
    }

    public class MailChimpAccountModule
    {
        public string id { get; set; }
        public string name { get; set; }
        public string added { get; set; }
        public string data { get; set; }
    }

    public class MailChimpAccountOrder
    {
        public int order_id { get; set; }
        public string type { get; set; }
        public double amount { get; set; }
        public string date { get; set; }
        public double credits_used { get; set; }
    }
}
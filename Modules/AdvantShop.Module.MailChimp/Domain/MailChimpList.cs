//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;

namespace AdvantShop.Modules
{
    public class MailChimpLists
    {
        public int total { get; set; }
        public List<MailChimpList> data { get; set; }
        public List<MailChimpListErrors> errors { get; set; }
    }

    public class MailChimpList
    {
        public string id { get; set; }
        public string web_id { get; set; }
        public string name { get; set; }
        public string date_created { get; set; }
        public bool email_type_option { get; set; }
        public bool use_awesomebar { get; set; }
        public string default_from_name { get; set; }
        public string default_from_email { get; set; }
        public string default_subject { get; set; }
        public string default_language { get; set; }
        public string list_rating { get; set; }
        public string subscribe_url_short { get; set; }
        public string subscribe_url_long { get; set; }
        public string beamer_address { get; set; }
        public string visibility { get; set; }
        public MailChimpListStats stats { get; set; }
        public List<string> modules { get; set; }
    }

    public class MailChimpListStats
    {
        public string member_count { get; set; }
        public string unsubscribe_count { get; set; }
        public string cleaned_count { get; set; }
        public string member_count_since_send { get; set; }
        public string unsubscribe_count_since_send { get; set; }
        public string cleaned_count_since_send { get; set; }
        public string campaign_count { get; set; }
        public string grouping_count { get; set; }
        public string group_count { get; set; }
        public string merge_var_count { get; set; }
        public string avg_sub_rate { get; set; }
        public string avg_unsub_rate { get; set; }
        public string target_sub_rate { get; set; }
        public string open_rate { get; set; }
        public string click_rate { get; set; }
    }

    public class MailChimpListErrors
    {
        public string param { get; set; }
        public string code { get; set; }
        public string error { get; set; }

    }
}
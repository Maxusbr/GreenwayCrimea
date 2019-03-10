//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

namespace AdvantShop.Modules
{

    public class MailchimpCampaign
    {
        public string id { get; set; }
        public string web_id { get; set; }
        public string list_id { get; set; }
        public string folder_id { get; set; }
        public string template_id { get; set; }
        public string content_type { get; set; }
        public string content_edited_by { get; set; }
        public string title { get; set; }
        public string type { get; set; }
        public string create_time { get; set; }
        public string send_time { get; set; }
        public string content_updated_time { get; set; }
        public string status { get; set; }
        public string from_name { get; set; }
        public string from_email { get; set; }
        public string subject { get; set; }
        public string to_name { get; set; }
        public string archive_url { get; set; }
        public string archive_url_long { get; set; }
        public string emails_sent { get; set; }
        public bool inline_css { get; set; }
        public string analytics { get; set; }
        public string analytics_tag { get; set; }
        public bool authenticate { get; set; }
        public bool ecomm360 { get; set; }
        public bool auto_tweet { get; set; }
        public string auto_fb_post { get; set; }
        public bool auto_footer { get; set; }
        public bool timewarp { get; set; }
        public string timewarp_schedule { get; set; }
        public MailchimpCampaignTracking tracking { get; set; }

        public string parent_id { get; set; }
        public bool is_child { get; set; }
        public string tests_sent { get; set; }
        public string tests_remain { get; set; }
        //public string segment_text { get; set; }
        //public string segment_opts { get; set; }
        //public string saved_segment { get; set; }
        //public string type_opts { get; set; }

    }

    public class MailchimpCampaignTracking
    {
        public bool html_clicks { get; set; }
        public bool text_clicks { get; set; }
        public bool opens { get; set; }
    }

    public class MailchimpCreateCampaignObject
    {
        public string apikey { get; set; }
        public string type { get; set; }
        public MailchimpCreateCampaignObjectOptions options { get; set; }
        public MailchimpCreateCampaignObjectContent content { get; set; }
    }

    public class MailchimpCreateCampaignObjectOptions
    {
        public string list_id { get; set; }
        public string subject { get; set; }
        public string from_email { get; set; }
        public string from_name { get; set; }
        public string to_name { get; set; }
    }

    public class MailchimpCreateCampaignObjectContent
    {
        public string html { get; set; }
        public string sections { get; set; }
        public string text { get; set; }
        public string url { get; set; }
        public string archive { get; set; }
    }
}
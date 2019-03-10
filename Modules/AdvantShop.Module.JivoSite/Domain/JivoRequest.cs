using System.Collections.Generic;

namespace AdvantShop.Module.JivoSite.Domain
{
    public class JivoRequest
    {
        public string token { get; set; }
        public string user_token { get; set; }
        public string event_name { get; set; }
        public int chat_id { get; set; }
        public string widget_id { get; set; }
        public JivoVisitor visitor { get; set; }
        public JivoAgent agent { get; set; }
        public List<JivoAgent> agents { get; set; }
        public Chat chat { get; set; }
        public int offline_message_id { get; set; }
        public string message { get; set; }
    }
}

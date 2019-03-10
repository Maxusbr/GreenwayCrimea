using System.Collections.Generic;

namespace AdvantShop.Module.JivoSite.Domain
{
    public class JivoBaseResponse
    {
        public JivoBaseResponse()
        {
            result = "ok";
        }

        public string result { get; set; }
    }

    public class JivoChatAcceptedResponse : JivoBaseResponse
    {
        public List<CustomData> custom_data { get; set; }
        public ContactInfo contact_info { get; set; }
        public bool enable_assign { get; set; }
        public string crm_link { get; set; }
    }
}

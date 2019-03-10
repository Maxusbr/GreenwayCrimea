//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;

namespace AdvantShop.Module.UniSender.Domain
{
    public class UniSenderMessage
    {
        public string Message_id { get; set; }
    }
    public class UniSenderEmailList
    {
        public List<List<string>> Data { get; set; }
    }

    public class UniSenderResponse
    {
        public List<KeyValuePair<string, string>> Warnings { get; set; }

        public string Error { get; set; }

        public string Code { get; set; }
    }

    public class UniSenderGetListsResponse : UniSenderResponse
    {
        public List<UniSenderList> Result { get; set; }
    }

    public class UniSenderSubscribeResponse : UniSenderResponse
    {
        public UniSenderListMember Result { get; set; }
    }

    public class UniSenderUnsubscribeResponse : UniSenderResponse
    {
        public object Result { get; set; }
    }

    public class UniSenderCreateCampaignResponse : UniSenderResponse
    {
        public UniSenderCampaign Result { get; set; }
    }

    public class UniSenderCreateMessageResponse : UniSenderResponse
    {
        public UniSenderMessage Result { get; set; }
    }
    
    public class UniSenderListMembersResponse : UniSenderResponse
    {
        public UniSenderEmailList Result { get; set; }
    }

}
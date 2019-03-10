using System.Collections.Generic;

namespace AdvantShop.Module.JivoSite.Domain
{
    public class Chat
    {
        public string invitation { get; set; }
        public List<Message> messages { get; set; }
    }
}
namespace AdvantShop.Module.JivoSite.Domain
{
    public class Message
    {
        public string type { get; set; }
        public int timestamp { get; set; }
        public string message { get; set; }
        public int agent_id { get; set; }
    }
}

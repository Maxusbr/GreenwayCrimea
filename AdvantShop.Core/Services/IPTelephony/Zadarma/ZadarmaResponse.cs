namespace AdvantShop.Core.Services.IPTelephony.Zadarma
{
    public class ZadarmaResponse
    {
        public string Status { get; set; }
        public string Message { get; set; }

        public bool Success { get { return Status == "success"; } }
    }

    public class ZadarmaRecordResponse : ZadarmaResponse
    {
        public string Link { get; set; }
        public string[] Links { get; set; }
    }

    public class ZadarmaCallbackResponse : ZadarmaResponse
    {
        public string From { get; set; }
        public string To { get; set; }
    }
}

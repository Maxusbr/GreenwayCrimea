namespace AdvantShop.Core.Services.IPTelephony.CallBack
{
    public class CallBackAnswer
    {
        public bool Success { get; private set; }
        public string Message { get; private set; }

        public CallBackAnswer(bool success, string message)
        {
            Success = success;
            Message = message;
        }
    }
}

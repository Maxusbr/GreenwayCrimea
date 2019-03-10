namespace AdvantShop.Core.Services.Bonuses.Sms.Gateways
{
    public interface ISmsGateway
    {
        string Send(long destination, string message, string from);
    }
}

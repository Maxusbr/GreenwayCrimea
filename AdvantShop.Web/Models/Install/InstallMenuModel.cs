namespace AdvantShop.Models.Install
{
    public enum InstallStep
    {
        None = 0,
        TrialSelect = 1,
        Shopinfo = 2,
        Finance = 3,
        Payment = 4,
        Shipping = 5,
        OpenId = 6,
        Notify = 7,
        Final = 8,

    }

    public struct InstallMenuItem
    {
        public string MenuName;
        public InstallStep Step;
        public string StyleClass;
        public bool IsActive;
    }
}
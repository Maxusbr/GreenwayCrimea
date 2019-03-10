using AdvantShop.Core.Common.Attributes;

namespace AdvantShop.Core.Services.Orders
{
    public enum OrderType
    {
        [Localize("Core.Orders.OrderType.NotSelected")]
        None,

        [Localize("Core.Orders.OrderType.ShoppingCart")]
        ShoppingCart,

        [Localize("Core.Orders.OrderType.Offline")]
        Offline,

        [Localize("Core.Orders.OrderType.OneClick")]
        OneClick,

        [Localize("Core.Orders.OrderType.LandingPage")]
        LandingPage,

        [Localize("Core.Orders.OrderType.Mobile")]
        Mobile,

        [Localize("Core.Orders.OrderType.Phone")]
        Phone,

        [Localize("Core.Orders.OrderType.LiveChat")]
        LiveChat,

        [Localize("Core.Orders.OrderType.SocialNetworks")]
        SocialNetworks,

        [Localize("Core.Orders.OrderType.FindCheaper")]
        FindCheaper,

        [Localize("Core.Orders.OrderType.AbandonedCart")]
        AbandonedCart,

        [Localize("Core.Orders.OrderType.Callback")]
        Callback,

        [Localize("Core.Orders.OrderType.PreOrder")]
        PreOrder,

        [Localize("Core.Orders.OrderType.Feedback")]
        Feedback,

        [Localize("Core.Orders.OrderType.Vk")]
        Vk,
    }
}

using AdvantShop.Core.Common.Attributes;

namespace AdvantShop.Shipping.NovaPoshta
{
    public enum enNovaPoshtaDeliveryType
    {
        [Localize("Core.Shipping.NovaPoshtaDeliveryType.DoorsDoors")]
        DoorsDoors = 1,

        [Localize("Core.Shipping.NovaPoshtaDeliveryType.DoorsWarehouse")]
        DoorsWarehouse = 2,

        [Localize("Core.Shipping.NovaPoshtaDeliveryType.WarehouseDoors")]
        WarehouseDoors = 3,

        [Localize("Core.Shipping.NovaPoshtaDeliveryType.WarehouseWarehouse")]
        WarehouseWarehouse = 4
    }
}
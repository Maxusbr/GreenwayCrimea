using System;
using System.Xml.Serialization;

namespace AdvantShop.Core.Services.Shipping.Grastin.Api
{
    [Serializable]
    [XmlRoot(ElementName = "Orders")]
    public class RussianPostOrderResponse : GrastinOrderCourierResponse
    {
    }
}

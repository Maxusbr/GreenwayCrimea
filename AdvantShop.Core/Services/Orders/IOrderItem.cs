//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

namespace AdvantShop.Orders
{
    public interface IOrderItem
    {
        int OrderItemID { get; set; }
        int OrderID { get; set; }
        int? ProductID { get; set; }
        int? CertificateID { get; set; }
        string ArtNo { get; set; }
        string Name { get; set; }
        float Price { get; set; }
        float Amount { get; set; }
    }
}
//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;

namespace AdvantShop.Orders
{
    public class GiftCertificate
    {
        public int CertificateId { get; set; }
        public string CertificateCode { get; set; }

        public int OrderId { get; set; }
        public string ApplyOrderNumber { get; set; }

        public string FromName { get; set; }
        public string ToName { get; set; }

        public float Sum { get; set; }

        public string CertificateMessage { get; set; }

        public bool Used { get; set; }
        public bool Enable { get; set; }

        public string ToEmail { get; set; }

        public DateTime CreationDate { get; set; }

        public override int GetHashCode()
        {
            return CertificateCode.GetHashCode() ^ Sum.GetHashCode() ^ Used.GetHashCode() * 123 ^ Paid.GetHashCode() * 321 ^ Enable.GetHashCode() * 323;
        }

        private Order _certificateOrder;
        public Order CertificateOrder { get { return _certificateOrder ?? (_certificateOrder = OrderService.GetOrder(OrderId)); } }

        public bool Paid { get { return OrderService.IsPaidOrder(OrderId); } }
    }

    public class GiftCertificateOrderModel
    {
        public GiftCertificate GiftCertificate { get; set; }

        public string EmailFrom { get; set; }

        public string Phone { get; set; }

        public int PaymentId { get; set; }
    }
}
using AdvantShop.Orders;
using AdvantShop.Core.Services.Catalog;
using System;
using System.Globalization;

namespace AdvantShop.Web.Admin.Models.Certificates
{
    public class AdminCertificatesModel
    {
        public AdminCertificatesModel()
        {

        }
        public AdminCertificatesModel(GiftCertificate model)
        {
            CertificateId = model.CertificateId;
            CertificateCode = model.CertificateCode;
            OrderId = model.OrderId;
            ApplyOrderNumber = model.ApplyOrderNumber;
            FromName = model.FromName;
            ToName = model.ToName;
            Sum = model.Sum;
            CertificateMessage = model.CertificateMessage;
            Used = model.Used;
            Enable = model.Enable;
            ToEmail = model.ToEmail;
            CreationDate = model.CreationDate;
        }
        public int CertificateId { get; set; }
        public string CertificateCode { get; set; }

        public int OrderId { get; set; }
        public string ApplyOrderNumber { get; set; }

        public string FromName { get; set; }
        public string ToName { get; set; }

        public float Sum { get; set; }
        public string FullSum {
            get {
                return PriceFormatService.FormatPrice(Sum);
            } }

        public string CertificateMessage { get; set; }

        public bool Used { get; set; }
        public bool Enable { get; set; }

        public string ToEmail { get; set; }

        public DateTime CreationDate { get; set; }

        public string CreationDates {
            get {
                return CreationDate.ToString("dd.MM.yyyy HH:mm");
            } }
        
        public bool Paid { get { return OrderService.IsPaidOrder(OrderId); } }
    }
}

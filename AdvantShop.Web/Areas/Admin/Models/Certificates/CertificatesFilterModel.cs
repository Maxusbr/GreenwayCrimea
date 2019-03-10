using AdvantShop.Web.Infrastructure.Admin;
using System;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Orders;

namespace AdvantShop.Web.Admin.Models.Certificates
{
    public class CertificatesFilterModel : BaseFilterModel
    {
        public int CertificateId { get; set; }
        public string CertificateCode { get; set; }

        public string OrderId { get; set; }
        public string ApplyOrderNumber { get; set; }

        public string FromName { get; set; }
        public string ToName { get; set; }

        public string Sum { get; set; }

        public string CertificateMessage { get; set; }

        public bool? Used { get; set; }
        public bool? Enable { get; set; }
        public bool? Payed { get; set; }

        public string ToEmail { get; set; }
        public string FromEmail { get; set; }
        
        public DateTime CreationDate { get; set; }

        public string CreationDateFrom { get; set; }
        public string CreationDateTo { get; set; }

        public bool Paid { get { return OrderService.IsPaidOrder(OrderId.TryParseInt()); } }
    }
    
}

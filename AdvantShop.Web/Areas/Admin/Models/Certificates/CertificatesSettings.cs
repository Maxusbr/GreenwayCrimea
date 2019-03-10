using System.Collections.Generic;
using System.Web.Mvc;

namespace AdvantShop.Web.Admin.Models.Certificates
{
    public class CertificatesSettings
    {
        public List<SelectListItem> PaymentMethods { get; set; }
        public List<SelectListItem> Taxes { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvantShop.Module.SimaLand.Models
{
    public class SimalandCertificate
    {
        public int id { get; set; }
        public int sid { get; set; }
        public string text { get; set; }
    }

    public class CertificateItems
    {
        public List<SimalandCertificate> items { get; set; }
    }


}

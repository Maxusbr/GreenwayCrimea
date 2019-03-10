using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvantShop.Module.AdditionalMarkers.Models
{
    public class ProductViewMarker
    {
        public string ProductUrl { get; set; }
        public int ProductId { get; set; }
        public List<Marker> Markers { get; set; }
    }
}

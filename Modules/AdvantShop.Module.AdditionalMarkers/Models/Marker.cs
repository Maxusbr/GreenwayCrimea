using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvantShop.Module.AdditionalMarkers.Models
{
    public class Marker
    {
        public int MarkerId { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public string ColorName { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public bool OpenNewTab { get; set; }
        public int SortOrder { get; set; }
    }

    public class MarkerSelected : Marker
    {
        public bool Selected { get; set; }

        public MarkerSelected()
        {}

        public MarkerSelected(Marker marker)
        {
            MarkerId = marker.MarkerId;
            Name = marker.Name;
            Color = marker.Color;
            ColorName = marker.ColorName;
            Url = marker.Url;
            Description = marker.Description;
        }
    }

}

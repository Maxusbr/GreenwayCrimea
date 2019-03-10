using System;
using AdvantShop.Diagnostics;
using Newtonsoft.Json;

namespace AdvantShop.Core.Services.CustomerSegments
{
    public class CustomerSegment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Filter { get; set; }
        public DateTime CreatedDate { get; set; }

        
        private CustomerSegmentFilter _segmentFilter;
        public CustomerSegmentFilter SegmentFilter
        {
            get
            {
                if (_segmentFilter != null)
                    return _segmentFilter;

                _segmentFilter = new CustomerSegmentFilter();

                if (string.IsNullOrEmpty(Filter))
                    return _segmentFilter;

                try
                {
                    _segmentFilter = JsonConvert.DeserializeObject<CustomerSegmentFilter>(Filter);
                }
                catch (Exception ex)
                {
                    Debug.Log.Error(ex);
                }

                return _segmentFilter;
            }
            set { _segmentFilter = value; }
        }
    }
}

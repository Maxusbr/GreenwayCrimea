using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdvantShop.Catalog;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.CustomerSegments;
using AdvantShop.Diagnostics;
using AdvantShop.Web.Admin.Models.CustomerSegments;
using Newtonsoft.Json;

namespace AdvantShop.Web.Admin.Handlers.CustomerSegments
{
    public class SaveCustomerSegment
    {
        private readonly CustomerSegmentModel _segmentModel;

        public SaveCustomerSegment(CustomerSegmentModel segmentModel)
        {
            _segmentModel = segmentModel;
        }

        public int Execute()
        {
            var segment = _segmentModel.IsEditMode
                                ? CustomerSegmentService.Get(_segmentModel.Id)
                                : new CustomerSegment();

            if (segment == null)
                return 0;

            segment.Name = _segmentModel.Name.DefaultOrEmpty();

            var filter = _segmentModel.SegmentFilter;
            if (filter == null)
                filter = new CustomerSegmentFilter();

            if (filter.LastOrderDateTo != null)
                filter.LastOrderDateTo = new DateTime(filter.LastOrderDateTo.Value.Year, filter.LastOrderDateTo.Value.Month, filter.LastOrderDateTo.Value.Day, 23, 59, 59);

            if (filter.BirthDayTo != null)
                filter.BirthDayTo = new DateTime(filter.BirthDayTo.Value.Year, filter.BirthDayTo.Value.Month, filter.BirthDayTo.Value.Day, 23, 59, 59);

            filter.Categories = new List<int>();

            if (_segmentModel.Categories != null)
            {
                foreach (var cat in _segmentModel.Categories)
                {
                    var id = CategoryService.GetCategoryIdByName(cat);
                    if (id != 0)
                        filter.Categories.Add(id);
                }
            }

            filter.Cities = _segmentModel.Cities;
            filter.Countries = _segmentModel.Countries;

            filter.CustomerFields =
                _segmentModel.CustomerFields != null && _segmentModel.CustomerFields.Count > 0
                    ? _segmentModel.CustomerFields.Where(x => !string.IsNullOrEmpty(x.Value))
                        .Select(x => new KeyValuePair<int, string>(x.Id, x.Value))
                        .ToList()
                    : null;

            try
            {
                segment.Filter = JsonConvert.SerializeObject(filter);

                if (_segmentModel.IsEditMode)
                {
                    CustomerSegmentService.Update(segment);
                }
                else
                {
                    CustomerSegmentService.Add(segment);
                }

                Task.Run(() => new RecalcCustomerSegment(segment.Id).Execute());
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }

            return segment.Id;
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdvantShop.Catalog;
using AdvantShop.Core.Caching;
using AdvantShop.Models.Catalog;
using System;

namespace AdvantShop.Handlers.Catalog
{
    public class FilterPropertyHandler
    {
        #region Fields

        private readonly int _categoryId;
        private readonly bool _indepth;

        private readonly List<int> _selectedPropertyIds;
        private readonly List<int> _availablePropertyIds;
        private readonly Dictionary<int, KeyValuePair<float, float>> _rangePropertyIds;

        #endregion

        public FilterPropertyHandler(int categoryId, bool indepth, List<int> selectedPropertyIds,
                                    List<int> availablePropertyIds, Dictionary<int, KeyValuePair<float, float>> rangePropertyIds)
        {
            _categoryId = categoryId;
            _indepth = indepth;
            _selectedPropertyIds = selectedPropertyIds ?? new List<int>();
            _availablePropertyIds = availablePropertyIds;
            _rangePropertyIds = rangePropertyIds;
        }


        public List<FilterItemModel> Get()
        {
            var propertyValues = 
                CacheManager.Get(CacheNames.PropertiesInCategoryCacheName(_categoryId, _indepth),
                    () => PropertyService.GetPropertyValuesByCategories(_categoryId, _indepth));

            if (propertyValues == null)
                return new List<FilterItemModel>();

            var properties = propertyValues.Select(item => new UsedProperty
            {
                PropertyId = item.PropertyId,
                Name = string.IsNullOrEmpty(item.Property.NameDisplayed) ? item.Property.Name : item.Property.NameDisplayed,
                Expanded = item.Property.Expanded,
                Unit = item.Property.Unit,
                Type = item.Property.Type,
				Description = item.Property.Description,
                ValuesList = propertyValues.Where(value => value.PropertyId == item.PropertyId).ToList(),
            }).Distinct(new PropertyComparer());

            var list = new List<FilterItemModel>();

            foreach (var property in properties)
            {
                var item = new FilterItemModel()
                {
                    Expanded = property.Expanded,
                    Type = "prop",
                    Title = property.Name,
					Subtitle = property.Unit,
					Description = property.Description,
                };

                GetItemValues(item, property);

                if (item.Values.Count > 0)
                    list.Add(item);
            }

            return list;
        }

        private void GetItemValues(FilterItemModel item, UsedProperty property)
        {
            var selected = false;
            var expanded = false;

            switch (property.Type)
            {
                case (int)PropertyType.Checkbox:
                    item.Control = "checkbox";

                    foreach (var value in property.ValuesList)
                    {
                        selected = _selectedPropertyIds != null && _selectedPropertyIds.Contains(value.PropertyValueId);
                        if (selected)
                            expanded = true;

                        item.Values.Add(new FilterListItemModel()
                        {
                            Id = value.PropertyValueId.ToString(),
                            Text = value.Value,
                            Selected = selected,
                            Available =
                                _availablePropertyIds != null && !_availablePropertyIds.Contains(value.PropertyValueId) ? false : true,
                        });
                    }
                    break;

                case (int)PropertyType.Selectbox:
                    item.Control = "select";
                    
                    foreach (var value in property.ValuesList)
                    {
                        if (_availablePropertyIds == null ||
                            (_availablePropertyIds != null && _availablePropertyIds.Contains(value.PropertyValueId)))
                        {
                            if (!selected)
                                selected = _selectedPropertyIds != null && _selectedPropertyIds.Contains(value.PropertyValueId);

                            if (selected)
                                expanded = true;
                            
                            item.Values.Add(new FilterListItemModel()
                            {
                                Id = value.PropertyValueId.ToString(),
                                Text = value.Value,
                                Selected = selected,
                                Available = true,
                            });
                        }
                    }
                    break;

                case (int)PropertyType.Range:
                    item.Control = "range";

                    var list = _availablePropertyIds != null
                            ? property.ValuesList.Where(x => _availablePropertyIds.Contains(x.PropertyValueId)).Select(v => v.RangeValue).Where(v => v != 0).ToList()
                            : property.ValuesList.Select(v => v.RangeValue).Where(v => v != 0).ToList();

                    if (list.Count < 2)
                        break;
                    
                    var min = list.Min();
                    var max = list.Max();

                    if (min == max && min == 0)
                        break;

                    var curmin = min;
                    var curmax = max;

                    float step = 1f;
                    List<int> listDecimalPlaces = new List<int>();
                    listDecimalPlaces = list.Select(x => x.ToString().Remove(0, x.ToString().Contains(",") ? x.ToString().IndexOf(",") + 1 : 0).Length).ToList();
                    var range = Math.Abs(max - min);
                    CalcStep(range, ref step);

                    selected = _rangePropertyIds != null && _rangePropertyIds.ContainsKey(property.PropertyId);

                    if (selected)
                    {
                        curmin = _rangePropertyIds[property.PropertyId].Key;
                        curmax = _rangePropertyIds[property.PropertyId].Value;
                        expanded = true;
                    }

                    item.Values.Add(new FilterRangeItemModel()
                    {
                        Id = property.PropertyId,
                        CurrentMax = curmax,
                        CurrentMin = curmin,
                        DecimalPlaces = listDecimalPlaces.Max(),
                        Step = step,
                        Min = min,
                        Max = max,
                    });
                    break;
            }

            if (item.Expanded || expanded)
                item.Expanded = true;
        }



        public Task<List<FilterItemModel>> GetAsync()
        {
            return Task.Factory.StartNew(() =>
            {
                Localization.Culture.InitializeCulture();
                return Get();
            });
        }

        private void CalcStep(float range, ref float step)
        {
            if (range == 0)
            {
                step = 0;
            }
            else if (range < 0.001)
            {
                step = 0.00001f;
            }
            else if (range < 0.01)
            {
                step = 0.0001f;
            }
            else if (range < 0.1)
            {
                step = 0.001f;
            }
            else if (range < 1)
            {
                step = 0.01f;
            }
            else if (range < 10)
            {
                step = 0.1f;
            }
            else if (range < 100)
            {
                step = 1f;
            }
            else if (range < 1000)
            {
                step = 10f;
            }
            else if (range < 10000)
            {
                step = 100f;
            }
            else
            {
                step = 1000f;
            }
        }
    }
}
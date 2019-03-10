//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using AdvantShop.Configuration;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Diagnostics;
using AdvantShop.Orders;
using AdvantShop.Repository.Currencies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AdvantShop.Shipping
{  
    public class ShippingManager
    {
        private readonly PreOrder _preOrder;
        private readonly bool _showOnlyInDetails;

        public ShippingManager(PreOrder preOrder)
        {
            _preOrder = preOrder;
        }

        public ShippingManager(PreOrder preOrder, bool showOnlyInDetails)
        {
            _preOrder = preOrder;
            _showOnlyInDetails = showOnlyInDetails;
        }       

        public List<BaseShippingOption> GetOptions(bool getAll = true)
        {
            var listMethods = ShippingMethodService.GetAllShippingMethods(true);
            var availableMethods = GetShippingMethodsByGeoMapping(listMethods);
            var context = HttpContext.Current;

            if (_showOnlyInDetails)
                availableMethods = availableMethods.Where(x => x.ShowInDetails);

            if (!getAll && _preOrder.ShippingOption != null)
                availableMethods = availableMethods.Where(x => x.ShippingMethodId == _preOrder.ShippingOption.MethodId);

            var header = context.Request.Headers["not_delete_fix"];

            var baseItems = _preOrder.Items.DeepCloneJson();

            var tasks = availableMethods.Select(item => Task.Factory.StartNew(() =>
            {
                try
                {
                    HttpContext.Current = context;
                    var type = ReflectionExt.GetTypeByAttributeValue<ShippingKeyAttribute>(typeof(BaseShipping), atr => atr.Value, item.ShippingType);
                    //todo Пересмотреть передачу параметров, чтобы не требовалось DeepCloneJson
                    _preOrder.Items = baseItems.DeepCloneJson();
                    var curShipment = (BaseShipping)Activator.CreateInstance(type, item, _preOrder);
                    return curShipment.GetOptions();
                }
                catch (Exception ex)
                {
                    Debug.Log.Error(ex);
                    return null;
                }
            })).ToList();

            List<BaseShippingOption> items = null;

            // если пришел запрос от Я.Маркета ограничиваем время ответа
            if (HttpContext.Current != null && HttpContext.Current.Request.Url.ToString().Contains("/api/"))
            {
                var delay = Task.Delay(3850);
                Task.WhenAny(delay, Task.WhenAll(tasks)).Wait();

                items = tasks.Where(x => x.IsCompleted).Select(x => x.Result).Where(x => x != null).SelectMany(x => x).ToList();
            }
            else
            {
                items = tasks.Select(x => x.Result).Where(x => x != null).SelectMany(x => x).ToList();
            }

            var modules = AttachedModules.GetModules<IShippingCalculator>();
            foreach (var module in modules)
            {
                if (module != null)
                {
                    var classInstance = (IShippingCalculator)Activator.CreateInstance(module);
                    classInstance.ProcessOptions(items, _preOrder.Items);
                }
            }

            var currency = SettingsCatalog.DefaultCurrency;

            foreach (var item in items)
            {
                item.Rate = PriceService.RoundPrice(item.Rate, CurrencyService.CurrentCurrency, currency.Rate);
            }

            return items;
        }


        private IEnumerable<ShippingMethod> GetShippingMethodsByGeoMapping(IEnumerable<ShippingMethod> listMethods)
        {
            var items = new List<ShippingMethod>();
            foreach (var shippingMethod in listMethods)
            {
                if (ShippingPaymentGeoMaping.IsExistGeoShipping(shippingMethod.ShippingMethodId))
                {
                    if (ShippingPaymentGeoMaping.CheckShippingEnabledGeo(shippingMethod.ShippingMethodId, _preOrder.CountryDest, _preOrder.CityDest))
                        items.Add(shippingMethod);
                }
                else
                    items.Add(shippingMethod);
            }
            return items;
        }

        public override int GetHashCode()
        {
            return (_preOrder.CityDest ?? "").GetHashCode()
                   ^ (_preOrder.CountryDest ?? "").GetHashCode()
                   ^ (_preOrder.RegionDest ?? "").GetHashCode()
                   ^ (_preOrder.AddressDest ?? "").GetHashCode()
                   ^ (_preOrder.ZipDest ?? "").GetHashCode();
        }
    }
}
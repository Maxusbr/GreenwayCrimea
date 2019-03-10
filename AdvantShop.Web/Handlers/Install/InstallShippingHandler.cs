using System.Collections.Generic;
using AdvantShop.Core;
using AdvantShop.Shipping;
using AdvantShop.Shipping.Edost;
using AdvantShop.Shipping.FixedRate;
using AdvantShop.Trial;
using AdvantShop.ViewModel.Install;
using Resources;

namespace AdvantShop.Handlers.Install
{
    public class InstallShippingHandler
    {
        public InstallShippingModel Get()
        {
            var model = new InstallShippingModel()
            {
                ShowEddost = AdvantshopConfigService.GetActivityShipping("eDost"),
                ShowEdostPass = !(Demo.IsDemoEnabled || TrialService.IsTrialEnabled),
            };

            var sm = ShippingMethodService.GetShippingMethodByName(Resource.Install_UserContols_ShippingView_Chb_Self);
            if (sm != null)
            {
                model.UseSelfDelivery = true;
            }


            sm = ShippingMethodService.GetShippingMethodByName(Resource.Install_UserContols_ShippingView_Chb_Courier);
            if (sm != null && sm.ShippingType == "FixedRate")
            {
                model.UseCourier = true;
                model.Courier = sm.Params[FixeRateShippingTemplate.ShippingPrice];
            }

            sm = ShippingMethodService.GetShippingMethodByName(Resource.Install_UserContols_ShippingView_eDost);
            if (sm != null && sm.ShippingType == "eDost")
            {
                model.UseEdost = true;
                model.EDostNumer = sm.Params[EdostTemplate.ShopId];
                model.EDostPass = sm.Params[EdostTemplate.Password];
            }
            
            return model;
        }

        public void Update(InstallShippingModel model)
        {
            var sm = ShippingMethodService.GetShippingMethodByName(Resource.Install_UserContols_ShippingView_Chb_Self);
            if (sm != null)
                ShippingMethodService.DeleteShippingMethod(sm.ShippingMethodId);

            if (model.UseSelfDelivery)
            {
                var method = new ShippingMethod
                {
                    ShippingType = "FreeShipping",
                    Name = Resource.Install_UserContols_ShippingView_Chb_Self,
                    Description = string.Empty,
                    Enabled = true,
                    SortOrder = 0
                };
                ShippingMethodService.InsertShippingMethod(method);
            }

            sm = ShippingMethodService.GetShippingMethodByName(Resource.Install_UserContols_ShippingView_Chb_Courier);
            if (sm != null)
                ShippingMethodService.DeleteShippingMethod(sm.ShippingMethodId);

            if (model.UseCourier)
            {
                var method = new ShippingMethod
                {
                    ShippingType = "FixedRate",
                    Name = Resource.Install_UserContols_ShippingView_Chb_Courier,
                    Description = string.Empty,
                    Enabled = true,
                    SortOrder = 1
                };
                var id = ShippingMethodService.InsertShippingMethod(method);
                var parameters = new Dictionary<string, string>
                {
                    {FixeRateShippingTemplate.ShippingPrice, model.Courier},
                    //{FixeRateShippingTemplate.Extracharge, "0"},
                };
                ShippingMethodService.UpdateShippingParams(id, parameters);
            }

            sm = ShippingMethodService.GetShippingMethodByName(Resource.Install_UserContols_ShippingView_eDost);
            if (sm != null)
                ShippingMethodService.DeleteShippingMethod(sm.ShippingMethodId);

            if (model.UseEdost)
            {
                var method = new ShippingMethod
                {
                    ShippingType = "eDost",
                    Name = Resource.Install_UserContols_ShippingView_eDost,
                    Description = string.Empty,
                    Enabled = true,
                    SortOrder = 2
                };
                var id = ShippingMethodService.InsertShippingMethod(method);
                var parameters = new Dictionary<string, string>
                {
                    {EdostTemplate.ShopId, model.EDostNumer},
                    {EdostTemplate.Password, model.EDostPass},
                };
                ShippingMethodService.UpdateShippingParams(id, parameters);
            }
        }
    }
}
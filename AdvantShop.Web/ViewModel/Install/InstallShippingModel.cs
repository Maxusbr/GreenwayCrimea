using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Helpers;
using AdvantShop.Trial;
using Resources;

namespace AdvantShop.ViewModel.Install
{
    public class InstallShippingModel : IValidatableObject
    {
        public bool UseSelfDelivery { get; set; }
        public bool UseCourier { get; set; }
        public string Courier { get; set; }
        public bool ShowEddost { get; set; }
        public bool UseEdost { get; set; }
        public string EDostNumer { get; set; }
        public string EDostPass { get; set; }
        public bool ShowEdostPass { get; set; }
        
        public string BackUrl { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (UseCourier)
            {
                float isParse;
                if (Courier.IsNullOrEmpty() || !StringHelper.GetMoneyFromString(Courier, out isParse))
                {
                    yield return
                        new ValidationResult(Resource.Install_UserContols_ShippingView_Err_WrongPrice,
                            new[] {"Courier"});
                }
            }

            if (UseEdost)
            {
                if (string.IsNullOrWhiteSpace(EDostNumer))
                {
                    yield return
                        new ValidationResult(Resource.Install_UserContols_ShippingView_Err_NeedNum,
                            new[] { "EDostNumer" });
                }

                if (!(Demo.IsDemoEnabled || TrialService.IsTrialEnabled))
                {
                    if (string.IsNullOrWhiteSpace(EDostPass))
                    {
                        yield return
                            new ValidationResult(Resource.Install_UserContols_ShippingView_Err_NeedPass,
                                new[] { "EDostNumer" });
                    }
                }
            }
        }


    }
}
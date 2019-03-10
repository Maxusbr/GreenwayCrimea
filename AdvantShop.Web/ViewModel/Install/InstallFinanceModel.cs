using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Helpers;
using Resources;

namespace AdvantShop.ViewModel.Install
{
    public class InstallFinanceModel : IValidatableObject
    {
        public string SalesPlan { get; set; }
        public string ProfitPlan { get; set; }
        public bool AmountLimitation { get; set; }
        public string MinimalOrderPrice { get; set; }
        public string MaximalPriceCertificate { get; set; }
        public string MinimalPriceCertificate { get; set; }
        public bool ShowBankSettings { get; set; }
        //public string CompanyName { get; set; }
        //public string CompanyAddress { get; set; }
        //public string Inn { get; set; }
        //public string Kpp { get; set; }
        //public string Rs { get; set; }
        //public string BankName { get; set; }
        //public string Ks { get; set; }
        //public string Bik { get; set; }
        //public string StampUrl { get; set; }
        public string BackUrl { get; set; }
        public string Error { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            float isParse = 0;
            if (!SalesPlan.IsNotEmpty() || !StringHelper.GetMoneyFromString(SalesPlan, out isParse))
            {
                yield return new ValidationResult(Resource.Install_UserContols_FinanceView_Err_Plan, new[] { "SalesPlan" });
            }

            if (!ProfitPlan.IsNotEmpty() || !StringHelper.GetMoneyFromString(ProfitPlan, out isParse))
            {
                yield return new ValidationResult(Resource.Install_UserContols_FinanceView_Err_PlaPribl, new[] { "ProfitPlan" });
            }

            if (!MinimalOrderPrice.IsNotEmpty() || !StringHelper.GetMoneyFromString(MinimalOrderPrice, out isParse))
            {
                yield return new ValidationResult(Resource.Install_UserContols_FinanceView_Err_MinOrderPrice, new[] { "MinimalOrderPrice" });
            }

            if (!MaximalPriceCertificate.IsNotEmpty() || !StringHelper.GetMoneyFromString(MaximalPriceCertificate, out isParse))
            {
                yield return new ValidationResult(Resource.Install_UserContols_FinanceView_Err_Plan, new[] { "MaximalPriceCertificate" });
            }

            if (!MinimalPriceCertificate.IsNotEmpty() || !StringHelper.GetMoneyFromString(MinimalPriceCertificate, out isParse))
            {
                yield return new ValidationResult(Resource.Install_UserContols_FinanceView_Err_MinPriceGift, new[] { "MinimalPriceCertificate" });
            }

            if (HttpContext.Current != null && HttpContext.Current.Request.Files.Count > 0)
            {
                var stamp = HttpContext.Current.Request.Files["stamp"];
                if (stamp != null && stamp.ContentLength > 0)
                {
                    if (!FileHelpers.CheckFileExtension(stamp.FileName, EAdvantShopFileTypes.Image))
                    {
                        yield return new ValidationResult(Resource.Admin_CommonSettings_InvalidLogoFormat, new[] { "StampUrl" });
                    }
                }
            }
        }
    }
}
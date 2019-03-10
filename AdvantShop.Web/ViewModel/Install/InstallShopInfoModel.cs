using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Customers;
using AdvantShop.Helpers;
using AdvantShop.Saas;
using AdvantShop.Trial;
using Resources;

namespace AdvantShop.ViewModel.Install
{
    public class InstallShopInfoModel : IValidatableObject
    {
        public bool DisplayKey { get; set; }
        public string Key { get; set; }
        public string ShopName { get; set; }
        public string ShopUrl { get; set; }

        public string Country { get; set; }
        public List<SelectListItem> Countries { get; set; } 

        public string Region { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        //public string Director { get; set; }
        //public string Accountant { get; set; }
        //public string Manager { get; set; }
        public string Error { get; set; }
        public string LogoUrl { get; set; }
        public string FaviconUrl { get; set; }
        public string BackUrl { get; set; }

        public InstallShopInfoModel()
        {
            Key = string.Empty;
            ShopName = string.Empty;
            ShopUrl = string.Empty;
            Country = string.Empty;
            Region = string.Empty;
            City = string.Empty;
            Phone = string.Empty;
            //Director = string.Empty;
            //Accountant = string.Empty;
            //Manager = string.Empty;
        }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ShopName.IsNullOrEmpty())
            {
                yield return new ValidationResult(Resource.Install_UserContols_ShopinfoView_Err_NeedName, new[]{"ShopName"});
            }

            if (ShopUrl.IsNullOrEmpty())
            {
                yield return new ValidationResult(Resource.Install_UserContols_ShopinfoView_Err_NeedUrl, new[] { "ShopUrl" });
            }

            if (!(Demo.IsDemoEnabled || TrialService.IsTrialEnabled || SaasDataService.IsSaasEnabled))
            {
                if (!SettingsLic.Activate(Key))
                    yield return new ValidationResult(Resource.Install_UserContols_ShopinfoView_Err_WrongKey, new[] { "Key" });
            }

            if (HttpContext.Current != null && HttpContext.Current.Request.Files.Count > 0)
            {
                var logo = HttpContext.Current.Request.Files["logo"];
                if (logo != null && logo.ContentLength > 0)
                {
                    if (!FileHelpers.CheckFileExtension(logo.FileName, EAdvantShopFileTypes.Image))
                    {
                        yield return new ValidationResult(Resource.Admin_CommonSettings_InvalidLogoFormat, new[] { "LogoUrl" });
                    }
                }

                var favicon = HttpContext.Current.Request.Files["favicon"];
                if (favicon != null && favicon.ContentLength > 0)
                {
                    if (!FileHelpers.CheckFileExtension(favicon.FileName, EAdvantShopFileTypes.Favicon))
                    {
                        yield return new ValidationResult(Resource.Admin_CommonSettings_InvalidFaviconFormat, new[] { "FaviconUrl" });
                    }
                }
            }
        }
    }
}
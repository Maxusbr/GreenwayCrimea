using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdvantShop.Configuration;
using AdvantShop.Design;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.Repository;
using AdvantShop.Trial;
using AdvantShop.ViewModel.Install;

namespace AdvantShop.Handlers.Install
{
    public class InstallShopInfoHandler
    {
        public InstallShopInfoModel Get()
        {
            var model = new InstallShopInfoModel()
            {
                ShopName = SettingsMain.ShopName,
                ShopUrl = SettingsMain.SiteUrl,
                Key = SettingsLic.LicKey,
                DisplayKey = !(Demo.IsDemoEnabled || TrialService.IsTrialEnabled || Saas.SaasDataService.IsSaasEnabled),
                Phone = SettingsMain.Phone,
                //Director = SettingsBank.Director,
                //Accountant = SettingsBank.Accountant,
                //Manager = SettingsBank.Manager,
                City = SettingsMain.City
            };

            if (!string.IsNullOrWhiteSpace(SettingsMain.LogoImageName))
                model.LogoUrl = FoldersHelper.GetPath(FolderType.Pictures, SettingsMain.LogoImageName, true);

            if (!string.IsNullOrWhiteSpace(SettingsMain.FaviconImageName))
                model.FaviconUrl = FoldersHelper.GetPath(FolderType.Pictures, SettingsMain.FaviconImageName, true);
            

            var countryId = SettingsMain.SellerCountryId;
            model.Country = countryId.ToString();

            model.Countries =
                CountryService.GetAllCountries()
                    .Select(
                        x =>
                            new SelectListItem()
                            {
                                Text = x.Name,
                                Value = x.CountryId.ToString(),
                                Selected = x.CountryId == countryId
                            }).ToList();
            
            
            if (SettingsMain.SellerRegionId != 0)
            {
                var region = RegionService.GetRegion(SettingsMain.SellerRegionId);
                if (region != null)
                    model.Region = region.Name;
            }
            
            return model;
        }

        public void Update(InstallShopInfoModel model)
        {
            if (!(Demo.IsDemoEnabled || TrialService.IsTrialEnabled))
            {
                SettingsLic.LicKey = model.Key;
            }

            if (!TemplateService.IsExistTemplate(SettingsDesign.Template))
                SettingsDesign.ChangeTemplate(TemplateService.DefaultTemplateId);

            SettingsMain.ShopName = model.ShopName;
            SettingsMain.SiteUrl = model.ShopUrl;
            SettingsMain.LogoImageAlt = model.ShopName;

            if (HttpContext.Current.Request.Files.Count > 0)
            {
                var logo = HttpContext.Current.Request.Files["logo"];
                if (logo != null && logo.ContentLength > 0)
                {
                    FileHelpers.CreateDirectory(FoldersHelper.GetPathAbsolut(FolderType.Pictures));
                    FileHelpers.DeleteFile(FoldersHelper.GetPathAbsolut(FolderType.Pictures, SettingsMain.LogoImageName));
                    SettingsMain.LogoImageName = logo.FileName;
                    logo.SaveAs(FoldersHelper.GetPathAbsolut(FolderType.Pictures, logo.FileName));
                }

                var favicon = HttpContext.Current.Request.Files["favicon"];
                if (favicon != null && favicon.ContentLength > 0)
                {
                    FileHelpers.CreateDirectory(FoldersHelper.GetPathAbsolut(FolderType.Pictures));
                    FileHelpers.DeleteFile(FoldersHelper.GetPathAbsolut(FolderType.Pictures, SettingsMain.FaviconImageName));
                    SettingsMain.FaviconImageName = favicon.FileName;
                    favicon.SaveAs(FoldersHelper.GetPathAbsolut(FolderType.Pictures, favicon.FileName));
                }
            }

            var countryId = 0;
            int.TryParse(model.Country, out countryId);
            SettingsMain.SellerCountryId = countryId;

            var regionId = !string.IsNullOrEmpty(model.Region) ? RegionService.GetRegionIdByName(model.Region) : 0;
            SettingsMain.SellerRegionId = regionId;

            SettingsMain.City = model.City;
            SettingsMain.Phone = model.Phone;
            //SettingsBank.Director = model.Director;
            //SettingsBank.Accountant = model.Accountant;
            //SettingsBank.Manager = model.Manager;
        }
    }
}
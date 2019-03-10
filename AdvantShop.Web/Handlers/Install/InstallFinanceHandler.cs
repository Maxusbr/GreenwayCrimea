using System;
using System.Web;
using AdvantShop.Configuration;
using AdvantShop.Core;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.Orders;
using AdvantShop.ViewModel.Install;

namespace AdvantShop.Handlers.Install
{
    public class InstallFinanceHandler
    {
        public InstallFinanceModel Get()
        {
            var model = new InstallFinanceModel()
            {
                SalesPlan = String.Format("{0:## ##0.00}", OrderStatisticsService.SalesPlan),
                ProfitPlan = String.Format("{0:## ##0.00}", OrderStatisticsService.ProfitPlan),
                //CompanyName = SettingsBank.CompanyName,
                //CompanyAddress = SettingsBank.Address,
                //Inn = SettingsBank.INN,
                //Kpp = SettingsBank.KPP,
                //Rs = SettingsBank.KPP,
                //BankName = SettingsBank.BankName,
                //Ks = SettingsBank.KS,
                //Bik = SettingsBank.BIK,
                AmountLimitation = SettingsCheckout.AmountLimitation,
                MinimalOrderPrice = String.Format("{0:## ##0.00}", SettingsCheckout.MinimalOrderPriceForDefaultGroup),
                MinimalPriceCertificate = String.Format("{0:## ##0.00}", SettingsCheckout.MinimalPriceCertificate),
                MaximalPriceCertificate = String.Format("{0:## ##0.00}", SettingsCheckout.MaximalPriceCertificate),
                //StampUrl =
                //    !string.IsNullOrWhiteSpace(SettingsBank.StampImageName)
                //        ? FoldersHelper.GetPath(FolderType.Pictures, SettingsBank.StampImageName, true)
                //        : null,
                ShowBankSettings = AdvantshopConfigService.GetActivityCommonSetting("banksettings")
            };
            
            return model;
        }

        public void Update(InstallFinanceModel model)
        {
            float sales;
            float profit;
            StringHelper.GetMoneyFromString(model.SalesPlan, out sales);
            StringHelper.GetMoneyFromString(model.ProfitPlan, out profit);
            OrderStatisticsService.SetProfitPlan(sales, profit);

            SettingsCheckout.AmountLimitation = model.AmountLimitation;

            float minimalOrderPrice;
            float minimalPriceCertificate;
            float maximalPriceCertificate;

            StringHelper.GetMoneyFromString(model.MinimalOrderPrice, out minimalOrderPrice);
            SettingsCheckout.MinimalOrderPriceForDefaultGroup = minimalOrderPrice;

            StringHelper.GetMoneyFromString(model.MinimalPriceCertificate, out minimalPriceCertificate);
            SettingsCheckout.MinimalPriceCertificate = minimalPriceCertificate;

            StringHelper.GetMoneyFromString(model.MaximalPriceCertificate, out maximalPriceCertificate);
            SettingsCheckout.MaximalPriceCertificate = maximalPriceCertificate;

            //SettingsBank.Address = model.CompanyAddress;
            //SettingsBank.CompanyName = model.CompanyName;
            //SettingsBank.INN = model.Inn;
            //SettingsBank.KPP = model.Kpp;
            //SettingsBank.RS = model.Rs;
            //SettingsBank.BankName = model.BankName;
            //SettingsBank.KS = model.Ks;
            //SettingsBank.BIK = model.Bik;

            //if (HttpContext.Current.Request.Files.Count > 0)
            //{
            //    var stamp = HttpContext.Current.Request.Files["stamp"];
            //    if (stamp != null && stamp.ContentLength > 0)
            //    {
            //        FileHelpers.CreateDirectory(FoldersHelper.GetPathAbsolut(FolderType.Pictures));
            //        FileHelpers.DeleteFile(FoldersHelper.GetPathAbsolut(FolderType.Pictures, SettingsBank.StampImageName));
            //        SettingsBank.StampImageName = stamp.FileName;
            //        stamp.SaveAs(FoldersHelper.GetPathAbsolut(FolderType.Pictures, stamp.FileName));
            //    }
            //}
        }
    }
}
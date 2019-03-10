//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;
using System.IO;
using AdvantShop.Core.Services.Localization;
using AdvantShop.FilePath;

namespace AdvantShop.Payment
{
    public class PaymentIcons
    {
        private static Dictionary<string, int> Icons = new Dictionary<string, int>()
        {
            {"sberbank", 1},
            {"bill", 2},
            {"cash", 3},
            {"mailru", 4},
            {"webmoney", 5},
            {"robokassa", 6},
            {"yandexmoney", 7},
            {"authorizenet", 8},
            {"googlecheckout", 9},
            {"eway", 10},
            {"check", 11},
            {"paypal", 12},
            {"twocheckout", 13},
            {"assist", 14},
            {"zpayment", 15},
            {"platron", 16},
            {"rbkmoney", 17},
            {"cyberplat", 18},
            {"moneybookers", 19},
            {"amazonsimplepay", 20},
            {"chronopay", 21},
            {"payonline", 22},
            {"qiwi", 23},
            {"psigate", 24},
            {"paypoint", 25},
            {"sagepay", 26},
            {"worldpay", 27},
            {"cashondelivery", 28},
            {"pickpoint", 29},
            {"onpay", 30},
            {"walletonecheckout", 31},
            {"giftcertificate", 32},
            {"masterbank", 33},
            {"interkassa", 34},
            {"liqpay", 35},
            {"billua", 36},
            {"kupivkredit", 37},
            {"yescredit", 38},
            {"payanyway", 39},
            {"moscowbank", 40},
            {"gateline", 41},
            {"qppi", 42},
            {"bitpay", 43},
            {"intellectmoney", 44},
            {"avangard", 45},
            {"dibs", 46},
            {"rsbcredit", 47},
            {"directcredit", 48},
            {"paypalexpresscheckout", 49},
            {"interkassa2", 50},
            {"monexy", 51},
            {"netpay", 52},
            {"yandexkassa", 53},
            {"alfabankua", 54},
            {"intellectmoneymainprotocol", 55},
            {"cloudpayments", 56},
            {"paymaster", 57},
            {"sberbankacquiring", 58},
            {"webpay", 59 }
        };

        public static string GetPaymentIcon(string type, string iconName, string name)
        {
            name = name.ToLower();

            if (!string.IsNullOrWhiteSpace(iconName) && File.Exists(FoldersHelper.GetPathAbsolut(FolderType.PaymentLogo, iconName)))
            {
                return FoldersHelper.GetPath(FolderType.PaymentLogo, iconName, false);
            }

            var folderPath = FoldersHelper.GetPath(FolderType.PaymentLogo, string.Empty, false);

            if (name.Contains(LocalizationService.GetResource("Core.Payment.PaymentIcons.CreditCard").ToLower()))
            {
                return folderPath + "plasticcard.gif";
            }
            if (name.Contains(LocalizationService.GetResource("Core.Payment.PaymentIcons.ElectronMoney").ToLower()))
            {
                return folderPath + "emoney.gif";
            }
            if (name.Contains(LocalizationService.GetResource("Core.Payment.PaymentIcons.Qiwi").ToLower()))
            {
                return folderPath + "qiwi.gif";
            }
            if (name.Contains(LocalizationService.GetResource("Core.Payment.PaymentIcons.Euroset").ToLower()))
            {
                return folderPath + "euroset.gif";
            }
            if (name.Contains("кредит"))
            {
                return folderPath + "loan.jpg";
            }

            if (name.Contains("терминал"))
            {
                return folderPath + "terminal.jpg";
            }

            var typeIcon = type.ToLower();

            if (Icons.ContainsKey(typeIcon))
            {
                var defaultPicture = string.Format("{0}{1}_default.gif", folderPath, Icons[typeIcon]);
                if (File.Exists(FoldersHelper.GetPathAbsolut(FolderType.PaymentLogo, defaultPicture)))
                {
                    return defaultPicture;
                }
            }

            return folderPath + "3_default.gif";
        }
    }
}
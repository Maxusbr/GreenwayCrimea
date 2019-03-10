//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;
using System.IO;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.FilePath;

namespace AdvantShop.Shipping
{
    public class ShippingIcons
    {
        private static Dictionary<string, int> Icons = new Dictionary<string, int>()
        {
            {"freeshipping", 1},
            {"fixedrate", 2},
            {"shippingbyweight", 3},
            {"fedex", 4},
            {"ups", 5},
            {"usps", 6},
            {"edost", 7},
            {"shippingbyshippingcost", 8},
            {"shippingbyorderprice", 9},
            {"shippingbyrangeweightanddistance", 10},
            {"novaposhta", 11},
            {"selfdelivery", 12},
            {"multiship", 13},
            {"sdek", 14},
            {"shippingbyproductamount", 15},
            {"emspost", 16},
            {"checkoutru", 17},
        };

        public static string GetShippingIcon(string type, string iconName, string namefragment)
        {
            string folderPath = FoldersHelper.GetPath(FolderType.ShippingLogo, null, false);
            
            if ((type == "Edost" || type == "Sdek" || type == "YandexDelivery" || type == "CheckoutRu" || type == "DDelivery" || type == "Boxberry") &&
                iconName.IsNullOrEmpty())
            {
                namefragment = namefragment.ToLower();
                if (namefragment.Contains("ems"))
                    return folderPath + "7_ems.gif";
                if (namefragment.Contains("почта россии"))
                    return folderPath + "7_pochtarussia.gif";
                if (namefragment.Contains("спср экспресс") || namefragment.Contains("спср"))
                    return folderPath + "7_spsrExpress.gif";
                if (namefragment.Contains("сдэк"))
                    return folderPath + "7_Sdek.gif";
                if (namefragment.Contains("dhl"))
                    return folderPath + "7_dhl.gif";
                if (namefragment.Contains("ups"))
                    return folderPath + "7_ups.gif";
                if (namefragment.Contains("желдорэкспедиция"))
                    return folderPath + "7_trainroadExpedition.gif";
                if (namefragment.Contains("автотрейдинг"))
                    return folderPath + "7_autotraiding.gif";
                if (namefragment.Contains("пэк"))
                    return folderPath + "7_pek.gif";
                if (namefragment.Contains("деловые линии"))
                    return folderPath + "7_delovielinies.gif";
                if (namefragment.Contains("мегаполис"))
                    return folderPath + "7_megapolis.gif";
                if (namefragment.Contains("гарантпост"))
                    return folderPath + "7_garantpost.gif";
                if (namefragment.Contains("pony"))
                    return folderPath + "7_ponyexpress.gif";
                if (namefragment.Contains("pickpoint"))
                    return folderPath + "7_pickpoint.gif";
                if (namefragment.Contains("boxberry"))
                    return folderPath + "7_boxberry.gif";
                if (namefragment.Contains("энергия"))
                    return folderPath + "7_energia.gif";
                if (namefragment.Contains("hermes"))
                    return folderPath + "7_hermes.gif";
                if (namefragment.Contains("dpd"))
                    return folderPath + "7_dpd.gif";
                if (namefragment.Contains("shoplogistics"))
                    return folderPath + "7_shoplogistics.gif";
                if (namefragment.Contains("b2c"))
                    return folderPath + "7_b2c.gif";
                if (namefragment.Contains("inpost"))
                    return folderPath + "7_inpost.gif";
                if (namefragment.Contains("ратэк"))
                    return folderPath + "7_ratek.gif";

                if (iconName.IsNotEmpty() && File.Exists(FoldersHelper.GetPathAbsolut(FolderType.ShippingLogo, iconName)))
                    return folderPath + iconName;

                return folderPath + "7_default.gif";
            }

            if (iconName.IsNotEmpty() && File.Exists(FoldersHelper.GetPathAbsolut(FolderType.ShippingLogo, iconName)))
                return folderPath + iconName;

            return string.Format("{0}{1}.gif", folderPath,
                Icons.ContainsKey(type.ToLower()) ? Icons[type.ToLower()].ToString() : "7_default");
        }
    }
}
//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
//using System.ServiceModel;
using System.Text;
using AdvantShop.Catalog;

namespace AdvantShop.Modules
{
    public class RitmzImportRemains
    {
        private static string _moduleName = "Ritmz";
        // it work with using file to mimimaze memory used
        private enum RitmZErrors
        {
            StartTimeInvalid,
            EndTimeInvalid
        }

        private static void WriteError(List<RitmZErrors> list, string exportWay)
        {
            using (var fs = new FileStream(exportWay, FileMode.Create))
            using (var writer = new StreamWriter(fs, Encoding.UTF8))
            {
                writer.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                if (list.Count > 0)
                {
                    writer.WriteLine("<errors>");
                    for (int i = 0; i < list.Count; i++)
                        writer.WriteLine(string.Format("<error>{0}</error>", GetErrorString(list[i])));
                    writer.WriteLine("</errors>");
                }
            }
        }

        private static string GetErrorString(RitmZErrors item)
        {
            switch (item)
            {
                case RitmZErrors.StartTimeInvalid:
                    return "Не указана дата начала выгрузки";
                case RitmZErrors.EndTimeInvalid:
                    return "Не указана дата окончания выгрузки";
                default:
                    return string.Empty;
            }
        }

        public static void GetRemains(string productsArtS, string warehouse)
        {
            throw new Exception("TODO");
            //TODO: Uncomment!!!

            //var binding = new BasicHttpBinding(BasicHttpSecurityMode.TransportCredentialOnly);
            //binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;
            //var endPointAddr = new EndpointAddress("http://cc.Ritm-Z.com:8008/RitmZ_GM82/ws/GetRemains.1cws");
            //var getRemains = new RitmzRemains.WSGetRemainsPortTypeClient(binding, endPointAddr);
            //getRemains.ClientCredentials.UserName.UserName = ModuleSettingsProvider.GetSettingValue<string>("RitmzLogin", _moduleName);
            //getRemains.ClientCredentials.UserName.Password = ModuleSettingsProvider.GetSettingValue<string>("RitmzPassword", _moduleName); 
            //var ritmzRemains = getRemains.ПолучитьОстатки(productsArtS, warehouse);
            //if (ritmzRemains.remains != null)
            //{
            //    for (int i = 0; i < ritmzRemains.remains.Length; ++i)
            //    {
            //        int productId = 0;
            //        if (!(Int32.TryParse(ritmzRemains.remains[i].item.id, out productId) && ProductService.IsExists(productId)))
            //            continue;

            //        var offer = OfferService.GetProductOffers(productId).First();
            //        offer.Amount = ritmzRemains.remains[0].quantity;
            //        OfferService.UpdateOffer(offer);
            //    }
            //}
        }
    }
}
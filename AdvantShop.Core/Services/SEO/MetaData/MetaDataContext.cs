using System;
using System.Web;
using System.Collections.Generic;
using AdvantShop.Configuration;
using AdvantShop.FilePath;


namespace AdvantShop.Core.Services.SEO.MetaData
{
    public class MetaDataContext
    {
        private const string MetaDataObjectKey = "MetaDataObjectKey";

        public static OpenGraphModel CurrentObject
        {
            get
            {
                return HttpContext.Current == null ? null : HttpContext.Current.Items[MetaDataObjectKey] as OpenGraphModel;
            }
            set
            {
                if (HttpContext.Current != null)
                    HttpContext.Current.Items[MetaDataObjectKey] = value;
            }
        }
    }

    public class OpenGraphModel
    {
        public OpenGraphModel()
        {
            SiteName = SettingsMain.ShopName;
            Admins = SettingsSEO.OpenGraphFbAdmins;
            Url = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Path);
            Type = OpenGraphType.Website;
            Images = new List<string>();

            if (!string.IsNullOrEmpty(SettingsMain.LogoImageName))
                Images.Add(FoldersHelper.GetPath(FolderType.Pictures, SettingsMain.LogoImageName, false));
        }

        public string SiteName { get; set; }

        public string Url { get; set; }

        public OpenGraphType Type { get; set; }

        public string Admins { get; private set; }

        public List<string> Images { get; set; }
    }

    public enum OpenGraphType
    {
        Website = 0,
        Product = 1,
        Article = 2
    };
}

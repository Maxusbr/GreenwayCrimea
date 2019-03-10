//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.IO;
using AdvantShop.Configuration;
using AdvantShop.Core.Controls;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.ExportImport;
using Resources;

namespace Admin
{
    partial class SiteMapGenerateXML : AdvantShopAdminPage
    {
        // Leave empty if you don't need subfolders
        private const string MapFolder = ""; //"sitemap/"
        private const string FileName = "sitemap.xml";

        private readonly string _fileAbsPath;

        private void MsgErr(string messageText)
        {
            lblError.Visible = true;
            lblError.Text = messageText;
        }

        protected string LinkToSiteMapFile
        {
            get { return UrlService.GetAbsoluteLink("/") + FileName; }
        }

        protected SiteMapGenerateXML()
        {
            _fileAbsPath = SettingsGeneral.AbsolutePath + FileName;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblError.Visible = false;
            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_SiteMapGenerate_Header));
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            lastMod.Text = File.Exists(_fileAbsPath)
                ? AdvantShop.Localization.Culture.ConvertDate((new FileInfo(_fileAbsPath).LastWriteTime))
                : @"---";
        }

        protected void btnCreateMap_Click(object sender, EventArgs e)
        {
            new ExportXmlMap(_fileAbsPath).Create();
        }
    }
}
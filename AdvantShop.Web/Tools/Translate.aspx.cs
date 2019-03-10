//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web.Hosting;
using CsvHelper.Configuration;

namespace Tools
{
    public partial class Translate : System.Web.UI.Page
    {

        protected void Button1_Click(object sender, EventArgs e)
        {
            Tr();
        }

        private void Tr()
        {
            var fullFileName = HostingEnvironment.MapPath("~/loc.csv");
            var sbRu = new StringBuilder();
            var sbEn = new StringBuilder();

            using (var csvReader = new CsvHelper.CsvReader(new StreamReader(fullFileName), new CsvConfiguration() {Delimiter = ",", HasHeaderRecord = false,}))
            {
                while (csvReader.Read())
                {
                    var key = csvReader.GetField(0);
                    var resource = csvReader.GetField(1);
                    var ru = csvReader.GetField(2);
                    var en = csvReader.GetField(3);

                    if (string.IsNullOrWhiteSpace(key))
                    {
                        sbRu.AppendLine("");
                        sbEn.AppendLine("");
                        continue;
                    }

                    var ruWord = "";
                    var enWord = "";

                    if (!string.IsNullOrEmpty(resource))
                    {
                        ruWord = Resources.Resource.ResourceManager.GetString(resource, new CultureInfo("ru-RU"));
                        enWord = Resources.Resource.ResourceManager.GetString(resource, new CultureInfo("en-US"));
                    }
                    else
                    {
                        ruWord = ru;
                        enWord = en;
                    }

                    ruWord = ruWord.Replace("'", "''");
                    enWord = enWord.Replace("'", "''");

                    sbRu.AppendFormat(
                        "Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, '{0}', '{1}'); \n",
                        key, ruWord);

                    sbEn.AppendFormat(
                        "Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, '{0}', '{1}'); \n",
                        key, enWord);
                }
            }


            lblRu.Text = sbRu.ToString();
            lblEn.Text = sbEn.ToString();
        }

    }
}
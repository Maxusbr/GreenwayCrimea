using System;
using System.IO;
using System.Web.Hosting;
using AdvantShop.App.Landing.Controllers.Domain;
using AdvantShop.Diagnostics;
using Newtonsoft.Json;

namespace AdvantShop.App.Landing.Domain
{
    public class LpBlockConfigService
    {
        public LpBlockConfig Get(string blockKey, string templateName)
        {
            try
            {
                var path = string.Format("{0}/{1}/Blocks/{2}/", LpService.TepmlateFolder, templateName, blockKey);
                var configPath = HostingEnvironment.MapPath(path + "config.json");

                if (!File.Exists(configPath))
                {
                    path = string.Format("{0}/Blocks/{1}/", LpService.ViewsFolder, blockKey);
                    configPath = HostingEnvironment.MapPath(path + "config.json");
                    if (!File.Exists(configPath))
                        return null;
                }

                var configContent = "";

                using (var sr = new StreamReader(configPath))
                    configContent = sr.ReadToEnd();

                var blockConfig = JsonConvert.DeserializeObject<LpBlockConfig>(configContent);
                if (blockConfig != null)
                {
                    blockConfig.BlockPath = path + blockKey + ".cshtml";
                }

                return blockConfig;
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }

            return null;
        }
    }
}

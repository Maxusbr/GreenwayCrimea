using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Hosting;
using AdvantShop.App.Landing.Controllers.Domain;
using AdvantShop.Diagnostics;
using Newtonsoft.Json;

namespace AdvantShop.App.Landing.Domain
{
    public class LpTemplateService
    {
        public List<LpTemplate> GetTemplates()
        {
            var templates = new List<LpTemplate>();

            try
            {
                var path = HostingEnvironment.MapPath(LpService.TepmlateFolder);
                foreach (var directory in Directory.GetDirectories(path))
                {
                    var configPath = directory + "\\config.json";
                    if (File.Exists(configPath))
                    {
                        var configContent = "";

                        using (var sr = new StreamReader(configPath))
                            configContent = sr.ReadToEnd();

                        var template = JsonConvert.DeserializeObject<LpTemplate>(configContent);
                        if (template != null && !string.IsNullOrEmpty(template.Name))
                            templates.Add(template);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }

            return templates;
        }

        public LpTemplate GetTemplate(string name)
        {
            try
            {
                var configPath = HostingEnvironment.MapPath(LpService.TepmlateFolder + "/" + name + "\\config.json");
                if (File.Exists(configPath))
                {
                    var configContent = "";

                    using (var sr = new StreamReader(configPath))
                        configContent = sr.ReadToEnd();

                    return JsonConvert.DeserializeObject<LpTemplate>(configContent);
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }

            return null;
        }

    }
}

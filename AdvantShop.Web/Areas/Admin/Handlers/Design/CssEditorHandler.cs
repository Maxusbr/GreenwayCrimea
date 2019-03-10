
using AdvantShop.Diagnostics;
using System;
using System.IO;



namespace AdvantShop.Web.Admin.Handlers.Design
{
    public class CssEditorHandler
    {
        string _path;

        public CssEditorHandler() {
            _path = System.Web.Hosting.HostingEnvironment.MapPath("~/userfiles/extra.css");
        }

        public string GetFileContent()
        {
            string cssContent = string.Empty;
            try
            {
                if (!File.Exists(_path))
                {
                    using (File.Create(_path))
                    {
                        //nothing here, just  create file
                    }
                }

                using (TextReader reader = new StreamReader(_path))
                {
                    cssContent = reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }
            return cssContent;
        }

        public bool SaveFileContent(string cssContent)
        {
            try
            {
                using (TextWriter writer = new StreamWriter(_path, false))
                {
                    writer.Write(cssContent);
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
                return false;
            }

            return true;
        }
    }
}

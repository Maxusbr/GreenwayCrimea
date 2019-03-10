using System.Collections.Generic;
using AdvantShop.Configuration;
using AdvantShop.Core.Caching;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using SquishIt.Framework;
using System.Web.Hosting;
using System.IO;

namespace AdvantShop.Core
{
    public class JsCssTool
    {
        private const string CacheKeyPrefix = "squishit_";
        
        public static string MiniCss(List<string> paths, string filename)
        {
            var outputfile = "~/" + FoldersHelper.PhotoFoldersPath[FolderType.Combine] + filename;
            
            var bundle = Bundle.Css();

            //if (File.Exists(HostingEnvironment.MapPath(outputfile)))
            //{
            //    return string.Format("<link rel=\"stylesheet\" type=\"text/css\" href=\"{0}?{1}\" />", outputfile.Trim('~'), File.GetLastWriteTime(outputfile).ToString("yyyyMMddHHmmss"));
            //}
            //else
            //{
            //    bundle.ClearCache();

                foreach (var item in paths)
                    bundle.Add(item);

                return bundle.WithMinifier(new SquishIt.Framework.Minifiers.CSS.MsMinifier()).Render(outputfile); //  + "?#"
            //}
        }

        public static string MiniJs(List<string> paths, string filename)
        {
            var outputfile = "~/" + FoldersHelper.PhotoFoldersPath[FolderType.Combine] + filename;
            var bundle = Bundle.JavaScript();

            //if (File.Exists(HostingEnvironment.MapPath(outputfile)))
            //{
            //    return string.Format("<script type=\"text/javascript\" src=\"{0}?{1}\"></script>", outputfile.Trim('~'), File.GetLastWriteTime(outputfile).ToString("yyyyMMddHHmmss"));
            //}
            //else
            //{
                foreach (var item in paths)
                    bundle.Add(item);

            //    bundle.ClearCache();

                return bundle.WithMinifier(new SquishIt.Framework.Minifiers.JavaScript.MsMinifier()).Render(outputfile); //  + "?#"
            //}
        }

        public static void ReCreateIfNotExist()
        {
            if (!FileHelpers.IsDirectoryHaveFiles(SettingsGeneral.AbsolutePath + "/" + FoldersHelper.PhotoFoldersPath[FolderType.Combine]))
            {
                CacheManager.RemoveByPattern(CacheKeyPrefix);
            }
        }

        public static void Clear()
        {
            FileHelpers.DeleteFilesFromPath(SettingsGeneral.AbsolutePath + "/" + FoldersHelper.PhotoFoldersPath[FolderType.Combine]);
        }
    }
}
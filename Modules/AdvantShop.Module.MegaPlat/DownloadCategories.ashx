<%@ WebHandler Language="C#" Class="Advantshop.UserControls.Modules.DownloadCategories" %>

using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using AdvantShop.Catalog;
using AdvantShop.Core.Modules;


namespace Advantshop.UserControls.Modules
{
    public class DownloadCategories : IHttpHandler
    {
        private const string _moduleName = "MegaPlat";

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";

            if (context.Request["apikey"] !=
                ModuleSettingsProvider.GetSettingValue<string>("MegaPlatApiKey", _moduleName))
            {
                context.Response.Write("Неверный apikey");
                return;
            }


            string filename = context.Server.MapPath("~/modules/megaplat/categories.csv");
            if (File.Exists(filename))
                File.Delete(filename);

            using (TextWriter writer = new StreamWriter(filename, false, Encoding.UTF8))
            {
                foreach (Category category in CategoryService.GetChildCategoriesByCategoryId(0, false))
                {
                    RenderCategory(category.CategoryId, writer);
                }
            }

            var file = new FileInfo(filename);

            context.Response.Clear();
            context.Response.AddHeader("Content-Disposition", "attachment; filename=" + "categories.csv");
            context.Response.AddHeader("Content-Length", file.Length.ToString(CultureInfo.InvariantCulture));
            context.Response.ContentType = "application/octet-stream";
            context.Response.WriteFile(file.FullName);
            context.Response.End();

            if (File.Exists(filename))
                File.Delete(filename);
        }

        public bool IsReusable
        {
            get { return true; }
        }

        private void RenderCategory(int categoryID, TextWriter writer)
        {
            foreach (Category category in CategoryService.GetChildCategoriesByCategoryId(categoryID, false))
            {
                writer.WriteLine("[" + GetParentCategoriesAsString(category.CategoryId) + "]");
                RenderCategory(category.CategoryId, writer);
            }
        }

        private string GetParentCategoriesAsString(int childCategoryId)
        {
            var res = new StringBuilder();
            IList<Category> categoies = CategoryService.GetParentCategories(childCategoryId);
            for (int i = categoies.Count - 1; i >= 0; i--)
            {
                if (i != categoies.Count - 1)
                {
                    res.Append(" >> ");
                }
                res.Append(categoies[i].Name);
            }
            return res.ToString();
        }
    }
}

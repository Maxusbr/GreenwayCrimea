using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Hosting;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Modules;
using CsvHelper;

namespace AdvantShop.Module.SupplierOfHappiness.Domain
{
    public class SupplierOfHappinessService
    {
        public static void UpdateCategoriesList()
        {

            var categoriesList = SupplierOfHappinessRepository.GetCategories();

            var filePath = HostingEnvironment.MapPath(SupplierOfHappiness.FilePathFull);
            File.Delete(filePath);
            new WebClient().DownloadFile(SupplierOfHappiness.UrlPathFull, filePath);

            var stringAddCategories = string.Empty;

            using (var csvReader = new CsvReader(new StreamReader(filePath, Encoding.UTF8, true)))
            {
                csvReader.Configuration.Delimiter = ";";
                csvReader.Configuration.HasHeaderRecord = true;
                while (csvReader.Read())
                {
                    if (
                        !categoriesList.Any(
                            item =>
                                string.Equals(item.Category, csvReader.CurrentRecord[22].Trim()) &&
                                string.Equals(item.SubCategory, csvReader.CurrentRecord[23].Trim())))
                    {
                        SupplierOfHappinessRepository.AddCategory(
                            new SupplierOfHappinessCategory
                            {
                                Category = csvReader.CurrentRecord[22].Trim(),
                                SubCategory = csvReader.CurrentRecord[23].Trim(),
                                AdvCategoryId = null
                            });
                        categoriesList.Add(
                            new SupplierOfHappinessCategory
                            {
                                Category = csvReader.CurrentRecord[22].Trim(),
                                SubCategory = csvReader.CurrentRecord[23].Trim(),
                                AdvCategoryId = null
                            });
                        stringAddCategories += csvReader.CurrentRecord[22].Trim() + "//" +
                                               csvReader.CurrentRecord[23].Trim() + "<br/>";
                    }
                }
            }

            if (!string.IsNullOrEmpty(stringAddCategories))
            {
                ModulesService.SendModuleMail(
                    Guid.Empty,
                    "Модуль 'Поставщик счастья' - новая категория",
                    "Появились новые категории требующие привязки <br/>" + stringAddCategories,
                    SettingsMail.EmailForFeedback,
                    true);
            }
        }

        public static void SetDefaultCategories()
        {
            foreach (var sohCategoryModel in SupplierOfHappinessRepository.GetCategories())
            {
                SetDefaultCategory(sohCategoryModel.Category, sohCategoryModel.SubCategory);
            }
        }

        public static int? SetDefaultCategory(string category, string subCategory)
        {
            var advCategoryId = CategoryService.SubParseAndCreateCategory(string.Format("[{0}>>{1}]", category, subCategory));
            if (advCategoryId == -1)
            {
                return null;
            }

            SupplierOfHappinessRepository.UpdateCategory(new SupplierOfHappinessCategory
            {
                AdvCategoryId = advCategoryId,
                Category = category,
                SubCategory = subCategory
            });

            return advCategoryId;
        }
    }
}
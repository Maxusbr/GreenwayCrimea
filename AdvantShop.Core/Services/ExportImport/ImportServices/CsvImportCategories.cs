using System.Linq;
using System.Text;
using AdvantShop.Catalog;
using AdvantShop.Core.Caching;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Diagnostics;
using AdvantShop.FullSearch;
using AdvantShop.Helpers;
using AdvantShop.Statistic;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Configuration;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.SQL;
using AdvantShop.FilePath;
using AdvantShop.SEO;

namespace AdvantShop.ExportImport
{
    public class CsvImportCategories
    {
        private readonly string _fullPath;
        private readonly bool _hasHeadrs;

        private Dictionary<string, int> _fieldMapping;
        private readonly string _separator;
        private readonly string _encodings;

        private readonly Dictionary<string, int> _categoryMapping = new Dictionary<string, int>(); // first - categoryId in csv, second - categoryId in db

        public CsvImportCategories(string filePath, bool hasHeadrs, string separator, string encodings, Dictionary<string, int> fieldMapping)
        {
            _fullPath = filePath;
            _hasHeadrs = hasHeadrs;
            _fieldMapping = fieldMapping;
            _encodings = encodings;
            _separator = separator;
        }

        private CsvReader InitReader(bool? hasHeaderRecord = null)
        {
            var reader = new CsvReader(new StreamReader(_fullPath, Encoding.GetEncoding(_encodings ?? EncodingsEnum.Utf8.StrName())));

            reader.Configuration.Delimiter = _separator ?? SeparatorsEnum.SemicolonSeparated.StrName();
            reader.Configuration.HasHeaderRecord = hasHeaderRecord ?? _hasHeadrs;

            return reader;
        }

        public List<string[]> ReadFirstRecord()
        {
            var list = new List<string[]>();
            using (var csv = InitReader())
            {
                int count = 0;
                while (csv.Read())
                {
                    if (count == 2)
                        break;

                    if (csv.CurrentRecord != null)
                        list.Add(csv.CurrentRecord);
                    count++;
                }
            }
            return list;
        }

        public Task Process()
        {
            return CommonStatistic.StartNew(() =>
           {
               CommonStatistic.IsRun = true;
               try
               {
                   _process();
               }
               catch (Exception ex)
               {
                   Debug.Log.Error(ex);
                   CommonStatistic.WriteLog(ex.Message);
               }
               finally
               {
                   CommonStatistic.IsRun = false;
               }
           });
        }

        private void _process()
        {
            if (_fieldMapping == null)
                MapFileds();

            if (_fieldMapping == null)
                throw new Exception("can mapping colums");


            CommonStatistic.TotalRow = GetRowCount();

            var postProcessing = _fieldMapping.ContainsKey(CategoryFields.ParentCategory.StrName());

            if (postProcessing)
                CommonStatistic.TotalRow *= 2;

            ProcessRow(false);
            if (postProcessing && CommonStatistic.IsRun)
                ProcessRow(true);

            CommonStatistic.IsRun = false;

            CategoryService.RecalculateProductsCountManual();
            CategoryService.SetCategoryHierarchicallyEnabled(0);
            LuceneSearch.CreateAllIndexInBackground();
            ProductService.PreCalcProductParamsMassInBackground();

            CacheManager.Clean();
            FileHelpers.DeleteFilesFromImageTempInBackground();
            FileHelpers.DeleteFile(_fullPath);
        }

        private void MapFileds()
        {
            _fieldMapping = new Dictionary<string, int>();
            using (var csv = InitReader(false))
            {
                csv.Read();
                for (var i = 0; i < csv.CurrentRecord.Length; i++)
                {
                    if (csv.CurrentRecord[i] == ProductFields.None.StrName()) continue;
                    if (!_fieldMapping.ContainsKey(csv.CurrentRecord[i]))
                        _fieldMapping.Add(csv.CurrentRecord[i], i);
                }
            }
        }

        private long GetRowCount()
        {
            long count = 0;
            using (var csv = InitReader())
            {
                while (csv.Read())
                    count++;
            }
            return count;
        }

        private void ProcessRow(bool postProcess)
        {
            if (!File.Exists(_fullPath)) return;
            using (var csv = InitReader())
            {
                while (csv.Read())
                {
                    if (!CommonStatistic.IsRun)
                    {
                        csv.Dispose();
                        FileHelpers.DeleteFile(_fullPath);
                        return;
                    }
                    try
                    {
                        var categoryInStrings = PrepareRow(csv);
                        if (categoryInStrings == null) continue;

                        if (!postProcess)
                            UpdateInsertCategory(categoryInStrings);
                        else
                            PostProcess(categoryInStrings);
                    }
                    catch (Exception ex)
                    {
                        Debug.Log.Error(ex);
                    }
                }
            }
        }

        private Dictionary<CategoryFields, object> PrepareRow(ICsvReader csv)
        {
            var categoryInStrings = new Dictionary<CategoryFields, object>();

            foreach (CategoryFields field in Enum.GetValues(typeof(CategoryFields)))
            {
                switch (field.Status())
                {
                    case CsvFieldStatus.String:
                        GetString(field, csv, categoryInStrings);
                        break;
                    case CsvFieldStatus.StringRequired:
                        GetStringRequired(field, csv, categoryInStrings);
                        break;
                    case CsvFieldStatus.NotEmptyString:
                        GetStringNotNull(field, csv, categoryInStrings);
                        break;
                    case CsvFieldStatus.Float:
                        if (!GetDecimal(field, csv, categoryInStrings))
                            return null;
                        break;
                    case CsvFieldStatus.NullableFloat:
                        if (!GetNullableDecimal(field, csv, categoryInStrings))
                            return null;
                        break;
                    case CsvFieldStatus.Int:
                        if (!GetInt(field, csv, categoryInStrings))
                            return null;
                        break;
                }
            }
            return categoryInStrings;
        }


        private void UpdateInsertCategory(Dictionary<CategoryFields, object> categoryInStrings)
        {
            try
            {
                bool addingNew;
                Category category = null;

                var categoryId = categoryInStrings.ContainsKey(CategoryFields.CategoryId)
                                    ? Convert.ToString(categoryInStrings[CategoryFields.CategoryId])
                                    : string.Empty;

                if (string.IsNullOrWhiteSpace(categoryId))
                {
                    addingNew = true;
                }
                else
                {
                    category = CategoryService.GetCategory(categoryId.TryParseInt());
                    addingNew = category == null;
                }

                if (addingNew)
                    category = new Category();

                if (categoryInStrings.ContainsKey(CategoryFields.Name))
                    category.Name = categoryInStrings[CategoryFields.Name].AsString();
                else
                    category.Name = category.Name ?? string.Empty;

                if (categoryInStrings.ContainsKey(CategoryFields.Slug))
                {
                    var url = categoryInStrings[CategoryFields.Slug].AsString().IsNotEmpty()
                                      ? categoryInStrings[CategoryFields.Slug].AsString()
                                      : (category.Name != "" ? category.Name.Reduce(50) : category.CategoryId.ToString());
                    category.UrlPath = UrlService.GetAvailableValidUrl(category.CategoryId, ParamType.Category, url);
                }
                else
                {
                    var url = category.Name != "" ? category.Name.Reduce(50) : category.CategoryId.ToString();

                    category.UrlPath = category.UrlPath ?? UrlService.GetAvailableValidUrl(category.CategoryId, ParamType.Category, url);
                }

                if (categoryInStrings.ContainsKey(CategoryFields.SortOrder))
                    category.SortOrder = Convert.ToString(categoryInStrings[CategoryFields.SortOrder]).TryParseInt();

                if (categoryInStrings.ContainsKey(CategoryFields.Enabled))
                    category.Enabled = categoryInStrings[CategoryFields.Enabled].AsString().Trim().Equals("+");
                else
                    category.Enabled = true;

                if (categoryInStrings.ContainsKey(CategoryFields.Hidden))
                    category.Hidden = categoryInStrings[CategoryFields.Hidden].AsString().Trim().Equals("+");

                if (categoryInStrings.ContainsKey(CategoryFields.BriefDescription))
                    category.BriefDescription = categoryInStrings[CategoryFields.BriefDescription].AsString();

                if (categoryInStrings.ContainsKey(CategoryFields.Description))
                    category.Description = categoryInStrings[CategoryFields.Description].AsString();

                if (categoryInStrings.ContainsKey(CategoryFields.DisplayStyle))
                {
                    ECategoryDisplayStyle style;
                    Enum.TryParse(categoryInStrings[CategoryFields.DisplayStyle].AsString(), true, out style);

                    category.DisplayStyle = style;
                }

                if (categoryInStrings.ContainsKey(CategoryFields.Sorting))
                {
                    ESortOrder sorting;
                    Enum.TryParse(categoryInStrings[CategoryFields.Sorting].AsString(), true, out sorting);

                    category.Sorting = sorting;
                }

                if (categoryInStrings.ContainsKey(CategoryFields.DisplayBrandsInMenu))
                    category.DisplayBrandsInMenu = categoryInStrings[CategoryFields.DisplayBrandsInMenu].AsString().Trim().Equals("+");

                if (categoryInStrings.ContainsKey(CategoryFields.DisplaySubCategoriesInMenu))
                    category.DisplaySubCategoriesInMenu = categoryInStrings[CategoryFields.DisplaySubCategoriesInMenu].AsString().Trim().Equals("+");


                if (!addingNew)
                {
                    CategoryService.UpdateCategory(category, false);
                    CommonStatistic.TotalUpdateRow++;
                }
                else
                {
                    category.CategoryId = CategoryService.AddCategory(category, false);
                    CommonStatistic.TotalAddRow++;
                }

                if (category.CategoryId > 0)
                {
                    _categoryMapping.Add(categoryId, category.CategoryId);

                    OtherFields(categoryInStrings, category.CategoryId);
                }
            }
            catch (Exception e)
            {
                CommonStatistic.TotalErrorRow++;
                Log(CommonStatistic.RowPosition + ": " + e.Message);
            }

            categoryInStrings.Clear();
            CommonStatistic.RowPosition++;
        }


        private void OtherFields(IDictionary<CategoryFields, object> fields, int categoryId)
        {
            if (fields.ContainsKey(CategoryFields.Title) || fields.ContainsKey(CategoryFields.H1) 
                || fields.ContainsKey(CategoryFields.MetaKeywords) || fields.ContainsKey(CategoryFields.MetaDescription)) {

                var meta = new MetaInfo { ObjId = categoryId, Type = MetaType.Category };

                if (fields.ContainsKey(CategoryFields.Title))
                    meta.Title = fields[CategoryFields.Title].AsString();
                else
                    meta.Title = meta.Title ?? SettingsSEO.CategoryMetaTitle;

                if (fields.ContainsKey(CategoryFields.H1))
                    meta.H1 = fields[CategoryFields.H1].AsString();
                else
                    meta.H1 = meta.H1 ?? SettingsSEO.CategoryMetaH1;

                if (fields.ContainsKey(CategoryFields.MetaKeywords))
                    meta.MetaKeywords = fields[CategoryFields.MetaKeywords].AsString();
                else
                    meta.MetaKeywords = meta.MetaKeywords ?? SettingsSEO.CategoryMetaKeywords;

                if (fields.ContainsKey(CategoryFields.MetaDescription))
                    meta.MetaDescription = fields[CategoryFields.MetaDescription].AsString();
                else
                    meta.MetaDescription = meta.MetaDescription ?? SettingsSEO.CategoryMetaDescription;

                MetaInfoService.SetMeta(meta);
            }
            if (fields.ContainsKey(CategoryFields.Tags))
            {
                TagService.DeleteMap(categoryId, ETagType.Category);

                var i = 0;

                foreach (var tagName in fields[CategoryFields.Tags].AsString().Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    var tag = TagService.Get(tagName);
                    if (tag == null)
                    {
                        var tagId = TagService.Add(new Tag
                        {
                            Name = tagName,
                            Enabled = true,
                            UrlPath = UrlService.GetAvailableValidUrl(0, ParamType.Tag, tagName)
                        });
                        TagService.AddMap(categoryId, tagId, ETagType.Category, i*10);
                    }
                    else
                    {
                        TagService.AddMap(categoryId, tag.Id, ETagType.Category, i*10);
                    }

                    i++;
                }
            }

            if (fields.ContainsKey(CategoryFields.Picture))
            {
                var photo = fields[CategoryFields.Picture].AsString();
                if (!string.IsNullOrEmpty(photo))
                {
                    PhotoFromString(photo, categoryId, PhotoType.CategoryBig, CategoryImageType.Big,
                        SettingsPictureSize.BigCategoryImageWidth,
                        SettingsPictureSize.BigCategoryImageHeight);
                }
            }

            if (fields.ContainsKey(CategoryFields.MiniPicture))
            {
                var photo = fields[CategoryFields.MiniPicture].AsString();
                if (!string.IsNullOrEmpty(photo))
                {
                    PhotoFromString(photo, categoryId, PhotoType.CategorySmall, CategoryImageType.Small,
                        SettingsPictureSize.SmallCategoryImageWidth,
                        SettingsPictureSize.SmallCategoryImageHeight);
                }
            }

            if (fields.ContainsKey(CategoryFields.Icon))
            {
                var photo = fields[CategoryFields.Icon].AsString();
                if (!string.IsNullOrEmpty(photo))
                {
                    PhotoFromString(photo, categoryId, PhotoType.CategoryIcon, CategoryImageType.Icon,
                        SettingsPictureSize.IconCategoryImageWidth,
                        SettingsPictureSize.IconCategoryImageHeight);
                }
            }

            if (fields.ContainsKey(CategoryFields.PropertyGroups))
            {
                PropertyGroupService.DeleteGroupCategoriesByCategoryId(categoryId);

                foreach (var groupName in fields[CategoryFields.PropertyGroups].AsString().Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    var group = PropertyGroupService.Get(groupName);
                    if (group == null)
                    {
                        var groupId = PropertyGroupService.Add(new PropertyGroup() { Name = groupName });

                        PropertyGroupService.AddGroupToCategory(groupId, categoryId);
                    }
                    else
                    {
                        PropertyGroupService.AddGroupToCategory(group.PropertyGroupId, categoryId);
                    }
                }
            }
        }

        public void PostProcess(IDictionary<CategoryFields, object> fields)
        {
            var rowPosition = CommonStatistic.RowPosition;
            CommonStatistic.RowPosition++;

            if (!fields.ContainsKey(CategoryFields.CategoryId))
                return;

            var csvCategoryId = Convert.ToString(fields[CategoryFields.CategoryId]);
            var categoryId = _categoryMapping.ContainsKey(csvCategoryId) ? _categoryMapping[csvCategoryId] : -1;

            if (!CategoryService.IsExistCategory(categoryId))
            {
                Log(rowPosition + ": " + "Category Id '" + csvCategoryId + "' not found");
                return;
            }


            if (fields.ContainsKey(CategoryFields.ParentCategory))
            {
                var csvParentCategoryId = Convert.ToString(fields[CategoryFields.ParentCategory]);

                var parentCategoryId = -1;

                if (_categoryMapping.ContainsKey(csvParentCategoryId))
                {
                    parentCategoryId = _categoryMapping[csvParentCategoryId];
                }
                else if (CategoryService.IsExistCategory(csvParentCategoryId.TryParseInt()))
                {
                    parentCategoryId = csvParentCategoryId.TryParseInt();
                }

                if (CategoryService.IsExistCategory(parentCategoryId))
                {
                    SQLDataAccess.ExecuteNonQuery(
                        "Update Catalog.Category Set ParentCategory=@ParentCategory where CategoryID = @CategoryID",
                        CommandType.Text,
                        new SqlParameter("@ParentCategory", parentCategoryId),
                        new SqlParameter("@CategoryID", categoryId));
                }
                else
                {
                    Log(rowPosition + ": " + "Parent Category Id '" + csvParentCategoryId + "' not found");
                }
            }
        }

        private void PhotoFromString(string photo, int categoryId, PhotoType photoType, CategoryImageType imageType, int width, int height)
        {
            // if remote picture we must download it
            if (photo.Contains("http://") || photo.Contains("https://"))
            {
                var uri = new Uri(photo);
                var photoname = uri.PathAndQuery.Trim('/').Replace("/", "-");
                photoname = Path.GetInvalidFileNameChars().Aggregate(photoname, (current, c) => current.Replace(c.ToString(), ""));

                FileHelpers.CreateDirectory(FoldersHelper.GetPathAbsolut(FolderType.ImageTemp));

                if (string.IsNullOrWhiteSpace(photoname) ||
                    IsCategoryHasThisPhotoByName(categoryId, photoname, photoType) ||
                    !FileHelpers.DownloadRemoteImageFile(photo, FoldersHelper.GetPathAbsolut(FolderType.ImageTemp, photoname)))
                {
                    //if error in download proccess
                    return;
                }

                photo = photoname;
            }

            photo = string.IsNullOrEmpty(photo) ? photo : photo.Trim();

            // temp picture folder
            var fullfilename = FoldersHelper.GetPathAbsolut(FolderType.ImageTemp, photo);

            if (!File.Exists(fullfilename))
                return;

            PhotoService.DeletePhotos(categoryId, photoType);

            var tempName = PhotoService.AddPhoto(new Photo(0, categoryId, photoType) { OriginName = photo });
            if (!string.IsNullOrWhiteSpace(tempName))
            {
                using (Image image = Image.FromFile(fullfilename))
                {
                    FileHelpers.SaveResizePhotoFile(FoldersHelper.GetImageCategoryPathAbsolut(imageType, tempName),
                        width, height, image);
                }
            }
        }

        public static bool IsCategoryHasThisPhotoByName(int categoryId, string originName, PhotoType photoType)
        {
            var name = SQLDataAccess.ExecuteScalar<string>(
                    "select top 1 PhotoName from Catalog.Photo where ObjID=@categoryId and OriginName=@originName and type=@type",
                    CommandType.Text,
                    new SqlParameter("@categoryId", categoryId),
                    new SqlParameter("@originName", originName),
                    new SqlParameter("@type", photoType.ToString()));

            return name.IsNotEmpty();
        }


        #region Help methods

        private bool GetString(CategoryFields rEnum, ICsvReaderRow csv, IDictionary<CategoryFields, object> categoryInStrings)
        {
            var nameField = rEnum.StrName();
            if (_fieldMapping.ContainsKey(nameField))
                categoryInStrings.Add(rEnum, TrimAnyWay(csv[_fieldMapping[nameField]]));
            return true;
        }

        private bool GetStringNotNull(CategoryFields rEnum, ICsvReaderRow csv, IDictionary<CategoryFields, object> categoryInStrings)
        {
            var nameField = rEnum.StrName();
            if (!_fieldMapping.ContainsKey(nameField)) return true;
            var tempValue = TrimAnyWay(csv[_fieldMapping[nameField]]);
            if (!string.IsNullOrEmpty(tempValue))
                categoryInStrings.Add(rEnum, tempValue);
            return true;
        }

        private bool GetStringRequired(CategoryFields rEnum, ICsvReaderRow csv, IDictionary<CategoryFields, object> categoryInStrings)
        {
            var nameField = rEnum.StrName();
            if (!_fieldMapping.ContainsKey(nameField)) return true;
            var tempValue = TrimAnyWay(csv[_fieldMapping[nameField]]);
            if (!string.IsNullOrEmpty(tempValue))
                categoryInStrings.Add(rEnum, tempValue);
            else
            {
                LogInvalidData(string.Format(LocalizationService.GetResource("Core.ExportImport.ImportCsv.CanNotEmpty"), rEnum.Localize(), CommonStatistic.RowPosition + 2));
                return false;
            }
            return true;
        }

        private bool GetDecimal(CategoryFields rEnum, ICsvReaderRow csv, IDictionary<CategoryFields, object> categoryInStrings)
        {
            var nameField = rEnum.StrName();
            if (!_fieldMapping.ContainsKey(nameField)) return true;
            var value = TrimAnyWay(csv[_fieldMapping[nameField]]);
            if (string.IsNullOrEmpty(value))
                value = "0";
            float decValue;
            if (float.TryParse(value, out decValue))
            {
                categoryInStrings.Add(rEnum, decValue);
            }
            else if (float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out decValue))
            {
                categoryInStrings.Add(rEnum, decValue);
            }
            else
            {
                LogInvalidData(string.Format(LocalizationService.GetResource("Core.ExportImport.ImportCsv.MustBeNumber"), rEnum.Localize(), CommonStatistic.RowPosition + 2));
                return false;
            }
            return true;
        }

        private bool GetNullableDecimal(CategoryFields rEnum, ICsvReaderRow csv, IDictionary<CategoryFields, object> categoryInStrings)
        {
            var nameField = rEnum.StrName();
            if (!_fieldMapping.ContainsKey(nameField)) return true;
            var value = TrimAnyWay(csv[_fieldMapping[nameField]]);
            if (string.IsNullOrEmpty(value))
            {
                categoryInStrings.Add(rEnum, default(float?));
                return true;
            }

            float decValue;
            if (float.TryParse(value, out decValue))
            {
                categoryInStrings.Add(rEnum, decValue);
            }
            else if (float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out decValue))
            {
                categoryInStrings.Add(rEnum, decValue);
            }
            else
            {
                LogInvalidData(string.Format(LocalizationService.GetResource("Core.ExportImport.ImportCsv.MustBeNumber"), rEnum.Localize(), CommonStatistic.RowPosition + 2));
                return false;
            }
            return true;
        }

        private bool GetInt(CategoryFields rEnum, ICsvReaderRow csv, IDictionary<CategoryFields, object> categoryInStrings)
        {
            var nameField = rEnum.StrName();
            if (!_fieldMapping.ContainsKey(nameField)) return true;
            var value = TrimAnyWay(csv[_fieldMapping[nameField]]);
            if (string.IsNullOrEmpty(value))
                value = "0";
            int intValue;
            if (int.TryParse(value, out intValue))
            {
                categoryInStrings.Add(rEnum, intValue);
            }
            else
            {
                LogInvalidData(string.Format(LocalizationService.GetResource("Core.ExportImport.ImportCsv.MustBeNumber"), rEnum.Localize(), CommonStatistic.RowPosition + 2));
                return false;
            }
            return true;
        }

        private static string TrimAnyWay(string str)
        {
            return string.IsNullOrEmpty(str) ? str : str.Trim();
        }

        private static void LogInvalidData(string message)
        {
            CommonStatistic.WriteLog(message);
            CommonStatistic.TotalErrorRow++;
            CommonStatistic.RowPosition++;
        }

        private static void Log(string message)
        {
            CommonStatistic.WriteLog(message);
        }

        #endregion
    }
}
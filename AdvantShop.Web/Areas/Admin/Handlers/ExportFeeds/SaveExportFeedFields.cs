using System;
using System.Linq;
using System.Collections.Generic;

using AdvantShop.ExportImport;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using Newtonsoft.Json;

namespace AdvantShop.Web.Admin.Handlers.ExportFeeds
{
    public class SaveExportFeedFields
    {
        private readonly int _exportFeedId;
        private readonly List<string> _fields;

        public SaveExportFeedFields(int exportFeedId, List<string> fields)
        {
            _exportFeedId = exportFeedId;
            _fields = fields;
        }

        public bool Execute()
        {
            if (!_fields.Any(item => item != ProductFields.None.ToString()))
            {
                return false;
            }

            var exportFeed = ExportFeedService.GetExportFeed(_exportFeedId);
            if (exportFeed == null || (exportFeed.Type != EExportFeedType.Csv && exportFeed.Type != EExportFeedType.Reseller))
            {
                return false;
            }
            var settings = ExportFeedSettingsProvider.GetSettings(_exportFeedId);
            if (settings == null)
            {
                return false;
            }

            var fieldMapping = new List<ProductFields>();
            var moduleFieldMapping = new List<CSVField>();
            ProcessMapping(fieldMapping, moduleFieldMapping);

            // костыли, переделать так чтобы о типах никто не знал
            // как вариант переписать чтобы в объекте AdvancedSettings уже приходили замапленные поля. и сохранять все разом
            var exportFeedAdvancedSettings = string.Empty;
            try
            {
                if (exportFeed.Type == EExportFeedType.Csv)
                {
                    var exportFeedCsvOptions = JsonConvert.DeserializeObject<ExportFeedCsvOptions>(settings.AdvancedSettings);
                    exportFeedCsvOptions.FieldMapping = fieldMapping;
                    exportFeedCsvOptions.ModuleFieldMapping = moduleFieldMapping;
                    exportFeedAdvancedSettings = JsonConvert.SerializeObject(exportFeedCsvOptions);
                }
                else if(exportFeed.Type == EExportFeedType.Reseller)
                {
                    var exportFeedResellerOptions = JsonConvert.DeserializeObject<ExportFeedResellerOptions>(settings.AdvancedSettings);
                    exportFeedResellerOptions.FieldMapping = fieldMapping;
                    exportFeedResellerOptions.ModuleFieldMapping = moduleFieldMapping;
                    exportFeedAdvancedSettings = JsonConvert.SerializeObject(exportFeedResellerOptions);
                }
            }
            catch
            {
                return false;
            }
            
            ExportFeedSettingsProvider.SetAdvancedSettings(_exportFeedId, exportFeedAdvancedSettings);

            return true;
        }

        private List<CSVField> GetModuleFields()
        {
            var result = new List<CSVField>();
            foreach (var csvExportImportModule in AttachedModules.GetModules<ICSVExportImport>())
            {
                var classInstance = (ICSVExportImport)Activator.CreateInstance(csvExportImportModule, null);
                if (ModulesRepository.IsActiveModule(classInstance.ModuleStringId) && classInstance.CheckAlive())
                {
                    result.AddRange(classInstance.GetCSVFields());
                }
            }
            return result;
        }

        private void ProcessMapping(List<ProductFields> fieldMapping, List<CSVField> moduleFieldMapping)
        {
            var listModuleFields = GetModuleFields();

            foreach (var field in _fields)
            {
                ProductFields currentField;
                if (Enum.TryParse(field, out currentField))
                {
                    if (!fieldMapping.Contains(currentField) && currentField != ProductFields.None)
                    {
                        fieldMapping.Add(currentField);
                    }
                }
                else if (listModuleFields.Select(f => f.StrName).Contains(field))
                {
                    if (!moduleFieldMapping.Select(f => f.StrName).Contains(field))
                    {
                        moduleFieldMapping.Add(listModuleFields.First(f => f.StrName == field));
                    }

                }
            }
        }
    }
}

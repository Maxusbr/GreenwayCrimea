using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Diagnostics;
using AdvantShop.Web.Admin.Models.Settings;
using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Web.Admin.Controllers.Settings
{
    public partial class LogErrorsController : BaseAdminController
    {
        #region Add/Edit/Get/Delete

        public JsonResult GetLogErrors(ErrlogFilterModel model)
        {
            var result = GetErrors(model);

            return result != null ? JsonOk(result) : JsonError();
        }

        public JsonResult GetItemLogError(ErrType type, DateTime time)
        {
            var result = GetError(type, time);

            return result != null ? JsonOk(result) : JsonError();
        }

        #endregion

        #region Help method

        private FilterResult<LogEntry> GetErrors(ErrlogFilterModel filter)
        {
            if (!System.IO.File.Exists(Debug.GetErrFileName(filter.Type)))
                return null;

            var type = filter.Type.ToString();
            var pages = Directory.GetFiles(Debug.ErrFilesPath, type + "*").Where(file=>  new FileInfo(file).Length > 3).Count();

            var model = new FilterResult<LogEntry>
            {
                DataItems = new List<LogEntry>(),
                TotalPageCount = pages,
                TotalItemsCount = pages * filter.ItemsPerPage,
                PageIndex = filter.Page != 0 ? filter.Page : 1
            };
            
            try
            {
                var filePath = Debug.GetErrFileName(filter.Type) +
                           (filter.Page != 1 ? "." + (filter.Page - 1) : "");

                using (var csv = new CsvHelper.CsvReader(new StreamReader(filePath, Encoding.UTF8, true)))
                {
                    csv.Configuration.Delimiter = Debug.CharSeparate;
                    csv.Configuration.HasHeaderRecord = false;
                    while (csv.Read())
                    {
                        if (csv.CurrentRecord.Count(x => !string.IsNullOrEmpty(x)) < 4)
                            continue;

                        var item = new LogEntry
                        {
                            DateTime = csv.CurrentRecord[0].TryParseDateTime(),
                            Level = csv.CurrentRecord[1] ?? "",
                            Message = csv.CurrentRecord[2] ?? "",
                            ErrorMessage = csv.CurrentRecord[3] ?? "",
                            Type = type
                        };
                        model.DataItems.Add(item);
                    }
                }
                model.DataItems.Reverse();

                return model;
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
                return null;
            }
        }

        private AdvException GetError(ErrType errType, DateTime time)
        {
            if (!System.IO.File.Exists(Debug.GetErrFileName(errType)))
                return null;

            try
            {
                using (var csv = new CsvHelper.CsvReader(new StreamReader(Debug.GetErrFileName(errType), Encoding.UTF8, true)))
                {
                    csv.Configuration.Delimiter = Debug.CharSeparate;
                    csv.Configuration.HasHeaderRecord = false;
                    while (csv.Read())
                    {
                        if (csv.CurrentRecord.Count(x => !string.IsNullOrEmpty(x)) < 4)
                            continue;

                        if (csv.CurrentRecord[0].TryParseDateTime() != time)
                            continue;

                        var error = new AdvException();
                        try
                        {
                            if (csv.CurrentRecord[4] != "none")
                                error = AdvException.GetFromJsonString(csv.CurrentRecord[4]);
                        }
                        catch (Exception ex)
                        {
                            Debug.Log.Error(ex);
                        }

                        error.ExceptionData.ManualMessage = csv.CurrentRecord[2];

                        return error;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }
            return null;
        }

        #endregion
    }
}

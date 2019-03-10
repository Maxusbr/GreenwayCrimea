//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Web;
//using Microsoft.SqlServer.Management.Common;
//using Microsoft.SqlServer.Management.Smo;
using Newtonsoft.Json;
//using Rule = Microsoft.SqlServer.Management.Smo.Rule;
using ICSharpCode.SharpZipLib.Zip;
using System.Globalization;

//using AdvantShop.Helpers;
//using AdvantShop.Configuration;

namespace Advantshop_Tools
{
    public class VersionInformations
    {
        public string lastVersion = string.Empty;
        public string versionHistory = string.Empty;
    }

    public class UpdaterService
    {
        //private static readonly string ShopCodeMaskFile = HttpContext.Current.Server.MapPath("~/App_Data/shopCodeMaskFile.txt");
        //private static readonly string ShopBaseMaskFile = HttpContext.Current.Server.MapPath("~/App_Data/shopBaseMaskFile.txt");
        //private static readonly string UpdateSourceZipFile =
        //    HttpContext.Current.Server.MapPath("~/App_Data/lastVersionCode.zip");

        //private static readonly string RootDirectory = HttpContext.Current.Server.MapPath("~/");
        //private const string ServiceUrl = "http://update.advantshop.net/updater/";

        //private static readonly List<string> ExclusionFoldersAndFiles =
        //    new List<string>{
        //        ".svn\\",
        //        "exports\\",
        //        "Export\\",
        //        "pictures\\",
        //        "pictures_elbuz\\",
        //        "pictures_default\\",
        //        "pictures_extra\\",
        //        "price_download\\",
        //        "price_temp\\",
        //        "upload_images\\",
        //        //"images\\",
        //        "userfiles\\",
        //        "App_Data\\Lucene\\",
        //        "App_Data\\errlog\\",
        //        "App_Data\\notepad\\",
        //        //"App_WebReferences",
        //        "ckeditor\\",
        //        "_rev\\",
        //        "_SQL\\",
        //        "design\\",
        //        "info\\",

        //        //--Модули, todo: переделать: отделить исходники модулей от сервисов модулей
        //        "Modules\\Snowfall",
        //        "Modules\\Watermark",
        //        "Modules\\AdvQrCode",
        //        "Modules\\MoySklad",
        //        "Modules\\OnePageCheckout",
        //        "Modules\\SmsNotifications",
        //        "Modules\\StoreReviews",
        //        //--Модули
                
        //        "App_Data\\shopBaseMaskFile.txt",
        //        "App_Data\\shopCodeMaskFile.txt",
        //        "App_Data\\bak.sql",
        //        "App_Data\\dak_code.zip",

        //        "App_Data\\LogTempData.txt",
        //        "Web.ConnectionString.config",
        //        "Web.ModeSettings.config",
        //        "Yamarket.xml",
        //        "robots.txt",
        //        "sitemap.html",
        //        "sitemap.xml",
        //        "combined_"};

        //#region Code mask-file

        //public static bool CreateCodeMaskFile()
        //{
        //    try
        //    {
        //        using (var outputFile = new StreamWriter(ShopCodeMaskFile))
        //        {
        //            foreach (var advFileName in Directory.GetFiles(RootDirectory, "*.*", SearchOption.AllDirectories))
        //            {
        //                var fileName = advFileName.Replace(HttpContext.Current.Request.PhysicalApplicationPath, "");

        //                if (ExclusionFoldersAndFiles.Any(fileName.Contains)) continue;

        //                using (HashAlgorithm hashAlg = new SHA1Managed())
        //                {
        //                    using (Stream file = new FileStream(advFileName, FileMode.Open, FileAccess.Read))
        //                    {
        //                        byte[] hash = hashAlg.ComputeHash(file);
        //                        outputFile.WriteLine(fileName + ";" + BitConverter.ToString(hash));
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //    return true;
        //}

        //private static StreamReader GetEtalonMaskFileFromServer()
        //{
        //    var request = WebRequest.Create(
        //               string.Format("{0}HttpHandlers/GetCodeMaskFileByVersion.ashx?version={1}&license={2}&lang={3}",
        //               ServiceUrl,
        //               (new AppSettingsReader()).GetValue("Version", typeof(String)),
        //               Tools_Updater_ExecuteScalar<string>("[Settings].[sp_GetSettingValue]", CommandType.StoredProcedure, new SqlParameter { ParameterName = "@SettingName", Value = "LicKey" }),
        //               CultureInfo.CurrentCulture.TwoLetterISOLanguageName));
        //    request.Method = "GET";

        //    var response = request.GetResponse();

        //    return new StreamReader(response.GetResponseStream());
        //}

        //private static string CompareCodeMaskFiles()
        //{
        //    var result = string.Empty;

        //    var storeMaskFileDictionary = new Dictionary<string, string>();

        //    try
        //    {
        //        //1. Create dictionary store mask file like < AccessDenied.aspx, 03-D4-1F-1A-84-1B-21-EE-15-5F-FB-24-FD-B6-C0-56-AB-7C-7F-40 >
        //        using (var streamReaderThisMaskFile = new StreamReader(ShopCodeMaskFile))
        //        {
        //            while (streamReaderThisMaskFile.Peek() >= 0)
        //            {
        //                var stringStoreMaskFile = streamReaderThisMaskFile.ReadLine();
        //                if (string.IsNullOrEmpty(stringStoreMaskFile))
        //                {
        //                    continue;
        //                }
        //                var pair = stringStoreMaskFile.Split(';');
        //                storeMaskFileDictionary.Add(pair[0], pair[1]);
        //            }
        //        }

        //        //2. equal mask files
        //        using (var streamReaderEtalonMaskFile = GetEtalonMaskFileFromServer())// todo: get file from remoute server
        //        {
        //            long count = 0;
        //            while (streamReaderEtalonMaskFile.Peek() >= 0)
        //            {
        //                var stringEtalonMaskFile = streamReaderEtalonMaskFile.ReadLine();
        //                ++count;
        //                if (string.IsNullOrEmpty(stringEtalonMaskFile))
        //                {
        //                    continue;
        //                }
        //                var etalonPair = stringEtalonMaskFile.Split(';');

        //                // if file not found in this store
        //                if (!storeMaskFileDictionary.ContainsKey(etalonPair[0]))
        //                {
        //                    result += string.Format("<span style=\"color:red;display:inline-block;width: 210px;\">file from string {0} not found </span>: {1}<br/>", count, etalonPair[0]);
        //                    continue;
        //                }

        //                if (!string.Equals(etalonPair[1], storeMaskFileDictionary[etalonPair[0]]))
        //                {
        //                    result += string.Format("<span style=\"color:#e7a9b0;display:inline-block;width: 210px;\">discrepancy in string {0} </span>: {1}<br/>", count, etalonPair[0]);
        //                }
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        result = "Error matching: " + ex;
        //    }

        //    return result;
        //}

        //public static string CompareCodeVersions(bool updateMask)
        //{
        //    if (updateMask || !File.Exists(ShopCodeMaskFile))
        //    {
        //        if (!CreateCodeMaskFile())
        //        {
        //            return "error CreateCodeMaskFile";
        //        }
        //    }

        //    return CompareCodeMaskFiles();

        //}
        //#endregion

        //#region Base mask file

        //public static void CreateBaseMaskFile()
        //{
        //    var remoteServerName = string.Empty;
        //    var instanceName = string.Empty;
        //    var login = string.Empty;
        //    var password = string.Empty;
        //    var keyValueConnection = ConfigurationManager.ConnectionStrings["AdvantConnectionString"].ConnectionString
        //                                      .Replace("'", "")
        //                                      .Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
        //    foreach (var keyValue in keyValueConnection)
        //    {
        //        var pair = keyValue.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
        //        if (pair.Length > 1)
        //        {
        //            switch (pair[0].ToLower().Replace(" ", ""))
        //            {
        //                case "datasource":
        //                    remoteServerName = pair[1];
        //                    break;
        //                case "initialcatalog":
        //                    instanceName = pair[1];
        //                    break;
        //                case "userid":
        //                    login = pair[1];
        //                    break;
        //                case "password":
        //                    password = pair[1];
        //                    break;
        //            }
        //        }
        //    }

        //    CreateBaseBackup(remoteServerName, instanceName, login, password, false, ShopBaseMaskFile);
        //}

        //private static string CompareBaseMaskFiles()
        //{
        //    var result = string.Empty;

        //    try
        //    {
        //        using (var streamReaderThisMaskFile = new StreamReader(ShopBaseMaskFile))
        //        {
        //            var request = WebRequest.Create(
        //                string.Format("{0}HttpHandlers/GetBaseMaskFileByVersion.ashx?version={1}&license={2}&lang={3}",
        //                ServiceUrl,
        //                (new AppSettingsReader()).GetValue("Version", typeof(String)),
        //                Tools_Updater_ExecuteScalar<string>("[Settings].[sp_GetSettingValue]", CommandType.StoredProcedure, new SqlParameter { ParameterName = "@SettingName", Value = "LicKey" }),
        //                CultureInfo.CurrentCulture.TwoLetterISOLanguageName));

        //            request.Method = "GET";

        //            var response = request.GetResponse();

        //            using (var streamReaderEtalonMaskFile = new StreamReader(response.GetResponseStream()))// todo: get file from remoute server
        //            {
        //                long count = 0;
        //                while (streamReaderThisMaskFile.Peek() >= 0 && streamReaderEtalonMaskFile.Peek() >= 0)
        //                {
        //                    var stringThisMaskFile = streamReaderThisMaskFile.ReadLine();
        //                    var stringEtalonMaskFile = streamReaderEtalonMaskFile.ReadLine();
        //                    ++count;

        //                    if (stringEtalonMaskFile != stringThisMaskFile)
        //                    {
        //                        result += string.Format("<span style=\"color:#e7a9b0;display:inline-block;width: 210px;\">discrepancy in string {0}</span>: <br/>Original file string: {1}<br/>This-Store string:{2}", count, stringEtalonMaskFile, stringThisMaskFile);
        //                        break;
        //                    }
        //                }
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        result = "Error matching: " + ex;
        //    }

        //    return result;
        //}

        //public static string CompareBaseVersions(bool updateMask)
        //{
        //    if (updateMask || !File.Exists(ShopBaseMaskFile))
        //    {
        //        CreateBaseMaskFile();
        //    }

        //    return CompareBaseMaskFiles();
        //}

        //#endregion

        //public static VersionInformations GetLastVersionInformation()
        //{
        //    var result = new VersionInformations();
        //    try
        //    {
        //        var request = WebRequest.Create(string.Format("{0}/HttpHandlers/GetVersionsHistory.ashx?license={1}&version={2}&lang={3}",
        //            ServiceUrl,
        //            Tools_Updater_ExecuteScalar<string>("[Settings].[sp_GetSettingValue]", CommandType.StoredProcedure, new SqlParameter { ParameterName = "@SettingName", Value = "LicKey" }),
        //            ConfigurationManager.AppSettings["Version"],
        //            CultureInfo.CurrentCulture.TwoLetterISOLanguageName));

        //        request.Method = "GET";

        //        var response = request.GetResponse();

        //        result = JsonConvert.DeserializeObject<VersionInformations>((new StreamReader(response.GetResponseStream())).ReadToEnd());
        //    }
        //    catch (Exception)
        //    {

        //    }
        //    return result;
        //}

        //public static string GetLastSqlVersion()
        //{
        //    var result = string.Empty;
        //    try
        //    {
        //        var request = WebRequest.Create(string.Format("{0}/HttpHandlers/GetLastSqlVersion.ashx?license={1}&version={2}&lang={3}",
        //            ServiceUrl,
        //            Tools_Updater_ExecuteScalar<string>("[Settings].[sp_GetSettingValue]", CommandType.StoredProcedure, new SqlParameter { ParameterName = "@SettingName", Value = "LicKey" }),
        //            ConfigurationManager.AppSettings["Version"],
        //            CultureInfo.CurrentCulture.TwoLetterISOLanguageName));

        //        request.Method = "GET";

        //        var response = request.GetResponse();

        //        result = (new StreamReader(response.GetResponseStream())).ReadToEnd();
        //    }
        //    catch (Exception)
        //    {

        //    }
        //    return result;
        //}

        //public static void CreateBaseBackup()
        //{
        //    var remoteServerName = string.Empty;
        //    var instanceName = string.Empty;
        //    var login = string.Empty;
        //    var password = string.Empty;
        //    var keyValueConnection = ConfigurationManager.ConnectionStrings["AdvantConnectionString"].ConnectionString
        //                                      .Replace("'", "")
        //                                      .Split(new [] { ';' }, StringSplitOptions.RemoveEmptyEntries);
        //    foreach (var keyValue in keyValueConnection)
        //    {
        //        var pair = keyValue.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
        //        if (pair.Length > 1)
        //        {
        //            switch (pair[0].ToLower().Replace(" ", ""))
        //            {
        //                case "datasource":
        //                    remoteServerName = pair[1];
        //                    break;
        //                case "initialcatalog":
        //                    instanceName = pair[1];
        //                    break;
        //                case "userid":
        //                    login = pair[1];
        //                    break;
        //                case "password":
        //                    password = pair[1];
        //                    break;
        //            }
        //        }
        //    }
        //    CreateBaseBackup(remoteServerName, instanceName, login, password, true, HttpContext.Current.Server.MapPath("~\\App_Data\\bak.sql"));
        //}

        //public static void CreateBaseBackup(string remoteServerName, string instanceName, string login, string password, bool includeData, string outputFile)
        //{
        //    var srvConn2 = new ServerConnection
        //    {
        //        ServerInstance = remoteServerName,
        //        LoginSecure = false,
        //        Login = login,
        //        Password = password
        //    };

        //    var srv3 = new Server(srvConn2);
        //    var database = srv3.Databases[instanceName];

        //    using (var sw = new StreamWriter(outputFile, false, System.Text.Encoding.UTF8))
        //    {

        //        var dvVersion = Tools_Updater_ExecuteScalar<string>(
        //            "SELECT [settingValue] FROM [Settings].[InternalSettings] WHERE [settingKey] = 'db_version'",
        //            CommandType.Text);
        //        sw.Write("--db_version: " + dvVersion);

        //        var options = new ScriptingOptions
        //        {
        //            ScriptData = includeData,
        //            ScriptDrops = false,
        //            EnforceScriptingOptions = true,
        //            ScriptSchema = true,
        //            IncludeHeaders = false,
        //            Indexes = true,
        //            NonClusteredIndexes = true,
        //            WithDependencies = false,
        //            DriAllKeys = true
        //        };

        //        var regex = new Regex("\r\n|\t", RegexOptions.Compiled);

        //        foreach (Schema schema in database.Schemas)
        //        {
        //            if (!schema.IsSystemObject)
        //            {
        //                foreach (var st in schema.Script())
        //                {
        //                    sw.Write("\r\nGO--\r\n" + st.Trim(new[] { '\r', '\n', ' ' }));
        //                }
        //            }
        //        }

        //        foreach (Table table in database.Tables)
        //        {
        //            if (!table.IsSystemObject)
        //            {
        //                foreach (var st in table.EnumScript(options))
        //                {
        //                    var temp = regex.Replace(st, " ");
        //                    sw.Write("\r\nGO--\r\n" + temp.Trim(new[] { ' ' }));
        //                }
        //            }
        //        }

        //        foreach (UserDefinedFunction function in database.UserDefinedFunctions)
        //        {
        //            if (!function.IsSystemObject)
        //            {
        //                foreach (var st in function.Script())
        //                {
        //                    var temp = regex.Replace(st, " ");
        //                    sw.Write("\r\nGO--\r\n" + temp.Trim(new[] { ' ' }));
        //                }
        //            }
        //        }

        //        foreach (Trigger trigger in database.Triggers)
        //        {
        //            if (!trigger.IsSystemObject)
        //            {
        //                foreach (var st in trigger.Script(options))
        //                {
        //                    var temp = regex.Replace(st, " ");
        //                    sw.Write("\r\nGO--\r\n" + temp.Trim(new[] { ' ' }));
        //                }
        //            }
        //        }

        //        foreach (View view in database.Views)
        //        {
        //            if (!view.IsSystemObject)
        //            {
        //                foreach (var st in view.Script(options))
        //                {
        //                    var temp = regex.Replace(st, " ");
        //                    sw.Write("\r\nGO--\r\n" + temp.Trim(new[] { ' ' }));
        //                }
        //            }
        //        }

        //        foreach (Rule rule in database.Rules)
        //        {
        //            foreach (var st in rule.Script(options))
        //            {
        //                var temp = regex.Replace(st, " ");
        //                sw.Write("\r\nGO--\r\n" + temp.Trim(new[] { ' ' }));
        //            }
        //        }

        //        foreach (StoredProcedure storedProcedure in database.StoredProcedures)
        //        {
        //            if (!storedProcedure.IsSystemObject)
        //            {
        //                foreach (var st in storedProcedure.Script())
        //                {
        //                    var temp = regex.Replace(st, " ");
        //                    sw.Write("\r\nGO--\r\n" + temp.Trim(new[] { ' ' }));
        //                }
        //            }
        //        }
        //    }
        //}

        //public static void CreateCodeBackup()
        //{
        //    AdvantShop_Helpers_FileHelpers_ZipFiles(HttpContext.Current.Server.MapPath("~/"), "App_Data\\dak_code.zip", string.Empty, true);
        //}
        
        //public static bool UpdateAvantshop()
        //{
        //    //1. Download zip file
        //    new WebClient().DownloadFile(
        //        string.Format("{0}/HttpHandlers/GetLastVersion.ashx?license={1}&lang={2}",
        //            ServiceUrl,
        //            Tools_Updater_ExecuteScalar<string>("[Settings].[sp_GetSettingValue]", CommandType.StoredProcedure, new SqlParameter { ParameterName = "@SettingName", Value = "LicKey" }),
        //            CultureInfo.CurrentCulture.TwoLetterISOLanguageName),
        //        UpdateSourceZipFile);

        //    //2. Check available unzip
        //    if (!File.Exists(UpdateSourceZipFile) || !AdvantShop_Helpers_FileHelpers_CanUnZipFile(UpdateSourceZipFile))
        //        return false;

        //    //3. Delete current version, delete all files by filenames from etalonMaskFile
        //    using (var streamReaderEtalonMaskFile = GetEtalonMaskFileFromServer())
        //    {
        //        while (streamReaderEtalonMaskFile.Peek() >= 0)
        //        {
        //            var stringEtalonMaskFile = streamReaderEtalonMaskFile.ReadLine();
        //            if (string.IsNullOrEmpty(stringEtalonMaskFile))
        //            {
        //                continue;
        //            }
        //            var fileNameFromMask =
        //                (HttpContext.Current.Server.MapPath("~/" + stringEtalonMaskFile.Split(';')[0]));

        //            if (File.Exists(fileNameFromMask))
        //            {
        //                File.Delete(fileNameFromMask);
        //            }
        //        }
        //    }

        //    //4. Unzip files
        //    AdvantShop_Helpers_FileHelpers_UnZipFile(UpdateSourceZipFile, HttpContext.Current.Server.MapPath("~/"));

        //    //5. Execute sql-patch

        //    var sqlCommand = GetLastSqlVersion();
        //    if (!string.IsNullOrEmpty(sqlCommand))
        //    {
        //        foreach (var command in sqlCommand.Split(new[] { "GO--" }, StringSplitOptions.RemoveEmptyEntries))
        //        {
        //            Tools_Updater_ExecuteNonQuery(command, CommandType.Text);
        //        }
        //    }
        //    //6. Delete zip file (удаление скаченного zip файла исходников, чтобы место не занимал)
        //    File.Delete(UpdateSourceZipFile);

        //    return true;
        //}

        ////*********************************************************************************************************************************************
        //public static bool AdvantShop_Helpers_FileHelpers_CanUnZipFile(string inputPathOfZipFile)
        //{
        //    int result;
        //    if (File.Exists(inputPathOfZipFile))
        //    {
        //        using (var zipStream = new ZipInputStream(File.OpenRead(inputPathOfZipFile)))
        //        {
        //            zipStream.GetNextEntry();
        //            result = zipStream.Available;
        //        }
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //    return result == 1;
        //}

        //public static bool AdvantShop_Helpers_FileHelpers_UnZipFile(string inputPathOfZipFile, string outputPathOfZipFile)
        //{
        //    try
        //    {
        //        if (File.Exists(inputPathOfZipFile))
        //        {
        //            string baseDirectory = Path.GetDirectoryName(outputPathOfZipFile);

        //            using (var zipStream = new ZipInputStream(File.OpenRead(inputPathOfZipFile)))
        //            {
        //                //check Available unzip, also can chack with zipStream.CanDecompressEntry
        //                if (!AdvantShop_Helpers_FileHelpers_CanUnZipFile(inputPathOfZipFile))
        //                {
        //                    return false;
        //                }

        //                ZipEntry theEntry;
        //                while ((theEntry = zipStream.GetNextEntry()) != null)
        //                {
        //                    if (theEntry.IsFile)
        //                    {
        //                        if (!string.IsNullOrEmpty(theEntry.Name))
        //                        {
        //                            string strNewFile = @"" + baseDirectory + @"\" + theEntry.Name;

        //                            if (ExclusionFoldersAndFiles.Any(strNewFile.Contains)) continue;

        //                            if (File.Exists(strNewFile))
        //                            {
        //                                File.Delete(strNewFile);
        //                                //continue;
        //                            }

        //                            using (FileStream streamWriter = File.Create(strNewFile))
        //                            {
        //                                int size = 2048;
        //                                var data = new byte[size];
        //                                while (true)
        //                                {
        //                                    size = zipStream.Read(data, 0, data.Length);
        //                                    if (size > 0)
        //                                        streamWriter.Write(data, 0, size);
        //                                    else
        //                                        break;
        //                                }
        //                                streamWriter.Close();
        //                            }
        //                        }
        //                    }
        //                    else if (theEntry.IsDirectory)
        //                    {
        //                        string strNewDirectory = @"" + baseDirectory + @"\" + theEntry.Name;

        //                        if (ExclusionFoldersAndFiles.Any(strNewDirectory.Contains)) continue;

        //                        if (!Directory.Exists(strNewDirectory))
        //                        {
        //                            Directory.CreateDirectory(strNewDirectory);
        //                        }
        //                    }
        //                }
        //                zipStream.Close();
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //    return true;
        //}

        //public static bool AdvantShop_Helpers_FileHelpers_ZipFiles(string inputFolderPath, string outputPathAndFile, string password, bool recurse)
        //{
        //    try
        //    {
        //        var itemsList = AdvantShop_Helpers_FileHelpers_GenerateFileList(inputFolderPath, recurse); // generate file list
        //        int trimLength = (Directory.GetParent(inputFolderPath)).ToString().Length;
        //        // find number of chars to remove     // from orginal file path
        //        trimLength += 1; //remove '\'
        //        string outPath = inputFolderPath + @"\" + outputPathAndFile;
        //        using (var zipStream = new ZipOutputStream(File.Create(outPath))) // create zip stream
        //        {
        //            if (!string.IsNullOrEmpty(password)) zipStream.Password = password;
        //            zipStream.SetLevel(9); // maximum compression
        //            var buffer = new byte[4096];
        //            foreach (string item in itemsList) // for each file, generate a zipentry
        //            {
        //                var entry = new ZipEntry(item.Remove(0, trimLength)) { IsUnicodeText = true, DateTime = DateTime.Now };
        //                zipStream.PutNextEntry(entry);

        //                if (!item.EndsWith(@"/")) // if a file ends with '/' its a directory
        //                {
        //                    using (FileStream fs = File.OpenRead(item))
        //                    {
        //                        int sourceBytes;
        //                        do
        //                        {
        //                            sourceBytes = fs.Read(buffer, 0,
        //                            buffer.Length);
        //                            zipStream.Write(buffer, 0, sourceBytes);

        //                        } while (sourceBytes > 0);
        //                    }
        //                }
        //            }
        //            zipStream.Finish();
        //            zipStream.Close();
        //            itemsList.Clear();
        //        }
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}

        //private static List<string> AdvantShop_Helpers_FileHelpers_GenerateFileList(string dir, bool recurse)
        //{
        //    var files = new List<string>();
        //    bool empty = true;
        //    foreach (string file in Directory.GetFiles(dir)) // add each file in directory
        //    {
        //        files.Add(file);
        //        empty = false;
        //    }

        //    if (empty)
        //    {
        //        // if directory is completely empty, add it
        //        if (Directory.GetDirectories(dir).Length == 0)
        //        {
        //            files.Add(dir + @"/");
        //        }
        //    }

        //    if (recurse)
        //        foreach (string dirs in Directory.GetDirectories(dir)) // recursive
        //        {
        //            files.AddRange(AdvantShop_Helpers_FileHelpers_GenerateFileList(dirs, true));
        //        }
        //    return files; // return file list
        //}

        //public static void Tools_Updater_ExecuteNonQuery(string commandText, CommandType commandType, params SqlParameter[] parameters)
        //{
        //    using (var cmd = new SqlCommand())
        //    {
        //        cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["AdvantConnectionString"].ConnectionString);
        //        cmd.CommandText = commandText;
        //        cmd.CommandType = commandType;
        //        cmd.Parameters.Clear();
        //        if (parameters != null)
        //        {
        //            cmd.Parameters.AddRange(parameters);
        //        }
        //        cmd.Connection.Open();
        //        cmd.ExecuteNonQuery();
        //        cmd.Connection.Close();
        //    }
        //}

        //public static TResult Tools_Updater_ExecuteScalar<TResult>(string commandText, CommandType commandType,
        //                                             params SqlParameter[] parameters) where TResult : IConvertible
        //{
        //    using (var cmd = new SqlCommand())
        //    {
        //        cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["AdvantConnectionString"].ConnectionString);
        //        cmd.CommandText = commandText;
        //        cmd.CommandType = commandType;
        //        cmd.Parameters.Clear();
        //        if (parameters != null)
        //        {
        //            cmd.Parameters.AddRange(parameters);
        //        }

        //        cmd.Connection.Open();
        //        object o = cmd.ExecuteScalar();
        //        return o is IConvertible ? (TResult)Convert.ChangeType(o, typeof(TResult)) : default(TResult);
        //    }
        //}
        //*********************************************************************************************************************************************
        public static string CompareBaseVersions(bool updateMasks)
        {
            throw new NotImplementedException();
        }

        public static void CreateBaseBackup()
        {
            throw new NotImplementedException();
        }

        public static VersionInformations GetLastVersionInformation()
        {
            throw new NotImplementedException();
        }

        public static void CreateCodeBackup()
        {
            throw new NotImplementedException();
        }

        public static void UpdateAvantshop()
        {
            throw new NotImplementedException();
        }

        public static string CompareCodeVersions(bool updateMasks)
        {
            throw new NotImplementedException();
        }
    }
}
using AdvantShop.Core.Modules;
using AdvantShop.Core.Scheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.IO;
using AdvantShop.Module.SimaLand.Models;

namespace AdvantShop.Module.SimaLand.Service
{
    public enum DownloadImageType
    {
        MainPhoto,
        AllPhoto,
        NoPhoto
    }

    public class PSLModuleSettings
    {
        public static readonly int Version = 10;

        #region Implementation of ModuleDbSettings

        public static int RequestCount
        {
            get { return ModuleSettingsProvider.GetSettingValue<int>("RequestCount", SimaLand.ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("RequestCount", value, SimaLand.ModuleStringId); }
        }

        public static bool WorkOnlySimaLand
        {
            get { return ModuleSettingsProvider.GetSettingValue<bool>("WorkOnlySimaLand", SimaLand.ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("WorkOnlySimaLand", value, SimaLand.ModuleStringId); }
        }

        public static bool LinkInViewOrder
        {
            get { return ModuleSettingsProvider.GetSettingValue<bool>("LinkInViewOrder", SimaLand.ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("LinkInViewOrder", value, SimaLand.ModuleStringId); }
        }

        public static bool AddPriceInRange
        {
            get { return ModuleSettingsProvider.GetSettingValue<bool>("AddPriceInRange", SimaLand.ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("AddPriceInRange", value, SimaLand.ModuleStringId); }
        }

        public static string LastUpdateCategory
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("LastUpdateCategory", SimaLand.ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("LastUpdateCategory", value, SimaLand.ModuleStringId); }
        }

        public static string ElapsedTimeParseCategory
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("ElapsedTimeParseCategory", SimaLand.ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("ElapsedTimeParseCategory", value, SimaLand.ModuleStringId); }
        }

        public static string LastUpdateProducts
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("LastUpdateProducts", SimaLand.ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("LastUpdateProducts", value, SimaLand.ModuleStringId); }
        }

        public static string ElapsedTimeParseProducts
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("ElapsedTimeParseProducts", SimaLand.ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("ElapsedTimeParseProducts", value, SimaLand.ModuleStringId); }
        }

        public static float fromPriceRange
        {
            get { return ModuleSettingsProvider.GetSettingValue<float>("fromPriceRange", SimaLand.ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("fromPriceRange", value, SimaLand.ModuleStringId); }
        }

        public static float toPriceRange
        {
            get { return ModuleSettingsProvider.GetSettingValue<float>("toPriceRange", SimaLand.ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("toPriceRange", value, SimaLand.ModuleStringId); }
        }

        public static string PathToFile
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("PathToFile", SimaLand.ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("PathToFile", value, SimaLand.ModuleStringId); }
        }

        public static string PathToBackFile
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("PathToBackFile", SimaLand.ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("PathToBackFile", value, SimaLand.ModuleStringId); }
        }

        public static bool AddingCategories
        {
            get { return ModuleSettingsProvider.GetSettingValue<bool>("AddingCategories", SimaLand.ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("AddingCategories", value, SimaLand.ModuleStringId); }
        }

        public static bool DownloadMarkers
        {
            get { return ModuleSettingsProvider.GetSettingValue<bool>("DownloadMarkers", SimaLand.ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("DownloadMarkers", value, SimaLand.ModuleStringId); }
        }

        public static bool AddedProductToChildCategory
        {
            get { return ModuleSettingsProvider.GetSettingValue<bool>("AddedProductToChildCategory", SimaLand.ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("AddedProductToChildCategory", value, SimaLand.ModuleStringId); }
        }

        public static bool AutoUpdate
        {
            get { return ModuleSettingsProvider.GetSettingValue<bool>("AutoUpdate", SimaLand.ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("AutoUpdate", value, SimaLand.ModuleStringId); }
        }

        public static string StopMessage
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("StopMessage", SimaLand.ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("StopMessage", value, SimaLand.ModuleStringId); }
        }

        public static string TimeUpdate
        {
            get {
                var time = ModuleSettingsProvider.GetSettingValue<string>("TimeUpdate", SimaLand.ModuleStringId);
                if (string.IsNullOrEmpty(time))
                {
                    time = TimeUpdate = string.Format("{0}:{1}", new Random().Next(6), new Random().Next(59));
                    return time;
                }
                return ModuleSettingsProvider.GetSettingValue<string>("TimeUpdate", SimaLand.ModuleStringId);
            }
            set { ModuleSettingsProvider.SetSettingValue("TimeUpdate", value, SimaLand.ModuleStringId); }
        }

        public static bool ImportDiscount
        {
            get { return ModuleSettingsProvider.GetSettingValue<bool>("ImportDiscount", SimaLand.ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("ImportDiscount", value, SimaLand.ModuleStringId); }
        }

        //public static bool DownloadImage
        //{
        //    get { return ModuleSettingsProvider.GetSettingValue<bool>("DownloadImage", SimaLand.ModuleStringId); }
        //    set { ModuleSettingsProvider.SetSettingValue("DownloadImage", value, SimaLand.ModuleStringId); }
        //}

        public static bool UsePrefix
        {
            get { return ModuleSettingsProvider.GetSettingValue<bool>("UsePrefix", SimaLand.ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("UsePrefix", value, SimaLand.ModuleStringId); }
        }

        public static string PrefixText
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("PrefixText", SimaLand.ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("PrefixText", value, SimaLand.ModuleStringId); }
        }

        public static bool MinMax
        {
            get { return ModuleSettingsProvider.GetSettingValue<bool>("MinMax", SimaLand.ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("MinMax", value, SimaLand.ModuleStringId); }
        }

        public static bool ThreePayTwo
        {
            get { return ModuleSettingsProvider.GetSettingValue<bool>("ThreePayTwo", SimaLand.ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("ThreePayTwo", value, SimaLand.ModuleStringId); }
        }

        public static string ColorThreePayTwo
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("ColorThreePayTwo", SimaLand.ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("ColorThreePayTwo", value, SimaLand.ModuleStringId); }
        }

        public static string HrefThreePayTwo
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("HrefThreePayTwo", SimaLand.ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("HrefThreePayTwo", value, SimaLand.ModuleStringId); }
        }

        public static bool MTGift
        {
            get { return ModuleSettingsProvider.GetSettingValue<bool>("MTGift", SimaLand.ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("MTGift", value, SimaLand.ModuleStringId); }
        }

        public static string ColorMTGift
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("ColorMTGift", SimaLand.ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("ColorMTGift", value, SimaLand.ModuleStringId); }
        }

        public static string HrefMTGift
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("HrefMTGift", SimaLand.ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("HrefMTGift", value, SimaLand.ModuleStringId); }
        }

        public static string JWT
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("JWT", SimaLand.ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("JWT", value, SimaLand.ModuleStringId); }
        }

        public static List<MarkupPriceRange> PriceRange
        {
            get { return JsonConvert.DeserializeObject<List<MarkupPriceRange>>(ModuleSettingsProvider.GetSettingValue<string>("PriceRange", SimaLand.ModuleStringId)).OrderByDescending(x => x.Markup).ToList(); }
            set { ModuleSettingsProvider.SetSettingValue("PriceRange", JsonConvert.SerializeObject(value), SimaLand.ModuleStringId); }
        }

        public static bool AutoUpdateBalance
        {
            get { return ModuleSettingsProvider.GetSettingValue<bool>("AutoUpdateBalance", SimaLand.ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("AutoUpdateBalance", value, SimaLand.ModuleStringId); }
        }

        public static int TimePeriodBalance
        {
            get { return ModuleSettingsProvider.GetSettingValue<int>("TimePeriodBalance", SimaLand.ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("TimePeriodBalance", value, SimaLand.ModuleStringId); }
        }

        public static bool NotUpdateDescription
        {
            get { return ModuleSettingsProvider.GetSettingValue<bool>("NotUpdateDescription", SimaLand.ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("NotUpdateDescription", value, SimaLand.ModuleStringId); }
        }

        public static bool NotUpdateName
        {
            get { return ModuleSettingsProvider.GetSettingValue<bool>("NotUpdateName", SimaLand.ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("NotUpdateName", value, SimaLand.ModuleStringId); }
        }

        public static bool NotUpdateUrl
        {
            get { return ModuleSettingsProvider.GetSettingValue<bool>("NotUpdateUrl", SimaLand.ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("NotUpdateUrl", value, SimaLand.ModuleStringId); }
        }

        public static bool NotUpdateProperty
        {
            get { return ModuleSettingsProvider.GetSettingValue<bool>("NotUpdateProperty", SimaLand.ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("NotUpdateProperty", value, SimaLand.ModuleStringId); }
        }

        public static DownloadImageType DwnlImageType
        {
            get {

                DownloadImageType result;
                try
                {
                   Enum.TryParse(ModuleSettingsProvider.GetSettingValue<string>("DwnlImageType", SimaLand.ModuleStringId), out result);
                }
                catch (Exception ex)
                {
                    result = DownloadImageType.MainPhoto;
                }
                return result;
            }
            set { ModuleSettingsProvider.SetSettingValue("DwnlImageType", value, SimaLand.ModuleStringId); }
        }

        public static bool ReloadImages
        {
            get { return ModuleSettingsProvider.GetSettingValue<bool>("ReloadImages", SimaLand.ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("ReloadImages", value, SimaLand.ModuleStringId); }
        }

        public static bool AlwaysAvailable
        {
            get { return ModuleSettingsProvider.GetSettingValue<bool>("AlwaysAvailable", SimaLand.ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("AlwaysAvailable", value, SimaLand.ModuleStringId); }
        }

        public static void SetSettings()
        {
            RequestCount = 0;
            LastUpdateCategory = "01.01.1990";
            ElapsedTimeParseCategory = "0";
            LastUpdateProducts = "01.01.1990";
            ElapsedTimeParseProducts = "0";
            WorkOnlySimaLand = true;
            LinkInViewOrder = false;
            AddPriceInRange = false;
            fromPriceRange = 0.0f;
            toPriceRange = 0.0f;
            PathToFile = "";
            PathToBackFile = "";
            AddingCategories = false;
            DownloadMarkers = false;
            AddedProductToChildCategory = false;
            AutoUpdate = false;
            StopMessage = "";
            TimeUpdate = string.Format("{0}:{1}", new Random().Next(6), new Random().Next(59));
            ImportDiscount = false;
            //DownloadImage = true;
            UsePrefix = false;
            PrefixText = "";
            MinMax = false;
            ThreePayTwo = false;
            ColorThreePayTwo = "cd0303";
            MTGift = false;
            ColorMTGift = "cd0303";
            HrefMTGift = "";
            HrefThreePayTwo = "";
            JWT = "";
            #region PriceRange

                PriceRange = new List<MarkupPriceRange>() {
                    new MarkupPriceRange
                    {
                        Id =1,
                        MinPrice = 0,
                        MaxPrice = 100,
                        Markup = 80
                    },
                    new MarkupPriceRange
                    {
                        Id =2,
                        MinPrice = 101,
                        MaxPrice = 300,
                        Markup = 70
                    },
                    new MarkupPriceRange
                    {
                        Id =3,
                        MinPrice = 301,
                        MaxPrice = 700,
                        Markup = 60
                    },
                    new MarkupPriceRange
                    {
                        Id =4,
                        MinPrice = 701,
                        MaxPrice = 1000,
                        Markup = 50
                    },
                    new MarkupPriceRange
                    {
                        Id =5,
                        MinPrice = 1001,
                        MaxPrice = 999999,
                        Markup = 40
                    }
                };

            #endregion
            AutoUpdateBalance = false;
            TimePeriodBalance = 6;
            NotUpdateDescription = false;
            NotUpdateName = false;
            NotUpdateUrl = false;
            DwnlImageType = DownloadImageType.MainPhoto;
            ReloadImages = false;
            AlwaysAvailable = false;
        }

        public static void RemoveSettings()
        {
            ModuleSettingsProvider.RemoveSqlSetting("RequestCount", SimaLand.ModuleStringId);
            ModuleSettingsProvider.RemoveSqlSetting("LastUpdateCategory", SimaLand.ModuleStringId);
            ModuleSettingsProvider.RemoveSqlSetting("ElapsedTimeParseCategory", SimaLand.ModuleStringId);
            ModuleSettingsProvider.RemoveSqlSetting("LastUpdateProducts", SimaLand.ModuleStringId);
            ModuleSettingsProvider.RemoveSqlSetting("ElapsedTimeParseProducts", SimaLand.ModuleStringId);
            ModuleSettingsProvider.RemoveSqlSetting("WorkOnlySimaLand", SimaLand.ModuleStringId);
            ModuleSettingsProvider.RemoveSqlSetting("LinkInViewOrder", SimaLand.ModuleStringId);
            ModuleSettingsProvider.RemoveSqlSetting("AddPriceInRange", SimaLand.ModuleStringId);
            ModuleSettingsProvider.RemoveSqlSetting("fromPriceRange", SimaLand.ModuleStringId);
            ModuleSettingsProvider.RemoveSqlSetting("toPriceRange", SimaLand.ModuleStringId);
            ModuleSettingsProvider.RemoveSqlSetting("PathToFile", SimaLand.ModuleStringId);
            ModuleSettingsProvider.RemoveSqlSetting("PathToBackFile", SimaLand.ModuleStringId);
            ModuleSettingsProvider.RemoveSqlSetting("ProcessParsing", SimaLand.ModuleStringId);
            ModuleSettingsProvider.RemoveSqlSetting("AddingCategories", SimaLand.ModuleStringId);
            ModuleSettingsProvider.RemoveSqlSetting("DownloadMarkers", SimaLand.ModuleStringId);
            ModuleSettingsProvider.RemoveSqlSetting("AddedProductToChildCategory", SimaLand.ModuleStringId);
            ModuleSettingsProvider.RemoveSqlSetting("AutoUpdate", SimaLand.ModuleStringId);
            ModuleSettingsProvider.RemoveSqlSetting("StopMessage", SimaLand.ModuleStringId);
            ModuleSettingsProvider.RemoveSqlSetting("TimeUpdate", SimaLand.ModuleStringId);
            ModuleSettingsProvider.RemoveSqlSetting("ImportDiscount", SimaLand.ModuleStringId);
            ModuleSettingsProvider.RemoveSqlSetting("DownloadImage", SimaLand.ModuleStringId);
            ModuleSettingsProvider.RemoveSqlSetting("UsePrefix", SimaLand.ModuleStringId);
            ModuleSettingsProvider.RemoveSqlSetting("PrefixText", SimaLand.ModuleStringId);
            ModuleSettingsProvider.RemoveSqlSetting("MinMax", SimaLand.ModuleStringId);
            ModuleSettingsProvider.RemoveSqlSetting("ThreePayTwo", SimaLand.ModuleStringId);
            ModuleSettingsProvider.RemoveSqlSetting("ColorThreePayTwo", SimaLand.ModuleStringId);
            ModuleSettingsProvider.RemoveSqlSetting("HrefThreePayTwo", SimaLand.ModuleStringId);
            ModuleSettingsProvider.RemoveSqlSetting("MTGift", SimaLand.ModuleStringId);
            ModuleSettingsProvider.RemoveSqlSetting("ColorMTGift", SimaLand.ModuleStringId);
            ModuleSettingsProvider.RemoveSqlSetting("HrefMTGift", SimaLand.ModuleStringId);
            ModuleSettingsProvider.RemoveSqlSetting("JWT", SimaLand.ModuleStringId);
            ModuleSettingsProvider.RemoveSqlSetting("PriceRange", SimaLand.ModuleStringId);
            ModuleSettingsProvider.RemoveSqlSetting("AutoUpdateBalance", SimaLand.ModuleStringId);
            ModuleSettingsProvider.RemoveSqlSetting("TimePeriodBalance", SimaLand.ModuleStringId);
            ModuleSettingsProvider.RemoveSqlSetting("NotUpdateDescription", SimaLand.ModuleStringId);
            ModuleSettingsProvider.RemoveSqlSetting("NotUpdateName", SimaLand.ModuleStringId);
            ModuleSettingsProvider.RemoveSqlSetting("NotUpdateUrl", SimaLand.ModuleStringId);
            ModuleSettingsProvider.RemoveSqlSetting("DwnlImageType", SimaLand.ModuleStringId);
            ModuleSettingsProvider.RemoveSqlSetting("ReloadImages", SimaLand.ModuleStringId);
            ModuleSettingsProvider.RemoveSqlSetting("AlwaysAvailable", SimaLand.ModuleStringId);
        }

        #endregion
    }

    public class ModuleSettingsModel
    {
        public bool lArtno { get; set; }
        public bool aRange { get; set; }
        public float fPrice { get; set; }
        public float tPrice { get; set; }
        public bool dMarkers { get; set; }
        public bool addCategories { get; set; }
        public bool au { get; set; }
        public string auh { get; set; }
        public string aum { get; set; }
        public string dwnlbtn { get; set; }
        public bool dwnlbtnEnabled { get; set; }
        public bool impdisc { get; set; }
        public bool dwnlImg { get; set; }
        public bool useprefix { get; set; }
        public string txtprefix { get; set; }
        public bool minmax { get; set; }
        public bool threepaytwo { get; set; }
        public string colorthreepaytwo { get; set; }
        public string hrefthreepaytwo { get; set; }
        public bool mtgift { get; set; }
        public string colormtgift { get; set; }
        public string hrefmtgift { get; set; }
        public string dwnlimagetype { get; set; }
        public bool reloadImages { get; set; }
        public bool alwaysAvailable { get; set; }

        public ModuleSettingsModel()
        {
            lArtno = PSLModuleSettings.LinkInViewOrder;
            aRange = PSLModuleSettings.AddPriceInRange;
            fPrice = PSLModuleSettings.fromPriceRange;
            tPrice = PSLModuleSettings.toPriceRange;
            dMarkers = PSLModuleSettings.DownloadMarkers;
            addCategories = PSLModuleSettings.AddedProductToChildCategory;
            au = PSLModuleSettings.AutoUpdate;
            //auh = PSLModuleSettings.TimeUpdate.Split(':')[0];
            //aum = PSLModuleSettings.TimeUpdate.Split(':')[1];
            dwnlbtn = SimalandImportStatistic.Process ? "Остановить загрузку" : "Загрузить товары";
            dwnlbtnEnabled = PSLModuleSettings.AddingCategories;
            impdisc = PSLModuleSettings.ImportDiscount;
            //dwnlImg = PSLModuleSettings.DownloadImage;
            useprefix = PSLModuleSettings.UsePrefix;
            txtprefix = PSLModuleSettings.PrefixText;
            minmax = PSLModuleSettings.MinMax;
            threepaytwo = PSLModuleSettings.ThreePayTwo;
            colorthreepaytwo = PSLModuleSettings.ColorThreePayTwo;
            hrefthreepaytwo = PSLModuleSettings.HrefThreePayTwo;
            mtgift = PSLModuleSettings.MTGift;
            colormtgift = PSLModuleSettings.ColorMTGift;
            hrefmtgift = PSLModuleSettings.HrefMTGift;
            dwnlimagetype = PSLModuleSettings.DwnlImageType.ToString();
            reloadImages = PSLModuleSettings.ReloadImages;
            alwaysAvailable = PSLModuleSettings.AlwaysAvailable;
        }

        public StatusMessage SaveSettings()
        {

            if (aRange)
            {
                if (fPrice < tPrice)
                {
                    PSLModuleSettings.fromPriceRange = fPrice;
                    PSLModuleSettings.toPriceRange = tPrice;
                    PSLModuleSettings.AddPriceInRange = aRange;
                }
                else
                {
                    return new StatusMessage("Некорректно введен диапазон цен", StatusMessage.Status.Error);
                }

            }
            else
            {
                PSLModuleSettings.AddPriceInRange = aRange;
            }
            if (aRange != PSLModuleSettings.LinkInViewOrder)
            {
                ActiveRenderArtNo(lArtno);
            }

            //PSLModuleSettings.TimeUpdate = auh + ":" + aum;

            if (!au)
            {
                if (SimaLand.GetTasks().Count > 0)
                    TaskManager.TaskManagerInstance().RemoveModuleTask(SimaLand.GetTasks().First());
            }


            PSLModuleSettings.AutoUpdate = au;
            TaskManager.TaskManagerInstance().ManagedTask(TaskSettings.Settings);

            PSLModuleSettings.ImportDiscount = impdisc;

            PSLModuleSettings.DownloadMarkers = dMarkers;
            //PSLModuleSettings.DownloadImage = dwnlImg;
            PSLModuleSettings.AddedProductToChildCategory = addCategories;
            PSLModuleSettings.MinMax = minmax;
            PSLModuleSettings.ThreePayTwo = threepaytwo;
            PSLModuleSettings.ColorThreePayTwo = colorthreepaytwo;
            PSLModuleSettings.MTGift = mtgift;
            PSLModuleSettings.ColorMTGift = colormtgift;
            PSLModuleSettings.HrefMTGift = string.IsNullOrEmpty(hrefmtgift) ? "" : hrefmtgift;
            PSLModuleSettings.HrefThreePayTwo = string.IsNullOrEmpty(hrefthreepaytwo) ? "" : hrefthreepaytwo;
            PSLModuleSettings.AlwaysAvailable = alwaysAvailable;

            DownloadImageType dit;
            if (Enum.TryParse(dwnlimagetype, out dit))
                PSLModuleSettings.DwnlImageType = dit;

            PSLModuleSettings.ReloadImages = reloadImages;

            if (!string.IsNullOrEmpty(txtprefix))
            {
                PSLModuleSettings.PrefixText = txtprefix;
                PSLModuleSettings.UsePrefix = useprefix;
            }
            return new StatusMessage("Сохранено", StatusMessage.Status.Success);            
        }

        private void ActiveRenderArtNo(bool check)
        {
            var path = System.Web.Hosting.HostingEnvironment.MapPath("~/admin/ViewOrder.aspx");
            PSLModuleSettings.PathToFile = path;
            var scr = @"<!--programmcustompromo--><script type='text/javascript' src='../Modules/" + SimaLand.ModuleStringId + @"/Content/Scripts/custom-view-order.js?" + new Random().Next(0, 1000) + "'></script>";
            var file = System.IO.File.ReadAllLines(path);
            var backPath = System.Web.Hosting.HostingEnvironment.MapPath("~/modules/" + SimaLand.ModuleStringId + "/back/");
            var fileName = "ViewOrder.aspx";
            PSLModuleSettings.PathToBackFile = backPath + fileName;
            if (check && !custom(file))
            {
                try
                {
                    if (!Directory.Exists(backPath))
                    {
                        Directory.CreateDirectory(backPath);
                    }
                    System.IO.File.Copy(path, backPath + fileName, true);
                    var teg = file[file.Length - 1];
                    file[file.Length - 1] = scr;

                    System.IO.File.WriteAllLines(path, file);
                    System.IO.File.AppendAllLines(path, new List<string> { teg });

                    PSLModuleSettings.LinkInViewOrder = true;
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                try
                {
                    if (custom(file) && System.IO.File.Exists(backPath + fileName))
                    {
                        System.IO.File.Copy(backPath + fileName, path, true);

                        PSLModuleSettings.LinkInViewOrder = false;
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }

        private bool custom(string[] strings)
        {
            for (int i = strings.Length - 1; i > strings.Length - 3; i--)
            {
                if (strings[i].Contains("programmcustompromo"))
                {
                    return true;
                }
            }
            return false;
        }
    }
}

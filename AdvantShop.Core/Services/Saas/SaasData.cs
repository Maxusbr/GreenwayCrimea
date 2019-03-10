//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;

namespace AdvantShop.Saas
{
    public class SaasData
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public int ProductsCount { get; set; }
        public int PhotosCount { get; set; }
        /// <summary>
        /// Объем файлового хранилища, ГБ. 0 - неограниченно
        /// </summary>
        public int FileStorageVolume { get; set; }
        //public int ManagersCount { get; set; }
        public bool HaveExcel { get; set; }
        public bool Have1C { get; set; }
        public bool HaveExportFeeds { get; set; }
        public bool HavePriceRegulating { get; set; }
        //public bool HaveBankIntegration { get; set; }

        public bool HaveCrm { get; set; }
        public bool HaveTelephony { get; set; }
        public bool HaveMobileAdmin { get; set; }
        public bool HaveTags { get; set; }
        public bool HaveCustomerLog { get; set; }

        public bool HaveCustom { get; set; }
        public bool HaveVIPsupport { get; set; }

        public bool MobileVersion { get; set; }
        public bool GoogleYandexMetriks { get; set; }
        public bool LandingPage { get; set; }
        public bool RoleActions { get; set; }
        public bool OrderStatuses { get; set; }
        public bool OrderAdditionFields { get; set; }
        public bool BonusSystem { get; set; }
        public bool DeepAnalytics { get; set; }

        public bool BizProcess { get; set; }

        public bool CustomerAdditionFields { get; set; }
        public int EmployeesCount { get; set; }


        public string SupportChat { get; set; }

        public bool IsWork { get; set; }

        public string ClientNumber { get; set; }
        public int LeftDay { get; set; }
        public DateTime LastUpdate { get; set; }

        public decimal Money { get; set; }
        public decimal Bonus { get; set; }
        public string Error { get; set; }

        public bool IsCorrect
        {
            get { return Error == "fine" || string.IsNullOrEmpty(Error); }
        }

        public string BalanceFormating { get; set; }

        public bool HaveDomains { get; set; }

        public decimal Balance
        {
            get
            {
                return Money + Bonus;
            }
        }

        public SaasData()
        {
            Name = string.Empty;
            LastUpdate = DateTime.Now.AddDays(-7);
            Money = 0;
            Bonus = 0;
            Error = "fine";
        }
    }
}

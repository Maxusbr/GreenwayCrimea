using System;
using NUnit.Framework;
using System.Threading;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Admin.Settings.SystemSettings.Countries.Region
{
    [TestFixture]
    public class SettingsRegionSortTest : BaseSeleniumTest
    {

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Customers);
            InitializeService.LoadData(
             "data\\Admin\\Settings\\Countries\\Region\\Customers.Country.csv",
               "data\\Admin\\Settings\\Countries\\Region\\Customers.Region.csv",
              "data\\Admin\\Settings\\Countries\\Region\\Customers.City.csv",
            "data\\Admin\\Settings\\Countries\\Region\\Customers.Customer.csv",
           "data\\Admin\\Settings\\Countries\\Region\\Customers.CustomerGroup.csv"

           );

            Init();
            GoToAdmin("settingssystem#?systemTab=countries");
            GetGridCell(0, "Name", "Country").Click();
            Thread.Sleep(2000);
        }

        [Test]
        public void RegionSortName()
        {
            testname = "RegionSortName";
            VerifyBegin(testname);

            GetGridCell(-1, "Name", "Region").Click();
            WaitForAjax();
            VerifyAreEqual("TestRegion1", GetGridCell(0, "Name", "Region").Text, "sort Name 1 asc");
            VerifyAreEqual("TestRegion16", GetGridCell(9, "Name", "Region").Text, "sort Name 10 asc");

            GetGridCell(-1, "Name", "Region").Click();
            WaitForAjax();
            VerifyAreEqual("TestRegion99", GetGridCell(0, "Name", "Region").Text, "sort Name 1 desc");
            VerifyAreEqual("TestRegion90", GetGridCell(9, "Name", "Region").Text, "sort Name 10 desc");

            VerifyFinally(testname);
        }

        [Test]
        public void RegionSortCode()
        {
            testname = "RegionSortCode";
            VerifyBegin(testname);

            GetGridCell(-1, "RegionCode", "Region").Click();
            WaitForAjax();
            VerifyAreEqual("TestRegion90", GetGridCell(0, "Name", "Region").Text, "sort Code 1 asc");
            VerifyAreEqual("TestRegion99", GetGridCell(9, "Name", "Region").Text, "sort Code 10 asc");

            GetGridCell(-1, "RegionCode", "Region").Click();
            WaitForAjax();
            VerifyAreEqual("TestRegion89", GetGridCell(0, "Name", "Region").Text, "sort Code 1 desc");
            VerifyAreEqual("TestRegion80", GetGridCell(9, "Name", "Region").Text, "sort Code 10 desc");

            VerifyFinally(testname);
        }

        [Test]
        public void RegionSortOrder()
        {
            testname = "RegionSortOrder";
            VerifyBegin(testname);

            GetGridCell(-1, "SortOrder", "Region").Click();
            WaitForAjax();
            VerifyAreEqual("TestRegion1", GetGridCell(0, "Name", "Region").Text, "sort SortOrder 1 asc");
            VerifyAreEqual("TestRegion10", GetGridCell(9, "Name", "Region").Text, "sort SortOrder 10 asc");

            GetGridCell(-1, "SortOrder", "Region").Click();
            WaitForAjax();
            VerifyAreEqual("TestRegion101", GetGridCell(0, "Name", "Region").Text, "sort SortOrder 1 desc");
            VerifyAreEqual("TestRegion92", GetGridCell(9, "Name", "Region").Text, "sort SortOrder 10 desc");

            VerifyFinally(testname);
        }
    }
}

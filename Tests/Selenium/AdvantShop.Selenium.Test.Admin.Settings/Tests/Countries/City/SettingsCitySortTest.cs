using System;
using NUnit.Framework;
using System.Threading;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Admin.Settings.SystemSettings.Countries.City
{
    [TestFixture]
    public class SettingsCitySortTest : BaseSeleniumTest
    {

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Customers);
            InitializeService.LoadData(
             "data\\Admin\\Settings\\Countries\\City\\Customers.Country.csv",
               "data\\Admin\\Settings\\Countries\\City\\Customers.Region.csv",
              "data\\Admin\\Settings\\Countries\\City\\Customers.City.csv",
            "data\\Admin\\Settings\\Countries\\City\\Customers.Customer.csv",
           "data\\Admin\\Settings\\Countries\\City\\Customers.CustomerGroup.csv"

           );

            Init();
            GoToAdmin("settingssystem#?systemTab=countries");
            GetGridCell(0, "Name", "Country").Click();
            Thread.Sleep(2000);
            GetGridCell(0, "Name", "Region").Click();
            Thread.Sleep(2000);

        }

        [Test]
        public void CitySortDisplay()
        {
            testname = "CitySortDisplay";
            VerifyBegin(testname);

            GetGridCell(-1, "DisplayInPopup", "City").Click();
            WaitForAjax();
            VerifyAreEqual("TestCity15", GetGridCell(0, "Name", "City").Text, "sort DisplayInPopup 1 asc");
            VerifyAreEqual("TestCity24", GetGridCell(9, "Name", "City").Text, "sort DisplayInPopup 10 asc");

            GetGridCell(-1, "DisplayInPopup", "City").Click();
            WaitForAjax();
            VerifyAreEqual("TestCity10", GetGridCell(0, "Name", "City").Text, "sort DisplayInPopup 1 desc");
            VerifyAreEqual("TestCity19", GetGridCell(9, "Name", "City").Text, "sort DisplayInPopup 10 desc");

            VerifyFinally(testname);
        }
        [Test]
        public void CitySortMobile()
        {
            testname = "CitySortMobile";
            VerifyBegin(testname);

            GetGridCell(-1, "MobilePhoneNumber", "City").Click();
            WaitForAjax();
            VerifyAreEqual("TestCity101", GetGridCell(0, "Name", "City").Text, "sort MobilePhoneNumber 1 asc");
            VerifyAreEqual("TestCity92", GetGridCell(9, "Name", "City").Text, "sort MobilePhoneNumber 10 asc");

            GetGridCell(-1, "MobilePhoneNumber", "City").Click();
            WaitForAjax();
            VerifyAreEqual("TestCity1", GetGridCell(0, "Name", "City").Text, "sort MobilePhoneNumber 1 desc");
            VerifyAreEqual("TestCity10", GetGridCell(9, "Name", "City").Text, "sort MobilePhoneNumber 10 desc");

            VerifyFinally(testname);
        }

        [Test]
        public void CitySortName()
        {
            testname = "CitySortName";
            VerifyBegin(testname);

            GetGridCell(-1, "Name", "City").Click();
            WaitForAjax();
            VerifyAreEqual("TestCity1", GetGridCell(0, "Name", "City").Text, "sort Name 1 asc");
            VerifyAreEqual("TestCity16", GetGridCell(9, "Name", "City").Text, "sort Name 10 asc");

            GetGridCell(-1, "Name", "City").Click();
            WaitForAjax();
            VerifyAreEqual("TestCity99", GetGridCell(0, "Name", "City").Text, "sort Name 1 desc");
            VerifyAreEqual("TestCity90", GetGridCell(9, "Name", "City").Text, "sort Name 10 desc");

            VerifyFinally(testname);
        }

        [Test]
        public void CitySortPhone()
        {
            testname = "CitySortPhone";
            VerifyBegin(testname);

            GetGridCell(-1, "PhoneNumber", "City").Click();
            WaitForAjax();
            VerifyAreEqual("TestCity1", GetGridCell(0, "Name", "City").Text, "sort PhoneNumber 1 asc");
            VerifyAreEqual("TestCity10", GetGridCell(9, "Name", "City").Text, "sort PhoneNumber 10 asc");

            GetGridCell(-1, "PhoneNumber", "City").Click();
            WaitForAjax();
            VerifyAreEqual("TestCity101", GetGridCell(0, "Name", "City").Text, "sort PhoneNumber 1 desc");
            VerifyAreEqual("TestCity92", GetGridCell(9, "Name", "City").Text, "sort PhoneNumber 10 desc");

            VerifyFinally(testname);
        }


        [Test]
        public void CitySortSortOrder()
        {
            testname = "CitySortSortOrder";
            VerifyBegin(testname);

            GetGridCell(-1, "CitySort", "City").Click();
            WaitForAjax();
            VerifyAreEqual("TestCity1", GetGridCell(0, "Name", "City").Text, "sort CitySort 1 asc");
            VerifyAreEqual("TestCity10", GetGridCell(9, "Name", "City").Text, "sort CitySort 10 asc");

            GetGridCell(-1, "CitySort", "City").Click();
            WaitForAjax();
            VerifyAreEqual("TestCity101", GetGridCell(0, "Name", "City").Text, "sort CitySort 1 desc");
            VerifyAreEqual("TestCity92", GetGridCell(9, "Name", "City").Text, "sort CitySort 10 desc");

            VerifyFinally(testname);
        }
    }
}

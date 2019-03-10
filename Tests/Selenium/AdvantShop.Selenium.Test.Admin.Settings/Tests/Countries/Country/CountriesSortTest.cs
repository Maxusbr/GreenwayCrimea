using System;
using NUnit.Framework;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Admin.Settings.SystemSettings.Countries.Country
{
    [TestFixture]
    public class CountriesSortTest : BaseSeleniumTest
    {

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Customers);
            InitializeService.LoadData(
             "data\\Admin\\Settings\\Countries\\Customers.Country.csv",
               "data\\Admin\\Settings\\Countries\\Customers.Region.csv",
              "data\\Admin\\Settings\\Countries\\Customers.City.csv",
            "data\\Admin\\Settings\\Countries\\Customers.Customer.csv",
           "data\\Admin\\Settings\\Countries\\Customers.CustomerGroup.csv"

           );

            Init();
            GoToAdmin("settingssystem#?systemTab=countries");

        }

        [Test]
        public void CountriesSortName()
        {
            testname = "CountriesSortName";
            VerifyBegin(testname);

            GetGridCell(-1, "Name", "Country").Click();
            WaitForAjax();
            VerifyAreEqual("TestCountry1", GetGridCell(0, "Name", "Country").Text, "sort Name 1 asc");
            VerifyAreEqual("TestCountry16", GetGridCell(9, "Name", "Country").Text, "sort Name 10 asc");

            GetGridCell(-1, "Name", "Country").Click();
            WaitForAjax();
            VerifyAreEqual("TestCountry99", GetGridCell(0, "Name", "Country").Text, "sort Name 1 desc");
            VerifyAreEqual("TestCountry90", GetGridCell(9, "Name", "Country").Text, "sort Name 10 desc");

            VerifyFinally(testname);
        }

        [Test]
        public void CountriesSortIso2()
        {
            testname = "CountriesSortIso2";
            VerifyBegin(testname);

            GetGridCell(-1, "Iso2", "Country").Click();
            WaitForAjax();
            VerifyAreEqual("TestCountry1", GetGridCell(0, "Name", "Country").Text, "sort Iso2 1 asc");
            VerifyAreEqual("TestCountry10", GetGridCell(9, "Name", "Country").Text, "sort Iso2 10 asc");

            GetGridCell(-1, "Iso2", "Country").Click();
            WaitForAjax();
            VerifyAreEqual("TestCountry101", GetGridCell(0, "Name", "Country").Text, "sort Iso2 1 desc");
            VerifyAreEqual("TestCountry92", GetGridCell(9, "Name", "Country").Text, "sort Iso2 10 desc");

            VerifyFinally(testname);
        }

        [Test]
        public void CountriesSortIso3()
        {
            testname = "CountriesSortIso3";
            VerifyBegin(testname);

            GetGridCell(-1, "Iso3", "Country").Click();
            WaitForAjax();
            VerifyAreEqual("TestCountry1", GetGridCell(0, "Name", "Country").Text, "sort Iso3 1 asc");
            VerifyAreEqual("TestCountry10", GetGridCell(9, "Name", "Country").Text, "sort Iso3 10 asc");

            GetGridCell(-1, "Iso3", "Country").Click();
            WaitForAjax();
            VerifyAreEqual("TestCountry101", GetGridCell(0, "Name", "Country").Text, "sort Iso3 1 desc");
            VerifyAreEqual("TestCountry92", GetGridCell(9, "Name", "Country").Text, "sort Iso3 10 desc");

            VerifyFinally(testname);
        }


        [Test]
        public void CountriesSortDisplay()
        {
            testname = "CountriesSortDisplay";
            VerifyBegin(testname);

            GetGridCell(-1, "DisplayInPopup", "Country").Click();
            WaitForAjax();
            VerifyAreEqual("TestCountry7", GetGridCell(0, "Name", "Country").Text, "sort DisplayInPopup 1 asc");
            VerifyAreEqual("TestCountry16", GetGridCell(9, "Name", "Country").Text, "sort DisplayInPopup 10 asc");

            GetGridCell(-1, "DisplayInPopup", "Country").Click();
            WaitForAjax();
            VerifyAreEqual("TestCountry2", GetGridCell(0, "Name", "Country").Text, "sort DisplayInPopup 1 desc");
            VerifyAreEqual("TestCountry11", GetGridCell(9, "Name", "Country").Text, "sort DisplayInPopup 10 desc");

            VerifyFinally(testname);
        }


        [Test]
        public void CountriesSortCode()
        {
            testname = "CountriesSortCode";
            VerifyBegin(testname);

            GetGridCell(-1, "DialCode", "Country").Click();
            WaitForAjax();
            VerifyAreEqual("TestCountry1", GetGridCell(0, "Name", "Country").Text, "sort DialCode 1 asc");
            VerifyAreEqual("TestCountry10", GetGridCell(9, "Name", "Country").Text, "sort DialCode 10 asc");

            GetGridCell(-1, "DialCode", "Country").Click();
            WaitForAjax();
            VerifyAreEqual("TestCountry101", GetGridCell(0, "Name", "Country").Text, "sort DialCode 1 desc");
            VerifyAreEqual("TestCountry92", GetGridCell(9, "Name", "Country").Text, "sort DialCode 10 desc");

            VerifyFinally(testname);
        }
        [Test]
        public void CountriesSortSortOrder()
        {
            testname = "CountriesSortSortOrder";
            VerifyBegin(testname);

            GetGridCell(-1, "SortOrder", "Country").Click();
            WaitForAjax();
            VerifyAreEqual("TestCountry1", GetGridCell(0, "Name", "Country").Text, "sort SortOrder 1 asc");
            VerifyAreEqual("TestCountry10", GetGridCell(9, "Name", "Country").Text, "sort SortOrder 10 asc");

            GetGridCell(-1, "SortOrder", "Country").Click();
            WaitForAjax();
            VerifyAreEqual("TestCountry101", GetGridCell(0, "Name", "Country").Text, "sort SortOrder 1 desc");
            VerifyAreEqual("TestCountry92", GetGridCell(9, "Name", "Country").Text, "sort SortOrder 10 desc");

            VerifyFinally(testname);
        }
    }
}

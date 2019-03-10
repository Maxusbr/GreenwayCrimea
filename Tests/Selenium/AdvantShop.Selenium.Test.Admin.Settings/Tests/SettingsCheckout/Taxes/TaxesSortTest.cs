using System;
using NUnit.Framework;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Admin.Settings.SettingsCheckout.Taxes
{
    [TestFixture]
    public class SettingsTaxesSortTest : BaseSeleniumTest
    {

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Taxes);
            InitializeService.LoadData(

            "data\\Admin\\Settings\\Taxes\\Catalog.Tax.csv",
           "data\\Admin\\Settings\\Taxes\\Settings.Settings.csv"

           );

            Init();

            GoToAdmin("settingscheckout#?checkoutTab=taxes");
        }

        [Test]
        public void ByName()
        {
            testname = "TaxesSortByName";
            VerifyBegin(testname);

            GetGridCell(-1, "Name", "Taxes").Click();
            WaitForAjax();
            VerifyAreEqual("Tax 1", GetGridCell(0, "Name", "Taxes").Text, "Name 1 asc");
            VerifyAreEqual("Tax 107", GetGridCell(9, "Name", "Taxes").Text, "Name 10 asc");

            GetGridCell(-1, "Name", "Taxes").Click();
            WaitForAjax();
            VerifyAreEqual("Tax 99", GetGridCell(0, "Name", "Taxes").Text, "Name 1 desc");
            VerifyAreEqual("Tax 90", GetGridCell(9, "Name", "Taxes").Text, "Name 10 desc");

            VerifyFinally(testname);
        }
        
        [Test]
        public void ByEnabled()
        {
            testname = "TaxesSortByEnabled";
            VerifyBegin(testname);

            GetGridCell(-1, "Enabled", "Taxes").Click();
            WaitForAjax();
            VerifyIsFalse(GetGridCell(0, "Enabled", "Taxes").FindElement(By.TagName("input")).Selected, "Enabled 1 asc");
            VerifyIsFalse(GetGridCell(9, "Enabled", "Taxes").FindElement(By.TagName("input")).Selected, "Enabled 10 asc");

            string ascLine1 = GetGridCell(0, "Name", "Taxes").Text;
            string ascLine10 = GetGridCell(9, "Name", "Taxes").Text;

            VerifyIsFalse(ascLine1.Equals(ascLine10), "asc different taxes");

            GetGridCell(-1, "Enabled", "Taxes").Click();
            WaitForAjax();
            VerifyIsTrue(GetGridCell(0, "Enabled", "Taxes").FindElement(By.TagName("input")).Selected, "Enabled 1 desc");
            VerifyIsTrue(GetGridCell(9, "Enabled", "Taxes").FindElement(By.TagName("input")).Selected, "Enabled 10 desc");

            string descLine1 = GetGridCell(0, "Name", "Taxes").Text;
            string descLine10 = GetGridCell(9, "Name", "Taxes").Text;

            VerifyIsFalse(descLine1.Equals(descLine10), "desc different taxes");

            VerifyFinally(testname);
        }

        [Test]
        public void ByTaxType()
        {
            testname = "TaxesSortTaxType";
            VerifyBegin(testname);

            GetGridCell(-1, "TaxTypeFormatted", "Taxes").Click();
            WaitForAjax();
            VerifyAreEqual("Другой", GetGridCell(0, "TaxTypeFormatted", "Taxes").Text, "TaxType 1 asc");
            VerifyAreEqual("Другой", GetGridCell(9, "TaxTypeFormatted", "Taxes").Text, "TaxType 10 asc");

            string ascLine1 = GetGridCell(0, "Name", "Taxes").Text;
            string ascLine10 = GetGridCell(9, "Name", "Taxes").Text;

            VerifyIsFalse(ascLine1.Equals(ascLine10), "asc different taxes");

            GetGridCell(-1, "TaxTypeFormatted", "Taxes").Click();
            WaitForAjax();
            VerifyAreEqual("НДС по ставке 18%", GetGridCell(0, "TaxTypeFormatted", "Taxes").Text, "TaxType 1 desc");
            VerifyAreEqual("НДС по ставке 18%", GetGridCell(9, "TaxTypeFormatted", "Taxes").Text, "TaxType 10 desc");

            string descLine1 = GetGridCell(0, "Name", "Taxes").Text;
            string descLine10 = GetGridCell(9, "Name", "Taxes").Text;

            VerifyIsFalse(descLine1.Equals(descLine10), "desc different taxes");

            VerifyFinally(testname);
        }

        [Test]
        public void ByRate()
        {
            testname = "TaxesSortByRate";
            VerifyBegin(testname);

            GetGridCell(-1, "Rate", "Taxes").Click();
            WaitForAjax();
            VerifyAreEqual("0", GetGridCell(0, "Rate", "Taxes").Text, "Rate 1 asc");
            VerifyAreEqual("9", GetGridCell(9, "Rate", "Taxes").Text, "Rate 10 asc");

            GetGridCell(-1, "Rate", "Taxes").Click();
            WaitForAjax();
            VerifyAreEqual("106", GetGridCell(0, "Rate", "Taxes").Text, "Rate 1 desc");
            VerifyAreEqual("97", GetGridCell(9, "Rate", "Taxes").Text, "Rate 10 desc");

            VerifyFinally(testname);
        }
    }
}
using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Admin.Settings.SettingsCheckout.Taxes
{
    [TestFixture]
    public class SettingsTaxesSearchTest : BaseSeleniumTest
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
        }

        [Test]
        public void SearchExistName()
        {
            testname = "TaxesSearchExistName";
            VerifyBegin(testname);

            GoToAdmin("settingscheckout#?checkoutTab=taxes");
            
            GetGridIdFilter("gridTaxes", "Tax 68");
            driver.FindElement(By.CssSelector(".tab-pane.active")).FindElement(By.TagName("h1")).Click();
            WaitForAjax();
            Thread.Sleep(2000);

            VerifyAreEqual("Tax 68", GetGridCell(0, "Name", "Taxes").Text, "search exist tax");
            VerifyAreEqual("Найдено записей: 1", driver.FindElement(By.CssSelector("[grid-unique-id=\"gridTaxes\"]")).FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");

            VerifyFinally(testname);
        }
        
        [Test]
        public void SearchNotExist()
        {
            testname = "TaxesSearchNotExist";
            VerifyBegin(testname);

            GoToAdmin("settingscheckout#?checkoutTab=taxes");
            
            GetGridIdFilter("gridTaxes", "Tax Test 123");
            driver.FindElement(By.CssSelector(".tab-pane.active")).FindElement(By.TagName("h1")).Click();
            WaitForAjax();
            Thread.Sleep(2000);

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "search not exist tax");
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector("[grid-unique-id=\"gridTaxes\"]")).FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");

            VerifyFinally(testname);
        }

        [Test]
        public void SearchMuchSymbols()
        {
            testname = "TaxesSearchMuchSymbols";
            VerifyBegin(testname);

            GoToAdmin("settingscheckout#?checkoutTab=taxes");
            
            GetGridIdFilter("gridTaxes", "1111111111222222222222222222333333333333334444444444rrrrrrrrrrrreeeeeeeeewwwwwwwwwwwwwwwwwww");
            driver.FindElement(By.CssSelector(".tab-pane.active")).FindElement(By.TagName("h1")).Click();
            WaitForAjax();
            Thread.Sleep(2000);

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "search too much symbols");
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector("[grid-unique-id=\"gridTaxes\"]")).FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");

            VerifyFinally(testname);
        }

        [Test]
        public void SearchInvalidSymbols()
        {
            testname = "TaxesSearchInvalidSymbols";
            VerifyBegin(testname);

            GoToAdmin("settingscheckout#?checkoutTab=taxes");
            
            GetGridIdFilter("gridTaxes", "########@@@@@@@@&&&&&&&******,,,,..");
            driver.FindElement(By.CssSelector(".tab-pane.active")).FindElement(By.TagName("h1")).Click();
            WaitForAjax();
            Thread.Sleep(2000);

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "search invalid symbols");
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector("[grid-unique-id=\"gridTaxes\"]")).FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");

            VerifyFinally(testname);
        }
    }
}
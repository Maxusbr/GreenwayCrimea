using System;
using NUnit.Framework;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;
using System.Threading;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.SeleniumTest.Admin.Settings.SettingsCheckout.Taxes
{
    [TestFixture]
    public class SettingsTaxesFilterEnabledTest : BaseMultiSeleniumTest
    {

        [SetUp]
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
        public void FilterEnabled()
        {
            testname = "SettingsTaxesFilterEnabled";
            VerifyBegin(testname);
            if (!GetGridCell(0, "IsDefault", "Taxes").FindElement(By.TagName("input")).Selected)
            {
                GetGridCell(0, "IsDefault", "Taxes").Click();
                Thread.Sleep(1000);
            }
            //check filter disabled
            Functions.GridFilterTabSet(driver, baseURL, name: "Enabled", gridId: "gridTaxes");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Неактивные");
            driver.FindElement(By.CssSelector(".tab-pane.active")).FindElement(By.TagName("h1")).Click();
            WaitForAjax();
            VerifyAreEqual("Найдено записей: 65", driver.FindElement(By.CssSelector("[grid-unique-id=\"gridTaxes\"]")).FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter disabled");

            VerifyAreEqual("Tax 100", GetGridCell(0, "Name", "Taxes").Text, "Name filter disabled 1");
            VerifyAreEqual("Tax 44", GetGridCell(9, "Name", "Taxes").Text, "Name filter disabled 10");

            VerifyIsFalse(GetGridCell(0, "Enabled", "Taxes").FindElement(By.TagName("input")).Selected, "select filter disabled 1");
            VerifyIsFalse(GetGridCell(9, "Enabled", "Taxes").FindElement(By.TagName("input")).Selected, "select filter disabled 10");

            //check filter enabled
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Активные");
            driver.FindElement(By.CssSelector(".tab-pane.active")).FindElement(By.TagName("h1")).Click();
            WaitForAjax();
            VerifyAreEqual("Найдено записей: 42", driver.FindElement(By.CssSelector("[grid-unique-id=\"gridTaxes\"]")).FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter enabled");

            VerifyAreEqual("Tax 1", GetGridCell(0, "Name", "Taxes").Text, "Name filter enabled 1");
            VerifyAreEqual("Tax 18", GetGridCell(9, "Name", "Taxes").Text, "Name filter enabled 10");

            VerifyIsTrue(GetGridCell(0, "Enabled", "Taxes").FindElement(By.TagName("input")).Selected, "select filter enabled 1");
            VerifyIsTrue(GetGridCell(9, "Enabled", "Taxes").FindElement(By.TagName("input")).Selected, "select filter enabled 10");

            //check delete with filter
           
            driver.FindElement(By.CssSelector("[grid-unique-id=\"gridTaxes\"]")).FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(500);
            driver.FindElement(By.CssSelector("[grid-unique-id=\"gridTaxes\"]")).FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Thread.Sleep(500);
            Functions.GridDropdownTabDelete(driver, baseURL, gridId: "gridTaxes");
            ScrollTo(By.Id("header-top"));
            VerifyAreEqual("Tax 1", GetGridCell(0, "Name", "Taxes").Text, "delete all except default");
            VerifyAreEqual("Найдено записей: 1", driver.FindElement(By.CssSelector("[grid-unique-id=\"gridTaxes\"]")).FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all after deleting");

            //check delete filter
            ScrollTo(By.Id("header-top"));
            Functions.GridFilterClose(driver, baseURL, "Enabled");
            WaitForAjax();
            VerifyAreEqual("Найдено записей: 66", driver.FindElement(By.CssSelector("[grid-unique-id=\"gridTaxes\"]")).FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter enabled deleting");

            GoToAdmin("settingscheckout#?checkoutTab=taxes");
            VerifyAreEqual("Найдено записей: 66", driver.FindElement(By.CssSelector("[grid-unique-id=\"gridTaxes\"]")).FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter enabled deleting after refreshing");

            VerifyFinally(testname);
        }
    }
}
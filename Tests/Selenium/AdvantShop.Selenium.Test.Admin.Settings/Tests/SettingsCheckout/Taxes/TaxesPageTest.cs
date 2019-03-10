using System;
using NUnit.Framework;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;
using System.Threading;

namespace AdvantShop.SeleniumTest.Admin.Settings.SettingsCheckout.Taxes
{
    [TestFixture]
    public class SettingsTaxesPageTest : BaseSeleniumTest
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
        public void Page()
        {
            testname = "SettingsTaxesPage";
            VerifyBegin(testname);

            VerifyAreEqual("Tax 1", GetGridCell(0, "Name", "Taxes").Text, "page 1 line 1");
            VerifyAreEqual("Tax 107", GetGridCell(9, "Name", "Taxes").Text, "page 1 line 10");

            ScrollToElements(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector("[grid-unique-id=\"gridTaxes\"]")).FindElement(By.CssSelector(".pagination-page:nth-child(4) a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Tax 11", GetGridCell(0, "Name", "Taxes").Text, "page 2 line 1");
            VerifyAreEqual("Tax 2", GetGridCell(9, "Name", "Taxes").Text, "page 2 line 10");

            ScrollToElements(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector("[grid-unique-id=\"gridTaxes\"]")).FindElement(By.CssSelector(".pagination-page:nth-child(5) a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Tax 20", GetGridCell(0, "Name", "Taxes").Text, "page 3 line 1");
            VerifyAreEqual("Tax 29", GetGridCell(9, "Name", "Taxes").Text, "page 3 line 10");

            ScrollToElements(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector("[grid-unique-id=\"gridTaxes\"]")).FindElement(By.CssSelector(".pagination-page:nth-child(6) a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Tax 3", GetGridCell(0, "Name", "Taxes").Text, "page 4 line 1");
            VerifyAreEqual("Tax 38", GetGridCell(9, "Name", "Taxes").Text, "page 4 line 10");

            ScrollToElements(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector("[grid-unique-id=\"gridTaxes\"]")).FindElement(By.CssSelector(".pagination-page:nth-child(7) a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Tax 39", GetGridCell(0, "Name", "Taxes").Text, "page 5 line 1");
            VerifyAreEqual("Tax 47", GetGridCell(9, "Name", "Taxes").Text, "page 5 line 10");

            ScrollToElements(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector("[grid-unique-id=\"gridTaxes\"]")).FindElement(By.CssSelector(".pagination-page:nth-child(7) a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Tax 48", GetGridCell(0, "Name", "Taxes").Text, "page 6 line 1");
            VerifyAreEqual("Tax 56", GetGridCell(9, "Name", "Taxes").Text, "page 6 line 10");

            //to begin
            ScrollToElements(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector("[grid-unique-id=\"gridTaxes\"]")).FindElement(By.CssSelector(".pagination-first a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Tax 1", GetGridCell(0, "Name", "Taxes").Text, "page 1 line 1");
            VerifyAreEqual("Tax 107", GetGridCell(9, "Name", "Taxes").Text, "page 1 line 10");

            VerifyFinally(testname);
        }

        [Test]
        public void PageToPrevious()
        {
            testname = "SettingsTaxesPageToPrevious";
            VerifyBegin(testname);

            VerifyAreEqual("Tax 1", GetGridCell(0, "Name", "Taxes").Text, "page 1 line 1");
            VerifyAreEqual("Tax 107", GetGridCell(9, "Name", "Taxes").Text, "page 1 line 10");

            ScrollToElements(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector("[grid-unique-id=\"gridTaxes\"]")).FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Tax 11", GetGridCell(0, "Name", "Taxes").Text, "page 2 line 1");
            VerifyAreEqual("Tax 2", GetGridCell(9, "Name", "Taxes").Text, "page 2 line 10");

            ScrollToElements(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector("[grid-unique-id=\"gridTaxes\"]")).FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Tax 20", GetGridCell(0, "Name", "Taxes").Text, "page 3 line 1");
            VerifyAreEqual("Tax 29", GetGridCell(9, "Name", "Taxes").Text, "page 3 line 10");

            ScrollToElements(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector("[grid-unique-id=\"gridTaxes\"]")).FindElement(By.CssSelector(".pagination-prev a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Tax 11", GetGridCell(0, "Name", "Taxes").Text, "page 2 line 1");
            VerifyAreEqual("Tax 2", GetGridCell(9, "Name", "Taxes").Text, "page 2 line 10");

            ScrollToElements(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector("[grid-unique-id=\"gridTaxes\"]")).FindElement(By.CssSelector(".pagination-prev a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Tax 1", GetGridCell(0, "Name", "Taxes").Text, "page 1 line 1");
            VerifyAreEqual("Tax 107", GetGridCell(9, "Name", "Taxes").Text, "page 1 line 10");

            //to end
            ScrollToElements(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector("[grid-unique-id=\"gridTaxes\"]")).FindElement(By.CssSelector(".pagination-last a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Tax 93", GetGridCell(0, "Name", "Taxes").Text, "last page line 1");
            VerifyAreEqual("Tax 99", GetGridCell(6, "Name", "Taxes").Text, "last page line 7");

            VerifyFinally(testname);
        }
    }
}
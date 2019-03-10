using System;
using NUnit.Framework;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Admin.Settings.SettingsCheckout.Taxes
{
    [TestFixture]
    public class SettingsTaxesPresentTest : BaseSeleniumTest
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
        public void Present10()
        {
            testname = "TaxesPresent10";
            VerifyBegin(testname);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            PageSelectItemsTab("10", "gridTaxes");
            VerifyAreEqual("Tax 1", GetGridCell(0, "Name", "Taxes").Text, "line 1");
            VerifyAreEqual("Tax 107", GetGridCell(9, "Name", "Taxes").Text, "line 10");

            VerifyFinally(testname);
        }

        [Test]
        public void Present20()
        {
            testname = "TaxesPresent20";
            VerifyBegin(testname);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            PageSelectItemsTab("20", "gridTaxes");
            VerifyAreEqual("Tax 1", GetGridCell(0, "Name", "Taxes").Text, "line 1");
            VerifyAreEqual("Tax 2", GetGridCell(19, "Name", "Taxes").Text, "line 20");

            VerifyFinally(testname);
        }

        [Test]
        public void Present50()
        {
            testname = "TaxesPresent50";
            VerifyBegin(testname);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            PageSelectItemsTab("50", "gridTaxes");
            VerifyAreEqual("Tax 1", GetGridCell(0, "Name", "Taxes").Text, "line 1");
            VerifyAreEqual("Tax 47", GetGridCell(49, "Name", "Taxes").Text, "line 50");

            VerifyFinally(testname);
        }

        [Test]
        public void Present100()
        {
            testname = "TaxesPresent100";
            VerifyBegin(testname);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            PageSelectItemsTab("100", "gridTaxes");
            VerifyAreEqual("Tax 1", GetGridCell(0, "Name", "Taxes").Text, "line 1");
            VerifyAreEqual("Tax 92", GetGridCell(99, "Name", "Taxes").Text, "line 100");

            VerifyFinally(testname);
        }
    }
}
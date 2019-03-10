using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Admin.Settings.SettingsCheckout.Taxes
{
    [TestFixture]
    public class SettingsTaxesTest : BaseSeleniumTest
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
        public void GoToEditByName()
        {
            testname = "TaxesGoToEditByName";
            VerifyBegin(testname);

            GetGridCell(0, "Name", "Taxes").FindElement(By.TagName("span")).Click();
            WaitForElem(By.CssSelector(".modal-header"));

            VerifyAreEqual("Редактирование налога", driver.FindElement(By.TagName("h2")).Text, "open pop edit up");

            driver.FindElement(By.XPath("//button[contains(text(), 'Отмена')]")).Click();

            VerifyFinally(testname);
        }


        [Test]
        public void GoToEditByServiceCol()
        {
            testname = "TaxesGoToEditByServiceCol";
            VerifyBegin(testname);

            GetGridCell(0, "_serviceColumn", "Taxes").FindElement(By.CssSelector(".link-invert.ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            WaitForElem(By.CssSelector(".modal-header"));

            VerifyAreEqual("Редактирование налога", driver.FindElement(By.TagName("h2")).Text, "open pop edit up");

            driver.FindElement(By.XPath("//button[contains(text(), 'Отмена')]")).Click();

            VerifyFinally(testname);
        }
        
        [Test]
        public void InplaceEnabled()
        {
            testname = "TaxesInplaceEnabled";
            VerifyBegin(testname);

            VerifyIsTrue(GetGridCell(1, "Enabled", "Taxes").FindElement(By.TagName("input")).Selected, "pre check Enabled");

            GetGridCell(1, "Enabled", "Taxes").Click();
            VerifyIsFalse(GetGridCell(1, "Enabled", "Taxes").FindElement(By.TagName("input")).Selected, "Enabled inplace");
            
            Refresh();

            VerifyIsFalse(GetGridCell(1, "Enabled", "Taxes").FindElement(By.TagName("input")).Selected, "Enabled inplace after refreshing");

            VerifyFinally(testname);
        }

        [Test]
        public void InplaceIsDefault()
        {
            testname = "TaxesInplaceIsDefault";
            VerifyBegin(testname);

            VerifyIsFalse(GetGridCell(2, "IsDefault", "Taxes").FindElement(By.TagName("input")).Selected, "pre check IsDefault no");

            GetGridCell(2, "IsDefault", "Taxes").Click();
            //VerifyIsTrue(GetGridCell(2, "IsDefault", "Taxes").FindElement(By.TagName("input")).Selected, "IsDefault inplace");
            
            Refresh();

            VerifyIsTrue(GetGridCell(2, "IsDefault", "Taxes").FindElement(By.TagName("input")).Selected, "IsDefault inplace after refreshing");
            VerifyIsFalse(GetGridCell(0, "IsDefault", "Taxes").FindElement(By.TagName("input")).Selected, "prev IsDefault no");

            //back default
            GetGridCell(0, "IsDefault", "Taxes").Click();
            Refresh();

            VerifyFinally(testname);
        }

        [Test]
        public void zSelectDelete()
        {
            testname = "TaxesSelectDelete";
            VerifyBegin(testname);

            //check delete cancel 
            GetGridCell(1, "_serviceColumn", "Taxes").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-times.link-invert")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-cancel")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Tax 10", GetGridCell(1, "Name", "Taxes").Text, "1 grid canсel delete");

            //check delete
            GetGridCell(1, "_serviceColumn", "Taxes").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-times.link-invert")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Tax 100", GetGridCell(1, "Name", "Taxes").Text, "1 grid delete");

            //check select 
            GetGridCell(1, "selectionRowHeaderCol", "Taxes").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            GetGridCell(2, "selectionRowHeaderCol", "Taxes").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            GetGridCell(3, "selectionRowHeaderCol", "Taxes").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            VerifyIsTrue(GetGridCell(1, "selectionRowHeaderCol", "Taxes").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected 2 grid"); //1 default
            VerifyIsTrue(GetGridCell(2, "selectionRowHeaderCol", "Taxes").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected 3 grid");
            VerifyIsTrue(GetGridCell(3, "selectionRowHeaderCol", "Taxes").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected 4 grid");
            VerifyAreEqual("3", driver.FindElement(By.CssSelector("[grid-unique-id=\"gridTaxes\"]")).FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text, "count selected");

            //check delete selected items
            Functions.GridDropdownTabDelete(driver, baseURL, gridId: "gridTaxes");
            VerifyAreEqual("Tax 103", GetGridCell(1, "Name", "Taxes").Text, "selected 2 grid delete");
            VerifyAreEqual("Tax 104", GetGridCell(2, "Name", "Taxes").Text, "selected 3 grid delete");
            VerifyAreEqual("Tax 105", GetGridCell(3, "Name", "Taxes").Text, "selected 4 grid delete");

            //check select all on page
            driver.FindElement(By.CssSelector("[grid-unique-id=\"gridTaxes\"]")).FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(1000);
            VerifyIsTrue(GetGridCell(0, "selectionRowHeaderCol", "Taxes").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected all on page 1 grid");
            VerifyIsTrue(GetGridCell(9, "selectionRowHeaderCol", "Taxes").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected all on page 10 grid");

            //check delete all on page
            Functions.GridDropdownTabDelete(driver, baseURL, gridId: "gridTaxes");
            VerifyAreEqual("Tax 15", GetGridCell(1, "Name", "Taxes").Text, "selected all on page 2 grid delete");
            VerifyAreEqual("Tax 22", GetGridCell(9, "Name", "Taxes").Text, "selected all on page 10 grid delete");
            VerifyAreEqual("Tax 1", GetGridCell(0, "Name", "Taxes").Text, "delete all on page except default");

            //check select all
            driver.FindElement(By.CssSelector("[grid-unique-id=\"gridTaxes\"]")).FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[grid-unique-id=\"gridTaxes\"]")).FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Thread.Sleep(1000);
            VerifyAreEqual("94", driver.FindElement(By.CssSelector("[grid-unique-id=\"gridTaxes\"]")).FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text, "count all selected after deleting");

            //check deselect all 
            driver.FindElement(By.CssSelector("[grid-unique-id=\"gridTaxes\"]")).FindElement(By.CssSelector("[data-e2e=\"gridSelectionDeselectAllFn\"]")).Click();
            Thread.Sleep(1000);
            VerifyIsTrue(!GetGridCell(0, "selectionRowHeaderCol", "Taxes").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "deselect all 1 grid");
            VerifyIsTrue(!GetGridCell(9, "selectionRowHeaderCol", "Taxes").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "deselect all 10 grid");

            //check delete all
            driver.FindElement(By.CssSelector("[grid-unique-id=\"gridTaxes\"]")).FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[grid-unique-id=\"gridTaxes\"]")).FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Functions.GridDropdownTabDelete(driver, baseURL, gridId: "gridTaxes");

            VerifyAreEqual("Tax 1", GetGridCell(0, "Name", "Taxes").Text, "delete all except default");
            VerifyAreEqual("Найдено записей: 1", driver.FindElement(By.CssSelector("[grid-unique-id=\"gridTaxes\"]")).FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all after deleting");

            GoToAdmin("settingscheckout#?checkoutTab=taxes");
            VerifyAreEqual("Tax 1", GetGridCell(0, "Name", "Taxes").Text, "delete all except default after refreshing");
            VerifyAreEqual("Найдено записей: 1", driver.FindElement(By.CssSelector("[grid-unique-id=\"gridTaxes\"]")).FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all after deleting after refreshing");

            VerifyFinally(testname);
        }
    }
}
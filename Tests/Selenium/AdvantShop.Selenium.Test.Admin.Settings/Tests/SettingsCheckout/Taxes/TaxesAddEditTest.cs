using System;
using NUnit.Framework;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;
using System.Threading;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.SeleniumTest.Admin.Settings.SettingsCheckout.Taxes.AddEdit
{
    [TestFixture]
    public class SettingsTaxesAddEditTest : BaseSeleniumTest
    {

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Taxes);
            InitializeService.LoadData(

           "data\\Admin\\Settings\\Taxes\\Settings.Settings.csv",
            "data\\Admin\\Settings\\Taxes\\Catalog.Tax.csv"

           );

            Init();
        }

        [Test]
        public void aTaxesGrid()
        {
            testname = "TaxesGrid";
            VerifyBegin(testname);
            
            GoToAdmin("settingscheckout#?checkoutTab=taxes");

            VerifyAreEqual("Налоги", driver.FindElement(By.CssSelector(".tab-pane.active")).FindElement(By.TagName("h1")).Text, "h1 taxes page");

            VerifyAreEqual("Tax 1", GetGridCell(0, "Name", "Taxes").Text, "Name");
            VerifyAreEqual("Без НДС", GetGridCell(0, "TaxTypeFormatted", "Taxes").Text, "Tax Type");
            VerifyAreEqual("0", GetGridCell(0, "Rate", "Taxes").Text, "Rate");

            VerifyIsTrue(GetGridCell(0, "Enabled", "Taxes").FindElement(By.TagName("input")).Selected, "Enabled");
            VerifyIsTrue(GetGridCell(0, "IsDefault", "Taxes").FindElement(By.TagName("input")).Selected, "IsDefault");
            VerifyIsFalse(GetGridCell(1, "IsDefault", "Taxes").FindElement(By.TagName("input")).Selected, "IsDefault no");

            VerifyAreEqual("Найдено записей: 107", driver.FindElement(By.CssSelector("[grid-unique-id=\"gridTaxes\"]")).FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");

            VerifyFinally(testname);
        }

        [Test]
        public void TaxAddEnabled()
        {
            testname = "TaxAddEnabled";
            VerifyBegin(testname);

            GoToAdmin("settingscheckout#?checkoutTab=taxes");

            driver.FindElement(By.CssSelector("[data-e2e=\"taxAdd\"]")).Click();
            Thread.Sleep(2000);
            WaitForElem(By.Name("addEditTaxForm"));

            VerifyAreEqual("Новый налог", driver.FindElement(By.Name("addEditTaxForm")).FindElement(By.TagName("h2")).Text, "pop up h2");

            driver.FindElement(By.CssSelector("[data-e2e=\"taxName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"taxName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"taxName\"]")).SendKeys("Tax Added Name");

            driver.FindElement(By.CssSelector("[data-e2e=\"taxEnabled\"]")).FindElement(By.TagName("span")).Click();

            driver.FindElement(By.CssSelector("[data-e2e=\"taxRate\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"taxRate\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"taxRate\"]")).SendKeys("10");

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"taxType\"]")))).SelectByText("НДС по ставке 10%");

            driver.FindElement(By.CssSelector("[data-e2e=\"taxButtonSave\"]")).Click();
            Thread.Sleep(2000);

            //check admin grid
            GoToAdmin("settingscheckout#?checkoutTab=taxes");

            GetGridIdFilter("gridTaxes", "Tax Added Name");
            driver.FindElement(By.CssSelector(".tab-pane.active")).FindElement(By.TagName("h1")).Click();
            WaitForAjax();

            VerifyAreEqual("Tax Added Name", GetGridCell(0, "Name", "Taxes").Text, "tax Name added");
            VerifyAreEqual("НДС по ставке 10%", GetGridCell(0, "TaxTypeFormatted", "Taxes").Text, "Tax Type added");
            VerifyAreEqual("10", GetGridCell(0, "Rate", "Taxes").Text, "Tax Rate added");

            VerifyIsTrue(GetGridCell(0, "Enabled", "Taxes").FindElement(By.TagName("input")).Selected, "tax Enabled added");
            VerifyIsFalse(GetGridCell(0, "IsDefault", "Taxes").FindElement(By.TagName("input")).Selected, "tax IsDefault added");

            //check admin pop up
            GetGridCell(0, "_serviceColumn", "Taxes").FindElement(By.CssSelector(".link-invert.ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            WaitForElem(By.Name("addEditTaxForm"));

            VerifyAreEqual("Tax Added Name", driver.FindElement(By.CssSelector("[data-e2e=\"taxName\"]")).GetAttribute("value"), "tax Name added pop up");
            VerifyAreEqual("10", driver.FindElement(By.CssSelector("[data-e2e=\"taxRate\"]")).GetAttribute("value"), "tax Rate added pop up");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"taxEnabled\"]")).FindElement(By.TagName("input")).Selected, "tax Rate added pop up");
            VerifyIsFalse(driver.FindElement(By.CssSelector("[data-e2e=\"taxIsDefault\"]")).FindElement(By.TagName("input")).Selected, "tax IsDefault added pop up");

            IWebElement selectElem = driver.FindElement(By.CssSelector("[data-e2e=\"taxType\"]"));
            SelectElement select = new SelectElement(selectElem);
            VerifyIsTrue(select.SelectedOption.Text.Contains("НДС по ставке 10%"), "tax Type pop up");

            //check admin tax added to product
            GoToAdmin("product/edit/348");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Цена и наличие')]")).Click();
            Thread.Sleep(1000);

            IWebElement selectElemProd = driver.FindElement(By.Id("TaxId"));
            SelectElement select2 = new SelectElement(selectElemProd);
            int lastElem = select2.Options.Count - 1;
            VerifyIsTrue(select2.Options[lastElem].Text.Contains("Tax Added Name"), "tax added to product");

            VerifyFinally(testname);
        }

        [Test]
        public void TaxAddDisabled()
        {
            testname = "TaxAddDisabled";
            VerifyBegin(testname);
            
            GoToAdmin("settingscheckout#?checkoutTab=taxes");

            driver.FindElement(By.CssSelector("[data-e2e=\"taxAdd\"]")).Click();
            Thread.Sleep(2000);
            WaitForElem(By.Name("addEditTaxForm"));
            
            driver.FindElement(By.CssSelector("[data-e2e=\"taxName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"taxName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"taxName\"]")).SendKeys("Tax Added Name Disabled");
            
            driver.FindElement(By.CssSelector("[data-e2e=\"taxRate\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"taxRate\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"taxRate\"]")).SendKeys("14");

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"taxType\"]")))).SelectByText("Другой");

            driver.FindElement(By.CssSelector("[data-e2e=\"taxButtonSave\"]")).Click();
            Thread.Sleep(2000);

            //check admin grid
            GoToAdmin("settingscheckout#?checkoutTab=taxes");

            VerifyAreEqual("Найдено записей: 108", driver.FindElement(By.CssSelector("[grid-unique-id=\"gridTaxes\"]")).FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all with added");

            GetGridIdFilter("gridTaxes", "Tax Added Name Disabled");
            driver.FindElement(By.CssSelector(".tab-pane.active")).FindElement(By.TagName("h1")).Click();
            WaitForAjax();

            VerifyAreEqual("Tax Added Name Disabled", GetGridCell(0, "Name", "Taxes").Text, "tax Name added");
            VerifyAreEqual("Другой", GetGridCell(0, "TaxTypeFormatted", "Taxes").Text, "Tax Type added");
            VerifyAreEqual("14", GetGridCell(0, "Rate", "Taxes").Text, "Tax Rate added");

            VerifyIsFalse(GetGridCell(0, "Enabled", "Taxes").FindElement(By.TagName("input")).Selected, "tax Enabled added");
            VerifyIsFalse(GetGridCell(0, "IsDefault", "Taxes").FindElement(By.TagName("input")).Selected, "tax IsDefault added");

            //check admin pop up
            GetGridCell(0, "_serviceColumn", "Taxes").FindElement(By.CssSelector(".link-invert.ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            WaitForElem(By.Name("addEditTaxForm"));

            VerifyAreEqual("Tax Added Name Disabled", driver.FindElement(By.CssSelector("[data-e2e=\"taxName\"]")).GetAttribute("value"), "tax Name added pop up");
            VerifyAreEqual("14", driver.FindElement(By.CssSelector("[data-e2e=\"taxRate\"]")).GetAttribute("value"), "tax Rate added pop up");
            VerifyIsFalse(driver.FindElement(By.CssSelector("[data-e2e=\"taxEnabled\"]")).FindElement(By.TagName("input")).Selected, "tax Rate added pop up");
            VerifyIsFalse(driver.FindElement(By.CssSelector("[data-e2e=\"taxIsDefault\"]")).FindElement(By.TagName("input")).Selected, "tax IsDefault added pop up");

            IWebElement selectElem = driver.FindElement(By.CssSelector("[data-e2e=\"taxType\"]"));
            SelectElement select = new SelectElement(selectElem);
            VerifyIsTrue(select.SelectedOption.Text.Contains("Другой"), "tax Type pop up");

            //check admin tax added in product hidden
            GoToAdmin("product/edit/346");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Цена и наличие')]")).Click();
            Thread.Sleep(1000);

            IWebElement selectElemProd = driver.FindElement(By.Id("TaxId"));
            SelectElement select2 = new SelectElement(selectElemProd);
            VerifyIsTrue(select2.Options.Count == 42, "disabled tax added to product hidden"); //enabled taxes only

            VerifyFinally(testname);
        }
        
        [Test]
        public void TaxEdit()
        {
            testname = "TaxEdit";
            VerifyBegin(testname);

            //pre check admin tax count all
            GoToAdmin("product/edit/35");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Цена и наличие')]")).Click();
            Thread.Sleep(1000);

            IWebElement selectProdBegin = driver.FindElement(By.Id("TaxId"));
            SelectElement select = new SelectElement(selectProdBegin);
            int allTaxOptionsBegin = select.Options.Count;

            //test
            GoToAdmin("settingscheckout#?checkoutTab=taxes");

            GetGridIdFilter("gridTaxes", "Tax 40");
            driver.FindElement(By.CssSelector(".tab-pane.active")).FindElement(By.TagName("h1")).Click();
            WaitForAjax();
            GetGridCell(0, "_serviceColumn", "Taxes").FindElement(By.CssSelector(".link-invert.ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(2000);
            WaitForElem(By.Name("addEditTaxForm"));

            //pre check admin pop up
            VerifyAreEqual("Tax 40", driver.FindElement(By.CssSelector("[data-e2e=\"taxName\"]")).GetAttribute("value"), "pre check tax Name added pop up");
            VerifyAreEqual("39", driver.FindElement(By.CssSelector("[data-e2e=\"taxRate\"]")).GetAttribute("value"), "pre check tax Rate added pop up");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"taxEnabled\"]")).FindElement(By.TagName("input")).Selected, "pre check tax Rate added pop up");
            VerifyIsFalse(driver.FindElement(By.CssSelector("[data-e2e=\"taxIsDefault\"]")).FindElement(By.TagName("input")).Selected, "pre check tax IsDefault added pop up");

            IWebElement ElemPopUpBegin = driver.FindElement(By.CssSelector("[data-e2e=\"taxType\"]"));
            SelectElement select1 = new SelectElement(ElemPopUpBegin);
            VerifyIsTrue(select1.SelectedOption.Text.Contains("Другой"), "pre check tax Type pop up");

            driver.FindElement(By.CssSelector("[data-e2e=\"taxName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"taxName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"taxName\"]")).SendKeys("Edited Name");

            driver.FindElement(By.CssSelector("[data-e2e=\"taxEnabled\"]")).FindElement(By.TagName("span")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"taxIsDefault\"]")).FindElement(By.TagName("span")).Click();

            driver.FindElement(By.CssSelector("[data-e2e=\"taxRate\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"taxRate\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"taxRate\"]")).SendKeys("111");

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"taxType\"]")))).SelectByText("НДС по ставке 18%");

            driver.FindElement(By.CssSelector("[data-e2e=\"taxButtonSave\"]")).Click();
            Thread.Sleep(2000);

            //check admin prev name
            GoToAdmin("settingscheckout#?checkoutTab=taxes");

            GetGridIdFilter("gridTaxes", "Tax 40");
            driver.FindElement(By.CssSelector(".tab-pane.active")).FindElement(By.TagName("h1")).Click();
            WaitForAjax();
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "prev tax name");
            
            //check admin grid
            GetGridIdFilter("gridTaxes", "Edited Name");
            driver.FindElement(By.CssSelector(".tab-pane.active")).FindElement(By.TagName("h1")).Click();
            WaitForAjax();

            VerifyAreEqual("Edited Name", GetGridCell(0, "Name", "Taxes").Text, "tax Name edited");
            VerifyAreEqual("НДС по ставке 18%", GetGridCell(0, "TaxTypeFormatted", "Taxes").Text, "Tax Type edited");
            VerifyAreEqual("111", GetGridCell(0, "Rate", "Taxes").Text, "Tax Rate edited");

            VerifyIsFalse(GetGridCell(0, "Enabled", "Taxes").FindElement(By.TagName("input")).Selected, "tax Enabled edited");
            VerifyIsTrue(GetGridCell(0, "IsDefault", "Taxes").FindElement(By.TagName("input")).Selected, "tax IsDefault edited");

            //check admin pop up
            GetGridCell(0, "_serviceColumn", "Taxes").FindElement(By.CssSelector(".link-invert.ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            WaitForElem(By.Name("addEditTaxForm"));

            VerifyAreEqual("Edited Name", driver.FindElement(By.CssSelector("[data-e2e=\"taxName\"]")).GetAttribute("value"), "tax Name edited pop up");
            VerifyAreEqual("111", driver.FindElement(By.CssSelector("[data-e2e=\"taxRate\"]")).GetAttribute("value"), "tax Rate edited pop up");
            VerifyIsFalse(driver.FindElement(By.CssSelector("[data-e2e=\"taxEnabled\"]")).FindElement(By.TagName("input")).Selected, "tax Rate edited pop up");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"taxIsDefault\"]")).FindElement(By.TagName("input")).Selected, "tax IsDefault edited pop up");

            IWebElement ElemPopUpEnd = driver.FindElement(By.CssSelector("[data-e2e=\"taxType\"]"));
            SelectElement select2 = new SelectElement(ElemPopUpEnd);
            VerifyIsTrue(select2.SelectedOption.Text.Contains("НДС по ставке 18%"), "tax Type pop up");

            //check admin tax added to product
            GoToAdmin("product/edit/35");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Цена и наличие')]")).Click();
            Thread.Sleep(1000);

            IWebElement selectProdEnd = driver.FindElement(By.Id("TaxId"));
            SelectElement select3 = new SelectElement(selectProdEnd);
            VerifyIsFalse(select3.SelectedOption.Text.Contains("Edited Name"), "new tax default edited");

            int allTaxOptionsEnd = select3.Options.Count;
            int allTaxOptionsEndCount = allTaxOptionsBegin - 1;
            VerifyIsTrue(allTaxOptionsEndCount == allTaxOptionsEnd, "tax default edited count all");

            VerifyFinally(testname);
        }


        [Test]
        public void zAddDefault()
        {
            testname = "TaxAddDefault";
            VerifyBegin(testname);

            //pre check admin default tax and count all
            GoToAdmin("product/edit/37");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Цена и наличие')]")).Click();
            Thread.Sleep(1000);

            IWebElement selectProdBegin = driver.FindElement(By.Id("TaxId"));
            SelectElement select = new SelectElement(selectProdBegin);
            int allTaxOptionsBegin = select.Options.Count;

            VerifyIsTrue(select.SelectedOption.Text.Contains("Tax 1"), "pre check tax default");

            //test
            GoToAdmin("settingscheckout#?checkoutTab=taxes");

            driver.FindElement(By.CssSelector("[data-e2e=\"taxAdd\"]")).Click();
            Thread.Sleep(2000);
            WaitForElem(By.Name("addEditTaxForm"));

            driver.FindElement(By.CssSelector("[data-e2e=\"taxName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"taxName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"taxName\"]")).SendKeys("Tax Added Default Name");

            driver.FindElement(By.CssSelector("[data-e2e=\"taxEnabled\"]")).FindElement(By.TagName("span")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"taxIsDefault\"]")).FindElement(By.TagName("span")).Click();

            driver.FindElement(By.CssSelector("[data-e2e=\"taxRate\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"taxRate\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"taxRate\"]")).SendKeys("26");

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"taxType\"]")))).SelectByText("НДС по ставке 18%");

            driver.FindElement(By.CssSelector("[data-e2e=\"taxButtonSave\"]")).Click();
            Thread.Sleep(2000);

            //check admin prev default
            GoToAdmin("settingscheckout#?checkoutTab=taxes");

            GetGridIdFilter("gridTaxes", "Tax 1");
            driver.FindElement(By.CssSelector(".tab-pane.active")).FindElement(By.TagName("h1")).Click();
            WaitForAjax();

            VerifyAreEqual("Tax 1", GetGridCell(0, "Name", "Taxes").Text, "tax Name prev default");
            VerifyIsFalse(GetGridCell(0, "IsDefault", "Taxes").FindElement(By.TagName("input")).Selected, "tax IsDefault prev");

            //check admin grid
            GetGridIdFilter("gridTaxes", "Tax Added Default Name");
            driver.FindElement(By.CssSelector(".tab-pane.active")).FindElement(By.TagName("h1")).Click();
            WaitForAjax();

            VerifyAreEqual("Tax Added Default Name", GetGridCell(0, "Name", "Taxes").Text, "tax Name added");
            VerifyAreEqual("НДС по ставке 18%", GetGridCell(0, "TaxTypeFormatted", "Taxes").Text, "Tax Type added");
            VerifyAreEqual("26", GetGridCell(0, "Rate", "Taxes").Text, "Tax Rate added");

            VerifyIsTrue(GetGridCell(0, "Enabled", "Taxes").FindElement(By.TagName("input")).Selected, "tax Enabled added");
            VerifyIsTrue(GetGridCell(0, "IsDefault", "Taxes").FindElement(By.TagName("input")).Selected, "tax IsDefault added");

            //check admin pop up
            GetGridCell(0, "_serviceColumn", "Taxes").FindElement(By.CssSelector(".link-invert.ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            WaitForElem(By.Name("addEditTaxForm"));

            VerifyAreEqual("Tax Added Default Name", driver.FindElement(By.CssSelector("[data-e2e=\"taxName\"]")).GetAttribute("value"), "tax Name added pop up");
            VerifyAreEqual("26", driver.FindElement(By.CssSelector("[data-e2e=\"taxRate\"]")).GetAttribute("value"), "tax Rate added pop up");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"taxEnabled\"]")).FindElement(By.TagName("input")).Selected, "tax Rate added pop up");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"taxIsDefault\"]")).FindElement(By.TagName("input")).Selected, "tax IsDefault added pop up");

            IWebElement selectElem = driver.FindElement(By.CssSelector("[data-e2e=\"taxType\"]"));
            SelectElement select1 = new SelectElement(selectElem);
            VerifyIsTrue(select1.SelectedOption.Text.Contains("НДС по ставке 18%"), "tax Type pop up");

            //check admin tax added to product
            GoToAdmin("product/edit/37");

            //scroll
            driver.FindElement(By.XPath("//div[contains(text(), 'Цена и наличие')]")).Click();
            Thread.Sleep(1000);

            IWebElement selectProdEnd = driver.FindElement(By.Id("TaxId"));
            SelectElement select2 = new SelectElement(selectProdEnd);
            VerifyIsTrue(select2.SelectedOption.Text.Contains("Tax Added Default Name"), "new tax default added");

            int allTaxOptionsEnd = select2.Options.Count;
            int allTaxOptionsEndCount = allTaxOptionsBegin + 1;
            VerifyIsTrue(allTaxOptionsEndCount == allTaxOptionsEnd, "tax default added count all");

            VerifyFinally(testname);
        }

    }
}
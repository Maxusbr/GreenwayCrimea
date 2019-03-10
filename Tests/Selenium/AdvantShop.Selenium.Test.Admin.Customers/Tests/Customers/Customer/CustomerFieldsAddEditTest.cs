using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using AdvantShop.Selenium.Core.Infrastructure;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;

namespace AdvantShop.Web.Site.Selenium.Test.Admin.Customers.CustomerFields
{
    [TestFixture]
    public class CustomerFieldsAddEditTest : BaseSeleniumTest
    {

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Customers);
            InitializeService.LoadData(
            "data\\Admin\\Customers\\CustomerFields\\Customers.Customer.csv",
           "data\\Admin\\Customers\\CustomerFields\\Customers.CustomerGroup.csv",
               "data\\Admin\\Customers\\CustomerFields\\Customers.CustomerField.csv",
               "data\\Admin\\Customers\\CustomerFields\\Customers.CustomerFieldValue.csv",
               "data\\Admin\\Customers\\CustomerFields\\Customers.CustomerFieldValuesMap.csv"

           );

            Init();
        }
        
        [Test]
        public void CustomerFieldsAddSelectTest()
        {
            testname = "CustomerFieldsAddSelectTest";
            VerifyBegin(testname);

            GoToAdmin("settingscustomers#?tab=customerFields");

            GetButton(eButtonType.Add).Click();
            Thread.Sleep(1000);

            VerifyAreEqual("Новое поле", driver.FindElement(By.TagName("h2")).Text, "h2 add pop up");

            driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldName\"]")).SendKeys("New Customer Field Select");

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldType\"]")))).SelectByText("Выбор");

            driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldSortOrder\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldSortOrder\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldSortOrder\"]")).SendKeys("1");

            VerifyIsFalse(driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldRequired\"]")).FindElement(By.TagName("input")).Selected, "default not required");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldEnabled\"]")).FindElement(By.TagName("input")).Selected, "default enabled");
            VerifyIsFalse(driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldShowInClient\"]")).FindElement(By.TagName("input")).Selected, "default not ShowInClient");

            VerifyIsTrue(driver.FindElement(By.CssSelector(".modal-body")).Text.Contains("Значения поля"), "select type");

            driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldValue\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldValue\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldValue\"]")).SendKeys("Value Added 1");

            driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldAddValue\"]")).Click();

            driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldValue\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldValue\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldValue\"]")).SendKeys("Value Added 2");

            driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldAddValue\"]")).Click();

            GetButton(eButtonType.Save).Click();
            Thread.Sleep(500);

            //check admin settings
            GoToAdmin("settingscustomers#?tab=customerFields");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("New Customer Field Select");
            XPathContainsText("h1", "Дополнительные поля покупателя");

            VerifyAreEqual("New Customer Field Select", GetGridCell(0, "Name", "CustomerFields").Text, "customer field name");
            VerifyAreEqual("Выбор", GetGridCell(0, "FieldTypeFormatted", "CustomerFields").Text, "customer field type");
            VerifyAreEqual("Список значений", GetGridCell(0, "HasValues", "CustomerFields").Text, "customer field values");
            VerifyIsFalse(GetGridCell(0, "Required", "CustomerFields").FindElement(By.TagName("input")).Selected, "customer field Required");
            VerifyAreEqual("1", GetGridCell(0, "SortOrder", "CustomerFields").FindElement(By.TagName("input")).GetAttribute("value"), "customer field SortOrder");
            VerifyIsTrue(GetGridCell(0, "Enabled", "CustomerFields").FindElement(By.TagName("input")).Selected, "customer field enabled");
            VerifyIsFalse(GetGridCell(0, "ShowInClient", "CustomerFields").FindElement(By.TagName("input")).Selected, "customer field ShowInClient");

            //check admin edit customer
            GoToAdmin("customers/edit/cfc2c33b-1e84-415e-8482-e98156341601");

            VerifyIsTrue(driver.PageSource.Contains("New Customer Field Select"), "customer edit field name");

            IWebElement selectElemCustomerField = driver.FindElement(By.CssSelector("[validation-input-text=\"New Customer Field Select\"]"));
            SelectElement select = new SelectElement(selectElemCustomerField);

            IList<IWebElement> allOptionsCustomerField = select.Options;

            VerifyIsTrue(allOptionsCustomerField.Count == 3, "customer edit count field's values"); //2 customer field's values + null select
            
            VerifyFinally(testname);
        }

        [Test]
        public void CustomerFieldsAddTextDisabledTest()
        {
            testname = "CustomerFieldsAddTextDisabledTest";
            VerifyBegin(testname);

            GoToAdmin("settingscustomers#?tab=customerFields");

            GetButton(eButtonType.Add).Click();
            Thread.Sleep(1000);
            
            driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldName\"]")).SendKeys("Customer Field Added Text Disabled");

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldType\"]")))).SelectByText("Текстовое поле");

            driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldSortOrder\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldSortOrder\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldSortOrder\"]")).SendKeys("2");

            driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldRequired\"]")).FindElement(By.TagName("span")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldEnabled\"]")).FindElement(By.TagName("span")).Click();

            VerifyIsFalse(driver.FindElement(By.CssSelector(".modal-body")).Text.Contains("Значения поля"), "no select type");
            
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(500);

            //check admin settings
            GoToAdmin("settingscustomers#?tab=customerFields");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("Customer Field Added Text Disabled");
            XPathContainsText("h1", "Дополнительные поля покупателя");

            VerifyAreEqual("Customer Field Added Text Disabled", GetGridCell(0, "Name", "CustomerFields").Text, "customer field name");
            VerifyAreEqual("Текстовое поле", GetGridCell(0, "FieldTypeFormatted", "CustomerFields").Text, "customer field type");
            VerifyAreEqual("", GetGridCell(0, "HasValues", "CustomerFields").Text, "customer field values");
            VerifyIsTrue(GetGridCell(0, "Required", "CustomerFields").FindElement(By.TagName("input")).Selected, "customer field Required");
            VerifyAreEqual("2", GetGridCell(0, "SortOrder", "CustomerFields").FindElement(By.TagName("input")).GetAttribute("value"), "customer field SortOrder");
            VerifyIsFalse(GetGridCell(0, "Enabled", "CustomerFields").FindElement(By.TagName("input")).Selected, "customer field enabled");

            //check admin edit customer
            GoToAdmin("customers/edit/cfc2c33b-1e84-415e-8482-e98156341601");

            VerifyIsFalse(driver.PageSource.Contains("Customer Field Added Text Disabled"), "customer edit field name disabled");
            VerifyIsFalse(driver.FindElements(By.CssSelector("[validation-input-text=\"Customer Field Added Text Disabled\"]")).Count > 0, "customer edit field disabled");
   
            VerifyFinally(testname);
        }

        [Test]
        public void CustomerFieldsAddTextEnabledTest()
        {
            testname = "CustomerFieldsAddTextEnabledTest";
            VerifyBegin(testname);

            GoToAdmin("settingscustomers#?tab=customerFields");

            GetButton(eButtonType.Add).Click();
            Thread.Sleep(1000);

            driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldName\"]")).SendKeys("Customer Field Text Enabled");

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldType\"]")))).SelectByText("Текстовое поле");

            driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldSortOrder\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldSortOrder\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldSortOrder\"]")).SendKeys("3");

            driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldRequired\"]")).FindElement(By.TagName("span")).Click();

            driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldShowInClient\"]")).FindElement(By.TagName("span")).Click();

            VerifyIsFalse(driver.FindElement(By.CssSelector(".modal-body")).Text.Contains("Значения поля"), "no select type");

            GetButton(eButtonType.Save).Click();
            Thread.Sleep(500);

            //check admin settings
            GoToAdmin("settingscustomers#?tab=customerFields");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("Customer Field Text Enabled");
            XPathContainsText("h1", "Дополнительные поля покупателя");

            VerifyAreEqual("Customer Field Text Enabled", GetGridCell(0, "Name", "CustomerFields").Text, "customer field name");
            VerifyAreEqual("Текстовое поле", GetGridCell(0, "FieldTypeFormatted", "CustomerFields").Text, "customer field type");
            VerifyAreEqual("", GetGridCell(0, "HasValues", "CustomerFields").Text, "customer field values");
            VerifyIsTrue(GetGridCell(0, "Required", "CustomerFields").FindElement(By.TagName("input")).Selected, "customer field Required");
            VerifyAreEqual("3", GetGridCell(0, "SortOrder", "CustomerFields").FindElement(By.TagName("input")).GetAttribute("value"), "customer field SortOrder");
            VerifyIsTrue(GetGridCell(0, "Enabled", "CustomerFields").FindElement(By.TagName("input")).Selected, "customer field enabled");
            VerifyIsTrue(GetGridCell(0, "ShowInClient", "CustomerFields").FindElement(By.TagName("input")).Selected, "customer field show in client");

            //check admin edit customer
            GoToAdmin("customers/edit/cfc2c33b-1e84-415e-8482-e98156341601");

            VerifyIsTrue(driver.PageSource.Contains("Customer Field Text Enabled"), "customer edit field name enabled");
            VerifyIsTrue(driver.FindElements(By.CssSelector("[validation-input-text=\"Customer Field Text Enabled\"]")).Count > 0, "customer edit field enabled");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[validation-input-text=\"Customer Field Text Enabled\"]")).GetAttribute("ng-required").Equals("true"), "customer edit required");

            driver.FindElement(By.CssSelector("[validation-input-text=\"Customer Field Text Enabled\"]")).Click();
            driver.FindElement(By.CssSelector("[validation-input-text=\"Customer Field Text Enabled\"]")).SendKeys("test 1");
            DropFocus("h1");

            VerifyAreEqual("test 1", driver.FindElement(By.CssSelector("[validation-input-text=\"Customer Field Text Enabled\"]")).GetAttribute("value"), "customer edit field text");

            VerifyFinally(testname);
        }
        
        [Test]
        public void CustomerFieldsEditTypeTest()
        {
            testname = "CustomerFieldsEditTypeTest";
            VerifyBegin(testname);

            //pre check edit customer
            GoToAdmin("customers/edit/cfc2c33b-1e84-415e-8482-e98156341601");

            VerifyIsTrue(driver.PageSource.Contains("Customer Field 2"), "pre check edit customer field name");
            VerifyIsTrue(driver.FindElements(By.CssSelector("[validation-input-text=\"Customer Field 2\"]")).Count > 0, "pre check customer edit field enabled");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[validation-input-text=\"Customer Field 2\"]")).GetAttribute("ng-required").Equals("true"), "pre check customer edit required");

            driver.FindElement(By.CssSelector("[validation-input-text=\"Customer Field 2\"]")).Click();
            driver.FindElement(By.CssSelector("[validation-input-text=\"Customer Field 2\"]")).SendKeys("string");
            DropFocus("h1");

            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            VerifyAreEqual("string", driver.FindElement(By.CssSelector("[validation-input-text=\"Customer Field 2\"]")).GetAttribute("value"), "pre check customer edit field print text");

            GoToAdmin("settingscustomers#?tab=customerFields");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("Customer Field 2");
            XPathContainsText("h1", "Дополнительные поля покупателя");

            VerifyAreEqual("Customer Field 2", GetGridCell(0, "Name", "CustomerFields").Text, "customer field name");

            GetGridCell(0, "_serviceColumn", "CustomerFields").FindElement(By.CssSelector(".link-invert.ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(1000);

            //pre check edit field pop up
            VerifyAreEqual("Редактирование поля", driver.FindElement(By.TagName("h2")).Text, "h2 edit customer field");

            VerifyAreEqual("Customer Field 2", driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldName\"]")).GetAttribute("value"), "pre check edit pop up name");
            VerifyAreEqual("20", driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldSortOrder\"]")).GetAttribute("value"), "pre check edit pop up sort");

            IWebElement selectElemType = driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldType\"]"));
            SelectElement select = new SelectElement(selectElemType);
            VerifyIsTrue(select.AllSelectedOptions[0].Text.Contains("Текстовое поле"), "pre check edit pop up type");

            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldRequired\"]")).FindElement(By.TagName("input")).Selected, "pre check edit pop up required");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldEnabled\"]")).FindElement(By.TagName("input")).Selected, "pre check edit pop up enabled");

            VerifyIsFalse(driver.FindElement(By.CssSelector(".modal-body")).Text.Contains("Значения поля"), "pre check edit pop no select type");

            //edit
            driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldName\"]")).SendKeys("Edited Customer Field");

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldType\"]")))).SelectByText("Выбор");

            VerifyIsTrue(driver.FindElement(By.CssSelector(".modal-body")).Text.Contains("Значения поля"), "select type");

            driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldValue\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldValue\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldValue\"]")).SendKeys("Value 1");

            driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldAddValue\"]")).Click();

            driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldValue\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldValue\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldValue\"]")).SendKeys("Value 2");

            driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldAddValue\"]")).Click();

            driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldValue\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldValue\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldValue\"]")).SendKeys("Value 3");

            driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldAddValue\"]")).Click();

            driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldSortOrder\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldSortOrder\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldSortOrder\"]")).SendKeys("100");

            driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldRequired\"]")).FindElement(By.TagName("span")).Click();

            GetButton(eButtonType.Save).Click();
            Thread.Sleep(500);

            //check admin settings
            GoToAdmin("settingscustomers#?tab=customerFields");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("Customer Field 2");
            XPathContainsText("h1", "Дополнительные поля покупателя");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "modified customer field");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("Edited Customer Field");
            XPathContainsText("h1", "Дополнительные поля покупателя");

            VerifyAreEqual("Edited Customer Field", GetGridCell(0, "Name", "CustomerFields").Text, "customer field name");
            VerifyAreEqual("Выбор", GetGridCell(0, "FieldTypeFormatted", "CustomerFields").Text, "customer field type");
            VerifyAreEqual("Список значений", GetGridCell(0, "HasValues", "CustomerFields").Text, "customer field values");
            VerifyIsFalse(GetGridCell(0, "Required", "CustomerFields").FindElement(By.TagName("input")).Selected, "customer field Required");
            VerifyAreEqual("100", GetGridCell(0, "SortOrder", "CustomerFields").FindElement(By.TagName("input")).GetAttribute("value"), "customer field SortOrder");
            VerifyIsTrue(GetGridCell(0, "Enabled", "CustomerFields").FindElement(By.TagName("input")).Selected, "customer field enabled");

            //check admin edit customer
            GoToAdmin("customers/edit/cfc2c33b-1e84-415e-8482-e98156341601");

            VerifyIsTrue(driver.PageSource.Contains("Edited Customer Field"), "customer edit field name");
            VerifyIsTrue(driver.FindElements(By.CssSelector("[validation-input-text=\"Edited Customer Field\"]")).Count > 0, "customer edit field enabled");

            IWebElement selectElemEditedType = driver.FindElement(By.CssSelector("[validation-input-text=\"Edited Customer Field\"]"));
            SelectElement select2 = new SelectElement(selectElemEditedType);

            IList<IWebElement> allOptionsEditedCustomerField = select2.Options;

            VerifyIsTrue(allOptionsEditedCustomerField.Count == 4, "customer edit count field's values"); //3 customer field's values + null select

            VerifyFinally(testname);
        }

        [Test]
        public void CustomerFieldsEditEnabledTest()
        {
            testname = "CustomerFieldsEditEnabledTest";
            VerifyBegin(testname);

            //pre check edit customer
            GoToAdmin("customers/edit/cfc2c33b-1e84-415e-8482-e98156341601");

            VerifyIsFalse(driver.PageSource.Contains("Customer Field 5"), "pre check edit customer field name");
            VerifyIsFalse(driver.FindElements(By.CssSelector("[validation-input-text=\"Customer Field 5\"]")).Count > 0, "pre check customer edit field enabled");

            GoToAdmin("settingscustomers#?tab=customerFields");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("Customer Field 5");
            XPathContainsText("h1", "Дополнительные поля покупателя");

            VerifyAreEqual("Customer Field 5", GetGridCell(0, "Name", "CustomerFields").Text, "customer field name");

            //edit
            GetGridCell(0, "_serviceColumn", "CustomerFields").FindElement(By.CssSelector(".link-invert.ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(1000);
          
            driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldEnabled\"]")).FindElement(By.TagName("span")).Click();

            GetButton(eButtonType.Save).Click();
            Thread.Sleep(500);

            //check admin settings
            GoToAdmin("settingscustomers#?tab=customerFields");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("Customer Field 5");
            XPathContainsText("h1", "Дополнительные поля покупателя");

            VerifyAreEqual("Customer Field 5", GetGridCell(0, "Name", "CustomerFields").Text, "customer field name");
            VerifyAreEqual("Текстовое поле", GetGridCell(0, "FieldTypeFormatted", "CustomerFields").Text, "customer field type");
            VerifyAreEqual("", GetGridCell(0, "HasValues", "CustomerFields").Text, "customer field values");
            VerifyIsTrue(GetGridCell(0, "Required", "CustomerFields").FindElement(By.TagName("input")).Selected, "customer field Required");
            VerifyAreEqual("50", GetGridCell(0, "SortOrder", "CustomerFields").FindElement(By.TagName("input")).GetAttribute("value"), "customer field SortOrder");
            VerifyIsTrue(GetGridCell(0, "Enabled", "CustomerFields").FindElement(By.TagName("input")).Selected, "customer field enabled");

            //check admin edit customer
            GoToAdmin("customers/edit/cfc2c33b-1e84-415e-8482-e98156341601");

            VerifyIsTrue(driver.PageSource.Contains("Customer Field 5"), "customer edit field name");
            VerifyIsTrue(driver.FindElements(By.CssSelector("[validation-input-text=\"Customer Field 5\"]")).Count > 0, "customer edit field enabled");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[validation-input-text=\"Customer Field 5\"]")).GetAttribute("ng-required").Equals("true"), "customer edit field required");

            VerifyFinally(testname);
        }

        [Test]
        public void CustomerFieldsAddDateTest()
        {
            testname = "CustomerFieldsAddDateTest";
            VerifyBegin(testname);

            GoToAdmin("settingscustomers#?tab=customerFields");

            GetButton(eButtonType.Add).Click();
            Thread.Sleep(1000);
            
            driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldName\"]")).SendKeys("New Customer Field Date");

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldType\"]")))).SelectByText("Дата");

            driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldSortOrder\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldSortOrder\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldSortOrder\"]")).SendKeys("4");

            VerifyIsFalse(driver.FindElement(By.CssSelector(".modal-body")).Text.Contains("Значения поля"), "no select type");

            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldEnabled\"]")).FindElement(By.TagName("input")).Selected, "default enabled");
            
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(500);

            //check admin settings
            GoToAdmin("settingscustomers#?tab=customerFields");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("New Customer Field Date");
            XPathContainsText("h1", "Дополнительные поля покупателя");

            VerifyAreEqual("New Customer Field Date", GetGridCell(0, "Name", "CustomerFields").Text, "customer field name");
            VerifyAreEqual("Дата", GetGridCell(0, "FieldTypeFormatted", "CustomerFields").Text, "customer field type");
            VerifyAreEqual("", GetGridCell(0, "HasValues", "CustomerFields").Text, "customer field no values");
            VerifyIsFalse(GetGridCell(0, "Required", "CustomerFields").FindElement(By.TagName("input")).Selected, "customer field Required");
            VerifyAreEqual("4", GetGridCell(0, "SortOrder", "CustomerFields").FindElement(By.TagName("input")).GetAttribute("value"), "customer field SortOrder");
            VerifyIsTrue(GetGridCell(0, "Enabled", "CustomerFields").FindElement(By.TagName("input")).Selected, "customer field enabled");
            VerifyIsFalse(GetGridCell(0, "ShowInClient", "CustomerFields").FindElement(By.TagName("input")).Selected, "customer field ShowInClient");

            //check admin edit customer
            GoToAdmin("customers/edit/cfc2c33b-1e84-415e-8482-e98156341601");

            VerifyIsTrue(driver.PageSource.Contains("New Customer Field Date"), "customer edit field name enabled");
            VerifyIsTrue(driver.FindElements(By.CssSelector("[validation-input-text=\"New Customer Field Date\"]")).Count > 0, "customer edit field enabled");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[validation-input-text=\"New Customer Field Date\"]")).GetAttribute("ng-required").Equals("false"), "customer edit not required");

            VerifyIsFalse(driver.FindElement(By.CssSelector(".table.table-condensed.day-view")).Displayed, "calender not displayed");

            driver.FindElement(By.CssSelector(".glyphicon.glyphicon-calendar")).Click();
            VerifyIsTrue(driver.FindElement(By.CssSelector(".table.table-condensed.day-view")).Displayed, "calender displayed");

            driver.FindElement(By.CssSelector("[validation-input-text=\"New Customer Field Date\"]")).Click();
            driver.FindElement(By.CssSelector("[validation-input-text=\"New Customer Field Date\"]")).SendKeys("03.03.2017");
            DropFocus("h1");

            VerifyAreEqual("03.03.2017", driver.FindElement(By.CssSelector("[validation-input-text=\"New Customer Field Date\"]")).GetAttribute("value"), "customer edit field date");

            VerifyFinally(testname);
        }
    }
}
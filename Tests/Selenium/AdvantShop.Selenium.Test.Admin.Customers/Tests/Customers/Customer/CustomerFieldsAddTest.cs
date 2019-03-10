using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using AdvantShop.Selenium.Core.Infrastructure;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.Web.Site.Selenium.Test.Admin.Customers.CustomerFields
{
    [TestFixture]
    public class CustomerFieldsAddTest : BaseSeleniumTest
    {

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Customers);
            InitializeService.LoadData(
            "data\\Admin\\Customers\\CustomerFields\\CustomerFieldAdd\\Customers.Customer.csv",
           "data\\Admin\\Customers\\CustomerFields\\CustomerFieldAdd\\Customers.CustomerGroup.csv",
               "data\\Admin\\Customers\\CustomerFields\\CustomerFieldAdd\\Customers.CustomerField.csv",
               "data\\Admin\\Customers\\CustomerFields\\CustomerFieldAdd\\Customers.CustomerFieldValue.csv",
               "data\\Admin\\Customers\\CustomerFields\\CustomerFieldAdd\\Customers.CustomerFieldValuesMap.csv"

           );

            Init();
        }

         

        
        [Test]
        public void CustomerFieldsAddNumberTest()
        {
            testname = "CustomerFieldsAddNumberTest";
            VerifyBegin(testname);

            GoToAdmin("settingscustomers#?tab=customerFields");

            GetButton(eButtonType.Add).Click();
            Thread.Sleep(1000);

            driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldName\"]")).SendKeys("New Customer Field Number");

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldType\"]")))).SelectByText("Числовое поле");

            driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldSortOrder\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldSortOrder\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldSortOrder\"]")).SendKeys("4");

            driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldRequired\"]")).FindElement(By.TagName("span")).Click();

            VerifyIsFalse(driver.FindElement(By.CssSelector(".modal-body")).Text.Contains("Значения поля"), "no select type");

            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            //check admin settings
            GoToAdmin("settingscustomers#?tab=customerFields");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("New Customer Field Number");
            XPathContainsText("h1", "Дополнительные поля покупателя");

            VerifyAreEqual("New Customer Field Number", GetGridCell(0, "Name", "CustomerFields").Text, "customer field name");
            VerifyAreEqual("Числовое поле", GetGridCell(0, "FieldTypeFormatted", "CustomerFields").Text, "customer field type");
            VerifyAreEqual("", GetGridCell(0, "HasValues", "CustomerFields").Text, "customer field values");
            VerifyIsTrue(GetGridCell(0, "Required", "CustomerFields").FindElement(By.TagName("input")).Selected, "customer field Required");
            VerifyAreEqual("4", GetGridCell(0, "SortOrder", "CustomerFields").FindElement(By.TagName("input")).GetAttribute("value"), "customer field SortOrder");
            VerifyIsTrue(GetGridCell(0, "Enabled", "CustomerFields").FindElement(By.TagName("input")).Selected, "customer field enabled");

            //check admin edit customer
            GoToAdmin("customers/edit/cfc2c33b-1e84-415e-8482-e98156341601");

            VerifyIsTrue(driver.PageSource.Contains("New Customer Field Number"), "customer edit field name");
            VerifyIsTrue(driver.FindElements(By.CssSelector("[validation-input-text=\"New Customer Field Number\"]")).Count > 0, "customer edit field enabled");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[validation-input-text=\"New Customer Field Number\"]")).GetAttribute("ng-required").Equals("true"), "customer edit required");

            driver.FindElement(By.CssSelector("[validation-input-text=\"New Customer Field Number\"]")).Click();
            driver.FindElement(By.CssSelector("[validation-input-text=\"New Customer Field Number\"]")).Clear();
            driver.FindElement(By.CssSelector("[validation-input-text=\"New Customer Field Number\"]")).SendKeys("string");
            DropFocus("h1");
            VerifyAreEqual("rgb(241, 89, 89)", driver.FindElement(By.CssSelector("[validation-input-text=\"New Customer Field Number\"]")).GetCssValue("border-top-color"), "customer edit field print text");

            GoToAdmin("customers/edit/cfc2c33b-1e84-415e-8482-e98156341601");
            driver.FindElement(By.CssSelector("[validation-input-text=\"New Customer Field Number\"]")).Click();
            driver.FindElement(By.CssSelector("[validation-input-text=\"New Customer Field Number\"]")).Clear();
            driver.FindElement(By.CssSelector("[validation-input-text=\"New Customer Field Number\"]")).SendKeys("111");
            DropFocus("h1");
            VerifyAreNotEqual("rgb(241, 89, 89)", driver.FindElement(By.CssSelector("[validation-input-text=\"New Customer Field Number\"]")).GetCssValue("border-top-color"), "customer edit field print number");

            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            VerifyAreEqual("111", driver.FindElement(By.CssSelector("[validation-input-text=\"New Customer Field Number\"]")).GetAttribute("value"), "customer edit field print number");

            VerifyFinally(testname);
        }

        [Test]
        public void CustomerFieldsAddTextAreaTest()
        {
            testname = "CustomerFieldsAddTextAreaTest";
            VerifyBegin(testname);

            GoToAdmin("settingscustomers#?tab=customerFields");

            GetButton(eButtonType.Add).Click();
            Thread.Sleep(1000);

            driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldName\"]")).SendKeys("New Customer Field Area");

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldType\"]")))).SelectByText("Многострочное текстовое поле");

            driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldSortOrder\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldSortOrder\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"customerFieldSortOrder\"]")).SendKeys("5");

            VerifyIsFalse(driver.FindElement(By.CssSelector(".modal-body")).Text.Contains("Значения поля"), "no select type");

            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            //check admin settings
            GoToAdmin("settingscustomers#?tab=customerFields");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("New Customer Field Area");
            XPathContainsText("h1", "Дополнительные поля покупателя");

            VerifyAreEqual("New Customer Field Area", GetGridCell(0, "Name", "CustomerFields").Text, "customer field name");
            VerifyAreEqual("Многострочное текстовое поле", GetGridCell(0, "FieldTypeFormatted", "CustomerFields").Text, "customer field type");
            VerifyAreEqual("", GetGridCell(0, "HasValues", "CustomerFields").Text, "customer field values");
            VerifyIsFalse(GetGridCell(0, "Required", "CustomerFields").FindElement(By.TagName("input")).Selected, "customer field Required");
            VerifyAreEqual("5", GetGridCell(0, "SortOrder", "CustomerFields").FindElement(By.TagName("input")).GetAttribute("value"), "customer field SortOrder");
            VerifyIsTrue(GetGridCell(0, "Enabled", "CustomerFields").FindElement(By.TagName("input")).Selected, "customer field enabled");

            //check admin edit customer
            GoToAdmin("customers/edit/cfc2c33b-1e84-415e-8482-e98156341601");

            VerifyIsTrue(driver.PageSource.Contains("New Customer Field Area"), "customer edit field name");
            VerifyIsTrue(driver.FindElements(By.CssSelector("[validation-input-text=\"New Customer Field Area\"]")).Count > 0, "customer edit field enabled");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[validation-input-text=\"New Customer Field Area\"]")).GetAttribute("ng-required").Equals("false"), "customer edit not required");

            driver.FindElement(By.CssSelector("[validation-input-text=\"New Customer Field Area\"]")).Click();
            driver.FindElement(By.CssSelector("[validation-input-text=\"New Customer Field Area\"]")).SendKeys("test 1" + Keys.Enter + "test 2" + Keys.Enter + "test 3");
            DropFocus("h1");

            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            VerifyAreEqual("test 1\r\ntest 2\r\ntest 3", driver.FindElement(By.CssSelector("[validation-input-text=\"New Customer Field Area\"]")).GetAttribute("value"), "customer edit field textarea");

            VerifyFinally(testname);
        }
        
    }
}
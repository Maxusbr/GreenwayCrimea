using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.SeleniumTest.Admin.Settings.SystemSettings.Countries.City
{
    [TestFixture]
    public class SettingsCityFilterTest : BaseSeleniumTest
    {

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Customers);
            InitializeService.LoadData(
             "data\\Admin\\Settings\\Countries\\City\\Customers.Country.csv",
               "data\\Admin\\Settings\\Countries\\City\\Customers.Region.csv",
              "data\\Admin\\Settings\\Countries\\City\\Customers.City.csv",
            "data\\Admin\\Settings\\Countries\\City\\Customers.Customer.csv",
           "data\\Admin\\Settings\\Countries\\City\\Customers.CustomerGroup.csv"

           );

            Init();
        }
        
        [Test]
        public void CityFilterDisplay()
        {
            testname = "CityFilterDisplay";
            VerifyBegin(testname);
            GoToAdmin("settingssystem#?systemTab=countries");

            GetGridCell(0, "Name", "Country").Click();
            Thread.Sleep(2000);
            GetGridCell(0, "Name", "Region").Click();
            Thread.Sleep(2000);
            Functions.GridFilterSet(driver, baseURL, "DisplayInPopup");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Да");

            VerifyAreEqual("Найдено записей: 5", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter DisplayInPopup count");

            VerifyIsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count == 5, "filter DisplayInPopup row");
            VerifyAreEqual("TestCity10", GetGridCell(0, "Name", "City").Text, "filter DisplayInPopup value1");

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Нет");

            VerifyAreEqual("Найдено записей: 96", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter DisplayInPopup count 2");

            VerifyIsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count == 10, "filter DisplayInPopup row 2");
            VerifyAreEqual("TestCity1", GetGridCell(0, "Name", "City").Text, "filter DisplayInPopup value 2");

            Functions.GridFilterClose(driver, baseURL, "DisplayInPopup");
            VerifyAreEqual("TestCity1", GetGridCell(0, "Name", "City").Text, "filter DisplayInPopup exit 1");
            VerifyAreEqual("TestCity6", GetGridCell(5, "Name", "City").Text, "filter DisplayInPopup exit 5");

            VerifyFinally(testname);
        }


        [Test]
        public void CityFilterPhone()
        {
            testname = "CityFilterPhone";
            VerifyBegin(testname);
            GoToAdmin("settingssystem#?systemTab=countries");

            GetGridCell(0, "Name", "Country").Click();
            Thread.Sleep(2000);
            GetGridCell(0, "Name", "Region").Click();
            Thread.Sleep(2000);
            Functions.GridFilterSet(driver, baseURL, "PhoneNumber");
            //search by not exist 
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("qwe");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "no such element");

            //search too much symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("111111111122222222222222222222222222222");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "too much symbols");

            //search by exist
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("111113");
            XPathContainsText("h1", "Список городов");
            VerifyAreEqual("Найдено записей: 1", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter PhoneNumber count");

            VerifyIsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count == 1, "filter PhoneNumber row");
            VerifyAreEqual("TestCity3", GetGridCell(0, "Name", "City").Text, "filter PhoneNumber value");
            Functions.GridFilterClose(driver, baseURL, "PhoneNumber");
            VerifyAreEqual("TestCity1", GetGridCell(0, "Name", "City").Text, "filter PhoneNumber exit 1");
            VerifyAreEqual("TestCity6", GetGridCell(5, "Name", "City").Text, "filter PhoneNumber exit 5");

            VerifyFinally(testname);
        }
        [Test]
        public void CityFilterMobile()
        {
            testname = "CityFilterMobile";
            VerifyBegin(testname);
            GoToAdmin("settingssystem#?systemTab=countries");

            GetGridCell(0, "Name", "Country").Click();
            Thread.Sleep(2000);
            GetGridCell(0, "Name", "Region").Click();
            Thread.Sleep(2000);
            Functions.GridFilterSet(driver, baseURL, "MobilePhoneNumber");

            //search by not exist 
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("qwe");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "no such element");

            //search too much symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("111111111122222222222222222222222222222");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "too much symbols");

            //search by exist
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("999998");
            XPathContainsText("h1", "Список городов");
            VerifyAreEqual("Найдено записей: 1", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter MobilePhoneNumber count");

            VerifyIsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count == 1, "filter MobilePhoneNumber row");
            VerifyAreEqual("TestCity2", GetGridCell(0, "Name", "City").Text, "filter MobilePhoneNumber value");
            Functions.GridFilterClose(driver, baseURL, "MobilePhoneNumber");
            VerifyAreEqual("TestCity1", GetGridCell(0, "Name", "City").Text, "filter MobilePhoneNumber exit 1");
            VerifyAreEqual("TestCity6", GetGridCell(5, "Name", "City").Text, "filter MobilePhoneNumber exit 5");

            VerifyFinally(testname);
        }
        [Test]
        public void CityFilterName()
        {
            testname = "CityFilteName";
            VerifyBegin(testname);
            GoToAdmin("settingssystem#?systemTab=countries");

            GetGridCell(0, "Name", "Country").Click();
            Thread.Sleep(2000);
            GetGridCell(0, "Name", "Region").Click();
            Thread.Sleep(2000);
            Functions.GridFilterSet(driver, baseURL, "Name");

            //search by not exist 
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("qwew");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "no such element");

            //search too much symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("111111111122222222222222222222222222222");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "too much symbols");

            //search by exist
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("TestCity10");
            XPathContainsText("h1", "Список городов");

            VerifyIsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count == 3, "filter Name row");
            VerifyAreEqual("Найдено записей: 3", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter Name count");
            VerifyAreEqual("TestCity10", GetGridCell(0, "Name", "City").Text, "filter Name value");

            Functions.GridFilterClose(driver, baseURL, "Name");
            VerifyAreEqual("Найдено записей: 101", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter Name exit");
            VerifyAreEqual("TestCity1", GetGridCell(0, "Name", "City").Text, "filter Name exit");
            VerifyAreEqual("TestCity6", GetGridCell(5, "Name", "City").Text, "filter Name exit");

            VerifyFinally(testname);
        }

        [Test]
        public void CityFilterSort()
        {
            testname = "CityFilterSort";
            VerifyBegin(testname);
            GoToAdmin("settingssystem#?systemTab=countries");

            GetGridCell(0, "Name", "Country").Click();
            Thread.Sleep(2000);
            GetGridCell(0, "Name", "Region").Click();
            Thread.Sleep(2000);
            Functions.GridFilterSet(driver, baseURL, "CitySort");

            //search by not exist 
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).SendKeys("50000");


            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).SendKeys("60000");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "no such element");

            //search too much symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).SendKeys("111111111122222222222222222222222222222");
            
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).SendKeys("111111111122222222222222222222222222233");
            XPathContainsText("h1", "Список городов");
            VerifyAreEqual("rgb(241, 89, 89)", driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).GetCssValue("border-top-color"), "filter too much symbols");
            VerifyAreEqual("rgb(241, 89, 89)", driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).GetCssValue("border-top-color"), "filter too much symbols");

            //search by exist
            GoToAdmin("settingssystem#?systemTab=countries");
            GetGridCell(0, "Name", "Country").Click();
            Thread.Sleep(2000);
            GetGridCell(0, "Name", "Region").Click();
            Thread.Sleep(2000);
            Functions.GridFilterSet(driver, baseURL, "CitySort");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).SendKeys("10");
            
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).SendKeys("15");
            XPathContainsText("h1", "Список городов");
            VerifyAreEqual("Найдено записей: 6", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter SortOrder count");
            VerifyIsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count == 6, "filter SortOrder 2 row");
            VerifyAreEqual("TestCity10", GetGridCell(0, "Name", "City").Text, "filter SortOrder 2 value");

            //check go to edit and back 
            GetGridCell(0, "_serviceColumn", "City").FindElement(By.TagName("ui-modal-trigger")).Click();
            Thread.Sleep(4000);
            WaitForElem(By.CssSelector(".modal-content"));

            VerifyAreEqual("Редактирование города", driver.FindElement(By.TagName("h2")).Text, "pop up h2");
            XPathContainsText("button", "Отмена");

            VerifyAreEqual("Найдено записей: 6", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter SortOrder return");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"CitySort\"]")).Displayed);

            //check delete with filter
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(2000);
            Functions.GridDropdownDelete(driver, baseURL);

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "delete filtered items");
            WaitForAjax();
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "delete filtered items count");


            Functions.GridFilterClose(driver, baseURL, "CitySort");
            VerifyAreEqual("TestCity1", GetGridCell(0, "Name", "City").Text, "filter SortOrder exit 1 ");
            VerifyAreEqual("TestCity6", GetGridCell(5, "Name", "City").Text, "filter SortOrder exit 5");
            VerifyAreEqual("Найдено записей: 95", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter SortOrder return");

            VerifyFinally(testname);
        }
    }
}

using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.SeleniumTest.Admin.Settings.SystemSettings.Countries.Country
{
    [TestFixture]
    public class CountriesFilterTest : BaseSeleniumTest
    {

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Customers);
            InitializeService.LoadData(
             "data\\Admin\\Settings\\Countries\\Customers.Country.csv",
               "data\\Admin\\Settings\\Countries\\Customers.Region.csv",
              "data\\Admin\\Settings\\Countries\\Customers.City.csv",
            "data\\Admin\\Settings\\Countries\\Customers.Customer.csv",
           "data\\Admin\\Settings\\Countries\\Customers.CustomerGroup.csv"

           );

            Init();
        }
        [Test]
        public void CountriesFilterCode()
        {
            testname = "CountriesFilterCode";
            VerifyBegin(testname);
            GoToAdmin("settingssystem#?systemTab=countries");

            Functions.GridFilterSet(driver, baseURL, "DialCode");

            //search by not exist 
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("qqqqwd");
            VerifyAreEqual("", driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Text, "no such element");

            //search too much symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("111111111122222222222222222222222222222");
            VerifyAreEqual("", driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Text, "too much symbols");
           
            //search by exist
            GoToAdmin("settingssystem#?systemTab=countries");
            Functions.GridFilterSet(driver, baseURL, "DialCode");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("112");
            XPathContainsText("h1", "Список стран");
            WaitForAjax();
            VerifyAreEqual("Найдено записей: 1", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter DialCode 2 count");
            VerifyIsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count == 1, "filter DialCode 2 row");
            VerifyAreEqual("TestCountry2", GetGridCell(0, "Name", "Country").Text, "filter DialCode 2 value");
            Functions.GridFilterClose(driver, baseURL, "DialCode");
            VerifyAreEqual("TestCountry1", GetGridCell(0, "Name", "Country").Text, "filter DialCode exit 1");
            VerifyAreEqual("TestCountry6", GetGridCell(5, "Name", "Country").Text, "filter DialCode exit 5");

            VerifyFinally(testname);
        }
        [Test]
        public void CountriesFilterDisplay()
        {
            testname = "CountriesFilterDisplay";
            VerifyBegin(testname);
            GoToAdmin("settingssystem#?systemTab=countries");

            Functions.GridFilterSet(driver, baseURL, "DisplayInPopup");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Да");

            VerifyAreEqual("Найдено записей: 5", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter DisplayInPopup count");

            VerifyIsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count == 5, "filter DisplayInPopup row");
            VerifyAreEqual("TestCountry2", GetGridCell(0, "Name", "Country").Text, "filter DisplayInPopup value1");

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Нет");

            VerifyAreEqual("Найдено записей: 96", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter DisplayInPopup count 2");

            VerifyIsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count == 10, "filter DisplayInPopup row 2");
            VerifyAreEqual("TestCountry1", GetGridCell(0, "Name", "Country").Text, "filter DisplayInPopup value 2");

            Functions.GridFilterClose(driver, baseURL, "DisplayInPopup");
            VerifyAreEqual("TestCountry1", GetGridCell(0, "Name", "Country").Text, "filter DisplayInPopup exit 1");
            VerifyAreEqual("TestCountry6", GetGridCell(5, "Name", "Country").Text, "filter DisplayInPopup exit 5");

            VerifyFinally(testname);
        }
      
   
        [Test]
        public void CountriesFilterIso2()
        {
            testname = "CountriesFilterIso2";
            VerifyBegin(testname);
            GoToAdmin("settingssystem#?systemTab=countries");
         
            Functions.GridFilterSet(driver, baseURL, "Iso2");
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
            GoToAdmin("settingssystem#?systemTab=countries");
            Functions.GridFilterSet(driver, baseURL, "Iso2");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("Z");
            XPathContainsText("h1", "Список стран");
            VerifyAreEqual("Найдено записей: 3", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter Iso2 count");

            VerifyIsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count == 3, "filter Iso2 row");
            VerifyAreEqual("TestCountry26", GetGridCell(0, "Name", "Country").Text, "filter Iso2 value");
            Functions.GridFilterClose(driver, baseURL, "Iso2");
            VerifyAreEqual("TestCountry1", GetGridCell(0, "Name", "Country").Text, "filter Iso2 exit 1");
            VerifyAreEqual("TestCountry6", GetGridCell(5, "Name", "Country").Text, "filter Iso2 exit 5");

            VerifyFinally(testname);
        }
        [Test]
        public void CountriesFilterIso3()
        {
            testname = "CountriesFilterIso3";
            VerifyBegin(testname);
            GoToAdmin("settingssystem#?systemTab=countries");
      
            Functions.GridFilterSet(driver, baseURL, "Iso3");
           
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
            GoToAdmin("settingssystem#?systemTab=countries");
            Functions.GridFilterSet(driver, baseURL, "Iso3");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("z1");
            XPathContainsText("h1", "Список стран");
            VerifyAreEqual("Найдено записей: 3", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter Iso3 count");

            VerifyIsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count == 3, "filter Iso3 row");
            VerifyAreEqual("TestCountry26", GetGridCell(0, "Name", "Country").Text, "filter Iso3 value");
            Functions.GridFilterClose(driver, baseURL, "Iso3");
            VerifyAreEqual("TestCountry1", GetGridCell(0, "Name", "Country").Text, "filter Iso3 exit 1");
            VerifyAreEqual("TestCountry6", GetGridCell(5, "Name", "Country").Text, "filter Iso3 exit 5");

            VerifyFinally(testname);
        }
        [Test]
        public void CountriesFilterName()
        {
            testname = "CountriesFilteName";
            VerifyBegin(testname);
            GoToAdmin("settingssystem#?systemTab=countries");

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
            GoToAdmin("settingssystem#?systemTab=countries");
            Functions.GridFilterSet(driver, baseURL, "Name");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("TestCountry10");
            XPathContainsText("h1", "Список стран");

            VerifyIsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count == 3, "filter Name row");
            VerifyAreEqual("Найдено записей: 3", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter Name count");
            VerifyAreEqual("TestCountry10", GetGridCell(0, "Name", "Country").Text, "filter Name value");
                       
            Functions.GridFilterClose(driver, baseURL, "Name");
            VerifyAreEqual("Найдено записей: 101", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter Name exit");
            VerifyAreEqual("TestCountry1", GetGridCell(0, "Name", "Country").Text, "filter Name exit");
            VerifyAreEqual("TestCountry6", GetGridCell(5, "Name", "Country").Text, "filter Name exit");

            VerifyFinally(testname);
        }
        [Test]
        public void CountriesFilterSort()
        {
            testname = "CountriesFilterSort";
            VerifyBegin(testname);
            GoToAdmin("settingssystem#?systemTab=countries");
            Functions.GridFilterSet(driver, baseURL, "SortOrder");

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
            XPathContainsText("h1", "Список стран");
            VerifyAreEqual("rgb(241, 89, 89)", driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).GetCssValue("border-top-color"), "too much symbols");
            VerifyAreEqual("rgb(241, 89, 89)", driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).GetCssValue("border-top-color"), "too much symbols");

            //search by exist
            GoToAdmin("settingssystem#?systemTab=countries");
            Functions.GridFilterSet(driver, baseURL, "SortOrder");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).SendKeys("10");


            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).SendKeys("15");
            XPathContainsText("h1", "Список стран");
            VerifyAreEqual("Найдено записей: 6", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter SortOrder count");
            VerifyIsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count ==6, "filter SortOrder 2 row");
            VerifyAreEqual("TestCountry10", GetGridCell(0, "Name", "Country").Text, "filter SortOrder 2 value");

            //check go to edit and back 
            GetGridCell(0, "_serviceColumn", "Country").FindElement(By.TagName("ui-modal-trigger")).Click();
            Thread.Sleep(4000);
            WaitForElem(By.CssSelector(".modal-content"));

            VerifyAreEqual("Редактирование страны", driver.FindElement(By.TagName("h2")).Text, "pop up h2");
            XPathContainsText("button", "Отмена");

            VerifyAreEqual("Найдено записей: 6", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter SortOrder return");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"SortOrder\"]")).Displayed);

            //check delete with filter
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(2000);
            Functions.GridDropdownDelete(driver, baseURL);

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "delete filtered items");
            WaitForAjax();
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "delete filtered items count");


            Functions.GridFilterClose(driver, baseURL, "SortOrder");
            VerifyAreEqual("TestCountry1", GetGridCell(0, "Name", "Country").Text, "filter SortOrder exit 1 ");
            VerifyAreEqual("TestCountry6", GetGridCell(5, "Name", "Country").Text, "filter SortOrder exit 5");
            VerifyAreEqual("Найдено записей: 95", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter SortOrder return");

            VerifyFinally(testname);
        }
      
    }
}

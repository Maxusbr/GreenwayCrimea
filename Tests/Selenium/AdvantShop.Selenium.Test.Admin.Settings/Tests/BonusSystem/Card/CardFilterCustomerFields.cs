using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;

namespace AdvantShop.SeleniumTest.Admin.Settings.BonusSystem.Cards
{
    [TestFixture]
    public class BonusSystemCardFilterCustomerFields : BaseMultiSeleniumTest
    {
        [SetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog | ClearType.Customers | ClearType.Bonuses);
            InitializeService.LoadData(

             "Data\\Admin\\Settings\\BonusSystem\\BonusCardFilterCustFields\\Catalog.Product.csv",
           "Data\\Admin\\Settings\\BonusSystem\\BonusCardFilterCustFields\\Catalog.Offer.csv",
           "Data\\Admin\\Settings\\BonusSystem\\BonusCardFilterCustFields\\Catalog.Category.csv",
          "Data\\Admin\\Settings\\BonusSystem\\BonusCardFilterCustFields\\Catalog.ProductCategories.csv",
           "Data\\Admin\\Settings\\BonusSystem\\BonusCardFilterCustFields\\Bonus.Grade.csv",
           "Data\\Admin\\Settings\\BonusSystem\\BonusCardFilterCustFields\\Bonus.Card.csv",
           "Data\\Admin\\Settings\\BonusSystem\\BonusCardFilterCustFields\\Customers.CustomerGroup.csv",
           "data\\Admin\\Settings\\BonusSystem\\BonusCardFilterCustFields\\Customers.Country.csv",
            "data\\Admin\\Settings\\BonusSystem\\BonusCardFilterCustFields\\Customers.Region.csv",
            "data\\Admin\\Settings\\BonusSystem\\BonusCardFilterCustFields\\Customers.City.csv",
            "data\\Admin\\Settings\\BonusSystem\\BonusCardFilterCustFields\\Customers.Customer.csv",
            "data\\Admin\\Settings\\BonusSystem\\BonusCardFilterCustFields\\Customers.Contact.csv",
              "data\\Admin\\Settings\\BonusSystem\\BonusCardFilterCustFields\\Customers.CustomerField.csv",
             "data\\Admin\\Settings\\BonusSystem\\BonusCardFilterCustFields\\Customers.CustomerFieldValue.csv",
              "data\\Admin\\Settings\\BonusSystem\\BonusCardFilterCustFields\\Customers.CustomerFieldValuesMap.csv"
                );
            Init();

            GoToAdmin("cards");
        }
        
        [Test]
        public void FilterCustomerFieldSelect1()
        {
            testname = "FilterCustomerFieldSelect1";
            VerifyBegin(testname);

            //check filter 
            Functions.GridFilterSet(driver, baseURL, name: "_noopColumnCustomerField_1");

            //check filter no items
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Customer Field 1 Value 2");
            WaitForAjax();
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter no items count");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "filter no items");

            //check filter 
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Customer Field 1 Value 1");
            WaitForAjax();
            VerifyAreEqual("Найдено записей: 3", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter");

            VerifyAreEqual("530801", GetGridCell(0, "CardNumber").FindElement(By.TagName("a")).Text, "filter result card num 1");
            VerifyAreEqual("LastName1 FirstName1", GetGridCell(0, "FIO").Text, "filter result FIO 1");
            VerifyAreEqual("530802", GetGridCell(1, "CardNumber").FindElement(By.TagName("a")).Text, "filter result card num 2");
            VerifyAreEqual("LastName2 FirstName2", GetGridCell(1, "FIO").Text, "filter result FIO 2");
            VerifyAreEqual("530803", GetGridCell(2, "CardNumber").FindElement(By.TagName("a")).Text, "filter result card num 3");
            VerifyAreEqual("LastName3 FirstName3", GetGridCell(2, "FIO").Text, "filter result FIO 3");

            //check all  
            IWebElement selectElem = driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]"));
            SelectElement select = new SelectElement(selectElem);

            IList<IWebElement> allOptions = select.Options;

            VerifyIsTrue(allOptions.Count == 3, "count filter select values");  //2 values + null option 

            string strUrl = driver.Url;

            //check go to edit and back 
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".link-invert.ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(4000);
            WaitForElem(By.XPath("//h2[contains(text(), 'Карта')]"));
            VerifyIsTrue(driver.Url.Contains("edit"), "card edit");

            driver.Navigate().GoToUrl(strUrl);

            WaitForElem(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total"));
            VerifyAreEqual("Найдено записей: 3", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter return");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"_noopColumnCustomerField_1\"]")).Displayed);

            //check delete with filter
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Functions.GridDropdownDelete(driver, baseURL);

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "delete filtered items");

            //check delete filter
            ScrollTo(By.Id("header-top"));
            Functions.GridFilterClose(driver, baseURL, name: "_noopColumnCustomerField_1");
            Refresh();
            VerifyAreEqual("Найдено записей: 10", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter deleting 1");

            GoToAdmin("cards");
            VerifyAreEqual("Найдено записей: 10", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter deleting 2");

            VerifyFinally(testname);
        }

        [Test]
        public void FilterCustomerFieldSelect2()
        {
            testname = "FilterCustomerFieldSelect2";
            VerifyBegin(testname);

            //check filter 
            Functions.GridFilterSet(driver, baseURL, name: "_noopColumnCustomerField_2");

            //check filter value 1
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Customer Field 2 Value 1");
            WaitForAjax();
            VerifyAreEqual("Найдено записей: 1", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter select value 1");
            VerifyAreEqual("530803", GetGridCell(0, "CardNumber").FindElement(By.TagName("a")).Text, "filter select value 1 result card num");
            VerifyAreEqual("LastName3 FirstName3", GetGridCell(0, "FIO").Text, "filter select value 1 result FIO");

            //check filter value 2
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Customer Field 2 Value 2");
            WaitForAjax();
            VerifyAreEqual("Найдено записей: 1", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter select value 2");
            
            VerifyAreEqual("530804", GetGridCell(0, "CardNumber").FindElement(By.TagName("a")).Text, "filter select value 2 result card num");
            VerifyAreEqual("LastName4 FirstName4", GetGridCell(0, "FIO").Text, "filter select value 2 result FIO");

            //check all  
            IWebElement selectElem = driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]"));
            SelectElement select = new SelectElement(selectElem);

            IList<IWebElement> allOptions = select.Options;

            VerifyIsTrue(allOptions.Count == 3, "count filter select values");  //2 values + null option 

            string strUrl = driver.Url;

            //check go to edit and back 
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".link-invert.ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(4000);
            WaitForElem(By.XPath("//h2[contains(text(), 'Карта')]"));
            VerifyIsTrue(driver.Url.Contains("edit"), "card edit");

            driver.Navigate().GoToUrl(strUrl);

            WaitForElem(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total"));
            VerifyAreEqual("Найдено записей: 1", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter return");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"_noopColumnCustomerField_2\"]")).Displayed);

            //check delete with filter
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Functions.GridDropdownDelete(driver, baseURL);

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "delete filtered items");

            //check delete filter
            ScrollTo(By.Id("header-top"));
            Functions.GridFilterClose(driver, baseURL, name: "_noopColumnCustomerField_2");
            Refresh();
            VerifyAreEqual("Найдено записей: 12", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter deleting 1");

            GoToAdmin("cards");
            VerifyAreEqual("Найдено записей: 12", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter deleting 2");

            VerifyFinally(testname);
        }


        [Test]
        public void FilterCustomerFieldText()
        {
            testname = "FilterCustomerFieldText";
            VerifyBegin(testname);

            //check filter not exist
            Functions.GridFilterSet(driver, baseURL, name: "_noopColumnCustomerField_4");

            //search by not exist card
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("Field Text 5");
            DropFocus("h1");
            Blur();
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "no such element");
            
            //search too much symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("1111111111222222222222222222333333333333334444444444rrrrrrrrrrr");
            DropFocus("h1");
            Blur();
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "too much symbols");

            //search invalid symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("########@@@@@@@@&&&&&&&******,,,,..");
            DropFocus("h1");
            Blur();
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "invalid symbols");

            //check filter exist
            Functions.GridFilterClose(driver, baseURL, name: "_noopColumnCustomerField_4");
            Functions.GridFilterSet(driver, baseURL, name: "_noopColumnCustomerField_3");

            //search by exist card
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("Field Text 5");
            DropFocus("h1");
            Blur();

            VerifyAreEqual("Найдено записей: 1", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter count items");
            VerifyAreEqual("530805", GetGridCell(0, "CardNumber").FindElement(By.TagName("a")).Text, "filter result Card Number");
            VerifyAreEqual("LastName5 FirstName5", GetGridCell(0, "FIO").Text, "filter result FIO");

            string strUrl = driver.Url;

            //check go to edit and back 
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".link-invert.ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(4000);
            WaitForElem(By.XPath("//h2[contains(text(), 'Карта')]"));
            VerifyIsTrue(driver.Url.Contains("edit"), "card edit");

            driver.Navigate().GoToUrl(strUrl);

            WaitForElem(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total"));
            VerifyAreEqual("Найдено записей: 1", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter return");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"_noopColumnCustomerField_3\"]")).Displayed);

            //check delete with filter
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Functions.GridDropdownDelete(driver, baseURL);

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "delete filtered items");
            
            //check delete filter
            ScrollTo(By.Id("header-top"));
            Functions.GridFilterClose(driver, baseURL, name: "_noopColumnCustomerField_3");
            Refresh();
            VerifyAreEqual("Найдено записей: 12", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter deleting 1");

            GoToAdmin("cards");
            VerifyAreEqual("Найдено записей: 12", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter deleting 2");

            VerifyFinally(testname);
        }

        [Test]
        public void FilterCustomerFieldNum()
        {
            testname = "FilterCustomerFieldNum";
            VerifyBegin(testname);

            //check filter not exist
            Functions.GridFilterSet(driver, baseURL, name: "_noopColumnCustomerField_6");

            //check min too much symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).SendKeys("1111111111");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "filter min many symbols");

            //check max too much symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).SendKeys("1111111111");
            VerifyAreEqual("Найдено записей: 4", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter max many symbols");

            //check min and max too much symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).SendKeys("1111111111");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).SendKeys("1111111111");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "filter min/max many symbols");

            //check invalid symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Clear();

            //check min invalid symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).SendKeys("########@@@@@@@@&&&&&&&******");
            VerifyAreEqual("", driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Text, "filter min imvalid symbols");
            VerifyAreEqual("Найдено записей: 13", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter count cards min imvalid symbols");

            GoToAdmin("cards");
            Functions.GridFilterSet(driver, baseURL, name: "_noopColumnCustomerField_6");

            //check max invalid symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).SendKeys("########@@@@@@@@&&&&&&&******");
            VerifyAreEqual("", driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Text, "filter max imvalid symbols");
            VerifyAreEqual("Найдено записей: 13", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter count cards max imvalid symbols");

            //check min and max invalid symbols

            GoToAdmin("cards");
            Functions.GridFilterSet(driver, baseURL, name: "_noopColumnCustomerField_6");

            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).SendKeys("########@@@@@@@@&&&&&&&******");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).SendKeys("########@@@@@@@@&&&&&&&******");
            VerifyAreEqual("", driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Text, "filter both min imvalid symbols");
            VerifyAreEqual("", driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Text, "filter both max imvalid symbols");
            VerifyAreEqual("Найдено записей: 13", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter count cards min/max imvalid symbols");

            GoToAdmin("cards");
            Functions.GridFilterSet(driver, baseURL, name: "_noopColumnCustomerField_6");

            //check filter min not exist
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).SendKeys("1000");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "filter min not exist");

            //check max not exist
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).SendKeys("1000");
            VerifyAreEqual("Найдено записей: 4", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter max not exist");

            //check min and max not exist
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).SendKeys("1000");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).SendKeys("1000");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "filter min/max not exist");

            //check filter 
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).SendKeys("8");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).SendKeys("10");
            VerifyAreEqual("Найдено записей: 3", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter customer field number");

            VerifyAreEqual("530808", GetGridCell(0, "CardNumber").FindElement(By.TagName("a")).Text, "filter result card num 1");
            VerifyAreEqual("LastName8 FirstName8", GetGridCell(0, "FIO").Text, "filter result FIO 1");
            VerifyAreEqual("530809", GetGridCell(1, "CardNumber").FindElement(By.TagName("a")).Text, "filter result card num 2");
            VerifyAreEqual("LastName9 FirstName9", GetGridCell(1, "FIO").Text, "filter result FIO 2");
            VerifyAreEqual("530810", GetGridCell(2, "CardNumber").FindElement(By.TagName("a")).Text, "filter result card num 3");
            VerifyAreEqual("LastName10 FirstName10", GetGridCell(2, "FIO").Text, "filter result FIO 3");

            //check delete with filter
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Functions.GridDropdownDelete(driver, baseURL);

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "delete filtered items");

            //check delete filter
            ScrollTo(By.Id("header-top"));
            Functions.GridFilterClose(driver, baseURL, name: "_noopColumnCustomerField_6");
            Refresh();
            VerifyAreEqual("Найдено записей: 10", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter deleting 1");

            GoToAdmin("cards");
            VerifyAreEqual("Найдено записей: 10", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter deleting 2");

            VerifyFinally(testname);
        }


        [Test]
        public void FilterCustomerFieldMultiLinesText()
        {
            testname = "FilterCustomerFieldMultiLinesText";
            VerifyBegin(testname);

            //check filter not exist
            Functions.GridFilterSet(driver, baseURL, name: "_noopColumnCustomerField_8");

            //search by not exist card
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("many");
            DropFocus("h1");
            Blur();
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "no such element");

            //search too much symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("1111111111222222222222222222333333333333334444444444rrrrrrrrrrr");
            DropFocus("h1");
            Blur();
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "too much symbols");

            //search invalid symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("########@@@@@@@@&&&&&&&******,,,,..");
            DropFocus("h1");
            Blur();
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "invalid symbols");

            //check filter exist
            Functions.GridFilterClose(driver, baseURL, name: "_noopColumnCustomerField_8");
            Functions.GridFilterSet(driver, baseURL, name: "_noopColumnCustomerField_7");

            //search by exist card
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("many");
            DropFocus("h1");
            Blur();

            VerifyAreEqual("Найдено записей: 1", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter count items");
            VerifyAreEqual("530811", GetGridCell(0, "CardNumber").FindElement(By.TagName("a")).Text, "filter result Card Number");
            VerifyAreEqual("LastName11 FirstName11", GetGridCell(0, "FIO").Text, "filter result FIO");

            string strUrl = driver.Url;

            //check go to edit and back 
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".link-invert.ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(4000);
            WaitForElem(By.XPath("//h2[contains(text(), 'Карта')]"));
            VerifyIsTrue(driver.Url.Contains("edit"), "card edit");

            driver.Navigate().GoToUrl(strUrl);

            WaitForElem(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total"));
            VerifyAreEqual("Найдено записей: 1", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter return");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"_noopColumnCustomerField_7\"]")).Displayed);

            //check delete with filter
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Functions.GridDropdownDelete(driver, baseURL);

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "delete filtered items");

            //check delete filter
            ScrollTo(By.Id("header-top"));
            Functions.GridFilterClose(driver, baseURL, name: "_noopColumnCustomerField_7");
            Refresh();
            VerifyAreEqual("Найдено записей: 12", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter deleting 1");

            GoToAdmin("cards");
            VerifyAreEqual("Найдено записей: 12", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter deleting 2");

            VerifyFinally(testname);
        }

        [Test]
        public void FilterCustomerFieldDate()
        {
            testname = "FilterCustomerFieldDate";
            VerifyBegin(testname);

            //check filter not exist
            Functions.GridFilterSet(driver, baseURL, name: "_noopColumnCustomerField_10");

            //check filter min not exist
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).SendKeys("31.12.2050");
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Clear();
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "filter customer field date min not exist");

            //check max not exist
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).SendKeys("31.12.2050");
            VerifyIsFalse(driver.PageSource.Contains("Ни одной записи не найдено"), "filter customer field date max not exist");

            //check min and max not exist
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).SendKeys("31.12.2050");
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).SendKeys("31.12.2050");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "filter customer field date min/max not exist");

            //check filter 
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).SendKeys("01.03.2017");
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).SendKeys("25.05.2017");
            VerifyAreEqual("Найдено записей: 2", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter customer field date");

            VerifyAreEqual("530812", GetGridCell(0, "CardNumber").FindElement(By.TagName("a")).Text, "filter customer field date line 1");
            VerifyAreEqual("LastName12 FirstName12", GetGridCell(0, "FIO").Text, "filter customer field date FIO line 1");
            VerifyAreEqual("530813", GetGridCell(1, "CardNumber").FindElement(By.TagName("a")).Text, "filter field date line 2");
            VerifyAreEqual("LastName13 FirstName13", GetGridCell(1, "FIO").Text, "filter customer field date FIO line 2");

            //check delete with filter
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Functions.GridDropdownDelete(driver, baseURL);

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "delete filtered items");

            //check delete filter
            ScrollTo(By.Id("header-top"));
            Functions.GridFilterClose(driver, baseURL, name: "_noopColumnCustomerField_10");
            Refresh();
            VerifyAreEqual("Найдено записей: 11", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter deleting 1");

            GoToAdmin("cards");
            VerifyAreEqual("Найдено записей: 11", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter deleting 2");

            VerifyFinally(testname);
        }
    }
}

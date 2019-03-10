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
    public class BonusSystemCardFilter : BaseMultiSeleniumTest
    {
        [SetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog | ClearType.Customers | ClearType.Bonuses);
            InitializeService.LoadData(

             "Data\\Admin\\Settings\\BonusSystem\\BonusCardFilter\\Catalog.Product.csv",
           "Data\\Admin\\Settings\\BonusSystem\\BonusCardFilter\\Catalog.Offer.csv",
           "Data\\Admin\\Settings\\BonusSystem\\BonusCardFilter\\Catalog.Category.csv",
          "Data\\Admin\\Settings\\BonusSystem\\BonusCardFilter\\Catalog.ProductCategories.csv",
               "Data\\Admin\\Settings\\BonusSystem\\BonusCardFilter\\Bonus.Grade.csv",
           "Data\\Admin\\Settings\\BonusSystem\\BonusCardFilter\\Bonus.Card.csv",
           "Data\\Admin\\Settings\\BonusSystem\\BonusCardFilter\\Customers.CustomerGroup.csv",
           "data\\Admin\\Settings\\BonusSystem\\BonusCardFilter\\Customers.Country.csv",
            "data\\Admin\\Settings\\BonusSystem\\BonusCardFilter\\Customers.Region.csv",
            "data\\Admin\\Settings\\BonusSystem\\BonusCardFilter\\Customers.City.csv",
            "data\\Admin\\Settings\\BonusSystem\\BonusCardFilter\\Customers.Customer.csv",
            "data\\Admin\\Settings\\BonusSystem\\BonusCardFilter\\Customers.Contact.csv"
                );
            Init();

            GoToAdmin("cards");
        }

         

        [Test]
        public void CardFilterNum()
        {
            testname = "CardFilterNum";
            VerifyBegin(testname);

            //check filter 
            Functions.GridFilterSet(driver, baseURL, name: "CardNumber");

            //search by not exist card
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("53624351");
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

            //search by exist card
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("530899");
            Blur();

            VerifyAreEqual("530899", GetGridCell(0, "CardNumber").FindElement(By.TagName("a")).Text, "filter result Card Number");
            VerifyAreEqual("LastName99 FirstName99", GetGridCell(0, "FIO").Text, "filter result FIO");
            VerifyAreEqual("Платиновый", GetGridCell(0, "GradeName").Text, "filter result GradeName");
            VerifyAreEqual("30", GetGridCell(0, "GradePersent").Text, "filter result Grade Percent");
            VerifyAreEqual("12.01.2017 15:40", GetGridCell(0, "CreatedFormatted").Text, "filter result card date");

            string strUrl = driver.Url;

            //check go to edit and back 
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".link-invert.ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(4000);
            WaitForElem(By.XPath("//h2[contains(text(), 'Карта')]"));
            VerifyIsTrue(driver.Url.Contains("edit"), "card edit");

            driver.Navigate().GoToUrl(strUrl);
            Thread.Sleep(2000);

            WaitForElem(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total"));
            VerifyAreEqual("Найдено записей: 1", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter card num return");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"CardNumber\"]")).Displayed);

            //check delete with filter
            GetGridCell(0, "_serviceColumn").FindElement(By.TagName("ui-grid-custom-delete")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "delete filtered items");

            //check delete filter
            ScrollTo(By.Id("header-top"));
            Functions.GridFilterClose(driver, baseURL, name: "CardNumber");
            Refresh();
            VerifyAreEqual("Найдено записей: 119", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter card num deleting 1");

            GoToAdmin("cards");
            VerifyAreEqual("Найдено записей: 119", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter card num deleting 2");

            VerifyFinally(testname);
        }


        [Test]
        public void CardFilterGrade()
        {
            testname = "CardFilterGrade";
            VerifyBegin(testname);

            Functions.GridFilterSet(driver, baseURL, name: "GradeName");

            //check filter GradeName no items
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Серебряный");
            WaitForAjax();
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter GradeName no items count");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "filter GradeName no items");

            //check filter GradeName
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Золотой");
            WaitForAjax();
            VerifyAreEqual("Найдено записей: 23", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter GradeName");

            VerifyAreEqual("530868", GetGridCell(0, "CardNumber").FindElement(By.TagName("a")).Text, "filter result GradeName");
            VerifyAreEqual("LastName68 FirstName68", GetGridCell(0, "FIO").Text, "filter result FIO");
            VerifyAreEqual("Золотой", GetGridCell(0, "GradeName").Text, "filter result GradeName");
            VerifyAreEqual("10", GetGridCell(0, "GradePersent").Text, "filter result Grade Percent");
            VerifyAreEqual("12.02.2017 15:40", GetGridCell(0, "CreatedFormatted").Text, "filter result card date");

            //check all GradeName
            IWebElement selectElem = driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]"));
            SelectElement select = new SelectElement(selectElem);

            IList<IWebElement> allOptions = select.Options;

            VerifyIsTrue(allOptions.Count == 6, "count managers");  //5 GradeNames + null option 

            string strUrl = driver.Url;

            //check go to edit and back 
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".link-invert.ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(4000);
            WaitForElem(By.XPath("//h2[contains(text(), 'Карта')]"));
            VerifyIsTrue(driver.Url.Contains("edit"), "card edit");

            driver.Navigate().GoToUrl(strUrl);
            Thread.Sleep(2000);

            WaitForElem(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total"));
            VerifyAreEqual("Найдено записей: 23", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter GradeName return");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"GradeName\"]")).Displayed);

            //check delete with filter
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Functions.GridDropdownDelete(driver, baseURL);

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "delete filtered items");

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "delete filtered items");

            //check delete filter
            ScrollTo(By.Id("header-top"));
            Functions.GridFilterClose(driver, baseURL, name: "GradeName");
            Refresh();
            VerifyAreEqual("Найдено записей: 97", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter GradeName deleting 1");

            GoToAdmin("cards");
            VerifyAreEqual("Найдено записей: 97", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter GradeName deleting 2");

            VerifyFinally(testname);
        }

        [Test]
        public void CardFilterCreatedDate()
        {
            testname = "CardFilterCreatedDate";
            VerifyBegin(testname);

            //check filter 
            Functions.GridFilterSet(driver, baseURL, name: "CreatedFormatted");

            //check filter min not exist
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).SendKeys("31.12.2050 00:00");
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Clear();
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "filter add date min not exist");

            //check max not exist
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).SendKeys("31.12.2050 00:00");
            VerifyAreEqual("Найдено записей: 120", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter add date max not exist");

            //check min and max not exist
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).SendKeys("31.12.2050 00:00");
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).SendKeys("31.12.2050 00:00");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "filter add date  min/max not exist");

            //check filter add date 
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).SendKeys("01.03.2017 00:00");
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).SendKeys("05.05.2017 00:00");
            VerifyAreEqual("Найдено записей: 51", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter add date");

            VerifyAreEqual("530801", GetGridCell(0, "CardNumber").FindElement(By.TagName("a")).Text, "card num add date line 1");
            VerifyIsTrue(GetGridCell(0, "CreatedFormatted").Text.Contains("2017"), "add date line 1");
            VerifyAreEqual("530810", GetGridCell(9, "CardNumber").FindElement(By.TagName("a")).Text, "card num add date line 10");
            VerifyIsTrue(GetGridCell(9, "CreatedFormatted").Text.Contains("2017"), "add date line 10");

            //check delete with filter
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Functions.GridDropdownDelete(driver, baseURL);

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "delete filtered items");

            //check delete filter
            ScrollTo(By.Id("header-top"));
            Functions.GridFilterClose(driver, baseURL, name: "CreatedFormatted");
            VerifyAreEqual("Найдено записей: 69", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter add date after deleting 1");

            GoToAdmin("cards");
            VerifyAreEqual("Найдено записей: 69", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter add date after deleting 2");

            VerifyFinally(testname);
        }

        [Test]
        public void CardFilterBonusAmount()
        {
            testname = "CardFilterBonusAmount";
            VerifyBegin(testname);

            //check filter 
            Functions.GridFilterSet(driver, baseURL, name: "_noopColumnBonusAmount");

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
            //DropFocus("h1");
            VerifyAreEqual("Найдено записей: 120", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter max many symbols");

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
            VerifyAreEqual("Найдено записей: 120", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter count cards min many symbols");

            GoToAdmin("cards");
            Functions.GridFilterSet(driver, baseURL, name: "_noopColumnBonusAmount");

            //check max invalid symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).SendKeys("########@@@@@@@@&&&&&&&******");
            VerifyAreEqual("", driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Text, "filter max imvalid symbols");
            VerifyAreEqual("Найдено записей: 120", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter count cards max many symbols");

            //check min and max invalid symbols

            GoToAdmin("cards");
            Functions.GridFilterSet(driver, baseURL, name: "_noopColumnBonusAmount");

            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).SendKeys("########@@@@@@@@&&&&&&&******");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).SendKeys("########@@@@@@@@&&&&&&&******");
            VerifyAreEqual("", driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Text, "filter both min imvalid symbols");
            VerifyAreEqual("", driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Text, "filter both max imvalid symbols");
            VerifyAreEqual("Найдено записей: 120", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter count cards min/max many symbols");

            GoToAdmin("cards");
            Functions.GridFilterSet(driver, baseURL, name: "_noopColumnBonusAmount");

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
            VerifyAreEqual("Найдено записей: 120", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter max not exist");

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
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).SendKeys("40");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).SendKeys("99");
            VerifyAreEqual("Найдено записей: 60", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter bonus amount");
            
            VerifyAreEqual("530840", GetGridCell(0, "CardNumber").Text, "bonus amount card num line 1");
            VerifyAreEqual("530849", GetGridCell(9, "CardNumber").FindElement(By.TagName("a")).Text, "bonus amount card num line 10");

            //check delete with filter
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Functions.GridDropdownDelete(driver, baseURL);

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "delete filtered items");

            //check delete filter
            ScrollTo(By.Id("header-top"));
            Functions.GridFilterClose(driver, baseURL, name: "_noopColumnBonusAmount");
            VerifyAreEqual("Найдено записей: 60", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter bonus amount after deleting 1");

            GoToAdmin("cards");
            VerifyAreEqual("Найдено записей: 60", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter bonus amount after deleting 2");

            VerifyFinally(testname);
        }

        [Test]
        public void CardFilterFIO()
        {
            testname = "CardFilterFIO";
            VerifyBegin(testname);

            //check filter 
            Functions.GridFilterSet(driver, baseURL, name: "_noopColumnFIO");

            //search by not exist Contact
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("123123123 name contact 3");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "no such element");

            //search too much symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("1111111111222222222222222222333333333333334444444444rrrrrrrrrrrreeeeeeeeewwwwwwwwwwwwwwwwwww");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "too much symbols");

            //search invalid symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("########@@@@@@@@&&&&&&&******,,,,..");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "invalid symbols");

            //search by exist FIO
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("LastName2");

            VerifyAreEqual("530802", GetGridCell(0, "CardNumber").FindElement(By.TagName("a")).Text, "CardNumber line 1");
            VerifyAreEqual("LastName2 FirstName2", GetGridCell(0, "FIO").Text, "FIO line 1");
            VerifyAreEqual("530828", GetGridCell(9, "CardNumber").FindElement(By.TagName("a")).Text, "CardNumber line 10");
            VerifyAreEqual("LastName28 FirstName28", GetGridCell(9, "FIO").Text, "FIO line 10");
            VerifyAreEqual("Найдено записей: 11", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter FIO");

            string strUrl = driver.Url;

            //check go to edit and back 
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(4000);
            WaitForElem(By.XPath("//h2[contains(text(), 'Карта')]"));
            VerifyIsTrue(driver.Url.Contains("edit"), "card edit");

            driver.Navigate().GoToUrl(strUrl);
            Thread.Sleep(2000);

            WaitForElem(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total"));
            VerifyAreEqual("Найдено записей: 11", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter FIO return");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"_noopColumnFIO\"]")).Displayed);

            //check delete with filter
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Functions.GridDropdownDelete(driver, baseURL);

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "delete filtered items");

            //check delete filter
            ScrollTo(By.Id("header-top"));
            Functions.GridFilterClose(driver, baseURL, name: "_noopColumnFIO");
            VerifyAreEqual("Найдено записей: 109", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter FIO deleting 1");

            GoToAdmin("cards");
            VerifyAreEqual("Найдено записей: 109", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter FIO deleting 2");

            VerifyFinally(testname);
        }

        [Test]
        public void CardFilterEmail()
        {
            testname = "CardFilterEmail";
            VerifyBegin(testname);

            //check filter 
            Functions.GridFilterSet(driver, baseURL, name: "_noopColumnEmail");

            //search by not exist Contact
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("LastName2 FirstName2");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "no such element");

            //search too much symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("1111111111222222222222222222333333333333334444444444rrrrrrrrrrrreeeeeeeeewwwwwwwwwwwwwwwwwww");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "too much symbols");

            //search invalid symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("########@@@@@@@@&&&&&&&******,,,,..");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "invalid symbols");

            //search by exist email
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("test@mail.ru3");

            VerifyAreEqual("530803", GetGridCell(0, "CardNumber").FindElement(By.TagName("a")).Text, "CardNumber line 1");
            VerifyAreEqual("LastName3 FirstName3", GetGridCell(0, "FIO").Text, "FIO line 1");
            VerifyAreEqual("530838", GetGridCell(9, "CardNumber").FindElement(By.TagName("a")).Text, "CardNumber line 10");
            VerifyAreEqual("LastName38 FirstName38", GetGridCell(9, "FIO").Text, "FIO line 10");
            VerifyAreEqual("Найдено записей: 11", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter email");

            string strUrl = driver.Url;

            //check go to edit and back 
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(4000);
            WaitForElem(By.XPath("//h2[contains(text(), 'Карта')]"));
            VerifyIsTrue(driver.Url.Contains("edit"), "card edit");

            driver.Navigate().GoToUrl(strUrl);
            Thread.Sleep(2000);

            WaitForElem(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total"));
            VerifyAreEqual("Найдено записей: 11", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter email return");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"_noopColumnEmail\"]")).Displayed);

            //check delete with filter
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Functions.GridDropdownDelete(driver, baseURL);

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "delete filtered items");

            //check delete filter
            ScrollTo(By.Id("header-top"));
            Functions.GridFilterClose(driver, baseURL, name: "_noopColumnEmail");
            VerifyAreEqual("Найдено записей: 109", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter email deleting 1");

            GoToAdmin("cards");
            VerifyAreEqual("Найдено записей: 109", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter email deleting 2");

            VerifyFinally(testname);
        }

        [Test]
        public void CardFilterMobile()
        {
            testname = "CardFilterMobile";
            VerifyBegin(testname);

            //check filter 
            Functions.GridFilterSet(driver, baseURL, name: "_noopColumnMobilePhone");

            //search by not exist Contact
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("89279272727");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "no such element");

            //search too much symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("1111111111222222222222222222333333333333334444444444rrrrrrrrrrrreeeeeeeeewwwwwwwwwwwwwwwwwww");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "too much symbols");

            //search invalid symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("########@@@@@@@@&&&&&&&******,,,,..");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "invalid symbols");

            //search by exist mobile
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("3");

            VerifyAreEqual("530803", GetGridCell(0, "CardNumber").FindElement(By.TagName("a")).Text, "CardNumber line 1");
            VerifyAreEqual("LastName3 FirstName3", GetGridCell(0, "FIO").Text, "FIO line 1");
            VerifyAreEqual("530836", GetGridCell(9, "CardNumber").FindElement(By.TagName("a")).Text, "CardNumber line 9");
            VerifyAreEqual("LastName36 FirstName36", GetGridCell(9, "FIO").Text, "FIO line 9");
            VerifyAreEqual("Найдено записей: 21", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter mobile");

            string strUrl = driver.Url;

            //check go to edit and back 
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(4000);
            WaitForElem(By.XPath("//h2[contains(text(), 'Карта')]"));
            VerifyIsTrue(driver.Url.Contains("edit"), "card edit");

            driver.Navigate().GoToUrl(strUrl);
            Thread.Sleep(2000);

            WaitForElem(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total"));
            VerifyAreEqual("Найдено записей: 21", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter mobile return");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"_noopColumnMobilePhone\"]")).Displayed);

            //check delete with filter
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Functions.GridDropdownDelete(driver, baseURL);

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "delete filtered items");

            //check delete filter
            ScrollTo(By.Id("header-top"));
            Functions.GridFilterClose(driver, baseURL, name: "_noopColumnMobilePhone");
            VerifyAreEqual("Найдено записей: 99", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter mobile deleting 1");

            GoToAdmin("cards");
            VerifyAreEqual("Найдено записей: 99", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter mobile deleting 2");

            VerifyFinally(testname);
        }

        [Test]
        public void CardFilterRegisterDate()
        {
            testname = "CardFilterRegisterDate";
            VerifyBegin(testname);

            //check filter 
            Functions.GridFilterSet(driver, baseURL, name: "_noopColumnRegDate");

            //check filter min not exist
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).SendKeys("31.12.2050 00:00");
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Clear();
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "filter reg date min not exist");

            //check max not exist
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).SendKeys("31.12.2050 00:00");
            VerifyAreEqual("Найдено записей: 120", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter reg date max not exist");

            //check min and max not exist
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).SendKeys("31.12.2050 00:00");
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).SendKeys("31.12.2050 00:00");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "filter reg date min/max not exist");

            //check filter reg date 
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).SendKeys("01.07.2017 12:00");
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).SendKeys("15.08.2017 21:00");
            VerifyAreEqual("Найдено записей: 46", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter reg date");

            VerifyAreEqual("530875", GetGridCell(0, "CardNumber").FindElement(By.TagName("a")).Text, "card num reg date line 1");
            VerifyAreEqual("530884", GetGridCell(9, "CardNumber").FindElement(By.TagName("a")).Text, "card num reg date line 10");

            //check delete with filter
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Functions.GridDropdownDelete(driver, baseURL);

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "delete filtered items");

            //check delete filter
            ScrollTo(By.Id("header-top"));
            Functions.GridFilterClose(driver, baseURL, name: "_noopColumnRegDate");
            VerifyAreEqual("Найдено записей: 74", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter reg date after deleting 1");

            GoToAdmin("cards");
            VerifyAreEqual("Найдено записей: 74", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter reg date after deleting 2");

            VerifyFinally(testname);
        }

        [Test]
        public void CardFilterLocation()
        {
            testname = "CardFilterLocation";
            VerifyBegin(testname);

            //check filter 
            Functions.GridFilterSet(driver, baseURL, name: "_noopColumnLocation");

            //search by not exist Contact
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("Самара");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "no such element");

            //search too much symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("1111111111222222222222222222333333333333334444444444rrrrrrrrrrrreeeeeeeeewwwwwwwwwwwwwwwwwww");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "too much symbols");

            //search invalid symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("########@@@@@@@@&&&&&&&******,,,,..");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "invalid symbols");

            //search by exist location
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("Москва");

            VerifyAreEqual("530801", GetGridCell(0, "CardNumber").FindElement(By.TagName("a")).Text, "CardNumber line 1");
            VerifyAreEqual("LastName1 FirstName1", GetGridCell(0, "FIO").Text, "FIO line 1");
            VerifyAreEqual("530810", GetGridCell(9, "CardNumber").FindElement(By.TagName("a")).Text, "CardNumber line 10");
            VerifyAreEqual("LastName10 FirstName10", GetGridCell(9, "FIO").Text, "FIO line 10");
            VerifyAreEqual("Найдено записей: 47", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter location");

            string strUrl = driver.Url;

            //check go to edit and back 
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(4000);
            WaitForElem(By.XPath("//h2[contains(text(), 'Карта')]"));
            VerifyIsTrue(driver.Url.Contains("edit"), "card edit");

            driver.Navigate().GoToUrl(strUrl);
            Thread.Sleep(2000);

            WaitForElem(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total"));
            VerifyAreEqual("Найдено записей: 47", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter location return");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"_noopColumnLocation\"]")).Displayed);

            //check delete with filter
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Functions.GridDropdownDelete(driver, baseURL);

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "delete filtered items");

            //check delete filter
            ScrollTo(By.Id("header-top"));
            Functions.GridFilterClose(driver, baseURL, name: "_noopColumnLocation");
            VerifyAreEqual("Найдено записей: 73", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter location deleting 1");

            GoToAdmin("cards");
            VerifyAreEqual("Найдено записей: 73", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter location deleting 2");

            VerifyFinally(testname);
        }


    }
}

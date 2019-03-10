using System;
using NUnit.Framework;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;
using System.Threading;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.SeleniumTest.Admin.Marketing.Voting
{
    [TestFixture]
    public class VotingFilterTest : BaseMultiSeleniumTest
    {
        [SetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.CMS);
            InitializeService.LoadData(
        "data\\Admin\\Voting\\Voice.Answer.csv",
        "data\\Admin\\Voting\\Voice.VoiceTheme.csv"
          );

            Init();
            GoToAdmin("voting");
        }

        [Test]
        public void FilterByTheme()
        {
            testname = "VotingFilterByTheme";
            VerifyBegin(testname);

            //check filter 
            Functions.GridFilterSet(driver, baseURL, name: "Name");

            //search by not exist 
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("Test Voice theme");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "no such element");

            //search too much symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("111111111122222222222222222222222222222");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "too much symbols");

            //search by exist
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("VoiceTheme4");

            VerifyAreEqual("VoiceTheme4", GetGridCell(0, "Name", "Voting").Text, "filter Theme line 1");
            VerifyAreEqual("VoiceTheme48", GetGridCell(9, "Name", "Voting").Text, "filter Theme line 10");
            VerifyAreEqual("Найдено записей: 11", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter Theme count");

            //check go to edit and back 
            GetGridCell(0, "_serviceColumn", "Voting").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(4000);
            WaitForElem(By.CssSelector(".modal-content"));

            VerifyAreEqual("Редактирование голосования", driver.FindElement(By.TagName("h2")).Text, "pop up h2");
            XPathContainsText("button", "Отмена");

            VerifyAreEqual("Найдено записей: 11", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter Theme return");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"Name\"]")).Displayed);

            //check delete with filter
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Functions.GridDropdownDelete(driver, baseURL);

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "delete filtered items");

            //check delete filter
            ScrollTo(By.Id("header-top"));
            Functions.GridFilterClose(driver, baseURL, name: "Name");
            VerifyAreEqual("Найдено записей: 159", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter Theme deleting 1");

            GoToAdmin("voting");
            VerifyAreEqual("Найдено записей: 159", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter Theme deleting 2");

            VerifyFinally(testname);
        }

        [Test]
        public void FilterIsDefault()
        {
            testname = "VotingFilterIsDefault";
            VerifyBegin(testname);

            //check filter default
            Functions.GridFilterSet(driver, baseURL, name: "IsDefault");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Текущие");
            VerifyAreEqual("Найдено записей: 1", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter IsDefault count");
            VerifyAreEqual("VoiceTheme7", GetGridCell(0, "Name", "Voting").Text, "filter IsDefault name");
            VerifyIsTrue(GetGridCell(0, "IsDefault", "Voting").FindElement(By.CssSelector("input")).Selected, "filter IsDefault checkbox");

            //check filter not default
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Не текущие");
            VerifyAreEqual("Найдено записей: 169", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter no IsDefault count");

            VerifyAreEqual("VoiceTheme1", GetGridCell(0, "Name", "Voting").Text, "filter no IsDefault line 1");
            VerifyAreEqual("VoiceTheme11", GetGridCell(9, "Name", "Voting").Text, "filter no IsDefault line 10");
            
            //check delete with filter
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Functions.GridDropdownDelete(driver, baseURL);

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "delete filtered items");

            //check delete filter
            ScrollTo(By.Id("header-top"));
            Functions.GridFilterClose(driver, baseURL, name: "IsDefault");
            VerifyAreEqual("Найдено записей: 1", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter IsDefault after deleting 1");

            GoToAdmin("voting");
            VerifyAreEqual("Найдено записей: 1", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter IsDefault after deleting 2");

            VerifyFinally(testname);
        }


        [Test]
        public void FilterIsHaveNullVoice()
        {
            testname = "VotingFilterIsHaveNullVoice";
            VerifyBegin(testname);

            //check filter default
            Functions.GridFilterSet(driver, baseURL, name: "IsHaveNullVoice");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Имеют пустые голоса");
            VerifyAreEqual("Найдено записей: 10", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter IsHaveNullVoice count");
            VerifyAreEqual("VoiceTheme161", GetGridCell(0, "Name", "Voting").Text, "filter IsHaveNullVoice name line 1");
            VerifyAreEqual("VoiceTheme170", GetGridCell(9, "Name", "Voting").Text, "filter IsHaveNullVoice name line 10");
            VerifyIsTrue(GetGridCell(0, "IsHaveNullVoice", "Voting").FindElement(By.CssSelector("input")).Selected, "filter IsHaveNullVoice checkbox line 1");
            VerifyIsTrue(GetGridCell(9, "IsHaveNullVoice", "Voting").FindElement(By.CssSelector("input")).Selected, "filter IsHaveNullVoice checkbox line 10");

            //check filter not default
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Не имеют пустые голоса");
            VerifyAreEqual("Найдено записей: 160", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter no IsHaveNullVoice count");
            VerifyAreEqual("VoiceTheme1", GetGridCell(0, "Name", "Voting").Text, "filter no IsHaveNullVoice name line 1");
            VerifyAreEqual("VoiceTheme10", GetGridCell(9, "Name", "Voting").Text, "filter no IsHaveNullVoice name line 10");
            VerifyIsFalse(GetGridCell(0, "IsHaveNullVoice", "Voting").FindElement(By.CssSelector("input")).Selected, "filter no IsHaveNullVoice checkbox line 1");
            VerifyIsFalse(GetGridCell(9, "IsHaveNullVoice", "Voting").FindElement(By.CssSelector("input")).Selected, "filter no IsHaveNullVoice checkbox line 10");

            //check delete with filter
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Functions.GridDropdownDelete(driver, baseURL);

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "delete filtered items");

            //check delete filter
            ScrollTo(By.Id("header-top"));
            Functions.GridFilterClose(driver, baseURL, name: "IsHaveNullVoice");
            VerifyAreEqual("Найдено записей: 10", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter IsHaveNullVoice after deleting 1");

            GoToAdmin("voting");
            VerifyAreEqual("Найдено записей: 10", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter IsHaveNullVoice after deleting 2");

            VerifyFinally(testname);
        }

        [Test]
        public void FilterIsClose()
        {
            testname = "VotingFilterIsClose";
            VerifyBegin(testname);

            //check filter default
            Functions.GridFilterSet(driver, baseURL, name: "IsClose");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Закрытые");
            VerifyAreEqual("Найдено записей: 37", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter IsClose count");
            VerifyAreEqual("VoiceTheme64", GetGridCell(0, "Name", "Voting").Text, "filter IsClose name line 1");
            VerifyAreEqual("VoiceTheme73", GetGridCell(9, "Name", "Voting").Text, "filter IsClose name line 10");
            VerifyIsTrue(GetGridCell(0, "IsClose", "Voting").FindElement(By.CssSelector("input")).Selected, "filter IsClose checkbox line 1");
            VerifyIsTrue(GetGridCell(9, "IsClose", "Voting").FindElement(By.CssSelector("input")).Selected, "filter IsClose checkbox line 10");

            //check filter not default
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Не закрытые");
            VerifyAreEqual("Найдено записей: 133", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter no IsClose count");
            VerifyAreEqual("VoiceTheme1", GetGridCell(0, "Name", "Voting").Text, "filter no IsClose name line 1");
            VerifyAreEqual("VoiceTheme10", GetGridCell(9, "Name", "Voting").Text, "filter no IsClose name line 10");
            VerifyIsFalse(GetGridCell(0, "IsClose", "Voting").FindElement(By.CssSelector("input")).Selected, "filter no IsClose checkbox line 1");
            VerifyIsFalse(GetGridCell(9, "IsClose", "Voting").FindElement(By.CssSelector("input")).Selected, "filter no IsClose checkbox line 10");

            //check delete with filter
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Functions.GridDropdownDelete(driver, baseURL);

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "delete filtered items");

            //check delete filter
            ScrollTo(By.Id("header-top"));
            Functions.GridFilterClose(driver, baseURL, name: "IsClose");
            VerifyAreEqual("Найдено записей: 37", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter IsClose after deleting 1");

            GoToAdmin("voting");
            VerifyAreEqual("Найдено записей: 37", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter IsClose after deleting 2");

            VerifyFinally(testname);
        }

        [Test]
        public void FilterByDateAdded()
        {
            testname = "VotingFilterByDateAdded";
            VerifyBegin(testname);

            Functions.GridFilterSet(driver, baseURL, name: "DateAdded");

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
            VerifyAreEqual("Найдено записей: 170", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter add date max not exist");

            //check min and max not exist
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).SendKeys("31.12.2050 00:00");
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).SendKeys("31.12.2050 00:00");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "filter sum min/max not exist");

            //check filter add date
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).SendKeys("01.01.2013 00:00");
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).SendKeys("26.02.2013 23:00");
            VerifyAreEqual("Найдено записей: 55", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter add date");

            VerifyAreEqual("VoiceTheme114", GetGridCell(0, "Name", "Voting").Text, "filter add date name line 1");
            VerifyAreEqual("01.01.2013 14:52", GetGridCell(0, "DateAdded", "Voting").Text, "filter add date name add date line 1");
            VerifyAreEqual("VoiceTheme123", GetGridCell(9, "Name", "Voting").Text, "filter add date checkbox line 10");
            VerifyAreEqual("10.01.2013 13:52", GetGridCell(9, "DateAdded", "Voting").Text, "filter add date checkbox line 10");

            //check delete with filter
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Functions.GridDropdownDelete(driver, baseURL);

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "delete filtered items");

            //check delete filter
            ScrollTo(By.Id("header-top"));
            Functions.GridFilterClose(driver, baseURL, name: "DateAdded");
            VerifyAreEqual("Найдено записей: 115", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter add date after deleting 1");

            GoToAdmin("voting");
            VerifyAreEqual("Найдено записей: 115", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter add date after deleting 2");

            VerifyFinally(testname);
        }

        [Test]
        public void FilterByDateModify()
        {
            testname = "VotingFilterByDateModify";
            VerifyBegin(testname);

            Functions.GridFilterSet(driver, baseURL, name: "DateModify");

            //check filter min not exist
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).SendKeys("31.12.2050 00:00");
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Clear();
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "filter DateModify min not exist");

            //check max not exist
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).SendKeys("31.12.2050 00:00");
            VerifyAreEqual("Найдено записей: 170", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter DateModify max not exist");

            //check min and max not exist
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).SendKeys("31.12.2050 00:00");
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).SendKeys("31.12.2050 00:00");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "filter sum min/max not exist");

            //check filter DateModify
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).SendKeys("01.04.2014 00:00");
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).SendKeys("28.04.2014 23:00");
            VerifyAreEqual("Найдено записей: 27", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter DateModify count");

            VerifyAreEqual("VoiceTheme74", GetGridCell(0, "Name", "Voting").Text, "filter DateModify name line 1");
            VerifyAreEqual("01.04.2014 15:26", GetGridCell(0, "DateModify", "Voting").Text, "filter DateModify name DateModify line 1");
            VerifyAreEqual("VoiceTheme83", GetGridCell(9, "Name", "Voting").Text, "filter DateModify checkbox line 10");
            VerifyAreEqual("11.04.2014 23:26", GetGridCell(9, "DateModify", "Voting").Text, "filter DateModify checkbox line 10");

            //check delete with filter
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Functions.GridDropdownDelete(driver, baseURL);

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "delete filtered items");

            //check delete filter
            ScrollTo(By.Id("header-top"));
            Functions.GridFilterClose(driver, baseURL, name: "DateModify");
            VerifyAreEqual("Найдено записей: 143", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter DateModify after deleting 1");

            GoToAdmin("voting");
            VerifyAreEqual("Найдено записей: 143", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter DateModify after deleting 2");

            VerifyFinally(testname);
        }

        [Test]
        public void FilterCountAnswers()
        {
            testname = "VotingFilterCountAnswers";
            VerifyBegin(testname);

            Functions.GridFilterSet(driver, baseURL, name: "CountAnswers");

            //check min too much symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).SendKeys("111111111");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "filter CountAnswers min many symbols");

            //check max too much symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).SendKeys("111111111");
            VerifyAreEqual("Найдено записей: 170", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter CountAnswers max many symbols");

            //check min and max too much symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).SendKeys("111111111");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).SendKeys("111111111");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "filter CountAnswers min/max many symbols");

            //check invalid symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Clear();

            //check min invalid symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).SendKeys("########@@@@@@@@&&&&&&&******");
            VerifyAreEqual("", driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Text, "filter CountAnswers min imvalid symbols");
            VerifyAreEqual("Найдено записей: 170", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter CountAnswers count min many symbols");

            GoToAdmin("voting");
            Functions.GridFilterSet(driver, baseURL, name: "CountAnswers");

            //check max invalid symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).SendKeys("########@@@@@@@@&&&&&&&******");
            VerifyAreEqual("", driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Text, "filter CountAnswers max imvalid symbols");
            VerifyAreEqual("Найдено записей: 170", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter CountAnswers count max many symbols");

            //check min and max invalid symbols

            GoToAdmin("voting");
            Functions.GridFilterSet(driver, baseURL, name: "CountAnswers");

            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).SendKeys("########@@@@@@@@&&&&&&&******");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).SendKeys("########@@@@@@@@&&&&&&&******");
            VerifyAreEqual("", driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Text, "filter CountAnswers both min imvalid symbols");
            VerifyAreEqual("", driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Text, "filter CountAnswers both max imvalid symbols");
            VerifyAreEqual("Найдено записей: 170", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter CountAnswers count min/max many symbols");

            GoToAdmin("voting");
            Functions.GridFilterSet(driver, baseURL, name: "CountAnswers");

            //check filter min not exist
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).SendKeys("1000");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "filter CountAnswers min not exist");

            //check max not exist
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).SendKeys("1000");
            VerifyAreEqual("Найдено записей: 170", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter CountAnswers max not exist");

            //check min and max not exist
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).SendKeys("1000");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).SendKeys("1000");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "filter CountAnswers min/max not exist");

            //check filter
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).SendKeys("3");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).SendKeys("5");
            VerifyAreEqual("Найдено записей: 3", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter CountAnswers count");

            VerifyAreEqual("VoiceTheme3", GetGridCell(0, "Name", "Voting").Text, "filter CountAnswers name line 1");
            VerifyAreEqual("3", GetGridCell(0, "CountAnswers", "Voting").Text, "filter CountAnswers name line 1");
            VerifyAreEqual("VoiceTheme4", GetGridCell(1, "Name", "Voting").Text, "filter CountAnswers checkbox line 2");
            VerifyAreEqual("4", GetGridCell(1, "CountAnswers", "Voting").Text, "filter CountAnswers checkbox line 2");

            //check delete with filter
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Functions.GridDropdownDelete(driver, baseURL);

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "delete filtered items");

            //check delete filter
            ScrollTo(By.Id("header-top"));
            Functions.GridFilterClose(driver, baseURL, name: "CountAnswers");
            VerifyAreEqual("Найдено записей: 167", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter CountAnswers after deleting 1");

            GoToAdmin("voting");
            VerifyAreEqual("Найдено записей: 167", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter CountAnswers after deleting 2");

            VerifyFinally(testname);
        }
    }
}

using System;
using NUnit.Framework;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;
using System.Threading;

namespace AdvantShop.SeleniumTest.Admin.Marketing.Voting.Answers
{
    [TestFixture]
    public class VotingAnswersTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.CMS);
            InitializeService.LoadData(
        "data\\Admin\\VotingAnswers\\Voice.Answer.csv",
        "data\\Admin\\VotingAnswers\\Voice.VoiceTheme.csv"
          );

            Init();
            GoToAdmin("voting");
            GetGridCell(0, "ID", "Voting").FindElement(By.TagName("a")).Click();
            Thread.Sleep(3000);
            ScrollTo(By.CssSelector("[data-e2e=\"gridFilterDropdownButton\"]"));
        }


        [Test]
        public void Grid()
        {
            testname = "VotingAnswersGrid";
            VerifyBegin(testname);
            
            VerifyIsTrue(driver.FindElement(By.TagName("h1")).Text.Contains("Варианты ответов"), "h1 page");

            VerifyAreEqual("Найдено записей: 105", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "grid count all");
            VerifyAreEqual("Answer 1", GetGridCell(0, "Name", "Answers").Text, "grid name");
            VerifyAreEqual("1", GetGridCell(0, "CountVoice", "Answers").Text, "grid CountVoice");
            VerifyAreEqual("1", GetGridCell(0, "SortOrder", "Answers").Text, "grid SortOrder");
            VerifyIsTrue(GetGridCell(0, "IsVisible", "Answers").FindElement(By.TagName("input")).Selected, "grid IsVisible");
            
            VerifyAreEqual("05.09.2012 11:55", GetGridCell(0, "DateAdded", "Answers").Text, "grid DateAdded");
            VerifyAreEqual("04.10.2014 12:23", GetGridCell(0, "DateModify", "Answers").Text, "grid DateModify");

            VerifyFinally(testname);
        }


        [Test]
        public void ToEdit()
        {
            testname = "VotingAnswersGoToEdit";
            VerifyBegin(testname);
            
            GetGridCell(0, "_serviceColumn", "Answers").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(3000);
            WaitForElem(By.CssSelector(".modal-content"));

            VerifyAreEqual("Редактирование варианта ответа", driver.FindElement(By.TagName("h2")).Text, "pop up h2");
            XPathContainsText("button", "Отмена");

            VerifyFinally(testname);
        }

      
        [Test]
        public void zSelectDelete()
        {
            testname = "VotingtAnswersSelectDelete";
            VerifyBegin(testname);

            //check delete cancel 
            GetGridCell(0, "_serviceColumn", "Answers").FindElement(By.TagName("ui-grid-custom-delete")).Click();
            driver.FindElement(By.ClassName("swal2-cancel")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Answer 1", GetGridCell(0, "Name", "Answers").Text, "1 grid canсel delete");
            VerifyAreEqual("Найдено записей: 105", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all canсel delete");

            //check delete
            GetGridCell(0, "_serviceColumn", "Answers").FindElement(By.TagName("ui-grid-custom-delete")).Click();
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);
            VerifyAreNotEqual("Answer 1", GetGridCell(0, "Name", "Answers").Text, "1 grid delete");

            //check select 
            GetGridCell(0, "selectionRowHeaderCol", "Answers").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            GetGridCell(1, "selectionRowHeaderCol", "Answers").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            GetGridCell(2, "selectionRowHeaderCol", "Answers").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            VerifyIsTrue(GetGridCell(0, "selectionRowHeaderCol", "Answers").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected 2 grid");
            VerifyIsTrue(GetGridCell(1, "selectionRowHeaderCol", "Answers").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected 3 grid");
            VerifyIsTrue(GetGridCell(2, "selectionRowHeaderCol", "Answers").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected 4 grid");
            VerifyAreEqual("3", driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text, "count selected");

            //check delete selected items
            Functions.GridDropdownDelete(driver, baseURL);
            VerifyAreEqual("Answer 102", GetGridCell(0, "Name", "Answers").Text, "selected 2 grid delete");
            VerifyAreEqual("Answer 103", GetGridCell(1, "Name", "Answers").Text, "selected 3 grid delete");
            VerifyAreEqual("Answer 104", GetGridCell(2, "Name", "Answers").Text, "selected 4 grid delete");

            //check select all on page
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            VerifyIsTrue(GetGridCell(0, "selectionRowHeaderCol", "Answers").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected all on page 1 grid");
            VerifyIsTrue(GetGridCell(9, "selectionRowHeaderCol", "Answers").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected all on page 10 grid");

            //check delete all on page
            Functions.GridDropdownDelete(driver, baseURL);
            VerifyAreEqual("Answer 17", GetGridCell(0, "Name", "Answers").Text, "selected all on page 1 grid delete");
            VerifyAreEqual("Answer 25", GetGridCell(9, "Name", "Answers").Text, "selected all on page 10 grid delete");

            //check select all
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            VerifyAreEqual("91", driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text, "count all selected after deleting");
            VerifyIsTrue(GetGridCell(0, "selectionRowHeaderCol", "Answers").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected all 1 grid delete");
            VerifyIsTrue(GetGridCell(9, "selectionRowHeaderCol", "Answers").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected all 10 grid delete");

            //check deselect all 
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDeselectAllFn\"]")).Click();
            VerifyIsTrue(!GetGridCell(0, "selectionRowHeaderCol", "Answers").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "deselect all 1 grid");
            VerifyIsTrue(!GetGridCell(9, "selectionRowHeaderCol", "Answers").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "deselect all 10 grid");

            //check delete all
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Functions.GridDropdownDelete(driver, baseURL);

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "1 delete all");
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "1 count all after deleting");

            GoToAdmin("voting");
            GetGridCell(0, "ID", "Voting").FindElement(By.TagName("a")).Click();
            Thread.Sleep(3000);
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "2 delete all");
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "2 count all after deleting");

            VerifyFinally(testname);
        }

    }
}

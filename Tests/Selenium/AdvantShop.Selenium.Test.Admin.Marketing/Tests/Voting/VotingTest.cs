using System;
using NUnit.Framework;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;
using System.Threading;

namespace AdvantShop.SeleniumTest.Admin.Marketing.Voting
{
    [TestFixture]
    public class VotingTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
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
        public void Grid()
        {
            testname = "VotingGrid";
            VerifyBegin(testname);

            VerifyIsTrue(driver.FindElement(By.TagName("h1")).Text.Contains("Голосование"), "h1 page");

            VerifyAreEqual("Найдено записей: 170", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "grid count all");
            VerifyAreEqual("VoiceTheme1", GetGridCell(0, "Name", "Voting").Text, "grid name");
            VerifyIsFalse(GetGridCell(0, "IsDefault", "Voting").FindElement(By.TagName("input")).Selected, "grid IsDefault");
            VerifyIsFalse(GetGridCell(0, "IsHaveNullVoice", "Voting").FindElement(By.TagName("input")).Selected, "grid IsHaveNullVoice");
            VerifyIsFalse(GetGridCell(0, "IsClose", "Voting").FindElement(By.TagName("input")).Selected, "grid IsClose");
            VerifyAreEqual("1", GetGridCell(0, "CountAnswers", "Voting").Text, "grid CountAnswers");
            VerifyAreEqual("05.09.2012 11:52", GetGridCell(0, "DateAdded", "Voting").Text, "grid DateAdded");
            VerifyAreEqual("15.01.2014 14:26", GetGridCell(0, "DateModify", "Voting").Text, "grid DateModify");

            VerifyIsTrue(GetGridCell(6, "IsDefault", "Voting").FindElement(By.TagName("input")).Selected, "grid IsDefault true");
            VerifyAreEqual("VoiceTheme7", GetGridCell(6, "Name", "Voting").Text, "grid name IsDefault");

            VerifyFinally(testname);
        }


        [Test]
        public void ToEdit()
        {
            testname = "VotingGoToEdit";
            VerifyBegin(testname);

            GetGridCell(0, "_serviceColumn", "Voting").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(3000);
            WaitForElem(By.CssSelector(".modal-content"));

            VerifyAreEqual("Редактирование голосования", driver.FindElement(By.TagName("h2")).Text, "pop up h2");
            XPathContainsText("button", "Отмена");

            VerifyFinally(testname);
        }

        [Test]
        public void ToAnswers()
        {
            testname = "VotingGoToAnswers";
            VerifyBegin(testname);

            GetGridCell(0, "ID", "Voting").FindElement(By.TagName("a")).Click();
            Thread.Sleep(3000);

            VerifyIsTrue(driver.FindElement(By.TagName("h1")).Text.Contains("Варианты ответов"), "h1 answers page");
            
            driver.FindElement(By.CssSelector("[data-e2e=\"btnBack\"]")).Click();
            Thread.Sleep(3000);

            VerifyIsTrue(driver.FindElement(By.TagName("h1")).Text.Contains("Голосование"), "h1 page return");
            VerifyAreEqual("VoiceTheme1", GetGridCell(0, "Name", "Voting").Text, "grid name return");

            VerifyFinally(testname);
        }


        [Test]
        public void zSelectDelete()
        {
            testname = "VotingtSelectDelete";
            VerifyBegin(testname);

            //check delete cancel 
            GetGridCell(0, "_serviceColumn", "Voting").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-times.link-invert")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-cancel")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("VoiceTheme1", GetGridCell(0, "Name", "Voting").Text, "1 grid canсel delete");
            VerifyAreEqual("Найдено записей: 170", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all canсel delete");

            //check delete
            GetGridCell(0, "_serviceColumn", "Voting").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-times.link-invert")).Click();
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);
            VerifyAreNotEqual("VoiceTheme1", GetGridCell(0, "Name", "Voting").Text, "1 grid delete");

            //check select 
            GetGridCell(0, "selectionRowHeaderCol", "Voting").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            GetGridCell(1, "selectionRowHeaderCol", "Voting").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            GetGridCell(2, "selectionRowHeaderCol", "Voting").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            VerifyIsTrue(GetGridCell(0, "selectionRowHeaderCol", "Voting").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected 2 grid");
            VerifyIsTrue(GetGridCell(1, "selectionRowHeaderCol", "Voting").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected 3 grid");
            VerifyIsTrue(GetGridCell(2, "selectionRowHeaderCol", "Voting").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected 4 grid");
            VerifyAreEqual("3", driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text, "count selected");

            //check delete selected items
            Functions.GridDropdownDelete(driver, baseURL);
            VerifyAreEqual("VoiceTheme5", GetGridCell(0, "Name", "Voting").Text, "selected 2 grid delete");
            VerifyAreEqual("VoiceTheme6", GetGridCell(1, "Name", "Voting").Text, "selected 3 grid delete");
            VerifyAreEqual("VoiceTheme7", GetGridCell(2, "Name", "Voting").Text, "selected 4 grid delete");

            //check select all on page
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            VerifyIsTrue(GetGridCell(0, "selectionRowHeaderCol", "Voting").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected all on page 1 grid");
            VerifyIsTrue(GetGridCell(9, "selectionRowHeaderCol", "Voting").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected all on page 10 grid");

            //check delete all on page
            Functions.GridDropdownDelete(driver, baseURL);
            VerifyAreEqual("VoiceTheme15", GetGridCell(0, "Name", "Voting").Text, "selected all on page 1 grid delete"); 
            VerifyAreEqual("VoiceTheme24", GetGridCell(9, "Name", "Voting").Text, "selected all on page 10 grid delete");

            //check select all
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            VerifyAreEqual("156", driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text, "count all selected after deleting");
            VerifyIsTrue(GetGridCell(0, "selectionRowHeaderCol", "Voting").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected all 1 grid delete");
            VerifyIsTrue(GetGridCell(9, "selectionRowHeaderCol", "Voting").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected all 10 grid delete");

            //check deselect all 
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDeselectAllFn\"]")).Click();
            VerifyIsTrue(!GetGridCell(0, "selectionRowHeaderCol", "Voting").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "deselect all 1 grid");
            VerifyIsTrue(!GetGridCell(9, "selectionRowHeaderCol", "Voting").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "deselect all 10 grid");

            //check delete all
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Functions.GridDropdownDelete(driver, baseURL);

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "1 delete all");
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "1 count all after deleting");

            GoToAdmin("voting");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "2 delete all");
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "2 count all after deleting");

            VerifyFinally(testname);
        }

    }
}

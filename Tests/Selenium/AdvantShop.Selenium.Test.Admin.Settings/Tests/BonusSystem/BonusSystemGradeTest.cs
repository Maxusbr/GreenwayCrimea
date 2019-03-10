using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.SeleniumTest.Admin.Settings.BonusSystem.Grade
{
    [TestFixture]
    public class BonusSystemGradeTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog);
            InitializeService.LoadData(

             "Data\\Admin\\Settings\\BonusSystem\\Catalog.Product.csv",
           "Data\\Admin\\Settings\\BonusSystem\\Catalog.Offer.csv",
           "Data\\Admin\\Settings\\BonusSystem\\Catalog.Category.csv",
           "Data\\Admin\\Settings\\BonusSystem\\Catalog.ProductCategories.csv"

                );
            Init();
        }
         
        [Test]
        public void CheckGradeGrid()
        {
            GoToAdmin("grades");
            testname = "CheckGradeGrid";
            VerifyBegin(testname);

            VerifyIsFalse(Is404Page(""), " 404 error");
            VerifyAreEqual("Грейды", driver.FindElement(By.TagName("h1")).Text, " grade title h1");

            VerifyAreEqual("Гостевой", GetGridCell(0, "Name").Text, "grade name 1");
            VerifyAreEqual("Бронзовый", GetGridCell(1, "Name").Text, "grade name 2");
            VerifyAreEqual("Серебряный", GetGridCell(2, "Name").Text, "grade name 3");
            VerifyAreEqual("Золотой", GetGridCell(3, "Name").Text, "grade name 4");
            VerifyAreEqual("Платиновый", GetGridCell(4, "Name").Text, "grade name 4");

            VerifyAreEqual("3", GetGridCell(0, "BonusPercent").FindElement(By.TagName("input")).GetAttribute("value"), "grade BonusPercent 1");
            VerifyAreEqual("5", GetGridCell(1, "BonusPercent").FindElement(By.TagName("input")).GetAttribute("value"), "grade BonusPercent 2");
            VerifyAreEqual("7", GetGridCell(2, "BonusPercent").FindElement(By.TagName("input")).GetAttribute("value"), "grade BonusPercent 3");
            VerifyAreEqual("10", GetGridCell(3, "BonusPercent").FindElement(By.TagName("input")).GetAttribute("value"), "grade BonusPercent 4");
            VerifyAreEqual("30", GetGridCell(4, "BonusPercent").FindElement(By.TagName("input")).GetAttribute("value"), "grade BonusPercent 4");


            VerifyAreEqual("0", GetGridCell(0, "PurchaseBarrier").FindElement(By.TagName("input")).GetAttribute("value"), "grade PurchaseBarrier 1");
            VerifyAreEqual("5000", GetGridCell(1, "PurchaseBarrier").FindElement(By.TagName("input")).GetAttribute("value"), "grade PurchaseBarrier 2");
            VerifyAreEqual("15000", GetGridCell(2, "PurchaseBarrier").FindElement(By.TagName("input")).GetAttribute("value"), "grade PurchaseBarrier 3");
            VerifyAreEqual("25000", GetGridCell(3, "PurchaseBarrier").FindElement(By.TagName("input")).GetAttribute("value"), "grade PurchaseBarrier 4");
            VerifyAreEqual("50000", GetGridCell(4, "PurchaseBarrier").FindElement(By.TagName("input")).GetAttribute("value"), "grade PurchaseBarrier 4");

            VerifyAreEqual("0", GetGridCell(0, "SortOrder").FindElement(By.TagName("input")).GetAttribute("value"), "grade SortOrder 1");
            VerifyAreEqual("1", GetGridCell(1, "SortOrder").FindElement(By.TagName("input")).GetAttribute("value"), "grade SortOrder 2");
            VerifyAreEqual("2", GetGridCell(2, "SortOrder").FindElement(By.TagName("input")).GetAttribute("value"), "grade SortOrder 3");
            VerifyAreEqual("3", GetGridCell(3, "SortOrder").FindElement(By.TagName("input")).GetAttribute("value"), "grade SortOrder 4");
            VerifyAreEqual("4", GetGridCell(4, "SortOrder").FindElement(By.TagName("input")).GetAttribute("value"), "grade SortOrder 4");

            VerifyFinally(testname);
        }
        [Test]
        public void GradeGridSortTest()
        {
            GoToAdmin("grades");
            testname = "GradeGridSortTest";
            VerifyBegin(testname);

            GetGridCell(-1, "Name").Click();
            WaitForAjax();
            VerifyAreEqual("Бронзовый", GetGridCell(0, "Name").Text, "grade sort by name 1");
            VerifyAreEqual("Гостевой", GetGridCell(1, "Name").Text, "grade sort by name 2");

            GetGridCell(-1, "Name").Click();
            WaitForAjax();
            VerifyAreEqual("Серебряный", GetGridCell(0, "Name").Text, "grade 2 sort by name 1");
            VerifyAreEqual("Платиновый", GetGridCell(1, "Name").Text, "grade 2 sort by name 2");

            GetGridCell(-1, "BonusPercent").Click();
            WaitForAjax();
            VerifyAreEqual("Гостевой", GetGridCell(0, "Name").Text, "grade sort by BonusPercent 1");
            VerifyAreEqual("Бронзовый", GetGridCell(1, "Name").Text, "grade sort by BonusPercent 2");

            GetGridCell(-1, "BonusPercent").Click();
            WaitForAjax();
            VerifyAreEqual("Платиновый", GetGridCell(0, "Name").Text, "grade 2 sort by BonusPercent 1");
            VerifyAreEqual("Золотой", GetGridCell(1, "Name").Text, "grade 2 sort by BonusPercent 2");

            GetGridCell(-1, "PurchaseBarrier").Click();
            WaitForAjax();
            VerifyAreEqual("Гостевой", GetGridCell(0, "Name").Text, "grade sort by PurchaseBarrier 1");
            VerifyAreEqual("Бронзовый", GetGridCell(1, "Name").Text, "grade sort by PurchaseBarrier 2");

            GetGridCell(-1, "PurchaseBarrier").Click();
            WaitForAjax();
            VerifyAreEqual("Платиновый", GetGridCell(0, "Name").Text, "grade 2 sort by PurchaseBarrier 1");
            VerifyAreEqual("Золотой", GetGridCell(1, "Name").Text, "grade 2 sort by PurchaseBarrier 2");

            GetGridCell(-1, "SortOrder").Click();
            WaitForAjax();
            VerifyAreEqual("Гостевой", GetGridCell(0, "Name").Text, "grade sort by SortOrder 1");
            VerifyAreEqual("Бронзовый", GetGridCell(1, "Name").Text, "grade sort by SortOrder 2");

            GetGridCell(-1, "SortOrder").Click();
            WaitForAjax();
            VerifyAreEqual("Платиновый", GetGridCell(0, "Name").Text, "grade 2 sort by SortOrder 1");
            VerifyAreEqual("Золотой", GetGridCell(1, "Name").Text, "grade 2 sort by SortOrder 2");

            VerifyFinally(testname);
        }

        [Test]
        public void GradeGridSearch()
        {
            GoToAdmin("grades");
            testname = "GradeGridSearch";
            VerifyBegin(testname);

            GetGridFilter().SendKeys("Бронзовый");
            DropFocus("h1");
            VerifyAreEqual("Бронзовый", GetGridCell(0, "Name").Text, "search valid element");

            //search not exist product
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("Небронзовый");
            DropFocus("h1");
            WaitForAjax();
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "search noexist element");

            //search too much symbols
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("1111111111222222222222222222333333333333334444444444rrrrrrrrrrrreeeeeeeeewwwwwwwwwwwwwwwwwww");
            DropFocus("h1");
            WaitForAjax();
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "search long name element");

            //search invalid symbols
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("########@@@@@@@@&&&&&&&******,,,,..");
            DropFocus("h1");
            WaitForAjax();
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "search invalid simvol");

            VerifyFinally(testname);
            //drop search
            /*  GetGridFilter().Click();
              GetGridFilter().Clear();
              DropFocus("h1");
              WaitForAjax();
              VerifyAreEqual("Гостевой", GetGridCell(0, "Name").Text, "clear search");*/
        }

        [Test]
        public void GradeGridFilter()
        {
            GoToAdmin("grades");
            testname = "GradeGridFilterName";
            VerifyBegin(testname);

            Filter("Name", "Золотой");
            VerifyAreEqual("Золотой", GetGridCell(0, "Name").Text, "fillter name element");
            FilterClose("Name");

            Filter("BonusPercent", "7");
            VerifyAreEqual("Серебряный", GetGridCell(0, "Name").Text, "fillter BonusPercent element");
            FilterClose("BonusPercent");

            Filter("PurchaseBarrier", "5000");
            VerifyAreEqual("Бронзовый", GetGridCell(0, "Name").Text, "fillter PurchaseBarrier element");
            FilterClose("PurchaseBarrier");

            VerifyFinally(testname);
        }


        [Test]
        public void GradeGridGoToRules()
        {
            GoToAdmin("grades");
            testname = "GradeGridGoToRules";
            VerifyBegin(testname);
            VerifyIsTrue(driver.PageSource.Contains("Для автоматической смены грейда необходимо добавить правило \"Смена грейда\""), "page source rule");
            ScrollTo(By.TagName("footer"));
            driver.FindElement(By.CssSelector("[data-e2e=\"GoRules\"] a")).Click();
            Thread.Sleep(2000);
            Functions.OpenNewTab(driver, baseURL);
            // XPathContainsText("a", "Правила");
            VerifyIsTrue(driver.Url.Contains("rules"), "url rules");
            VerifyAreEqual("Правила", driver.FindElement(By.TagName("h1")).Text, " rules title h1");

            VerifyFinally(testname);
        }
        [Test]
        public void GradeGridzGoToEdit()
        {
            GoToAdmin("grades");
            testname = "GradeGridGoToEdit";
            VerifyBegin(testname);

            //go to edit
            GetGridCell(1, "Name").Click();
            Thread.Sleep(1000);
            VerifyAreEqual("Грейд \"Бронзовый\"", driver.FindElement(By.TagName("h2")).Text, " grade title h2");
            VerifyAreEqual("Бронзовый", driver.FindElement(By.CssSelector("[data-e2e=\"gradeName\"]")).GetAttribute("value"), "pop up window name grade");
            VerifyAreEqual("5", driver.FindElement(By.CssSelector("[data-e2e=\"gradeBonusPercent\"]")).GetAttribute("value"), "pop up window gradeBonusPercent grade");
            VerifyAreEqual("5000", driver.FindElement(By.CssSelector("[data-e2e=\"gradePurchaseBarrier\"]")).GetAttribute("value"), "pop up window gradePurchaseBarrier grade");
            VerifyAreEqual("1", driver.FindElement(By.CssSelector("[data-e2e=\"gradeSortOrder\"]")).GetAttribute("value"), "pop up window gradeSortOrder grade");

            driver.FindElement(By.CssSelector("[data-e2e=\"gradeName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gradeName\"]")).SendKeys("TestGrade");

            driver.FindElement(By.CssSelector("[data-e2e=\"gradeBonusPercent\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gradeBonusPercent\"]")).SendKeys("90");

            driver.FindElement(By.CssSelector("[data-e2e=\"gradePurchaseBarrier\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gradePurchaseBarrier\"]")).SendKeys("100");

            driver.FindElement(By.CssSelector("[data-e2e=\"gradeSortOrder\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gradeSortOrder\"]")).SendKeys("-1");
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gradeButtonSave\"]")).Click();
            Thread.Sleep(1000);

            VerifyAreEqual("TestGrade", GetGridCell(0, "Name").Text, "grade name 1");
            VerifyAreEqual("90", GetGridCell(0, "BonusPercent").FindElement(By.TagName("input")).GetAttribute("value"), "grade BonusPercent 1");
            VerifyAreEqual("100", GetGridCell(0, "PurchaseBarrier").FindElement(By.TagName("input")).GetAttribute("value"), "grade PurchaseBarrier 1");
            VerifyAreEqual("-1", GetGridCell(0, "SortOrder").FindElement(By.TagName("input")).GetAttribute("value"), "grade SortOrder 1");

            VerifyFinally(testname);
        }


        [Test]
        public void GradeGridzSelectAndDelTest()
        {
            testname = "GradeGridzSelectAndDelTest";
            VerifyBegin(testname);

            GoToAdmin("settingsbonus");           
            (new SelectElement(driver.FindElement(By.Id("BonusGradeId")))).SelectByText("Платиновый");
            driver.FindElement(By.Id("CardNumTo")).SendKeys("1");
            driver.FindElement(By.CssSelector("[data-e2e=\"SettingsBonusSave\"]")).Click();
            Thread.Sleep(2000);
           
            GoToAdmin("grades");          

            //check delete cancel 
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-times.link-invert")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-cancel")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("TestGrade", GetGridCell(0, "Name").Text, "1 grid canсel delete");

            //check delete
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-times.link-invert")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Серебряный", GetGridCell(1, "Name").Text, "1 grid delete");

            //check select 
            GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            GetGridCell(1, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            GetGridCell(2, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            VerifyIsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected 2 grid"); //1 admin
            VerifyIsTrue(GetGridCell(1, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected 3 grid");
            VerifyIsTrue(GetGridCell(2, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected 4 grid");
            VerifyAreEqual("3", driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text, "count selected");

            //доделать удаление
            //check delete selected items
            Functions.GridDropdownTabDelete(driver, baseURL, gridId: "grid");
            VerifyAreEqual("Платиновый", GetGridCell(0, "Name").Text, "selected 3 grid delete");

            //check select all on page
            driver.FindElement(By.CssSelector("[grid-unique-id=\"grid\"]")).FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(1000);
            VerifyIsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected  page 1 grid");
           
            //check delete all on page
            Functions.GridDropdownTabDelete(driver, baseURL, gridId: "grid");

            VerifyAreEqual("Платиновый", GetGridCell(0, "Name").Text, " no delete vip grade");
            VerifyIsFalse(driver.PageSource.Contains("Ни одной записи не найдено"), "no notes after del");

            VerifyFinally(testname);
        }

        [Test]
        public void GradezAddNew()
        {
            GoToAdmin("grades");
            testname = "CheckGradeGrid";
            VerifyBegin(testname);

            driver.FindElement(By.CssSelector("[data-e2e=\"GradeAdd\"]")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Новый грейд", driver.FindElement(By.TagName("h2")).Text, " grade title h2");
            driver.FindElement(By.CssSelector("[data-e2e=\"gradeName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gradeName\"]")).SendKeys("NewTestGrade");

            driver.FindElement(By.CssSelector("[data-e2e=\"gradeBonusPercent\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gradeBonusPercent\"]")).SendKeys("5");

            driver.FindElement(By.CssSelector("[data-e2e=\"gradePurchaseBarrier\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gradePurchaseBarrier\"]")).SendKeys("5");

            driver.FindElement(By.CssSelector("[data-e2e=\"gradeSortOrder\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gradeSortOrder\"]")).SendKeys("10");
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gradeButtonSave\"]")).Click();
            Thread.Sleep(1000);

            GetGridFilter().SendKeys("NewTestGrade");
            DropFocus("h1");
            VerifyAreEqual("NewTestGrade", GetGridCell(0, "Name").Text, "searchnew element");
            VerifyAreEqual("5", GetGridCell(0, "BonusPercent").FindElement(By.TagName("input")).GetAttribute("value"), "grade BonusPercent 5");
            VerifyAreEqual("5", GetGridCell(0, "PurchaseBarrier").FindElement(By.TagName("input")).GetAttribute("value"), "grade PurchaseBarrier 5");
            VerifyAreEqual("10", GetGridCell(0, "SortOrder").FindElement(By.TagName("input")).GetAttribute("value"), "grade SortOrder 10");


            VerifyFinally(testname);
        }

        public void Filter(string name, string date)
        {
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterDropdownButton\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterDropdownItem\"][data-e2e-filter-dropdown-name=\"" + name + "\"]")).Click();
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"" + name + "\"]")).Displayed, "display filter by "+ name);
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e-grid-filter-block-name=\"" + name + "\"] input")).SendKeys(date);
            DropFocus("h1");
        }
        public void FilterClose(string name)
        {
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"" + name + "\"] [data-e2e=\"gridFilterItemClose\"]")).Click();
            Thread.Sleep(2000);
            VerifyIsFalse(driver.FindElements(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"" + name + "\"]")).Count > 0);
            VerifyAreEqual("Гостевой", GetGridCell(0, "Name").Text, " close fillter");
        }
    }
}

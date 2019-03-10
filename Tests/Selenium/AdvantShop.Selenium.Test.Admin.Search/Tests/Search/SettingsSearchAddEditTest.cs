using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using AdvantShop.Selenium.Core.Infrastructure;

namespace AdvantShop.Web.Site.Selenium.Test.Admin.SettingsSearch
{
    [TestFixture]
    public class SettingsSearchAddEditTest : BaseSeleniumTest
    {

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.SettingsSearch);
            InitializeService.LoadData(
           "data\\Admin\\SettingsSearch\\Settings.SettingsSearch.csv"

           );

            Init();
        }

         

        [Test]
        public void SettingsSearchAdd()
        {
            testname = "SettingsSearchAdd";
            VerifyBegin(testname);

            GoToAdmin("settingssearch");

            GetButton(eButtonType.Add).Click();

            driver.FindElement(By.CssSelector("[data-e2e=\"nameSettingsSearch\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"nameSettingsSearch\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"nameSettingsSearch\"]")).SendKeys("NewSettingsSearch");

            driver.FindElement(By.CssSelector("[data-e2e=\"linkSettingsSearch\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"linkSettingsSearch\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"linkSettingsSearch\"]")).SendKeys("coupons");

            driver.FindElement(By.CssSelector("[data-e2e=\"keywordsSettingsSearch\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"keywordsSettingsSearch\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"keywordsSettingsSearch\"]")).SendKeys("купоны");

            driver.FindElement(By.CssSelector("[data-e2e=\"sortOrderSettingsSearch\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"sortOrderSettingsSearch\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"sortOrderSettingsSearch\"]")).SendKeys("1");

            driver.FindElement(By.CssSelector("[data-e2e=\"saveSettingsSearch\"]")).Click();

            //check admin
            GoToAdmin("settingssearch");

            VerifyAreEqual("Найдено записей: 151", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all after adding");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("NewSettingsSearch");
            DropFocus("h1");
            WaitForAjax();
            
            VerifyAreEqual("NewSettingsSearch", GetGridCell(0, "Title").FindElement(By.TagName("input")).GetAttribute("value"), "Title");
            VerifyAreEqual("coupons", GetGridCell(0, "Link").Text, "Link");
            VerifyAreEqual("купоны", GetGridCell(0, "KeyWords").FindElement(By.TagName("input")).GetAttribute("value"), "KeyWords");
            VerifyAreEqual("1", GetGridCell(0, "SortOrder").FindElement(By.TagName("input")).GetAttribute("value"), "SortOrder");

            //check link
            GetGridCell(0, "Link").Click();

            Functions.OpenNewTab(driver, baseURL);
            VerifyIsTrue(driver.WindowHandles.Count.Equals(2), "count tabs");
            
            VerifyIsTrue(driver.Url.Contains("coupons"), "check url from settings search grid");
            VerifyAreEqual("Купоны", driver.FindElement(By.TagName("h1")).Text, "h1 page from settings search grid");
            
            Functions.CloseTab(driver, baseURL);

            //check search
            GoToAdmin();

            driver.FindElement(By.Id("header-bottom")).FindElement(By.TagName("input")).Click();
            driver.FindElement(By.Id("header-bottom")).FindElement(By.TagName("input")).SendKeys("купоны");
            MouseFocus(driver, By.XPath("//span[contains(text(), 'NewSettingsSearch')]"));
            driver.FindElement(By.XPath("//span[contains(text(), 'NewSettingsSearch')]")).Click();
            Thread.Sleep(3000);

            VerifyIsTrue(driver.Url.Contains("coupons"), "check url from search");
            VerifyAreEqual("Купоны", driver.FindElement(By.TagName("h1")).Text, "h1 page from search");
            
            VerifyFinally(testname);
        }

        [Test]
        public void SettingsSearchEdit()
        {
            testname = "SettingsSearchEdit";
            VerifyBegin(testname);

            GoToAdmin("settingssearch");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("test title 111");
            DropFocus("h1");
            WaitForAjax();

            VerifyAreEqual("test title 111", GetGridCell(0, "Title").FindElement(By.TagName("input")).GetAttribute("value"), "Title grid before");

            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".link-invert.ui-grid-custom-service-icon.fa.fa-pencil")).Click();

            //pre check
            VerifyAreEqual("test title 111", driver.FindElement(By.CssSelector("[data-e2e=\"nameSettingsSearch\"]")).GetAttribute("value"), "Title edit pop up");
            VerifyAreEqual("link110", driver.FindElement(By.CssSelector("[data-e2e=\"linkSettingsSearch\"]")).GetAttribute("value"), "Link edit pop up");
            VerifyAreEqual("keywords 110", driver.FindElement(By.CssSelector("[data-e2e=\"keywordsSettingsSearch\"]")).GetAttribute("value"), "KeyWords edit pop up");
            VerifyAreEqual("1110", driver.FindElement(By.CssSelector("[data-e2e=\"sortOrderSettingsSearch\"]")).GetAttribute("value"), "SortOrder edit pop up");

            //edit
            driver.FindElement(By.CssSelector("[data-e2e=\"nameSettingsSearch\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"nameSettingsSearch\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"nameSettingsSearch\"]")).SendKeys("edit name");

            driver.FindElement(By.CssSelector("[data-e2e=\"linkSettingsSearch\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"linkSettingsSearch\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"linkSettingsSearch\"]")).SendKeys("orders");

            driver.FindElement(By.CssSelector("[data-e2e=\"keywordsSettingsSearch\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"keywordsSettingsSearch\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"keywordsSettingsSearch\"]")).SendKeys("заказы");

            driver.FindElement(By.CssSelector("[data-e2e=\"sortOrderSettingsSearch\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"sortOrderSettingsSearch\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"sortOrderSettingsSearch\"]")).SendKeys("2");

            driver.FindElement(By.CssSelector("[data-e2e=\"saveSettingsSearch\"]")).Click();

            //check admin
            GoToAdmin("settingssearch");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("test title 111");
            DropFocus("h1");
            WaitForAjax();

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "search old item");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("edit name");
            DropFocus("h1");
            WaitForAjax();

            VerifyAreEqual("edit name", GetGridCell(0, "Title").FindElement(By.TagName("input")).GetAttribute("value"), "Title");
            VerifyAreEqual("orders", GetGridCell(0, "Link").Text, "Link");
            VerifyAreEqual("заказы", GetGridCell(0, "KeyWords").FindElement(By.TagName("input")).GetAttribute("value"), "KeyWords");
            VerifyAreEqual("2", GetGridCell(0, "SortOrder").FindElement(By.TagName("input")).GetAttribute("value"), "SortOrder");

            //check link
            GetGridCell(0, "Link").Click();

            Functions.OpenNewTab(driver, baseURL);
            VerifyIsTrue(driver.WindowHandles.Count.Equals(2), "count tabs");

            VerifyIsTrue(driver.Url.Contains("orders"), "check url from settings search grid");
            VerifyAreEqual("Заказы", driver.FindElement(By.TagName("h1")).Text, "h1 page from settings search grid");

            Functions.CloseTab(driver, baseURL);

            //check search
            GoToAdmin();

            driver.FindElement(By.Id("header-bottom")).FindElement(By.TagName("input")).Click();
            driver.FindElement(By.Id("header-bottom")).FindElement(By.TagName("input")).SendKeys("заказы");
            //  driver.FindElement(By.CssSelector("a.search-col-item-link > span")).Click();
            MouseFocus(driver, By.XPath("//span[contains(text(), 'edit name')]"));
            driver.FindElement(By.XPath("//span[contains(text(), 'edit name')]")).Click();
            Thread.Sleep(3000);

            VerifyIsTrue(driver.Url.Contains("orders"), "check url from search");
            VerifyAreEqual("Заказы", driver.FindElement(By.TagName("h1")).Text, "h1 page from search");
            
            VerifyFinally(testname);
        }

        [Test]
        public void SettingsSearchAddWithout()
        {
            testname = "SettingsSearchAddWithout";
            VerifyBegin(testname);

            GoToAdmin("settingssearch");

            GetButton(eButtonType.Add).Click();

            driver.FindElement(By.CssSelector("[data-e2e=\"nameSettingsSearch\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"nameSettingsSearch\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"nameSettingsSearch\"]")).SendKeys("NewSettingsSearchwithout");

            driver.FindElement(By.CssSelector("[data-e2e=\"linkSettingsSearch\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"linkSettingsSearch\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"linkSettingsSearch\"]")).SendKeys("brands");
            
            driver.FindElement(By.CssSelector("[data-e2e=\"sortOrderSettingsSearch\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"sortOrderSettingsSearch\"]")).Clear();

            driver.FindElement(By.CssSelector("[data-e2e=\"saveSettingsSearch\"]")).Click();

            //check admin
            GoToAdmin("settingssearch");
            
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("NewSettingsSearchwithout");
            DropFocus("h1");
            WaitForAjax();

            VerifyAreEqual("NewSettingsSearchwithout", GetGridCell(0, "Title").FindElement(By.TagName("input")).GetAttribute("value"), "Title");
            VerifyAreEqual("brands", GetGridCell(0, "Link").Text, "Link");
            VerifyAreEqual("", GetGridCell(0, "KeyWords").FindElement(By.TagName("input")).GetAttribute("value"), "KeyWords");
            VerifyAreEqual("0", GetGridCell(0, "SortOrder").FindElement(By.TagName("input")).GetAttribute("value"), "SortOrder");
            
            VerifyFinally(testname);
        }

    }
}
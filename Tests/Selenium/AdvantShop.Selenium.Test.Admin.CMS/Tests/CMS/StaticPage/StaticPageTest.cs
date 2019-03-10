using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.SeleniumTest.Admin.CMS.StaticPage
{
    [TestFixture]
    public class StaticPageTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.CMS);
            InitializeService.LoadData(
           "data\\Admin\\CMS\\StaticPage\\CMS.StaticPage.csv"
           );

            Init();
        }
        [Test]
        public void ChekingPage()
        {
            GoToAdmin("staticpages");
            Assert.AreEqual("Page1", GetGridCell(0, "PageName").Text);
            Assert.AreEqual("1", driver.FindElement(By.CssSelector(".ui-grid-custom-edit-form input")).GetAttribute("value"));
            Assert.AreEqual("15.06.2012 14:22", GetGridCell(0, "ModifyDateFormatted").Text);
            Assert.AreEqual("true",driver.FindElement(By.Name("switchOnOff_0")).GetAttribute("value"));

            Assert.AreEqual("Page10", GetGridCell(9, "PageName").Text);
            Assert.AreEqual("10", driver.FindElements(By.CssSelector(".ui-grid-custom-edit-form input"))[9].GetAttribute("value"));
            Assert.AreEqual("06.06.2012 14:22", GetGridCell(9, "ModifyDateFormatted").Text);
            Assert.AreEqual("false", driver.FindElement(By.Name("switchOnOff_9")).GetAttribute("value"));
            GetGridCell(0, "PageName").Click();
            Thread.Sleep(1000);
            Assert.AreEqual("Статическая страница \"Page1\"", driver.FindElement(By.TagName("h1")).Text); 
            Assert.AreEqual("Page1", driver.FindElement(By.Name("PageName")).GetAttribute("value"));
            Assert.AreEqual("1", driver.FindElement(By.Name("SortOrder")).GetAttribute("value"));
            Assert.AreEqual("page1", driver.FindElement(By.Name("UrlPath")).GetAttribute("value"));

            Thread.Sleep(1000);
            driver.SwitchTo().Frame(0);
            Thread.Sleep(1000);
            Assert.AreEqual("text1", driver.FindElement(By.TagName("body")).Text);
            driver.SwitchTo().DefaultContent();

            GoToClient("pages/page1");
            Assert.AreEqual("Page1", driver.FindElement(By.TagName("h1")).Text);
            Assert.AreEqual("text1", driver.FindElement(By.CssSelector(".staticpage-content")).Text);
            Assert.IsTrue(Is404Page("pages/page10"));
        }

        [Test]
        public void SortPage()
        {
            GoToAdmin("staticpages");

            GetGridCell(-1, "PageName").Click();
            WaitForAjax();
            Assert.AreEqual("Page1", GetGridCell(0, "PageName").Text);
            Assert.AreEqual("Page16", GetGridCell(9, "PageName").Text);

            GetGridCell(-1, "PageName").Click();
            WaitForAjax();
            Assert.AreEqual("Page99", GetGridCell(0, "PageName").Text);
            Assert.AreEqual("Page90", GetGridCell(9, "PageName").Text);

            GetGridCell(-1, "SortOrder").Click();
            WaitForAjax();
            Assert.AreEqual("Page1", GetGridCell(0, "PageName").Text);
            Assert.AreEqual("Page10", GetGridCell(9, "PageName").Text);

            GetGridCell(-1, "SortOrder").Click();
            WaitForAjax();
            Assert.AreEqual("Page101", GetGridCell(0, "PageName").Text);
            Assert.AreEqual("Page92", GetGridCell(9, "PageName").Text);

              GetGridCell(-1, "Enabled").Click();
            WaitForAjax();
            Assert.AreEqual("Page6", GetGridCell(0, "PageName").Text);
            Assert.AreEqual("Page15", GetGridCell(9, "PageName").Text);

            GetGridCell(-1, "Enabled").Click();
            WaitForAjax();
            Assert.AreEqual("Page11", GetGridCell(0, "PageName").Text);
            Assert.AreEqual("Page20", GetGridCell(9, "PageName").Text);

            GetGridCell(-1, "ModifyDateFormatted").Click();
            WaitForAjax();
            Assert.AreEqual("Page101", GetGridCell(0, "PageName").Text);
            Assert.AreEqual("Page92", GetGridCell(9, "PageName").Text);

            GetGridCell(-1, "ModifyDateFormatted").Click();
            WaitForAjax();
            Assert.AreEqual("Page1", GetGridCell(0, "PageName").Text);
            Assert.AreEqual("Page10", GetGridCell(9, "PageName").Text);   
        }

        [Test]
        public void SearchPage()
        {
            GoToAdmin("staticpages");
            GetGridFilter().SendKeys("page100");
            DropFocus("h1");
            Blur();
            Assert.AreEqual("Page100", GetGridCell(0, "PageName").Text);

            //search not exist product
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("page111");
            DropFocus("h1");
            Blur();
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));

            //search too much symbols
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("1111111111222222222222222222333333333333334444444444rrrrrrrrrrrreeeeeeeeewwwwwwwwwwwwwwwwwww");
            DropFocus("h1");
            Blur();
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));

            //search invalid symbols
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("########@@@@@@@@&&&&&&&******,,,,..");
            DropFocus("h1");
            Blur();
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));
        }

        [Test]
        public void FilterPage()
        {
            GoToAdmin("staticpages");
            //имя
            Functions.GridFilterSet(driver, baseURL, "PageName");
            DropFocus("h1");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("page22");
            DropFocus("h1");
            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count == 1);
            Assert.AreEqual("Page22", GetGridCell(0, "PageName").Text);
            Functions.GridFilterClose(driver, baseURL, "PageName");
            Assert.AreEqual("Page1", GetGridCell(0, "PageName").Text);
            Assert.AreEqual("Page10", GetGridCell(9, "PageName").Text);
          
            //Активность
            Functions.GridFilterSet(driver, baseURL, "Enabled");
            DropFocus("h1");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Активные");
            DropFocus("h1");
            Assert.AreEqual("Page1", GetGridCell(0, "PageName").Text);
            Assert.AreEqual("Page15", GetGridCell(9, "PageName").Text);

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Неактивные");
            DropFocus("h1");
            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count == 5);
            Assert.AreEqual("Page6", GetGridCell(0, "PageName").Text);
            Assert.AreEqual("Page10", GetGridCell(4, "PageName").Text);
            Functions.GridFilterClose(driver, baseURL, "Enabled");
            Assert.AreEqual("Page1", GetGridCell(0, "PageName").Text);
            Assert.AreEqual("Page10", GetGridCell(9, "PageName").Text);

            //дата
            Functions.GridFilterSet(driver, baseURL, "ModifyDateFormatted");
            DropFocus("h1");

            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).SendKeys("12.06.2012 10:00");
            DropFocus("h1");
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).SendKeys("15.06.2012 10:00");
            
            DropFocus("h1");
            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count == 3);
            Assert.AreEqual("Page2", GetGridCell(0, "PageName").Text);
            Assert.AreEqual("Page4", GetGridCell(2, "PageName").Text);
            Functions.GridFilterClose(driver, baseURL, "ModifyDateFormatted");
            Assert.AreEqual("Page1", GetGridCell(0, "PageName").Text);
            Assert.AreEqual("Page10", GetGridCell(9, "PageName").Text);

        }
        [Test]
        public void xInplacePage()
        {
            GoToAdmin("staticpages");
            
            Assert.AreEqual("true", driver.FindElement(By.Name("switchOnOff_0")).GetAttribute("value"));
            Assert.IsFalse(Is404Page("pages/page1"));

            Assert.AreEqual("Page1", GetGridCell(0, "PageName").Text);
            driver.FindElement(By.CssSelector("[data-e2e=\"switchOnOffLabel\"]")).Click();
            
            Assert.AreEqual("false", driver.FindElement(By.Name("switchOnOff_0")).GetAttribute("value"));
            Assert.IsTrue(Is404Page("pages/page1"));
        }
    }
}

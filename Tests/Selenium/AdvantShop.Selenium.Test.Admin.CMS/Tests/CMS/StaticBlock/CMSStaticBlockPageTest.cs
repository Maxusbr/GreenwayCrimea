using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
namespace AdvantShop.Web.Site.Selenium.Test.Admin.CMS.StaticBlock
{
    [TestFixture]
    public class CMSStaticBlockPageTest : BaseSeleniumTest
    {

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.CMS);
            InitializeService.LoadData(
           "data\\Admin\\CMS\\StaticBlock\\CMS.StaticBlock.csv"

           );

            Init();
        }


        [Test]
        public void CMSStaticBlockPage()
        {
            GoToAdmin("staticblock");
            Assert.AreEqual("bannerDetails", GetGridCell(0, "Key").Text);
            Assert.AreEqual("staticblockkey106", GetGridCell(9, "Key").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(4) a")).Click();
            Thread.Sleep(2000);
            WaitForAjax();
            Assert.AreEqual("staticblockkey107", GetGridCell(0, "Key").Text);
            Assert.AreEqual("staticblockkey115", GetGridCell(9, "Key").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(5) a")).Click();
            Thread.Sleep(2000);
            WaitForAjax();
            Assert.AreEqual("staticblockkey116", GetGridCell(0, "Key").Text);
            Assert.AreEqual("staticblockkey124", GetGridCell(9, "Key").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(6) a")).Click();
            Thread.Sleep(2000);
            WaitForAjax();
            Assert.AreEqual("staticblockkey125", GetGridCell(0, "Key").Text);
            Assert.AreEqual("staticblockkey133", GetGridCell(9, "Key").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(7) a")).Click();
            Thread.Sleep(2000);
            WaitForAjax();
            Assert.AreEqual("staticblockkey134", GetGridCell(0, "Key").Text);
            Assert.AreEqual("staticblockkey142", GetGridCell(9, "Key").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(7) a")).Click();
            Thread.Sleep(2000);
            WaitForAjax();
            Assert.AreEqual("staticblockkey143", GetGridCell(0, "Key").Text);
            Assert.AreEqual("staticblockkey17", GetGridCell(9, "Key").Text);
        }

        [Test]
        public void CMSStaticBlockPageToBegin()
        {
            GoToAdmin("staticblock");
            Assert.AreEqual("bannerDetails", GetGridCell(0, "Key").Text);
            Assert.AreEqual("staticblockkey106", GetGridCell(9, "Key").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            WaitForAjax();
            Assert.AreEqual("staticblockkey107", GetGridCell(0, "Key").Text);
            Assert.AreEqual("staticblockkey115", GetGridCell(9, "Key").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            WaitForAjax();
            Assert.AreEqual("staticblockkey116", GetGridCell(0, "Key").Text);
            Assert.AreEqual("staticblockkey124", GetGridCell(9, "Key").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            WaitForAjax();
            Assert.AreEqual("staticblockkey125", GetGridCell(0, "Key").Text);
            Assert.AreEqual("staticblockkey133", GetGridCell(9, "Key").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            WaitForAjax();
            Assert.AreEqual("staticblockkey134", GetGridCell(0, "Key").Text);
            Assert.AreEqual("staticblockkey142", GetGridCell(9, "Key").Text);

            //to begin
            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-first a")).Click();
            Thread.Sleep(2000);
            WaitForAjax();
            Assert.AreEqual("bannerDetails", GetGridCell(0, "Key").Text);
            Assert.AreEqual("staticblockkey106", GetGridCell(9, "Key").Text);

        }

        [Test]
        public void CMSStaticBlockPageToEnd()
        {
            GoToAdmin("staticblock");
            Assert.AreEqual("bannerDetails", GetGridCell(0, "Key").Text);
            Assert.AreEqual("staticblockkey106", GetGridCell(9, "Key").Text);

            //to end
            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-last a")).Click();
            Thread.Sleep(2000);
            WaitForAjax();
            Assert.AreEqual("staticblockkey90", GetGridCell(0, "Key").Text);
            Assert.AreEqual("staticblockkey99", GetGridCell(9, "Key").Text);
        }

        [Test]
        public void CMSStaticBlockPageToNext()
        {
            GoToAdmin("staticblock");
            Assert.AreEqual("bannerDetails", GetGridCell(0, "Key").Text);
            Assert.AreEqual("staticblockkey106", GetGridCell(9, "Key").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            WaitForAjax();
            Assert.AreEqual("staticblockkey107", GetGridCell(0, "Key").Text);
            Assert.AreEqual("staticblockkey115", GetGridCell(9, "Key").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            WaitForAjax();
            Assert.AreEqual("staticblockkey116", GetGridCell(0, "Key").Text);
            Assert.AreEqual("staticblockkey124", GetGridCell(9, "Key").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            WaitForAjax();
            Assert.AreEqual("staticblockkey125", GetGridCell(0, "Key").Text);
            Assert.AreEqual("staticblockkey133", GetGridCell(9, "Key").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            WaitForAjax();
            Assert.AreEqual("staticblockkey134", GetGridCell(0, "Key").Text);
            Assert.AreEqual("staticblockkey142", GetGridCell(9, "Key").Text);
        }

        [Test]
        public void CMSStaticBlockPageToPrevious()
        {
            GoToAdmin("staticblock");
            Assert.AreEqual("bannerDetails", GetGridCell(0, "Key").Text);
            Assert.AreEqual("staticblockkey106", GetGridCell(9, "Key").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            WaitForAjax();
            Assert.AreEqual("staticblockkey107", GetGridCell(0, "Key").Text);
            Assert.AreEqual("staticblockkey115", GetGridCell(9, "Key").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            WaitForAjax();
            Assert.AreEqual("staticblockkey116", GetGridCell(0, "Key").Text);
            Assert.AreEqual("staticblockkey124", GetGridCell(9, "Key").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            Thread.Sleep(2000);
            WaitForAjax();
            Assert.AreEqual("staticblockkey107", GetGridCell(0, "Key").Text);
            Assert.AreEqual("staticblockkey115", GetGridCell(9, "Key").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            Thread.Sleep(2000);
            WaitForAjax();
            Assert.AreEqual("bannerDetails", GetGridCell(0, "Key").Text);
            Assert.AreEqual("staticblockkey106", GetGridCell(9, "Key").Text);
        }
    }
}
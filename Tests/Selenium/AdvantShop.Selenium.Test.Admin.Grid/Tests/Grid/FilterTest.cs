using System;
using NUnit.Framework;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Admin.GridProducts
{
    [TestFixture]
    public class GridProductsFilterTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog);
            InitializeService.LoadData(
                "Data\\Admin\\Grid\\FilterGrid\\Catalog.Category.csv",
                "Data\\Admin\\Grid\\FilterGrid\\Catalog.Product.csv",
                "Data\\Admin\\Grid\\FilterGrid\\Catalog.ProductCategories.csv",
                "Data\\Admin\\Grid\\FilterGrid\\Catalog.Offer.csv",
                 "Data\\Admin\\Grid\\FilterGrid\\Catalog.Photo.csv"
            );

            Init();
        }
        
        [Test]
        public void GridFilterImg()
        {
            GoToAdmin("catalog?categoryid=1");

            //check filter img
            Functions.GridFilterSet(driver, baseURL, name: "PhotoSrc");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("С фотографией");
            Thread.Sleep(1000);
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"PhotoSrc\"]")).Displayed);

            Assert.AreEqual("TestProduct1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("TestProduct10", GetGridCell(9, "Name").Text);

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Без фотографии");
            Thread.Sleep(1000);


            Assert.AreEqual("TestProduct21", GetGridCell(0, "Name").Text);
            Assert.AreEqual("TestProduct55", GetGridCell(9, "Name").Text);

            //check go to edit and back
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector("[ng-href=\"product/edit/21\"]")).Click();
            Thread.Sleep(4000);

            GoBack();

            Assert.AreEqual("TestProduct21", GetGridCell(0, "Name").Text);
            Assert.AreEqual("TestProduct55", GetGridCell(9, "Name").Text);
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"PhotoSrc\"]")).Displayed);

            //close
            Functions.GridFilterClose(driver, baseURL, name: "PhotoSrc");
            Assert.AreEqual("TestProduct1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("TestProduct10", GetGridCell(9, "Name").Text);
        }

      [Test]
      public void GridFilterPrice()
      {
            GoToAdmin("catalog?categoryid=2");

            //check filter price
            Functions.GridFilterSet(driver, baseURL, name: "PriceString");

            //ckeck initial count
            Assert.AreEqual("Найдено записей: 10", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text);
            Assert.AreEqual("26", driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"PriceString\"] [data-e2e=\"gridFilterItemRangeFrom\"]")).GetAttribute("value"));
            Assert.AreEqual("35", driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"PriceString\"] [data-e2e=\"gridFilterItemRangeTo\"]")).GetAttribute("value"));

            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"PriceString\"] [data-e2e=\"gridFilterItemRangeFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"PriceString\"] [data-e2e=\"gridFilterItemRangeFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"PriceString\"] [data-e2e=\"gridFilterItemRangeFrom\"]")).SendKeys("30");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"PriceString\"] [data-e2e=\"gridFilterItemRangeTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"PriceString\"] [data-e2e=\"gridFilterItemRangeTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"PriceString\"] [data-e2e=\"gridFilterItemRangeTo\"]")).SendKeys("35");
            DropFocus("h2");
            Refresh();

            Assert.AreEqual("TestProduct30", GetGridCell(0, "Name").Text);
            Assert.AreEqual("TestProduct35", GetGridCell(5, "Name").Text);
            Assert.AreEqual("Найдено записей: 6", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text);

            //check go to edit and back
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector("[ng-href=\"product/edit/30\"]")).Click();
            Thread.Sleep(4000);

            GoBack();

            Assert.AreEqual("TestProduct30", GetGridCell(0, "Name").Text);
            Assert.AreEqual("TestProduct35", GetGridCell(5, "Name").Text);
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"PriceString\"]")).Displayed);
            Assert.AreEqual("Найдено записей: 6", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text);

            //close
            Functions.GridFilterClose(driver, baseURL, name: "PriceString");
            Assert.AreEqual("TestProduct26", GetGridCell(0, "Name").Text);
            Assert.AreEqual("TestProduct35", GetGridCell(9, "Name").Text);
            Refresh();
            Assert.AreEqual("Найдено записей: 10", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text);
        }

        [Test]
        public void GridFilterAmount()
        {
            GoToAdmin("catalog?categoryid=2");

            //check filter amount
            Functions.GridFilterSet(driver, baseURL, name: "Amount");

            //ckeck initial amount
            Assert.AreEqual("26", driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"Amount\"] [data-e2e=\"gridFilterItemRangeFrom\"]")).GetAttribute("value"));
            Assert.AreEqual("35", driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"Amount\"] [data-e2e=\"gridFilterItemRangeTo\"]")).GetAttribute("value"));

            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"Amount\"] [data-e2e=\"gridFilterItemRangeFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"Amount\"] [data-e2e=\"gridFilterItemRangeFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"Amount\"] [data-e2e=\"gridFilterItemRangeFrom\"]")).SendKeys("30");
            Thread.Sleep(1000);
            
            Assert.AreEqual("TestProduct30", GetGridCell(0, "Name").Text);
            Assert.AreEqual("TestProduct35", GetGridCell(5, "Name").Text);

            string strUrl = driver.Url;

            //check go to edit and back
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector("[ng-href=\"product/edit/30\"]")).Click();
            Thread.Sleep(4000);
            
            driver.Navigate().GoToUrl(strUrl);

            Assert.AreEqual("TestProduct30", GetGridCell(0, "Name").Text);
            Assert.AreEqual("TestProduct35", GetGridCell(5, "Name").Text);
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"Amount\"]")).Displayed);

            //close
            Functions.GridFilterClose(driver, baseURL, name: "Amount");
            Assert.AreEqual("TestProduct26", GetGridCell(0, "Name").Text);
            Assert.AreEqual("TestProduct35", GetGridCell(9, "Name").Text);
        }

        [Test]
        public void GridFilterSort()
        {
            GoToAdmin("catalog?categoryid=2");

            //check filter sort
            Functions.GridFilterSet(driver, baseURL, name: "SortOrder");

            //ckeck initial count
            Assert.AreEqual("", driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"SortOrder\"] [data-e2e=\"gridFilterItemRangeFrom\"]")).GetAttribute("value"));
            Assert.AreEqual("", driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"SortOrder\"] [data-e2e=\"gridFilterItemRangeTo\"]")).GetAttribute("value"));

            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"SortOrder\"] [data-e2e=\"gridFilterItemRangeFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"SortOrder\"] [data-e2e=\"gridFilterItemRangeFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"SortOrder\"] [data-e2e=\"gridFilterItemRangeFrom\"]")).SendKeys("30");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"SortOrder\"] [data-e2e=\"gridFilterItemRangeTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"SortOrder\"] [data-e2e=\"gridFilterItemRangeTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"SortOrder\"] [data-e2e=\"gridFilterItemRangeTo\"]")).SendKeys("35");

            Thread.Sleep(2000);
            Assert.AreEqual("TestProduct30", GetGridCell(0, "Name").Text);
            Assert.AreEqual("TestProduct35", GetGridCell(5, "Name").Text);

            //check go to edit and back
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector("[ng-href=\"product/edit/30\"]")).Click();
            Thread.Sleep(4000);
            GoBack();

            Assert.AreEqual("TestProduct30", GetGridCell(0, "Name").Text);
            Assert.AreEqual("TestProduct35", GetGridCell(5, "Name").Text);
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"SortOrder\"]")).Displayed);

            //close
            Functions.GridFilterClose(driver, baseURL, name: "SortOrder");
            Assert.AreEqual("TestProduct26", GetGridCell(0, "Name").Text);
            Assert.AreEqual("TestProduct35", GetGridCell(9, "Name").Text);
        }
      
        [Test]
        public void GridFilterActivity()
        {
            GoToAdmin("catalog?categoryid=2");

            //check activity on
            Functions.GridFilterSelectDropFocus(driver, baseURL, filterName: "Enabled", filterItem: "Активные", tag: "h2");
            Assert.AreEqual("TestProduct31", GetGridCell(0, "Name").Text);
            Assert.AreEqual("TestProduct35", GetGridCell(4, "Name").Text);
          
            //close
            Functions.GridFilterClose(driver, baseURL, name: "Enabled");
            Assert.AreEqual("TestProduct26", GetGridCell(0, "Name").Text);
            Assert.AreEqual("TestProduct35", GetGridCell(9, "Name").Text);

            Refresh();

            //check activity off
            Functions.GridFilterSelectDropFocus(driver, baseURL, filterName: "Enabled", filterItem: "Неактивные", tag: "h2");
            Assert.AreEqual("TestProduct26", GetGridCell(0, "Name").Text);
            Assert.AreEqual("TestProduct30", GetGridCell(4, "Name").Text);

            //close
            Functions.GridFilterClose(driver, baseURL, name: "Enabled");
            Assert.AreEqual("TestProduct26", GetGridCell(0, "Name").Text);
            Assert.AreEqual("TestProduct35", GetGridCell(9, "Name").Text);
        }
       
        [Test]
        public void GridFilterOfferAndCount()
        {
            //price
            GoToAdmin("catalog?categoryid=2");
            Functions.GridFilterSet(driver, baseURL, name: "PriceString");

            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"PriceString\"] [data-e2e=\"gridFilterItemRangeFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"PriceString\"] [data-e2e=\"gridFilterItemRangeFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"PriceString\"] [data-e2e=\"gridFilterItemRangeFrom\"]")).SendKeys("30");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"PriceString\"] [data-e2e=\"gridFilterItemRangeTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"PriceString\"] [data-e2e=\"gridFilterItemRangeTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"PriceString\"] [data-e2e=\"gridFilterItemRangeTo\"]")).SendKeys("33");
            Thread.Sleep(1000);
            Assert.AreEqual("TestProduct30", GetGridCell(0, "Name").Text);
            Assert.AreEqual("TestProduct33", GetGridCell(3, "Name").Text);

            //amount
            Functions.GridFilterSet(driver, baseURL, name: "Amount");

            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"Amount\"] [data-e2e=\"gridFilterItemRangeFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"Amount\"] [data-e2e=\"gridFilterItemRangeFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"Amount\"] [data-e2e=\"gridFilterItemRangeFrom\"]")).SendKeys("33");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"Amount\"] [data-e2e=\"gridFilterItemRangeTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"Amount\"] [data-e2e=\"gridFilterItemRangeTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"Amount\"] [data-e2e=\"gridFilterItemRangeTo\"]")).SendKeys("35");
            Thread.Sleep(1000);
            Assert.AreEqual("TestProduct33", GetGridCell(0, "Name").Text);

            //close price
            Functions.GridFilterClose(driver, baseURL, name: "PriceString");
            Assert.AreEqual("TestProduct33", GetGridCell(0, "Name").Text);
            Assert.AreEqual("TestProduct34", GetGridCell(1, "Name").Text);
            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"Amount\"]")).Count == 1);

            //close amount
            Functions.GridFilterClose(driver, baseURL, name: "Amount");
            Assert.AreEqual("TestProduct26", GetGridCell(0, "Name").Text);
            Assert.AreEqual("TestProduct35", GetGridCell(9, "Name").Text);

        }
      
        [Test]
        public void GridFilterArtNo()
        {
            GoToAdmin("catalog?categoryId=1");
            Functions.GridFilterSet(driver, baseURL, name: "_noopColumnArtNo");

            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"_noopColumnArtNo\"] [data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"_noopColumnArtNo\"] [data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"_noopColumnArtNo\"] [data-e2e=\"gridFilterItemInput\"]")).SendKeys("10");
            Thread.Sleep(1000);
            Assert.AreEqual("TestProduct10", GetGridCell(0, "Name").Text);

            //check go to edit and back
            driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"grid[0][\'_serviceColumn\']\"] [ng-href=\"product/edit/10\"]")).Click();
            Thread.Sleep(4000);
            GoBack();

            Assert.AreEqual("TestProduct10", GetGridCell(0, "Name").Text);
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"_noopColumnArtNo\"]")).Displayed);

            //close
            Functions.GridFilterClose(driver, baseURL, name: "_noopColumnArtNo");
            Assert.AreEqual("TestProduct1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("TestProduct10", GetGridCell(9, "Name").Text);
        }

        [Test]
        public void GridFilterName()
        {
            GoToAdmin("catalog?categoryId=1");
            Functions.GridFilterSet(driver, baseURL, name: "_noopColumnName");

            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"_noopColumnName\"] [data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"_noopColumnName\"] [data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"_noopColumnName\"] [data-e2e=\"gridFilterItemInput\"]")).SendKeys("TestProduct5");

            Assert.AreEqual("TestProduct5", GetGridCell(0, "Name").Text);

            //check go to edit and back
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector("[ng-href=\"product/edit/5\"]")).Click();
            Thread.Sleep(4000);

            GoBack();

            Assert.AreEqual("TestProduct5", GetGridCell(0, "Name").Text);
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"_noopColumnName\"]")).Displayed);

            //close
            Functions.GridFilterClose(driver, baseURL, name: "_noopColumnName");
            Assert.AreEqual("TestProduct1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("TestProduct10", GetGridCell(9, "Name").Text);
        }
    }
}

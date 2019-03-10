using System;
using NUnit.Framework;
using System.Threading;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Admin.GridProducts
{
    [TestFixture]
    public class GridProductsTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog);
            InitializeService.LoadData(
           "data\\Admin\\Grid\\GridProductsTest\\Catalog.Product.csv",
           "data\\Admin\\Grid\\GridProductsTest\\Catalog.Offer.csv",
           "data\\Admin\\Grid\\GridProductsTest\\Catalog.Category.csv",
           "data\\Admin\\Grid\\GridProductsTest\\Catalog.ProductCategories.csv");
             
            Init();
        }
        
        [Test]
        public void ProductInCategory()
        {
            //Functions.RecalculateSearch(driver, baseURL);
            GoToAdmin("catalog?categoryid=1");
            
            /* check Activity*/
            //1 товар активен в CSV
            Assert.IsTrue(GetGridCell(0, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);

            //2 товар неактивен в CSV
            Assert.IsFalse(GetGridCell(1, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);

            //товар 2 был неактивный, сделать его активным
            GetGridCell(1, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffLabel\"]")).Click();

            //товар 1 был активный, сделать его неактивным
            GetGridCell(0, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffLabel\"]")).Click();

            Assert.IsTrue(GetGridCell(1, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);
            Assert.IsFalse(GetGridCell(0, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);

            //check go to edit
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector("[ng-href=\"product/edit/1\"]")).Click();
            Assert.IsTrue(driver.FindElement(By.TagName("h1")).Text.Contains("Товар \"TestProduct1\""));

            driver.FindElement(By.LinkText("TestCategory1")).Click();
            Thread.Sleep(2000);

            //Assert.AreEqual("TestProduct1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("TestCategory1", driver.FindElement(By.CssSelector(".sticky-page-name")).FindElement(By.TagName("h2")).Text);
            Assert.IsTrue(driver.Url.Contains("catalog?categoryId=1"));
            
            /* check search */
            GoToAdmin("catalog?categoryid=1");

            //check search exist product
            Assert.AreEqual("Найдено записей: 20", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text);
            GetGridFilter().SendKeys("TestProduct4");
            DropFocus("h2");
            Refresh();
            Assert.AreEqual("Найдено записей: 1", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text);
            Assert.AreEqual("TestProduct4", GetGridCell(0, "Name").Text);

            //check search not exist product
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("TestProduct22");
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));

            //check search too much symbols 
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("1111111111222222222222222222333333333333334444444444rrrrrrrrrrrreeeeeeeeewwwwwwwwwwwwwwwwwww");
            DropFocus("h2");
            Refresh();
            Assert.AreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text);
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));

            //check search invalid symbols 
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("########@@@@@@@@&&&&&&&******,,,,..");
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));
        }

        [Test]
        public void ProductGridSort()
        {
            GoToAdmin("catalog?categoryid=1");

            //check sort by name
            GetGridCell(-1, "Name").Click();
            WaitForAjax();
            Assert.AreEqual("TestProduct1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("TestProduct18", GetGridCell(9, "Name").Text);

            GetGridCell(-1, "Name").Click();
            WaitForAjax();
            Assert.AreEqual("TestProduct9", GetGridCell(0, "Name").Text);
            Assert.AreEqual("TestProduct19", GetGridCell(9, "Name").Text);
            
            //check sort by ProductArtNo
            GetGridCell(-1, "ProductArtNo").Click();
            WaitForAjax();
            Assert.AreEqual("TestProduct1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("1", GetGridCell(0, "ProductArtNo").Text);
            Assert.AreEqual("TestProduct18", GetGridCell(9, "Name").Text);
            Assert.AreEqual("18", GetGridCell(9, "ProductArtNo").Text);

            GetGridCell(-1, "ProductArtNo").Click();
            WaitForAjax();
            Assert.AreEqual("TestProduct9", GetGridCell(0, "Name").Text);
            Assert.AreEqual("9", GetGridCell(0, "ProductArtNo").Text);
            Assert.AreEqual("TestProduct19", GetGridCell(9, "Name").Text);
            Assert.AreEqual("19", GetGridCell(9, "ProductArtNo").Text);
            
            //check sort by Price
            GetGridCell(-1, "PriceString").Click();
            WaitForAjax();
            Assert.AreEqual("1  руб.", GetGridCell(0, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("10  руб.", GetGridCell(9, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));

            GetGridCell(-1, "PriceString").Click();
            WaitForAjax();
            Assert.AreEqual("20  руб.", GetGridCell(0, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("11  руб.", GetGridCell(9, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));
            
            //check sort by Amount
            GetGridCell(-1, "Amount").Click();
            WaitForAjax();
            Assert.AreEqual("1", GetGridCell(0, "Amount").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("10", GetGridCell(9, "Amount").FindElement(By.TagName("input")).GetAttribute("value"));

            GetGridCell(-1, "Amount").Click();
            WaitForAjax();
            Assert.AreEqual("20", GetGridCell(0, "Amount").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("11", GetGridCell(9, "Amount").FindElement(By.TagName("input")).GetAttribute("value"));
            
            //check sort by SortOrder
            GetGridCell(-1, "SortOrder").Click();
            WaitForAjax();
            Assert.AreEqual("1", GetGridCell(0, "SortOrder").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("10", GetGridCell(9, "SortOrder").FindElement(By.TagName("input")).GetAttribute("value"));

            GetGridCell(-1, "SortOrder").Click();
            WaitForAjax();
            Assert.AreEqual("20", GetGridCell(0, "SortOrder").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("11", GetGridCell(9, "SortOrder").FindElement(By.TagName("input")).GetAttribute("value"));
            
            //check sort by Activity
            GetGridCell(-1, "Enabled").Click();
            WaitForAjax();
            Assert.IsFalse(GetGridCell(0, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);
            Assert.IsFalse(GetGridCell(1, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);

            GetGridCell(-1, "Enabled").Click();
            WaitForAjax();
            Assert.IsTrue(GetGridCell(0, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);
            Assert.IsTrue(GetGridCell(9, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);
        }
    }
}

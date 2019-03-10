using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.Web.Site.Selenium.Test.Admin.PriceRegulation
{
    [TestFixture]
    public class PriceRegulationIncreaseSupplyPriceTest : BaseMultiSeleniumTest
    {
        [SetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog);
            InitializeService.LoadData(
           "data\\Admin\\Catalog\\PriceRegulation\\PriceRegulationIncreaseSupplyPriceTest\\Catalog.Product.csv",
           "data\\Admin\\Catalog\\PriceRegulation\\PriceRegulationIncreaseSupplyPriceTest\\Catalog.Offer.csv",
           "data\\Admin\\Catalog\\PriceRegulation\\PriceRegulationIncreaseSupplyPriceTest\\Catalog.Category.csv",
           "data\\Admin\\Catalog\\PriceRegulation\\PriceRegulationIncreaseSupplyPriceTest\\Catalog.Brand.csv",
           "data\\Admin\\Catalog\\PriceRegulation\\PriceRegulationIncreaseSupplyPriceTest\\Catalog.ProductCategories.csv"
           );
             
            Init();
        }
        
        [Test]
        public void IncreaseSupplyPriceAllNumber()
        {
            Functions.PriceRegulation(driver, baseURL, Select: "+ от закупочной цены", SelectOption: "абсолютное число");
            driver.FindElement(By.CssSelector("[data-e2e=\"PriceRegulationAllCheckbox\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"PriceRegulationValue\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"PriceRegulationValue\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"PriceRegulationValue\"]")).SendKeys("1000");
            driver.FindElement(By.CssSelector("[data-e2e=\"PriceRegulationOk\"]")).Click();
            WaitForAjax();

            //assert
            GoToAdmin("catalog");
            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory1')]")).Click();
            Thread.Sleep(3000);
            Assert.AreEqual("TestProduct1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("1 100  руб.", GetGridCell(0, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("TestProduct10", GetGridCell(9, "Name").Text);
            Assert.AreEqual("2 000  руб.", GetGridCell(9, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));
            
            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory3')]")).Click();
            Thread.Sleep(3000);
            Assert.AreEqual("TestProduct41", GetGridCell(0, "Name").Text);
            Assert.AreEqual("5 100  руб.", GetGridCell(0, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("TestProduct50", GetGridCell(9, "Name").Text);
            Assert.AreEqual("6 000  руб.", GetGridCell(9, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));

            
        }

        [Test]
        public void IncreaseSupplyPriceCategoryNumber()
        {
            Functions.PriceRegulation(driver, baseURL, Select: "+ от закупочной цены", SelectOption: "абсолютное число");

            driver.FindElement(By.CssSelector("[data-e2e=\"PriceRegulationAllCheckbox\"]")).Click();
            Thread.Sleep(1000);
            Assert.IsTrue(driver.FindElements(By.CssSelector(".jstree-container-ul.jstree-children.jstree-no-dots")).Count == 0);
            driver.FindElement(By.CssSelector("[data-e2e=\"PriceRegulationCatCheckbox\"]")).Click();
            Thread.Sleep(1000);
            Assert.IsTrue(driver.FindElements(By.CssSelector(".jstree-container-ul.jstree-children.jstree-no-dots")).Count == 1);
            
            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory2')]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.CssSelector("[data-e2e=\"PriceRegulationValue\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"PriceRegulationValue\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"PriceRegulationValue\"]")).SendKeys("2000");
            driver.FindElement(By.CssSelector("[data-e2e=\"PriceRegulationOk\"]")).Click();
            WaitForAjax();

            //assert
            GoToAdmin("catalog");
            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory1')]")).Click();
            Thread.Sleep(3000);
            Assert.AreEqual("TestProduct1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("1  руб.", GetGridCell(0, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("TestProduct10", GetGridCell(9, "Name").Text);
            Assert.AreEqual("10  руб.", GetGridCell(9, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));

            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory2')]")).Click();
            Thread.Sleep(3000);
            Assert.AreEqual("TestProduct21", GetGridCell(0, "Name").Text);
            Assert.AreEqual("4 100  руб.", GetGridCell(0, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("TestProduct30", GetGridCell(9, "Name").Text);
            Assert.AreEqual("5 000  руб.", GetGridCell(9, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));

            
        }

        [Test]
        public void IncreaseSupplyPriceCategoryWithSubNumber()
        {
            Functions.PriceRegulation(driver, baseURL, Select: "+ от закупочной цены", SelectOption: "абсолютное число");
            driver.FindElement(By.CssSelector("[data-e2e=\"PriceRegulationCatCheckbox\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory4')]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.CssSelector("[data-e2e=\"PriceRegulationValue\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"PriceRegulationValue\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"PriceRegulationValue\"]")).SendKeys("500");
            driver.FindElement(By.CssSelector("[data-e2e=\"PriceRegulationOk\"]")).Click();
            WaitForAjax();

            //assert
            GoToAdmin("catalog");
            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory1')]")).Click();
            Thread.Sleep(3000);
            Assert.AreEqual("TestProduct1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("1  руб.", GetGridCell(0, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("TestProduct10", GetGridCell(9, "Name").Text);
            Assert.AreEqual("10  руб.", GetGridCell(9, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));

            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory4')]")).Click();
            Thread.Sleep(3000);
            Assert.AreEqual("TestProduct61", GetGridCell(0, "Name").Text);
            Assert.AreEqual("6 600  руб.", GetGridCell(0, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("TestProduct70", GetGridCell(9, "Name").Text);
            Assert.AreEqual("7 500  руб.", GetGridCell(9, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));

            driver.FindElement(By.Id("4")).FindElement(By.CssSelector(".jstree-icon.jstree-ocl")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory5')]")).Click();
            Thread.Sleep(3000);
            Assert.AreEqual("TestProduct81", GetGridCell(0, "Name").Text);
            Assert.AreEqual("8 600  руб.", GetGridCell(0, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("TestProduct90", GetGridCell(9, "Name").Text);
            Assert.AreEqual("9 500  руб.", GetGridCell(9, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));

            
        }
        
        [Test]
        public void IncreaseSupplyPriceSubCategoryNumber()
        {
            Functions.PriceRegulation(driver, baseURL, Select: "+ от закупочной цены", SelectOption: "абсолютное число");
            driver.FindElement(By.CssSelector("[data-e2e=\"PriceRegulationCatCheckbox\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElements(By.CssSelector(".jstree-icon.jstree-ocl"))[3].Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory5')]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.CssSelector("[data-e2e=\"PriceRegulationValue\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"PriceRegulationValue\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"PriceRegulationValue\"]")).SendKeys("1000");
            driver.FindElement(By.CssSelector("[data-e2e=\"PriceRegulationOk\"]")).Click();
            WaitForAjax();

            //assert
            GoToAdmin("catalog");
            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory1')]")).Click();
            Thread.Sleep(3000);
            Assert.AreEqual("TestProduct1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("1  руб.", GetGridCell(0, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("TestProduct10", GetGridCell(9, "Name").Text);
            Assert.AreEqual("10  руб.", GetGridCell(9, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));

            ScrollTo(By.CssSelector("[data-e2e=\"gridFilterDropdownButton\"]"));
            /* изменения только в подкатегории?
            Thread.Sleep(3000);

            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory4')]")).Click();
            Thread.Sleep(2000);

           ScrollTo(By.CssSelector("[data-e2e=\"gridFilterDropdownButton\"]"));

            Assert.AreEqual("TestProduct61", GetGridCell(0, "Name").Text);
            Thread.Sleep(1000);
            Assert.AreEqual("61", driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"grid[0][\'Price\']\"] input")).GetAttribute("value"));
            Thread.Sleep(1000);
            Assert.AreEqual("TestProduct70", GetGridCell(9, "Name").Text);
            Thread.Sleep(1000);
            Assert.AreEqual("70", driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"grid[9][\'Price\']\"] input")).GetAttribute("value"));*/

            driver.FindElement(By.Id("4")).FindElement(By.CssSelector(".jstree-icon.jstree-ocl")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory5')]")).Click();
            Thread.Sleep(3000);
            Assert.AreEqual("TestProduct81", GetGridCell(0, "Name").Text);
            Assert.AreEqual("9 100  руб.", GetGridCell(0, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("TestProduct90", GetGridCell(9, "Name").Text);
            Assert.AreEqual("10 000  руб.", GetGridCell(9, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));

            
        }
        

        [Test]
        public void IncreaseSupplyPriceAllPercent()
        {
            Functions.PriceRegulation(driver, baseURL, Select: "+ от закупочной цены", SelectOption: "%");
            driver.FindElement(By.CssSelector("[data-e2e=\"PriceRegulationAllCheckbox\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"PriceRegulationValue\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"PriceRegulationValue\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"PriceRegulationValue\"]")).SendKeys("5");
            driver.FindElement(By.CssSelector("[data-e2e=\"PriceRegulationOk\"]")).Click();
            WaitForAjax();

            //assert
            GoToAdmin("catalog");
            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory1')]")).Click();
            Thread.Sleep(3000);
            Assert.AreEqual("TestProduct1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("105  руб.", GetGridCell(0, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("TestProduct10", GetGridCell(9, "Name").Text);
            Assert.AreEqual("1 050  руб.", GetGridCell(9, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));

            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory3')]")).Click();
            Thread.Sleep(3000);
            Assert.AreEqual("TestProduct41", GetGridCell(0, "Name").Text);
            Assert.AreEqual("4 305  руб.", GetGridCell(0, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("TestProduct50", GetGridCell(9, "Name").Text);
            Assert.AreEqual("5 250  руб.", GetGridCell(9, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));

            
        }

        [Test]
        public void IncreaseSupplyPriceCategoryPercent()
        {
            Functions.PriceRegulation(driver, baseURL, Select: "+ от закупочной цены", SelectOption: "%");
            driver.FindElement(By.CssSelector("[data-e2e=\"PriceRegulationCatCheckbox\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory2')]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.CssSelector("[data-e2e=\"PriceRegulationValue\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"PriceRegulationValue\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"PriceRegulationValue\"]")).SendKeys("10");
            driver.FindElement(By.CssSelector("[data-e2e=\"PriceRegulationOk\"]")).Click();
            WaitForAjax();

            //assert
            GoToAdmin("catalog");
            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory1')]")).Click();
            Thread.Sleep(3000);
            Assert.AreEqual("TestProduct1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("1  руб.", GetGridCell(0, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("TestProduct10", GetGridCell(9, "Name").Text);
            Assert.AreEqual("10  руб.", GetGridCell(9, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));
            
            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory2')]")).Click();
            Thread.Sleep(3000);
            Assert.AreEqual("TestProduct21", GetGridCell(0, "Name").Text);
            Assert.AreEqual("2 310  руб.", GetGridCell(0, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("TestProduct30", GetGridCell(9, "Name").Text);
            Assert.AreEqual("3 300  руб.", GetGridCell(9, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));

            
        }

        [Test]
        public void IncreaseSupplyPriceCategoryWithSubPercent()
        {
            Functions.PriceRegulation(driver, baseURL, Select: "+ от закупочной цены", SelectOption: "%");
            driver.FindElement(By.CssSelector("[data-e2e=\"PriceRegulationCatCheckbox\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory4')]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.CssSelector("[data-e2e=\"PriceRegulationValue\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"PriceRegulationValue\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"PriceRegulationValue\"]")).SendKeys("5");
            driver.FindElement(By.CssSelector("[data-e2e=\"PriceRegulationOk\"]")).Click();
            WaitForAjax();

            //assert
            GoToAdmin("catalog");
            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory1')]")).Click();
            Thread.Sleep(3000);
            Assert.AreEqual("TestProduct1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("1  руб.", GetGridCell(0, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("TestProduct10", GetGridCell(9, "Name").Text);
            Assert.AreEqual("10  руб.", GetGridCell(9, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));
            
            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory4')]")).Click();
            Thread.Sleep(3000);
            Assert.AreEqual("TestProduct61", GetGridCell(0, "Name").Text);
            Assert.AreEqual("6 405  руб.", GetGridCell(0, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("TestProduct70", GetGridCell(9, "Name").Text);
            Assert.AreEqual("7 350  руб.", GetGridCell(9, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));
            
            driver.FindElement(By.Id("4")).FindElement(By.CssSelector(".jstree-icon.jstree-ocl")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory5')]")).Click();
            Thread.Sleep(3000);
            Assert.AreEqual("TestProduct81", GetGridCell(0, "Name").Text);
            Assert.AreEqual("8 505  руб.", GetGridCell(0, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("TestProduct90", GetGridCell(9, "Name").Text);
            Assert.AreEqual("9 450  руб.", GetGridCell(9, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));

            
        }

        [Test]
        public void IncreaseSupplyPriceSubCategoryPercent()
        {
            Functions.PriceRegulation(driver, baseURL, Select: "+ от закупочной цены", SelectOption: "%");
            driver.FindElement(By.CssSelector("[data-e2e=\"PriceRegulationCatCheckbox\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElements(By.CssSelector(".jstree-icon.jstree-ocl"))[3].Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory5')]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.CssSelector("[data-e2e=\"PriceRegulationValue\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"PriceRegulationValue\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"PriceRegulationValue\"]")).SendKeys("7");
            driver.FindElement(By.CssSelector("[data-e2e=\"PriceRegulationOk\"]")).Click();
            WaitForAjax();

            //assert
            GoToAdmin("catalog");
            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory1')]")).Click();
            Thread.Sleep(3000);
            Assert.AreEqual("TestProduct1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("1  руб.", GetGridCell(0, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("TestProduct10", GetGridCell(9, "Name").Text);
            Assert.AreEqual("10  руб.", GetGridCell(9, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));

            ScrollTo(By.CssSelector("[data-e2e=\"gridFilterDropdownButton\"]"));

            /* изменени только в подкатегории???
            Thread.Sleep(3000);

            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory4')]")).Click();
            Thread.Sleep(2000);

           ScrollTo(By.CssSelector("[data-e2e=\"gridFilterDropdownButton\"]"));

            Assert.AreEqual("TestProduct61", GetGridCell(0, "Name").Text);
            Thread.Sleep(1000);
            Assert.AreEqual("61", driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"grid[0][\'Price\']\"] input")).GetAttribute("value"));
            Thread.Sleep(1000);
            Assert.AreEqual("TestProduct70", GetGridCell(9, "Name").Text);
            Thread.Sleep(1000);
            Assert.AreEqual("70", driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"grid[9][\'Price\']\"] input")).GetAttribute("value")); */
            
            driver.FindElement(By.Id("4")).FindElement(By.CssSelector(".jstree-icon.jstree-ocl")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory5')]")).Click();
            Thread.Sleep(3000);
            Assert.AreEqual("TestProduct81", GetGridCell(0, "Name").Text);
            Assert.AreEqual("8 667  руб.", GetGridCell(0, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));
            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            MouseFocus(driver, By.TagName("ui-grid-custom-footer"));
            Thread.Sleep(2000);
            Assert.AreEqual("TestProduct90", GetGridCell(9, "Name").Text);
            Assert.AreEqual("9 630  руб.", GetGridCell(9, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));

            
        }

    }
}

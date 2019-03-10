using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.Web.Site.Selenium.Test.Admin.PriceRegulation
{
    [TestFixture]
    public class PriceRegulationDecreaseTest : BaseMultiSeleniumTest
    {
        [SetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog);
            InitializeService.LoadData(
           "data\\Admin\\Catalog\\PriceRegulation\\PriceRegulationDecreaseTest\\Catalog.Product.csv",
           "data\\Admin\\Catalog\\PriceRegulation\\PriceRegulationDecreaseTest\\Catalog.Offer.csv",
           "data\\Admin\\Catalog\\PriceRegulation\\PriceRegulationDecreaseTest\\Catalog.Category.csv",
           "data\\Admin\\Catalog\\PriceRegulation\\PriceRegulationDecreaseTest\\Catalog.Brand.csv",
           "data\\Admin\\Catalog\\PriceRegulation\\PriceRegulationDecreaseTest\\Catalog.ProductCategories.csv"
           );
             
            Init();
        }
        
        [Test]
        public void DecreaseAllNumber()
        {
            Functions.PriceRegulation(driver, baseURL, Select: "Уменьшить", SelectOption: "абсолютное число");
            driver.FindElement(By.CssSelector("[data-e2e=\"PriceRegulationAllCheckbox\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"PriceRegulationValue\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"PriceRegulationValue\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"PriceRegulationValue\"]")).SendKeys("500");
            driver.FindElement(By.CssSelector("[data-e2e=\"PriceRegulationOk\"]")).Click();
            WaitForAjax();

            //assert
            GoToAdmin("catalog");
            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory1')]")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("TestProduct1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("500  руб.", GetGridCell(0, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("TestProduct10", GetGridCell(9, "Name").Text);
            Assert.AreEqual("9 500  руб.", GetGridCell(9, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));

            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory3')]")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("TestProduct41", GetGridCell(0, "Name").Text);
            Assert.AreEqual("40 500  руб.", GetGridCell(0, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("TestProduct50", GetGridCell(9, "Name").Text);
            Assert.AreEqual("49 500  руб.", GetGridCell(9, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));

            
        }

        [Test]
        public void DecreaseCategoryNumber()
        {
            Functions.PriceRegulation(driver, baseURL, Select: "Уменьшить", SelectOption: "абсолютное число");
            driver.FindElement(By.CssSelector("[data-e2e=\"PriceRegulationCatCheckbox\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory2')]")).Click();
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
            Assert.AreEqual("1 000  руб.", GetGridCell(0, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("TestProduct10", GetGridCell(9, "Name").Text);
            Assert.AreEqual("10 000  руб.", GetGridCell(9, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));

            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory2')]")).Click();
            Thread.Sleep(3000);
            Assert.AreEqual("TestProduct21", GetGridCell(0, "Name").Text);
            Assert.AreEqual("20 500  руб.", GetGridCell(0, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("TestProduct30", GetGridCell(9, "Name").Text);
            Assert.AreEqual("29 500  руб.", GetGridCell(9, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));

            
        }
        
        [Test]
        public void DecreaseCategoryWithSubNumber()
        {
            Functions.PriceRegulation(driver, baseURL, Select: "Уменьшить", SelectOption: "абсолютное число");
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
            Assert.AreEqual("1 000  руб.", GetGridCell(0, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("TestProduct10", GetGridCell(9, "Name").Text);
            Assert.AreEqual("10 000  руб.", GetGridCell(9, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));

            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory4')]")).Click();
            Thread.Sleep(3000);
            Assert.AreEqual("TestProduct61", GetGridCell(0, "Name").Text);
            Assert.AreEqual("60 500  руб.", GetGridCell(0, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("TestProduct70", GetGridCell(9, "Name").Text);
            Assert.AreEqual("69 500  руб.", GetGridCell(9, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));

            driver.FindElement(By.Id("4")).FindElement(By.CssSelector(".jstree-icon.jstree-ocl")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory5')]")).Click();
            Thread.Sleep(3000);
            Assert.AreEqual("TestProduct81", GetGridCell(0, "Name").Text);
            Assert.AreEqual("80 500  руб.", GetGridCell(0, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("TestProduct90", GetGridCell(9, "Name").Text);
            Assert.AreEqual("89 500  руб.", GetGridCell(9, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));

            
        }

        [Test]
        public void DecreaseSubCategoryNumber()
        {
            Functions.PriceRegulation(driver, baseURL, Select: "Уменьшить", SelectOption: "абсолютное число");
            driver.FindElement(By.CssSelector("[data-e2e=\"PriceRegulationCatCheckbox\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElements(By.CssSelector(".jstree-icon.jstree-ocl"))[3].Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory5')]")).Click();
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
            Assert.AreEqual("1 000  руб.", GetGridCell(0, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("TestProduct10", GetGridCell(9, "Name").Text);
            Assert.AreEqual("10 000  руб.", GetGridCell(9, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));

            ScrollTo(By.CssSelector("[data-e2e=\"gridFilterDropdownButton\"]"));

            /*изменение только в подкатегории?
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory4')]")).Click();
            Thread.Sleep(2000);

         ScrollTo(By.CssSelector("[data-e2e=\"gridFilterDropdownButton\"]"));

            Assert.AreEqual("TestProduct61", GetGridCell(0, "Name").Text);
            Thread.Sleep(1000);
            Assert.AreEqual("61000", driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"grid[0][\'Price\']\"] input")).GetAttribute("value"));
            Thread.Sleep(1000);
            Assert.AreEqual("TestProduct70", GetGridCell(9, "Name").Text);
            Thread.Sleep(1000);
            Assert.AreEqual("70000", driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"grid[9][\'Price\']\"] input")).GetAttribute("value")); */

            driver.FindElements(By.CssSelector(".jstree-icon.jstree-ocl"))[3].Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory5')]")).Click();
            Thread.Sleep(3000);
            Assert.AreEqual("TestProduct81", GetGridCell(0, "Name").Text);
            Assert.AreEqual("80 500  руб.", GetGridCell(0, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("TestProduct90", GetGridCell(9, "Name").Text);
            Assert.AreEqual("89 500  руб.", GetGridCell(9, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));

            
        }
        
        [Test]
        public void DecreaseAllPercent()
        {
            Functions.PriceRegulation(driver, baseURL, Select: "Уменьшить", SelectOption: "%");

            driver.FindElement(By.CssSelector("[data-e2e=\"PriceRegulationAllCheckbox\"]")).Click();
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
            Assert.AreEqual("900  руб.", GetGridCell(0, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("TestProduct10", GetGridCell(9, "Name").Text);
            Assert.AreEqual("9 000  руб.", GetGridCell(9, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));

            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory3')]")).Click();
            Thread.Sleep(3000);
            Assert.AreEqual("TestProduct41", GetGridCell(0, "Name").Text);
            Assert.AreEqual("36 900  руб.", GetGridCell(0, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("TestProduct50", GetGridCell(9, "Name").Text);
            Assert.AreEqual("45 000  руб.", GetGridCell(9, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));

            
        }

        [Test]
        public void DecreaseCategoryPercent()
        {
            Functions.PriceRegulation(driver, baseURL, Select: "Уменьшить", SelectOption: "%");

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
            driver.FindElement(By.CssSelector("[data-e2e=\"PriceRegulationValue\"]")).SendKeys("5");
            driver.FindElement(By.CssSelector("[data-e2e=\"PriceRegulationOk\"]")).Click();
            WaitForAjax();

            //assert
            GoToAdmin("catalog");
            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory1')]")).Click();
            Thread.Sleep(3000);
            Assert.AreEqual("TestProduct1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("1 000  руб.", GetGridCell(0, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("TestProduct10", GetGridCell(9, "Name").Text);
            Assert.AreEqual("10 000  руб.", GetGridCell(9, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));

            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory2')]")).Click();
            Thread.Sleep(3000);
            Assert.AreEqual("TestProduct21", GetGridCell(0, "Name").Text);
            Assert.AreEqual("19 950  руб.", GetGridCell(0, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("TestProduct30", GetGridCell(9, "Name").Text);
            Assert.AreEqual("28 500  руб.", GetGridCell(9, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));

            
        }

        [Test]
        public void DecreaseCategoryWithSubPercent()
        {
            Functions.PriceRegulation(driver, baseURL, Select: "Уменьшить", SelectOption: "%");
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
            Assert.AreEqual("1 000  руб.", GetGridCell(0, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("TestProduct10", GetGridCell(9, "Name").Text);
            Assert.AreEqual("10 000  руб.", GetGridCell(9, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));

            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory4')]")).Click();
            Thread.Sleep(3000);
            Assert.AreEqual("TestProduct61", GetGridCell(0, "Name").Text);
            Assert.AreEqual("57 950  руб.", GetGridCell(0, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("TestProduct70", GetGridCell(9, "Name").Text);
            Assert.AreEqual("66 500  руб.", GetGridCell(9, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));

            driver.FindElement(By.Id("4")).FindElement(By.CssSelector(".jstree-icon.jstree-ocl")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory5')]")).Click();
            Thread.Sleep(3000);
            Assert.AreEqual("TestProduct81", GetGridCell(0, "Name").Text);
            Assert.AreEqual("76 950  руб.", GetGridCell(0, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("TestProduct90", GetGridCell(9, "Name").Text);
            Assert.AreEqual("85 500  руб.", GetGridCell(9, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));

            
        }

        [Test]
        public void DecreaseSubCategoryPercent()
        {
            Functions.PriceRegulation(driver, baseURL, Select: "Уменьшить", SelectOption: "%");
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
            Assert.AreEqual("1 000  руб.", GetGridCell(0, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("TestProduct10", GetGridCell(9, "Name").Text);
            Assert.AreEqual("10 000  руб.", GetGridCell(9, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));

            ScrollTo(By.CssSelector("[data-e2e=\"gridFilterDropdownButton\"]"));

            /*изменение только в подкатегории?
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory4')]")).Click();
            Thread.Sleep(2000);

            ScrollTo(By.CssSelector("[data-e2e=\"gridFilterDropdownButton\"]"));

            Assert.AreEqual("TestProduct61", GetGridCell(0, "Name").Text);
            Thread.Sleep(1000);
            Assert.AreEqual("61000", driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"grid[0][\'Price\']\"] input")).GetAttribute("value"));
            Thread.Sleep(1000);
            Assert.AreEqual("TestProduct70", GetGridCell(9, "Name").Text);
            Thread.Sleep(1000);
            Assert.AreEqual("70000", driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"grid[9][\'Price\']\"] input")).GetAttribute("value")); */

            driver.FindElement(By.Id("4")).FindElement(By.CssSelector(".jstree-icon.jstree-ocl")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory5')]")).Click();
            Thread.Sleep(3000);
            Assert.AreEqual("TestProduct81", GetGridCell(0, "Name").Text);
            Assert.AreEqual("75 330  руб.", GetGridCell(0, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));
            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            MouseFocus(driver, By.TagName("ui-grid-custom-footer"));
            Thread.Sleep(2000);
            Assert.AreEqual("TestProduct90", GetGridCell(9, "Name").Text);
            Assert.AreEqual("83 700  руб.", GetGridCell(9, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));

            
        }
    }
}

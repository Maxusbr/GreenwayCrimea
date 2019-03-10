using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.Web.Site.Selenium.Test.Admin.PriceRegulation
{
    [TestFixture]
    public class PriceRegulationIncreasePriceZeroTest : BaseMultiSeleniumTest
    {
        [SetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog);
            InitializeService.LoadData(
           "data\\Admin\\Catalog\\PriceRegulation\\PriceRegulationIncreasePriceZeroTest\\Catalog.Product.csv",
           "data\\Admin\\Catalog\\PriceRegulation\\PriceRegulationIncreasePriceZeroTest\\Catalog.Offer.csv",
           "data\\Admin\\Catalog\\PriceRegulation\\PriceRegulationIncreasePriceZeroTest\\Catalog.Category.csv",
           "data\\Admin\\Catalog\\PriceRegulation\\PriceRegulationIncreasePriceZeroTest\\Catalog.Brand.csv",
           "data\\Admin\\Catalog\\PriceRegulation\\PriceRegulationIncreasePriceZeroTest\\Catalog.ProductCategories.csv"
           );
             
            Init();
        }
        
        [Test]
        public void IncreasePriceZeroAllNumber()
        {
            Functions.PriceRegulation(driver, baseURL, Select: "Увеличить", SelectOption: "абсолютное число");
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
            Assert.AreEqual("0 руб.", GetGridCell(0, "PriceString").Text);
            Assert.AreEqual("TestProduct10", GetGridCell(9, "Name").Text);
            Assert.AreEqual("0 руб.", GetGridCell(9, "PriceString").Text);

            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory3')]")).Click();
            Thread.Sleep(3000);
            Assert.AreEqual("TestProduct41", GetGridCell(0, "Name").Text);
            Assert.AreEqual("0 руб.", GetGridCell(0, "PriceString").Text);
            Assert.AreEqual("TestProduct50", GetGridCell(9, "Name").Text);
            Assert.AreEqual("0 руб.", GetGridCell(9, "PriceString").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            Refresh();
            Assert.AreEqual("TestProduct51", GetGridCell(0, "Name").Text);
            Assert.AreEqual("1 051  руб.", GetGridCell(0, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("TestProduct60", GetGridCell(9, "Name").Text);
            Assert.AreEqual("1 060  руб.", GetGridCell(9, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));

            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory4')]")).Click();
            Thread.Sleep(3000);
            Assert.AreEqual("TestProduct61", GetGridCell(0, "Name").Text);
            Assert.AreEqual("1 061  руб.", GetGridCell(0, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("TestProduct70", GetGridCell(9, "Name").Text);
            Assert.AreEqual("1 070  руб.", GetGridCell(9, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));

            
        }

        [Test]
        public void IncreasePriceZeroCategoryNumber()
        {
            Functions.PriceRegulation(driver, baseURL, Select: "Увеличить", SelectOption: "абсолютное число");
            driver.FindElement(By.CssSelector("[data-e2e=\"PriceRegulationCatCheckbox\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory3')]")).Click();
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
            Assert.AreEqual("0 руб.", GetGridCell(0, "PriceString").Text);
            Assert.AreEqual("TestProduct10", GetGridCell(9, "Name").Text);
            Assert.AreEqual("0 руб.", GetGridCell(9, "PriceString").Text);

            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory3')]")).Click();
            Thread.Sleep(3000);
            Assert.AreEqual("TestProduct41", GetGridCell(0, "Name").Text);
            Assert.AreEqual("0 руб.", GetGridCell(0, "PriceString").Text);
            Assert.AreEqual("TestProduct50", GetGridCell(9, "Name").Text);
            Assert.AreEqual("0 руб.", GetGridCell(9, "PriceString").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            Refresh();
            Assert.AreEqual("TestProduct51", GetGridCell(0, "Name").Text);
            Assert.AreEqual("1 051  руб.", GetGridCell(0, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("TestProduct60", GetGridCell(9, "Name").Text);
            Assert.AreEqual("1 060  руб.", GetGridCell(9, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));

            
        }

        [Test]
        public void IncreasePriceZeroSubCategoryNumber()
        {
            Functions.PriceRegulation(driver, baseURL, Select: "Увеличить", SelectOption: "абсолютное число");
            driver.FindElement(By.CssSelector("[data-e2e=\"PriceRegulationCatCheckbox\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElements(By.CssSelector(".jstree-icon.jstree-ocl"))[3].Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory5')]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"PriceRegulationValue\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"PriceRegulationValue\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"PriceRegulationValue\"]")).SendKeys("1000");
            driver.FindElement(By.CssSelector("[data-e2e=\"PriceRegulationOk\"]")).Click();
            WaitForAjax();

            //assert
            GoToAdmin("catalog");
            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory1')]")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("TestProduct1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("0 руб.", GetGridCell(0, "PriceString").Text);
            Assert.AreEqual("TestProduct10", GetGridCell(9, "Name").Text);
            Assert.AreEqual("0 руб.", GetGridCell(9, "PriceString").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridFilterDropdownButton\"]"));

            /*изменение только в подкатегории?
            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory4')]")).Click();
            Thread.Sleep(2000);

            ScrollTo(By.CssSelector("[data-e2e=\"gridFilterDropdownButton\"]"));

            Assert.AreEqual("TestProduct61", GetGridCell(0, "Name").Text);
            //Thread.Sleep(1000);
            Assert.AreEqual("0 руб.", GetGridCell(0, "PriceString").Text);
            //Thread.Sleep(1000);
            Assert.AreEqual("TestProduct70", GetGridCell(9, "Name").Text);
            //Thread.Sleep(1000);
            Assert.AreEqual("0 руб.", GetGridCell(0, "PriceString").Text);

            Thread.Sleep(2000);*/
            driver.FindElements(By.CssSelector(".jstree-icon.jstree-ocl"))[3].Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory5')]")).Click();
            Thread.Sleep(3000);
            Assert.AreEqual("TestProduct81", GetGridCell(0, "Name").Text);
            Assert.AreEqual("1 081  руб.", GetGridCell(0, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("TestProduct90", GetGridCell(9, "Name").Text);
            Assert.AreEqual("1 090  руб.", GetGridCell(9, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            Refresh();
            Assert.AreEqual("TestProduct91", GetGridCell(0, "Name").Text);
            Assert.AreEqual("1 091  руб.", GetGridCell(0, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("TestProduct100", GetGridCell(9, "Name").Text);
            Assert.AreEqual("1 100  руб.", GetGridCell(9, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            Refresh();
            Assert.AreEqual("TestProduct101", GetGridCell(0, "Name").Text);
            Assert.AreEqual("0 руб.", GetGridCell(0, "PriceString").Text);
            Assert.AreEqual("TestProduct110", GetGridCell(9, "Name").Text);
            Assert.AreEqual("0 руб.", GetGridCell(9, "PriceString").Text);

            
        }

        [Test]
        public void IncreasePriceZeroAllPercent()
        {
            Functions.PriceRegulation(driver, baseURL, Select: "Увеличить", SelectOption: "%");
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
            Assert.AreEqual("0 руб.", GetGridCell(0, "PriceString").Text);
            Assert.AreEqual("TestProduct10", GetGridCell(9, "Name").Text);
            Assert.AreEqual("0 руб.", GetGridCell(9, "PriceString").Text);

            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory3')]")).Click();
            Thread.Sleep(3000);
            Assert.AreEqual("TestProduct41", GetGridCell(0, "Name").Text);
            Assert.AreEqual("0 руб.", GetGridCell(0, "PriceString").Text);
            Assert.AreEqual("TestProduct50", GetGridCell(9, "Name").Text);
            Assert.AreEqual("0 руб.", GetGridCell(9, "PriceString").Text);

            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory4')]")).Click();
            Thread.Sleep(3000);
            Assert.AreEqual("TestProduct61", GetGridCell(0, "Name").Text);
            Assert.AreEqual("67,10  руб.", GetGridCell(0, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("TestProduct70", GetGridCell(9, "Name").Text);
            Assert.AreEqual("77  руб.", GetGridCell(9, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));

            
        }

        [Test]
        public void IncreasePriceZeroCategoryPercent()
        {
            Functions.PriceRegulation(driver, baseURL, Select: "Увеличить", SelectOption: "%");
            driver.FindElement(By.CssSelector("[data-e2e=\"PriceRegulationCatCheckbox\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory3')]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.CssSelector("[data-e2e=\"PriceRegulationValue\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"PriceRegulationValue\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"PriceRegulationValue\"]")).SendKeys("5");
            driver.FindElement(By.CssSelector("[data-e2e=\"PriceRegulationOk\"]")).Click();
            WaitForAjax();

            //assert
            GoToAdmin("catalog");
            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory1')]")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("TestProduct1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("0 руб.", GetGridCell(0, "PriceString").Text);
            Assert.AreEqual("TestProduct10", GetGridCell(9, "Name").Text);
            Assert.AreEqual("0 руб.", GetGridCell(9, "PriceString").Text);

            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory3')]")).Click();
            Thread.Sleep(3000);
            Assert.AreEqual("TestProduct41", GetGridCell(0, "Name").Text);
            Assert.AreEqual("0 руб.", GetGridCell(0, "PriceString").Text);
            Assert.AreEqual("TestProduct50", GetGridCell(9, "Name").Text);
            Assert.AreEqual("0 руб.", GetGridCell(9, "PriceString").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            Refresh();
            Assert.AreEqual("TestProduct51", GetGridCell(0, "Name").Text);
            Assert.AreEqual("53,55  руб.", GetGridCell(0, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("TestProduct60", GetGridCell(9, "Name").Text);
            Assert.AreEqual("63  руб.", GetGridCell(9, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));

            
        }

        [Test]
        public void IncreasePriceZeroSubCategoryPercent()
        {
            Functions.PriceRegulation(driver, baseURL, Select: "Увеличить", SelectOption: "%");
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
            Assert.AreEqual("0 руб.", GetGridCell(0, "PriceString").Text);
            Assert.AreEqual("TestProduct10", GetGridCell(9, "Name").Text);
            Assert.AreEqual("0 руб.", GetGridCell(9, "PriceString").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridFilterDropdownButton\"]"));

            /*изменение только в подкатегории?
            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory4')]")).Click();
            Thread.Sleep(2000);

            ScrollTo(By.CssSelector("[data-e2e=\"gridFilterDropdownButton\"]"));

            Assert.AreEqual("TestProduct61", GetGridCell(0, "Name").Text);
            //Thread.Sleep(1000);
            Assert.AreEqual("0 руб.", GetGridCell(0, "PriceString").Text);
            //Thread.Sleep(1000);
            Assert.AreEqual("TestProduct70", GetGridCell(9, "Name").Text);
            //Thread.Sleep(1000);
            Assert.AreEqual("0 руб.", GetGridCell(0, "PriceString").Text);

            Thread.Sleep(2000); */
            driver.FindElements(By.CssSelector(".jstree-icon.jstree-ocl"))[3].Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory5')]")).Click();
            Thread.Sleep(3000);
            Assert.AreEqual("TestProduct81", GetGridCell(0, "Name").Text);
            Assert.AreEqual("86,67  руб.", GetGridCell(0, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("TestProduct90", GetGridCell(9, "Name").Text);
            Assert.AreEqual("96,30  руб.", GetGridCell(9, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            Refresh();
            Assert.AreEqual("TestProduct91", GetGridCell(0, "Name").Text);
            Assert.AreEqual("97,37  руб.", GetGridCell(0, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("TestProduct100", GetGridCell(9, "Name").Text);
            Assert.AreEqual("107  руб.", GetGridCell(9, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("TestProduct101", GetGridCell(0, "Name").Text);
            Assert.AreEqual("0  руб.", GetGridCell(0, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("TestProduct110", GetGridCell(9, "Name").Text);
            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            MouseFocus(driver, By.TagName("ui-grid-custom-footer"));
            Thread.Sleep(2000);
            Assert.AreEqual("0  руб.", GetGridCell(9, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));

            
        }
    }
}

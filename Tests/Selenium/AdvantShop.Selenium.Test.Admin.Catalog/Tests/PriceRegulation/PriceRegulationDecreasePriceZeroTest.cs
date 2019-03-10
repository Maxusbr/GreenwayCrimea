using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.Web.Site.Selenium.Test.Admin.PriceRegulation
{
    [TestFixture]
    public class PriceRegulationDecreasePriceZeroTest : BaseMultiSeleniumTest
    {

        [SetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog);
            InitializeService.LoadData(
           "data\\Admin\\Catalog\\PriceRegulation\\PriceRegulationDecreasePriceZeroTest\\Catalog.Product.csv",
           "data\\Admin\\Catalog\\PriceRegulation\\PriceRegulationDecreasePriceZeroTest\\Catalog.Offer.csv",
           "data\\Admin\\Catalog\\PriceRegulation\\PriceRegulationDecreasePriceZeroTest\\Catalog.Category.csv",
           "data\\Admin\\Catalog\\PriceRegulation\\PriceRegulationDecreasePriceZeroTest\\Catalog.Brand.csv",
           "data\\Admin\\Catalog\\PriceRegulation\\PriceRegulationDecreasePriceZeroTest\\Catalog.ProductCategories.csv"
           );

            Init();
        }

        [Test]
        public void DecreasePriceZeroAllNumber()
        {
            Functions.PriceRegulation(driver, baseURL, Select: "Уменьшить", SelectOption: "абсолютное число");
            driver.FindElement(By.CssSelector("[data-e2e=\"PriceRegulationAllCheckbox\"]")).Click();
            //Thread.Sleep(1000);
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
            Assert.AreEqual("0 руб.", GetGridCell(0, "PriceString").Text);
            Assert.AreEqual("TestProduct10", GetGridCell(9, "Name").Text);
            Assert.AreEqual("0 руб.", GetGridCell(9, "PriceString").Text);

            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory3')]")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("TestProduct41", GetGridCell(0, "Name").Text);
            Assert.AreEqual("0 руб.", GetGridCell(0, "PriceString").Text);
            Assert.AreEqual("TestProduct50", GetGridCell(9, "Name").Text);
            Assert.AreEqual("0 руб.", GetGridCell(9, "PriceString").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);

            Refresh();

            Assert.AreEqual("TestProduct51", GetGridCell(0, "Name").Text);
            Assert.AreEqual("50 500  руб.", GetGridCell(0, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("TestProduct60", GetGridCell(9, "Name").Text);
            Assert.AreEqual("59 500  руб.", GetGridCell(9, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));

            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory4')]")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("TestProduct61", GetGridCell(0, "Name").Text);
            Assert.AreEqual("60 500  руб.", GetGridCell(0, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("TestProduct70", GetGridCell(9, "Name").Text);
            Assert.AreEqual("69 500  руб.", GetGridCell(9, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));


        }

        [Test]
        public void DecreasePriceZeroCategoryNumber()
        {
            Functions.PriceRegulation(driver, baseURL, Select: "Уменьшить", SelectOption: "абсолютное число");
            driver.FindElement(By.CssSelector("[data-e2e=\"PriceRegulationCatCheckbox\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory3')]")).Click();
            Thread.Sleep(2000);
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
            Assert.AreEqual("0 руб.", GetGridCell(0, "PriceString").Text);
            Assert.AreEqual("TestProduct10", GetGridCell(9, "Name").Text);
            Assert.AreEqual("0 руб.", GetGridCell(9, "PriceString").Text);

            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory3')]")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("TestProduct41", GetGridCell(0, "Name").Text);
            Assert.AreEqual("0 руб.", GetGridCell(0, "PriceString").Text);
            Assert.AreEqual("TestProduct50", GetGridCell(9, "Name").Text);
            Assert.AreEqual("0 руб.", GetGridCell(9, "PriceString").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);

            Refresh();

            Assert.AreEqual("TestProduct51", GetGridCell(0, "Name").Text);
            Assert.AreEqual("50 500  руб.", GetGridCell(0, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("TestProduct60", GetGridCell(9, "Name").Text);
            Assert.AreEqual("59 500  руб.", GetGridCell(9, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));


        }

        [Test]
        public void DecreasePriceZeroSubCategoryNumber()
        {
            Functions.PriceRegulation(driver, baseURL, Select: "Уменьшить", SelectOption: "абсолютное число");
            driver.FindElement(By.CssSelector("[data-e2e=\"PriceRegulationCatCheckbox\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElements(By.CssSelector(".jstree-icon.jstree-ocl"))[3].Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory5')]")).Click();
            Thread.Sleep(2000);
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
            Assert.AreEqual("0 руб.", GetGridCell(0, "PriceString").Text);
            Assert.AreEqual("TestProduct10", GetGridCell(9, "Name").Text);
            Assert.AreEqual("0 руб.", GetGridCell(9, "PriceString").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridFilterDropdownButton\"]"));

            /*изменение только в подкатегории?
            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory4')]")).Click();
            Thread.Sleep(2000);

            ScrollTo(By.CssSelector("[data-e2e=\"gridFilterDropdownButton\"]"));
            
            Assert.AreEqual("TestProduct61", GetGridCell(0, "Name").Text);
            Thread.Sleep(1000);
            Assert.AreEqual("0 руб.", GetGridCell(0, "PriceString").Text);
            Thread.Sleep(1000);
            Assert.AreEqual("TestProduct70", GetGridCell(9, "Name").Text);
            Thread.Sleep(1000);
            Assert.AreEqual("0 руб.", GetGridCell(0, "PriceString").Text);

            Thread.Sleep(2000);*/
            driver.FindElements(By.CssSelector(".jstree-icon.jstree-ocl"))[3].Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory5')]")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("TestProduct81", GetGridCell(0, "Name").Text);
            Assert.AreEqual("80 500  руб.", GetGridCell(0, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("TestProduct90", GetGridCell(9, "Name").Text);
            Assert.AreEqual("89 500  руб.", GetGridCell(9, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));
             
            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);

            Refresh();

            Assert.AreEqual("TestProduct91", GetGridCell(0, "Name").Text);
            Assert.AreEqual("90 500  руб.", GetGridCell(0, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("TestProduct100", GetGridCell(9, "Name").Text);
            Assert.AreEqual("99 500  руб.", GetGridCell(9, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));

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
        public void DecreasePriceZeroAllPercent()
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
            Thread.Sleep(2000);
            Assert.AreEqual("TestProduct1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("0 руб.", GetGridCell(0, "PriceString").Text);
            Assert.AreEqual("TestProduct10", GetGridCell(9, "Name").Text);
            Assert.AreEqual("0 руб.", GetGridCell(9, "PriceString").Text);

            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory3')]")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("TestProduct41", GetGridCell(0, "Name").Text);
            Assert.AreEqual("0 руб.", GetGridCell(0, "PriceString").Text);
            Assert.AreEqual("TestProduct50", GetGridCell(9, "Name").Text);
            Assert.AreEqual("0 руб.", GetGridCell(9, "PriceString").Text);

            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory4')]")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("TestProduct61", GetGridCell(0, "Name").Text);
            Assert.AreEqual("54 900  руб.", GetGridCell(0, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("TestProduct70", GetGridCell(9, "Name").Text);
            Assert.AreEqual("63 000  руб.", GetGridCell(9, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));


        }

        [Test]
        public void DecreasePriceZeroCategoryPercent()
        {
            Functions.PriceRegulation(driver, baseURL, Select: "Уменьшить", SelectOption: "%");
            driver.FindElement(By.CssSelector("[data-e2e=\"PriceRegulationCatCheckbox\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory3')]")).Click();
            Thread.Sleep(2000);
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
            Thread.Sleep(2000);
            Assert.AreEqual("TestProduct41", GetGridCell(0, "Name").Text);
            Assert.AreEqual("0 руб.", GetGridCell(0, "PriceString").Text);
            Assert.AreEqual("TestProduct50", GetGridCell(9, "Name").Text);
            Assert.AreEqual("0 руб.", GetGridCell(9, "PriceString").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);

            Refresh();

            Assert.AreEqual("TestProduct51", GetGridCell(0, "Name").Text);
            Assert.AreEqual("48 450  руб.", GetGridCell(0, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("TestProduct60", GetGridCell(9, "Name").Text);
            Assert.AreEqual("57 000  руб.", GetGridCell(9, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));


        }

        [Test]
        public void DecreasePriceZeroSubCategoryPercent()
        {
            Functions.PriceRegulation(driver, baseURL, Select: "Уменьшить", SelectOption: "%");
            driver.FindElement(By.CssSelector("[data-e2e=\"PriceRegulationCatCheckbox\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElements(By.CssSelector(".jstree-icon.jstree-ocl"))[3].Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory5')]")).Click();
            Thread.Sleep(2000);
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"PriceRegulationSelectOption\"]")))).SelectByText("%");
            driver.FindElement(By.CssSelector("[data-e2e=\"PriceRegulationValue\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"PriceRegulationValue\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"PriceRegulationValue\"]")).SendKeys("7");
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
            Thread.Sleep(1000);
            Assert.AreEqual("0 руб.", GetGridCell(0, "PriceString").Text);
            Thread.Sleep(1000);
            Assert.AreEqual("TestProduct70", GetGridCell(9, "Name").Text);
            Thread.Sleep(1000);
            Assert.AreEqual("0 руб.", GetGridCell(0, "PriceString").Text);

            Thread.Sleep(2000); */
            driver.FindElements(By.CssSelector(".jstree-icon.jstree-ocl"))[3].Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory5')]")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("TestProduct81", GetGridCell(0, "Name").Text);
            Assert.AreEqual("75 330  руб.", GetGridCell(0, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("TestProduct90", GetGridCell(9, "Name").Text);
            Assert.AreEqual("83 700  руб.", GetGridCell(9, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);

            Refresh();

            Assert.AreEqual("TestProduct91", GetGridCell(0, "Name").Text);
            Assert.AreEqual("84 630  руб.", GetGridCell(0, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("TestProduct100", GetGridCell(9, "Name").Text);
            Assert.AreEqual("93 000  руб.", GetGridCell(9, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("TestProduct101", GetGridCell(0, "Name").Text);
            Assert.AreEqual("0  руб.", GetGridCell(0, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));
            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            MouseFocus(driver, By.TagName("ui-grid-custom-footer"));
            Thread.Sleep(2000);
            Assert.AreEqual("TestProduct110", GetGridCell(9, "Name").Text);
            Assert.AreEqual("0  руб.", GetGridCell(9, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));


        }
    }
}

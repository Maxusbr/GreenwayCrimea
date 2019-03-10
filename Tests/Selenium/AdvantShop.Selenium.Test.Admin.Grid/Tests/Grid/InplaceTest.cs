using System;
using NUnit.Framework;
using System.Threading;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.SeleniumTest.Admin.GridProducts
{
    [TestFixture]
    public class GridProductsInplaceTest : BaseSeleniumTest
    {
         
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog);
            InitializeService.LoadData(
                "Data\\Admin\\Grid\\InplaceGrid\\Catalog.Category.csv",
                "Data\\Admin\\Grid\\InplaceGrid\\Catalog.Product.csv",
                "Data\\Admin\\Grid\\InplaceGrid\\Catalog.ProductCategories.csv",
                "Data\\Admin\\Grid\\InplaceGrid\\Catalog.Size.csv",
                "Data\\Admin\\Grid\\InplaceGrid\\Catalog.Offer.csv"
            );
            
            Init();
        }
        
        [Test]
        public void InplaceCorrectDataTest()
        {
            GoToAdmin("catalog?categoryId=2");

            //check price
            Assert.AreEqual("TestProduct26", GetGridCell(0, "Name").FindElement(By.TagName("a")).Text);
            GetGridCell(0, "PriceString").FindElement(By.TagName("input")).Click();
            GetGridCell(0, "PriceString").FindElement(By.TagName("input")).Clear();
            GetGridCell(0, "PriceString").FindElement(By.TagName("input")).SendKeys("123");
            DropFocus("h2");
            //check amount
            GetGridCell(0, "Amount").FindElement(By.TagName("input")).Click();
            GetGridCell(0, "Amount").FindElement(By.TagName("input")).Clear();
            GetGridCell(0, "Amount").FindElement(By.TagName("input")).SendKeys("123");
            DropFocus("h2");
            //check sort
            GetGridCell(0, "SortOrder").FindElement(By.TagName("input")).Click();
            GetGridCell(0, "SortOrder").FindElement(By.TagName("input")).Clear();
            GetGridCell(0, "SortOrder").FindElement(By.TagName("input")).SendKeys("123");
            DropFocus("h2");
            //check do active
            GetGridCell(0, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffLabel\"]")).Click();
            Thread.Sleep(1000);
            Refresh();

            Assert.AreEqual("TestProduct26", GetGridCell(9, "Name").FindElement(By.TagName("a")).Text);
            Assert.IsTrue(GetGridCell(9, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);
            Assert.AreEqual("123  руб.", GetGridCell(9, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("123", GetGridCell(9, "Amount").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("123", GetGridCell(9, "SortOrder").FindElement(By.TagName("input")).GetAttribute("value"));

            //check do not active
            Assert.AreEqual("TestProduct31", GetGridCell(4, "Name").FindElement(By.TagName("a")).Text);
            Assert.IsTrue(GetGridCell(4, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);
            GetGridCell(4, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffLabel\"]")).Click();

            Refresh();

            Assert.AreEqual("TestProduct31", GetGridCell(4, "Name").FindElement(By.TagName("a")).Text);
            Assert.IsFalse(GetGridCell(4, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);
        }

        [Test]
        public void InplaceEditIncorrectDataLongTest()
        {
            GoToAdmin("catalog?categoryId=3");

            Assert.AreEqual("TestProduct37", GetGridCell(1, "Name").Text);

            //check no edit price
            Assert.IsTrue(GetGridCell(0, "PriceString").FindElements(By.TagName("input")).Count==0);
            Assert.IsTrue(GetGridCell(1, "PriceString").FindElements(By.TagName("input")).Count == 1);
            Assert.IsTrue(GetGridCell(2, "PriceString").FindElements(By.TagName("input")).Count == 1);

            //check no edit amount
            Assert.IsTrue(GetGridCell(0, "Amount").FindElements(By.TagName("input")).Count == 0);
            Assert.IsTrue(GetGridCell(1, "Amount").FindElements(By.TagName("input")).Count == 1);
            Assert.IsTrue(GetGridCell(2, "Amount").FindElements(By.TagName("input")).Count == 1);

            //check long price
            GetGridCell(1, "PriceString").FindElement(By.TagName("input")).Click();
            GetGridCell(1, "PriceString").FindElement(By.TagName("input")).Clear();
            GetGridCell(1, "PriceString").FindElement(By.TagName("input")).SendKeys("10000000000");
                     
            //check long amount
            GetGridCell(1, "Amount").FindElement(By.TagName("input")).Click();
            GetGridCell(1, "Amount").FindElement(By.TagName("input")).Clear();
            GetGridCell(1, "Amount").FindElement(By.TagName("input")).SendKeys("1000000000000");

            //check long sort
            GetGridCell(1, "SortOrder").FindElement(By.TagName("input")).Click();
            GetGridCell(1, "SortOrder").FindElement(By.TagName("input")).Clear();
            GetGridCell(1, "SortOrder").FindElement(By.TagName("input")).SendKeys("1000000000");
            DropFocus("h2");

            GoToAdmin("catalog?categoryId=3");
            Assert.AreEqual("TestProduct37", GetGridCell(2, "Name").Text);
            Assert.AreEqual("10000 000 000  руб.", GetGridCell(2, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("1000000000000", GetGridCell(2, "Amount").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("1000000000", GetGridCell(2, "SortOrder").FindElement(By.TagName("input")).GetAttribute("value"));

        }

        [Test]
        public void InplaceEditIncorrectDataInvalidTest()
        {
            GoToAdmin("catalog?categoryId=5");

            //check invalid price
            GetGridCell(0, "PriceString").FindElement(By.TagName("input")).Click();
            GetGridCell(0, "PriceString").FindElement(By.TagName("input")).Clear();
            GetGridCell(0, "PriceString").FindElement(By.TagName("input")).SendKeys("hgvjhlhlhk");

            //check invalid amount
            GetGridCell(0, "Amount").FindElement(By.TagName("input")).Click();
            GetGridCell(0, "Amount").FindElement(By.TagName("input")).Clear();
            GetGridCell(0, "Amount").FindElement(By.TagName("input")).SendKeys("hgvjhlhlhk");

            //check invalid sort
            GetGridCell(0, "SortOrder").FindElement(By.TagName("input")).Click();
            GetGridCell(0, "SortOrder").FindElement(By.TagName("input")).Clear();
            GetGridCell(0, "SortOrder").FindElement(By.TagName("input")).SendKeys("hgvjhlhlhk");
            DropFocus("h2");

            GoToAdmin("catalog?categoryId=5");

            Assert.AreEqual("39  руб.", GetGridCell(0, "PriceString").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("39", GetGridCell(0, "Amount").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("39", GetGridCell(0, "SortOrder").FindElement(By.TagName("input")).GetAttribute("value"));

        }
    }
}


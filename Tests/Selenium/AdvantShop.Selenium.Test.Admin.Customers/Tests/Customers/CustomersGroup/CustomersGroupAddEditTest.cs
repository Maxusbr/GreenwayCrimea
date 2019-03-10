using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.SeleniumTest.Admin.CustomersGroup
{
    [TestFixture]
    public class CustomersGroupAddEditTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog | ClearType.Customers);
            InitializeService.LoadData(
           "data\\Admin\\Customers\\CustomersGroup\\Catalog.Product.csv",
           "data\\Admin\\Customers\\CustomersGroup\\Catalog.Offer.csv",
           "data\\Admin\\Customers\\CustomersGroup\\Catalog.Category.csv",
           "data\\Admin\\Customers\\CustomersGroup\\Catalog.ProductCategories.csv",
           "data\\Admin\\Customers\\CustomersGroup\\Customers.Customer.csv",
           "data\\Admin\\Customers\\CustomersGroup\\Customers.CustomerGroup.csv",
           "data\\Admin\\Customers\\CustomersGroup\\Customers.Departments.csv",
           "data\\Admin\\Customers\\CustomersGroup\\Customers.Managers.csv",
            "data\\Admin\\Customers\\CustomersGroup\\Customers.ManagerTask.csv"
           );
             
            Init();
        }
        

        [Test]
        public void CustomersGroupAdd()
        {
            GoToAdmin("customergroups");
            driver.FindElements(By.CssSelector(".btn.btn-sm.btn-success.btn--margin-left"))[0].Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"CustomerGroupName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"CustomerGroupName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"CustomerGroupName\"]")).SendKeys("CustomersGroupNew");
            driver.FindElement(By.CssSelector("[data-e2e=\"CustomerGroupDiscount\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"CustomerGroupDiscount\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"CustomerGroupDiscount\"]")).SendKeys("50");
            driver.FindElement(By.CssSelector("[data-e2e=\"CustomerGroupMinOrderPrice\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"CustomerGroupMinOrderPrice\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"CustomerGroupMinOrderPrice\"]")).SendKeys("100");
            driver.FindElement(By.CssSelector("[data-e2e=\"CustomerGroupOK\"]")).Click();
            Thread.Sleep(2000);

            GoToAdmin("customergroups");
            GetGridFilter().Click();
            GetGridFilter().SendKeys("CustomersGroupNew");
            DropFocus("h1");
            Thread.Sleep(2000);

            Assert.AreEqual("CustomersGroupNew", GetGridCell(0, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("50", GetGridCell(0, "GroupDiscount").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("100", GetGridCell(0, "MinimumOrderPrice").FindElement(By.TagName("input")).GetAttribute("value"));
        }

        [Test]
        public void CustomersGroupEditInplace()
        {
            GoToAdmin("customergroups");
            GetGridFilter().Click();
            GetGridFilter().SendKeys("CustomerGroup100");
            DropFocus("h1");
            Thread.Sleep(2000);

            Assert.AreEqual("CustomerGroup100", GetGridCell(0, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"));

            //check input name
            GetGridCell(0, "GroupName").FindElement(By.TagName("input")).Click();
            GetGridCell(0, "GroupName").FindElement(By.TagName("input")).Clear();
            GetGridCell(0, "GroupName").FindElement(By.TagName("input")).SendKeys("CustomerGroup100ChangedName");
            DropFocus("h1");

            //check input discount
            GetGridCell(0, "GroupDiscount").FindElement(By.TagName("input")).Click();
            GetGridCell(0, "GroupDiscount").FindElement(By.TagName("input")).Clear();
            GetGridCell(0, "GroupDiscount").FindElement(By.TagName("input")).SendKeys("7");
            DropFocus("h1");

            //check input minimum order price
            GetGridCell(0, "MinimumOrderPrice").FindElement(By.TagName("input")).Click();
            GetGridCell(0, "MinimumOrderPrice").FindElement(By.TagName("input")).Clear();
            GetGridCell(0, "MinimumOrderPrice").FindElement(By.TagName("input")).SendKeys("5000");
            DropFocus("h1");

            GoToAdmin("customergroups");
            GetGridFilter().Click();
            GetGridFilter().SendKeys("CustomerGroup100ChangedName");
            DropFocus("h1");
            Thread.Sleep(2000);

            Assert.AreEqual("CustomerGroup100ChangedName", GetGridCell(0, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("7", GetGridCell(0, "GroupDiscount").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("5000", GetGridCell(0, "MinimumOrderPrice").FindElement(By.TagName("input")).GetAttribute("value"));
        }
    }
}
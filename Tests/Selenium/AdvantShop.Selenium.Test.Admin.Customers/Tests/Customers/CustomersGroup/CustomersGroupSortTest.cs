using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.SeleniumTest.Admin.CustomersGroup
{
    [TestFixture]
    public class CustomersGroupSortTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog);
            InitializeService.ClearData(ClearType.Customers);
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
        public void CustomerGroupSort10()
        {
            GoToAdmin("customergroups");
            Functions.GridPaginationSelect10(driver, baseURL);

            //check sort by name
            GetGridCell(-1, "GroupName").Click();
            Thread.Sleep(2000);
            Assert.AreEqual("CustomerGroup1", GetGridCell(0, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("CustomerGroup107", GetGridCell(9, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"));

            GetGridCell(-1, "GroupName").Click();
            Thread.Sleep(2000);
            Assert.AreEqual("CustomerGroup99", GetGridCell(0, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("CustomerGroup90", GetGridCell(9, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"));

            //check sort by group discount
            GetGridCell(-1, "GroupDiscount").Click();
            Thread.Sleep(2000);
            Assert.AreEqual("CustomerGroup101", GetGridCell(0, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("CustomerGroup110", GetGridCell(9, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("3", GetGridCell(0, "GroupDiscount").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("3", GetGridCell(9, "GroupDiscount").FindElement(By.TagName("input")).GetAttribute("value"));

            GetGridCell(-1, "GroupDiscount").Click();
            Thread.Sleep(2000);
            Assert.AreEqual("CustomerGroup121", GetGridCell(0, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("CustomerGroup130", GetGridCell(9, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("50", GetGridCell(0, "GroupDiscount").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("50", GetGridCell(9, "GroupDiscount").FindElement(By.TagName("input")).GetAttribute("value"));

            //check sort by minimum order price 
            GetGridCell(-1, "MinimumOrderPrice").Click();
            Thread.Sleep(2000);
            Assert.AreEqual("CustomerGroup21", GetGridCell(0, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("CustomerGroup30", GetGridCell(9, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("10", GetGridCell(0, "MinimumOrderPrice").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("10", GetGridCell(9, "MinimumOrderPrice").FindElement(By.TagName("input")).GetAttribute("value"));

            GetGridCell(-1, "MinimumOrderPrice").Click();
            Thread.Sleep(2000);
            Assert.AreEqual("CustomerGroup101", GetGridCell(0, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("CustomerGroup110", GetGridCell(9, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("300", GetGridCell(0, "MinimumOrderPrice").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("300", GetGridCell(9, "MinimumOrderPrice").FindElement(By.TagName("input")).GetAttribute("value"));
        }

        [Test]
        public void CustomerGroupSort20()
        {
            GoToAdmin("customergroups");
            Functions.GridPaginationSelect20(driver, baseURL);
            ScrollTo(By.Id("header-top"));

            //check sort by name
            GetGridCell(-1, "GroupName").Click();
            Thread.Sleep(2000);
            Assert.AreEqual("CustomerGroup1", GetGridCell(0, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("CustomerGroup116", GetGridCell(19, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"));
            
            GetGridCell(-1, "GroupName").Click();
            Thread.Sleep(2000);
            Assert.AreEqual("CustomerGroup99", GetGridCell(0, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("CustomerGroup81", GetGridCell(19, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"));

            //check sort by group discount
            GetGridCell(-1, "GroupDiscount").Click();
            Thread.Sleep(2000);
            Assert.AreEqual("CustomerGroup101", GetGridCell(0, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("3", GetGridCell(0, "GroupDiscount").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("CustomerGroup120", GetGridCell(19, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("3", GetGridCell(19, "GroupDiscount").FindElement(By.TagName("input")).GetAttribute("value"));
            
            GetGridCell(-1, "GroupDiscount").Click();
            Assert.AreEqual("CustomerGroup121", GetGridCell(0, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("50", GetGridCell(0, "GroupDiscount").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("CustomerGroup140", GetGridCell(19, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("50", GetGridCell(19, "GroupDiscount").FindElement(By.TagName("input")).GetAttribute("value"));

            //check sort by minimum prder price
            GetGridCell(-1, "MinimumOrderPrice").Click();
            Thread.Sleep(2000);
            Assert.AreEqual("CustomerGroup21", GetGridCell(0, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("10", GetGridCell(0, "MinimumOrderPrice").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("CustomerGroup40", GetGridCell(19, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("10", GetGridCell(19, "MinimumOrderPrice").FindElement(By.TagName("input")).GetAttribute("value"));
            
            GetGridCell(-1, "MinimumOrderPrice").Click();
            Thread.Sleep(2000);
            Assert.AreEqual("CustomerGroup101", GetGridCell(0, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("300", GetGridCell(0, "MinimumOrderPrice").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("CustomerGroup120", GetGridCell(19, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("300", GetGridCell(19, "MinimumOrderPrice").FindElement(By.TagName("input")).GetAttribute("value"));
        }

        [Test]
        public void CustomerGroupSort50()
        {
            GoToAdmin("customergroups");
            Functions.GridPaginationSelect50(driver, baseURL);
            ScrollTo(By.Id("header-top"));

            //check sort by name
            GetGridCell(-1, "GroupName").Click();
            Thread.Sleep(2000);
            Assert.AreEqual("CustomerGroup143", GetGridCell(49, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"));
            
            GetGridCell(-1, "GroupName").Click();
            Thread.Sleep(2000);
            Assert.AreEqual("CustomerGroup99", GetGridCell(0, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("CustomerGroup54", GetGridCell(49, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"));

            //check sort by group discount
            GetGridCell(-1, "GroupDiscount").Click();
            Thread.Sleep(2000);
            Assert.AreEqual("CustomerGroup101", GetGridCell(0, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("3", GetGridCell(0, "GroupDiscount").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("CustomerGroup20", GetGridCell(49, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("10", GetGridCell(49, "GroupDiscount").FindElement(By.TagName("input")).GetAttribute("value"));
            
            GetGridCell(-1, "GroupDiscount").Click();
            Thread.Sleep(2000);
            Assert.AreEqual("CustomerGroup121", GetGridCell(0, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("50", GetGridCell(0, "GroupDiscount").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("CustomerGroup170", GetGridCell(49, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("50", GetGridCell(49, "GroupDiscount").FindElement(By.TagName("input")).GetAttribute("value"));

            //check sort by minimum order price
            GetGridCell(-1, "MinimumOrderPrice").Click();
            Thread.Sleep(2000);
            Assert.AreEqual("CustomerGroup21", GetGridCell(0, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("10", GetGridCell(0, "MinimumOrderPrice").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("CustomerGroup70", GetGridCell(49, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("50", GetGridCell(49, "MinimumOrderPrice").FindElement(By.TagName("input")).GetAttribute("value"));
            
            GetGridCell(-1, "MinimumOrderPrice").Click();
            Thread.Sleep(2000);
            Assert.AreEqual("CustomerGroup101", GetGridCell(0, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("300", GetGridCell(0, "MinimumOrderPrice").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("CustomerGroup150", GetGridCell(49, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("300", GetGridCell(49, "MinimumOrderPrice").FindElement(By.TagName("input")).GetAttribute("value"));
        }

        [Test]
        public void CustomerGroupSort100()
        {
            GoToAdmin("customergroups");
            Functions.GridPaginationSelect100(driver, baseURL);
            ScrollTo(By.Id("header-top"));

            //check sort by name
            GetGridCell(-1, "GroupName").Click();
            Thread.Sleep(2000);
            Assert.AreEqual("CustomerGroup1", GetGridCell(0, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("CustomerGroup189", GetGridCell(99, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"));
            
            GetGridCell(-1, "GroupName").Click();
            Thread.Sleep(2000);
            Assert.AreEqual("CustomerGroup99", GetGridCell(0, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("CustomerGroup19", GetGridCell(99, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"));

            //check sort by group discount
            GetGridCell(-1, "GroupDiscount").Click();
            Thread.Sleep(2000);
            Assert.AreEqual("CustomerGroup101", GetGridCell(0, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("3", GetGridCell(0, "GroupDiscount").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("CustomerGroup70", GetGridCell(99, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("25", GetGridCell(99, "GroupDiscount").FindElement(By.TagName("input")).GetAttribute("value"));
            
            GetGridCell(-1, "GroupDiscount").Click();
            Thread.Sleep(2000);
            Assert.AreEqual("CustomerGroup121", GetGridCell(0, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("50", GetGridCell(0, "GroupDiscount").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("CustomerGroup80", GetGridCell(99, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("25", GetGridCell(99, "GroupDiscount").FindElement(By.TagName("input")).GetAttribute("value"));

            //check sort by minimum order price
            GetGridCell(-1, "MinimumOrderPrice").Click();
            Thread.Sleep(2000);
            Assert.AreEqual("CustomerGroup21", GetGridCell(0, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("10", GetGridCell(0, "MinimumOrderPrice").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("CustomerGroup20", GetGridCell(99, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("100", GetGridCell(99, "MinimumOrderPrice").FindElement(By.TagName("input")).GetAttribute("value"));
            
            GetGridCell(-1, "MinimumOrderPrice").Click();
            Thread.Sleep(2000);
            Assert.AreEqual("CustomerGroup101", GetGridCell(0, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("300", GetGridCell(0, "MinimumOrderPrice").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("CustomerGroup200", GetGridCell(99, "GroupName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("190", GetGridCell(99, "MinimumOrderPrice").FindElement(By.TagName("input")).GetAttribute("value"));
        }
    }
}
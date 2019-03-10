using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.SeleniumTest.Admin.GridProducts
{
    [TestFixture]
    public class GridAllProductsSelectedInCategoryTest : BaseSeleniumTest
    {
        
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog);
            InitializeService.LoadData(
           "data\\Admin\\Grid\\AllProdSelectInCatTest\\Catalog.Product.csv",
           "data\\Admin\\Grid\\AllProdSelectInCatTest\\Catalog.Offer.csv",
           "data\\Admin\\Grid\\AllProdSelectInCatTest\\Catalog.Category.csv",
           "data\\Admin\\Grid\\AllProdSelectInCatTest\\Catalog.ProductCategories.csv");
             
            Init();
        }
        
        // все товары в категории TestCategory1 активны
        [Test]
        public void ProductsAllInCategorySelectedNotActive()
        {
            GoToAdmin("catalog?categoryid=1");

            //выбрать все товары на странице
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
           
            //выбрать все товары категории
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownButton\"]")).Click();
           
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownItem\"][data-e2e-grid-selection-dropdown-index=\"5\"]")).Click();
           
           Refresh();
           
            Assert.IsFalse(GetGridCell(0, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);
            Assert.IsFalse(GetGridCell(9, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);
        }

        // все товары в категории TestCategory2 неактивны
        [Test]
        public void ProductsAllInCategorySelectedActive()
        {
            GoToAdmin("catalog?categoryid=2");

            //выбрать все товары на странице
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();

            //выбрать все товары категории
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();

            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownButton\"]")).Click();
          
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownItem\"][data-e2e-grid-selection-dropdown-index=\"4\"]")).Click();
            
            Assert.IsTrue(GetGridCell(0, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);
            Assert.IsTrue(GetGridCell(9, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);
        }
    }
}
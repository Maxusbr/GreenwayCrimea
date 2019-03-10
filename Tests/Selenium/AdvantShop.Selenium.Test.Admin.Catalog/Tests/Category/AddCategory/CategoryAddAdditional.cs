using System;
using NUnit.Framework;
using System.Threading;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;
using AdvantShop.Selenium.Core.Infrastructure;
//using AdvantShop.Selenium.Core.Infrastructure;

namespace AdvantShop.SeleniumTest.Admin.MainPage.Category.AddCategory
{
    [TestFixture]
    public class CategoryAddAdditional : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog);
            InitializeService.LoadData(
                "Data\\Admin\\Catalog\\AddCategory\\Catalog.Product.csv",
                "Data\\Admin\\Catalog\\AddCategory\\Catalog.Offer.csv",
                "Data\\Admin\\Catalog\\AddCategory\\Catalog.Category.csv",
                "data\\Admin\\Catalog\\AddCategory\\Catalog.Brand.csv",
                "Data\\Admin\\Catalog\\AddCategory\\Catalog.ProductCategories.csv");

             
            Init();

        }

        [Test]
        public void DisplayStyleListNewCategory()
        {
           GoToAdmin("catalog");
            driver.FindElement(By.CssSelector("[data-e2e=\"AddCategory\"] span")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.Id("Name")).SendKeys("New_Category_List");
            DropFocus("h1");
            (new SelectElement(driver.FindElement(By.Id("DisplayStyle")))).SelectByText("Список");
            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            GoToAdmin("catalog");
            driver.FindElement(By.CssSelector("[data-e2e=\"AddCategory\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.Id("Name")).SendKeys("New_Category_List_Child");
            driver.FindElement(By.XPath("//a[contains(text(), 'Изменить')]")).Click();
            Thread.Sleep(2000);
            WaitForElem(By.CssSelector(".modal-content"));

            driver.FindElement(By.XPath("//span[contains(text(), 'New_Category_List')]")).Click();
            Thread.Sleep(1000);
            WaitForElemEnabled(By.XPath("//button[contains(text(), 'Изменить')]"));
            driver.FindElement(By.XPath("//button[contains(text(), 'Изменить')]")).Click();
            Thread.Sleep(1000);

            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            GoToClient("categories/new_category_list");
            Assert.IsTrue(driver.FindElements(By.CssSelector(".product-categories-item-thin")).Count > 0);           
        }
        [Test]
        public void DisplayStyleTileNewCategory()
        {
           GoToAdmin("catalog");
            driver.FindElement(By.CssSelector("[data-e2e=\"AddCategory\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.Id("Name")).SendKeys("New_Category_Tile");
            DropFocus("h1");

            (new SelectElement(driver.FindElement(By.Id("DisplayStyle")))).SelectByText("Плитка");
            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            GoToAdmin("catalog");
            driver.FindElement(By.CssSelector("[data-e2e=\"AddCategory\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.Id("Name")).SendKeys("New_Category_Tile_Child");
            driver.FindElement(By.XPath("//a[contains(text(), 'Изменить')]")).Click();
            Thread.Sleep(1000);
            WaitForElem(By.CssSelector(".modal-content"));

            driver.FindElement(By.XPath("//span[contains(text(), 'New_Category_Tile')]")).Click();
            Thread.Sleep(1000);
            WaitForElemEnabled(By.XPath("//button[contains(text(), 'Изменить')]"));
            driver.FindElement(By.XPath("//button[contains(text(), 'Изменить')]")).Click();
            Thread.Sleep(1000);

            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            GoToClient("categories/new_category_tile");
            Assert.IsTrue(driver.FindElements(By.CssSelector(".product-categories.product-categories-slim")).Count > 0);
        }

        [Test]
        public void DisplayStyleNoPreCategoryNewCategory()
        {
           GoToAdmin("catalog");
            driver.FindElement(By.CssSelector("[data-e2e=\"AddCategory\"]")).Click();
            Thread.Sleep(2000);
            Refresh();
            driver.FindElement(By.Id("Name")).Click();
            driver.FindElement(By.Id("Name")).SendKeys("New_Category_No_PreCategory");
            DropFocus("h1");
            (new SelectElement(driver.FindElement(By.Id("DisplayStyle")))).SelectByText("Не показывать");
            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            GoToAdmin("catalog");
            driver.FindElement(By.CssSelector("[data-e2e=\"AddCategory\"]")).Click();
            Thread.Sleep(1000);
            Refresh();
            driver.FindElement(By.Id("Name")).SendKeys("New_Category_No_PreCategory_Child");
           driver.FindElement(By.XPath("//a[contains(text(), 'Изменить')]")).Click();
            Thread.Sleep(1000);
            WaitForElem(By.CssSelector(".modal-content"));

            driver.FindElement(By.XPath("//span[contains(text(), 'New_Category_No_PreCategory')]")).Click();
            Thread.Sleep(1000);
            WaitForElemEnabled(By.XPath("//button[contains(text(), 'Изменить')]"));
            driver.FindElement(By.XPath("//button[contains(text(), 'Изменить')]")).Click();
            Thread.Sleep(1000);

            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

           GoToClient("categories/new_category_no_precategory");
            Assert.IsTrue(driver.FindElements(By.CssSelector(".product-categories.product-categories-slim")).Count == 0);
            Assert.IsTrue(driver.FindElements(By.CssSelector(".product-categories-thin")).Count == 0);
          }
        
        [Test]
        public void BrandInCategoryNewCategory()
        {
           GoToAdmin("catalog");
            driver.FindElement(By.CssSelector("[data-e2e=\"AddCategory\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.Id("Name")).SendKeys("New_Category_Show_Brand");
            DropFocus("h1");
            ScrollTo(By.Id("DisplayStyle"));
            driver.FindElements(By.CssSelector(".adv-radio-label input"))[0].Click();
            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            GoToAdmin("catalog");
            driver.FindElement(By.CssSelector("[data-e2e=\"AddCategory\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.Id("Name")).SendKeys("New_Category_Show_Brand_Child");
           driver.FindElement(By.XPath("//a[contains(text(), 'Изменить')]")).Click();
            Thread.Sleep(1000);
            WaitForElem(By.CssSelector(".modal-content"));
            driver.FindElement(By.XPath("//span[contains(text(), 'New_Category_Show_Brand')]")).Click();
            Thread.Sleep(2000);
            WaitForElemEnabled(By.XPath("//button[contains(text(), 'Изменить')]"));
            driver.FindElement(By.XPath("//button[contains(text(), 'Изменить')]")).Click();
            Thread.Sleep(1000);
            ScrollTo(By.Id("DisplayStyle"));
            driver.FindElements(By.CssSelector(".adv-radio-label input"))[0].Click();
            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            GoToAdmin("catalog?categoryid=1");
            GetGridCell(0, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffLabel\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"grid[1][\'Enabled\']\"] [data-e2e=\"switchOnOffLabel\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"grid[2][\'Enabled\']\"] [data-e2e=\"switchOnOffLabel\"]")).Click();

            GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            GetGridCell(1, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            GetGridCell(2, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownButton\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownItem\"][data-e2e-grid-selection-dropdown-index=\"3\"]")).Click();
            Thread.Sleep(1000);
            WaitForElem(By.CssSelector(".modal-content"));
            driver.FindElement(By.XPath("//span[contains(text(), 'New_Category_Show_Brand')]")).Click();
            Thread.Sleep(2000);
            WaitForElemEnabled(By.XPath("//button[contains(text(), 'Добавить товары')]"));
            driver.FindElement(By.XPath("//button[contains(text(), 'Добавить товары')]")).Click();
            Thread.Sleep(1000);

         /*  GoToAdmin("catalog");
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("//span[contains(text(), 'New_Category_Show_Brand')]")).Click();
            Thread.Sleep(1000);
            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            GetGridCell(0, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffLabel\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"grid[1][\'Enabled\']\"] [data-e2e=\"switchOnOffLabel\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"grid[2][\'Enabled\']\"] [data-e2e=\"switchOnOffLabel\"]")).Click();
            Thread.Sleep(1000);
            */
            GoToClient("catalog");
            Assert.IsTrue(driver.PageSource.Contains("Производители"));
            Assert.IsTrue(driver.PageSource.Contains("BrandName1"));
        }

        [Test]
        public void BrandInCategoryAddCategoryNo()
        {
           GoToAdmin("catalog");
            driver.FindElement(By.CssSelector("[data-e2e=\"AddCategory\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.Id("Name")).SendKeys("New_Category_Hide_Brand");
            DropFocus("h1");
            ScrollTo(By.Id("DisplayStyle"));
            driver.FindElements(By.CssSelector(".adv-radio-label input"))[1].Click();
            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            GoToAdmin("catalog");
            driver.FindElement(By.CssSelector("[data-e2e=\"AddCategory\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.Id("Name")).SendKeys("New_Category_Hide_Brand_Child");
            driver.FindElement(By.XPath("//a[contains(text(), 'Изменить')]")).Click();
            Thread.Sleep(1000);
            WaitForElem(By.CssSelector(".modal-content"));
            driver.FindElement(By.XPath("//span[contains(text(), 'New_Category_Hide_Brand')]")).Click();
            Thread.Sleep(1000);
            WaitForElemEnabled(By.XPath("//button[contains(text(), 'Изменить')]"));
            driver.FindElement(By.XPath("//button[contains(text(), 'Изменить')]")).Click();
            Thread.Sleep(1000);

            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            GoToAdmin("catalog?categoryid=1");
            GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            GetGridCell(1, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            GetGridCell(2, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownButton\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownItem\"][data-e2e-grid-selection-dropdown-index=\"3\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("//span[contains(text(), 'New_Category_Hide_Brand')]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("//button[contains(text(), 'Добавить товары')]")).Click();
            Thread.Sleep(1000);

            //driver.Navigate().GoToUrl(baseURL + "/adminv2/catalog");
            //Thread.Sleep(1000);
            //driver.FindElement(By.XPath("//span[contains(text(), 'New_Category_Hide_Brand')]")).Click();
            //Thread.Sleep(1000);

            //var element = driver.FindElement(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            //IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
            //jse.ExecuteScript("arguments[0].scrollIntoView(true)", element);

            //Thread.Sleep(1000);

            //GetGridCell(0, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffLabel\"]")).Click();
            //Thread.Sleep(1000);
            //driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"grid[1][\'Enabled\']\"] [data-e2e=\"switchOnOffLabel\"]")).Click();
            //Thread.Sleep(1000);
            //driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"grid[2][\'Enabled\']\"] [data-e2e=\"switchOnOffLabel\"]")).Click();
            //Thread.Sleep(1000);

           GoToClient("catalog");
            //Assert.IsTrue(driver.PageSource.Contains("Производители"));
            Assert.IsFalse(driver.PageSource.Contains("BrandName1"));
        }

        [Test]
        public void HiddenCategoryInmenuNewCategory()
        {
           GoToAdmin("catalog");
            driver.FindElement(By.CssSelector("[data-e2e=\"AddCategory\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.Id("Name")).SendKeys("New_Category_Hidden_InMenu");
            DropFocus("h1");

           ScrollTo(By.Id("DisplayBrandsInMenu"));
           
            driver.FindElements(By.CssSelector(".adv-radio-label input"))[4].Click();
           
            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            GoToClient("catalog");
            Assert.IsFalse(driver.PageSource.Contains("New_Category_Hidden_InMenu"));
        }

        [Test]
        public void HiddenCategoryInmenuNoNewCategory()
        {
           GoToAdmin("catalog");
            driver.FindElement(By.CssSelector("[data-e2e=\"AddCategory\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.Id("Name")).SendKeys("New_Category_Hidden_InMenu_No");
            DropFocus("h1");

            ScrollTo(By.Id("DisplayBrandsInMenu"));
           
            driver.FindElements(By.CssSelector(".adv-radio-label input"))[5].Click();
           
            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            GoToClient("catalog");
            Assert.IsTrue(driver.PageSource.Contains("New_Category_Hidden_InMenu_No"));
        }

        [Test]
        public void ATwoLevelInCategoryNewCategory()
        {
           GoToAdmin("catalog");
            driver.FindElement(By.CssSelector("[data-e2e=\"AddCategory\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.Id("Name")).SendKeys("New_Category_Two_Levels_InMenu");
            DropFocus("h1");
            ScrollTo(By.Id("DisplayStyle"));
            driver.FindElements(By.CssSelector(".adv-radio-label input"))[2].Click();
            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);


            GoToAdmin("catalog");
            driver.FindElement(By.CssSelector("[data-e2e=\"AddCategory\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.Id("Name")).SendKeys("New_Category_Two_Levels_InMenu_Child_1");
            DropFocus("h1");
            driver.FindElement(By.XPath("//a[contains(text(), 'Изменить')]")).Click();
            Thread.Sleep(1000);
            WaitForElem(By.CssSelector(".modal-content"));
            driver.FindElement(By.XPath("//span[contains(text(), 'New_Category_Two_Levels_InMenu')]")).Click();
            Thread.Sleep(1000);
            WaitForElemEnabled(By.XPath("//button[contains(text(), 'Изменить')]"));
            driver.FindElement(By.XPath("//button[contains(text(), 'Изменить')]")).Click();
            Thread.Sleep(1000);
            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            GoToAdmin("catalog");
            driver.FindElement(By.CssSelector("[data-e2e=\"AddCategory\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.Id("Name")).SendKeys("New_Category_Two_Levels_InMenu_Child_2");
            DropFocus("h1");
            driver.FindElement(By.XPath("//a[contains(text(), 'Изменить')]")).Click();
            Thread.Sleep(1000);
            WaitForElem(By.CssSelector(".modal-content"));

            driver.FindElement(By.XPath("//span[contains(text(), 'New_Category_Two_Levels_InMenu')]")).Click();
            Thread.Sleep(1000);
            WaitForElemEnabled(By.XPath("//button[contains(text(), 'Изменить')]"));
            driver.FindElement(By.XPath("//button[contains(text(), 'Изменить')]")).Click();
            Thread.Sleep(1000);
            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            GoToClient("catalog");

            Assert.IsTrue(driver.PageSource.Contains("New_Category_Two_Levels_InMenu_Child_1"));
            Assert.IsTrue(driver.PageSource.Contains("New_Category_Two_Levels_InMenu_Child_2"));
        }

        [Test]
        public void ATwoLevelInCategoryNewCategoryNo()
        {
           GoToAdmin("catalog");
            driver.FindElement(By.CssSelector("[data-e2e=\"AddCategory\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.Id("Name")).SendKeys("New_Category_Two_Levels_InMenu_No");
            DropFocus("h1");
            ScrollTo(By.Id("DisplayStyle"));
            driver.FindElements(By.CssSelector(".adv-radio-label input"))[3].Click();
            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            GoToAdmin("catalog");
            driver.FindElement(By.LinkText("Добавить категорию")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.Id("Name")).SendKeys("New_Category_Two_Levels_InMenu_No_Child_1");
            DropFocus("h1");
            driver.FindElement(By.XPath("//a[contains(text(), 'Изменить')]")).Click();
            Thread.Sleep(1000);
            WaitForElem(By.CssSelector(".modal-content"));
            driver.FindElement(By.XPath("//span[contains(text(), 'New_Category_Two_Levels_InMenu_No')]")).Click();
            Thread.Sleep(1000);
            WaitForElemEnabled(By.XPath("//button[contains(text(), 'Изменить')]"));
            driver.FindElement(By.XPath("//button[contains(text(), 'Изменить')]")).Click();
            Thread.Sleep(1000);
            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            GoToAdmin("catalog");
            driver.FindElement(By.CssSelector("[data-e2e=\"AddCategory\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.Id("Name")).SendKeys("New_Category_Two_Levels_InMenu_No_Child_2");
            DropFocus("h1");
            driver.FindElement(By.XPath("//a[contains(text(), 'Изменить')]")).Click();
            Thread.Sleep(1000);
            WaitForElem(By.CssSelector(".modal-content"));
            driver.FindElement(By.XPath("//span[contains(text(), 'New_Category_Two_Levels_InMenu_No')]")).Click();
            Thread.Sleep(1000);
            WaitForElemEnabled(By.XPath("//button[contains(text(), 'Изменить')]"));
            driver.FindElement(By.XPath("//button[contains(text(), 'Изменить')]")).Click();
            Thread.Sleep(1000);
            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            GoToClient("catalog");

            Assert.IsTrue(driver.PageSource.Contains("New_Category_Two_Levels_InMenu_Child_1"));
            Assert.IsTrue(driver.FindElements(By.CssSelector(".menu-dropdown-sub-columns-item"))[1].FindElements(By.CssSelector(".menu-dropdown-sub-block.menu-dropdown-sub-block-cats-only")).Count > 0);
             Assert.IsTrue(driver.FindElements(By.CssSelector(".menu-dropdown-sub-columns-item"))[0].FindElements(By.CssSelector(".menu-dropdown-sub-block.menu-dropdown-sub-block-cats-only")).Count == 0);
        }
    }
}

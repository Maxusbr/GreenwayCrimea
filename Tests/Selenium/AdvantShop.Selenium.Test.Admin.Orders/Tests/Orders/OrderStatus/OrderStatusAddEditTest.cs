using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.SeleniumTest.Admin.Orders.OrderStatus
{
    [TestFixture]
    public class OrderStatusAddEditTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Orders | ClearType.Catalog);
            InitializeService.LoadData(
             "data\\Admin\\Orders\\OrderStatusAddEdit\\Catalog.Product.csv",
           "data\\Admin\\Orders\\OrderStatusAddEdit\\Catalog.Offer.csv",
           "data\\Admin\\Orders\\OrderStatusAddEdit\\Catalog.Category.csv",
           "data\\Admin\\Orders\\OrderStatusAddEdit\\Catalog.ProductCategories.csv",
            "data\\Admin\\Orders\\OrderStatusAddEdit\\[Order].OrderContact.csv",
            "data\\Admin\\Orders\\OrderStatusAddEdit\\[Order].OrderCurrency.csv",
             "data\\Admin\\Orders\\OrderStatusAddEdit\\[Order].OrderItems.csv",
             "data\\Admin\\Orders\\OrderStatusAddEdit\\[Order].OrderSource.csv",
             "data\\Admin\\Orders\\OrderStatusAddEdit\\[Order].OrderStatus.csv",
             "data\\Admin\\Orders\\OrderStatusAddEdit\\[Order].[Order].csv"
          );

            Init();
            
        }

        [Test]
        public void OrderStatusAddDefault()
        {
            testname = "OrderStatusAddDefault";
            VerifyBegin(testname);

            GoToAdmin("orderstatuses");
            driver.FindElement(By.CssSelector("[data-e2e=\"btnAdd\"]")).Click();
            Thread.Sleep(2000);
            WaitForElem(By.CssSelector(".modal-content"));

            VerifyAreEqual("Новый статус заказа", driver.FindElement(By.TagName("h2")).Text, "pop up h2");

            driver.FindElement(By.CssSelector("[data-e2e=\"orderStatusColor\"]")).FindElement(By.TagName("input")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"orderStatusColor\"]")).FindElement(By.TagName("input")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"orderStatusColor\"]")).FindElement(By.TagName("input")).SendKeys("#80d5fa");

            driver.FindElement(By.CssSelector("[data-e2e=\"orderStatusName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"orderStatusName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"orderStatusName\"]")).SendKeys("New Order Status Test");

            driver.FindElement(By.CssSelector("[data-e2e=\"orderStatusIsDefault\"]")).Click();

            driver.FindElement(By.CssSelector("[data-e2e=\"orderStatusIsCompleted\"]")).Click();

            driver.FindElement(By.CssSelector("[data-e2e=\"orderStatusSortOrder\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"orderStatusSortOrder\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"orderStatusSortOrder\"]")).SendKeys("1");

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"orderStatusCommand\"]")))).SelectByText("Списание товара со склада");
            
            driver.FindElement(By.CssSelector("[data-e2e=\"orderStatusSave\"]")).Click();
            Thread.Sleep(2000);

            GoToAdmin("orderstatuses");

            VerifyAreEqual("Найдено записей: 126", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "grid count all");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("New Order Status Test");

            VerifyAreEqual("color: rgb(128, 213, 250);", GetGridCell(0, "Color").FindElement(By.TagName("i")).GetAttribute("style"), "grid color");
            VerifyAreEqual("New Order Status Test", GetGridCell(0, "StatusName").FindElement(By.TagName("a")).Text, "grid name");
            VerifyIsTrue(GetGridCell(0, "IsDefault").FindElement(By.CssSelector("[data-e2e=\"switchOnOffSelect\"]")).Selected, "grid default");
            VerifyIsFalse(GetGridCell(0, "IsCanceled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffSelect\"]")).Selected, "grid IsCanceled");
            VerifyIsTrue(GetGridCell(0, "IsCompleted").FindElement(By.CssSelector("[data-e2e=\"switchOnOffSelect\"]")).Selected, "grid IsCompleted");
            VerifyAreEqual("1", GetGridCell(0, "SortOrder").Text, "grid sort order");
            VerifyAreEqual("Списание товара со склада", GetGridCell(0, "CommandFormatted").Text, "grid status command");
           
            GetGridCell(0, "StatusName").FindElement(By.TagName("a")).Click();
            Thread.Sleep(3000);
            WaitForElem(By.CssSelector(".modal-content"));

            VerifyAreEqual("New Order Status Test", driver.FindElement(By.CssSelector("[data-e2e=\"orderStatusName\"]")).GetAttribute("value"), "pop up name");
            VerifyAreEqual("1", driver.FindElement(By.CssSelector("[data-e2e=\"orderStatusSortOrder\"]")).GetAttribute("value"), "pop up sort order");

            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"orderStatusIsDefault\"]")).FindElement(By.TagName("input")).Selected, "pop up default");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"orderStatusIsCompleted\"]")).FindElement(By.TagName("input")).Selected, "pop up completed");
            VerifyIsFalse(driver.FindElement(By.CssSelector("[data-e2e=\"orderStatusIsCanceled\"]")).FindElement(By.TagName("input")).Selected, "pop up canceled");
            VerifyIsFalse(driver.FindElement(By.CssSelector("[data-e2e=\"orderStatusHidden\"]")).FindElement(By.TagName("input")).Selected, "pop up hidden");

            IWebElement selectElem1 = driver.FindElement(By.CssSelector("[data-e2e=\"orderStatusCommand\"]"));
            SelectElement select1 = new SelectElement(selectElem1);
            VerifyIsTrue(select1.SelectedOption.Text.Contains("Списание товара со склада"), "pop up command");

            //check prev default status not default
            GoToAdmin("orderstatuses");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("Order Status5");

            VerifyAreEqual("Order Status5", GetGridCell(0, "StatusName").FindElement(By.TagName("a")).Text, "grid prev default name");
            VerifyIsFalse(GetGridCell(0, "IsDefault").FindElement(By.CssSelector("[data-e2e=\"switchOnOffSelect\"]")).Selected, "grid prev default not checked");

            //check new status added to orders
            GoToAdmin("orders/edit/1");

            IWebElement selectElem2 = driver.FindElement(By.Id("Order_OrderStatusId"));
            SelectElement select2 = new SelectElement(selectElem2);
            VerifyIsTrue(select2.Options.Count == 126, "new status added to orders count");

            VerifyFinally(testname);
        }


        [Test]
        public void OrderStatusEdit()
        {
            testname = "OrderStatusEdit";
            VerifyBegin(testname);

            GoToAdmin("orderstatuses");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("Order Status107");

            VerifyAreEqual("Order Status107", GetGridCell(0, "StatusName").FindElement(By.TagName("a")).Text, "pre check grid name");
            VerifyAreEqual("color: rgb(255, 255, 224);", GetGridCell(0, "Color").FindElement(By.TagName("i")).GetAttribute("style"), "pre check grid color");
            VerifyIsFalse(GetGridCell(0, "IsDefault").FindElement(By.CssSelector("[data-e2e=\"switchOnOffSelect\"]")).Selected, "pre check grid default");
            VerifyIsTrue(GetGridCell(0, "IsCanceled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffSelect\"]")).Selected, "pre check grid IsCanceled");
            VerifyIsTrue(GetGridCell(0, "IsCompleted").FindElement(By.CssSelector("[data-e2e=\"switchOnOffSelect\"]")).Selected, "pre check grid IsCompleted");
            VerifyAreEqual("107", GetGridCell(0, "SortOrder").Text, "grid sort order");
            VerifyAreEqual("Нет команды", GetGridCell(0, "CommandFormatted").Text, "pre check grid status command");

            GetGridCell(0, "StatusName").FindElement(By.TagName("a")).Click();
            Thread.Sleep(4000);
            WaitForElem(By.CssSelector(".modal-content"));

            VerifyAreEqual("Order Status107", driver.FindElement(By.CssSelector("[data-e2e=\"orderStatusName\"]")).GetAttribute("value"), "pre check pop up name");
            VerifyAreEqual("107", driver.FindElement(By.CssSelector("[data-e2e=\"orderStatusSortOrder\"]")).GetAttribute("value"), "pre check pop up sort order");

            VerifyIsFalse(driver.FindElement(By.CssSelector("[data-e2e=\"orderStatusIsDefault\"]")).FindElement(By.TagName("input")).Selected, "pre check pop up default");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"orderStatusIsCompleted\"]")).FindElement(By.TagName("input")).Selected, "pre check pop up completed");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"orderStatusIsCanceled\"]")).FindElement(By.TagName("input")).Selected, "pre check pop up canceled");
            VerifyIsFalse(driver.FindElement(By.CssSelector("[data-e2e=\"orderStatusHidden\"]")).FindElement(By.TagName("input")).Selected, "pre check pop up hidden");

            IWebElement selectElem1 = driver.FindElement(By.CssSelector("[data-e2e=\"orderStatusCommand\"]"));
            SelectElement select1 = new SelectElement(selectElem1);
            VerifyIsTrue(select1.SelectedOption.Text.Contains("Нет команды"), "pre check pop up command");

            driver.FindElement(By.CssSelector("[data-e2e=\"orderStatusColor\"]")).FindElement(By.TagName("input")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"orderStatusColor\"]")).FindElement(By.TagName("input")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"orderStatusColor\"]")).FindElement(By.TagName("input")).SendKeys("#FF00FF");

            driver.FindElement(By.CssSelector("[data-e2e=\"orderStatusName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"orderStatusName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"orderStatusName\"]")).SendKeys("Edited Status Name");

            driver.FindElement(By.CssSelector("[data-e2e=\"orderStatusIsCanceled\"]")).Click();

            driver.FindElement(By.CssSelector("[data-e2e=\"orderStatusIsCompleted\"]")).Click();

            driver.FindElement(By.CssSelector("[data-e2e=\"orderStatusHidden\"]")).Click();
            
            driver.FindElement(By.CssSelector("[data-e2e=\"orderStatusSortOrder\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"orderStatusSortOrder\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"orderStatusSortOrder\"]")).SendKeys("2");
            driver.FindElement(By.CssSelector("[data-e2e=\"orderStatusColor\"]")).FindElement(By.TagName("input")).Click();

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"orderStatusCommand\"]")))).SelectByText("Возврат товара на склад");

            driver.FindElement(By.CssSelector("[data-e2e=\"orderStatusSave\"]")).Click();
            Thread.Sleep(2000);
            
            //check edited status
            GoToAdmin("orderstatuses");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("Order Status107");

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "prev name");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("Edited Status Name");

            VerifyAreEqual("color: rgb(255, 0, 255);", GetGridCell(0, "Color").FindElement(By.TagName("i")).GetAttribute("style"), "grid color");
            VerifyAreEqual("Edited Status Name", GetGridCell(0, "StatusName").FindElement(By.TagName("a")).Text, "grid name");
            VerifyIsFalse(GetGridCell(0, "IsDefault").FindElement(By.CssSelector("[data-e2e=\"switchOnOffSelect\"]")).Selected, "grid default");
            VerifyIsFalse(GetGridCell(0, "IsCanceled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffSelect\"]")).Selected, "grid IsCanceled");
            VerifyIsFalse(GetGridCell(0, "IsCompleted").FindElement(By.CssSelector("[data-e2e=\"switchOnOffSelect\"]")).Selected, "grid IsCompleted");
            VerifyAreEqual("2", GetGridCell(0, "SortOrder").Text, "grid sort order");
            VerifyAreEqual("Возврат товара на склад", GetGridCell(0, "CommandFormatted").Text, "grid status command");

            GetGridCell(0, "StatusName").FindElement(By.TagName("a")).Click();
            Thread.Sleep(4000);
            WaitForElem(By.CssSelector(".modal-content"));

            VerifyAreEqual("Edited Status Name", driver.FindElement(By.CssSelector("[data-e2e=\"orderStatusName\"]")).GetAttribute("value"), "pop up name");
            VerifyAreEqual("2", driver.FindElement(By.CssSelector("[data-e2e=\"orderStatusSortOrder\"]")).GetAttribute("value"), " pop up sort order");

            VerifyIsFalse(driver.FindElement(By.CssSelector("[data-e2e=\"orderStatusIsDefault\"]")).FindElement(By.TagName("input")).Selected, "pop up default");
            VerifyIsFalse(driver.FindElement(By.CssSelector("[data-e2e=\"orderStatusIsCompleted\"]")).FindElement(By.TagName("input")).Selected, "pop up completed");
            VerifyIsFalse(driver.FindElement(By.CssSelector("[data-e2e=\"orderStatusIsCanceled\"]")).FindElement(By.TagName("input")).Selected, "pop up canceled");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"orderStatusHidden\"]")).FindElement(By.TagName("input")).Selected, "pop up hidden");

            IWebElement selectElem2 = driver.FindElement(By.CssSelector("[data-e2e=\"orderStatusCommand\"]"));
            SelectElement select2 = new SelectElement(selectElem2);
            VerifyIsTrue(select2.SelectedOption.Text.Contains("Возврат товара на склад"), "pop up command");

            VerifyFinally(testname);
        }

    }
}

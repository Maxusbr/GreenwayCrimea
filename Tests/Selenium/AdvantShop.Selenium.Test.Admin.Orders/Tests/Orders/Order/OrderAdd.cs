using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;
using AdvantShop.Selenium.Core.Infrastructure;
using System.Collections.ObjectModel;
using System;

namespace AdvantShop.SeleniumTest.Admin.Orders.Order
{
    [TestFixture]
    public class OrderAdd : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog | ClearType.Orders | ClearType.Customers);
            InitializeService.LoadData(
             "Data\\Admin\\Orders\\Order\\[Order].OrderSource.csv",
             "data\\Admin\\Orders\\Order\\Catalog.Product.csv",
           "data\\Admin\\Orders\\Order\\Catalog.Offer.csv",
           "data\\Admin\\Orders\\Order\\Catalog.Category.csv",
           "data\\Admin\\Orders\\Order\\Catalog.ProductCategories.csv",
            "data\\Admin\\Orders\\Order\\[Order].OrderContact.csv",
            "data\\Admin\\Orders\\Order\\[Order].OrderCurrency.csv",
             "data\\Admin\\Orders\\Order\\[Order].OrderItems.csv",
             "data\\Admin\\Orders\\Order\\[Order].OrderStatus.csv",

            "data\\Admin\\Orders\\Order\\Customers.Customer.csv",
           "data\\Admin\\Orders\\Order\\Customers.CustomerGroup.csv",
               "data\\Admin\\Orders\\Order\\Customers.Managers.csv",


               "data\\Admin\\Orders\\Order\\[Order].[Order].csv"
               
          );

            Init();
        }
       
        [Test]
        public void OrdersAdd()
        {
            testname = "OrdersAdd";
            VerifyBegin(testname);
            GoToAdmin("orders/add");

            VerifyAreEqual("Создание нового заказа", driver.FindElement(By.TagName("h1")).FindElement(By.TagName("span")).Text, "OrdersAdd");

            (new SelectElement(driver.FindElement(By.Id("Order_ManagerId")))).SelectByText("test testov");
            (new SelectElement(driver.FindElement(By.Id("Order_OrderSourceId")))).SelectByText("Source1");

            driver.FindElement(By.Id("Order_OrderCustomer_LastName")).Click();
            driver.FindElement(By.Id("Order_OrderCustomer_LastName")).SendKeys("TestSurname");

            driver.FindElement(By.Id("Order_OrderCustomer_FirstName")).Click();
            driver.FindElement(By.Id("Order_OrderCustomer_FirstName")).SendKeys("TestName");

            driver.FindElement(By.Id("Order_OrderCustomer_Patronymic")).Click();
            driver.FindElement(By.Id("Order_OrderCustomer_Patronymic")).SendKeys("TestPatronymic");

            driver.FindElement(By.Id("Order_OrderCustomer_Country")).Click();
            driver.FindElement(By.Id("Order_OrderCustomer_Country")).SendKeys("Россия");

            driver.FindElement(By.Id("Order_OrderCustomer_Region")).Click();
            driver.FindElement(By.Id("Order_OrderCustomer_Region")).SendKeys("Московская область");

            driver.FindElement(By.Id("Order_OrderCustomer_City")).Click();
            driver.FindElement(By.Id("Order_OrderCustomer_City")).SendKeys("Москва");

            driver.FindElement(By.Id("Order_OrderCustomer_Email")).Click();
            driver.FindElement(By.Id("Order_OrderCustomer_Email")).SendKeys("customertest@mail.ru");

            driver.FindElement(By.Id("Order_OrderCustomer_Street")).Click();
            driver.FindElement(By.Id("Order_OrderCustomer_Street")).SendKeys("TestStreet1");

            driver.FindElement(By.Id("Order_OrderCustomer_Zip")).Click();
            driver.FindElement(By.Id("Order_OrderCustomer_Zip")).SendKeys("111111");

            driver.FindElement(By.Id("Order_OrderCustomer_Phone")).Click();
            driver.FindElement(By.Id("Order_OrderCustomer_Phone")).SendKeys("+79277777777");

            driver.FindElement(By.Id("Order_OrderCustomer_House")).Click();
            driver.FindElement(By.Id("Order_OrderCustomer_House")).SendKeys("1");

            driver.FindElement(By.Id("Order_OrderCustomer_Apartment")).Click();
            driver.FindElement(By.Id("Order_OrderCustomer_Apartment")).SendKeys("2");

            driver.FindElement(By.Id("Order_OrderCustomer_Structure")).Click();
            driver.FindElement(By.Id("Order_OrderCustomer_Structure")).SendKeys("3");

            driver.FindElement(By.Id("Order_OrderCustomer_Entrance")).Click();
            driver.FindElement(By.Id("Order_OrderCustomer_Entrance")).SendKeys("4");

            driver.FindElement(By.Id("Order_OrderCustomer_Floor")).Click();
            driver.FindElement(By.Id("Order_OrderCustomer_Floor")).SendKeys("5");
            DropFocus("h1");
            driver.FindElement(By.LinkText("Добавить товар")).Click();
            WaitForAjax();
            driver.FindElement(By.CssSelector(".btn-cancel")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.Id("Order_OrderCustomer_LastName")).Click();

            GoToAdmin("orders?filterby=drafts");
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".link-invert.ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(4000);
            ScrollTo(By.XPath("//h2[contains(text(), 'Содержание заказа')]"));
            DelElement(driver);
            driver.FindElement(By.LinkText("Добавить товар")).Click();
            WaitForAjax();
            GetGridFilter().SendKeys("TestProduct2");
            DropFocus("h2");
            WaitForAjax();
            VerifyAreEqual("TestProduct2", GetGridCell(0, "Name", "OffersSelectvizr").Text, " Search product in Grid");

            GetGridCell(0, "selectionRowHeaderCol", "OffersSelectvizr").Click();
            GetGridCell(1, "selectionRowHeaderCol", "OffersSelectvizr").Click();

            driver.FindElement(By.CssSelector(".btn.btn-save.btn-primary")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("TestProduct2\r\nАртикул: 2", GetGridCell(0, "Name", "OrderItems").Text, "Name product at order");
            VerifyAreEqual("2", GetGridCell(0, "Price", "OrderItems").FindElement(By.TagName("input")).GetAttribute("value"), " Price product at order");
            VerifyAreEqual("1", GetGridCell(0, "Amount", "OrderItems").FindElement(By.TagName("input")).GetAttribute("value"), " Count product at order");
           // VerifyAreEqual("в наличии", GetGridCell(0, "Available", "OrderItems").Text, " Available product at order");
            VerifyIsTrue(GetGridCell(0, "Cost", "OrderItems").Text.Contains("2 "), " Cost product at order");


            driver.FindElement(By.Id("Order_StatusComment")).SendKeys("Comments orders");
            driver.FindElement(By.Id("Order_AdminOrderComment")).SendKeys("Admin comment orders");

            ScrollTo(By.Id("header-top"));
            Thread.Sleep(4000);
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            GoToAdmin("orders");
            //  VerifyAreEqual("2", GetGridCell(0, "Number").Text, "Grid Gifts Number");
            VerifyAreEqual("Новый", GetGridCell(0, "StatusName").Text, " Grid orders StatusName");
            VerifyAreEqual("TestName TestSurname", GetGridCell(0, "BuyerName").Text, " Grid orders BuyerName");
            VerifyAreEqual("test testov", GetGridCell(0, "ManagerName").Text, " Grid orders ManagerName");
            VerifyIsTrue(GetGridCell(0, "SumFormatted").Text.Contains("22 "), " Grid orders SumFormatted");
            
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".link-invert.ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(4000);
            VerifyAreEqual("TestSurname", driver.FindElement(By.Id("Order_OrderCustomer_LastName")).GetAttribute("value"), "\r\n Draft LastName");
            VerifyAreEqual("TestName", driver.FindElement(By.Id("Order_OrderCustomer_FirstName")).GetAttribute("value"), "\r\nDraft FirstName");
            VerifyAreEqual("TestPatronymic", driver.FindElement(By.Id("Order_OrderCustomer_Patronymic")).GetAttribute("value"), "\r\nDraft Patronymic");
            VerifyAreEqual("Россия", driver.FindElement(By.Id("Order_OrderCustomer_Country")).GetAttribute("value"), "\r\nDraft Country");
            VerifyAreEqual("Московская область", driver.FindElement(By.Id("Order_OrderCustomer_Region")).GetAttribute("value"), "\r\nDraft Region");
            VerifyAreEqual("Москва", driver.FindElement(By.Id("Order_OrderCustomer_City")).GetAttribute("value"), "\r\nDraft City");
            VerifyAreEqual("customertest@mail.ru", driver.FindElement(By.Id("Order_OrderCustomer_Email")).GetAttribute("value"), "\r\nDraft Email");
            VerifyAreEqual("111111", driver.FindElement(By.Id("Order_OrderCustomer_Zip")).GetAttribute("value"), "\r\nDraft Zip");
            VerifyAreEqual("+79277777777", driver.FindElement(By.Id("Order_OrderCustomer_Phone")).GetAttribute("value"), "\r\nDraft Phone");
            VerifyAreEqual("TestStreet1", driver.FindElement(By.Id("Order_OrderCustomer_Street")).GetAttribute("value"), "\r\nDraft Street");
            VerifyAreEqual("1", driver.FindElement(By.Id("Order_OrderCustomer_House")).GetAttribute("value"), "\r\nDraft House");
            VerifyAreEqual("2", driver.FindElement(By.Id("Order_OrderCustomer_Apartment")).GetAttribute("value"), "\r\nDraft Apartment");
            VerifyAreEqual("3", driver.FindElement(By.Id("Order_OrderCustomer_Structure")).GetAttribute("value"), "\r\nDraft Structure");
            VerifyAreEqual("4", driver.FindElement(By.Id("Order_OrderCustomer_Entrance")).GetAttribute("value"), "\r\nDraft Entrance");
            VerifyAreEqual("5", driver.FindElement(By.Id("Order_OrderCustomer_Floor")).GetAttribute("value"), "\r\nDraft Floor");

                IWebElement selectElem = driver.FindElement(By.Id("Order_ManagerId"));
                 SelectElement select = new SelectElement(selectElem);
                 VerifyAreEqual("test testov", (select.AllSelectedOptions[0].Text), "\r\nDraft Менеджер");
                 selectElem = driver.FindElement(By.Id("Order_OrderSourceId"));
                 select = new SelectElement(selectElem);
                 VerifyAreEqual("Source1", (select.AllSelectedOptions[0].Text), "\r\nDraft Источник");

            VerifyAreEqual("TestProduct2\r\nАртикул: 2", GetGridCell(0, "Name", "OrderItems").Text, "Draft Name product at order");
            VerifyAreEqual("2", GetGridCell(0, "Price", "OrderItems").FindElement(By.TagName("input")).GetAttribute("value").ToString(), " Draft  product at order");
            VerifyAreEqual("1", GetGridCell(0, "Amount", "OrderItems").FindElement(By.TagName("input")).GetAttribute("value").ToString(), " Draft Count product at order");
            //VerifyAreEqual("в наличии", GetGridCell(0, "Available", "OrderItems").Text, " Draft Available product at order");
            VerifyIsTrue(GetGridCell(0, "Cost", "OrderItems").Text.Contains("2 "), "Draft Cost product at order");

            VerifyAreEqual("Comments orders", driver.FindElement(By.Id("Order_StatusComment")).Text, "Draft Comment");
            VerifyAreEqual("Admin comment orders", driver.FindElement(By.Id("Order_AdminOrderComment")).Text, "Draft AdminOrderComment");

            driver.FindElement(By.CssSelector("[data-e2e=\"SaveCustomer\"]")).Click();
            Thread.Sleep(2000);
            Functions.OpenNewTab(driver, baseURL);

            VerifyAreEqual("TestSurname", driver.FindElement(By.Id("Customer_LastName")).GetAttribute("value"), "\r\n LastName");
               VerifyAreEqual("TestName", driver.FindElement(By.Id("Customer_FirstName")).GetAttribute("value"), "\r\n FirstName");
               VerifyAreEqual("TestPatronymic", driver.FindElement(By.Id("Customer_Patronymic")).GetAttribute("value"), "\r\n Patronymic");
               VerifyAreEqual("Россия", driver.FindElement(By.Id("CustomerContact_Country")).GetAttribute("value"), "\r\n Country");
               VerifyAreEqual("Московская область", driver.FindElement(By.Id("CustomerContact_Region")).GetAttribute("value"), "\r\n Region");
               VerifyAreEqual("Москва", driver.FindElement(By.Id("CustomerContact_City")).GetAttribute("value"), "\r\n City");
               VerifyAreEqual("customertest@mail.ru", driver.FindElement(By.Id("Customer_EMail")).GetAttribute("value"), "\r\n Email");
               VerifyAreEqual("111111", driver.FindElement(By.Id("CustomerContact_Zip")).GetAttribute("value"), "\r\n Zip");
               VerifyAreEqual("+79277777777", driver.FindElement(By.Id("Customer_Phone")).GetAttribute("value"), "\r\n Phone");
               VerifyAreEqual("TestStreet1", driver.FindElement(By.Id("CustomerContact_Street")).GetAttribute("value"), "\r\n Street");
               VerifyAreEqual("1", driver.FindElement(By.Id("CustomerContact_House")).GetAttribute("value"), "\r\n House");
               VerifyAreEqual("2", driver.FindElement(By.Id("CustomerContact_Apartment")).GetAttribute("value"), "\r\n Apartment");
               VerifyAreEqual("3", driver.FindElement(By.Id("CustomerContact_Structure")).GetAttribute("value"), "\r\n Structure");
               VerifyAreEqual("4", driver.FindElement(By.Id("CustomerContact_Entrance")).GetAttribute("value"), "\r\n Entrance");
               VerifyAreEqual("5", driver.FindElement(By.Id("CustomerContact_Floor")).GetAttribute("value"), "\r\n Floor");

            driver.FindElement(By.CssSelector("[data-e2e=\"btnSave\"]")).Click();
            Thread.Sleep(2000);
            GoToAdmin("customers");
            VerifyAreEqual("TestName TestSurname", GetGridCell(0, "Name").Text, "\r\nг grid customer name");

            VerifyFinally(testname);
        }

        public static void DelElement(IWebDriver driver)
        {
            int count = driver.FindElements(By.CssSelector(".ui-grid-custom-service-icon")).Count;
            for (int i = 0; i < count; i++)
            {
                driver.FindElement(By.CssSelector(".ui-grid-custom-service-icon")).Click();
                Thread.Sleep(2000);
                driver.FindElement(By.ClassName("swal2-confirm")).Click();
                Thread.Sleep(2000);
            }
        }
    }
}

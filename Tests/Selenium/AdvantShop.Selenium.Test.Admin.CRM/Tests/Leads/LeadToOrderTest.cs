using System;
using NUnit.Framework;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;
using AdvantShop.Selenium.Core.Infrastructure;
using System.Threading;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;

namespace AdvantShop.Web.Site.Selenium.Test.Admin.CRM.Lead
{
    [TestFixture]
    public class CRMLeadToOrderTest : BaseSeleniumTest
    {

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.CRM | ClearType.Customers | ClearType.Catalog);
            InitializeService.LoadData(
           "data\\Admin\\CRM\\Lead\\Catalog.Product.csv",
           "data\\Admin\\CRM\\Lead\\Catalog.Offer.csv",
           "data\\Admin\\CRM\\Lead\\Catalog.Category.csv",
           "data\\Admin\\CRM\\Lead\\Catalog.ProductCategories.csv",
         "data\\Admin\\CRM\\Lead\\Customers.Customer.csv",
           "data\\Admin\\CRM\\Lead\\Customers.CustomerGroup.csv",
                   "data\\Admin\\CRM\\Lead\\Customers.Departments.csv",
               "data\\Admin\\CRM\\Lead\\Customers.CustomerField.csv",
               "data\\Admin\\CRM\\Lead\\Customers.CustomerFieldValue.csv",
               "data\\Admin\\CRM\\Lead\\Customers.CustomerFieldValuesMap.csv",
               "data\\Admin\\CRM\\Lead\\Customers.Managers.csv",
                "data\\Admin\\CRM\\Lead\\CRM.DealStatus.csv",
               //    "data\\Admin\\CRM\\Lead\\CRM.BizProcessRule.csv",
               "data\\Admin\\CRM\\Lead\\[Order].OrderSource.csv",
                  //   "data\\Admin\\CRM\\Lead\\[Order].OrderStatus.csv",
                "data\\Admin\\CRM\\Lead\\[Order].LeadCurrency.csv",
                 "data\\Admin\\CRM\\Lead\\[Order].LeadEvent.csv",
                    "data\\Admin\\CRM\\Lead\\[Order].LeadItem.csv",
                "data\\Admin\\CRM\\Lead\\[Order].Lead.csv",
         "data\\Admin\\CRM\\Lead\\Customers.TaskGroup.csv",
         "data\\Admin\\CRM\\Lead\\Customers.Task.csv"

          );

            Init();
            GoToAdmin("settingscrm");
        }

         
        
        [Test]
        public void OrderFromLead()
        {
            testname = "OrderFromLead";
            VerifyBegin(testname);
            
            //check order statuses count
            IWebElement selectElem = driver.FindElement(By.Name("OrderStatusIdFromLead"));
            SelectElement select = new SelectElement(selectElem);

            IList<IWebElement> allOptions = select.Options;

            VerifyIsTrue(allOptions.Count == 7, "order statuses count"); // 6 order statuses + null select

            //set OrderStatusIdFromLead
            IWebElement selectElem6 = driver.FindElement(By.Name("OrderStatusIdFromLead"));
            SelectElement select6 = new SelectElement(selectElem6);
            if (!select6.AllSelectedOptions[0].Text.Contains("Доставлен"))
            {
                (new SelectElement(driver.FindElement(By.Name("OrderStatusIdFromLead")))).SelectByText("Доставлен");

                driver.FindElement(By.CssSelector("[data-e2e=\"SettingsCrmSave\"]")).Click();
                Thread.Sleep(4000);
            }
            
            GoToAdmin("settingscrm");

            //check order status selected
            IWebElement selectElem1 = driver.FindElement(By.Name("OrderStatusIdFromLead"));
            SelectElement select1 = new SelectElement(selectElem1);
            VerifyIsTrue(select1.AllSelectedOptions[0].Text.Contains("Доставлен"), "settings order status selected");

            //pre check lead final deal status selected
            IWebElement selectElem2 = driver.FindElement(By.Name("FinalDealStatusId"));
            SelectElement select2 = new SelectElement(selectElem2);
            VerifyIsTrue(select2.AllSelectedOptions[0].Text.Contains("Сделка заключена"), "settings lead final deal status selected");

            //check lead to order
            GoToAdmin("leads/edit/25");
            
            VerifyIsTrue(GetGridCell(0, "Name", "LeadItems").Text.Contains("TestProduct25"), "lead product grid");

            driver.FindElement(By.LinkText("Создать заказ")).Click();
            // driver.FindElement(By.CssSelector("[data-e2e=\"LeadCreateOrder\"]")).Click();
            Thread.Sleep(2000);
            
            GoToAdmin("orders/edit/3");

            VerifyIsTrue(driver.Url.Contains("orders/edit"), "url order");

            //check created order
            VerifyAreEqual("LastName25", driver.FindElement(By.Id("Order_OrderCustomer_LastName")).GetAttribute("value"), "OrderCustomer Last name");
            VerifyAreEqual("FirstName25", driver.FindElement(By.Id("Order_OrderCustomer_FirstName")).GetAttribute("value"), "OrderCustomer first name");
            VerifyAreEqual("Patron25", driver.FindElement(By.Id("Order_OrderCustomer_Patronymic")).GetAttribute("value"), "OrderCustomer patronymic");
            VerifyAreEqual("testmail@mail.ru25", driver.FindElement(By.Id("Order_OrderCustomer_Email")).GetAttribute("value"), "OrderCustomer email");
            VerifyAreEqual("25", driver.FindElement(By.Id("Order_OrderCustomer_Phone")).GetAttribute("value"), "OrderCustomer phone");

            IWebElement selectElem3 = driver.FindElement(By.Id("Order_ManagerId"));
            SelectElement select3 = new SelectElement(selectElem3);
            VerifyIsTrue(select3.AllSelectedOptions[0].Text.Contains("test testov"), "order manager selected");

            IWebElement selectElem4 = driver.FindElement(By.Id("Order_OrderSourceId"));
            SelectElement select4 = new SelectElement(selectElem4);
            VerifyIsTrue(select4.AllSelectedOptions[0].Text.Contains("Корзина интернет магазина"), "order source selected");

            IWebElement selectElem5 = driver.FindElement(By.Id("Order_OrderStatusId"));
            SelectElement select5 = new SelectElement(selectElem5);
            VerifyIsTrue(select5.AllSelectedOptions[0].Text.Contains("Доставлен"), "order status selected");

            VerifyIsTrue(GetGridCell(0, "Name", "OrderItems").Text.Contains("TestProduct25"), "order product grid");

            //check lead final deal status selected
            GoToAdmin("leads");

            GetGridFilterTab(0, "Patron25");

            VerifyAreEqual("Сделка заключена", GetGridCell(0, "DealStatusName").Text, "lead final deal status selected");

            VerifyFinally(testname);
        }
    }
}
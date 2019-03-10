using System;
using NUnit.Framework;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;
using AdvantShop.Selenium.Core.Infrastructure;
using System.Threading;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.Web.Site.Selenium.Test.Admin.CRM.Leads
{
    [TestFixture]
    public class CRMLeadAddEditCustomerFieldsTest : BaseSeleniumTest
    {

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.CRM | ClearType.Customers | ClearType.Catalog);
            InitializeService.LoadData(
           "data\\Admin\\CRM\\Lead\\CustomerFields\\Catalog.Product.csv",
           "data\\Admin\\CRM\\Lead\\CustomerFields\\Catalog.Offer.csv",
           "data\\Admin\\CRM\\Lead\\CustomerFields\\Catalog.Category.csv",
           "data\\Admin\\CRM\\Lead\\CustomerFields\\Catalog.ProductCategories.csv",
         "data\\Admin\\CRM\\Lead\\CustomerFields\\Customers.Customer.csv",
           "data\\Admin\\CRM\\Lead\\CustomerFields\\Customers.CustomerGroup.csv",
                   "data\\Admin\\CRM\\Lead\\CustomerFields\\Customers.Departments.csv",
               "data\\Admin\\CRM\\Lead\\CustomerFields\\Customers.CustomerField.csv",
               "data\\Admin\\CRM\\Lead\\CustomerFields\\Customers.CustomerFieldValue.csv",
               "data\\Admin\\CRM\\Lead\\CustomerFields\\Customers.CustomerFieldValuesMap.csv",
               "data\\Admin\\CRM\\Lead\\CustomerFields\\Customers.Managers.csv",
                "data\\Admin\\CRM\\Lead\\CustomerFields\\CRM.DealStatus.csv",
               "data\\Admin\\CRM\\Lead\\CustomerFields\\[Order].OrderSource.csv",
                "data\\Admin\\CRM\\Lead\\CustomerFields\\[Order].LeadCurrency.csv",
                 "data\\Admin\\CRM\\Lead\\CustomerFields\\[Order].LeadEvent.csv",
                    "data\\Admin\\CRM\\Lead\\CustomerFields\\[Order].LeadItem.csv",
                "data\\Admin\\CRM\\Lead\\CustomerFields\\[Order].Lead.csv",
         "data\\Admin\\CRM\\Lead\\CustomerFields\\Customers.TaskGroup.csv",
         "data\\Admin\\CRM\\Lead\\CustomerFields\\Customers.Task.csv"

          );

            Init();
        }

         
        
        [Test]
        public void LeadAddCustomerFields()
        {
            testname = "LeadAddCustomerFields";
            VerifyBegin(testname);

            GoToAdmin("leads");

            GetButton(eButtonType.Add).Click();
            WaitForElem(By.CssSelector(".modal-content"));
            Thread.Sleep(2000);

            //check only number input
            driver.FindElement(By.CssSelector("[validation-input-text=\"Customer Field 3\"]")).Click();
            driver.FindElement(By.CssSelector("[validation-input-text=\"Customer Field 3\"]")).Clear();
            driver.FindElement(By.CssSelector("[validation-input-text=\"Customer Field 3\"]")).SendKeys("tttttt");
            
            VerifyAreEqual("", driver.FindElement(By.CssSelector("[validation-input-text=\"Customer Field 3\"]")).GetAttribute("value"), "add lead customer field only number");
            
            GoToAdmin("leads");

            GetButton(eButtonType.Add).Click();
            WaitForElem(By.CssSelector(".modal-content"));
            Thread.Sleep(2000);

            //check customer fields
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadLastName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadLastName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadLastName\"]")).SendKeys("LeadCustomerFieldLastName");

            driver.FindElement(By.CssSelector("[data-e2e=\"LeadFirstName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadFirstName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadFirstName\"]")).SendKeys("LeadCustomerFieldFirstName");

            driver.FindElement(By.CssSelector("[data-e2e=\"LeadPatronymic\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadPatronymic\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadPatronymic\"]")).SendKeys("LeadCustomerFieldPatron");

            driver.FindElement(By.CssSelector("[data-e2e=\"LeadPhoneNum\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadPhoneNum\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadPhoneNum\"]")).SendKeys("+71231218888");

            driver.FindElement(By.CssSelector("[data-e2e=\"LeadEmail\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadEmail\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadEmail\"]")).SendKeys("mailtest321@mail.ru");

           // ScrollTo(By.CssSelector("[data-e2e=\"LeadManager\"]"));
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"LeadManager\"]")))).SelectByText("test testov");

            //customer fileds
          //  ScrollTo(By.CssSelector("[data-e2e=\"LeadPhoneNum\"]"));
            (new SelectElement(driver.FindElement(By.CssSelector("[validation-input-text=\"Customer Field 1\"]")))).SelectByText("Customer Field 1 Value 4");

            driver.FindElement(By.CssSelector("[validation-input-text=\"Customer Field 2\"]")).Click();
            driver.FindElement(By.CssSelector("[validation-input-text=\"Customer Field 2\"]")).Clear();
            driver.FindElement(By.CssSelector("[validation-input-text=\"Customer Field 2\"]")).SendKeys("text customer field test");
            
            driver.FindElement(By.CssSelector("[validation-input-text=\"Customer Field 3\"]")).Click();
            driver.FindElement(By.CssSelector("[validation-input-text=\"Customer Field 3\"]")).Clear();
            driver.FindElement(By.CssSelector("[validation-input-text=\"Customer Field 3\"]")).SendKeys("1000");

            driver.FindElement(By.CssSelector("[validation-input-text=\"Customer Field 4\"]")).Click();
            driver.FindElement(By.CssSelector("[validation-input-text=\"Customer Field 4\"]")).Clear();
            driver.FindElement(By.CssSelector("[validation-input-text=\"Customer Field 4\"]")).SendKeys("big text customer field test\r\nline 2 test test\r\nline3test test test");

            ScrollTo(By.CssSelector("[data-e2e=\"LeadNoProducts\"]"));

            driver.FindElement(By.CssSelector("[data-e2e=\"LeadItemAdd\"]")).Click();
            Thread.Sleep(3000);

            XPathContainsText("span", "TestCategory1");
            
            GetGridCell(1, "selectionRowHeaderCol", "OffersSelectvizr").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"] span")).Click();
            
            XPathContainsText("button", "Выбрать");
            Thread.Sleep(3000);

            driver.FindElement(By.CssSelector("[data-e2e=\"LeadAdd\"]")).Click();
            Thread.Sleep(3000);

            //check admin grid
            GoToAdmin("leads");

            GetGridFilterTab(0, "LeadCustomerFieldFirstName");

            VerifyAreEqual("121", GetGridCell(0, "Id").Text, "lead added grid number");
            VerifyIsTrue(GetGridCell(0, "Id").FindElement(By.TagName("a")).GetAttribute("href").Contains("leads/edit/121"), "lead added grid href");
            VerifyAreEqual("LeadCustomerFieldLastName LeadCustomerFieldFirstName LeadCustomerFieldPatron", GetGridCell(0, "FullName").Text, "lead added grid full name");
            VerifyAreEqual("Новый", GetGridCell(0, "DealStatusName").Text, "lead grid DealStatusName");
            VerifyAreEqual("test testov", GetGridCell(0, "ManagerName").Text, "lead grid added manager");
            VerifyAreEqual("1", GetGridCell(0, "ProductsCount").Text, "lead grid added products count");
            VerifyAreEqual("2", GetGridCell(0, "Sum").Text, "lead grid added sum");

            //check admin lead details
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(4000);
            
            IWebElement selectElem6 = driver.FindElement(By.Id("Lead_DealStatusId"));
            SelectElement select6 = new SelectElement(selectElem6);
            VerifyIsTrue(select6.AllSelectedOptions[0].Text.Contains("Новый"), "lead details deal status");

            VerifyAreEqual("", driver.FindElement(By.Id("Lead_Description")).GetAttribute("value"), "lead details description");
            VerifyAreEqual("2", driver.FindElement(By.Id("Lead_Sum")).GetAttribute("value"), "lead details sum");

            IWebElement selectElem4 = driver.FindElement(By.Id("Lead_ManagerId"));
            SelectElement select4 = new SelectElement(selectElem4);
            VerifyIsTrue(select4.AllSelectedOptions[0].Text.Contains("test testov"), "lead manager");

            IWebElement selectElem5 = driver.FindElement(By.Id("Lead_OrderSourceId"));
            SelectElement select5 = new SelectElement(selectElem5);
            VerifyIsTrue(select5.AllSelectedOptions[0].Text.Contains("Другое"), "lead order source");

            VerifyIsTrue(GetGridCell(0, "Name", "LeadItems").Text.Contains("TestProduct2"), "lead product grid");

            //check customer info
            VerifyAreEqual("LeadCustomerFieldLastName", driver.FindElement(By.Id("Lead_Customer_LastName")).GetAttribute("value"), "customer info last name");
            VerifyAreEqual("LeadCustomerFieldFirstName", driver.FindElement(By.Id("Lead_Customer_FirstName")).GetAttribute("value"), "customer info first name");
            VerifyAreEqual("LeadCustomerFieldPatron", driver.FindElement(By.Id("Lead_Customer_Patronymic")).GetAttribute("value"), "customer info patrominic");
            VerifyAreEqual("mailtest321@mail.ru", driver.FindElement(By.Id("Lead_Customer_EMail")).GetAttribute("value"), "customer info email");
            VerifyAreEqual("+71231218888", driver.FindElement(By.Id("Lead_Customer_Phone")).GetAttribute("value"), "customer info phone");

            //check customer fields
            IWebElement selectElem7 = driver.FindElement(By.CssSelector("[validation-input-text=\"Customer Field 1\"]"));
            SelectElement select7 = new SelectElement(selectElem7);
            VerifyIsTrue(select7.AllSelectedOptions[0].Text.Contains("Customer Field 1 Value 4"), "customer info field select");

            VerifyAreEqual("text customer field test", driver.FindElement(By.CssSelector("[validation-input-text=\"Customer Field 2\"]")).GetAttribute("value"), "customer info field test");
            VerifyAreEqual("1000", driver.FindElement(By.CssSelector("[validation-input-text=\"Customer Field 3\"]")).GetAttribute("value"), "customer info field number");
            VerifyAreEqual("big text customer field test\r\n\r\nline 2 test test\r\n\r\nline3test test test", driver.FindElement(By.CssSelector("[validation-input-text=\"Customer Field 4\"]")).GetAttribute("value"), "customer info field big text");
            
            //check new customer
            GoToAdmin("customers");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("LeadCustomerFieldFirstName");

            VerifyAreEqual("LeadCustomerFieldFirstName LeadCustomerFieldLastName", GetGridCell(0, "Name").Text, "new customer name grid saved");
            VerifyAreEqual("+71231218888", GetGridCell(0, "Phone").Text, "new customer Phone grid saved");
            VerifyAreEqual("mailtest321@mail.ru", GetGridCell(0, "Email").Text, "new customer Email grid saved");
            VerifyAreEqual("0", GetGridCell(0, "OrdersCount").Text, "new customer OrdersCount grid saved");
            VerifyAreEqual("", GetGridCell(0, "LastOrderNumber").Text, "new customer LastOrderNumber grid saved");
            VerifyAreEqual("0", GetGridCell(0, "OrdersSum").Text, "new customer OrdersSum grid saved");
            VerifyAreEqual("", GetGridCell(0, "ManagerName").Text, "new customer lead's ManagerName, not customer");

            GetGridCell(0, "Name").FindElement(By.TagName("a")).Click();
            Thread.Sleep(4000);

            //check new customer edit
            VerifyAreEqual("LeadCustomerFieldLastName", driver.FindElement(By.Id("Customer_LastName")).GetAttribute("value"), "new customer first name edit saved");
            VerifyAreEqual("LeadCustomerFieldFirstName", driver.FindElement(By.Id("Customer_FirstName")).GetAttribute("value"), "new customer last name edit saved");
            VerifyAreEqual("LeadCustomerFieldPatron", driver.FindElement(By.Id("Customer_Patronymic")).GetAttribute("value"), "new customer patronymic edit saved");
            VerifyAreEqual("mailtest321@mail.ru", driver.FindElement(By.Id("Customer_EMail")).GetAttribute("value"), "new customer email edit saved");
            VerifyAreEqual("+71231218888", driver.FindElement(By.Id("Customer_Phone")).GetAttribute("value"), "new customer phone edit saved");

            //check new customer edit fields
            VerifyAreEqual("text customer field test", driver.FindElement(By.CssSelector("[validation-input-text=\"Customer Field 2\"]")).GetAttribute("value"), "customer edit field text");
            VerifyAreEqual("1000", driver.FindElement(By.CssSelector("[validation-input-text=\"Customer Field 3\"]")).GetAttribute("value"), "customer edit field number");
            VerifyAreEqual("big text customer field test\r\n\r\nline 2 test test\r\n\r\nline3test test test", driver.FindElement(By.CssSelector("[validation-input-text=\"Customer Field 4\"]")).GetAttribute("value"), "customer edit field big text");
            
            IWebElement selectElem8 = driver.FindElement(By.CssSelector("[validation-input-text=\"Customer Field 1\"]"));
            SelectElement select8 = new SelectElement(selectElem8);
            VerifyIsTrue(select8.AllSelectedOptions[0].Text.Contains("Customer Field 1 Value 4"), "customer edit field select");

            //check new customer edit lead grid
            VerifyAreEqual("121", GetGridCell(0, "Id", "Leads").Text, "customer edit grid lead id");
            VerifyAreEqual("Новый", GetGridCell(0, "DealStatusName", "Leads").Text, "customer edit grid lead deal status");
            VerifyAreEqual("LeadCustomerFieldLastName LeadCustomerFieldFirstName LeadCustomerFieldPatron", GetGridCell(0, "FullName", "Leads").Text, "customer edit grid lead full name");
            VerifyAreEqual("2", GetGridCell(0, "Sum", "Leads").Text, "customer edit grid lead sum");
            VerifyAreEqual("test testov", GetGridCell(0, "ManagerName", "Leads").Text, "customer edit grid lead manager");

            VerifyFinally(testname);
        }

        [Test]
        public void LeadAddCustomerFieldDelete()
        {
            testname = "LeadAddCustomerFieldDelete";
            VerifyBegin(testname);

            GoToAdmin("leads");

            GetButton(eButtonType.Add).Click();
            WaitForElem(By.CssSelector(".modal-content"));
            Thread.Sleep(3000);
            
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadLastName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadLastName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadLastName\"]")).SendKeys("Last");

            XPathContainsText("span", "LastName FirstName, mail@mail.com +7 495 800 200 01");

            XPathContainsText("h2", "Новый лид");

            VerifyAreEqual("LastName", driver.FindElement(By.CssSelector("[data-e2e=\"LeadLastName\"]")).GetAttribute("value"), "existing customer LastName");
            VerifyAreEqual("FirstName", driver.FindElement(By.CssSelector("[data-e2e=\"LeadFirstName\"]")).GetAttribute("value"), "existing customer FirstName");
            VerifyAreEqual("Patronymic", driver.FindElement(By.CssSelector("[data-e2e=\"LeadPatronymic\"]")).GetAttribute("value"), "existing customer Patronymic");
            VerifyAreEqual("+7 495 800 200 01", driver.FindElement(By.CssSelector("[data-e2e=\"LeadPhoneNum\"]")).GetAttribute("value"), "existing customer PhoneNum");
            VerifyAreEqual("mail@mail.com", driver.FindElement(By.CssSelector("[data-e2e=\"LeadEmail\"]")).GetAttribute("value"), "existing customer Email");

            //customer fields
            IWebElement selectElem = driver.FindElement(By.CssSelector("[validation-input-text=\"Customer Field 1\"]"));
            SelectElement select = new SelectElement(selectElem);
            VerifyIsTrue(select.AllSelectedOptions[0].Text.Contains("Customer Field 1 Value 3"), "existing customer field select");
            VerifyAreEqual("pre check дополнительное поле текст", driver.FindElement(By.CssSelector("[validation-input-text=\"Customer Field 2\"]")).GetAttribute("value"), "existing customer field text");
            VerifyAreEqual("123123", driver.FindElement(By.CssSelector("[validation-input-text=\"Customer Field 3\"]")).GetAttribute("value"), "existing customer field number");
            VerifyAreEqual("", driver.FindElement(By.CssSelector("[validation-input-text=\"Customer Field 4\"]")).GetAttribute("value"), "existing customer field big text");

            VerifyIsTrue(driver.FindElement(By.CssSelector(".modal-content")).FindElement(By.CssSelector("[data-e2e=\"LeadCustomer\"]")).Text.Contains("FirstName LastName +7 495 800 200 01 mail@mail.com"), "customer added info");

            driver.FindElement(By.CssSelector("[data-e2e=\"LeadCustomerDelete\"]")).Click();
            Thread.Sleep(4000);

            //check delete customer
            VerifyAreEqual("", driver.FindElement(By.CssSelector("[data-e2e=\"LeadLastName\"]")).GetAttribute("value"), "existing customer LastName deleted");
            VerifyAreEqual("", driver.FindElement(By.CssSelector("[data-e2e=\"LeadFirstName\"]")).GetAttribute("value"), "existing customer FirstName deleted");
            VerifyAreEqual("", driver.FindElement(By.CssSelector("[data-e2e=\"LeadPatronymic\"]")).GetAttribute("value"), "existing customer Patronymic deleted");
            VerifyAreEqual("", driver.FindElement(By.CssSelector("[data-e2e=\"LeadPhoneNum\"]")).GetAttribute("value"), "existing customer PhoneNum deleted");
            VerifyAreEqual("", driver.FindElement(By.CssSelector("[data-e2e=\"LeadEmail\"]")).GetAttribute("value"), "existing customer Email deleted");

            //check delete customer fields
            IWebElement selectElem1 = driver.FindElement(By.CssSelector("[validation-input-text=\"Customer Field 1\"]"));
            SelectElement select1 = new SelectElement(selectElem1);
            VerifyIsTrue(select1.AllSelectedOptions[0].Text.Contains("---"), "existing customer field select deleted");
            VerifyAreEqual("", driver.FindElement(By.CssSelector("[validation-input-text=\"Customer Field 2\"]")).GetAttribute("value"), "existing customer field text deleted");
            VerifyAreEqual("", driver.FindElement(By.CssSelector("[validation-input-text=\"Customer Field 3\"]")).GetAttribute("value"), "existing customer field number deleted");
            VerifyAreEqual("", driver.FindElement(By.CssSelector("[validation-input-text=\"Customer Field 4\"]")).GetAttribute("value"), "existing customer field big text deleted");

            VerifyFinally(testname);
        }

    }
}
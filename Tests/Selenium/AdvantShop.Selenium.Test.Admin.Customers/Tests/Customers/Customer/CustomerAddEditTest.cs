using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using AdvantShop.Selenium.Core.Infrastructure;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;

namespace AdvantShop.Web.Site.Selenium.Test.Admin.Customers.AddEdit
{
    [TestFixture]
    public class CustomerAddEditTest : BaseSeleniumTest
    {

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Customers | ClearType.Orders | ClearType.Catalog);
            InitializeService.LoadData(
               "data\\Admin\\Customers\\CustomerAddEdit\\Customers.CustomerGroup.csv",
                  "data\\Admin\\Customers\\CustomerAddEdit\\Customers.Country.csv",
            "data\\Admin\\Customers\\CustomerAddEdit\\Customers.Region.csv",
            "data\\Admin\\Customers\\CustomerAddEdit\\Customers.City.csv",
            "data\\Admin\\Customers\\CustomerAddEdit\\Customers.Customer.csv",
            "data\\Admin\\Customers\\CustomerAddEdit\\Customers.Contact.csv",
                       "data\\Admin\\Customers\\CustomerAddEdit\\Customers.Departments.csv",
           "data\\Admin\\Customers\\CustomerAddEdit\\Customers.Managers.csv",
               "data\\Admin\\Customers\\CustomerAddEdit\\Customers.CustomerField.csv",
               "data\\Admin\\Customers\\CustomerAddEdit\\Customers.CustomerFieldValue.csv",
               "data\\Admin\\Customers\\CustomerAddEdit\\Customers.CustomerFieldValuesMap.csv",
             "data\\Admin\\Customers\\CustomerAddEdit\\Catalog.Product.csv",
           "data\\Admin\\Customers\\CustomerAddEdit\\Catalog.Offer.csv",
           "data\\Admin\\Customers\\CustomerAddEdit\\Catalog.Category.csv",
           "data\\Admin\\Customers\\CustomerAddEdit\\Catalog.ProductCategories.csv",
            "data\\Admin\\Customers\\CustomerAddEdit\\[Order].OrderContact.csv",
              "Data\\Admin\\Customers\\CustomerAddEdit\\[Order].OrderSource.csv",
            "data\\Admin\\Customers\\CustomerAddEdit\\[Order].OrderCurrency.csv",
             "data\\Admin\\Customers\\CustomerAddEdit\\[Order].OrderItems.csv",
             "data\\Admin\\Customers\\CustomerAddEdit\\[Order].OrderStatus.csv",
                 "data\\Admin\\Customers\\CustomerAddEdit\\[Order].PaymentMethod.csv",
            "data\\Admin\\Customers\\CustomerAddEdit\\[Order].ShippingMethod.csv",
               "data\\Admin\\Customers\\CustomerAddEdit\\[Order].[Order].csv",
          
               "data\\Admin\\Customers\\CustomerAddEdit\\[Order].OrderCustomer.csv"

           );

            Init();
        }

         

        [Test]
        public void CustomerAdd()
        {
            testname = "CustomerAdd";
            VerifyBegin(testname);

            GoToAdmin("customers/add");

            VerifyAreEqual("Новый покупатель", driver.FindElement(By.TagName("h1")).FindElement(By.TagName("span")).Text, "add customer h1");

            driver.FindElement(By.Id("Customer_LastName")).Click();
            driver.FindElement(By.Id("Customer_LastName")).SendKeys("TestSurname");
            
            driver.FindElement(By.Id("Customer_FirstName")).Click();
            driver.FindElement(By.Id("Customer_FirstName")).SendKeys("TestName");

            driver.FindElement(By.Id("Customer_Patronymic")).Click();
            driver.FindElement(By.Id("Customer_Patronymic")).SendKeys("TestPatronymic");

            driver.FindElement(By.Id("CustomerContact_Country")).Click();
            driver.FindElement(By.Id("CustomerContact_Country")).SendKeys("Россия");

            driver.FindElement(By.Id("CustomerContact_Region")).Click();
            driver.FindElement(By.Id("CustomerContact_Region")).SendKeys("Московская область");

            driver.FindElement(By.Id("CustomerContact_City")).Click();
            driver.FindElement(By.Id("CustomerContact_City")).SendKeys("Москва");

            driver.FindElement(By.Id("Customer_EMail")).Click();
            driver.FindElement(By.Id("Customer_EMail")).SendKeys("customertest@mail.ru");

            //driver.FindElement(By.Id("CustomerContact_Address")).Click();
            //driver.FindElement(By.Id("CustomerContact_Address")).SendKeys("МКАД");

            driver.FindElement(By.Id("CustomerContact_Zip")).Click();
            driver.FindElement(By.Id("CustomerContact_Zip")).SendKeys("111111");

            driver.FindElement(By.Id("Customer_Phone")).Click();
            driver.FindElement(By.Id("Customer_Phone")).SendKeys("+79277777777");
            
            driver.FindElement(By.Id("CustomerContact_Street")).Click();
            driver.FindElement(By.Id("CustomerContact_Street")).SendKeys("улица Мира");
            
            driver.FindElement(By.Id("Customer_Password")).Click();
            driver.FindElement(By.Id("Customer_Password")).SendKeys("123123");

            driver.FindElement(By.Id("CustomerContact_House")).Click();
            driver.FindElement(By.Id("CustomerContact_House")).SendKeys("1");

            driver.FindElement(By.Id("CustomerContact_Apartment")).Click();
            driver.FindElement(By.Id("CustomerContact_Apartment")).SendKeys("2");

            driver.FindElement(By.Id("CustomerContact_Structure")).Click();
            driver.FindElement(By.Id("CustomerContact_Structure")).SendKeys("3");

            driver.FindElement(By.Id("CustomerContact_Entrance")).Click();
            driver.FindElement(By.Id("CustomerContact_Entrance")).SendKeys("4");

            driver.FindElement(By.Id("CustomerContact_Floor")).Click();
            driver.FindElement(By.Id("CustomerContact_Floor")).SendKeys("5");

            //check all managers
            IWebElement selectElemManager = driver.FindElement(By.Id("Customer_ManagerId"));
            SelectElement select = new SelectElement(selectElemManager);

            IList<IWebElement> allOptionsManager = select.Options;

            VerifyIsTrue(allOptionsManager.Count == 3, "count managers"); //2 managers + null select

            IWebElement selectElem = driver.FindElement(By.Id("Customer_ManagerId"));
            SelectElement select2 = new SelectElement(selectElem);
            VerifyIsTrue(select2.AllSelectedOptions[0].Text.Contains("-"), "no manager ay first");

            (new SelectElement(driver.FindElement(By.Id("Customer_ManagerId")))).SelectByText("test testov");

            driver.FindElement(By.CssSelector("[data-e2e=\"SubscribedNews\"] span")).Click();

            //check all customer groups
            IWebElement selectElemCustomerGroup = driver.FindElement(By.Id("Customer_CustomerGroupId"));
            SelectElement select3 = new SelectElement(selectElemCustomerGroup);

            IList<IWebElement> allOptionsCustomerGroup = select3.Options;

            VerifyIsTrue(allOptionsCustomerGroup.Count == 2, "count customer groups"); //2 customer groups

            (new SelectElement(driver.FindElement(By.Id("Customer_CustomerGroupId")))).SelectByValue("2");

            driver.FindElement(By.Id("Customer_AdminComment")).Click();
            driver.FindElement(By.Id("Customer_AdminComment")).Clear();
            driver.FindElement(By.Id("Customer_AdminComment")).SendKeys("Customer Admin Comment Test");

            //customer fileds
            driver.FindElement(By.CssSelector("[validation-input-text=\"Customer Field 3\"]")).Click();
            driver.FindElement(By.CssSelector("[validation-input-text=\"Customer Field 3\"]")).Clear();
            driver.FindElement(By.CssSelector("[validation-input-text=\"Customer Field 3\"]")).SendKeys("999");

            driver.FindElement(By.CssSelector("[validation-input-text=\"Customer Field 2\"]")).Click();
            driver.FindElement(By.CssSelector("[validation-input-text=\"Customer Field 2\"]")).Clear();
            driver.FindElement(By.CssSelector("[validation-input-text=\"Customer Field 2\"]")).SendKeys("дополнительное поле");

            VerifyIsTrue(driver.PageSource.Contains("Customer Field 1"), "customer field not required 1");
            VerifyIsTrue(driver.PageSource.Contains("Customer Field 4"), "customer field not required 2");
            VerifyIsFalse(driver.PageSource.Contains("Customer Field 5"), "customer field not disabled");

            GetButton(eButtonType.Save).Click();

            //check admin
            GoToAdmin("customers");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("TestName TestSurname");
            DropFocus("h1");
            WaitForAjax();
            VerifyAreEqual("TestName TestSurname", GetGridCell(0, "Name").Text, "grid customer name");

            GetGridCell(0, "Name").Click();

            VerifyIsTrue(driver.FindElement(By.Id("Customer_SubscribedForNews")).Selected, "subscribed for news");

            IWebElement selectElemEditManager = driver.FindElement(By.Id("Customer_ManagerId"));
            SelectElement selectEdit = new SelectElement(selectElemEditManager);
            VerifyIsTrue(selectEdit.AllSelectedOptions[0].Text.Contains("test testov"), "manager");

            IWebElement selectElemEditCustomerGroup = driver.FindElement(By.Id("Customer_CustomerGroupId"));
            SelectElement selectEdit2 = new SelectElement(selectElemEditCustomerGroup);
            VerifyIsTrue(selectEdit2.AllSelectedOptions[0].Text.Contains("CustomerGroup2 - 10%"), "customer group");

            VerifyAreEqual("Customer Admin Comment Test", driver.FindElement(By.Id("Customer_AdminComment")).GetAttribute("value"), "admin's comment");

            VerifyAreEqual("TestSurname", driver.FindElement(By.Id("Customer_LastName")).GetAttribute("value"), "last name");
            VerifyAreEqual("TestName", driver.FindElement(By.Id("Customer_FirstName")).GetAttribute("value"), "first name");
            VerifyAreEqual("TestPatronymic", driver.FindElement(By.Id("Customer_Patronymic")).GetAttribute("value"), "patronymic");
            VerifyAreEqual("Россия", driver.FindElement(By.Id("CustomerContact_Country")).GetAttribute("value"), "country");
            VerifyAreEqual("Московская область", driver.FindElement(By.Id("CustomerContact_Region")).GetAttribute("value"), "region");
            VerifyAreEqual("Москва", driver.FindElement(By.Id("CustomerContact_City")).GetAttribute("value"), "Город");
            VerifyAreEqual("customertest@mail.ru", driver.FindElement(By.Id("Customer_EMail")).GetAttribute("value"), "Email");
           //VerifyAreEqual("МКАД", driver.FindElement(By.Id("CustomerContact_Address")).GetAttribute("value"), "\r\nАдрес");
            VerifyAreEqual("111111", driver.FindElement(By.Id("CustomerContact_Zip")).GetAttribute("value"), "zip");
            VerifyAreEqual("+79277777777", driver.FindElement(By.Id("Customer_Phone")).GetAttribute("value"), "phone number");
            VerifyAreEqual("улица Мира", driver.FindElement(By.Id("CustomerContact_Street")).GetAttribute("value"), "street");
            VerifyAreEqual("1", driver.FindElement(By.Id("CustomerContact_House")).GetAttribute("value"), "house");
            VerifyAreEqual("2", driver.FindElement(By.Id("CustomerContact_Apartment")).GetAttribute("value"), "appartment");
            VerifyAreEqual("3", driver.FindElement(By.Id("CustomerContact_Structure")).GetAttribute("value"), "structure");
            VerifyAreEqual("4", driver.FindElement(By.Id("CustomerContact_Entrance")).GetAttribute("value"), "entrance");
            VerifyAreEqual("5", driver.FindElement(By.Id("CustomerContact_Floor")).GetAttribute("value"), "floor");

            //check customer fields
            VerifyAreEqual("999", driver.FindElement(By.CssSelector("[validation-input-text=\"Customer Field 3\"]")).GetAttribute("value"), "customer filed number");
            VerifyAreEqual("дополнительное поле", driver.FindElement(By.CssSelector("[validation-input-text=\"Customer Field 2\"]")).GetAttribute("value"), "customer filed test");

            VerifyFinally(testname);
        }

        [Test]
        public void CustomerEdit()
        {
            testname = "CustomerEdit";
            VerifyBegin(testname);

            GoToAdmin("customers");
            
            VerifyAreEqual("Покупатели", driver.FindElement(By.TagName("h1")).Text, "h1 customer grid");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("FirstName");
            DropFocus("h1");
            WaitForAjax();
            VerifyAreEqual("FirstName LastName", GetGridCell(0, "Name").Text, "customer name grid before edit");

            GetGridCell(0, "Name").FindElement(By.TagName("a")).Click();

            //pre check
            VerifyIsTrue(!driver.FindElement(By.Id("Customer_SubscribedForNews")).Selected, "pre check subscribe for news");
            VerifyAreEqual("LastName", driver.FindElement(By.Id("Customer_LastName")).GetAttribute("value"), "pre check last name");
            VerifyAreEqual("FirstName", driver.FindElement(By.Id("Customer_FirstName")).GetAttribute("value"), "pre check first name");
            VerifyAreEqual("Patronymic", driver.FindElement(By.Id("Customer_Patronymic")).GetAttribute("value"), "pre check patromymic");
            VerifyAreEqual("Россия", driver.FindElement(By.Id("CustomerContact_Country")).GetAttribute("value"), "pre check Страна");
            VerifyAreEqual("Московская область", driver.FindElement(By.Id("CustomerContact_Region")).GetAttribute("value"), "pre check Регион");
            VerifyAreEqual("Москва", driver.FindElement(By.Id("CustomerContact_City")).GetAttribute("value"), "pre check сшен");
            VerifyAreEqual("mail@mail.com", driver.FindElement(By.Id("Customer_EMail")).GetAttribute("value"), "pre check Email");
            VerifyAreEqual("222222", driver.FindElement(By.Id("CustomerContact_Zip")).GetAttribute("value"), "pre check zip");
            VerifyAreEqual("+7 495 800 200 01", driver.FindElement(By.Id("Customer_Phone")).GetAttribute("value"), "pre check phone number");
            VerifyAreEqual("Улица", driver.FindElement(By.Id("CustomerContact_Street")).GetAttribute("value"), "pre check street");
            VerifyAreEqual("1", driver.FindElement(By.Id("CustomerContact_House")).GetAttribute("value"), "pre check house");
            VerifyAreEqual("2", driver.FindElement(By.Id("CustomerContact_Apartment")).GetAttribute("value"), "pre check apartment");
            VerifyAreEqual("3", driver.FindElement(By.Id("CustomerContact_Structure")).GetAttribute("value"), "pre check structure");
            VerifyAreEqual("4", driver.FindElement(By.Id("CustomerContact_Entrance")).GetAttribute("value"), "pre check entrance");
            VerifyAreEqual("5", driver.FindElement(By.Id("CustomerContact_Floor")).GetAttribute("value"), "pre check floor");

            IWebElement selectElemManager = driver.FindElement(By.Id("Customer_ManagerId"));
            SelectElement select = new SelectElement(selectElemManager);
            VerifyIsTrue(select.AllSelectedOptions[0].Text.Contains("test testov"), "pre check manager");

            IWebElement selectElemCustomerGroup = driver.FindElement(By.Id("Customer_CustomerGroupId"));
            SelectElement select3 = new SelectElement(selectElemCustomerGroup);
            VerifyIsTrue(select3.AllSelectedOptions[0].Text.Contains("CustomerGroup2 - 10%"), "pre check customer group");

            VerifyAreEqual("Admin Comment Deafult", driver.FindElement(By.Id("Customer_AdminComment")).GetAttribute("value"), "pre check admin's comment");

            //pre check customer fields
            VerifyAreEqual("123123", driver.FindElement(By.CssSelector("[validation-input-text=\"Customer Field 3\"]")).GetAttribute("value"), "pre check customer field number");
            VerifyAreEqual("pre check дополнительное поле текст", driver.FindElement(By.CssSelector("[validation-input-text=\"Customer Field 2\"]")).GetAttribute("value"), "pre check customer field test");

            IWebElement selectElemField = driver.FindElement(By.CssSelector("[validation-input-text=\"Customer Field 1\"]"));
            SelectElement select2 = new SelectElement(selectElemField);
            VerifyIsTrue(select2.AllSelectedOptions[0].Text.Contains("Customer Field 1 Value 3"), "pre check customer field select");

            //edit
            driver.FindElement(By.Id("Customer_LastName")).Click();
            driver.FindElement(By.Id("Customer_LastName")).Clear();
            driver.FindElement(By.Id("Customer_LastName")).SendKeys("changedSurname");

            driver.FindElement(By.Id("Customer_FirstName")).Click();
            driver.FindElement(By.Id("Customer_FirstName")).Clear();
            driver.FindElement(By.Id("Customer_FirstName")).SendKeys("changedName");

            driver.FindElement(By.Id("Customer_Patronymic")).Click();
            driver.FindElement(By.Id("Customer_Patronymic")).Clear();
            driver.FindElement(By.Id("Customer_Patronymic")).SendKeys("changedPatronymic");

            driver.FindElement(By.Id("CustomerContact_Country")).Click();
            driver.FindElement(By.Id("CustomerContact_Country")).Clear();
            driver.FindElement(By.Id("CustomerContact_Country")).SendKeys("Страна");

            driver.FindElement(By.Id("CustomerContact_Region")).Click();
            driver.FindElement(By.Id("CustomerContact_Region")).Clear();
            driver.FindElement(By.Id("CustomerContact_Region")).SendKeys("Регион");

            driver.FindElement(By.Id("CustomerContact_City")).Click();
            driver.FindElement(By.Id("CustomerContact_City")).Clear();
            driver.FindElement(By.Id("CustomerContact_City")).SendKeys("Город");

            driver.FindElement(By.Id("Customer_EMail")).Click();
            driver.FindElement(By.Id("Customer_EMail")).Clear();
            driver.FindElement(By.Id("Customer_EMail")).SendKeys("editedtest@mail.ru");
            
            driver.FindElement(By.Id("CustomerContact_Zip")).Click();
            driver.FindElement(By.Id("CustomerContact_Zip")).Clear();
            driver.FindElement(By.Id("CustomerContact_Zip")).SendKeys("555555");

            driver.FindElement(By.Id("Customer_Phone")).Click();
            driver.FindElement(By.Id("Customer_Phone")).Clear();
            driver.FindElement(By.Id("Customer_Phone")).SendKeys("+79308888888");

            driver.FindElement(By.Id("CustomerContact_Street")).Click();
            driver.FindElement(By.Id("CustomerContact_Street")).Clear();
            driver.FindElement(By.Id("CustomerContact_Street")).SendKeys("улица Мира");

            driver.FindElement(By.Id("CustomerContact_House")).Click();
            driver.FindElement(By.Id("CustomerContact_House")).Clear();
            driver.FindElement(By.Id("CustomerContact_House")).SendKeys("5");

            driver.FindElement(By.Id("CustomerContact_Apartment")).Click();
            driver.FindElement(By.Id("CustomerContact_Apartment")).Clear();
            driver.FindElement(By.Id("CustomerContact_Apartment")).SendKeys("4");

            driver.FindElement(By.Id("CustomerContact_Structure")).Click();
            driver.FindElement(By.Id("CustomerContact_Structure")).Clear();
            driver.FindElement(By.Id("CustomerContact_Structure")).SendKeys("2");

            driver.FindElement(By.Id("CustomerContact_Entrance")).Click();
            driver.FindElement(By.Id("CustomerContact_Entrance")).Clear();
            driver.FindElement(By.Id("CustomerContact_Entrance")).SendKeys("3");

            driver.FindElement(By.Id("CustomerContact_Floor")).Click();
            driver.FindElement(By.Id("CustomerContact_Floor")).Clear();
            driver.FindElement(By.Id("CustomerContact_Floor")).SendKeys("1");

            (new SelectElement(driver.FindElement(By.Id("Customer_ManagerId")))).SelectByText("Elena El");

            (new SelectElement(driver.FindElement(By.Id("Customer_CustomerGroupId")))).SelectByValue("1");
            
            driver.FindElement(By.Id("Customer_AdminComment")).Click();
            driver.FindElement(By.Id("Customer_AdminComment")).Clear();
            driver.FindElement(By.Id("Customer_AdminComment")).SendKeys("Edited Test Comment");

            driver.FindElement(By.CssSelector("[data-e2e=\"SubscribedNews\"] span")).Click();
            
            //customer fileds
            driver.FindElement(By.CssSelector("[validation-input-text=\"Customer Field 3\"]")).Click();
            driver.FindElement(By.CssSelector("[validation-input-text=\"Customer Field 3\"]")).Clear();
            driver.FindElement(By.CssSelector("[validation-input-text=\"Customer Field 3\"]")).SendKeys("5000");

            driver.FindElement(By.CssSelector("[validation-input-text=\"Customer Field 2\"]")).Click();
            driver.FindElement(By.CssSelector("[validation-input-text=\"Customer Field 2\"]")).Clear();
            driver.FindElement(By.CssSelector("[validation-input-text=\"Customer Field 2\"]")).SendKeys("отредактированное доп. поле тест");

            (new SelectElement(driver.FindElement(By.CssSelector("[validation-input-text=\"Customer Field 1\"]")))).SelectByText("Customer Field 1 Value 5");

            GetButton(eButtonType.Save).Click();

            //check admin
            GoToAdmin("customers");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("FirstName LastName");
            DropFocus("h1");
            WaitForAjax();
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "grid previous customer");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("changedName changedSurname");
            DropFocus("h1");
            WaitForAjax();
            VerifyAreEqual("changedName changedSurname", GetGridCell(0, "Name").Text, "grid customer name");

            GetGridCell(0, "Name").Click();
            
            IWebElement selectElemEditManager = driver.FindElement(By.Id("Customer_ManagerId"));
            SelectElement selectEdit = new SelectElement(selectElemEditManager);
            VerifyIsTrue(selectEdit.AllSelectedOptions[0].Text.Contains("Elena El"), "manager");

            IWebElement selectElemEditCustomerGroup = driver.FindElement(By.Id("Customer_CustomerGroupId"));
            SelectElement select4 = new SelectElement(selectElemEditCustomerGroup);
            VerifyIsTrue(select4.AllSelectedOptions[0].Text.Contains("CustomerGroup1 - 10%"), "customer group");

            VerifyAreEqual("Edited Test Comment", driver.FindElement(By.Id("Customer_AdminComment")).GetAttribute("value"), "admin's comment");

            VerifyIsTrue(driver.FindElement(By.Id("Customer_SubscribedForNews")).Selected, "подписка на новости");
            VerifyAreEqual("changedSurname", driver.FindElement(By.Id("Customer_LastName")).GetAttribute("value"), "last name");
            VerifyAreEqual("changedName", driver.FindElement(By.Id("Customer_FirstName")).GetAttribute("value"), "first name");
            VerifyAreEqual("changedPatronymic", driver.FindElement(By.Id("Customer_Patronymic")).GetAttribute("value"), "patronymic");
            VerifyAreEqual("Страна", driver.FindElement(By.Id("CustomerContact_Country")).GetAttribute("value"), "country");
            VerifyAreEqual("Регион", driver.FindElement(By.Id("CustomerContact_Region")).GetAttribute("value"), "region");
            VerifyAreEqual("Город", driver.FindElement(By.Id("CustomerContact_City")).GetAttribute("value"), "city");
            VerifyAreEqual("editedtest@mail.ru", driver.FindElement(By.Id("Customer_EMail")).GetAttribute("value"), "Email");
            VerifyAreEqual("555555", driver.FindElement(By.Id("CustomerContact_Zip")).GetAttribute("value"), "zip");
            VerifyAreEqual("+79308888888", driver.FindElement(By.Id("Customer_Phone")).GetAttribute("value"), "phone number");
            VerifyAreEqual("улица Мира", driver.FindElement(By.Id("CustomerContact_Street")).GetAttribute("value"), "street");
            VerifyAreEqual("5", driver.FindElement(By.Id("CustomerContact_House")).GetAttribute("value"), "house");
            VerifyAreEqual("4", driver.FindElement(By.Id("CustomerContact_Apartment")).GetAttribute("value"), "apartment");
            VerifyAreEqual("2", driver.FindElement(By.Id("CustomerContact_Structure")).GetAttribute("value"), "structure");
            VerifyAreEqual("3", driver.FindElement(By.Id("CustomerContact_Entrance")).GetAttribute("value"), "entrance");
            VerifyAreEqual("1", driver.FindElement(By.Id("CustomerContact_Floor")).GetAttribute("value"), "floor");

            //check customer fields
            VerifyAreEqual("5000", driver.FindElement(By.CssSelector("[validation-input-text=\"Customer Field 3\"]")).GetAttribute("value"), "customer field number");
            VerifyAreEqual("отредактированное доп. поле тест", driver.FindElement(By.CssSelector("[validation-input-text=\"Customer Field 2\"]")).GetAttribute("value"), "customer field text");

            IWebElement selectElemFieldEdit = driver.FindElement(By.CssSelector("[validation-input-text=\"Customer Field 1\"]"));
            SelectElement selectEdit2 = new SelectElement(selectElemFieldEdit);
            VerifyIsTrue(selectEdit2.AllSelectedOptions[0].Text.Contains("Customer Field 1 Value 5"), "customer field select edited");

            VerifyFinally(testname);
        }

    }
}
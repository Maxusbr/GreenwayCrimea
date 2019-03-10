using NUnit.Framework;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;
using AdvantShop.Selenium.Core.Infrastructure;
using System.Threading;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;

namespace AdvantShop.Web.Site.Selenium.Test.Admin.CRM.Leads
{
    [TestFixture]
    public class CRMLeadAddEditTest : BaseSeleniumTest
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
             //  "data\\Admin\\CRM\\Lead\\Customers.CustomerField.csv",
         //      "data\\Admin\\CRM\\Lead\\Customers.CustomerFieldValue.csv",
         //      "data\\Admin\\CRM\\Lead\\Customers.CustomerFieldValuesMap.csv",
               "data\\Admin\\CRM\\Lead\\Customers.Managers.csv",
                "data\\Admin\\CRM\\Lead\\CRM.DealStatus.csv",
               //    "data\\Admin\\CRM\\Lead\\CRM.BizProcessRule.csv",
               "data\\Admin\\CRM\\Lead\\[Order].OrderSource.csv",
                "data\\Admin\\CRM\\Lead\\[Order].LeadCurrency.csv",
                 "data\\Admin\\CRM\\Lead\\[Order].LeadEvent.csv",
                    "data\\Admin\\CRM\\Lead\\[Order].LeadItem.csv",
                "data\\Admin\\CRM\\Lead\\[Order].Lead.csv",
         "data\\Admin\\CRM\\Lead\\Customers.TaskGroup.csv",
         "data\\Admin\\CRM\\Lead\\Customers.Task.csv"

          );

            Init();
        }

         

     
        [Test]
        public void aLeadAddNoProduct()
        {
            testname = "LeadAddNoProduct";
            VerifyBegin(testname);

            GoToAdmin("leads");

            GetButton(eButtonType.Add).Click();
            WaitForElem(By.CssSelector(".modal-content"));
            Thread.Sleep(2000);

            VerifyAreEqual("Новый лид", driver.FindElement(By.CssSelector(".modal-header-title")).Text, "h1 lead modal");
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadLastName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadLastName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadLastName\"]")).SendKeys("NoProductLastName");

            driver.FindElement(By.CssSelector("[data-e2e=\"LeadFirstName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadFirstName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadFirstName\"]")).SendKeys("NoProductFirstName");

            driver.FindElement(By.CssSelector("[data-e2e=\"LeadPatronymic\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadPatronymic\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadPatronymic\"]")).SendKeys("NoProductPatronymic");

            driver.FindElement(By.CssSelector("[data-e2e=\"LeadPhoneNum\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadPhoneNum\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadPhoneNum\"]")).SendKeys("+71231212123");

            driver.FindElement(By.CssSelector("[data-e2e=\"LeadEmail\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadEmail\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadEmail\"]")).SendKeys("mailtest@mail.ru");

            ScrollTo(By.CssSelector("[data-e2e=\"LeadNoProducts\"]"));

            VerifyAreEqual("Нет товаров", driver.FindElement(By.CssSelector("[data-e2e=\"LeadNoProducts\"]")).Text, "no products at first");
            VerifyIsFalse(driver.FindElements(By.CssSelector("[data-e2e=\"LeadAddItemsTable\"]")).Count > 0, "no products table at first");

            driver.FindElement(By.CssSelector("[data-e2e=\"LeadDescription\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadDescription\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadDescription\"]")).SendKeys("Description Test");

            VerifyAreEqual("0", driver.FindElement(By.CssSelector("[data-e2e=\"LeadSum\"]")).GetAttribute("value"), "lead sum no products");

            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"LeadCurrency\"]")).Text.Contains("руб."), "currency");

            IWebElement selectElem1 = driver.FindElement(By.CssSelector("[data-e2e=\"LeadDealStatus\"]"));
            SelectElement select1 = new SelectElement(selectElem1);
            VerifyIsTrue(select1.AllSelectedOptions[0].Text.Contains("Новый"), "selected new status at first");
            
            //check all lead statuses
            IWebElement selectElem = driver.FindElement(By.CssSelector("[data-e2e=\"LeadDealStatus\"]"));
            SelectElement select = new SelectElement(selectElem);
            IList<IWebElement> allOptionsLeadStatus = select.Options;
            VerifyIsTrue(allOptionsLeadStatus.Count == 6, "count all lead statuses");

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"LeadDealStatus\"]")))).SelectByText("Созвон с клиентом");

            IWebElement selectElem2 = driver.FindElement(By.CssSelector("[data-e2e=\"LeadManager\"]"));
            SelectElement select2 = new SelectElement(selectElem2);
            VerifyIsTrue(select2.AllSelectedOptions[0].Text.Contains("Выберите менеджера"), "no manager at first");

            //check all managers
            IWebElement selectElem3 = driver.FindElement(By.CssSelector("[data-e2e=\"LeadManager\"]"));
            SelectElement select3 = new SelectElement(selectElem3);
            IList<IWebElement> allOptionsLeadManager = select3.Options;
            VerifyIsTrue(allOptionsLeadManager.Count == 3, "count all 2 lead managers + null manager");

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"LeadManager\"]")))).SelectByText("test testov");

            driver.FindElement(By.CssSelector("[data-e2e=\"LeadAdd\"]")).Click();
            Thread.Sleep(4000);

            //check admin grid
            GoToAdmin("leads");
            
            GetGridFilterTab(0, "NoProductLastName");

            VerifyAreEqual("121", GetGridCell(0, "Id").Text, "lead added number");
            VerifyIsTrue(GetGridCell(0, "Id").FindElement(By.TagName("a")).GetAttribute("href").Contains("leads/edit/121"), "lead added href");
            VerifyAreEqual("Созвон с клиентом", GetGridCell(0, "DealStatusName").Text, "lead added deal status name");
            VerifyAreEqual("NoProductLastName NoProductFirstName NoProductPatronymic", GetGridCell(0, "FullName").Text, "lead added full name");
            VerifyAreEqual("test testov", GetGridCell(0, "ManagerName").Text, "lead added manager name");
            VerifyAreEqual("0", GetGridCell(0, "ProductsCount").Text, "lead added products count");
            VerifyAreEqual("0", GetGridCell(0, "Sum").Text, "lead added sum");

            //check admin lead details
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(4000);

            VerifyIsTrue(driver.FindElement(By.TagName("h1")).Text.Contains("Лид № 121"), "h1 lead page");

            IWebElement selectElem6 = driver.FindElement(By.Id("Lead_DealStatusId"));
            SelectElement select6 = new SelectElement(selectElem6);
            VerifyIsTrue(select6.AllSelectedOptions[0].Text.Contains("Созвон с клиентом"), "lead deal status");
            
            VerifyAreEqual("Description Test", driver.FindElement(By.Id("Lead_Description")).GetAttribute("value"), "lead description");
            VerifyAreEqual("0", driver.FindElement(By.Id("Lead_Sum")).GetAttribute("value"));

            IWebElement selectElem4 = driver.FindElement(By.Id("Lead_ManagerId"));
            SelectElement select4 = new SelectElement(selectElem4);
            VerifyIsTrue(select4.AllSelectedOptions[0].Text.Contains("test testov"), "lead manager");

            IWebElement selectElem5 = driver.FindElement(By.Id("Lead_OrderSourceId"));
            SelectElement select5 = new SelectElement(selectElem5);

            VerifyIsTrue(select5.AllSelectedOptions[0].Text.Contains("Другое"), "lead order source");
            
            VerifyIsTrue(driver.FindElement(By.CssSelector("[grid-unique-id=\"gridLeadItems\"]")).Text.Contains("Выберите товары"), "lead no products");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[grid-unique-id=\"gridTasks\"]")).Text.Contains("Ни одной записи не найдено"), "lead no tasks");
            
            VerifyFinally(testname);
        }

        [Test]
        public void LeadAddSumEditable()
        {
            GoToAdmin("leads");

            GetButton(eButtonType.Add).Click();
            WaitForElem(By.CssSelector(".modal-content"));
            Thread.Sleep(2000);

            driver.FindElement(By.CssSelector("[data-e2e=\"LeadFirstName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadFirstName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadFirstName\"]")).SendKeys("sum editable");

            driver.FindElement(By.CssSelector("[data-e2e=\"LeadPhoneNum\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadPhoneNum\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadPhoneNum\"]")).SendKeys("+71231212923");

            Assert.AreEqual("Нет товаров", driver.FindElement(By.CssSelector("[data-e2e=\"LeadNoProducts\"]")).Text);
            Assert.IsFalse(driver.FindElements(By.CssSelector("[data-e2e=\"LeadAddItemsTable\"]")).Count > 0);

            driver.FindElement(By.CssSelector("[data-e2e=\"LeadAdd\"]")).Click();
            Thread.Sleep(3000);

            //check admin
            GoToAdmin("leads");

            GetGridFilterTab(0, "sum editable");

            Assert.AreEqual("sum editable", GetGridCell(0, "FullName").Text);
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(4000);

            isElementNotPresent(By.Id("Lead_Sum"), "readonly"); //можно редактировать вручную 

        }

        [Test]
        public void LeadAddSumNotEditable()
        {
            GoToAdmin("leads");

            GetButton(eButtonType.Add).Click();
            WaitForElem(By.CssSelector(".modal-content"));
            Thread.Sleep(2000);

            driver.FindElement(By.CssSelector("[data-e2e=\"LeadFirstName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadFirstName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadFirstName\"]")).SendKeys("sum not editable");

            driver.FindElement(By.CssSelector("[data-e2e=\"LeadPhoneNum\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadPhoneNum\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadPhoneNum\"]")).SendKeys("+8231212923");

            ScrollTo(By.CssSelector("[data-e2e=\"LeadNoProducts\"]"));

            driver.FindElement(By.CssSelector("[data-e2e=\"LeadItemAdd\"]")).Click();
            Thread.Sleep(3000);

            XPathContainsText("span", "TestCategory1");

            GetGridCell(0, "selectionRowHeaderCol", "OffersSelectvizr").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"] span")).Click();
            
            XPathContainsText("button", "Выбрать");

            driver.FindElement(By.CssSelector("[data-e2e=\"LeadAdd\"]")).Click();
            Thread.Sleep(4000);

            //check admin
            GoToAdmin("leads");

            GetGridFilterTab(0, "sum not editable");

            Assert.AreEqual("sum not editable", GetGridCell(0, "FullName").Text);
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(4000);
            
            Assert.IsTrue(driver.FindElement(By.Id("Lead_Sum")).GetAttribute("readonly").Equals("true")); //нельзя редактировать вручную 

        }

        [Test]
        public void LeadAdd()
        {
            testname = "LeadAdd";
            VerifyBegin(testname);

            GoToAdmin("leads");

            GetButton(eButtonType.Add).Click();
            WaitForElem(By.CssSelector(".modal-content"));
            Thread.Sleep(2000);

            driver.FindElement(By.CssSelector("[data-e2e=\"LeadLastName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadLastName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadLastName\"]")).SendKeys("WithProductLastName");

            driver.FindElement(By.CssSelector("[data-e2e=\"LeadFirstName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadFirstName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadFirstName\"]")).SendKeys("WithProductFirstName");

            driver.FindElement(By.CssSelector("[data-e2e=\"LeadPatronymic\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadPatronymic\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadPatronymic\"]")).SendKeys("WithProductPatronymic");

            driver.FindElement(By.CssSelector("[data-e2e=\"LeadPhoneNum\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadPhoneNum\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadPhoneNum\"]")).SendKeys("+71231212143");

            driver.FindElement(By.CssSelector("[data-e2e=\"LeadEmail\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadEmail\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadEmail\"]")).SendKeys("mailtest123@mail.ru");

            ScrollTo(By.CssSelector("[data-e2e=\"LeadNoProducts\"]"));

            driver.FindElement(By.CssSelector("[data-e2e=\"LeadItemAdd\"]")).Click();
            Thread.Sleep(3000);

            XPathContainsText("span", "TestCategory1");

            GetGridCell(0, "selectionRowHeaderCol", "OffersSelectvizr").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"] span")).Click();
            GetGridCell(1, "selectionRowHeaderCol", "OffersSelectvizr").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"] span")).Click();

            VerifyAreEqual("2", driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text, "selected items count");

            XPathContainsText("button", "Выбрать");

            VerifyIsFalse(driver.FindElements(By.CssSelector("[data-e2e=\"LeadNoProducts\"]")).Count > 0, "products added");
            VerifyIsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"LeadAddItemsTable\"]")).Count > 0, "products table presents");

            VerifyIsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"LeadItemArtNoName\"]"))[0].Text.Contains("TestProduct1"), "product 1 added");
            VerifyIsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"LeadItemArtNoName\"]"))[1].Text.Contains("TestProduct2"), "product 2 added");

            VerifyIsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"LeadItemPrice\"]"))[0].Text.Contains("1"), "product 1 price added");
            VerifyIsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"LeadItemPrice\"]"))[1].Text.Contains("2"), "product 2 price added");

            driver.FindElement(By.CssSelector("[data-e2e=\"LeadAdd\"]")).Click();
            Thread.Sleep(4000);

            //check admin lead details
            GoToAdmin("leads");

            GetGridFilterTab(0, "WithProductLastName");

            VerifyAreEqual("WithProductLastName WithProductFirstName WithProductPatronymic", GetGridCell(0, "FullName").Text, "lead added full name");
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(4000);

            VerifyIsFalse(driver.FindElement(By.CssSelector("[grid-unique-id=\"gridLeadItems\"]")).Text.Contains("Выберите товары"), "lead details products added");

            VerifyIsTrue(GetGridCell(0, "Name", "LeadItems").Text.Contains("TestProduct1"), "product 1 grid");
            VerifyIsTrue(GetGridCell(1, "Name", "LeadItems").Text.Contains("TestProduct2"), "product 2 grid");

            VerifyAreEqual("1", GetGridCell(0, "Price", "LeadItems").FindElement(By.TagName("input")).GetAttribute("value"), "product 1 Price grid");
            VerifyAreEqual("2", GetGridCell(1, "Price", "LeadItems").FindElement(By.TagName("input")).GetAttribute("value"), "product 2 Price grid");

            VerifyAreEqual("1", GetGridCell(0, "Amount", "LeadItems").FindElement(By.TagName("input")).GetAttribute("value"), "product 1 Amount grid");
            VerifyAreEqual("1", GetGridCell(1, "Amount", "LeadItems").FindElement(By.TagName("input")).GetAttribute("value"), "product 2 Amount grid");

           // VerifyAreEqual("в наличии", GetGridCell(0, "Available", "LeadItems").Text, "product 1 Available grid");
           // VerifyAreEqual("в наличии", GetGridCell(1, "Available", "LeadItems").Text, "product 2 Available grid");

            VerifyAreEqual("1 руб.", GetGridCell(0, "Cost", "LeadItems").Text, "product 1 Cost grid");
            VerifyAreEqual("2 руб.", GetGridCell(1, "Cost", "LeadItems").Text, "product 2 Cost grid");
            
            VerifyFinally(testname);
        }

        [Test]
        public void LeadAddDeleteItems()
        {
            testname = "LeadAddDeleteItems";
            VerifyBegin(testname);

            GoToAdmin("leads");

            GetButton(eButtonType.Add).Click();
            WaitForElem(By.CssSelector(".modal-content"));
            Thread.Sleep(2000);

            driver.FindElement(By.CssSelector("[data-e2e=\"LeadLastName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadLastName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadLastName\"]")).SendKeys("LastNameDelItem");

            driver.FindElement(By.CssSelector("[data-e2e=\"LeadFirstName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadFirstName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadFirstName\"]")).SendKeys("FirstNameDelItem");

            driver.FindElement(By.CssSelector("[data-e2e=\"LeadPatronymic\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadPatronymic\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadPatronymic\"]")).SendKeys("PatronymicDelItem");

            driver.FindElement(By.CssSelector("[data-e2e=\"LeadPhoneNum\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadPhoneNum\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadPhoneNum\"]")).SendKeys("+71237712143");

            driver.FindElement(By.CssSelector("[data-e2e=\"LeadEmail\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadEmail\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadEmail\"]")).SendKeys("mailtqweest123@mail.ru");

            ScrollTo(By.CssSelector("[data-e2e=\"LeadNoProducts\"]"));

            driver.FindElement(By.CssSelector("[data-e2e=\"LeadItemAdd\"]")).Click();
            Thread.Sleep(3000);

            XPathContainsText("span", "TestCategory1");

            GetGridCell(0, "selectionRowHeaderCol", "OffersSelectvizr").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"] span")).Click();
            
            XPathContainsText("button", "Выбрать");

            VerifyIsFalse(driver.FindElements(By.CssSelector("[data-e2e=\"LeadNoProducts\"]")).Count > 0, "products added");
            VerifyIsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"LeadAddItemsTable\"]")).Count > 0, "products table presents");

            driver.FindElement(By.CssSelector("[data-e2e=\"LeadItemRemove\"]")).Click();
            Thread.Sleep(2000);

            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"LeadNoProducts\"]")).Text.Contains("Нет товаров"), "products deleted");
            VerifyIsFalse(driver.FindElements(By.CssSelector("[data-e2e=\"LeadAddItemsTable\"]")).Count > 0, "no table - products deleted");

            driver.FindElement(By.CssSelector("[data-e2e=\"LeadAdd\"]")).Click();
            Thread.Sleep(3000);

            //check admin
            GoToAdmin("leads");

            GetGridFilterTab(0, "LastNameDelItem");

            VerifyAreEqual("LastNameDelItem FirstNameDelItem PatronymicDelItem", GetGridCell(0, "FullName").Text, "lead added full name");
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(4000);

            VerifyIsTrue(driver.FindElement(By.CssSelector("[grid-unique-id=\"gridLeadItems\"]")).Text.Contains("Выберите товары"), "lead no products");

            VerifyFinally(testname);
        }

        [Test]
        public void LeadEditDeleteProducts()
        {
            testname = "LeadEditDeleteProducts";
            VerifyBegin(testname);

            GoToAdmin("leads/edit/10");

            VerifyIsTrue(GetGridCell(0, "Name", "LeadItems").Text.Contains("TestProduct10"), "pre check product lead");
            var attr = driver.FindElement(By.Id("Lead_Sum")).GetAttribute("readonly");
            VerifyIsTrue(attr.Equals("true"), "pre check sum not editable");

            GetGridCell(0, "_serviceColumn", "LeadItems").FindElement(By.TagName("a")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);

            GoToAdmin("leads/edit/10");

            VerifyIsTrue(driver.FindElement(By.CssSelector("[grid-unique-id=\"gridLeadItems\"]")).Text.Contains("Выберите товары"), "products deleted");
            isElementNotPresent(By.Id("Lead_Sum"), "readonly"); //можно редактировать вручную 

            VerifyFinally(testname);
        }

        [Test]
        public void LeadEditDeleteNotAllProducts()
        {
            testname = "LeadEditDeleteNotAllProducts";
            VerifyBegin(testname);

            GoToAdmin("leads/edit/1");

            VerifyIsTrue(GetGridCell(0, "Name", "LeadItems").Text.Contains("TestProduct1"), "pre check product 1 lead");
            VerifyIsTrue(GetGridCell(1, "Name", "LeadItems").Text.Contains("TestProduct2"), "pre check product 2 lead");
            var attr = driver.FindElement(By.Id("Lead_Sum")).GetAttribute("readonly");
            VerifyIsTrue(attr.Equals("true"), "pre check sum not editable");

            GetGridCell(0, "_serviceColumn", "LeadItems").FindElement(By.TagName("a")).Click();
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);

            GoToAdmin("leads/edit/1");

            VerifyIsTrue(GetGridCell(0, "Name", "LeadItems").Text.Contains("TestProduct2"), "product 2 not deleted");
            VerifyIsFalse(driver.FindElement(By.CssSelector("[grid-unique-id=\"gridLeadItems\"]")).Text.Contains("Выберите товары"), "products not all deleted");
            attr = driver.FindElement(By.Id("Lead_Sum")).GetAttribute("readonly");
            VerifyIsTrue(attr.Equals("true"), "sum not editable");

            VerifyFinally(testname);
        }

        [Test]
        public void LeadAddExistedCustomer()
        {
            testname = "LeadAddExistedCustomer";
            VerifyBegin(testname);

            GoToAdmin("leads");

            GetButton(eButtonType.Add).Click();
            WaitForElem(By.CssSelector(".modal-content"));
            Thread.Sleep(2000);

            VerifyIsFalse(driver.FindElement(By.CssSelector(".modal-content")).Text.Contains("Выбран покупатель"), "no customer added");

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

            VerifyIsTrue(driver.FindElement(By.CssSelector(".modal-content")).FindElement(By.CssSelector("[data-e2e=\"LeadCustomer\"]")).Text.Contains("FirstName LastName +7 495 800 200 01 mail@mail.com"), "customer info added");
            VerifyIsTrue(driver.FindElement(By.CssSelector(".modal-content")).Text.Contains("Выбран покупатель"), "customer added");

            //add products
            ScrollTo(By.CssSelector("[data-e2e=\"LeadNoProducts\"]"));

            driver.FindElement(By.CssSelector("[data-e2e=\"LeadItemAdd\"]")).Click();
            Thread.Sleep(3000);

            XPathContainsText("span", "TestCategory1");

            GetGridCell(0, "selectionRowHeaderCol", "OffersSelectvizr").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"] span")).Click();

            XPathContainsText("button", "Выбрать");

            driver.FindElement(By.CssSelector("[data-e2e=\"LeadAdd\"]")).Click();
            Thread.Sleep(4000);

            VerifyAreEqual("LastName", driver.FindElement(By.Id("Lead_Customer_LastName")).GetAttribute("value"), "lead customer first name");
            VerifyAreEqual("FirstName", driver.FindElement(By.Id("Lead_Customer_FirstName")).GetAttribute("value"), "lead customer last name");
            VerifyAreEqual("Patronymic", driver.FindElement(By.Id("Lead_Customer_Patronymic")).GetAttribute("value"), "lead customer patronymic");
            VerifyAreEqual("mail@mail.com", driver.FindElement(By.Id("Lead_Customer_EMail")).GetAttribute("value"), "lead customer email");
            VerifyAreEqual("+7 495 800 200 01", driver.FindElement(By.Id("Lead_Customer_Phone")).GetAttribute("value"), "lead customer phone");

            driver.FindElement(By.LinkText("Карточка клиента")).Click();
            Thread.Sleep(4000);

            Functions.OpenNewTab(driver, baseURL);
            VerifyIsTrue(driver.WindowHandles.Count.Equals(2), "count tabs");

            VerifyIsTrue(driver.Url.Contains("customers/edit"), "customer edit tab opened");

            //check customer edit
            VerifyAreEqual("LastName", driver.FindElement(By.Id("Customer_LastName")).GetAttribute("value"), "customer first name edit");
            VerifyAreEqual("FirstName", driver.FindElement(By.Id("Customer_FirstName")).GetAttribute("value"), "customer last name edit");
            VerifyAreEqual("Patronymic", driver.FindElement(By.Id("Customer_Patronymic")).GetAttribute("value"), "customer patronymic edit");
            VerifyAreEqual("mail@mail.com", driver.FindElement(By.Id("Customer_EMail")).GetAttribute("value"), "customer email edit");
            VerifyAreEqual("+7 495 800 200 01", driver.FindElement(By.Id("Customer_Phone")).GetAttribute("value"), "customer phone edit");

            //check customer edit lead grid
            VerifyAreEqual("Новый", GetGridCell(0, "DealStatusName", "Leads").Text, "customer edit grid lead deal status");
            VerifyAreEqual("LastName FirstName Patronymic", GetGridCell(0, "FullName", "Leads").Text, "customer edit grid lead full name");
            VerifyAreEqual("1", GetGridCell(0, "Sum", "Leads").Text, "customer edit grid lead sum");

            Functions.CloseTab(driver, baseURL);

            VerifyFinally(testname);
        }

        [Test]
        public void LeadAddCustomerDelete()
        {
            testname = "LeadAddCustomerDelete";
            VerifyBegin(testname);

            GoToAdmin("leads");

            GetButton(eButtonType.Add).Click();
            WaitForElem(By.CssSelector(".modal-content"));
            Thread.Sleep(2000);
            
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadLastName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadLastName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadLastName\"]")).SendKeys("Last");

            XPathContainsText("span", "LastName FirstName, mail@mail.com +7 495 800 200 01");
            Thread.Sleep(2000);

            XPathContainsText("h2", "Новый лид");

            VerifyAreEqual("LastName", driver.FindElement(By.CssSelector("[data-e2e=\"LeadLastName\"]")).GetAttribute("value"), "existing customer LastName");
            VerifyAreEqual("FirstName", driver.FindElement(By.CssSelector("[data-e2e=\"LeadFirstName\"]")).GetAttribute("value"), "existing customer FirstName");
            VerifyAreEqual("Patronymic", driver.FindElement(By.CssSelector("[data-e2e=\"LeadPatronymic\"]")).GetAttribute("value"), "existing customer Patronymic");
            VerifyAreEqual("+7 495 800 200 01", driver.FindElement(By.CssSelector("[data-e2e=\"LeadPhoneNum\"]")).GetAttribute("value"), "existing customer PhoneNum");
            VerifyAreEqual("mail@mail.com", driver.FindElement(By.CssSelector("[data-e2e=\"LeadEmail\"]")).GetAttribute("value"), "existing customer Email");

            VerifyIsTrue(driver.FindElement(By.CssSelector(".modal-content")).FindElement(By.CssSelector("[data-e2e=\"LeadCustomer\"]")).Text.Contains("FirstName LastName +7 495 800 200 01 mail@mail.com"), "customer added info");

            driver.FindElement(By.CssSelector("[data-e2e=\"LeadCustomerDelete\"]")).Click();
            Thread.Sleep(3000);

            //check delete customer
            VerifyAreEqual("", driver.FindElement(By.CssSelector("[data-e2e=\"LeadLastName\"]")).GetAttribute("value"), "existing customer LastName deleted");
            VerifyAreEqual("", driver.FindElement(By.CssSelector("[data-e2e=\"LeadFirstName\"]")).GetAttribute("value"), "existing customer FirstName deleted");
            VerifyAreEqual("", driver.FindElement(By.CssSelector("[data-e2e=\"LeadPatronymic\"]")).GetAttribute("value"), "existing customer Patronymic deleted");
            VerifyAreEqual("", driver.FindElement(By.CssSelector("[data-e2e=\"LeadPhoneNum\"]")).GetAttribute("value"), "existing customer PhoneNum deleted");
            VerifyAreEqual("", driver.FindElement(By.CssSelector("[data-e2e=\"LeadEmail\"]")).GetAttribute("value"), "existing customer Email deleted");

            VerifyFinally(testname);
        }
        
        [Test]
        public void LeadAddDiscountAddNumber()
        {
            testname = "LeadAddDiscountAddNumber";
            VerifyBegin(testname);

            GoToAdmin("leads");

            GetButton(eButtonType.Add).Click();
            WaitForElem(By.CssSelector(".modal-content"));
            Thread.Sleep(2000);

            driver.FindElement(By.CssSelector("[data-e2e=\"LeadLastName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadLastName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadLastName\"]")).SendKeys("LastNameAddDiscount");

            driver.FindElement(By.CssSelector("[data-e2e=\"LeadFirstName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadFirstName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadFirstName\"]")).SendKeys("FirstNameAddDiscount");

            driver.FindElement(By.CssSelector("[data-e2e=\"LeadPatronymic\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadPatronymic\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadPatronymic\"]")).SendKeys("PatronymicAddDiscount");

            driver.FindElement(By.CssSelector("[data-e2e=\"LeadPhoneNum\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadPhoneNum\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadPhoneNum\"]")).SendKeys("+71237712143");

            driver.FindElement(By.CssSelector("[data-e2e=\"LeadEmail\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadEmail\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadEmail\"]")).SendKeys("mailtqweesdiscountt123@mail.ru");

            ScrollTo(By.CssSelector("[data-e2e=\"LeadNoProducts\"]"));

            driver.FindElement(By.CssSelector("[data-e2e=\"LeadItemAdd\"]")).Click();
            Thread.Sleep(3000);

            driver.FindElement(By.Id("4")).FindElement(By.CssSelector(".jstree-icon.jstree-ocl")).Click();
            XPathContainsText("span", "TestCategory5");

            GetGridFilter().Click();
            GetGridFilter().SendKeys("TestProduct100");
            XPathContainsText("h2", "Выбор товара");

            VerifyAreEqual("TestProduct100", GetGridCell(0, "Name", "OffersSelectvizr").Text, "modal add product");

            GetGridCell(0, "selectionRowHeaderCol", "OffersSelectvizr").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"] span")).Click();

            XPathContainsText("button", "Выбрать");

            driver.FindElement(By.CssSelector("[data-e2e=\"LeadAdd\"]")).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("leads");

            GetGridFilterTab(0, "FirstNameAddDiscount");

            Assert.AreEqual("LastNameAddDiscount FirstNameAddDiscount PatronymicAddDiscount", GetGridCell(0, "FullName").Text);
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(4000);

            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"DiscountAddLink\"]")).Text.Contains("Добавить скидку"), "no discount text add");
            VerifyIsFalse(driver.FindElement(By.CssSelector("[data-e2e=\"DiscountAddLink\"]")).Text.Contains("Скидка"), "no discount text saved");

            driver.FindElement(By.LinkText("Добавить скидку")).Click();

            driver.FindElement(By.CssSelector("[data-e2e=\"LeadDiscountNumber\"]")).Click();

            driver.FindElement(By.Name("orderFormDiscount")).FindElement(By.TagName("input")).Click();
            driver.FindElement(By.Name("orderFormDiscount")).FindElement(By.TagName("input")).Clear();
            driver.FindElement(By.Name("orderFormDiscount")).FindElement(By.TagName("input")).SendKeys("80");

            driver.FindElement(By.CssSelector("[data-e2e=\"LeadDiscountAdd\"]")).Click();

            Refresh();

            VerifyAreEqual("- 80 руб.", driver.FindElement(By.CssSelector("[data-e2e=\"LeadDiscount\"]")).Text, "lead discount added saved");
            VerifyAreEqual("20 руб.", driver.FindElement(By.CssSelector("[data-e2e=\"LeadDiscountSummary\"]")).Text, "lead summary");

            VerifyIsFalse(driver.FindElement(By.CssSelector("[data-e2e=\"DiscountAddLink\"]")).Text.Contains("Добавить скидку"), "lead discount added text add");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"DiscountAddLink\"]")).Text.Contains("Скидка"), "lead discount added text save");

            VerifyFinally(testname);
        }

        [Test]
        public void LeadEditDiscountAddPercent()
        {
            testname = "LeadEditDiscountAddPercent";
            VerifyBegin(testname);

            GoToAdmin("leads/edit/60");

            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"DiscountAddLink\"]")).Text.Contains("Добавить скидку"), "no discount text add");
            VerifyIsFalse(driver.FindElement(By.CssSelector("[data-e2e=\"DiscountAddLink\"]")).Text.Contains("Скидка"), "no discount text saved");

            driver.FindElement(By.LinkText("Добавить скидку")).Click();

            driver.FindElement(By.CssSelector("[data-e2e=\"LeadDiscountPercent\"]")).Click();

            driver.FindElement(By.Name("orderFormDiscount")).FindElement(By.TagName("input")).Click();
            driver.FindElement(By.Name("orderFormDiscount")).FindElement(By.TagName("input")).Clear();
            driver.FindElement(By.Name("orderFormDiscount")).FindElement(By.TagName("input")).SendKeys("50");

            driver.FindElement(By.CssSelector("[data-e2e=\"LeadDiscountAdd\"]")).Click();

            Refresh();

            VerifyAreEqual("- 1 800 руб.", driver.FindElement(By.CssSelector("[data-e2e=\"LeadDiscount\"]")).Text, "lead discount added saved");
            VerifyAreEqual("(50%)", driver.FindElement(By.CssSelector("[data-e2e=\"LeadDiscountPercentAdded\"]")).Text, "lead discount percent added");
            VerifyAreEqual("1 800 руб.", driver.FindElement(By.CssSelector("[data-e2e=\"LeadDiscountSummary\"]")).Text, "lead summary");

            VerifyIsFalse(driver.FindElement(By.CssSelector("[data-e2e=\"DiscountAddLink\"]")).Text.Contains("Добавить скидку"), "lead discount added text add");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"DiscountAddLink\"]")).Text.Contains("Скидка"), "lead discount added text save");

            VerifyFinally(testname);
        }

        [Test]
        public void LeadEditProductsAdd()
        {
            testname = "LeadEditProductsAdd";
            VerifyBegin(testname);

            GoToAdmin("leads/edit/70");

            VerifyIsTrue(GetGridCell(0, "Name", "LeadItems").Text.Contains("TestProduct70"), "pre check lead product grid name");
            VerifyAreEqual("70", GetGridCell(0, "Amount", "LeadItems").FindElement(By.TagName("input")).GetAttribute("value"), "pre check lead product grid amount");
            VerifyAreEqual("4900", driver.FindElement(By.Id("Lead_Sum")).GetAttribute("value"), "pre check lead sum");

            driver.FindElement(By.CssSelector("[ data-e2e=\"LeadProductAdd\"]")).Click();
            Thread.Sleep(3000);

            XPathContainsText("span", "TestCategory3");
            GetGridCell(0, "selectionRowHeaderCol", "OffersSelectvizr").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"] span")).Click();
            XPathContainsText("button", "Выбрать");

            GoToAdmin("leads/edit/70");

            VerifyIsTrue(GetGridCell(1, "Name", "LeadItems").Text.Contains("TestProduct41"), "lead added product grid name");
            VerifyAreEqual("1", GetGridCell(1, "Amount", "LeadItems").FindElement(By.TagName("input")).GetAttribute("value"), "lead added product grid amount");
            VerifyAreEqual("4941", driver.FindElement(By.Id("Lead_Sum")).GetAttribute("value"), "lead sum after adding product");

            var attr = driver.FindElement(By.Id("Lead_Sum")).GetAttribute("readonly");
            Assert.IsTrue(attr.Equals("true"));  //нельзя редактировать вручную 

            VerifyFinally(testname);
        }

        [Test]
        public void LeadEditManager()
        {
            testname = "LeadEditManager";
            VerifyBegin(testname);

            GoToAdmin("leads/edit/21");

            IWebElement selectElem1 = driver.FindElement(By.Id("Lead_ManagerId"));
            SelectElement select1 = new SelectElement(selectElem1);
            VerifyIsTrue(select1.SelectedOption.Text.Contains("test testov"), "pre check lead manager");
            
            (new SelectElement(driver.FindElement(By.Id("Lead_ManagerId")))).SelectByText("Elena El");

            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"btnSave\"]")).Click();
            Thread.Sleep(3000);

            GoToAdmin("leads/edit/21");

            IWebElement selectElem2 = driver.FindElement(By.Id("Lead_ManagerId"));
            SelectElement select2 = new SelectElement(selectElem2);
            VerifyIsTrue(select2.SelectedOption.Text.Contains("Elena El"), "lead manager edited");

            VerifyFinally(testname);
        }


        [Test]
        public void LeadEditProductsCount()
        {
            testname = "LeadEditProductsCount";
            VerifyBegin(testname);

            GoToAdmin("leads");

            GetButton(eButtonType.Add).Click();
            WaitForElem(By.CssSelector(".modal-content"));
            Thread.Sleep(2000);

            driver.FindElement(By.CssSelector("[data-e2e=\"LeadFirstName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadFirstName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadFirstName\"]")).SendKeys("LeadEditProductsCount");

            driver.FindElement(By.CssSelector("[data-e2e=\"LeadPhoneNum\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadPhoneNum\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"LeadPhoneNum\"]")).SendKeys("+71223423423431212923");

            driver.FindElement(By.CssSelector("[data-e2e=\"LeadItemAdd\"]")).Click();
            Thread.Sleep(3000);

            XPathContainsText("span", "TestCategory1");

            GetGridCell(0, "selectionRowHeaderCol", "OffersSelectvizr").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"] span")).Click();

            XPathContainsText("button", "Выбрать");

            driver.FindElement(By.CssSelector("[data-e2e=\"LeadAdd\"]")).Click();
            Thread.Sleep(3000);

            GoToAdmin("leads");

            GetGridFilterTab(0, "LeadEditProductsCount");
            
            VerifyAreEqual("LeadEditProductsCount", GetGridCell(0, "FullName").Text, "lead added full name");

            //check admin lead details
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(4000);

            VerifyIsTrue(GetGridCell(0, "Name", "LeadItems").Text.Contains("TestProduct1"), "pre check lead product grid name");
            VerifyAreEqual("1", GetGridCell(0, "Amount", "LeadItems").FindElement(By.TagName("input")).GetAttribute("value"), "pre check lead product grid amount");
            //VerifyAreEqual("в наличии", GetGridCell(0, "Available", "LeadItems").Text, "pre check lead product Available grid");

            var attrAvailable = driver.FindElement(By.Id("Lead_Sum")).GetAttribute("readonly");
            Assert.IsTrue(attrAvailable.Equals("true"));  //нельзя редактировать вручную 

            //check not available
            //GetGridCell(0, "Amount", "LeadItems").FindElement(By.TagName("input")).Click();
            //GetGridCell(0, "Amount", "LeadItems").FindElement(By.TagName("input")).Clear();
            //GetGridCell(0, "Amount", "LeadItems").FindElement(By.TagName("input")).SendKeys("2");
            //DropFocus("h1");

            //Refresh();

            //VerifyAreEqual("доступно 1", GetGridCell(0, "Available", "LeadItems").Text, "lead product not Available grid");
            //VerifyAreEqual("2", GetGridCell(0, "Amount", "LeadItems").FindElement(By.TagName("input")).GetAttribute("value"), "lead product not Available amount grid");

            //var attrNotAvailable = driver.FindElement(By.Id("Lead_Sum")).GetAttribute("readonly");
            //Assert.IsTrue(attrNotAvailable.Equals("true"));  //нельзя редактировать вручную 

            //check products count 0
            GetGridCell(0, "Amount", "LeadItems").FindElement(By.TagName("input")).Click();
            GetGridCell(0, "Amount", "LeadItems").FindElement(By.TagName("input")).Clear();
            GetGridCell(0, "Amount", "LeadItems").FindElement(By.TagName("input")).SendKeys("0");
            DropFocus("h1");
            Thread.Sleep(3000);

            //VerifyAreEqual("в наличии", GetGridCell(0, "Available", "LeadItems").Text, "lead product available count 0 grid");
            VerifyAreEqual("0", GetGridCell(0, "Amount", "LeadItems").FindElement(By.TagName("input")).GetAttribute("value"), "lead product available count 0 amount grid");
            isElementNotPresent(By.Id("Lead_Sum"), "readonly"); //можно редактировать вручную 

            Refresh();

            VerifyAreEqual("0", GetGridCell(0, "Amount", "LeadItems").FindElement(By.TagName("input")).GetAttribute("value"), "lead product available count 0 amount grid after refresh");
            isElementNotPresent(By.Id("Lead_Sum"), "readonly");

            VerifyFinally(testname);
        }


        [Test]
        public void LeadEditproductsAddBySearch()
        {
            testname = "LeadEditproductsAddBySearch";
            VerifyBegin(testname);

            GoToAdmin("leads/edit/25");

            driver.FindElement(By.CssSelector("[ data-e2e=\"LeadProductAdd\"]")).Click();
            Thread.Sleep(2000);

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("TestProduct55");

            XPathContainsText("h2", "Выбор товара");

            VerifyAreEqual("TestProduct55", GetGridCell(0, "Name", "OffersSelectvizr").Text, "modal add product by search");

            GetGridCell(0, "selectionRowHeaderCol", "OffersSelectvizr").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"] span")).Click();
            XPathContainsText("button", "Выбрать");

            GoToAdmin("leads/edit/25");

            VerifyIsTrue(GetGridCell(1, "Name", "LeadItems").Text.Contains("TestProduct55"), "lead product grid added by search");

            var attrAvailable = driver.FindElement(By.Id("Lead_Sum")).GetAttribute("readonly");
            Assert.IsTrue(attrAvailable.Equals("true"));  //нельзя редактировать вручную 


            VerifyFinally(testname);
        }
    }
}
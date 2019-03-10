using System;
using NUnit.Framework;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.SeleniumTest.Admin.Settings.BonusSystem.SmsTemplate.AddEdit
{
    [TestFixture]
    public class BonusSystemSmsTemplateAddEdit : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog | ClearType.Customers | ClearType.Bonuses);
            InitializeService.LoadData(

             "Data\\Admin\\Settings\\BonusSystem\\SmsTemplateAddEdit\\Catalog.Product.csv",
           "Data\\Admin\\Settings\\BonusSystem\\SmsTemplateAddEdit\\Catalog.Offer.csv",
           "Data\\Admin\\Settings\\BonusSystem\\SmsTemplateAddEdit\\Catalog.Category.csv",
          "Data\\Admin\\Settings\\BonusSystem\\SmsTemplateAddEdit\\Catalog.ProductCategories.csv",
               "Data\\Admin\\Settings\\BonusSystem\\SmsTemplateAddEdit\\Bonus.Grade.csv",
           "Data\\Admin\\Settings\\BonusSystem\\SmsTemplateAddEdit\\Bonus.Card.csv",
             "Data\\Admin\\Settings\\BonusSystem\\SmsTemplateAddEdit\\Bonus.SmsTemplate.csv",
           "Data\\Admin\\Settings\\BonusSystem\\SmsTemplateAddEdit\\Customers.CustomerGroup.csv",
           "Data\\Admin\\Settings\\BonusSystem\\SmsTemplateAddEdit\\Customers.Customer.csv"


                );
            Init();
        }

        [Test]
        public void SmsTemplateAdd()
        {
            testname = "SmsTemplateAdd";
            VerifyBegin(testname);

            GoToAdmin("smstemplates");

            driver.FindElement(By.CssSelector("[data-e2e=\"Add\"]")).Click();
            WaitForElem(By.CssSelector(".modal-content"));

            VerifyIsTrue(driver.FindElement(By.TagName("h2")).Text.Contains("Добавить шаблон"), "pop up header");

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"smsType\"]")))).SelectByText("При смене грейда");

            driver.FindElement(By.CssSelector("[data-e2e=\"SmsBody\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"SmsBody\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"SmsBody\"]")).SendKeys("New Sms Template Added");

            WaitForElemEnabled(By.XPath("//span[contains(text(), 'Добавить')]"));
            XPathContainsText("span", "Добавить");
            
            VerifyAreEqual("При смене грейда", GetGridCell(1, "SmsType").FindElement(By.TagName("a")).Text, "sms type added");
            VerifyAreEqual("New Sms Template Added", GetGridCell(1, "SmsBody").Text, "sms body added");
            
            VerifyFinally(testname);
        }


        [Test]
        public void SmsTemplateEdit()
        {
            testname = "SmsTemplateEdit";
            VerifyBegin(testname);

            GoToAdmin("smstemplates");

            GetGridCell(0, "_serviceColumn").FindElement(By.TagName("ui-modal-trigger")).Click();
            WaitForElem(By.CssSelector(".modal-content"));
            
            driver.FindElement(By.CssSelector("[data-e2e=\"SmsBody\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"SmsBody\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"SmsBody\"]")).SendKeys("Edited text");

            WaitForElemEnabled(By.XPath("//span[contains(text(), 'Сохранить')]"));
            XPathContainsText("span", "Сохранить");

            VerifyAreEqual("При пополнении бонусов", GetGridCell(0, "SmsType").FindElement(By.TagName("a")).Text, "sms type not edited");
            VerifyAreEqual("Edited text", GetGridCell(0, "SmsBody").Text, "sms body edited");

            VerifyFinally(testname);
        }

    }
}

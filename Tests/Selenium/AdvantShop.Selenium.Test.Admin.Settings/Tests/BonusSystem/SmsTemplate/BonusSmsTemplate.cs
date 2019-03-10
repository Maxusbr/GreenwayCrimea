using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Admin.Settings.BonusSystem.SmsTemplate
{
    [TestFixture]
    public class BonusSystemSmsTemplate : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog | ClearType.Customers | ClearType.Bonuses);
            InitializeService.LoadData(

             "Data\\Admin\\Settings\\BonusSystem\\SmsTemplate\\Catalog.Product.csv",
           "Data\\Admin\\Settings\\BonusSystem\\SmsTemplate\\Catalog.Offer.csv",
           "Data\\Admin\\Settings\\BonusSystem\\SmsTemplate\\Catalog.Category.csv",
          "Data\\Admin\\Settings\\BonusSystem\\SmsTemplate\\Catalog.ProductCategories.csv",
               "Data\\Admin\\Settings\\BonusSystem\\SmsTemplate\\Bonus.Grade.csv",
           "Data\\Admin\\Settings\\BonusSystem\\SmsTemplate\\Bonus.Card.csv",
             "Data\\Admin\\Settings\\BonusSystem\\SmsTemplate\\Bonus.SmsTemplate.csv",
           "Data\\Admin\\Settings\\BonusSystem\\SmsTemplate\\Customers.CustomerGroup.csv",
           "Data\\Admin\\Settings\\BonusSystem\\SmsTemplate\\Customers.Customer.csv"


                );
            Init();

            GoToAdmin("smstemplates");
        }



        [Test]
        public void SmsTemplateGrid()
        {
            testname = "SmsTemplateGrid";
            VerifyBegin(testname);
            
            VerifyAreEqual("При продаже", GetGridCell(0, "SmsType").FindElement(By.TagName("a")).Text, "sms type line 1");
            VerifyAreEqual("Sms body 1", GetGridCell(0, "SmsBody").Text, "sms body line 1");

            VerifyAreEqual("При пополнении бонусов", GetGridCell(1, "SmsType").FindElement(By.TagName("a")).Text, "sms type line 2");
            VerifyAreEqual("Sms body 2", GetGridCell(1, "SmsBody").Text, "sms body line 2");

            VerifyAreEqual("При списании бонусов", GetGridCell(2, "SmsType").FindElement(By.TagName("a")).Text, "sms type line 3");
            VerifyAreEqual("Sms body 3", GetGridCell(2, "SmsBody").Text, "sms body line 3");

            VerifyAreEqual("При смене грейда", GetGridCell(3, "SmsType").FindElement(By.TagName("a")).Text, "sms type line 4");
            VerifyAreEqual("Sms body 4", GetGridCell(3, "SmsBody").Text, "sms body line 4");

            VerifyAreEqual("Отмена продажи", GetGridCell(4, "SmsType").FindElement(By.TagName("a")).Text, "sms type line 5");
            VerifyAreEqual("Sms body 5", GetGridCell(4, "SmsBody").Text, "sms body line 5");

            VerifyAreEqual("Аннулирование баллов", GetGridCell(5, "SmsType").FindElement(By.TagName("a")).Text, "sms type line 6");
            VerifyAreEqual("Sms body 6", GetGridCell(5, "SmsBody").Text, "sms body line 6");

            VerifyFinally(testname);
        }

        [Test]
        public void SmsTemplatezSelectDelete()
        {
            testname = "SmsTemplatezSelectDelete";
            VerifyBegin(testname);

            //check delete cancel 
            GetGridCell(0, "_serviceColumn").FindElement(By.TagName("ui-grid-custom-delete")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-cancel")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("При продаже", GetGridCell(0, "SmsType").FindElement(By.TagName("a")).Text, "1 grid canсel delete");

            //check delete
            GetGridCell(0, "_serviceColumn").FindElement(By.TagName("ui-grid-custom-delete")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);
            VerifyAreNotEqual("При продаже", GetGridCell(0, "SmsType").FindElement(By.TagName("a")).Text, "1 grid delete");

            //check select 
            GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            GetGridCell(1, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            VerifyIsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected 2 grid");
            VerifyIsTrue(GetGridCell(1, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected 3 grid");
            VerifyAreEqual("2", driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text, "count selected");

            //check delete selected items
            Functions.GridDropdownDelete(driver, baseURL);
            VerifyAreEqual("При смене грейда", GetGridCell(0, "SmsType").FindElement(By.TagName("a")).Text, "selected 2 grid delete");
            VerifyAreEqual("Отмена продажи", GetGridCell(1, "SmsType").FindElement(By.TagName("a")).Text, "selected 3 grid delete");
            
            //check select all
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Thread.Sleep(1000);
            VerifyAreEqual("3", driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text, "count all selected after deleting");

            //check deselect all 
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDeselectAllFn\"]")).Click();
            Thread.Sleep(1000);
            VerifyIsTrue(!GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "deselect all 1 grid");
            VerifyIsTrue(!GetGridCell(2, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "deselect all 3 grid");

            //check delete all
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Functions.GridDropdownDelete(driver, baseURL);

            GoToAdmin("smstemplates");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "delete all");

            VerifyFinally(testname);
        }

    }
}

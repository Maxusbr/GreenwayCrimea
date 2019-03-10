using System;
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
    public class CRMLeadTabDealStatusTest : BaseSeleniumTest
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
               "data\\Admin\\CRM\\Lead\\Customers.CustomerField.csv",
               "data\\Admin\\CRM\\Lead\\Customers.CustomerFieldValue.csv",
               "data\\Admin\\CRM\\Lead\\Customers.CustomerFieldValuesMap.csv",
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
            GoToAdmin("leads");
        }


         

        [Test]
        public void aTabNew()
        {
            testname = "LeadTabNew";
            VerifyBegin(testname);

            TabClick("Новый", "Еще", "Сделка отклонена");

            VerifyIsTrue(driver.Url.Contains("leads"), "lead page");
            VerifyAreEqual("Найдено записей: 31", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count leads");

            VerifyFinally(testname);
        }

        [Test]
        public void CallToClient()
        {
            testname = "LeadTabCallToClient";
            VerifyBegin(testname);

            TabClick("Созвон с клиентом", "Еще", "Сделка отклонена");

            VerifyIsTrue(driver.Url.Contains("leads"), "lead page");
            VerifyAreEqual("Найдено записей: 21", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count leads");

            VerifyFinally(testname);
        }

        [Test]
        public void PlanToClient()
        {
            testname = "LeadTabPlanToClient";
            VerifyBegin(testname);

            TabClick("Выставление КП", "Еще", "Сделка отклонена");

            VerifyIsTrue(driver.Url.Contains("leads"), "lead page");
            VerifyAreEqual("Найдено записей: 26", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count leads");

            VerifyFinally(testname);
        }


        [Test]
        public void WaitClient()
        {
            testname = "LeadTabWaitClient";
            VerifyBegin(testname);

            TabClick("Ожидание решения клиента", "Еще", "Сделка отклонена");

            VerifyIsTrue(driver.Url.Contains("leads"), "lead page");
            VerifyAreEqual("Найдено записей: 14", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count leads");

            VerifyFinally(testname);
        }

        [Test]
        public void SuccessDeal()
        {
            testname = "LeadTabSuccessDeal";
            VerifyBegin(testname);

            TabClick("Сделка заключена", "Еще", "Сделка отклонена");

            VerifyIsTrue(driver.Url.Contains("leads"), "lead page");
            VerifyAreEqual("Найдено записей: 10", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count leads");

            VerifyFinally(testname);
        }

        [Test]
        public void zDealCanceled()
        {
            testname = "LeadTabDealCanceled";
            VerifyBegin(testname);

            TabClick("Сделка отклонена", "Еще", "Сделка отклонена");

            VerifyIsTrue(driver.Url.Contains("leads"), "lead page");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "count leads");

            VerifyFinally(testname);
        }

    }
}
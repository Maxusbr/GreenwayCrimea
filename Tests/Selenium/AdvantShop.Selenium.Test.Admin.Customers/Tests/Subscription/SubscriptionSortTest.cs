using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using AdvantShop.Selenium.Core.Infrastructure;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;

namespace AdvantShop.Selenium.Test.Admin.Customers.Tests.Subscription
{
    [TestFixture]
    public class SubscriptionSortTest : BaseSeleniumTest
    {

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Customers);
            InitializeService.LoadData(
               "data\\Admin\\Subscription\\Customers.CustomerGroup.csv",
                  "data\\Admin\\Subscription\\Customers.Country.csv",
            "data\\Admin\\Subscription\\Customers.Region.csv",
            "data\\Admin\\Subscription\\Customers.City.csv",
            "data\\Admin\\Subscription\\Customers.Customer.csv",
            "data\\Admin\\Subscription\\Customers.Contact.csv",
           "data\\Admin\\Subscription\\Customers.Departments.csv",
           "data\\Admin\\Subscription\\Customers.Managers.csv",
               "data\\Admin\\Subscription\\Customers.CustomerField.csv",
               "data\\Admin\\Subscription\\Customers.CustomerFieldValue.csv",
               "data\\Admin\\Subscription\\Customers.CustomerFieldValuesMap.csv",
               "data\\Admin\\Subscription\\Customers.Subscription.csv"

           );

            Init();
        }
        [Test]
        public void SubscriptionSortByEmail()
        {
            GoToAdmin("subscription");
            testname = "SubscriptionSortByEmail";
            VerifyBegin(testname);

            GetGridCell(-1, "Email").Click();
            WaitForAjax();

            VerifyAreEqual("testmail1@test.ru", GetGridCell(0, "Email").Text, "subscription 1 Email asc");
            VerifyAreEqual("testmail18@test.ru", GetGridCell(9, "Email").Text, "subscription 10 Email asc");

            GetGridCell(-1, "Email").Click();
            WaitForAjax();

            VerifyAreEqual("testmail9@test.ru", GetGridCell(0, "Email").Text, "subscription 1 Email desc");
            VerifyAreEqual("testmail20@test.ru", GetGridCell(9, "Email").Text, "subscription 10 Email desc");

            VerifyFinally(testname);
        }
        [Test]
        public void SubscriptionSortByEnabled()
        {
            GoToAdmin("subscription");
            testname = "SubscriptionSortByEnabled";
            VerifyBegin(testname);

            GetGridCell(-1, "Enabled").Click();
            WaitForAjax();

            VerifyAreEqual("testmail11@test.ru", GetGridCell(0, "Email").Text, "subscription 1 Enabled asc");
            VerifyAreEqual("testmail20@test.ru", GetGridCell(9, "Email").Text, "subscription 10 Enabled asc");

            GetGridCell(-1, "Enabled").Click();
            WaitForAjax();

            VerifyAreEqual("testmail1@test.ru", GetGridCell(0, "Email").Text, "subscription 1 Enabled desc");
            VerifyAreEqual("testmail10@test.ru", GetGridCell(9, "Email").Text, "subscription 10 Enabled desc");

            VerifyFinally(testname);
        }
        [Test]
        public void SubscriptionSortBySubDate()
        {
            GoToAdmin("subscription");
            testname = "SubscriptionSortBySubDate";
            VerifyBegin(testname);

            GetGridCell(-1, "SubscribeDateStr").Click();
            WaitForAjax();

            VerifyAreEqual("testmail22@test.ru", GetGridCell(0, "Email").Text, "subscription 1 SubDate asc");
            VerifyAreEqual("testmail13@test.ru", GetGridCell(9, "Email").Text, "subscription 10 SubDate asc");

            GetGridCell(-1, "SubscribeDateStr").Click();
            WaitForAjax();

            VerifyAreEqual("testmail1@test.ru", GetGridCell(0, "Email").Text, "subscription 1 SubDate desc");
            VerifyAreEqual("testmail10@test.ru", GetGridCell(9, "Email").Text, "subscription 10 SubDate desc");

            VerifyFinally(testname);
        }
        [Test]
        public void SubscriptionSortByUnsubDate()
        {
            GoToAdmin("subscription");
            testname = "SubscriptionSortByUnsubDate";
            VerifyBegin(testname);

            GetGridCell(-1, "UnsubscribeDateStr").Click();
            WaitForAjax();

            VerifyAreEqual("testmail1@test.ru", GetGridCell(0, "Email").Text, "subscription 1 UnsubDate asc");
            VerifyAreEqual("testmail10@test.ru", GetGridCell(9, "Email").Text, "subscription 10 UnsubDate asc");

            GetGridCell(-1, "UnsubscribeDateStr").Click();
            WaitForAjax();

            VerifyAreEqual("testmail22@test.ru", GetGridCell(0, "Email").Text, "subscription 1 UnsubDate desc");
            VerifyAreEqual("testmail13@test.ru", GetGridCell(9, "Email").Text, "subscription 10 UnsubDate desc");

            VerifyFinally(testname);
        }
    }
    }

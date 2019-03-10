using System;
using NUnit.Framework;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Admin.Settings.CustomerFields.Values
{
    [TestFixture]
    public class SettingsCustomerFieldValueSortTest : BaseSeleniumTest
    {

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Customers);
            InitializeService.LoadData(
            "data\\Admin\\Settings\\CustomerFieldValues\\Customers.Customer.csv",
           "data\\Admin\\Settings\\CustomerFieldValues\\Customers.CustomerGroup.csv",
               "data\\Admin\\Settings\\CustomerFieldValues\\Customers.CustomerField.csv",
               "data\\Admin\\Settings\\CustomerFieldValues\\Customers.CustomerFieldValue.csv"

           );

            Init();

            GoToAdmin("settingscustomers#?tab=customerFields");

            GetGridCell(0, "HasValues", "CustomerFields").Click();
        }

         


        [Test]
        public void CustomerFieldValueSortName()
        {
            testname = "CustomerFieldValueSortName";
            VerifyBegin(testname);
            
            GetGridCell(-1, "Value", "CustomerFieldValues").Click();
            WaitForAjax();
            VerifyAreEqual("Value 1", GetGridCell(0, "Value", "CustomerFieldValues").Text, "sort Name 1 asc");
            VerifyAreEqual("Value 107", GetGridCell(9, "Value", "CustomerFieldValues").Text, "sort Name 10 asc");

            GetGridCell(-1, "Value", "CustomerFieldValues").Click();
            WaitForAjax();
            VerifyAreEqual("Value 99", GetGridCell(0, "Value", "CustomerFieldValues").Text, "sort Name 1 desc");
            VerifyAreEqual("Value 90", GetGridCell(9, "Value", "CustomerFieldValues").Text, "sort Name 10 desc");

            VerifyFinally(testname);
        }
        
        [Test]
        public void CustomerFieldValueSortSortOrder()
        {
            testname = "CustomerFieldValueSortSortOrder";
            VerifyBegin(testname);
            
            GetGridCell(-1, "SortOrder", "CustomerFieldValues").Click();
            WaitForAjax();
            VerifyAreEqual("1", GetGridCell(0, "SortOrder", "CustomerFieldValues").FindElement(By.TagName("input")).GetAttribute("value"), "sort SortOrder 1 asc");
            VerifyAreEqual("10", GetGridCell(9, "SortOrder", "CustomerFieldValues").FindElement(By.TagName("input")).GetAttribute("value"), "sort SortOrder 10 asc");

            GetGridCell(-1, "SortOrder", "CustomerFieldValues").Click();
            WaitForAjax();
            VerifyAreEqual("140", GetGridCell(0, "SortOrder", "CustomerFieldValues").FindElement(By.TagName("input")).GetAttribute("value"), "sort SortOrder 1 desc");
            VerifyAreEqual("131", GetGridCell(9, "SortOrder", "CustomerFieldValues").FindElement(By.TagName("input")).GetAttribute("value"), "sort SortOrder 10 desc");

            VerifyFinally(testname);
        }
    }
}
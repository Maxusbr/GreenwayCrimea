using System;
using NUnit.Framework;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Admin.Settings.BonusSystem.SmsHistory
{
    [TestFixture]
    public class BonusSmsLogSort : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog | ClearType.Customers | ClearType.Bonuses);
            InitializeService.LoadData(

            "Data\\Admin\\Settings\\BonusSystem\\SmsHistory\\Catalog.Product.csv",
            "Data\\Admin\\Settings\\BonusSystem\\SmsHistory\\Catalog.Offer.csv",
            "Data\\Admin\\Settings\\BonusSystem\\SmsHistory\\Catalog.Category.csv",
            "Data\\Admin\\Settings\\BonusSystem\\SmsHistory\\Catalog.ProductCategories.csv",
            "Data\\Admin\\Settings\\BonusSystem\\SmsHistory\\Bonus.Grade.csv",
            "Data\\Admin\\Settings\\BonusSystem\\SmsHistory\\Bonus.Card.csv",
            "Data\\Admin\\Settings\\BonusSystem\\SmsHistory\\Bonus.SmsTemplate.csv",
            "Data\\Admin\\Settings\\BonusSystem\\SmsHistory\\Bonus.SmsLog.csv",
            "Data\\Admin\\Settings\\BonusSystem\\SmsHistory\\Customers.CustomerGroup.csv",
            "Data\\Admin\\Settings\\BonusSystem\\SmsHistory\\Customers.Customer.csv"
                );
            Init();

            GoToAdmin("smstemplates/smslog");
        }

        [Test]
        public void ByStatus()
        {
            testname = "LogSortByStatus";
            VerifyBegin(testname);

            GetGridCell(-1, "State", "log").Click();
            WaitForAjax();
            VerifyAreEqual("Get", GetGridCell(0, "State", "log").Text, "sort by State asc 1");
            VerifyAreEqual("Get", GetGridCell(9, "State", "log").Text, "sort by State asc 10");

            GetGridCell(-1, "State", "log").Click();
            WaitForAjax();
            VerifyAreEqual("Send", GetGridCell(0, "State", "log").Text, "sort by State desc 1");
            VerifyAreEqual("Send", GetGridCell(9, "State", "log").Text, "sort by State desc 10");

            VerifyFinally(testname);
        }
        

        [Test]
        public void ByCreatedDate()
        {
            testname = "LogSortByCreatedDate";
            VerifyBegin(testname);

            GetGridCell(-1, "Created_Str", "log").Click();
            WaitForAjax();
            VerifyAreEqual("01.03.2017 03:40:00", GetGridCell(0, "Created_Str", "log").Text, "sort by created date asc 1");
            VerifyAreEqual("10.03.2017 03:40:00", GetGridCell(9, "Created_Str", "log").Text, "sort by created date asc 10");

            GetGridCell(-1, "Created_Str", "log").Click();
            WaitForAjax();
            VerifyAreEqual("20.04.2017 03:40:00", GetGridCell(0, "Created_Str", "log").Text, "sort by created date desc 1");
            VerifyAreEqual("11.04.2017 03:40:00", GetGridCell(9, "Created_Str", "log").Text, "sort by created date desc 10");

            VerifyFinally(testname);
        }  
    }
}

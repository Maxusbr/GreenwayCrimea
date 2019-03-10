//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Text;
using AdvantShop.Core;
using AdvantShop.Core.Services.Localization;

namespace AdvantShop
{
    public class Demo
    {
        public static bool IsDemoEnabled
        {
            get { return ModeConfigService.IsModeEnabled(ModeConfigService.Modes.DemoMode); }
        }

        public static string GetRandomName()
        {
            var rnd = new Random();
            return GetRandomName(rnd);
        }

        private static string GetRandomName(Random rnd)
        {
            string[] strMassFirstName =
            {
                LocalizationService.GetResource("Core.Demo.RandomName"),
                LocalizationService.GetResource("Core.Demo.RandomName1"),
                LocalizationService.GetResource("Core.Demo.RandomName2"),
                LocalizationService.GetResource("Core.Demo.RandomName3"),
                LocalizationService.GetResource("Core.Demo.RandomName4"),
                LocalizationService.GetResource("Core.Demo.RandomName5"),
                LocalizationService.GetResource("Core.Demo.RandomName6"),
                LocalizationService.GetResource("Core.Demo.RandomName7"),
                LocalizationService.GetResource("Core.Demo.RandomName8"),
            };

            var intTemp = rnd.Next(strMassFirstName.Length);

            return strMassFirstName[intTemp];
        }

        public static string GetRandomLastName()
        {
            var rnd = new Random();
            return GetRandomLastName(rnd);
        }

       private static string GetRandomLastName(Random rnd)
       {
           string[] strMassLastName =
           {
               LocalizationService.GetResource("Core.Demo.RandomLastName"),
               LocalizationService.GetResource("Core.Demo.RandomLastName1"),
               LocalizationService.GetResource("Core.Demo.RandomLastName2"),
               LocalizationService.GetResource("Core.Demo.RandomLastName3"),
               LocalizationService.GetResource("Core.Demo.RandomLastName4"),
               LocalizationService.GetResource("Core.Demo.RandomLastName5"),
               LocalizationService.GetResource("Core.Demo.RandomLastName6"),
               LocalizationService.GetResource("Core.Demo.RandomLastName7"),
               LocalizationService.GetResource("Core.Demo.RandomLastName8"),
           };

            var intTemp = rnd.Next(strMassLastName.Length);

            return strMassLastName[intTemp];
        }

        public static string GetRandomCity()
        {
            var rnd = new Random();
            return GetRandomCity(rnd);
        }

        private static string GetRandomCity(Random rnd)
        {
            string[] strMassRandomCity =
            {
                LocalizationService.GetResource("Core.Demo.RandomCity"),
                LocalizationService.GetResource("Core.Demo.RandomCity1"),
                LocalizationService.GetResource("Core.Demo.RandomCity2"),
                LocalizationService.GetResource("Core.Demo.RandomCity3"),
                LocalizationService.GetResource("Core.Demo.RandomCity4"),
                LocalizationService.GetResource("Core.Demo.RandomCity5"),
            };

            var intTemp = rnd.Next(strMassRandomCity.Length);

            return strMassRandomCity[intTemp];
        }

        public static string GetRandomAdress()
        {
            var rnd = new Random();
            return GetRandomAdress(rnd);
        }

        private static string GetRandomAdress(Random rnd)
        {
            string[] strMassRandomAdress =
            {
                LocalizationService.GetResource("Core.Demo.RandomAdress"),
                LocalizationService.GetResource("Core.Demo.RandomAdress1"),
                LocalizationService.GetResource("Core.Demo.RandomAdress2"),
                LocalizationService.GetResource("Core.Demo.RandomAdress3"),
                LocalizationService.GetResource("Core.Demo.RandomAdress4"),
                LocalizationService.GetResource("Core.Demo.RandomAdress5"),
            };

            var intTemp = rnd.Next(strMassRandomAdress.Length);

            return string.Format(LocalizationService.GetResource("Core.Demo.RandomAdressFormat"), strMassRandomAdress[intTemp], intTemp + 1);
        }

        public static string GetRandomPhone()
        {
            var rnd = new Random();
            return GetRandomPhone(rnd);
        }

        private static string GetRandomPhone(Random rnd)
        {
            int intTemp;

            var sb = new StringBuilder();

            for (byte i = 0; i <= 7; i++)
            {
                intTemp = rnd.Next(10);
                sb.Append(intTemp.ToString());
            }

            return string.Format("+0 92{0}", sb.ToString());
        }

        public static string GetRandomEmail()
        {
            var rnd = new Random();
            return GetRandomEmail(rnd);
        }

        private static string GetRandomEmail(Random rnd)
        {
            string[] strMassName = { "my", "main", "we", "love", "you", "life", "big", "hello", "haha" };
            string[] strMassDomain = { "net", "com", "ru", "tv", "ws", "us", "ee", "de", "info" };

            var strFirstPart = new StringBuilder();

            for (byte i = 0; i <= 1; i++)
            {
                int intTemp = rnd.Next(strMassName.Length);
                strFirstPart.Append(strMassName[intTemp]);
            }

            return string.Format("{0}@{1}.{2}",
                strFirstPart.ToString(),
                strMassName[rnd.Next(strMassName.Length)],
                strMassDomain[rnd.Next(strMassDomain.Length)]);
        }
    }
}
using System.Web;
using System.IO;
using AdvantShop.Helpers;
using System.Text;
using AdvantShop.Customers;
using System;

namespace AdvantShop.Web.Admin.Handlers.Subscription
{
    public class ImportSubscriptionHandlers
    {
        private readonly HttpPostedFileBase _file;
        private readonly string _outputFilePath;

        public class Results
        {
            public bool Result { get; set; }
            public string Error { get; set; }
        } 

        public ImportSubscriptionHandlers(HttpPostedFileBase file, string outputFilePath)
        {
            _file = file;
            _outputFilePath = outputFilePath;

            FileHelpers.DeleteFile(outputFilePath);
        }

        public Results Execute()
        {
            if (_file == null || string.IsNullOrEmpty(_file.FileName))
            {
                return new Results { Result = false, Error = "Не найден файл" };
            }

            if(_file.ContentType != "application/vnd.ms-excel")
            {
                return new Results { Result = false, Error = "Не верный формат файла" };
            }

            _file.SaveAs(_outputFilePath);

            if (!File.Exists(_outputFilePath))
            {
                return new Results { Result = false, Error = "Не найден файл" };
            }

            var result = Import(_outputFilePath);

            return result; // new Results { Result = true, Status = "Импорт выполнен успешно" };
        }

        public Results Import(string FilePath)
        {
            try
            {
                using (var streamReader = new StreamReader(FilePath, Encoding.UTF8))
                {
                    while (streamReader.Peek() >= 0)
                    {

                        var subscriptionParams = streamReader.ReadLine().Split(new[] { ';' });
                        if (!subscriptionParams[0].Contains("@") && !subscriptionParams[0].Contains(".") && !(subscriptionParams[0].IndexOf(".") > subscriptionParams[0].IndexOf("@")))
                        {
                            continue;
                        }
                        var subscription = SubscriptionService.GetSubscription(subscriptionParams[0]);
                        if (subscription != null)
                        {
                            subscription.Email = subscriptionParams[0];
                            subscription.Subscribe = subscriptionParams[1] == "1";
                            subscription.UnsubscribeReason = subscriptionParams[4];
                            SubscriptionService.UpdateSubscription(subscription);
                        }
                        else
                        {
                            SubscriptionService.AddSubscription(new AdvantShop.Customers.Subscription
                            {
                                Email = subscriptionParams[0],
                                Subscribe = subscriptionParams[1] == "1",
                                UnsubscribeReason = subscriptionParams[4]
                            });
                        }
                    }
                }
                return new Results { Result = true };
            }
            catch
            {
                return new Results { Result = false, Error = "Ошибка импорта файла" };
            }
        }
    }
}

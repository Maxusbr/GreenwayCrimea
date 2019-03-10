using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using AdvantShop.Core.Services.Orders;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.Orders;
using AdvantShop.Shipping.Sdek;
using AdvantShop.Diagnostics;
using AdvantShop.Configuration;
using AdvantShop.Catalog;
using AdvantShop.Repository.Currencies;
using AdvantShop.Taxes;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Repository;
using System.Globalization;

namespace AdvantShop.Shipping.Sdek
{
    public class SdekApiService
    {
        #region Constants

        private readonly string _authLogin;
        private readonly string _authPassword;
        private readonly string _tariff;
        private readonly string _cityFrom;
        private readonly float _additionalPrice;
        private readonly float _defaultLength;
        private readonly float _defaultWidth;
        private readonly float _defaultHeight;
        private readonly float _defaultWeight;
        private readonly int _deliveryNote;

        private const string UrlNewOrders = "https://integration.cdek.ru/new_orders.php";
        private const string UrlNewSchedule = "https://integration.cdek.ru/new_schedule.php";
        private const string UrlCallCourier = "https://integration.cdek.ru/call_courier.php";
        private const string UrlDeleteOrders = "https://integration.cdek.ru/delete_orders.php";
        private const string UrlOrdersPrint = "https://integration.cdek.ru/orders_print.php";
        private const string UrlOrdersStatusReport = "https://integration.cdek.ru/status_report_h.php";
        private const string UrlOrdersInfoReport = "https://integration.cdek.ru/info_report.php";

        private const string UrlCalculatePrice = "http://api.cdek.ru/calculator/calculate_price_by_json.php";
        private const string UrlGetListOfCityPoints = "https://integration.cdek.ru/pvzlist.php";

        #region Tariffs

        public readonly List<SdekTariff> Tariffs = new List<SdekTariff>
        {
            new SdekTariff
            {
                TariffId = 1,
                Name = "Экспресс лайт дверь-дверь",
                Mode = "Д-Д",
                Description = "Классическая экспресс-доставка по России документов и грузов до 30 кг.",
                WeightLimitation = 30
            },
            new SdekTariff
            {
                TariffId = 3,
                Name = "Супер-экспресс до 18",
                Mode = "Д-Д",
                Description = "Срочная доставка документов и грузов «из рук в руки» по России к определенному часу.",
                WeightLimitation = 18
            },
            //new SdekTariff {TariffId = 4, Name = "Рассылка", Mode = "Д-Д"},
            new SdekTariff
            {
                TariffId = 5,
                Name = "Экономичный экспресс склад-склад",
                Mode = "С-С",
                Description =
                    "Недорогая доставка грузов по России ЖД и автотранспортом (доставка грузов с увеличением сроков)."
            },
            //new SdekTariff {TariffId = 7, Name = "Международный экспресс документы", Mode = "Д-Д"},
            //new SdekTariff {TariffId = 8, Name = "Международный экспресс грузы", Mode = "Д-Д"},
            new SdekTariff
            {
                TariffId = 10,
                Name = "Экспресс лайт склад-склад",
                Mode = "С-С",
                Description = "Классическая экспресс-доставка по России документов и грузов.",
                WeightLimitation = 30
            },
            new SdekTariff
            {
                TariffId = 11,
                Name = "Экспресс лайт склад-дверь",
                Mode = "С-Д",
                Description = "Классическая экспресс-доставка по России документов и грузов.",
                WeightLimitation = 30
            },
            new SdekTariff
            {
                TariffId = 12,
                Name = "Экспресс лайт дверь-склад",
                Mode = "Д-С",
                Description = "Классическая экспресс-доставка по России документов и грузов.",
                WeightLimitation = 30
            },
            new SdekTariff
            {
                TariffId = 15,
                Name = "Экспресс тяжеловесы склад-склад",
                Mode = "С-С",
                Description = "Классическая экспресс-доставка по России документов и грузов.",
                WeightLimitation = 30
            },
            new SdekTariff
            {
                TariffId = 16,
                Name = "Экспресс тяжеловесы склад-дверь",
                Mode = "С-Д",
                Description = "Классическая экспресс-доставка по России документов и грузов.",
                WeightLimitation = 30
            },
            new SdekTariff
            {
                TariffId = 17,
                Name = "Экспресс тяжеловесы дверь-склад",
                Mode = "Д-С",
                Description = "Классическая экспресс-доставка по России документов и грузов.",
                WeightLimitation = 30
            },
            new SdekTariff
            {
                TariffId = 18,
                Name = "Экспресс тяжеловесы дверь-дверь",
                Mode = "Д-Д",
                Description = "Классическая экспресс-доставка по России документов и грузов.",
                WeightLimitation = 30
            },

            new SdekTariff
            {
                TariffId = 57,
                Name = "Супер-экспресс до 9",
                Mode = "Д-Д",
                Description =
                    "Срочная доставка документов и грузов «из рук в руки» по России к определенному часу (доставка за 1-2 суток).",
                WeightLimitation = 5
            },
            new SdekTariff
            {
                TariffId = 58,
                Name = "Супер-экспресс до 10",
                Mode = "Д-Д",
                Description =
                    "Срочная доставка документов и грузов «из рук в руки» по России к определенному часу (доставка за 1-2 суток).",
                WeightLimitation = 5
            },
            new SdekTariff
            {
                TariffId = 59,
                Name = "Супер-экспресс до 12",
                Mode = "Д-Д",
                Description =
                    "Срочная доставка документов и грузов «из рук в руки» по России к определенному часу (доставка за 1-2 суток).",
                WeightLimitation = 5
            },
            new SdekTariff
            {
                TariffId = 60,
                Name = "Супер-экспресс до 14",
                Mode = "Д-Д",
                Description =
                    "Срочная доставка документов и грузов «из рук в руки» по России к определенному часу (доставка за 1-2 суток).",
                WeightLimitation = 5
            },
            new SdekTariff
            {
                TariffId = 61,
                Name = "Супер-экспресс до 16",
                Mode = "Д-Д",
                Description =
                    "Срочная доставка документов и грузов «из рук в руки» по России к определенному часу (доставка за 1-2 суток)."
            },
            new SdekTariff
            {
                TariffId = 62,
                Name = "Магистральный экспресс склад-склад",
                Mode = "С-С",
                Description = "Быстрая экономичная доставка грузов по России"
            },
            new SdekTariff
            {
                TariffId = 63,
                Name = "Магистральный супер-экспресс склад-склад",
                Mode = "С-С",
                Description = "Быстрая экономичная доставка грузов по России"
            },

            //new SdekTariff {TariffId = 66, Name = "Блиц-экспресс 01", Mode = "Д-Д"},
            //new SdekTariff {TariffId = 67, Name = "Блиц-экспресс 02", Mode = "Д-Д"},
            //new SdekTariff {TariffId = 68, Name = "Блиц-экспресс 03", Mode = "Д-Д"},
            //new SdekTariff {TariffId = 69, Name = "Блиц-экспресс 04", Mode = "Д-Д"},
            //new SdekTariff {TariffId = 70, Name = "Блиц-экспресс 05", Mode = "Д-Д"},
            //new SdekTariff {TariffId = 71, Name = "Блиц-экспресс 06", Mode = "Д-Д"},
            //new SdekTariff {TariffId = 72, Name = "Блиц-экспресс 07", Mode = "Д-Д"},
            //new SdekTariff {TariffId = 73, Name = "Блиц-экспресс 08", Mode = "Д-Д"},
            //new SdekTariff {TariffId = 74, Name = "Блиц-экспресс 09", Mode = "Д-Д"},
            //new SdekTariff {TariffId = 75, Name = "Блиц-экспресс 10", Mode = "Д-Д"},
            //new SdekTariff {TariffId = 76, Name = "Блиц-экспресс 11", Mode = "Д-Д"},
            //new SdekTariff {TariffId = 77, Name = "Блиц-экспресс 12", Mode = "Д-Д"},
            //new SdekTariff {TariffId = 78, Name = "Блиц-экспресс 13", Mode = "Д-Д"},
            //new SdekTariff {TariffId = 79, Name = "Блиц-экспресс 14", Mode = "Д-Д"},
            //new SdekTariff {TariffId = 80, Name = "Блиц-экспресс 15", Mode = "Д-Д"},
            //new SdekTariff {TariffId = 81, Name = "Блиц-экспресс 16", Mode = "Д-Д"},
            //new SdekTariff {TariffId = 82, Name = "Блиц-экспресс 17", Mode = "Д-Д"},
            //new SdekTariff {TariffId = 83, Name = "Блиц-экспресс 18", Mode = "Д-Д"},
            //new SdekTariff {TariffId = 84, Name = "Блиц-экспресс 19", Mode = "Д-Д"},
            //new SdekTariff {TariffId = 85, Name = "Блиц-экспресс 20", Mode = "Д-Д"},
            //new SdekTariff {TariffId = 86, Name = "Блиц-экспресс 21", Mode = "Д-Д"},
            //new SdekTariff {TariffId = 87, Name = "Блиц-экспресс 22", Mode = "Д-Д"},
            //new SdekTariff {TariffId = 88, Name = "Блиц-экспресс 23", Mode = "Д-Д"},
            //new SdekTariff {TariffId = 89, Name = "Блиц-экспресс 24", Mode = "Д-Д"},
            //для интернет магазинов
            new SdekTariff
            {
                TariffId = 136,
                Name = "Посылка склад-склад",
                Mode = "С-С",
                Description =
                    "Услуга экономичной доставки товаров по России для компаний, осуществляющих дистанционную торговлю.",
                WeightLimitation = 30
            },
            new SdekTariff
            {
                TariffId = 137,
                Name = "Посылка склад-дверь",
                Mode = "С-Д",
                Description =
                    "Услуга экономичной доставки товаров по России для компаний, осуществляющих дистанционную торговлю.",
                WeightLimitation = 30
            },
            new SdekTariff
            {
                TariffId = 138,
                Name = "Посылка дверь-склад",
                Mode = "Д-С",
                Description =
                    "Услуга экономичной доставки товаров по России для компаний, осуществляющих дистанционную торговлю.",
                WeightLimitation = 30
            },
            new SdekTariff
            {
                TariffId = 139,
                Name = "Посылка дверь-дверь",
                Mode = "Д-Д",
                Description =
                    "Услуга экономичной доставки товаров по России для компаний, осуществляющих дистанционную торговлю.",
                WeightLimitation = 30
            },

            new SdekTariff
            {
                TariffId = 233,
                Name = "Экономичная посылка склад-дверь",
                Mode = "С-Д",
                Description =
                    "Услуга экономичной наземной доставки товаров по России для компаний, осуществляющих дистанционную торговлю. Услуга действует по направлениям из Москвы в подразделения СДЭК, находящиеся за Уралом и в Крым.",
                WeightLimitation = 50
            },

            new SdekTariff
            {
                TariffId = 234,
                Name = "Экономичная посылка склад-склад",
                Mode = "С-С",
                Description =
                    "Услуга экономичной наземной доставки товаров по России для компаний, осуществляющих дистанционную торговлю. Услуга действует по направлениям из Москвы в подразделения СДЭК, находящиеся за Уралом и в Крым.",
                WeightLimitation = 50
            },


            new SdekTariff
            {
                TariffId = 291,
                Name = "CDEK Express склад-склад",
                Mode = "С-С",
                Description =
                    "Сервис по доставке товаров из-за рубежа в России с услугами по таможенному оформлению.",
                WeightLimitation = 50
            },

            new SdekTariff
            {
                TariffId = 293,
                Name = "CDEK Express дверь-дверь",
                Mode = "С-С",
                Description =
                    "Сервис по доставке товаров из-за рубежа в России с услугами по таможенному оформлению.",
                WeightLimitation = 50
            },

            new SdekTariff
            {
                TariffId = 294,
                Name = "CDEK Express склад-дверь",
                Mode = "С-С",
                Description =
                    "Сервис по доставке товаров из-за рубежа в России с услугами по таможенному оформлению.",
                WeightLimitation = 50
            },


            new SdekTariff
            {
                TariffId = 295,
                Name = "CDEK Express дверь-склад",
                Mode = "С-С",
                Description =
                    "Сервис по доставке товаров из-за рубежа в России с услугами по таможенному оформлению.",
                WeightLimitation = 50
            },


            new SdekTariff
            {
                TariffId = 7,
                Name = "Международный экспресс документы дверь-дверь",
                Mode = "Д-Д",
                Description =
                    "Экспресс-доставка за/из-за границы документов и писем.",
                WeightLimitation = 2
            },
                new SdekTariff
            {
                TariffId = 8,
                Name = "Международный экспресс грузы дверь-дверь",
                Mode = "Д-Д",
                Description =
                    "Экспресс-доставка за/из-за границы грузов и посылок от 0,5 кг до 30 кг..",
                WeightLimitation = 30
            },
                new SdekTariff
            {
                TariffId = 180,
                Name = "Международный экспресс грузы дверь-склад",
                Mode = "Д-С",
                Description =
                    "Экспресс-доставка за/из-за границы грузов и посылок от 0,5 кг до 30 кг..",
                WeightLimitation = 30
            },
                new SdekTariff
            {
                TariffId = 183,
                Name = "Международный экспресс документы дверь-склад",
                Mode = "Д-С",
                Description =
                    "Экспресс-доставка за/из-за границы документов и писем.",
                WeightLimitation = 2
            }

        };

        #endregion

        #region AddedServises

        private static readonly List<AddedService> AddedServises = new List<AddedService>
        {
            new AddedService
            {
                Code = 30,
                Name = "Примерка на дому",
                Description =
                    "Курьер доставляет покупателю несколько единиц товара (одежда, обувь и пр.) для примерки. Время ожидания курьера в этом случае составляет 30 минут."
            },
            new AddedService
            {
                Code = 36,
                Name = "Частичная доставка",
                Description =
                    "Во время доставки товара покупатель может отказаться от одной или нескольких позиций, и выкупить только часть заказа"
            },
            new AddedService
            {
                Code = 37,
                Name = "Осмотр вложения",
                Description = "Проверка покупателем содержимого заказа до его оплаты (вскрытие посылки)."
            },
            new AddedService
            {
                Code = 3,
                Name = "Доставка в выходной день",
                Description = "Осуществление доставки заказа в выходные и нерабочие дни"
            },
            new AddedService
            {
                Code = 16,
                Name = "Забор в городе отправителя",
                Description =
                    "Дополнительная услуга забора груза в городе отправителя, при условии что тариф доставки с режимом «от склада»"
            },
            new AddedService
            {
                Code = 17,
                Name = "Доставка в городе получателя",
                Description =
                    "Дополнительная услуга доставки груза в городе получателя, при условии что тариф доставки с режимом «до склада» (только для тарифов «Магистральный», «Магистральный супер-экспресс»)"
            },
            new AddedService
            {
                Code = 2,
                Name = "Страхование",
                Description =
                    "Обеспечение страховой защиты посылки. Размер дополнительного сбора страхования вычисляется от размера объявленной стоимости отправления. Важно: Услуга начисляется автоматически для всех заказов ИМ, не разрешена для самостоятельной передачи в тэге AddService."
            }
        };

        #endregion

        #endregion

        public SdekApiService()
        {
        }

        public SdekApiService(string authLogin, string authPassword, string tariff, string cityFrom,
            float additionalPrice, float defaultLength, float defaultWidth, float defaultHeight, float defaultWeight, int deliveryNote)
        {
            _authLogin = authLogin;
            _authPassword = authPassword;
            _tariff = tariff;
            _cityFrom = cityFrom;
            _additionalPrice = additionalPrice;
            _defaultLength = defaultLength;
            _defaultWidth = defaultWidth;
            _defaultHeight = defaultHeight;
            _defaultWeight = defaultWeight;
            _deliveryNote = deliveryNote;
        }

        public string GetCalculatedPrice(string jsonData)
        {
            return PostRequestGetString(UrlCalculatePrice, jsonData, "application/json");
        }

        #region GetListOfCityPoints

        public List<SdekPvz> GetListOfCityPoints(int sdekCityId, bool secondTry)
        {
            var listPvz = new List<SdekPvz>();

            try
            {
                ServicePointManager.Expect100Continue = false;
                var urTemp = UrlGetListOfCityPoints;
              
                var type = "&type=PVZ";

                var sdekTariff = Tariffs.FirstOrDefault(item => item.TariffId.ToString() == _tariff);
                if (sdekTariff != null && sdekTariff.Name.ToLower().Contains("inpost"))
                {
                    type = "&type=POSTOMAT";
                }

                var result = string.Empty;
                try
                {
                    var request = WebRequest.Create(urTemp + "?cityid=" + sdekCityId + type);
                    request.Method = "GET";
                    request.Timeout = 1500;

                    using (var response = request.GetResponse())
                    {
                        result = (new StreamReader(response.GetResponseStream())).ReadToEnd();
                    }
                }
                catch(Exception ex)
                {
                    Debug.LogError(ex, "Сервер Sdek " + urTemp + " - недоступен, ошибка: " + ex.Message);
                    if (secondTry)
                    {
                        return new List<SdekPvz>();
                    }
                    else
                    {
                        throw new Exception();
                    }
                }

                using (var xmlReader = XmlReader.Create(new StringReader(result)))
                {
                    while (xmlReader.Read())
                    {
                        if (xmlReader.NodeType != XmlNodeType.Element || !xmlReader.IsStartElement() || !string.Equals(xmlReader.Name, "Pvz"))
                            continue;

                        var sdekPvz = new SdekPvz
                        {
                            Code = xmlReader.GetAttribute("Code"),
                            Name = xmlReader.GetAttribute("Name"),
                            CityCode = xmlReader.GetAttribute("CityCode"),
                            City = xmlReader.GetAttribute("City"),
                            WorkTime = xmlReader.GetAttribute("WorkTime"),
                            Address = xmlReader.GetAttribute("Address"),
                            Phone = xmlReader.GetAttribute("Phone"),
                            Note = xmlReader.GetAttribute("Note"),
                        };
                        listPvz.Add(sdekPvz);

                        while (xmlReader.Read() && !string.Equals(xmlReader.Name, "Pvz"))
                        {
                            if (string.Equals(xmlReader.Name, "WeightLimit"))
                            {
                                sdekPvz.WeightLimit = new SdekPvzWeightLimit
                                {
                                    WeightMin = xmlReader.GetAttribute("WeightMin").TryParseInt(true),
                                    WeightMax = xmlReader.GetAttribute("WeightMax").TryParseInt(true)
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                if (!secondTry)
                    GetListOfCityPoints(sdekCityId, true);
            }

            return listPvz;
        }

        #endregion

        #region SendNewOrders

        public SdekStatusAnswer SendNewOrders(Order order, int tariffId, SdekParamsSendOrder parametr)
        {
            if (order == null || order.OrderCustomer == null)
            {
                return new SdekStatusAnswer
                {
                    Status = false,
                    Message = "Ошибка при добавление заказа в систему СДЭК"
                };
            }

            var sdekSendCityCode = SdekService.GetSdekCityId(_cityFrom, string.Empty);
            var sdekRecCityCode = SdekService.GetSdekCityId(order.OrderCustomer.City, order.OrderCustomer.Region);
            var dateExecute = order.OrderDate.ToUniversalTime().ToString("yyyy-MM-ddThh:mm:ss");
            var descriptions = parametr.Description;

            var resultXml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";

            resultXml +=
                string.Format(
                    "<DeliveryRequest Number=\"{0}\" Date=\"{1}\" Account=\"{2}\" Secure=\"{3}\" OrderCount=\"1\" DeveloperKey=\"96a1f68557c674d0224d760ed5455419\">",
                    order.OrderID,
                    dateExecute,
                    _authLogin,
                    SdekService.GetMd5Hash(MD5.Create(), dateExecute + "&" + _authPassword));

            //var contact = Customers.CustomerService.GetCustomerContact(order.ShippingContactID.ToString());
           
            //Order tag
            resultXml += string.Format(
             "<Order Number=\"{0}\" SendCityCode=\"{1}\" RecCityCode=\"{2}\" SendCityPostCode=\"{10}\" RecCityPostCode=\"\" RecipientName=\"{3}\" " +
             " RecipientEmail=\"{4}\" Phone=\"{5}\" TariffTypeCode=\"{6}\" {7} SellerName=\"{8}\" DeliveryRecipientCost=\"{9}\">",
             order.Number,
             sdekSendCityCode,
             sdekRecCityCode,
             order.OrderCustomer.FirstName + " " + order.OrderCustomer.LastName,
             order.OrderCustomer.Email,
             order.OrderCustomer.Phone,
             tariffId,
             !string.IsNullOrEmpty(descriptions) ? string.Format("Comment=\"{0}\"", descriptions) : string.Empty,
             SettingsMain.ShopName.Replace("\"", "'"),
             order.Payed ? 0 : order.ShippingCost,
             order.OrderCustomer.Zip
             );

            //Address tag
            var sdekTariff = Tariffs.FirstOrDefault(item => item.TariffId == tariffId);

            if (order.OrderPickPoint != null && sdekTariff != null && sdekTariff.Mode.EndsWith("-С"))
            {
                resultXml += string.Format("<Address Street=\"{0}\" House=\"0\" Flat=\"0\" PvzCode=\"{1}\"/>",
                    order.OrderPickPoint.PickPointAddress.Replace("\"", "'"), order.OrderPickPoint.PickPointId);
            }
            else
            {
                resultXml += string.Format("<Address Street=\"{0}\" House=\"0\" Flat=\"0\" PvzCode=\"{1}\"/>",
                    order.OrderCustomer.GetCustomerAddress().Replace("\"", "'"), string.Empty);
            }

            var itemsPrice = order.OrderItems.Sum(item => item.Price * item.Amount);
            var difference = (order.Sum - order.ShippingCost) - itemsPrice;


            float itemsPriceWithDifferenceSum = 0;

            var items = order.OrderItems.Select(item => new Measure { XYZ = new[] { item.Height, item.Width, item.Length }, Amount = item.Amount }).ToList();
            var dimensions = MeasureHelper.GetDimensions(items);

            //сдек в см
            var length = dimensions[0] / 10;
            var width = dimensions[1] / 10;
            var height = dimensions[2] / 10;

            //Package tag
            resultXml += string.Format("<Package Number=\"{0}\" BarCode=\"{0}\" Weight=\"{1}\" SizeA=\"{2}\" SizeB=\"{3}\" SizeC=\"{4}\">", 1,
                order.OrderItems.Sum(item => (item.Weight > 0 ? item.Weight : _defaultWeight) * 1000 * item.Amount),
                length > 0 ? length.ToString(CultureInfo.InvariantCulture) : (_defaultLength / 10).ToString(CultureInfo.InvariantCulture),
                width > 0 ? width.ToString(CultureInfo.InvariantCulture) : (_defaultWidth / 10).ToString(CultureInfo.InvariantCulture),
                height > 0 ? height.ToString(CultureInfo.InvariantCulture) : (_defaultHeight / 10).ToString(CultureInfo.InvariantCulture));

            int index = 1;
            foreach (var orderItem in order.OrderItems)
            {
                var relationSum = Math.Round(difference / 100 * (orderItem.Price / (itemsPrice / 100)), 2);
                var product = orderItem.ProductID != null ? ProductService.GetProduct(orderItem.ProductID.Value) : null;

                itemsPriceWithDifferenceSum += (orderItem.Price + (float)relationSum) * orderItem.Amount;

                var itemCost = orderItem.Price + relationSum + (index == order.OrderItems.Count ? Math.Round(order.Sum - order.ShippingCost) - itemsPriceWithDifferenceSum : 0);

                var tax = GetTaxType(orderItem.TaxType);

                resultXml += string.Format(
                    "<Item WareKey=\"{0}\" Cost=\"{1}\" Payment=\"{2}\" Weight=\"{3}\" Amount=\"{4}\" CommentEx=\"{5}\" Comment=\"{5}\" Link=\"{6}\"{7}{8} />",
                    orderItem.ArtNo,
                    (itemCost).ToString("F2"),
                    order.Payed
                        ? "0"
                        : (itemCost).ToString("F2"),
                    (orderItem.Weight > 0 ? orderItem.Weight : _defaultWeight) * 1000, //GetActualWeght(orderItem),
                    orderItem.Amount,
                    HttpUtility.UrlEncode(orderItem.Name.Replace("\"", "'")),
                    product != null ? SettingsMain.SiteUrl.Trim('/') + "/products/" + product.UrlPath : string.Empty,
                    !order.Payed ? " PaymentVATRate=\"VAT" + (tax.HasValue ? tax.ToString() : "X") + "\"" : string.Empty,
                    !order.Payed ? tax != null ? "PaymentVATSum =\"" + (itemCost * tax.Value / (100 + tax.Value)).ToString("F2") + "\"" : " PaymentVATSum =\"0\"" : string.Empty);
                index++;
            }

            resultXml += "</Package>";
            resultXml += "</Order>";
            resultXml += "</DeliveryRequest>";

            var responceString = PostRequestGetString(UrlNewOrders, "xml_request=" + resultXml,
                "application/x-www-form-urlencoded");

            if (responceString.Contains("ERR_SHOP_ADDITIONAL_COLLECTION_VAT_RATE_MISTAKE") ||
                responceString.Contains("ERR_GOODS_VAT_RATE_MISTAKE"))
            {
                return new SdekStatusAnswer
                {
                    Status = false,
                    Message = "Неверно указана ставка НДС, ставка должна быть: 0%/10%/18%"
                };
            }
            else if (responceString.Contains("ERR_ORDER_DUBL_EXISTS"))
            {
                return new SdekStatusAnswer { Message = "Заказ уже существует в системе" };
            }
            else if (responceString.Contains("ERR_INVALID_WEIGHT"))
            {
                return new SdekStatusAnswer { Message = "Значение веса должно быть положительным" };
            }
            else if (responceString.Contains("ERR_NEED_ATTRIBUTE") && responceString.Contains("PHONE"))
            {
                return new SdekStatusAnswer { Message = "Не заполнен обязательный атрибут Phone" };
            }
            else if (responceString.Contains("ERR_ATTRIBUTE_EMPTY") && responceString.Contains("ADDRESS"))
            {
                return new SdekStatusAnswer { Message = "Нет адреса доставки" };
            }
            else if (responceString.Contains("ERR_WEIGHT_LIMIT"))
            {
                return new SdekStatusAnswer { Message = "Невозможна отправка груза свыше выше ограничений тарифа" };
            }
            else if (responceString.Contains("ERR_"))
            {
                var errMsg = string.Empty;
                if (responceString.Contains("Msg=\"") && responceString.Contains("\"/><Order") && responceString.IndexOf("Msg=\"") < responceString.IndexOf("\"/><Order"))
                {
                    errMsg = "Ошибка добавления заказа. Ошибка: " +
                             responceString.Substring(responceString.IndexOf("Msg=\""), responceString.IndexOf("\"/><Order") - responceString.IndexOf("Msg=\"")).Replace("Msg=\"", "");
                }
                else
                {
                    errMsg = "Ошибка добавления заказа. Ошибка. ";
                    Debug.LogError("Error Sdek Response: " + responceString);
                }
                return new SdekStatusAnswer { Message = errMsg };
            }

            return new SdekStatusAnswer { Status = true, Message = "Заказ добавлен в систему СДЭК" };
        }

        #endregion

        #region ReportOrderStatuses

        public void ReportOrderStatuses()
        {
            var dateExecute = DateTime.UtcNow.Date.ToString("yyyy-MM-ddThh:mm:ss");
            var resultXml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";

            resultXml += string.Format("<StatusReport Date=\"{0}\" Account=\"{1}\" Secure=\"{2}\" ShowHistory=\"{3}\">",
                dateExecute,
                _authLogin,
                SdekService.GetMd5Hash(MD5.Create(), dateExecute + "&" + _authPassword),
                1);

            resultXml += "</StatusReport>";

            var responceString = PostRequestGetString(UrlNewOrders, "xml_request=" + resultXml, "application/x-www-form-urlencoded");
        }
        
        public SdekStatusAnswer ReportOrderStatuses(Order order)
        {
            var dateExecute = DateTime.UtcNow.Date.ToString("yyyy-MM-ddThh:mm:ss");
            var resultXml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";

            resultXml += string.Format(
                "<StatusReport Date=\"{0}\" Account=\"{1}\" Secure=\"{2}\" ShowHistory=\"{3}\">",
                dateExecute,
                _authLogin,
                SdekService.GetMd5Hash(MD5.Create(), dateExecute + "&" + _authPassword),
                1);

            resultXml += string.Format("<Order DispatchNumber=\"{0}\" Number=\"{1}\" Date=\"{2}\" />",
                order.OrderID,
                order.Number,
                order.OrderDate.ToUniversalTime().ToString("yyyy-MM-ddThh:mm:ss"));

            resultXml += "</StatusReport>";

            var responceString = PostRequestGetString(UrlOrdersStatusReport, "xml_request=" + resultXml,
                "application/x-www-form-urlencoded");

            SdekOrderStatusInfo result;

            using (var reader = new StringReader(responceString))
            {
                var serializer = new XmlSerializer(typeof(SdekOrderStatusInfo));
                result = (SdekOrderStatusInfo)serializer.Deserialize(reader);
            }

            return new SdekStatusAnswer
            {
                Message = result != null && !result.ErrorCode.IsNotEmpty()
                        ? "Информация о статусах получена"
                        : string.Format("Заказ {0} в системе не найден", order.Number),
                Status = result != null && (!result.ErrorCode.IsNotEmpty() || result.Orders != null),
                Object = CreateOrderStatusReport(result)
            };
        }

        private string CreateOrderStatusReport(SdekOrderStatusInfo sdekOrderStatusInfo)
        {
            var filePath = FoldersHelper.GetPathAbsolut(FolderType.PriceTemp);

            FileHelpers.CreateDirectory(filePath);

            var fileName = string.Format("SdekOrderStatusInfoReport_{0}.txt", DateTime.Now.ToShortDateString());

            using (var writer = new StreamWriter(filePath + "\\" + fileName))
            {
                if (sdekOrderStatusInfo.Orders == null || !sdekOrderStatusInfo.Orders.Any() || sdekOrderStatusInfo.ErrorCode.IsNotEmpty() || 
                    sdekOrderStatusInfo.Orders != null && !sdekOrderStatusInfo.Orders[0].Msg.IsNullOrEmpty())
                {
                    writer.WriteLine("Заказ не найден в системе СДЭК");
                    writer.WriteLine(sdekOrderStatusInfo.Msg);
                    writer.WriteLine(sdekOrderStatusInfo.Orders != null ? string.Join("\r\n", sdekOrderStatusInfo.Orders.Where(x => x.Msg.IsNotEmpty()).Select(x => x.Msg).ToList()) : string.Empty);
                    writer.Dispose();
                    return fileName;
                }

                writer.WriteLine("Отчет «Статусы заказов» {0} - {1}",
                    sdekOrderStatusInfo.DateFirst,
                    sdekOrderStatusInfo.DateLast);

                writer.WriteLine("\tНомер акта приема-передачи: " + sdekOrderStatusInfo.Orders[0].ActNumber);

                writer.WriteLine("\tНомер отправления клиента: " + sdekOrderStatusInfo.Orders[0].Number);

                writer.WriteLine("\tНомер отправления СДЭК (присваивается при импорте заказов): " + sdekOrderStatusInfo.Orders[0].DispatchNumber);

                writer.WriteLine("\tДата доставки: " + sdekOrderStatusInfo.Orders[0].DeliveryDate);

                writer.WriteLine("\tПолучатель при доставке: " + sdekOrderStatusInfo.Orders[0].RecipientName);

                if (sdekOrderStatusInfo.Orders[0].Status != null)
                {
                    writer.WriteLine("Текущий статус заказа");

                    writer.WriteLine("\tCтатус: {0} {1} {2}. Город изменения статуса {3} {4}",
                        sdekOrderStatusInfo.Orders[0].Status.Code,
                        sdekOrderStatusInfo.Orders[0].Status.Date,
                        sdekOrderStatusInfo.Orders[0].Status.Description,
                        sdekOrderStatusInfo.Orders[0].Status.CityCode,
                        sdekOrderStatusInfo.Orders[0].Status.CityName);

                    writer.WriteLine("История изменения статусов");

                    foreach (var state in sdekOrderStatusInfo.Orders[0].Status.State)
                    {
                        writer.WriteLine("\t{0} {1} {2}. Города изменения статуса {3} {4}",
                            state.Code,
                            state.Date,
                            state.Description,
                            state.CityCode,
                            state.CityName);
                    }
                }

                if (sdekOrderStatusInfo.Orders[0].Reason != null)
                {
                    writer.WriteLine("Текущий дополнительный статус {0} {1} {2}",
                        sdekOrderStatusInfo.Orders[0].Reason.Code,
                        sdekOrderStatusInfo.Orders[0].Reason.Date,
                        sdekOrderStatusInfo.Orders[0].Reason.Description);
                }

                if (sdekOrderStatusInfo.Orders[0].DelayReason != null)
                {
                    writer.WriteLine("Текущая причина задержки {0} {1} {2}",
                        sdekOrderStatusInfo.Orders[0].DelayReason.Code,
                        sdekOrderStatusInfo.Orders[0].DelayReason.Date,
                        sdekOrderStatusInfo.Orders[0].DelayReason.Description);
                }

                if (sdekOrderStatusInfo.Orders[0].Call != null)
                {
                    if (sdekOrderStatusInfo.Orders[0].Call.CallGood != null)
                    {
                        writer.WriteLine("История прозвонов получателя");
                        foreach (var callGood in sdekOrderStatusInfo.Orders[0].Call.CallGood.Good)
                        {
                            writer.WriteLine("\tУдачный прозвон {0}, дата доставки {1}",
                                callGood.Date,
                                callGood.DateDeliv);
                        }
                    }

                    if (sdekOrderStatusInfo.Orders[0].Call.CallFail != null)
                    {
                        writer.WriteLine("История неудачных прозвонов");
                        foreach (var callFail in sdekOrderStatusInfo.Orders[0].Call.CallFail.Fail)
                        {
                            writer.WriteLine("\tНеудачный прозвон {0}, Причина неудачного прозвона {1} {2}",
                                callFail.Date,
                                callFail.ReasonCode,
                                callFail.ReasonDescription);
                        }
                    }

                    if (sdekOrderStatusInfo.Orders[0].Call.CallDelay != null)
                    {
                        writer.WriteLine("История переносов прозвона");
                        foreach (var callDelay in sdekOrderStatusInfo.Orders[0].Call.CallDelay.Delay)
                        {
                            writer.WriteLine("\tПеренос прозвона {0}, Дата, на которую перенесен прозвон {1}",
                                callDelay.Date,
                                callDelay.DateNext);
                        }
                    }
                }
            }

            return fileName;
        }

        #endregion

        #region ReportOrdersInfo

        public SdekStatusAnswer ReportOrdersInfo(Order order)
        {
            var dateExecute = DateTime.UtcNow.Date.ToString("yyyy-MM-ddThh:mm:ss");
            var resultXml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";

            resultXml += string.Format(
                "<InfoRequest Date=\"{0}\" Account=\"{1}\" Secure=\"{2}\">",
                dateExecute,
                _authLogin,
                SdekService.GetMd5Hash(MD5.Create(), dateExecute + "&" + _authPassword));

            resultXml += string.Format(
                "<Order DispatchNumber=\"{0}\" Number=\"{1}\" Date=\"{2}\" />",
                string.Empty, //order.OrderID,
                order.Number,
                order.OrderDate.ToUniversalTime().ToString("yyyy-MM-ddThh:mm:ss"));

            resultXml += "</InfoRequest>";

            var responceString = PostRequestGetString(UrlOrdersInfoReport, "xml_request=" + resultXml,
                "application/x-www-form-urlencoded");

            if (responceString.Contains("ERR_INVALID_NUMBER"))
            {
                return new SdekStatusAnswer
                {
                    Message = string.Format("Заказ {0} в системе не найден", order.Number),
                    Status = false,
                    Object = CreateOrderInfoReport(null)
                };
            }

            SdekOrderInfoReport result;

            using (var reader = new StringReader(responceString))
            {
                var serializer = new XmlSerializer(typeof(SdekOrderInfoReport));
                result = (SdekOrderInfoReport)serializer.Deserialize(reader);
            }

            return new SdekStatusAnswer
            {
                Message = result != null && result.Orders != null
                    ? "Информация о заказе получена"
                    : string.Format("Заказ {0} в системе не найден", order.Number),
                Status = result != null && result.Orders != null,
                Object = CreateOrderInfoReport(result)
            };
        }
        
        private string CreateOrderInfoReport(SdekOrderInfoReport sdekOrderInfoReport)
        {
            var filePath = FoldersHelper.GetPathAbsolut(FolderType.PriceTemp);
            FileHelpers.CreateDirectory(filePath);
            var fileName = string.Format("SdekOrderInfoReport_{0}.txt", DateTime.Now.ToShortDateString());

            using (var writer = new StreamWriter(filePath + fileName))
            {
                if (sdekOrderInfoReport == null || sdekOrderInfoReport.Orders == null || !sdekOrderInfoReport.Orders.Any())
                {
                    writer.WriteLine("Заказ не найден в системе СДЭК");
                    writer.Dispose();
                    return fileName;
                }

                writer.WriteLine("Отчет «Информация по заказам»");

                writer.WriteLine("Заказ");

                writer.WriteLine("\tДата, в которую был передан заказ в базу СДЭК: " + sdekOrderInfoReport.Orders[0].Date);

                writer.WriteLine("\tНомер отправления клиента: " + sdekOrderInfoReport.Orders[0].Number);

                writer.WriteLine("\tНомер отправления СДЭК (присваивается при импорте заказов): " + sdekOrderInfoReport.Orders[0].DispatchNumber);

                writer.WriteLine("\tКод типа тарифа: " + sdekOrderInfoReport.Orders[0].TariffTypeCode);

                writer.WriteLine("\tРасчетный вес (в граммах): " + sdekOrderInfoReport.Orders[0].Weight);

                writer.WriteLine("\tСтоимость услуги доставки, руб: " + sdekOrderInfoReport.Orders[0].DeliverySum);

                writer.WriteLine("\tДата последнего изменения суммы по услуге доставки: " + sdekOrderInfoReport.Orders[0].DateLastChange);

                writer.WriteLine("\tГород отправителя {0} {1}, почтовый индекс {2}",
                    sdekOrderInfoReport.Orders[0].SendCity.Code,
                    sdekOrderInfoReport.Orders[0].SendCity.Name,
                    sdekOrderInfoReport.Orders[0].SendCity.PostCode);

                writer.WriteLine("\tГород получателя {0} {1}, почтовый индекс {2}",
                  sdekOrderInfoReport.Orders[0].RecCity.Code,
                  sdekOrderInfoReport.Orders[0].RecCity.Name,
                  sdekOrderInfoReport.Orders[0].RecCity.PostCode);


                var addedService =
                    AddedServises.FirstOrDefault(
                        item => item.Code == sdekOrderInfoReport.Orders[0].AddedService.ServiceCode);
                writer.WriteLine("\tДополнительные услуги к заказам: {0} руб. {1} {2}",
                    sdekOrderInfoReport.Orders[0].AddedService.Sum,
                    sdekOrderInfoReport.Orders[0].AddedService.ServiceCode,
                    addedService != null ? addedService.Name : string.Empty);
            }

            return fileName;
        }

        #endregion

        #region ReportOrdersInfo

        public void ReportOrdersInfo()
        {
            var dateExecute = DateTime.UtcNow.Date.ToString("yyyy-MM-ddThh:mm:ss");
            var resultXml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";

            resultXml += string.Format(
                "<InfoRequest Date=\"{0}\" Account=\"{1}\" Secure=\"{2}\">",
                dateExecute,
                _authLogin,
                SdekService.GetMd5Hash(MD5.Create(), dateExecute + "&" + _authPassword));

            resultXml += "</InfoRequest>";

            var responceString = PostRequestGetString(UrlNewOrders, "xml_request=" + resultXml,
                "application/x-www-form-urlencoded");
        }

        #endregion

        #region PrintFormOrder

        public string PrintFormOrder(Order order)
        {
            var dateExecute = DateTime.UtcNow.Date.ToString("yyyy-MM-ddThh:mm:ss");
            var resultXml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";

            resultXml += string.Format(
                "<OrdersPrint Date=\"{0}\" Account=\"{1}\" Secure=\"{2}\" OrderCount=\"1\" CopyCount=\"{3}\">",
                dateExecute,
                _authLogin,
                SdekService.GetMd5Hash(MD5.Create(), dateExecute + "&" + _authPassword),
                _deliveryNote > 0 ? _deliveryNote : 1);

            resultXml += string.Format("<Order DispatchNumber=\"{0}\" Number=\"{1}\" Date=\"{2}\"/>",
                string.Empty,
                order.Number,
                order.OrderDate.ToUniversalTime().ToString("yyyy-MM-ddThh:mm:ss"));

            resultXml += "</OrdersPrint>";

            var printFormFilePath = FoldersHelper.GetPathAbsolut(FolderType.PriceTemp);
            FileHelpers.CreateDirectory(printFormFilePath);

            var fileName = string.Format("SdekPrintFormOrder{0}.pdf", order.OrderID);

            using (var responseStream = PostRequestGetStream(UrlOrdersPrint, "xml_request=" + resultXml, "application/x-www-form-urlencoded"))
            {
                using (var memoryStream = new MemoryStream())
                {
                    responseStream.CopyTo(memoryStream);
                    memoryStream.Position = 0;

                    var responseString = (new StreamReader(memoryStream)).ReadToEnd();
                    if (responseString.Contains("ERR_INVALID_NUMBER"))
                    {
                        memoryStream.Dispose();
                        responseStream.Dispose();
                        return CreatePrintFormNotOrder(order.Number);
                    }

                    using (
                        var filestream = new FileStream(printFormFilePath + fileName, FileMode.Create,
                            FileAccess.ReadWrite))
                    {
                        memoryStream.Position = 0;
                        memoryStream.CopyTo(filestream);
                    }
                }
            }

            return fileName;
        }

        private string CreatePrintFormNotOrder(string orderNumber)
        {
            var filePath = FoldersHelper.GetPathAbsolut(FolderType.PriceTemp);
            var fileName = string.Format("SdekPrintFormOrder_{0}.txt", DateTime.Now.ToShortDateString());

            using (var writer = new StreamWriter(filePath + fileName))
            {
                writer.WriteLine("Заказ {0} не найден в системе СДЭК", orderNumber);
            }
            return fileName;
        }

        #endregion

        #region DeleteOrder

        public SdekStatusAnswer DeleteOrder(Order order)
        {
            var dateExecute = DateTime.UtcNow.Date.ToString("yyyy-MM-ddThh:mm:ss");
            var resultXml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";

            resultXml += string.Format(
                "<DeleteRequest Number=\"{0}\" Date=\"{1}\" Account=\"{2}\" Secure=\"{3}\" OrderCount=\"1\">",
                order.OrderID,
                dateExecute,
                _authLogin,
                SdekService.GetMd5Hash(MD5.Create(), dateExecute + "&" + _authPassword));

            resultXml += string.Format("<Order Number=\"{0}\"/>", order.Number);

            resultXml += "</DeleteRequest>";

            var responceString = PostRequestGetString(UrlDeleteOrders, "xml_request=" + resultXml, "application/x-www-form-urlencoded");

            if (responceString.Contains("ERR_ORDER_NOTFIND"))
                return new SdekStatusAnswer
                {
                    Message = string.Format("Заказ {0} не найден в системе СДЭК", order.Number)
                };

            return new SdekStatusAnswer
            {
                Message = string.Format("Заказ {0} удален из системы СДЭК", order.Number),
                Status = true
            };
        }

        #endregion

        #region CallCustomer

        public SdekStatusAnswer CallCustomer(Order order)
        {
            var dateExecute = DateTime.UtcNow.Date.ToString("yyyy-MM-ddThh:mm:ss");
            var resultXml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";

            resultXml += string.Format(
                "<ScheduleRequest Date=\"{0}\" Account=\"{1}\" Secure=\"{2}\" OrderCount=\"1\">",
                dateExecute,
                _authLogin,
                SdekService.GetMd5Hash(MD5.Create(), dateExecute + "&" + _authPassword));

            resultXml += string.Format(
                "<Order DispatchNumber=\"{0}\" Number=\"{1}\" Date=\"{2}\"/>",
                string.Empty, //order.OrderID,
                order.Number,
                order.OrderDate.ToUniversalTime().ToString("yyyy-MM-ddThh:mm:ss"));

            resultXml += string.Format(
                "<Attempt ID=\"{0}\" Date=\"{1}\"/>",
                order.OrderID,
                DateTime.Now.AddDays(1).ToString("yyyy-MM-ddThh:mm:ss"));

            resultXml += "</ScheduleRequest>";

            var responceString = PostRequestGetString(UrlNewSchedule, "xml_request=" + resultXml,
                "application/x-www-form-urlencoded");

            if (responceString.Contains("ERR_INVALID_NUMBER"))
            {
                return new SdekStatusAnswer
                {
                    Message = string.Format("Заказ {0} не найден в системе СДЭК", order.Number)
                };
            }

            return new SdekStatusAnswer
            {
                Message = string.Format("Прозвон получателя создан в заказе {0}", order.Number),
                Status = true
            };
        }

        #endregion

        #region CallCourier

        public SdekStatusAnswer CallCourier(DateTime date, DateTime timeBegin, DateTime timeEnd, string cityName, string street, string house, string flat, string phone, string name, string weight)
        {
            var dateExecute = DateTime.UtcNow.Date.ToString("yyyy-MM-ddTHH:mm:ss");
            var resultXml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";

            resultXml += string.Format(
                "<CallCourier Date=\"{0}\" Account=\"{1}\" Secure=\"{2}\" CallCount=\"1\">",
                dateExecute,
                _authLogin,
                SdekService.GetMd5Hash(MD5.Create(), dateExecute + "&" + _authPassword));

            resultXml += string.Format(
                "<Call Date=\"{0}\" TimeBeg=\"{1}\" TimeEnd=\"{2}\" SendCityCode=\"{3}\" SendPhone=\"{4}\" SenderName=\"{5}\" Weight=\"{6}\" >",
                date.ToString("yyyy-MM-dd"),
                timeBegin.ToString("HH:mm:ss"),
                timeEnd.ToString("HH:mm:ss"),
                SdekService.GetSdekCityId(cityName, string.Empty),
                phone,
                name,
                weight);

            resultXml += string.Format("<Address Street=\"{0}\" House=\"{1}\" Flat=\"{2}\" />", street, house, flat);

            resultXml += "</Call></CallCourier>";

            var serializer = new XmlSerializer(typeof(SdekXmlResponse));
            var result = new SdekXmlResponse();

            try
            {
                result = (SdekXmlResponse)serializer.Deserialize(new StreamReader(PostRequestGetStream(UrlCallCourier, "xml_request=" + resultXml, "application/x-www-form-urlencoded")));
            }
            catch (WebException ex)
            {
                using (var eResponse = ex.Response)
                {
                    using (var eStream = eResponse.GetResponseStream())
                    {
                        if (eStream != null)
                            using (var reader = new StreamReader(eStream))
                            {
                                result = (SdekXmlResponse)serializer.Deserialize(reader);
                            }
                    }
                }
            }
            
            if (result.CallCourier == null)
                return new SdekStatusAnswer { Message = "Сервис не отвечает", Status = false };
            
            return new SdekStatusAnswer
            {
                Message = result.CallCourier.Msg,
                Status = string.IsNullOrEmpty(result.CallCourier.ErrorCode)
            };
        }

        #endregion

        #region Help methods

        private string PostRequestGetString(string url, string data, string contentType, bool secondTry = false)
        {
            try
            {
                var request = WebRequest.Create(url);
                request.Method = "POST";
                request.Timeout = 3000;

                byte[] byteArray = Encoding.UTF8.GetBytes(data);

                request.ContentType = contentType;
                request.ContentLength = byteArray.Length;
                using (Stream dataStream = request.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Close();
                }
                using (var response = request.GetResponse())
                {
                    return (new StreamReader(response.GetResponseStream())).ReadToEnd();
                }
            }
            catch (Exception)
            {
                if (secondTry) throw;
                return PostRequestGetString(url, data, contentType, true);
            }
        }

        private Stream PostRequestGetStream(string url, string data, string contentType, bool secondTry = false)
        {
            try
            {
                var request = WebRequest.Create(url);
                request.Method = "POST";
                request.Timeout = 3000;

                byte[] byteArray = Encoding.UTF8.GetBytes(data);

                request.ContentType = contentType;
                request.ContentLength = byteArray.Length;
                using (Stream dataStream = request.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Close();
                }

                return request.GetResponse().GetResponseStream();
            }
            catch (Exception)
            {
                if (secondTry) throw;
                return PostRequestGetStream(url, data, contentType, true);
            }
        }

        private int? GetTaxType(TaxType? taxType)
        {
            if (taxType == null)
                return null;

            if (taxType.Value == TaxType.Without)
                return null;

            if (taxType.Value == TaxType.Zero)
                return 0;

            if (taxType.Value == TaxType.Ten)
                return 10;

            if (taxType.Value == TaxType.Eighteen)
                return 18;

            return null;
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using AdvantShop.Core.Caching;
using AdvantShop.Diagnostics;

namespace AdvantShop.Core.Services.Shipping.Grastin.Api
{
    public class GrastinApiService
    {
        private readonly string _apiKey;

        public List<string> LastActionErrors { get; set; }

        public GrastinApiService(string apiKey)
        {
            _apiKey = apiKey;
        }

        /// <summary>
        /// Получение списка пунктов самовывоза
        /// </summary>
        /// <returns></returns>
        public List<Selfpickup> GetGrastinSelfPickups()
        {
            return CacheManager.Get(string.Format("GrastinApi-{0}-{1}", _apiKey, EnApiMethod.SelfPickup), 1440, () =>
            {
                var response = MakeRequest<GrastinSelfPickupResponse, GrastinSelfPickup>(new GrastinSelfPickup());

                if (response != null)
                    return response.Selfpickups;

                return null;
            });
        }

        /// <summary>
        /// Получение списка пунктов самовывоза Boxberry
        /// </summary>
        /// <returns></returns>
        public List<SelfpickupBoxberry> GetBoxberrySelfPickup()
        {
            return GetBoxberrySelfPickup(string.Empty);
        }

        /// <summary>
        /// Получение списка пунктов самовывоза Boxberry
        /// </summary>
        /// <param name="filterCity">Часть строки населенного пункта для фильтрации.</param>
        /// <returns></returns>
        public List<SelfpickupBoxberry> GetBoxberrySelfPickup(string filterCity)
        {
            if (string.IsNullOrEmpty(filterCity))
                return CacheManager.Get(
                    string.Format("GrastinApi-{0}-{1}", _apiKey, EnApiMethod.BoxberrySelfPickup),
                    60,
                    () =>
                    {
                        var responseCache = MakeRequest<GrastinBoxberrySelfPickupResponse, GrastinBoxberrySelfPickup>(
                            new GrastinBoxberrySelfPickup());

                        if (responseCache != null)
                            return responseCache.SelfpickupsBoxberry;

                        return null;
                    });

            var response =
               MakeRequest<GrastinBoxberrySelfPickupResponse, GrastinBoxberrySelfPickup>(new GrastinBoxberrySelfPickup()
                {
                    FilterCity = filterCity
                });

            if (response != null)
                return response.SelfpickupsBoxberry;

            return null;
        }

        /// <summary>
        /// Получение списка доступных индексов Boxberry
        /// </summary>
        /// <returns></returns>
        public List<PostcodeBoxberry> GetBoxberryPostCode()
        {
            return GetBoxberryPostCode(string.Empty);
        }

        /// <summary>
        /// Получение списка доступных индексов Boxberry
        /// </summary>
        /// <param name="filterCity">Часть строки населенного пункта для фильтрации.</param>
        /// <returns></returns>
        public List<PostcodeBoxberry> GetBoxberryPostCode(string filterCity)
        {
            if (string.IsNullOrEmpty(filterCity))
                return CacheManager.Get(
                    string.Format("GrastinApi-{0}-{1}", _apiKey, EnApiMethod.BoxberryPostCode),
                    10,
                    () =>
                    {
                        var responseCache = MakeRequest<GrastinBoxberryPostCodeResponse, GrastinBoxberryPostCode>(
                            new GrastinBoxberryPostCode());

                        if (responseCache != null)
                            return responseCache.PostcodesBoxberry;

                        return null;
                    });

            var response =
                MakeRequest<GrastinBoxberryPostCodeResponse, GrastinBoxberryPostCode>(new GrastinBoxberryPostCode()
                {
                    FilterCity = filterCity
                });

            if (response != null)
                return response.PostcodesBoxberry;

            return null;
        }

        /// <summary>
        /// Получение списка пунктов самовывоза Hermes
        /// </summary>
        /// <returns></returns>
        public List<SelfpickupHermes> GetHermesSelfPickup()
        {
            return GetHermesSelfPickup(string.Empty);
        }

        /// <summary>
        /// Получение списка пунктов самовывоза Hermes
        /// </summary>
        /// <param name="filterCity">Часть строки населенного пункта для фильтрации.</param>
        /// <returns></returns>
        public List<SelfpickupHermes> GetHermesSelfPickup(string filterCity)
        {
            if (string.IsNullOrEmpty(filterCity))
                return CacheManager.Get(
                    string.Format("GrastinApi-{0}-{1}", _apiKey, EnApiMethod.HermesSelfPickup),
                    60,
                    () =>
                    {
                        var responseCache = MakeRequest<GrastinHermesSelfPickupResponse, GrastinHermesSelfPickup>(
                            new GrastinHermesSelfPickup());

                        if (responseCache != null)
                            return responseCache.SelfpickupHermes;

                        return null;
                    });

            var response =
                MakeRequest<GrastinHermesSelfPickupResponse, GrastinHermesSelfPickup>(new GrastinHermesSelfPickup()
                {
                    FilterCity = filterCity
                });

            if (response != null)
                return response.SelfpickupHermes;

            return null;
        }

        /// <summary>
        /// Получение списка офисов транспортных компаний
        /// </summary>
        /// <returns></returns>
        public List<Office> GetTransportCompanyOffices()
        {
            return CacheManager.Get(
                string.Format("GrastinApi-{0}-{1}", _apiKey, EnApiMethod.TransportCompanyOfficeList),
                1440,
                () =>
                {
                    var response =
                        MakeRequest<GrastinTransportCompanyOfficeListResponse, GrastinTransportCompanyOfficeList>(
                            new GrastinTransportCompanyOfficeList());

                    if (response != null)
                        return response.Offices;

                    return null;
                });
        }

        /// <summary>
        /// Получение списка складов приёмки
        /// </summary>
        /// <returns></returns>
        public List<Warehouse> GetWarehouses()
        {
            return CacheManager.Get(string.Format("GrastinApi-{0}-{1}", _apiKey, EnApiMethod.Warehouse), 1440, () =>
            {
                var response = MakeRequest<GrastinWarehouseResponse, GrastinWarehouse>(new GrastinWarehouse());

                if (response != null)
                    return response.Warehouses;

                return null;
            });
        }

        /// <summary>
        /// Получение списка регионов доставки
        /// </summary>
        /// <returns></returns>
        public List<DeliveryRegion> GetDeliveryRegions()
        {
            return CacheManager.Get(string.Format("GrastinApi-{0}-{1}", _apiKey, EnApiMethod.DeliveryRegions), 1440,
                () =>
                {
                    var response =
                        MakeRequest<GrastinDeliveryRegionResponse, GrastinDeliveryRegion>(new GrastinDeliveryRegion());
                    if (response != null)
                        return response.DeliveryRegions;

                    return null;
                });
        }

        /// <summary>
        /// Получение списка договоров
        /// </summary>
        /// <returns></returns>
        public List<Contract> GetContracts()
        {
            return CacheManager.Get(string.Format("GrastinApi-{0}-{1}", _apiKey, EnApiMethod.GetContractList), 60,
                () =>
                {
                    var response =
                        MakeRequest<GrastinContractsResponse, GrastinContracts>(new GrastinContracts());
                    if (response != null)
                        return response.Contracts;

                    return null;
                });
        }

        /// <summary>
        /// Добавление заказа в курьерскую службу
        /// </summary>
        /// <returns></returns>
        public List<OrderResponse> AddGrastinOrder(GrastinOrderCourier order)
        {
            var response =
                MakeRequest<GrastinOrderCourierResponse, GrastinOrderCourier>(order);

            if (response != null)
                return response.OrdersResponse;

            return null;
        }

        /// <summary>
        /// Добавление заказа Почта России
        /// </summary>
        /// <returns></returns>
        public List<OrderResponse> AddRussianPostOrder(RussianPostOrderContainer order)
        {
            var response =
                MakeRequest<RussianPostOrderResponse, RussianPostOrderContainer>(order);
            if (response != null)
                return response.OrdersResponse;

            return null;
        }

        /// <summary>
        /// Добавление заказа Boxberry
        /// </summary>
        /// <returns></returns>
        public List<OrderResponse> AddBoxberryOrder(BoxberryOrderContainer order)
        {
            var response =
                MakeRequest<BoxberryOrderResponse, BoxberryOrderContainer>(order);
            if (response != null)
                return response.OrdersResponse;

            return null;
        }

        /// <summary>
        /// Добавление заказа Hermes
        /// </summary>
        /// <returns></returns>
        public List<OrderResponse> AddHermesOrder(HermesOrderContainer order)
        {
            var response =
                MakeRequest<HermesOrderResponse, HermesOrderContainer>(order);
            if (response != null)
                return response.OrdersResponse;

            return null;
        }

        /// <summary>
        /// Создание заявки на забор товара
        /// </summary>
        /// <returns></returns>
        public bool AddRequestForIntake(RequestForIntakeContainer request)
        {
            var response =
                MakeRequest<RequestForIntakeResponse, RequestForIntakeContainer>(request);
            if (response != null)
                return !string.IsNullOrEmpty(response.Status);

            return false;
        }

        /// <summary>
        /// Получение акта приема-передачи
        /// </summary>
        /// <returns>Путь к файлу акта</returns>
        public string GetAct(ActContainer info, string path)
        {
            string filePath = null;

            using (var responseStream = MakeRequestGetStream<ActContainer>(info))
            {
                if (responseStream != null)
                {
                    filePath = path + string.Format("GrastinPrintOrderAct{0}.pdf", info.Orders[0].OrderNumber);

                    using (var filestream = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite))
                        responseStream.CopyTo(filestream);
                }
            }

            return filePath;
        }

        /// <summary>
        /// Получение маркировки
        /// </summary>
        /// <returns>Путь к файлу маркировки</returns>
        public string GetMark(MarkContainer info, string path)
        {
            string filePath = null;

            using (var responseStream = MakeRequestGetStream<MarkContainer>(info))
            {
                if (responseStream != null)
                {
                    filePath = path + string.Format("GrastinPrintOrderMark{0}.pdf", info.Orders[0].OrderNumber);

                    using (var filestream = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite))
                        responseStream.CopyTo(filestream);
                }
            }

            return filePath;
        }

        /// <summary>
        /// Получение стоимости доставки
        /// </summary>
        /// <returns></returns>
        public List<CostResponse> CalcShipingCost(CalcShipingCostContainer data)
        {
            var response =
                MakeRequest<CalcShipingCostResponse, CalcShipingCostContainer>(data);
            if (response != null)
                return response.CostsResponse;

            return null;
        }

        /// <summary>
        /// Получение информации по заказу
        /// </summary>
        /// <returns></returns>
        public List<OrdersInfo> GetOrderInfo(OrderInfoContainer data)
        {
            var response =
                MakeRequest<OrderInfoResponse, OrderInfoContainer>(data);
            if (response != null)
                return response.OrdersInfoResponse;

            return null;
        }


        #region Private Methods

        public Stream MakeRequestGetStream<TD>(TD data)
            where TD : GrastinBaseObject
        {
            LastActionErrors = null;

            try
            {
                var request = CreateRequest(data);

                var response = request.GetResponse();
                if (response.ContentType == "text/xml")
                {
                    using (var stream = response.GetResponseStream())
                    {
                        var errorResponse = Deserialize<GrastinErrorResponse>(stream) as GrastinErrorResponse;
                        if (errorResponse != null)
                        {
                            LastActionErrors = errorResponse.Errors;
                            Debug.Log.Error(string.Format("Grastin errors: {0}",
                                errorResponse.Errors != null ? string.Join(" ", errorResponse.Errors) : string.Empty));
                        }
                    }
                    return null;
                }
                return response.GetResponseStream();
            }
            catch (WebException ex)
            {
                using (var eResponse = ex.Response)
                {
                    if (eResponse != null)
                    {
                        using (var eStream = eResponse.GetResponseStream())
                            if (eStream != null)
                                using (var reader = new StreamReader(eStream))
                                {
                                    var error = reader.ReadToEnd();
                                    LastActionErrors = new List<string>()
                                    {
                                        string.IsNullOrEmpty(error) ? ex.Message : error
                                    };

                                    if (string.IsNullOrEmpty(error))
                                        Debug.Log.Error(ex);
                                    else
                                        Debug.Log.Error(error, ex);
                                }
                            else
                            {
                                Debug.Log.Error(ex);
                                LastActionErrors = new List<string>() {ex.Message};
                            }
                    }
                    else
                    {
                        Debug.Log.Error(ex);
                        LastActionErrors = new List<string>() { ex.Message };
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
                LastActionErrors = new List<string>() { ex.Message };
            }

            return null;
        }

        public T MakeRequest<T, TD>(TD data)
            where T : class , new()
            where TD : GrastinBaseObject
        {
            LastActionErrors = null;

            object obj;
            T responseObj = default(T);

            try
            {
                var request = CreateRequest(data);

                using (var response = request.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        if (stream != null)
                        {
#if !DEBUG
                            // для Release режима десериализуем сразу из потока
                            obj = Deserialize<T>(stream);
#endif
#if DEBUG
                            // для режима отладки десериализуем так,
                            // чтобы можно было посмотреть ответ сервера
                            string result;
                            using (var reader = new StreamReader(stream))
                            {
                                result = reader.ReadToEnd();
                            }
                            using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(result)))
                            {
                                obj = Deserialize<T>(memoryStream);
                            }
#endif
                            var errorResponse = obj as GrastinErrorResponse;
                            if (errorResponse != null)
                            {
                                LastActionErrors = errorResponse.Errors;
                                Debug.Log.Error(string.Format("Grastin errors: {0}",
                                    errorResponse.Errors != null ? string.Join(" ", errorResponse.Errors) : string.Empty));
                            }
                            else
                                responseObj = (T) obj;
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                using (var eResponse = ex.Response)
                {
                    if (eResponse != null)
                    {
                        using (var eStream = eResponse.GetResponseStream())
                            if (eStream != null)
                                using (var reader = new StreamReader(eStream))
                                {
                                    var error = reader.ReadToEnd();
                                    LastActionErrors = new List<string>() { error };
                                    Debug.Log.Error(error, ex);
                                }
                            else
                                Debug.Log.Error(ex);
                    }
                    else
                        Debug.Log.Error(ex);
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }

            return responseObj;
        }

        private object Deserialize<T>(Stream stream) where T : class, new()
        {
            using (var reader = XmlReader.Create(stream))
            {

                reader.MoveToContent();

                var isError = reader.Name ==
                              typeof(GrastinErrorResponse).GetCustomAttributes<XmlRootAttribute>().First().ElementName;


                XmlSerializer deserializer = new XmlSerializer(isError ? typeof(GrastinErrorResponse) : typeof(T));
                return deserializer.Deserialize(reader);
            }
        }

        private HttpWebRequest CreateRequest<TD>(TD data)
            where TD : GrastinBaseObject
        {
            data.ApiKey = _apiKey;

            string dataText;
            using (MemoryStream stream = new MemoryStream())
            {
                WriteDataToStream(stream, data);

                stream.Seek(0, SeekOrigin.Begin);
                dataText = Encoding.UTF8.GetString(stream.ToArray());
            }
            var byteArray = Encoding.UTF8.GetBytes(string.Format("XMLPackage={0}", dataText));


            var request = (HttpWebRequest) WebRequest.Create("http://api.grastin.ru/api.php");
            request.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
            request.Method = "POST";
            //request.Accept = "application/xml";

            using (Stream dataStream = request.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
            }
            return request;
        }

        private void WriteDataToStream<T>(Stream stream, T data)
            where T : GrastinBaseObject
        {
            using (var writer = XmlWriter.Create(stream, new XmlWriterSettings { Encoding = Encoding.UTF8, Indent = true, CheckCharacters = true, OmitXmlDeclaration = true }))
            {
                if (data != null)
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    XmlSerializerNamespaces ns = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
                    ns.Add("", "");
                    serializer.Serialize(writer, data, ns);
                }
            }
        }

        #endregion
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using AdvantShop.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AdvantShop.Module.MoySklad.Domain
{
    #region IEnumerator

    /// <summary>
    /// Запрашивает коллекцию объектво частями, по мере чтения.
    /// </summary>
    /// <typeparam name="T">Тип объектво списка.</typeparam>
    /// <typeparam name="TR">Тип объекта, который представляет ответ сервера.</typeparam>
    public class EnumeratObject<T, TR> : IEnumerator<T>, IEnumerable<T>
        where T : class
        where TR : class, IEntityList<T>
    {
        private List<T> _collection;
        private int _index;
        private readonly string _url;
        private readonly byte _limit;
        private readonly bool _keepActualCollection; // хранить только последнюю полученную коллекцию, чтобы не хранить всю коллекцию в памяти
        private int _offset;
        private MetaData _lastMetaData;
        private T _current;

        private EnumeratObject()
        {
            _index = -1;
        }

        /// <param name="url">Размещение списка</param>
        /// <param name="limit">Максимальное количество сущностей для извлечения.</param>
        /// <param name="keepActualCollection">Хранение в памяти только последней полученной коллекции. Оптималь подходит для единоразового прочтения коллекции.</param>
        public EnumeratObject(string url, byte limit = 100, bool keepActualCollection = false) : this()
        {
            _url = url;
            _limit = limit;
            _keepActualCollection = keepActualCollection;
        }

        public bool MoveNext()
        {
            if (_collection == null || (_index + 1 >= _collection.Count && _lastMetaData.Size >= _offset))
            {
                var response = MoySkladApiService.MakeRequest<TR>(string.Format("{0}{1}limit={2}&offset={3}", _url, (_url.Contains("?") ? "&" : "?"), _limit, _offset), method: "GET");
                if (response != null)
                {
                    _collection = _collection ?? (_collection = new List<T>());
                    if (_keepActualCollection)
                    {
                        _collection.Clear();
                        _index = -1;
                    }

                    _collection.AddRange(response.Rows);

                    _offset += _limit;
                    _lastMetaData = response.Meta;
                }
            }
            if (_collection == null || ++_index >= _collection.Count)
            {
                return false;
            }
            _current = _collection[_index];
            return true;
        }

        public void Reset()
        {
            _index = -1;
            if (_keepActualCollection)
            {
                _collection = null;
                _lastMetaData = null;
                _offset = 0;
            }
        }

        public T Current
        {
            get { return _current; }
        }


        object IEnumerator.Current
        {
            get { return Current; }
        }

        public void Dispose() { }

        public IEnumerator<T> GetEnumerator()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    #endregion

    public class MoySkladApiService
    {
        private const string MoySkladApiUrl = "https://online.moysklad.ru/api/remap/1.1";
        private const int MillisecondsBetweenRequests = 200; // ограничение API, не более 5 запросов в секунду
        private static DateTime _lastRequestApi = DateTime.UtcNow.AddHours(-1);

        #region Private methods

        private static string SerializeObject(object value)
        {
            return JsonConvert.SerializeObject(value,
                Formatting.None,
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    NullValueHandling = NullValueHandling.Ignore
                });
        }

        internal static T MakeRequest<T>(string url, string data = null, string method = "POST", string contentType = "application/json") where T : class
        {
            try
            {
                TimeSpan time = DateTime.UtcNow - _lastRequestApi;
                if (time.TotalMilliseconds < MillisecondsBetweenRequests)
                    Thread.Sleep(time);

                var request = WebRequest.Create(MoySkladApiUrl + url) as HttpWebRequest;
                request.Method = method;
                request.Credentials = new NetworkCredential(MoySkladApiSettings.Login, MoySkladApiSettings.Password);
                request.ContentType = contentType;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls;

                if (!string.IsNullOrEmpty(data))
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(data);
                    request.ContentLength = bytes.Length;

                    using (var requestStream = request.GetRequestStream())
                    {
                        requestStream.Write(bytes, 0, bytes.Length);
                        requestStream.Close();
                    }
                }

                _lastRequestApi = DateTime.UtcNow;

                var responseContent = "";
                using (var response = request.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        if (stream != null)
                            using (var reader = new StreamReader(stream))
                            {
                                responseContent = reader.ReadToEnd();
                            }
                    }
                }

                return JsonConvert.DeserializeObject<T>(responseContent);
            }
            catch (WebException ex)
            {
                try
                {
                    var result = "";

                    using (var eResponse = ex.Response)
                    {
                        if (eResponse != null)
                        {
                            if (!string.IsNullOrEmpty(eResponse.Headers["X-Lognex-Auth"]))
                                result += string.Format("Расширенный код ошибки {0}. ",
                                    eResponse.Headers["X-Lognex-Auth"]);

                            if (!string.IsNullOrEmpty(eResponse.Headers["X-Lognex-Auth-Message"]))
                                result += string.Format("{0}. ", eResponse.Headers["X-Lognex-Auth-Message"]);

                            if (!string.IsNullOrEmpty(eResponse.Headers["X-Lognex-API-Version-Deprecated"]))
                                result += string.Format("Дата отключения запрошенной версии API {0}. ",
                                    eResponse.Headers["X-Lognex-API-Version-Deprecated"]);

                            using (var eStream = eResponse.GetResponseStream())
                            {
                                using (var reader = new StreamReader(eStream))
                                {
                                    result += reader.ReadToEnd();
                                    Debug.Log.Error("MoySkaldApiService. Error on url: " + url + ". " + result);
                                }
                            }
                        }
                    }
                }
                catch (Exception exIn)
                {
                    Debug.Log.Error("MoySkaldApiService. Error on url: " + url, exIn);
                }
                Debug.Log.Error("MoySkaldApiService. Error on url: " + url, ex);
            }
            catch (Exception ex)
            {
                Debug.Log.Error("MoySkaldApiService. Error on url: " + url, ex);
            }

            return null;
        }

        /// <summary>
        /// Запрашивает коллекцию объектво всю сразу (множеством запросов).
        /// </summary>
        /// <typeparam name="T">Тип объектво списка.</typeparam>
        /// <typeparam name="TR">Тип объекта, который представляет ответ сервера.</typeparam>
        /// <param name="url">Размещение списка</param>
        /// <param name="limit">Максимальное количество сущностей для извлечения.</param>
        /// <returns>Коллекция объектво.</returns>
        internal static List<T> GetEntityList<T, TR>(string url, byte limit = 100)
            where T : class
            where TR : class, IEntityList<T>
        {
            List<T> result = null;
            var offset = 0;
            TR response;

            do
            {
                response = MakeRequest<TR>(string.Format("{0}?limit={1}&offset={2}", url, limit, offset), method: "GET");
                if (response != null)
                {
                    (result ?? (result = new List<T>()))
                        .AddRange(response.Rows);

                    offset += limit;
                }
            } while (response != null && response.Meta.Size >= offset);

            return result;
        }

        #endregion

        #region Moy sklad api methods

        public static EntityProductMetadataResponse GetMetadataProduct()
        {
            return MakeRequest<EntityProductMetadataResponse>("/entity/product/metadata", method: "GET");
        }

        public static List<EntityProductResponse> GetProducts()
        {
            return GetEntityList<EntityProductResponse, EntityProductsResponse>("/entity/product");
        }

        /// <param name="optimalUseOfMemory">Хранение в памяти только последней полученной коллекции. Оптималь подходит для единоразового прочтения коллекции.</param>
        public static IEnumerable<EntityProductResponse> GetEnumeratorProducts(bool optimalUseOfMemory = false)
        {
            return new EnumeratObject<EntityProductResponse, EntityProductsResponse>("/entity/product", keepActualCollection: optimalUseOfMemory);
        }

        public static EntityProductResponse AddProduct(EntityProduct product)
        {
            return MakeRequest<EntityProductResponse>("/entity/product", SerializeObject(product));
        }

        public static List<EntityVariantResponse> GetVariants()
        {
            return GetEntityList<EntityVariantResponse, EntityVariantsResponse>("/entity/variant");
        }

        /// <param name="optimalUseOfMemory">Хранение в памяти только последней полученной коллекции. Оптималь подходит для единоразового прочтения коллекции.</param>
        public static IEnumerable<EntityVariantResponse> GetEnumeratorVariants(bool optimalUseOfMemory = false)
        {
            return new EnumeratObject<EntityVariantResponse, EntityVariantsResponse>("/entity/variant", keepActualCollection: optimalUseOfMemory);
        }

        public static EntityProductResponse AddVariant(EntityVariant variant)
        {
            return MakeRequest<EntityProductResponse>("/entity/variant", SerializeObject(variant));
        }

        public static List<EntityProductFolderFromList> GetFolders()
        {
            return GetEntityList<EntityProductFolderFromList, EntityProductFoldersResponse>("/entity/productFolder");
        }

        /// <param name="optimalUseOfMemory">Хранение в памяти только последней полученной коллекции. Оптималь подходит для единоразового прочтения коллекции.</param>
        public static IEnumerable<EntityProductFolderFromList> GetEnumeratorFolders(bool optimalUseOfMemory = false)
        {
            return new EnumeratObject<EntityProductFolderFromList, EntityProductFoldersResponse>("/entity/productFolder", keepActualCollection: optimalUseOfMemory);
        }

        public static EntityProductFolderResponse AddFolder(EntityProductFolder folder)
        {
            return MakeRequest<EntityProductFolderResponse>("/entity/productFolder", SerializeObject(folder));
        }

        public static List<EntityStateResponse> GetStates()
        {
            return GetEntityList<EntityStateResponse, EntityStatesResponse>("/entity/state");
        }

        /// <param name="optimalUseOfMemory">Хранение в памяти только последней полученной коллекции. Оптималь подходит для единоразового прочтения коллекции.</param>
        public static IEnumerable<EntityStateResponse> GetEnumeratorStates(bool optimalUseOfMemory = false)
        {
            return new EnumeratObject<EntityStateResponse, EntityStatesResponse>("/entity/state", keepActualCollection: optimalUseOfMemory);
        }

        public static List<EntityCustomerOrderResponse> GetCustomerOrders()
        {
            return GetEntityList<EntityCustomerOrderResponse, EntityCustomerOrdersResponse>("/entity/customerOrder");
        }

        /// <param name="optimalUseOfMemory">Хранение в памяти только последней полученной коллекции. Оптималь подходит для единоразового прочтения коллекции.</param>
        public static IEnumerable<EntityCustomerOrderResponse> GetEnumeratorCustomerOrders(bool optimalUseOfMemory = false)
        {
            return new EnumeratObject<EntityCustomerOrderResponse, EntityCustomerOrdersResponse>("/entity/customerOrder?expand=state", keepActualCollection: optimalUseOfMemory);
        }

        public static List<EntityCounterpartyResponse> GetCounterparties()
        {
            return GetEntityList<EntityCounterpartyResponse, EntityCounterpartiesResponse>("/entity/counterparty");
        }

        /// <param name="optimalUseOfMemory">Хранение в памяти только последней полученной коллекции. Оптималь подходит для единоразового прочтения коллекции.</param>
        public static IEnumerable<EntityCounterpartyResponse> GetEnumeratorCounterparties(bool optimalUseOfMemory = false)
        {
            return new EnumeratObject<EntityCounterpartyResponse, EntityCounterpartiesResponse>("/entity/counterparty", keepActualCollection: optimalUseOfMemory);
        }

        public static EntityCounterpartyResponse GetCounterparty(string counterpartyId)
        {
            return MakeRequest<EntityCounterpartyResponse>("/entity/counterparty/" + counterpartyId, null, "GET");
        }

        public static EntityCounterpartyResponse AddCounterparty(EntityCounterparty counterparty)
        {
            return MakeRequest<EntityCounterpartyResponse>("/entity/counterparty", SerializeObject(counterparty));
        }

        public static EntityCounterpartyResponse UpdateCounterparty(EntityCounterparty counterparty)
        {
            return MakeRequest<EntityCounterpartyResponse>("/entity/counterparty/" + counterparty.Id, SerializeObject(counterparty), "PUT");
        }

        public static List<EntityCounterpartyContactpersonResponse> GetCounterpartyContactpersons(string counterpartyId)
        {
            return GetEntityList<EntityCounterpartyContactpersonResponse, EntityCounterpartyContactpersonsResponse>("/entity/counterparty/" + counterpartyId + "/contactpersons");
        }

        /// <param name="optimalUseOfMemory">Хранение в памяти только последней полученной коллекции. Оптималь подходит для единоразового прочтения коллекции.</param>
        public static IEnumerable<EntityCounterpartyContactpersonResponse> GetEnumeratorCounterpartyContactpersons(string counterpartyId, bool optimalUseOfMemory = false)
        {
            return new EnumeratObject<EntityCounterpartyContactpersonResponse, EntityCounterpartyContactpersonsResponse>("/entity/counterparty/" + counterpartyId + "/contactpersons", keepActualCollection: optimalUseOfMemory);
        }

        public static EntityCounterpartyContactpersonResponse GetCounterpartyContactperson(string counterpartyContactpersonId, string counterpartyId)
        {
            return MakeRequest<EntityCounterpartyContactpersonResponse>("/entity/counterparty/" + counterpartyId + "/contactpersons/" + counterpartyContactpersonId, null, "GET");
        }

        public static List<EntityCounterpartyContactpersonResponse> AddCounterpartyContactperson(EntityCounterpartyContactperson counterpartyContactperson, string counterpartyId)
        {
            return MakeRequest<List<EntityCounterpartyContactpersonResponse>>("/entity/counterparty/" + counterpartyId + "/contactpersons", SerializeObject(counterpartyContactperson));//counterpartyContactperson.Agent.Meta.Href.Split('/').Last()
        }

        public static EntityCounterpartyContactpersonResponse UpdateCounterpartyContactperson(EntityCounterpartyContactperson counterpartyContactperson, string counterpartyId)
        {
            return MakeRequest<EntityCounterpartyContactpersonResponse>("/entity/counterparty/" + counterpartyId + "/contactpersons/" + counterpartyContactperson.Id, SerializeObject(counterpartyContactperson), "PUT");//counterpartyContactperson.Agent.Meta.Href.Split('/').Last()
        }

        #endregion
    }
}
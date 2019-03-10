using System;
using AdvantShop.Catalog;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Services.Crm;
using AdvantShop.Core.Services.FullSearch;
using AdvantShop.Customers;
using AdvantShop.Helpers;
using AdvantShop.Orders;
using AdvantShop.Repository;
using AdvantShop.Web.Admin.Models.Search;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdvantShop.Diagnostics;
using System.Threading.Tasks;

namespace AdvantShop.Web.Admin.Handlers.Search
{
    public class GetSearchAutocomplete
    {
        private readonly string _type;
        private readonly string _q;
        private readonly UrlHelper _url;
        private int _itemsCount = 5;

        public GetSearchAutocomplete(string q, string type)
        {
            _type = type;
            _q = HttpUtility.HtmlEncode(q);
            _url = new UrlHelper(HttpContext.Current.Request.RequestContext);
        }

        public List<SearchAutocompleteModel> Execute()
        {
            var model = new List<SearchAutocompleteModel>();


            try
            {
                if (!string.IsNullOrEmpty(_type))
                {
                    _itemsCount = 15;

                    switch (_type)
                    {
                        case "settings":
                            model.Add(GetSettings(_q));
                            break;
                        case "categories":
                            model.Add(GetCategories(_q));
                            break;
                        case "products":
                            model.Add(GetProducts(_q));
                            break;
                        case "customers":
                            model.Add(GetCustomers(_q));
                            break;
                        case "orders":
                            model.Add(GetOrders(_q));
                            break;
                        case "leads":
                            model.Add(GetLeads(_q));
                            break;
                        case "tasks":
                            model.Add(GetTasks(_q));
                            break;
                        case "modules":
                            model.Add(GetModules(_q));
                            break;
                    }
                }
                else
                {

                    var searchTasks = new List<Task<List<SearchAutocompleteModel>>>();

                    searchTasks.Add(GetAsync(GetSettings, _q));
                    searchTasks.Add(GetAsync(GetCategories, _q));
                    searchTasks.Add(GetAsync(GetProducts, _q));
                    searchTasks.Add(GetAsync(GetCustomers, _q));
                    searchTasks.Add(GetAsync(GetOrders, _q));
                    searchTasks.Add(GetAsync(GetSettings, _q));

                    if (!Saas.SaasDataService.IsSaasEnabled || Saas.SaasDataService.CurrentSaasData.HaveCrm)
                    {
                        searchTasks.Add(GetAsync(GetLeads, _q));
                        searchTasks.Add(GetAsync(GetTasks, _q));
                    }

                    searchTasks.Add(GetAsync(GetModules, _q));

                    model = searchTasks.Select(x => x.Result).SelectMany(x => x).Where(x => x != null).ToList();

                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }

            return model;
        }

        private Task<List<SearchAutocompleteModel>> GetAsync(Func<string, SearchAutocompleteModel> action, string param)
        {
            return System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                Localization.Culture.InitializeCulture();
                return new List<SearchAutocompleteModel>
                {
                    action(param)
                };
            });
        }

        private SearchAutocompleteModel GetProducts(string q)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            var tanslitQ = StringHelper.TranslitToRusKeyboard(q);
            var productIds =
                ProductSeacherAdmin.Search(q)
                    .SearchResultItems.Select(item => item.Id)
                    .Union(ProductSeacherAdmin.Search(tanslitQ).SearchResultItems.Select(item => item.Id))
                    .Distinct()
                    .ToList();

            var products = productIds.Take(_itemsCount).Select(ProductService.GetProduct).Where(x => x != null).ToList();

            var model = new SearchAutocompleteModel()
            {
                Category = "Товары",
                Items = products.Select(product => new SearchAutocompleteItem()
                {
                    Text = product.Name,
                    Description = "атрикул: " + product.ArtNo,
                    Url = _url.Action("Edit", "Product", new { id = product.ProductId })
                }).ToList(),
            };

            if (productIds.Count > _itemsCount)
            {
                model.More = new SearchAutocompleteItem()
                {
                    Text = string.Format("Все результаты ({0})", productIds.Count()),
                    Url = _url.Action("Index", "Catalog", new { showMethod = "AllProducts", search = q })
                };
            }

            watch.Stop();
            model.Time = watch.ElapsedMilliseconds;
            return model;
        }

        private SearchAutocompleteModel GetCategories(string q)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            var tanslitQ = StringHelper.TranslitToRusKeyboard(q);
            var categoryIds =
                CategorySeacher.Search(q)
                    .SearchResultItems.Select(item => item.Id)
                    .Union(CategorySeacher.Search(tanslitQ).SearchResultItems.Select(item => item.Id))
                    .Distinct()
                    .ToList();

            var categories = categoryIds.Take(_itemsCount).Select(CategoryService.GetCategory).Where(x => x != null).ToList();


            var model = new SearchAutocompleteModel()
            {
                Category = "Категории",
                Items = categories.Select(category => new SearchAutocompleteItem()
                {
                    Text = (category.ParentCategory != null && category.ID != 0 ? category.ParentCategory.Name + " > " : "") + category.Name,
                    Url = _url.Action("Edit", "Category", new { id = category.CategoryId })
                }).ToList()
            };


            if (categoryIds.Count > _itemsCount)
            {
                model.More = new SearchAutocompleteItem()
                {
                    Text = string.Format("Все результаты ({0})", categoryIds.Count()),
                    Url = _url.Action("Index", "Catalog", new { categorySearch = q })
                };
            }


            watch.Stop();
            model.Time = watch.ElapsedMilliseconds;
            return model;
        }

        private SearchAutocompleteModel GetLeads(string q)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            List<Lead> leads = new List<Lead>();
            foreach (var word in q.Split(" "))
            {
                leads.AddRange(LeadService.GetLeadsForAutocomplete(word).Where(lead => !leads.Any(l => l.Id == lead.Id)));
            }

            var model = new SearchAutocompleteModel()
            {
                Category = "Лиды",
                Items = leads.Take(_itemsCount).Select(lead => new SearchAutocompleteItem()
                {
                    Text = "№" + lead.Id + ", " + (lead.DealStatus != null ? lead.DealStatus.Name : ""),
                    Url = _url.Action("Edit", "Leads", new { id = lead.Id })
                }).ToList()
            };

            watch.Stop();
            model.Time = watch.ElapsedMilliseconds;
            return model;
        }

        private SearchAutocompleteModel GetTasks(string q)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            List<Core.Services.Crm.Task> tasks = new List<Core.Services.Crm.Task>();

            foreach (var word in q.Split(" "))
            {
                tasks.AddRange(TaskService.GetTasksForAutocomplete(word).Where(task => !tasks.Any(t => t.Id == task.Id)));
            }
            var model = new SearchAutocompleteModel()
            {
                Category = "Задачи",
                Items = tasks.Take(_itemsCount).Select(task => new SearchAutocompleteItem()
                {
                    Text = "№" + task.Id + ", " + task.Name,
                    Url = _url.Action("Edit", "Tasks", new { id = task.Id })
                }).ToList()
            };

            watch.Stop();
            model.Time = watch.ElapsedMilliseconds;
            return model;
        }

        private SearchAutocompleteModel GetModules(string q)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            var tanslitQ = StringHelper.TranslitToRusKeyboard(q);

            var modules = ModulesRepository.GetModulesFromDb().Where(m => m.StringId.Contains(q) || (m.Name != null && (m.Name.Contains(q) || m.Name.Contains(tanslitQ)))).Take(_itemsCount);
            var model = new SearchAutocompleteModel()
            {
                Category = "Модули",
                Items = modules.Select(module => new SearchAutocompleteItem()
                {
                    Text = module.Name,
                    Url = _url.Action("Details", "Modules", new { id = module.StringId })
                }).ToList()
            };

            watch.Stop();
            model.Time = watch.ElapsedMilliseconds;
            return model;
        }

        private SearchAutocompleteModel GetOrders(string q)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            List<OrderAutocomplete> orders = new List<OrderAutocomplete>();

            foreach (var word in q.Split(" "))
            {
                orders.AddRange(OrderService.GetOrdersForAutocomplete(word).Where(order => !orders.Any(o => o.OrderID == order.OrderID)));
            }

            var model = new SearchAutocompleteModel()
            {
                Category = "Заказы",
                Items = orders.Take(_itemsCount).Select(order => new SearchAutocompleteItem()
                {
                    Text = "№" + order.Number + ", " + order.StatusName,
                    Url = _url.Action("Edit", "Orders", new { id = order.OrderID })
                }).ToList()
            };

            watch.Stop();
            model.Time = watch.ElapsedMilliseconds;
            return model;
        }


        private SearchAutocompleteModel GetSettings(string q)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            var settings = SettingsSearchService.GetSettingsSearchForAutocomplete(q).Take(_itemsCount);
            var model = new SearchAutocompleteModel()
            {
                Category = "Настройки",
                Items = settings.Select(setting => new SearchAutocompleteItem()
                {
                    Text = setting.Title,
                    Url = setting.Link
                }).ToList()
            };

            watch.Stop();
            model.Time = watch.ElapsedMilliseconds;
            return model;
        }


        private SearchAutocompleteModel GetCustomers(string q)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            var model = new SearchAutocompleteModel()
            {
                Category = "Покупатели",
                Items = new List<SearchAutocompleteItem>()
            };

            if (_type != "customers")
            {
                var customersByClientCode = ClientCodeService.SearchCustomers2(q).Take(_itemsCount).ToList();
                if (customersByClientCode.Count > 0)
                {
                    model.Items.AddRange(customersByClientCode.Select(customer => new SearchAutocompleteItem()
                    {
                        Text = new[] { customer.Code.ToString(), (customer.FirstName + " " + customer.LastName).Trim(), customer.Phone }.Where(str => str.IsNotEmpty()).AggregateString(", "),
                        Url = _url.Action("Edit", "Customers", new { id = customer.Id != Guid.Empty ? customer.Id : (Guid?)null, code = customer.Code })
                    }));
                }
            }

            List<Customer> customers = new List<Customer>();

            foreach (var word in q.Split(" "))
            {
                customers.AddRange(CustomerService.GetCustomersForAutocomplete(word).Where(customer => !customers.Any(c => c.Id == customer.Id)));
            }


            model.Items.AddRange(customers.Take(_itemsCount).Select(customer => new SearchAutocompleteItem()
            {
                Text = new[] { (customer.FirstName + " " + customer.LastName).Trim(), customer.EMail, customer.Phone }.Where(str => str.IsNotEmpty()).AggregateString(", "),
                Url = _url.Action("Edit", "Customers", new { id = customer.Id }),
                Id = customer.Id.ToString()
            }));

            watch.Stop();
            model.Time = watch.ElapsedMilliseconds;
            return model;
        }
    }
}

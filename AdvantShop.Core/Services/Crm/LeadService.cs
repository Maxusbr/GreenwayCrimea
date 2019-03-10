using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.Services.Crm.DealStatuses;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Core.Services.Loging;
using AdvantShop.Core.Services.Loging.TrafficSource;
using AdvantShop.Core.SQL;
using AdvantShop.Customers;
using AdvantShop.Mails;
using AdvantShop.Repository.Currencies;
using AdvantShop.FilePath;
using AdvantShop.Core.Services.Crm.BusinessProcesses;
using AdvantShop.Core.UrlRewriter;
using Newtonsoft.Json;

namespace AdvantShop.Core.Services.Crm
{
	public class LeadService
	{
		#region Lead

		public static Lead GetLead(int id)
		{
			return SQLDataAccess.Query<Lead>("Select * From [Order].[Lead] Where Id=@Id", new { Id = id }).FirstOrDefault();
		}

		public static List<Lead> GetAllLeads()
		{
			return SQLDataAccess.Query<Lead>("Select * From [Order].[Lead]").ToList();
		}

		public static void AddLead(Lead lead, bool isFromAdminArea = false)
		{
			var customerAdded = false;
			if (lead.Customer != null && !lead.Customer.RegistredUser)
			{
				lead.CustomerId = CustomerService.InsertNewCustomer(lead.Customer); // если пользователь с таким email существует, то Guid.Empty
				customerAdded = lead.CustomerId != Guid.Empty;
			}

			if (!customerAdded && lead.CustomerId != null && lead.Customer != null)
			{
				Customer customer = null;

				if (lead.CustomerId != null && lead.CustomerId != Guid.Empty)
					customer = CustomerService.GetCustomer(lead.CustomerId.Value);

				if (customer == null && !string.IsNullOrEmpty(lead.Email))
					customer = CustomerService.GetCustomerByEmail(lead.Email);

				if (customer == null && !string.IsNullOrEmpty(lead.Phone))
					customer = CustomerService.GetCustomersByPhone(lead.Phone).FirstOrDefault();


				if (customer != null)
				{
					lead.CustomerId = customer.Id;
					var updateCustomer = false;

					if (!string.IsNullOrWhiteSpace(lead.Customer.FirstName) && lead.Customer.FirstName != customer.FirstName)
					{
						customer.FirstName = lead.Customer.FirstName;
						updateCustomer = true;
					}

					if (!string.IsNullOrWhiteSpace(lead.Customer.LastName) && lead.Customer.LastName != customer.LastName)
					{
						customer.LastName = lead.Customer.LastName;
						updateCustomer = true;
					}

					if (!string.IsNullOrWhiteSpace(lead.Customer.Patronymic) && lead.Customer.Patronymic != customer.Patronymic)
					{
						customer.Patronymic = lead.Customer.Patronymic;
						updateCustomer = true;
					}

					if (!string.IsNullOrWhiteSpace(lead.Customer.Phone) && lead.Customer.Phone != customer.Phone)
					{
						customer.Phone = lead.Customer.Phone;
						updateCustomer = true;
					}

					if (!string.IsNullOrWhiteSpace(lead.Customer.EMail) && string.IsNullOrWhiteSpace(customer.EMail))
					{
						customer.EMail = lead.Customer.EMail;
						updateCustomer = true;
					}

					if (updateCustomer)
						CustomerService.UpdateCustomer(customer);
				}
			}

			if (lead.DealStatusId == 0)
			{
				var dealStatus = DealStatusService.GetList().FirstOrDefault();
				if (dealStatus != null)
					lead.DealStatusId = dealStatus.Id;
			}

			lead.Id = SQLDataAccess.ExecuteScalar<int>(
				"Insert Into [Order].[Lead] " +
					"(CustomerId,FirstName,LastName,Patronymic,Description,Sum,Phone,Email,ManagerId,CreatedDate,Discount,DiscountValue,OrderSourceId,Comment," +
					"DealStatusId,IsFromAdminArea,DeliveryDate,DeliveryTime,ShippingMethodId,ShippingName,ShippingCost,ShippingPickPoint) " +
				"Values " +
					"(@CustomerId,@FirstName,@LastName,@Patronymic,@Description,@Sum,@Phone,@Email,@ManagerId,@CreatedDate,@Discount,@DiscountValue,@OrderSourceId,@Comment," +
					"@DealStatusId,@IsFromAdminArea,@DeliveryDate,@DeliveryTime,@ShippingMethodId,@ShippingName,@ShippingCost,@ShippingPickPoint); " +
				"SELECT scope_identity();",
				CommandType.Text,
				new SqlParameter("@CustomerId", lead.CustomerId ?? (object)DBNull.Value),
				new SqlParameter("@FirstName", lead.FirstName ?? string.Empty),
				new SqlParameter("@LastName", lead.LastName ?? string.Empty),
				new SqlParameter("@Patronymic", lead.Patronymic ?? string.Empty),
				new SqlParameter("@Description", lead.Description ?? string.Empty),
				new SqlParameter("@Sum", lead.Sum),
				new SqlParameter("@Phone", lead.Phone ?? string.Empty),
				new SqlParameter("@Email", lead.Email ?? string.Empty),
				new SqlParameter("@ManagerId", lead.ManagerId ?? (object)DBNull.Value),
				new SqlParameter("@Discount", lead.Discount),
				new SqlParameter("@DiscountValue", lead.DiscountValue),
				new SqlParameter("@OrderSourceId", lead.OrderSourceId),
				new SqlParameter("@Comment", lead.Comment ?? string.Empty),
				new SqlParameter("@DealStatusId", lead.DealStatusId),
				new SqlParameter("@IsFromAdminArea", lead.IsFromAdminArea),
				new SqlParameter("@CreatedDate", lead.CreatedDate != DateTime.MinValue ? lead.CreatedDate : DateTime.Now),

				new SqlParameter("@DeliveryDate", lead.DeliveryDate ?? (object)DBNull.Value),
				new SqlParameter("@DeliveryTime", lead.DeliveryTime ?? (object)DBNull.Value),
				new SqlParameter("@ShippingMethodId", lead.ShippingMethodId != 0 ? lead.ShippingMethodId : (object)DBNull.Value),
				new SqlParameter("@ShippingName", lead.ShippingName ?? (object)DBNull.Value),
				new SqlParameter("@ShippingCost", lead.ShippingCost),
				new SqlParameter("@ShippingPickPoint", lead.ShippingPickPoint ?? (object)DBNull.Value)
			);

			if (lead.LeadCurrency == null)
				lead.LeadCurrency = CurrencyService.CurrentCurrency;

			AddLeadCurrency(lead.Id, lead.LeadCurrency);

			if (lead.LeadItems == null)
				lead.LeadItems = new List<LeadItem>();

			foreach (var item in lead.LeadItems)
				AddLeadItem(lead.Id, item);


			var leadItemsTable = string.Empty;
			if (lead.LeadItems.Count > 0)
				leadItemsTable = GenerateHtmlLeadItemsTable(lead.LeadItems, lead.LeadCurrency);

			var mailTemplate = new LeadMailTemplate(lead.Id.ToString(), lead.FirstName, lead.Phone, lead.Comment, leadItemsTable, lead.Email);
			mailTemplate.BuildMail();

			if (lead.Customer != null && lead.Customer.EMail.IsNotEmpty())
			{
				SendMail.SendMailNow(Guid.Empty, lead.Customer.EMail, mailTemplate.Subject, mailTemplate.Body, true);
			}

			SendMail.SendMailNow(Guid.Empty, SettingsMail.EmailForLeads, mailTemplate.Subject, mailTemplate.Body, true);

			var loger = LogingManager.GetTrafficSourceLoger();
			loger.LogOrderTafficSource(lead.Id, TrafficSourceType.Lead, isFromAdminArea);

			BizProcessExecuter.LeadAdded(lead);

			ModulesExecuter.LeadAdded(lead);
		}

		public static void UpdateLead(Lead lead)
		{
			var prevState = GetLead(lead.Id);

			SQLDataAccess.ExecuteNonQuery(
				"Update [Order].[Lead] " +
					"Set CustomerId = @CustomerId, " +
						"FirstName = @FirstName, " +
						"LastName = @LastName, " +
						"Patronymic = @Patronymic, " +
						"Description = @Description, " +
						"Sum = @Sum, " +
						"Phone = @Phone, " +
						"Email = @Email, " +
						"OrderSourceId = @OrderSourceId," +
						"ManagerId = @ManagerId, " +
						"Discount = @Discount, " +
						"DiscountValue = @DiscountValue, " +
						"DealStatusId = @DealStatusId, " +

						"DeliveryDate = @DeliveryDate, " +
						"DeliveryTime = @DeliveryTime, " +
						"ShippingMethodId = @ShippingMethodId, " +
						"ShippingName = @ShippingName, " +
						"ShippingCost = @ShippingCost, " +
						"ShippingPickPoint = @ShippingPickPoint " +

				"Where Id = @Id",
				CommandType.Text,
				new SqlParameter("@Id", lead.Id),
				new SqlParameter("@CustomerId", lead.CustomerId ?? (object)DBNull.Value),
				new SqlParameter("@FirstName", lead.FirstName ?? string.Empty),
				new SqlParameter("@LastName", lead.LastName ?? string.Empty),
				new SqlParameter("@Patronymic", lead.Patronymic ?? string.Empty),
				new SqlParameter("@Description", lead.Description ?? string.Empty),
				new SqlParameter("@Sum", lead.Sum),
				new SqlParameter("@Phone", lead.Phone ?? string.Empty),
				new SqlParameter("@Email", lead.Email ?? string.Empty),
				new SqlParameter("@ManagerId", lead.ManagerId ?? (object)DBNull.Value),
				new SqlParameter("@Discount", lead.Discount),
				new SqlParameter("@DiscountValue", lead.DiscountValue),
				new SqlParameter("@OrderSourceId", lead.OrderSourceId),
				new SqlParameter("@DealStatusId", lead.DealStatusId),

				new SqlParameter("@DeliveryDate", lead.DeliveryDate ?? (object)DBNull.Value),
				new SqlParameter("@DeliveryTime", lead.DeliveryTime ?? (object)DBNull.Value),
				new SqlParameter("@ShippingMethodId", lead.ShippingMethodId != 0 ? lead.ShippingMethodId : (object)DBNull.Value),
				new SqlParameter("@ShippingName", lead.ShippingName ?? (object)DBNull.Value),
				new SqlParameter("@ShippingCost", lead.ShippingCost),
				new SqlParameter("@ShippingPickPoint", lead.ShippingPickPoint ?? (object)DBNull.Value)
				);

			if (lead.CustomerId != null && lead.CustomerId != Guid.Empty && lead.Customer != null)
			{
				CustomerService.UpdateCustomer(lead.Customer);

				if (lead.Customer.Contacts != null && lead.Customer.Contacts.Count > 0)
				{
					foreach (var contact in lead.Customer.Contacts)
					{
						if (contact.ContactId == Guid.Empty ||
							CustomerService.GetCustomerContact(contact.ContactId.ToString()) == null)
						{
							CustomerService.AddContact(contact, lead.Customer.Id);
						}
						else
						{
							CustomerService.UpdateContact(contact);
						}
					}
				}
			}
			else if (lead.Customer != null)
			{
				CustomerService.InsertNewCustomer(lead.Customer);
			}

			var leadItems = GetLeadItems(lead.Id);
			foreach (var item in leadItems)
			{
				if (lead.LeadItems.Find(x => x.LeadItemId == item.LeadItemId) == null)
				{
					DeleteLeadItem(item);
				}
			}

			foreach (var item in lead.LeadItems)
			{
				if (leadItems.Find(x => x.LeadItemId == item.LeadItemId) != null)
				{
					UpdateLeadItem(lead.Id, item);
				}
				else
				{
					AddLeadItem(lead.Id, item);
				}
			}

			if (prevState.DealStatusId != lead.DealStatusId)
			{
				BizProcessExecuter.LeadStatusChanged(lead);

				var dealStatus = DealStatusService.Get(lead.DealStatusId);
				if (dealStatus != null)
					LeadEventService.AddEvent(new LeadEvent()
					{
						LeadId = lead.Id,
						Message = LocalizationService.GetResourceFormat("Core.Lead.DealStatusChangedFormat", dealStatus.Name),
						Type = LeadEventType.Other,
						CreatedBy = CustomerContext.CurrentCustomer != null ? CustomerContext.CurrentCustomer.GetShortName() : ""
					});
			}

			ModulesExecuter.LeadUpdated(lead);
		}

		public static void UpdateLeadManager(int leadId, int? managerId)
		{
			SQLDataAccess.ExecuteNonQuery(
				"UPDATE [Order].[Lead] SET [ManagerId] = @ManagerId WHERE [Id] = @Id",
				CommandType.Text,
				new SqlParameter("@Id", leadId),
				new SqlParameter("@ManagerId", managerId ?? (object)DBNull.Value));
		}

		public static void DeleteLead(int leadId)
		{
			SQLDataAccess.ExecuteNonQuery("Delete From [Order].[Lead] Where Id=@Id", CommandType.Text, new SqlParameter("@Id", leadId));

			ModulesExecuter.LeadDeleted(leadId);
		}

		public static int GetLeadsCount(int? managerId = null)
		{
			var dealStatus = DealStatusService.GetList().FirstOrDefault();
			if (dealStatus == null)
				return 0;

			var sql = "Select Count(Id) From [Order].[Lead] Where DealStatusId IN (@DealStatusId,@DealStatusIdTwo)";
			var dealStatusTwo = DealStatusService.GetList().FirstOrDefault(o => o.Name.Equals("Запрос на регистрацию")) ??
								dealStatus;

			if (managerId != null)
			{
				switch (SettingsManager.ManagersLeadConstraint)
				{
					case ManagersLeadConstraint.Assigned:
						sql += " And ManagerId=@ManagerId ";
						break;

					case ManagersLeadConstraint.AssignedAndFree:
						sql += " And (ManagerId=@ManagerId OR ManagerId is null) ";
						break;
				}

				return
					Convert.ToInt32(
						SQLDataAccess.ExecuteScalar(sql, CommandType.Text,
							new SqlParameter("@DealStatusId", dealStatus.Id),
							new SqlParameter("@DealStatusIdTwo", dealStatusTwo.Id),
							new SqlParameter("@ManagerId", managerId)));
			}

			return Convert.ToInt32(SQLDataAccess.ExecuteScalar(sql, CommandType.Text,
				new SqlParameter("@DealStatusId", dealStatus.Id),
				new SqlParameter("@DealStatusIdTwo", dealStatusTwo.Id)));
		}

		public static List<Lead> GetLeadsByCustomer(Guid customerId)
		{
			return
				SQLDataAccess.Query<Lead>("Select * From [Order].[Lead] Where CustomerId=@customerId", new { customerId }).ToList();
		}

		public static List<Lead> GetLeadsByPhone(string phone)
		{
			return
				SQLDataAccess.Query<Lead>("Select * From [Order].[Lead] Where Phone=@phone", new { phone }).ToList();
		}

		#endregion

		#region Lead Currency

		public static LeadCurrency GetLeadCurrency(int id)
		{
			return SQLDataAccess.Query<LeadCurrency>(
				"SELECT * FROM [Order].[LeadCurrency] WHERE [LeadId] = @LeadId", new { LeadId = id }).FirstOrDefault();
		}

		public static void AddLeadCurrency(int leadId, LeadCurrency currency)
		{
			SQLDataAccess.ExecuteNonQuery(
				"Insert Into [Order].[LeadCurrency] " +
				"(LeadId,CurrencyCode,CurrencyNumCode,CurrencyValue,CurrencySymbol,IsCodeBefore,RoundNumbers,EnablePriceRounding) " +
				"Values " +
				"(@LeadId,@CurrencyCode,@CurrencyNumCode,@CurrencyValue,@CurrencySymbol,@IsCodeBefore,@RoundNumbers,@EnablePriceRounding) ",
				CommandType.Text,
				new SqlParameter("@LeadId", leadId),
				new SqlParameter("@CurrencyCode", currency.CurrencyCode),
				new SqlParameter("@CurrencyNumCode", currency.CurrencyNumCode),
				new SqlParameter("@CurrencyValue", currency.CurrencyValue),
				new SqlParameter("@CurrencySymbol", currency.CurrencySymbol),
				new SqlParameter("@IsCodeBefore", currency.IsCodeBefore),
				new SqlParameter("@RoundNumbers", currency.RoundNumbers),
				new SqlParameter("@EnablePriceRounding", currency.EnablePriceRounding)
				);
		}

		#endregion

		#region Lead Items

		public static List<LeadItem> GetLeadItems(int leadId)
		{
			return
				SQLDataAccess.Query<LeadItem>("Select * From [Order].[LeadItem] Where LeadId=@LeadId",
					new { LeadId = leadId }).ToList();
		}

		public static void AddLeadItem(int leadId, LeadItem item)
		{
			item.LeadItemId = SQLDataAccess.ExecuteScalar<int>(
				"Insert Into [Order].[LeadItem] " +
					"(LeadId,ProductId,Name,ArtNo,Price,Amount,Weight,Color,Size,PhotoId) " +
				"Values " +
					"(@LeadId,@ProductId,@Name,@ArtNo,@Price,@Amount,@Weight,@Color,@Size,@PhotoId); " +
				"SELECT scope_identity();",
				CommandType.Text,
				new SqlParameter("@LeadId", leadId),
				new SqlParameter("@ProductId", item.ProductId ?? (object)DBNull.Value),
				new SqlParameter("@Name", item.Name),
				new SqlParameter("@ArtNo", item.ArtNo),
				new SqlParameter("@Price", item.Price),
				new SqlParameter("@Amount", item.Amount),
				new SqlParameter("@Weight", item.Weight),
				new SqlParameter("@Color", item.Color ?? (object)DBNull.Value),
				new SqlParameter("@Size", item.Size ?? (object)DBNull.Value),
				new SqlParameter("@PhotoId", item.PhotoId != 0 && item.PhotoId != null ? item.PhotoId : (object)DBNull.Value),
				new SqlParameter("@Width", item.Width),
				new SqlParameter("@Length", item.Length),
				new SqlParameter("@Height", item.Height)
				);
		}

		public static void UpdateLeadItem(int leadId, LeadItem item)
		{
			SQLDataAccess.ExecuteNonQuery(
				"Update [Order].[LeadItem] " +
					"Set LeadId=@LeadId, " +
						"ProductId=@ProductId, " +
						"Name=@Name, " +
						"ArtNo=@ArtNo, " +
						"Price=@Price, " +
						"Amount=@Amount, " +
						"Weight=@Weight, " +
						"Color=@Color, " +
						"Size=@Size, " +
						"PhotoId=@PhotoId " +
				"Where LeadItemId=@LeadItemId",
				CommandType.Text,
				new SqlParameter("@LeadItemId", item.LeadItemId),
				new SqlParameter("@LeadId", leadId),
				new SqlParameter("@ProductId", item.ProductId ?? (object)DBNull.Value),
				new SqlParameter("@Name", item.Name),
				new SqlParameter("@ArtNo", item.ArtNo),
				new SqlParameter("@Price", item.Price),
				new SqlParameter("@Amount", item.Amount),
				new SqlParameter("@Weight", item.Weight),
				new SqlParameter("@Color", item.Color ?? (object)DBNull.Value),
				new SqlParameter("@Size", item.Size ?? (object)DBNull.Value),
				new SqlParameter("@PhotoId", item.PhotoId != 0 && item.PhotoId != null ? item.PhotoId : (object)DBNull.Value),
				new SqlParameter("@Width", item.Width),
				new SqlParameter("@Height", item.Height),
				new SqlParameter("@Length", item.Length)
				);
		}

		public static void DeleteLeadItem(LeadItem leadItem)
		{
			SQLDataAccess.ExecuteNonQuery("Delete From [Order].[LeadItem] Where LeadItemId=@LeadItemId",
				CommandType.Text, new SqlParameter("@LeadItemId", leadItem.LeadItemId));
		}

		#endregion

		public static List<Lead> GetLeadsForAutocomplete(string query)
		{
			if (query.IsDecimal())
			{
				return SQLDataAccess.Query<Lead>(
			 "SELECT * FROM [Order].[Lead] " +
			 "Left Join [Customers].[Customer] on [Lead].[CustomerId] = [Customer].[CustomerId] " +
			 "WHERE convert(nvarchar,[id]) = @q " +
			 "OR [Customer].[Email] LIKE @q + '%' " +
			 "OR [Customer].[Phone] LIKE '%' + @q + '%' " +
			 "OR [Customer].[StandardPhone] LIKE '%' + @q + '%'", new { q = query }).ToList();
			}
			else
			{
				return SQLDataAccess.Query<Lead>(
					"SELECT * FROM [Order].[Lead] " +
					"Left Join [Customers].[Customer] on [Lead].[CustomerId] = [Customer].[CustomerId] " +
					"WHERE convert(nvarchar,[id]) = @q " +
					"OR [Customer].[Email] LIKE @q + '%' " +
					"OR [Customer].[FirstName] LIKE @q + '%' " +
					"OR [Customer].[LastName] LIKE @q + '%' " +
					"OR [Customer].[Phone] LIKE '%' + @q + '%' ", new { q = query }).ToList();
			}
		}


		public static string GenerateHtmlLeadItemsTable(IList<LeadItem> leadItems, LeadCurrency leadCurrency)
		{
			var htmlOrderTable = new StringBuilder();

			htmlOrderTable.Append("<table class='orders-table' style='border-collapse: collapse; width: 100%;'>");
			htmlOrderTable.Append("<tr class='orders-table-header'>");
			htmlOrderTable.AppendFormat("<th class='photo' style='border-bottom: 1px solid #e3e3e3; margin-right: 15px; padding: 20px 0; padding-left: 20px; text-align: left;'>{0}</th>", LocalizationService.GetResource("Core.Lead.Letter.Goods"));
			htmlOrderTable.Append("<th class='name' style='border-bottom: 1px solid #e3e3e3; padding: 20px 0; text-align: left; width: 50%;'></th>");
			htmlOrderTable.AppendFormat("<th class='price' style='border-bottom: 1px solid #e3e3e3; padding: 20px 0; text-align: center;'>{0}</th>", LocalizationService.GetResource("Core.Lead.Letter.Price"));
			htmlOrderTable.AppendFormat("<th class='amount' style='border-bottom: 1px solid #e3e3e3; padding: 20px 0; text-align: center;' >{0}</th>", LocalizationService.GetResource("Core.Lead.Letter.Count"));
			htmlOrderTable.AppendFormat("<th class='total-price' style='border-bottom: 1px solid #e3e3e3; padding: 20px 0; text-align: center;'>{0}<span class='curency' style='font-weight: normal;'> ({1})</span></th>", LocalizationService.GetResource("Core.Lead.Letter.Cost"), CurrencyService.CurrentCurrency.Symbol);
			htmlOrderTable.Append("</tr>");

			var currency = new Currency
			{
				Iso3 = leadCurrency.CurrencyCode,
				NumIso3 = leadCurrency.CurrencyNumCode,
				IsCodeBefore = leadCurrency.IsCodeBefore,
				Rate = leadCurrency.CurrencyValue,
				Symbol = leadCurrency.CurrencySymbol,
				RoundNumbers = leadCurrency.RoundNumbers,
				EnablePriceRounding = leadCurrency.EnablePriceRounding
			};
			// Добавление заказанных товаров
			foreach (var item in leadItems)
			{
				if (item.ProductId.HasValue)
				{
					htmlOrderTable.Append("<tr>");
					if (item.ProductId != null)
					{
						Photo photo;
						if (item.PhotoId.HasValue && item.PhotoId != 0 && (photo = PhotoService.GetPhoto((int)item.PhotoId)) != null)
						{
							htmlOrderTable.AppendFormat("<td class='photo' style='border-bottom: 1px solid #e3e3e3; margin-right: 15px; padding: 20px 0; padding-left: 20px; text-align: left;'><img src='{0}' /></td>",
														 FoldersHelper.GetImageProductPath(ProductImageType.XSmall, photo.PhotoName, false), SettingsPictureSize.XSmallProductImageWidth);
						}
						else
						{
							htmlOrderTable.AppendFormat("<td>&nbsp;</td>");
						}
					}

					var product = item.ProductId.HasValue ? ProductService.GetProduct(item.ProductId.Value) : null;

					htmlOrderTable.AppendFormat("<td class='name' style='border-bottom: 1px solid #e3e3e3; padding: 20px 0; text-align: left; width: 50%;'>" +
														"<div class='description' style='display: inline-block;'>" +
															"{0} {1} {2} {3}" +
														"</div>" +
												"</td>",

														 "<div class='prod-name' style='font-size: 18px; font-weight: bold; margin-bottom: 5px;'><a href='" +
															(product != null ?
																SettingsMain.SiteUrl.Trim('/') + "/" + UrlService.GetLink(ParamType.Product, product.UrlPath, product.ProductId)
															: "") +
															"' class='cs-link' style='color: #0764c3; text-decoration: none;'>" + item.Name + "</a></div>",
															item.Color.IsNotEmpty() ? "<div class='prod-option' style='margin-bottom: 5px;'><span class='cs-light' style='color: #acacac;'>" + SettingsCatalog.ColorsHeader + ":</span><span class='value cs-link' style='color: #0764c3; font-weight: bold; padding-left: 10px;'>" + item.Color + "</span></div>" : "",
															item.Size.IsNotEmpty() ? "<div class='prod-option' style='margin-bottom: 5px;'><span class='cs-light' style='color: #acacac;'>" + SettingsCatalog.SizesHeader + ":</span><span class='value cs-link' style='color: #0764c3; font-weight: bold; padding-left: 10px;'>" + item.Size + "</span></div>" : "",
															string.Empty);
					htmlOrderTable.AppendFormat("<td class='price' style='border-bottom: 1px solid #e3e3e3; padding: 20px 0; text-align: center;'>{0}</td>", PriceFormatService.FormatPrice(item.Price, currency));
					htmlOrderTable.AppendFormat("<td class='amount' style='border-bottom: 1px solid #e3e3e3; padding: 20px 0; text-align: center;'>{0}</td>", item.Amount);
					htmlOrderTable.AppendFormat("<td class='total-price' style='border-bottom: 1px solid #e3e3e3; padding: 20px 0; text-align: center;'>{0}</td>", PriceFormatService.FormatPrice(item.Price * item.Amount, currency));
					htmlOrderTable.Append("</tr>");
				}
			}

			// Стоимость заказа
			htmlOrderTable.Append("<tr>");
			htmlOrderTable.AppendFormat("<td class='footer-name' colspan='4' style='border-bottom: none; font-weight: bold; padding: 10px 0; text-align: right;'>{0}:</td>", LocalizationService.GetResource("Core.Lead.Letter.OrderCost"));
			htmlOrderTable.AppendFormat("<td class='footer-value' style='border-bottom: none; font-weight: bold; padding: 10px 0; text-align: center;'>{0}</td>", PriceFormatService.FormatPrice(leadItems.Sum(item => item.Price * item.Amount), currency));
			htmlOrderTable.Append("</tr>");

			var total = leadItems.Sum(item => item.Price * item.Amount);
			if (total < 0) total = 0;

			// Итого
			htmlOrderTable.Append("<tr>");
			htmlOrderTable.AppendFormat("<td class='footer-name' colspan='4' style='border-bottom: none; font-weight: bold; padding: 10px 0; text-align: right;'>{0}:</td>", LocalizationService.GetResource("Core.Lead.Letter.OrderTotal"));
			htmlOrderTable.AppendFormat("<td class='footer-value' style='border-bottom: none; font-weight: bold; padding: 10px 0; text-align: center;'>{0}</td>", PriceFormatService.FormatPrice(total, currency));
			htmlOrderTable.Append("</tr>");

			htmlOrderTable.Append("</table>");

			return htmlOrderTable.ToString();
		}

		#region Statistics

		public static Dictionary<DateTime, float> GetLeadsCountStatistics(string group, DateTime minDate, DateTime maxDate)
		{
			return SQLDataAccess.ExecuteReadDictionary<DateTime, float>(
				"Select DATEADD(" + group + ", DATEDIFF(" + group + ", 0, [CreatedDate]), 0) as 'Date', Count([Id]) as Count " +
				"FROM [Order].[Lead] " +
				"WHERE [CreatedDate] > @MinDate and [CreatedDate] <= @MaxDate " +
				"GROUP BY DATEADD(" + group + ", DATEDIFF(" + group + ", 0, [CreatedDate]), 0)",
				CommandType.Text,
				"Date", "Count",
				new SqlParameter("@MinDate", minDate),
				new SqlParameter("@MaxDate", maxDate));
		}

		[Obsolete("LeadStatus not exist")]
		public static Dictionary<string, int> GetLeadsStatusesStatistics(DateTime minDate, DateTime maxDate)
		{
			return SQLDataAccess.ExecuteReadDictionary<string, int>(
				"Select LeadStatus, Count([Id]) as Count " +
				"FROM [Order].[Lead] " +
				"WHERE [CreatedDate] > @MinDate and [CreatedDate] <= @MaxDate " +
				"GROUP BY LeadStatus",
				CommandType.Text,
				"LeadStatus", "Count",
				new SqlParameter("@MinDate", minDate),
				new SqlParameter("@MaxDate", maxDate));
		}

		#endregion

		public static bool CheckAccess(Lead lead)
		{
			var customer = CustomerContext.CurrentCustomer;
			if (customer.IsModerator)
			{
				var manager = ManagerService.GetManager(customer.Id);
				if (manager != null && manager.Active)
				{
					if (SettingsManager.ManagersLeadConstraint == ManagersLeadConstraint.Assigned &&
						lead.ManagerId != manager.ManagerId)
						return false;

					if (SettingsManager.ManagersLeadConstraint == ManagersLeadConstraint.AssignedAndFree &&
						lead.ManagerId != manager.ManagerId && lead.ManagerId != null)
						return false;
				}
			}
			return true;
		}
	}
}

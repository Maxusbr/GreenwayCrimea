using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Core.Services.Orders;
using AdvantShop.Helpers;
using AdvantShop.Localization;
using AdvantShop.Orders;
using AdvantShop.Statistic;
using AdvantShop.Taxes;
using CsvHelper;
using CsvHelper.Configuration;
using OfficeOpenXml;

namespace AdvantShop.ExportImport.Excel
{
	public class ExcelExport
	{
		public const string templateSingleOrder = "~/App_Data/Reports/exportSingleOrder.xlsx";

		private static int RenderOrderItems(ExcelWorksheet worksheet, Order order)
		{
			var i = 1;
			foreach (OrderItem item in order.OrderItems)
			{
				item.Weight = item.Weight > 0 ? item.Weight : ProductService.GetProduct(item.ProductID ?? 0)?.Weight ?? 0;
				//copy style
				if (i != 1)
				{
					worksheet.InsertRow(18, 1, 17); //shift down
													//worksheet.Row(19 + i).StyleID = worksheet.Row(20).StyleID;
				}

				var indx = i != 1 ? 18 : 17;
				//worksheet.Cells[indx, 1, indx, 2].Merge = true;
				worksheet.Cells[indx, 1].Value = item.ArtNo;
				worksheet.Cells[indx, 2].Value = item.Name;
				//var html = new StringBuilder();
				//if (!string.IsNullOrEmpty(item.Color))
				//	html.Append(SettingsCatalog.ColorsHeader + ": " + item.Color + " \n");

				//if (!string.IsNullOrEmpty(item.Size))
				//	html.Append(SettingsCatalog.SizesHeader + ": " + item.Size + " \n");

				//foreach (EvaluatedCustomOptions ev in item.SelectedOptions)
				//{
				//	html.Append(string.Format("- {0}: {1} \n", ev.CustomOptionTitle, ev.OptionTitle));
				//}
				//worksheet.Cells[i != 1 ? 21 : 20, 2].Value = html.ToString();
				worksheet.Cells[indx, 3].Value = item.Price;//PriceFormatService.FormatPrice(item.Price, order.OrderCurrency);
				worksheet.Cells[indx, 4].Value = item.Weight;
				worksheet.Cells[indx, 5].Value = item.Amount;
				worksheet.Cells[indx, 6].Value = item.Weight * item.Amount;
				worksheet.Cells[indx, 7].Value = item.Price * item.Amount;//PriceFormatService.FormatPrice(item.Price * item.Amount, order.OrderCurrency);

				i++;
			}

			return i;
		}

		public static void SingleOrder(string templatePath, string filename, Order order)
		{
			using (var excel = new ExcelPackage(new System.IO.FileInfo(templatePath)))
			{
				var worksheet = excel.Workbook.Worksheets.First();

				for (int i = 1; i <= 50; i++)
				{
					worksheet.Row(i).CustomHeight = false;
				}

				worksheet.Name = string.Format("{0} {1}", LocalizationService.GetResource("Core.ExportImport.ExcelSingleOrder.ItemNum"), order.Number);
				//title
				worksheet.Cells[1, 1].Value = string.Format("{0} {1}", LocalizationService.GetResource("Core.ExportImport.ExcelSingleOrder.ItemNum"), order.Number);

				//status
				worksheet.Cells[2, 1].Value = "(" + order.OrderStatus.StatusName + ")";

				// Date
				worksheet.Cells[4, 2].Value = LocalizationService.GetResource("Core.ExportImport.ExcelSingleOrder.Date");
				worksheet.Cells[4, 3].Value = Culture.ConvertShortDate(order.OrderDate);

				//StatusComment
				//worksheet.Cells[5, 2].Value = LocalizationService.GetResource("Admin.Orders.OrderItem.StatusComment");
				//worksheet.Cells[5, 3].Value = order.StatusComment;

				//Email
				worksheet.Cells[5, 2].Value = LocalizationService.GetResource("Admin.Orders.OrderItem.Email");
				worksheet.Cells[5, 3].Value = order.OrderCustomer.Email;

				//Phone
				worksheet.Cells[6, 2].Value = LocalizationService.GetResource("Admin.Orders.OrderItem.Phone");
				worksheet.Cells[6, 3].Value = order.OrderCustomer.Phone;

				//worksheet.Cells[9, 1].Value = LocalizationService.GetResource("Admin.Orders.OrderItem.Billing");
				//worksheet.Cells[9, 2].Value = LocalizationService.GetResource("Admin.Orders.OrderItem.Shipping");
				//worksheet.Cells[9, 3].Value = LocalizationService.GetResource("Admin.Orders.OrderItem.ShippingMethod");

				var customerName = StringHelper.AggregateStrings(" ", order.OrderCustomer.LastName,
					order.OrderCustomer.FirstName, order.OrderCustomer.Patronymic);
				//worksheet.Cells[10, 1].Value = "     " + LocalizationService.GetResource("Admin.Orders.OrderItem.ContactName") + customerName;
				//worksheet.Cells[10, 2].Value = "     " + LocalizationService.GetResource("Admin.Orders.OrderItem.ContactName") + customerName;
				worksheet.Cells[9, 3].Value = customerName;

				var customerFields = Customers.CustomerFieldService.GetCustomerFieldsWithValue(order.OrderCustomer.CustomerID);
				if (customerFields.Any(o => o.Name.Equals("GreenwayId")))
				{
					worksheet.Cells[10, 3].Value = customerFields.FirstOrDefault(o => o.Name.Equals("GreenwayId"))?.Value ?? "";
				}
				//var shippingMethodName = order.ArchivedShippingName;
				//if (order.OrderPickPoint != null)
				//	shippingMethodName += order.OrderPickPoint.PickPointAddress.Replace("<br/>", " ");
				//worksheet.Cells[10, 3].Value = "     " + shippingMethodName;


				//worksheet.Cells[11, 1].Value = "     " + LocalizationService.GetResource("Admin.Orders.OrderItem.Country") + order.OrderCustomer.Country;
				//worksheet.Cells[11, 2].Value = "     " + LocalizationService.GetResource("Admin.Orders.OrderItem.Country") + order.OrderCustomer.Country;
				//worksheet.Cells[11, 3].Value = LocalizationService.GetResource("Admin.Orders.OrderItem.PaymentType");

				//worksheet.Cells[11, 1].Value = "     " + LocalizationService.GetResource("Admin.Orders.OrderItem.City") + order.OrderCustomer.City;
				worksheet.Cells[11, 2].Value = "     " + LocalizationService.GetResource("Admin.Orders.OrderItem.City");
				worksheet.Cells[11, 3].Value = order.OrderCustomer.City;
				//worksheet.Cells[12, 3].Value = "     " + order.PaymentMethodName;

				//worksheet.Cells[13, 1].Value = "     " + LocalizationService.GetResource("Admin.Orders.OrderItem.Zone") + order.OrderCustomer.Region;
				//worksheet.Cells[13, 2].Value = "     " + LocalizationService.GetResource("Admin.Orders.OrderItem.Zone") + order.OrderCustomer.Region;


				//worksheet.Cells[14, 1].Value = "     " + LocalizationService.GetResource("Admin.Orders.OrderItem.Zip") + order.OrderCustomer.Zip;
				//worksheet.Cells[14, 2].Value = "     " + LocalizationService.GetResource("Admin.Orders.OrderItem.Zip") + order.OrderCustomer.Zip;

				//worksheet.Cells[15, 1].Value = "     " +
				//							   LocalizationService.GetResource("Admin.Orders.OrderItem.Address") + order.OrderCustomer.GetCustomerAddress();
				worksheet.Cells[12, 2].Value = "     " +
											   LocalizationService.GetResource("Admin.Orders.OrderItem.Address");
				worksheet.Cells[12, 3].Value = order.OrderCustomer.GetCustomerAddress();

				worksheet.Cells[14, 2].Value = LocalizationService.GetResource("Admin.Orders.OrderItem.OrderItem");

				worksheet.Cells[16, 1].Value = LocalizationService.GetResource("Admin.ExportField.ArtNo");
				worksheet.Cells[16, 2].Value = LocalizationService.GetResource("Admin.Orders.OrderItem.ItemName");
				worksheet.Cells[16, 3].Value = LocalizationService.GetResource("Admin.Orders.OrderItem.Price");
				worksheet.Cells[16, 4].Value = LocalizationService.GetResource("Admin.ExportField.Volume");
				worksheet.Cells[16, 5].Value = LocalizationService.GetResource("Admin.Orders.OrderItem.ItemAmount");
				worksheet.Cells[16, 6].Value = LocalizationService.GetResource("Admin.ExportField.TotallVolume");
				worksheet.Cells[16, 7].Value = LocalizationService.GetResource("Admin.Orders.OrderItem.ItemCost");

				//productprice
				//var currency = order.OrderCurrency;
				float productPrice = order.OrderItems.Sum(item => item.Amount * item.Price);
				worksheet.Cells[18, 6].Value = LocalizationService.GetResource("Admin.Orders.OrderItem.ProductsPrice");
				worksheet.Cells[18, 7].Value = productPrice;//PriceFormatService.FormatPrice(productPrice, currency);


				int summaryRow = 19;
				int styleRow = 19;
				//totalsum
				worksheet.Cells[summaryRow + 1, 6].Value = LocalizationService.GetResource("Admin.Orders.OrderItem.TotalPrice");
				worksheet.Cells[summaryRow + 1, 7].Value = order.Sum; //PriceFormatService.FormatPrice(order.Sum, currency);

				//comment
				worksheet.Cells[21, 1].Value = LocalizationService.GetResource("Admin.Orders.OrderItem.CustomerComment");
				worksheet.Cells[22, 1].Value = order.AdminOrderComment;



				//if (order.PaymentCost > 0)
				//{
				//insert before summaryRow row with copy style from styleRow
				/*
				worksheet.InsertRow(summaryRow, 1, styleRow);
				worksheet.Cells[summaryRow, 5].Value = "(" + order.ArchivedPaymentName + ")";

				worksheet.InsertRow(summaryRow, 1, styleRow);
				worksheet.Cells[summaryRow, 5].Value = LocalizationService.GetResource("Core.ExportImport.ExcelSingleOrder.PaymentExtracharge");
				worksheet.Cells[summaryRow, 6].Value = "+" + PriceFormatService.FormatPrice(order.PaymentCost, currency);

				//}

				worksheet.InsertRow(summaryRow, 1, styleRow);
				worksheet.Cells[summaryRow, 5].Value = "(" + shippingMethodName + ")";

				worksheet.InsertRow(summaryRow, 1, styleRow);
				worksheet.Cells[summaryRow, 5].Value = LocalizationService.GetResource("Core.ExportImport.ExcelSingleOrder.ShippingPrice");
				worksheet.Cells[summaryRow, 6].Value = "+" + PriceFormatService.FormatPrice(order.ShippingCost, order.OrderCurrency);
				
				var taxedItems = TaxService.GetOrderTaxes(order);
				if (taxedItems.Count > 0)
				{
					foreach (var tax in taxedItems.Keys)
					{
						worksheet.InsertRow(summaryRow, 1, styleRow);
						worksheet.Cells[summaryRow, 5].Value = (tax.ShowInPrice ? LocalizationService.GetResource("Core.Tax.IncludeTax") + " " : "") + tax.Name + ":";
						worksheet.Cells[summaryRow, 6].Value = (tax.ShowInPrice ? "" : "+") + PriceFormatService.FormatPrice(taxedItems[tax], currency);
					}
				}
				else
				{
					worksheet.InsertRow(summaryRow, 1, styleRow);
					worksheet.Cells[summaryRow, 5].Value = LocalizationService.GetResource("Admin.Orders.OrderItem.Taxes");
					worksheet.Cells[summaryRow, 6].Value = "+" + PriceFormatService.FormatPrice(0, currency);
				}
				
				float bonusPrice = order.BonusCost;
				if (bonusPrice > 0)
				{
					worksheet.InsertRow(summaryRow, 1, styleRow);
					worksheet.Cells[summaryRow, 5].Value = LocalizationService.GetResource("Admin.Orders.OrderItem.Bonuses");
					worksheet.Cells[summaryRow, 6].Value = "-" + PriceFormatService.FormatPrice(bonusPrice, currency);
				}

				if (order.Certificate != null)
				{
					worksheet.InsertRow(summaryRow, 1, styleRow);
					worksheet.Cells[summaryRow, 5].Value = LocalizationService.GetResource("Admin.Orders.OrderItem.Certificate");
					worksheet.Cells[summaryRow, 6].Value = "-" + string.Format("-{0}", PriceFormatService.FormatPrice(order.Certificate.Price, currency));
				}
				*/
				if (order.OrderDiscount != 0 || order.OrderDiscountValue != 0)
				{
					//var productsIgnoreDiscountPrice = order.OrderItems.Where(item => item.IgnoreOrderDiscount).Sum(item => item.Price * item.Amount);
					worksheet.InsertRow(summaryRow, 1, styleRow);
					worksheet.Cells[summaryRow, 6].Value = LocalizationService.GetResource("Admin.Orders.OrderItem.Discount");
					//worksheet.Cells[summaryRow, 6].Value = "-" +
					//									   PriceFormatService.FormatDiscountPercent(productPrice - productsIgnoreDiscountPrice,
					//										   order.OrderDiscount, order.OrderDiscountValue,
					//										   currency.CurrencyValue, currency.CurrencySymbol,
					//										   currency.IsCodeBefore, false);
					worksheet.Cells[summaryRow, 7].Value = order.OrderDiscountValue;
				}
				/*
				if (order.Coupon != null)
				{
					//insert before summaryRow row with copy style from styleRow
					worksheet.InsertRow(summaryRow, 1, styleRow);
					worksheet.Cells[summaryRow, 5].Value = LocalizationService.GetResource("Admin.Orders.OrderItem.Coupon");
					var productsWithCoupon = order.OrderItems.Where(item => item.IsCouponApplied).Sum(item => item.Price * item.Amount);
					switch (order.Coupon.Type)
					{
						case CouponType.Fixed:
							worksheet.Cells[summaryRow, 6].Value = string.Format("-{0} ({1})",
																		PriceFormatService.FormatPrice(order.Coupon.Value, currency),
																		order.Coupon.Code);
							break;
						case CouponType.Percent:
							worksheet.Cells[summaryRow, 6].Value = string.Format("-{0} ({1}%) ({2})",
													  PriceFormatService.FormatPrice(productsWithCoupon * order.Coupon.Value / 100, currency),
													  PriceFormatService.FormatPriceInvariant(order.Coupon.Value),
													  order.Coupon.Code);
							break;
					}

				}
				*/
				var indx = RenderOrderItems(worksheet, order);
				float volume = order.OrderItems.Sum(item => item.Amount * item.Weight);
				worksheet.Cells[summaryRow + indx - 1, 6].Value = LocalizationService.GetResource("Admin.ExportField.Volume");
				worksheet.Cells[summaryRow + indx - 1, 7].Value = volume;

				excel.SaveAs(new FileInfo(filename));
			}
		}

		public static void MultiOrder(IList<Order> orders, string filename, string encoding)
		{
			using (var streamWriter = new StreamWriter(filename, false, Encoding.GetEncoding(encoding)))
			using (var writer = new CsvWriter(streamWriter, new CsvConfiguration { Delimiter = ";" }))
			{
				// headers
				writer.WriteField(LocalizationService.GetResource("Core.ExportImport.MultiOrder.OrderID"));
				writer.WriteField(LocalizationService.GetResource("Core.ExportImport.MultiOrder.Status"));
				writer.WriteField(LocalizationService.GetResource("Core.ExportImport.MultiOrder.OrderDate"));
				writer.WriteField(LocalizationService.GetResource("Core.ExportImport.MultiOrder.FIO"));
				writer.WriteField(LocalizationService.GetResource("Core.ExportImport.MultiOrder.CustomerEmail"));
				writer.WriteField(LocalizationService.GetResource("Core.ExportImport.MultiOrder.CustomerPhone"));
				writer.WriteField(LocalizationService.GetResource("Core.ExportImport.MultiOrder.OrderedItems"));
				writer.WriteField(LocalizationService.GetResource("Core.ExportImport.MultiOrder.Total"));
				writer.WriteField(LocalizationService.GetResource("Core.ExportImport.MultiOrder.Currency"));
				writer.WriteField(LocalizationService.GetResource("Core.ExportImport.MultiOrder.Tax"));
				writer.WriteField(LocalizationService.GetResource("Core.ExportImport.MultiOrder.Cost"));
				writer.WriteField(LocalizationService.GetResource("Core.ExportImport.MultiOrder.Profit"));
				writer.WriteField(LocalizationService.GetResource("Core.ExportImport.MultiOrder.Payment"));
				writer.WriteField(LocalizationService.GetResource("Core.ExportImport.MultiOrder.Shipping"));
				writer.WriteField(LocalizationService.GetResource("Core.ExportImport.MultiOrder.ShippingAddress"));
				writer.WriteField(LocalizationService.GetResource("Core.ExportImport.MultiOrder.CustomerComment"));
				writer.WriteField(LocalizationService.GetResource("Core.ExportImport.MultiOrder.AdminComment"));
				writer.WriteField(LocalizationService.GetResource("Core.ExportImport.MultiOrder.StatusComment"));
				writer.WriteField(LocalizationService.GetResource("Core.ExportImport.MultiOrder.Payed"));
				writer.NextRecord();

				foreach (Order order in orders)
				{
					if (!CommonStatistic.IsRun)
						return;

					writer.WriteField(order.Number);
					writer.WriteField(order.OrderStatus != null ? order.OrderStatus.StatusName : LocalizationService.GetResource("Core.ExportImport.MultiOrder.NullStatus"));
					writer.WriteField(order.OrderDate.ToString("dd.MM.yyyy HH:mm:ss"));

					if (order.OrderCustomer != null)
					{
						writer.WriteField(order.OrderCustomer.LastName + " " + order.OrderCustomer.FirstName);
						writer.WriteField(order.OrderCustomer.Email ?? string.Empty);
						writer.WriteField(order.OrderCustomer.Phone ?? string.Empty);
					}
					else
					{
						writer.WriteField(LocalizationService.GetResource("Core.ExportImport.MultiOrder.NullCustomer"));
						writer.WriteField(string.Empty);
						writer.WriteField(string.Empty);
					}

					if (order.OrderCurrency != null)
					{
						writer.WriteField(RenderOrderedItems(order.OrderItems) ?? string.Empty);
						writer.WriteField(PriceService.RoundPrice(order.Sum, order.OrderCurrency));
						writer.WriteField(order.OrderCurrency.CurrencySymbol);
						writer.WriteField(PriceService.RoundPrice(order.TaxCost, order.OrderCurrency));
						float totalCost = order.OrderItems.Sum(oi => oi.SupplyPrice * oi.Amount);
						writer.WriteField(PriceService.RoundPrice(totalCost, order.OrderCurrency));
						writer.WriteField(PriceService.RoundPrice(order.Sum - order.ShippingCost - order.TaxCost - totalCost, order.OrderCurrency));
						writer.WriteField(order.PaymentMethodName);
						writer.WriteField(order.ArchivedShippingName + " - " + PriceService.RoundPrice(order.ShippingCost, order.OrderCurrency));
						writer.WriteField(order.OrderCustomer != null
							? new List<string>
							{
								order.OrderCustomer.Zip,
								order.OrderCustomer.Country,
								order.OrderCustomer.Region,
								order.OrderCustomer.City,
								order.OrderCustomer.GetCustomerAddress(),
								order.OrderCustomer.CustomField1,
								order.OrderCustomer.CustomField2,
								order.OrderCustomer.CustomField3
							}.Where(s => s.IsNotEmpty()).AggregateString(", ")
							: string.Empty);
						writer.WriteField(order.CustomerComment ?? string.Empty);
						writer.WriteField(order.AdminOrderComment ?? string.Empty);
						writer.WriteField(order.StatusComment ?? string.Empty);
						writer.WriteField(LocalizationService.GetResource(order.Payed ? "Admin.Yes" : "Admin.No"));
					}
					writer.NextRecord();
					CommonStatistic.RowPosition++;
				}
			}
		}

		private static string RenderOrderedItems(IEnumerable<OrderItem> items)
		{
			var res = new StringBuilder();

			foreach (OrderItem orderItem in items)
			{
				res.AppendFormat("[{0} - {1} - {2}{3}{4}], ", orderItem.ArtNo, orderItem.Name, orderItem.Amount,
					LocalizationService.GetResource("Core.ExportImport.ExcelOrder.Pieces"),
					RenderSelectedOptions(orderItem.SelectedOptions, orderItem.Color, orderItem.Size));
			}

			return res.ToString().TrimEnd(new[] { ',', ' ' });
		}

		private static string RenderSelectedOptions(IList<EvaluatedCustomOptions> evlist, string color, string size)
		{
			var html = string.Empty;
			if (evlist != null || !string.IsNullOrEmpty(color) || !string.IsNullOrEmpty(size))
			{
				if (!string.IsNullOrEmpty(color))
					html = SettingsCatalog.ColorsHeader + ": " + color;

				if (!string.IsNullOrEmpty(size))
					html += (!string.IsNullOrEmpty(html) ? ", " : "") + SettingsCatalog.SizesHeader + ": " + size;

				if (evlist != null)
				{
					foreach (EvaluatedCustomOptions ev in evlist)
						html += (!string.IsNullOrEmpty(html) ? ", " : "") + (string.Format("{0}: {1},", ev.CustomOptionTitle, ev.OptionTitle));
				}

				if (!string.IsNullOrEmpty(html))
					html = " (" + html + ")";
			}

			return html;
		}
	}
}

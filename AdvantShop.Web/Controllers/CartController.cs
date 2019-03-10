using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.Core.Services.SEO;
using AdvantShop.Customers;
using AdvantShop.Handlers.Cart;
using AdvantShop.Helpers;
using AdvantShop.Orders;
using AdvantShop.ViewModel.Cart;
using AdvantShop.ViewModel.Common;
using AdvantShop.Web.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using AdvantShop.Models.Cart;

namespace AdvantShop.Controllers
{
	[SessionState(SessionStateBehavior.ReadOnly)]
	public partial class CartController : BaseClientController
	{
		#region Cart

		public ActionResult Index(string products)
		{
			if (!String.IsNullOrWhiteSpace(products))
			{
				foreach (var item in products.Split(";"))
				{
					int offerId;
					var newItem = new ShoppingCartItem() { CustomerId = CustomerContext.CustomerId };

					var parts = item.Split("-");
					if (parts.Length > 0 && (offerId = parts[0].TryParseInt(0)) != 0 && OfferService.GetOffer(offerId) != null)
					{
						newItem.OfferId = offerId;
					}
					else
					{
						continue;
					}

					newItem.Amount = parts.Length > 1 ? parts[1].TryParseFloat() : 1;

					var currentItem = ShoppingCartService.CurrentShoppingCart.FirstOrDefault(shpCartitem => shpCartitem.OfferId == newItem.OfferId);
					if (currentItem != null)
					{
						currentItem.Amount = newItem.Amount;
						ShoppingCartService.UpdateShoppingCartItem(currentItem);
					}
					else
					{
						ShoppingCartService.AddShoppingCartItem(newItem);
					}
				}
			}

			var shpCart = ShoppingCartService.CurrentShoppingCart;

			var model = new CartViewModel()
			{
				Cart = shpCart,
				ShowConfirmButton = true,
				PhotoWidth = SettingsPictureSize.XSmallProductImageWidth
			};

			foreach (var module in AttachedModules.GetModules<IShoppingCart>())
			{
				var moduleObject = (IShoppingCart)Activator.CreateInstance(module, null);
				model.ShowConfirmButton &= moduleObject.ShowConfirmButtons;
			}

			model.ShowBuyOneClick = model.ShowConfirmButton && SettingsCheckout.BuyInOneClick;

			SetMetaInformation(T("Cart.Index.ShoppingCart"));

			var tagManager = GoogleTagManagerContext.Current;
			if (tagManager.Enabled)
			{
				tagManager.PageType = ePageType.cart;
				tagManager.ProdIds = shpCart.Select(item => item.Offer.ArtNo).ToList();
				tagManager.Products = shpCart.Select(x => new TransactionProduct()
				{
					SKU = x.Offer.ArtNo,
					Category = x.Offer.Product.MainCategory != null ? x.Offer.Product.MainCategory.Name : string.Empty,
					Name = x.Offer.Product.Name,
					Price = x.Price,
					Quantity = x.Amount
				}).ToList();
				tagManager.TotalValue = shpCart.TotalPrice;
			}

			WriteLog("", Url.AbsoluteRouteUrl("Cart"), ePageType.cart);
			return View(model);
		}


		[ChildActionOnly]
		public ActionResult ShoppingCart()
		{
			var itemsAmount = ShoppingCartService.CurrentShoppingCart.TotalItems;
			var amount = string.Format("{0} {1}", itemsAmount == 0 ? "" : itemsAmount.ToString(),
								  Strings.Numerals(itemsAmount,
									T("Cart.Product0"), T("Cart.Product1"), T("Cart.Product2"), T("Cart.Product5")));

			return PartialView("ShoppingCart", new ShoppingCartViewModel() { Amount = amount });
		}

		[HttpPost]
		public JsonResult GetCart()
		{
			return Json(new GetCartHandler().Get());
		}

		[HttpGet]
		public ActionResult AddToCart(int id)
		{
			if (id == 0)
			{
				return Json(new { status = "fail" });
			}
			var offer = OfferService.GetOffer(id);
			if (offer == null) return Json(new { status = "fail" });

			AddToCart(new CartItemAddingModel { OfferId = offer.OfferId, ProductId = offer.OfferId });
			return RedirectToAction("Index");
		}

		[HttpPost]
		public JsonResult AddToCart(CartItemAddingModel item)
		{
			Offer offer;

			if (item.OfferId == 0 && item.ProductId != 0)
			{
				var product = ProductService.GetProduct(item.ProductId);

				if (product == null || product.Offers.Count == 0)
					return Json(new { status = "fail" });

				if (product.Offers.Count != 1)
					return Json(new { status = "redirect" });

				offer = product.Offers.First();
			}
			else
			{
				offer = OfferService.GetOffer(item.OfferId);
			}

			if (offer == null || !offer.Product.Enabled || !offer.Product.CategoryEnabled)
				return Json(new { status = "fail" });


			List<EvaluatedCustomOptions> listOptions = null;
			var selectedOptions = !String.IsNullOrWhiteSpace(item.AttributesXml) && item.AttributesXml != "null"
									? HttpUtility.UrlDecode(item.AttributesXml)
									: null;

			if (selectedOptions != null)
			{
				try
				{
					listOptions = CustomOptionsService.DeserializeFromXml(selectedOptions, offer.Product.Currency.Rate);
				}
				catch (Exception)
				{
					listOptions = null;
				}
			}

			if (CustomOptionsService.DoesProductHaveRequiredCustomOptions(offer.ProductId) && listOptions == null)
				return Json(new { status = "redirect" });

			if (Single.IsNaN(item.Amount) || item.Amount == 0)
			{
				var prodMinAmount = offer.Product.MinAmount == null
							? offer.Product.Multiplicity
							: offer.Product.Multiplicity > offer.Product.MinAmount
								? offer.Product.Multiplicity
								: offer.Product.MinAmount.Value;

				item.Amount = prodMinAmount > 0 ? prodMinAmount : 1;
			}


			var cartItem = new ShoppingCartItem()
			{
				OfferId = offer.OfferId,
				Amount = item.Amount,
				AttributesXml = listOptions != null ? selectedOptions : string.Empty,
			};
			var cartId = ShoppingCartService.AddShoppingCartItem(cartItem);

			foreach (var gift in OfferService.GetProductGifts(offer.ProductId))
			{
				ShoppingCartService.AddShoppingCartItem(new ShoppingCartItem()
				{
					OfferId = gift.OfferId,
					Amount = SettingsCheckout.MultiplyGiftsCount ? item.Amount : 1,
					IsGift = true
				});
			}

			if (item.Payment != 0)
				CommonHelper.SetCookie("payment", item.Payment.ToString());

			ModulesExecuter.AddToCart(cartItem, Url.AbsoluteRouteUrl("Product", new { url = offer.Product.UrlPath }));

			return Json(new { ShoppingCartService.CurrentShoppingCart.TotalItems, status = "success", cartId = cartId });
		}

		public JsonResult AddCartItems(List<CartItemAddingModel> items, int payment = 0)
		{
			if (!items.Any())
				return Json(new { status = "fail" });

			foreach (var item in items)
			{
				AddToCart(item);
			}
			var cart = ShoppingCartService.CurrentShoppingCart;
			var mainCartItem = cart.FirstOrDefault(x => x.OfferId == items[0].OfferId) ?? cart.FirstOrDefault();
			return Json(new { cart.TotalItems, status = "success", cartId = mainCartItem != null ? mainCartItem.ShoppingCartItemId : 0 });
		}

		[HttpPost]
		public JsonResult UpdateCart(Dictionary<int, float> items)
		{
			if (items == null)
				return Json(new { status = "fail" });

			var cart = ShoppingCartService.CurrentShoppingCart;

			foreach (var pair in items)
			{
				var shpCartItem = cart.Find(x => x.ShoppingCartItemId == pair.Key);

				if (shpCartItem == null || !(pair.Value > 0))
				{
					return Json(new { status = "fail" });
				}
				if (shpCartItem.FrozenAmount)
					continue;

				if (cart.Any(x => x.IsGift) && SettingsCheckout.MultiplyGiftsCount)
				{
					foreach (var gift in OfferService.GetProductGifts(shpCartItem.Offer.ProductId))
					{
						var giftItem = cart.Find(x => x.OfferId == gift.OfferId && x.IsGift);
						if (giftItem == null) continue;

						giftItem.Amount = giftItem.Amount + (pair.Value - shpCartItem.Amount);
						if (!(giftItem.Amount > 0))
							giftItem.Amount = pair.Value;
						ShoppingCartService.UpdateShoppingCartItem(giftItem);
					}
				}

				shpCartItem.Amount = pair.Value;
				ShoppingCartService.UpdateShoppingCartItem(shpCartItem);
			}

			return Json(new { ShoppingCartService.CurrentShoppingCart.TotalItems, status = "success" });
		}

		[HttpPost]
		public JsonResult RemoveFromCart(int itemId)
		{
			if (itemId == 0)
				return Json(new { status = "fail" });

			var cart = ShoppingCartService.CurrentShoppingCart;

			var cartItem = cart.Find(item => item.ShoppingCartItemId == itemId);
			if (cartItem != null)
			{
				if (cart.Any(x => x.IsGift))
				{
					var gifts = OfferService.GetProductGifts(cartItem.Offer.ProductId);
					foreach (var gift in gifts)
					{
						var giftItem = cart.Find(x => x.OfferId == gift.OfferId && x.IsGift);
						if (giftItem == null) continue;
						var productIds = OfferService.GetProductIdsByGift(gift.OfferId);    // products with this gift
						var cartItems = cart.Where(x => productIds.Contains(x.Offer.ProductId) && !x.IsGift).ToList();  // all cart items with this gift
						if (cartItems.Count <= 1)   // remove gift from cart only when there is only one main product with this gift in cart
						{
							ShoppingCartService.DeleteShoppingCartItem(giftItem);
						}
						else
						{
							giftItem.Amount = SettingsCheckout.MultiplyGiftsCount
								? cartItems.Where(x => x.ShoppingCartItemId != itemId).Sum(x => x.Amount)
								: 1;
							ShoppingCartService.UpdateShoppingCartItem(giftItem);
						}
					}
				}

				ShoppingCartService.DeleteShoppingCartItem(cartItem);
			}

			return Json(new
			{
				ShoppingCartService.CurrentShoppingCart.TotalItems,
				status = "success",
				offerId = cartItem != null ? cartItem.OfferId : 0
			});
		}

		[HttpPost]
		public JsonResult ClearCart()
		{
			ShoppingCartService.ClearShoppingCart(ShoppingCartType.ShoppingCart);

			return Json(new { ShoppingCartService.CurrentShoppingCart.TotalItems, status = "success" });
		}

		#endregion

		#region Certificate and coupon

		[HttpPost]
		public JsonResult ApplyCertificateOrCoupon(string code)
		{
			code = code.Trim();

			var customerGroup = CustomerContext.CurrentCustomer.CustomerGroup;

			if (customerGroup.CustomerGroupId != CustomerGroupService.DefaultCustomerGroup)
				return Json(new { status = "fail" });

			var cert = GiftCertificateService.GetCertificateByCode(code);
			var coupon = CouponService.GetCouponByCode(code);

			if (SettingsCheckout.EnableGiftCertificateService && cert != null && cert.Paid && !cert.Used && cert.Enable)
			{
				GiftCertificateService.AddCustomerCertificate(cert.CertificateId);

				return Json(new { status = "success" });
			}

			if (coupon != null && coupon.Enabled && (coupon.ExpirationDate == null || coupon.ExpirationDate > DateTime.Now) &&
				(coupon.PossibleUses == 0 || coupon.PossibleUses > coupon.ActualUses))
			{
				CouponService.AddCustomerCoupon(coupon.CouponID);

				return Json(new { status = "success" });
			}

			return Json(new { status = "fail" });
		}

		[HttpPost]
		public JsonResult DeleteCertificate()
		{
			var cerertificate = GiftCertificateService.GetCustomerCertificate();
			if (cerertificate != null)
			{
				GiftCertificateService.DeleteCustomerCertificate(cerertificate.CertificateId);
			}
			return Json(new { status = "success" });
		}

		[HttpPost]
		public JsonResult DeleteCoupon()
		{
			var coupon = CouponService.GetCustomerCoupon();
			if (coupon != null)
			{
				CouponService.DeleteCustomerCoupon(coupon.CouponID);
			}
			return Json(new { status = "success" });
		}

		#endregion
	}
}
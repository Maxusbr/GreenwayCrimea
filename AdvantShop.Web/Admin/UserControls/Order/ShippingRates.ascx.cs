using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Orders;
using AdvantShop.Repository.Currencies;
using AdvantShop.Shipping;
using System.Web;
using AdvantShop.Diagnostics;
using AdvantShop.Shipping.CheckoutRu;
using AdvantShop.Shipping.Edost;
using AdvantShop.Shipping.RangeWeightAndDistanceOption;
using AdvantShop.Shipping.Sdek;
using AdvantShop.Shipping.ShippingYandexDelivery;
using AdvantShop.Payment;
using Resources;
using AdvantShop.Shipping.PointDelivery;
using Newtonsoft.Json;
using AdvantShop.Shipping.Boxberry;

namespace Admin.UserControls.Order
{
    public partial class ShippingRatesControl : UserControl
    {
        #region Fields

        //private const string PefiksId = "RadioShipRate_";

        private List<AbstractShippingOption> _shippingRates;

        public class ShippingOptionEx
        {
            public string PickpointId { get; set; }
            public string PickpointAddress { get; set; }
            public string AdditionalData { get; set; }
        }

        public ShippingOptionEx SelectShippingOptionEx
        {
            get
            {
                return SelectedItem is EdostPickPointOption ||
                       SelectedItem is EdostBoxberryOption ||
                       SelectedItem is EdostCashOnDeliveryBoxberryOption ||
                       SelectedItem is EdostCashOnDeliveryPickPointOption ||
                       SelectedItem is SdekOption ||
                       SelectedItem is CheckoutPointOption ||
                       SelectedItem is CheckoutOption ||
                       SelectedItem is YandexDeliveryPickupPointOption ||
                       SelectedItem is PointDeliveryOption ||
                       SelectedItem is BoxberryOption ||
                       SelectedItem is BoxberryPointOption
                    ? new ShippingOptionEx()
                    {
                        PickpointId = pickpointId.Value,
                        PickpointAddress = pickAddress.Value,
                        AdditionalData = pickAdditional.Value ?? string.Empty
                    }
                    : null;
            }
            set
            {
                if (value != null)
                {
                    pickpointId.Value = value.PickpointId;
                    pickAddress.Value = value.PickpointAddress;
                    pickAdditional.Value = value.AdditionalData ?? string.Empty;
                }
            }
        }

        public string SelectedId
        {

            get { return _selectedID.Value; }
            set
            {
                if (_shippingRates.Count > 0)
                {
                    _selectedID.Value = _shippingRates.Find(s => s.Id == value) != null
                        ? value
                        : _shippingRates[0].Id;
                }
            }
        }

        private AbstractShippingOption _selectedItem;
        public AbstractShippingOption SelectedItem
        {
            get
            {
                if (_selectedItem != null)
                    return _selectedItem;

                CalculateShippingRates();

                _selectedItem = _selectedItem ?? _shippingRates.Find(x => x.Id == SelectedId);

                if (SelectShippingOptionEx != null && SelectShippingOptionEx.PickpointId != null && _selectedItem is YandexDeliveryPickupPointOption)
                {
                    var option = _selectedItem as YandexDeliveryPickupPointOption;
                    if (option.PickPoints != null)
                    {
                        var selectedOption =
                            option.PickPoints.FirstOrDefault(
                                x => x.PickupPoints.Find(p => p.id == SelectShippingOptionEx.PickpointId) != null);
                        if (selectedOption != null)
                            _selectedItem.Rate = selectedOption.CostWithRules;
                    }
                }

                return _selectedItem;
            }
        }

        public string Country { get; set; }
        public string Zip { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
		public int SelectedPaymentId { get; set; }
        //public ShoppingCart ShoppingCart { get; set; }

        public List<OrderItem> OrderItems { get; set; }
        public float TotalDiscount { get; set; }

        private Currency _currency = CurrencyService.CurrentCurrency;
        public Currency Currency
        {
            get { return _currency; }
            set { if (value != null) _currency = value; }
        }

        protected string Amount;
        protected string Weight;
        protected string Cost;
        protected string WidgetCode;
        protected string Dimensions;
        protected string ShowAssessedValue;


        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (_shippingRates == null)
                _shippingRates = new List<AbstractShippingOption>();
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
        }

        public void ClearRates()
        {
            RadioList.Controls.Clear();
            RadioList.Visible = false;
            //ShippingManager.CurrentShippingRates = new List<AbstractShippingOption>();
            _shippingRates = new List<AbstractShippingOption>();
        }

        public void Update()
        {
            if (_shippingRates.Count > 0)
            {
                ClearRates();
                CalculateShippingRates();
            }
        }

        public void LoadMethods()
        {
            CalculateShippingRates();
            GenerateShippingRates();
        }

        public void LoadMethods(string selectedId)
        {
            SelectedId = selectedId;
            LoadMethods();
        }

        #region private

        private void CalculateShippingRates()
        {
            if (_shippingRates == null) _shippingRates = new List<AbstractShippingOption>();

            var shippingId = _selectedID.Value;
            var typeStr = hfType.Value;
            BaseShippingOption selectedShipping = null;
            var payment = PaymentService.GetPaymentMethod(SelectedPaymentId);
            BasePaymentOption selected = null;
            if (SelectedPaymentId > 0 && payment is CashOnDelivery)
                selected = new CashOnDeliverytOption(payment, 0);

            if (SelectedPaymentId > 0 && payment is PickPoint)
                selected = new PickPointOption(payment, 0);

            try
            {
                if (!string.IsNullOrEmpty(shippingId) && !string.IsNullOrEmpty(typeStr))
                {
                    var type = Type.GetType(typeStr);
                    if (type != null)
                    {
                        selectedShipping = (BaseShippingOption)Activator.CreateInstance(type);

                        // selectedShipping.Id считается как MethodId + "_" + (Name + MethodId).GetHashCode()
                        selectedShipping.Name = HttpUtility.HtmlDecode(hfName.Value);
                        selectedShipping.MethodId = hfMethodId.Value.TryParseInt();

                        var distance = hfDistance.Value;

                        if (!string.IsNullOrEmpty(distance) && selectedShipping is RangeWeightAndDistanceOption)
                        {
                            ((RangeWeightAndDistanceOption)selectedShipping).Distance = distance.TryParseFloat();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error("Admin ShippingRates", ex);
            }

            

            var preOrder = new PreOrder()
            {
                CountryDest = Country,
                ZipDest = Zip,
                CityDest = City,
                RegionDest = Region,
                Items = OrderItems.Select(x => new PreOrderItem(x)).ToList(),
                Currency = Currency ?? CurrencyService.CurrentCurrency,
                TotalDiscount = TotalDiscount
            };

            if (selectedShipping != null)
                preOrder.ShippingOption = selectedShipping;

			if (selected != null)
				preOrder.PaymentOption = selected;

            var shippingManager = new ShippingManager(preOrder);
            var options = shippingManager.GetOptions();
			foreach (var option in options)
			{
				option.ApplyPay(selected);
			}

            _shippingRates.AddRange(options);
        }

        private void GenerateShippingRates()
        {
            if ((_shippingRates != null) && (_shippingRates.Count != 0))
            {
                RadioList.Visible = true;
                int id = 1;
                string shippingRateGroup = string.Empty;

                var table = new HtmlTable();
                table.Style.Add(HtmlTextWriterStyle.Width, "100%");
                foreach (var shippingListItem in _shippingRates)
                {
                    if (shippingRateGroup != shippingListItem.Name && shippingListItem.Id != "1")
                    {
                        if (id != 1)
                        {
                            table.Controls.Add(GetTableLabel("<br />", id++));
                        }

                        shippingRateGroup = shippingListItem.Name;
                        table.Controls.Add(GetTableLabel(shippingRateGroup, id++));
                    }

                    table.Controls.Add(GetTableRadioButton(shippingListItem));
                }

                RadioList.Controls.Add(table);
            }
            else
            {
                lblNoShipping.Visible = true;
            }
        }

        private Control GetTableRadioButton(AbstractShippingOption shippingListItem)
        {
            var tr = new HtmlTableRow();
            var td = new HtmlTableCell();
            
            var radioButton = new RadioButton
            {
                GroupName = "ShippingRateGroup",
                ID = shippingListItem.Id,
                CssClass = "radio-shipping"
            };

            if (String.IsNullOrEmpty(_selectedID.Value))
            {
                _selectedID.Value = radioButton.ID;
            }

            radioButton.Checked = radioButton.ID == _selectedID.Value;

            var strShippingPrice = shippingListItem.Rate != 0
                ? PriceFormatService.FormatPrice(shippingListItem.Rate, Currency)
                : shippingListItem.ZeroPriceMessage;

            radioButton.Text = string.Format("{0} <span class='price'>{1}</span>{2}",
                shippingListItem.NameRate,
                strShippingPrice,
                !string.IsNullOrWhiteSpace(shippingListItem.DeliveryTime)
                    ? ", " + shippingListItem.DeliveryTime
                    : "");

            radioButton.Text += RenderExtend(shippingListItem, 0, pickAddress.Value, false);
            radioButton.Text +=
                string.Format(" <span class='shipping-type' data-methodId='{0}' data-name='{1}' data-type='{2}'></span> ",
                    shippingListItem.MethodId,
                    HttpUtility.HtmlEncode(shippingListItem.Name),
                    shippingListItem.ModelType.FullName + "," + shippingListItem.ModelType.Assembly.FullName);

            var panel = new Panel { CssClass = "inline-b" };

            var url = shippingListItem.IconName.Contains("http://") || shippingListItem.IconName.Contains("https://") ? 
                            shippingListItem.IconName : SettingsMain.SiteUrl + "/" + shippingListItem.IconName;

            using (var img = new Image { ImageUrl = url })
            {
                panel.Controls.Add(img);
                td.Controls.Add(panel);
            }

            var panel2 = new Panel { CssClass = "inline-b" };
            panel2.Controls.Add(radioButton);
            

            td.Controls.Add(panel2);
            tr.Controls.Add(td);

            return tr;
        }

        private Control GetTableLabel(string name, int id)
        {
            var tr = new HtmlTableRow();
            var td = new HtmlTableCell();

            var label = new Label { Text = @"<b>" + name + @"</b>", ID = "Label" + id };

            td.Controls.Add(label);
            tr.Controls.Add(td);

            return tr;
        }

        // Временная затычка (для рендеринга опций)
        public string RenderExtend(AbstractShippingOption shippingItem, int distance, string pickupAddress, bool isSelected)
        {
            var result = string.Empty;

            if (shippingItem == null)
                return result;

            if (shippingItem is EdostBoxberryOption)
            {
                var option = (EdostBoxberryOption)shippingItem;

                result = "<div class=\"shipping-points-b\"> <select class=\"shipping-points\">";
                foreach (var point in option.ShippingPoints)
                {
                    result += string.Format("<option data-description=\"{0}\" value=\"{1}\" {3}>{2}</option>",
                        HttpUtility.HtmlEncode(point.Description), point.Id, point.Address,
                        ""
                    //point.Id.ToString() == shippingItem.Ext.PickpointId ? "selected='selected'" : string.Empty
                    );
                }
                result += "</select> ";
                result += "</div>";
            }
            else if (shippingItem is EdostPickPointOption)
            {
                var option = (EdostPickPointOption)shippingItem;

                var temp = option.Pickpointmap.IsNotEmpty()
                    ? string.Format(",{{city:'{0}', ids:null}}", option.Pickpointmap)
                    : string.Empty;

                result =
                    string.Format(
                        "<br/><div class=\"address-pickpoint\">{0}</div>" +
                        "<a href=\"javascript:void(0);\" class='pickpoint' onclick=\"PickPoint.open(SetPickPointAnswer{1});\">{2}</a><br />",
                        isSelected ? pickupAddress : string.Empty,
                        temp, Resource.ShippingRates_PickpointSelect);
            }
            else if (shippingItem is EdostCashOnDeliveryBoxberryOption)
            {
                var option = (EdostCashOnDeliveryBoxberryOption)shippingItem;

                result = "<div class=\"shipping-points-b\"> <select class=\"shipping-points\">";
                foreach (var point in option.ShippingPoints)
                {
                    result += string.Format("<option data-description=\"{0}\" value=\"{1}\" {3}>{2}</option>",
                        HttpUtility.HtmlEncode(point.Description), point.Id, point.Address,
                        ""
                    //point.Id.ToString() == shippingItem.Ext.PickpointId ? "selected='selected'" : string.Empty
                    );
                }
                result += "</select> ";
                result += "</div>";
            }
            else if (shippingItem is EdostCashOnDeliveryPickPointOption)
            {
                var option = (EdostCashOnDeliveryPickPointOption)shippingItem;

                var temp = option.Pickpointmap.IsNotEmpty()
                    ? string.Format(",{{city:'{0}', ids:null}}", option.Pickpointmap)
                    : string.Empty;

                result =
                    string.Format(
                        "<br/><div class=\"address-pickpoint\">{0}</div>" +
                        "<a href=\"javascript:void(0);\" class='pickpoint' onclick=\"PickPoint.open(SetPickPointAnswer{1});\">{2}</a><br />",
                        isSelected ? pickupAddress : string.Empty,
                        temp, Resource.ShippingRates_PickpointSelect);
            }
            else if (shippingItem is RangeWeightAndDistanceOption)
            {
                var option = (RangeWeightAndDistanceOption)shippingItem;

                //if (option.Params != null &&
                //    option.Params.ElementOrDefault(ShippingByRangeWeightAndDistanceTemplate.UseDistance).TryParseBool())
                //{
                result =
                    string.Format(
                        " <input data-plugin=\"spinbox\" data-spinbox-options=\"{{min:0,max:100000,step:1}}\" type=\"text\" class=\"tDistance\" value=\"{0}\"  data-id='{1}'/>",
                        distance, shippingItem.MethodId);
                //}
            }
            else if (shippingItem is SdekOption)
            {
                var option = (SdekOption)shippingItem;

                result = "<input type=\"hidden\" class=\"hiddenSdekTariff\" value=\"" + "" + "\">";  // shippingItem.AdditionalData
                if (option.ShippingPoints != null)
                {
                    result += "<div class=\"shipping-points-b\"> <select class=\"shipping-points-sdek\">";
                    foreach (var point in option.ShippingPoints)
                    {
                        if (option.ShippingPoints.IndexOf(point) == 0)
                        {

                            result += string.Format("<option selected=\"selected\" data-description=\"{0}\" data-tariffid=\"{1}\" value=\"{2}\">{3}</option>",
                                                        HttpUtility.HtmlEncode(point.Description), "", point.Code, point.Address);
                        }
                        else
                        {
                            result += string.Format("<option data-description=\"{0}\" data-tariffid=\"{1}\" value=\"{2}\">{3}</option>",
                                                    HttpUtility.HtmlEncode(point.Description), "", point.Code, point.Address);
                        }
                        // shippingItem.Ext.AdditionalData
                    }
                    result += "</select></div>";
                }
            }
            else if (shippingItem is CheckoutPointOption)
            {
                var option = (CheckoutPointOption)shippingItem;

                result = string.Format("<input type=\"hidden\" class=\"hiddenCheckoutInfo\" value=\"\">");//shippingItem.Ext.AdditionalData
                if (option.ShippingPoints != null)
                {
                    result += "<div class=\"shipping-points-b\"> <select class=\"shipping-points-checkout\">";
                    foreach (var point in option.ShippingPoints)
                    {
                        result +=
                         string.Format(
                             "<option data-description=\"{0}\" data-additional='{1}' data-rate=\"{2}\" data-full-rate=\"{3}\" value=\"{4}\" data-checkout-address=\"{6}\">{5} - {3}</option>",
                             HttpUtility.HtmlEncode(point.Description), 
                             JsonConvert.SerializeObject(point),
                             //point.Delivery + ";" + point.DeliveryType + ";" + point.MinDeliveryTerm + ";" + point.MaxDeliveryTerm, 
                             point.Rate,
                             point.Rate,
                             point.Code, point.Address,
                             HttpUtility.HtmlEncode(point.Address));
                    }
                    result += "</select></div>";
                }
                else
                {
                    result += "<input type=\"hidden\" class=\"hiddenCheckoutExpressRate\" value=\"" + shippingItem.Rate + "\">";
                }
            }
            else if (shippingItem is CheckoutOption)
            {
                var option = (CheckoutOption)shippingItem;

                result = string.Format("<input type=\"hidden\" class=\"hiddenCheckoutInfo\" value=\"{0}\">", HttpUtility.HtmlEncode(Newtonsoft.Json.JsonConvert.SerializeObject(option)));
                result += "<input type=\"hidden\" class=\"hiddenCheckoutExpressRate\" value=\"" + shippingItem.Rate + "\">";

            }
            else if (shippingItem is YandexDeliveryPickupPointOption)
            {
                var option = (YandexDeliveryPickupPointOption)shippingItem;

                divMultiShip.Visible = true;

                WidgetCode = option.WidgetCodeYa;
                ShowAssessedValue = option.ShowAssessedValue.ToString().ToLower();
                Weight = option.Weight;
                Cost = option.Cost;
                Dimensions = option.Dimensions;
                Amount = option.Amount;

                result =
                    string.Format(
                        "<br/><div class=\"yandexdelivery-address\">{0}</div>" +
                        "<a href=\"javascript:void(0);\" class='yandex-pickpoint' data-ydwidget-open>{1}</a><br />",
                        isSelected ? pickupAddress : string.Empty,
                        Resource.ShippingRates_PickpointSelect);
            }
            else if (shippingItem is PointDeliveryOption)
            {
                var option = (PointDeliveryOption)shippingItem;

                result = "<div class=\"shipping-points-b\"> <select class=\"shipping-points\">";
                foreach (var point in option.ShippingPoints)
                {
                    result += string.Format("<option data-description=\"{0}\" value=\"{1}\" {3}>{2}</option>",
                        HttpUtility.HtmlEncode(point.Description), point.Id, point.Address,
                        ""
                        //point.Id.ToString() == shippingItem.Ext.PickpointId ? "selected='selected'" : string.Empty
                    );
                }
                result += "</select> ";
                result += "</div>";
            }

            return result;
        }

        #endregion
    }
}
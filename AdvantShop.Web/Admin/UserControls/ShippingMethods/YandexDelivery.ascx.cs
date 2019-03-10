using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Diagnostics;
using AdvantShop.Shipping.ShippingYandexDelivery;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace AdvantShop.Admin.UserControls.ShippingMethods
{
    public partial class YandexDelivery : ParametersControl
    {
        protected bool IsActive 
		{
			 get { return ViewState["IsActive"].ToString().TryParseBool(); }
            set { ViewState["IsActive"] = value; }
		}
        private const string blockComments = @"/\*(.*?)\*/";

        protected void Page_PreRender(object sender, EventArgs e)
        {
            lblStatus.Text = IsActive ? "Активен" : "Не активен";
        }

        public override Dictionary<string, string> Parameters
        {
            get
            {
                var dictionary = new Dictionary<string, string>();
                var isActive = hfIsActive.Value.TryParseBool();

                dictionary.Add(YandexDeliveryTemplate.IsActive, isActive.ToString());

                if (isActive)
                {
                    dictionary.Add(YandexDeliveryTemplate.CityFrom, txtCityFrom.Text);

                    dictionary.Add(YandexDeliveryTemplate.DefaultWeight, txtWeight.Text);
                    dictionary.Add(YandexDeliveryTemplate.DefaultHeight, txtHeightAvg.Text);
                    dictionary.Add(YandexDeliveryTemplate.DefaultWidth, txtWidthAvg.Text);
                    dictionary.Add(YandexDeliveryTemplate.DefaultLength, txtLengthAvg.Text);

                    var widgetCode = txtWidgetCode.Text.Trim();
                    if (!string.IsNullOrEmpty(widgetCode))
                    {
                        // Save only url of widget like https://delivery.yandex.ru/widget/loader?resource_id=..&sid=..&key=..

                        var index = widgetCode.IndexOf("https://delivery.yandex.ru/widget/");
                        if (index != -1)
                        {
                            var url = widgetCode.Substring(index).Replace("\"></script>", "");
                            dictionary.Add(YandexDeliveryTemplate.WidgetCode, url);
                        }
                    }

                    dictionary.Add(YandexDeliveryTemplate.ClientId, txtClientId.Text);
                    dictionary.Add(YandexDeliveryTemplate.SenderId, txtSenderId.Text);
                    dictionary.Add(YandexDeliveryTemplate.WarehouseId, txtWarehouseId.Text);
                    dictionary.Add(YandexDeliveryTemplate.RequisiteId, txtRequisiteId.Text);

                    dictionary.Add(YandexDeliveryTemplate.ShowAssessedValue, chkShowAssessedValue.Checked.ToString());
                }
                else
                {
                    Save(dictionary);
                }

                return dictionary;
            }
            set
            {
                IsActive = value.ElementOrDefault(YandexDeliveryTemplate.IsActive).TryParseBool();
                hfIsActive.Value = IsActive.ToString();

                if (IsActive)
                {
                    txtCityFrom.Text = value.ElementOrDefault(YandexDeliveryTemplate.CityFrom) ?? "Москва";
                    txtWeight.Text = value.ElementOrDefault(YandexDeliveryTemplate.DefaultWeight) ?? "1";

                    txtHeightAvg.Text = value.ElementOrDefault(YandexDeliveryTemplate.DefaultHeight) ?? "100";
                    txtWidthAvg.Text = value.ElementOrDefault(YandexDeliveryTemplate.DefaultWidth) ?? "100";
                    txtLengthAvg.Text = value.ElementOrDefault(YandexDeliveryTemplate.DefaultLength) ?? "100";

                    txtWidgetCode.Text = value.ElementOrDefault(YandexDeliveryTemplate.WidgetCode);

                    txtClientId.Text = value.ElementOrDefault(YandexDeliveryTemplate.ClientId);
                    txtSenderId.Text = value.ElementOrDefault(YandexDeliveryTemplate.SenderId);
                    txtWarehouseId.Text = value.ElementOrDefault(YandexDeliveryTemplate.WarehouseId);
                    txtRequisiteId.Text = value.ElementOrDefault(YandexDeliveryTemplate.RequisiteId);

                    chkShowAssessedValue.Checked = value.ElementOrDefault(YandexDeliveryTemplate.ShowAssessedValue).TryParseBool();
                }
            }
        }

        protected void Save(Dictionary<string, string> dictionary)
        {
            var errorMessage = string.Empty;
            lError.Visible = false;
            
            try
            {
                var plainKeyText = Regex.Replace(txtApiKeys.Text, blockComments, "", RegexOptions.Singleline).Replace("[", "").Replace("]", "");
                var apiKeysStr = plainKeyText.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                var substring = apiKeysStr.FirstOrDefault(x => x.Contains("searchDeliveryList"));
                if (substring != null)
                {
                    dictionary.Add(YandexDeliveryTemplate.SecretKeyDelivery, substring.Replace("searchDeliveryList:", "").Trim());
                }

                substring = apiKeysStr.FirstOrDefault(x => x.Contains("createOrder"));
                if (substring != null)
                {
                    dictionary.Add(YandexDeliveryTemplate.SecretKeyCreateOrder, substring.Replace("createOrder:", "").Trim());
                }
            }
            catch (Exception ex)
            {
                lError.Text = errorMessage.IsNotEmpty() ? errorMessage : "Ошибка активации";
                lError.Visible = true;

                Debug.Log.Error(ex);
                return;
            }


            try
            {
                var plainClientKeyText = "{ " + Regex.Replace(txtApiClientKeys.Text.Substring(1, txtApiClientKeys.Text.Length - 2), blockComments, "", RegexOptions.Singleline) + " }";
                var apiClientKeys = JsonConvert.DeserializeObject<YaDeliveryConfigParams>(plainClientKeyText);

                dictionary.Add(YandexDeliveryTemplate.ClientId, apiClientKeys.client_id.Trim());
                dictionary.Add(YandexDeliveryTemplate.SenderId, (apiClientKeys.sender_ids.FirstOrDefault() ?? string.Empty).Trim());
                dictionary.Add(YandexDeliveryTemplate.WarehouseId, (apiClientKeys.warehouse_ids.FirstOrDefault() ?? string.Empty).Trim());
                dictionary.Add(YandexDeliveryTemplate.RequisiteId, (apiClientKeys.requisite_ids.FirstOrDefault() ?? string.Empty).Trim());
                dictionary.Add(YandexDeliveryTemplate.ShowAssessedValue, false.ToString());
            }
            catch (Exception ex)
            {
                dictionary.Add(YandexDeliveryTemplate.ClientId, "");
                dictionary.Add(YandexDeliveryTemplate.SenderId, "");
                dictionary.Add(YandexDeliveryTemplate.WarehouseId, "");
                dictionary.Add(YandexDeliveryTemplate.RequisiteId, "");

                Debug.Log.Error(ex);
            }

            if (dictionary.ContainsKey(YandexDeliveryTemplate.IsActive))
                dictionary[YandexDeliveryTemplate.IsActive] = true.ToString();
            else
                dictionary.Add(YandexDeliveryTemplate.IsActive, true.ToString());
        }

    }
}
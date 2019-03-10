//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Linq;
using System.Collections.Generic;
//using System.Web.UI.WebControls;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Shipping;
using AdvantShop.Shipping.Sdek;
using System.Web.UI.WebControls;

namespace Admin.UserControls.ShippingMethods
{
    public partial class SdekControl : ParametersControl
    {
        //private List<string> activeTariffIds = new List<string>();

        protected void Msg(string message)
        {
            lblMessageSdek.Text = message;
            lblMessageSdek.Visible = true;
        }

        protected void ClearMsg()
        {
            lblMessageSdek.Visible = false;
        }

        public void Page_Init(object sender, EventArgs e)
        {
            var service = new SdekApiService();

            ddlTypeAddPrice.Items.Clear();
            var listTypeAdditionalPrice = new List<ListItem>();
            listTypeAdditionalPrice.Add(new ListItem() { Value = "Fixed", Text = "Фиксированная", Selected = true });
            listTypeAdditionalPrice.Add(new ListItem() { Value = "Percent", Text = "Процентная" });
            ddlTypeAddPrice.Items.AddRange(listTypeAdditionalPrice.ToArray());

            ddlTariff.DataSource = service.Tariffs.Select(item => new SdekTariff
            {
                TariffId = item.TariffId,
                Name = item.Name,
                Mode = item.Mode,
                //Active = activeTariffIds.Contains(item.TariffId.ToString()),
                Description = item.Description,
                WeightLimitation = item.WeightLimitation
            });
            ddlTariff.DataBind();


            //lvTariffs.DataSource = Sdek.tariffs.Select(item => new SdekTariff
            //    {
            //        TariffId = item.TariffId,
            //        Name = item.Name,
            //        Mode = item.Mode,
            //        Active = activeTariffIds.Contains(item.TariffId.ToString()),
            //        Description = item.Description,
            //        WeightLimitation = item.WeightLimitation
            //    });
            //lvTariffs.DataBind();
        }

        public override Dictionary<string, string> Parameters
        {
            get
            {
                return _valid || ValidateFormData(new[] { txtAuthLogin, txtAuthPassword, txtAdditionalPrice, txtCityFrom }, new[] { txtAdditionalPrice })
                                    ? new Dictionary<string, string>
                               {
                                   {SdekTemplate.AuthLogin, txtAuthLogin.Text.Trim()},
                                   {SdekTemplate.AuthPassword, txtAuthPassword.Text.Trim()},
                                   {SdekTemplate.Tariff, ddlTariff.SelectedValue},
                                   {SdekTemplate.AdditionalPrice, txtAdditionalPrice.Text.Trim()},
                                   {SdekTemplate.CityFrom, txtCityFrom.Text.Trim()},
                                   {DefaultWeightParams.DefaultWeight, txtWeight.Text },
                                   {DefaultCargoParams.DefaultLength, txtLength.Text },
                                   {DefaultCargoParams.DefaultWidth, txtWidth.Text },
                                   {DefaultCargoParams.DefaultHeight, txtHeight.Text },
                                   {SdekTemplate.DefaultCourierNameContact, txtSenderName.Text},
                                   {SdekTemplate.DefaultCourierPhone, txtSenderPhone.Text},
                                   {SdekTemplate.DefaultCourierCity, txtSenderCity.Text},
                                   {SdekTemplate.DefaultCourierStreet, txtSenderStreet.Text},
                                   {SdekTemplate.DefaultCourierHouse, txtSenderHouse.Text},
                                   {SdekTemplate.DefaultCourierFlat, txtSenderFlat.Text},
                                   {SdekTemplate.TypeAdditionPrice, ddlTypeAddPrice.SelectedValue },
                                   {SdekTemplate.DescriptionForSendOrder, txtDescrForSend.Text },
                                   {SdekTemplate.DeliveryNote, txtCountDeviveryDoc.Text }
                               }
                                    : null;
            }
            set
            {
                txtSenderFlat.Text = value.ElementOrDefault(SdekTemplate.DefaultCourierFlat);
                txtSenderHouse.Text = value.ElementOrDefault(SdekTemplate.DefaultCourierHouse);
                txtSenderStreet.Text = value.ElementOrDefault(SdekTemplate.DefaultCourierStreet);
                txtSenderPhone.Text = value.ElementOrDefault(SdekTemplate.DefaultCourierPhone);
                txtSenderName.Text = value.ElementOrDefault(SdekTemplate.DefaultCourierNameContact);
                txtSenderCity.Text = value.ElementOrDefault(SdekTemplate.DefaultCourierCity);
                txtAuthLogin.Text = value.ElementOrDefault(SdekTemplate.AuthLogin);
                txtAuthPassword.Text = value.ElementOrDefault(SdekTemplate.AuthPassword);
                txtAdditionalPrice.Text = value.ElementOrDefault(SdekTemplate.AdditionalPrice);
                txtCityFrom.Text = value.ElementOrDefault(SdekTemplate.CityFrom);

                if (!value.ElementOrDefault(SdekTemplate.TypeAdditionPrice).IsNullOrEmpty())
                {
                    ddlTypeAddPrice.SelectedValue = value.ElementOrDefault(SdekTemplate.TypeAdditionPrice);
                }

                if (ddlTariff.Items.FindByValue(value.ElementOrDefault(SdekTemplate.Tariff)) != null)
                {
                    ddlTariff.SelectedValue = value.ElementOrDefault(SdekTemplate.Tariff);
                }

                txtWeight.Text = value.ElementOrDefault(DefaultWeightParams.DefaultWeight);
                txtLength.Text = value.ElementOrDefault(DefaultCargoParams.DefaultLength);
                txtWidth.Text = value.ElementOrDefault(DefaultCargoParams.DefaultWidth);
                txtHeight.Text = value.ElementOrDefault(DefaultCargoParams.DefaultHeight);
                txtDescrForSend.Text = value.ElementOrDefault(SdekTemplate.DescriptionForSendOrder);
                txtCountDeviveryDoc.Text = value.ElementOrDefault(SdekTemplate.DeliveryNote);
            }
        }


        protected void btnCallCourier_OnClick(object sender, EventArgs e)
        {
            ClearMsg();

            if (string.IsNullOrEmpty(txtSenderCity.Text) || string.IsNullOrEmpty(txtSenderStreet.Text) || string.IsNullOrEmpty(txtSenderHouse.Text)
                || string.IsNullOrEmpty(txtSenderFlat.Text) || string.IsNullOrEmpty(txtSenderPhone.Text) || string.IsNullOrEmpty(txtSenderName.Text)
                || string.IsNullOrEmpty(txtSenderWeght.Text))
            {
                Msg("Не заполнены обязательные поля");
                return;
            }

            int shippingId = Request["ShippingMethodID"].ToString().TryParseInt();
            var shippingMethod = ShippingMethodService.GetShippingMethod(shippingId);
            if (shippingMethod == null || shippingMethod.ShippingType != "Sdek")
            {
                Msg("Не удалось получить метод доставки");
                return;
            }

            var timeBegin = txtTimeFrom.Text.Split(new[] { ':' });
            if (timeBegin.Length != 2)
            {
                Msg("Неверно указано начальное время ожидания");
                return;
            }

            var timeEnd = txtTimeTo.Text.Split(new[] { ':' });
            if (timeEnd.Length != 2)
            {
                Msg("Неверно указано конечное время ожидания");
                return;
            }

            int BeginsTime = timeBegin[0].TryParseInt();
            int EndsTime = timeEnd[0].TryParseInt();
            var resultTime = Math.Abs(BeginsTime - EndsTime);
            if (resultTime < 3)
            {
                Msg("Минимальный промежуток ожидания 3 часа");
                return;
            }

            if (txtSenderWeght.Text.TryParseFloat() <= 0)
            {
                Msg("Не указан вес заказа");
                return;
            }

            if (txtSenderPhone.Text.IsNullOrEmpty())
            {
                Msg("Слишком короткий номер телефоно, либо не указан");
                return;
            }

            var cityIdSkek = SdekService.GetSdekCityId(txtSenderCity.Text, string.Empty);
            if (cityIdSkek == 0)
            {
                Msg("В системе СДЭК отсутствует данный город, проверьте написание");
                return;
            }

            var dict = new Dictionary<string, string>
                {
                    {SdekTemplate.DefaultCourierCity, txtSenderCity.Text},
                    {SdekTemplate.DefaultCourierPhone, txtSenderPhone.Text},
                    {SdekTemplate.DefaultCourierStreet, txtSenderStreet.Text},
                    {SdekTemplate.DefaultCourierHouse, txtSenderHouse.Text},
                    {SdekTemplate.DefaultCourierFlat, txtSenderFlat.Text},
                    {SdekTemplate.DefaultCourierNameContact, txtSenderName.Text},
                };
            if (shippingId > 0)
            {
                ShippingMethodService.UpdateShippingParams(shippingId, dict);
            }

            var date = Convert.ToDateTime(txtDate.Text);
            var dateBegin = new DateTime(date.Year, date.Month, date.Day, Convert.ToInt32(timeBegin[0]), Convert.ToInt32(timeBegin[1]), 0);
            var dateEnd = new DateTime(date.Year, date.Month, date.Day, Convert.ToInt32(timeEnd[0]), Convert.ToInt32(timeEnd[1]), 0);

            SdekStatusAnswer result = (new Sdek(shippingMethod, null)).CallCourier(
                date,
                dateBegin,
                dateEnd,
                txtSenderCity.Text,
                txtSenderStreet.Text,
                txtSenderHouse.Text,
                txtSenderFlat.Text,
                txtSenderPhone.Text,
                txtSenderName.Text,
                txtSenderWeght.Text);

            Msg(result.Status ? "Заявка отпавлена" : "Ошибка: " + result.Message);
        }
    }
}
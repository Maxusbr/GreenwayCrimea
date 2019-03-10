using AdvantShop.Core.Modules;
using AdvantShop.Diagnostics;
using AdvantShop.Module.SimaLand.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvantShop.Module.SimaLand.Service
{
    public class AdvPropertiesService
    {
        public static void CheckProperties(SimalandProduct sProduct, int productId)
        {
            if (!string.IsNullOrEmpty(sProduct.has_battery))
            {
                Properties(productId, "В комплекте есть батарея", sProduct.has_battery.TryParseBoolean().YesNo());
            }

            if (!string.IsNullOrEmpty(sProduct.has_sound))
            {
                Properties(productId, "Есть звук", sProduct.has_sound.TryParseBoolean().YesNo());
            }

            if (!string.IsNullOrEmpty(sProduct.has_radiocontrol))
            {
                Properties(productId, "Есть дистанционное управление", sProduct.has_radiocontrol.TryParseBoolean().YesNo());
            }

            if (!string.IsNullOrEmpty(sProduct.is_on_ac_power))
            {
                Properties(productId, "Работает от сети", sProduct.is_on_ac_power.TryParseBoolean().YesNo());
            }

            if (!string.IsNullOrEmpty(sProduct.has_rus_voice))
            {
                Properties(productId, "Есть русский голос", sProduct.has_rus_voice.TryParseBoolean().YesNo());
            }

            if (!string.IsNullOrEmpty(sProduct.min_age.ToString()))
            {
                Properties(productId, "Рекомендуемый возраст", sProduct.min_age.ToString());
            }

            if (!string.IsNullOrEmpty(sProduct.power.ToString()))
            {
                Properties(productId, "Мощность", sProduct.power.ToString());
            }

            //

            if (!string.IsNullOrEmpty(sProduct.volume.ToString()))
            {
                Properties(productId, "Объем, л", sProduct.volume.ToString());
            }

            if (!string.IsNullOrEmpty(sProduct.product_volume.ToString()))
            {
                Properties(productId, "Объем продукта, л", sProduct.product_volume.ToString());
            }

            if (!string.IsNullOrEmpty(sProduct.has_body_drawing.ToString()))
            {
                Properties(productId, "Наличие рисунка на корпусе", sProduct.has_body_drawing.ToString());
            }

            if (!string.IsNullOrEmpty(sProduct.isbn))
            {
                Properties(productId, "Международный стандартный книжный номер", sProduct.isbn);
            }
            //

            if (!string.IsNullOrEmpty(sProduct.page_count.ToString()))
            {
                Properties(productId, "Количество страниц", sProduct.page_count.ToString());
            }

            if (sProduct.country != null)
            {
                Properties(productId, "Страна производитель", sProduct.country.name.ToString());
            }

            if (!string.IsNullOrEmpty(sProduct.stuff))
            {
                Properties(productId, "Материалы", sProduct.stuff.ToString());
            }

            if (sProduct.attrs != null)
            {
                foreach (var attr in sProduct.attrs)
                {
                    if (attr != null)
                    {
                        Properties(productId, attr.attr_name, attr.value);
                    }
                }
            }

            //Certificate
            try
            {
                if (SimalandProductService.Certificates != null)
                    Properties(productId, "Тип сертификата", SimalandProductService.Certificates.First(x => x.id == sProduct.certificate_type_id).text);
            }
            catch (Exception ex)
            {
                LogService.ErrLog(ex.Message);
            }

        }

        private static void Properties(int productId, string type, string value)
        {
            try
            {
                var range = 0;
                var sort = 0;
                ModulesRepository.ModuleExecuteNonQuery("[Catalog].[sp_ParseProductProperty]", CommandType.StoredProcedure,
                                              new SqlParameter("@nameProperty", type),
                                              new SqlParameter("@propertyValue", value),
                                              new SqlParameter("@rangeValue", range),
                                              new SqlParameter("@productId", productId),
                                              new SqlParameter("@sort", sort));
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }
        }
    }
}

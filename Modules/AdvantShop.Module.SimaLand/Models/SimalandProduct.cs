using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvantShop.Module.SimaLand.Models
{
    public class SimalandProduct
    {
        public int id { get; set; }
        public int sid { get; set; }
        public string uid { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
        public string balance { get; set; }
        public int is_disabled { get; set; }
        public string reason_of_disabling { get; set; }
        public int? category_id { get; set; }
        public float price { get; set; }
        public string currency { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public int? country_id { get; set; }
        public int is_adult { get; set; }
        public int parent_item_id { get; set; }
        public string description { get; set; }
        //public int is_markdown { get; set; }
        public float? box_depth { get; set; }
        public float? box_height { get; set; }
        public float? box_width { get; set; }
        public float depth { get; set; }
        public float height { get; set; }
        public float width { get; set; }
        public float weight { get; set; }
        //public int is_price_fixed { get; set; }
        public int? trademark_id { get; set; }
        public string has_battery { get; set; }
        public string has_sound { get; set; }
        public string has_radiocontrol { get; set; }
        public string is_on_ac_power { get; set; }
        public string has_rus_voice { get; set; }
        public int? min_age { get; set; }
        public float? power { get; set; }
        public float? volume { get; set; }
        public float? product_volume { get; set; }
        public int? has_body_drawing { get; set; }
        public string isbn { get; set; }
        public int? page_count { get; set; }
        public SimalandCountry country { get; set; }
        public string stuff { get; set; }
        public SimalandTrademark trademark { get; set; }
        public int is_hit { get; set; }
        public bool isNovelty { get; set; }
        public SimalandOffer offer { get; set; }
        public int[] categories { get; set; }
        public string has_discount { get; set; }
        public int? discountPercent { get; set; }
        public float price_max { get; set; }
        public int? max_qty { get; set; }
        public int? min_qty { get; set; }
        public int? has_3_pay_2_action { get; set; }
        public string hasGift { get; set; }
        public List<SlAttributes> attrs { get; set; }
        public int certificate_type_id { get; set; }
        public int[] photoIndexes { get; set; }

        //public string hasGiftAssignee { get; set; }
        //public int gift_id { get; set; }
        //public int is_gift { get; set; }



        //public string minimum_order_quantity { get; set; }
        //public float price_per_square_meter { get; set; }
        //public float price_per_linear_meter { get; set; }
        //public int? boxtype_id { get; set; }
        //public int in_box { get; set; }
        //public float in_set { get; set; }
        //public int unit_id { get; set; }
        //public int? nested_unit_id { get; set; }
        //public int cart_min_diff { get; set; }
        //public int keep_package { get; set; }
        //public int per_package { get; set; }
        //public string video_file_name { get; set; }
        //public bool video_file_url { get; set; }
        //public string supplier_code { get; set; }
        //public int? series_id { get; set; }
        //public int? is_licensed { get; set; }
        //public int is_exclusive { get; set; }
        //public int is_motley { get; set; }
        //public int is_protected { get; set; }
        //public string offer_id { get; set; }
        //public int certificated_type_id { get; set; }
        //public int has_usb { get; set; }
        //public int has_clockwork { get; set; }
        //public int is_inertial { get; set; }
        //public int has_rus_pack { get; set; }
        //public int has_light { get; set; }
        //public int is_day_offer { get; set; }
        //public string page_title { get; set; }
        //public string page_keywords { get; set; }
        //public string page_description { get; set; }
        //public int modifier_id { get; set; }
        //public string modifier_value { get; set; }
        //public float surface_area { get; set; }
        //public float linear_meters { get; set; }
        //public int is_loco { get; set; }
        //public string novelted_at { get; set; }
        //public int is_paid_delivery { get; set; }
        //public float package_volume { get; set; }
        //public int transport_condition_id { get; set; }
        //public int is_boxed { get; set; }
        //public float box_volume { get; set; }
        //public int box_capacity { get; set; }
        //public float packing_volume_factor { get; set; }
        //public int is_tire_spike { get; set; }
        //public int is_tire_run_flat { get; set; }
        //public int tire_season_id { get; set; }
        //public int tire_diameter_id { get; set; }
        //public int tire_width_id { get; set; }
        //public int tire_section_height_id { get; set; }
        //public int tire_load_index_id { get; set; }
        //public int tire_speed_index_id { get; set; }
        //public int wheel_lz_id { get; set; }
        //public int wheel_width_id { get; set; }
        //public int wheel_diameter_id { get; set; }
        //public int wheel_dia_id { get; set; }
        //public int wheel_pcd_id { get; set; }
        //public int wheel_et_id { get; set; }
        //public int has_cord_case { get; set; }
        //public int has_teapot { get; set; }
        //public int has_termostat { get; set; }
        //public int is_imprintable { get; set; }
        //public int is_add_to_cart_multiple { get; set; }
        //public int supply_period { get; set; }
        //public int has_action { get; set; }
        //public int has_jewwelry_action { get; set; }
        //public int has_best_fabric { get; set; }
        //public int has_number_one_made_in_russia { get; set; }
        //public string photoIndexes { get; set; }
        //public string audio_filename { get; set; }
        //public int photo_3d_count { get; set; }
        //public int is_prepay_needed { get; set; }
        //public bool is_paid_delivery_ekb { get; set; }
        //public int mean_rating { get; set; }
        //public int comments_count { get; set; }
        //public string currencySign { get; set; }
        //public string isEnough { get; set; }
        //public bool isAddToCartMultiple { get; set; }
        //public string minQty { get; set; }
        //public string qtyRule { get; set; }
        //public string pluralNameFormat { get; set; }
        //public string inBoxPluralNameFormat { get; set; }
        //public string balancePluralNameFormat { get; set; }
        //public string photos { get; set; }
        //public string img { get; set; }

        //public string itemUrl { get; set; }
        //public int hasVolumeDiscount { get; set; }
        //public string modifier { get; set; }
        //public string size { get; set; }
        //public string series { get; set; }
        //public string ecommerce_variant { get; set; }
        //public int loan_category_id { get; set; }
        //public string cart_item { get; set; }

        public class V5
        {
            public string balance { get; set; }
            public float price { get; set; }
            public int sid { get; set; }
            public int id { get; set; }
        }

        public class SlAttributes
        {
            public string attr_name { get; set; }
            public string value { get; set; }
        }

    }
}

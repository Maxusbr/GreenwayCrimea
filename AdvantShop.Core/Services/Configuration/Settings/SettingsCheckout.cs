//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using AdvantShop.Customers;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Saas;

namespace AdvantShop.Configuration
{
    public enum eOutOfStockAction
    {
        Preorder,
        Order,
        Lead,
        Cart
    }

    public class SettingsCheckout
    {
        #region Settings

        public static bool AmountLimitation
        {
            get { return Convert.ToBoolean(SettingProvider.Items["AmountLimitation"]); }
            set { SettingProvider.Items["AmountLimitation"] = value.ToString(); }
        }

        public static eOutOfStockAction OutOfStockAction
        {
            get {
                var action = SettingProvider.Items["OutOfStockAction"].TryParseEnum<eOutOfStockAction>();
                return action == eOutOfStockAction.Lead && SaasDataService.IsSaasEnabled && !SaasDataService.CurrentSaasData.HaveCrm ? eOutOfStockAction.Order : action;
            }
            set { SettingProvider.Items["OutOfStockAction"] = value.ToString(); }
        }



        public static bool DecrementProductsCount
        {
            get { return Convert.ToBoolean(SettingProvider.Items["DecrementProductsCount"]); }
            set { SettingProvider.Items["DecrementProductsCount"] = value.ToString(); }
        }

        public static bool ProceedToPayment
        {
            get { return Convert.ToBoolean(SettingProvider.Items["ProceedToPayment"]); }
            set { SettingProvider.Items["ProceedToPayment"] = value.ToString(); }
        }

        public static float MinimalOrderPriceForDefaultGroup
        {
            get
            {
                return CustomerGroupService.GetMinimumOrderPrice(CustomerGroupService.DefaultCustomerGroup);
            }
            set
            {
                var group = CustomerGroupService.GetCustomerGroup(CustomerGroupService.DefaultCustomerGroup);
                group.MinimumOrderPrice = value;
                CustomerGroupService.UpdateCustomerGroup(group);
            }
        }

        public static float MinimalPriceCertificate
        {
            get
            {
                float minimalPriceCertificate = 0;
                float.TryParse(SettingProvider.Items["MinimalPriceCertificate"], out minimalPriceCertificate);
                return minimalPriceCertificate;
            }
            set { SettingProvider.Items["MinimalPriceCertificate"] = value.ToString("#0.00"); }
        }

        public static float MaximalPriceCertificate
        {
            get
            {
                float maximalPriceCertificate = 0;
                float.TryParse(SettingProvider.Items["MaximalPriceCertificate"], out maximalPriceCertificate);
                return maximalPriceCertificate;
            }
            set { SettingProvider.Items["MaximalPriceCertificate"] = value.ToString("#0.00"); }
        }

        public static bool EnableGiftCertificateService
        {
            get { return Convert.ToBoolean(SettingProvider.Items["EnableGiftCertificateService"]); }
            set { SettingProvider.Items["EnableGiftCertificateService"] = value.ToString(); }
        }

        public static bool DisplayPromoTextbox
        {
            get { return Convert.ToBoolean(SettingProvider.Items["DisplayPromoTextbox"]); }
            set { SettingProvider.Items["DisplayPromoTextbox"] = value.ToString(); }
        }


        public static bool EnableDiscountModule
        {
            get { return Convert.ToBoolean(SettingProvider.Items["EnableDiscountModule"]); }
            set { SettingProvider.Items["EnableDiscountModule"] = value.ToString(); }
        }

        public static bool EnableManagersModule
        {
            get { return true; }  // always true according SPRIN-929
            //get { return Convert.ToBoolean(SettingProvider.Items["EnableManagersModule"]); }
            set {
                //SettingProvider.Items["EnableManagersModule"] = value.ToString();
            }
        }
        public static bool ShowManagersPage
        {
            get { return Convert.ToBoolean(SettingProvider.Items["ShowManagersPage"]); }
            set { SettingProvider.Items["ShowManagersPage"] = value.ToString(); }
        }

        public static bool PrintOrder_ShowStatusInfo
        {
            get { return Convert.ToBoolean(SettingProvider.Items["PrintOrder_ShowStatusInfo"]); }
            set { SettingProvider.Items["PrintOrder_ShowStatusInfo"] = value.ToString(); }
        }

        public static bool PrintOrder_ShowMap
        {
            get { return Convert.ToBoolean(SettingProvider.Items["PrintOrder_ShowMap"]); }
            set { SettingProvider.Items["PrintOrder_ShowMap"] = value.ToString(); }
        }

        public static string PrintOrder_MapType
        {
            get { return SettingProvider.Items["PrintOrder_MapType"]; }
            set { SettingProvider.Items["PrintOrder_MapType"] = value; }
        }

        public static string SuccessOrderScript
        {
            get { return SettingProvider.Items["SuccessOrderScript"]; }
            set { SettingProvider.Items["SuccessOrderScript"] = value; }
        }

        public static string OrderNumberFormat
        {
            get { return SettingProvider.Items["OrderNumberFormat"]; }
            set { SettingProvider.Items["OrderNumberFormat"] = value; }
        }

        public static bool ManagerConfirmed
        {
            get { return Convert.ToBoolean(SettingProvider.Items["ManagerConfirmed"]); }
            set { SettingProvider.Items["ManagerConfirmed"] = value.ToString(); }
        }

        public static bool MultiplyGiftsCount
        {
            get { return Convert.ToBoolean(SettingProvider.Items["MultiplyGiftsCount"]); }
            set { SettingProvider.Items["MultiplyGiftsCount"] = value.ToString(); }
        }

        public static bool DenyToByPreorderedProductsWithZerroAmount
        {
            get { return Convert.ToBoolean(SettingProvider.Items["DenyToByPreorderedProductsWithZerroAmount"]); }
            set { SettingProvider.Items["DenyToByPreorderedProductsWithZerroAmount"] = value.ToString(); }
        }


        #endregion

        #region Buy one click

        public static bool BuyInOneClick
        {
            get { return Convert.ToBoolean(SettingProvider.Items["BuyInOneClick"]); }
            set { SettingProvider.Items["BuyInOneClick"] = value.ToString(); }
        }

        public static bool BuyInOneClickDisableInCheckout
        {
            get { return Convert.ToBoolean(SettingProvider.Items["BuyInOneClick_DisableInCheckout"]); }
            set { SettingProvider.Items["BuyInOneClick_DisableInCheckout"] = value.ToString(); }
        }
                
        public static string BuyInOneClickLinkText
        {
            get { return SettingProvider.Items["BuyInOneClick_LinkText"]; }
            set { SettingProvider.Items["BuyInOneClick_LinkText"] = value; }
        }

        public static string BuyInOneClickFirstText
        {
            get { return SettingProvider.Items["BuyInOneClick_FirstText"]; }
            set { SettingProvider.Items["BuyInOneClick_FirstText"] = value; }
        }

        public static string BuyInOneClickButtonText
        {
            get { return SettingProvider.Items["BuyInOneClick_ButtonText"]; }
            set { SettingProvider.Items["BuyInOneClick_ButtonText"] = value; }
        }

        public static bool BuyInOneClickCreateOrder
        {
            get { return Convert.ToBoolean(SettingProvider.Items["BuyInOneClick_CreateOrder"]); }
            set { SettingProvider.Items["BuyInOneClick_CreateOrder"] = value.ToString(); }
        }



        public static string BuyInOneClickName
        {
            get { return SettingProvider.Items["BuyInOneClickName"]; }
            set { SettingProvider.Items["BuyInOneClickName"] = value; }
        }

        public static bool IsShowBuyInOneClickName
        {
            get { return Convert.ToBoolean(SettingProvider.Items["IsShowBuyInOneClickName"]); }
            set { SettingProvider.Items["IsShowBuyInOneClickName"] = value.ToString(); }
        }

        public static bool IsRequiredBuyInOneClickName
        {
            get { return Convert.ToBoolean(SettingProvider.Items["IsRequiredBuyInOneClickName"]); }
            set { SettingProvider.Items["IsRequiredBuyInOneClickName"] = value.ToString(); }
        }

        public static string BuyInOneClickEmail
        {
            get { return SettingProvider.Items["BuyInOneClickEmail"]; }
            set { SettingProvider.Items["BuyInOneClickEmail"] = value; }
        }

        public static bool IsShowBuyInOneClickEmail
        {
            get { return Convert.ToBoolean(SettingProvider.Items["IsShowBuyInOneClickEmail"]); }
            set { SettingProvider.Items["IsShowBuyInOneClickEmail"] = value.ToString(); }
        }

        public static bool IsRequiredBuyInOneClickEmail
        {
            get { return Convert.ToBoolean(SettingProvider.Items["IsRequiredBuyInOneClickEmail"]); }
            set { SettingProvider.Items["IsRequiredBuyInOneClickEmail"] = value.ToString(); }
        }

        public static string BuyInOneClickPhone
        {
            get { return SettingProvider.Items["BuyInOneClickPhone"]; }
            set { SettingProvider.Items["BuyInOneClickPhone"] = value; }
        }

        public static bool IsShowBuyInOneClickPhone
        {
            get { return Convert.ToBoolean(SettingProvider.Items["IsShowBuyInOneClickPhone"]); }
            set { SettingProvider.Items["IsShowBuyInOneClickPhone"] = value.ToString(); }
        }

        public static bool IsRequiredBuyInOneClickPhone
        {
            get { return Convert.ToBoolean(SettingProvider.Items["IsRequiredBuyInOneClickPhone"]); }
            set { SettingProvider.Items["IsRequiredBuyInOneClickPhone"] = value.ToString(); }
        }

        public static string BuyInOneClickComment
        {
            get { return SettingProvider.Items["BuyInOneClickComment"]; }
            set { SettingProvider.Items["BuyInOneClickComment"] = value; }
        }

        public static bool IsShowBuyInOneClickComment
        {
            get { return Convert.ToBoolean(SettingProvider.Items["IsShowBuyInOneClickComment"]); }
            set { SettingProvider.Items["IsShowBuyInOneClickComment"] = value.ToString(); }
        }

        public static bool IsRequiredBuyInOneClickComment
        {
            get { return Convert.ToBoolean(SettingProvider.Items["IsRequiredBuyInOneClickComment"]); }
            set { SettingProvider.Items["IsRequiredBuyInOneClickComment"] = value.ToString(); }
        }

        public static int BuyInOneClickDefaultShippingMethod
        {
            get { return SettingProvider.Items["BuyInOneClickDefaultShippingMethod"].TryParseInt(); }
            set { SettingProvider.Items["BuyInOneClickDefaultShippingMethod"] = value.ToString(); }
        }

        public static int BuyInOneClickDefaultPaymentMethod
        {
            get { return SettingProvider.Items["BuyInOneClickDefaultPaymentMethod"].TryParseInt(); }
            set { SettingProvider.Items["BuyInOneClickDefaultPaymentMethod"] = value.ToString(); }
        }

        #endregion

        #region Checkout Fields

        public static string CustomerFirstNameField
        {
            get { return SettingProvider.Items["CustomerFirstNameField"]; }
            set { SettingProvider.Items["CustomerFirstNameField"] = value; }
        }

        public static bool IsShowLastName
        {
            get { return Convert.ToBoolean(SettingProvider.Items["IsShowLastName"]); }
            set { SettingProvider.Items["IsShowLastName"] = value.ToString(); }
        }

        public static bool IsRequiredLastName
        {
            get { return Convert.ToBoolean(SettingProvider.Items["IsRequiredLastName"]); }
            set { SettingProvider.Items["IsRequiredLastName"] = value.ToString(); }
        }

        public static bool IsShowPatronymic
        {
            get { return Convert.ToBoolean(SettingProvider.Items["IsShowPatronymic"]); }
            set { SettingProvider.Items["IsShowPatronymic"] = value.ToString(); }
        }

        public static bool IsRequiredPatronymic
        {
            get { return Convert.ToBoolean(SettingProvider.Items["IsRequiredPatronymic"]); }
            set { SettingProvider.Items["IsRequiredPatronymic"] = value.ToString(); }
        }

        public static string CustomerPhoneField
        {
            get { return SettingProvider.Items["CustomerPhoneField"]; }
            set { SettingProvider.Items["CustomerPhoneField"] = value; }
        }

        public static bool IsShowPhone
        {
            get { return Convert.ToBoolean(SettingProvider.Items["IsShowPhone"]); }
            set { SettingProvider.Items["IsShowPhone"] = value.ToString(); }
        }

        public static bool IsRequiredPhone
        {
            get { return Convert.ToBoolean(SettingProvider.Items["IsRequiredPhone"]); }
            set { SettingProvider.Items["IsRequiredPhone"] = value.ToString(); }
        }

        public static bool IsShowCountry
        {
            get { return Convert.ToBoolean(SettingProvider.Items["IsShowCountry"]); }
            set { SettingProvider.Items["IsShowCountry"] = value.ToString(); }
        }

        public static bool IsRequiredCountry
        {
            get { return Convert.ToBoolean(SettingProvider.Items["IsRequiredCountry"]); }
            set { SettingProvider.Items["IsRequiredCountry"] = value.ToString(); }
        }

        public static bool IsShowState
        {
            get { return Convert.ToBoolean(SettingProvider.Items["IsShowState"]); }
            set { SettingProvider.Items["IsShowState"] = value.ToString(); }
        }

        public static bool IsRequiredState
        {
            get { return Convert.ToBoolean(SettingProvider.Items["IsRequiredState"]); }
            set { SettingProvider.Items["IsRequiredState"] = value.ToString(); }
        }

        public static bool IsShowCity
        {
            get { return Convert.ToBoolean(SettingProvider.Items["IsShowCity"]); }
            set { SettingProvider.Items["IsShowCity"] = value.ToString(); }
        }

        public static bool IsRequiredCity
        {
            get { return Convert.ToBoolean(SettingProvider.Items["IsRequiredCity"]); }
            set { SettingProvider.Items["IsRequiredCity"] = value.ToString(); }
        }

        public static bool IsShowZip
        {
            get { return Convert.ToBoolean(SettingProvider.Items["IsShowZip"]); }
            set { SettingProvider.Items["IsShowZip"] = value.ToString(); }
        }

        public static bool IsRequiredZip
        {
            get { return Convert.ToBoolean(SettingProvider.Items["IsRequiredZip"]); }
            set { SettingProvider.Items["IsRequiredZip"] = value.ToString(); }
        }

        public static bool IsShowAddress
        {
            get { return Convert.ToBoolean(SettingProvider.Items["IsShowAddress"]); }
            set { SettingProvider.Items["IsShowAddress"] = value.ToString(); }
        }

        public static bool IsRequiredAddress
        {
            get { return Convert.ToBoolean(SettingProvider.Items["IsRequiredAddress"]); }
            set { SettingProvider.Items["IsRequiredAddress"] = value.ToString(); }
        }

        public static bool IsShowUserAgreementText
        {
            get { return SettingProvider.Items["IsShowUserAgreementText"] == null ? true : Convert.ToBoolean(SettingProvider.Items["IsShowUserAgreementText"]); }
            set { SettingProvider.Items["IsShowUserAgreementText"] = value.ToString(); }
        }

        public static string UserAgreementText
        {
            get { return SettingProvider.Items["UserAgreementText"]; }
            set { SettingProvider.Items["UserAgreementText"] = value; }
        }

        public static bool IsShowUserComment
        {
            get { return Convert.ToBoolean(SettingProvider.Items["IsShowUserComment"]); }
            set { SettingProvider.Items["IsShowUserComment"] = value.ToString(); }
        }

        public static bool IsShowFullAddress
        {
            get { return Convert.ToBoolean(SettingProvider.Items["IsShowFullAddress"]); }
            set { SettingProvider.Items["IsShowFullAddress"] = value.ToString(); }
        }

        public static bool ZipDisplayPlace
        {
            get { return Convert.ToBoolean(SettingProvider.Items["ZipDisplayPlace"]); }
            set { SettingProvider.Items["ZipDisplayPlace"] = value.ToString(); }
        }

        
        #region Custom shipping fields

        public static bool IsShowCustomShippingField1
        {
            get { return Convert.ToBoolean(SettingProvider.Items["IsShowCustomShippingField1"]); }
            set { SettingProvider.Items["IsShowCustomShippingField1"] = value.ToString(); }
        }

        public static bool IsReqCustomShippingField1
        {
            get { return Convert.ToBoolean(SettingProvider.Items["IsReqCustomShippingField1"]); }
            set { SettingProvider.Items["IsReqCustomShippingField1"] = value.ToString(); }
        }

        public static string CustomShippingField1
        {
            get { return SettingProvider.Items["CustomShippingField1"]; }
            set { SettingProvider.Items["CustomShippingField1"] = value; }
        }


        public static bool IsShowCustomShippingField2
        {
            get { return Convert.ToBoolean(SettingProvider.Items["IsShowCustomShippingField2"]); }
            set { SettingProvider.Items["IsShowCustomShippingField2"] = value.ToString(); }
        }

        public static bool IsReqCustomShippingField2
        {
            get { return Convert.ToBoolean(SettingProvider.Items["IsReqCustomShippingField2"]); }
            set { SettingProvider.Items["IsReqCustomShippingField2"] = value.ToString(); }
        }

        public static string CustomShippingField2
        {
            get { return SettingProvider.Items["CustomShippingField2"]; }
            set { SettingProvider.Items["CustomShippingField2"] = value; }
        }


        public static bool IsShowCustomShippingField3
        {
            get { return Convert.ToBoolean(SettingProvider.Items["IsShowCustomShippingField3"]); }
            set { SettingProvider.Items["IsShowCustomShippingField3"] = value.ToString(); }
        }

        public static bool IsReqCustomShippingField3
        {
            get { return Convert.ToBoolean(SettingProvider.Items["IsReqCustomShippingField3"]); }
            set { SettingProvider.Items["IsReqCustomShippingField3"] = value.ToString(); }
        }

        public static string CustomShippingField3
        {
            get { return SettingProvider.Items["CustomShippingField3"]; }
            set { SettingProvider.Items["CustomShippingField3"] = value; }
        }

        #endregion

        #endregion
    }
}
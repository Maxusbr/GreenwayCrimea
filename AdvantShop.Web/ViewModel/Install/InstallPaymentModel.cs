using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Resources;

namespace AdvantShop.ViewModel.Install
{
    public class InstallPaymentModel : IValidatableObject
    {
        public bool Cash { get; set; }
        public bool ShowFizBank { get; set; }
        public bool FizBank { get; set; }
        public bool ShowUrBank { get; set; }
        public bool UrBank { get; set; }

        public bool ShowCreditCard { get; set; }
        public bool CreditCard { get; set; }
        public string CreditcardType { get; set; }
        public string LoginRobokassaCreditcard { get; set; }
        public string PasswordRobokassaCreditcard { get; set; }
        public string ShopIdAssistCreditcard { get; set; }
        public string LoginAssistCreditcard { get; set; }
        public string PassAssistCreditcard { get; set; }
        public string SellerIdPlatronCreditcard { get; set; }
        public string PaySystemCreditcard { get; set; }
        public string PayPassCreditcard { get; set; }
        public string PayPoketCreditcard { get; set; }
        public string PassZpaymentCreditcard { get; set; }
        public string SecretKeyZpaymentCreditcard { get; set; }


        public bool ShowEMony { get; set; }
        public bool EMoney { get; set; }
        public string EMoneyType { get; set; }
        public string LoginRobokassaElectronMoney { get; set; }
        public string PassRobokassaElectronMoney { get; set; }
        public string ShopIdAssistElectronMoney { get; set; }
        public string LoginAssistElectronMoney { get; set; }
        public string PassAssistElectronMoney { get; set; }
        public string SellerIdPlatronElectronMoney { get; set; }
        public string PaySystemElectronMoney { get; set; }
        public string PayPassElectronMoney { get; set; }
        public string PayPoketElectronMoney { get; set; }
        public string PassZpaymentElectronMoney { get; set; }
        public string SecretKeyZpaymentElectronMoney { get; set; }

        public bool ShowTerminals { get; set; }
        public bool Terminals { get; set; }
        public string TerminalType { get; set; }
        public string LoginRobokassaTerminals { get; set; }
        public string PassRobokassaTerminals { get; set; }
        public string ShopIdAssistTerminals { get; set; }
        public string LoginAssistTerminals { get; set; }
        public string PassAssistTerminals { get; set; }
        public string SellerIdPlatronTerminals { get; set; }
        public string PaySystemTerminals { get; set; }
        public string PayPassTerminals { get; set; }
        public string PayPoketTerminals { get; set; }
        public string PassZpaymentTerminals { get; set; }
        public string SecretKeyZpaymentTerminals { get; set; }


        public bool ShowIphone { get; set; }
        public bool Iphone { get; set; }
        public string IphoneType { get; set; }
        public string LoginRobokassaIPhone { get; set; }
        public string PassRobokassaIPhone { get; set; }

        public string BackUrl { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (CreditCard && CreditcardType != null)
            {
                if (CreditcardType.ToLower() == "robokassa")
                {
                    if (string.IsNullOrWhiteSpace(LoginRobokassaCreditcard))
                        yield return
                            new ValidationResult(Resource.Install_UserContols_PaymentView_Err_NeedRobokassaLogin,
                                new[] {"Creditcard"});

                    if (string.IsNullOrWhiteSpace(PasswordRobokassaCreditcard))
                        yield return
                            new ValidationResult(Resource.Install_UserContols_PaymentView_Err_NeedRobokassaPass,
                                new[] {"Creditcard"});
                }

                if (CreditcardType.ToLower() == "assist")
                {
                    if (string.IsNullOrWhiteSpace(LoginAssistCreditcard))
                        yield return
                            new ValidationResult(Resource.Install_UserContols_PaymentView_Err_NeedAssistLogin,
                                new[] {"Creditcard"});

                    if (string.IsNullOrWhiteSpace(PassAssistCreditcard))
                        yield return
                            new ValidationResult(Resource.Install_UserContols_PaymentView_Err_NeedAssistPass,
                                new[] {"Creditcard"});

                    if (string.IsNullOrWhiteSpace(ShopIdAssistCreditcard))
                        yield return
                            new ValidationResult(Resource.Install_UserContols_PaymentView_Err_NeedAssistShopId,
                                new[] {"Creditcard"});
                }

                if (CreditcardType.ToLower() == "platron")
                {
                    if (string.IsNullOrWhiteSpace(SellerIdPlatronCreditcard))
                        yield return
                            new ValidationResult(Resource.Install_UserContols_PaymentView_Err_NeedPlatronSellerId,
                                new[] {"Creditcard"});

                    if (string.IsNullOrWhiteSpace(PaySystemCreditcard))
                        yield return
                            new ValidationResult(Resource.Install_UserContols_PaymentView_Err_NeedPlatronPaySystem,
                                new[] {"Creditcard"});

                    if (string.IsNullOrWhiteSpace(PayPassCreditcard))
                        yield return
                            new ValidationResult(Resource.Install_UserContols_PaymentView_Err_NeedPlatronPass,
                                new[] {"Creditcard"});
                }

                if (CreditcardType.ToLower() == "zpayment")
                {
                    if (string.IsNullOrWhiteSpace(PayPoketCreditcard))
                        yield return
                            new ValidationResult(Resource.Install_UserContols_PaymentView_Err_NeedZpaymentPayPoket,
                                new[] { "Creditcard" });

                    if (string.IsNullOrWhiteSpace(PassZpaymentCreditcard))
                        yield return
                            new ValidationResult(Resource.Install_UserContols_PaymentView_Err_NeedZpaymentPass,
                                new[] { "Creditcard" });

                    if (string.IsNullOrWhiteSpace(SecretKeyZpaymentCreditcard))
                        yield return
                            new ValidationResult(Resource.Install_UserContols_PaymentView_Err_NeedZpaymentSecretKey,
                                new[] { "Creditcard" });
                }
            }

            if (EMoney && EMoneyType != null)
            {
                if (EMoneyType.ToLower() == "robokassa")
                {
                    if (string.IsNullOrWhiteSpace(LoginRobokassaElectronMoney))
                        yield return
                            new ValidationResult(Resource.Install_UserContols_PaymentView_Err_NeedRobokassaLogin,
                                new[] { "EMoney" });

                    if (string.IsNullOrWhiteSpace(PassRobokassaElectronMoney))
                        yield return
                            new ValidationResult(Resource.Install_UserContols_PaymentView_Err_NeedRobokassaPass,
                                new[] { "EMoney" });
                }

                if (EMoneyType.ToLower() == "assist")
                {
                    if (string.IsNullOrWhiteSpace(LoginAssistElectronMoney))
                        yield return
                            new ValidationResult(Resource.Install_UserContols_PaymentView_Err_NeedAssistLogin,
                                new[] { "EMoney" });

                    if (string.IsNullOrWhiteSpace(PassAssistElectronMoney))
                        yield return
                            new ValidationResult(Resource.Install_UserContols_PaymentView_Err_NeedAssistPass,
                                new[] { "EMoney" });

                    if (string.IsNullOrWhiteSpace(ShopIdAssistElectronMoney))
                        yield return
                            new ValidationResult(Resource.Install_UserContols_PaymentView_Err_NeedAssistShopId,
                                new[] { "EMoney" });
                }

                if (EMoneyType.ToLower() == "platron")
                {
                    if (string.IsNullOrWhiteSpace(SellerIdPlatronElectronMoney))
                        yield return
                            new ValidationResult(Resource.Install_UserContols_PaymentView_Err_NeedPlatronSellerId,
                                new[] { "EMoney" });

                    if (string.IsNullOrWhiteSpace(PaySystemElectronMoney))
                        yield return
                            new ValidationResult(Resource.Install_UserContols_PaymentView_Err_NeedPlatronPaySystem,
                                new[] { "EMoney" });

                    if (string.IsNullOrWhiteSpace(PayPassElectronMoney))
                        yield return
                            new ValidationResult(Resource.Install_UserContols_PaymentView_Err_NeedPlatronPass,
                                new[] { "EMoney" });
                }

                if (EMoneyType.ToLower() == "zpayment")
                {
                    if (string.IsNullOrWhiteSpace(PayPoketElectronMoney))
                        yield return
                            new ValidationResult(Resource.Install_UserContols_PaymentView_Err_NeedZpaymentPayPoket,
                                new[] { "EMoney" });

                    if (string.IsNullOrWhiteSpace(PassZpaymentElectronMoney))
                        yield return
                            new ValidationResult(Resource.Install_UserContols_PaymentView_Err_NeedZpaymentPass,
                                new[] { "EMoney" });

                    if (string.IsNullOrWhiteSpace(SecretKeyZpaymentElectronMoney))
                        yield return
                            new ValidationResult(Resource.Install_UserContols_PaymentView_Err_NeedZpaymentSecretKey,
                                new[] { "EMoney" });
                }
            }


            if (Terminals && TerminalType != null)
            {
                if (TerminalType.ToLower() == "robokassa")
                {
                    if (string.IsNullOrWhiteSpace(LoginRobokassaTerminals))
                        yield return
                            new ValidationResult(Resource.Install_UserContols_PaymentView_Err_NeedRobokassaLogin,
                                new[] { "Terminals" });

                    if (string.IsNullOrWhiteSpace(PassRobokassaTerminals))
                        yield return
                            new ValidationResult(Resource.Install_UserContols_PaymentView_Err_NeedRobokassaPass,
                                new[] { "Terminals" });
                }

                if (TerminalType.ToLower() == "assist")
                {
                    if (string.IsNullOrWhiteSpace(LoginAssistTerminals))
                        yield return
                            new ValidationResult(Resource.Install_UserContols_PaymentView_Err_NeedAssistLogin,
                                new[] { "Terminals" });

                    if (string.IsNullOrWhiteSpace(PassAssistTerminals))
                        yield return
                            new ValidationResult(Resource.Install_UserContols_PaymentView_Err_NeedAssistPass,
                                new[] { "Terminals" });

                    if (string.IsNullOrWhiteSpace(ShopIdAssistTerminals))
                        yield return
                            new ValidationResult(Resource.Install_UserContols_PaymentView_Err_NeedAssistShopId,
                                new[] { "Terminals" });
                }

                if (TerminalType.ToLower() == "platron")
                {
                    if (string.IsNullOrWhiteSpace(SellerIdPlatronTerminals))
                        yield return
                            new ValidationResult(Resource.Install_UserContols_PaymentView_Err_NeedPlatronSellerId,
                                new[] { "Terminals" });

                    if (string.IsNullOrWhiteSpace(PaySystemTerminals))
                        yield return
                            new ValidationResult(Resource.Install_UserContols_PaymentView_Err_NeedPlatronPaySystem,
                                new[] { "Terminals" });

                    if (string.IsNullOrWhiteSpace(PayPassTerminals))
                        yield return
                            new ValidationResult(Resource.Install_UserContols_PaymentView_Err_NeedPlatronPass,
                                new[] { "Terminals" });
                }

                if (TerminalType.ToLower() == "zpayment")
                {
                    if (string.IsNullOrWhiteSpace(PayPoketTerminals))
                        yield return
                            new ValidationResult(Resource.Install_UserContols_PaymentView_Err_NeedZpaymentPayPoket,
                                new[] { "Terminals" });

                    if (string.IsNullOrWhiteSpace(PassZpaymentTerminals))
                        yield return
                            new ValidationResult(Resource.Install_UserContols_PaymentView_Err_NeedZpaymentPass,
                                new[] { "Terminals" });

                    if (string.IsNullOrWhiteSpace(SecretKeyZpaymentTerminals))
                        yield return
                            new ValidationResult(Resource.Install_UserContols_PaymentView_Err_NeedZpaymentSecretKey,
                                new[] { "Terminals" });
                }
            }

            if (Iphone && IphoneType != null)
            {
                if (IphoneType.ToLower() == "robokassa")
                {
                    if (string.IsNullOrWhiteSpace(LoginRobokassaIPhone))
                        yield return
                            new ValidationResult(Resource.Install_UserContols_PaymentView_Err_NeedRobokassaLogin,
                                new[] { "Iphone" });

                    if (string.IsNullOrWhiteSpace(PassRobokassaIPhone))
                        yield return
                            new ValidationResult(Resource.Install_UserContols_PaymentView_Err_NeedRobokassaPass,
                                new[] { "Iphone" });
                }
            }
        }
    }
}
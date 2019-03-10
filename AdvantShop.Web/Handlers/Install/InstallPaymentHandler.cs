using AdvantShop.Configuration;
using AdvantShop.Core;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Payment;
using AdvantShop.ViewModel.Install;
using Resources;

namespace AdvantShop.Handlers.Install
{
    public class InstallPaymentHandler
    {
        public InstallPaymentModel Get()
        {
            var model = new InstallPaymentModel()
            {
                ShowFizBank = AdvantshopConfigService.GetActivityPayment("SberBank"),
                ShowUrBank = AdvantshopConfigService.GetActivityPayment("Bill"),
                ShowCreditCard = AdvantshopConfigService.GetActivityPayment("Robokassa"),
                ShowEMony = AdvantshopConfigService.GetActivityPayment("Robokassa"),
                ShowTerminals = AdvantshopConfigService.GetActivityPayment("Robokassa"),
                ShowIphone = AdvantshopConfigService.GetActivityPayment("Robokassa"),
            };

            var pm = PaymentService.GetPaymentMethodByName(Resource.Install_UserContols_PaymentView_Cash);
            model.Cash = pm != null && pm is Cash;

            pm = PaymentService.GetPaymentMethodByName(Resource.Install_UserContols_PaymentView_BankTransferFiz);
            model.FizBank = pm != null && pm is SberBank;

            pm = PaymentService.GetPaymentMethodByName(Resource.Install_UserContols_PaymentView_BankTransferUr);
            model.UrBank = pm != null && pm is Bill;

            //pm = PaymentService.GetPaymentMethodByName(Resource.Install_UserContols_PaymentView_CreditCard);
            //model.CreditCard = pm != null;

            //pm = PaymentService.GetPaymentMethodByName(Resource.Install_UserContols_PaymentView_ElectronMoney);
            //model.EMoney = pm != null;

            //pm = PaymentService.GetPaymentMethodByName(Resource.Install_UserContols_PaymentView_Terminals);
            //model.Terminals = pm != null;

            //pm = PaymentService.GetPaymentMethodByName(Resource.Install_UserContols_PaymentView_ByIPnone);
            //model.Iphone = pm != null;

            LoadCreditCard(model);
            LoadElectronMoney(model);
            LoadTerminals(model);

            pm = PaymentService.GetPaymentMethodByName(Resource.Install_UserContols_PaymentView_ByIPnone);
            if (pm != null && pm is Robokassa)
            {
                model.IphoneType = "Robokassa";
                model.LoginRobokassaIPhone = pm.Parameters[RobokassaTemplate.MerchantLogin];
                model.PassRobokassaIPhone = pm.Parameters[RobokassaTemplate.Password];
            }

            return model;
        }

        public void Update(InstallPaymentModel model)
        {
            var pm = PaymentService.GetPaymentMethodByName(Resource.Install_UserContols_PaymentView_Cash);
            if (pm != null)
                PaymentService.DeletePaymentMethod(pm.PaymentMethodId);

            if (model.Cash)
            {
                var method = new Cash
                {
                    Name = Resource.Install_UserContols_PaymentView_Cash,
                    Description = Resource.Install_UserContols_PaymentView_Cash, 
                    SortOrder = 0, 
                    Enabled = true
                };
                var id = PaymentService.AddPaymentMethod(method);
            }

            pm = PaymentService.GetPaymentMethodByName(Resource.Install_UserContols_PaymentView_BankTransferFiz);
            if (pm != null)
                PaymentService.DeletePaymentMethod(pm.PaymentMethodId);

            if (model.FizBank)
            {
                var method = new SberBank
                {
                    Name = Resource.Install_UserContols_PaymentView_BankTransferFiz,
                    Description = Resource.Install_UserContols_PaymentView_BankTransferFiz,
                    SortOrder = 0,
                    Enabled = true,
                    CompanyName = "", //SettingsBank.CompanyName,
                    TransAccount = "", //SettingsBank.RS,
                    INN = "", //SettingsBank.INN,
                    KPP = "", //SettingsBank.KPP,
                    BankName = "", //SettingsBank.BankName,
                    CorAccount = "", //SettingsBank.KS,
                    BIK = "", //SettingsBank.BIK
                };
                var id = PaymentService.AddPaymentMethod(method);
            }

            pm = PaymentService.GetPaymentMethodByName(Resource.Install_UserContols_PaymentView_BankTransferUr);
            if (pm != null)
                PaymentService.DeletePaymentMethod(pm.PaymentMethodId);

            if (model.UrBank)
            {
                var method = new Bill
                {
                    Name = Resource.Install_UserContols_PaymentView_BankTransferUr,
                    Description = Resource.Install_UserContols_PaymentView_BankTransferUr,
                    SortOrder = 0,
                    Enabled = true,
                    Accountant = "", //SettingsBank.Accountant,
                    CompanyName = "", //SettingsBank.CompanyName,
                    TransAccount = "", //SettingsBank.RS,
                    CorAccount = "", //SettingsBank.KS,
                    Address = "",
                    Telephone = SettingsMain.Phone,
                    INN = "", //SettingsBank.INN,
                    KPP = "", //SettingsBank.KPP,
                    BIK = "", //SettingsBank.BIK,
                    BankName = "", //SettingsBank.BankName,
                    Director = "", //SettingsBank.Director,
                    Manager = "", //SettingsBank.Manager
                };
                var id = PaymentService.AddPaymentMethod(method);
            }

            if (model.CreditCard)
                SaveCreditCard(model);

            if (model.EMoney)
                SaveElectronMoney(model);

            if (model.Terminals)
                SaveTerminals(model);

            pm = PaymentService.GetPaymentMethodByName(Resource.Install_UserContols_PaymentView_ByIPnone);
            if (pm != null)
                PaymentService.DeletePaymentMethod(pm.PaymentMethodId);

            if (model.Iphone)
            {
                var method = new Robokassa
                {
                    Name = Resource.Install_UserContols_PaymentView_ByIPnone,
                    Description = Resource.Install_UserContols_PaymentView_ByIPnone,
                    SortOrder = 0,
                    Enabled = true,
                    MerchantLogin = model.LoginRobokassaIPhone ?? string.Empty,
                    Password = model.PassRobokassaIPhone ?? string.Empty,
                    CurrencyLabel = "RUR",
                };
                var id = PaymentService.AddPaymentMethod(method);
            }
        }

        #region Load payments

        private void LoadCreditCard(InstallPaymentModel model)
        {
            var pm = PaymentService.GetPaymentMethodByName(Resource.Install_UserContols_PaymentView_CreditCard);
            if (pm == null) return;
            model.CreditcardType = "Robokassa";
            if (pm is Robokassa)
            {
                var temp = (Robokassa)pm;
                model.CreditcardType = "Robokassa";
                model.LoginRobokassaCreditcard = temp.MerchantLogin;
                model.PasswordRobokassaCreditcard = temp.Password;
            }

            if (pm is Assist)
            {
                var temp = (Assist)pm;
                model.CreditcardType = "Assist";
                model.LoginAssistCreditcard = temp.Login;
                model.PassAssistCreditcard = temp.Password;
                model.ShopIdAssistCreditcard = temp.MerchantID.ToString();
            }

            if (pm is Platron)
            {
                model.CreditcardType = "Platron";
                model.SellerIdPlatronCreditcard = pm.Parameters[PlatronTemplate.MerchantId];
                model.PaySystemCreditcard = pm.Parameters[PlatronTemplate.PaymentSystem];
                model.PayPassCreditcard = pm.Parameters[PlatronTemplate.SecretKey];
            }

            if (pm is ZPayment)
            {
                model.CreditcardType = "ZPayment";
                model.PayPoketCreditcard = pm.Parameters[ZPaymentTemplate.Purse];
                model.PassZpaymentCreditcard = pm.Parameters[ZPaymentTemplate.Password];
                model.SecretKeyZpaymentCreditcard = pm.Parameters[ZPaymentTemplate.SecretKey];
            }
        }

        private void LoadElectronMoney(InstallPaymentModel model)
        {
            var pm = PaymentService.GetPaymentMethodByName(Resource.Install_UserContols_PaymentView_ElectronMoney);
            if (pm == null) return;

            model.EMoneyType = "Robokassa";
            if (pm is Robokassa)
            {
                model.EMoneyType = "Robokassa";
                model.LoginRobokassaElectronMoney = pm.Parameters[RobokassaTemplate.MerchantLogin];
                model.PassRobokassaElectronMoney = pm.Parameters[RobokassaTemplate.Password];
            }

            if (pm is Assist)
            {
                model.EMoneyType = "Assist";
                model.LoginAssistElectronMoney = pm.Parameters[AssistTemplate.Login];
                model.PassAssistElectronMoney = pm.Parameters[AssistTemplate.Password];
                model.ShopIdAssistElectronMoney = pm.Parameters[AssistTemplate.MerchantID];
            }

            if (pm is Platron)
            {
                model.EMoneyType = "Platron";
                model.SellerIdPlatronElectronMoney = pm.Parameters[PlatronTemplate.MerchantId];
                model.PaySystemElectronMoney = pm.Parameters[PlatronTemplate.PaymentSystem];
                model.PayPassElectronMoney = pm.Parameters[PlatronTemplate.SecretKey];
            }

            if (pm is ZPayment)
            {
                model.EMoneyType = "ZPayment";
                model.PayPoketElectronMoney = pm.Parameters[ZPaymentTemplate.Purse];
                model.PassZpaymentElectronMoney = pm.Parameters[ZPaymentTemplate.Password];
                model.SecretKeyZpaymentElectronMoney = pm.Parameters[ZPaymentTemplate.SecretKey];
            }
        }

        private void LoadTerminals(InstallPaymentModel model)
        {
            var pm = PaymentService.GetPaymentMethodByName(Resource.Install_UserContols_PaymentView_Terminals);
            if (pm == null) return;

            model.TerminalType = "Robokassa";
            if (pm is Robokassa)
            {
                model.TerminalType = "Robokassa";
                model.LoginRobokassaTerminals = pm.Parameters[RobokassaTemplate.MerchantLogin];
                model.PassRobokassaTerminals = pm.Parameters[RobokassaTemplate.Password];
            }

            if (pm is Assist)
            {
                model.TerminalType = "Assist";
                model.LoginAssistTerminals = pm.Parameters[AssistTemplate.Login];
                model.PassAssistTerminals = pm.Parameters[AssistTemplate.Password];
                model.ShopIdAssistTerminals = pm.Parameters[AssistTemplate.MerchantID];
            }

            if (pm is Platron)
            {
                model.TerminalType = "Platron";
                model.SellerIdPlatronTerminals = pm.Parameters[PlatronTemplate.MerchantId];
                model.PaySystemTerminals = pm.Parameters[PlatronTemplate.PaymentSystem];
                model.PayPassTerminals = pm.Parameters[PlatronTemplate.SecretKey];
            }

            if (pm is ZPayment)
            {
                model.TerminalType = "ZPayment";
                model.PayPoketTerminals = pm.Parameters[ZPaymentTemplate.Purse];
                model.PassZpaymentTerminals = pm.Parameters[ZPaymentTemplate.Password];
                model.SecretKeyZpaymentTerminals = pm.Parameters[ZPaymentTemplate.SecretKey];
            }
        }

        #endregion

        #region Update payments

        private void SaveCreditCard(InstallPaymentModel model)
        {
            if (string.IsNullOrWhiteSpace(model.CreditcardType))
                return;

            var pm = PaymentService.GetPaymentMethodByName(Resource.Install_UserContols_PaymentView_CreditCard);
            if (pm != null)
                PaymentService.DeletePaymentMethod(pm.PaymentMethodId);

            switch (model.CreditcardType.ToLower())
            {
                case "robokassa":
                    var methodR = new Robokassa
                    {
                        Name = Resource.Install_UserContols_PaymentView_CreditCard,
                        Description = Resource.Install_UserContols_PaymentView_CreditCard,
                        SortOrder = 0,
                        Enabled = true,
                        MerchantLogin = model.LoginRobokassaCreditcard,
                        Password = model.PassZpaymentCreditcard,
                        CurrencyLabel = "RUR"
                    };
                    PaymentService.AddPaymentMethod(methodR);
                    break;

                case "assist":
                    var methodA = new Assist
                    {
                        Name = Resource.Install_UserContols_PaymentView_CreditCard,
                        Description = Resource.Install_UserContols_PaymentView_CreditCard,
                        SortOrder = 0,
                        Enabled = true,
                        Login = model.LoginAssistCreditcard ?? string.Empty,
                        Password = model.PassAssistCreditcard ?? string.Empty,
                        MerchantID = model.ShopIdAssistCreditcard.TryParseInt(),
                        Sandbox = false,
                        CurrencyCode = "RUB",
                        CurrencyValue = 1
                    };
                    PaymentService.AddPaymentMethod(methodA);
                    break;

                case "platron":
                    var methodP = new Platron
                    {
                        Name = Resource.Install_UserContols_PaymentView_CreditCard,
                        Description = Resource.Install_UserContols_PaymentView_CreditCard,
                        SortOrder = 0,
                        Enabled = true,
                        MerchantId = model.SellerIdPlatronCreditcard ?? string.Empty,
                        PaymentSystem = model.PaySystemCreditcard ?? string.Empty,
                        SecretKey = model.PayPassCreditcard ?? string.Empty,
                        Currency = "RUR",
                        CurrencyValue = 1,
                    };
                    PaymentService.AddPaymentMethod(methodP);
                    break;

                case "zpayment":
                    var methodZ = new ZPayment
                    {
                        Name = Resource.Install_UserContols_PaymentView_CreditCard,
                        Description = Resource.Install_UserContols_PaymentView_CreditCard,
                        SortOrder = 0,
                        Enabled = true,
                        Purse = model.PayPoketCreditcard ?? string.Empty,
                        Password = model.PassZpaymentCreditcard ?? string.Empty,
                        SecretKey = model.SecretKeyZpaymentCreditcard ?? string.Empty
                    };
                    PaymentService.AddPaymentMethod(methodZ);
                    break;
            }
        }

        private void SaveElectronMoney(InstallPaymentModel model)
        {
            if (string.IsNullOrWhiteSpace(model.EMoneyType))
                return;

            var pm = PaymentService.GetPaymentMethodByName(Resource.Install_UserContols_PaymentView_ElectronMoney);
            if (pm != null)
                PaymentService.DeletePaymentMethod(pm.PaymentMethodId);

            switch (model.EMoneyType.ToLower())
            {
                case "robokassa":
                    var methodR = new Robokassa
                    {
                        Name = Resource.Install_UserContols_PaymentView_ElectronMoney,
                        Description = Resource.Install_UserContols_PaymentView_ElectronMoney,
                        SortOrder = 0,
                        Enabled = true,
                        MerchantLogin = model.LoginRobokassaElectronMoney,
                        Password = model.PassZpaymentElectronMoney,
                        CurrencyLabel = "RUR"
                    };
                    PaymentService.AddPaymentMethod(methodR);
                    break;

                case "assist":
                    var methodA = new Assist
                    {
                        Name = Resource.Install_UserContols_PaymentView_ElectronMoney,
                        Description = Resource.Install_UserContols_PaymentView_ElectronMoney,
                        SortOrder = 0,
                        Enabled = true,
                        Login = model.LoginAssistElectronMoney ?? string.Empty,
                        Password = model.PassAssistElectronMoney ?? string.Empty,
                        MerchantID = model.ShopIdAssistElectronMoney.TryParseInt(),
                        Sandbox = false,
                        CurrencyCode = "RUB",
                        CurrencyValue = 1
                    };
                    PaymentService.AddPaymentMethod(methodA);
                    break;

                case "platron":
                    var methodP = new Platron
                    {
                        Name = Resource.Install_UserContols_PaymentView_ElectronMoney,
                        Description = Resource.Install_UserContols_PaymentView_ElectronMoney,
                        SortOrder = 0,
                        Enabled = true,
                        MerchantId = model.SellerIdPlatronElectronMoney ?? string.Empty,
                        PaymentSystem = model.PaySystemElectronMoney ?? string.Empty,
                        SecretKey = model.PayPassElectronMoney ?? string.Empty,
                        Currency = "RUR",
                        CurrencyValue = 1,
                    };
                    PaymentService.AddPaymentMethod(methodP);
                    break;

                case "zpayment":
                    var methodZ = new ZPayment
                    {
                        Name = Resource.Install_UserContols_PaymentView_ElectronMoney,
                        Description = Resource.Install_UserContols_PaymentView_ElectronMoney,
                        SortOrder = 0,
                        Enabled = true,
                        Purse = model.PayPoketElectronMoney ?? string.Empty,
                        Password = model.PassZpaymentElectronMoney ?? string.Empty,
                        SecretKey = model.SecretKeyZpaymentElectronMoney ?? string.Empty
                    };
                    PaymentService.AddPaymentMethod(methodZ);
                    break;
            }
        }

        private void SaveTerminals(InstallPaymentModel model)
        {
            if (string.IsNullOrWhiteSpace(model.TerminalType))
                return;

            var pm = PaymentService.GetPaymentMethodByName(Resource.Install_UserContols_PaymentView_Terminals);
            if (pm != null)
                PaymentService.DeletePaymentMethod(pm.PaymentMethodId);

            switch (model.TerminalType.ToLower())
            {
                case "robokassa":
                    var methodR = new Robokassa
                    {
                        Name = Resource.Install_UserContols_PaymentView_Terminals,
                        Description = Resource.Install_UserContols_PaymentView_Terminals,
                        SortOrder = 0,
                        Enabled = true,
                        MerchantLogin = model.LoginRobokassaTerminals,
                        Password = model.PassZpaymentTerminals,
                        CurrencyLabel = "RUR"
                    };
                    PaymentService.AddPaymentMethod(methodR);
                    break;

                case "assist":
                    var methodA = new Assist
                    {
                        Name = Resource.Install_UserContols_PaymentView_Terminals,
                        Description = Resource.Install_UserContols_PaymentView_Terminals,
                        SortOrder = 0,
                        Enabled = true,
                        Login = model.LoginAssistTerminals ?? string.Empty,
                        Password = model.PassAssistTerminals ?? string.Empty,
                        MerchantID = model.ShopIdAssistTerminals.TryParseInt(),
                        Sandbox = false,
                        CurrencyCode = "RUB",
                        CurrencyValue = 1
                    };
                    PaymentService.AddPaymentMethod(methodA);
                    break;

                case "platron":
                    var methodP = new Platron
                    {
                        Name = Resource.Install_UserContols_PaymentView_Terminals,
                        Description = Resource.Install_UserContols_PaymentView_Terminals,
                        SortOrder = 0,
                        Enabled = true,
                        MerchantId = model.SellerIdPlatronTerminals ?? string.Empty,
                        PaymentSystem = model.PaySystemTerminals ?? string.Empty,
                        SecretKey = model.PayPassTerminals ?? string.Empty,
                        Currency = "RUR",
                        CurrencyValue = 1,
                    };
                    PaymentService.AddPaymentMethod(methodP);
                    break;

                case "zpayment":
                    var methodZ = new ZPayment
                    {
                        Name = Resource.Install_UserContols_PaymentView_Terminals,
                        Description = Resource.Install_UserContols_PaymentView_Terminals,
                        SortOrder = 0,
                        Enabled = true,
                        Purse = model.PayPoketTerminals ?? string.Empty,
                        Password = model.PassZpaymentTerminals ?? string.Empty,
                        SecretKey = model.SecretKeyZpaymentTerminals ?? string.Empty
                    };
                    PaymentService.AddPaymentMethod(methodZ);
                    break;
            }
        }





        #endregion
    }
}
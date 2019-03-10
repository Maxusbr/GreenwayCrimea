using System.Collections.Generic;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Payment;

namespace Admin.UserControls.PaymentMethods
{
    public partial class SberBankControl : ParametersControl
    {
        public override Dictionary<string, string> Parameters
        {
            get
            {
                return _valid ||
                       ValidateFormData(new[]
                       {txtCompanyName, txtTransAccount, txtINN, txtBankName, txtCorAccount, txtBIK})
                    ? new Dictionary<string, string>
                    {
                        {SberBankTemplate.CompanyName, txtCompanyName.Text},
                        {SberBankTemplate.TransAccount, txtTransAccount.Text},
                        {SberBankTemplate.INN, txtINN.Text},
                        {SberBankTemplate.KPP, txtKPP.Text},
                        {SberBankTemplate.BankName, txtBankName.Text},
                        {SberBankTemplate.CorAccount, txtCorAccount.Text},
                        {SberBankTemplate.BIK, txtBIK.Text}
                    }
                    : null;
            }
            set
            {
                txtCompanyName.Text = value.ElementOrDefault(SberBankTemplate.CompanyName);
                txtTransAccount.Text = value.ElementOrDefault(SberBankTemplate.TransAccount);
                txtINN.Text = value.ElementOrDefault(SberBankTemplate.INN);
                txtKPP.Text = value.ElementOrDefault(SberBankTemplate.KPP);
                txtBankName.Text = value.ElementOrDefault(SberBankTemplate.BankName);
                txtCorAccount.Text = value.ElementOrDefault(SberBankTemplate.CorAccount);
                txtBIK.Text = value.ElementOrDefault(SberBankTemplate.BIK);
            }
        }
    }
}
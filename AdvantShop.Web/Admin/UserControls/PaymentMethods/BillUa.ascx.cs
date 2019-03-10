using System.Collections.Generic;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Payment;

namespace Admin.UserControls.PaymentMethods
{
    public partial class BillUaControl : ParametersControl
    {
        public override Dictionary<string, string> Parameters
        {
            get
            {
                return _valid ||
                       ValidateFormData(
                           new[]
                           {
                               txtCompanyName, txtCompanyCode, txtBankName, txtBankCode, txtCredit,
                               txtCompanyEssentials
                           }, null, null)
                    ? new Dictionary<string, string>
                    {
                        {BillUaTemplate.CompanyName, txtCompanyName.Text},
                        {BillUaTemplate.CompanyCode, txtCompanyCode.Text},
                        {BillUaTemplate.BankName, txtBankName.Text},
                        {BillUaTemplate.BankCode, txtBankCode.Text},
                        {BillUaTemplate.Credit, txtCredit.Text},
                        {BillUaTemplate.CompanyEssentials, txtCompanyEssentials.Text}
                        
                    }
                    : null;
            }
            set
            {
                txtCompanyName.Text = value.ElementOrDefault(BillUaTemplate.CompanyName);
                txtCompanyCode.Text = value.ElementOrDefault(BillUaTemplate.CompanyCode);
                txtBankName.Text = value.ElementOrDefault(BillUaTemplate.BankName);
                txtBankCode.Text = value.ElementOrDefault(BillUaTemplate.BankCode);
                txtCredit.Text = value.ElementOrDefault(BillUaTemplate.Credit);
                txtCompanyEssentials.Text = value.ElementOrDefault(BillUaTemplate.CompanyEssentials);
            }
        }

    }
}
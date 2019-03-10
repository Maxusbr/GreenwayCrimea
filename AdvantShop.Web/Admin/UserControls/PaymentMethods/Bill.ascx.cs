using System.Collections.Generic;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Payment;

namespace Admin.UserControls.PaymentMethods
{
    public partial class BillControl : ParametersControl
    {
        protected string StampImg { get; set; }


        public override Dictionary<string, string> Parameters
        {
            get
            {
                return _valid ||
                       ValidateFormData(
                           new[]
                           {
                               txtCompanyName, txtTransAccount, txtCorAccount, txtAddress,
                               txtTelephone, txtINN, txtBIK, txtBankName, txtDirector, txtAccountant,
                               txtPosAccountant, txtPosDirector, txtPosManager
                           })
                    ? new Dictionary<string, string>
                    {
                        {BillTemplate.Accountant, txtAccountant.Text},
                        {BillTemplate.CompanyName, txtCompanyName.Text},
                        {BillTemplate.TransAccount, txtTransAccount.Text},
                        {BillTemplate.CorAccount, txtCorAccount.Text},
                        {BillTemplate.Address, txtAddress.Text},
                        {BillTemplate.Telephone, txtTelephone.Text},
                        {BillTemplate.INN, txtINN.Text},
                        {BillTemplate.KPP, txtKPP.Text},
                        {BillTemplate.BIK, txtBIK.Text},
                        {BillTemplate.BankName, txtBankName.Text},
                        {BillTemplate.Director, txtDirector.Text},
                        {BillTemplate.Manager, txtManager.Text},
                        {BillTemplate.StampImageName, hfStampImg.Value},
                        {BillTemplate.PosAccountant, txtPosAccountant.Text},
                        {BillTemplate.PosDirector, txtPosDirector.Text},
                        {BillTemplate.PosManager, txtPosManager.Text}
                    }
                    : null;
            }
            set
            {
                txtAccountant.Text = value.ElementOrDefault(BillTemplate.Accountant);
                txtCompanyName.Text = value.ElementOrDefault(BillTemplate.CompanyName);
                txtTransAccount.Text = value.ElementOrDefault(BillTemplate.TransAccount);
                txtCorAccount.Text = value.ElementOrDefault(BillTemplate.CorAccount);
                txtAddress.Text = value.ElementOrDefault(BillTemplate.Address);
                txtTelephone.Text = value.ElementOrDefault(BillTemplate.Telephone);
                txtINN.Text = value.ElementOrDefault(BillTemplate.INN);
                txtKPP.Text = value.ElementOrDefault(BillTemplate.KPP); 
                txtBIK.Text = value.ElementOrDefault(BillTemplate.BIK);
                txtBankName.Text = value.ElementOrDefault(BillTemplate.BankName);
                txtDirector.Text = value.ElementOrDefault(BillTemplate.Director);
                txtManager.Text = value.ElementOrDefault(BillTemplate.Manager);
                StampImg = value.ElementOrDefault(BillTemplate.StampImageName);
                txtPosDirector.Text = value.ElementOrDefault(BillTemplate.PosDirector);
                txtPosManager.Text = value.ElementOrDefault(BillTemplate.PosManager);
                txtPosAccountant.Text = value.ElementOrDefault(BillTemplate.PosAccountant);

                hfStampImg.Value = StampImg;
                //txtCurrencyValue.Text = value.ElementOrDefault(BillTemplate.CurrencyValue);
            }
        }
    }
}
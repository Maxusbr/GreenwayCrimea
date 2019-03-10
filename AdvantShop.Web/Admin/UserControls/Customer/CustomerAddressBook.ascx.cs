using System;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Customers;
using AdvantShop.Repository;
using Resources;

namespace Admin.UserControls
{
    public partial class CustomerAddressBook : System.Web.UI.UserControl
    {
        public Customer Customer { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            DisplayAddressBook();
        }

        protected void btnDeleteContact_Click(object sender, EventArgs e)
        {
            CustomerService.DeleteContact(CustomerContacts.SelectedValue.TryParseGuid());

            foreach (CustomerContact customerRow in Customer.Contacts)
            {
                if (customerRow.ContactId.ToString() == CustomerContacts.SelectedValue)
                {
                    Customer.Contacts.Remove(customerRow);
                    CustomerContacts.Items.Remove(CustomerContacts.SelectedItem);
                    break;
                }
            }
            DisplayAddressBook();
        }

        protected void btnAddChangeContact_Click(object sender, EventArgs e)
        {
            if (!ValidateFormData())
                return;

            var contact = CustomerContacts.SelectedValue.IsNullOrEmpty()
                                ? new CustomerContact()
                                : CustomerService.GetCustomerContact(CustomerContacts.SelectedValue);

            contact.Name = txtContactName.Text;
            contact.Country = cboCountry.SelectedItem.Text;
            contact.CountryId = Int32.Parse(cboCountry.SelectedValue);
            contact.City = HttpUtility.HtmlEncode(txtContactCity.Text);
            contact.Region = HttpUtility.HtmlEncode(txtContactZone.Text);
            contact.Street = HttpUtility.HtmlEncode(txtContactAddress.Text);
            contact.Zip = HttpUtility.HtmlEncode(txtContactZip.Text);


            switch (ViewState["AddOrEdit"].ToString())
            {
                case "Add":
                    contact.CustomerGuid = Customer.Id;
                    CustomerService.AddContact(contact, Customer.Id);
                    break;
                case "Edit":
                    contact.ContactId = new Guid(CustomerContacts.SelectedValue);
                    CustomerService.UpdateContact(contact);
                    break;
            }

            DisplayAddressBook();
            mvAdressBook.SetActiveView(vAdressBook);
        }

        private void ShowMessage(Label lbl, string message)
        {
            if (lbl == null) return;

            lbl.Text = message;
            lbl.Visible = true;
        }

        private bool ValidateFormData()
        {
            bool valid = true;
            foreach (var lbl in new[] { msgContactName, msgContactCity })
            {
                lbl.Visible = false;
            }

            foreach (var validatedField in new[] { txtContactName, txtContactCity }
                .Where(validatedField => string.IsNullOrEmpty(validatedField.Text)))
            {
                ShowMessage((Label)FindControl("msg" + validatedField.ID.Substring(3)), Resource.Admin_Messages_EnterValue);
                valid = false;
            }
            return valid;
        }

        protected void btnAddNewContact_Click(object sender, EventArgs e)
        {

            ViewState["AddOrEdit"] = "Add";
            btnAddChangeContact.Text = Resource.Admin_ViewCustomer_Add;

            txtContactName.Text = "";
            txtContactCity.Text = "";
            txtContactZone.Text = "";
            txtContactAddress.Text = "";
            txtContactZip.Text = "";
            cboCountry.DataSource = CountryService.GetAllCountryIdAndName();
            cboCountry.DataBind();
            mvAdressBook.SetActiveView(vAddEditContact);
        }

        protected void btnEditContact_Click(object sender, EventArgs e)
        {
            cboCountry.DataSource = CountryService.GetAllCountryIdAndName();
            cboCountry.DataBind();

            foreach (CustomerContact customerRow in Customer.Contacts)
            {
                if (customerRow.ContactId.ToString() == CustomerContacts.SelectedValue)
                {
                    txtContactName.Text = customerRow.Name;

                    if (cboCountry.Items.FindByValue(customerRow.CountryId.ToString()) != null)
                        cboCountry.SelectedValue = customerRow.CountryId.ToString();
                    else
                    {
                        if (cboCountry.Items.FindByValue(SettingsMain.SellerCountryId.ToString()) != null)
                            cboCountry.SelectedValue = SettingsMain.SellerCountryId.ToString();
                    }

                    txtContactCity.Text = HttpUtility.HtmlDecode(customerRow.City);
                    txtContactZone.Text = HttpUtility.HtmlDecode(customerRow.Region);
                    txtContactAddress.Text = HttpUtility.HtmlDecode(customerRow.Street);
                    txtContactZip.Text = HttpUtility.HtmlDecode(customerRow.Zip);
                    break;
                }
            }

            ViewState["AddOrEdit"] = "Edit";
            btnAddChangeContact.Text = Resource.Admin_ViewCustomer_Edit;

            mvAdressBook.SetActiveView(vAddEditContact);
        }

        private void DisplayAddressBook()
        {
            CustomerContacts.Items.Clear();

            foreach (CustomerContact customerRow in Customer.Contacts)
            {
                var li = new ListItem();
                var liText = new StringBuilder();
                liText.AppendFormat("<b>{0}:</b>&nbsp;{1}<br />", Resource.Admin_ViewCustomer_ContactPerson, customerRow.Name);
                liText.AppendFormat("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b>{0}</b>&nbsp;{1}<br />",
                                    Resource.Admin_ViewCustomer_ContactCountry, customerRow.Country);
                liText.AppendFormat("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b>{0}</b>&nbsp;{1}<br />",
                                    Resource.Admin_ViewCustomer_ContactCity, customerRow.City);

                if (customerRow.Region.Trim() != "")
                {
                    liText.AppendFormat("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b>{0}</b>&nbsp;{1}<br />",
                                        Resource.Admin_ViewCustomer_ContactZone, customerRow.Region);
                }
                if (customerRow.Zip.Trim() != "")
                {
                    liText.AppendFormat("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b>{0}</b>&nbsp;{1}<br />",
                                        Resource.Admin_ViewCustomer_ContactZip, customerRow.Zip);
                }
                liText.AppendFormat("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b>{0}</b>&nbsp;{1}<br />",
                                    Resource.Admin_ViewCustomer_ContactAddress, customerRow.Street);

                li.Text = liText.ToString();
                li.Value = customerRow.ContactId.ToString();
                CustomerContacts.Items.Add(li);
            }

            if (CustomerContacts.Items.Count > 0)
            {
                CustomerContacts.SelectedIndex = 0;

                btnDeleteContact.Enabled = true;
                btnEditContact.Enabled = true;
            }
            else
            {
                btnDeleteContact.Enabled = false;
                btnEditContact.Enabled = false;
            }
        }

    }
}
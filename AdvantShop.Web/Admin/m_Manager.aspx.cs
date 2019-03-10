using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.WebControls;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Customers;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.Saas;
using Resources;

namespace Admin
{
    public partial class m_Manager : AdvantShopAdminPage
    {

        private int? _managerId;
        protected int ManagerId
        {
            get
            {
                return _managerId ?? (_managerId = Request["id"].TryParseInt()).Value;
            }
        }

        private void MsgErr(bool clean)
        {
            if (clean)
            {
                lblError.Visible = false;
                lblError.Text = string.Empty;
            }
            else
            {
                lblError.Visible = false;
            }
        }

        private void MsgErr(string messageText)
        {
            lblError.Visible = true;
            lblError.Text = @"<br/>" + messageText;
        }

        private void SetRoleActions(Guid customerId)
        {
            foreach (RoleAction roleActionKey in new List<RoleAction>
            {
                RoleAction.Crm,
                RoleAction.Orders,                
                RoleAction.Customers
            })
            {
                RoleActionService.UpdateOrInsertCustomerRoleAction(customerId, roleActionKey.ToString(), true);
            }
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            if (!Valid()) return;

            if (ManagerId != 0)
            {
                SaveManager();
            }
            else
            {
                CreateManager();
            }

            // Close window
            if (lblError.Visible == false)
            {
                CommonHelper.RegCloseScript(this, string.Empty);
            }
        }

        protected void btnDeleteImage_Click(object sender, EventArgs e)
        {
            if (ManagerId != 0)
            {
                ManagerService.DeleteManagerPhoto(ManagerId);

                pnlImage.Visible = false;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_m_Manager_Header));

            lblImageInfo.Text = string.Format("* {0} {1}x{2}px", Resource.Admin_m_Manager_ResultImageSize, SettingsPictureSize.ManagerFotoWidth, SettingsPictureSize.ManagerFotoHeight);

            PopupGridCustomers.CustomersRole = new List<Role> { Role.Administrator, Role.Moderator, Role.User };
            PopupGridCustomers.ExceptIds = ManagerService.GetManagerIdsList();

            pnlChooseCustomer.Visible = ManagerId == 0;

            if (!IsPostBack)
            {
                ddlDepartment.Items.Clear();

                ddlDepartment.Items.Add(new ListItem(Resource.Admin_Catalog_No, "null"));
                foreach (var department in DepartmentService.GetDepartmentsList())
                    ddlDepartment.Items.Add(new ListItem(department.Name, department.DepartmentId.ToString()));

                if (ManagerId != 0)
                {
                    btnOK.Text = Resource.Admin_m_Manager_Save;
                    LoadManagerById(ManagerId);

                }
                else
                {
                    btnOK.Text = Resource.Admin_m_Manager_Add;
                    pnlImage.Visible = false;

                    imgManagerPhoto.ImageUrl = string.Empty;
                    txtPosition.Text = string.Empty;
                }
            }
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            if (PopupGridCustomers.SelectedCustomers != null && PopupGridCustomers.SelectedCustomers.Count > 0)
            {
                var customer = CustomerService.GetCustomer(PopupGridCustomers.SelectedCustomers[0]);
                hfCustomerId.Value = customer.Id.ToString();
                lblCustomer.Text = string.Format("{0} {1}", customer.FirstName, customer.LastName);

                PopupGridCustomers.CleanSelection();
            }
        }

        private bool Valid()
        {
            MsgErr(true);

            if (rbExistCustomer.Checked && hfCustomerId.Value.IsNullOrEmpty())
            {
                MsgErr(Resource.Admin_m_Manager_SetCustomer);
                return false;
            }

            if (rbNewCustomer.Checked)
            {
                Validate("NewCustomer");
                if (!IsValid)
                    return false;

                bool valid = true;
                valid &= txtEmail.Text.IsNotEmpty();
                valid &= txtPassword.Text.IsNotEmpty();
                valid &= txtPasswordConfirm.Text.IsNotEmpty();
                valid &= txtLastName.Text.IsNotEmpty();
                valid &= txtFirstName.Text.IsNotEmpty();
                if (!valid)
                {
                    MsgErr(Resource.Client_OrderConfirmation_EnterEmptyField);
                    return false;
                }

                var email = txtEmail.Text.Trim();
                if (!ValidationHelper.IsValidEmail(email))
                {
                    MsgErr(Resource.Admin_ViewCustomer_NotValidEmail);
                    return false;
                }
                if (CustomerService.ExistsEmail(email))
                {
                    MsgErr(Resource.Admin_CreateCustomer_CustomerErrorEmailExist);
                    return false;
                }
                if (txtPassword.Text.Length < 6)
                {
                    MsgErr(Resource.Admin_CreateCustomer_PasswordLenght);
                    return false;
                }
                if (txtPasswordConfirm.Text != txtPassword.Text)
                {
                    MsgErr(Resource.Admin_CreateCustomer_PasswordNotMatch);
                    return false;
                }
            }
            
            return true;
        }

        private void SaveManager()
        {
            MsgErr(true); // Clean

            Guid customerId;
            if (!Guid.TryParse(hfCustomerId.Value, out customerId))
            {
                return;
            }

            var customer = CustomerService.GetCustomer(customerId);
            if (customer == null)
            {
                return;
            }

            var manager = new Manager
            {
                ManagerId = ManagerId,
                CustomerId = customerId,
                Position = txtPosition.Text,
                DepartmentId = ddlDepartment.SelectedValue.TryParseInt(true),
                Active = ckbActive.Checked
            };

            ManagerService.AddOrUpdateManager(manager);

            if (customer.CustomerRole == Role.User)
            {
                customer.CustomerRole = Role.Moderator;
                CustomerService.UpdateCustomer(customer);
            }

            SetRoleActions(customer.Id);
            
            if (fuManagerPhoto.HasFile)
            {
                if (!FileHelpers.CheckFileExtension(fuManagerPhoto.FileName, EAdvantShopFileTypes.Image))
                {
                    MsgErr(Resource.Admin_ErrorMessage_WrongImageExtension);
                    return;
                }
                PhotoService.DeletePhotos(ManagerId, PhotoType.Manager);

                var tempName = PhotoService.AddPhoto(new Photo(0, ManagerId, PhotoType.Manager) { OriginName = fuManagerPhoto.FileName });
                if (!string.IsNullOrWhiteSpace(tempName))
                {
                    FileHelpers.SaveFile(FoldersHelper.GetPathAbsolut(FolderType.ManagerPhoto, tempName), fuManagerPhoto.FileContent);
                    //if you need resize
                    //using (var image = System.Drawing.Image.FromStream(fuManagerPhoto.FileContent))
                    //    FileHelpers.SaveResizePhotoFile(FoldersHelper.GetPathAbsolut(FolderType.ManagerPhoto, tempName), SettingsPictureSize.ManagerFotoWidth, SettingsPictureSize.ManagerFotoHeight, image);
                }
            }
        }

        private void CreateManager()
        {
            MsgErr(true); // Clean

            Guid customerId = Guid.Empty;
            Customer customer = null;
            if (rbExistCustomer.Checked && 
                (!Guid.TryParse(hfCustomerId.Value, out customerId) || (customer = CustomerService.GetCustomer(customerId)) == null))
            {
                return;
            }
            if (fuManagerPhoto.HasFile && !FileHelpers.CheckFileExtension(fuManagerPhoto.FileName, EAdvantShopFileTypes.Image))
            {
                MsgErr(Resource.Admin_ErrorMessage_WrongImageExtension);
                return;
            }
            if (rbNewCustomer.Checked)
            {
                customer = new Customer
                {
                    Password = txtPassword.Text,
                    FirstName = txtFirstName.Text,
                    LastName = txtLastName.Text,
                    Phone = txtPhone.Text,
                    StandardPhone = StringHelper.ConvertToStandardPhone(txtPhone.Text),
                    EMail = txtEmail.Text,
                    CustomerRole = Role.Moderator,
                    CustomerGroupId = CustomerGroupService.DefaultCustomerGroup
                };
                customer.Id = CustomerService.InsertNewCustomer(customer);
            }

            if (customer == null || customer.Id.Equals(Guid.Empty))
            {
                return;
            }

            var manager = new Manager
            {
                CustomerId = customer.Id,
                Position = txtPosition.Text,
                DepartmentId = ddlDepartment.SelectedValue.TryParseInt(true),
                Active = ckbActive.Checked
            };

            ManagerService.AddOrUpdateManager(manager);

            if (customer.CustomerRole == Role.User)
            {
                customer.CustomerRole = Role.Moderator;
                CustomerService.UpdateCustomer(customer);
            }

            SetRoleActions(customer.Id);

            if (fuManagerPhoto.HasFile)
            {
                var tempName = PhotoService.AddPhoto(new Photo(0, manager.ManagerId, PhotoType.Manager) { OriginName = fuManagerPhoto.FileName });
                if (!string.IsNullOrWhiteSpace(tempName))
                {
                    using (var image = System.Drawing.Image.FromStream(fuManagerPhoto.FileContent))
                        FileHelpers.SaveResizePhotoFile(FoldersHelper.GetPathAbsolut(FolderType.ManagerPhoto, tempName), SettingsPictureSize.ManagerFotoWidth, SettingsPictureSize.ManagerFotoHeight, image);
                }
            }

            if (lblError.Visible == false)
            {
                txtPosition.Text = string.Empty;
                ddlDepartment.SelectedIndex = 0;
            }
        }

        private void LoadManagerById(int managerId)
        {
            var manager = ManagerService.GetManager(managerId);
            if (manager == null)
            {
                MsgErr("Manager with this ID not exist");
                return;
            }
            
            txtPosition.Text = manager.Position;
            ckbActive.Checked = manager.Active;

            if (manager.DepartmentId.HasValue)
            {
                ddlDepartment.SelectedValue = manager.DepartmentId.Value.ToString(CultureInfo.InvariantCulture);
            }

            var customer = CustomerService.GetCustomer(manager.CustomerId);
            hfCustomerId.Value = customer.Id.ToString();
            lnkCustomer.Text = string.Format("{0} {1}", customer.FirstName, customer.LastName);
            
            if (manager.Photo.PhotoName.IsNotEmpty())
            {
                pnlImage.Visible = true;
                lblImage.Text = manager.Photo.PhotoName;
                imgManagerPhoto.ImageUrl = FoldersHelper.GetPath(FolderType.ManagerPhoto, manager.Photo.PhotoName, true);
                imgManagerPhoto.ToolTip = manager.Photo.PhotoName;
            }
            else
            {
                lblImage.Text = @"No picture";
                pnlImage.Visible = false;
            }
        }

        protected void lnkCustomer_Click(object sender, EventArgs e)
        {
            CommonHelper.RegCloseScript(this, "'EditCustomer.aspx?CustomerId=" + hfCustomerId.Value +"';");
        }
    }
}
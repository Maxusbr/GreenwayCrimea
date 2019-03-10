using System;
using System.Linq;
using System.Collections.Generic;
using System.Web;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Services.Bonuses;
using AdvantShop.Core.Services.Bonuses.Model;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Customers;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;
using AdvantShop.Mails;
using AdvantShop.Security;
using AdvantShop.ViewModel.User;

namespace AdvantShop.Handlers.User
{
	public class RegistrationHandler
	{
		/// <summary>
		/// Is valid fields
		/// </summary>
		/// <param name="model">Registration model</param>
		/// <returns>List of errors</returns>
		public List<string> IsValid(RegistrationViewModel model)
		{
			var errors = new List<string>();

			var isValid = ValidationHelper.IsValidEmail(model.Email);

			if (!string.IsNullOrWhiteSpace(model.Email) && CustomerService.CheckCustomerExist(model.Email))
			{
				errors.Add(string.Format(LocalizationService.GetResource("User.Registration.ErrorCustomerExist"), "forgotpassword"));
			}

			isValid &= !String.IsNullOrWhiteSpace(model.PasswordConfirm) && !String.IsNullOrWhiteSpace(model.Password) &&
					   model.Password == model.PasswordConfirm;

			if (!isValid)
			{
				errors.Add(LocalizationService.GetResource("User.Registration.ErrorPasswordNotMatch"));
			}

			isValid &= model.Password.Length >= 6;
			if (!isValid)
			{
				errors.Add(LocalizationService.GetResource("User.Registration.PasswordLenght"));
			}

			if (SettingsCheckout.IsShowPhone && SettingsCheckout.IsRequiredPhone && String.IsNullOrWhiteSpace(model.Phone))
				isValid = false;

			if (SettingsCheckout.IsShowLastName && SettingsCheckout.IsRequiredLastName && String.IsNullOrWhiteSpace(model.LastName))
				isValid = false;

			if (SettingsCheckout.IsShowPatronymic && SettingsCheckout.IsRequiredPatronymic && String.IsNullOrWhiteSpace(model.Patronymic))
				isValid = false;

			isValid &= !String.IsNullOrWhiteSpace(model.FirstName);

			if (SettingsCheckout.IsShowUserAgreementText && !model.Agree)
			{
				errors.Add(LocalizationService.GetResource("User.Registration.ErrorAgreement"));
			}

			if (BonusSystem.IsActive && model.WantBonusCard)
			{
				var bonusCard = BonusSystemService.GetCard(CustomerContext.CurrentCustomer.Id);
				if (bonusCard != null)
				{
					errors.Add("Бонусная карта уже используется");
				}
			}

			if (!isValid)
				errors.Add(LocalizationService.GetResource("User.Registration.Error"));

			return errors;
		}

		public void Register(RegistrationViewModel model)
		{
			var groupId = model.CustomerGroup == CustomerGroupType.Partner ? 
				CustomerGroupService.GetCustomerGroup("Партнер")?.CustomerGroupId : 
				CustomerGroupService.DefaultCustomerGroup;

			var customer = new Customer(groupId ?? CustomerGroupService.DefaultCustomerGroup)
			{
				Id = CustomerContext.CustomerId,
				Password = HttpUtility.HtmlEncode(model.Password),
				FirstName = HttpUtility.HtmlEncode(model.FirstName),
				LastName =
					SettingsCheckout.IsShowLastName ? HttpUtility.HtmlEncode(model.LastName) : string.Empty,
				Patronymic =
					SettingsCheckout.IsShowPatronymic
						? HttpUtility.HtmlEncode(model.Patronymic)
						: string.Empty,
				Phone = SettingsCheckout.IsShowPhone ? HttpUtility.HtmlEncode(model.Phone) : string.Empty,
				StandardPhone = StringHelper.ConvertToStandardPhone(SettingsCheckout.IsShowPhone ? HttpUtility.HtmlEncode(model.Phone) : string.Empty),
				SubscribedForNews = model.NewsSubscription,
				EMail = HttpUtility.HtmlEncode(model.Email),
				CustomerRole = Role.User
			};

			CustomerService.InsertNewCustomer(customer);
			AuthorizeService.SignIn(customer.EMail, customer.Password, false, true);

			if (model.CustomerFields != null)
			{
				foreach (var customerField in model.CustomerFields)
				{
					CustomerFieldService.AddUpdateMap(customer.Id, customerField.Id, customerField.Value ?? "");
				}
			}

			if (BonusSystem.IsActive && model.WantBonusCard)
			{
				try
				{
					customer.BonusCardNumber = BonusSystemService.AddCard(new Card { CardId = customer.Id });
					CustomerService.UpdateCustomer(customer);
				}
				catch (Exception ex)
				{
					Debug.Log.Error(ex);
				}
			}

			var regMailTemplate = new RegistrationMailTemplate(SettingsMain.SiteUrl, customer.FirstName,
										customer.LastName, AdvantShop.Localization.Culture.ConvertDate(DateTime.Now), customer.Password,
										customer.SubscribedForNews ? LocalizationService.GetResource("User.Registration.Yes") : LocalizationService.GetResource("User.Registration.No"),
										customer.EMail, customer.Phone, SettingsCheckout.IsShowPatronymic ? customer.Patronymic.ToString() : string.Empty);
			regMailTemplate.BuildMail();

			if (!CustomerContext.CurrentCustomer.IsVirtual)
			{
				CreateLead(model);
				SendMail.SendMailNow(customer.Id, customer.EMail, regMailTemplate.Subject, regMailTemplate.Body, true);
				SendMail.SendMailNow(Guid.Empty, SettingsMail.EmailForRegReport, regMailTemplate.Subject, regMailTemplate.Body, true, customer.EMail);
			}

			ModulesExecuter.Registration(customer);
		}

		private void CreateLead(RegistrationViewModel model)
		{
			var message = $"Регистрация нового пользователя в группу: \"{model.CustomerGroup.Localize()}\"";

			var lead = new Core.Services.Crm.Lead()
			{
				Email = model.Email,
				FirstName = model.FirstName,
				LastName = model.LastName,
				Phone = model.Phone,

				Customer = new Customer(CustomerGroupService.DefaultCustomerGroup)
				{
					FirstName = model.FirstName,
					LastName = model.LastName,
					EMail = model.Email,
					Phone = model.Phone,
					StandardPhone = model.Phone != null ? StringHelper.ConvertToStandardPhone(model.Phone) : default(long?),
					CustomerRole = Role.User
				},

				Comment = message,
				OrderSourceId = 0
			};
			var dealStatus = Core.Services.Crm.DealStatuses.DealStatusService.GetList().FirstOrDefault(o => o.Name.Equals("Запрос на регистрацию"));
			if (dealStatus != null)
				lead.DealStatusId = dealStatus.Id;
			Core.Services.Crm.LeadService.AddLead(lead);
		}
	}
}
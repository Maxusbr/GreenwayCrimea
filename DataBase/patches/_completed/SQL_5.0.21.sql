CREATE TABLE [CMS].[AdminComment](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ParentId] [int] NULL,
	[ObjId] [int] NOT NULL,
	[Type] [nvarchar](50) NOT NULL,
	[CustomerId] [uniqueidentifier] NULL,
	[Name] [nvarchar](150) NOT NULL,
	[Email] [nvarchar](150) NOT NULL,
	[Text] [nvarchar](max) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateModified] [datetime] NOT NULL,
 CONSTRAINT [PK_AdminComment] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO--
ALTER TABLE [CMS].[AdminComment] ADD  CONSTRAINT [DF_AdminComment_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO--
ALTER TABLE [CMS].[AdminComment] ADD  CONSTRAINT [DF_AdminComment_DateModified]  DEFAULT (getdate()) FOR [DateModified]
GO--

UPDATE Customers.Task SET [Status] = -1 WHERE InProgress = 1
GO--
UPDATE Customers.Task SET [Status] = 2 WHERE [Status] = 1
GO--
UPDATE Customers.Task SET [Status] = 1 WHERE [Status] = -1
GO--

ALTER TABLE Customers.Task DROP CONSTRAINT [DF_Task_InProgress]
GO--
ALTER TABLE Customers.Task DROP COLUMN InProgress
GO--

ALTER TABLE Customers.Task ADD Accepted bit NULL
GO--
UPDATE Customers.Task SET Accepted = 0
GO--
ALTER TABLE Customers.Task ALTER COLUMN Accepted bit NOT NULL
GO--
ALTER TABLE Customers.Task ADD  CONSTRAINT [DF_Task_Accepted] DEFAULT (0) FOR Accepted
GO--

IF (EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'Module' 
                 AND  TABLE_NAME = 'BlogProduct'))
begin
ALTER TABLE Module.BlogProduct DROP CONSTRAINT FK_BlogProduct_BlogItem
ALTER TABLE Module.BlogProduct DROP CONSTRAINT FK_BlogProduct_Product

ALTER TABLE [Module].[BlogProduct]  WITH CHECK ADD CONSTRAINT [FK_BlogProduct_BlogItem] FOREIGN KEY([BlogId])
REFERENCES [Module].[BlogItem] ([ItemId]) ON UPDATE NO ACTION  ON DELETE  CASCADE

ALTER TABLE [Module].[BlogProduct]  WITH CHECK ADD CONSTRAINT [FK_BlogProduct_Product] FOREIGN KEY([ProductId])
REFERENCES [Catalog].[Product] ([ProductId]) ON UPDATE NO ACTION ON DELETE  CASCADE

end

GO--


ALTER TABLE CMS.AdminComment ADD Deleted bit NULL
GO--
UPDATE CMS.AdminComment SET Deleted = 0
GO--
ALTER TABLE CMS.AdminComment ALTER COLUMN Deleted bit NOT NULL
GO--
ALTER TABLE CMS.AdminComment ADD  CONSTRAINT [DF_AdminComment_Deleted] DEFAULT (0) FOR Deleted
GO--


ALTER PROCEDURE [Catalog].[sp_GetPropertyValueByID] @PropertyValueId INT
AS
  BEGIN
      SELECT [PropertyValueId],
             [Property].[PropertyId],
             [value],
             [PropertyValue].[sortorder],
             [Property].useinfilter,
             [Property].useindetails,
             [Property].useinbrief,
             [Property].Name       AS PropertyName,
             [Property].SortOrder  AS PropertySortOrder,
             [Property].Expanded,
             [Property].[Type],
			 [Property].[Description],
             GroupId,
             GroupName,
             GroupSortOrder
      FROM   [Catalog].[PropertyValue]
      INNER JOIN [Catalog].[Property] ON [Property].[Propertyid] = [PropertyValue].[PropertyID]
      LEFT JOIN [Catalog].PropertyGroup ON PropertyGroup.PropertyGroupId = [Property].GroupId
      WHERE  [PropertyValue].[PropertyValueId] = @PropertyValueId
  END 
 GO--

ALTER TABLE [Customers].[Task] ALTER COLUMN DueDate datetime null
GO--

alter table Customers.Task alter column AssignedManagerId int null
alter table Customers.Task alter column AppointedManagerId int null

alter table Customers.ManagerTask alter column AssignedManagerId int null
alter table Customers.ManagerTask alter column AppointedManagerId int null

GO--

delete [Customers].[RoleAction] where [Key] = 'DisplayMainPageNew' or [Key] = 'DisplayMainPageDiscount'

GO--

Update [Customers].[RoleAction] Set [Key] = 'EditProductListOnMain', [Name] = 'Редактирование списков товаров на главной'  where [Key] = 'DisplayMainPageBestsellers'

GO--
DECLARE @CustomerID [uniqueidentifier];
DECLARE @RoleActionKey [nvarchar](50);
DECLARE @Enabled [bit];

DECLARE  new_cursor CURSOR FOR SELECT * From [Customers].[CustomerRoleAction]

Open new_cursor

FETCH NEXT FROM new_cursor INTO @CustomerID, @RoleActionKey, @Enabled

WHILE @@FETCH_STATUS = 0
BEGIN

        IF @RoleActionKey = 'DisplayMainPageNew'
        BEGIN
			IF Exists(Select * From [Customers].[CustomerRoleAction] where RoleActionKey = 'EditProductListOnMain')
				BEGIN
					Delete [Customers].[CustomerRoleAction] where RoleActionKey = @RoleActionKey and CustomerID = @CustomerID
				END
			Else
				begin
					Insert into [Customers].[CustomerRoleAction] (CustomerID,RoleActionKey,[Enabled]) Values (@CustomerID,'EditProductListOnMain',@Enabled)
					Delete [Customers].[CustomerRoleAction] where RoleActionKey = @RoleActionKey and CustomerID = @CustomerID
				end
        END

		IF @RoleActionKey = 'DisplayMainPageDiscount'
        BEGIN
			IF Exists(Select * From [Customers].[CustomerRoleAction] where RoleActionKey = 'EditProductListOnMain')
				BEGIN
					Delete [Customers].[CustomerRoleAction] where RoleActionKey = @RoleActionKey and CustomerID = @CustomerID
				END
			Else
				begin
					Insert into [Customers].[CustomerRoleAction] (CustomerID,RoleActionKey,[Enabled]) Values (@CustomerID,'EditProductListOnMain',@Enabled)
					Delete [Customers].[CustomerRoleAction] where RoleActionKey = @RoleActionKey and CustomerID = @CustomerID
				end
        END
		IF @RoleActionKey = 'DisplayMainPageBestsellers'
        BEGIN
			IF Exists(Select * From [Customers].[CustomerRoleAction] where RoleActionKey = 'EditProductListOnMain')
				BEGIN
					Delete [Customers].[CustomerRoleAction] where RoleActionKey = @RoleActionKey and CustomerID = @CustomerID
				END
			Else
				begin
					Insert into [Customers].[CustomerRoleAction] (CustomerID,RoleActionKey,[Enabled]) Values (@CustomerID,'EditProductListOnMain',@Enabled)
					Delete [Customers].[CustomerRoleAction] where RoleActionKey = @RoleActionKey and CustomerID = @CustomerID
				end
        END

FETCH NEXT FROM new_cursor INTO @CustomerID, @RoleActionKey, @Enabled
END

close new_cursor

GO--

if ((Select Count(SettingId) From [Settings].[Settings] Where Name = 'BrandTitle') = 0)
begin
	Insert Into [Settings].[Settings] ([Name],[Value]) Values('BrandTitle', '#STORE_NAME# - #BRAND_NAME#')
end

if ((Select Count(SettingId) From [Settings].[Settings] Where Name = 'BrandMetaKeywords') = 0)
begin
	Insert Into [Settings].[Settings] ([Name],[Value]) Values('BrandMetaKeywords', '#STORE_NAME# - #BRAND_NAME#')
end

if ((Select Count(SettingId) From [Settings].[Settings] Where Name = 'BrandMetaDescription') = 0)
begin
	Insert Into [Settings].[Settings] ([Name],[Value]) Values('BrandMetaDescription', '#STORE_NAME# - #BRAND_NAME#')
end

if ((Select Count(SettingId) From [Settings].[Settings] Where Name = 'BrandH1') = 0)
begin
	Insert Into [Settings].[Settings] ([Name],[Value]) Values('BrandH1', '#BRAND_NAME#')
end

GO--


Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Js.Review.Respond', 'Ответить') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Js.Review.Respond', 'Respond')
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Js.Review.Delete', 'Удалить') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Js.Review.Delete', 'Delete')
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Checkout.Success.CheckoutSuccessOrder', 'Спасибо, ваш заказ оформлен.') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Checkout.Success.CheckoutSuccessOrder', 'Thanks, your order is issued.')
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Checkout.Success.ReturnOnMain', 'Вернуться на главную') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Checkout.Success.ReturnOnMain', 'To return on main')
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Checkout.Success.FullVersionWebsite', 'Полная версия сайта') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Checkout.Success.FullVersionWebsite', 'Full version of the website')
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Home.Index.Catalog', 'Каталог') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Home.Index.Catalog', 'Catalog')

GO--

CREATE TABLE [CMS].[AdminNotification](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[Tag] [nvarchar](50) NULL,
	[Type] [nvarchar](50) NULL,
	[Title] [nvarchar](255) NOT NULL,
	[Body] [nvarchar](max) NULL,
	[IconPath] [nvarchar](255) NULL,
	[Data] [nvarchar](max) NULL,
 CONSTRAINT [PK_AdminNotification] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO--

ALTER TABLE [CMS].[AdminNotification] ADD  CONSTRAINT [DF_AdminNotification_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO--

CREATE TABLE [Customers].[AdminNotifications](
	[CustomerId] [uniqueidentifier] NOT NULL,
	[AdminNotificationid] [int] NOT NULL,
 CONSTRAINT [PK_AdminNotifications] PRIMARY KEY CLUSTERED 
(
	[CustomerId] ASC,
	[AdminNotificationid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO--

ALTER TABLE [Customers].[AdminNotifications]  WITH CHECK ADD  CONSTRAINT [FK_AdminNotifications_AdminNotification] FOREIGN KEY([AdminNotificationid])
REFERENCES [CMS].[AdminNotification] ([Id])
ON DELETE CASCADE
GO--
ALTER TABLE [Customers].[AdminNotifications] CHECK CONSTRAINT [FK_AdminNotifications_AdminNotification]
GO--

ALTER TABLE [Customers].[AdminNotifications]  WITH CHECK ADD  CONSTRAINT [FK_AdminNotifications_Customer] FOREIGN KEY([CustomerId])
REFERENCES [Customers].[Customer] ([CustomerID])
ON DELETE CASCADE
GO--
ALTER TABLE [Customers].[AdminNotifications] CHECK CONSTRAINT [FK_AdminNotifications_Customer]
GO--

Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Js.OrderConfermationCart.PhoneMask', '') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Js.OrderConfermationCart.PhoneMask', '')
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Js.Order.AddressShippingMethod', 'Адрес') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Js.Order.AddressShippingMethod', 'Address')

GO--

Update [Settings].[ModuleSettings] Set ModuleName = 'SupplierOfHappiness' Where ModuleName = 'supplierofhappiness'

GO--

DELETE FROM [Settings].[MailFormatType] Where MailFormatTypeID >= 23

DELETE FROM  [Settings].[MailFormat] where FormatType >= 23
GO--

SET IDENTITY_INSERT Settings.MailFormatType ON

INSERT INTO Settings.MailFormatType (MailFormatTypeId,TypeName,SortOrder,Comment)
VALUES (23, 'Смена комментария заказа пользователем', 200, 'Письмо при смене комментария пользователем(#ORDER_ID#, #ORDER_USER_COMMENT#, #STORE_NAME#, #NUMBER#)')


INSERT INTO Settings.MailFormatType (MailFormatTypeId,TypeName,SortOrder,Comment)
VALUES (24, 'Произведение/Отмена оплаты', 210, 'Письмо при проведении/отмене оплаты(#ORDER_ID#, #PAY_STATUS#, #STORE_NAME#, #NUMBER#, #SUM#)')

INSERT INTO Settings.MailFormatType (MailFormatTypeId,TypeName,SortOrder,Comment)
VALUES (25, 'Обновление задачи', 220, 'Уведомление постановщику и исполнителю задачи о ее изменении. Доступные переменные:
#CHANGES_TABLE# - таблица с изменениями задачи
#MODIFIER# - менеджер, внесший изменения в задачу
#MODIFIER_LINK# - сслыка на менеджера, изменившего задачу
#TASK_ID# - номер задачи
#TASK_GROUP# - группа задачи
#MANAGER_NAME# - исполнитель
#MANAGER_LINK# - сслыка на исполнителя
#APPOINTEDMANAGER# - постановщик
#APPOINTEDMANAGER_LINK# - ссылка на постановщика
#TASK_NAME# - название задачи
#TASK_DESCRIPTION# - описание задачи
#TASK_STATUS# - статус задачи
#TASK_PRIORITY# - приоритет задачи
#DUEDATE# - крайний срок
#DATE_CREATED# - дата создания задачи
#TASK_URL# - ссылка на задачу
#TASK_ATTACHMENTS# - прикрепленные файлы')

INSERT INTO Settings.MailFormatType (MailFormatTypeId,TypeName,SortOrder,Comment)
VALUES (26, 'Создание задачи', 230, 'Уведомление исполнителю задачи. Доступные переменные:
#TASK_ID# - номер задачи
#TASK_GROUP# - группа задачи
#MANAGER_NAME# - исполнитель
#MANAGER_LINK# - сслыка на исполнителя
#APPOINTEDMANAGER# - постановщик
#APPOINTEDMANAGER_LINK# - ссылка на постановщика
#TASK_NAME# - название задачи
#TASK_DESCRIPTION# - описание задачи
#TASK_STATUS# - статус задачи
#TASK_PRIORITY# - приоритет задачи
#DUEDATE# - крайний срок
#DATE_CREATED# - дата создания задачи
#TASK_URL# - ссылка на задачу
#TASK_ATTACHMENTS# - прикрепленные файлы')

INSERT INTO Settings.MailFormatType (MailFormatTypeId,TypeName,SortOrder,Comment)
VALUES (27, 'Удаление задачи', 240, 'Уведомление постановщику и исполнителю задачи о ее удалении. Доступные переменные:
#MODIFIER# - менеджер, удаливший задачу
#MODIFIER_LINK# - сслыка на менеджера, удалившего задачу
#TASK_ID# - номер задачи
#TASK_GROUP# - группа задачи
#MANAGER_NAME# - исполнитель
#MANAGER_LINK# - сслыка на исполнителя
#APPOINTEDMANAGER# - постановщик
#APPOINTEDMANAGER_LINK# - ссылка на постановщика
#TASK_NAME# - название задачи
#TASK_DESCRIPTION# - описание задачи
#TASK_STATUS# - статус задачи
#TASK_PRIORITY# - приоритет задачи
#DUEDATE# - крайний срок
#DATE_CREATED# - дата создания задачи
#TASK_ATTACHMENTS# - прикрепленные файлы')

INSERT INTO Settings.MailFormatType (MailFormatTypeId,TypeName,SortOrder,Comment)
VALUES (28, 'Новый комментарий к задаче', 250, 'Уведомление постановщику и исполнителю задачи о новом комментарии. Доступные переменные:
#AUTHOR# - автор комментария
#AUTHOR_LINK# - сслыка на автора
#COMMENT# - текст комментария
#TASK_ID# - номер задачи
#TASK_GROUP# - группа задачи
#MANAGER_NAME# - исполнитель
#MANAGER_LINK# - сслыка на исполнителя
#APPOINTEDMANAGER# - постановщик
#APPOINTEDMANAGER_LINK# - ссылка на постановщика
#TASK_NAME# - название задачи
#TASK_DESCRIPTION# - описание задачи
#TASK_STATUS# - статус задачи
#TASK_PRIORITY# - приоритет задачи
#DUEDATE# - крайний срок
#DATE_CREATED# - дата создания задачи
#TASK_URL# - ссылка на задачу
#TASK_ATTACHMENTS# - прикрепленные файлы')

SET IDENTITY_INSERT Settings.MailFormatType OFF
GO--

insert into [Settings].[MailFormat] ([FormatName],[FormatText],[FormatType],[SortOrder],[Enable],[AddDate],[ModifyDate],[FormatSubject])
values ('Смена комментария заказа пользователем', '<div style="color: #4c4f56; font-family: Arial, Helvetica, sans-serif; font-size: 14px;">
<div class="header" style="border-bottom: 1px solid #ededed; display: table; margin-bottom: 25px; padding-bottom: 25px; width: 100%;">
<div class="logo" style="display: table-cell; text-align: left; vertical-align: middle;">#LOGO#</div>

<div class="phone" style="display: table-cell; text-align: right; vertical-align: middle;">
<div class="tel" style="font-size: 26px; font-weight: bold; line-height: 1; margin-bottom: 5px;">&nbsp;</div>

<div class="inform" style="font-size: 12px;">&nbsp;</div>
</div>
</div>', 23, 1100, 1, getdate(), getdate(), 'Пользователь изменил свой комментарий к заказу № #NUMBER#')

GO--

insert into [Settings].[MailFormat] ([FormatName],[FormatText],[FormatType],[SortOrder],[Enable],[AddDate],[ModifyDate],[FormatSubject])
values ('Письмо при смене статуса заказа оплачено/не оплачено', '<div style="color: #4c4f56; font-family: Arial, Helvetica, sans-serif; font-size: 14px;">
<div class="header" style="border-bottom: 1px solid #ededed; display: table; margin-bottom: 25px; padding-bottom: 25px; width: 100%;">
<div class="logo" style="display: table-cell; text-align: left; vertical-align: middle;">#LOGO#</div>

<div class="phone" style="display: table-cell; text-align: right; vertical-align: middle;">
<div class="tel" style="font-size: 26px; font-weight: bold; line-height: 1; margin-bottom: 5px;">&nbsp;</div>

<div class="inform" style="font-size: 12px;">&nbsp;</div>
</div>
</div>

<p>По заказу № #NUMBER# была #PAY_STATUS# оплата на сумму #SUM#.</p>
</div>', 24, 1100, 1, getdate(), getdate(), 'Изменился статус оплаты заказа № #NUMBER#')

GO--

insert into [Settings].[MailFormat] ([FormatName],[FormatText],[FormatType],[SortOrder],[Enable],[AddDate],[ModifyDate],[FormatSubject])
values ('Обновление задачи', '<div style="color: #4c4f56; font-family: Arial, Helvetica, sans-serif; font-size: 14px;">
<div class="header" style="border-bottom: 1px solid #ededed; display: table; margin-bottom: 25px; padding-bottom: 25px; width: 100%;">
<div class="logo" style="display: table-cell; text-align: left; vertical-align: middle;">#LOGO#</div>

<div class="phone" style="display: table-cell; text-align: right; vertical-align: middle;">
<div class="tel" style="font-size: 26px; font-weight: bold; line-height: 1; margin-bottom: 5px;">&nbsp;</div>

<div class="inform" style="font-size: 12px;">&nbsp;</div>
</div>
</div>

<div>
<p><a href="#MODIFIER_LINK#">#MODIFIER#</a> обновил(-а) задачу №#TASK_ID#.</p>

<div class="o-title vi" style="font-size: 14px; font-weight: bold; margin: 5px 0;"><a href="#TASK_URL#">#TASK_NAME#</a></div>

<div class="l-row">
<div class="l-value vi" style="display: inline-block; margin: 5px 0;">#CHANGES_TABLE#</div>
</div>
</div>
</div>', 25, 1200, 1, getdate(), getdate(), 'Задача №#TASK_ID# обновлена. #TASK_NAME#')
GO--
insert into [Settings].[MailFormat] ([FormatName],[FormatText],[FormatType],[SortOrder],[Enable],[AddDate],[ModifyDate],[FormatSubject])
values ('Создание задачи', '<div style="color: #4c4f56; font-family: Arial, Helvetica, sans-serif; font-size: 14px;">
<div class="header" style="border-bottom: 1px solid #ededed; display: table; margin-bottom: 25px; padding-bottom: 25px; width: 100%;">
<div class="logo" style="display: table-cell; text-align: left; vertical-align: middle;">#LOGO#</div>

<div class="phone" style="display: table-cell; text-align: right; vertical-align: middle;">
<div class="tel" style="font-size: 26px; font-weight: bold; line-height: 1; margin-bottom: 5px;">&nbsp;</div>

<div class="inform" style="font-size: 12px;">&nbsp;</div>
</div>
</div>

<div>
<p><a href="#APPOINTEDMANAGER_LINK#">#APPOINTEDMANAGER#</a> создал(-а) задачу №#TASK_ID#.</p>

<div class="o-title vi" style="font-size: 14px; font-weight: bold; margin: 5px 0;"><a href="#TASK_URL#">#TASK_NAME#</a></div>

<div class="l-row">
<div class="l-value vi" style="display: inline-block; margin: 5px 0;">
<table>
	<tbody>
		<tr>
			<td style="color:#acacac;padding:5px 15px 5px 0;">Группа:</td>
			<td>#TASK_GROUP#</td>
		</tr>
		<tr>
			<td style="color:#acacac;padding:5px 15px 5px 0;">Дата создания:</td>
			<td>#DATE_CREATED#</td>
		</tr>
		<tr>
			<td style="color:#acacac;padding:5px 15px 5px 0;">Исполнитель:</td>
			<td><a href="#MANAGER_LINK#">#MANAGER_NAME#</a></td>
		</tr>
		<tr>
			<td style="color:#acacac;padding:5px 15px 5px 0;">Постановщик:</td>
			<td><a href="#APPOINTEDMANAGER_LINK#">#APPOINTEDMANAGER#</a></td>
		</tr>
		<tr>
			<td style="color:#acacac;padding:5px 15px 5px 0;">Статус:</td>
			<td>#TASK_STATUS#</td>
		</tr>
		<tr>
			<td style="color:#acacac;padding:5px 15px 5px 0;">Приоритет:</td>
			<td>#TASK_PRIORITY#</td>
		</tr>
		<tr>
			<td style="color:#acacac;padding:5px 15px 5px 0;">Крайний срок:</td>
			<td>#DUEDATE#</td>
		</tr>
		<tr>
			<td style="color:#acacac;padding:5px 15px 5px 0;">Приложения:</td>
			<td>#TASK_ATTACHMENTS#</td>
		</tr>
		<tr>
			<td colspan="2" style="padding:10px 0;">#TASK_DESCRIPTION#</td>
		</tr>
	</tbody>
</table>
</div>
</div>
</div>
</div>', 26, 1210, 1, getdate(), getdate(), 'Вам назначена задача №#TASK_ID#. #TASK_NAME#')
GO--
insert into [Settings].[MailFormat] ([FormatName],[FormatText],[FormatType],[SortOrder],[Enable],[AddDate],[ModifyDate],[FormatSubject])
values ('Удаление задачи', '<div style="color: #4c4f56; font-family: Arial, Helvetica, sans-serif; font-size: 14px;">
<div class="header" style="border-bottom: 1px solid #ededed; display: table; margin-bottom: 25px; padding-bottom: 25px; width: 100%;">
<div class="logo" style="display: table-cell; text-align: left; vertical-align: middle;">#LOGO#</div>

<div class="phone" style="display: table-cell; text-align: right; vertical-align: middle;">
<div class="tel" style="font-size: 26px; font-weight: bold; line-height: 1; margin-bottom: 5px;">&nbsp;</div>

<div class="inform" style="font-size: 12px;">&nbsp;</div>
</div>
</div>

<div>
<p><a href="#MODIFIER_LINK#">#MODIFIER#</a> удалил(-а) задачу №#TASK_ID#.</p>

<div class="o-title vi" style="font-size: 14px; font-weight: bold; margin: 5px 0;">#TASK_NAME#</div>
</div>
</div>', 27, 1220, 1, getdate(), getdate(), 'Задача №#TASK_ID# удалена. #TASK_NAME#')
GO--
insert into [Settings].[MailFormat] ([FormatName],[FormatText],[FormatType],[SortOrder],[Enable],[AddDate],[ModifyDate],[FormatSubject])
values ('Новый комментарий к задаче', '<div style="color: #4c4f56; font-family: Arial, Helvetica, sans-serif; font-size: 14px;">
<div class="header" style="border-bottom: 1px solid #ededed; display: table; margin-bottom: 25px; padding-bottom: 25px; width: 100%;">
<div class="logo" style="display: table-cell; text-align: left; vertical-align: middle;">#LOGO#</div>

<div class="phone" style="display: table-cell; text-align: right; vertical-align: middle;">
<div class="tel" style="font-size: 26px; font-weight: bold; line-height: 1; margin-bottom: 5px;">&nbsp;</div>

<div class="inform" style="font-size: 12px;">&nbsp;</div>
</div>
</div>

<div>
<p><a href="#AUTHOR_LINK#">#AUTHOR#</a> добавил(-а) комментарий к задаче №#TASK_ID#.</p>

<div class="o-title vi" style="font-size: 14px; font-weight: bold; margin: 5px 0;"><a href="#TASK_URL#">#TASK_NAME#</a></div>

<div class="l-row">
<div class="l-value vi" style="display: inline-block; margin: 5px 0;">#COMMENT#</div>
</div>
</div>
</div>', 28, 1230, 1, getdate(), getdate(), 'Новый комментарий к задаче №#TASK_ID#. #TASK_NAME#')
GO--

ALTER PROCEDURE [Catalog].[sp_AddProductToCategory] 
	@ProductId int,
	@CategoryId int,
	@SortOrder int,
	@MainCategory bit
AS
BEGIN

DECLARE @Main bit
	SET NOCOUNT ON;
if(@MainCategory = 1) 
	Begin
		set @Main = @MainCategory
		update [Catalog].[ProductCategories] set main = 0 where ProductID=@ProductID
	end

if (select count(*) from [Catalog].[ProductCategories] where ProductID=@ProductID and main=1) = 0
	set @Main = 1
else
	set @Main = 0
if (select count(*) from [Catalog].[ProductCategories] where CategoryID=@CategoryID and ProductID=@ProductID) = 0 
begin
	INSERT INTO [Catalog].[ProductCategories] (CategoryID, ProductID, SortOrder, Main) VALUES (@CategoryID, @ProductID, @SortOrder, @Main);
end
END

GO--

ALTER TABLE Settings.ExportFeedSettings ADD
    FileName nvarchar(50) NULL,
    FileExtention nvarchar(50) NULL,
    PriceMargin float(53) NULL,
    ExportNotAmountProducts bit NULL,
    ExportNotActiveProducts bit NULL,
    AdditionalUrlTags nvarchar(MAX) NULL,
    Active bit NULL,
    IntervalType nvarchar(50) NULL,
    Interval int NULL,
    JobStartTime datetime NULL,
    AdvancedSettings nvarchar(MAX) NULL
	
GO--
    
Alter Table [Settings].ExportFeedSettings 
    Alter Column [Value] nvarchar(max) NULL
	
GO--


Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Edost.CachOnDelivery.Sum', 'Стоимость доставки увеличится на {0}{1}') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Edost.CachOnDelivery.Sum', 'The cost of delivery will increase on {0}{1}')
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Edost.CachOnDelivery.Transfer', ' + доплатить при получении за денежный перевод: {0} Итого переплата за наложенный платеж: {1}') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Edost.CachOnDelivery.Transfer', ' + to pay in addition when receiving for money transfer: {0} Total an overpayment for cash on delivery: {1}')

GO--

UPDATE [Settings].[MailFormatType] SET [Comment] = 'Письмо при регистрации ( #EMAIL# ; #FIRSTNAME# ; #LASTNAME# ; #PHONE# ; #REGDATE# ; #PASSWORD# ; #SUBSRCIBE# ; #SHOPURL# )' WHERE [MailFormatTypeID] = 1
GO--

ALTER PROCEDURE [Order].[sp_GetOrdersMonthProgress]
AS
BEGIN
	SET NOCOUNT ON;
	select 
		sum([Sum]*CurrencyValue) as 'Sum', 
		SUM(([Sum] - [ShippingCost] - [TaxCost])*CurrencyValue) - SUM([SupplyTotal]) as 'Profit' 
	from [order].[order] Inner Join [Order].[OrderCurrency] On [OrderCurrency].[OrderId] = [Order].[OrderId]
	where datepart(month, [OrderDate]) = Datepart(month, getdate()) and datepart(year, [OrderDate]) = Datepart(year, getdate()) and [PaymentDate] is not null
END
GO--

ALTER PROCEDURE [Order].[sp_GetProfitByDays]
	@MinDate datetime,
	@MaxDate datetime
AS
BEGIN
	SET NOCOUNT ON;
	select 
		DATEADD(dd, 0, DATEDIFF(dd, 0, [OrderDate])) as 'Date',
		SUM(([Sum] - [ShippingCost] - [TaxCost])*CurrencyValue) - SUM([SupplyTotal]) as 'Profit'
	FROM [Order].[Order] Inner Join [Order].[OrderCurrency] On [OrderCurrency].[OrderId] = [Order].[OrderId]
	WHERE [OrderDate] > @MinDate and [OrderDate] < @MaxDate and [PaymentDate] is not null
	GROUP BY DATEADD(dd, 0, DATEDIFF(dd, 0, [OrderDate]))
END
GO--



UPDATE [Settings].[InternalSettings] SET [settingValue] = '5.0.21' WHERE [settingKey] = 'db_version'

alter table [Customers].[Customer] add [Enabled] bit null
GO--
update [Customers].[Customer] set [Enabled] = 1
GO--
alter table [Customers].[Customer] alter column [Enabled] bit not null
GO--

ALTER PROCEDURE [Customers].[sp_AddCustomer]
	@CustomerID uniqueidentifier,
	@CustomerGroupID int,
	@Password nvarchar(100),
	@FirstName nvarchar(70),
	@LastName nvarchar(70),
	@Phone nvarchar(max),
	@StandardPhone bigint,
	@RegistrationDateTime datetime,
	@Email nvarchar(100),
	@CustomerRole int,
	@Patronymic nvarchar(70),
	@BonusCardNumber bigint,
	@AdminComment nvarchar(MAX),
	@ManagerId int,
	@Rating int,
	@Enabled bit
AS
BEGIN
	if @CustomerID is null
		Set @CustomerID = newID()
	INSERT INTO [Customers].[Customer]
		([CustomerID]
		,[CustomerGroupID]
		,[Password]
		,[FirstName]
		,[LastName]
		,[Phone]
		,[StandardPhone]
		,[RegistrationDateTime]
		,[Email]
		,[CustomerRole]
		,[Patronymic]
		,[BonusCardNumber]
		,[AdminComment]
		,[ManagerId]
		,[Rating]
		,[Enabled])
	VALUES
		(@CustomerID
		,@CustomerGroupID
		,@Password
		,@FirstName
		,@LastName
		,@Phone
		,@StandardPhone
		,@RegistrationDateTime
		,@Email
		,@CustomerRole
		,@Patronymic
		,@BonusCardNumber
		,@AdminComment
		,@ManagerId
		,@Rating
		,@Enabled);
	SELECT CustomerID From [Customers].[Customer] Where Email = @Email
END
GO--

ALTER PROCEDURE [Customers].[sp_UpdateCustomerInfo] 
	@customerid uniqueidentifier, 
	@firstname nvarchar (70), 
	@lastname nvarchar(70), 
	@patronymic nvarchar(70), 
	@phone nvarchar(max), 
	@standardphone bigint, @email nvarchar(100) ,
	@customergroupid INT = NULL, 
	@customerrole INT, 
	@bonuscardnumber bigint, 
	@admincomment nvarchar (max), 
	@managerid INT, 
	@rating INT,
	@avatar nvarchar(100),
	@Enabled bit
AS
BEGIN
	UPDATE [customers].[customer]
	SET [firstname] = @firstname,
		[lastname] = @lastname,
		[patronymic] = @patronymic,
		[phone] = @phone,
		[standardphone] = @standardphone,
		[email] = @email,
		[customergroupid] = @customergroupid,
		[customerrole] = @customerrole,
		[bonuscardnumber] = @bonuscardnumber,
		[admincomment] = @admincomment,
		[managerid] = @managerid,
		[rating] = @rating,
		[avatar] = @avatar,
		[Enabled] = @Enabled
	WHERE customerid = @customerid
END
GO--

delete from [Settings].[Localization] where ResourceKey = 'Admin.Settings.Customers.Administrators'
delete from [Settings].[Localization] where ResourceKey = 'Admin.Settings.Customers.Managers'
delete from [Settings].[Localization] where ResourceKey = 'Admin.Settings.Customers.Moderators'
delete from [Settings].[Localization] where ResourceKey = 'Admin.Settings.Customers'
delete from [Settings].[Localization] where ResourceKey = 'Admin.Settings.Customers.Title'
GO--



ALTER TABLE Settings.MailFormatType ADD
	MailType nvarchar(70) NULL
	
GO--

Update [Settings].[MailFormatType] Set MailType = 'None' Where MailFormatTypeID = 0
Update [Settings].[MailFormatType] Set MailType = 'OnRegistration' Where MailFormatTypeID = 1
Update [Settings].[MailFormatType] Set MailType = 'OnPwdRepair' Where MailFormatTypeID = 2
Update [Settings].[MailFormatType] Set MailType = 'OnNewOrder' Where MailFormatTypeID = 3
Update [Settings].[MailFormatType] Set MailType = 'OnChangeOrderStatus' Where MailFormatTypeID = 4
Update [Settings].[MailFormatType] Set MailType = 'OnFeedback' Where MailFormatTypeID = 8
Update [Settings].[MailFormatType] Set MailType = 'OnProductDiscuss' Where MailFormatTypeID = 11
Update [Settings].[MailFormatType] Set MailType = 'OnOrderByRequest' Where MailFormatTypeID = 12
Update [Settings].[MailFormatType] Set MailType = 'OnSendLinkByRequest' Where MailFormatTypeID = 13
Update [Settings].[MailFormatType] Set MailType = 'OnSendFailureByRequest' Where MailFormatTypeID = 14
Update [Settings].[MailFormatType] Set MailType = 'OnSendGiftCertificate' Where MailFormatTypeID = 15
Update [Settings].[MailFormatType] Set MailType = 'OnBuyInOneClick' Where MailFormatTypeID = 16
Update [Settings].[MailFormatType] Set MailType = 'OnBillingLink' Where MailFormatTypeID = 17
Update [Settings].[MailFormatType] Set MailType = 'OnSetOrderManager' Where MailFormatTypeID = 18
Update [Settings].[MailFormatType] Set MailType = 'OnSetManagerTask' Where MailFormatTypeID = 19
Update [Settings].[MailFormatType] Set MailType = 'OnChangeManagerTaskStatus' Where MailFormatTypeID = 20
Update [Settings].[MailFormatType] Set MailType = 'OnLead' Where MailFormatTypeID = 21
Update [Settings].[MailFormatType] Set MailType = 'OnProductDiscussAnswer' Where MailFormatTypeID = 22
Update [Settings].[MailFormatType] Set MailType = 'OnChangeUserComment' Where MailFormatTypeID = 23
Update [Settings].[MailFormatType] Set MailType = 'OnPayOrder' Where MailFormatTypeID = 24
Update [Settings].[MailFormatType] Set MailType = 'OnTaskChanged' Where MailFormatTypeID = 25
Update [Settings].[MailFormatType] Set MailType = 'OnTaskCreated' Where MailFormatTypeID = 26
Update [Settings].[MailFormatType] Set MailType = 'OnTaskDeleted' Where MailFormatTypeID = 27
Update [Settings].[MailFormatType] Set MailType = 'OnTaskCommentAdded' Where MailFormatTypeID = 28

GO--


ALTER TABLE Settings.MailFormat ADD
	MailFormatTypeId int NULL
	
GO--


Update [Settings].[MailFormat] Set MailFormatTypeId = (Select MailFormatTypeId From Settings.MailFormatType as mft Where mft.[MailFormatTypeID] = [MailFormat].[FormatType])

GO--

ALTER TRIGGER [Settings].[UpdateMailFormat]
   ON  [Settings].[MailFormat]
   FOR UPDATE
AS 
BEGIN
	IF ((SELECT COUNT([MailFormatID]) FROM inserted WHERE [Enable] = 1) > 0)
	BEGIN
		UPDATE [Settings].[MailFormat] SET [Enable] = 0 WHERE [MailFormatTypeId] = (SELECT top 1 [MailFormatTypeId] FROM inserted)
		UPDATE [Settings].[MailFormat] SET [Enable] = 1 WHERE [MailFormatID] = (SELECT top 1 [MailFormatID] FROM inserted)
	END
END

GO--


ALTER TABLE Settings.MailFormat
	DROP COLUMN FormatType
GO--

Delete From [Settings].[MailFormat] Where [MailFormatTypeId] is null
GO--

Delete From [Settings].[MailFormatType] Where [MailType] is null
GO--

ALTER TABLE [Settings].[MailFormatType] ALTER COLUMN MailType nvarchar(70) NOT NULL
GO--


delete from [Settings].[MailFormat] 
where [MailFormatTypeId] not in (select MailFormattypeID from [Settings].[MailFormattype])
  
GO--


ALTER TABLE Settings.MailFormat ADD CONSTRAINT
	FK_MailFormat_MailFormatType FOREIGN KEY
	(
	MailFormatTypeId
	) REFERENCES Settings.MailFormatType
	(
	MailFormatTypeID
	) ON UPDATE  NO ACTION 
	 ON DELETE  SET NULL 

	 
GO--


 
UPDATE [Settings].[InternalSettings] SET [settingValue] = '5.0.23' WHERE [settingKey] = 'db_version'
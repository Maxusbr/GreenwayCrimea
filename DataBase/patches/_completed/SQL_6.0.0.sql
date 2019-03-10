UPDATE [Settings].[MailFormatType] 
SET [Comment] = 'Письмо при регистрации ( #EMAIL# ; #FIRSTNAME# ; #LASTNAME# ; #PHONE# ; #REGDATE# ; #PASSWORD# ; #SUBSRCIBE# ; #SHOPURL# ; #PATRONYMIC# )'
WHERE [TypeName] = 'При регистрации'

GO--

--alter table [Order].[OrderCurrency]
--add [RoundNumbers] float null

GO--

update [Order].[OrderCurrency] set RoundNumbers = (
select isnull(RoundNumbers, 0) from Catalog.Currency where [OrderCurrency].[CurrencyCode] = Currency.[CurrencyIso3])

GO--

alter table [Order].[OrderCurrency]
alter column [RoundNumbers] float not null

GO--

--alter table [Order].[OrderCurrency]
--add [EnablePriceRounding] float

GO--

update [Order].[OrderCurrency] set [EnablePriceRounding] = (
select isnull([EnablePriceRounding], 0) from Catalog.Currency where [OrderCurrency].[CurrencyCode] = Currency.[CurrencyIso3])

GO--

alter table [Order].[OrderCurrency]
alter column [EnablePriceRounding] bit not null

GO--

alter PROCEDURE [Catalog].[sp_UpdateOrInsertProductProperty]
@ProductID int,
@Name nvarchar(255),
@Value nvarchar(255),
@RangeValue float,
@SortOrder int
AS
BEGIN
Declare @propertyId int;
Set @propertyId = 0;
Select @propertyId = PropertyID From Catalog.Property Where Name = @Name;
if( @propertyId = 0 )
begin
Insert into Catalog.Property (Name, UseInFilter, SortOrder, Expanded, [Type], [UseInDetails], [useinbrief]) Values (@Name, 0, 0, 0, 0, 1, 0)
Select @propertyId = PropertyID From Catalog.Property Where Name = @Name
end
declare @propertyValueId int;
Set @propertyValueId = 0;
Select @propertyValueId = PropertyValueID From Catalog.PropertyValue Where Value = @Value and RangeValue = @RangeValue and PropertyID = @propertyId;
if(@propertyValueId = 0)
begin
Insert into Catalog.PropertyValue (PropertyID, Value, RangeValue) Values (@propertyId, @Value, @RangeValue)
Select @propertyValueId = PropertyValueID From Catalog.PropertyValue Where PropertyID = @propertyId and Value = @Value and RangeValue = @RangeValue
end
if((Select COUNT(ProductID) From Catalog.ProductPropertyValue Where ProductID = @ProductID and PropertyValueID = @propertyValueId) = 0 )
begin
Insert into Catalog.ProductPropertyValue (ProductID, PropertyValueID) Values (@ProductID, @propertyValueId)
end
END 

GO--



ALTER TABLE [Order].[Order] ADD
	IsDraft bit NULL
	
GO--

Update [Order].[Order] Set IsDraft = 0

GO--


ALTER TABLE [Order].[OrderCustomer] ADD
	Country nvarchar(70) NULL,
	Region nvarchar(70) NULL,
	City nvarchar(255) NULL,
	Zip nvarchar(70) NULL,
	Street nvarchar(1000) NULL,
	CustomField1 nvarchar(1000) NULL,
	CustomField2 nvarchar(1000) NULL,
	CustomField3 nvarchar(1000) NULL,
	House nvarchar(10) NULL,
	Apartment nvarchar(10) NULL,
	Structure nvarchar(10) NULL,
	Entrance nvarchar(10) NULL,
	Floor nvarchar(10) NULL	
GO--


Update customer 
Set 
	customer.Country = contact.Country,
	customer.Region = contact.Zone,
	customer.City = contact.City,
	customer.Zip = contact.Zip,
	customer.Street = contact.Address,
	customer.CustomField1 = contact.CustomField1,
	customer.CustomField2 = contact.CustomField2,
	customer.CustomField3 = contact.CustomField3
	
From	
	[Order].[OrderCustomer] as customer
	Inner Join [Order].[Order] as o On customer.[OrderId] = o.[OrderId] 
	Inner Join [Order].[OrderContact] as contact On contact.[OrderContactID] = o.[ShippingContactID] 

GO--


if not exists( select * from [Settings].[MailFormatType] where [MailType]= 'OnSendToCustomer')
Insert Into [Settings].[MailFormatType] ([TypeName],[SortOrder],[Comment],[MailType]) Values ('Письмо покупателю', 300, 'Письмо покупателю (#FIRSTNAME#, #LASTNAME#, #PATRONYMIC#, #STORE_NAME#)', 'OnSendToCustomer')

GO--

if not exists( select * from [Settings].[MailFormat]  where [MailFormatTypeId]= (Select top(1) [MailFormatTypeID] From [Settings].[MailFormatType] Where [MailType] = 'OnSendToCustomer'))
Insert Into [Settings].[MailFormat] ([FormatName],[FormatText],[SortOrder],[Enable],[AddDate],[ModifyDate],[FormatSubject],[MailFormatTypeId])
Values ('Письмо покупателю', '<div style="color: #4c4f56; font-family: Arial, Helvetica, sans-serif; font-size: 14px;">
<div class="header" style="border-bottom: 1px solid #ededed; display: table; margin-bottom: 25px; padding-bottom: 25px; width: 100%;">
<div class="logo" style="display: table-cell; text-align: left; vertical-align: middle;">#LOGO#</div>

<div class="phone" style="display: table-cell; text-align: right; vertical-align: middle;">
<div class="tel" style="font-size: 26px; font-weight: bold; line-height: 1; margin-bottom: 5px;">&nbsp;</div>

<div class="inform" style="font-size: 12px;">&nbsp;</div>
</div>
</div>

<div>
#TEXT#
</div>
</div>', 1300, 1, getdate(), getdate(), '#STORE_NAME#', (Select top(1) [MailFormatTypeID] From [Settings].[MailFormatType] Where [MailType] = 'OnSendToCustomer'))
GO--


alter table settings.ExportFeedSettings
alter column FileName nvarchar(100)

GO--


ALTER TABLE [Order].[Order] ADD
	DeliveryDate datetime NULL,
	DeliveryTime nvarchar(50) NULL
	
GO--


IF NOT EXISTS( SELECT *  FROM sys.columns   WHERE Name = N'TrackNumber' AND Object_ID = Object_ID(N'[Order].[Order]'))
BEGIN
ALTER TABLE [Order].[Order] ADD
	TrackNumber nvarchar(255) NULL
END
GO--


ALTER TABLE [Order].[Order] ADD
	IsFromAdminArea bit NULL
	
GO--


Update [Order].[Order] Set IsFromAdminArea = 0

GO--

CREATE TABLE [Order].[OrderTrafficSource](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ObjId] [int] NOT NULL,
	[ObjType] [int] NOT NULL,
	[CustomerId] [uniqueidentifier] NOT NULL,
	[CreateOn] [datetime] NULL,
	[Referrer] [nvarchar](max) NULL,
	[Url] [nvarchar](max) NULL,
	[utm_source] [nvarchar](max) NULL,
	[utm_medium] [nvarchar](max) NULL,
	[utm_campaign] [nvarchar](max) NULL,
	[utm_content] [nvarchar](max) NULL,
	[utm_term] [nvarchar](max) NULL,
	[GoogleClientId] [nvarchar](50) NULL,
 CONSTRAINT [PK_OrderTrafficSource] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO--

alter table [Customers].[Customer] add [HeadCustomerId] uniqueidentifier null
GO--
alter table [Customers].[Customer] add [BirthDay] datetime null
GO--
alter table [Customers].[Customer] add [City] nvarchar(70)
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
	@Enabled bit,
	@HeadCustomerId uniqueidentifier,
	@BirthDay datetime,
	@City nvarchar(70)
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
		,[Enabled]
		,[HeadCustomerId]
		,[BirthDay]
		,[City])
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
		,@Enabled
		,@HeadCustomerId
		,@BirthDay
		,@City);
	SELECT CustomerID From [Customers].[Customer] Where Email = @Email
END
GO--

ALTER PROCEDURE [Customers].[sp_UpdateCustomerInfo] 
	@customerid uniqueidentifier, 
	@firstname nvarchar (70), 
	@lastname nvarchar(70), 
	@patronymic nvarchar(70), 
	@phone nvarchar(max), 
	@standardphone bigint, 
	@email nvarchar(100) ,
	@customergroupid INT = NULL, 
	@customerrole INT, 
	@bonuscardnumber bigint, 
	@admincomment nvarchar (max), 
	@managerid INT, 
	@rating INT,
	@avatar nvarchar(100),
	@Enabled bit,
	@HeadCustomerId uniqueidentifier,
	@BirthDay datetime,
	@City nvarchar(70)
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
		[Enabled] = @Enabled,
		[HeadCustomerId] = @HeadCustomerId,
		[BirthDay] = @BirthDay,
		[City] = @City
	WHERE customerid = @customerid
END
GO--

CREATE TRIGGER [Customers].[CustomerDeleted] ON [Customers].[Customer]
WITH EXECUTE AS CALLER
FOR DELETE
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE [Customers].[Customer] SET [HeadCustomerId] = null WHERE [HeadCustomerId] in (SELECT [CustomerID] FROM Deleted)
END
GO--

	
CREATE TABLE [Customers].[ManagerRole](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[SortOrder] [int] NOT NULL,
 CONSTRAINT [PK_ManagerRole] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO--

CREATE TABLE [Customers].[ManagerRolesMap](
	[CustomerId] [uniqueidentifier] NOT NULL,
	[ManagerRoleId] [int] NOT NULL,
 CONSTRAINT [PK_ManagerRolesMap] PRIMARY KEY CLUSTERED 
(
	[CustomerId] ASC,
	[ManagerRoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO--

ALTER TABLE [Customers].[ManagerRolesMap]  WITH CHECK ADD  CONSTRAINT [FK_ManagerRolesMap_Customer] FOREIGN KEY([CustomerId])
REFERENCES [Customers].[Customer] ([CustomerID])
ON DELETE CASCADE
GO--

ALTER TABLE [Customers].[ManagerRolesMap] CHECK CONSTRAINT [FK_ManagerRolesMap_Customer]
GO--

ALTER TABLE [Customers].[ManagerRolesMap]  WITH CHECK ADD  CONSTRAINT [FK_ManagerRolesMap_ManagerRole] FOREIGN KEY([ManagerRoleId])
REFERENCES [Customers].[ManagerRole] ([Id])
ON DELETE CASCADE
GO--

ALTER TABLE [Customers].[ManagerRolesMap] CHECK CONSTRAINT [FK_ManagerRolesMap_ManagerRole]
GO--

CREATE TABLE [Customers].[CustomerField](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[FieldType] [int] NOT NULL,
	[SortOrder] [int] NOT NULL,
	[Required] [bit] NOT NULL,
	[Enabled] [bit] NOT NULL,
 CONSTRAINT [PK_CustomerField] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO--

CREATE TABLE [Customers].[CustomerFieldValue](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CustomerFieldId] [int] NOT NULL,
	[Value] [nvarchar](255) NOT NULL,
	[SortOrder] [int] NOT NULL,
 CONSTRAINT [PK_CustomerFieldValue] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO--

ALTER TABLE [Customers].[CustomerFieldValue]  WITH CHECK ADD  CONSTRAINT [FK_CustomerFieldValue_CustomerField] FOREIGN KEY([CustomerFieldId])
REFERENCES [Customers].[CustomerField] ([Id])
ON DELETE CASCADE
GO--

ALTER TABLE [Customers].[CustomerFieldValue] CHECK CONSTRAINT [FK_CustomerFieldValue_CustomerField]
GO--

CREATE TABLE [Customers].[CustomerFieldValuesMap](
	[CustomerId] [uniqueidentifier] NOT NULL,
	[CustomerFieldId] [int] NOT NULL,
	[Value] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_CustomerFieldValues] PRIMARY KEY CLUSTERED 
(
	[CustomerId] ASC,
	[CustomerFieldId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO--

ALTER TABLE [Customers].[CustomerFieldValuesMap]  WITH CHECK ADD  CONSTRAINT [FK_CustomerFieldValues_Customer] FOREIGN KEY([CustomerId])
REFERENCES [Customers].[Customer] ([CustomerID])
ON DELETE CASCADE
GO--

ALTER TABLE [Customers].[CustomerFieldValuesMap] CHECK CONSTRAINT [FK_CustomerFieldValues_Customer]
GO--

ALTER TABLE [Customers].[CustomerFieldValuesMap]  WITH CHECK ADD  CONSTRAINT [FK_CustomerFieldValues_CustomerField] FOREIGN KEY([CustomerFieldId])
REFERENCES [Customers].[CustomerField] ([Id])
ON DELETE CASCADE
GO--

ALTER TABLE [Customers].[CustomerFieldValuesMap] CHECK CONSTRAINT [FK_CustomerFieldValues_CustomerField]
GO--


if not exists( select * from [Customers].[RoleAction]  where [Key]= 'DisplayUsers')
INSERT INTO [Customers].[RoleAction] ([Name],[Key],[Enabled],[Category],[SortOrder]) VALUES ('Редактирование сотрудников', 'DisplayUsers', 1, 'Настройка', 10)
GO--

ALTER TABLE [Order].[Order] ADD
	OrderDiscountValue float(53) NULL
GO--


ALTER PROCEDURE [Catalog].[sp_GetPropertyValuesByPropertyID] @PropertyID INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [PropertyValueID]
		,[Property].[PropertyID]
		,[Value]
		,[PropertyValue].[SortOrder]
		,[Property].UseinFilter
		,[Property].UseIndetails
		,[Property].UseInBrief
		,Property.NAME AS PropertyName
		,Property.SortOrder AS PropertySortOrder
		,Property.Expanded
		,Property.[Type]
		,Property.[Description]
		,GroupId
		,GroupName
		,GroupSortorder
		,unit
	FROM [Catalog].[PropertyValue]
	INNER JOIN [Catalog].[Property] ON [Property].[PropertyID] = [PropertyValue].[PropertyID]
	LEFT JOIN [Catalog].PropertyGroup ON PropertyGroup.PropertyGroupID = [Property].GroupID
	WHERE [Property].[PropertyID] = @PropertyID
	order by [PropertyValue].[SortOrder]
END


GO--

ALTER PROCEDURE [Catalog].[sp_GetPropertyValuesByProductID] @ProductID INT  
AS  
BEGIN  
 SET NOCOUNT ON;  
  
 SELECT  
   [PropertyValue].[PropertyValueID]  
  ,[PropertyValue].[PropertyID]  
  ,[PropertyValue].[Value]  
  ,[PropertyValue].[SortOrder]  
  ,[Property].UseinFilter  
  ,[Property].UseIndetails  
  ,[Property].UseInBrief  
  ,[Property].[Name] as PropertyName  
  ,[Property].[SortOrder] as PropertySortOrder  
  ,[Property].[Expanded] as Expanded  
  ,[Property].[Type] as [Type]  
  ,[Property].GroupId as GroupId  
  ,[Property].[Description] as [Description]
  ,GroupName  
  ,GroupSortorder
  ,unit
 FROM [Catalog].[PropertyValue]  
 INNER JOIN [Catalog].[ProductPropertyValue] ON [ProductPropertyValue].[PropertyValueID] = [PropertyValue].[PropertyValueID]  
 inner join [Catalog].[Property] on [Property].[PropertyID] = [PropertyValue].[PropertyID]  
 left join Catalog.PropertyGroup on propertyGroup.PropertyGroupID = [Property].GroupID  
 WHERE [ProductID] = @ProductID  
 ORDER BY case when PropertyGroup.GroupSortOrder is null then 1 else 0 end, 
 PropertyGroup.GroupSortOrder,PropertyGroup.GroupName, [Property].[SortOrder], [Property].Name, [PropertyValue].[SortOrder], [PropertyValue].Value  
END

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
             GroupSortOrder,
			 unit
      FROM   [Catalog].[PropertyValue]
      INNER JOIN [Catalog].[Property] ON [Property].[Propertyid] = [PropertyValue].[PropertyID]
      LEFT JOIN [Catalog].PropertyGroup ON PropertyGroup.PropertyGroupId = [Property].GroupId
      WHERE  [PropertyValue].[PropertyValueId] = @PropertyValueId
  END 

  GO--



ALTER TABLE [Catalog].[Product]
ALTER COLUMN ArtNo nvarchar(100) NOT NULL
GO--
ALTER TABLE [Catalog].[Offer]
ALTER COLUMN ArtNo nvarchar(100) NOT NULL
GO--
ALTER PROCEDURE [Catalog].[sp_AddOffer]
			@ArtNo nvarchar(100),
			@ProductID int,
			@Amount float,
			@Price money,
			@SupplyPrice money,
			@ColorID int,
			@SizeID int,
			@Main bit
AS
BEGIN

	INSERT INTO [Catalog].[Offer]
		(
		   ArtNo,
           [ProductID]
           ,[Amount]
           ,[Price]
           ,[SupplyPrice]
			,ColorID
			,SizeID
			,Main
		)
     VALUES
		(
			@ArtNo
			,@ProductID
			,@Amount
			,@Price
			,@SupplyPrice
			,@ColorID
			,@SizeID
			,@Main
		);
	SELECT SCOPE_IDENTITY();
END

GO--

IF not EXISTS (
  SELECT * 
  FROM   sys.columns 
  WHERE  object_id = OBJECT_ID(N'[catalog].[product]') 
         AND name = 'BarCode'
)
Begin
	alter table catalog.product add BarCode nvarchar(50) null
End

GO--

ALTER PROCEDURE [Catalog].[sp_UpdateOffer]
			@OfferID int,
			@ProductID int,
			@ArtNo nvarchar(100),
			@Amount float,
			@Price money,
			@SupplyPrice money,
			@ColorID int,
			@SizeID int,
			@Main bit
AS
BEGIN
		UPDATE [Catalog].[Offer]
		SET 		
			  [ProductID] = @ProductID,
			  ArtNo=@ArtNo
			  ,[Amount] = @Amount
			  ,[Price] = @Price
			  ,[SupplyPrice] = @SupplyPrice
			  ,[ColorID] = @ColorID
			  ,[SizeID] = @SizeID
			  ,Main = @Main
		WHERE [OfferID] = @OfferID
END

GO--


UPDATE [CMS].[Menu] SET [MenuItemUrlPath] ='forgotpassword' WHERE [MenuItemUrlPath]='fogotpassword'

GO--

ALTER PROCEDURE [Settings].[sp_GetExportFeedProducts] @exportFeedId int
	,@onlyCount BIT
	,@exportNotAvailable bit
	,@selectedCurrency NVARCHAR(10)

AS
BEGIN
	DECLARE @res TABLE (productId INT PRIMARY KEY CLUSTERED);
	DECLARE @lproduct TABLE (productId INT PRIMARY KEY CLUSTERED);

	INSERT INTO @lproduct
	SELECT [ProductID]
	FROM [Settings].[ExportFeedSelectedProducts]
	WHERE [ExportFeedId] = @exportFeedId;

	DECLARE @lcategory TABLE (CategoryId INT PRIMARY KEY CLUSTERED);
	DECLARE @l TABLE (CategoryId INT PRIMARY KEY CLUSTERED);

	INSERT INTO @l
	SELECT t.CategoryId
	FROM [Settings].[ExportFeedSelectedCategories] AS t
	INNER JOIN CATALOG.Category ON t.CategoryId = Category.CategoryId
	WHERE [ExportFeedId] = @exportFeedId
		AND HirecalEnabled = 1
		AND Enabled = 1

	DECLARE @l1 INT

	SET @l1 = (
			SELECT MIN(CategoryId)
			FROM @l
			);

	WHILE @l1 IS NOT NULL
	BEGIN
		INSERT INTO @lcategory
		SELECT id
		FROM Settings.GetChildCategoryByParent(@l1) AS dt
		INNER JOIN CATALOG.Category ON CategoryId = id
		WHERE dt.id NOT IN (
				SELECT CategoryId
				FROM @lcategory
				)
			AND HirecalEnabled = 1
			AND Enabled = 1

		SET @l1 = (
				SELECT MIN(CategoryId)
				FROM @l
				WHERE CategoryId > @l1
				);
	END;

	IF @onlyCount = 1
	BEGIN
		SELECT COUNT(OfferId)
		FROM (
			(
				SELECT OfferId
				FROM [Catalog].[Product]
				INNER JOIN [Catalog].[Offer] ON [Offer].[ProductID] = [Product].[ProductID]
				INNER JOIN [Catalog].[ProductCategories] ON [ProductCategories].[ProductID] = [Product].[ProductID]
					AND (
						CategoryId IN (
							SELECT CategoryId
							FROM @lcategory
							)
						OR [ProductCategories].[ProductID] IN (
							SELECT productId
							FROM @lproduct
							)
						)
				--LEFT JOIN [Catalog].[Photo] ON [Product].[ProductID] = [Photo].[ObjId] and Type ='Product' AND [Photo].[Main] = 1
				WHERE (
						SELECT TOP (1) [ProductCategories].[CategoryId]
						FROM [Catalog].[ProductCategories]
						INNER JOIN [Catalog].[Category] ON [Category].[CategoryId] = [ProductCategories].[CategoryId]
						WHERE [ProductID] = [Product].[ProductID]
							AND [Enabled] = 1
							AND [Main] = 1
						) = [ProductCategories].[CategoryId]
					AND (Offer.Price > 0 OR @exportNotAvailable = 1)
					AND (
						Offer.Amount > 0
						OR Product.AllowPreOrder = 1
						OR @exportNotAvailable = 1
						)
					AND CategoryEnabled = 1
					AND (Product.Enabled = 1 OR @exportNotAvailable = 1)
				)
			) AS dd
	END
	ELSE
	BEGIN
		DECLARE @defaultCurrencyRatio FLOAT;

		SELECT @defaultCurrencyRatio = CurrencyValue
		FROM [Catalog].[Currency]
		WHERE CurrencyIso3 = @selectedCurrency;

		SELECT [Product].[Enabled]
			,[Product].[ProductID]
			,[Product].[Discount]
			,AllowPreOrder
			,Amount
			,[ProductCategories].[CategoryId] AS [ParentCategory]
			,([Offer].[Price] / @defaultCurrencyRatio) AS Price
			,ShippingPrice
			,[Product].[Name]
			,[Product].[UrlPath]
			,[Product].[Description]
			,[Product].[BriefDescription]
			,[Product].SalesNote
			,OfferId
			,[Product].ArtNo
			,[Offer].Main
			,[Offer].ColorID
			,ColorName
			,[Offer].SizeID
			,SizeName
			,BrandName
			,CountryName as BrandCountry
			,GoogleProductCategory
			,YandexMarketCategory
			,YandexTypePrefix
			,YandexModel
			,Gtin
			,Adult
			,CurrencyValue
			,[Settings].PhotoToString(Offer.ColorID, Product.ProductId) AS Photos
			,ManufacturerWarranty
			,[Weight]
			,[Product].[Enabled]
			,[Offer].SupplyPrice
			,[Offer].ArtNo AS OfferArtNo
			,[Product].BarCode

		FROM [Catalog].[Product]
		INNER JOIN [Catalog].[Offer] ON [Offer].[ProductID] = [Product].[ProductID]
		INNER JOIN [Catalog].[ProductCategories] ON [ProductCategories].[ProductID] = [Product].[ProductID]
			AND (
				CategoryId IN (
					SELECT CategoryId
					FROM @lcategory
					)
				OR [ProductCategories].[ProductID] IN (
					SELECT productId
					FROM @lproduct
					)
				)
		--LEFT JOIN [Catalog].[Photo] ON [Product].[ProductID] = [Photo].[ObjId] and Type ='Product' AND [Photo].[Main] = 1
		LEFT JOIN [Catalog].[Color] ON [Color].ColorID = [Offer].ColorID
		LEFT JOIN [Catalog].[Size] ON [Size].SizeID = [Offer].SizeID
		LEFT JOIN [Catalog].Brand ON Brand.BrandID = [Product].BrandID
		LEFT JOIN [Customers].Country ON Brand.CountryID = Country.CountryID
		INNER JOIN [Catalog].Currency ON Currency.CurrencyID = [Product].CurrencyID
		WHERE (
				SELECT TOP (1) [ProductCategories].[CategoryId]
				FROM [Catalog].[ProductCategories]
				INNER JOIN [Catalog].[Category] ON [Category].[CategoryId] = [ProductCategories].[CategoryId]
				WHERE [ProductID] = [Product].[ProductID]
					AND [Enabled] = 1
					AND [Main] = 1
				) = [ProductCategories].[CategoryId]
			AND (Offer.Price > 0 OR @exportNotAvailable = 1)
			AND (
				Offer.Amount > 0
				OR Product.AllowPreOrder = 1
				OR @exportNotAvailable = 1
				)
			AND CategoryEnabled = 1
			AND (Product.Enabled = 1 OR @exportNotAvailable = 1)
	END
END

GO--

ALTER PROCEDURE [Settings].[sp_GetCsvProducts]     
   @exportFeedId int    
  ,@onlyCount BIT    
  ,@exportNoInCategory BIT    
  --,@exportNotActive BIT  
  --,@exportNotAmount BIT  
AS    
BEGIN    
 DECLARE @res TABLE (productId INT PRIMARY KEY CLUSTERED);    
 DECLARE @lproduct TABLE (productId INT PRIMARY KEY CLUSTERED);    
 DECLARE @lproductNoCat TABLE (productId INT PRIMARY KEY CLUSTERED);    
    
 INSERT INTO @lproduct    
  SELECT [ProductID]    
  FROM [Settings].[ExportFeedSelectedProducts]    
  WHERE [ExportFeedId] = @exportFeedId;    
    
 IF (@exportNoInCategory = 1)    
 BEGIN    
  INSERT INTO @lproductNoCat    
   SELECT [ProductID]    
   FROM [Catalog].Product    
   WHERE [ProductID] NOT IN (    
  SELECT [ProductID]    
  FROM [Catalog].[ProductCategories]    
  );    
 END    
    
 DECLARE @lcategory TABLE (CategoryId INT PRIMARY KEY CLUSTERED);    
 DECLARE @l TABLE (CategoryId INT PRIMARY KEY CLUSTERED);    
    
 INSERT INTO @l    
  SELECT t.CategoryId    
  FROM [Settings].[ExportFeedSelectedCategories] AS t    
  INNER JOIN CATALOG.Category ON t.CategoryId = Category.CategoryId    
  WHERE [ExportFeedId] = @exportFeedId    
    
 DECLARE @l1 INT    
    
 SET @l1 = (SELECT MIN(CategoryId) FROM @l);    
    
 WHILE @l1 IS NOT NULL    
 BEGIN  
     
  INSERT INTO @lcategory    
   SELECT id    
   FROM Settings.GetChildCategoryByParent(@l1) AS dt    
   INNER JOIN CATALOG.Category ON CategoryId = id    
   WHERE dt.id NOT IN (SELECT CategoryId FROM @lcategory)  
    
  SET @l1 = (SELECT MIN(CategoryId) FROM @l  WHERE CategoryId > @l1);    
 END;    
    
 IF @onlyCount = 1    
 BEGIN    
  SELECT COUNT(ProductID)    
  FROM [Catalog].[Product]    
  WHERE   
  (  
 EXISTS (    
  SELECT 1    
  FROM [Catalog].[ProductCategories]    
  WHERE [ProductCategories].[ProductID] = [Product].[ProductID]    
   AND ([ProductCategories].[ProductID] IN (SELECT productId FROM @lproduct)    
    OR [ProductCategories].CategoryId IN (SELECT CategoryId FROM @lcategory))    
 )    
    OR EXISTS (    
  SELECT 1    
  FROM @lproductNoCat AS TEMP    
  WHERE TEMP.productId = [Product].[ProductID]    
 )  
   )     
 END    
 ELSE    
 BEGIN    
  SELECT *    
  FROM [Catalog].[Product]    
  LEFT JOIN [Catalog].[Photo] ON [Photo].[ObjId] = [Product].[ProductID] AND Type = 'Product' AND Photo.[Main] = 1    
  WHERE   
  (  
 EXISTS (    
  SELECT 1    
  FROM [Catalog].[ProductCategories]    
  WHERE [ProductCategories].[ProductID] = [Product].[ProductID]    
   AND ([ProductCategories].[ProductID] IN (SELECT productId FROM @lproduct)    
    OR [ProductCategories].CategoryId IN (SELECT CategoryId FROM @lcategory))    
    )    
    OR EXISTS (    
  SELECT 1    
  FROM @lproductNoCat AS TEMP    
  WHERE TEMP.productId = [Product].[ProductID]    
    )  
   )       
 END    
END 
GO--

ALTER TABLE Catalog.Product ADD
	Cbid float(53) NULL,
	Fee float(53) NULL
	
GO--


ALTER PROCEDURE [Catalog].[sp_AddProduct]    
	@ArtNo nvarchar(50) = '',  
	@Name nvarchar(255),     
	@Ratio float,  
	@Discount float,  
	@Weight float,   
	@BriefDescription nvarchar(max),  
	@Description nvarchar(max),  
	@Enabled tinyint,  
	@Recomended bit,  
	@New bit,  
	@BestSeller bit,  
	@OnSale bit,  
	@BrandID int,  
	@AllowPreOrder bit,  
	@UrlPath nvarchar(150),  
	@Unit nvarchar(50),  
	@ShippingPrice float,  
	@MinAmount float,  
	@MaxAmount float,  
	@Multiplicity float,  
	@HasMultiOffer bit,  
	@SalesNote nvarchar(50),  
	@GoogleProductCategory nvarchar(500),  
	@YandexMarketCategory nvarchar(500),  
	@Gtin nvarchar(50),  
	@Adult bit,
	@Length float,
	@Width float,
	@Height float,
	@CurrencyID int,
	@ActiveView360 bit,
	@ManufacturerWarranty bit,
	@ModifiedBy nvarchar(50),
	@YandexTypePrefix nvarchar(500),
	@YandexModel nvarchar(500),
	@BarCode nvarchar(50),
	@Cbid float,
    @Fee float
AS  
BEGIN  
Declare @Id int  
INSERT INTO [Catalog].[Product]  
           ([ArtNo]  
           ,[Name]                     
           ,[Ratio]  
           ,[Discount]  
           ,[Weight]  
           ,[BriefDescription]  
           ,[Description]  
           ,[Enabled]  
           ,[DateAdded]  
           ,[DateModified]  
           ,[Recomended]  
           ,[New]  
           ,[BestSeller]  
           ,[OnSale]  
           ,[BrandID]  
           ,[AllowPreOrder]  
		   ,[UrlPath]  
		   ,[Unit]  
		   ,[ShippingPrice]  
		   ,[MinAmount]  
		   ,[MaxAmount]  
		   ,[Multiplicity]  
		   ,[HasMultiOffer]  
		   ,[SalesNote]  
		   ,GoogleProductCategory  
		   ,YandexMarketCategory
		   ,Gtin  
		   ,Adult
		   ,Length
		   ,Width
		   ,Height
		   ,CurrencyID
		   ,ActiveView360
		   ,ManufacturerWarranty
		   ,ModifiedBy
		   ,YandexTypePrefix
		   ,YandexModel
		   ,BarCode
		   ,Cbid
		   ,Fee
          )  
     VALUES  
           (@ArtNo,  
		   @Name,       
		   @Ratio,  
		   @Discount,  
		   @Weight,
		   @BriefDescription,  
		   @Description,  
		   @Enabled,  
		   GETDATE(),  
		   GETDATE(),  
		   @Recomended,  
		   @New,  
		   @BestSeller,  
		   @OnSale,  
		   @BrandID,  
		   @AllowPreOrder,  
		   @UrlPath,  
		   @Unit,  
		   @ShippingPrice,  
		   @MinAmount,  
		   @MaxAmount,  
		   @Multiplicity,  
		   @HasMultiOffer,  
		   @SalesNote,  
		   @GoogleProductCategory,  
		   @YandexMarketCategory,
		   @Gtin,  
		   @Adult,
		   @Length,
		   @Width,
		   @Height,
		   @CurrencyID,
		   @ActiveView360,
		   @ManufacturerWarranty,
		   @ModifiedBy,
		   @YandexTypePrefix,
		   @YandexModel,
		   @BarCode,
		   @Cbid,
		   @Fee
   );  
  
 SET @ID = SCOPE_IDENTITY();  
 if @ArtNo=''  
 begin  
  set @ArtNo = Convert(nvarchar(50), @ID)   
  
  WHILE (SELECT COUNT(*) FROM [Catalog].[Product] WHERE [ArtNo] = @ArtNo) > 0  
  begin  
    SET @ArtNo = @ArtNo + '_A'  
  end  
  
  UPDATE [Catalog].[Product] SET [ArtNo] = @ArtNo WHERE [ProductID] = @ID   
 end  
 Select @ID  
END 

GO--



ALTER PROCEDURE [Catalog].[sp_UpdateProductById]  
	@ProductID int,      
	@ArtNo nvarchar(50),  
	@Name nvarchar(255),  
	@Ratio float,    
	@Discount float,  
	@Weight float,  
	@BriefDescription nvarchar(max),  
	@Description nvarchar(max),  
	@Enabled bit,  
	@Recomended bit,  
	@New bit,  
	@BestSeller bit,  
	@OnSale bit,  
	@BrandID int,  
	@AllowPreOrder bit,  
	@UrlPath nvarchar(150),  
	@Unit nvarchar(50),  
	@ShippingPrice money,  
	@MinAmount float,  
	@MaxAmount float,  
	@Multiplicity float,  
	@HasMultiOffer bit,  
	@SalesNote nvarchar(50),  
	@GoogleProductCategory nvarchar(500),  
	@YandexMarketCategory nvarchar(500),  
	@Gtin nvarchar(50),  
	@Adult bit,
	@Length float,
	@Width float,
	@Height float,
	@CurrencyID int,
	@ActiveView360 bit,
	@ManufacturerWarranty bit,
	@ModifiedBy nvarchar(50),
	@YandexTypePrefix nvarchar(500),
	@YandexModel nvarchar(500),
	@BarCode nvarchar(50),
	@Cbid float,
    @Fee float
	
AS  
BEGIN  
UPDATE [Catalog].[Product]  
 SET [ArtNo] = @ArtNo  
	,[Name] = @Name  
	,[Ratio] = @Ratio  
	,[Discount] = @Discount  
	,[Weight] = @Weight
	,[BriefDescription] = @BriefDescription  
	,[Description] = @Description  
	,[Enabled] = @Enabled  
	,[Recomended] = @Recomended  
	,[New] = @New  
	,[BestSeller] = @BestSeller  
	,[OnSale] = @OnSale  
	,[DateModified] = GETDATE()  
	,[BrandID] = @BrandID  
	,[AllowPreOrder] = @AllowPreOrder  
	,[UrlPath] = @UrlPath  
	,[Unit] = @Unit  
	,[ShippingPrice] = @ShippingPrice  
	,[MinAmount] = @MinAmount  
	,[MaxAmount] = @MaxAmount  
	,[Multiplicity] = @Multiplicity  
	,[HasMultiOffer] = @HasMultiOffer  
	,[SalesNote] = @SalesNote  
	,[GoogleProductCategory]=@GoogleProductCategory  
	,[YandexMarketCategory]=@YandexMarketCategory
	,[Gtin]=@Gtin  
	,[Adult] = @Adult
	,[Length] = @Length
	,[Width] = @Width
	,[Height] = @Height
	,[CurrencyID] = @CurrencyID
	,[ActiveView360] = @ActiveView360
	,[ManufacturerWarranty] = @ManufacturerWarranty
	,[ModifiedBy] = @ModifiedBy
	,[YandexTypePrefix] = @YandexTypePrefix
	,[YandexModel] = @YandexModel
	,[BarCode] = @BarCode
	,[Cbid] = @Cbid
    ,[Fee] = @Fee
WHERE ProductID = @ProductID    
END  


GO--

 

CREATE TABLE [Settings].[SettingsSearch](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](250) NOT NULL,
	[Link] [nvarchar](250) NOT NULL,
	[KeyWords] [nvarchar](1000) NULL,
 CONSTRAINT [PK_SettingsSearch] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_SettingsSearch] UNIQUE NONCLUSTERED 
(
	[Title] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO--
alter table Settings.SettingsSearch
add SortOrder int not null DEFAULT 0

GO--



ALTER PROCEDURE [Catalog].[sp_IsCouponAppliedToProduct]  @CouponID int,  @ProductID int  AS 
BEGIN  
	Declare @cCount int;  
	Declare @pCount int;  
	Declare @Count int;
	
	Set @Count = 0;
	
	SET @cCount = (SELECT Count(CategoryID) From Catalog.CouponCategories Where CouponID=@CouponID)  
	SET @pCount = (SELECT Count(ProductID) From Catalog.CouponProducts Where CouponID=@CouponID)  
	
	IF(@cCount = 0 AND @pCount = 0)  
		 Set @Count = 1 
	ELSE IF(@pCount <> 0)  
	BEGIN   
		SET @Count = (Select Count(ProductID) From Catalog.CouponProducts Where CouponID=@CouponID and ProductID=@ProductID)
	END
	  
	IF(@Count = 0)  
	BEGIN   
		SET @Count = (Select Count(ProductID) 
						From Catalog.CouponCategories 
						INNER JOIN Catalog.ProductCategories on CouponCategories.CategoryID = ProductCategories.CategoryID    
						Where CouponID=@CouponID and ProductID=@ProductID)
	END
	
	Select @Count
END

GO--
if not exists( select * from [Settings].[MailFormatType]  where [MailType]= 'OnUserRegistered')
insert into [Settings].[MailFormatType] ([TypeName],[SortOrder],[Comment],[MailType]) 
values ('Регистрация сотрудника', 260, 
'Письмо сотруднику при регистрации со ссылкой на страницу установки пароля. Доступные переменные:
#STORE_URL# - Ссылка на сайт
#STORE_NAME# - Название магазина
#LINK# - Ссылка на страницу установки пароля
#EMAIL# - Email
#FIRSTNAME# - Имя
#LASTNAME# - Фамилия
#REGDATE# - Дата регистрации',
'OnUserRegistered')
GO--

if not exists( select * from [Settings].[MailFormatType]  where [MailType]= 'OnUserPasswordRepair')
insert into [Settings].[MailFormatType] ([TypeName],[SortOrder],[Comment],[MailType]) 
values ('Восстановление пароля сотрудника', 270, 
'Письмо сотруднику со ссылкой на страницу смены пароля. Доступные переменные:
#STORE_URL# - Ссылка на сайт
#STORE_NAME# - Название магазина
#LINK# - Ссылка на страницу смены пароля
#EMAIL# - Email',
'OnUserPasswordRepair')
GO--


if not exists( select * from [Settings].[MailFormat]  where [MailFormatTypeId]= (Select top(1) [MailFormatTypeID] From [Settings].[MailFormatType] Where [MailType] = 'OnUserRegistered'))
Insert Into [Settings].[MailFormat] ([FormatName],[FormatText],[SortOrder],[Enable],[AddDate],[ModifyDate],[FormatSubject],[MailFormatTypeId])
Values ('Регистрация сотрудника', '<div style="color: #4c4f56; font-family: Arial, Helvetica, sans-serif; font-size: 14px;">
<div class="header" style="border-bottom: 1px solid #ededed; display: table; margin-bottom: 25px; padding-bottom: 25px; width: 100%;">
<div class="logo" style="display: table-cell; text-align: left; vertical-align: middle;">#LOGO#</div>
<div class="phone" style="display: table-cell; text-align: right; vertical-align: middle;">
<div class="tel" style="font-size: 26px; font-weight: bold; line-height: 1; margin-bottom: 5px;">&nbsp;</div>
<div class="inform" style="font-size: 12px;">&nbsp;</div>
</div>
</div>
<p>Вы зарегистрированы на сайте&nbsp;<a href="#STORE_URL#">#STORE_URL#</a>.</p>
<p>Для того чтобы установить пароль, перейдите по следующей ссылке: <a href="#LINK#">#LINK#</a></p>
<div class="data" style="display: table; width: 100%;">
<div class="data-cell" style="display: table-cell; padding: 0; padding-right: 1%; width: 24%;">
<div class="o-title vi" style="font-size: 14px; font-weight: bold; margin: 5px 0;">Указанная информация</div>
<div class="l-row">
<div class="l-name vi cs-light" style="color: #acacac; display: inline-block; margin: 5px 0; padding-right: 15px; width: 90px; vertical-align: middle;">Дата регистрации:</div>
<div class="l-value vi" style="display: inline-block; margin: 5px 0;">#REGDATE#</div>
</div>
<div class="l-row">
<div class="l-name vi cs-light" style="color: #acacac; display: inline-block; margin: 5px 0; padding-right: 15px; width: 90px; vertical-align: middle;">Имя:</div>
<div class="l-value vi" style="display: inline-block; margin: 5px 0;">#FIRSTNAME#</div>
</div>
<div class="l-row">
<div class="l-name vi cs-light" style="color: #acacac; display: inline-block; margin: 5px 0; padding-right: 15px; width: 90px; vertical-align: middle;">Фамилия:</div>
<div class="l-value vi" style="display: inline-block; margin: 5px 0;">#LASTNAME#</div>
</div>
<div class="l-row">
<div class="l-name vi cs-light" style="color: #acacac; display: inline-block; margin: 5px 0; padding-right: 15px; width: 90px; vertical-align: middle;">Email:</div>
<div class="l-value vi" style="display: inline-block; margin: 5px 0;">#EMAIL#</div>
</div>
</div>
</div>
</div>', 1400, 1, getdate(), getdate(), 'Вы зарегистрированы на сайте #STORE_URL#', (Select top(1) [MailFormatTypeID] From [Settings].[MailFormatType] Where [MailType] = 'OnUserRegistered'))
GO--


if not exists( select * from [Settings].[MailFormat]  where [MailFormatTypeId]= (Select top(1) [MailFormatTypeID] From [Settings].[MailFormatType] Where [MailType] = 'OnUserPasswordRepair'))
Insert Into [Settings].[MailFormat] ([FormatName],[FormatText],[SortOrder],[Enable],[AddDate],[ModifyDate],[FormatSubject],[MailFormatTypeId])
Values ('Восстановление пароля сотрудника', '<div style="color: #4c4f56; font-family: Arial, Helvetica, sans-serif; font-size: 14px;">
<div class="header" style="border-bottom: 1px solid #ededed; display: table; margin-bottom: 25px; padding-bottom: 25px; width: 100%;">
<div class="logo" style="display: table-cell; text-align: left; vertical-align: middle;">#LOGO#</div>
<div class="phone" style="display: table-cell; text-align: right; vertical-align: middle;">
<div class="tel" style="font-size: 26px; font-weight: bold; line-height: 1; margin-bottom: 5px;">&nbsp;</div>
<div class="inform" style="font-size: 12px;">&nbsp;</div>
</div>
</div>
<p>Для того чтобы восстановить пароль, перейдите по следующей ссылке: <a href="#LINK#">#LINK#</a></p>
</div>', 1410, 1, getdate(), getdate(), 'Восстановление пароля', (Select top(1) [MailFormatTypeID] From [Settings].[MailFormatType] Where [MailType] = 'OnUserPasswordRepair'))
GO--


IF ((Select Count(*) From [CMS].[StaticBlock] Where [Key] = 'head') <= 0)
begin
	Insert Into [CMS].[StaticBlock] ([Key],[InnerName],[Content],[Added],[Modified],[Enabled]) 
	Values ('head', 'Блок в теге head', '', getdate(), getdate(), 0)
end

GO--

ALTER PROCEDURE [Catalog].[PreCalcProductParams] @productId       INT,
                                                 @ModerateReviews BIT,
												 @OnlyAvailable BIT
AS
     BEGIN
         SET NOCOUNT ON;
         DECLARE @CountPhoto INT;
         DECLARE @Type NVARCHAR(10);
         DECLARE @PhotoId INT;
         DECLARE @MaxAvailable FLOAT;
         DECLARE @VideosAvailable BIT;
         DECLARE @Colors NVARCHAR(MAX);
         DECLARE @NotSamePrices BIT;
         DECLARE @MinPrice FLOAT;
         DECLARE @PriceTemp FLOAT;
         DECLARE @AmountSort BIT;
         DECLARE @OfferId INT;
         DECLARE @Comments INT;
         DECLARE @CategoryId INT;
         DECLARE @Gifts BIT;
         IF NOT EXISTS
         (
             SELECT ProductID
             FROM [Catalog].Product
             WHERE ProductID = @productId
         )
             RETURN;
         SET @Type = 'Product';      
         --@CountPhoto      
         SET @CountPhoto =
         (
             SELECT TOP (1) CASE
                                WHEN
             (
                 SELECT Offer.ColorID
                 FROM [Catalog].[Offer]
                 WHERE [ProductID] = @productId
                       AND main = 1
             ) IS NOT NULL
                                THEN
             (
                 SELECT COUNT(DISTINCT PhotoId)
                 FROM [Catalog].[Photo]
                      INNER JOIN [Catalog].[Offer] ON [Photo].ColorID = Offer.ColorID OR [Photo].ColorID is NULL
                 WHERE [Photo].[ObjId] = Offer.[ProductId]
                       AND [Offer].Main = 1
                       AND TYPE = @Type
                       AND Offer.[ProductId] = @productId
             )
                                ELSE
             (
                 SELECT COUNT(PhotoId)
                 FROM [Catalog].[Photo]
                 WHERE [Photo].[ObjId] = @productId
                       AND TYPE = @Type
             )
                            END
         );      
         --@PhotoId      
         SET @PhotoId =
         (
             SELECT CASE
                        WHEN  



			(
                 SELECT Offer.ColorID
                 FROM [Catalog].[Offer]
                 WHERE [ProductID] = @productId
                       AND main = 1
             ) IS NOT NULL
                        THEN
             (
                 SELECT TOP (1) PhotoId
                 FROM [Catalog].[Photo]
				 INNER JOIN [Catalog].[Offer] ON [Photo].ColorID = Offer.ColorID OR [Photo].ColorID is NULL
                 WHERE([Photo].ColorID = Offer.ColorID
                       OR [Photo].ColorID IS NULL)
                      AND [Photo].[ObjId] = @productId
                      AND Type = @Type
                 ORDER BY [Photo]. main DESC,
                          [Photo].[PhotoSortOrder],
                          [PhotoId]
             )
                        ELSE
             (
                 SELECT TOP (1) PhotoId
                 FROM [Catalog].[Photo]
                 WHERE [Photo].[ObjId] = @productId
                       AND Type = @Type
                 ORDER BY main DESC,
                          [Photo].[PhotoSortOrder],
                          [PhotoId]
             )
             END



         );       
         --VideosAvailable      
         IF
         (
             SELECT COUNT(ProductVideoID)
             FROM [Catalog].[ProductVideo]
             WHERE ProductID = @productId
         ) > 0
             BEGIN
                 SET @VideosAvailable = 1;
             END;
         ELSE
             BEGIN
                 SET @VideosAvailable = 0;
             END;      
      
         --@MaxAvailable      
         SET @MaxAvailable =
         (
             SELECT MAX(Offer.Amount)
             FROM [Catalog].Offer
             WHERE ProductId = @productId
         );   
      
         --AmountSort      
         SET @AmountSort =
         (
             SELECT CASE
                        WHEN @MaxAvailable <= 0
                             OR @MaxAvailable < ISNULL(Product.MinAmount, 0)
                        THEN 0
                        ELSE 1
                    END
             FROM [Catalog].Offer
                  INNER JOIN [Catalog].Product ON Product.ProductId = Offer.ProductId
             WHERE Offer.ProductId = @productId
                   AND main = 1
         );      
         --Colors      
         SET @Colors =
         (
             SELECT [Settings].[ProductColorsToString](@productId)
         );      
      
         --@NotSamePrices      
         IF
         (
             SELECT MAX(price) - MIN(price)
             FROM [Catalog].offer
             WHERE offer.productid = @productId
                   AND price > 0 AND (@OnlyAvailable = 0

                        OR amount > 0)
         ) > 0
             BEGIN
                 SET @NotSamePrices = 1;
             END;
         ELSE
             BEGIN
                 SET @NotSamePrices = 0;
             END;      
  
         --@MinPrice      
         SET @MinPrice =
         (
             SELECT MIN(price)
             FROM CATALOG.offer
             WHERE offer.productid = @productId
                   AND price > 0 AND (@OnlyAvailable = 0

                        OR amount > 0)
         );      
  
         --@OfferId    
         SET @OfferId =
         (
             SELECT OfferID
             FROM CATALOG.offer
             WHERE offer.productid = @productId
                   AND (offer.Main = 1
                        OR offer.Main IS NULL)
         );     
  
  
         --@PriceTemp      
         SET @PriceTemp =
         (
             SELECT(@MinPrice - @MinPrice * [Product].Discount / 100) * CurrencyValue
             FROM Catalog.Product
                  INNER JOIN Catalog.Currency ON Product.Currencyid = Currency.Currencyid
             WHERE Product.Productid = @productId
         );      
  
         --@Comments    
         SET @Comments =
         (
             SELECT COUNT(ReviewId)
             FROM CMS.Review
             WHERE EntityId = @productId
                   AND (Checked = 1
                        OR @ModerateReviews = 0)
         );     
       
         --@Gifts    
         SET @Gifts =
         (
             SELECT TOP (1) CASE
                                WHEN COUNT(ProductID) > 0
                                THEN 1
                                ELSE 0
                            END
             FROM [Catalog].[ProductGifts]
             WHERE ProductID = @productId
         );    
     
         --@CategoryId    
         DECLARE @MainCategoryId INT;
         SET @MainCategoryId =
         (
             SELECT TOP 1 CategoryID
             FROM [Catalog].ProductCategories
             WHERE ProductID = @productId
             ORDER BY Main DESC
         );
         IF @MainCategoryId IS NOT NULL
             BEGIN
                 SET @CategoryId =
                 (
                     SELECT TOP 1 id
                     FROM [Settings].[GetParentsCategoryByChild](@MainCategoryId)
                     ORDER BY sort DESC
                 );
             END;
         IF
         (
             SELECT COUNT(productid)
             FROM [Catalog].ProductExt
             WHERE productid = @productId
         ) > 0
             BEGIN
                 UPDATE [Catalog].[ProductExt]
                   SET
                       [CountPhoto] = @CountPhoto,
                       [PhotoId] = @PhotoId,
                       [VideosAvailable] = @VideosAvailable,
                       [MaxAvailable] = @MaxAvailable,
                       [NotSamePrices] = @NotSamePrices,
                       [MinPrice] = @MinPrice,
                       [Colors] = @Colors,
                       [AmountSort] = @AmountSort,
                       [OfferId] = @OfferId,
                       [Comments] = @Comments,
                       [CategoryId] = @CategoryId,
                       [PriceTemp] = @PriceTemp,
                       [Gifts] = @Gifts
                 WHERE [ProductId] = @productId;
             END;
         ELSE
             BEGIN
                 INSERT INTO [Catalog].[ProductExt]
                 ([ProductId],
                  [CountPhoto],
                  [PhotoId],
                  [VideosAvailable],
                  [MaxAvailable],
                  [NotSamePrices],
                  [MinPrice],
                  [Colors],
                  [AmountSort],
                  [OfferId],
                  [Comments],
                  [CategoryId],
                  [PriceTemp],
                  [Gifts]
                 )
                 VALUES
                 (@productId,
                  @CountPhoto,
                  @PhotoId,
                  @VideosAvailable,
                  @MaxAvailable,
                  @NotSamePrices,
                  @MinPrice,
                  @Colors,
                  @AmountSort,
                  @OfferId,
                  @Comments,
                  @CategoryId,
                  @PriceTemp,
                  @Gifts
                 );
             END;
     END;
GO--

ALTER PROCEDURE [Catalog].[PreCalcProductParamsMass] @ModerateReviews BIT,
                                                     @OnlyAvailable   BIT
AS
     BEGIN
         SET NOCOUNT ON;
         INSERT INTO [Catalog].[ProductExt]
         (ProductId,
          CountPhoto,
          PhotoId,
          VideosAvailable,
          MaxAvailable,
          NotSamePrices,
          MinPrice,
          Colors,
          AmountSort,
          OfferId,
          Comments,
          CategoryId
         )
         (
             SELECT ProductId,
                    0,
                    NULL,
                    0,
                    0,
                    0,
                    0,
                    NULL,
                    0,
                    NULL,
                    0,
                    NULL
             FROM [Catalog].Product
             WHERE Product.ProductId NOT IN
             (
                 SELECT ProductId
                 FROM [Catalog].[ProductExt]
             )
         );
         UPDATE [Catalog].[ProductExt]
           SET
               [CountPhoto] =
         (
             SELECT TOP (1) CASE
                                WHEN
             (
                 SELECT Offer.ColorID
                 FROM [Catalog].[Offer]
                 WHERE [ProductID] = [ProductExt].ProductId
                       AND main = 1
             ) IS NOT NULL
                                THEN
             (
                 SELECT COUNT(PhotoId)
                 FROM [Catalog].[Photo]
                      INNER JOIN [Catalog].[Offer] ON [Photo].ColorID = Offer.ColorID OR [Photo].ColorID is NULL
                 WHERE [Photo].[ObjId] = Offer.[ProductId]
                       AND TYPE = 'Product'
                       AND [Offer].Main = 1
                       AND Offer.[ProductId] = [ProductExt].ProductId
             )
                                ELSE
             (
                 SELECT COUNT(PhotoId)
                 FROM [Catalog].[Photo]
                 WHERE [Photo].[ObjId] = [ProductExt].ProductId
                       AND TYPE = 'Product'
             )
                            END
         ),
               [PhotoId] =
         (
             SELECT TOP (1) CASE
                                WHEN  
			(
                 SELECT Offer.ColorID
                 FROM [Catalog].[Offer]
                 WHERE [ProductID] = [ProductExt].ProductId
                       AND main = 1
             ) IS NOT NULL
                                THEN
             (
                 SELECT TOP (1) PhotoId
                 FROM [Catalog].[Photo]
					INNER JOIN [Catalog].[Offer] ON [Photo].ColorID = Offer.ColorID OR [Photo].ColorID is NULL
                 WHERE([Photo].ColorID = Offer.ColorID
                       OR [Photo].ColorID IS NULL)
                      AND [Photo].[ObjId] = [ProductExt].ProductId
                      AND TYPE = 'Product'
                 ORDER BY [Photo].main DESC,
                          [Photo].[PhotoSortOrder],
                          [PhotoId]
             )
                                ELSE
             (
                 SELECT TOP (1) PhotoId
                 FROM [Catalog].[Photo]
                 WHERE [Photo].[ObjId] = [ProductExt].ProductId
                       AND TYPE = 'Product'
                 ORDER BY main DESC,
                          [Photo].[PhotoSortOrder],
                          [PhotoId]
             )
                            END
         ),
               [VideosAvailable] =
         (
             SELECT TOP (1) CASE
                                WHEN COUNT(ProductVideoID) > 0
                                THEN 1
                                ELSE 0
                            END
             FROM [Catalog].[ProductVideo]
             WHERE ProductID = [ProductExt].ProductId
         ),
               [MaxAvailable] =
         (
             SELECT MAX(Offer.Amount)
             FROM [Catalog].Offer
             WHERE ProductId = [ProductExt].ProductId
         ),
               [NotSamePrices] =
         (
             SELECT TOP (1) CASE
                                WHEN MAX(price) - MIN(price) > 0
                                THEN 1
                                ELSE 0
                            END
             FROM [Catalog].offer
             WHERE offer.productid = [ProductExt].ProductId
                   AND price > 0
                   AND (@OnlyAvailable = 0
                        OR amount > 0)
         ),
               [MinPrice] =
         (
             SELECT MIN(price)
             FROM [Catalog].offer
             WHERE offer.productid = [ProductExt].ProductId
                   AND price > 0
                   AND (@OnlyAvailable = 0
                        OR amount > 0)
         ),
               [PriceTemp] =
         (
             SELECT([MinPrice] - [MinPrice] * [Product].Discount / 100) * CurrencyValue
             FROM catalog.product
                  INNER JOIN catalog.Currency ON product.currencyid = Currency.currencyid
             WHERE product.productid = [ProductExt].ProductId
         ),
               [Colors] =
         (
             SELECT [Settings].[ProductColorsToString]([ProductExt].ProductId)
         ),
               [AmountSort] =
         (
             SELECT TOP (1) CASE
                                WHEN MaxAvailable <= 0
                                     OR MaxAvailable < ISNULL(Product.MinAmount, 0)
                                THEN 0
                                ELSE 1
                            END
             FROM [Catalog].Offer
                  INNER JOIN [Catalog].Product ON Product.ProductId = Offer.ProductId
             WHERE Offer.ProductId = [ProductExt].ProductId
                   AND main = 1
         ),
               [OfferId] =
         (
             SELECT TOP (1) OfferID
             FROM [Catalog].offer
             WHERE offer.productid = [ProductExt].ProductId
                   AND (offer.Main = 1
                        OR offer.Main IS NULL)
         ),
               [Comments] =
         (
             SELECT COUNT(ReviewId)
             FROM CMS.Review
             WHERE EntityId = [ProductExt].ProductId
                   AND (Checked = 1
                        OR @ModerateReviews = 0)
         ),
               [Gifts] =
         (
             SELECT TOP (1) CASE
                                WHEN COUNT(ProductID) > 0
                                THEN 1
                                ELSE 0
                            END
             FROM [Catalog].[ProductGifts]
             WHERE ProductID = [ProductExt].ProductId
         ),    
	     
               -- 1. get main category of product, 2. get root category by main category    
               [CategoryId] =
         (
             SELECT TOP 1 id
             FROM [Settings].[GetParentsCategoryByChild]
             (
             (
                 SELECT TOP 1 CategoryID
                 FROM [Catalog].ProductCategories
                 WHERE ProductID = [ProductExt].ProductId
                 ORDER BY Main DESC
             )
             )
             ORDER BY sort DESC
         );
     END;

GO--


ALTER TABLE Catalog.Property ADD
	NameDisplayed nvarchar(100) NULL

GO--

Update Catalog.Property Set NameDisplayed = Name

GO--


ALTER TABLE Catalog.Property ALTER COLUMN NameDisplayed nvarchar(100) NOT NULL

GO--


ALTER TABLE Catalog.PropertyGroup ADD
	GroupNameDisplayed nvarchar(255) NULL
	
GO--

Update Catalog.PropertyGroup Set GroupNameDisplayed = GroupName

GO--


ALTER TABLE Catalog.PropertyGroup ALTER COLUMN GroupNameDisplayed nvarchar(255) NOT NULL

GO--


ALTER PROCEDURE [Catalog].[sp_UpdateProperty]
	@PropertyID int,
    @Name nvarchar(100),
	@NameDisplayed nvarchar(100),
    @UseInFilter bit,
    @SortOrder int,
    @Expanded bit,
	@UseInDetails bit,
	@Description nvarchar(500),
	@Unit nvarchar(25),
	@Type tinyint,
	@GroupId int,
	@UseInBrief bit
AS

BEGIN
	SET NOCOUNT ON;

UPDATE [Catalog].[Property]
   SET [Name] = @Name
	  ,[NameDisplayed] = @NameDisplayed
      ,[UseInFilter] = @UseInFilter
      ,[SortOrder] = @SortOrder
      ,[Expanded] = @Expanded
	  ,[UseInDetails] = @UseInDetails
	  ,[Description] = @Description
	  ,[Unit] = @Unit
	  ,[Type] = @Type
	  ,[GroupId] = @GroupId
	  ,[UseInBrief] = @UseInBrief
 WHERE [PropertyID] = @PropertyID

END

GO--

ALTER PROCEDURE [Catalog].[sp_AddProperty]
	@Name nvarchar(100),
	@NameDisplayed nvarchar(100),
	@UseInFilter bit = 0, 
	@SortOrder int = 0,
	@Expanded bit = 0,
	@UseInDetails bit = 1,
	@Description nvarchar(500),
	@Unit nvarchar(25),
	@Type tinyint,
	@GroupId int,	
	@UseInBrief bit = 0
AS

BEGIN
	SET NOCOUNT ON;

	INSERT INTO [Catalog].[Property] (Name, NameDisplayed, SortOrder, UseInFilter, Expanded, UseInDetails, Description, Unit, Type, GroupId, UseInBrief) 
	VALUES (@Name, @NameDisplayed, @SortOrder, @UseInFilter, @Expanded, @UseInDetails, @Description, @Unit, @Type, @GroupId, @UseInBrief)

	SELECT SCOPE_IDENTITY()
END

GO--

ALTER PROCEDURE [Catalog].[sp_GetPropertyInFilter] (@categoryid INT, @usedepth bit)
AS
BEGIN
  IF (@usedepth = 1)
  BEGIN
      ;WITH products (propertyvalueid)
           AS
           (
			   SELECT DISTINCT propertyvalueid
			   FROM            [catalog].[productpropertyvalue]
			   WHERE productid IN
				   (
					  SELECT     [productcategories].productid
					  FROM       [catalog].[productcategories]
					  INNER JOIN [catalog].product
					  ON         [productcategories].productid = [product].productid AND [product].[enabled] = 1 
					  where [categoryid] IN (SELECT id FROM [settings].[getchildcategorybyparent] ( @categoryid ))
				   )
           )
		   
		SELECT     [propertyvalue].[propertyvalueid],
				   [propertyvalue].[propertyid],
				   [propertyvalue].[value],
				   [propertyvalue].[rangevalue],
				   [property].Name                 AS propertyName,
				   [property].NameDisplayed        AS propertyNameDisplayed,
				   [property].Sortorder            AS propertySortorder,
				   [property].Expanded             AS propertyExpanded,
				   [property].Unit                 AS propertyUnit,
				   [property].TYPE                 AS propertyType,
				   [property].Description		   AS propertyDescription
				   
		FROM       [catalog].[propertyvalue] 	
		INNER JOIN [catalog].[property]	ON [property].propertyid = [propertyvalue].propertyid AND [property].[useinfilter] = 1				   
		WHERE      [propertyvalue].[useinfilter] = 1
				   AND [propertyvalue].propertyvalueid IN (SELECT propertyvalueid FROM products)
		ORDER BY   propertysortorder,
				   [propertyvalue].[propertyid],
				   [propertyvalue].[sortorder],
				   [propertyvalue].[rangevalue],
				   [propertyvalue].[value]
  END 
  ELSE 
  BEGIN 
  
	;WITH products (propertyvalueid)
		 AS
		 (
			 SELECT DISTINCT propertyvalueid
			 FROM            [catalog].[productpropertyvalue]
			 WHERE productid IN
				 (
					SELECT     [productcategories].productid
					FROM       [catalog].[productcategories]
					INNER JOIN [catalog].product
					ON         [productcategories].productid = [product].productid AND [product].[enabled] = 1 
					where [categoryid] IN (@categoryid)
				 )
		 )
		 
	SELECT     [propertyvalue].[propertyvalueid],
			   [propertyvalue].[propertyid],
			   [propertyvalue].[value],
			   [propertyvalue].[rangevalue],
			   [property].Name                 AS propertyName,
			   [property].NameDisplayed        AS propertyNameDisplayed,
			   [property].Sortorder            AS propertySortorder,
			   [property].Expanded             AS propertyExpanded,
			   [property].Unit                 AS propertyUnit,
			   [property].TYPE                 AS propertyType,
			   [property].Description		   AS propertyDescription
			   
	FROM       [catalog].[propertyvalue]
	INNER JOIN [catalog].[property]	ON [property].propertyid = [propertyvalue].propertyid AND [property].[useinfilter] = 1
	WHERE      [propertyvalue].[useinfilter] = 1 AND 
			   [propertyvalue].propertyvalueid IN (SELECT propertyvalueid FROM products)
	ORDER BY   propertysortorder,
			   [propertyvalue].[propertyid],
			   [propertyvalue].[sortorder],
			   [propertyvalue].[rangevalue],
			   [propertyvalue].[value]			   
  END
END

GO--

ALTER PROCEDURE [Catalog].[sp_GetPropertyValuesByPropertyID] @PropertyID INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [PropertyValueID]
		,[Property].[PropertyID]
		,[Value]
		,[PropertyValue].[SortOrder]
		,[Property].UseinFilter
		,[Property].UseIndetails
		,[Property].UseInBrief
		,Property.Name AS PropertyName
		,Property.NameDisplayed AS PropertyNameDisplayed
		,Property.SortOrder AS PropertySortOrder
		,Property.Expanded
		,Property.[Type]
		,Property.[Description]
		,GroupId
		,GroupName
		,GroupNameDisplayed
		,GroupSortorder
		,unit
	FROM [Catalog].[PropertyValue]
	INNER JOIN [Catalog].[Property] ON [Property].[PropertyID] = [PropertyValue].[PropertyID]
	LEFT JOIN [Catalog].PropertyGroup ON PropertyGroup.PropertyGroupID = [Property].GroupID
	WHERE [Property].[PropertyID] = @PropertyID
	order by [PropertyValue].[SortOrder]
END


GO--

ALTER PROCEDURE [Catalog].[sp_GetPropertyValuesByProductID] @ProductID INT  
AS  
BEGIN  
 SET NOCOUNT ON;  
  
 SELECT  
   [PropertyValue].[PropertyValueID]  
  ,[PropertyValue].[PropertyID]  
  ,[PropertyValue].[Value]  
  ,[PropertyValue].[SortOrder]  
  ,[Property].UseinFilter  
  ,[Property].UseIndetails  
  ,[Property].UseInBrief  
  ,[Property].[Name] as PropertyName  
  ,[Property].[NameDisplayed] AS PropertyNameDisplayed
  ,[Property].[SortOrder] as PropertySortOrder  
  ,[Property].[Expanded] as Expanded  
  ,[Property].[Type] as [Type]  
  ,[Property].GroupId as GroupId  
  ,[Property].[Description] as [Description]
  ,GroupName
  ,GroupNameDisplayed  
  ,GroupSortorder
  ,unit
 FROM [Catalog].[PropertyValue]  
 INNER JOIN [Catalog].[ProductPropertyValue] ON [ProductPropertyValue].[PropertyValueID] = [PropertyValue].[PropertyValueID]  
 inner join [Catalog].[Property] on [Property].[PropertyID] = [PropertyValue].[PropertyID]  
 left join Catalog.PropertyGroup on propertyGroup.PropertyGroupID = [Property].GroupID  
 WHERE [ProductID] = @ProductID  
 ORDER BY case when PropertyGroup.GroupSortOrder is null then 1 else 0 end, 
 PropertyGroup.GroupSortOrder,PropertyGroup.GroupName, [Property].[SortOrder], [Property].Name, [PropertyValue].[SortOrder], [PropertyValue].Value  
END

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
			 [Property].NameDisplayed AS PropertyNameDisplayed,
             [Property].SortOrder  AS PropertySortOrder,
             [Property].Expanded,
             [Property].[Type],
			 [Property].[Description],
             GroupId,
             GroupName,
			 GroupNameDisplayed,
             GroupSortOrder,
			 unit
      FROM   [Catalog].[PropertyValue]
      INNER JOIN [Catalog].[Property] ON [Property].[Propertyid] = [PropertyValue].[PropertyID]
      LEFT JOIN [Catalog].PropertyGroup ON PropertyGroup.PropertyGroupId = [Property].GroupId
      WHERE  [PropertyValue].[PropertyValueId] = @PropertyValueId
  END 

GO--


ALTER TABLE [Customers].[Customer]
   ADD InnerId INT IDENTITY

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
	@Enabled bit,
	@HeadCustomerId uniqueidentifier,
	@BirthDay datetime,
	@City nvarchar(70)
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
		,[Enabled]
		,[HeadCustomerId]
		,[BirthDay]
		,[City])

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
		,@Enabled
		,@HeadCustomerId
		,@BirthDay
		,@City);

	SELECT CustomerID From [Customers].[Customer] Where CustomerId = @CustomerID
END

GO--


ALTER TABLE Catalog.TagMap ADD
	SortOrder int NULL

GO--

Update Catalog.TagMap Set SortOrder = 0

GO--

ALTER TABLE Catalog.TagMap 
	ALTER COLUMN SortOrder int not null
	
GO--


ALTER TABLE Customers.Contact ADD
	Street nvarchar(255) NULL,
	House nvarchar(10) NULL,
	Apartment nvarchar(10) NULL,
	Structure nvarchar(10) NULL,
	Entrance nvarchar(10) NULL,
	Floor nvarchar(10) NULL
GO--

Update [Customers].[Contact] Set Street = Address
GO--

CREATE SCHEMA CRM
GO--

CREATE TABLE [CRM].[BizProcessRule](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EventType] [int] NOT NULL,
	[ObjectType] [int] NOT NULL,
	[EventObjId] [int] NULL,
	[Priority] [int] NOT NULL,
	[TaskName] [nvarchar](255) NOT NULL,
	[TaskDescription] [nvarchar](max) NOT NULL,
	[TaskDueDateInterval] [nvarchar](255) NULL,
	[TaskCreateInterval] [nvarchar](255) NULL,
	[ManagerFilter] [nvarchar](max) NULL,
	[Filter] [nvarchar](max) NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateModified] [datetime] NOT NULL,
 CONSTRAINT [PK_BizProcessRule] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO--

ALTER TABLE [CRM].[BizProcessRule] ADD  CONSTRAINT [DF_BizProcessRule_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO--

ALTER TABLE [CRM].[BizProcessRule] ADD  CONSTRAINT [DF_BizProcessRule_DateModified]  DEFAULT (getdate()) FOR [DateModified]
GO--

ALTER TABLE [Order].Lead ADD
	LastName nvarchar(255) NULL,
	Patronymic nvarchar(255) NULL,
	Description nvarchar(MAX) NULL,
	Sum float(53) NULL
	
GO--

Update [Order].[Lead] Set LastName='', Patronymic='', Sum=0
GO--

EXEC sp_rename N'[Order].Lead.Name', N'FirstName', 'COLUMN' 
GO--

CREATE TABLE [CRM].[DealStatus](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[SortOrder] [int] NOT NULL,
 CONSTRAINT [PK_DealStatus] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO--

If ((Select Count(*) From [CRM].[DealStatus]) = 0)
Begin
	Insert Into [CRM].[DealStatus] ([Name],[SortOrder]) Values ('Новый', 10);
	Insert Into [CRM].[DealStatus] ([Name],[SortOrder]) Values ('Созвон с клиентом', 20);
	Insert Into [CRM].[DealStatus] ([Name],[SortOrder]) Values ('Выставление КП', 30);
	Insert Into [CRM].[DealStatus] ([Name],[SortOrder]) Values ('Ожидание решения клиента', 40);
	Insert Into [CRM].[DealStatus] ([Name],[SortOrder]) Values ('Сделка заключена', 50);
	Insert Into [CRM].[DealStatus] ([Name],[SortOrder]) Values ('Сделка отклонена', 60);
End

GO--

ALTER TABLE [Order].Lead ADD
	DealStatusId int NULL,
	DiscountValue float NULL

GO--

Update [Order].Lead Set DiscountValue = 0 

GO--


ALTER PROCEDURE [Catalog].[sp_GetColorsByCategory]  
 @CategoryID int,  
 @Indepth bit,
 @Type nvarchar(50),
 @OnlyAvailable bit
AS  
BEGIN  
 if(@Indepth = 1)  
 begin  
 
  ;with cte as ( 
	   select distinct ColorID from Catalog.Offer   
	   inner join Catalog.Product on Offer.ProductID = Product.ProductID   
	   inner join Catalog.ProductCategories on ProductCategories.ProductID = Product.ProductID   
	   and ProductCategories.CategoryID in (select id from Settings.GetChildCategoryByParent(@CategoryID)) 
		 and Product.Enabled = 1 and Product.CategoryEnabled=1 and (@OnlyAvailable = 0 OR Amount > 0)
    )
  Select Color.ColorID, ColorName, ColorCode, PhotoId, ObjId, PhotoName, SortOrder from Catalog.Color color
  Left Join Catalog.Photo On Photo.ObjId=Color.ColorId and Type=@type 
  INNER join cte on cte.ColorID = color. ColorID
       order by Color.SortOrder
 end  
 else  
 begin  
  ;with cte as ( 
	   select distinct ColorID from Catalog.Offer   
	   inner join Catalog.Product on Offer.ProductID = Product.ProductID   
	   inner join Catalog.ProductCategories on ProductCategories.ProductID = Product.ProductID   
	   and ProductCategories.CategoryID = @CategoryID and Product.Enabled = 1 and Product.CategoryEnabled=1
	   and (@OnlyAvailable = 0 OR Amount > 0)
    )
  Select Color.ColorID, ColorName, ColorCode, PhotoId, ObjId, PhotoName, SortOrder from Catalog.Color color
  Left Join Catalog.Photo On Photo.ObjId=Color.ColorId and Type=@type 
  INNER join cte on cte.ColorID = color. ColorID
       order by Color.SortOrder
 end  
END

GO--


ALTER PROCEDURE [Catalog].[sp_GetSizesByCategory]
	@CategoryID int,
	@indepth bit,
	@OnlyAvailable bit

AS
BEGIN
	if(@inDepth = 1)
	begin
		Select * from Catalog.Size where SizeID in 
		(select SizeID from Catalog.Offer 
			inner join Catalog.Product on Offer.ProductID=Product.ProductID 
			inner join Catalog.ProductCategories on ProductCategories.ProductID= Product.ProductID 
				and ProductCategories.CategoryID in (select id from Settings.GetChildCategoryByParent(@CategoryID)) 
				where Product.Enabled = 1 and Product.CategoryEnabled=1 and (@OnlyAvailable = 0 OR Amount > 0))
			order by Size.SortOrder
	end
	else
	begin
		Select * from Catalog.Size where SizeID in 
		(select SizeID from Catalog.Offer 
			inner join Catalog.Product on Offer.ProductID=Product.ProductID 
			inner join Catalog.ProductCategories on ProductCategories.ProductID= Product.ProductID 
				and ProductCategories.CategoryID = @CategoryID and 
				Product.Enabled = 1 and Product.CategoryEnabled=1  and (@OnlyAvailable = 0 OR Amount > 0)) 
			order by Size.SortOrder
	end
END


GO--


CREATE TABLE [Order].[LeadEvent](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LeadId] [int] NOT NULL,
	[Type] [int] NOT NULL,
	[Message] [nvarchar](max) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] nvarchar(max) NULL
 CONSTRAINT [PK_LeadEvents] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO--

ALTER TABLE [Order].[LeadEvent]  WITH CHECK ADD  CONSTRAINT [FK_LeadEvents_Lead] FOREIGN KEY([LeadId])
REFERENCES [Order].[Lead] ([Id])
ON DELETE CASCADE
GO--

ALTER TABLE [Order].[LeadEvent] CHECK CONSTRAINT [FK_LeadEvents_Lead]

GO--
-- Begin block 'New role actions'
Update  Customers.CustomerRoleAction Set RoleActionKey = 'Catalog' Where 
RoleActionKey ='DisplayCatalog' or 
RoleActionKey = 'DisplayCatalogDictionaries' or 
RoleActionKey = 'DisplayImportExport' or 
RoleActionKey= 'DisplayMainPageNew' or 
RoleActionKey = 'DisplayMainPageBestsellers' or 
RoleActionKey ='DisplayMainPageDiscount' or 
RoleActionKey = 'DisplayComments' or
RoleActionKey = 'DisplayBrands' or
RoleActionKey = 'DisplayPriceRegulation' or
RoleActionKey = 'EditProductListOnMain' or
RoleActionKey = 'DisplayTag'

Update  Customers.CustomerRoleAction Set RoleActionKey = 'Cms' Where 
RoleActionKey ='DisplayMenus' or 
RoleActionKey = 'DisplayNews' or 
RoleActionKey = 'DisplayCarousel' or 
RoleActionKey= 'DisplayStaticPages' or 
RoleActionKey = 'DisplayStaticBlocks'

Update  Customers.CustomerRoleAction Set RoleActionKey = 'Customers' Where 
RoleActionKey ='DisplayCustomers' or 
RoleActionKey = 'DisplaySubscription'

Update  Customers.CustomerRoleAction Set RoleActionKey = 'Orders' Where 
RoleActionKey ='DisplayOrders' or 
RoleActionKey = 'DisplayOrderStatuses' or
RoleActionKey = 'DisplayAdminMainPageStatistics' or
RoleActionKey = 'DisplaySendPaymentLink'

Update  Customers.CustomerRoleAction Set RoleActionKey = 'Marketing' Where 
RoleActionKey ='DisplayCertificates' or 
RoleActionKey = 'AllowEditDiscounts' or
RoleActionKey = 'AllowEditCoupones' or
RoleActionKey = 'DislayVotes' or
RoleActionKey = 'DisplaySiteMap' or
RoleActionKey = 'DisplaySendMessages' 

Update  Customers.CustomerRoleAction Set RoleActionKey = 'Settings' Where 
RoleActionKey ='DisplayCommonSettings' or 
RoleActionKey = 'DisplayCountries' or
RoleActionKey = 'DisplayCurrencies' or
RoleActionKey = 'DisplayPayments' or
RoleActionKey = 'DisplayShippings' or
RoleActionKey = 'DisplayTaxes' or
RoleActionKey = 'DisplayMailFormats' or
RoleActionKey = 'DisplayLog' or
RoleActionKey = 'DisplayRedirect' or
RoleActionKey = 'DisplayUsers'

Update  Customers.CustomerRoleAction Set RoleActionKey = 'Design' Where 
RoleActionKey ='DisplayDesignTransformer' or 
RoleActionKey = 'DisplayDesignSettings'

Update  Customers.CustomerRoleAction Set RoleActionKey = 'Modules' Where 
RoleActionKey ='DisplayModules'


DECLARE @tempTable TABLE(
    CustomerID uniqueidentifier NOT NULL,
    RoleActionKey nvarchar(50) NOT NULL,
	Enabled bit NOT NULL
);

INSERT INTO @tempTable
SELECT CustomerID, RoleActionKey, [Enabled]
	FROM Customers.CustomerRoleAction 
	Where [Enabled] = 1
	GROUP BY CustomerID, RoleActionKey, [Enabled]
	

DELETE FROM Customers.CustomerRoleAction 

INSERT INTO Customers.CustomerRoleAction  ( CustomerID, RoleActionKey, [Enabled] )
SELECT  CustomerID, RoleActionKey, [Enabled]
FROM    @tempTable 

Delete from @tempTable
-- End block 'New role actions'

GO--


ALTER TABLE Catalog.Tag ADD
	SortOrder int NULL
GO--

Update Catalog.Tag Set SortOrder = 0
GO--

alter table [CRM].[BizProcessRule] add TaskPriority int null
GO--
update [CRM].[BizProcessRule] set TaskPriority = 1
GO--
alter table [CRM].[BizProcessRule] alter column TaskPriority int not null
GO--
alter table [CRM].[BizProcessRule] add TaskGroupId int null
GO--
ALTER TABLE [CRM].[BizProcessRule]  WITH CHECK ADD  CONSTRAINT [FK_BizProcessRule_TaskGroup] FOREIGN KEY([TaskGroupId])
REFERENCES [Customers].[TaskGroup] ([Id])
ON DELETE SET NULL
GO--
ALTER TABLE [CRM].[BizProcessRule] CHECK CONSTRAINT [FK_BizProcessRule_TaskGroup]
GO--

ALTER TABLE Customers.Task ADD 
	DateAppointed datetime NULL,
	IsAutomatic bit NULL,
	IsDeferred bit NULL
GO--

UPDATE Customers.Task SET DateAppointed = DateCreated, IsAutomatic = 0, IsDeferred = 0
GO--
ALTER TABLE Customers.Task ALTER COLUMN DateAppointed datetime NOT NULL
GO--
ALTER TABLE Customers.Task ALTER COLUMN IsAutomatic bit NOT NULL
GO--
ALTER TABLE Customers.Task ALTER COLUMN IsDeferred bit NOT NULL
GO--
ALTER TABLE Customers.Task ADD  CONSTRAINT DF_Task_DateAppointed DEFAULT (getdate()) FOR DateAppointed
GO--


Alter table [Settings].ExportFeedSettings
Add ExportAllProducts bit Null
GO--
Update [Settings].ExportFeedSettings Set ExportAllProducts = 0
GO--
Alter table [Settings].ExportFeedSettings
Alter Column ExportAllProducts bit Not Null
GO--


ALTER TABLE Catalog.Size ALTER COLUMN SizeName nvarchar(300) NOT NULL

GO--

ALTER PROCEDURE [Catalog].[sp_AddSize]
	@SizeName nvarchar(300),
	@SortOrder int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [Catalog].[Size] (SizeName, SortOrder) VALUES (@SizeName, @SortOrder)
	SELECT SCOPE_IDENTITY()
END

GO--


ALTER PROCEDURE [Catalog].[sp_UpdateSize]
	@SizeId int,
	@SizeName nvarchar(300),
	@SortOrder int
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [Catalog].[Size]
	SET SizeName = @SizeName
	   ,SortOrder = @SortOrder
	WHERE SizeID = @SizeId
END

GO--

ALTER TABLE Catalog.Color ALTER COLUMN ColorName nvarchar(300) NOT NULL

GO--

ALTER PROCEDURE [Catalog].[sp_AddColor]
	@ColorName nvarchar(300),
	@ColorCode nchar(7),
	@SortOrder int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [Catalog].[Color] (ColorName, ColorCode, SortOrder) VALUES (@ColorName, @ColorCode, @SortOrder)
	SELECT SCOPE_IDENTITY()
END

GO--

ALTER PROCEDURE [Catalog].[sp_UpdateColor]
	@ColorId int,
	@ColorName nvarchar(300),
	@ColorCode nchar(7),
	@SortOrder int
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [Catalog].[Color]
	SET ColorName = @ColorName
	   ,ColorCode = @ColorCode
	   ,SortOrder = @SortOrder
	WHERE ColorID = @ColorId
END

GO--

ALTER TABLE [Customers].[Call] ADD [Phone] nvarchar(250) NULL
GO--
UPDATE [Customers].[Call] SET Phone = (CASE WHEN [Type] = 'Out' THEN [DstNum] ELSE [SrcNum] END)
GO--
UPDATE [Customers].[Call] SET Phone = '' WHERE Phone IS NULL
GO--
ALTER TABLE [Customers].[Call] ALTER COLUMN [Phone] nvarchar(250) NOT NULL
GO--


ALTER TABLE [Order].LeadEvent ADD
	Title nvarchar(300) NULL
GO--

Update [Order].LeadEvent Set Title = 'Комментарий'
GO--

ALTER TABLE [Order].LeadEvent ALTER COLUMN Title nvarchar(300) NOT NULL
GO--



alter table [Order].[LeadCurrency]
add [RoundNumbers] float null
GO--

update [Order].[LeadCurrency] set RoundNumbers = (
select isnull(RoundNumbers, 0) from Catalog.Currency where [LeadCurrency].[CurrencyCode] = Currency.[CurrencyIso3])
GO--

alter table [Order].[LeadCurrency]
alter column [RoundNumbers] float not null
GO--

alter table [Order].[LeadCurrency]
add [EnablePriceRounding] bit
GO--

update [Order].[LeadCurrency] set [EnablePriceRounding] = (
select isnull([EnablePriceRounding], 0) from Catalog.Currency where [LeadCurrency].[CurrencyCode] = Currency.[CurrencyIso3])
GO--

alter table [Order].[LeadCurrency]
alter column [EnablePriceRounding] bit not null
GO--


if not exists( select * from [Settings].[Settings]  where [Name]= 'FilterVisibility')
Insert Into [Settings].[Settings] ([Name],[Value]) Values ('FilterVisibility', 'True')
GO--


ALTER TABLE [Order].LeadEvent ADD
	TaskId int NULL
GO--

ALTER TABLE [Order].LeadEvent ADD CONSTRAINT FK_LeadEvent_Task FOREIGN KEY
	(TaskId) REFERENCES Customers.Task
	(Id) ON UPDATE  NO ACTION 
	ON DELETE  SET NULL 
GO--


ALTER TABLE [Order].[Order]
	DROP CONSTRAINT FK_Order_Managers
GO--

ALTER TABLE [Order].[Order] ADD CONSTRAINT
	FK_Order_Managers FOREIGN KEY
	(
	ManagerId
	) REFERENCES Customers.Managers
	(
	ManagerId
	) ON UPDATE  NO ACTION 
	 ON DELETE  SET NULL 
GO--

ALTER PROCEDURE [Catalog].[SetCategoryHierarchicallyEnabled] @CatParent INT
AS
     BEGIN
         DECLARE @tbl TABLE
         (Child          INT,
          Parent         INT,
          [Level]        INT,
          [Enabled]      BIT,
          HirecalEnabled BIT
         );
         WITH cteSort
              AS (
              SELECT [Category].CategoryID AS Child,
                     [Category].ParentCategory AS Parent,
                     1 AS [Level],
                     [Category].Enabled,
                     [Category].HirecalEnabled
              FROM [Catalog].[Category]
              WHERE CategoryID = @CatParent
              UNION ALL
              SELECT [Category].CategoryID AS Child,
                     [Category].ParentCategory AS Parent,
                     cteSort.[Level] + 1 AS [Level],
                     [Category].Enabled,
                     [Category].Enabled&cteSort.HirecalEnabled AS HirecalEnabled
              FROM [Catalog].[Category]
                   INNER JOIN cteSort ON [Category].ParentCategory = cteSort.Child
                                         AND [Category].CategoryID <> 0)
              INSERT INTO @tbl
                     SELECT Child,
                            Parent,
                            [Level],
                            [Enabled],
                            HirecalEnabled
                     FROM cteSort;
         UPDATE [Catalog].[Category]
           SET
               HirecalEnabled = temp.HirecalEnabled
         FROM [Catalog].[Category] c
              INNER JOIN @tbl temp ON c.CategoryID = temp.Child;
         UPDATE [Catalog].[Product]
           SET
               CategoryEnabled = c.Enabled&c.HirecalEnabled
         FROM [Catalog].[Product] p
              INNER JOIN [Catalog].ProductCategories pc ON p.ProductID = pc.ProductID
                                                           AND Main = 1
              INNER JOIN [Catalog].[Category] c ON pc.CategoryID = c.CategoryID
              INNER JOIN @tbl temp ON c.CategoryID = temp.Child;
         UPDATE [Catalog].[Product]
           SET
               CategoryEnabled = 0
         FROM [Catalog].[Product] p
              LEFT JOIN [Catalog].ProductCategories pc ON p.ProductID = pc.ProductID
         WHERE pc.CategoryID IS NULL;
     END;
GO--

ALTER PROCEDURE [Catalog].[SetProductHierarchicallyEnabled]  
  @ProductId int  
AS  
BEGIN  
Declare @CountEnabled int  
Set @CountEnabled = (Select Count(CategoryID) 
                       From [Catalog].[Category]   
					  Where [Category].HirecalEnabled=1 and [Category].Enabled=1  and CategoryID in (Select CategoryID From [Catalog].[ProductCategories] Where ProductId=@ProductId and Main=1))  
					  
if (@CountEnabled > 0 )  
 Update [Catalog].[Product] set CategoryEnabled = 1 where ProductId=@ProductId  
else  
 Update [Catalog].[Product] set CategoryEnabled = 0 where ProductId=@ProductId  
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
		SUM(([Sum] - [ShippingCost] - ([Taxcost] - [TaxCost]* ISNULL(OrderTax.TaxShowInPrice, 1)))*CurrencyValue) - SUM([SupplyTotal]) as 'Profit'
		
	FROM [Order].[Order] 
	Inner Join [Order].[OrderCurrency] On [OrderCurrency].[OrderId] = [Order].[OrderId] 
	left join [Order].OrderTax on ordertax.OrderID = [order].OrderID
	WHERE [OrderDate] > @MinDate and [OrderDate] < @MaxDate and [PaymentDate] is not null
	GROUP BY DATEADD(dd, 0, DATEDIFF(dd, 0, [OrderDate]))
END

GO--

ALTER PROCEDURE [Order].[sp_GetProfitByMonths]
	-- Add the parameters for the stored procedure here
	@MinDate datetime,
	@MaxDate datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	select 
	Month([OrderDate]) as 'Month',
	Year([OrderDate]) as 'Year',
	 SUM([Sum]) - SUM([ShippingCost]) - SUM([Taxcost] - [TaxCost] * ISNULL(OrderTax.TaxShowInPrice, 1)) - SUM([SupplyTotal])  as 'Profit'
	FROM [Order].[Order] 
	left join [Order].OrderTax on OrderTax.OrderId = [Order].OrderId
	WHERE [OrderDate] > @MinDate and [OrderDate] < @MaxDate and [PaymentDate] is not null
	GROUP BY Month([OrderDate]) , Year([OrderDate])
END

GO--

ALTER PROCEDURE [Order].[sp_GetProfitPeriods]
AS
BEGIN

	SET NOCOUNT ON;
	-- Temp table
	declare @Cost money
	declare @temp table (
		[NUM] int identity,
		[Count] int,

		[Sum] money,
		[SumWDiscount] money,
		[Cost] money default 0,
		[Tax] money,
		[Shipping] money,
		[ExtraCharge] money default 0
	)
   -- ExtraCharges
   declare @extraCharges table(
		[Value] money default 0,
		[OrderID] int,
		[OrderDate] datetime
   )
	insert into @extraCharges
			select [ParamValue] , [OrderID], [OrderDate]
			from [Order].[Order]
				inner join [Order].[ShippingMethod] 
					on [Order].[ShippingMethodID] = [ShippingMethod].[ShippingMethodID]
				inner join [Order].[ShippingParam] 
					on [ShippingParam].[ShippingMethodID] = [ShippingMethod].[ShippingMethodID] 
			where [ShippingParam].[ParamName] = 'Extracharge' and [ShippingMethod].[ShippingMethodID] = [Order].[ShippingMethodID]
				and [PaymentDate] is not null		
	
	-- Today profit
	insert into @temp 
		select 
		Count(*) as 'Count', 
		sum([Sum]) as 'Sum',
		Sum(case when OrderDiscount = 100 then   ([Sum] - [ShippingCost] - ([TaxCost]  - [TaxCost] * ISNULL(TaxShowInPrice, 1))) 
		else  ([Sum] - [ShippingCost] - ([TaxCost]  - [TaxCost] * ISNULL(TaxShowInPrice, 1)))  * 100 / (100 - OrderDiscount)end ) as 'SumWDiscount',
		SUM([SupplyTotal]) as 'SupplyTotal',
		sum([TaxCost] - [TaxCost] * ISNULL(TaxShowInPrice, 1)) as 'Tax',  
		sum([ShippingCost]) as 'Shipping',
		(select sum([Value]) from @extraCharges where DATEADD(dd, 0, DATEDIFF(dd, 0, [OrderDate])) = DATEADD(dd, 0, DATEDIFF(dd, 0, Getdate())))
	from [Order].[Order]
	left join [Order].[OrderTax] on OrderTax.OrderID = [Order].OrderID
	where [PaymentDate] is not null and DATEADD(dd, 0, DATEDIFF(dd, 0, [OrderDate])) = DATEADD(dd, 0, DATEDIFF(dd, 0, Getdate()))
	
	
	
   -- Yesterday profit
    
    insert into @temp 
		select 
		Count(*) as 'Count', 
		sum([Sum]) as 'Sum', 
		Sum(case when OrderDiscount = 100 then   ([Sum] - [ShippingCost] - ([TaxCost]  - [TaxCost] * ISNULL(TaxShowInPrice, 1))) 
		else  ([Sum] - [ShippingCost] - ([TaxCost]  - [TaxCost] * ISNULL(TaxShowInPrice, 1)))  * 100 / (100 - OrderDiscount)end ) as 'SumWDiscount',
		SUM([SupplyTotal]) as 'SupplyTotal',
		sum([TaxCost] - [TaxCost] * ISNULL(TaxShowInPrice, 1)) as 'Tax',  
		sum([ShippingCost]) as 'Shipping',
		( select sum([Value]) from @extraCharges where DATEADD(dd, 0, DATEDIFF(dd, 0, [OrderDate])) = DATEADD(dd, -1, DATEDIFF(dd, 0, Getdate())) )
	from [Order].[Order]
	left join [Order].[OrderTax] on OrderTax.OrderID = [Order].OrderID 
	where DATEADD(dd, 0, DATEDIFF(dd, 0, [OrderDate])) = DATEADD(dd, -1, DATEDIFF(dd, 0, Getdate()))and [PaymentDate] is not null
	
	
-- Month profit
    
    insert into @temp
		select 
		Count(*) as 'Count', 
		sum([Sum]) as 'Sum', 
		Sum(case when OrderDiscount = 100 then   ([Sum] - [ShippingCost] - ([TaxCost]  - [TaxCost] * ISNULL(TaxShowInPrice, 1))) 
		else  ([Sum] - [ShippingCost] - ([TaxCost]  - [TaxCost] * ISNULL(TaxShowInPrice, 1)))  * 100 / (100 - OrderDiscount)end ) as 'SumWDiscount',
		SUM([SupplyTotal]) as 'SupplyTotal',
		sum([TaxCost] - [TaxCost] * ISNULL(TaxShowInPrice, 1)) as 'Tax',  
		sum([ShippingCost]) as 'Shipping',
		(select sum([Value]) from @extraCharges where Month([OrderDate]) = Month(getdate()) and Year([OrderDate]) = Year(getdate()))
	from [Order].[Order] 
	left join [Order].[OrderTax] on OrderTax.OrderID = [Order].OrderID
	where Month([OrderDate]) = Month(getdate()) and Year([OrderDate]) = Year(getdate())and [PaymentDate] is not null
	
	--Total profit
	
	insert into @temp 
		select 
		Count(*) as 'Count', 
		sum([Sum]) as 'Sum', 
		Sum(case when OrderDiscount = 100 then   ([Sum] - [ShippingCost] - ([TaxCost]  - [TaxCost] * ISNULL(TaxShowInPrice, 1))) 
		else  ([Sum] - [ShippingCost] - ([TaxCost]  - [TaxCost] * ISNULL(TaxShowInPrice, 1)))  * 100 / (100 - OrderDiscount)end ) as 'SumWDiscount',
		SUM([SupplyTotal]) as 'SupplyTotal',
		sum([TaxCost] - [TaxCost] * ISNULL(TaxShowInPrice, 1)) as 'Tax',  
		sum([ShippingCost]) as 'Shipping',
		(select sum([Value]) from @extraCharges)
	from [Order].[Order] 
	left join [Order].[OrderTax] on OrderTax.OrderID = [Order].OrderID
	where [PaymentDate] is not null
	
	update @temp set [ExtraCharge] = 0 where [ExtraCharge] is null
	
	select * from @temp
	
	select 
		[Count], 
		[Sum],
		[SumWDiscount],
		[Cost], 
		[Tax], 
		[Shipping], 
		[Sum] - [Cost] - [Tax] - [Shipping] + [ExtraCharge] as 'Profit',
		Profitability=
		case 
		when [Sum] - [Tax] - [Shipping]=0 then 0 else ( 1 - ( [Cost]/( [Sum] - [Tax] - [Shipping] ) ) )*100 end 
		--([Sum] - [Cost] - [Tax] - [Shipping] + [ExtraCharge])/([Sum] - [Tax] - [Shipping])*100 as 'Profitability'
	from @temp

END

GO--


If (EXISTS (Select * From [Settings].[Settings] Where Name = 'FinalDealStatusId'))
Begin
	Update [Settings].[Settings] Set Value = (Select Top(1) Id From [CRM].[DealStatus] Where Name = 'Сделка заключена') Where Name = 'FinalDealStatusId'
End
Else
Begin
	Insert Into [Settings].[Settings] ([Name],[Value]) Values ('FinalDealStatusId', (Select Top(1) Id From [CRM].[DealStatus] Where Name = 'Сделка заключена'))
End

GO--

If (EXISTS (Select * From [Settings].[Settings] Where Name = 'OrderStatusIdFromLead'))
Begin
	Update [Settings].[Settings] Set Value = (SELECT Top(1)OrderStatusID FROM [Order].OrderStatus Order By SortOrder) Where Name = 'OrderStatusIdFromLead'
End
Else
Begin
	Insert Into [Settings].[Settings] ([Name],[Value]) Values ('OrderStatusIdFromLead', (SELECT Top(1)OrderStatusID FROM [Order].OrderStatus Order By SortOrder))
End

GO--

CREATE SCHEMA Bonus
GO--
CREATE TABLE Bonus.Grade
	(
	Id int NOT NULL IDENTITY (1, 1),
	Name nvarchar(250) NOT NULL,
	BonusPercent money NOT NULL,
	SortOrder int NOT NULL,
	PurchaseBarrier money NOT NULL
	)  ON [PRIMARY]
GO--
ALTER TABLE Bonus.Grade ADD CONSTRAINT
	PK_Grade PRIMARY KEY CLUSTERED 
	(
	Id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO--
CREATE TABLE [Bonus].[CustomRule](
	[RuleType] [int] NOT NULL,
	[Enabled] [bit] NULL,
	[Name] [nvarchar](250) NULL,
	[Params] [nvarchar](max) NULL,
 CONSTRAINT [PK_CustomRule] PRIMARY KEY CLUSTERED 
(
	[RuleType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO--

CREATE TABLE [Bonus].[Card](
	[CardId] [uniqueidentifier] NOT NULL,
	[CardNumber] [bigint] NOT NULL,
	[BonusAmount] [money] NOT NULL,
	[CreateOn] [datetime] NOT NULL,
	[Blocked] [bigint] NOT NULL,
	[GradeId] [int] NOT NULL,
	[DateLastWipeBonus] [datetime] NULL,
 CONSTRAINT [PK_Card] PRIMARY KEY CLUSTERED 
(
	[CardId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO--

ALTER TABLE [Bonus].[Card]  WITH CHECK ADD  CONSTRAINT [FK_Card_Customer] FOREIGN KEY([CardId])
REFERENCES [Customers].[Customer] ([CustomerID])
GO--

ALTER TABLE [Bonus].[Card] CHECK CONSTRAINT [FK_Card_Customer]
GO--

ALTER TABLE [Bonus].[Card]  WITH CHECK ADD  CONSTRAINT [FK_Card_Grade] FOREIGN KEY([GradeId])
REFERENCES [Bonus].[Grade] ([Id])
GO--

ALTER TABLE [Bonus].[Card] CHECK CONSTRAINT [FK_Card_Grade]
GO--
CREATE TABLE [Bonus].[AdditionBonus](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CardId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](250) NULL,
	[Amount] [money] NOT NULL,
	[StartDate] [date] NULL,
	[EndDate] [date] NULL,
	[Description] [nvarchar](500) NULL,
	[Status] [int] NOT NULL,
 CONSTRAINT [PK_AdditionBonus] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO--

ALTER TABLE [Bonus].[AdditionBonus]  WITH CHECK ADD  CONSTRAINT [FK_AdditionBonus_Card] FOREIGN KEY([CardId])
REFERENCES [Bonus].[Card] ([CardId])
GO--

ALTER TABLE [Bonus].[AdditionBonus] CHECK CONSTRAINT [FK_AdditionBonus_Card]
GO--
CREATE TABLE [Bonus].[Purchase](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CardId] [uniqueidentifier] NOT NULL,
	[CreateOn] [datetime] NOT NULL,
	[CreateOnCut] [date] NOT NULL,
	[PurchaseAmount] [money] NOT NULL,
	[CashAmount] [money] NOT NULL,
	[MainBonusAmount] [money] NOT NULL,
	[AdditionBonusAmount] [money] NOT NULL,
	[NewBonusAmount] [money] NOT NULL,
	[Comment] [nvarchar](500) NULL,
	[MainBonusBalance] [money] NOT NULL,
	[AdditionBonusBalance] [money] NOT NULL,
	[Status] [int] NOT NULL,
	[PurchaseFullAmount] [money] NOT NULL,
	[OrderId] [int] NOT NULL,
 CONSTRAINT [PK_Purchase] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO--

ALTER TABLE [Bonus].[Purchase]  WITH CHECK ADD  CONSTRAINT [FK_Purchase_Card] FOREIGN KEY([CardId])
REFERENCES [Bonus].[Card] ([CardId])
GO

ALTER TABLE [Bonus].[Purchase] CHECK CONSTRAINT [FK_Purchase_Card]
GO--

ALTER TABLE [Bonus].[Purchase]  WITH CHECK ADD  CONSTRAINT [FK_Purchase_Order] FOREIGN KEY([OrderId])
REFERENCES [Order].[Order] ([OrderID])
GO--

ALTER TABLE [Bonus].[Purchase] CHECK CONSTRAINT [FK_Purchase_Order]
GO--

CREATE TABLE [Bonus].[Transaction](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CardId] [uniqueidentifier] NOT NULL,
	[Amount] [money] NOT NULL,
	[Basis] [nvarchar](500) NULL,
	[CreateOn] [datetime] NOT NULL,
	[CreateOnCut] [date] NOT NULL,
	[OperationType] [smallint] NOT NULL,
	[Balance] [money] NOT NULL,
	[PurchaseId] [int] NULL,
	[AdditionalBonusId] [int] NULL,
 CONSTRAINT [PK_Transaction] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO--

ALTER TABLE [Bonus].[Transaction]  WITH CHECK ADD  CONSTRAINT [FK_Transaction_AdditionBonus] FOREIGN KEY([AdditionalBonusId])
REFERENCES [Bonus].[AdditionBonus] ([Id])
GO--

ALTER TABLE [Bonus].[Transaction] CHECK CONSTRAINT [FK_Transaction_AdditionBonus]
GO--

ALTER TABLE [Bonus].[Transaction]  WITH CHECK ADD  CONSTRAINT [FK_Transaction_Card] FOREIGN KEY([CardId])
REFERENCES [Bonus].[Card] ([CardId])
GO--

ALTER TABLE [Bonus].[Transaction] CHECK CONSTRAINT [FK_Transaction_Card]
GO--

ALTER TABLE [Bonus].[Transaction]  WITH CHECK ADD  CONSTRAINT [FK_Transaction_Purchase] FOREIGN KEY([PurchaseId])
REFERENCES [Bonus].[Purchase] ([Id])
GO--

ALTER TABLE [Bonus].[Transaction] CHECK CONSTRAINT [FK_Transaction_Purchase]
GO--

CREATE TABLE [Bonus].[PersentHistory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CardId] [uniqueidentifier] NOT NULL,
	[GradeName] [nvarchar](250) NOT NULL,
	[BonusPersent] [money] NOT NULL,
	[CreateOn] [datetime] NOT NULL,
	[ByAction] [nvarchar](250) NULL,
 CONSTRAINT [PK_PersentHistory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO--

ALTER TABLE [Bonus].[PersentHistory]  WITH CHECK ADD  CONSTRAINT [FK_PersentHistory_Card] FOREIGN KEY([CardId])
REFERENCES [Bonus].[Card] ([CardId])
GO--

ALTER TABLE [Bonus].[PersentHistory] CHECK CONSTRAINT [FK_PersentHistory_Card]
GO--

CREATE TABLE [Bonus].[RuleLog](
	[CardId] [uniqueidentifier] NOT NULL,
	[RuleType] [int] NOT NULL,
	[Created] [date] NOT NULL,
 CONSTRAINT [PK_RuleLog] PRIMARY KEY CLUSTERED 
(
	[CardId] ASC,
	[Created] ASC,
	[RuleType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO--

ALTER TABLE Bonus.RuleLog ADD CONSTRAINT FK_RuleLog_Card FOREIGN KEY (CardId) 
REFERENCES Bonus.Card (CardId) 
ON UPDATE  NO ACTION 
ON DELETE  CASCADE 
	
GO--

ALTER TABLE Bonus.RuleLog ADD CONSTRAINT FK_RuleLog_CustomRule FOREIGN KEY (RuleType) 
REFERENCES Bonus.CustomRule (RuleType) 
ON UPDATE  NO ACTION 
ON DELETE  CASCADE 

GO--

CREATE TABLE [Bonus].[SmsTemplate](
	[SmsTypeId] [int] NOT NULL,
	[SmsBody] [nvarchar](500) NULL,
 CONSTRAINT [PK_SmsTemplate] PRIMARY KEY CLUSTERED 
(
	[SmsTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO--

CREATE TABLE [Bonus].[SmsLog](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Body] [nvarchar](max) NULL,
	[State] [nvarchar](max) NULL,
	[Phone] [bigint] NOT NULL,
	[Created] [datetime] NOT NULL,
 CONSTRAINT [PK_SmsLog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO--

Alter Table [Order].[Lead] Alter Column LeadStatus nvarchar(50) null

GO--

ALTER TABLE [Customers].[CustomerField] ADD ShowInClient bit NULL
GO--
UPDATE [Customers].[CustomerField] SET ShowInClient = 0
GO--
ALTER TABLE [Customers].[CustomerField] ALTER COLUMN ShowInClient bit NOT NULL
GO--

ALTER PROCEDURE [Customers].[sp_GetContactIDByContent]
	@Name nvarchar(100),
	@Country nvarchar(100),
	@City nvarchar(100),
	@Zone nvarchar(100),
	@Zip nvarchar(100),
	@Street nvarchar(255),
	@CustomerID uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [ContactID] FROM [Customers].[Contact] 
		WHERE [Name] = @Name 
			and [Country] = @Country 
			and [City] = @City 
			and [Zone] = @Zone
			and [Zip] = @Zip
			and [Street] = @Street
			and [CustomerID] = @CustomerID
END

GO--

if not exists( select * from [Settings].[MailFormatType] where [MailType]= 'OnOrderCommentAdded')
INSERT INTO [Settings].[MailFormatType] ([TypeName],[SortOrder],[Comment],[MailType])
VALUES ('Новый комментарий к заказу', 280, 'Уведомление менеджера заказа о новом комментарии (#AUTHOR#, #AUTHOR_LINK#, #COMMENT#, #ORDER_NUMBER#, #ORDER_LINK#)', 'OnOrderCommentAdded')

GO--

if not exists( select * from [Settings].[MailFormatType] where [MailType]= 'OnCustomerCommentAdded')
INSERT INTO [Settings].[MailFormatType] ([TypeName],[SortOrder],[Comment],[MailType])
VALUES ('Новый комментарий к покупателю', 280, 'Уведомление менеджера покупателя о новом комментарии (#AUTHOR#, #AUTHOR_LINK#, #COMMENT#, #CUSTOMER#, #CUSTOMER_LINK#)', 'OnCustomerCommentAdded')
GO--

if not exists( select * from [Settings].[MailFormat]  where [MailFormatTypeId]= (Select top(1) [MailFormatTypeID] From [Settings].[MailFormatType] Where [MailType] = 'OnOrderCommentAdded'))
INSERT INTO [Settings].[MailFormat] ([FormatName],[FormatText],[SortOrder],[Enable],[AddDate],[ModifyDate],[FormatSubject],[MailFormatTypeId])
VALUES ('Новый комментарий к заказу', '<div style="color: #4c4f56; font-family: Arial, Helvetica, sans-serif; font-size: 14px;">
<div class="header" style="border-bottom: 1px solid #ededed; display: table; margin-bottom: 25px; padding-bottom: 25px; width: 100%;">
<div class="logo" style="display: table-cell; text-align: left; vertical-align: middle;">#LOGO#</div>

<div class="phone" style="display: table-cell; text-align: right; vertical-align: middle;">
<div class="tel" style="font-size: 26px; font-weight: bold; line-height: 1; margin-bottom: 5px;">&nbsp;</div>

<div class="inform" style="font-size: 12px;">&nbsp;</div>
</div>
</div>

<div>
<p><a href="#AUTHOR_LINK#">#AUTHOR#</a> добавил(-а) комментарий к <a href="#ORDER_LINK#">заказу №#ORDER_NUMBER#</a>.</p>

<div class="l-row">
<div class="l-value vi" style="display: inline-block; margin: 5px 0;">#COMMENT#</div>
</div>
</div>
</div>', 1240, 1, getdate(), getdate(), 'Новый комментарий к заказу №#ORDER_NUMBER#',
(Select top(1) [MailFormatTypeID] From [Settings].[MailFormatType] Where [MailType] = 'OnOrderCommentAdded'))
GO--

if not exists( select * from [Settings].[MailFormat]  where [MailFormatTypeId]= (Select top(1) [MailFormatTypeID] From [Settings].[MailFormatType] Where [MailType] = 'OnCustomerCommentAdded'))
INSERT INTO [Settings].[MailFormat] ([FormatName],[FormatText],[SortOrder],[Enable],[AddDate],[ModifyDate],[FormatSubject],[MailFormatTypeId])
VALUES ('Новый комментарий к покупателю', '<div style="color: #4c4f56; font-family: Arial, Helvetica, sans-serif; font-size: 14px;">
<div class="header" style="border-bottom: 1px solid #ededed; display: table; margin-bottom: 25px; padding-bottom: 25px; width: 100%;">
<div class="logo" style="display: table-cell; text-align: left; vertical-align: middle;">#LOGO#</div>

<div class="phone" style="display: table-cell; text-align: right; vertical-align: middle;">
<div class="tel" style="font-size: 26px; font-weight: bold; line-height: 1; margin-bottom: 5px;">&nbsp;</div>

<div class="inform" style="font-size: 12px;">&nbsp;</div>
</div>
</div>

<div>
<p><a href="#AUTHOR_LINK#">#AUTHOR#</a> добавил(-а) комментарий к покупателю <a href="#CUSTOMER_LINK#">#CUSTOMER#</a>.</p>

<div class="l-row">
<div class="l-value vi" style="display: inline-block; margin: 5px 0;">#COMMENT#</div>
</div>
</div>
</div>', 1250, 1, getdate(), getdate(), 'Новый комментарий к покупателю #CUSTOMER#',
(Select top(1) [MailFormatTypeID] From [Settings].[MailFormatType] Where [MailType] = 'OnCustomerCommentAdded'))
GO--

update [Settings].[Settings] set [Value] = replace([Value], 'Продолжить', 'Подтвердить заказ') where  [Name] = 'UserAgreementText'
update [Settings].[Localization] set [ResourceValue] = replace([ResourceValue], 'Регистрация', 'Зарегистрироваться')  where resourcekey='User.Registration.RegisterButton' and [LanguageId] = 1

GO--

ALTER TABLE [Order].[Order] ADD LeadId int NULL
GO--
ALTER TABLE [Order].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_Lead] FOREIGN KEY([LeadId])
REFERENCES [Order].[Lead] ([Id])
ON DELETE SET NULL
GO--

Alter table Customers.Contact 
Alter column House nvarchar(50) NULL
GO--
Alter table Customers.Contact 
Alter column Apartment nvarchar(50) NULL
GO--

ALTER TABLE [Order].[Lead] ADD IsFromAdminArea bit NULL
GO--
UPDATE [Order].[Lead] SET IsFromAdminArea = 0
GO--
ALTER TABLE [Order].[Lead] ADD CONSTRAINT [DF_Lead_IsFromAdminArea]  DEFAULT ((0)) FOR [IsFromAdminArea]
GO--

update [CMS].[StaticBlock] set Content = Replace(Content, '2016', '2017') where [Key] = 'LeftBottom'
update [CMS].[StaticBlock] set Content = Replace(Content, '2015', '2017') where [Key] = 'LeftBottom'
update [CMS].[StaticBlock] set Content = Replace(Content, '2014', '2017') where [Key] = 'LeftBottom'
update [CMS].[StaticBlock] set Content = Replace(Content, 'AdVantShop.NET', 'AdvantShop') where [Key] = 'LeftBottom'

GO--

IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Module' AND  TABLE_NAME = 'LandingPageProductDescription'))
                            BEGIN
                                CREATE TABLE Module.LandingPageProductDescription
                                (
                                ProductId int NOT NULL,
                                Description nvarchar(MAX) NOT NULL
                                )  ON [PRIMARY]
                                 TEXTIMAGE_ON [PRIMARY]

                           ALTER TABLE Module.LandingPageProductDescription ADD CONSTRAINT
                                PK_LandingPageProductDescription PRIMARY KEY CLUSTERED 
                                (
                                ProductId
                                ) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

                           ALTER TABLE Module.LandingPageProductDescription ADD CONSTRAINT
                                FK_LandingPageProductDescription_Product FOREIGN KEY
                                (
                                ProductId
                                ) REFERENCES Catalog.Product
                                (
                                ProductId
                                ) ON UPDATE  NO ACTION 
                                 ON DELETE  CASCADE 
                            END
							
GO--

update [Settings].[ModuleSettings] set [Value] = 'False' where Name = 'Process301Redirect' and ModuleName = 'yandexmarketimport'

GO--

update [Settings].[MailFormatType] set [Comment] = 'При смене статуса заказа ( #ORDERID# ; #ORDERSTATUS#; #STATUSCOMMENT#; #NUMBER#, #ORDERTABLE#, #TRACKNUMBER# )' where MailType = 'OnChangeOrderStatus'

GO--

GO--

alter table [Order].[OrderItems] alter column Name nvarchar(200)

GO--

ALTER PROCEDURE [Order].[sp_AddOrderItem]
	@OrderID int,
	@Name nvarchar(200),
	@Price float,
	@Amount float,
	@ProductID int,
	@ArtNo nvarchar(150),
	@SupplyPrice float,
	@Weight float,
	@IsCouponApplied bit,
	@Color nvarchar(50),
	@Size nvarchar(50),
	@DecrementedAmount float,
	@PhotoID int
	
AS
BEGIN
	
	INSERT INTO [Order].OrderItems
			([OrderID]
			,[ProductID]
			,[Name]
			,[Price]
			,[Amount]
			,[ArtNo]
			,[SupplyPrice]
			,[Weight]
			,[IsCouponApplied]
			,[Color]
			,[Size]
			,[DecrementedAmount]
			,PhotoID
			)
     VALUES
			(@OrderID
			,@ProductID
			,@Name
			,@Price
			,@Amount
			,@ArtNo
			,@SupplyPrice
			,@Weight
			,@IsCouponApplied
			,@Color
			,@Size
			,@DecrementedAmount
			,@PhotoID
			);
     
	SELECT SCOPE_IDENTITY()
END

GO--

ALTER PROCEDURE [Order].[sp_UpdateOrderItem]
	@OrderItemID int,
	@OrderID int,
	@Name nvarchar(200),
	@Price float,
	@Amount float,
	@ProductID int,
	@ArtNo nvarchar(150),
	@SupplyPrice float,
	@Weight float,
	@IsCouponApplied bit,
	@Color nvarchar(50),
	@Size nvarchar(50),
	@DecrementedAmount float,
	@PhotoID int
		
AS
BEGIN
	Update [Order].[OrderItems]
           set
		    [Name] = @Name
           ,[Price] = @Price
           ,[Amount] = @Amount
           ,[ArtNo] = @ArtNo
           ,[SupplyPrice] = @SupplyPrice
           ,[Weight] = @Weight
           ,[IsCouponApplied] = @IsCouponApplied
           ,[Color] = Color
		   ,[Size] = Size
		   ,[DecrementedAmount] = DecrementedAmount
           ,[PhotoID] = @PhotoID
	 WHERE OrderItemID = @OrderItemID
END

GO--

If (((Select Len([Content]) From [CMS].[StaticBlock] Where [Key] = 'CatalogLeft') < 2) and (EXISTS(select * From [CMS].[StaticBlock] where [Key] = 'leftAsideBanners')))
Begin
	Update [CMS].[StaticBlock] set [Content] = (select top(1)Content from CMS.StaticBlock where [Key] = 'leftAsideBanners') where [Key] = 'CatalogLeft'
End

GO--

Delete from [CMS].[StaticBlock] where [Key] = 'leftAsideBanners'

GO--

if not exists( select * from [CMS].[StaticBlock] where [Key]= 'MobileOrderSuccessTop')
INSERT INTO [CMS].[StaticBlock] ([Key],InnerName,Content,Added,Modified,Enabled) 
VALUES ('MobileOrderSuccessTop','Успешное оформление заказа (блок сверху, мобильная версия)', '<div>Ваш заказ</div><div class="checkout-confirm-number">№ #ORDER_ID#</div>',
GETDATE(),GETDATE(),1)

GO--
if(select count(*) from [Bonus].[Grade]) = 0
begin
insert into [Bonus].[Grade] ([Name],[BonusPercent], [SortOrder],[PurchaseBarrier]) values ('Гостевой',3,0,0)
insert into [Bonus].[Grade] ([Name],[BonusPercent], [SortOrder],[PurchaseBarrier]) values ('Бронзовый',5,1,5000)
insert into [Bonus].[Grade] ([Name],[BonusPercent], [SortOrder],[PurchaseBarrier]) values ('Серебряный',7,2,15000)
insert into [Bonus].[Grade] ([Name],[BonusPercent], [SortOrder],[PurchaseBarrier]) values ('Золотой',10,3,25000)
insert into [Bonus].[Grade] ([Name],[BonusPercent], [SortOrder],[PurchaseBarrier]) values ('Платиновый',30,4,50000)
end

GO--

if(select count(*) from [Bonus].[SmsTemplate]) = 0
begin
insert into [Bonus].[SmsTemplate] ([SmsTypeId],[SmsBody]) values (2,'Спасибо за покупку в "#CompanyName#" на сумму #Purchase#. Использовано #UsedBonus# баллов. На Ваш счет начислено #AddBonus# баллов. Ваш баланс: #Balance# баллов.')
insert into [Bonus].[SmsTemplate] ([SmsTypeId],[SmsBody]) values (3,'Вас приветствует "#CompanyName#". Вам начислены бонусы в размере #Bonus#. Ваш баланс: #Balance#  баллов. #Basis#')
insert into [Bonus].[SmsTemplate] ([SmsTypeId],[SmsBody]) values (4,'Вас приветствует "#CompanyName#". У Вас списано бонусов в размере #Bonus#. Ваш баланс: #Balance# баллов.')
insert into [Bonus].[SmsTemplate] ([SmsTypeId],[SmsBody]) values (6,'Ваша карта #CardNumber# в "#CompanyName#" повышена до уровня #GradeName# (#GradePercent# %).Ваш баланс: #Balance# баллов.')
insert into [Bonus].[SmsTemplate] ([SmsTypeId],[SmsBody]) values (8,'Заказ № #Purchase# отменен. Остаток на карте: #Balance# бонусов')
insert into [Bonus].[SmsTemplate] ([SmsTypeId],[SmsBody]) values (10,'Вас приветствует #CompanyName#. Текущий остаток на бонусной карте: #Balance# баллов. Через #DayLeft# дней баллы будет аннулированы. Успейте потратить бонусные баллы в нашей компании.')
END

if not exists( select * from [settings].[settings] where [Name]= 'BonusSystem.DefaultGrade')
begin
insert into settings.settings (Name,Value) values ('BonusSystem.DefaultGrade',Cast( (select id from [Bonus].[Grade] where Name='Гостевой') as nvarchar(max)))
insert into settings.settings (Name,Value) values ('BonusSystem.CardFrom','100000')
insert into settings.settings (Name,Value) values ('BonusSystem.CardTo','999999')
end
GO--

-- все сотрудники - менеджеры
INSERT INTO [Customers].[Managers] ([CustomerId],[DepartmentId],[Position],[Active])
SELECT c.CustomerId, null, null, 1 FROM [Customers].[Customer] as c 
WHERE NOT EXISTS (SELECT ManagerId FROM [Customers].[Managers] WHERE CustomerId = c.CustomerId) AND c.CustomerRole in (50, 100)
GO--

UPDATE [CMS].[Menu] SET [MenuItemUrlPath] = 'myaccount#?tab=commoninf' WHERE [MenuItemUrlPath] = 'myaccount'
   
GO--

Declare @capchaEnabled nvarchar(MAX);
Set @capchaEnabled = (Select Top(1) [Value] FROM [Settings].[Settings] Where Name = 'EnableCheckOrderConfirmCode')

if (not exists(Select * From [Settings].[Settings] Where Name = 'EnableCaptchaInCheckout'))
	Insert into [Settings].[Settings] ([Name],[Value]) Values ('EnableCaptchaInCheckout', @capchaEnabled)

if (not exists(Select * From [Settings].[Settings] Where Name = 'EnableCaptchaInRegistration'))
	Insert into [Settings].[Settings] ([Name],[Value]) Values ('EnableCaptchaInRegistration', @capchaEnabled)

if (not exists(Select * From [Settings].[Settings] Where Name = 'EnableCaptchaInPreOrder'))
	Insert into [Settings].[Settings] ([Name],[Value]) Values ('EnableCaptchaInPreOrder', @capchaEnabled)

if (not exists(Select * From [Settings].[Settings] Where Name = 'EnableCaptchaInGiftCerticate'))
	Insert into [Settings].[Settings] ([Name],[Value]) Values ('EnableCaptchaInGiftCerticate', @capchaEnabled)

if (not exists(Select * From [Settings].[Settings] Where Name = 'EnableCaptchaInFeedback'))
	Insert into [Settings].[Settings] ([Name],[Value]) Values ('EnableCaptchaInFeedback', @capchaEnabled)

if (not exists(Select * From [Settings].[Settings] Where Name = 'CaptchaMode'))
	Insert into [Settings].[Settings] ([Name],[Value]) Values ('CaptchaMode', '1')

if (not exists(Select * From [Settings].[Settings] Where Name = 'CaptchaLength'))
	Insert into [Settings].[Settings] ([Name],[Value]) Values ('CaptchaLength', '0')

GO--


Alter Table [Catalog].[Product] 
Add DiscountAmount float

GO--

Update [Catalog].[Product] Set DiscountAmount = 0

GO--

ALTER PROCEDURE [Catalog].[sp_AddProduct]    
	@ArtNo nvarchar(50) = '',  
	@Name nvarchar(255),     
	@Ratio float,  
	@Discount float,
	@DiscountAmount float,  
	@Weight float,   
	@BriefDescription nvarchar(max),  
	@Description nvarchar(max),  
	@Enabled tinyint,  
	@Recomended bit,  
	@New bit,  
	@BestSeller bit,  
	@OnSale bit,  
	@BrandID int,  
	@AllowPreOrder bit,  
	@UrlPath nvarchar(150),  
	@Unit nvarchar(50),  
	@ShippingPrice float,  
	@MinAmount float,  
	@MaxAmount float,  
	@Multiplicity float,  
	@HasMultiOffer bit,  
	@SalesNote nvarchar(50),  
	@GoogleProductCategory nvarchar(500),  
	@YandexMarketCategory nvarchar(500),  
	@Gtin nvarchar(50),  
	@Adult bit,
	@Length float,
	@Width float,
	@Height float,
	@CurrencyID int,
	@ActiveView360 bit,
	@ManufacturerWarranty bit,
	@ModifiedBy nvarchar(50),
	@YandexTypePrefix nvarchar(500),
	@YandexModel nvarchar(500),
	@BarCode nvarchar(50),
	@Cbid float,
    @Fee float
AS  
BEGIN  
Declare @Id int  
INSERT INTO [Catalog].[Product]  
           ([ArtNo]  
           ,[Name]                     
           ,[Ratio]  
           ,[Discount]
		   ,[DiscountAmount]  
           ,[Weight]  
           ,[BriefDescription]  
           ,[Description]  
           ,[Enabled]  
           ,[DateAdded]  
           ,[DateModified]  
           ,[Recomended]  
           ,[New]  
           ,[BestSeller]  
           ,[OnSale]  
           ,[BrandID]  
           ,[AllowPreOrder]  
		   ,[UrlPath]  
		   ,[Unit]  
		   ,[ShippingPrice]  
		   ,[MinAmount]  
		   ,[MaxAmount]  
		   ,[Multiplicity]  
		   ,[HasMultiOffer]  
		   ,[SalesNote]  
		   ,GoogleProductCategory  
		   ,YandexMarketCategory
		   ,Gtin  
		   ,Adult
		   ,Length
		   ,Width
		   ,Height
		   ,CurrencyID
		   ,ActiveView360
		   ,ManufacturerWarranty
		   ,ModifiedBy
		   ,YandexTypePrefix
		   ,YandexModel
		   ,BarCode
		   ,Cbid
		   ,Fee
          )  
     VALUES  
           (@ArtNo,  
		   @Name,       
		   @Ratio,  
		   @Discount,  
		   @DiscountAmount,
		   @Weight,
		   @BriefDescription,  
		   @Description,  
		   @Enabled,  
		   GETDATE(),  
		   GETDATE(),  
		   @Recomended,  
		   @New,  
		   @BestSeller,  
		   @OnSale,  
		   @BrandID,  
		   @AllowPreOrder,  
		   @UrlPath,  
		   @Unit,  
		   @ShippingPrice,  
		   @MinAmount,  
		   @MaxAmount,  
		   @Multiplicity,  
		   @HasMultiOffer,  
		   @SalesNote,  
		   @GoogleProductCategory,  
		   @YandexMarketCategory,
		   @Gtin,  
		   @Adult,
		   @Length,
		   @Width,
		   @Height,
		   @CurrencyID,
		   @ActiveView360,
		   @ManufacturerWarranty,
		   @ModifiedBy,
		   @YandexTypePrefix,
		   @YandexModel,
		   @BarCode,
		   @Cbid,
		   @Fee
   );  
  
 SET @ID = SCOPE_IDENTITY();  
 if @ArtNo=''  
 begin  
  set @ArtNo = Convert(nvarchar(50), @ID)   
  
  WHILE (SELECT COUNT(*) FROM [Catalog].[Product] WHERE [ArtNo] = @ArtNo) > 0  
  begin  
    SET @ArtNo = @ArtNo + '_A'  
  end  
  
  UPDATE [Catalog].[Product] SET [ArtNo] = @ArtNo WHERE [ProductID] = @ID   
 end  
 Select @ID  
END 

GO--

ALTER PROCEDURE [Catalog].[sp_UpdateProductById]  
	@ProductID int,      
	@ArtNo nvarchar(50),  
	@Name nvarchar(255),  
	@Ratio float,    
	@Discount float,
	@DiscountAmount float,  
	@Weight float,  
	@BriefDescription nvarchar(max),  
	@Description nvarchar(max),  
	@Enabled bit,  
	@Recomended bit,  
	@New bit,  
	@BestSeller bit,  
	@OnSale bit,  
	@BrandID int,  
	@AllowPreOrder bit,  
	@UrlPath nvarchar(150),  
	@Unit nvarchar(50),  
	@ShippingPrice money,  
	@MinAmount float,  
	@MaxAmount float,  
	@Multiplicity float,  
	@HasMultiOffer bit,  
	@SalesNote nvarchar(50),  
	@GoogleProductCategory nvarchar(500),  
	@YandexMarketCategory nvarchar(500),  
	@Gtin nvarchar(50),  
	@Adult bit,
	@Length float,
	@Width float,
	@Height float,
	@CurrencyID int,
	@ActiveView360 bit,
	@ManufacturerWarranty bit,
	@ModifiedBy nvarchar(50),
	@YandexTypePrefix nvarchar(500),
	@YandexModel nvarchar(500),
	@BarCode nvarchar(50),
	@Cbid float,
    @Fee float
	
AS  
BEGIN  
UPDATE [Catalog].[Product]  
 SET [ArtNo] = @ArtNo  
	,[Name] = @Name  
	,[Ratio] = @Ratio  
	,[Discount] = @Discount
	,[DiscountAmount] = @DiscountAmount  
	,[Weight] = @Weight
	,[BriefDescription] = @BriefDescription  
	,[Description] = @Description  
	,[Enabled] = @Enabled  
	,[Recomended] = @Recomended  
	,[New] = @New  
	,[BestSeller] = @BestSeller  
	,[OnSale] = @OnSale  
	,[DateModified] = GETDATE()  
	,[BrandID] = @BrandID  
	,[AllowPreOrder] = @AllowPreOrder  
	,[UrlPath] = @UrlPath  
	,[Unit] = @Unit  
	,[ShippingPrice] = @ShippingPrice  
	,[MinAmount] = @MinAmount  
	,[MaxAmount] = @MaxAmount  
	,[Multiplicity] = @Multiplicity  
	,[HasMultiOffer] = @HasMultiOffer  
	,[SalesNote] = @SalesNote  
	,[GoogleProductCategory]=@GoogleProductCategory  
	,[YandexMarketCategory]=@YandexMarketCategory
	,[Gtin]=@Gtin  
	,[Adult] = @Adult
	,[Length] = @Length
	,[Width] = @Width
	,[Height] = @Height
	,[CurrencyID] = @CurrencyID
	,[ActiveView360] = @ActiveView360
	,[ManufacturerWarranty] = @ManufacturerWarranty
	,[ModifiedBy] = @ModifiedBy
	,[YandexTypePrefix] = @YandexTypePrefix
	,[YandexModel] = @YandexModel
	,[BarCode] = @BarCode
	,[Cbid] = @Cbid
    ,[Fee] = @Fee
WHERE ProductID = @ProductID    
END  

GO--

ALTER PROCEDURE [Customers].[sp_GetRecentlyView] 
 @CustomerId uniqueidentifier,
 @rowsCount int,
 @Type nvarchar(50) 
AS  
BEGIN
	SELECT TOP(@rowsCount) RecentlyViewsData.ProductID, Product.Name, Product.UrlPath, Ratio, PhotoName, 
	[Photo].[Description] as PhotoDescription, Discount, DiscountAmount, MinPrice as BasePrice, CurrencyValue

	FROM [Customers].RecentlyViewsData  

	Inner Join [Catalog].Product ON Product.ProductID = RecentlyViewsData.ProductId 
	Left Join [Catalog].[ProductExt] ON [Product].[ProductID] = [ProductExt].[ProductID] 
	Inner Join Catalog.Currency On Currency.CurrencyID = Product.CurrencyID
	Left Join Catalog.Photo ON Photo.[ObjId] = Product.ProductID and [Type]=@Type AND [Photo].[Main]=1	

	WHERE Product.Enabled = 1 And CategoryEnabled = 1 And RecentlyViewsData.CustomerID = @CustomerId  
	ORDER BY ViewDate Desc
END

GO--

if not exists( select * from [Settings].[Localization] where [ResourceKey]= 'Core.ExportImport.ProductFields.DiscountAmount')
begin
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.ExportImport.ProductFields.DiscountAmount', 'Скидка (число)');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.ExportImport.ProductFields.DiscountAmount', 'Discount (number)'); 
end

Update [Settings].[Localization] Set ResourceValue = 'Скидка (%, процент)' Where ResourceKey = 'Core.ExportImport.ProductFields.Discount' and LanguageId = 1
Update [Settings].[Localization] Set ResourceValue = 'Discount (%, percent)' Where ResourceKey = 'Core.ExportImport.ProductFields.Discount' and LanguageId = 2

GO--

ALTER PROCEDURE [Settings].[sp_GetExportFeedProducts] @exportFeedId int
	,@onlyCount BIT
	,@exportNotAvailable bit
	,@selectedCurrency NVARCHAR(10)

AS
BEGIN
	DECLARE @res TABLE (productId INT PRIMARY KEY CLUSTERED);
	DECLARE @lproduct TABLE (productId INT PRIMARY KEY CLUSTERED);

	INSERT INTO @lproduct
	SELECT [ProductID]
	FROM [Settings].[ExportFeedSelectedProducts]
	WHERE [ExportFeedId] = @exportFeedId;

	DECLARE @lcategory TABLE (CategoryId INT PRIMARY KEY CLUSTERED);
	DECLARE @l TABLE (CategoryId INT PRIMARY KEY CLUSTERED);

	INSERT INTO @l
	SELECT t.CategoryId
	FROM [Settings].[ExportFeedSelectedCategories] AS t
	INNER JOIN CATALOG.Category ON t.CategoryId = Category.CategoryId
	WHERE [ExportFeedId] = @exportFeedId
		AND HirecalEnabled = 1
		AND Enabled = 1

	DECLARE @l1 INT

	SET @l1 = (
			SELECT MIN(CategoryId)
			FROM @l
			);

	WHILE @l1 IS NOT NULL
	BEGIN
		INSERT INTO @lcategory
		SELECT id
		FROM Settings.GetChildCategoryByParent(@l1) AS dt
		INNER JOIN CATALOG.Category ON CategoryId = id
		WHERE dt.id NOT IN (
				SELECT CategoryId
				FROM @lcategory
				)
			AND HirecalEnabled = 1
			AND Enabled = 1

		SET @l1 = (
				SELECT MIN(CategoryId)
				FROM @l
				WHERE CategoryId > @l1
				);
	END;

	IF @onlyCount = 1
	BEGIN
		SELECT COUNT(OfferId)
		FROM (
			(
				SELECT OfferId
				FROM [Catalog].[Product]
				INNER JOIN [Catalog].[Offer] ON [Offer].[ProductID] = [Product].[ProductID]
				INNER JOIN [Catalog].[ProductCategories] ON [ProductCategories].[ProductID] = [Product].[ProductID]
					AND (
						CategoryId IN (
							SELECT CategoryId
							FROM @lcategory
							)
						OR [ProductCategories].[ProductID] IN (
							SELECT productId
							FROM @lproduct
							)
						)
				--LEFT JOIN [Catalog].[Photo] ON [Product].[ProductID] = [Photo].[ObjId] and Type ='Product' AND [Photo].[Main] = 1
				WHERE (
						SELECT TOP (1) [ProductCategories].[CategoryId]
						FROM [Catalog].[ProductCategories]
						INNER JOIN [Catalog].[Category] ON [Category].[CategoryId] = [ProductCategories].[CategoryId]
						WHERE [ProductID] = [Product].[ProductID]
							AND [Enabled] = 1
							AND [Main] = 1
						) = [ProductCategories].[CategoryId]
					AND (Offer.Price > 0 OR @exportNotAvailable = 1)
					AND (
						Offer.Amount > 0
						OR Product.AllowPreOrder = 1
						OR @exportNotAvailable = 1
						)
					AND CategoryEnabled = 1
					AND (Product.Enabled = 1 OR @exportNotAvailable = 1)
				)
			) AS dd
	END
	ELSE
	BEGIN
		DECLARE @defaultCurrencyRatio FLOAT;

		SELECT @defaultCurrencyRatio = CurrencyValue
		FROM [Catalog].[Currency]
		WHERE CurrencyIso3 = @selectedCurrency;

		SELECT [Product].[Enabled]
			,[Product].[ProductID]
			,[Product].[Discount]
			,[Product].[DiscountAmount]
			,AllowPreOrder
			,Amount
			,[ProductCategories].[CategoryId] AS [ParentCategory]
			,([Offer].[Price] / @defaultCurrencyRatio) AS Price
			,ShippingPrice
			,[Product].[Name]
			,[Product].[UrlPath]
			,[Product].[Description]
			,[Product].[BriefDescription]
			,[Product].SalesNote
			,OfferId
			,[Product].ArtNo
			,[Offer].Main
			,[Offer].ColorID
			,ColorName
			,[Offer].SizeID
			,SizeName
			,BrandName
			,CountryName as BrandCountry
			,GoogleProductCategory
			,YandexMarketCategory
			,YandexTypePrefix
			,YandexModel
			,Gtin
			,Adult
			,CurrencyValue
			,[Settings].PhotoToString(Offer.ColorID, Product.ProductId) AS Photos
			,ManufacturerWarranty
			,[Weight]
			,[Product].[Enabled]
			,[Offer].SupplyPrice
			,[Offer].ArtNo AS OfferArtNo
			,[Product].BarCode

		FROM [Catalog].[Product]
		INNER JOIN [Catalog].[Offer] ON [Offer].[ProductID] = [Product].[ProductID]
		INNER JOIN [Catalog].[ProductCategories] ON [ProductCategories].[ProductID] = [Product].[ProductID]
			AND (
				CategoryId IN (
					SELECT CategoryId
					FROM @lcategory
					)
				OR [ProductCategories].[ProductID] IN (
					SELECT productId
					FROM @lproduct
					)
				)
		--LEFT JOIN [Catalog].[Photo] ON [Product].[ProductID] = [Photo].[ObjId] and Type ='Product' AND [Photo].[Main] = 1
		LEFT JOIN [Catalog].[Color] ON [Color].ColorID = [Offer].ColorID
		LEFT JOIN [Catalog].[Size] ON [Size].SizeID = [Offer].SizeID
		LEFT JOIN [Catalog].Brand ON Brand.BrandID = [Product].BrandID
		LEFT JOIN [Customers].Country ON Brand.CountryID = Country.CountryID
		INNER JOIN [Catalog].Currency ON Currency.CurrencyID = [Product].CurrencyID
		WHERE (
				SELECT TOP (1) [ProductCategories].[CategoryId]
				FROM [Catalog].[ProductCategories]
				INNER JOIN [Catalog].[Category] ON [Category].[CategoryId] = [ProductCategories].[CategoryId]
				WHERE [ProductID] = [Product].[ProductID]
					AND [Enabled] = 1
					AND [Main] = 1
				) = [ProductCategories].[CategoryId]
			AND (Offer.Price > 0 OR @exportNotAvailable = 1)
			AND (
				Offer.Amount > 0
				OR Product.AllowPreOrder = 1
				OR @exportNotAvailable = 1
				)
			AND CategoryEnabled = 1
			AND (Product.Enabled = 1 OR @exportNotAvailable = 1)
	END
END

GO--

Delete From Catalog.RelatedCategories Where [CategoryId] not in (Select CategoryId From Catalog.Category) or [RelatedCategoryId] not in (Select CategoryId From Catalog.Category)

GO--

ALTER TABLE Catalog.RelatedCategories ADD CONSTRAINT
	PK_RelatedCategories PRIMARY KEY CLUSTERED 
	(
	CategoryId,
	RelatedCategoryId,
	RelatedType
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO--


Delete From Catalog.RelatedPropertyValues Where [CategoryId] not in (Select CategoryId From Catalog.Category) or [PropertyValueId] not in (Select [PropertyValueID] From Catalog.[PropertyValue])

GO--

ALTER TABLE Catalog.RelatedPropertyValues ADD CONSTRAINT
	PK_RelatedPropertyValues PRIMARY KEY CLUSTERED 
	(
	CategoryId,
	PropertyValueId,
	RelatedType
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO--


Delete From Catalog.RelatedProperties Where [CategoryId] not in (Select CategoryId From Catalog.Category) or [PropertyId] not in (Select [PropertyId] From Catalog.[Property])

GO--

ALTER TABLE Catalog.RelatedProperties ADD CONSTRAINT
	PK_RelatedProperties PRIMARY KEY CLUSTERED 
	(
	CategoryId,
	PropertyId,
	RelatedType
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO--

ALTER TRIGGER [Catalog].[ProductDeleted] ON [Catalog].[Product]
WITH EXECUTE AS CALLER
FOR DELETE
AS
BEGIN
	SET NOCOUNT ON;
	DELETE FROM [SEO].[MetaInfo] WHERE [ObjId] in (select ProductID FROM Deleted) and Type='Product'	
	DELETE FROM [CMS].[Review] WHERE [EntityId] in (select ProductID FROM Deleted) and Type = 0	
	DELETE FROM [Catalog].[RelatedProducts] Where [ProductID] in (select ProductID FROM Deleted) or [LinkedProductID] in (select ProductID FROM Deleted)
END

GO--

ALTER TRIGGER [Catalog].[CategoryDeleted] ON [Catalog].[Category]
WITH EXECUTE AS CALLER
FOR DELETE
AS
BEGIN
	SET NOCOUNT ON;
	DELETE FROM [SEO].[MetaInfo] WHERE [ObjId] in (select CategoryID FROM Deleted) and Type='Category' 
	DELETE FROM [Catalog].[RelatedCategories] WHERE [CategoryId] in (select CategoryID FROM Deleted) or [RelatedCategoryId] in (select CategoryID FROM Deleted)
END

GO--

ALTER TABLE [Order].[OrderItems] ADD IgnoreOrderDiscount bit NULL
GO--

UPDATE [Order].[OrderItems] SET IgnoreOrderDiscount = 0
GO--

ALTER TABLE [Order].[OrderItems] ALTER COLUMN IgnoreOrderDiscount bit NOT NULL
GO--

ALTER PROCEDURE [Order].[sp_AddOrderItem]
	@OrderID int,
	@Name nvarchar(100),
	@Price float,
	@Amount float,
	@ProductID int,
	@ArtNo nvarchar(255),
	@SupplyPrice float,
	@Weight float,
	@IsCouponApplied bit,
	@Color nvarchar(50),
	@Size nvarchar(50),
	@DecrementedAmount float,
	@PhotoID int,
	@IgnoreOrderDiscount bit
AS
BEGIN
	
	INSERT INTO [Order].OrderItems
			([OrderID]
			,[ProductID]
			,[Name]
			,[Price]
			,[Amount]
			,[ArtNo]
			,[SupplyPrice]
			,[Weight]
			,[IsCouponApplied]
			,[Color]
			,[Size]
			,[DecrementedAmount]
			,[PhotoID]
			,[IgnoreOrderDiscount]
			)
     VALUES
			(@OrderID
			,@ProductID
			,@Name
			,@Price
			,@Amount
			,@ArtNo
			,@SupplyPrice
			,@Weight
			,@IsCouponApplied
			,@Color
			,@Size
			,@DecrementedAmount
			,@PhotoID
			,@IgnoreOrderDiscount
			);
     
	SELECT SCOPE_IDENTITY()
END

GO--

ALTER PROCEDURE [Order].[sp_UpdateOrderItem]
	@OrderItemID int,
	@OrderID int,
	@Name nvarchar(100),
	@Price float,
	@Amount float,
	@ProductID int,
	@ArtNo nvarchar(50),
	@SupplyPrice float,
	@Weight float,
	@IsCouponApplied bit,
	@Color nvarchar(50),
	@Size nvarchar(50),
	@DecrementedAmount float,
	@PhotoID int,
	@IgnoreOrderDiscount bit
AS
BEGIN
	Update [Order].[OrderItems]
           set
		    [Name] = @Name
           ,[Price] = @Price
           ,[Amount] = @Amount
           ,[ArtNo] = @ArtNo
           ,[SupplyPrice] = @SupplyPrice
           ,[Weight] = @Weight
           ,[IsCouponApplied] = @IsCouponApplied
           ,[Color] = Color
		   ,[Size] = Size
		   ,[DecrementedAmount] = DecrementedAmount
           ,[PhotoID] = @PhotoID
		   ,[IgnoreOrderDiscount] = @IgnoreOrderDiscount
	 WHERE OrderItemID = @OrderItemID
END

GO--

alter table dbo.saasdata
alter column value nvarchar (max)

GO--

Alter Table Catalog.Tag
Add VisibilityForUsers bit Null

GO--

Update Catalog.Tag Set VisibilityForUsers = 1

GO--

Alter Table Catalog.Tag
Alter Column VisibilityForUsers bit Null

GO--

Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Orders.Order.BonusCost', 'Бонусы'); 
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.Orders.Order.BonusCost', 'Bonuses');

GO--

Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Orders.OrderType.Feedback', 'Отправить сообщение (feedback)'); 
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.Orders.OrderType.Feedback', 'Feedback');

GO--

ALTER TABLE Bonus.Purchase
	DROP CONSTRAINT FK_Purchase_Order
GO--

ALTER TABLE Bonus.Purchase
	DROP CONSTRAINT FK_Purchase_Card
GO--

ALTER TABLE Bonus.[Transaction]
	DROP CONSTRAINT FK_Transaction_Purchase
GO--

ALTER TABLE Bonus.Purchase ADD CONSTRAINT
	FK_Purchase_Card FOREIGN KEY
	(
	CardId
	) REFERENCES Bonus.Card
	(
	CardId
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
GO--

ALTER TABLE Bonus.[Transaction] ADD CONSTRAINT
	FK_Transaction_Purchase FOREIGN KEY
	(
	PurchaseId
	) REFERENCES Bonus.Purchase
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
GO--

DELETE FROM [CRM].[BizProcessRule] WHERE [EventType] = 6

GO--
alter table dbo.SaasData alter column [Value] nvarchar(max)
GO--


ALTER TABLE Catalog.Product ADD
	AccrueBonuses bit NULL
	
GO--

UPDATE Catalog.Product Set AccrueBonuses = 1

GO--

ALTER PROCEDURE [Catalog].[sp_AddProduct]    
	@ArtNo nvarchar(50) = '',  
	@Name nvarchar(255),     
	@Ratio float,  
	@Discount float,
	@DiscountAmount float,  
	@Weight float,   
	@BriefDescription nvarchar(max),  
	@Description nvarchar(max),  
	@Enabled tinyint,  
	@Recomended bit,  
	@New bit,  
	@BestSeller bit,  
	@OnSale bit,  
	@BrandID int,  
	@AllowPreOrder bit,  
	@UrlPath nvarchar(150),  
	@Unit nvarchar(50),  
	@ShippingPrice float,  
	@MinAmount float,  
	@MaxAmount float,  
	@Multiplicity float,  
	@HasMultiOffer bit,  
	@SalesNote nvarchar(50),  
	@GoogleProductCategory nvarchar(500),  
	@YandexMarketCategory nvarchar(500),  
	@Gtin nvarchar(50),  
	@Adult bit,
	@Length float,
	@Width float,
	@Height float,
	@CurrencyID int,
	@ActiveView360 bit,
	@ManufacturerWarranty bit,
	@ModifiedBy nvarchar(50),
	@YandexTypePrefix nvarchar(500),
	@YandexModel nvarchar(500),
	@BarCode nvarchar(50),
	@Cbid float,
    @Fee float,
	@AccrueBonuses bit
AS  
BEGIN  
Declare @Id int  
INSERT INTO [Catalog].[Product]  
           ([ArtNo]  
           ,[Name]                     
           ,[Ratio]  
           ,[Discount]
		   ,[DiscountAmount]  
           ,[Weight]  
           ,[BriefDescription]  
           ,[Description]  
           ,[Enabled]  
           ,[DateAdded]  
           ,[DateModified]  
           ,[Recomended]  
           ,[New]  
           ,[BestSeller]  
           ,[OnSale]  
           ,[BrandID]  
           ,[AllowPreOrder]  
		   ,[UrlPath]  
		   ,[Unit]  
		   ,[ShippingPrice]  
		   ,[MinAmount]  
		   ,[MaxAmount]  
		   ,[Multiplicity]  
		   ,[HasMultiOffer]  
		   ,[SalesNote]  
		   ,GoogleProductCategory  
		   ,YandexMarketCategory
		   ,Gtin  
		   ,Adult
		   ,Length
		   ,Width
		   ,Height
		   ,CurrencyID
		   ,ActiveView360
		   ,ManufacturerWarranty
		   ,ModifiedBy
		   ,YandexTypePrefix
		   ,YandexModel
		   ,BarCode
		   ,Cbid
		   ,Fee
		   ,AccrueBonuses
          )  
     VALUES  
           (@ArtNo,  
		   @Name,       
		   @Ratio,  
		   @Discount,  
		   @DiscountAmount,
		   @Weight,
		   @BriefDescription,  
		   @Description,  
		   @Enabled,  
		   GETDATE(),  
		   GETDATE(),  
		   @Recomended,  
		   @New,  
		   @BestSeller,  
		   @OnSale,  
		   @BrandID,  
		   @AllowPreOrder,  
		   @UrlPath,  
		   @Unit,  
		   @ShippingPrice,  
		   @MinAmount,  
		   @MaxAmount,  
		   @Multiplicity,  
		   @HasMultiOffer,  
		   @SalesNote,  
		   @GoogleProductCategory,  
		   @YandexMarketCategory,
		   @Gtin,  
		   @Adult,
		   @Length,
		   @Width,
		   @Height,
		   @CurrencyID,
		   @ActiveView360,
		   @ManufacturerWarranty,
		   @ModifiedBy,
		   @YandexTypePrefix,
		   @YandexModel,
		   @BarCode,
		   @Cbid,
		   @Fee,
		   @AccrueBonuses
   );  
  
 SET @ID = SCOPE_IDENTITY();  
 if @ArtNo=''  
 begin  
  set @ArtNo = Convert(nvarchar(50), @ID)   
  
  WHILE (SELECT COUNT(*) FROM [Catalog].[Product] WHERE [ArtNo] = @ArtNo) > 0  
  begin  
    SET @ArtNo = @ArtNo + '_A'  
  end  
  
  UPDATE [Catalog].[Product] SET [ArtNo] = @ArtNo WHERE [ProductID] = @ID   
 end  
 Select @ID  
END 

GO--

ALTER PROCEDURE [Catalog].[sp_UpdateProductById]  
	@ProductID int,      
	@ArtNo nvarchar(50),  
	@Name nvarchar(255),  
	@Ratio float,    
	@Discount float,
	@DiscountAmount float,  
	@Weight float,  
	@BriefDescription nvarchar(max),  
	@Description nvarchar(max),  
	@Enabled bit,  
	@Recomended bit,  
	@New bit,  
	@BestSeller bit,  
	@OnSale bit,  
	@BrandID int,  
	@AllowPreOrder bit,  
	@UrlPath nvarchar(150),  
	@Unit nvarchar(50),  
	@ShippingPrice money,  
	@MinAmount float,  
	@MaxAmount float,  
	@Multiplicity float,  
	@HasMultiOffer bit,  
	@SalesNote nvarchar(50),  
	@GoogleProductCategory nvarchar(500),  
	@YandexMarketCategory nvarchar(500),  
	@Gtin nvarchar(50),  
	@Adult bit,
	@Length float,
	@Width float,
	@Height float,
	@CurrencyID int,
	@ActiveView360 bit,
	@ManufacturerWarranty bit,
	@ModifiedBy nvarchar(50),
	@YandexTypePrefix nvarchar(500),
	@YandexModel nvarchar(500),
	@BarCode nvarchar(50),
	@Cbid float,
    @Fee float,
	@AccrueBonuses bit
	
AS  
BEGIN  
UPDATE [Catalog].[Product]  
 SET [ArtNo] = @ArtNo  
	,[Name] = @Name  
	,[Ratio] = @Ratio  
	,[Discount] = @Discount
	,[DiscountAmount] = @DiscountAmount  
	,[Weight] = @Weight
	,[BriefDescription] = @BriefDescription  
	,[Description] = @Description  
	,[Enabled] = @Enabled  
	,[Recomended] = @Recomended  
	,[New] = @New  
	,[BestSeller] = @BestSeller  
	,[OnSale] = @OnSale  
	,[DateModified] = GETDATE()  
	,[BrandID] = @BrandID  
	,[AllowPreOrder] = @AllowPreOrder  
	,[UrlPath] = @UrlPath  
	,[Unit] = @Unit  
	,[ShippingPrice] = @ShippingPrice  
	,[MinAmount] = @MinAmount  
	,[MaxAmount] = @MaxAmount  
	,[Multiplicity] = @Multiplicity  
	,[HasMultiOffer] = @HasMultiOffer  
	,[SalesNote] = @SalesNote  
	,[GoogleProductCategory]=@GoogleProductCategory  
	,[YandexMarketCategory]=@YandexMarketCategory
	,[Gtin]=@Gtin  
	,[Adult] = @Adult
	,[Length] = @Length
	,[Width] = @Width
	,[Height] = @Height
	,[CurrencyID] = @CurrencyID
	,[ActiveView360] = @ActiveView360
	,[ManufacturerWarranty] = @ManufacturerWarranty
	,[ModifiedBy] = @ModifiedBy
	,[YandexTypePrefix] = @YandexTypePrefix
	,[YandexModel] = @YandexModel
	,[BarCode] = @BarCode
	,[Cbid] = @Cbid
    ,[Fee] = @Fee
	,[AccrueBonuses] = @AccrueBonuses
WHERE ProductID = @ProductID    
END  

GO--

ALTER PROCEDURE [Settings].[sp_GetExportFeedProducts] @exportFeedId int
	,@onlyCount BIT
	,@exportNotAvailable bit
	,@selectedCurrency NVARCHAR(10)
AS
BEGIN
	DECLARE @res TABLE (productId INT PRIMARY KEY CLUSTERED);
	DECLARE @lproduct TABLE (productId INT PRIMARY KEY CLUSTERED);




	INSERT INTO @lproduct
	SELECT [ProductID]
	FROM [Settings].[ExportFeedSelectedProducts]
	WHERE [ExportFeedId] = @exportFeedId;

	DECLARE @lcategory TABLE (CategoryId INT PRIMARY KEY CLUSTERED);
	DECLARE @l TABLE (CategoryId INT PRIMARY KEY CLUSTERED);

	INSERT INTO @l
	SELECT t.CategoryId
	FROM [Settings].[ExportFeedSelectedCategories] AS t
	INNER JOIN CATALOG.Category ON t.CategoryId = Category.CategoryId
	WHERE [ExportFeedId] = @exportFeedId
		AND HirecalEnabled = 1
		AND Enabled = 1

	DECLARE @l1 INT

	SET @l1 = (
			SELECT MIN(CategoryId)
			FROM @l
			);

	WHILE @l1 IS NOT NULL
	BEGIN
		INSERT INTO @lcategory
		SELECT id
		FROM Settings.GetChildCategoryByParent(@l1) AS dt
		INNER JOIN CATALOG.Category ON CategoryId = id
		WHERE dt.id NOT IN (
				SELECT CategoryId
				FROM @lcategory
				)
			AND HirecalEnabled = 1
			AND Enabled = 1

		SET @l1 = (
				SELECT MIN(CategoryId)
				FROM @l
				WHERE CategoryId > @l1
				);
	END;

	IF @onlyCount = 1
	BEGIN
		SELECT COUNT(OfferId)
		FROM (
			(
				SELECT OfferId
				FROM [Catalog].[Product]
				INNER JOIN [Catalog].[Offer] ON [Offer].[ProductID] = [Product].[ProductID]
				INNER JOIN [Catalog].[ProductCategories] ON [ProductCategories].[ProductID] = [Product].[ProductID]
					AND (
						CategoryId IN (
							SELECT CategoryId
							FROM @lcategory
							)
						OR [ProductCategories].[ProductID] IN (
							SELECT productId
							FROM @lproduct
							)
						)
				--LEFT JOIN [Catalog].[Photo] ON [Product].[ProductID] = [Photo].[ObjId] and Type ='Product' AND [Photo].[Main] = 1
				WHERE (
						SELECT TOP (1) [ProductCategories].[CategoryId]
						FROM [Catalog].[ProductCategories]
						INNER JOIN [Catalog].[Category] ON [Category].[CategoryId] = [ProductCategories].[CategoryId]
						WHERE [ProductID] = [Product].[ProductID]
							AND [Enabled] = 1
							AND [Main] = 1
						) = [ProductCategories].[CategoryId]
					AND (Offer.Price > 0 OR @exportNotAvailable = 1)
					AND (
						Offer.Amount > 0
						OR Product.AllowPreOrder = 1
						OR @exportNotAvailable = 1
						)
					AND CategoryEnabled = 1
					AND (Product.Enabled = 1 OR @exportNotAvailable = 1)
				)
			) AS dd
	END
	ELSE
	BEGIN
		DECLARE @defaultCurrencyRatio FLOAT;

		SELECT @defaultCurrencyRatio = CurrencyValue
		FROM [Catalog].[Currency]
		WHERE CurrencyIso3 = @selectedCurrency;

		SELECT [Product].[Enabled]
			,[Product].[ProductID]
			,[Product].[Discount]
			,[Product].[DiscountAmount]
			,AllowPreOrder
			,Amount
			,[ProductCategories].[CategoryId] AS [ParentCategory]
			,([Offer].[Price] / @defaultCurrencyRatio) AS Price
			,ShippingPrice
			,[Product].[Name]
			,[Product].[UrlPath]
			,[Product].[Description]
			,[Product].[BriefDescription]
			,[Product].SalesNote
			,OfferId
			,[Product].ArtNo
			,[Offer].Main
			,[Offer].ColorID
			,ColorName
			,[Offer].SizeID
			,SizeName
			,BrandName
			,CountryName as BrandCountry
			,GoogleProductCategory
			,YandexMarketCategory
			,YandexTypePrefix
			,YandexModel
			,Gtin
			,Adult
			,CurrencyValue
			,[Settings].PhotoToString(Offer.ColorID, Product.ProductId) AS Photos
			,ManufacturerWarranty
			,[Weight]
			,[Product].[Enabled]
			,[Offer].SupplyPrice
			,[Offer].ArtNo AS OfferArtNo
			,[Product].BarCode
			,[Product].Cbid
			,[Product].Fee

		FROM [Catalog].[Product]
		INNER JOIN [Catalog].[Offer] ON [Offer].[ProductID] = [Product].[ProductID]
		INNER JOIN [Catalog].[ProductCategories] ON [ProductCategories].[ProductID] = [Product].[ProductID]
			AND (
				CategoryId IN (
					SELECT CategoryId
					FROM @lcategory
					)
				OR [ProductCategories].[ProductID] IN (
					SELECT productId
					FROM @lproduct
					)
				)
		--LEFT JOIN [Catalog].[Photo] ON [Product].[ProductID] = [Photo].[ObjId] and Type ='Product' AND [Photo].[Main] = 1
		LEFT JOIN [Catalog].[Color] ON [Color].ColorID = [Offer].ColorID
		LEFT JOIN [Catalog].[Size] ON [Size].SizeID = [Offer].SizeID
		LEFT JOIN [Catalog].Brand ON Brand.BrandID = [Product].BrandID
		LEFT JOIN [Customers].Country ON Brand.CountryID = Country.CountryID
		INNER JOIN [Catalog].Currency ON Currency.CurrencyID = [Product].CurrencyID
		WHERE (
				SELECT TOP (1) [ProductCategories].[CategoryId]
				FROM [Catalog].[ProductCategories]
				INNER JOIN [Catalog].[Category] ON [Category].[CategoryId] = [ProductCategories].[CategoryId]
				WHERE [ProductID] = [Product].[ProductID]
					AND [Enabled] = 1
					AND [Main] = 1
				) = [ProductCategories].[CategoryId]
			AND (Offer.Price > 0 OR @exportNotAvailable = 1)
			AND (
				Offer.Amount > 0
				OR Product.AllowPreOrder = 1
				OR @exportNotAvailable = 1
				)
			AND CategoryEnabled = 1
			AND (Product.Enabled = 1 OR @exportNotAvailable = 1)
	END
END


GO--
ALTER TABLE [Order].OrderItems ADD
	AccrueBonuses bit NULL
	
GO--

Update [Order].OrderItems Set AccrueBonuses = 1
	
GO--

ALTER TABLE [Order].[OrderItems] ALTER COLUMN AccrueBonuses bit NOT NULL

GO--

ALTER PROCEDURE [Order].[sp_AddOrderItem]
	@OrderID int,
	@Name nvarchar(100),
	@Price float,
	@Amount float,
	@ProductID int,
	@ArtNo nvarchar(255),
	@SupplyPrice float,
	@Weight float,
	@IsCouponApplied bit,
	@Color nvarchar(50),
	@Size nvarchar(50),
	@DecrementedAmount float,
	@PhotoID int,
	@IgnoreOrderDiscount bit,
	@AccrueBonuses bit
AS
BEGIN
	
	INSERT INTO [Order].OrderItems
			([OrderID]
			,[ProductID]
			,[Name]
			,[Price]
			,[Amount]
			,[ArtNo]
			,[SupplyPrice]
			,[Weight]
			,[IsCouponApplied]
			,[Color]
			,[Size]
			,[DecrementedAmount]
			,[PhotoID]
			,[IgnoreOrderDiscount]
			,[AccrueBonuses]
			)
     VALUES
			(@OrderID
			,@ProductID
			,@Name
			,@Price
			,@Amount
			,@ArtNo
			,@SupplyPrice
			,@Weight
			,@IsCouponApplied
			,@Color
			,@Size
			,@DecrementedAmount
			,@PhotoID
			,@IgnoreOrderDiscount
			,@AccrueBonuses
			);
     
	SELECT SCOPE_IDENTITY()
END

GO--

ALTER PROCEDURE [Order].[sp_UpdateOrderItem]
	@OrderItemID int,
	@OrderID int,
	@Name nvarchar(100),
	@Price float,
	@Amount float,
	@ProductID int,
	@ArtNo nvarchar(50),
	@SupplyPrice float,
	@Weight float,
	@IsCouponApplied bit,
	@Color nvarchar(50),
	@Size nvarchar(50),
	@DecrementedAmount float,
	@PhotoID int,
	@IgnoreOrderDiscount bit,
	@AccrueBonuses bit
AS
BEGIN
	Update [Order].[OrderItems]
           set
		    [Name] = @Name
           ,[Price] = @Price
           ,[Amount] = @Amount
           ,[ArtNo] = @ArtNo
           ,[SupplyPrice] = @SupplyPrice
           ,[Weight] = @Weight
           ,[IsCouponApplied] = @IsCouponApplied
           ,[Color] = Color
		   ,[Size] = Size
		   ,[DecrementedAmount] = DecrementedAmount
           ,[PhotoID] = @PhotoID
		   ,[IgnoreOrderDiscount] = @IgnoreOrderDiscount
		   ,[AccrueBonuses] = @AccrueBonuses
	 WHERE OrderItemID = @OrderItemID
END

GO--

ALTER TABLE Customers.Task ADD ReviewId int NULL
GO--

ALTER TABLE Customers.Task WITH CHECK ADD CONSTRAINT FK_Task_Review FOREIGN KEY(ReviewId)
REFERENCES CMS.Review (ReviewId)
ON DELETE SET NULL
GO--

ALTER TABLE Customers.Task CHECK CONSTRAINT FK_Task_Review
GO--

ALTER PROCEDURE [Catalog].[sp_DeleteCategoryWithSubCategoies]
	@id int
AS
BEGIN
DECLARE @Hierarchycte TABLE (CategoryID int);
WITH Hierarchycte (CategoryID) AS	
(
	SELECT CategoryID	FROM Catalog.category WHERE CategoryID = @id
		union ALL	
	SELECT category.CategoryID FROM Catalog.category	
									INNER JOIN hierarchycte	ON category.ParentCategory = hierarchycte.CategoryID
									where category.CategoryID <>@id
									) 
insert into @Hierarchycte SELECT CategoryID	FROM Hierarchycte
SELECT CategoryID FROM @Hierarchycte where CategoryID <> 0
DELETE [Catalog].[Category] WHERE CategoryID IN (SELECT CategoryID FROM @Hierarchycte where CategoryID <> 0)

UPDATE pc
  SET
      main = 1
FROM catalog.ProductCategories pc
     INNER JOIN
(
    SELECT ProductId, min(CategoryID) as CategoryID
    FROM catalog.ProductCategories AS pc
    GROUP BY ProductID
    HAVING SUM(main*1) = 0
) iddata ON pc.ProductID = iddata.ProductID and pc.CategoryID = iddata.CategoryID

END

GO--

if not exists (select * from settings.settings where name = 'IsShowFullAddress')
insert into settings.settings (name, value) values ('IsShowFullAddress', 'False')

GO--
ALTER PROCEDURE [Catalog].[PreCalcProductParamsMass] @ModerateReviews BIT
	,@OnlyAvailable BIT
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [Catalog].[ProductExt] (
		ProductId
		,CountPhoto
		,PhotoId
		,VideosAvailable
		,MaxAvailable
		,NotSamePrices
		,MinPrice
		,Colors
		,AmountSort
		,OfferId
		,Comments
		,CategoryId
		) (
		SELECT ProductId
		,0
		,NULL
		,0
		,0
		,0
		,0
		,NULL
		,0
		,NULL
		,0
		,NULL FROM [Catalog].Product WHERE Product.ProductId NOT IN (
			SELECT ProductId
			FROM [Catalog].[ProductExt]
			)
		);

	UPDATE [Catalog].[ProductExt]
	SET [CountPhoto] = (
			SELECT TOP (1) CASE 
					WHEN Offer.ColorID IS NOT NULL
						THEN (
								SELECT Count(PhotoId)
								FROM [Catalog].[Photo]
								WHERE (
										[Photo].ColorID = Offer.ColorID
										OR [Photo].ColorID IS NULL
										)
									AND [Photo].[ObjId] = [ProductExt].ProductId
									AND TYPE = 'Product'
								)
					ELSE (
							SELECT Count(PhotoId)
							FROM [Catalog].[Photo]
							WHERE [Photo].[ObjId] = [ProductExt].ProductId
								AND TYPE = 'Product'
							)
					END
			FROM [Catalog].[Offer]
			WHERE [ProductID] = [ProductExt].ProductId
				AND main = 1
			)
		,[PhotoId] = (
			SELECT TOP (1) CASE 
					WHEN Offer.ColorID IS NOT NULL
						AND amount <> 0
						AND price > 0
						THEN (
								SELECT TOP (1) PhotoId
								FROM [Catalog].[Photo]
								WHERE (
										[Photo].ColorID = Offer.ColorID
										OR [Photo].ColorID IS NULL
										)
									AND [Photo].[ObjId] = [ProductExt].ProductId
									AND TYPE = 'Product'
								ORDER BY main DESC
									,[Photo].[PhotoSortOrder]
									,[PhotoId]
								)
					ELSE (
							SELECT TOP (1) PhotoId
							FROM [Catalog].[Photo]
							WHERE [Photo].[ObjId] = [ProductExt].ProductId
								AND TYPE = 'Product'
							ORDER BY main DESC
								,[Photo].[PhotoSortOrder]
								,[PhotoId]
							)
					END
			FROM [Catalog].[Offer]
			WHERE [ProductID] = [ProductExt].ProductId
				AND main = 1
			)
		,[VideosAvailable] = (
			SELECT TOP (1) CASE 
					WHEN COUNT(ProductVideoID) > 0
						THEN 1
					ELSE 0
					END
			FROM [Catalog].[ProductVideo]
			WHERE ProductID = [ProductExt].ProductId
			)
		,[MaxAvailable] = (
			SELECT MAX(Offer.Amount)
			FROM [Catalog].Offer
			WHERE ProductId = [ProductExt].ProductId
			)
		,[NotSamePrices] = (
			SELECT TOP (1) CASE 
					WHEN MAX(price) - MIN(price) > 0
						THEN 1
					ELSE 0
					END
			FROM [Catalog].offer
			WHERE offer.productid = [ProductExt].ProductId
				AND price > 0
				AND (
					@OnlyAvailable = 0
					OR amount > 0
					)
			)
		,[MinPrice] = (
			SELECT MIN(price)
			FROM [Catalog].offer
			WHERE offer.productid = [ProductExt].ProductId
				AND price > 0
				AND (
					@OnlyAvailable = 0
					OR amount > 0
					)
			)
		,[Colors] = (
			SELECT [Settings].[ProductColorsToString]([ProductExt].ProductId)
			)
		,[AmountSort] = (
			SELECT TOP (1) CASE 
					WHEN MaxAvailable <= 0
						OR MaxAvailable < ISNULL(Product.MinAmount, 0)
						THEN 0
					ELSE 1
					END
			FROM [Catalog].Offer
			INNER JOIN [Catalog].Product ON Product.ProductId = Offer.ProductId
			WHERE Offer.ProductId = [ProductExt].ProductId
				AND main = 1
			)
		,[OfferId] = (
			SELECT TOP (1) OfferID
			FROM [Catalog].offer
			WHERE offer.productid = [ProductExt].ProductId
				AND (
					offer.Main = 1
					OR offer.Main IS NULL
					)
			)
		,[Comments] = (
			SELECT COUNT(ReviewId)
			FROM CMS.Review
			WHERE EntityId = [ProductExt].ProductId
				AND (
					Checked = 1
					OR @ModerateReviews = 0
					)
			)
		,[Gifts] = (
			SELECT TOP (1) CASE 
					WHEN COUNT(ProductID) > 0
						THEN 1
					ELSE 0
					END
			FROM [Catalog].[ProductGifts]
			WHERE ProductID = [ProductExt].ProductId
			)
		,
		-- 1. get main category of product, 2. get root category by main category    
		[CategoryId] = (
			SELECT TOP 1 id
			FROM [Settings].[GetParentsCategoryByChild]((
						SELECT TOP 1 CategoryID
						FROM [Catalog].ProductCategories
						WHERE ProductID = [ProductExt].ProductId
						ORDER BY Main DESC
						))
			ORDER BY sort DESC
			);

	UPDATE [Catalog].[ProductExt]
	SET [PriceTemp] = (
			SELECT ([MinPrice] - [MinPrice] * [Product].Discount / 100) * CurrencyValue
			FROM CATALOG.product
			INNER JOIN CATALOG.Currency ON product.currencyid = Currency.currencyid
			WHERE product.productid = [ProductExt].ProductId
			)
END;
GO--

ALTER TABLE Bonus.Card
	DROP CONSTRAINT FK_Card_Customer
GO--

ALTER TABLE Bonus.Card ADD CONSTRAINT
	FK_Card_Customer FOREIGN KEY
	(
	CardId
	) REFERENCES Customers.Customer
	(
	CustomerID
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
GO--

ALTER TABLE Bonus.AdditionBonus
	DROP CONSTRAINT FK_AdditionBonus_Card
GO--

ALTER TABLE Bonus.AdditionBonus ADD CONSTRAINT
	FK_AdditionBonus_Card FOREIGN KEY
	(
	CardId
	) REFERENCES Bonus.Card
	(
	CardId
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
GO--

ALTER TABLE Bonus.PersentHistory
	DROP CONSTRAINT FK_PersentHistory_Card
GO--

ALTER TABLE Bonus.PersentHistory ADD CONSTRAINT
	FK_PersentHistory_Card FOREIGN KEY
	(
	CardId
	) REFERENCES Bonus.Card
	(
	CardId
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
GO--

ALTER TABLE [Customers].[Call] ADD IsComplete bit NULL
GO--
UPDATE [Customers].[Call] SET IsComplete = 1
GO--
ALTER TABLE [Customers].[Call] ALTER COLUMN IsComplete bit NOT NULL
GO--
ALTER TABLE [Customers].[Call] ADD  CONSTRAINT [DF_Call_IsComplete]  DEFAULT ((0)) FOR [IsComplete]
GO--


CREATE TABLE Customers.VkUser
	(
	Id bigint NOT NULL,
	CustomerId uniqueidentifier NOT NULL,
	FirstName nvarchar(MAX) NULL,
	LastName nvarchar(MAX) NULL,
	BirthDate nvarchar(255) NULL,
	Photo100 nvarchar(MAX) NULL,
	MobilePhone nvarchar(255) NULL,
	HomePhone nvarchar(255) NULL,
	Sex nvarchar(10) NULL,
	ScreenName nvarchar(255) NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO--

CREATE UNIQUE NONCLUSTERED INDEX IX_VkUser ON Customers.VkUser
	(
	Id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO--

ALTER TABLE Customers.VkUser ADD CONSTRAINT
	FK_VkUser_Customer FOREIGN KEY
	(
	CustomerId
	) REFERENCES Customers.Customer
	(
	CustomerID
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
GO--

CREATE TABLE Customers.VkMessage
	(
	Id int NOT NULL IDENTITY (1, 1),
	MessageId bigint NOT NULL,
	UserId bigint NOT NULL,
	Date datetime NOT NULL,
	Body nvarchar(MAX) NULL,
	ChatId bigint NULL,
	FromId bigint NULL,
	Type nvarchar(50) NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO--

ALTER TABLE Customers.VkMessage ADD CONSTRAINT
	PK_VkMessage PRIMARY KEY CLUSTERED 
	(
	Id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO--

ALTER TRIGGER [Customers].[CustomerDeleted] ON [Customers].[Customer]
WITH EXECUTE AS CALLER
FOR DELETE
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE [Customers].[Customer] SET [HeadCustomerId] = null WHERE [HeadCustomerId] in (SELECT [CustomerID] FROM Deleted)
	DELETE FROM [Order].[Lead] Where [Lead].[CustomerId] in (SELECT [CustomerID] FROM Deleted)
END

GO--

IF not EXISTS( SELECT * FROM [information_schema].[columns] WHERE table_name = 'ProductList' 
AND column_name = 'Description')
begin
	alter table Catalog.ProductList add Description nvarchar(max) null
end

GO--

delete from  [Settings].[SettingsSearch]
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('1C', 'settingsapi', '1c, 1с, API', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('301 редирект', 'settingsseo#?seoTab=seo301', 'Redirect permanent', '200')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('404 страницы', 'settingsseo#?seoTab=seo404', '', '20')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('API', 'settingsapi', '', '30')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Favicon', 'settings', 'фавикон, Favicon', '100')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Google Analytics', 'settingsseo#?seoTab=seoGA', 'ga', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Google Merchant Center', 'exportfeeds/indexgoogle', 'Google Merchant Center', '20')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Google Tag Manager', 'settingsseo#?seoTab=seoGA', '', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('IP Телефония', 'settingstelephony', 'telphin, телфин, sipuni, сипуни, mango, манго телеком, zadarma, задарма', '30')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Open Graph', 'settingsseo#?seoTab=seoOpenGraph', '', '40')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('SEO параметры', 'settingsseo', '', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Авторизация', '/settingssystem#?systemTab=auth', 'Open ID, oauth', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Аналитика', 'analytics', 'Отчеты, статистика', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Аналитика по заказам', 'analytics/exportorders', '', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Аналитика по покупателям', 'analytics/exportcustomers', '', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Аналитика по товарам', 'analytics/exportproducts', '', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Бонусная система', 'settingsbonus', 'грейд, карта, бонусы, скидка', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Валюты', 'settingscatalog#?catalogTab=currency', 'Рубль, доллар, евро, курс', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Голосование', 'voting', 'Опрос', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Грейды', 'grades', 'бонус', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Группы покупателей', 'customergroups', 'Оптовик, Дилер, Обычный покупатель, Постоянный клиент, Минимальная сумма заказа', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Дизайн', 'design', 'Шаблон, тема, фон, цвет, цветовая схема, настройки шаблона, css', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Дополнительные поля покупателя', 'settingscustomers', '', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Дропшипперы', 'exportfeeds', 'Реселлеры, продавцы', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Журнал вызовов', 'calls', 'ip телефония, звонки', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Задачи', 'tasks', 'задачи', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Заказы', 'orders', 'Покупка, черновик', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Импорт категорий', 'import/importcategories', 'Загрузка', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Импорт покупателей', 'import/importcustomers', '', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Импорт товаров', 'import/importproducts', 'Загрузка, csv, Excel', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Интеграция с Freshdesk', 'settingscustomers#?tab=freshdesk', '', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Интеграция с ВКонтакте', 'settingscustomers#?tab=vk', 'vk, вконтакте', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Источники заказов', 'ordersources', '', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Карта сайта HTML', 'settingssystem#?systemTab=sitemap', '', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Карта сайта XML', 'settingssystem#?systemTab=sitemap', 'sitemap', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Карусель', 'carousel', 'Слайд, Слайдер, Баннер', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Каталог', 'catalog', 'Товары, категории', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Категории новостей', 'newscategory', '', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Купоны', 'coupons', 'код, промокод,', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Лиды', 'leads', '', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Логотип', 'settings', '', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Локализация', 'settingssystem#?systemTab=localization', '', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Меню', 'menus', 'Главное меню, Нижнее меню, Мобильное меню, Пункт', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Мобильная версия', 'settings/mobileversion', '', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Модули', 'modules', 'Приложения, плагины', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Налоги', 'settingscheckout#?checkoutTab=taxes', '', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Настройки', 'settings', 'Опции, Конфигурация, О магазине', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Настройки CRM', 'settingscrm', '', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Настройки задач', 'settingstasks', '', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Настройки карточки товара', 'settingscatalog#?catalogTab=product', 'товарб фотографии, Отзывы, Перекрестный маркетинг, Похожие товары, cross, up sale', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Настройки новостей', 'settingsnews', 'rss, подписка', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Настройки оформления заказа', 'settingscheckout', '', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Настройки почты', 'settingsmail', 'почта, письма, smtp', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Настройки производителей', 'settingscatalog#?catalogTab=brand', 'Бренд, производитель', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Новинки', 'mainpageproducts?type=new', 'Новые поступления', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Новости', 'news', 'Блог, Статьи', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Нумерация заказов', 'settingscheckout', '', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Обучение', 'academy', 'Уроки, помощь, видео, академия', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Отделы', 'settings/userssettings#?tab=Departments', '', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Отзывы о товарах', 'reviews', 'Комментарии', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Оформление заказов', 'settingscheckout', 'Настройки', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Печать заказа', 'settingscheckout', '', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('План продаж', 'settings#?indexTab=plan', '', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Подарки', 'settingscheckout', '', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Подарочные сертификаты', 'certificates', '', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Подписчики', 'subscription', 'Подписка, новости, рассылки', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Поиск', 'settingscatalog#?catalogTab=search', '', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Покупатели', 'customers', 'Клиенты, пользователи', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Покупка в один клик', 'settingscheckout', '', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Поля в оформлении заказа', 'settingscheckout#?checkoutTab=checkoutfields', '', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Посадочные страницы', 'landing', 'Лендинг', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Правила бонусов', 'rules', '', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Проекты', 'taskgroups', '', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Производители', 'brands', 'Бренды', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Рабочий стол', '/', 'Главная, дашборд', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Регулирование цен', 'priceregulation', 'Цены, ценообразование', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Роли сотрудников', 'settings/userssettings#?tab=ManagerRoles', '', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Свойства товаров', 'properties', 'Характеристики', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Системные настройки', 'settingssystem', '', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Скидки', 'discountspricerange', '', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Скрипты конверсий заказа', 'settingscheckout#?checkoutTab=scripts', 'CPO, партнер', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Смс история бонусов', 'smslog', '', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Смс шаблоны бонусов', 'smstemplates', '', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Сотрудники', 'settings/userssettings', '', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Социальные сети', 'settingssocial', 'Cоциальные кнопки,  Вконтакте, facebook', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Списки товаров', 'productlists', 'Товары, Свои списки', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Способы доставки', 'settings/shippingmethods', 'Метод доставки, Курьер, edost, Яндекс доставка, Сдэк, Чекаут,  Самовывоз, Почта, Ems, Пункты выдачи', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Способы оплаты', 'settings/paymentmethods', 'Платежи, Наличные, Карта, Счет, Сбербанк, Яндекс Деньги, Web money, Пластиковая карта, Наложенный платеж, robokassa, единая касса, w1', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Справочник размеров', 'sizes', 'Размеры', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Справочник цветов', 'colors', 'Цвета', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Статические блоки', 'staticblock', 'Виджет, группа, vk, счетчик', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Статические страницы', 'staticpages', 'О магазине, Доставка, Оплата, Контакты, Гарантии', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Статусы заказов', 'orderstatuses', 'Новый, В обработке, Отправлен, Доставлен, Отменен, Заказ завершен, Команда', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Страны и города', 'settingssystem#?systemTab=countries', 'Регионы', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Теги', 'tags', '', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Товары', 'catalog', '', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Товары на главной', 'mainpageproducts?type=new', '', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Товары со скидкой', 'mainpageproducts?type=sale', 'Скидки, распродажа', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Файл robots.txt', 'ettingsseo#?seoTab=seoRobotTxt', '', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Файлы', 'files', '', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Фильтры', 'settingscatalog', '', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Форматы писем', 'settingsmail#?notifyTab=formats', '', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Хиты', 'mainpageproducts?type=best', 'Бестселлеры', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Центр поддержки', 'supportcenter', 'вопрос, тикет, заявка, поддержка, написать', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Экспорт заказов', 'analytics/exportorders', '', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Экспорт категорий', 'exportcategories', 'Выгрузка', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Экспорт товаров', 'exportfeeds', 'Выгрузка, csv, excel', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Этапы сделки', 'settingscrm', 'crm', '0')
insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) values ('Яндекс Маркет', 'exportfeeds/indexyandex', 'yml,  yandex market', '0')

GO--

ALTER TABLE Catalog.Offer
	DROP CONSTRAINT FK_Offer_Size
	
ALTER TABLE Catalog.Offer ADD CONSTRAINT
	FK_Offer_Size FOREIGN KEY
	(
	SizeID
	) REFERENCES Catalog.Size
	(
	SizeID
	) ON UPDATE  NO ACTION 
	 ON DELETE  SET NULL 

GO--

ALTER TABLE Catalog.Offer
	DROP CONSTRAINT FK_Offer_Color
	
ALTER TABLE Catalog.Offer ADD CONSTRAINT
	FK_Offer_Color FOREIGN KEY
	(
	ColorID
	) REFERENCES Catalog.Color
	(
	ColorID
	) ON UPDATE  NO ACTION 
	 ON DELETE  SET NULL 
	 
GO--

update SEO.MetaInfo 
		set Title = Replace(Replace(Title,'#PAGE#',''),'#CATEGORY_NAME#','#CATEGORY_NAME##PAGE#'),
			MetaDescription = Replace(Replace(MetaDescription,'#PAGE#',''),'#CATEGORY_NAME#','#CATEGORY_NAME##PAGE#'),
			H1 = Replace(Replace(H1,'#PAGE#',''),'#CATEGORY_NAME#','#CATEGORY_NAME##PAGE#')
			where type = 'Category'

GO--

if ((Select Value from [Settings].[Settings] Where Name = 'SearchMaxItems') = '100')
	Update [Settings].[Settings] Set Value = '1000' Where Name = 'SearchMaxItems'
GO--

update settings.settings set Value = 'yandexmap' where Name = 'PrintOrder_MapType'

GO--


UPDATE [Settings].[InternalSettings] SET [settingValue] = '6.0.0' WHERE [settingKey] = 'db_version'
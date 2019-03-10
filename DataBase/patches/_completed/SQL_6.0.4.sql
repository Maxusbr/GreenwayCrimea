
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.FileHelpers.FilesHelpText.Common', 'Допустимые расширения файлов: {0}');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.FileHelpers.FilesHelpText.MaxFileSize', 'Максимальный размер файла: {0}');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.FileHelpers.FilesHelpText.TaskAttachment', 'Вы можете прикрепить к задаче файлы с расширениями {0} <br/>и не превышающие 15 MB');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.FileHelpers.FilesHelpText.LeadAttachment', 'Вы можете прикрепить к лиду файлы с расширениями {0} <br/>и не превышающие 15 MB');

Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Admin.Account.SetPassword.Title', 'Установить пароль');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Admin.Account.SetPassword.Title', 'Set password');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Admin.Account.SetPassword.ChangePassword', 'Сохранить пароль');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Admin.Account.SetPassword.ChangePassword', 'Save password');
GO--

-- Landing

CREATE TABLE [CMS].[Landing](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Enabled] [bit] NOT NULL,
	[Template] [nvarchar](max) NOT NULL,
	[Url] [nvarchar](max) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Landing] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO--


CREATE TABLE [CMS].[LandingSettings](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LandingId] [int] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Value] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_LandingSettings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO--

ALTER TABLE [CMS].[LandingSettings]  WITH CHECK ADD  CONSTRAINT [FK_LandingSettings_Landing] FOREIGN KEY([LandingId])
REFERENCES [CMS].[Landing] ([Id])
ON DELETE CASCADE
GO--

ALTER TABLE [CMS].[LandingSettings] CHECK CONSTRAINT [FK_LandingSettings_Landing]
GO--



CREATE TABLE [CMS].[LandingBlock](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LandingId] [int] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[ContentHtml] [nvarchar](max) NOT NULL,
	[Type] [nvarchar](255) NOT NULL,
	[Settings] [nvarchar](max) NOT NULL,
	[SortOrder] [int] NOT NULL,
	[Enabled] [bit] NOT NULL,
 CONSTRAINT [PK_LandingPageBlock_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO--

ALTER TABLE [CMS].[LandingBlock]  WITH CHECK ADD  CONSTRAINT [FK_LandingPageBlock_Landing] FOREIGN KEY([LandingId])
REFERENCES [CMS].[Landing] ([Id])
ON DELETE CASCADE
GO--

ALTER TABLE [CMS].[LandingBlock] CHECK CONSTRAINT [FK_LandingPageBlock_Landing]
GO--




CREATE TABLE [CMS].[LandingSubBlock](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LandingBlockId] [int] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[ContentHtml] [nvarchar](max) NOT NULL,
	[Type] [nvarchar](255) NOT NULL,
	[Settings] [nvarchar](max) NOT NULL,
	[SortOrder] [int] NOT NULL,
 CONSTRAINT [PK_LandingSubBlock] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO--

ALTER TABLE [CMS].[LandingSubBlock]  WITH CHECK ADD  CONSTRAINT [FK_LandingSubBlock_LandingBlock] FOREIGN KEY([LandingBlockId])
REFERENCES [CMS].[LandingBlock] ([Id])
ON DELETE CASCADE
GO--

ALTER TABLE [CMS].[LandingSubBlock] CHECK CONSTRAINT [FK_LandingSubBlock_LandingBlock]
GO--

-- End Landing


ALTER TABLE Settings.News ADD Enabled bit null
GO--
UPDATE Settings.News SET Enabled = 1
GO--
ALTER TABLE Settings.News ALTER COLUMN Enabled bit not null
GO--

Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'MyAccount.CommonInfo.Patronymic', 'Отчество');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'MyAccount.CommonInfo.Patronymic', 'Patronymic');
GO--
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Admin.ExportFeeed.SettingsYandex.ExportAllPhotos', 'Выгружать все фотографии');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Admin.ExportFeeed.SettingsYandex.ExportAllPhotos', 'Export all photos');

GO--

UPDATE [CMS].[StaticBlock] set Enabled = 0 where [Key] = 'CatalogLeft' and Content like '%<figure class="banner-main-page-aside"><a href="products/apple-iphone-5-64gb"><img alt="" class="banner-main-page-aside-pic" src="pictures/product/small/3816_small.jpg" /%'

GO--

if not exists (select * from Settings.Localization where ResourceKey= 'Core.Customers.SendMail.NotTrackNumber')
begin
    INSERT INTO Settings.Localization (LanguageId, ResourceKey, ResourceValue) VALUES (1, 'Core.Customers.SendMail.NotTrackNumber','Номер отслеживания заказа не определен')

    INSERT INTO Settings.Localization (LanguageId, ResourceKey, ResourceValue) VALUES (2, 'Core.Customers.SendMail.NotTrackNumber','Order tracking number is not defined')
end
GO--

update settings.MailFormatType set Comment = 'Письмо покупателю (#FIRSTNAME#, #LASTNAME#, #PATRONYMIC#, #TRACKNUMBER#, #STORE_NAME#)' where MailType = 'OnSendToCustomer'

GO--

ALTER TABLE [Catalog].[Product] ADD [IsDemo] bit NULL
GO--
ALTER TABLE [Catalog].[Category] ADD [IsDemo] bit NULL
GO--
ALTER TABLE [Catalog].[Brand] ADD [IsDemo] bit NULL
GO--

INSERT INTO Settings.Localization (LanguageId, ResourceKey, ResourceValue) VALUES (1, 'Admin.Catalog.ClearDemoData','Удаление демо каталога')
INSERT INTO Settings.Localization (LanguageId, ResourceKey, ResourceValue) VALUES (2, 'Admin.Catalog.ClearDemoData','Clear demo catalog')
GO--

ALTER FUNCTION [Settings].[PhotoToString]
(
	@ColorId int,
	@ProductId int
)
RETURNS varchar(Max)
AS
BEGIN
	DECLARE @result varchar(max)
	if @ColorId is null
	begin
		SELECT  @result = coalesce(@result + ',', '') + PhotoName
		FROM    Catalog.Photo
		WHERE [Photo].[ObjId]=@ProductId and Type ='Product' order by Main DESC, PhotoSortOrder ASC
	end
	else
	Begin
		SELECT  @result = coalesce(@result + ',', '') + PhotoName
		FROM    Catalog.Photo
		WHERE   [Photo].[ObjId]=@ProductId and Type ='Product' and (Photo.ColorID = @ColorID OR Photo.ColorID is null) order by Main DESC, PhotoSortOrder ASC
	end
	RETURN @Result
END

GO--
IF NOT EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'YandexSizeUnit'
          AND Object_ID = Object_ID(N'Catalog.Product'))
BEGIN
	Alter table Catalog.Product
	Add YandexSizeUnit nvarchar(10) NULL
END

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
	 @AccrueBonuses bit,
     @Taxid int, 
	 @YandexSizeUnit nvarchar(10)
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
             ,TaxId		
			 ,YandexSizeUnit	 
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
			 @AccrueBonuses,
             @TaxId,
			 @YandexSizeUnit			 
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
	 @AccrueBonuses bit,
	 @TaxId int,
	 @YandexSizeUnit nvarchar(10)   
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
	 ,[TaxId] = @TaxId 
	 ,[YandexSizeUnit] = @YandexSizeUnit
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
			,[Product].YandexSizeUnit
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

UPDATE [Settings].[InternalSettings] SET [settingValue] = '6.0.4' WHERE [settingKey] = 'db_version'
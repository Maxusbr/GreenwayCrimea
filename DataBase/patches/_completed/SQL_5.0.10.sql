GO--

Alter Table Settings.ExportFeed
Add LastExportFileFullName nvarchar(250) null

GO--

Alter Table Customers.City
Add MobilePhoneNumber nvarchar(max) null

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
  ,Property.Type  
  ,GroupId  
  ,GroupName  
  ,GroupSortorder  
 FROM [Catalog].[PropertyValue]  
 INNER JOIN [Catalog].[Property] ON [Property].[PropertyID] = [PropertyValue].[PropertyID]  
 LEFT JOIN [Catalog].PropertyGroup ON PropertyGroup.PropertyGroupID = [Property].GroupID  
 WHERE [Property].[PropertyID] = @PropertyID  
  order by [PropertyValue].[SortOrder], PropertyValue.Value 
END  

GO--

Insert Into [Settings].[Settings] ([Name],[Value]) Values ('BrandsPerPage', '12')

GO--

Insert Into [Settings].[Settings] ([Name],[Value]) Values ('RelatedProductsMaxCount', '10')

GO--

ALTER TABLE Catalog.Coupon ADD
	CurrencyIso3 nvarchar(3) NULL
	
GO--

Update Catalog.Coupon Set CurrencyIso3 = (Select top(1)Value From [Settings].[Settings] Where Name = 'DefaultCurrencyISO3')

GO--


alter table catalog.product add YandexModel nvarchar(500) null

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
	@YandexModel nvarchar(500)
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
		   @YandexModel
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
	@YandexModel nvarchar(500)
	
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
WHERE ProductID = @ProductID    
END  


GO--


ALTER PROCEDURE [Settings].[sp_GetExportFeedProducts] @exportFeedId int
	,@onlyCount BIT
	,@exportNotActive BIT
	,@exportNotAmount BIT
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
					AND (Offer.Price > 0 OR @exportNotAmount = 1)
					AND (
						Offer.Amount > 0
						OR Product.AllowPreOrder = 1 OR @exportNotAmount = 1
						)
					AND CategoryEnabled = 1
					AND (Enabled = 1 OR @exportNotActive = 1)
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
		INNER JOIN [Catalog].Currency ON Currency.CurrencyID = [Product].CurrencyID
		WHERE (
				SELECT TOP (1) [ProductCategories].[CategoryId]
				FROM [Catalog].[ProductCategories]
				INNER JOIN [Catalog].[Category] ON [Category].[CategoryId] = [ProductCategories].[CategoryId]
				WHERE [ProductID] = [Product].[ProductID]
					AND [Enabled] = 1
					AND [Main] = 1
				) = [ProductCategories].[CategoryId]
			AND (Offer.Price > 0 OR @exportNotAmount = 1)
			AND (
				Offer.Amount > 0
				OR Product.AllowPreOrder = 1
				OR @exportNotAmount = 1
				)
			AND CategoryEnabled = 1
			AND (Product.Enabled = 1 OR @exportNotActive = 1)
	END
END

GO--

ALTER PROCEDURE [Catalog].[PreCalcProductParamsMass] AS BEGIN  
SET NOCOUNT ON;  
INSERT INTO [Catalog].[ProductExt] (ProductId,CountPhoto,PhotoId,VideosAvailable,MaxAvailable,NotSamePrices,MinPrice,Colors,AmountSort,OfferId,Comments,CategoryId)  
  (SELECT ProductId, 0, NULL, 0, 0, 0, 0, NULL, 0, NULL, 0, NULL  
   FROM [Catalog].Product  
   WHERE Product.ProductId NOT IN (SELECT ProductId FROM [Catalog].[ProductExt]))  
         
UPDATE [Catalog].[ProductExt]  
SET [CountPhoto] =  
	   (SELECT Top(1) CASE  
		   WHEN Offer.ColorID IS NOT NULL THEN  
			(SELECT Count(PhotoId)  
			 FROM [Catalog].[Photo]  
			 WHERE ([Photo].ColorID = Offer.ColorID  
			  OR [Photo].ColorID IS NULL)  
			   AND [Photo].[ObjId] = [ProductExt].ProductId  
			   AND TYPE = 'Product')  
		   ELSE  
			(SELECT Count(PhotoId)  
			 FROM [Catalog].[Photo]  
			 WHERE [Photo].[ObjId] = [ProductExt].ProductId  
			   AND TYPE = 'Product')  
		  END  
		FROM [Catalog].[Offer]  
		WHERE [ProductID] = [ProductExt].ProductId AND main =1),  
      
    [PhotoId] =  
	   (SELECT Top(1) CASE  
		   WHEN Offer.ColorID IS NOT NULL THEN  
			(SELECT TOP (1) PhotoId  
			 FROM [Catalog].[Photo]  
			 WHERE ([Photo].ColorID = Offer.ColorID  
			  OR [Photo].ColorID IS NULL)  
			   AND [Photo].[ObjId] = [ProductExt].ProductId  
			   AND TYPE = 'Product'  
			 ORDER BY main DESC, [Photo].[PhotoSortOrder],[PhotoId])  
		   ELSE  
			(SELECT TOP (1) PhotoId  
			 FROM [Catalog].[Photo]  
			 WHERE [Photo].[ObjId] = [ProductExt].ProductId  
			   AND TYPE = 'Product'  
			 ORDER BY main DESC ,[Photo].[PhotoSortOrder])  
		  END  
		FROM [Catalog].[Offer]  
		WHERE [ProductID] = [ProductExt].ProductId AND main =1),  
      
    [VideosAvailable] =  
	   (SELECT Top(1) CASE  
		   WHEN COUNT(ProductVideoID) > 0 THEN 1  
		   ELSE 0  
		  END  
		FROM [Catalog].[ProductVideo]  
		WHERE ProductID = [ProductExt].ProductId)  
      
    ,[MaxAvailable] = (
		SELECT Max(Offer.Amount) 
		FROM [Catalog].Offer
		WHERE ProductId = [ProductExt].ProductId
		)
      
    ,[NotSamePrices] =  
	   (SELECT Top(1) CASE  
		   WHEN max(price) - min(price) > 0 THEN 1  
		   ELSE 0  
		  END  
		FROM [Catalog].offer  
		WHERE offer.productid = [ProductExt].ProductId and price >0),  
      
    [MinPrice] = (SELECT min(price) FROM [Catalog].offer WHERE offer.productid = [ProductExt].ProductId and price >0),  
     
	[PriceTemp] =  
	   (SELECT ([MinPrice] - [MinPrice] * [Product].Discount / 100) * CurrencyValue     
	    FROM catalog.product 
	    inner join catalog.Currency on product.currencyid = Currency.currencyid  
	    WHERE product.productid = [ProductExt].ProductId),  
  
    [Colors] = (SELECT [Settings].[ProductColorsToString]([ProductExt].ProductId)),  
     
    [AmountSort] =  
	   (SELECT Top(1) CASE  
		   WHEN MaxAvailable <= 0  
			 OR MaxAvailable < IsNull(Product.MinAmount, 0) THEN 0  
		   ELSE 1  
		  END  
		FROM [Catalog].Offer  
		INNER JOIN [Catalog].Product ON Product.ProductId = Offer.ProductId  
		WHERE Offer.ProductId = [ProductExt].ProductId AND main = 1),  
     
    [OfferId] =  
	   (SELECT Top(1) OfferID  
		FROM [Catalog].offer  
		WHERE offer.productid = [ProductExt].ProductId AND (offer.Main = 1 OR offer.Main IS NULL)),  
   
	[Comments] =   
	   (SELECT Count(ReviewId) From CMS.Review Where EntityId = [ProductExt].ProductId And Checked = 1),  
     
	[Gifts] =  
	   (SELECT Top(1) CASE  
		   WHEN COUNT(ProductID) > 0 THEN 1  
		   ELSE 0  
		  END  
		FROM [Catalog].[ProductGifts]  
		WHERE ProductID = [ProductExt].ProductId),  
   
	-- 1. get main category of product, 2. get root category by main category  
	[CategoryId] =      
	   (Select Top 1 id From [Settings].[GetParentsCategoryByChild]((SELECT top 1 CategoryID FROM [Catalog].ProductCategories WHERE ProductID = [ProductExt].ProductId ORDER BY Main DESC)) Order by sort desc)  
END 
 
GO--


UPDATE [Settings].[InternalSettings] SET [settingValue] = '5.0.10' WHERE [settingKey] = 'db_version'
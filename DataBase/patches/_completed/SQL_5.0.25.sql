
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

ALTER PROCEDURE [Catalog].[sp_AddProduct]    
	@ArtNo nvarchar(100) = '',  
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


ALTER PROCEDURE [Catalog].[sp_UpdateProductById]  
	@ProductID int,      
	@ArtNo nvarchar(100),  
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

 GO--
  update [CMS].[Menu]
  set [MenuItemUrlPath] ='forgotpassword'
  where [MenuItemUrlPath]='fogotpassword'
GO--

GO--

ALTER PROCEDURE [Catalog].[PreCalcProductParams] 
	@productId INT,
	@ModerateReviews BIT
AS      
BEGIN      
 SET NOCOUNT ON;      
      
 DECLARE @CountPhoto INT      
 DECLARE @Type NVARCHAR(10)      
 DECLARE @PhotoId INT      
 DECLARE @MaxAvailable float      
 DECLARE @VideosAvailable BIT      
 DECLARE @Colors NVARCHAR(max)      
 DECLARE @NotSamePrices BIT      
 DECLARE @MinPrice FLOAT    
 DECLARE @PriceTemp FLOAT       
 DECLARE @AmountSort BIT      
 DECLARE @OfferId int      
 DECLARE @Comments int    
 DECLARE @CategoryId int    
 DECLARE @Gifts bit    
       
 if not exists(select  ProductID from [Catalog].Product where ProductID=@productId)      
 return      
      
 SET @Type = 'Product'      
 --@CountPhoto      
 SET @CountPhoto = (      
   SELECT Top(1) CASE    
		 WHEN (select Offer.ColorID  FROM [Catalog].[Offer] where [ProductID]= @productId AND main =1 ) IS NOT NULL THEN    
	   (SELECT Count(PhotoId)    
		FROM [Catalog].[Photo] 
		inner join [Catalog].[Offer] on [Photo].ColorID = Offer.ColorID  
		WHERE   
		   [Photo].[ObjId] = @productId      
		  AND TYPE = @Type )    
		 ELSE    
	   (SELECT Count(PhotoId)    
		FROM [Catalog].[Photo]    
		WHERE [Photo].[ObjId] = @productId    
		  AND TYPE = @Type )    
		END         
   )      
 --@PhotoId      
 SET @PhotoId = (      
   SELECT CASE       
     WHEN Offer.ColorID IS NOT NULL and amount <> 0 and price> 0   
      THEN (      
        SELECT TOP (1) PhotoId      
        FROM [Catalog].[Photo]      
        WHERE (      
          [Photo].ColorID = Offer.ColorID      
          OR [Photo].ColorID IS NULL      
          )      
         AND [Photo].[ObjId] = @productId      
         AND Type = @Type      
        ORDER BY main DESC      
         ,[Photo].[PhotoSortOrder]      
        )      
     ELSE (      
       SELECT TOP (1) PhotoId      
       FROM [Catalog].[Photo]      
       WHERE [Photo].[ObjId] = @productId      
        AND Type = @Type      
        ORDER BY main DESC      
         ,[Photo].[PhotoSortOrder]  
         ,[PhotoId]  
       )      
     END      
   FROM [Catalog].[Offer]       
   WHERE [ProductID] = @productId and main =1      
   )       
 --VideosAvailable      
 IF (      
   SELECT COUNT(ProductVideoID)      
   FROM [Catalog].[ProductVideo]      
   WHERE ProductID = @productId      
   ) > 0      
 BEGIN      
  SET @VideosAvailable = 1      
 END      
 ELSE      
 BEGIN      
  SET @VideosAvailable = 0      
 END      
      
 --@MaxAvailable      
SET @MaxAvailable = (SELECT Max(Offer.Amount)  
   FROM [Catalog].Offer  
   WHERE ProductId = @productId)   
      
 --AmountSort      
 SET @AmountSort = (      
   SELECT CASE       
     WHEN @MaxAvailable <= 0      
      OR @MaxAvailable < IsNull(Product.MinAmount, 0)      
      THEN 0      
     ELSE 1      
     END      
   FROM [Catalog].Offer inner join [Catalog].Product on Product.ProductId=Offer.ProductId      
   WHERE Offer.ProductId = @productId      
    AND main = 1      
   )      
 --Colors      
 SET @Colors = (      
   SELECT [Settings].[ProductColorsToString](@productId)      
   )      
      
 --@NotSamePrices      
 IF (      
   SELECT max(price) - min(price)      
   FROM [Catalog].offer      
   WHERE offer.productid = @productId and price > 0   
   ) > 0      
 BEGIN      
  SET @NotSamePrices = 1      
 END      
 ELSE      
 BEGIN      
  SET @NotSamePrices = 0      
 END      
  
 --@MinPrice      
 SET @MinPrice = (      
   SELECT min(price)      
   FROM CATALOG.offer      
   WHERE offer.productid = @productId and price > 0   
   )      
  
 --@OfferId    
 SET @OfferId = (SELECT OfferID      
   FROM CATALOG.offer      
   WHERE offer.productid = @productId and (offer.Main = 1 OR offer.Main IS NULL))     
  
  
 --@PriceTemp      
 SET @PriceTemp = (      
   SELECT (@MinPrice - @MinPrice * [Product].Discount / 100) * CurrencyValue       
   FROM Catalog.Product   
   inner join Catalog.Currency on Product.Currencyid = Currency.Currencyid    
   WHERE Product.Productid = @productId  
   )      
  
 --@Comments    
 SET @Comments = (    
 SELECT Count(ReviewId) From CMS.Review Where EntityId = @productId And (Checked = 1 or @ModerateReviews = 0)  
 )     
       
--@Gifts    
SET @Gifts =    
   (SELECT Top(1) CASE WHEN COUNT(ProductID) > 0 THEN 1 ELSE 0 END    
    FROM [Catalog].[ProductGifts]    
    WHERE ProductID = @productId)    
     
 --@CategoryId    
 Declare @MainCategoryId int    
 SET @MainCategoryId = (SELECT top 1 CategoryID FROM [Catalog].ProductCategories WHERE ProductID = @productId ORDER BY Main DESC)    
 IF @MainCategoryId IS NOT NULL    
 BEGIN    
 SET @CategoryId = (    
  Select Top 1 id From [Settings].[GetParentsCategoryByChild](@MainCategoryId) Order by sort desc    
 )    
 END    
      
 IF (      
   SELECT Count(productid)      
   FROM [Catalog].ProductExt      
   WHERE productid = @productId      
   ) > 0      
 BEGIN      
  UPDATE [Catalog].[ProductExt]      
  SET [CountPhoto] = @CountPhoto      
   ,[PhotoId] = @PhotoId         
   ,[VideosAvailable] = @VideosAvailable      
   ,[MaxAvailable] = @MaxAvailable      
   ,[NotSamePrices] = @NotSamePrices      
   ,[MinPrice] = @MinPrice      
   ,[Colors] = @Colors      
   ,[AmountSort] = @AmountSort      
   ,[OfferId] = @OfferId    
   ,[Comments] = @Comments     
   ,[CategoryId] = @CategoryId    
   ,[PriceTemp] = @PriceTemp    
   ,[Gifts] = @Gifts    
  WHERE [ProductId] = @productId      
 END      
 ELSE      
 BEGIN      
  INSERT INTO [Catalog].[ProductExt] (      
    [ProductId]      
   ,[CountPhoto]      
   ,[PhotoId]         
   ,[VideosAvailable]      
   ,[MaxAvailable]      
   ,[NotSamePrices]      
   ,[MinPrice]      
   ,[Colors]      
   ,[AmountSort]      
   ,[OfferId]    
   ,[Comments]    
   ,[CategoryId]    
   ,[PriceTemp]    
   ,[Gifts]    
   )      
  VALUES (      
    @productId      
   ,@CountPhoto      
   ,@PhotoId      
   ,@VideosAvailable      
   ,@MaxAvailable      
   ,@NotSamePrices      
   ,@MinPrice      
   ,@Colors      
   ,@AmountSort      
   ,@OfferId    
   ,@Comments    
   ,@CategoryId    
   ,@PriceTemp    
   ,@Gifts    
   )      
 END      
END
 
GO--
ALTER PROCEDURE [Catalog].[PreCalcProductParamsMass] 
	@ModerateReviews BIT
AS 
BEGIN    
	SET NOCOUNT ON;    
	INSERT INTO [Catalog].[ProductExt] (ProductId,CountPhoto,PhotoId,VideosAvailable,MaxAvailable,NotSamePrices,MinPrice,Colors,AmountSort,OfferId,Comments,CategoryId)    
	  (SELECT ProductId, 0, NULL, 0, 0, 0, 0, NULL, 0, NULL, 0, NULL    
	   FROM [Catalog].Product    
	   WHERE Product.ProductId NOT IN (SELECT ProductId FROM [Catalog].[ProductExt]))    
	           
	UPDATE [Catalog].[ProductExt]    
	SET [CountPhoto] =  (
		SELECT Top(1) CASE    
		 WHEN (select Offer.ColorID  FROM [Catalog].[Offer] where [ProductID]= [ProductExt].ProductId AND main =1 ) IS NOT NULL THEN    
	   (SELECT Count(PhotoId)    
		FROM [Catalog].[Photo] 
		inner join [Catalog].[Offer] on [Photo].ColorID = Offer.ColorID  
		WHERE   
		   [Photo].[ObjId] = [ProductExt].ProductId      
		  AND TYPE = 'Product')    
		 ELSE    
	   (SELECT Count(PhotoId)    
		FROM [Catalog].[Photo]    
		WHERE [Photo].[ObjId] = [ProductExt].ProductId    
		  AND TYPE = 'Product')    
		END	 
	),    
	        
		[PhotoId] =    
		(SELECT Top(1) CASE    
		 WHEN Offer.ColorID IS NOT NULL and amount <> 0 and price> 0 THEN    
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
		(SELECT Count(ReviewId) From CMS.Review Where EntityId = [ProductExt].ProductId And (Checked = 1 Or @ModerateReviews = 0)),    
	       
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

IF ((Select Count(*) From [Settings].[MailFormatType] Where [TypeName] = 'Письмо покупателю') <= 0)
	Insert Into [Settings].[MailFormatType] ([TypeName],[SortOrder],[Comment],[MailType]) Values ('Письмо покупателю', 300, 'Письмо покупателю (#FIRSTNAME#, #LASTNAME#, #PATRONYMIC#, #STORE_NAME#)', 'OnSendToCustomer')

GO--

IF ((Select Count(*) From [Settings].[MailFormatType] Where [TypeName] = 'Письмо покупателю') <= 0)
BEGIN
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
END

GO--

ALTER TABLE [Order].[Order] ADD
	TrackNumber nvarchar(255) NULL
	
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
  ,[Property].Unit
  ,[Property].[Description] as [Description]
  ,GroupName  
  ,GroupSortorder  
 FROM [Catalog].[PropertyValue]  
 INNER JOIN [Catalog].[ProductPropertyValue] ON [ProductPropertyValue].[PropertyValueID] = [PropertyValue].[PropertyValueID]  
 inner join [Catalog].[Property] on [Property].[PropertyID] = [PropertyValue].[PropertyID]  
 left join Catalog.PropertyGroup on propertyGroup.PropertyGroupID = [Property].GroupID  
 WHERE [ProductID] = @ProductID  
 ORDER BY case when PropertyGroup.GroupSortOrder is null then 1 else 0 end, 
 PropertyGroup.GroupSortOrder,PropertyGroup.GroupName, [Property].[SortOrder], [Property].Name, [PropertyValue].[SortOrder], [PropertyValue].Value  
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
		,Property.NAME AS PropertyName
		,Property.SortOrder AS PropertySortOrder
		,Property.Expanded
		,Property.[Type]
		,Property.[Description]
		,Unit
		,GroupId
		,GroupName
		,GroupSortorder
	FROM [Catalog].[PropertyValue]
	INNER JOIN [Catalog].[Property] ON [Property].[PropertyID] = [PropertyValue].[PropertyID]
	LEFT JOIN [Catalog].PropertyGroup ON PropertyGroup.PropertyGroupID = [Property].GroupID
	WHERE [Property].[PropertyID] = @PropertyID
	order by [PropertyValue].[SortOrder]
END

GO--


UPDATE [Settings].[InternalSettings] SET [settingValue] = '5.0.25' WHERE [settingKey] = 'db_version'
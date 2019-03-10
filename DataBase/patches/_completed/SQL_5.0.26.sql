
ALTER PROCEDURE [Catalog].[PreCalcProductParamsMass] 
	@ModerateReviews BIT,
	@OnlyAvailable bit
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
	  WHERE [ProductID] = [ProductExt].ProductId AND main = 1	 
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
		ORDER BY main DESC, [Photo].[PhotoSortOrder], [PhotoId])    
		END    
	  FROM [Catalog].[Offer]    
	  WHERE [ProductID] = [ProductExt].ProductId and main =1),    
	        
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
	  WHERE offer.productid = [ProductExt].ProductId and price >0  and (@OnlyAvailable = 0 OR amount > 0) ),    
	        
	[MinPrice] = (SELECT min(price) FROM [Catalog].offer WHERE offer.productid = [ProductExt].ProductId and price >0 and (@OnlyAvailable = 0 OR amount > 0) ),    
	       
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
       (SELECT Count(distinct PhotoId)    
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
         ,[Photo].[PhotoSortOrder],[PhotoId]  
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


IF ((Select Count(*) From [Settings].[MailFormatType] Where [TypeName] = 'Письмо покупателю') <= 0)
	Insert Into [Settings].[MailFormatType] ([TypeName],[SortOrder],[Comment],[MailType]) Values ('Письмо покупателю', 300, 'Письмо покупателю (#FIRSTNAME#, #LASTNAME#, #PATRONYMIC#, #STORE_NAME#)', 'OnSendToCustomer')

GO--

IF ((Select Count(*) From [Settings].[MailFormat] Where [MailFormatTypeId] = (Select top(1) [MailFormatTypeID] From [Settings].[MailFormatType] Where [MailType] = 'OnSendToCustomer')) <= 0)
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

Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.Customers.ErrorDeleteManager', 'There are {0}, assigned to manager'); 
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.Customers.ErrorDeleteManager.Tasks', 'tasks'); 
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.Customers.ErrorDeleteManager.Orders', 'orders'); 
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.Customers.ErrorDeleteManager.Leads', 'leads'); 
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.Customers.ErrorDeleteManager.Customers', 'customers'); 
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.Customers.ErrorDeleteManager.AppointedTasks', 'appointed tasks'); 
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.Customers.ErrorDeleteCustomer.NotFound', 'User not found');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.Customers.ErrorDeleteCustomer.SelfDelete', 'You are trying to delete your account');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.Customers.ErrorDeleteCustomer.IsAdmin', 'You have no permission to delete administrator');

Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Customers.ErrorDeleteManager', 'У менеджера есть назначенные {0}'); 
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Customers.ErrorDeleteManager.Tasks', 'задачи'); 
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Customers.ErrorDeleteManager.Orders', 'заказы'); 
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Customers.ErrorDeleteManager.Leads', 'лиды'); 
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Customers.ErrorDeleteManager.Customers', 'покупатели'); 
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Customers.ErrorDeleteManager.AppointedTasks', 'порученные задачи'); 
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Customers.ErrorDeleteCustomer.NotFound', 'Пользователь не найден');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Customers.ErrorDeleteCustomer.SelfDelete', 'Вы пытаетесь удалить свою учетную запись');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Customers.ErrorDeleteCustomer.IsAdmin', 'Нет прав для удаления администратора');

GO--

Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.ExportImport.ProductFields.BarCode', 'Штрих код');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.ExportImport.ProductFields.BarCode', 'BarCode');

GO--

alter table catalog.product add BarCode nvarchar(50) null

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
	@YandexModel nvarchar(500),
	@BarCode nvarchar(50)
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
		   @BarCode
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
	@YandexModel nvarchar(500),
	@BarCode nvarchar(50)
	
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
WHERE ProductID = @ProductID    
END  

GO--

ALTER PROCEDURE [Settings].[sp_GetExportFeedProducts] @exportFeedId int
	,@onlyCount BIT
	,@exportNotActive BIT
	,@exportNotAmount BIT
	,@selectedCurrency NVARCHAR(10)
	,@allowPreOrder bit
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
						OR (Product.AllowPreOrder = 1 and @allowPreOrder =1)
						OR @exportNotAmount = 1
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
			AND (Offer.Price > 0 OR @exportNotAmount = 1)
			AND (
				Offer.Amount > 0
				OR (Product.AllowPreOrder = 1 and @allowPreOrder =1)
				OR @exportNotAmount = 1
				)
			AND CategoryEnabled = 1
			AND (Product.Enabled = 1 OR @exportNotActive = 1)
	END
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
		SUM(([Sum] - [ShippingCost] - ([Taxcost] - [TaxCost]*OrderTax.TaxShowInPrice))*CurrencyValue) - SUM([SupplyTotal]) as 'Profit'
		
	FROM [Order].[Order] 
	Inner Join [Order].[OrderCurrency] On [OrderCurrency].[OrderId] = [Order].[OrderId] 
	inner join [Order].OrderTax on ordertax.OrderID = [order].OrderID
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
	 SUM([Sum]) - SUM([ShippingCost]) - SUM([Taxcost] - [TaxCost]*OrderTax.TaxShowInPrice) - SUM([SupplyTotal])  as 'Profit'
	FROM [Order].[Order] 
	inner join [Order].OrderTax on OrderTax.OrderId = [Order].OrderId
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
		Sum(case when OrderDiscount = 100 then   ([Sum] - [ShippingCost] - ([TaxCost]  - [TaxCost] * TaxShowInPrice)) 
		else  ([Sum] - [ShippingCost] - ([TaxCost]  - [TaxCost] * TaxShowInPrice))  * 100 / (100 - OrderDiscount)end ) as 'SumWDiscount',
		SUM([SupplyTotal]) as 'SupplyTotal',
		sum([TaxCost] - [TaxCost] * TaxShowInPrice) as 'Tax',  
		sum([ShippingCost]) as 'Shipping',
		(select sum([Value]) from @extraCharges where DATEADD(dd, 0, DATEDIFF(dd, 0, [OrderDate])) = DATEADD(dd, 0, DATEDIFF(dd, 0, Getdate())))
	from [Order].[Order]
	inner join [Order].[OrderTax] on OrderTax.OrderID = [Order].OrderID
	where [PaymentDate] is not null and DATEADD(dd, 0, DATEDIFF(dd, 0, [OrderDate])) = DATEADD(dd, 0, DATEDIFF(dd, 0, Getdate()))
	
	
	
   -- Yesterday profit
    
    insert into @temp 
		select 
		Count(*) as 'Count', 
		sum([Sum]) as 'Sum', 
		Sum(case when OrderDiscount = 100 then   ([Sum] - [ShippingCost] - ([TaxCost]  - [TaxCost] * TaxShowInPrice)) 
		else  ([Sum] - [ShippingCost] - ([TaxCost]  - [TaxCost] * TaxShowInPrice))  * 100 / (100 - OrderDiscount)end ) as 'SumWDiscount',
		SUM([SupplyTotal]) as 'SupplyTotal',
		sum([TaxCost] - [TaxCost] * TaxShowInPrice) as 'Tax',  
		sum([ShippingCost]) as 'Shipping',
		( select sum([Value]) from @extraCharges where DATEADD(dd, 0, DATEDIFF(dd, 0, [OrderDate])) = DATEADD(dd, -1, DATEDIFF(dd, 0, Getdate())) )
	from [Order].[Order]
	inner join [Order].[OrderTax] on OrderTax.OrderID = [Order].OrderID 
	where DATEADD(dd, 0, DATEDIFF(dd, 0, [OrderDate])) = DATEADD(dd, -1, DATEDIFF(dd, 0, Getdate()))and [PaymentDate] is not null
	
	
-- Month profit
    
    insert into @temp
		select 
		Count(*) as 'Count', 
		sum([Sum]) as 'Sum', 
		Sum(case when OrderDiscount = 100 then   ([Sum] - [ShippingCost] - ([TaxCost]  - [TaxCost] * TaxShowInPrice)) 
		else  ([Sum] - [ShippingCost] - ([TaxCost]  - [TaxCost] * TaxShowInPrice))  * 100 / (100 - OrderDiscount)end ) as 'SumWDiscount',
		SUM([SupplyTotal]) as 'SupplyTotal',
		sum([TaxCost] - [TaxCost] * TaxShowInPrice) as 'Tax',  
		sum([ShippingCost]) as 'Shipping',
		(select sum([Value]) from @extraCharges where Month([OrderDate]) = Month(getdate()) and Year([OrderDate]) = Year(getdate()))
	from [Order].[Order] 
	inner join [Order].[OrderTax] on OrderTax.OrderID = [Order].OrderID
	where Month([OrderDate]) = Month(getdate()) and Year([OrderDate]) = Year(getdate())and [PaymentDate] is not null
	
	--Total profit
	
	insert into @temp 
		select 
		Count(*) as 'Count', 
		sum([Sum]) as 'Sum', 
		Sum(case when OrderDiscount = 100 then   ([Sum] - [ShippingCost] - ([TaxCost]  - [TaxCost] * TaxShowInPrice)) 
		else  ([Sum] - [ShippingCost] - ([TaxCost]  - [TaxCost] * TaxShowInPrice))  * 100 / (100 - OrderDiscount)end ) as 'SumWDiscount',
		SUM([SupplyTotal]) as 'SupplyTotal',
		sum([TaxCost] - [TaxCost] * TaxShowInPrice) as 'Tax',  
		sum([ShippingCost]) as 'Shipping',
		(select sum([Value]) from @extraCharges)
	from [Order].[Order] 
	inner join [Order].[OrderTax] on OrderTax.OrderID = [Order].OrderID
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

insert into Settings.Localization (LanguageId,ResourceKey,ResourceValue) Values (1,'Js.ProductView.Photos0','фотографий')
insert into Settings.Localization (LanguageId,ResourceKey,ResourceValue) Values (1,'Js.ProductView.Photos1','фотография')
insert into Settings.Localization (LanguageId,ResourceKey,ResourceValue) Values (1,'Js.ProductView.Photos2','фотографии')
insert into Settings.Localization (LanguageId,ResourceKey,ResourceValue) Values (1,'Js.ProductView.Photos5','фотографий')
insert into Settings.Localization (LanguageId,ResourceKey,ResourceValue) Values (2,'Js.ProductView.Photos0','photos')
insert into Settings.Localization (LanguageId,ResourceKey,ResourceValue) Values (2,'Js.ProductView.Photos1','photo')
insert into Settings.Localization (LanguageId,ResourceKey,ResourceValue) Values (2,'Js.ProductView.Photos2','photos')
insert into Settings.Localization (LanguageId,ResourceKey,ResourceValue) Values (2,'Js.ProductView.Photos5','photos')

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

If (NOT EXISTS(Select * From [CMS].[StaticBlock] Where [Key] = 'head'))
Begin
	Insert Into [CMS].[StaticBlock] ([Key],[InnerName],[Content],[Added],[Modified],[Enabled]) Values ('head', 'Блок в head', '', GETDATE(), GETDATE(), 1)
End

GO--

UPDATE [Settings].[InternalSettings] SET [settingValue] = '5.0.26' WHERE [settingKey] = 'db_version'
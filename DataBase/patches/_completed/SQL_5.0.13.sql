
ALTER TABLE [Order].ShippingMethod ADD
	DisplayIndex bit NULL
GO--

Update [Order].[ShippingMethod] Set [DisplayIndex] = Case [ShippingType] when 'FixedRate' then 0 when 'SelfDelivery' then 0 else  [DisplayCustomFields] end
GO--

ALTER PROCEDURE [Settings].[sp_GetCsvProducts]   
	  @exportFeedId int  
	 ,@onlyCount BIT  
	 ,@exportNoInCategory BIT  
	 ,@exportNotActive BIT
	 ,@exportNotAmount BIT
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
   AND (Enabled = 1 OR @exportNotActive = 1) 
   AND ((Select ISNULL(Max(Price), 0) From [Catalog].[Offer] Where [Offer].[ProductId] = [Product].[ProductID]) > 0 OR @exportNotAmount = 1)
   AND ((Select ISNULL(Max(Amount), 0) From [Catalog].[Offer] Where [Offer].[ProductId] = [Product].[ProductID]) > 0 OR Product.AllowPreOrder = 1 OR @exportNotAmount = 1) 
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
   AND (Enabled = 1 OR @exportNotActive = 1)  
   AND ((Select ISNULL(Max(Price), 0) From [Catalog].[Offer] Where [Offer].[ProductId] = [Product].[ProductID]) > 0 OR @exportNotAmount = 1)
   AND ((Select ISNULL(Max(Amount), 0) From [Catalog].[Offer] Where [Offer].[ProductId] = [Product].[ProductID]) > 0 OR Product.AllowPreOrder = 1 OR @exportNotAmount = 1)
 END  
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
   SELECT CASE       
     WHEN Offer.ColorID IS NOT NULL      
      THEN (      
        SELECT Count(PhotoId)      
        FROM [Catalog].[Photo]      
        WHERE (      
          [Photo].ColorID = Offer.ColorID      
          OR [Photo].ColorID IS NULL      
          )      
         AND [Photo].[ObjId] = @productId      
         AND Type = @Type      
        )      
     ELSE (      
       SELECT Count(PhotoId)      
       FROM [Catalog].[Photo]      
       WHERE [Photo].[ObjId] = @productId      
        AND Type = @Type      
       )      
     END      
   FROM [Catalog].[Offer]      
   WHERE [ProductID] = @productId and main =1      
   )      
 --@PhotoId      
 SET @PhotoId = (      
   SELECT CASE       
     WHEN Offer.ColorID IS NOT NULL      
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

CREATE TABLE [Order].[OrderSource](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
	[SortOrder] [int] NOT NULL,
	[Main] [bit] NOT NULL,
	[Type] [nvarchar](250) NOT NULL,
 CONSTRAINT [PK_OrderSource] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO--

INSERT INTO [Order].[OrderSource] ([Name],[SortOrder],[Main],[Type]) VALUES  ('Корзина интернет магазина',0,1,'ShoppingCart')
INSERT INTO [Order].[OrderSource] ([Name],[SortOrder],[Main],[Type]) VALUES  ('Оффлайн',10,1,'Offline')
INSERT INTO [Order].[OrderSource] ([Name],[SortOrder],[Main],[Type]) VALUES  ('В один клик',20,1,'OneClick')
INSERT INTO [Order].[OrderSource] ([Name],[SortOrder],[Main],[Type]) VALUES  ('Посадочная страница',30,1,'LandingPage')
INSERT INTO [Order].[OrderSource] ([Name],[SortOrder],[Main],[Type]) VALUES  ('Мобильная версия',40,1,'Mobile')
INSERT INTO [Order].[OrderSource] ([Name],[SortOrder],[Main],[Type]) VALUES  ('По телефону',50,1,'Phone')
INSERT INTO [Order].[OrderSource] ([Name],[SortOrder],[Main],[Type]) VALUES  ('Онлайн консультант',60,1,'LiveChat')
INSERT INTO [Order].[OrderSource] ([Name],[SortOrder],[Main],[Type]) VALUES  ('Социальные сети',70,1,'SocialNetworks')
INSERT INTO [Order].[OrderSource] ([Name],[SortOrder],[Main],[Type]) VALUES  ('Нашли дешевле',80,1,'FindCheaper')
INSERT INTO [Order].[OrderSource] ([Name],[SortOrder],[Main],[Type]) VALUES  ('Брошенные корзины',90,1,'AbandonedCart')
INSERT INTO [Order].[OrderSource] ([Name],[SortOrder],[Main],[Type]) VALUES  ('Обратный звонок',100,1,'Callback')
INSERT INTO [Order].[OrderSource] ([Name],[SortOrder],[Main],[Type]) VALUES  ('Другое',110,1,'None')

GO--

Alter Table [Order].[Order] 
Add [OrderSourceId] int NULL

GO--

Update [Order].[Order] Set OrderSourceId = (Select Id From [Order].[OrderSource] Where [Type] = OrderType And Main = 1)

GO--

Alter Table [Order].[Order] 
Alter Column [OrderSourceId] int NOT NULL

GO--

Alter Table [Order].[Lead] 
Add [OrderSourceId] int NULL

GO--

Update [Order].[Lead] Set OrderSourceId = (Select Id From [Order].[OrderSource] Where [Type] = OrderType And Main = 1)

GO--

Alter Table [Order].[Lead] 
Alter Column [OrderSourceId] int NOT NULL

GO--

ALTER TABLE [Order].Lead
DROP COLUMN OrderType

GO--

ALTER TABLE [Order].[Order]
DROP COLUMN OrderType

GO--

Delete From [Customers].[RoleAction] Where [Key] = 'DisplayExportFeed'
GO--

Delete From [Customers].[CustomerRoleAction] Where [RoleActionKey] = 'DisplayExportFeed'
GO--

Update settings.settings set value = 'False_temp' where name='BuyInOneClick_EnabledInCheckout' and value='True'

Update settings.settings set value = 'True' where name='BuyInOneClick_EnabledInCheckout' and value='False'

Update settings.settings set value = 'False' where name='BuyInOneClick_EnabledInCheckout' and value='False_temp'

GO--

Update settings.settings set name = 'BuyInOneClick_DisableInCheckout' where name='BuyInOneClick_EnabledInCheckout'

GO--

alter table catalog.category add Hidden bit null
GO--
update catalog.category set hidden=0
GO--
alter table catalog.category alter column Hidden bit not null
GO--

  
ALTER PROCEDURE [Catalog].[sp_AddCategory]    
  @Name nvarchar(255),  
  @ParentCategory int,  
  @Description nvarchar(max),  
  @BriefDescription nvarchar(max),  
  @SortOrder int,  
  @Enabled bit,  
  @Hidden bit,
  @DisplayStyle int,  
  @DisplayChildProducts bit,  
  @DisplayBrandsInMenu bit,  
  @DisplaySubCategoriesInMenu bit,  
  @UrlPath nvarchar(150),  
  @Sorting int  
AS  
BEGIN  
 INSERT INTO [Catalog].[Category]  
    (  
       [Name]  
      ,[ParentCategory]  
      ,[Description]  
      ,[BriefDescription]  
      ,[Products_Count]  
      ,[SortOrder]  
      ,[Enabled]  
	  ,[Hidden]
      ,[DisplayStyle]  
      ,[DisplayChildProducts]  
      ,[DisplayBrandsInMenu]  
      ,[DisplaySubCategoriesInMenu]  
      ,[UrlPath]  
      ,[Sorting]
   )  
   VALUES  
   (  
    @Name,  
    @ParentCategory,  
    @Description,  
    @BriefDescription,  
    0,  
    @SortOrder,  
    @Enabled,  
	@Hidden,
    @DisplayStyle,  
    @DisplayChildProducts,  
    @DisplayBrandsInMenu,  
    @DisplaySubCategoriesInMenu,  
    @UrlPath,  
    @Sorting);  
 Select SCOPE_IDENTITY();  
END  
  
GO--

  
ALTER PROCEDURE [Catalog].[sp_UpdateCategory]  
  
  @CategoryID int,  
  @Name nvarchar(255),  
  @ParentCategory int,  
  @Description nvarchar(max),  
  @BriefDescription nvarchar(max),  
  @SortOrder int,  
  @Enabled bit,  
  @Hidden bit,
  @DisplayStyle int,  
  @DisplayChildProducts bit = '0',  
  @DisplayBrandsInMenu bit,  
  @DisplaySubCategoriesInMenu bit,  
  @UrlPath nvarchar(150),  
  @Sorting int  
  
AS  
BEGIN  
  
UPDATE [Catalog].[Category]  
    SET   
     [Name] = @Name  
    ,[ParentCategory] = @ParentCategory  
    ,[Description] = @Description  
    ,[BriefDescription] = @BriefDescription  
    ,[SortOrder] = @SortOrder  
    ,[Enabled] = @Enabled  
	,[Hidden] = @Hidden  
    ,[DisplayStyle] = @DisplayStyle  
    ,[DisplayChildProducts] = @DisplayChildProducts  
    ,[DisplayBrandsInMenu] = @DisplayBrandsInMenu  
    ,[DisplaySubCategoriesInMenu] = @DisplaySubCategoriesInMenu  
    ,[UrlPath]=@UrlPath  
    ,Sorting = @Sorting  
  WHERE CategoryID = @CategoryID  
END  
  
GO--

ALTER PROCEDURE [Catalog].[sp_GetChildCategoriesByParentIDForMenu]
	@CurrentCategoryID int
AS
BEGIN
	
	IF (Select COUNT(CategoryID) From Catalog.Category Where ParentCategory = @CurrentCategoryID and Enabled = 1) > 0 
		BEGIN
			SELECT 
				[CategoryID],
				[Name],
				[Description],
				[BriefDescription],
				[ParentCategory],
				--[Picture],
				[Products_Count],
				[Total_Products_Count],
				[SortOrder],
				[Enabled],
				[Hidden],
				--[MiniPicture],
				[DisplayStyle],	
				[DisplayBrandsInMenu],
				[DisplaySubCategoriesInMenu],
				[UrlPath],
				(SELECT Count(CategoryID) FROM [Catalog].[Category] AS c WHERE c.ParentCategory = cat.CategoryID and Enabled = 1) AS [ChildCategories_Count] 
			FROM [Catalog].[Category] AS cat 
			WHERE [ParentCategory] = @CurrentCategoryID AND CategoryID <> 0 and Enabled = 1
			ORDER BY SortOrder, Name
		END
	ELSE
		BEGIN
			SELECT 
				[CategoryID],
				[Name],
				[Description],
				[BriefDescription],
				[ParentCategory],
				--[Picture],
				[Products_Count],
				[Total_Products_Count],
				[SortOrder],
				[Enabled],
				[Hidden],
				--[MiniPicture],
				[DisplayStyle],	
				[DisplayBrandsInMenu],
				[DisplaySubCategoriesInMenu],				
				[UrlPath],
				(SELECT Count(CategoryID) FROM [Catalog].[Category] AS c WHERE c.ParentCategory = cat.CategoryID and Enabled = 1) AS [ChildCategories_Count] 
			FROM [Catalog].[Category] AS cat WHERE [ParentCategory] = (Select ParentCategory From Catalog.Category 
			Where CategoryID = @CurrentCategoryID) AND CategoryID <> 0 and Enabled = 1
			ORDER BY SortOrder, Name
		END

END

GO--


UPDATE [Settings].[InternalSettings] SET [settingValue] = '5.0.13' WHERE [settingKey] = 'db_version'


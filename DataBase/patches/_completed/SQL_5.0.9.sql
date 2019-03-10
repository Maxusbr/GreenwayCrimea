GO-- 
alter table [Catalog].[ProductExt]
alter column MaxAvailable float

GO--
ALTER PROCEDURE [Catalog].[PreCalcProductParams] @productId INT    
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
	SELECT Count(ReviewId) From CMS.Review Where EntityId = @productId And Checked = 1  
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


exec [Catalog].[PreCalcProductParamsMass]
GO--

UPDATE [Settings].[InternalSettings] SET [settingValue] = '5.0.9' WHERE [settingKey] = 'db_version'

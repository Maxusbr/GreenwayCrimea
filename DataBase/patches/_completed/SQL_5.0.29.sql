
if not exists( select * from [CMS].[StaticBlock] where [key] = 'SalesInProductListSale')
insert into [CMS].[StaticBlock] ([Key],[InnerName],[Content],[Added],[Modified],[Enabled]) Values('SalesInProductListSale','Блок на странице скидок','',GETDATE(), GETDATE(), 0)

GO--

alter table Catalog.ProductList add Description nvarchar(max) null

GO--

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
               [Colors] =
         (
             SELECT [Settings].[ProductColorsToString]([ProductExt].ProductId)
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

	 UPDATE [Catalog].[ProductExt]
	SET [PriceTemp] = (
			SELECT ([MinPrice] - [MinPrice] * [Product].Discount / 100) * CurrencyValue
			FROM CATALOG.product
			INNER JOIN CATALOG.Currency ON product.currencyid = Currency.currencyid
			WHERE product.productid = [ProductExt].ProductId
			)
			
	  UPDATE [Catalog].[ProductExt]
	SET [AmountSort] = (SELECT Top(1) CASE    
		 WHEN MaxAvailable <= 0    
		OR MaxAvailable < IsNull(Product.MinAmount, 0) THEN 0    
		 ELSE 1    
		END    
	  FROM [Catalog].Offer    
	  INNER JOIN [Catalog].Product ON Product.ProductId = Offer.ProductId    
	  WHERE Offer.ProductId = [ProductExt].ProductId AND main = 1)		
END

GO--

ALTER PROCEDURE [Catalog].[PreCalcProductParams] 
    @productId INT,
    @ModerateReviews BIT,
	@OnlyAvailable BIT
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
 
 --AmountSort 
 
 UPDATE [Catalog].[ProductExt]
	SET [AmountSort] = (SELECT Top(1) CASE    
		 WHEN MaxAvailable <= 0    
		OR MaxAvailable < IsNull(Product.MinAmount, 0) THEN 0    
		 ELSE 1    
		END    
	  FROM [Catalog].Offer    
	  INNER JOIN [Catalog].Product ON Product.ProductId = Offer.ProductId    
	  WHERE Offer.ProductId = [ProductExt].ProductId AND main = 1)	
	   
  --@PriceTemp 
  
  UPDATE [Catalog].[ProductExt]
	SET [PriceTemp] = (
			SELECT ([MinPrice] - [MinPrice] * [Product].Discount / 100) * CurrencyValue
			FROM CATALOG.product
			INNER JOIN CATALOG.Currency ON product.currencyid = Currency.currencyid
			WHERE product.productid = [ProductExt].ProductId
			)    
END

GO--

update settings.settings
set value = replace (value, ' Нажимая кнопку "Продолжить", я подтверждаю свою дееспособность, даю согласие на обработку своих персональных данных.',
'Я подтверждаю свою дееспособность, даю согласие на обработку своих персональных данных.') where name = 'UserAgreementText' 


INSERT INTO Settings.Localization (LanguageId, ResourceKey, ResourceValue) VALUES (1, 'Js.Subscribe.ErrorAgreement','Необходимо согласиться с пользовательским соглашением')

INSERT INTO Settings.Localization (LanguageId, ResourceKey, ResourceValue) VALUES (2, 'Js.Subscribe.ErrorAgreement','You must agree with the user agreement')
	
GO--

IF ((Select Count(ResourceValue) from Settings.Localization where ResourceKey = 'Js.Modal.Close') = 0)
	BEGIN
		INSERT INTO Settings.Localization (LanguageId, ResourceKey, ResourceValue) VALUES (1, 'Js.Modal.Close','Закрыть')
		INSERT INTO Settings.Localization (LanguageId, ResourceKey, ResourceValue) VALUES (2, 'Js.Modal.Close','Close')
	END

GO--

if((select Count(*) from Settings.Localization where ResourceKey = 'BuyMore.NextAction.NotHaveEnough') = 0) 
Begin 
	INSERT INTO Settings.Localization (LanguageId, ResourceKey, ResourceValue) VALUES (1, 'BuyMore.NextAction.NotHaveEnough','Вам не хватает еще') 
End

if((select Count(*) from Settings.Localization where ResourceKey = 'BuyMore.NextAction.ToReceive') = 0) 
Begin 
	INSERT INTO Settings.Localization (LanguageId, ResourceKey, ResourceValue) VALUES (1, 'BuyMore.NextAction.ToReceive','чтобы получить') 
End

if((select Count(*) from Settings.Localization where ResourceKey = 'BuyMore.NextAction.FreeShipping') = 0) 
Begin 
	INSERT INTO Settings.Localization (LanguageId, ResourceKey, ResourceValue) VALUES (1, 'BuyMore.NextAction.FreeShipping','бесплатную доставку') 
End

if((select Count(*) from Settings.Localization where ResourceKey = 'BuyMore.NextAction.Discount') = 0) 
Begin 
	INSERT INTO Settings.Localization (LanguageId, ResourceKey, ResourceValue) VALUES (1, 'BuyMore.NextAction.Discount','скидку') 
End

if((select Count(*) from Settings.Localization where ResourceKey = 'BuyMore.NextAction.Gift') = 0) 
Begin 
	INSERT INTO Settings.Localization (LanguageId, ResourceKey, ResourceValue) VALUES (1, 'BuyMore.NextAction.Gift','подарок') 
End

if((select Count(*) from Settings.Localization where ResourceKey = 'BuyMore.CurrentAction.AmountIsMore') = 0) 
Begin 
	INSERT INTO Settings.Localization (LanguageId, ResourceKey, ResourceValue) VALUES (1, 'BuyMore.CurrentAction.AmountIsMore','Сумма вашего заказа более') 
End

if((select Count(*) from Settings.Localization where ResourceKey = 'BuyMore.CurrentAction.YouReceive') = 0) 
Begin 
INSERT INTO Settings.Localization (LanguageId, ResourceKey, ResourceValue) VALUES (1, 'BuyMore.CurrentAction.YouReceive','поэтому вы получаете:') 
End

if((select Count(*) from Settings.Localization where ResourceKey = 'BuyMore.CurrentAction.FreeShipping') = 0) 
Begin 
	INSERT INTO Settings.Localization (LanguageId, ResourceKey, ResourceValue) VALUES (1, 'BuyMore.CurrentAction.FreeShipping','бесплатную доставку') 
End

if((select Count(*) from Settings.Localization where ResourceKey = 'BuyMore.CurrentAction.Discount') = 0) 
Begin 
	INSERT INTO Settings.Localization (LanguageId, ResourceKey, ResourceValue) VALUES (1, 'BuyMore.CurrentAction.Discount','скидку') 
End

if((select Count(*) from Settings.Localization where ResourceKey = 'BuyMore.CurrentAction.Gift') = 0) 
Begin 
	INSERT INTO Settings.Localization (LanguageId, ResourceKey, ResourceValue) VALUES (1, 'BuyMore.CurrentAction.Gift','подарок') 
End

if((select Count(*) from Settings.Localization where ResourceKey = 'BuyMore.NextAction.NotHaveEnough') = 0) 
Begin 
	INSERT INTO Settings.Localization (LanguageId, ResourceKey, ResourceValue) VALUES (2, 'BuyMore.NextAction.NotHaveEnough','You do not have enough') 
End

if((select Count(*) from Settings.Localization where ResourceKey = 'BuyMore.NextAction.ToReceive') = 0) 
Begin 
	INSERT INTO Settings.Localization (LanguageId, ResourceKey, ResourceValue) VALUES (2, 'BuyMore.NextAction.ToReceive','to receive') 
End

if((select Count(*) from Settings.Localization where ResourceKey = 'BuyMore.NextAction.FreeShipping') = 0) 
Begin 
	INSERT INTO Settings.Localization (LanguageId, ResourceKey, ResourceValue) VALUES (2, 'BuyMore.NextAction.FreeShipping','free shipping') 
End

if((select Count(*) from Settings.Localization where ResourceKey = 'BuyMore.NextAction.Discount') = 0) 
Begin 
	INSERT INTO Settings.Localization (LanguageId, ResourceKey, ResourceValue) VALUES (2, 'BuyMore.NextAction.Discount','discount') 
End

if((select Count(*) from Settings.Localization where ResourceKey = 'BuyMore.NextAction.Gift') = 0) 
Begin 
	INSERT INTO Settings.Localization (LanguageId, ResourceKey, ResourceValue) VALUES (2, 'BuyMore.NextAction.Gift','gift') 
End

if((select Count(*) from Settings.Localization where ResourceKey = 'BuyMore.CurrentAction.AmountIsMore') = 0) 
Begin 
	INSERT INTO Settings.Localization (LanguageId, ResourceKey, ResourceValue) VALUES (2, 'BuyMore.CurrentAction.AmountIsMore','The amount of your order is more') 
End

if((select Count(*) from Settings.Localization where ResourceKey = 'BuyMore.CurrentAction.YouReceive') = 0) 
Begin 
	INSERT INTO Settings.Localization (LanguageId, ResourceKey, ResourceValue) VALUES (2, 'BuyMore.CurrentAction.YouReceive','therefore you receive:') 
End

if((select Count(*) from Settings.Localization where ResourceKey = 'BuyMore.CurrentAction.FreeShipping') = 0) 
Begin 
	INSERT INTO Settings.Localization (LanguageId, ResourceKey, ResourceValue) VALUES (2, 'BuyMore.CurrentAction.FreeShipping','free shipping') 
End

if((select Count(*) from Settings.Localization where ResourceKey = 'BuyMore.CurrentAction.Discount') = 0) 
Begin 
	INSERT INTO Settings.Localization (LanguageId, ResourceKey, ResourceValue) VALUES (2, 'BuyMore.CurrentAction.Discount','discount') 
End

if((select Count(*) from Settings.Localization where ResourceKey = 'BuyMore.CurrentAction.Gift') = 0) 
Begin 
	INSERT INTO Settings.Localization (LanguageId, ResourceKey, ResourceValue) VALUES (2, 'BuyMore.CurrentAction.Gift','gift') 
End

GO--

UPDATE [Settings].[InternalSettings] SET [settingValue] = '5.0.29' WHERE [settingKey] = 'db_version'
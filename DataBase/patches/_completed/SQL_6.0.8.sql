alter table [Order].orderitems alter column ArtNo nvarchar(100) not null
GO--
alter table [Order].orderitems alter column Color nvarchar(300) null
GO--
alter table [Order].orderitems alter column Size nvarchar(300) null
GO--
alter table [Order].[OrderItems] add Length float null, Width float null, Height float null
GO--
alter table [Order].[LeadItem] add Length float null, Width float null, Height float null
GO--
ALTER PROCEDURE [Order].[sp_UpdateOrderItem]  
	@OrderItemID int,  
	@OrderID int,  
	@Name nvarchar(255),  
	@Price float,  
	@Amount float,  
	@ProductID int,  
	@ArtNo nvarchar(100),  
	@SupplyPrice float,  
	@Weight float,  
	@IsCouponApplied bit,  
	@Color nvarchar(300),  
	@Size nvarchar(300),  
	@DecrementedAmount float,  
	@PhotoID int,  
	@IgnoreOrderDiscount bit,  
	@AccrueBonuses bit,
	@TaxId int,
	@TaxName nvarchar(50),
	@TaxType int,
	@TaxRate float(53),
	@TaxShowInPrice bit,
	@Width float,
	@Height float,
	@Length float
AS  
BEGIN  
 Update [Order].[OrderItems]  
 Set  
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
	,TaxId = @TaxId
	,TaxName = @TaxName
	,TaxType = @TaxType
	,TaxRate = @TaxRate
	,TaxShowInPrice = @TaxShowInPrice
	,Width = @Width
	,Height = @Height
	,[Length] = @Length
 Where OrderItemID = @OrderItemID  
END  
GO--
ALTER PROCEDURE [Order].[sp_AddOrderItem]  
	 @OrderID int,  
	 @Name nvarchar(255),  
	 @Price float,  
	 @Amount float,  
	 @ProductID int,  
	 @ArtNo nvarchar(100),  
	 @SupplyPrice float,  
	 @Weight float,  
	 @IsCouponApplied bit,  
	 @Color nvarchar(300),  
	 @Size nvarchar(300),  
	 @DecrementedAmount float,  
	 @PhotoID int,  
	 @IgnoreOrderDiscount bit,  
	 @AccrueBonuses bit,
	 @TaxId int,
	 @TaxName nvarchar(50),
	 @TaxType int,
	 @TaxRate float(53),
	 @TaxShowInPrice bit,
	 @Width float,
	 @Height float,
	 @Length float
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
	   ,TaxId
	   ,TaxName
	   ,TaxType
	   ,TaxRate
	   ,TaxShowInPrice
	   ,Width
	   ,Height
	   ,[Length]
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
	   ,@TaxId
	   ,@TaxName
	   ,@TaxType
	   ,@TaxRate
	   ,@TaxShowInPrice  
	   ,@Width
	   ,@Height
	   ,@Length 
   );  
       
 SELECT SCOPE_IDENTITY()  
END

GO--

update settings.Localization set ResourceValue = 'Доступны переменные о заказе и покупателе: <br> #ORDER_ID#, #ORDER_SUM#, #SHIPPING_SUM#, #CUSTOMER_EMAIL#, #CUSTOMER_FIRSTNAME#, #CUSTOMER_LASTNAME#, #CUSTOMER_PHONE#, #CUSTOMER_ID#<br><br> Строка товара помещается между тегами "&lt;&lt;" и "&gt;&gt;" и доступны переменные: #PRODUCT_ARTNO#, #PRODUCT_NAME#,  #PRODUCT_PRICE#, #PRODUCT_AMOUNT#' where ResourceKey = 'Admin.Settings.Checkout.CheckoutSuccessScriptVariables' and LanguageId = 1
update settings.Localization set ResourceValue = 'Available variables for the order and the customer:<br /> #ORDER_ID#, #ORDER_SUM#, #SHIPPING_SUM#, #CUSTOMER_EMAIL#, #CUSTOMER_FIRSTNAME#, #CUSTOMER_LASTNAME#, #CUSTOMER_PHONE#, #CUSTOMER_ID#<br /><br /> Product line is placed between "<<" and ">>" tags. Available variables for products: #PRODUCT_ARTNO#, #PRODUCT_NAME#,  #PRODUCT_PRICE#, #PRODUCT_AMOUNT#' where ResourceKey = 'Admin.Settings.Checkout.CheckoutSuccessScriptVariables' and LanguageId = 2
GO--

ALTER TABLE [Order].Lead ADD
	ModifiedDate datetime NULL
GO--

Update [Order].Lead Set ModifiedDate = CreatedDate
GO--
update settings.Localization set ResourceValue = 'Артикул:Размер:Цвет:Цена:ЗакупочнаяЦена:Наличие' 
where ResourceKey = 'Core.ExportImport.ProductFields.MultiOffer' and LanguageId = 1
GO--
ALTER PROCEDURE [Catalog].[sp_GetPropertyInFilter] (@categoryid INT, @usedepth bit)
AS
BEGIN
 IF (@usedepth = 1)
 BEGIN
     ;WITH ppvid (propertyvalueid)
          AS
          (
           select DISTINCT ppv.propertyvalueid from [catalog].product p 
                           inner join [catalog].[productpropertyvalue] ppv on p.productid = ppv.productid  AND p.[enabled] = 1
                           inner join [catalog].[productcategories] pc on p.productid=pc.productid
                           inner join [settings].[getchildcategorybyparent] (@categoryid) ch on pc.[categoryid] = ch.id              
          )
          
        SELECT     pv.[propertyvalueid],
                   pv.[propertyid],
                   pv.[value],
                   pv.[rangevalue],
                   p.name                 AS propertyname,
                   p.sortorder            AS propertysortorder,
                   p.expanded             AS propertyexpanded,
                   p.unit                 AS propertyunit,
                   p.TYPE                 AS propertytype,
                   p.Description          AS propertydescription,
				   p.NameDisplayed		  AS PropertyNameDisplayed
                  
        FROM       [catalog].[propertyvalue] pv
        INNER JOIN [catalog].[property] p ON p.propertyid = pv.propertyid AND p.[useinfilter] = 1    
        inner join ppvid on ppvid.propertyvalueid = pv.[propertyvalueid]
                      
        WHERE      pv.[useinfilter] = 1                  
        ORDER BY   propertysortorder,
                   pv.[propertyid],
                   pv.[sortorder],
                   pv.[rangevalue],
                   pv.[value]
 END
 ELSE
 BEGIN
 
    ;WITH ppvid (propertyvalueid)
         AS
         (
         select DISTINCT ppv.propertyvalueid from [catalog].product p 
                           inner join [catalog].[productpropertyvalue] ppv on p.productid = ppv.productid  AND p.[enabled] = 1
                           inner join [catalog].[productcategories] pc on p.productid=pc.productid and pc.[categoryid]=@categoryid            
         )
        
    SELECT     pv.[propertyvalueid],
               pv.[propertyid],
               pv.[value],
               pv.[rangevalue],
               p.[name]               AS propertyname,
               p.sortorder            AS propertysortorder,
               p.expanded             AS propertyexpanded,
               p.unit                 AS propertyunit,
               p.[TYPE]               AS propertytype,
               p.[Description]        AS propertydescription,
			   p.NameDisplayed		  AS PropertyNameDisplayed
              
    FROM       [catalog].[propertyvalue] pv
    INNER JOIN [catalog].[property] p    ON p.propertyid = pv.propertyid AND p.[useinfilter] = 1
    inner join ppvid on ppvid.propertyvalueid = pv.[propertyvalueid]
    WHERE      pv.[useinfilter] = 1              
    ORDER BY   propertysortorder,
               pv.[propertyid],
               pv.[sortorder],
               pv.[rangevalue],
               pv.[value]              
 END
END
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
             SELECT CASE WHEN [Product].Discount > 0 THEN (@MinPrice - @MinPrice * [Product].Discount / 100) * CurrencyValue ELSE @MinPrice - [Product].DiscountAmount END
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
ALTER PROCEDURE [Catalog].[PreCalcProductParamsMass] @ModerateReviews BIT, @OnlyAvailable bit AS 
BEGIN
   
INSERT INTO
   [Catalog].[ProductExt] (ProductId, CountPhoto, PhotoId, VideosAvailable, MaxAvailable, NotSamePrices, MinPrice, Colors, AmountSort, OfferId, Comments, CategoryId) (
   SELECT
      ProductId, 0, NULL, 0, 0, 0, 0, NULL, 0, NULL, 0, NULL 
   FROM
      [Catalog].Product 
   WHERE
      Product.ProductId NOT IN 
      (
         SELECT
            ProductId 
         FROM
            [Catalog].[ProductExt]
      )
) 
      UPDATE
         catalog.ProductExt 
      SET
         ProductExt.[CountPhoto] = tempTable.CountPhoto,
         ProductExt.[PhotoId] = tempTable.[PhotoId],
         ProductExt.[VideosAvailable] = tempTable.[VideosAvailable],
         ProductExt.[MaxAvailable] = tempTable.[MaxAvailable],
         ProductExt.[NotSamePrices] = tempTable.[NotSamePrices],
         ProductExt.[MinPrice] = tempTable.[MinPrice],
         ProductExt.[OfferId] = tempTable.[OfferId],
         ProductExt.[Comments] = tempTable.[Comments],
         ProductExt.[Gifts] = tempTable.[Gifts],
		 ProductExt.[Colors] = tempTable.[Colors],
         ProductExt.[CategoryId] = tempTable.[CategoryId] ,
		 ProductExt.PriceTemp = tempTable.PriceTemp,
		 ProductExt.AmountSort=tempTable.AmountSort
      FROM
         catalog.ProductExt 
         INNER JOIN
            (               
           select 
		pe.ProductId,
		CountPhoto=case when offerId.ColorID is null then countNocolor.countNocolor else countColor.countColor end,				
		PhotoId=case when offerId.ColorID is null then PhotoIdNoColor.PhotoIdNoColor else PhotoIdColor.PhotoIdColor end,
		[VideosAvailable]=isnull(videosAvailable.videosAvailable,0),
		[MaxAvailable]=maxAvailable.maxAvailable,
		[NotSamePrices]=isnull(notSamePrices.notSamePrices,0),
		[MinPrice]=isnull(minPrice.minPrice,0),
		[OfferId]=offerId.OfferId,
		[Comments]=isnull(comments.comments,0),
		[Gifts]=isnull(gifts.gifts,0),
		[Colors]=(SELECT [Settings].[ProductColorsToString](pe.ProductId)),
		[CategoryId] = (SELECT TOP 1 id	FROM [Settings].[GetParentsCategoryByChild](( SELECT TOP 1 CategoryID FROM [Catalog].ProductCategories	WHERE ProductID = pe.ProductId ORDER BY Main DESC))ORDER BY sort DESC),
		PriceTemp = CASE WHEN p.Discount > 0 THEN (isnull(minPrice.minPrice,0) - isnull(minPrice.minPrice,0) * p.Discount/100)*c.CurrencyValue ELSE (isnull(minPrice.minPrice,0) - p.DiscountAmount)*c.CurrencyValue END,
		AmountSort=CASE when maxAvailable.maxAvailable <= 0 OR maxAvailable.maxAvailable < IsNull(p.MinAmount, 0) THEN 0 ELSE 1 end

from Catalog.[ProductExt] pe
left join (SELECT o.ProductId, COUNT(*) countColor FROM [Catalog].[Photo] ph INNER JOIN [Catalog].[Offer] o  ON ph.[ObjId] = o.ProductId 
            WHERE ( ph.ColorID = o.ColorID OR ph.ColorID IS NULL ) AND TYPE = 'Product' AND o.Main = 1 
			group by o.ProductId
) countColor on pe.ProductId=countColor.ProductId

left join (SELECT [ObjId], COUNT(*) countNocolor FROM [Catalog].[Photo]  
            WHERE TYPE = 'Product'
			group by [ObjId]
) countNocolor on pe.ProductId=countNocolor.[ObjId]

left join (
select ProductId, PhotoId PhotoIdColor from (
SELECT o.ProductId, ph.PhotoId, Row_Number() over (PARTITION  by o.ProductId ORDER BY ph.main DESC ,ph.[PhotoSortOrder], ph.[PhotoId]) rn FROM [Catalog].[Photo] ph
							INNER JOIN [Catalog].[Offer] o ON ph.[ObjId] = o.ProductId
							WHERE (ph.ColorID = o.ColorID OR ph.ColorID IS NULL) AND TYPE = 'Product' ) ct where rn=1
) PhotoIdColor on pe.ProductId=PhotoIdColor.ProductId

left join (
select ProductId, PhotoId PhotoIdNoColor from (
SELECT ph.[ObjId] ProductId, ph.PhotoId, Row_Number() over (PARTITION  by ph.[ObjId] ORDER BY ph.main DESC ,ph.[PhotoSortOrder], ph.[PhotoId]) rn FROM [Catalog].[Photo] ph	WHERE TYPE = 'Product' ) ct where rn=1
) PhotoIdNoColor on pe.ProductId=PhotoIdNoColor.ProductId

left join (select pv.ProductID, CASE WHEN COUNT(pv.ProductVideoID) > 0 THEN 1	ELSE 0 END videosAvailable FROM [Catalog].[ProductVideo] pv group by pv.ProductID) videosAvailable on pe.ProductId=videosAvailable.ProductId
left join (select o.ProductID,Max(o.Amount) maxAvailable  FROM [Catalog].Offer o group by o.ProductID) maxAvailable on pe.ProductId=maxAvailable.ProductId
left join (select o.ProductID, CASE WHEN MAX(o.price) - MIN(o.price) > 0 THEN 1 ELSE 0 END notSamePrices  FROM [Catalog].Offer o where o.price > 0 AND (@OnlyAvailable = 0 OR o.amount > 0) group by o.ProductID) notSamePrices on pe.ProductId=notSamePrices.ProductId
left join (select o.ProductID,MIN(o.price) minPrice FROM [Catalog].Offer o where o.price > 0 AND (@OnlyAvailable = 0 OR o.amount > 0)  group by o.ProductID) minPrice on pe.ProductId=minPrice.ProductId
left join ( 
select ProductId, OfferID, colorId from (
select o.ProductID,o.OfferID, o.colorId, Row_Number() over (PARTITION  by o.OfferID ORDER BY o.OfferID) rn  FROM [Catalog].Offer o where o.Main = 1 )ct where rn=1
) offerId on pe.ProductId=offerId.ProductId
left join (select EntityId ProductID,count(ReviewId) comments  FROM CMS.Review  where (Checked = 1 OR @ModerateReviews = 0) group by EntityId) comments on pe.ProductId=comments.ProductId
left join (select pg.ProductID, CASE WHEN COUNT(pg.ProductID) > 0 THEN 1 ELSE 0 END gifts  FROM [Catalog].[ProductGifts] pg group by pg.ProductID) gifts on pe.ProductId=gifts.ProductId
inner join catalog.Product p on p.ProductID = pe.ProductID
INNER JOIN CATALOG.Currency c ON p.currencyid = c.currencyid
 )
AS tempTable 
ON tempTable.ProductId = ProductExt.ProductId 
END
GO--
UPDATE [Settings].[InternalSettings] SET [settingValue] = '6.0.8' WHERE [settingKey] = 'db_version'

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



update [Settings].[ModuleSettings] set [Value] = 'False' where Name = 'Process301Redirect' and ModuleName = 'yandexmarketimport'

GO--

update [Settings].[MailFormatType] set [Comment] = 'При смене статуса заказа ( #ORDERID# ; #ORDERSTATUS#; #STATUSCOMMENT#; #NUMBER#, #ORDERTABLE#, #TRACKNUMBER# )' where MailType = 'OnChangeOrderStatus'

GO--

If (NOT EXISTS(Select * From [CMS].[StaticBlock] Where [Key] = 'head'))
Begin
	Insert Into [CMS].[StaticBlock] ([Key],[InnerName],[Content],[Added],[Modified],[Enabled]) Values ('head', 'Блок в head', '', GETDATE(), GETDATE(), 1)
End

GO--

	
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Js.Order.Distance', 'Расстояние');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Js.Order.Distance', 'Distance');

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

INSERT INTO [CMS].[StaticBlock] ([Key],InnerName,Content,Added,Modified,Enabled) 
VALUES ('MobileOrderSuccessTop','Успешное оформление заказа (блок сверху, мобильная версия)', '<div>Ваш заказ</div><div class="checkout-confirm-number">№ #ORDER_ID#</div>',
GETDATE(),GETDATE(),1)

GO--


update [CMS].[StaticBlock] set Content = Replace(Content, '2016', '2017') where [Key] = 'LeftBottom'
update [CMS].[StaticBlock] set Content = Replace(Content, '2015', '2017') where [Key] = 'LeftBottom'
update [CMS].[StaticBlock] set Content = Replace(Content, '2014', '2017') where [Key] = 'LeftBottom'
update [CMS].[StaticBlock] set Content = Replace(Content, 'AdVantShop.NET', 'AdvantShop') where [Key] = 'LeftBottom'

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


declare @MosOblId int
set  @MosOblId = (select top 1 [RegionID] from [Customers].[Region] where [RegionName] = 'Московская область')


if(not exists (select * from [Customers].City where [CityName]='Абрамовка' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Абрамовка',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Абрамцево' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Абрамцево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Авдеево' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Авдеево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Авдотьино' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Авдотьино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Авсюнино' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Авсюнино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Акатьево' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Акатьево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Алабино' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Алабино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Алабушево' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Алабушево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Александрово' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Александрово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Алексино' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Алексино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Алексино-Шатур' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Алексино-Шатур',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Алпатьево' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Алпатьево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Алферьево' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Алферьево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Андреевка' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Андреевка',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Апрелевка' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Апрелевка',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Астапово' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Астапово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Атепцево' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Атепцево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Афанасовка' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Афанасовка',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Ашитково' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Ашитково',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Ашукино' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Ашукино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Бабенки' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Бабенки',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Бакшеево' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Бакшеево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Балашиха' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Балашиха',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Барабаново' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Барабаново',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Барановское' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Барановское',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Барвиха' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Барвиха',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Барвиха Санаторий' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Барвиха Санаторий',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Барыбино' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Барыбино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Барынино' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Барынино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Беззубово' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Беззубово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Бекасово' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Бекасово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Белоозерский' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Белоозерский',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Белоомут' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Белоомут',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Белые Колодези' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Белые Колодези',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Белые Столбы' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Белые Столбы',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Белый Раст' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Белый Раст',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Беляная Гора' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Беляная Гора',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Березка Дом отдыха' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Березка Дом отдыха',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Березняки' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Березняки',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Биорки' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Биорки',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Бирево' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Бирево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Бисерово' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Бисерово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Бобково' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Бобково',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Богатищево' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Богатищево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Болычево' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Болычево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Большие Вяземы' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Большие Вяземы',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Большие Дворы' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Большие Дворы',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Большое Алексеевское' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Большое Алексеевское',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Большое Гридино' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Большое Гридино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Большое Грызлово' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Большое Грызлово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Борисово' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Борисово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Боровково' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Боровково',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Бородино' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Бородино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Бортниково' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Бортниково',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Ботово' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Ботово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Бояркино' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Бояркино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Братовщина' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Братовщина',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Бронницы' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Бронницы',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Буденовец' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Буденовец',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Бужаниново' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Бужаниново',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Бужарово' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Бужарово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Буньково' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Буньково',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Бунятино' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Бунятино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Бурцево' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Бурцево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Васькино' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Васькино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Великий Двор' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Великий Двор',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Вельяминово' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Вельяминово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Вербилки' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Вербилки',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Верея' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Верея',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Веселево' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Веселево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Видное' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Видное',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Вишняковские Дачи' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Вишняковские Дачи',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Вождь Пролетариата' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Вождь Пролетариата',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Волково' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Волково',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Волоколамск' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Волоколамск',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Волченки' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Волченки',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Воробьево' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Воробьево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Вороново' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Вороново',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Воскресенск' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Воскресенск',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Востряково' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Востряково',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Высоковск' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Высоковск',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Вышегород' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Вышегород',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Ганусово' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Ганусово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Гарь-Покровское' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Гарь-Покровское',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Гидроузла Поселок' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Гидроузла Поселок',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Глубокое' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Глубокое',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Голицыно' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Голицыно',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Головково' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Головково',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Горбово Фабрика' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Горбово Фабрика',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Горетово' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Горетово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Горки-Коломенские' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Горки-Коломенские',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Горловка' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Горловка',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Городище' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Городище',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Горшково' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Горшково',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Гришино' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Гришино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Губино' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Губино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Давыдково' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Давыдково',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Давыдово' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Давыдово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Дарищи' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Дарищи',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Дашковка' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Дашковка',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Дворики' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Дворики',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Деденево' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Деденево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Дединово' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Дединово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Дедовск' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Дедовск',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Демихово' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Демихово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Денежниково' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Денежниково',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Деньково' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Деньково',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Дзержинский' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Дзержинский',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Дмитров' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Дмитров',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Дмитрово' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Дмитрово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Долгопрудный' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Долгопрудный',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Домодедово' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Домодедово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Донино' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Донино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Дорохово' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Дорохово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Дрезна' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Дрезна',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Дубки' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Дубки',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Дубна' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Дубна',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Дубнево' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Дубнево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Дубровицы' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Дубровицы',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Дурыкино' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Дурыкино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Духанино' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Духанино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Дютьково' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Дютьково',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Евсеево' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Евсеево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Егорьевск' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Егорьевск',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Елгозино' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Елгозино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Елизарово' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Елизарово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Ельдигино' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Ельдигино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Ерново' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Ерново',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Ершово' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Ершово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Ефремовская' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Ефремовская',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Жаворонки' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Жаворонки',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Железнодорожный' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Железнодорожный',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Житнево' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Житнево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Жуковский' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Жуковский',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Журавна' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Журавна',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Заветы Ильича' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Заветы Ильича',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Заворово' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Заворово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Загорские Дали' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Загорские Дали',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Закубежье' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Закубежье',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Заовражье' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Заовражье',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Запрудня' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Запрудня',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Зарайск' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Зарайск',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Заря Коммунизма' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Заря Коммунизма',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Захарово' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Захарово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Звенигород' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Звенигород',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Зверосовхоз' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Зверосовхоз',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Зеленая Роща' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Зеленая Роща',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Зеленоградский' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Зеленоградский',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Зеленый' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Зеленый',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Зендиково' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Зендиково',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Знамя Октября' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Знамя Октября',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Зыково' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Зыково',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Ивакино' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Ивакино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Ивановка' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Ивановка',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Ивантеевка' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Ивантеевка',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Ильинский' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Ильинский',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Ильинский Погост' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Ильинский Погост',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Ильинское' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Ильинское',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Ильинское-Теряевское' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Ильинское-Теряевское',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Ильинское-Усово' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Ильинское-Усово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Ильинское-Ярополецкое' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Ильинское-Ярополецкое',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Индустрия' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Индустрия',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Истра' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Истра',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Кабаново' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Кабаново',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Калининец' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Калининец',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Калистово' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Калистово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Каменское' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Каменское',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Каринское' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Каринское',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Кашино' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Кашино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Кашира' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Кашира',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Клеменово' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Клеменово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Клементьево' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Клементьево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Клемово Совхоз' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Клемово Совхоз',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Кленово' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Кленово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Климовск' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Климовск',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Клин' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Клин',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Клязьма' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Клязьма',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Княжево' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Княжево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Кокино' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Кокино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Кокошкино' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Кокошкино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Коломна' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Коломна',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Колычево' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Колычево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Колюбакино' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Колюбакино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Конобеево' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Конобеево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Королев' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Королев',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Корыстово' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Корыстово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Костомарово' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Костомарово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Косяево' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Косяево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Котельники' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Котельники',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Кошелево' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Кошелево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Красная Гора' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Красная Гора',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Красная Заря' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Красная Заря',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Красная Пойма' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Красная Пойма',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Красноармейск' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Красноармейск',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Красновидово' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Красновидово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Красногорск' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Красногорск',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Краснозаводск' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Краснозаводск',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Краснознаменск' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Краснознаменск',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Красный Ткач' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Красный Ткач',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Красный Холм' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Красный Холм',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Кратово' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Кратово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Кривандино' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Кривандино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Крутое' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Крутое',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Кубинка' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Кубинка',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Кузьмино' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Кузьмино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Куровское' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Куровское',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Куртино' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Куртино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Курьяново' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Курьяново',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Ладыгино' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Ладыгино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Леньково' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Леньково',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Леонтьево' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Леонтьево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Лесное Озеро' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Лесное Озеро',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Летний отдых' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Летний отдых',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Летуново' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Летуново',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Ликино-Дулево' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Ликино-Дулево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Липино' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Липино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Липицы' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Липицы',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Литвиново' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Литвиново',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Лобня' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Лобня',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Логиново' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Логиново',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Лоза' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Лоза',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Лопатинский' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Лопатинский',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Лосино-Петровский' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Лосино-Петровский',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Луговой Поселок' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Луговой Поселок',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Лужники' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Лужники',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Лукерьино' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Лукерьино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Лукино' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Лукино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Лукошкино' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Лукошкино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Лукьяново' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Лукьяново',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Лунев' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Лунев',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Луховицы' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Луховицы',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Лыткарино' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Лыткарино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Лыткино' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Лыткино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Лыщиково' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Лыщиково',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Львовский' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Львовский',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Люберцы' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Люберцы',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Любучаны' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Любучаны',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Макеево' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Макеево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Макшеево' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Макшеево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Малая Дубна' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Малая Дубна',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Маливо' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Маливо',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Малышево' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Малышево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Мамонтовка' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Мамонтовка',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Мамонтово' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Мамонтово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Манихино' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Манихино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Мансурово' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Мансурово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Марушкино' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Марушкино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Марфин Брод' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Марфин Брод',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Маслово Совхоз' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Маслово Совхоз',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Менделеево' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Менделеево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Мендюкино' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Мендюкино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Мещерино' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Мещерино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Мещерское' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Мещерское',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Мир Совхоз' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Мир Совхоз',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Мисцево' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Мисцево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Мисцево-Куровское' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Мисцево-Куровское',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Митякино' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Митякино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Михайловское' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Михайловское',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Мишеронский' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Мишеронский',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Мишутино' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Мишутино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Можайск' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Можайск',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Молодежный' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Молодежный',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Москвич' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Москвич',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Московский' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Московский',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Мостовик' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Мостовик',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Мураново' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Мураново',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Муханово' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Муханово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Мытищи' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Мытищи',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Назарьево' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Назарьево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Наро-Фоминск' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Наро-Фоминск',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Нарский' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Нарский',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Нарынка' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Нарынка',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Нахабино' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Нахабино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Некрасовский' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Некрасовский',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Нелидово' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Нелидово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Непецино' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Непецино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Нерастанное' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Нерастанное',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Нижнее Хорошево' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Нижнее Хорошево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Никитское' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Никитское',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Николо-Кропотки' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Николо-Кропотки',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Никольское-Гагарино' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Никольское-Гагарино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Никоновское' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Никоновское',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Новая Деревня' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Новая Деревня',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Новая Ольховка' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Новая Ольховка',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Новобратцевский' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Новобратцевский',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Новоегорий' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Новоегорий',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Новозагарье' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Новозагарье',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Новопетровское' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Новопетровское',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Новостройка' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Новостройка',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Новохаритоново' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Новохаритоново',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Новый Быт' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Новый Быт',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Ногинск' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Ногинск',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Обухово' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Обухово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Одинцово' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Одинцово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Одинцово-Вахромеево' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Одинцово-Вахромеево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Ожерелье' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Ожерелье',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Озерецкое' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Озерецкое',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Озеро Белое Санаторий' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Озеро Белое Санаторий',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Озеры' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Озеры',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Ольгово' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Ольгово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Ольявидово' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Ольявидово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Онуфриево' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Онуфриево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Опалиха' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Опалиха',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Орехово-Зуево' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Орехово-Зуево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Орудьево' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Орудьево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Осаново-Дубовое' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Осаново-Дубовое',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Осташево' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Осташево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Павловская Слобода' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Павловская Слобода',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Павловский Посад' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Павловский Посад',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Первомайское' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Первомайское',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Пересвет' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Пересвет',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Перхушково' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Перхушково',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Петрово' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Петрово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Печерники' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Печерники',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Пирочи' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Пирочи',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Поварово' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Поварово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Подмосковье Санаторий' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Подмосковье Санаторий',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Подольск' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Подольск',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Подосинки' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Подосинки',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Подхожее' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Подхожее',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Подъячево' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Подъячево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Покровка' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Покровка',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Покровское-Шереметьево' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Покровское-Шереметьево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Полбино' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Полбино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Полуряденки' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Полуряденки',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Полушкино' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Полушкино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Поминово' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Поминово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Поречье' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Поречье',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Поречье Санаторий' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Поречье Санаторий',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Починки' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Починки',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Правдинский' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Правдинский',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Привокзальный' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Привокзальный',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Приволье' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Приволье',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Приокск' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Приокск',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Пролетарский' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Пролетарский',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Протвино' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Протвино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Протекино' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Протекино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Псарьки' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Псарьки',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Птичное' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Птичное',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Пустоша' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Пустоша',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Пушкино' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Пушкино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Пущино' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Пущино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Пышелицы' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Пышелицы',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Радовицкий' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Радовицкий',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Радужный' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Радужный',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Раменки' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Раменки',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Раменское' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Раменское',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Рассудово' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Рассудово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Рахманово' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Рахманово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Редькино' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Редькино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Реутов' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Реутов',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Речицы' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Речицы',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Решетниково' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Решетниково',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Решоткино' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Решоткино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Ржавки' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Ржавки',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Рогачево' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Рогачево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Родники' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Родники',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Рошаль' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Рошаль',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Руза' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Руза',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Рыбное' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Рыбное',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Рязаново' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Рязаново',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Саввинская Слобода' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Саввинская Слобода',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Сватково' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Сватково',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Селково' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Селково',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Селятино' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Селятино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Семеново' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Семеново',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Семхоз' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Семхоз',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Сенеж' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Сенеж',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Сенницы' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Сенницы',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Сергиев Посад' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Сергиев Посад',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Серебряные Пруды' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Серебряные Пруды',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Середниково' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Середниково',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Серпухов' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Серпухов',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Симбухово' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Симбухово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Синичино' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Синичино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Ситне-Щелканово' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Ситне-Щелканово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Скоропусковский' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Скоропусковский',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Слобода' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Слобода',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Сменки' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Сменки',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Снегири' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Снегири',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Соболево' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Соболево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Соколова Пустынь' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Соколова Пустынь',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Сокольниково' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Сокольниково',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Солнечногорск' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Солнечногорск',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Сосновка' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Сосновка',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Софрино' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Софрино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Спасс' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Спасс',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Спасс-Заулок' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Спасс-Заулок',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Спутник' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Спутник',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Старая Купавна' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Старая Купавна',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Старая Руза' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Старая Руза',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Старая Ситня' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Старая Ситня',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Стариково' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Стариково',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Старый Городок' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Старый Городок',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Стегачево' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Стегачево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Степановское' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Степановское',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Степанцево' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Степанцево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Степанщино' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Степанщино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Столбовая' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Столбовая',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Стремилово' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Стремилово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Струпна' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Струпна',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Ступино' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Ступино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Судниково' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Судниково',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Сычево' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Сычево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Талдом' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Талдом',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Тарасково' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Тарасково',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Тарбушево' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Тарбушево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Татариново' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Татариново',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Таширово' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Таширово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Темпы' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Темпы',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Теряево' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Теряево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Тимонино' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Тимонино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Тишково' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Тишково',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Толстяково' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Толстяково',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Топканово' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Топканово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Торгашино' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Торгашино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Троицк' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Троицк',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Тропарево' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Тропарево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Трудовая' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Трудовая',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Туголесский Бор' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Туголесский Бор',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Туменское' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Туменское',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Тучково' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Тучково',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Тютьково' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Тютьково',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Уваровка' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Уваровка',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Ударный' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Ударный',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Удельная' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Удельная',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Узуново' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Узуново',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Ульянино' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Ульянино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Усово' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Усово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Успенское' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Успенское',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Федорцово' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Федорцово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Федосьино' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Федосьино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Федюково' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Федюково',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Фрязево' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Фрязево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Фрязино' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Фрязино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Харлампеево' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Харлампеево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Хатунь' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Хатунь',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Химки' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Химки',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Холщевики' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Холщевики',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Хотьково' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Хотьково',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Хрипань' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Хрипань',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Цветковский Совхоз' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Цветковский Совхоз',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Чемодурово' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Чемодурово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Черкизово' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Черкизово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Чернево' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Чернево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Черноголовка' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Черноголовка',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Черусти' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Черусти',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Чехов' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Чехов',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Чисмена' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Чисмена',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Чурилково' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Чурилково',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Шаликово' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Шаликово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Шатура' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Шатура',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Шатурторф' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Шатурторф',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Шеино' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Шеино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Шестаково' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Шестаково',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Шубино' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Шубино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Шугарово' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Шугарово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Шустиково' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Шустиково',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Щелково' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Щелково',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Щербинка' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Щербинка',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Электрогорск' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Электрогорск',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Электросталь' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Электросталь',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Электроугли' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Электроугли',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Юбилейный' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Юбилейный',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Юрлово' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Юрлово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Юрцово' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Юрцово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Якимовка' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Якимовка',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Яковлево' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Яковлево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Яковское' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Яковское',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Якоть' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Якоть',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Ям' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Ям',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Ямкино' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Ямкино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Ярополец' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Ярополец',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Яхрома' and [RegionID] = @MosOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@MosOblId, 'Яхрома',0, 0)

GO--


declare @LenOblId int
set  @LenOblId = (select top 1 [RegionID] from [Customers].[Region] where [RegionName] = 'Ленинградская область')

if(not exists (select * from [Customers].City where [CityName]='Аврово' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Аврово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Агалатово' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Агалатово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Алексеевка' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Алексеевка',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Алексино' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Алексино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Алеховщина' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Алеховщина',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Андреево' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Андреево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Андреевщина' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Андреевщина',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Андрианово' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Андрианово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Андронниково' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Андронниково',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Анисимово' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Анисимово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Аннино' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Аннино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Апраксин Бор' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Апраксин Бор',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Бабино' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Бабино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Батово' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Батово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Бегуницы' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Бегуницы',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Белогорка' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Белогорка',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Белое' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Белое',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Бережки' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Бережки',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Березовик' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Березовик',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Беседа' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Беседа',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Бокситогорск' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Бокситогорск',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Большая Вруда' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Большая Вруда',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Большая Ижора' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Большая Ижора',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Большая Пустомержа' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Большая Пустомержа',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Большие Коковичи' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Большие Коковичи',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Большие Колпаны' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Большие Колпаны',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Большие Сабицы' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Большие Сабицы',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Большие Шатновичи' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Большие Шатновичи',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Большое Жабино' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Большое Жабино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Большое Куземкино' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Большое Куземкино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Большое Ондорово' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Большое Ондорово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Большое Поле' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Большое Поле',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Большой Двор' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Большой Двор',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Борисова Грива' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Борисова Грива',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Боровое' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Боровое',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Бугры' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Бугры',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Будогощь' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Будогощь',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Ваганово' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Ваганово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Важины' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Важины',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Васкелово' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Васкелово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Васьково' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Васьково',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Веймарн' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Веймарн',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Великий Двор' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Великий Двор',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Верево' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Верево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Вещево' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Вещево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Виллози' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Виллози',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Винницы' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Винницы',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Вистино' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Вистино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Владимировка' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Владимировка',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Воейково' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Воейково',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Вознесенье' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Вознесенье',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Возрождение' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Возрождение',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Войсковицы' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Войсковицы',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Войскорово' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Войскорово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Володарское' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Володарское',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Волосово' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Волосово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Волочаевка' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Волочаевка',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Волошово' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Волошово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Волхов' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Волхов',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Вруда' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Вруда',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Всеволожск' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Всеволожск',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Выборг' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Выборг',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Вырица' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Вырица',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Выскатка' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Выскатка',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Высоцк' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Высоцк',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Гаврилово' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Гаврилово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Ганьково' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Ганьково',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Гарболово' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Гарболово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Гатчина' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Гатчина',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Гвардейское' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Гвардейское',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Гимрека' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Гимрека',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Гладкое' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Гладкое',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Глажево' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Глажево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Глебычево' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Глебычево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Глобицы' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Глобицы',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Голубково' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Голубково',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Гончарово' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Гончарово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Горбунки' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Горбунки',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Горка' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Горка',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Городец' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Городец',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Городище' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Городище',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Городок' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Городок',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Горьковское' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Горьковское',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Гостилицы' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Гостилицы',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Гостинополье' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Гостинополье',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Гремячево' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Гремячево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Григино' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Григино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Громово' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Громово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Дивенская' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Дивенская',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Доможирово' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Доможирово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Дружная Горка' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Дружная Горка',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Дубровка' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Дубровка',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Дятлово' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Дятлово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Елизаветино' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Елизаветино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Ермилово' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Ермилово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Ефимовский' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Ефимовский',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Ефремково' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Ефремково',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Жемчужина' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Жемчужина',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Живой Ручей' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Живой Ручей',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Житково' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Житково',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Журавлево' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Журавлево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Забелино' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Забелино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Заборье' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Заборье',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Заголодно' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Заголодно',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Загривье' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Загривье',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Загубье' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Загубье',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Зайцево' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Зайцево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Заклинье' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Заклинье',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Запорожское' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Запорожское',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Заручье' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Заручье',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Захонье' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Захонье',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Зеленец' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Зеленец',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Зеленый Холм' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Зеленый Холм',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Зимитицы' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Зимитицы',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Ивангород' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Ивангород',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Ивановское' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Ивановское',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Игнатовское' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Игнатовское',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Извара' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Извара',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Ильжо' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Ильжо',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Ильичево' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Ильичево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Имоченицы' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Имоченицы',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Исаково' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Исаково',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Иссад' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Иссад',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Казыченская' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Казыченская',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Калитино' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Калитино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Каменногорск' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Каменногорск',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Камышевка' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Камышевка',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Кармановская' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Кармановская',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Карташевская' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Карташевская',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Касколовка' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Касколовка',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Керстово' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Керстово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Кикерино' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Кикерино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Кингисепп' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Кингисепп',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Кингисеппский' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Кингисеппский',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Кипень' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Кипень',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Кипрушино' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Кипрушино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Кирилловское' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Кирилловское',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Кириши' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Кириши',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Кировск' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Кировск',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Кирпичное' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Кирпичное',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Кирьямо' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Кирьямо',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Кисельня' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Кисельня',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Климово' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Климово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Клопицы' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Клопицы',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Кобона' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Кобона',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Кобралово' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Кобралово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Кобринское' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Кобринское',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Коваши' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Коваши',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Колбеки' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Колбеки',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Коли' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Коли',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Колтуши' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Колтуши',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Колчаново' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Колчаново',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Коммунар' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Коммунар',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Коммунары' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Коммунары',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Комсомолец' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Комсомолец',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Кондратьево' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Кондратьево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Копорье' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Копорье',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Корбеничи' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Корбеничи',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Коркино' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Коркино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Коробицыно' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Коробицыно',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Коськово' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Коськово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Котельский' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Котельский',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Котлы' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Котлы',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Кошкино' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Кошкино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Краколье' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Краколье',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Красава' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Красава',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Красная Долина' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Красная Долина',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Красноармейское' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Красноармейское',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Красноозерное' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Красноозерное',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Красносельское' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Красносельское',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Красные Горы' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Красные Горы',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Красный Вал' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Красный Вал',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Красный Маяк' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Красный Маяк',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Красный Сокол' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Красный Сокол',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Кривко' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Кривко',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Кузнечное' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Кузнечное',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Кузра' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Кузра',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Кузьмоловский' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Кузьмоловский',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Куйвози' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Куйвози',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Кукуй' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Кукуй',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Курба' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Курба',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Курковицы' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Курковицы',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Куровицы' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Куровицы',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Кусино' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Кусино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Лаврово' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Лаврово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Лаголово' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Лаголово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Ладожское Озеро' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Ладожское Озеро',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Лампово' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Лампово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Ларионово' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Ларионово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Ларьян' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Ларьян',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Лебяжье' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Лебяжье',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Ленинское' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Ленинское',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Лесколово' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Лесколово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Лесной' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Лесной',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Лесобиржа' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Лесобиржа',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Лесогорский' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Лесогорский',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Лесозавод' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Лесозавод',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Липная Горка' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Липная Горка',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Лисино-Корпус' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Лисино-Корпус',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Логи' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Логи',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Лодейное Поле' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Лодейное Поле',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Ложголово' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Ложголово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Ломоносов' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Ломоносов',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Лопухинка' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Лопухинка',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Лосево' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Лосево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Луга' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Луга',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Лукаши' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Лукаши',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Лукинская' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Лукинская',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Любань' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Любань',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Малое Карлино' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Малое Карлино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Манихино' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Манихино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Матокса' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Матокса',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Мга' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Мга',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Межозерный' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Межозерный',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Мелегежская Горка' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Мелегежская Горка',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Мельниково' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Мельниково',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Мехбаза' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Мехбаза',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Миницкая' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Миницкая',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Михалево' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Михалево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Мичуринское' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Мичуринское',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Мозолево' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Мозолево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Молодцово' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Молодцово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Молосковицы' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Молосковицы',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Монастырек' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Монастырек',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Мотохово' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Мотохово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Мошковые Поляны' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Мошковые Поляны',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Мурино' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Мурино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Мшинская' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Мшинская',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Наволок' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Наволок',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Надкопанье' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Надкопанье',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Надпорожье' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Надпорожье',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Назия' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Назия',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Нежново' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Нежново',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Нижние Осельки' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Нижние Осельки',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Низино' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Низино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Низовская' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Низовская',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Николаевское' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Николаевское',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Николаевщина' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Николаевщина',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Никольский' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Никольский',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Никольское' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Никольское',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Новая' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Новая',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Новая Ладога' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Новая Ладога',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Новинка' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Новинка',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Ново-Девяткино' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Ново-Девяткино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Новолисино' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Новолисино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Новый Свет' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Новый Свет',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Нурма' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Нурма',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Овсище' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Овсище',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Овцино' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Овцино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Озерево' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Озерево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Озерное' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Озерное',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Окулово' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Окулово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Оломна' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Оломна',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Ольеши' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Ольеши',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Ополье' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Ополье',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Оредеж' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Оредеж',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Оржицы' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Оржицы',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Островно' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Островно',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Осьмино' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Осьмино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Отрадное' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Отрадное',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Павлово' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Павлово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Паша' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Паша',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Пашозеро' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Пашозеро',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Пелдуши' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Пелдуши',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Пельгора' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Пельгора',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Пельгорторф' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Пельгорторф',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Пеники' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Пеники',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Первомайское' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Первомайское',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Перечицы' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Перечицы',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Перово' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Перово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Пески' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Пески',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Песочное' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Песочное',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Петровское' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Петровское',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Пехенец' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Пехенец',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Печково' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Печково',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Пикалево' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Пикалево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Пирозеро' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Пирозеро',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Плодовое' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Плодовое',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Плоское' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Плоское',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Победа' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Победа',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Подборовье' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Подборовье',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Подвязье' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Подвязье',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Поддубье' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Поддубье',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Подпорожье' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Подпорожье',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Поляны' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Поляны',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Померанье' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Померанье',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Попкова Гора' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Попкова Гора',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Поречье' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Поречье',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Потанино' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Потанино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Поток' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Поток',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Починок' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Починок',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Прибытково' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Прибытково',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Приветнинское' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Приветнинское',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Приладожский' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Приладожский',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Приморск' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Приморск',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Приозерск' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Приозерск',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Пруды' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Пруды',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Пудомяги' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Пудомяги',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Пудость' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Пудость',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Путилово' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Путилово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Пушное' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Пушное',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Пчева' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Пчева',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Пчевжа' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Пчевжа',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Пяхта' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Пяхта',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Рабитицы' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Рабитицы',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Радогощь' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Радогощь',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Радофинниково' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Радофинниково',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Разбегаево' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Разбегаево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Раздолье' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Раздолье',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Разметелево' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Разметелево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Раковно' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Раковно',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Рапполово' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Рапполово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Рассвет' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Рассвет',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Ратница' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Ратница',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Рахья' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Рахья',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Ребовичи' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Ребовичи',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Рель' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Рель',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Реполка' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Реполка',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Ретюнь' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Ретюнь',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Решетниково' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Решетниково',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Рождествено' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Рождествено',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Романовка' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Романовка',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Ромашки' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Ромашки',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Ропша' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Ропша',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Рощино' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Рощино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Русско-Высоцкое' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Русско-Высоцкое',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Рыбежно' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Рыбежно',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Рябово' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Рябово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Сабск' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Сабск',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Санкт-Петербург' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Санкт-Петербург',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Саперное' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Саперное',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Сарка' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Сарка',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Сарожа' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Сарожа',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Светогорск' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Светогорск',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Свирица' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Свирица',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Свирьстрой' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Свирьстрой',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Свободное' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Свободное',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Севостьяново' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Севостьяново',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Сегла' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Сегла',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Селезнево' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Селезнево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Селиваново' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Селиваново',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Сельхозтехника' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Сельхозтехника',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Сельцо' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Сельцо',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Семиозерье' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Семиозерье',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Семрино' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Семрино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Серебрянка' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Серебрянка',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Сертолово' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Сертолово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Сиверский' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Сиверский',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Сидорово' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Сидорово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Синявино' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Синявино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Скреблово' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Скреблово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Сланцы' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Сланцы',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Слудицы' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Слудицы',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Соболевщина' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Соболевщина',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Советский' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Советский',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Совхозный' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Совхозный',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Соколинское' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Соколинское',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Сологубовка' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Сологубовка',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Сомино' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Сомино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Сорзуй' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Сорзуй',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Сосново' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Сосново',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Сосновый Бор' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Сосновый Бор',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Спирово' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Спирово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Спутник' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Спутник',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Среднее Село' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Среднее Село',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Старая Ладога' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Старая Ладога',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Старая Малукса' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Старая Малукса',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Старая Слобода' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Старая Слобода',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Старополье' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Старополье',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Старосиверская' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Старосиверская',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Стеклянный' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Стеклянный',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Суйда' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Суйда',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Сумино' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Сумино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Сусанино' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Сусанино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Суходолье' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Суходолье',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Сухое' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Сухое',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Сяськелево' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Сяськелево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Сясьстрой' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Сясьстрой',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Тайцы' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Тайцы',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Тарасово' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Тарасово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Тельман' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Тельман',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Тервеничи' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Тервеничи',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Терволово' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Терволово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Терпилицы' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Терпилицы',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Тесово' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Тесово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Тихвин' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Тихвин',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Тихорицы' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Тихорицы',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Токарево' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Токарево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Токари' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Токари',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Токсово' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Токсово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Толмачево' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Толмачево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Торковичи' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Торковичи',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Торосово' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Торосово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Торошковичи' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Торошковичи',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Тосно' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Тосно',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Труфаново' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Труфаново',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Тургошь' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Тургошь',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Тушемля' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Тушемля',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Ульяновка' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Ульяновка',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Усадище' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Усадище',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Усть-Луга' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Усть-Луга',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Утишье' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Утишье',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Ушаки' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Ушаки',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Ущевицы' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Ущевицы',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Фалилеево' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Фалилеево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Федоровское' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Федоровское',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Форносово' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Форносово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Фосфорит' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Фосфорит',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Хаппо-Ое' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Хаппо-Ое',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Хвалово' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Хвалово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Хотнежа' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Хотнежа',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Цвелодубово' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Цвелодубово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Цвылево' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Цвылево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Часовенское' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Часовенское',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Чаща' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Чаща',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Чемихино' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Чемихино',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Черкасово' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Черкасово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Черная Речка' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Черная Речка',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Черновское' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Черновское',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Чолово' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Чолово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Шалово' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Шалово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Шамокша' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Шамокша',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Шапки' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Шапки',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Шапша' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Шапша',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Шархиничи' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Шархиничи',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Шеменичи' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Шеменичи',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Шепелево' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Шепелево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Шлиссельбург' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Шлиссельбург',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Шпаньково' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Шпаньково',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Шум' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Шум',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Щеглово-Совхоз' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Щеглово-Совхоз',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Щугозеро' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Щугозеро',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Юкки' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Юкки',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Яблоницы' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Яблоницы',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Яльгелево' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Яльгелево',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Ям-Тесово' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Ям-Тесово',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Янега' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Янега',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Яровщина' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Яровщина',0, 0)
if(not exists (select * from [Customers].City where [CityName]='Ярославичи' and [RegionID] = @LenOblId )) insert into [Customers].City ([RegionID], [CityName], CitySort, DisplayInPopup) values (@LenOblId, 'Ярославичи',0, 0)



GO--


Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Js.Inplace.ErrorPropertyUpdate', 'Ошибка при обновлении свойства');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Js.Inplace.ErrorPropertyUpdate', 'Error when update properties for the item');

GO--

UPDATE [CMS].[Menu] SET [MenuItemUrlPath] = 'myaccount#?tab=commoninf' WHERE [MenuItemUrlPath] = 'myaccount'

GO--

ALTER PROCEDURE [Catalog].[SetCategoryHierarchicallyEnabled]
		@CatParent int
AS
BEGIN
Declare @flagAction bit
set @flagAction = (Select Enabled from [Catalog].[Category] WHERE CategoryID = @CatParent)
Declare @tbl TABLE (id  int )
if (@flagAction = 1)
	--if we enabled on
	begin
		;WITH Hierarchycte (id) AS
								(
								SELECT CategoryID FROM [Catalog].[Category] WHERE CategoryID = @CatParent and Enabled =1 
								union ALL
								SELECT CategoryID FROM [Catalog].[Category] INNER JOIN hierarchycte ON [Category].ParentCategory = hierarchycte.id and CategoryID<>0 and Enabled = 1 )
		insert into @tbl SELECT id FROM hierarchycte
	end
else
	begin
		;WITH Hierarchycte (id) AS
									(
									SELECT CategoryID FROM [Catalog].[Category] WHERE CategoryID = @CatParent 
									union ALL
									SELECT CategoryID FROM [Catalog].[Category] INNER JOIN hierarchycte ON [Category].ParentCategory = hierarchycte.id and CategoryID<>0)
		insert into @tbl SELECT id FROM hierarchycte
	end
	select * from @tbl
update [Catalog].[Category] set HirecalEnabled = @flagAction where CategoryID in (select id from @tbl)
update [Catalog].[Product]  set CategoryEnabled = @flagAction 
where ProductId in ( Select ProductID from [Catalog].[ProductCategories] where CategoryID in (select id from @tbl) and main = 1)

update [Catalog].[Product] set CategoryEnabled = 0 Where ProductID not in (Select ProductID from [Catalog].[ProductCategories])

END

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

Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.IPTelephony.Telphin.ETelphinEvent.DialIn', 'Входящий вызов');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.IPTelephony.Telphin.ETelphinEvent.DialOut', 'Исходящий вызов');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.IPTelephony.Telphin.ETelphinEvent.HangUp', 'Окончание соединения');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.IPTelephony.Telphin.ETelphinEvent.Answer', 'Ответ');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.IPTelephony.Telphin.ETelphinEvent.DialIn', 'Incoming call');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.IPTelephony.Telphin.ETelphinEvent.DialOut', 'Outgoing call');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.IPTelephony.Telphin.ETelphinEvent.HangUp', 'Hang up');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.IPTelephony.Telphin.ETelphinEvent.Answer', 'Answer');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Admin.SettingsTelephony.Errors.TelphinExtension.NotFound', 'Добавочный номер не найден');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Admin.SettingsTelephony.Errors.TelphinExtension.NotFound', 'An extension number not found');

GO--

IF (NOT EXISTS(SELECT * FROM [Settings].[Localization] WHERE ResourceKey = 'Feedback.Index.AgreementText'))
begin
	Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Feedback.Index.AgreementText', 'Нажимая кнопку "Отправить", я подтверждаю свою дееспособность, даю согласие на обработку своих персональных данных.');
	Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Feedback.Index.AgreementText', 'By clicking the "Send" button I agree to have my personal data processed.');
end

GO--


UPDATE [Settings].[InternalSettings] SET [settingValue] = '5.0.27' WHERE [settingKey] = 'db_version'
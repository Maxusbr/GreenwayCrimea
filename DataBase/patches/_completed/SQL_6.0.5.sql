ALTER TABLE [Customers].[Task] ADD [SortOrder] int NULL
GO--
UPDATE [Customers].[Task] SET [SortOrder] = Id
GO--
ALTER TABLE [Customers].[Task] ALTER COLUMN [SortOrder] int NOT NULL
GO--

UPDATE [Settings].[Localization] set [ResourceValue] = 'Все задачи' WHERE [ResourceKey] = 'Admin.Layout.LeftMenu.Tasks' AND [LanguageId] = 1
UPDATE [Settings].[Localization] set [ResourceValue] = 'All tasks' WHERE [ResourceKey] = 'Admin.Layout.LeftMenu.Tasks' AND [LanguageId] = 2
UPDATE [Settings].[Localization] set [ResourceValue] = 'Все задачи' WHERE [ResourceKey] = 'Admin.Tasks.Index.Title' AND [LanguageId] = 1
UPDATE [Settings].[Localization] set [ResourceValue] = 'All tasks' WHERE [ResourceKey] = 'Admin.Tasks.Index.Title' AND [LanguageId] = 2

UPDATE [Settings].[Localization] set [ResourceValue] = 'Категрия товаров google по умолчанию' WHERE [ResourceKey] = 'Admin.ExportFeeed.SettingsGoogle.GoogleProductCategory' AND [LanguageId] = 1
UPDATE [Settings].[Localization] set [ResourceValue] = 'Default google product category' WHERE [ResourceKey] = 'Admin.ExportFeeed.SettingsGoogle.GoogleProductCategory' AND [LanguageId] = 2


UPDATE [Settings].[Localization] set [ResourceValue] = 'Выгружать недоступные к покупке товары' WHERE [ResourceKey] = 'Admin.ExportFeed.SettingsYandex.ExportNotAvailable' AND [LanguageId] = 1
UPDATE [Settings].[Localization] set [ResourceValue] = 'Export not available products' WHERE [ResourceKey] = 'Admin.ExportFeed.SettingsYandex.ExportNotAvailable' AND [LanguageId] = 2

UPDATE [Settings].[Localization] set [ResourceValue] = 'Дополнительные параметры в url (UTM-метки)' WHERE [ResourceKey] = 'Admin.ExportFeed.Settings.AdditionalUrlTags' AND [LanguageId] = 1
UPDATE [Settings].[Localization] set [ResourceValue] = 'Дополнительные параметры в url (UTM-marks)' WHERE [ResourceKey] = 'Admin.ExportFeed.Settings.AdditionalUrlTags' AND [LanguageId] = 2





if((select Count(*) from Settings.Localization where ResourceKey = 'Js.Cart.Delete') = 0) 
begin
	INSERT INTO [Settings].[Localization] ([LanguageId], [ResourceKey], [ResourceValue]) VALUES (1, 'Js.Cart.Delete', 'Удалить')
	INSERT INTO [Settings].[Localization] ([LanguageId], [ResourceKey], [ResourceValue]) VALUES (2, 'Js.Cart.Delete', 'Delete')
end
if((select Count(*) from Settings.Localization where ResourceKey = 'Js.Cart.PreOrder') = 0) 
begin
	INSERT INTO [Settings].[Localization] ([LanguageId], [ResourceKey], [ResourceValue]) VALUES (1, 'Js.Cart.PreOrder', 'Под заказ')
	INSERT INTO [Settings].[Localization] ([LanguageId], [ResourceKey], [ResourceValue]) VALUES (2, 'Js.Cart.PreOrder', 'PreOrder')
end

if((select Count(*) from Settings.Localization where ResourceKey = 'Core.Orders.Order.TrackNumber') = 0) 
begin
	INSERT INTO [Settings].[Localization] ([LanguageId], [ResourceKey], [ResourceValue]) VALUES (1, 'Core.Orders.Order.TrackNumber', 'Трек-номер')
	INSERT INTO [Settings].[Localization] ([LanguageId], [ResourceKey], [ResourceValue]) VALUES (2, 'Core.Orders.Order.TrackNumber', 'Track Number')
end

GO--
	
INSERT INTO [Settings].[Localization] ([LanguageId], [ResourceKey], [ResourceValue]) VALUES (1, 'Admin.ExportFeed.SettingsGoogle.AvailableCategories', 'Доступные категории')
INSERT INTO [Settings].[Localization] ([LanguageId], [ResourceKey], [ResourceValue]) VALUES (2, 'Admin.ExportFeed.SettingsGoogle.AvailableCategories', 'Available сategories')

INSERT INTO [Settings].[Localization] ([LanguageId], [ResourceKey], [ResourceValue]) VALUES (1, 'Admin.ExportFeed.Settings.OfferId', 'Идентификатор модификации')
INSERT INTO [Settings].[Localization] ([LanguageId], [ResourceKey], [ResourceValue]) VALUES (2, 'Admin.ExportFeed.Settings.OfferId', 'Offer id')

INSERT INTO [Settings].[Localization] ([LanguageId], [ResourceKey], [ResourceValue]) VALUES (1, 'Admin.ExportFeed.Settings.OfferSku', 'Артикул модификации')
INSERT INTO [Settings].[Localization] ([LanguageId], [ResourceKey], [ResourceValue]) VALUES (2, 'Admin.ExportFeed.Settings.OfferSku', 'Offer sku')

INSERT INTO [Settings].[Localization] ([LanguageId], [ResourceKey], [ResourceValue]) VALUES (1, 'Admin.ExportFeed.Settings.BriefDescription', 'Краткое описание')
INSERT INTO [Settings].[Localization] ([LanguageId], [ResourceKey], [ResourceValue]) VALUES (2, 'Admin.ExportFeed.Settings.BriefDescription', 'Brief Description')
	
INSERT INTO [Settings].[Localization] ([LanguageId], [ResourceKey], [ResourceValue]) VALUES (1, 'Admin.ExportFeed.Settings.FullDescription', 'Полное описание')
INSERT INTO [Settings].[Localization] ([LanguageId], [ResourceKey], [ResourceValue]) VALUES (2, 'Admin.ExportFeed.Settings.FullDescription', 'Full Description')

INSERT INTO [Settings].[Localization] ([LanguageId], [ResourceKey], [ResourceValue]) VALUES (1, 'Admin.ExportFeed.Settings.DontUseDescription', 'Не выгружать')
INSERT INTO [Settings].[Localization] ([LanguageId], [ResourceKey], [ResourceValue]) VALUES (2, 'Admin.ExportFeed.Settings.DontUseDescription', 'Do not use description')

INSERT INTO [Settings].[Localization] ([LanguageId], [ResourceKey], [ResourceValue]) VALUES (1, 'Admin.ExportFeed.Settings.AvailableVariables', 'Доступные переменные')
INSERT INTO [Settings].[Localization] ([LanguageId], [ResourceKey], [ResourceValue]) VALUES (2, 'Admin.ExportFeed.Settings.AvailableVariables', 'Available variables')


GO--


IF ((SELECT Value FROM Settings.Settings WHERE Name = 'BonusSystem.SmsProviderType') = 'Sms4B')
BEGIN
	INSERT INTO Settings.Settings (Name, Value) VALUES ('BonusSystem.Sms4BLogin', (SELECT Value FROM Settings.Settings WHERE Name = 'BonusSystem.SmsLogin'))
	INSERT INTO Settings.Settings (Name, Value) VALUES ('BonusSystem.Sms4BPassword', (SELECT Value FROM Settings.Settings WHERE Name = 'BonusSystem.SmsPassword'))
END 
ELSE IF ((SELECT Value FROM Settings.Settings WHERE Name = 'BonusSystem.SmsProviderType') = 'StreamSms')
BEGIN
	INSERT INTO Settings.Settings (Name, Value) VALUES ('BonusSystem.StreamSmsLogin', (SELECT Value FROM Settings.Settings WHERE Name = 'BonusSystem.SmsLogin'))
	INSERT INTO Settings.Settings (Name, Value) VALUES ('BonusSystem.StreamSmsPassword', (SELECT Value FROM Settings.Settings WHERE Name = 'BonusSystem.SmsPassword'))
END

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
       order by Color.SortOrder, ColorName  
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
       order by Color.SortOrder, ColorName  
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
   order by Size.SortOrder, SizeName  
 end  
 else  
 begin  
  Select * from Catalog.Size where SizeID in   
  (select SizeID from Catalog.Offer   
   inner join Catalog.Product on Offer.ProductID=Product.ProductID   
   inner join Catalog.ProductCategories on ProductCategories.ProductID= Product.ProductID   
    and ProductCategories.CategoryID = @CategoryID and   
    Product.Enabled = 1 and Product.CategoryEnabled=1  and (@OnlyAvailable = 0 OR Amount > 0))   
   order by Size.SortOrder, SizeName  
 end  
END

GO--

Update Catalog.Currency set [EnablePriceRounding] = 1 where [RoundNumbers] <> -1
Update Catalog.Currency set [EnablePriceRounding] = 0 where [RoundNumbers] = -1

GO--

INSERT INTO Settings.Settings (Name, Value) VALUES ('ShopNameAdmin', 'AdvantShop')

GO--

if ((Select Count(*) From [Settings].[Localization] where [LanguageId]=1 and [ResourceKey]='Js.Reviews.ThxForReviewMsg' and [ResourceValue]='Он отобразиться на странице, как только пройдет модерацию') = 1)
begin
	Update [Settings].[Localization] 
	Set [ResourceValue] = 'Он отобразится на странице, как только пройдет модерацию' 
	Where [LanguageId] = 1 and [ResourceKey]='Js.Reviews.ThxForReviewMsg' and [ResourceValue]='Он отобразиться на странице, как только пройдет модерацию'
end

GO--

ALTER PROCEDURE [Catalog].[sp_AddProduct]      
	 @ArtNo nvarchar(100) = '',    
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
  set @ArtNo = Convert(nvarchar(100), @ID)     
    
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
		PriceTemp = (isnull(minPrice.minPrice,0) - isnull(minPrice.minPrice,0) * p.Discount/100)*c.CurrencyValue,
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
IF ((SELECT COUNT(*) FROM [Settings].[Localization] WHERE ResourceKey = 'Admin.Import.ImportProducts.ChangeNewFile') = 0)
BEGIN
	Insert into [Settings].[Localization] ([ResourceKey],[ResourceValue],[LanguageId]) Values('Admin.Import.ImportProducts.ChangeNewFile','Выбрать другой файл',1)
	Insert into [Settings].[Localization] ([ResourceKey],[ResourceValue],[LanguageId]) Values('Admin.Import.ImportProducts.ChangeNewFile','Select another file',2)
END
ELSE
BEGIN
	Update [Settings].[Localization] set [ResourceValue] = 'Выбрать другой файл' where [LanguageId] = 1 and [ResourceKey] = 'Admin.Import.ImportProducts.ChangeNewFile'
	Update [Settings].[Localization] set [ResourceValue] = 'Select another file' where [LanguageId] = 2 and [ResourceKey] = 'Admin.Import.ImportProducts.ChangeNewFile'
END

GO--


Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Admin.Home.Menu.CustomerSegments', 'Сегменты покупателей');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Admin.Home.Menu.CustomerSegments', 'Consumer segments');

Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Admin.CustomerSegments.Index.Title', 'Сегменты покупателей');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Admin.CustomerSegments.Index.Title', 'Consumer segments');

Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Admin.CustomerSegments.Index.Add', 'Добавить сегмент');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Admin.CustomerSegments.Index.Add', 'Add');

Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Admin.CustomerSegments.Edit.Title', 'Редактирование сегмента');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Admin.CustomerSegments.Edit.Title', 'Edit customer segment');

Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Admin.CustomerSegments.Add.Title', 'Создание пользовательского сегмента');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Admin.CustomerSegments.Add.Title', 'Create customer segment');

GO--

CREATE TABLE [Customers].[CustomerSegment](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Filter] [nvarchar](max) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_CustomerSegment] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO--

CREATE TABLE [Customers].[CustomerSegment_Customer](
	[CustomerId] [uniqueidentifier] NOT NULL,
	[SegmentId] [int] NOT NULL,
 CONSTRAINT [PK_CustomerSegment_Customer] PRIMARY KEY CLUSTERED 
(
	[CustomerId] ASC,
	[SegmentId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO--

ALTER TABLE [Customers].[CustomerSegment_Customer]  WITH CHECK ADD  CONSTRAINT [FK_CustomerSegment_Customer_Customer] FOREIGN KEY([CustomerId])
REFERENCES [Customers].[Customer] ([CustomerID])
ON DELETE CASCADE
GO--

ALTER TABLE [Customers].[CustomerSegment_Customer] CHECK CONSTRAINT [FK_CustomerSegment_Customer_Customer]
GO--

ALTER TABLE [Customers].[CustomerSegment_Customer]  WITH CHECK ADD  CONSTRAINT [FK_CustomerSegment_Customer_CustomerSegment] FOREIGN KEY([SegmentId])
REFERENCES [Customers].[CustomerSegment] ([Id])
ON DELETE CASCADE
GO--

ALTER TABLE [Customers].[CustomerSegment_Customer] CHECK CONSTRAINT [FK_CustomerSegment_Customer_CustomerSegment]
GO--

Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Admin.Settings.Checkout.ZipDisplayPlace', 'Показывать индекс над доставками при оформлении заказа');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Admin.Settings.Checkout.ZipDisplayPlace', 'Show zip code before shippings block in checkout');

Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Admin.Design.Theme.Title', 'Редактирование темы дизайна');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Admin.Design.Theme.Title', 'Edit design theme');

GO--

ALTER TABLE CMS.Carousel ADD Blank bit NULL
GO--
UPDATE CMS.Carousel SET Blank = 0
GO--
ALTER TABLE CMS.Carousel ALTER COLUMN Blank bit NOT NULL
GO--

ALTER PROCEDURE [CMS].[sp_UpdateCarousel]
	@CarouselID int,	
	@URL nvarchar(max),
	@SortOrder int,
	@Enabled bit,
	@DisplayInOneColumn bit,
	@DisplayInTwoColumns bit,
	@DisplayInMobile bit,
	@Blank bit
AS
BEGIN
	UPDATE [CMS].[Carousel] 
	SET 
		URL = @URL, 
		SortOrder = @SortOrder, 
		Enabled = @Enabled,
		DisplayInOneColumn = @DisplayInOneColumn,
		DisplayInTwoColumns = @DisplayInTwoColumns,
		DisplayInMobile = @DisplayInMobile,
		Blank = @Blank
	WHERE CarouselID = @CarouselID
END
GO--

ALTER PROCEDURE [CMS].[sp_InsertCarousel]
    @URL nvarchar(max),
	@SortOrder int,
    @Enabled bit,
    @DisplayInOneColumn bit,
	@DisplayInTwoColumns bit,
	@DisplayInMobile bit,
	@Blank bit
AS
BEGIN
	INSERT INTO [CMS].[Carousel]
        (URL
        ,SortOrder
        ,Enabled
		,DisplayInOneColumn
		,DisplayInTwoColumns
	    ,DisplayInMobile
		,Blank)
     VALUES
		(@URL
        ,@SortOrder			  
        ,@Enabled
        ,@DisplayInOneColumn
		,@DisplayInTwoColumns
	    ,@DisplayInMobile
		,@Blank)
	 Select SCOPE_IDENTITY()
END
GO--



UPDATE [Settings].[InternalSettings] SET [settingValue] = '6.0.5' WHERE [settingKey] = 'db_version'
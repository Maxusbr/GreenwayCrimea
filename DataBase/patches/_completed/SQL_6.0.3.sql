Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Admin.Settings.System.CustomersNotifications', 'Уведомления пользователям');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Admin.Settings.System.CustomersNotifications', 'Notifications to customers');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Admin.Settings.SystemSettings.CustomersNotifications', 'Уведомления пользователям');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Admin.Settings.SystemSettings.CustomersNotifications', 'Notifications to customers');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Admin.Settings.SystemSettings.ShowCookiesPolicyMessage', 'Показывать уведомление об использовании cookies');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Admin.Settings.SystemSettings.ShowCookiesPolicyMessage', 'Show cookies policy message');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Admin.Settings.SystemSettings.CookiesPolicyMessage', 'Уведомление об использовании cookies');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Admin.Settings.SystemSettings.CookiesPolicyMessage', 'Cookies policy message');

Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Admin.Analytics.ExportProducts.Title', 'Выгрузка товаров');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Admin.Analytics.ExportProducts.Title', 'Export products');

Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Admin.Analytics.ExportProducts.SubTitle', 'Выгрузка данных по товарам из оплаченных заказов');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Admin.Analytics.ExportProducts.SubTitle', 'Export data for goods from paid orders');

Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Admin.Analytics.ExportProducts.DateSpan', 'Временной интервал');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Admin.Analytics.ExportProducts.DateSpan', 'Time interval');

Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Admin.Analytics.ExportProducts.DateSpanFrom', 'От');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Admin.Analytics.ExportProducts.DateSpanFrom', 'From');

Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Admin.Analytics.ExportProducts.DateSpanTo', 'До');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Admin.Analytics.ExportProducts.DateSpanTo', 'To');

Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Admin.Analytics.ExportProducts.ColumnSeparator', 'Разделитель между колонками');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Admin.Analytics.ExportProducts.ColumnSeparator', 'Separator between columns');

Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Admin.Analytics.ExportProducts.FileEncoding', 'Кодировка файла');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Admin.Analytics.ExportProducts.FileEncoding', 'File encoding');

Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Admin.Analytics.ExportProducts.Unload', 'Выгружать');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Admin.Analytics.ExportProducts.Unload', 'Unload');

Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Admin.Analytics.ExportProducts.Categories', 'Категории');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Admin.Analytics.ExportProducts.Categories', 'Categories');

Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Admin.Analytics.ExportProducts.OneProduct', 'Один товар');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Admin.Analytics.ExportProducts.OneProduct', 'One product');

Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Admin.Analytics.ExportProducts.EnterProductArtno', 'Введите артикул товара');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Admin.Analytics.ExportProducts.EnterProductArtno', 'Enter product article');

Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Services.CMS.OrderAddedNotification.Title', 'Новый заказ'); 
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Services.CMS.OrderAddedNotification.Body', 'Заказ №{0}'); 
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Services.CMS.LeadAddedNotification.Title', 'Новый лид'); 
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Services.CMS.LeadAddedNotification.Body', 'Лид №{0}'); 

GO--

Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Admin.Settings.SettingsBonus.GoToGetBonusCardPage', 'Перейти на страницу получения бонусной карты');

insert into Settings.Localization (LanguageId, ResourceKey, ResourceValue) values (1, 'Admin.Orders.AddEdit.ExportExcel', 'Экспорт в Excel')
insert into Settings.Localization (LanguageId, ResourceKey, ResourceValue) values (2, 'Admin.Orders.AddEdit.ExportExcel', 'Export to Excel')

GO--

if((select Count(*) from Settings.Localization where ResourceKey = 'PaymentReceipt.Bill.Unit') = 0) 
begin
	Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'PaymentReceipt.Bill.Unit', 'Ед.изм.')
	Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'PaymentReceipt.Bill.Unit', 'Unit')
end

GO--

if not exists (select *  from settings.settings where name = 'OutOfStockAction')
	begin
		insert into settings.settings (name, value) values ('OutOfStockAction', 'Lead')
	end
else
	begin
		update settings.settings set value = 'Lead' where name = 'OutOfStockAction'
	end

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

if not exists (Select * from Settings.Settings  where Name = 'CustomersNotifications.CookiesPolicyMessage')
begin
    INSERT INTO Settings.Settings (Name, Value) VALUES ('CustomersNotifications.CookiesPolicyMessage', 'Уважаемый посетитель! Для лучшего функционирования сайта #STORE_URL# мы производим сбор Ваших метаданных (cookie, данные об IP-адресе и местоположении). В случае, если Вы не хотите, чтобы нами был осуществлён сбор Ваших метаданных, Вам необходимо покинуть данный сайт.')
end
GO--





IF NOT EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'TaxType' AND Object_ID = Object_ID(N'Catalog.Tax'))
Begin
    Delete From [Catalog].[Tax]

	ALTER TABLE Catalog.Tax ADD
		TaxType int NULL
End

GO--

IF ((Select Count(*) From [Catalog].[Tax]) = 0)
Begin
	Insert Into [Catalog].[Tax] ([Name],[Enabled],[ShowInPrice],[Rate],[TaxType]) Values ('Без НДС', 1, 1, 0, 1);
	Insert Into [Catalog].[Tax] ([Name],[Enabled],[ShowInPrice],[Rate],[TaxType]) Values ('НДС 0%', 1, 1, 0, 2);
	Insert Into [Catalog].[Tax] ([Name],[Enabled],[ShowInPrice],[Rate],[TaxType]) Values ('НДС 10%', 1, 1, 10, 3);
	Insert Into [Catalog].[Tax] ([Name],[Enabled],[ShowInPrice],[Rate],[TaxType]) Values ('НДС 18%', 1, 1, 18, 4);
End

GO--

IF ((select is_nullable from sys.columns where object_id = object_id('Catalog.Tax') and name = 'TaxType') <> 0)
Begin
	ALTER TABLE Catalog.Tax ALTER COLUMN
		TaxType int NOT NULL
End
	
GO--

IF NOT EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'TaxId' AND Object_ID = Object_ID(N'Catalog.Product'))
Begin
	ALTER TABLE Catalog.Product ADD
		TaxId int NULL
End

GO--

IF NOT EXISTS(Select 1 From [Settings].[Localization] Where [ResourceKey] = 'Core.ExportImport.ProductFields.Tax')
Begin
	Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.ExportImport.ProductFields.Tax', 'Налог')
	Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.ExportImport.ProductFields.Tax', 'Tax')
End

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
	 @TaxId int   
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
	WHERE ProductID = @ProductID      
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
     @Taxid int 	 
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
             @TaxId			 
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

IF NOT EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'TaxId' AND Object_ID = Object_ID(N'Order.OrderItems'))
Begin
	ALTER TABLE [Order].OrderItems ADD
		TaxId int NULL,
		TaxName nvarchar(50) NULL,
		TaxType int NULL,
		TaxRate float(53) NULL,
		TaxShowInPrice bit NULL
End

GO--
  
ALTER PROCEDURE [Order].[sp_AddOrderItem]  
	 @OrderID int,  
	 @Name nvarchar(100),  
	 @Price float,  
	 @Amount float,  
	 @ProductID int,  
	 @ArtNo nvarchar(255),  
	 @SupplyPrice float,  
	 @Weight float,  
	 @IsCouponApplied bit,  
	 @Color nvarchar(50),  
	 @Size nvarchar(50),  
	 @DecrementedAmount float,  
	 @PhotoID int,  
	 @IgnoreOrderDiscount bit,  
	 @AccrueBonuses bit,
	 @TaxId int,
	 @TaxName nvarchar(50),
	 @TaxType int,
	 @TaxRate float(53),
	 @TaxShowInPrice bit
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
   );  
       
 SELECT SCOPE_IDENTITY()  
END  

GO-- 
  
ALTER PROCEDURE [Order].[sp_UpdateOrderItem]  
	@OrderItemID int,  
	@OrderID int,  
	@Name nvarchar(100),  
	@Price float,  
	@Amount float,  
	@ProductID int,  
	@ArtNo nvarchar(50),  
	@SupplyPrice float,  
	@Weight float,  
	@IsCouponApplied bit,  
	@Color nvarchar(50),  
	@Size nvarchar(50),  
	@DecrementedAmount float,  
	@PhotoID int,  
	@IgnoreOrderDiscount bit,  
	@AccrueBonuses bit,
	@TaxId int,
	@TaxName nvarchar(50),
	@TaxType int,
	@TaxRate float(53),
	@TaxShowInPrice bit
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
 Where OrderItemID = @OrderItemID  
END  
  
GO--


IF NOT EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'TaxType' AND Object_ID = Object_ID(N'Order.ShippingMethod'))
Begin
	ALTER TABLE [Order].ShippingMethod ADD
		TaxType int NULL
End

GO--

IF ((Select top(1) TaxType From [Order].ShippingMethod) is null)
Begin
	Update [Order].ShippingMethod Set TaxType = 1
End

GO--

IF NOT EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'ShippingTaxType' AND Object_ID = Object_ID(N'Order.Order'))
Begin
	ALTER TABLE [Order].[Order] ADD
		ShippingTaxType int NULL
End

GO--

if ((Select Count(*) From [Settings].[Settings] Where Name = 'DefaultTaxId') = 0)
	Insert Into [Settings].[Settings] ([Name],[Value]) 
		Select Top(1) 'DefaultTaxId', TaxId From [Catalog].[Tax]

GO--


Update Settings.Settings set 
value = 'Я подтверждаю свою дееспособность, даю согласие на обработку своих персональных данных.'
where name = 'UserAgreementText' and value = ' Нажимая кнопку "Продолжить", я подтверждаю свою дееспособность, даю согласие на обработку своих персональных данных.'

GO--

IF((Select Count(*) from  Settings.Localization where LanguageId = 1 and ResourceKey = 'Admin.Marketing.ExportProducts') = 0)
Begin
	Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Admin.Marketing.ExportProducts', 'Выгрузка товаров - Мой магазин на платформе AdvantShop');
End
IF((Select Count(*) from  Settings.Localization where LanguageId = 2 and ResourceKey = 'Admin.Marketing.ExportProducts') = 0)
Begin
	Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, '', 'Export products - My shop on the AdvantShop platform');
End
IF((Select Count(*) from  Settings.Localization where LanguageId = 1 and ResourceKey = 'Admin.Marketing.Analytics.ExportCustomers') = 0)
Begin
	Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Admin.Marketing.Analytics.ExportCustomers', 'Выгрузка покупателей - Мой магазин на платформе AdvantShop');
End
IF((Select Count(*) from  Settings.Localization where LanguageId = 2 and ResourceKey = 'Admin.Marketing.Analytics.ExportCustomers') = 0)
Begin
	Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Admin.Marketing.Analytics.ExportCustomers', 'Export customers - My shop on the AdvantShop platform');
End
IF((Select Count(*) from  Settings.Localization where LanguageId = 1 and ResourceKey = 'Admin.Marketing.Analytics.ExportOrders') = 0)
Begin
	Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Admin.Marketing.Analytics.ExportOrders', 'Выгрузка заказов - Мой магазин на платформе AdvantShop');
End
IF((Select Count(*) from  Settings.Localization where LanguageId = 2 and ResourceKey = 'Admin.Marketing.Analytics.ExportOrders') = 0)
Begin
	Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Admin.Marketing.Analytics.ExportOrders', 'Export orders - My shop on the AdvantShop platform');
End
IF((Select Count(*) from  Settings.Localization where LanguageId = 1 and ResourceKey = 'Admin.Settings.System.Api') = 0)
Begin
	Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Admin.Settings.System.Api', 'API - Мой магазин на платформе AdvantShop');
End
IF((Select Count(*) from  Settings.Localization where LanguageId = 2 and ResourceKey = 'Admin.Settings.System.Api') = 0)
Begin
	Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Admin.Settings.System.Api', 'API - My shop on the AdvantShop platform');
End
IF((Select Count(*) from  Settings.Localization where LanguageId = 1 and ResourceKey = 'Admin.Service.Academy') = 0)
Begin
	Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Admin.Service.Academy', 'Обучение - Мой магазин на платформе AdvantShop');
End
IF((Select Count(*) from  Settings.Localization where LanguageId = 2 and ResourceKey = 'Admin.Service.Academy') = 0)
Begin
	Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Admin.Service.Academy', 'Academy - My shop on the AdvantShop platform');
End
IF((Select Count(*) from  Settings.Localization where LanguageId = 1 and ResourceKey = 'Admin.Service.Tariffs') = 0)
Begin
	Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Admin.Service.Tariffs', 'Тарифы - Мой магазин на платформе AdvantShop');
End
IF((Select Count(*) from  Settings.Localization where LanguageId = 2 and ResourceKey = 'Admin.Service.Tariffs') = 0)
Begin
	Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Admin.Service.Tariffs', 'Tariffs - My shop on the AdvantShop platform');
End
IF((Select Count(*) from  Settings.Localization where LanguageId = 1 and ResourceKey = 'Admin.Service.ChangeTariff') = 0)
Begin
	Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Admin.Service.ChangeTariff', 'Сменить тариф - Мой магазин на платформе AdvantShop');
End
IF((Select Count(*) from  Settings.Localization where LanguageId = 2 and ResourceKey = 'Admin.Service.Tariffs') = 0)
Begin
	Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Admin.Service.Tariffs', 'Change tariff - My shop on the AdvantShop platform');
End
IF((Select Count(*) from  Settings.Localization where LanguageId = 1 and ResourceKey = 'Admin.Service.Domain') = 0)
Begin
	Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Admin.Service.Domain', 'Привязать домен - Мой магазин на платформе AdvantShop');
End
IF((Select Count(*) from  Settings.Localization where LanguageId = 2 and ResourceKey = 'Admin.Service.Domain') = 0)
Begin
	Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Admin.Service.Domain', 'Associate domain - My shop on the AdvantShop platform');
End
IF((Select Count(*) from  Settings.Localization where LanguageId = 1 and ResourceKey = 'Admin.Service.SupportCenter') = 0)
Begin
	Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Admin.Service.SupportCenter', 'Центр поддержки - Мой магазин на платформе AdvantShop');
End
IF((Select Count(*) from  Settings.Localization where LanguageId = 2 and ResourceKey = 'Admin.Service.SupportCenter') = 0)
Begin
	Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Admin.Service.SupportCenter', 'Support center - My shop on the AdvantShop platform');
End
IF((Select Count(*) from  Settings.Localization where LanguageId = 1 and ResourceKey = 'Admin.Service.BuyTemplate') = 0)
Begin
	Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Admin.Service.BuyTemplate', 'Способ оплаты - Мой магазин на платформе AdvantShop');
End
IF((Select Count(*) from  Settings.Localization where LanguageId = 2 and ResourceKey = 'Admin.Service.BuyTemplate') = 0)
Begin
	Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Admin.Service.BuyTemplate', 'Payment method - My shop on the AdvantShop platform');
End

GO--

ALTER PROCEDURE [Catalog].[sp_ParseProductProperty]      
  @nameProperty nvarchar(100),      
  @propertyValue nvarchar(255),      
  @rangeValue float,    
  @productId int,      
  @sort int      
AS      
BEGIN      
 -- select or create property      
 Declare @propertyId int      
 if ((select count(PropertyID) from Catalog.[Property] where Name = @nameProperty)= 0)      
  begin      
   insert into Catalog.[Property] (Name,UseInFilter,UseInBrief,Useindetails,SortOrder,[type], NameDisplayed) values (@nameProperty,1,0,1,0,1, @nameProperty)      
   set @propertyId = (Select SCOPE_IDENTITY())      
  end      
 else      
  set @propertyId = (select top(1) PropertyID from Catalog.[Property] where Name = @nameProperty)      
      
  -- select or create value      
  Declare @propertyValueId int      
      
  Declare @useinfilter bit      
  set @useinfilter = (Select Top 1 UseInFilter from Catalog.[Property] Where PropertyID=@propertyId)      
  Declare @useindetails bit      
  set @useindetails = (Select Top 1 UseInDetails from Catalog.[Property] Where PropertyID=@propertyId)      
      
  if ((select count(PropertyValueID) from Catalog.[PropertyValue] where Value = @propertyValue and PropertyId=@propertyId)= 0)      
   begin      
    insert into Catalog.[PropertyValue] (PropertyId, Value, UseInFilter, UseInDetails, SortOrder, RangeValue) values (@propertyId, @propertyValue, @useinfilter, @useindetails, 0, @rangeValue)      
    set @propertyValueId = (Select SCOPE_IDENTITY())      
   end      
  else      
   set @propertyValueId = (select top(1) PropertyValueID from Catalog.[PropertyValue] where Value = @propertyValue and PropertyId=@propertyId)      
       
 --create link between product and property value      
 if ((select Count(*) from Catalog.ProductPropertyValue where ProductID=@productId and PropertyValueID=@propertyValueId)=0)      
  insert into Catalog.ProductPropertyValue (ProductID,PropertyValueID) values (@productId,@propertyValueId)       
END 

GO--

CREATE TABLE [Order].[ShippingCountryExcluded](
	[MethodId] [int] NOT NULL,
	[CountryId] [int] NOT NULL,
 CONSTRAINT [PK_ShippingPaymentCountryExcluded_1] PRIMARY KEY CLUSTERED 
(
	[MethodId] ASC,
	[CountryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO--

ALTER TABLE [Order].[ShippingCountryExcluded]  WITH CHECK ADD  CONSTRAINT [FK_ShippingCountryExcluded_Country] FOREIGN KEY([CountryId])
REFERENCES [Customers].[Country] ([CountryID])
ON UPDATE CASCADE
ON DELETE CASCADE

GO--

ALTER TABLE [Order].[ShippingCountryExcluded] CHECK CONSTRAINT [FK_ShippingCountryExcluded_Country]

GO--

ALTER TABLE [Order].[ShippingCountryExcluded]  WITH CHECK ADD  CONSTRAINT [FK_ShippingCountryExcluded_ShippingMethod] FOREIGN KEY([MethodId])
REFERENCES [Order].[ShippingMethod] ([ShippingMethodID])
ON UPDATE CASCADE
ON DELETE CASCADE

GO--

ALTER TABLE [Order].[ShippingCountryExcluded] CHECK CONSTRAINT [FK_ShippingCountryExcluded_ShippingMethod]

GO--

IF (EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'Module' 
                 AND  TABLE_NAME = 'BuyMore'))
BEGIN
	IF NOT EXISTS( SELECT * FROM [information_schema].[columns] WHERE table_name = 'BuyMore' AND column_name = 'ShippingsIds') 
	BEGIN
		Alter Table Module.BuyMore Add ShippingsIds nvarchar(50) null
	END

END
GO--

Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Admin.Settings.SeoSettings.TagsMeta', 'Тэги (мета по умолчанию)');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Admin.Settings.SeoSettings.TagsDefaultTitle', 'Заголовок страницы');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Admin.Settings.SeoSettings.TagsDefaultMetaKeywords', 'Ключевые слова');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Admin.Settings.SeoSettings.TagsDefaultMetaDescription', 'Мета описание');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Admin.Settings.SeoSettings.TagsDefaultH1', 'H1');

Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Admin.Settings.SeoSettings.TagsMeta', 'Tags (default meta)');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Admin.Settings.SeoSettings.TagsDefaultTitle', 'Page title');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Admin.Settings.SeoSettings.TagsDefaultMetaKeywords', 'Meta keywords');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Admin.Settings.SeoSettings.TagsDefaultMetaDescription', 'Meta description');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Admin.Settings.SeoSettings.TagsDefaultH1', 'H1');

Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Admin.MetaVariables.NewsCategory.Name', 'Название категории новостей');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Admin.MetaVariables.NewsCategory.Name', 'News category name');

Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Admin.Analytics.ExportOrders.Title', 'Выгрузка заказов');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Admin.Analytics.ExportOrders.Status', 'Выгружать заказы со статусом');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Admin.Analytics.ExportOrders.Period', 'Выгрузить заказы за период');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Admin.Analytics.ExportOrders.Period.From', 'От');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Admin.Analytics.ExportOrders.Period.To', 'До');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Admin.Analytics.ExportOrders.Encoding', 'Кодировка файла');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Admin.Analytics.ExportOrders.Export', 'Экспортировать');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Admin.ProgressData.ProcessExportSuccess', 'Данные успешно экспортированы.');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Admin.ProgressData.DownloadFile', 'Скачать файл');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Admin.Analytics.ExportOrders.ProcessName', 'Выгрузка данных о заказах');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.ExportImport.ExcelSingleOrder.PaymentExtracharge', 'Оплата:');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.ExportImport.ExcelSingleOrder.ShippingPrice', 'Доставка:');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.ExportImport.MultiOrder.NullStatus', 'Неизвестный');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.ExportImport.MultiOrder.NullCustomer', 'Неизвестный');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.ExportImport.MultiOrder.OrderID', 'Номер заказа');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.ExportImport.MultiOrder.Status', 'Статус');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.ExportImport.MultiOrder.OrderDate', 'Дата заказа');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.ExportImport.MultiOrder.FIO', 'ФИО');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.ExportImport.MultiOrder.CustomerEmail', 'Email');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.ExportImport.MultiOrder.CustomerPhone', 'Телефон');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.ExportImport.MultiOrder.OrderedItems', 'Товары');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.ExportImport.MultiOrder.Total', 'Итоговая стоимость');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.ExportImport.MultiOrder.Currency', 'Валюта');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.ExportImport.MultiOrder.Tax', 'Налог');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.ExportImport.MultiOrder.Cost', 'Стоимость товаров');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.ExportImport.MultiOrder.Profit', 'Выгода');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.ExportImport.MultiOrder.Payment', 'Метод оплаты');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.ExportImport.MultiOrder.Shipping', 'Метод доставки');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.ExportImport.MultiOrder.ShippingAddress', 'Адрес доставки');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.ExportImport.MultiOrder.CustomerComment', 'Комментарий пользователя');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.ExportImport.MultiOrder.AdminComment', 'Комментарй администратора');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.ExportImport.MultiOrder.StatusComment', 'Комментарий к статусу');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.ExportImport.MultiOrder.Payed', 'Оплачен');

Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Admin.Analytics.ExportOrders.Title', 'Export orders');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Admin.Analytics.ExportOrders.Status', 'Export orders with status');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Admin.Analytics.ExportOrders.Period', 'Export orders for the period');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Admin.Analytics.ExportOrders.Period.From', 'From');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Admin.Analytics.ExportOrders.Period.To', 'To');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Admin.Analytics.ExportOrders.Encoding', 'File encoding');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Admin.Analytics.ExportOrders.Export', 'Export');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Admin.ProgressData.ProcessExportSuccess', 'Data is exported successfully.');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Admin.ProgressData.DownloadFile', 'Download file');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Admin.Analytics.ExportOrders.ProcessName', 'Export orders data');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.ExportImport.ExcelSingleOrder.PaymentExtracharge', 'Payment:');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.ExportImport.ExcelSingleOrder.ShippingPrice', 'Shipping:');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.ExportImport.MultiOrder.NullStatus', 'Unknown');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.ExportImport.MultiOrder.NullCustomer', 'Unknown');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.ExportImport.MultiOrder.OrderID', 'Order number');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.ExportImport.MultiOrder.Status', 'Status');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.ExportImport.MultiOrder.OrderDate', 'Order date');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.ExportImport.MultiOrder.FIO', 'Name');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.ExportImport.MultiOrder.CustomerEmail', 'Email');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.ExportImport.MultiOrder.CustomerPhone', 'Phone');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.ExportImport.MultiOrder.OrderedItems', 'Order items');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.ExportImport.MultiOrder.Total', 'Total');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.ExportImport.MultiOrder.Currency', 'Currency');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.ExportImport.MultiOrder.Tax', 'Tax');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.ExportImport.MultiOrder.Cost', 'Products cost');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.ExportImport.MultiOrder.Profit', 'Profit');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.ExportImport.MultiOrder.Payment', 'Payment method');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.ExportImport.MultiOrder.Shipping', 'Shipping method');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.ExportImport.MultiOrder.ShippingAddress', 'Shipping address');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.ExportImport.MultiOrder.CustomerComment', 'Customer comment');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.ExportImport.MultiOrder.AdminComment', 'Admin comment');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.ExportImport.MultiOrder.StatusComment', 'Status comment');
Insert into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.ExportImport.MultiOrder.Payed', 'Payed');
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
		WHERE   [Photo].[ObjId]=@ProductId and Type ='Product' and (Photo.ColorID = @ColorID) order by Main DESC, PhotoSortOrder ASC
	end
	RETURN @Result
END

GO--

UPDATE [Settings].[InternalSettings] SET [settingValue] = '6.0.3' WHERE [settingKey] = 'db_version'
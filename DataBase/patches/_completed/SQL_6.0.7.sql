ALTER PROCEDURE [Settings].[sp_GetExportFeedProducts] @exportFeedId int
	,@onlyCount BIT
	,@exportNotAvailable bit
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
							AND [ProductCategories].[CategoryId] in (SELECT CategoryId FROM @lcategory)
								Order By Main DESC, [ProductCategories].[CategoryId]
						) = [ProductCategories].[CategoryId]
					AND (Offer.Price > 0 OR @exportNotAvailable = 1)
					AND (
						Offer.Amount > 0
						OR (Product.AllowPreOrder = 1 and  @allowPreOrder = 1)
						OR @exportNotAvailable = 1
						)
					AND CategoryEnabled = 1
					AND (Product.Enabled = 1 OR @exportNotAvailable = 1)
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
			,[Product].[DiscountAmount]
			,AllowPreOrder
			,Amount
			,[ProductCategories].[CategoryId] AS [ParentCategory]
			,[Offer].[Price] AS Price
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
			,[Product].Cbid
			,[Product].Fee
			,[Product].YandexSizeUnit
			,[Product].YandexName
			,[Product].MinAmount
			,[Product].Multiplicity		
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
					AND [ProductCategories].[CategoryId] in (SELECT CategoryId FROM @lcategory)
						Order By Main DESC, [ProductCategories].[CategoryId]
				) = [ProductCategories].[CategoryId]
			AND (Offer.Price > 0 OR @exportNotAvailable = 1)
			AND (
				Offer.Amount > 0
				OR (Product.AllowPreOrder = 1 and  @allowPreOrder = 1)
				OR @exportNotAvailable = 1
				)
			AND CategoryEnabled = 1
			AND (Product.Enabled = 1 OR @exportNotAvailable = 1)
	END
END

GO--

INSERT INTO Settings.Localization (LanguageId,ResourceKey,ResourceValue) VALUES (1,'Admin.ExportFeed.SettingsYandex.AllowPreOrderProducts','Выгружать товары под заказ')
INSERT INTO Settings.Localization (LanguageId,ResourceKey,ResourceValue) VALUES (2,'Admin.ExportFeed.SettingsYandex.AllowPreOrderProducts','Export PreOrder Products')

GO--

Update [Settings].[Localization] set [ResourceValue] =  '{0} - сумма заказов от {1} руб - {2}' where [LanguageId] = 1 and [ResourceKey] = 'Module.BonusSystem.CardGradesFormat'

GO--

Update [Settings].[Localization] set 
[ResourceValue] =  ' #NUMBER# - внутренний (уникальный) номер заказа <br> #YEAR# - год<br>#MONTH# - месяц <br> #DAY# - день <br> #RRR# - случайно сгенерированное число <br> переменные #NUMBER# или #RRR# обязательно должны присутствовать' 
where [LanguageId] = 1 and [ResourceKey] = 'Admin.Settings.Checkout.OrderNumberFormatComment'

Update [Settings].[Localization] set 
[ResourceValue] =  ' #NUMBER# - internal (unique) order number <br> #YEAR# - year <br> #MONTH# - month <br> #DAY# - day <br> #RRR# - random number <br> variables #NUMBER# or #RRR# shall be present' 
where [LanguageId] = 2 and [ResourceKey] = 'Admin.Settings.Checkout.OrderNumberFormatComment'

GO--

alter table [Order].orderitems alter column name nvarchar(255)

GO--

ALTER PROCEDURE [Order].[sp_AddOrderItem]  
	 @OrderID int,  
	 @Name nvarchar(255),  
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
	@Name nvarchar(255),  
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

update settings.Localization set ResourceValue = 'Участие в программе Заказ на маркете' where ResourceKey = 'Core.ExportImport.ProductFields.Cpa' and LanguageId = 1

GO--

delete from settings.localization where resourcekey = 'Admin.ExportFeeed.SettingsYandex.Available'

GO--

Insert into Settings.Localization (LanguageId,ResourceKey,ResourceValue) 
Values(1,'Admin.ExportFeeed.SettingsCsv.AllOffersToMultiOfferColumn','Выгружать все цены в колонку модификаций')

Insert into Settings.Localization (LanguageId,ResourceKey,ResourceValue) 
Values(2,'Admin.ExportFeeed.SettingsCsv.AllOffersToMultiOfferColumn','Unload all prices in the modification column')

GO--


CREATE TABLE [Shipping].[DDeliveryOrders](
	[OrderId] [int] NOT NULL,
	[DDeliveryOrder] [nvarchar](100) NOT NULL
) ON [PRIMARY]

ALTER TABLE [Shipping].[DDeliveryOrders]  WITH CHECK ADD  CONSTRAINT [FK_DDeliveryOrders_Order] FOREIGN KEY([OrderId])
REFERENCES [Order].[Order] ([OrderID])

GO--

Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Admin.ShippingMethods.DDelivery.ShopId', 'Идентификатор магазина')
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Admin.ShippingMethods.DDelivery.ShopId', 'Shop id')

Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Admin.ShippingMethods.DDelivery.GetShopId', 'Идентификатор магазина DDelivery(смотреть в Личном кабинете)')
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Admin.ShippingMethods.DDelivery.GetShopId', '
DDelivery store identifier (see in the Personal Area)')

Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Admin.ShippingMethods.DDelivery.UseWidget', 'Использовать виджет карты')
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Admin.ShippingMethods.DDelivery.UseWidget', 'Use map widget')

Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Admin.ShippingMethods.DDelivery.NoticeUseWidget', 'Переключение отображение рассчета доставки, с выпадющих списков, сортированных по компаниям на виджет Ddelivery')
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Admin.ShippingMethods.DDelivery.NoticeUseWidget', 'Switching the display of the delivery calculation, from drop-down lists, sorted by companies to the Ddelivery widget')


INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.ShippingMethods.DDelivery.Token','Токен')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.ShippingMethods.DDelivery.Token','Token')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.ShippingMethods.DDelivery.GetApiKey','Получить уникальный ключ (API-key) для авторизации в панели администратора сервиса.')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.ShippingMethods.DDelivery.GetApiKey','Get a unique key (API-key) for authorization in the service administrator panel.')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.ShippingMethods.DDelivery','Компания забора посылок')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.ShippingMethods.DDelivery','Parcel picking company')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.ShippingMethods.DDelivery.ListOfParcelPickingCompanies','Перечень компаний забора посылок')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.ShippingMethods.DDelivery.ListOfParcelPickingCompanies','List of parcel picking companies')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.ShippingMethods.DDelivery.CreateDraftOrderInDD','Создавать черновик заказа в системе DDelivery')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.ShippingMethods.DDelivery.CreateDraftOrderInDD','Create draft order in DDelivery system')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.ShippingMethods.DDelivery.CreateDraftOrder','Создавать черновик заказа')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.ShippingMethods.DDelivery.CreateDraftOrder','Create draft order')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.ShippingMethods.DDelivery.WhenCreatingAnOrder','При создании заказа в системе DDelivery, заказ будет создан в виде черновика (Рекомендовано)')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.ShippingMethods.DDelivery.WhenCreatingAnOrder','When creating an order in the DDelivery system, the order will be created as a draft (Recommended)')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.ShippingMethods.DDelivery.GroupDeliveryByType','Группировать доставку по типу')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.ShippingMethods.DDelivery.GroupDeliveryByType','Group delivery by type')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.ShippingMethods.DDelivery.ShippingMethodsGroupedByType','Способы доставки будут сгруппированы по типу доставки: пункты самовывоза, курьер и почта.')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.ShippingMethods.DDelivery.ShippingMethodsGroupedByType','Shipping methods will be grouped by type of delivery: self-delivery items, courier and mail.')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.ShippingMethods.DDelivery.DefaultProductParameters','Значения параметров товара по умолчанию')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.ShippingMethods.DDelivery.DefaultProductParameters','Default product parameter values')


GO--

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.Grastin.OrderActions.SendToGrastin','Отправить заказ в Grastin')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.Grastin.OrderActions.SendToGrastin','Send order to Grastin')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.Grastin.OrderActions.SendToGrastinPostRF','Отправить заказ в Grastin для почты РФ')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.Grastin.OrderActions.SendToGrastinPostRF','Send order to Grastin for RF post')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.Grastin.OrderActions.SendToGrastinBoxberry','Отправить заказ в Grastin для Boxberry')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.Grastin.OrderActions.SendToGrastinBoxberry','Send order to Grastin for Boxberry')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.Grastin.OrderActions.SendToGrastinHermes','Отправить заказ в Grastin для Hermes')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.Grastin.OrderActions.SendToGrastinHermes','Send order to Grastin for Hermes')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.Grastin.OrderActions.SendToGrastinForOrderPicking','Отправить заявку в Grastin на забор заказов')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.Grastin.OrderActions.SendToGrastinForOrderPicking','Send an application to Grastin for the order picking')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.Grastin.OrderActions.ReceiveFromGrastinAct','Получить из Grastin акт приема-передачи')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.Grastin.OrderActions.ReceiveFromGrastinAct','Receive from Grastin the act of acceptance-transfer')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.Grastin.OrderActions.ReceiveFromGrastinOrderMarking','Получить из Grastin маркировку заказа')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.Grastin.OrderActions.ReceiveFromGrastinOrderMarking','Get from Grastin order marking')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.Grastin.OrderShipment','Отправка заказа ')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.Grastin.OrderShipment','Order shipment ')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.Grastin.ToGrastin','в Grastin')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.Grastin.ToGrastin','to Grastin')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.Grastin.Warehouse','Склад приемка')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.Grastin.Warehouse','Warehouse acceptance')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.Grastin.FullName','ФИО')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.Grastin.FullName','Full name')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.Grastin.MobilePhone','Мобильный телефон')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.Grastin.MobilePhone','Mobile phone')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.Grastin.LandlinePhone','Стационарный телефон')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.Grastin.LandlinePhone','Landline phone')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.Grastin.DeliveryType','Тип доставки')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.Grastin.DeliveryType','Delivery type')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.Grastin.AdressSelfDelivery','Адрес точки самовывоза')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.Grastin.AdressSelfDelivery','Address of the point of self-delivery')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.Grastin.ZIPofDeliveryCity','Индекс города доставки')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.Grastin.ZIPofDeliveryCity','ZIP of delivery city')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.Grastin.Adress','Адрес')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.Grastin.Adress','Adress')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.Grastin.Note','Примечание')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.Grastin.Note','Note')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.Grastin.COD','Наложенный платеж')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.Grastin.COD','C.O.D.')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.Grastin.AssessedValue','Оценочная стоимость')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.Grastin.AssessedValue','Assessed value')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.Grastin.NumberOfSeats','Количество мест')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.Grastin.NumberOfSeats','Number of seats')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.Grastin.Cancel','Отмена')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.Grastin.Cancel','Cancel')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.Grastin.Send','Отправить')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.Grastin.Send','Send')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.Grastin.DateDelivery','Дата доставки')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.Grastin.DateDelivery','Delivery date')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.Grastin.Zip','Индекс')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.Grastin.Zip','ZIP')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.Grastin.Region','Регион')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.Grastin.Region','Region')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.Grastin.Area','Район')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.Grastin.Area','Area')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.Grastin.City','Город')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.Grastin.City','City')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.Grastin.DeliveryTime','Время доставки')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.Grastin.DeliveryTime','Delivery time')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.Grastin.From','с')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.Grastin.From','from')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.Grastin.To','по')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.Grastin.To','to')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.Grastin.OfficeTransportCompany','Офис транспортной компании')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.Grastin.OfficeTransportCompany','Office of the transport company')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.Grastin.RecipientType','Вид получателя')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.Grastin.RecipientType','Recipient type')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.Grastin.PassportData','Паспортные данные')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.Grastin.PassportData','Passport data')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.Grastin.CustomerOrganization','Организация покупателя')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.Grastin.CustomerOrganization','Customer organization')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.Grastin.INN','ИНН')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.Grastin.INN','Individual taxpayer number')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.Grastin.Checkpoint','Контрольно-пропускной пункт')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.Grastin.Checkpoint','Checkpoint')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.Grastin.ReceiveAct','Получение из Grastin акта приема-передачи')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.Grastin.ReceiveAct','Receiving from the Grastin the act of transfer and acceptance')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.Grastin.Contract','Договор')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.Grastin.Contract','Contract')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.Grastin.Receive','Получить')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.Grastin.Receive','Receive')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.Grastin.SendindApplication','Отправка заявки в Grastin на забор заказов')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.Grastin.SendindApplication','Sending an application to Grastin on the order picking')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.Grastin.RegionPickGoods','Регион забора товаров')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.Grastin.RegionPickGoods','Region for pick up goods')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.Grastin.TimePickUp','Время забора')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.Grastin.TimePickUp','Time for pick up')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.Grastin.VolumeOfOrders','Объем заказов')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.Grastin.VolumeOfOrders','Volume of orders')


INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.ShippingMethods.Grastin.APIkey','API ключ')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.ShippingMethods.Grastin.APIkey','API key')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.ShippingMethods.Grastin.PrefixToOrder','Префикс к заказу')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.ShippingMethods.Grastin.PrefixToOrder','Prefix to order')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.ShippingMethods.Grastin.PaymentForDelivery','Оплата для доставки')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.ShippingMethods.Grastin.PaymentForDelivery','Payment for delivery')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.ShippingMethods.Grastin.PaymentForSelfDelivery','Оплата для самовывоза')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.ShippingMethods.Grastin.PaymentForSelfDelivery','Payment for self-delivery')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.ShippingMethods.Grastin.DepartureCity','Город отправления')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.ShippingMethods.Grastin.DepartureCity','Departure city')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.ShippingMethods.Grastin.AgreementInMoscow','Выберите договор по Москве')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.ShippingMethods.Grastin.AgreementInMoscow','Choose a contract for Moscow')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.ShippingMethods.Grastin.AgreementInStPetersburg','Выберите договор по Санкт-Петербургу')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.ShippingMethods.Grastin.AgreementInStPetersburg','Choose a contract for St. Petersburg')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.ShippingMethods.Grastin.NizhnyNovgorod','Выберите договор по Нижнему Новгороду')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.ShippingMethods.Grastin.NizhnyNovgorod','Choose a contract for Nizhny Novgorod')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.ShippingMethods.Grastin.Orel','Выберите договор по Орлу')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.ShippingMethods.Grastin.Orel','Select the contract for Orel')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.ShippingMethods.Grastin.BoxberryContract','Выберите договор Boxberry')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.ShippingMethods.Grastin.BoxberryContract','Select a contract Boxberry')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.ShippingMethods.Grastin.DeliveryMethods','Методы доставки')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.ShippingMethods.Grastin.DeliveryMethods','Delivery methods')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.ShippingMethods.Grastin.SelectSeveralOptions','Можно выбрать несколько вариантов зажав клавиши Shift или Ctrl')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.ShippingMethods.Grastin.SelectSeveralOptions','You can select several options by holding down the Shift or Ctrl key')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.ShippinhMethods.Grastin.WithInsurance','Со страховкой')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.ShippinhMethods.Grastin.WithInsurance','With insurance')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.ShippingMethods.Grastin.HidePartners','Не показывать партнеров')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.ShippingMethods.Grastin.HidePartners','Do not show partners')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.ShippingMethods.Grastin.HideCityOfDeparture','Не показывать город отправления')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.ShippingMethods.Grastin.HideCityOfDeparture','Do not show city of departure')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.ShippingMethods.Grastin.ProhibitChangeDepartureCity','Запретить изменять город отправления')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.ShippingMethods.Grastin.ProhibitChangeDepartureCity','Prohibit changing city of departure')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.ShippingMethods.Grastin.DoNotShowShippingCost','Не показывать стоимость доставки')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.ShippingMethods.Grastin.DoNotShowShippingCost','Do not show the shipping cost')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.ShippingMethods.Grastin.DoNotShowDeliveryTime','Не показывать срок доставки')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.ShippingMethods.Grastin.DoNotShowDeliveryTime','Do not show delivery time')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.ShippingMethods.Grastin.TypeOfExtraCharge','Тип наценки')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.ShippingMethods.Grastin.TypeOfExtraCharge','Type of extra charge')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.ShippingMethods.Grastin.SpecifyTypeOfExtraCharge','Тут вы указываете тип наценки, если она есть.')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.ShippingMethods.Grastin.SpecifyTypeOfExtraCharge','Here you specify the type of the extra charge, if it is.')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.ShippingMethods.Grastin.FixedNumberExample100','Фиксированная - это фиксированное число, скажем 100 руб.')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.ShippingMethods.Grastin.FixedNumberExample100','Fixed - this is a fixed number, example 100 rubles.')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.ShippingMethods.Grastin.PercentageIfPercentageExample3','Процентная - это процент, скажем 3%.')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.ShippingMethods.Grastin.PercentageIfPercentageExample3','Percentage - is percentage, example 3%.')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.ShippingMethods.Grastin.ExtraCharge','Наценка')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.ShippingMethods.Grastin.ExtraCharge','Extra charge')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.ShippingMethods.Grastin.AddDaysToDeliveryTime','Добавить дни к сроку доставки')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.ShippingMethods.Grastin.AddDaysToDeliveryTime','Add days to delivery time')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.ShippingMethods.Grastin.ProhibitShowingPartnersInSpecificCities','Запретить показ партнеров в конкретных городах')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.ShippingMethods.Grastin.ProhibitShowingPartnersInSpecificCities','Prohibit showing partners in specific cities')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.ShippingMethods.Grastin.Example','Пример: ')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.ShippingMethods.Grastin.Example','Example: ')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.ShippingMethods.Grastin.ExampleStPetersburg','Санкт-Петербург')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.ShippingMethods.Grastin.ExampleStPetersburg','St. Petersburg')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.ShippingMethods.Grastin.Example.Tver','Тверь')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.ShippingMethods.Grastin.Example.Tver','Tver')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.ShippingMethods.Grastin.CreatePaymentMethodPaymentByCourier','Создать метод оплаты "Оплата курьеру"')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.ShippingMethods.Grastin.CreatePaymentMethodPaymentByCourier','Create a payment method "Payment by courier"')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.ShippingMethods.Grastin.CreatePaymentUponReceiptOfAnOrder','Создать метод оплаты "Оплата при получении заказа в пункте выдачи"')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.ShippingMethods.Grastin.CreatePaymentUponReceiptOfAnOrder','Create a payment method "Payment upon receipt of an order at the issuance point"')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.ShippingMethods.Grastin.SynchronizeOrderStatuses','Синхронизировать статусы заказов из Grastin')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.ShippingMethods.Grastin.SynchronizeOrderStatuses','Synchronize order statuses from Grastin')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.ShippingMethods.Grastin.Statuses','Статусы')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.ShippingMethods.Grastin.Statuses','Statuses')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.ShippingMethods.Grastin.DraftAcceptedViaAPI','Черновик, принят через API или личный кабинет и ждет рассмотрения логиста')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.ShippingMethods.Grastin.DraftAcceptedViaAPI','Draft, accepted via API or personal cabinet and waiting for the logistic review')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.ShippingMethods.Grastin.ConsideredByLogistician','Новый. Рассмотрен логистом, ждет получения заказа на складе')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.ShippingMethods.Grastin.ConsideredByLogistician','New. Considered by the logistician, waiting for receipt of the order in the warehouse')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.ShippingMethods.Grastin.Return','Возврат')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.ShippingMethods.Grastin.Return','Return')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.ShippingMethods.Grastin.Completed','Выполнен')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.ShippingMethods.Grastin.Completed','Completed')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.ShippingMethods.Grastin.OnDelivery','На доставке. Заказ отгружен курьеру')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.ShippingMethods.Grastin.OnDelivery','On delivery. The order is shipped to the courier')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.ShippingMethods.Grastin.OrderWasReceivedAtTheWarehouse','Получен от клиента. Заказ получен на склад от клиента')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.ShippingMethods.Grastin.OrderWasReceivedAtTheWarehouse','Received from the client. The order was received at the warehouse from the customer')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.ShippingMethods.Grastin.OrderCancelled','Заказ отменен')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.ShippingMethods.Grastin.OrderCancelled','Order cancelled')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.ShippingMethods.Grastin.OrderIsPreparedForShipment','Заказ подготовлен к отгрузке курьеру в машину')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.ShippingMethods.Grastin.OrderIsPreparedForShipment','The order is prepared for shipment to the courier in the car')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.ShippingMethods.Grastin.ProblemOrder','Проблемный заказ')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.ShippingMethods.Grastin.ProblemOrder','Problem order')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.ShippingMethods.Grastin.ReturnedToClient','Возвращен клиенту')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.ShippingMethods.Grastin.ReturnedToClient','Returned to client')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.ShippingMethods.Grastin.WrittenOff','Списан')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.ShippingMethods.Grastin.WrittenOff','Written off')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.ShippingMethods.Grastin.Weight','Вес')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.ShippingMethods.Grastin.Weight','Weight')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.ShippingMethods.Grastin.ProductWeight','Вес товара')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.ShippingMethods.Grastin.ProductWeight','Product weight')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.ShippingMethods.Grastin.ProductWeightWillAssume','Вес товара примет указанное значение, если у товара данный параметр не был задан.')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.ShippingMethods.Grastin.ProductWeightWillAssume','The product''s weight will assume the specified value if the item has not been specified for the product.')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.ShippingMethods.Grastin.ValueIsSpecifiedInKg','Значение указывается в кг, возможно указать дробное значение.')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.ShippingMethods.Grastin.ValueIsSpecifiedInKg','The value is specified in kg, it is possible to specify a fractional value.')

GO--

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.Js.OrderItemsSummary.DraftDDelivery','Создать черновик заказа в DDelivery')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.Js.OrderItemsSummary.DraftDDelivery','Create a draft order in DDelivery')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.Js.OrderItemsSummary.InformationDDelivery','Информация о заказе в DDelivery')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.Js.OrderItemsSummary.InformationDDelivery','Order information in DDelivery')

INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (1,'Admin.Js.OrderItemsSummary.CloseDDelivery','Закрыть заказ в DDelivery')
INSERT INTO [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) VALUES (2,'Admin.Js.OrderItemsSummary.CloseDDelivery','Close the order in DDelivery')

GO--

UPDATE [Settings].[InternalSettings] SET [settingValue] = '6.0.7' WHERE [settingKey] = 'db_version'
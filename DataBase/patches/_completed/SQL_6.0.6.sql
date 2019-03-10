
If (NOT EXISTS(Select * From [CMS].[StaticBlock] Where [Key] = 'mainpage_before_carousel'))
Begin
	Insert Into [CMS].[StaticBlock] ([Key],[InnerName],[Content],[Added],[Modified],[Enabled]) Values ('mainpage_before_carousel', 'Блок перед каруселью', '', GETDATE(), GETDATE(), 0)
End


GO--



ALTER PROCEDURE [Settings].[sp_GetExportFeedProducts] @exportFeedId int
	,@onlyCount BIT
	,@exportNotAvailable bit
	,@selectedCurrency NVARCHAR(10)
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
						OR Product.AllowPreOrder = 1
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
				OR Product.AllowPreOrder = 1
				OR @exportNotAvailable = 1
				)
			AND CategoryEnabled = 1
			AND (Product.Enabled = 1 OR @exportNotAvailable = 1)
	END
END



GO--

ALTER TABLE Bonus.Card ADD ManualGrade bit NOT NULL DEFAULT 0

GO--

If not exists (select top(1) CountryID from Customers.Country where CountryName = 'Россия')
    insert into Customers.Country (CountryName,CountryISO2,CountryISO3,DisplayInPopup,SortOrder,DialCode)
    values('Россия','RU','RUS',1,0,'7')

GO--

declare @regid int, @countryid int
set @regId = (select top(1) RegionID from Customers.Region where RegionName = 'Крым')
set @countryid = (select top(1) CountryID from Customers.Country where CountryName = 'Россия')

If (@regId) > 0
    delete from Customers.City where RegionID = @regId
else
    insert into Customers.Region (CountryID,RegionName,RegionCode,RegionSort) 
    Values(@countryid,'Крым',N'',0)

delete from shipping.SdekCities where OblName like '%Крым%'

GO--

declare @regid int
set @regId = (select top(1) RegionID from Customers.Region where RegionName = 'Крым')

insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Абрикосовка, Кировский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Абрикосовка, Сакский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Абрикосово',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Аграрное, гор. округ Симферополь',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Азов, Красногвардейский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Азовское, Джанкойский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Айвазовское, Кировский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Айвовое, Бахчисарайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Айкаван, Симферопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Акимовка',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Акрополис, Симферопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Александровка, Белогорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Александровка, Красногвардейский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Александровка, Симферопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Алексеевка, Белогорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Алексеевка, Первомайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Алупка, Ялтинский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Алушта',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Амурское',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Ана-Юрт, Симферопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Андреевка, Севастопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Андрусово, Симферопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Анновка, Белогорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Арбузово, Первомайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Армянск',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Аромат, Бахчисарайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Ароматное, Бахчисарайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Ароматное, Белогорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Артемовка',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Аэрофлотский, гор. округ Симферополь',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Бабенково',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Багерово',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Балаклава, Севастопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Баланово, Белогорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Барабаново, Белогорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Батальное',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Бахчисарай',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Баштановка, Бахчисарайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Белая скала',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Белоглинка',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Белогорск (Крым)',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Белокаменное',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Береговое',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Береговое, Бахчисарайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Береговое, гор. округ Ялта',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Березовка',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Битумный, гор. округ Симферополь',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Ближнее, гор. округ Феодосия',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Ближнее, Красногвардейский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Богатовка, ГО Судак',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Богатое',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Богатое Ущелье, Бахчисарайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Богатырь, Бахчисарайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Богдановка, Симферопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Большое Садовое, Бахчисарайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Бондаренково',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Бондаренково, гор. округ Алушта',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Бондаренково, Ленинский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Ботаническое',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Братское, Красноперекопский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Братское, Первомайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Брянское, Бахчисарайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Буревестник, Нижнегорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Валентиново',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Васильевка, Белогорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Васильковое, Кировский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Вересаево',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Вересаево, Сакский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Верхнесадовое',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Верхние Орешники, Белогорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Верхняя Кутузовка',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Верхоречье',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Веселовка',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Веселое, ГО Судак',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Весёлое, Симферопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Видное, Кировский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Видное, Красногвардейский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Викторовка, Бахчисарайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Вилино',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Винницкое',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Виноградное, гор. округ Феодосия',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Виноградное, Ленинский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Виноградный',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Виноградный, гор. округ Алушта',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Виноградово',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Витино, Сакский городской округ',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Вишенное',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Вишневка',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Вишневое, Севастопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Вишнёвое, Белогорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Вишняковка, Красногвардейский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Владимировка, Белогорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Владимирово, Красногвардейский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Владиславовка, Кировский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Владиславовка, Нижнегорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Водопойное, Черноморский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Воинка',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Войково, Ленинский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Войково, Первомайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Волошино, гор. округ Армянск',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Вольное, Джанкойский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Воробьёво',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Ворон гор. округ Судак',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Восточное',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Восход, гор. округ Ялта',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Восход, Красногвардейский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Высокогорное , гор. округ Ялта',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Высокое, Бахчисарайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Гаспра, Ялтинский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Гвардейское',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Гвардейское, Первомайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Генеральское',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Георгиевка, Советский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Геройское',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Глазовка',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Глубокий Яр, Бахчисарайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Головановка',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Голубинка, Бахчисарайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Гончарное, Севастопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Горка, Бахчисарайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Горлинка, Белогорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Горное, гор. округ Ялта',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Горностаевка',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Григорьевка, Красногвардейский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Гришино',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Громовка, гор. округ Судак',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Громово, Черноморский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Грушевка, ГО Судак',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Грушевое, Симферопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Грэсовский, гор. округ Симферополь',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Гурзуф, Ялтинский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Далекое',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Дальнее, Севастопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Дачное, Бахчисарайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Дачное, ГО Судак',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Демьяновка, Советский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Денисовка',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Джанкой',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Дивное, Белогорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Дивное, Симферопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Дмитровка, Советский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Днепровка',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Добровский',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Доброе, Симферопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Добролюбовка, Кировский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Добрушино',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Дозорное, Белогорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Долинное, Бахчисарайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Долинное, Кировский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Долиновка, Белогорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Донское',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Дорожное, Бахчисарайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Доходное, Красногвардейский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Дрофино',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Дружное, Симферопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Дубки, Симферопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Дубровка, Бахчисарайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Дубровское',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Дятловка, Советский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Евпатория',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Еленовка, Белогорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Елизаветово',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Емельяновка',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Ермаково',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Железнодорожное',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Желябовка',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Жемчужина',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Жемчужина Крыма',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Живописное, Симферопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Журавки, Кировский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Журавлевка, Симферопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Завет-Ленинский',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Заветное, Бахчисарайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Заветное, Ленинский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Заветное, Советский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Зайцево',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Залесное, Бахчисарайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Залесье, Симферопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Заливное, Нижнегорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Заозёрное, гор.окр. Евпатория',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Запрудное',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Заречное, Джанкойский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Заречное, Красногвардейский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Заря, Красногвардейский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Звёздное, Красногвардейский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Зеленая Нива',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Зеленогорское',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Зеленогорье',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Зеленое, Нижнегорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Зелёное, Бахчисарайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Земляничное',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Зерновое, Джанкойский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Зерновое, Красногвардейский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Зерновое, Сакский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Зимино',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Знаменское',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Золотое Поле',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Золотое, Красногвардейский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Зоркино',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Зубакино, Бахчисарайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Зуя',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Зыбины',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Ивановка, Ленинский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Ивановка, Нижнегорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Ивановка, Сакский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Известковое, Красногвардейский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Изобильное',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Изобильное, Кировский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Изобильное, Нижнегорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Изумрудное',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Изюмовка',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Ильинка, Красноперекопский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Ильичёво, Советский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Инкерман, Севастопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Источное, Красноперекопский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Ишунь',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Казанки, Бахчисарайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Калинино, Красногвардейский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Калинино, Первомайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Калиновка, Ленинский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Каменоломня, Сакский городской округ',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Камышинка, Симферопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Камышлы, Севастополь',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Камышное, Раздольненский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Карасёвка, Белогорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Карповка',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Карьерное',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Кацивели',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Кача, Севастопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Каштановка, Первомайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Каштановое',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Каштаны',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Керчь',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Кизиловка, Белогорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Кизиловое, Севастополь',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Кипарисное',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Кирово, Ленинский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Кировское',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Кировское, Черноморский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Кирсановка, Нижнегорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Клепинино',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Климово',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Клиновка, Симферопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Ключевое, Кировский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Ключевое, Симферопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Ключи, Симферопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Коврово, Нижнегорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Коктебель',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Колодезное, Красногвардейский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Коломенское, Советский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Колоски, Сакский городской округ',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Колхозное, Севастополь',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Кольцово',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Кольчугино',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Коммунары, Красногвардейский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Комсомольское, гор. округ Симферополь',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Кондратьево',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Константиновка, Симферопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Кореиз, Ялтинский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Кормовое',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Корнеевка, Советский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Косточковка',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Котельниково',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Кочергино, Бахчисарайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Красная Долина, Красногвардейский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Красная Заря, Бахчисарайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Красная Поляна',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Красная Поляна, Черноморский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Красная Слобода, Белогорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Красноармейское, Красноперекопский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Красновка, Кировский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Красногвардейское (Крым)',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Красногвардейское, Советский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Красногорское, Белогорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Краснодарка',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Краснознаменка',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Краснокаменка',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Краснокаменка, гор. округ Ялта',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Красноперекопск',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Красносельское',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Красносёловка, Белогорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Краснофлотское',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Красноярское, Черноморский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Красный Мак, Бахчисарайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Кремневка',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Крестьяновка, Первомайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Кривцово, Белогорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Криничное',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Кропоткино, Раздольненский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Крымка',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Крымская Роза',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Крымское',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Кубанское, Симферопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Кудрино, Бахчисарайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Куйбышево,  гор. округ Ялта',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Куйбышево, Бахчисарайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Кукурузное, Нижнегорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Кукушкино',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Куликовка, Сакский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Кумово, Раздольненский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Кунцево, Нижнегорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Куприно, Симферопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Курганное, Красногвардейский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Курортное, Белогорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Курортное, гор. округ Феодосия',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Курское',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Лавровое',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Лазаревка, Симферопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Лазурное',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Лебединка, Советский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Левадки',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Левитановка, Первомайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Лекарственное',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Ленино',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Ленинское, Красногвардейский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Ленинское, Ленинский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Лесновка',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Лесное, ГО Судак',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Лечебное',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Линейное, гор. округ Ялта',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Лиственное',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Литвиненково',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Лобаново',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Лозовое',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Луганское',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Луговое, Белогорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Луговое, Ленинский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Луговое, Нижнегорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Лужки, Нижнегорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Лучевое, Советский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Лучистое',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Лушино, Сакский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Льговское',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Любимовка, Севастопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Магазинка',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Мазанка',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Маковка, Советский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Маковское, Кировский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Максимовка, Раздольненский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Маленькое',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Малиновка, Бахчисарайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Малиновка, Белогорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Маловидное, Бахчисарайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Малое Садовое, Бахчисарайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Малореченское',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Малый маяк',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Марково, Советский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Мартыновка',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Марьино, Джанкойский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Марьяновка',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Марьяновка, Красногвардейский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Маслово',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Массандра',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Матросовка, Кировский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Машино, Бахчисарайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Маяк, Керчь',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Медведевка',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Медведево',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Межводное',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Межгорное, Симферопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Межгорье, Белогорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Междуречье, ГО Судак',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Межевое, Нижнегорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Мекензиевы Горы, Севастопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Мелехово, Белогорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Мелководное, Джанкойский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Мельники, Белогорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Мельничное, Белогорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Менделеево, Красногвардейский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Миндальное, ГО Судак',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Мирновка',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Мирное, Симферопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Мирный, Евпаторийский городской округ',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Миролюбовка',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Мироновка, Белогорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Мироновка, Красногвардейский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Митрофановка, Нижнегорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Митюрино, Джанкойский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Михайловка, Нижнегорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Михайловка, Сакский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Мичуринское',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Многоречье, Бахчисарайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Молодёжное, Симферопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Молочное, Красногвардейский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Молочное, Сакский городской округ',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Молочное, Симферопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Морское, ГО Судак',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Мостовое, Бахчисарайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Мраморное, Симферопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Муромское',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Мускатное',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Мысовое, Джанкойский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Нагорное, Бахчисарайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Надежда, Советский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Найденовка',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Насыпное',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Наташино',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Научный',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Научный, Бахчисарайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Нахимово, Красногвардейский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Находка, Джанкойский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Невское, Красногвардейский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Нежинское, Нижнегорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Некрасовка, Бахчисарайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Некрасовка, Советский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Некрасово, Белогорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Некрасово, Красногвардейский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Нижнегорский',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Нижнее запрудное',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Нижнезаморское',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Нижние Отрожки, Джанкойский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Нижняя Голубинка, Бахчисарайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Нижняя Кутузовка',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Низинное, Джанкойский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Низинное, Сакский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Никита',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Николаевка, Симферопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Николаевка, Советский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Новая Деревня, Первомайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Новая Жизнь',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Новенькое, Бахчисарайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Новоалександровка, Белогорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Новоандреевка',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Новобобровское, Балаклавский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Нововасильевка, Бахчисарайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Новогригорьевка, Белогорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Новогригорьевка, Нижнегорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Новодолинка, Красногвардейский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Новоекатериновка, Красногвардейский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Новожиловка',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Новозбурьевка, Симферопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Новоивановка',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Новоивановка, Красноперекопский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Новокленово',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Новоконстантиновка, Джанкойский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Новокрымское',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Новониколаевка, Ленинский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Новониколаевка, Симферопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Новоникольское, Красногвардейский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Новоозёрное',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Новоотрадное',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Новопавловка, Бахчисарайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Новопавловка, Красноперекопский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Новопокровка, Кировский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Новопокровка, Красногвардейский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Новополье',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Новоселовка, Симферопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Новоселовское, Раздольненский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Новосельское',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Новосёловка, Советский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Новостепное',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Новоульяновка, Бахчисарайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Новофедоровка',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Новофёдоровка, Джанкойский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Новоэстония',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Новый Мир, Симферопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Новый Сад, Симферопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Новый Свет, ГО Судак',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Овражки, Белогорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Озерное, Севастопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Октябрьское',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Октябрьское, Красногвардейский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Октябрьское, Ленинский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Октябрьское, Первомайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Октябрьское, Советский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Окуневка, Черноморский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Оленевка',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Олива',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Оползневое',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Опытное, Белогорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Орденоносное, Джанкойский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Орджоникидзе',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Орехово',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Орлиное, Севастопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Орловка, Красногвардейский район',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Орловка, Раздольненский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Орловка, Севастопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Орловское',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Осипенко, Севастопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Осовины, Ленинский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Останино',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Островское, Джанкойский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Островское, Первомайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Отважное, Кировский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Отрадное, Бахчисарайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Отрадное, гор. округ Ялта',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Отрадное, Джанкойский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Охотниково',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Охотничье, гор. округ Ялта',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Охотское',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Павловка, Джанкойский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Павловка, Севастопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Парковое, Ялтинский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Партенит',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Партизанское, гор. округ Ялта',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Партизанское, Симферопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Партизаны',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Пасечное, Белогорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Пахаревка, Джанкойский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Первомайское, Кировский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Первомайское, Первомайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Первомайское, Симферопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Переваловка, ГО Судак',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Перевальное',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Передовое, Балаклавский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Передовое, Симферопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Перекоп',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Перепёлкино, Джанкойский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Перово',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Песчаное',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Петровка',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Петровка, Симферопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Петрово, Белогорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Петропавловка, Симферопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Пионерское',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Пионерское, гор. округ Феодосия',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Пироговка, Нахимовский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Плодовое',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Плотинное',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Победино, Красногвардейский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Победное, Джанкойский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Поворотное, Белогорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Поворотное, Нахимовский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Подгорное ',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Пожарское, Симферопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Пологи, Красногвардейский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Полтавка',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Полюшко, Нахимовский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Поляна, Бахчисарайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Понизовка пгт, Ялтинский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Поповка, Сакский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Почтовое',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Правда',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Предмостное, Джанкойский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Предущельное, Бахчисарайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Прибрежное, Сакский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Приветное, гор. округ Алушта',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Приветное, Кировский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Приветное, Сакский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Привольное, Красноперекопский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Привольное, Симферопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Приморский',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Приозерное',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Приозёрное, Ленинский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Присивашное, Советский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Приятное Свидание, Бахчисарайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Пробуждение, Джанкойский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Прозрачное, Джанкойский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Пролётное, Симферопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Пролом, Белогорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Просторное',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Проточное, Красногвардейский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Прохладное',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Прудовое, Симферопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Пруды, Кировский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Пруды, Советский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Прямое, Красногвардейский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Путиловка, Бахчисарайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Пушкино, Алуштинский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Пушкино, Красногвардейский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Пушкино, Советский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Пчелиное, Белогорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Пчельники, Советский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Пшеничное, Нижнегорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Пшеничное, Первомайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Пятихатка',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Пятихатка, Красноперекопский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Равнополье',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Радостное, Белогорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Раздольное, Раздольненский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Раздольное, Советский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Разливы, Нижнегорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Рассадное, Бахчисарайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Растущее, Бахчисарайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Резервное, Севастопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Репино, Бахчисарайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Речное, Бахчисарайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Речное, Советский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Рисовое',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Ровенка, Советский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Ровное, Красногвардейский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Рогово, Красногвардейский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Родники, Белогорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Родники, Нижнегорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Родниковое',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Родниковое село, г. Севастополь',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Родниковское',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Родное',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Родное, Джанкойский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Розовое',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Розовое, Бахчисарайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Ромашкино, Сакский городской округ',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Россошанка, Балаклавский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Рощино',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Рубиновка, Джанкойский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Русаковка',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Русское, Белогорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Ручьи',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Рыбачье',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Рюмшино, Джанкойский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Садовое',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Саки',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Салгирка, Красногвардейский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Самохвалово, Бахчисарайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Санаторное',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Сары-Баш',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Сахарная Головка, Севастопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Светлое',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Севастьяновка, Бахчисарайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Северная Сторона, Севастопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Северное, Белогорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Семидворье',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Семисотка, Ленинский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Сенное, Белогорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Сенокосное',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Серноводское, Джанкойский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Серово, Нижнегорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Сизовка',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Симеиз, Ялтинский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Симферополь',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Синапное, Бахчисарайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Синекаменка, Белогорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Синицыно, Кировский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Сирень, Бахчисарайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Скалистое',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Скворцово',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Славное',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Славянка, Джанкойский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Славянское, Джанкойский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Славянское, Раздольненский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Сливянка, Нижнегорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Смежное, Джанкойский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Советский',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Советское, Джанкойский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Совхозное',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Совхозное, Красноперекопский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Совхозное, Симферопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Соколиное',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Соколы',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Соленое Озеро, Джанкойский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Солнечная Долина, ГО Судак',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Солнечногорское',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Солнечноселье, Бахчисарайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Солнечный, Севастопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Солонцовое, Джанкойский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Софиевка',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Спокойное, Симферопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Стальное, Бахчисарайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Стальное, Джанкойский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Старый Крым',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Стахановка',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Степановка, Нижнегорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Степное, гор. округ Феодосия',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Степное, Первомайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Стерегущее',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Стефановка',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Столбовое, Джанкойский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Столбовое, Сакский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Сторожевое, Симферопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Стрепетово, Нижнегорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Строгановка, Симферопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Суворово, Бахчисарайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Суворово, гор. округ Армянск',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Суворовское, Сакский городской округ',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Судак',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Сумское, Симферопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Сусанино',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Сухоречье, Симферопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Счастливое, Бахчисарайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Табачное',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Табачное, Джанкойский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Таврическое',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Танковое, Бахчисарайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Тарасовка, Джанкойский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Тарасовка, Нижнегорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Тенистое',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Тепловка',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Терновка, Балаклавский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Терновка, Севастопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Тёплое, Симферопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Тимирязево, Джанкойский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Тимофеевка, Джанкойский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Токарево, Кировский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Томашевка, Джанкойский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Тополевка, Белогорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Тополи',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Топольное, Симферопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Трудовое, Сакский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Трудовое, Симферопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Трудолюбовка, Бахчисарайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Трудолюбово, Симферопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Тургеневка',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Тургенево, Белогорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Тургенево, Джанкойский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Тыловое, Балаклавский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Уваровка',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Уварово',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Угловое',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Удачное',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Узловое гор. округ Феодосия',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Украинка, Белогорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Украинка, Симферопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Укромное',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Ульяновка, Белогорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Ульяновка, Красногвардейский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Урожайное',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Урожайное, Советский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Утёс, гор.окр. Алушта',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Учебное, Белогорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Уютное, Сакский городской округ',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Феодосия',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Ферсманово, Симферопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Фёдоровка, Джанкойский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Фёдоровка, Раздольненский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Филатовка',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Фонтаны, Симферопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Форос',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Фронтовое, Нахимовский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Фруктовое, Нахимовский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Фруктовое, Севастопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Фрунзе',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Фрунзе, Нижнегорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Фрунзе, Первомайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Фурмановка, Бахчисарайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Харитоновка, Симферопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Хлебное, Белогорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Хлебное, Джанкойский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Хлебное, Советский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Ходжа-Сала, Бахчисарайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Холмовка',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Холмовое, Красногвардейский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Холодовка',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Цветково',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Цветочное',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Цветущее,Нижнегорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Целинное',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Чайка',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Чайкино, Джанкойский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Чайковское, Симферопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Чапаевка',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Чапаево, Красногвардейский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Челядиново',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Челядиново, Ленинский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Червоное, Сакский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Черемисовка, Белогорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Черново',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Черноземное',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Черноморское',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Чернополье',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Чернышево',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Чистенькое',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Чистополье, Ленинский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Чкалово',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Шафранное, Симферопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Шевченково, Бахчисарайский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Шелковичное',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Широкое, Нижнегорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Широкое, Севастопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Широкое, Симферопольский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Школьное',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Штормовое, Сакский городской округ',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Шубино',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Щебетовка',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Щербаково, Красногвардейский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Щёлкино',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Южное, Ленинский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Юркино, Ленинский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Яблочное, Белогорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Яковлевка, Белогорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Ялта',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Янтарное',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Яркое Поле, Джанкойский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Яркое Поле, Кировский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Яркое, Джанкойский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Ясное, Джанкойский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Яснополянское',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Ястребки, Нижнегорский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Ястребовка, Красногвардейский р-н',0,0,null,null)
insert into Customers.City (RegionID,CityName,CitySort,DisplayInPopup,PhoneNumber,MobilePhoneNumber) values(@regId,'Ястребцы, Джанкойский р-н',0,0,null,null)

GO--


insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38609,'Абрикосовка, Кировский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(46748,'Абрикосовка, Сакский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38211,'Абрикосово','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(34439,'Аграрное, гор. округ Симферополь','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39365,'Азов, Красногвардейский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(4635,'Азовское, Джанкойский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39214,'Айвазовское, Кировский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39441,'Айвовое, Бахчисарайский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39486,'Айкаван, Симферопольский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(33860,'Акимовка','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39487,'Акрополис, Симферопольский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39281,'Александровка, Белогорский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38079,'Александровка, Красногвардейский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(36042,'Александровка, Симферопольский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39282,'Алексеевка, Белогорский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38212,'Алексеевка, Первомайский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(15501,'Алупка, Ялтинский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(15502,'Алушта','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(44738,'Амурское','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39488,'Ана-Юрт, Симферопольский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(16524,'Андреевка, Севастопольский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39489,'Андрусово, Симферопольский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39283,'Анновка, Белогорский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38335,'Арбузово, Первомайский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(15503,'Армянск','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39442,'Аромат, Бахчисарайский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39443,'Ароматное, Бахчисарайский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38799,'Ароматное, Белогорский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(37523,'Артемовка','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(34442,'Аэрофлотский, гор. округ Симферополь','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38610,'Бабенково','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(16531,'Багерово','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(16518,'Балаклава, Севастопольский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39285,'Баланово, Белогорский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39286,'Барабаново, Белогорский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38041,'Батальное','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(5214,'Бахчисарай','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(43637,'Баштановка, Бахчисарайский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38800,'Белая скала','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(23856,'Белоглинка','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(15504,'Белогорск (Крым)','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(36038,'Белокаменное','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(16079,'Береговое','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38851,'Береговое, Бахчисарайский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38841,'Береговое, гор. округ Ялта','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38204,'Березовка','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(34440,'Битумный, гор. округ Симферополь','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38839,'Ближнее, гор. округ Феодосия','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38815,'Ближнее, Красногвардейский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(15798,'Богатовка, ГО Судак','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(35714,'Богатое','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39444,'Богатое Ущелье, Бахчисарайский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39445,'Богатырь, Бахчисарайский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39490,'Богдановка, Симферопольский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(43638,'Большое Садовое, Бахчисарайский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38295,'Бондаренково','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39422,'Бондаренково, гор. округ Алушта','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(48399,'Бондаренково, Ленинский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38205,'Ботаническое','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38350,'Братское, Красноперекопский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38336,'Братское, Первомайский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(43639,'Брянское, Бахчисарайский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(36310,'Буревестник, Нижнегорский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(48302,'Валентиново','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(34495,'Васильевка, Белогорский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39215,'Васильковое, Кировский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38061,'Вересаево','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38054,'Вересаево, Сакский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(37270,'Верхнесадовое','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39287,'Верхние Орешники, Белогорский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(16468,'Верхняя Кутузовка','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(36516,'Верхоречье','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38055,'Веселовка','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(15791,'Веселое, ГО Судак','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39491,'Весёлое, Симферопольский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39216,'Видное, Кировский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39367,'Видное, Красногвардейский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39446,'Викторовка, Бахчисарайский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(15505,'Вилино','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38858,'Винницкое','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39407,'Виноградное, гор. округ Феодосия','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38043,'Виноградное, Ленинский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38294,'Виноградный','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39423,'Виноградный, гор. округ Алушта','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38056,'Виноградово','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(15748,'Витино, Сакский городской округ','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38801,'Вишенное','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38351,'Вишневка','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(16523,'Вишневое, Севастопольский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39289,'Вишнёвое, Белогорский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39369,'Вишняковка, Красногвардейский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39291,'Владимировка, Белогорский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39371,'Владимирово, Красногвардейский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(33799,'Владиславовка, Кировский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38785,'Владиславовка, Нижнегорский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38200,'Водопойное, Черноморский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(33941,'Воинка','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(16532,'Войково, Ленинский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38214,'Войково, Первомайский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39419,'Волошино, гор. округ Армянск','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38370,'Вольное, Джанкойский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38058,'Воробьёво','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39411,'Ворон гор. округ Судак','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38617,'Восточное','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39412,'Восход, гор. округ Ялта','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38816,'Восход, Красногвардейский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39413,'Высокогорное , гор. округ Ялта','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39447,'Высокое, Бахчисарайский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(15609,'Гаспра, Ялтинский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(31802,'Гвардейское','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38216,'Гвардейское, Первомайский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(16471,'Генеральское','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39226,'Георгиевка, Советский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38060,'Геройское','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38044,'Глазовка','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39448,'Глубокий Яр, Бахчисарайский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38798,'Головановка','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39449,'Голубинка, Бахчисарайский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(16528,'Гончарное, Севастопольский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39450,'Горка, Бахчисарайский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39293,'Горлинка, Белогорский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39414,'Горное, гор. округ Ялта','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38045,'Горностаевка','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39373,'Григорьевка, Красногвардейский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38217,'Гришино','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39421,'Громовка, гор. округ Судак','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38201,'Громово, Черноморский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(15796,'Грушевка, ГО Судак','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(34438,'Грушевое, Симферопольский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(34441,'Грэсовский, гор. округ Симферополь','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(15608,'Гурзуф, Ялтинский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38198,'Далекое','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(16519,'Дальнее, Севастопольский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(35591,'Дачное, Бахчисарайский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(15793,'Дачное, ГО Судак','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39227,'Демьяновка, Советский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(35286,'Денисовка','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(6874,'Джанкой','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39295,'Дивное, Белогорский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39492,'Дивное, Симферопольский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38619,'Дмитровка, Советский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38372,'Днепровка','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(37504,'Добровский','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(35928,'Доброе, Симферопольский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39217,'Добролюбовка, Кировский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38063,'Добрушино','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39297,'Дозорное, Белогорский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38852,'Долинное, Бахчисарайский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39218,'Долинное, Кировский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39298,'Долиновка, Белогорский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(35678,'Донское','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39451,'Дорожное, Бахчисарайский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39374,'Доходное, Красногвардейский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38786,'Дрофино','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39493,'Дружное, Симферопольский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(34433,'Дубки, Симферопольский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(43640,'Дубровка, Бахчисарайский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(48279,'Дубровское','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39228,'Дятловка, Советский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(7081,'Евпатория','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39299,'Еленовка, Белогорский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38064,'Елизаветово','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(37390,'Емельяновка','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38374,'Ермаково','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38853,'Железнодорожное','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(33826,'Желябовка','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38787,'Жемчужина','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38612,'Жемчужина Крыма','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39494,'Живописное, Симферопольский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(36183,'Журавки, Кировский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38860,'Журавлевка, Симферопольский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38375,'Завет-Ленинский','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39452,'Заветное, Бахчисарайский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38046,'Заветное, Ленинский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(36311,'Заветное, Советский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(47774,'Зайцево','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(36061,'Залесное, Бахчисарайский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(34434,'Залесье, Симферопольский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39248,'Заливное, Нижнегорский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(15747,'Заозёрное, гор.окр. Евпатория','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(16479,'Запрудное','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38367,'Заречное, Джанкойский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39376,'Заречное, Красногвардейский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39378,'Заря, Красногвардейский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39380,'Звёздное, Красногвардейский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38353,'Зеленая Нива','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38805,'Зеленогорское','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(16475,'Зеленогорье','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38789,'Зеленое, Нижнегорский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39453,'Зелёное, Бахчисарайский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(24443,'Земляничное','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38376,'Зерновое, Джанкойский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38821,'Зерновое, Красногвардейский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38065,'Зерновое, Сакский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38206,'Зимино','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(46414,'Знаменское','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38613,'Золотое Поле','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39383,'Золотое, Красногвардейский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38790,'Зоркино','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39454,'Зубакино, Бахчисарайский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(32162,'Зуя','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38806,'Зыбины','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38292,'Ивановка, Ленинский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38791,'Ивановка, Нижнегорский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(33866,'Ивановка, Сакский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39385,'Известковое, Красногвардейский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(16466,'Изобильное','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39219,'Изобильное, Кировский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38792,'Изобильное, Нижнегорский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(24560,'Изумрудное','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38614,'Изюмовка','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38352,'Ильинка, Красноперекопский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38620,'Ильичёво, Советский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(16517,'Инкерман, Севастопольский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38355,'Источное, Красноперекопский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38356,'Ишунь','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39455,'Казанки, Бахчисарайский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38817,'Калинино, Красногвардейский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(36309,'Калинино, Первомайский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(37733,'Калиновка, Ленинский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(15755,'Каменоломня, Сакский городской округ','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39495,'Камышинка, Симферопольский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39424,'Камышлы, Севастополь','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38296,'Камышное, Раздольненский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39301,'Карасёвка, Белогорский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38824,'Карповка','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38062,'Карьерное','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38843,'Кацивели','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(16514,'Кача, Севастопольский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38343,'Каштановка, Первомайский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(37274,'Каштановое','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38002,'Каштаны','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(15254,'Керчь','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39303,'Кизиловка, Белогорский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39425,'Кизиловое, Севастополь','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(16480,'Кипарисное','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(48677,'Кирово, Ленинский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(15506,'Кировское','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(43042,'Кировское, Черноморский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39249,'Кирсановка, Нижнегорский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38825,'Клепинино','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38819,'Климово','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39496,'Клиновка, Симферопольский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39220,'Ключевое, Кировский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39497,'Ключевое, Симферопольский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39498,'Ключи, Симферопольский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39250,'Коврово, Нижнегорский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(16082,'Коктебель','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38826,'Колодезное, Красногвардейский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39229,'Коломенское, Советский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(15753,'Колоски, Сакский городской округ','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39426,'Колхозное, Севастополь','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38066,'Кольцово','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(33513,'Кольчугино','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39386,'Коммунары, Красногвардейский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(34443,'Комсомольское, гор. округ Симферополь','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38373,'Кондратьево','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39499,'Константиновка, Симферопольский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(15610,'Кореиз, Ялтинский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38344,'Кормовое','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39230,'Корнеевка, Советский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38794,'Косточковка','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38827,'Котельниково','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39456,'Кочергино, Бахчисарайский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39387,'Красная Долина, Красногвардейский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39457,'Красная Заря, Бахчисарайский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(16104,'Красная Поляна','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38199,'Красная Поляна, Черноморский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39304,'Красная Слобода, Белогорский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38358,'Красноармейское, Красноперекопский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39221,'Красновка, Кировский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(15507,'Красногвардейское (Крым)','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38624,'Красногвардейское, Советский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39306,'Красногорское, Белогорский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(47798,'Краснодарка','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38829,'Краснознаменка','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(37273,'Краснокаменка','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38844,'Краснокаменка, гор. округ Ялта','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(15508,'Красноперекопск','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(46928,'Красносельское','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39308,'Красносёловка, Белогорский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38623,'Краснофлотское','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38202,'Красноярское, Черноморский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39458,'Красный Мак, Бахчисарайский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38823,'Кремневка','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38338,'Крестьяновка, Первомайский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39309,'Кривцово, Белогорский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38807,'Криничное','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(46075,'Кропоткино, Раздольненский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38378,'Крымка','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(23660,'Крымская Роза','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38067,'Крымское','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39500,'Кубанское, Симферопольский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39459,'Кудрино, Бахчисарайский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39415,'Куйбышево,  гор. округ Ялта','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(35112,'Куйбышево, Бахчисарайский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39251,'Кукурузное, Нижнегорский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38208,'Кукушкино','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(42182,'Куликовка, Сакский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38297,'Кумово, Раздольненский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39252,'Кунцево, Нижнегорский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39501,'Куприно, Симферопольский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39388,'Курганное, Красногвардейский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38808,'Курортное, Белогорский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38840,'Курортное, гор. округ Феодосия','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38809,'Курское','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(16481,'Лавровое','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39502,'Лазаревка, Симферопольский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(16482,'Лазурное','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39232,'Лебединка, Советский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(32872,'Левадки','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38339,'Левитановка, Первомайский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(35192,'Лекарственное','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(15509,'Ленино','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38820,'Ленинское, Красногвардейский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38047,'Ленинское, Ленинский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(36602,'Лесновка','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(15794,'Лесное, ГО Судак','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38810,'Лечебное','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39416,'Линейное, гор. округ Ялта','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38793,'Лиственное','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38802,'Литвиненково','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38596,'Лобаново','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(35129,'Лозовое','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38597,'Луганское','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39312,'Луговое, Белогорский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38049,'Луговое, Ленинский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39253,'Луговое, Нижнегорский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39254,'Лужки, Нижнегорский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39235,'Лучевое, Советский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(16470,'Лучистое','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(42529,'Лушино, Сакский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38611,'Льговское','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(16515,'Любимовка, Севастопольский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38349,'Магазинка','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38861,'Мазанка','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39237,'Маковка, Советский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39222,'Маковское, Кировский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38332,'Максимовка, Раздольненский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38862,'Маленькое','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39460,'Малиновка, Бахчисарайский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39315,'Малиновка, Белогорский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39461,'Маловидное, Бахчисарайский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39462,'Малое Садовое, Бахчисарайский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(16473,'Малореченское','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(16478,'Малый маяк','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39238,'Марково, Советский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38598,'Мартыновка','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38599,'Марьино, Джанкойский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38830,'Марьяновка','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38005,'Марьяновка, Красногвардейский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(36052,'Маслово','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38845,'Массандра','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39223,'Матросовка, Кировский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39463,'Машино, Бахчисарайский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(48400,'Маяк, Керчь','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38600,'Медведевка','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(37418,'Медведево','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(31712,'Межводное','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39522,'Межгорное, Симферопольский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39317,'Межгорье, Белогорский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(15790,'Междуречье, ГО Судак','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39256,'Межевое, Нижнегорский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(16516,'Мекензиевы Горы, Севастопольский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39319,'Мелехово, Белогорский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39180,'Мелководное, Джанкойский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39320,'Мельники, Белогорский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38812,'Мельничное, Белогорский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39389,'Менделеево, Красногвардейский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(15799,'Миндальное, ГО Судак','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38601,'Мирновка','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(31607,'Мирное, Симферопольский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(15749,'Мирный, Евпаторийский городской округ','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38831,'Миролюбовка','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39321,'Мироновка, Белогорский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39390,'Мироновка, Красногвардейский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39258,'Митрофановка, Нижнегорский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39181,'Митюрино, Джанкойский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38867,'Михайловка, Нижнегорский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(36527,'Михайловка, Сакский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38813,'Мичуринское','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39464,'Многоречье, Бахчисарайский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(31898,'Молодёжное, Симферопольский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39391,'Молочное, Красногвардейский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(15751,'Молочное, Сакский городской округ','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39523,'Молочное, Симферопольский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(15789,'Морское, ГО Судак','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39465,'Мостовое, Бахчисарайский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39524,'Мраморное, Симферопольский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38803,'Муромское','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38828,'Мускатное','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39189,'Мысовое, Джанкойский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39466,'Нагорное, Бахчисарайский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38618,'Надежда, Советский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38822,'Найденовка','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(33677,'Насыпное','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38057,'Наташино','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(31516,'Научный','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39467,'Научный, Бахчисарайский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39392,'Нахимово, Красногвардейский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39182,'Находка, Джанкойский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39393,'Невское, Красногвардейский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39260,'Нежинское, Нижнегорский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(43641,'Некрасовка, Бахчисарайский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38780,'Некрасовка, Советский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39326,'Некрасово, Белогорский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(37761,'Некрасово, Красногвардейский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(9514,'Нижнегорский','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(16483,'Нижнее запрудное','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39796,'Нижнезаморское','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39183,'Нижние Отрожки, Джанкойский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39468,'Нижняя Голубинка, Бахчисарайский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(16469,'Нижняя Кутузовка','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39184,'Низинное, Джанкойский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(47734,'Низинное, Сакский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38842,'Никита','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(36312,'Николаевка, Симферопольский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(48326,'Николаевка, Советский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38340,'Новая Деревня, Первомайский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(46098,'Новая Жизнь','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39469,'Новенькое, Бахчисарайский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39328,'Новоалександровка, Белогорский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(33608,'Новоандреевка','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39427,'Новобобровское, Балаклавский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(42273,'Нововасильевка, Бахчисарайский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39330,'Новогригорьевка, Белогорский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38795,'Новогригорьевка, Нижнегорский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39394,'Новодолинка, Красногвардейский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39395,'Новоекатериновка, Красногвардейский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(35265,'Новожиловка','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39525,'Новозбурьевка, Симферопольский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(35482,'Новоивановка','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38359,'Новоивановка, Красноперекопский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(37751,'Новокленово','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39188,'Новоконстантиновка, Джанкойский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38604,'Новокрымское','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38050,'Новониколаевка, Ленинский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39526,'Новониколаевка, Симферопольский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39396,'Новоникольское, Красногвардейский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(45331,'Новоозёрное','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38042,'Новоотрадное','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39470,'Новопавловка, Бахчисарайский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38360,'Новопавловка, Красноперекопский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38615,'Новопокровка, Кировский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38832,'Новопокровка, Красногвардейский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(35010,'Новополье','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38863,'Новоселовка, Симферопольский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(36243,'Новоселовское, Раздольненский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(35424,'Новосельское','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39240,'Новосёловка, Советский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38605,'Новостепное','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39471,'Новоульяновка, Бахчисарайский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(36208,'Новофедоровка','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39185,'Новофёдоровка, Джанкойский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38834,'Новоэстония','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39527,'Новый Мир, Симферопольский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39528,'Новый Сад, Симферопольский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(15792,'Новый Свет, ГО Судак','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39332,'Овражки, Белогорский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39428,'Озерное, Севастопольский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(15510,'Октябрьское','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(44830,'Октябрьское, Красногвардейский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(37272,'Октябрьское, Ленинский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38342,'Октябрьское, Первомайский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38781,'Октябрьское, Советский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(37510,'Окуневка, Черноморский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38203,'Оленевка','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38846,'Олива','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38847,'Оползневое','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39333,'Опытное, Белогорский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39186,'Орденоносное, Джанкойский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(16081,'Орджоникидзе','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(35422,'Орехово','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(16525,'Орлиное, Севастопольский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(37437,'Орловка, Красногвардейский район','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(37425,'Орловка, Раздольненский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39429,'Орловка, Севастопольский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38354,'Орловское','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(16530,'Осипенко, Севастопольский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(48398,'Осовины, Ленинский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38051,'Останино','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39187,'Островское, Джанкойский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38345,'Островское, Первомайский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(37714,'Отважное, Кировский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(43642,'Отрадное, Бахчисарайский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38848,'Отрадное, гор. округ Ялта','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39190,'Отрадное, Джанкойский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38059,'Охотниково','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39417,'Охотничье, гор. округ Ялта','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38796,'Охотское','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39191,'Павловка, Джанкойский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39430,'Павловка, Севастопольский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(15612,'Парковое, Ялтинский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(16465,'Партенит','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39418,'Партизанское, гор. округ Ялта','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38864,'Партизанское, Симферопольский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38616,'Партизаны','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39335,'Пасечное, Белогорский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39073,'Пахаревка, Джанкойский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(37814,'Первомайское, Кировский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(15511,'Первомайское, Первомайский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38865,'Первомайское, Симферопольский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(15795,'Переваловка, ГО Судак','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(31858,'Перевальное','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38849,'Передовое, Балаклавский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39529,'Передовое, Симферопольский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38838,'Перекоп','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39192,'Перепёлкино, Джанкойский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(24509,'Перово','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(37475,'Песчаное','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(31342,'Петровка','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39530,'Петровка, Симферопольский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38814,'Петрово, Белогорский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39531,'Петропавловка, Симферопольский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(31001,'Пионерское','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39409,'Пионерское, гор. округ Феодосия','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39431,'Пироговка, Нахимовский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(37271,'Плодовое','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(35545,'Плотинное','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39397,'Победино, Красногвардейский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(37278,'Победное, Джанкойский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39337,'Поворотное, Белогорский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39432,'Поворотное, Нахимовский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39433,'Подгорное ','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(44231,'Пожарское, Симферопольский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39398,'Пологи, Красногвардейский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38835,'Полтавка','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39434,'Полюшко, Нахимовский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39472,'Поляна, Бахчисарайский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(15716,'Понизовка пгт, Ялтинский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(44063,'Поповка, Сакский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(34256,'Почтовое','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38213,'Правда','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39193,'Предмостное, Джанкойский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(43643,'Предущельное, Бахчисарайский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38194,'Прибрежное, Сакский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(16474,'Приветное, гор. округ Алушта','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(36803,'Приветное, Кировский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(36804,'Приветное, Сакский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(45718,'Привольное, Красноперекопский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39533,'Привольное, Симферопольский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(16080,'Приморский','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(36685,'Приозерное','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(48401,'Приозёрное, Ленинский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39243,'Присивашное, Советский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39473,'Приятное Свидание, Бахчисарайский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39194,'Пробуждение, Джанкойский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39408,'Прозрачное, Джанкойский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39534,'Пролётное, Симферопольский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39339,'Пролом, Белогорский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38368,'Просторное','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39399,'Проточное, Красногвардейский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(33263,'Прохладное','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39535,'Прудовое, Симферопольский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39224,'Пруды, Кировский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38782,'Пруды, Советский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39400,'Прямое, Красногвардейский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39474,'Путиловка, Бахчисарайский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(42925,'Пушкино, Алуштинский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(16484,'Пушкино, Красногвардейский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38779,'Пушкино, Советский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39341,'Пчелиное, Белогорский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39244,'Пчельники, Советский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(37276,'Пшеничное, Нижнегорский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(34274,'Пшеничное, Первомайский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(35441,'Пятихатка','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38361,'Пятихатка, Красноперекопский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38866,'Равнополье','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39342,'Радостное, Белогорский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(15512,'Раздольное, Раздольненский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38622,'Раздольное, Советский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39262,'Разливы, Нижнегорский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39475,'Рассадное, Бахчисарайский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39476,'Растущее, Бахчисарайский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39435,'Резервное, Севастопольский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39477,'Репино, Бахчисарайский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39478,'Речное, Бахчисарайский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39245,'Речное, Советский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38362,'Рисовое','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39246,'Ровенка, Советский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38833,'Ровное, Красногвардейский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39401,'Рогово, Красногвардейский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39344,'Родники, Белогорский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39264,'Родники, Нижнегорский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(35134,'Родниковое','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38868,'Родниковое село, г. Севастополь','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39436,'Родниковское','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(36928,'Родное','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39195,'Родное, Джанкойский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(16467,'Розовое','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39479,'Розовое, Бахчисарайский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(15752,'Ромашкино, Сакский городской округ','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39437,'Россошанка, Балаклавский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38377,'Рощино','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39196,'Рубиновка, Джанкойский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38811,'Русаковка','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39347,'Русское, Белогорский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38207,'Ручьи','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(16476,'Рыбачье','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39197,'Рюмшино, Джанкойский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(24331,'Садовое','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(15513,'Саки','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39402,'Салгирка, Красногвардейский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(43644,'Самохвалово, Бахчисарайский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(36047,'Санаторное','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(37277,'Сары-Баш','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(16521,'Сахарная Головка, Севастопольский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38606,'Светлое','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(43645,'Севастьяновка, Бахчисарайский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(16522,'Северная Сторона, Севастопольский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39348,'Северное, Белогорский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(16472,'Семидворье','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38048,'Семисотка, Ленинский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39349,'Сенное, Белогорский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38210,'Сенокосное','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39198,'Серноводское, Джанкойский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39265,'Серово, Нижнегорский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(37379,'Сизовка','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(15611,'Симеиз, Ялтинский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(15345,'Симферополь','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39480,'Синапное, Бахчисарайский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39351,'Синекаменка, Белогорский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(36173,'Синицыно, Кировский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39481,'Сирень, Бахчисарайский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(35995,'Скалистое','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(35986,'Скворцово','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38209,'Славное','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39199,'Славянка, Джанкойский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39200,'Славянское, Джанкойский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(37724,'Славянское, Раздольненский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39268,'Сливянка, Нижнегорский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39201,'Смежное, Джанкойский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(11274,'Советский','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39202,'Советское, Джанкойский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(32632,'Совхозное','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38363,'Совхозное, Красноперекопский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39536,'Совхозное, Симферопольский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38854,'Соколиное','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(47775,'Соколы','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(37706,'Соленое Озеро, Джанкойский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(15797,'Солнечная Долина, ГО Судак','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(16477,'Солнечногорское','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39482,'Солнечноселье, Бахчисарайский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(16529,'Солнечный, Севастопольский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39203,'Солонцовое, Джанкойский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(42412,'Софиевка','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39537,'Спокойное, Симферопольский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39483,'Стальное, Бахчисарайский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38603,'Стальное, Джанкойский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(24385,'Старый Крым','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38218,'Стахановка','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39270,'Степановка, Нижнегорский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39410,'Степное, гор. округ Феодосия','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38348,'Степное, Первомайский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(36055,'Стерегущее','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38365,'Стефановка','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39204,'Столбовое, Джанкойский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38195,'Столбовое, Сакский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39538,'Сторожевое, Симферопольский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39271,'Стрепетово, Нижнегорский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(34435,'Строгановка, Симферопольский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(43646,'Суворово, Бахчисарайский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38837,'Суворово, гор. округ Армянск','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(15754,'Суворовское, Сакский городской округ','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(11432,'Судак','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39539,'Сумское, Симферопольский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38346,'Сусанино','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39540,'Сухоречье, Симферопольский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39484,'Счастливое, Бахчисарайский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(36911,'Табачное','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38607,'Табачное, Джанкойский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38364,'Таврическое','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38855,'Танковое, Бахчисарайский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39205,'Тарасовка, Джанкойский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39273,'Тарасовка, Нижнегорский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38850,'Тенистое','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(35541,'Тепловка','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(48274,'Терновка, Балаклавский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(16526,'Терновка, Севастопольский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39541,'Тёплое, Симферопольский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39206,'Тимирязево, Джанкойский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39207,'Тимофеевка, Джанкойский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(32015,'Токарево, Кировский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39208,'Томашевка, Джанкойский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39352,'Тополевка, Белогорский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(36918,'Тополи','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39542,'Топольное, Симферопольский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(37444,'Трудовое, Сакский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(37431,'Трудовое, Симферопольский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(43647,'Трудолюбовка, Бахчисарайский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39543,'Трудолюбово, Симферопольский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38856,'Тургеневка','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39353,'Тургенево, Белогорский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39209,'Тургенево, Джанкойский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39438,'Тыловое, Балаклавский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38797,'Уваровка','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38052,'Уварово','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(35011,'Угловое','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38836,'Удачное','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39420,'Узловое гор. округ Феодосия','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39355,'Украинка, Белогорский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(34436,'Украинка, Симферопольский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(37275,'Укромное','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39356,'Ульяновка, Белогорский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39403,'Ульяновка, Красногвардейский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(32140,'Урожайное','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38783,'Урожайное, Советский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(16486,'Утёс, гор.окр. Алушта','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39358,'Учебное, Белогорский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(15750,'Уютное, Сакский городской округ','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(11968,'Феодосия','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39544,'Ферсманово, Симферопольский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39210,'Фёдоровка, Джанкойский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38334,'Фёдоровка, Раздольненский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38357,'Филатовка','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(34437,'Фонтаны, Симферопольский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(34105,'Форос','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39439,'Фронтовое, Нахимовский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39440,'Фруктовое, Нахимовский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(16520,'Фруктовое, Севастопольский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(31121,'Фрунзе','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(47512,'Фрунзе, Нижнегорский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38215,'Фрунзе, Первомайский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(43648,'Фурмановка, Бахчисарайский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39545,'Харитоновка, Симферопольский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39359,'Хлебное, Белогорский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39211,'Хлебное, Джанкойский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39247,'Хлебное, Советский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39485,'Ходжа-Сала, Бахчисарайский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38857,'Холмовка','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39404,'Холмовое, Красногвардейский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(47800,'Холодовка','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(44011,'Цветково','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(35539,'Цветочное','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39275,'Цветущее,Нижнегорский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38371,'Целинное','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(16485,'Чайка','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38602,'Чайкино, Джанкойский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39546,'Чайковское, Симферопольский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38621,'Чапаевка','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39405,'Чапаево, Красногвардейский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38053,'Челядиново','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(48402,'Челядиново, Ленинский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38196,'Червоное, Сакский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39361,'Черемисовка, Белогорский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38347,'Черново','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38784,'Черноземное','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(15514,'Черноморское','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38804,'Чернополье','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(42216,'Чернышево','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(35640,'Чистенькое','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(35982,'Чистополье, Ленинский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38788,'Чкалово','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39547,'Шафранное, Симферопольский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(43649,'Шевченково, Бахчисарайский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38197,'Шелковичное','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39277,'Широкое, Нижнегорский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(16527,'Широкое, Севастопольский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38859,'Широкое, Симферопольский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(34301,'Школьное','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(15756,'Штормовое, Сакский городской округ','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(47799,'Шубино','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(31530,'Щебетовка','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(33071,'Щербаково, Красногвардейский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(12793,'Щёлкино','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38293,'Южное, Ленинский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(48397,'Юркино, Ленинский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39362,'Яблочное, Белогорский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39364,'Яковлевка, Белогорский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(12963,'Ялта','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38818,'Янтарное','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38366,'Яркое Поле, Джанкойский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(42217,'Яркое Поле, Кировский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38608,'Яркое, Джанкойский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39212,'Ясное, Джанкойский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(38369,'Яснополянское','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39278,'Ястребки, Нижнегорский р-н','Крым респ.','0.00')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39406,'Ястребовка, Красногвардейский р-н','Крым респ.','no limit')
insert into shipping.SdekCities (Id,CityName,OblName,NalSumLimit) Values(39213,'Ястребцы, Джанкойский р-н','Крым респ.','0.00')


GO--

Update Settings.Settings set Value = 'yandexmap' where Name = 'PrintOrder_MapType'

GO--

alter table Catalog.Product add YandexName nvarchar(255) null

GO--

ALTER PROCEDURE [Settings].[sp_GetExportFeedProducts] @exportFeedId int
	,@onlyCount BIT
	,@exportNotAvailable bit
	,@selectedCurrency NVARCHAR(10)
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
						OR Product.AllowPreOrder = 1
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
				OR Product.AllowPreOrder = 1
				OR @exportNotAvailable = 1
				)
			AND CategoryEnabled = 1
			AND (Product.Enabled = 1 OR @exportNotAvailable = 1)
	END
END

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
	 @YandexSizeUnit nvarchar(10),
	 @YandexName nvarchar(255)
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
			 ,YandexName
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
			 @YandexSizeUnit,
			 @YandexName
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
	 @YandexSizeUnit nvarchar(10),
	 @YandexName nvarchar(255)
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
	 ,[YandexName] = @YandexName
	WHERE ProductID = @ProductID      
END 

GO--

insert into settings.Localization (LanguageId,ResourceKey,ResourceValue) values(1,'Admin.ExportFeeed.SettingsYandex.TypeExportYandex','Упрощенный тип экспорта')
insert into settings.Localization (LanguageId,ResourceKey,ResourceValue) values(2,'Admin.ExportFeeed.SettingsYandex.TypeExportYandex','Simplified type of export')

insert into settings.Localization (LanguageId,ResourceKey,ResourceValue) values(1,'Core.ExportImport.ProductFields.YandexName','Яндекс-маркет название товара')
insert into settings.Localization (LanguageId,ResourceKey,ResourceValue) values(2,'Core.ExportImport.ProductFields.YandexName','Yandex-market product name')

GO--

UPDATE [Settings].[InternalSettings] SET [settingValue] = '6.0.6' WHERE [settingKey] = 'db_version'
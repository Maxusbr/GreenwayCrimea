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
   insert into Catalog.[Property] (Name,UseInFilter,UseInBrief,Useindetails,SortOrder,[type]) values (@nameProperty,1,0,1,0,1)      
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
DROP INDEX IX_Error404_Url_UrlReferer ON Settings.Error404
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
UPDATE [Settings].[InternalSettings] SET [settingValue] = '5.0.11' WHERE [settingKey] = 'db_version'


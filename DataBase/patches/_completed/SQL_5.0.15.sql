
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

Alter Table Settings.Error404
Add  IpAddress nvarchar(100) NULL

GO--

Alter Table Settings.Error404
Add  UserAgent nvarchar(max) NULL

GO--

ALTER PROCEDURE [Customers].[sp_UpdateCustomerInfo]  
   @CustomerID uniqueidentifier,  
   @FirstName nvarchar(70),  
   @LastName nvarchar(70),
   @Patronymic	nvarchar(70),
   @Phone nvarchar(max),     
   @StandardPhone bigint,     
   @Email nvarchar(100),  
   @CustomerGroupId int = NULL,  
   @CustomerRole int, 
   @BonusCardNumber bigint,
   @AdminComment nvarchar(MAX),
   @ManagerId int,
   @Rating int
AS  
BEGIN  
 UPDATE [Customers].[Customer]  
    SET [FirstName] = @FirstName,
		[LastName] = @LastName,
		[Patronymic] = @Patronymic,
		[Phone] = @Phone,		
		[StandardPhone] = @StandardPhone,
		[Email] = @Email,
		[CustomerGroupId] = @CustomerGroupId,
		[CustomerRole] = @CustomerRole,
		[BonusCardNumber] = @BonusCardNumber,
		[AdminComment] = @AdminComment,
		[ManagerId] = @ManagerId,
		[Rating] = @Rating
   WHERE CustomerID = @CustomerID
END 

GO--


ALTER PROCEDURE [Catalog].[SetProductHierarchicallyEnabled]  
  @ProductId int  
AS  
BEGIN  
Declare @CountEnabled int  
Set @CountEnabled = (Select Count(CategoryID) 
                       From [Catalog].[Category]   
					  Where [Category].HirecalEnabled=1 and CategoryID in (Select CategoryID From [Catalog].[ProductCategories] Where ProductId=@ProductId and Main=1))  
					  
if (@CountEnabled > 0 )  
 Update [Catalog].[Product] set CategoryEnabled = 1 where ProductId=@ProductId  
else  
 Update [Catalog].[Product] set CategoryEnabled = 0 where ProductId=@ProductId  
END  


GO--

Update settings.settings set value='False' where name='AvaliableFilterEnabled'

GO--
if not  exists(select * from settings.settings where name = 'SearchMaxItems')
insert into settings.settings (name, value) values ('SearchMaxItems','100')
GO--

Insert Into [CMS].[StaticBlock] ([Key],[InnerName],[Content],[Added],[Modified],[Enabled]) 
Values ('TopHeader', 'Верхняя часть в шапке сайта', '', getdate(), getdate(), 1)
GO--



UPDATE [Settings].[InternalSettings] SET [settingValue] = '5.0.15' WHERE [settingKey] = 'db_version'


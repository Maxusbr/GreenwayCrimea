Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Home.MainPageProducts.AllProducts', 'Все товары...')
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Home.MainPageProducts.AllProducts', 'All products...')
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Js.Inplace.PropertyHasBeenDelete', 'Свойство было удалено')
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Js.Inplace.PropertyHasBeenDelete', 'Property was deleted')

Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Js.Cart.PriceWithDiscount', 'Цена со скидкой')
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Js.Cart.PriceWithDiscount', 'Price with discount')


GO--
 ALTER TABLE Customers.Country ADD DialCode int NULL

GO-- 
 
 update [Customers].[Country] set DialCode=93  where Lower(CountryISO2) = 'af'
 update [Customers].[Country] set DialCode=355  where Lower(CountryISO2) = 'al'
 update [Customers].[Country] set DialCode=213  where Lower(CountryISO2) = 'dz'
 update [Customers].[Country] set DialCode=1684  where Lower(CountryISO2) = 'as'
 update [Customers].[Country] set DialCode=376  where Lower(CountryISO2) = 'ad'
 update [Customers].[Country] set DialCode=244  where Lower(CountryISO2) = 'ao'
 update [Customers].[Country] set DialCode=1264  where Lower(CountryISO2) = 'ai'
 update [Customers].[Country] set DialCode=1268  where Lower(CountryISO2) = 'ag'
 update [Customers].[Country] set DialCode=54  where Lower(CountryISO2) = 'ar'
 update [Customers].[Country] set DialCode=374  where Lower(CountryISO2) = 'am'
 update [Customers].[Country] set DialCode=297  where Lower(CountryISO2) = 'aw'
 update [Customers].[Country] set DialCode=61  where Lower(CountryISO2) = 'au'
 update [Customers].[Country] set DialCode=43  where Lower(CountryISO2) = 'at'
 update [Customers].[Country] set DialCode=994  where Lower(CountryISO2) = 'az'
 update [Customers].[Country] set DialCode=1242  where Lower(CountryISO2) = 'bs'
 update [Customers].[Country] set DialCode=973  where Lower(CountryISO2) = 'bh'
 update [Customers].[Country] set DialCode=880  where Lower(CountryISO2) = 'bd'
 update [Customers].[Country] set DialCode=1246  where Lower(CountryISO2) = 'bb'
 update [Customers].[Country] set DialCode=375  where Lower(CountryISO2) = 'by'
 update [Customers].[Country] set DialCode=32  where Lower(CountryISO2) = 'be'
 update [Customers].[Country] set DialCode=501  where Lower(CountryISO2) = 'bz'
 update [Customers].[Country] set DialCode=229  where Lower(CountryISO2) = 'bj'
 update [Customers].[Country] set DialCode=1441  where Lower(CountryISO2) = 'bm'
 update [Customers].[Country] set DialCode=975  where Lower(CountryISO2) = 'bt'
 update [Customers].[Country] set DialCode=591  where Lower(CountryISO2) = 'bo'
 update [Customers].[Country] set DialCode=387  where Lower(CountryISO2) = 'ba'
 update [Customers].[Country] set DialCode=267  where Lower(CountryISO2) = 'bw'
 update [Customers].[Country] set DialCode=55  where Lower(CountryISO2) = 'br'
 update [Customers].[Country] set DialCode=246  where Lower(CountryISO2) = 'io'
 update [Customers].[Country] set DialCode=1284  where Lower(CountryISO2) = 'vg'
 update [Customers].[Country] set DialCode=673  where Lower(CountryISO2) = 'bn'
 update [Customers].[Country] set DialCode=359  where Lower(CountryISO2) = 'bg'
 update [Customers].[Country] set DialCode=226  where Lower(CountryISO2) = 'bf'
 update [Customers].[Country] set DialCode=257  where Lower(CountryISO2) = 'bi'
 update [Customers].[Country] set DialCode=855  where Lower(CountryISO2) = 'kh'
 update [Customers].[Country] set DialCode=237  where Lower(CountryISO2) = 'cm'
 update [Customers].[Country] set DialCode=1  where Lower(CountryISO2) = 'ca'
 update [Customers].[Country] set DialCode=238  where Lower(CountryISO2) = 'cv'
 update [Customers].[Country] set DialCode=599  where Lower(CountryISO2) = 'bq'
 update [Customers].[Country] set DialCode=1345  where Lower(CountryISO2) = 'ky'
 update [Customers].[Country] set DialCode=236  where Lower(CountryISO2) = 'cf'
 update [Customers].[Country] set DialCode=235  where Lower(CountryISO2) = 'td'
 update [Customers].[Country] set DialCode=56  where Lower(CountryISO2) = 'cl'
 update [Customers].[Country] set DialCode=86  where Lower(CountryISO2) = 'cn'
 update [Customers].[Country] set DialCode=57  where Lower(CountryISO2) = 'co'
 update [Customers].[Country] set DialCode=269  where Lower(CountryISO2) = 'km'
 update [Customers].[Country] set DialCode=243  where Lower(CountryISO2) = 'cd'
 update [Customers].[Country] set DialCode=242  where Lower(CountryISO2) = 'cg'
 update [Customers].[Country] set DialCode=682  where Lower(CountryISO2) = 'ck'
 update [Customers].[Country] set DialCode=506  where Lower(CountryISO2) = 'cr'
 update [Customers].[Country] set DialCode=225  where Lower(CountryISO2) = 'ci'
 update [Customers].[Country] set DialCode=385  where Lower(CountryISO2) = 'hr'
 update [Customers].[Country] set DialCode=53  where Lower(CountryISO2) = 'cu'
 update [Customers].[Country] set DialCode=599  where Lower(CountryISO2) = 'cw'
 update [Customers].[Country] set DialCode=357  where Lower(CountryISO2) = 'cy'
 update [Customers].[Country] set DialCode=420  where Lower(CountryISO2) = 'cz'
 update [Customers].[Country] set DialCode=45  where Lower(CountryISO2) = 'dk'
 update [Customers].[Country] set DialCode=253  where Lower(CountryISO2) = 'dj'
 update [Customers].[Country] set DialCode=1767  where Lower(CountryISO2) = 'dm'
 update [Customers].[Country] set DialCode=1  where Lower(CountryISO2) = 'do'
 update [Customers].[Country] set DialCode=593  where Lower(CountryISO2) = 'ec'
 update [Customers].[Country] set DialCode=20  where Lower(CountryISO2) = 'eg'
 update [Customers].[Country] set DialCode=503  where Lower(CountryISO2) = 'sv'
 update [Customers].[Country] set DialCode=240  where Lower(CountryISO2) = 'gq'
 update [Customers].[Country] set DialCode=291  where Lower(CountryISO2) = 'er'
 update [Customers].[Country] set DialCode=372  where Lower(CountryISO2) = 'ee'
 update [Customers].[Country] set DialCode=251  where Lower(CountryISO2) = 'et'
 update [Customers].[Country] set DialCode=500  where Lower(CountryISO2) = 'fk'
 update [Customers].[Country] set DialCode=298  where Lower(CountryISO2) = 'fo'
 update [Customers].[Country] set DialCode=679  where Lower(CountryISO2) = 'fj'
 update [Customers].[Country] set DialCode=358  where Lower(CountryISO2) = 'fi'
 update [Customers].[Country] set DialCode=33  where Lower(CountryISO2) = 'fr'
 update [Customers].[Country] set DialCode=594  where Lower(CountryISO2) = 'gf'
 update [Customers].[Country] set DialCode=689  where Lower(CountryISO2) = 'pf'
 update [Customers].[Country] set DialCode=241  where Lower(CountryISO2) = 'ga'
 update [Customers].[Country] set DialCode=220  where Lower(CountryISO2) = 'gm'
 update [Customers].[Country] set DialCode=995  where Lower(CountryISO2) = 'ge'
 update [Customers].[Country] set DialCode=49  where Lower(CountryISO2) = 'de'
 update [Customers].[Country] set DialCode=233  where Lower(CountryISO2) = 'gh'
 update [Customers].[Country] set DialCode=350  where Lower(CountryISO2) = 'gi'
 update [Customers].[Country] set DialCode=30  where Lower(CountryISO2) = 'gr'
 update [Customers].[Country] set DialCode=299  where Lower(CountryISO2) = 'gl'
 update [Customers].[Country] set DialCode=1473  where Lower(CountryISO2) = 'gd'
 update [Customers].[Country] set DialCode=590  where Lower(CountryISO2) = 'gp'
 update [Customers].[Country] set DialCode=1671  where Lower(CountryISO2) = 'gu'
 update [Customers].[Country] set DialCode=502  where Lower(CountryISO2) = 'gt'
 update [Customers].[Country] set DialCode=224  where Lower(CountryISO2) = 'gn'
 update [Customers].[Country] set DialCode=245  where Lower(CountryISO2) = 'gw'
 update [Customers].[Country] set DialCode=592  where Lower(CountryISO2) = 'gy'
 update [Customers].[Country] set DialCode=509  where Lower(CountryISO2) = 'ht'
 update [Customers].[Country] set DialCode=504  where Lower(CountryISO2) = 'hn'
 update [Customers].[Country] set DialCode=852  where Lower(CountryISO2) = 'hk'
 update [Customers].[Country] set DialCode=36  where Lower(CountryISO2) = 'hu'
 update [Customers].[Country] set DialCode=354  where Lower(CountryISO2) = 'is'
 update [Customers].[Country] set DialCode=91  where Lower(CountryISO2) = 'in'
 update [Customers].[Country] set DialCode=62  where Lower(CountryISO2) = 'id'
 update [Customers].[Country] set DialCode=98  where Lower(CountryISO2) = 'ir'
 update [Customers].[Country] set DialCode=964  where Lower(CountryISO2) = 'iq'
 update [Customers].[Country] set DialCode=353  where Lower(CountryISO2) = 'ie'
 update [Customers].[Country] set DialCode=972  where Lower(CountryISO2) = 'il'
 update [Customers].[Country] set DialCode=39  where Lower(CountryISO2) = 'it'
 update [Customers].[Country] set DialCode=1876  where Lower(CountryISO2) = 'jm'
 update [Customers].[Country] set DialCode=81  where Lower(CountryISO2) = 'jp'
 update [Customers].[Country] set DialCode=962  where Lower(CountryISO2) = 'jo'
 update [Customers].[Country] set DialCode=7  where Lower(CountryISO2) = 'kz'
 update [Customers].[Country] set DialCode=254  where Lower(CountryISO2) = 'ke'
 update [Customers].[Country] set DialCode=686  where Lower(CountryISO2) = 'ki'
 update [Customers].[Country] set DialCode=965  where Lower(CountryISO2) = 'kw'
 update [Customers].[Country] set DialCode=996  where Lower(CountryISO2) = 'kg'
 update [Customers].[Country] set DialCode=856  where Lower(CountryISO2) = 'la'
 update [Customers].[Country] set DialCode=371  where Lower(CountryISO2) = 'lv'
 update [Customers].[Country] set DialCode=961  where Lower(CountryISO2) = 'lb'
 update [Customers].[Country] set DialCode=266  where Lower(CountryISO2) = 'ls'
 update [Customers].[Country] set DialCode=231  where Lower(CountryISO2) = 'lr'
 update [Customers].[Country] set DialCode=218  where Lower(CountryISO2) = 'ly'
 update [Customers].[Country] set DialCode=423  where Lower(CountryISO2) = 'li'
 update [Customers].[Country] set DialCode=370  where Lower(CountryISO2) = 'lt'
 update [Customers].[Country] set DialCode=352  where Lower(CountryISO2) = 'lu'
 update [Customers].[Country] set DialCode=853  where Lower(CountryISO2) = 'mo'
 update [Customers].[Country] set DialCode=389  where Lower(CountryISO2) = 'mk'
 update [Customers].[Country] set DialCode=261  where Lower(CountryISO2) = 'mg'
 update [Customers].[Country] set DialCode=265  where Lower(CountryISO2) = 'mw'
 update [Customers].[Country] set DialCode=60  where Lower(CountryISO2) = 'my'
 update [Customers].[Country] set DialCode=960  where Lower(CountryISO2) = 'mv'
 update [Customers].[Country] set DialCode=223  where Lower(CountryISO2) = 'ml'
 update [Customers].[Country] set DialCode=356  where Lower(CountryISO2) = 'mt'
 update [Customers].[Country] set DialCode=692  where Lower(CountryISO2) = 'mh'
 update [Customers].[Country] set DialCode=596  where Lower(CountryISO2) = 'mq'
 update [Customers].[Country] set DialCode=222  where Lower(CountryISO2) = 'mr'
 update [Customers].[Country] set DialCode=230  where Lower(CountryISO2) = 'mu'
 update [Customers].[Country] set DialCode=52  where Lower(CountryISO2) = 'mx'
 update [Customers].[Country] set DialCode=691  where Lower(CountryISO2) = 'fm'
 update [Customers].[Country] set DialCode=373  where Lower(CountryISO2) = 'md'
 update [Customers].[Country] set DialCode=377  where Lower(CountryISO2) = 'mc'
 update [Customers].[Country] set DialCode=976  where Lower(CountryISO2) = 'mn'
 update [Customers].[Country] set DialCode=382  where Lower(CountryISO2) = 'me'
 update [Customers].[Country] set DialCode=1664  where Lower(CountryISO2) = 'ms'
 update [Customers].[Country] set DialCode=212  where Lower(CountryISO2) = 'ma'
 update [Customers].[Country] set DialCode=258  where Lower(CountryISO2) = 'mz'
 update [Customers].[Country] set DialCode=95  where Lower(CountryISO2) = 'mm'
 update [Customers].[Country] set DialCode=264  where Lower(CountryISO2) = 'na'
 update [Customers].[Country] set DialCode=674  where Lower(CountryISO2) = 'nr'
 update [Customers].[Country] set DialCode=977  where Lower(CountryISO2) = 'np'
 update [Customers].[Country] set DialCode=31  where Lower(CountryISO2) = 'nl'
 update [Customers].[Country] set DialCode=687  where Lower(CountryISO2) = 'nc'
 update [Customers].[Country] set DialCode=64  where Lower(CountryISO2) = 'nz'
 update [Customers].[Country] set DialCode=505  where Lower(CountryISO2) = 'ni'
 update [Customers].[Country] set DialCode=227  where Lower(CountryISO2) = 'ne'
 update [Customers].[Country] set DialCode=234  where Lower(CountryISO2) = 'ng'
 update [Customers].[Country] set DialCode=683  where Lower(CountryISO2) = 'nu'
 update [Customers].[Country] set DialCode=672  where Lower(CountryISO2) = 'nf'
 update [Customers].[Country] set DialCode=850  where Lower(CountryISO2) = 'kp'
 update [Customers].[Country] set DialCode=1670  where Lower(CountryISO2) = 'mp'
 update [Customers].[Country] set DialCode=47  where Lower(CountryISO2) = 'no'
 update [Customers].[Country] set DialCode=968  where Lower(CountryISO2) = 'om'
 update [Customers].[Country] set DialCode=92  where Lower(CountryISO2) = 'pk'
 update [Customers].[Country] set DialCode=680  where Lower(CountryISO2) = 'pw'
 update [Customers].[Country] set DialCode=970  where Lower(CountryISO2) = 'ps'
 update [Customers].[Country] set DialCode=507  where Lower(CountryISO2) = 'pa'
 update [Customers].[Country] set DialCode=675  where Lower(CountryISO2) = 'pg'
 update [Customers].[Country] set DialCode=595  where Lower(CountryISO2) = 'py'
 update [Customers].[Country] set DialCode=51  where Lower(CountryISO2) = 'pe'
 update [Customers].[Country] set DialCode=63  where Lower(CountryISO2) = 'ph'
 update [Customers].[Country] set DialCode=48  where Lower(CountryISO2) = 'pl'
 update [Customers].[Country] set DialCode=351  where Lower(CountryISO2) = 'pt'
 update [Customers].[Country] set DialCode=1  where Lower(CountryISO2) = 'pr'
 update [Customers].[Country] set DialCode=974  where Lower(CountryISO2) = 'qa'
 update [Customers].[Country] set DialCode=262  where Lower(CountryISO2) = 're'
 update [Customers].[Country] set DialCode=40  where Lower(CountryISO2) = 'ro'
 update [Customers].[Country] set DialCode=7  where Lower(CountryISO2) = 'ru'
 update [Customers].[Country] set DialCode=250  where Lower(CountryISO2) = 'rw'
 update [Customers].[Country] set DialCode=590  where Lower(CountryISO2) = 'bl'
 update [Customers].[Country] set DialCode=290  where Lower(CountryISO2) = 'sh'
 update [Customers].[Country] set DialCode=1869  where Lower(CountryISO2) = 'kn'
 update [Customers].[Country] set DialCode=1758  where Lower(CountryISO2) = 'lc'
 update [Customers].[Country] set DialCode=590  where Lower(CountryISO2) = 'mf'
 update [Customers].[Country] set DialCode=508  where Lower(CountryISO2) = 'pm'
 update [Customers].[Country] set DialCode=1784  where Lower(CountryISO2) = 'vc'
 update [Customers].[Country] set DialCode=685  where Lower(CountryISO2) = 'ws'
 update [Customers].[Country] set DialCode=378  where Lower(CountryISO2) = 'sm'
 update [Customers].[Country] set DialCode=239  where Lower(CountryISO2) = 'st'
 update [Customers].[Country] set DialCode=966  where Lower(CountryISO2) = 'sa'
 update [Customers].[Country] set DialCode=221  where Lower(CountryISO2) = 'sn'
 update [Customers].[Country] set DialCode=381  where Lower(CountryISO2) = 'rs'
 update [Customers].[Country] set DialCode=248  where Lower(CountryISO2) = 'sc'
 update [Customers].[Country] set DialCode=232  where Lower(CountryISO2) = 'sl'
 update [Customers].[Country] set DialCode=65  where Lower(CountryISO2) = 'sg'
 update [Customers].[Country] set DialCode=1721  where Lower(CountryISO2) = 'sx'
 update [Customers].[Country] set DialCode=421  where Lower(CountryISO2) = 'sk'
 update [Customers].[Country] set DialCode=386  where Lower(CountryISO2) = 'si'
 update [Customers].[Country] set DialCode=677  where Lower(CountryISO2) = 'sb'
 update [Customers].[Country] set DialCode=252  where Lower(CountryISO2) = 'so'
 update [Customers].[Country] set DialCode=27  where Lower(CountryISO2) = 'za'
 update [Customers].[Country] set DialCode=82  where Lower(CountryISO2) = 'kr'
 update [Customers].[Country] set DialCode=211  where Lower(CountryISO2) = 'ss'
 update [Customers].[Country] set DialCode=34  where Lower(CountryISO2) = 'es'
 update [Customers].[Country] set DialCode=94  where Lower(CountryISO2) = 'lk'
 update [Customers].[Country] set DialCode=249  where Lower(CountryISO2) = 'sd'
 update [Customers].[Country] set DialCode=597  where Lower(CountryISO2) = 'sr'
 update [Customers].[Country] set DialCode=268  where Lower(CountryISO2) = 'sz'
 update [Customers].[Country] set DialCode=46  where Lower(CountryISO2) = 'se'
 update [Customers].[Country] set DialCode=41  where Lower(CountryISO2) = 'ch'
 update [Customers].[Country] set DialCode=963  where Lower(CountryISO2) = 'sy'
 update [Customers].[Country] set DialCode=886  where Lower(CountryISO2) = 'tw'
 update [Customers].[Country] set DialCode=992  where Lower(CountryISO2) = 'tj'
 update [Customers].[Country] set DialCode=255  where Lower(CountryISO2) = 'tz'
 update [Customers].[Country] set DialCode=66  where Lower(CountryISO2) = 'th'
 update [Customers].[Country] set DialCode=670  where Lower(CountryISO2) = 'tl'
 update [Customers].[Country] set DialCode=228  where Lower(CountryISO2) = 'tg'
 update [Customers].[Country] set DialCode=690  where Lower(CountryISO2) = 'tk'
 update [Customers].[Country] set DialCode=676  where Lower(CountryISO2) = 'to'
 update [Customers].[Country] set DialCode=1868  where Lower(CountryISO2) = 'tt'
 update [Customers].[Country] set DialCode=216  where Lower(CountryISO2) = 'tn'
 update [Customers].[Country] set DialCode=90  where Lower(CountryISO2) = 'tr'
 update [Customers].[Country] set DialCode=993  where Lower(CountryISO2) = 'tm'
 update [Customers].[Country] set DialCode=1649  where Lower(CountryISO2) = 'tc'
 update [Customers].[Country] set DialCode=688  where Lower(CountryISO2) = 'tv'
 update [Customers].[Country] set DialCode=1340  where Lower(CountryISO2) = 'vi'
 update [Customers].[Country] set DialCode=256  where Lower(CountryISO2) = 'ug'
 update [Customers].[Country] set DialCode=380  where Lower(CountryISO2) = 'ua'
 update [Customers].[Country] set DialCode=971  where Lower(CountryISO2) = 'ae'
 update [Customers].[Country] set DialCode=44  where Lower(CountryISO2) = 'gb'
 update [Customers].[Country] set DialCode=1  where Lower(CountryISO2) = 'us'
 update [Customers].[Country] set DialCode=598  where Lower(CountryISO2) = 'uy'
 update [Customers].[Country] set DialCode=998  where Lower(CountryISO2) = 'uz'
 update [Customers].[Country] set DialCode=678  where Lower(CountryISO2) = 'vu'
 update [Customers].[Country] set DialCode=39  where Lower(CountryISO2) = 'va'
 update [Customers].[Country] set DialCode=58  where Lower(CountryISO2) = 've'
 update [Customers].[Country] set DialCode=84  where Lower(CountryISO2) = 'vn'
 update [Customers].[Country] set DialCode=681  where Lower(CountryISO2) = 'wf'
 update [Customers].[Country] set DialCode=967  where Lower(CountryISO2) = 'ye'
 update [Customers].[Country] set DialCode=260  where Lower(CountryISO2) = 'zm'
 update [Customers].[Country] set DialCode=263  where Lower(CountryISO2) = 'zw'
GO--

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
   AND ((Select ISNULL(Max(Amount), 0) From [Catalog].[Offer] Where [Offer].[ProductId] = [Product].[ProductID]) > 0 OR @exportNotAmount = 1)   
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
   AND ((Select ISNULL(Max(Amount), 0) From [Catalog].[Offer] Where [Offer].[ProductId] = [Product].[ProductID]) > 0 OR @exportNotAmount = 1)  
 END    
END 

GO--

if not exists(select Id From [Shipping].[SdekCities] where CityName = 'Армения')
Begin
Insert  INTO [Shipping].[SdekCities] Values (15, 'Армения', '', 0.00)
end
if not exists(select Id From [Shipping].[SdekCities] where CityName = 'Ереван')
Begin
Insert  INTO [Shipping].[SdekCities] Values (7114, 'Ереван', '', 0.00)
end
if not exists(select Id From [Shipping].[SdekCities] where CityName = 'Абовян')
Begin
Insert  INTO [Shipping].[SdekCities] Values (31140, 'Абовян', 'Армения', 0.00)
end
if not exists(select Id From [Shipping].[SdekCities] where CityName = 'Апаран')
Begin
Insert  INTO [Shipping].[SdekCities] Values (4877, 'Апаран', 'Армения', 0.00)
end
if not exists(select Id From [Shipping].[SdekCities] where CityName = 'Армавир')
Begin
Insert  INTO [Shipping].[SdekCities] Values (31223, 'Армавир', 'Армения', 0.00)
end
if not exists(select Id From [Shipping].[SdekCities] where CityName = 'Аштарак')
Begin
Insert  INTO [Shipping].[SdekCities] Values (31139, 'Аштарак', 'Армения', 0.00)
end
if not exists(select Id From [Shipping].[SdekCities] where CityName = 'Ванадзор')
Begin
Insert  INTO [Shipping].[SdekCities] Values (5839, 'Ванадзор', 'Армения', 0.00)
end
if not exists(select Id From [Shipping].[SdekCities] where CityName = 'Горис')
Begin
Insert  INTO [Shipping].[SdekCities] Values (31385, 'Горис', 'Армения', 0.00)
end
if not exists(select Id From [Shipping].[SdekCities] where CityName = 'Гюмри')
Begin
Insert  INTO [Shipping].[SdekCities] Values (6730, 'Гюмри', 'Армения', 0.00)
end
if not exists(select Id From [Shipping].[SdekCities] where CityName = 'Егвард')
Begin
Insert  INTO [Shipping].[SdekCities] Values (31224, 'Егвард', 'Армения', 0.00)
end
if not exists(select Id From [Shipping].[SdekCities] where CityName = 'Ехегнадзор')
Begin
Insert  INTO [Shipping].[SdekCities] Values (31381, 'Ехегнадзор', 'Армения', 0.00)
end
if not exists(select Id From [Shipping].[SdekCities] where CityName = 'Капан')
Begin
Insert  INTO [Shipping].[SdekCities] Values (31384, 'Капан', 'Армения', 0.00)
end
if not exists(select Id From [Shipping].[SdekCities] where CityName = 'Масис')
Begin
Insert  INTO [Shipping].[SdekCities] Values (31225, 'Масис', 'Армения', 0.00)
end
if not exists(select Id From [Shipping].[SdekCities] where CityName = 'Раздан')
Begin
Insert  INTO [Shipping].[SdekCities] Values (10466, 'Раздан', 'Армения', 0.00)
end
if not exists(select Id From [Shipping].[SdekCities] where CityName = 'Севан')
Begin
Insert  INTO [Shipping].[SdekCities] Values (11080, 'Севан', 'Армения', 0.00)
end
if not exists(select Id From [Shipping].[SdekCities] where CityName = 'Спитак')
Begin
Insert  INTO [Shipping].[SdekCities] Values (31383, 'Спитак', 'Армения', 0.00)
end
if not exists(select Id From [Shipping].[SdekCities] where CityName = 'Цахкадзор')
Begin
Insert  INTO [Shipping].[SdekCities] Values (31382, 'Цахкадзор', 'Армения', 0.00)
end
if not exists(select Id From [Shipping].[SdekCities] where CityName = 'Эчмиадзин')
Begin
Insert  INTO [Shipping].[SdekCities] Values (31141, 'Эчмиадзин', 'Армения', 0.00)
end

GO--

if not exists(select CountryID From [Customers].[Country] where CountryName = 'Армения')
	Begin
		Insert  INTO [Customers].[Country] (CountryName,CountryISO2,CountryISO3) Values ('Армения', 'AM', 'ARM')
	end

if not exists(select CountryID From [Customers].[Country_ru] where CountryName = 'Армения')
	Begin
		Insert  INTO [Customers].[Country_ru] (CountryName,CountryISO2,CountryISO3) Values ('Армения', 'AM', 'ARM')
	end

if not exists(select CountryID From [Customers].[Country_en] where CountryName = 'Armeniya')
	Begin
		Insert  INTO [Customers].[Country_en] (CountryName,CountryISO2,CountryISO3) Values ('Armeniya', 'AM', 'ARM')
	end
 
declare @CountryID int
declare @RegionId int
set @CountryID = (select CountryID from [Customers].[Country] where CountryName = 'Армения')
  
if not exists(select RegionID From [Customers].[Region] where RegionName = 'Арагацотнская область')
	Begin
		if @CountryID is null 
			begin
				Insert  INTO [Customers].[Region] (RegionName) Values ('Арагацотнская область')
			end
		else
			begin
				Insert  INTO [Customers].[Region] (CountryID, RegionName) Values (@CountryID, 'Арагацотнская область')
			end
	end 

set @RegionId = (select RegionID from [Customers].[Region] where RegionName = 'Арагацотнская область')

if not exists(select CityID From [Customers].[City] where CityName = 'Апаран')
	Begin
		if @RegionId is null 
			Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Апаран',  0, 0)
		else
			Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Апаран', 0, 0)
	end

if not exists(select CityID From [Customers].[City] where CityName = 'Аштарак')
	Begin
		if @RegionId is null 
			Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Аштарак', 0, 0)
		else
			Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Аштарак', 0, 0)
	end
	
if not exists(select RegionID From [Customers].[Region] where RegionName = 'Араратская область')
	Begin
		if @CountryID is null 
			Insert  INTO [Customers].[Region] (RegionName) Values ('Араратская область')
		else
			Insert  INTO [Customers].[Region] (CountryID, RegionName) Values (@CountryID, 'Араратская область')
	end 

set @RegionId = (select RegionID from [Customers].[Region] where RegionName = 'Араратская область')

if not exists(select CityID From [Customers].[City] where CityName = 'Масис')
	Begin
		if @RegionId is null 
			Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Масис',  0, 0)
		else
			Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Масис', 0, 0)
	end
	
if not exists(select RegionID From [Customers].[Region] where RegionName = 'Армавирская область')
	Begin
		if @CountryID is null 
			Insert  INTO [Customers].[Region] (RegionName) Values ('Армавирская область')
		else
			Insert  INTO [Customers].[Region] (CountryID, RegionName) Values (@CountryID, 'Армавирская область')
	end	

set @RegionId = (select RegionID from [Customers].[Region] where RegionName = 'Армавирская область')

if not exists(select CityID From [Customers].[City] where CityName = 'Армавир')
	Begin
		if @RegionId is null 
			Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Армавир',  0, 0)
		else
			Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Армавир', 0, 0)
	end

if not exists(select CityID From [Customers].[City] where CityName = 'Эчмиадзин')
	Begin
		if @RegionId is null 
			Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Эчмиадзин',  0, 0)
		else
			Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Эчмиадзин', 0, 0)
	end
	
if not exists(select RegionID From [Customers].[Region] where RegionName = 'Вайоцдзорская область')
	Begin
		if @CountryID is null 
			Insert  INTO [Customers].[Region] (RegionName) Values ('Вайоцдзорская область')
		else
			Insert  INTO [Customers].[Region] (CountryID, RegionName) Values (@CountryID, 'Вайоцдзорская область')
	end	

set @RegionId = (select RegionID from [Customers].[Region] where RegionName = 'Вайоцдзорская область')

if not exists(select CityID From [Customers].[City] where CityName = 'Ехегнадзор')
	Begin
		if @RegionId is null 
			Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Ехегнадзор',  0, 0)
		else
			Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Ехегнадзор', 0, 0)
	end

if not exists(select RegionID From [Customers].[Region] where RegionName = 'Ереван')
	Begin
		if @CountryID is null 
			Insert  INTO [Customers].[Region] (RegionName) Values ('Ереван')
		else
			Insert  INTO [Customers].[Region] (CountryID, RegionName) Values (@CountryID, 'Ереван')
	end 

set @RegionId = (select RegionID from [Customers].[Region] where RegionName = 'Ереван')

if not exists(select CityID From [Customers].[City] where CityName = 'Ереван')
	Begin
		if @RegionId is null 
			Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Ереван',  0, 0)
		else
			Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Ереван', 0, 0)
	end

if not exists(select RegionID From [Customers].[Region] where RegionName = 'Котайкская область')
	Begin
		if @CountryID is null
			Insert  INTO [Customers].[Region] (RegionName) Values ('Котайкская область')
		else
			Insert  INTO [Customers].[Region] (CountryID, RegionName) Values (@CountryID, 'Котайкская область')
	end 

set @RegionId = (select RegionID from [Customers].[Region] where RegionName = 'Котайкская область')

if not exists(select CityID From [Customers].[City] where CityName = 'Абовян')
	Begin
		if @RegionId is null 
			Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Абовян',  0, 0)
		else
			Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Абовян', 0, 0)
	end

if not exists(select CityID From [Customers].[City] where CityName = 'Егвард')
	Begin
		if @RegionId is null 
			Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Егвард',  0, 0)
		else
			Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Егвард', 0, 0)
	end

if not exists(select CityID From [Customers].[City] where CityName = 'Раздан')
	Begin
		if @RegionId is null 
			Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Раздан',  0, 0)
		else
			Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Раздан', 0, 0)
	end

if not exists(select CityID From [Customers].[City] where CityName = 'Цахкадзор')
	Begin
		if @RegionId is null 
			Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Цахкадзор',  0, 0)
		else
			Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Цахкадзор', 0, 0)
	end
	
if not exists(select RegionID From [Customers].[Region] where RegionName = 'Лорийская область')
	Begin
		if @CountryID is null
			Insert  INTO [Customers].[Region] (RegionName) Values ('Лорийская область')
		else
			Insert  INTO [Customers].[Region] (CountryID, RegionName) Values (@CountryID, 'Лорийская область')
	end 

set @RegionId = (select RegionID from [Customers].[Region] where RegionName = 'Лорийская область')

if not exists(select CityID From [Customers].[City] where CityName = 'Ванадзор')
	Begin
		if @RegionId is null 
			Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Ванадзор',  0, 0)
		else
			Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Ванадзор', 0, 0)
	end 
	
if not exists(select CityID From [Customers].[City] where CityName = 'Спитак')
	Begin
		if @RegionId is null 
			Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Спитак',  0, 0)
		else
			Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Спитак', 0, 0)
	end 
	
if not exists(select RegionID From [Customers].[Region] where RegionName = 'Севанская область')
	Begin
		if @CountryID is null
			Insert  INTO [Customers].[Region] (RegionName) Values ('Севанская область')
		else
			Insert  INTO [Customers].[Region] (CountryID, RegionName) Values (@CountryID, 'Севанская область')
	end 

set @RegionId = (select RegionID from [Customers].[Region] where RegionName = 'Севанская область')

if not exists(select CityID From [Customers].[City] where CityName = 'Севан')
	Begin
		if @RegionId is null 
			Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Севан',  0, 0)
		else
			Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Севан', 0, 0)
	end 
	
if not exists(select RegionID From [Customers].[Region] where RegionName = 'Сюникская область')
	Begin
		if @CountryID is null
			Insert  INTO [Customers].[Region] (RegionName) Values ('Сюникская область')
		else
			Insert  INTO [Customers].[Region] (CountryID, RegionName) Values (@CountryID, 'Сюникская область')
	end	

set @RegionId = (select RegionID from [Customers].[Region] where RegionName = 'Сюникская область')

if not exists(select CityID From [Customers].[City] where CityName = 'Горис')
	Begin
		if @RegionId is null 
			Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Горис',  0, 0)
		else
			Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Горис', 0, 0)
	end 

if not exists(select CityID From [Customers].[City] where CityName = 'Капан')
	Begin
		if @RegionId is null 
			Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Капан',  0, 0)
		else
			Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Капан', 0, 0)
	end 
	
if not exists(select RegionID From [Customers].[Region] where RegionName = 'Ширакская область')
	Begin
		if @CountryID is null
			Insert  INTO [Customers].[Region] (RegionName) Values ('Ширакская область')
		else
			Insert  INTO [Customers].[Region] (CountryID, RegionName) Values (@CountryID, 'Ширакская область')
	end	

set @RegionId = (select RegionID from [Customers].[Region] where RegionName = 'Ширакская область')

if not exists(select CityID From [Customers].[City] where CityName = 'Гюмри')
	Begin
		if @RegionId is null 
			Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Гюмри',  0, 0)
		else
			Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Гюмри', 0, 0)
	end

GO--


ALTER PROCEDURE [Catalog].[sp_GetPropertyInFilter] (@categoryid INT, @usedepth bit)
AS
BEGIN
  IF (@usedepth = 1)
  BEGIN
      ;WITH products (propertyvalueid)
           AS
           (
			   SELECT DISTINCT propertyvalueid
			   FROM            [catalog].[productpropertyvalue]
			   WHERE productid IN
				   (
					  SELECT     [productcategories].productid
					  FROM       [catalog].[productcategories]
					  INNER JOIN [catalog].product
					  ON         [productcategories].productid = [product].productid AND [product].[enabled] = 1 
					  where [categoryid] IN (SELECT id FROM [settings].[getchildcategorybyparent] ( @categoryid ))
				   )
           )
		   
		SELECT     [propertyvalue].[propertyvalueid],
				   [propertyvalue].[propertyid],
				   [propertyvalue].[value],
				   [propertyvalue].[rangevalue],
				   [property].name                 AS propertyname,
				   [property].sortorder            AS propertysortorder,
				   [property].expanded             AS propertyexpanded,
				   [property].unit                 AS propertyunit,
				   [property].TYPE                 AS propertytype
				   
		FROM       [catalog].[propertyvalue] 	
		INNER JOIN [catalog].[property]	ON [property].propertyid = [propertyvalue].propertyid AND [property].[useinfilter] = 1				   
		WHERE      [propertyvalue].[useinfilter] = 1
				   AND [propertyvalue].propertyvalueid IN (SELECT propertyvalueid FROM products)
		ORDER BY   propertysortorder,
				   [propertyvalue].[propertyid],
				   [propertyvalue].[sortorder],
				   [propertyvalue].[rangevalue],
				   [propertyvalue].[value]
  END 
  ELSE 
  BEGIN 
  
	;WITH products (propertyvalueid)
		 AS
		 (
			 SELECT DISTINCT propertyvalueid
			 FROM            [catalog].[productpropertyvalue]
			 WHERE productid IN
				 (
					SELECT     [productcategories].productid
					FROM       [catalog].[productcategories]
					INNER JOIN [catalog].product
					ON         [productcategories].productid = [product].productid AND [product].[enabled] = 1 
					where [categoryid] IN (@categoryid)
				 )
		 )
		 
	SELECT     [propertyvalue].[propertyvalueid],
			   [propertyvalue].[propertyid],
			   [propertyvalue].[value],
			   [propertyvalue].[rangevalue],
			   [property].name                 AS propertyname,
			   [property].sortorder            AS propertysortorder,
			   [property].expanded             AS propertyexpanded,
			   [property].unit                 AS propertyunit,
			   [property].TYPE                 AS propertytype
			   
	FROM       [catalog].[propertyvalue]
	INNER JOIN [catalog].[property]	ON [property].propertyid = [propertyvalue].propertyid AND [property].[useinfilter] = 1
	WHERE      [propertyvalue].[useinfilter] = 1 AND 
			   [propertyvalue].propertyvalueid IN (SELECT propertyvalueid FROM products)
	ORDER BY   propertysortorder,
			   [propertyvalue].[propertyid],
			   [propertyvalue].[sortorder],
			   [propertyvalue].[rangevalue],
			   [propertyvalue].[value]			   
  END
END

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

-- Clear dublicates in Localization table
DECLARE @tempLocalization table(  
    [LanguageId] [int] NOT NULL,
	[ResourceKey] [nvarchar](100) NOT NULL,
	[ResourceValue] [nvarchar](max) NOT NULL);

INSERT INTO @tempLocalization ( [LanguageId], [ResourceKey], [ResourceValue])
	select [LanguageId], [ResourceKey], [ResourceValue]
	from (
	   select *, row_number() over (partition by [LanguageId],[ResourceKey] order by [ResourceKey]) as row_number
	   from [Settings].[Localization]
	   ) as rows
	where row_number = 1
	Order by [LanguageId]

Delete from [Settings].[Localization]

INSERT INTO [Settings].[Localization] ( [LanguageId], [ResourceKey], [ResourceValue])
	Select * from @tempLocalization

GO--


ALTER TABLE Settings.Localization ADD CONSTRAINT
	PK_Localization PRIMARY KEY CLUSTERED 
	( LanguageId, ResourceKey ) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO--

Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Js.Phone.PhonePlaceholder', '+7(999)999-99-99')
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Js.Phone.PhonePlaceholder', '+7(999)999-99-99')

GO--

UPDATE [Settings].[InternalSettings] SET [settingValue] = '5.0.19' WHERE [settingKey] = 'db_version'
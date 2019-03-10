
CREATE PROCEDURE [Settings].[sp_GetAllCsvProducts] 
  @onlyCount BIT 
AS  
BEGIN  
 IF @onlyCount = 1  
 BEGIN  
  SELECT COUNT(ProductID)  
  FROM [Catalog].[Product]    
 END  
 ELSE  
 BEGIN  
  SELECT *  
  FROM [Catalog].[Product]  
  LEFT JOIN [Catalog].[Photo] ON [Photo].[ObjId] = [Product].[ProductID]  
   AND Type = 'Product'  
   AND Photo.[Main] = 1 
 END  
END
GO--


UPDATE [Settings].[InternalSettings] SET [settingValue] = '5.0.6' WHERE [settingKey] = 'db_version'
GO--


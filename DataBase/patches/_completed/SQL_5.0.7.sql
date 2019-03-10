
Insert Into [Settings].[Settings] (Name, Value) Values ('ShowProductArtNo', 'False')
GO--

GO--
ALTER PROCEDURE [Catalog].[sp_RecalculateProductsCount]	
@UseAmount bit
AS
BEGIN
SET NOCOUNT ON 
;WITH cteSort AS
      (
      SELECT [Category].CategoryID AS Child,
			 [Category].ParentCategory AS Parent,
			 1  AS [Level]
        FROM [Catalog].[Category] WHERE CategoryID = 0 
      union ALL
      SELECT 
		 [Category].CategoryID AS Child,
		 [Category].ParentCategory AS Parent,
		 cteSort.[Level] + 1 AS [Level]
      FROM [Catalog].[Category] 
		   INNER JOIN cteSort ON [Category].ParentCategory = cteSort.Child and [Category].CategoryID<>0)

update c set 
   c.Products_Count=isnull(g.Products_Count,0)*c.Enabled, 
   c.Total_Products_Count=isnull(g.Total_Products_Count,0),
   c.CatLevel =cteSort.[Level]
from [Catalog].Category c
left join (
   select 
      pc.CategoryID, 
      SUM(1*p.Enabled)Products_Count,
      COUNT(*)Total_Products_Count
   from [Catalog].ProductCategories pc 
   inner join [Catalog].Product p on p.ProductID=pc.ProductID
   inner join [Catalog].[ProductExt] pExt on p.ProductID=pExt.ProductID
   where ((@UseAmount =1 and pExt.AmountSort=1) or (@UseAmount=0))
   group by pc.CategoryID
   )g on g.CategoryID=c.CategoryID
left join cteSort on cteSort.Child = c.[CategoryID]

declare @max int
set @max = (select top(1) CatLevel from [Catalog].[Category] order by CatLevel  Desc)
 while (@max >0)
 begin
     UPDATE t1
		SET t1.Products_Count = t1.Products_Count + t2.cnt ,
		t1.[Total_Products_Count] = t1.[Total_Products_Count] + t2.cnt2
		from [Catalog].[Category] as t1 
		cross apply (Select COALESCE(SUM(Products_Count),0) cnt,COALESCE(SUM([Total_Products_Count]),0) cnt2 from [Catalog].[Category] where ParentCategory =t1.CategoryID) t2
		where t1.CategoryID in (Select CategoryID from [Catalog].[Category] where CatLevel =@max)
     Set @max = @max -1
 end
END
GO--

if(select count(*) from settings.settings where name='NewsMainPageText') = 0
begin
	insert into settings.settings (name, value) values('NewsMainPageText', 'Подписка на новости')
end
else
begin
	if(select top 1 value from settings.settings where name='NewsMainPageText') = ''
		update settings.settings set value='Подписка на новости' where name='NewsMainPageText'
end

GO--

DECLARE @customOptionId int

DECLARE customOptions CURSOR FOR SELECT customOptionsId from Catalog.CustomOptions as co Where InputType = '2' and (Select Count(OptionID) From Catalog.Options Where co.CustomOptionsID = CustomOptionsID ) = 0
OPEN customOptions
FETCH customOptions INTO @customOptionId

WHILE @@FETCH_STATUS = 0
BEGIN

insert into  [Catalog].[Options] (CustomOptionsID, Title, PriceBC, PriceType, SortOrder) values (@customOptionId, '',0,0,0)

FETCH NEXT FROM customOptions INTO @customOptionId
END

CLOSE customOptions
DEALLOCATE customOptions

GO--

if(SELECT Count(*) FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_NAME='PK_OrderPaymentInfo') = 1 
	ALTER TABLE [Order].OrderPaymentInfo DROP CONSTRAINT PK_OrderPaymentInfo

GO--

Insert into cms.staticblock ([Key], InnerName, [Content], Added, Modified, Enabled) Values ('MobileOrderSuccess', 'успешное завершение заказа в мобильной версии','',GETDATE(),GETDATE(),1)
GO--

Update [Settings].[MailFormatType] Set Comment = Replace(Comment, '#AUTHOR#;', '#AUTHOR#; #USERMAIL#;') Where MailFormatTypeID = 11
GO--

Update [Settings].[MailFormat] Set [FormatText] = Replace([FormatText], '#AUTHOR#', '#AUTHOR# (#USERMAIL#)') Where FormatType = 11
GO--
UPDATE [Settings].[InternalSettings] SET [settingValue] = '5.0.7' WHERE [settingKey] = 'db_version'
GO--


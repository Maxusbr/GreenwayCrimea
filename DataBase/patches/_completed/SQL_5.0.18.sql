

CREATE TABLE [Order].[OrderHistory](
	[OrderId] [int] NOT NULL,
	[Parameter] [nvarchar](255) NOT NULL,
	[ParameterType] [int] NOT NULL,
	[ParameterValue] [nvarchar](50) NULL,
	[ParameterDescription] [nvarchar](max) NULL,
	[OldValue] [nvarchar](max) NOT NULL,
	[NewValue] [nvarchar](max) NOT NULL,
	[ManagerName] [nvarchar](255) NOT NULL,
	[ManagerId] [uniqueidentifier] NULL,
	[ModificationTime] [datetime] NOT NULL
) ON [PRIMARY]

GO--

Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Orders.Order.PaymentName', 'Метод оплаты') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Orders.Order.ShippingName', 'Метод доставки') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Orders.Order.AffiliateID', 'Партнер') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Orders.Order.ManagerId', 'Менеджер') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Orders.Order.OrderDiscount', 'Скидка') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Orders.Order.OrderDate', 'Дата создания заказа') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Orders.Order.PaymentDate', 'Дата оплаты') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Orders.Order.CustomerComment', 'Комментарий пользователя') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Orders.Order.StatusComment', 'Комментарий к статусу заказа') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Orders.Order.AdminOrderComment', 'Комментарий администратора') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Orders.Order.ShippingCost', 'Стоимость доставки') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Orders.Order.PaymentCost', 'Стоимость оплаты') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Orders.Order.BonusCardNumber', 'Бонусная карта') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Orders.Order.GroupName', 'Группа пользователя') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Orders.Order.GroupDiscount', 'Скидка группы пользователей') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Orders.Order.Certificate', 'Сертификат') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Orders.Order.Coupon', 'Купон') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Orders.Order.UseIn1C', 'Использовать в 1С') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Orders.Order.ManagerConfirmed', 'Подтвержден менеджером') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Orders.Order.OrderSourceId', 'Источник заказа') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Orders.Order.CustomData', 'Данные') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Orders.OrderContact.Name', 'Имя контакта') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Orders.OrderContact.Country', 'Страна') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Orders.OrderContact.Zone', 'Регион') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Orders.OrderContact.City', 'Город') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Orders.OrderContact.Zip', 'Индекс') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Orders.OrderContact.Address', 'Адрес') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Orders.OrderContact.CustomField1', 'Поле 1') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Orders.OrderContact.CustomField2', 'Поле 2') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Orders.OrderContact.CustomField3', 'Поле 3') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Orders.OrderCurrency.CurrencyCode', 'Код валюты') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Orders.OrderCurrency.CurrencyValue', 'Значение валюты') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Payment.PaymentDetails.CompanyName', 'Название компании') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Payment.PaymentDetails.INN', 'Инн') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Payment.PaymentDetails.Phone', 'Телефон') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Orders.OrderCustomer.CustomerIP', 'Ip пользователя') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Orders.OrderCustomer.FirstName', 'Имя') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Orders.OrderCustomer.LastName', 'Фамилия') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Orders.OrderCustomer.Patronymic', 'Отчество') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Orders.OrderCustomer.Email', 'Email') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Orders.OrderCustomer.Phone', 'Телефон') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Orders.OrderCustomer.StandardPhone', 'Телефон (форматированный)') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Orders.OnRefreshTotalOrder.Sum', 'Сумма заказа') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Orders.OnRefreshTotalOrder.TaxCost', 'Налог') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Orders.OnRefreshTotalOrder.BonusCost', 'Бонусы') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Orders.OnRefreshTotalOrder.DiscountCost', 'Скидка') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Orders.OrderItem.ArtNo', 'Артикул товара') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Orders.OrderItem.Name', 'Название товара') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Orders.OrderItem.Price', 'Цена товара') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Orders.OrderItem.Amount', 'Кол-во товара') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Orders.OrderItem.Color', 'Цвет товара') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Orders.OrderItem.Size', 'Размер товара') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Orders.OrderItem.Weight', 'Вес товара') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Orders.OrderItem.IsCouponApplied', 'Применен купон') 

GO--

Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.Orders.Order.PaymentName', 'Payment method') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.Orders.Order.ShippingName', 'Shipping method') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.Orders.Order.AffiliateID', 'Affiliate') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.Orders.Order.ManagerId', 'Manager') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.Orders.Order.OrderDiscount', 'Discount') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.Orders.Order.OrderDate', 'Order date') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.Orders.Order.PaymentDate', 'Payment date') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.Orders.Order.CustomerComment', 'Customer comment') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.Orders.Order.StatusComment', 'Status comment') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.Orders.Order.AdminOrderComment', 'Admin comment') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.Orders.Order.ShippingCost', 'Shipping cost') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.Orders.Order.PaymentCost', 'Payment cost') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.Orders.Order.BonusCardNumber', 'Bonus card number') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.Orders.Order.GroupName', 'Group name') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.Orders.Order.GroupDiscount', 'Group discount') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.Orders.Order.Certificate', 'Certificate') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.Orders.Order.Coupon', 'Coupon') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.Orders.Order.UseIn1C', 'Use in 1C') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.Orders.Order.ManagerConfirmed', 'Confirmed by manager') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.Orders.Order.OrderSourceId', 'Order source') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.Orders.Order.CustomData', 'Custom data') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.Orders.OrderContact.Name', 'Contact name') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.Orders.OrderContact.Country', 'Country') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.Orders.OrderContact.Zone', 'Zone') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.Orders.OrderContact.City', 'City') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.Orders.OrderContact.Zip', 'Zip') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.Orders.OrderContact.Address', 'Address') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.Orders.OrderContact.CustomField1', 'CustomField1') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.Orders.OrderContact.CustomField2', 'CustomField2') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.Orders.OrderContact.CustomField3', 'CustomField3') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.Orders.OrderCurrency.CurrencyCode', 'Currency code') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.Orders.OrderCurrency.CurrencyValue', 'Currency value') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.Payment.PaymentDetails.CompanyName', 'Company name') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.Payment.PaymentDetails.INN', 'Inn') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.Payment.PaymentDetails.Phone', 'Phone') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.Orders.OrderCustomer.CustomerIP', 'Customer IP') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.Orders.OrderCustomer.FirstName', 'First name') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.Orders.OrderCustomer.LastName', 'Last name') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.Orders.OrderCustomer.Patronymic', 'Patronymic') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.Orders.OrderCustomer.Email', 'Email') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.Orders.OrderCustomer.Phone', 'Phone') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.Orders.OrderCustomer.StandardPhone', 'Standard Phone') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.Orders.OnRefreshTotalOrder.Sum', 'Sum') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.Orders.OnRefreshTotalOrder.TaxCost', 'Tax cost') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.Orders.OnRefreshTotalOrder.BonusCost', 'Bonus cost') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.Orders.OnRefreshTotalOrder.DiscountCost', 'Discount cost') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.Orders.OrderItem.ArtNo', 'ArtNo') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.Orders.OrderItem.Name', 'Name') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.Orders.OrderItem.Price', 'Price') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.Orders.OrderItem.Amount', 'Amount') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.Orders.OrderItem.Color', 'Color') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.Orders.OrderItem.Size', 'Size') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.Orders.OrderItem.Weight', 'Weight') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.Orders.OrderItem.IsCouponApplied', 'Is coupon applied') 

GO--

Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Orders.OrderHistory.OrderCreated', 'Создан заказ ') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Orders.OrderHistory.OrderDeleted', 'Удален заказ ') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Orders.OrderHistory.DeletedProduct', 'Удален товар') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Orders.OrderHistory.DeletedCertificate', 'Удален сертификат') 

Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Orders.OrderHistory.AddedProduct', 'Добавлен товар') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Orders.OrderHistory.AddedCertificate', 'Добавлен сертификат') 

Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Orders.Order.ChangingStatus', 'Смена статуса заказа')
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Orders.Order.OrderPaied', 'Заказ оплачен')
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Orders.Order.OrderNotPaied', 'Заказ не оплачен') 


GO--


Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.Orders.OrderHistory.OrderCreated', 'Order created ') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.Orders.OrderHistory.OrderDeleted', 'Order deleted ') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.Orders.OrderHistory.DeletedProduct', 'Product deleted') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.Orders.OrderHistory.DeletedCertificate', 'Certificate deleted') 

Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.Orders.OrderHistory.AddedProduct', 'Product added') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.Orders.OrderHistory.AddedCertificate', 'Certificate added') 

Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.Orders.Order.ChangingStatus', 'Order status changed')
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.Orders.Order.OrderPaied', 'Order is paid')
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.Orders.Order.OrderNotPaied', 'Order is not paid') 

GO--


if not  exists(select * from [Settings].[Localization] where [ResourceKey]= 'Module.BonusSystem.GetBonusCardTitle')
begin

Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Module.BonusSystem.GetBonusCardTitle', 'Бонусная программа')
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Module.BonusSystem.ClientIdText', 'ИД посетителя')
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Module.BonusSystem.GetBonusCardHeader', 'Получить бонусную карту') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Module.BonusSystem.CardGrades', 'Номинал карты')
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Module.BonusSystem.CardGradesFormat', '{0} - сумма заказов до {1} руб - {2}')

Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Module.BonusSystem.GetBonusCardTitle', 'Bonus card')
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Module.BonusSystem.ClientIdText', 'Client Id')
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Module.BonusSystem.GetBonusCardHeader', 'Get bonus card') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Module.BonusSystem.CardGrades', 'Grades')
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Module.BonusSystem.CardGradesFormat', '{0} - order sum {1} - {2}')

end

GO--
ALTER PROCEDURE [Catalog].[sp_GetPropertyValuesByPropertyID] @PropertyID INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [PropertyValueID]
		,[Property].[PropertyID]
		,[Value]
		,[PropertyValue].[SortOrder]
		,[Property].UseinFilter
		,[Property].UseIndetails
		,[Property].UseInBrief
		,Property.NAME AS PropertyName
		,Property.SortOrder AS PropertySortOrder
		,Property.Expanded
		,Property.[Type]
		,Property.[Description]
		,GroupId
		,GroupName
		,GroupSortorder
	FROM [Catalog].[PropertyValue]
	INNER JOIN [Catalog].[Property] ON [Property].[PropertyID] = [PropertyValue].[PropertyID]
	LEFT JOIN [Catalog].PropertyGroup ON PropertyGroup.PropertyGroupID = [Property].GroupID
	WHERE [Property].[PropertyID] = @PropertyID
	order by [PropertyValue].[SortOrder]
END

GO--

insert into settings.settings (name, value) values('1c_DisableProductsDecremention', 'True')
Update [Order].ShippingParam Set [ParamValue] = 302 Where [ParamValue] = '301' and [ParamName] = 'Tariffs'

GO--

DROP PROCEDURE [Catalog].[sp_GetCategoriesByBrand]

GO--

DROP FUNCTION [Settings].[GetChildCategoryByParent]
GO--
 
Create FUNCTION [Settings].[GetChildCategoryByParent] (@ParentId INT)
RETURNS TABLE
AS
RETURN (
		WITH Hierarchycte(id) AS (
				SELECT CategoryID
				FROM [Catalog].[Category]
				WHERE CategoryID = @ParentId
				
				UNION ALL
				
				SELECT CategoryID
				FROM [Catalog].[Category]
				INNER JOIN hierarchycte ON [Category].ParentCategory = hierarchycte.id
					AND CategoryID <> 0
				)
		SELECT id
		FROM hierarchycte
)

GO--

DROP FUNCTION [Settings].[GetParentsCategoryByChild]
GO--
Create FUNCTION [Settings].[GetParentsCategoryByChild] (@ChildId int)
   RETURNS Table
AS RETURN (
	with parents as 
	(
	select CategoryID, ParentCategory, 1 As Sort
	from Catalog.Category
	where CategoryID = @ChildId -- this is the starting point you want in your recursion
	union all
	select C.CategoryID, C.ParentCategory , (p.Sort +1) as Sort
	from Catalog.Category c
	join parents p on C.CategoryID = P.ParentCategory  -- this is the recursion
	-- Since your parent id is not NULL the recursion will happen continously.
	-- For that we apply the condition C.id<>C.parentid 
    AND (C.CategoryID<>C.ParentCategory Or C.CategoryID <>0)
	) 
	select CategoryID as id, Sort as sort from parents
)

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

if not  exists(select * from settings.settings where name = 'IsMobileTemplateActive')
insert into settings.settings (name, value) values ('IsMobileTemplateActive','True')

GO--


UPDATE [Settings].[InternalSettings] SET [settingValue] = '5.0.18' WHERE [settingKey] = 'db_version'


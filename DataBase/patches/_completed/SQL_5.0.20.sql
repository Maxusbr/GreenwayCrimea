
insert into [Settings].[Localization] (LanguageId,ResourceKey,ResourceValue) Values(1,'PreOrder.Index.Available','Есть в наличии')
insert into [Settings].[Localization] (LanguageId,ResourceKey,ResourceValue) Values(2,'PreOrder.Index.Available','Available')

GO--

ALTER TABLE Catalog.Product ADD CONSTRAINT
	DF_Product_Ratio DEFAULT 0 FOR Ratio
GO--
ALTER TABLE Catalog.Product ADD CONSTRAINT
	DF_Product_Enabled DEFAULT 1 FOR Enabled
GO--
ALTER TABLE Catalog.Product ADD CONSTRAINT
	DF_Product_DateAdded DEFAULT getdate() FOR DateAdded
GO--
ALTER TABLE Catalog.Product ADD CONSTRAINT
	DF_Product_DateModified DEFAULT getdate() FOR DateModified
GO--
ALTER TABLE Catalog.Product ADD CONSTRAINT
	DF_Product_ActiveView360 DEFAULT 0 FOR ActiveView360
GO--
ALTER TABLE Catalog.Product ADD CONSTRAINT
	DF_Product_Multiplicity DEFAULT 1 FOR Multiplicity
GO--



ALTER TABLE Catalog.Category ADD CONSTRAINT
	DF_Category_DisplayBrandsInMenu DEFAULT 0 FOR DisplayBrandsInMenu
GO--
ALTER TABLE Catalog.Category ADD CONSTRAINT
	DF_Category_DisplaySubCategoriesInMenu DEFAULT 0 FOR DisplaySubCategoriesInMenu
GO--
ALTER TABLE Catalog.Category ADD CONSTRAINT
	DF_Category_CatLevel DEFAULT 0 FOR CatLevel
GO--
ALTER TABLE Catalog.Category ADD CONSTRAINT
	DF_Category_DisplayStyle DEFAULT 0 FOR DisplayStyle
GO--
ALTER TABLE Catalog.Category ADD CONSTRAINT
	DF_Category_Hidden DEFAULT 0 FOR Hidden
GO--


Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (1, 'Core.Orders.Order.ChangingPaymentStatus', 'Оплата заказа') 
Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (2, 'Core.Orders.Order.ChangingPaymentStatus', 'Payment status')
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
				   [property].TYPE                 AS propertytype,
				   [property].Description		   AS propertydescription
				   
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
			   [property].TYPE                 AS propertytype,
			   [property].Description		   AS propertydescription
			   
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

if not exists(select [Key] From [CMS].[StaticBlock] where [Key] = 'headerСenterBlock')
	Begin 
			Insert  INTO [CMS].[StaticBlock] ([Key], [InnerName], [Content], [Added], [Modified],Enabled) 
			Values ('headerСenterBlock', 'Блок над посиком в Header', ' ', GETDATE(),GETDATE(), 0)
	end 
	
if not exists(select [Key] From [CMS].[StaticBlock] where [Key] = 'headerСenterBlockAlt')
	Begin 
			Insert  INTO [CMS].[StaticBlock] ([Key], [InnerName], [Content], [Added], [Modified],Enabled) 
			Values ('headerСenterBlockAlt', 'Блок под поиском в Header', ' ', GETDATE(),GETDATE(), 0)
	end 

GO--

Update [Settings].[MailFormat] SET [FormatText] = '<div class="wrapper" style="color: #4c4f56; font-family: Arial, Helvetica, sans-serif; font-size: 14px;">  <div class="header" style="border-bottom: 1px solid #ededed; display: table; margin-bottom: 25px; padding-bottom: 25px; width: 100%;">  <div class="logo" style="display: table-cell; text-align: left; vertical-align: middle;">#LOGO#</div>    <div class="phone" style="display: table-cell; text-align: right; vertical-align: middle;">  <div class="tel" style="font-size: 26px; font-weight: bold; line-height: 1; margin-bottom: 5px;">#MAIN_PHONE#</div>  </div>  </div>    <div class="data" style="display: table; width: 100%;">  <div class="data-cell" style="display: table-cell; padding: 0; padding-right: 1%; width: 48%;">  <div class="o-title vi" style="font-size: 14px; font-weight: bold; margin: 5px 0;">Информация о товаре</div>    <div class="l-row">  <div class="l-name vi cs-light" style="color: #acacac; display: inline-block; margin: 5px 0; padding-right: 15px; width: 300px; vertical-align: middle;">Заказ №:</div>    <div class="l-value vi" style="display: inline-block; margin: 5px 0;">#NUMBER#</div>  </div>    <div class="l-row">  <div class="l-name vi cs-light" style="color: #acacac; display: inline-block; margin: 5px 0; padding-right: 15px; width: 300px; vertical-align: middle;">Артикул товара:</div>    <div class="l-value vi" style="display: inline-block; margin: 5px 0;">#ARTNO#</div>  </div>    <div class="l-row">  <div class="l-name vi cs-light" style="color: #acacac; display: inline-block; margin: 5px 0; padding-right: 15px; width: 300px; vertical-align: middle;">Желаемый товар:</div>    <div class="l-value vi" style="display: inline-block; margin: 5px 0;">#ARTNO# - #PRODUCTNAME#, #COLOR# #SIZE#</div>  </div>    <div class="l-row">  <div class="l-name vi cs-light" style="color: #acacac; display: inline-block; margin: 5px 0; padding-right: 15px; width: 300px; vertical-align: middle;">Количество:</div>    <div class="l-value vi" style="display: inline-block; margin: 5px 0;">#QUANTITY#</div>  </div>    <div class="l-row">  <div class="l-name vi cs-light" style="color: #acacac; display: inline-block; margin: 5px 0; padding-right: 15px; width: 300px; vertical-align: middle;">Фамилия, имя заказчика:</div>    <div class="l-value vi" style="display: inline-block; margin: 5px 0;">#USERNAME#</div>  </div>    <div class="l-row">  <div class="l-name vi cs-light" style="color: #acacac; display: inline-block; margin: 5px 0; padding-right: 15px; width: 300px; vertical-align: middle;">Ссылка на добавление товара в корзину:</div>    <div class="l-value vi" style="display: inline-block; margin: 5px 0;">#LINK#</div>  </div>    <div class="l-row">  <div class="l-name vi cs-light" style="color: #acacac; display: inline-block; margin: 5px 0; padding-right: 15px; width: 300px; vertical-align: middle;">Комментарий:</div>    <div class="l-value vi" style="display: inline-block; margin: 5px 0;">#COMMENT#</div>  </div>  </div>  </div>    <div class="comment" style="margin-top: 15px;"><span class="comment-title" style="font-weight: bold;">Комментарий администратора: </span><span class="comment-txt" style="padding-left: 5px;">Приносим свои извинения, но мы не можем выполнить ваш заказ.</span></div>  </div>' where [FormatName] = 'Невозможность выполнения заказа'

GO--



ALTER TABLE Customers.Customer ADD
	Avatar nvarchar(100) NULL
	
GO--

ALTER PROCEDURE [customers].[sp_updatecustomerinfo] 
	@customerid uniqueidentifier, 
	@firstname nvarchar (70) , 
	@lastname nvarchar(70), 
	@patronymic nvarchar(70), 
	@phone nvarchar(max), 
	@standardphone bigint, @email nvarchar(100) ,
	@customergroupid INT = NULL, 
	@customerrole INT, 
	@bonuscardnumber bigint, 
	@admincomment nvarchar (max), 
	@managerid INT, 
	@rating INT,
	@avatar nvarchar(100)
AS
BEGIN
  UPDATE [customers].[customer]
  SET    [firstname] = @firstname,
         [lastname] = @lastname,
         [patronymic] = @patronymic,
         [phone] = @phone,
         [standardphone] = @standardphone,
         [email] = @email,
         [customergroupid] = @customergroupid,
         [customerrole] = @customerrole,
         [bonuscardnumber] = @bonuscardnumber,
         [admincomment] = @admincomment,
         [managerid] = @managerid,
         [rating] = @rating,
		 [avatar] = @avatar
  WHERE  customerid = @customerid
END

GO--


CREATE TABLE [Customers].[TaskGroup](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[SortOrder] [int] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateModified] [datetime] NOT NULL,
 CONSTRAINT [PK_TaskGroup] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO--
ALTER TABLE [Customers].[TaskGroup] ADD  CONSTRAINT [DF_TaskGroup_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO--
ALTER TABLE [Customers].[TaskGroup] ADD  CONSTRAINT [DF_TaskGroup_DateModified]  DEFAULT (getdate()) FOR [DateModified]
GO--
ALTER TABLE [Customers].[TaskGroup] ADD  CONSTRAINT [DF_TaskGroup_SortOrder]  DEFAULT (0) FOR [SortOrder]
GO--

SET IDENTITY_INSERT [Customers].[TaskGroup] ON
GO--
insert into [Customers].[TaskGroup] ([Id], [Name], [SortOrder]) values (1, 'Общие', -1)
GO--
SET IDENTITY_INSERT [Customers].[TaskGroup] OFF
GO--

INSERT INTO [Settings].[Settings] ([Name],[Value]) VALUES ('Tasks_DefaultTaskGroup', '1')
GO--

CREATE TABLE [Customers].[Task](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TaskGroupId] [int] NOT NULL,
	[AssignedManagerId] [int] NOT NULL,
	[AppointedManagerId] [int] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[Status] [int] NOT NULL,
	[InProgress] [bit] NOT NULL,
	[Priority] [int] NOT NULL,
	[DueDate] [datetime] NOT NULL,
	[LeadId] [int] NULL,
	[OrderId] [int] NULL,
	[CustomerId] [uniqueidentifier] NULL,
	[ResultShort] [nvarchar](255) NULL,
	[ResultFull] [nvarchar](max) NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateModified] [datetime] NOT NULL,
 CONSTRAINT [PK_Task] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO--

ALTER TABLE [Customers].[Task]  WITH CHECK ADD  CONSTRAINT [FK_Task_Customer] FOREIGN KEY([CustomerId])
REFERENCES [Customers].[Customer] ([CustomerID])
ON DELETE SET NULL
GO--
ALTER TABLE [Customers].[Task] CHECK CONSTRAINT [FK_Task_Customer]
GO--

ALTER TABLE [Customers].[Task]  WITH CHECK ADD  CONSTRAINT [FK_Task_Lead] FOREIGN KEY([LeadId])
REFERENCES [Order].[Lead] ([Id])
ON DELETE SET NULL
GO--
ALTER TABLE [Customers].[Task] CHECK CONSTRAINT [FK_Task_Lead]
GO--

ALTER TABLE [Customers].[Task]  WITH CHECK ADD  CONSTRAINT [FK_Task_TaskGroup] FOREIGN KEY([TaskGroupId])
REFERENCES [Customers].[TaskGroup] ([Id])
GO--
ALTER TABLE [Customers].[Task] CHECK CONSTRAINT [FK_Task_TaskGroup]
GO--

ALTER TABLE [Customers].[Task]  WITH CHECK ADD  CONSTRAINT [FK_Task_Order] FOREIGN KEY([OrderId])
REFERENCES [Order].[Order] ([OrderID])
ON DELETE SET NULL
GO--
ALTER TABLE [Customers].[Task] CHECK CONSTRAINT [FK_Task_Order]
GO--

ALTER TABLE [Customers].[Task]  WITH CHECK ADD  CONSTRAINT [FK_TaskAppointedManagerId_Managers] FOREIGN KEY([AppointedManagerId])
REFERENCES [Customers].[Managers] ([ManagerId])
GO--
ALTER TABLE [Customers].[Task] CHECK CONSTRAINT [FK_TaskAppointedManagerId_Managers]
GO--

ALTER TABLE [Customers].[Task]  WITH CHECK ADD  CONSTRAINT [FK_TaskAssignedManagerId_Managers] FOREIGN KEY([AssignedManagerId])
REFERENCES [Customers].[Managers] ([ManagerId])
GO--
ALTER TABLE [Customers].[Task] CHECK CONSTRAINT [FK_TaskAssignedManagerId_Managers]
GO--

ALTER TABLE [Customers].[Task] ADD  CONSTRAINT [DF_Task_InProgress] DEFAULT (0) FOR InProgress
GO--

SET IDENTITY_INSERT [Customers].[Task] ON
GO--
insert into Customers.Task 
	([Id]
	,[TaskGroupId]
	,[AssignedManagerId]
	,[AppointedManagerId]
	,[Name]
	,[Description]
	,[Status]
	,[Priority]
	,[DueDate]
	,[LeadId]
	,[OrderId]
	,[CustomerId]
	,[ResultShort]
	,[ResultFull]
	,[DateCreated]
	,[DateModified])
select 
	 TaskId
	,1 --[TaskGroupId]
	,[AssignedManagerId]
	,[AppointedManagerId]
	,[Name]
	,[Description]
	,[Status]
	,1--[Priority]
	,[DueDate]
	,[LeadId]
	,[OrderId]
	,[CustomerId]
	,[ResultShort]
	,[ResultFull]
	,[DateCreated]
	,[DateModified] from Customers.ManagerTask where TaskId NOT IN (SELECT Id FROM Customers.Task)
GO--
SET IDENTITY_INSERT [Customers].[Task] OFF
GO--

Update [Settings].[Localization] SET ResourceValue = 'Округлять до сотен' where ResourceKey = 'Admin.Currencies.ECurrencyRound.UpToHundreds' and LanguageId = 1
Update [Settings].[Localization] SET ResourceValue = 'Округлять до десятков' where ResourceKey = 'Admin.Currencies.ECurrencyRound.UpToTens' and LanguageId = 1
Update [Settings].[Localization] SET ResourceValue = 'Округлять до тысяч' where ResourceKey = 'Admin.Currencies.ECurrencyRound.UpToThousands' and LanguageId = 1

GO--

if not exists(select Id From [Shipping].[SdekCities] where CityName ='Ак-Шыйрак') Begin Insert  INTO [Shipping].[SdekCities] Values (4663,'Ак-Шыйрак', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Ананьево') Begin Insert  INTO [Shipping].[SdekCities] Values (4830,'Ананьево', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Боконбаевское') Begin Insert  INTO [Shipping].[SdekCities] Values (5508,'Боконбаевское', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Бостери') Begin Insert  INTO [Shipping].[SdekCities] Values (5551,'Бостери', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Каджи-Сай') Begin Insert  INTO [Shipping].[SdekCities] Values (7514,'Каджи-Сай', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Кызыл Туу') Begin Insert  INTO [Shipping].[SdekCities] Values (8400,'Кызыл Туу', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Покровка') Begin Insert  INTO [Shipping].[SdekCities] Values (10297,'Покровка', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Пржевальск') Begin Insert  INTO [Shipping].[SdekCities] Values (10383,'Пржевальск', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Рыбачье') Begin Insert  INTO [Shipping].[SdekCities] Values (10753,'Рыбачье', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Тюп') Begin Insert  INTO [Shipping].[SdekCities] Values (11819,'Тюп', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Чолпон-Ата') Begin Insert  INTO [Shipping].[SdekCities] Values (12635,'Чолпон-Ата', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Ак-Суу') Begin Insert  INTO [Shipping].[SdekCities] Values (4661,'Ак-Суу', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Ананьево') Begin Insert  INTO [Shipping].[SdekCities] Values (4831,'Ананьево', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Араван') Begin Insert  INTO [Shipping].[SdekCities] Values (4892,'Араван', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Балыкчи') Begin Insert  INTO [Shipping].[SdekCities] Values (5128,'Балыкчи', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Беловодское') Begin Insert  INTO [Shipping].[SdekCities] Values (5290,'Беловодское', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Бишкек') Begin Insert  INTO [Shipping].[SdekCities] Values (5444,'Бишкек', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Боконбаевское') Begin Insert  INTO [Shipping].[SdekCities] Values (5509,'Боконбаевское', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Бордунский') Begin Insert  INTO [Shipping].[SdekCities] Values (5531,'Бордунский', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Быстровка') Begin Insert  INTO [Shipping].[SdekCities] Values (5757,'Быстровка', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Жалал Абад') Begin Insert  INTO [Shipping].[SdekCities] Values (7134,'Жалал Абад', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Ивановка') Begin Insert  INTO [Shipping].[SdekCities] Values (7287,'Ивановка', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Каинда') Begin Insert  INTO [Shipping].[SdekCities] Values (7531,'Каинда', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Каинды') Begin Insert  INTO [Shipping].[SdekCities] Values (7532,'Каинды', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Кант') Begin Insert  INTO [Shipping].[SdekCities] Values (7644,'Кант', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Кара-Балта') Begin Insert  INTO [Shipping].[SdekCities] Values (7657,'Кара-Балта', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Караван') Begin Insert  INTO [Shipping].[SdekCities] Values (7666,'Караван', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Каракол') Begin Insert  INTO [Shipping].[SdekCities] Values (7673,'Каракол', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Кемин') Begin Insert  INTO [Shipping].[SdekCities] Values (7820,'Кемин', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Кербен') Begin Insert  INTO [Shipping].[SdekCities] Values (7843,'Кербен', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Комсомол') Begin Insert  INTO [Shipping].[SdekCities] Values (31113,'Комсомол', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Кызыл Суу') Begin Insert  INTO [Shipping].[SdekCities] Values (8399,'Кызыл Суу', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Лебединовка') Begin Insert  INTO [Shipping].[SdekCities] Values (8522,'Лебединовка', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Майлуу-Суу') Begin Insert  INTO [Shipping].[SdekCities] Values (8870,'Майлуу-Суу', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Нарын') Begin Insert  INTO [Shipping].[SdekCities] Values (9458,'Нарын', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Ош') Begin Insert  INTO [Shipping].[SdekCities] Values (10009,'Ош', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Пульгон') Begin Insert  INTO [Shipping].[SdekCities] Values (10414,'Пульгон', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Соколук') Begin Insert  INTO [Shipping].[SdekCities] Values (11282,'Соколук', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Сокулук') Begin Insert  INTO [Shipping].[SdekCities] Values (11284,'Сокулук', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Сузак') Begin Insert  INTO [Shipping].[SdekCities] Values (31114,'Сузак', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Талас') Begin Insert  INTO [Shipping].[SdekCities] Values (11486,'Талас', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Таш-Кумыр') Begin Insert  INTO [Shipping].[SdekCities] Values (11559,'Таш-Кумыр', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Токмак') Begin Insert  INTO [Shipping].[SdekCities] Values (11657,'Токмак', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Тюп') Begin Insert  INTO [Shipping].[SdekCities] Values (11820,'Тюп', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Фрунзе') Begin Insert  INTO [Shipping].[SdekCities] Values (12119,'Фрунзе', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Чолпон-Ата') Begin Insert  INTO [Shipping].[SdekCities] Values (12636,'Чолпон-Ата', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Чуй') Begin Insert  INTO [Shipping].[SdekCities] Values (12647,'Чуй', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Эски-Ноокат') Begin Insert  INTO [Shipping].[SdekCities] Values (12909,'Эски-Ноокат', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Ат-Баши') Begin Insert  INTO [Shipping].[SdekCities] Values (4969,'Ат-Баши', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Дюрбельджин') Begin Insert  INTO [Shipping].[SdekCities] Values (7072,'Дюрбельджин', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Казарман') Begin Insert  INTO [Shipping].[SdekCities] Values (7523,'Казарман', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Кочкорка') Begin Insert  INTO [Shipping].[SdekCities] Values (8225,'Кочкорка', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Кош-Дебе') Begin Insert  INTO [Shipping].[SdekCities] Values (8226,'Кош-Дебе', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Мин-Куш') Begin Insert  INTO [Shipping].[SdekCities] Values (9208,'Мин-Куш', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Нарын') Begin Insert  INTO [Shipping].[SdekCities] Values (9459,'Нарын', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Сары-Булак') Begin Insert  INTO [Shipping].[SdekCities] Values (10966,'Сары-Булак', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Суусамыр') Begin Insert  INTO [Shipping].[SdekCities] Values (11455,'Суусамыр', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Угют') Begin Insert  INTO [Shipping].[SdekCities] Values (11842,'Угют', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Чаек') Begin Insert  INTO [Shipping].[SdekCities] Values (12500,'Чаек', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Чолпон') Begin Insert  INTO [Shipping].[SdekCities] Values (12634,'Чолпон', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Ак-Там') Begin Insert  INTO [Shipping].[SdekCities] Values (4662,'Ак-Там', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Ала-Бука') Begin Insert  INTO [Shipping].[SdekCities] Values (4700,'Ала-Бука', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Араван') Begin Insert  INTO [Shipping].[SdekCities] Values (4893,'Араван', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Арсланбоб') Begin Insert  INTO [Shipping].[SdekCities] Values (4938,'Арсланбоб', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Базар-Курган') Begin Insert  INTO [Shipping].[SdekCities] Values (5062,'Базар-Курган', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Баткен') Begin Insert  INTO [Shipping].[SdekCities] Values (5193,'Баткен', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Гульча') Begin Insert  INTO [Shipping].[SdekCities] Values (6705,'Гульча', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Джалал-Абад') Begin Insert  INTO [Shipping].[SdekCities] Values (6868,'Джалал-Абад', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Исфана') Begin Insert  INTO [Shipping].[SdekCities] Values (7449,'Исфана', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Кара-Кульджа') Begin Insert  INTO [Shipping].[SdekCities] Values (7660,'Кара-Кульджа', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Кара-Суу') Begin Insert  INTO [Shipping].[SdekCities] Values (7661,'Кара-Суу', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Караван') Begin Insert  INTO [Shipping].[SdekCities] Values (7667,'Караван', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Карамык') Begin Insert  INTO [Shipping].[SdekCities] Values (7675,'Карамык', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Кок-Янгак') Begin Insert  INTO [Shipping].[SdekCities] Values (8051,'Кок-Янгак', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Кызыл-Кия') Begin Insert  INTO [Shipping].[SdekCities] Values (8401,'Кызыл-Кия', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Майли-Сай') Begin Insert  INTO [Shipping].[SdekCities] Values (8869,'Майли-Сай', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Ош') Begin Insert  INTO [Shipping].[SdekCities] Values (10010,'Ош', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Сары-Таш') Begin Insert  INTO [Shipping].[SdekCities] Values (10967,'Сары-Таш', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Сопу-Коргон') Begin Insert  INTO [Shipping].[SdekCities] Values (11304,'Сопу-Коргон', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Сулюкта') Begin Insert  INTO [Shipping].[SdekCities] Values (11438,'Сулюкта', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Таш-Кумыр') Begin Insert  INTO [Shipping].[SdekCities] Values (11560,'Таш-Кумыр', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Узген') Begin Insert  INTO [Shipping].[SdekCities] Values (11848,'Узген', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Фрунзе') Begin Insert  INTO [Shipping].[SdekCities] Values (12120,'Фрунзе', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Хайдаркен') Begin Insert  INTO [Shipping].[SdekCities] Values (12178,'Хайдаркен', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Аманбаево') Begin Insert  INTO [Shipping].[SdekCities] Values (4798,'Аманбаево', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Кара-Куль') Begin Insert  INTO [Shipping].[SdekCities] Values (7659,'Кара-Куль', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Кировское') Begin Insert  INTO [Shipping].[SdekCities] Values (7926,'Кировское', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Ленинполь') Begin Insert  INTO [Shipping].[SdekCities] Values (8589,'Ленинполь', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Покровка') Begin Insert  INTO [Shipping].[SdekCities] Values (10298,'Покровка', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Талас') Begin Insert  INTO [Shipping].[SdekCities] Values (11487,'Талас', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Токтогул') Begin Insert  INTO [Shipping].[SdekCities] Values (11658,'Токтогул', '', 0.00) end
if not exists(select Id From [Shipping].[SdekCities] where CityName ='Толук') Begin Insert  INTO [Shipping].[SdekCities] Values (11667,'Толук', '', 0.00) end

GO--

if not exists(select CountryID From [Customers].[Country] where CountryName = 'Киргизия')
	Begin
		Insert  INTO [Customers].[Country] (CountryName,CountryISO2,CountryISO3) 
		Values ('Киргизия', 'KG', 'KGZ')
	end

if not exists(select CountryID From [Customers].[Country_ru] where CountryName = 'Киргизия')
	Begin
		Insert  INTO [Customers].[Country_ru] (CountryName,CountryISO2,CountryISO3) 
		Values ('Киргизия', 'KG', 'KGZ')
	end

if not exists(select CountryID From [Customers].[Country_en] where CountryName = 'Киргизия')
	Begin
		Insert  INTO [Customers].[Country_en] (CountryName,CountryISO2,CountryISO3) 
		Values ('Киргизия', 'KG', 'KGZ')
	end
	
GO--

declare @CountryID int
set @CountryID = (select CountryID from [Customers].[Country] where CountryName = 'Киргизия')

if not exists(select RegionID From [Customers].[Region] where RegionName = 'Иссык-Кульская обл.') Begin if @CountryID is null begin 
Insert  INTO [Customers].[Region] (RegionName) Values ('Иссык-Кульская обл.') end else 
begin Insert  INTO [Customers].[Region] (CountryID, RegionName) Values (@CountryID, 'Иссык-Кульская обл.') end end 

if not exists(select RegionID From [Customers].[Region] where RegionName = 'Киргизия') Begin if @CountryID is null begin 
Insert  INTO [Customers].[Region] (RegionName) Values ('Киргизия') end else 
begin Insert  INTO [Customers].[Region] (CountryID, RegionName) Values (@CountryID, 'Киргизия') end end

if not exists(select RegionID From [Customers].[Region] where RegionName = 'Нарынская обл.') Begin if @CountryID is null begin 
Insert  INTO [Customers].[Region] (RegionName) Values ('Нарынская обл.') end else 
begin Insert  INTO [Customers].[Region] (CountryID, RegionName) Values (@CountryID, 'Нарынская обл.') end end

if not exists(select RegionID From [Customers].[Region] where RegionName = 'Ошская обл.') Begin if @CountryID is null begin 
Insert  INTO [Customers].[Region] (RegionName) Values ('Ошская обл.') end else 
begin Insert  INTO [Customers].[Region] (CountryID, RegionName) Values (@CountryID, 'Ошская обл.') end end 

if not exists(select RegionID From [Customers].[Region] where RegionName = 'Таласская обл.') Begin if @CountryID is null begin 
Insert  INTO [Customers].[Region] (RegionName) Values ('Таласская обл.') end else 
begin Insert  INTO [Customers].[Region] (CountryID, RegionName) Values (@CountryID, 'Таласская обл.') end end 

declare @RegionId int
set @RegionId = (select RegionID from [Customers].[Region] where RegionName = 'Иссык-Кульская обл.')

if not exists(select CityID From [Customers].[City] where CityName = 'Ак-Шыйрак' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Ак-Шыйрак',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Ак-Шыйрак', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Ананьево' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Ананьево',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Ананьево', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Боконбаевское' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Боконбаевское',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Боконбаевское', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Бостери' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Бостери',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Бостери', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Каджи-Сай' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Каджи-Сай',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Каджи-Сай', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Кызыл Туу' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Кызыл Туу',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Кызыл Туу', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Покровка' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Покровка',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Покровка', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Пржевальск' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Пржевальск',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Пржевальск', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Рыбачье' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Рыбачье',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Рыбачье', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Тюп' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Тюп',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Тюп', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Чолпон-Ата' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Чолпон-Ата',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Чолпон-Ата', 0, 0) end

set @RegionId = (select RegionID from [Customers].[Region] where RegionName = 'Киргизия')

if not exists(select CityID From [Customers].[City] where CityName = 'Ак-Суу' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Ак-Суу',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Ак-Суу', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Ананьево' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Ананьево',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Ананьево', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Араван' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Араван',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Араван', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Балыкчи' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Балыкчи',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Балыкчи', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Беловодское' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Беловодское',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Беловодское', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Бишкек' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Бишкек',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Бишкек', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Боконбаевское' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Боконбаевское',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Боконбаевское', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Бордунский' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Бордунский',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Бордунский', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Быстровка' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Быстровка',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Быстровка', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Жалал Абад' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Жалал Абад',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Жалал Абад', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Ивановка' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Ивановка',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Ивановка', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Каинда' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Каинда',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Каинда', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Каинды' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Каинды',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Каинды', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Кант' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Кант',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Кант', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Кара-Балта' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Кара-Балта',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Кара-Балта', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Караван' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Караван',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Караван', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Каракол' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Каракол',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Каракол', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Кемин' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Кемин',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Кемин', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Кербен' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Кербен',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Кербен', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Комсомол' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Комсомол',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Комсомол', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Кызыл Суу' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Кызыл Суу',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Кызыл Суу', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Лебединовка' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Лебединовка',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Лебединовка', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Майлуу-Суу' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Майлуу-Суу',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Майлуу-Суу', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Нарын' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Нарын',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Нарын', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Ош' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Ош',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Ош', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Пульгон' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Пульгон',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Пульгон', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Соколук' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Соколук',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Соколук', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Сокулук' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Сокулук',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Сокулук', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Сузак' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Сузак',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Сузак', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Талас' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Талас',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Талас', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Таш-Кумыр' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Таш-Кумыр',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Таш-Кумыр', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Токмак' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Токмак',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Токмак', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Тюп' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Тюп',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Тюп', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Фрунзе' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Фрунзе',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Фрунзе', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Чолпон-Ата' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Чолпон-Ата',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Чолпон-Ата', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Чуй' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Чуй',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Чуй', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Эски-Ноокат' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Эски-Ноокат',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Эски-Ноокат', 0, 0) end

set @RegionId = (select RegionID from [Customers].[Region] where RegionName = 'Нарынская обл.')

if not exists(select CityID From [Customers].[City] where CityName = 'Ат-Баши' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Ат-Баши',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Ат-Баши', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Дюрбельджин' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Дюрбельджин',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Дюрбельджин', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Казарман' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Казарман',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Казарман', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Кочкорка' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Кочкорка',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Кочкорка', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Кош-Дебе' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Кош-Дебе',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Кош-Дебе', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Мин-Куш' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Мин-Куш',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Мин-Куш', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Нарын' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Нарын',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Нарын', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Сары-Булак' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Сары-Булак',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Сары-Булак', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Суусамыр' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Суусамыр',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Суусамыр', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Угют' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Угют',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Угют', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Чаек' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Чаек',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Чаек', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Чолпон' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Чолпон',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Чолпон', 0, 0) end

set @RegionId = (select RegionID from [Customers].[Region] where RegionName = 'Ошская обл.')

if not exists(select CityID From [Customers].[City] where CityName = 'Ак-Там' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Ак-Там',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Ак-Там', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Ала-Бука' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Ала-Бука',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Ала-Бука', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Араван' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Араван',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Араван', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Арсланбоб' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Арсланбоб',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Арсланбоб', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Базар-Курган' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Базар-Курган',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Базар-Курган', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Баткен' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Баткен',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Баткен', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Гульча' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Гульча',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Гульча', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Джалал-Абад' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Джалал-Абад',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Джалал-Абад', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Исфана' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Исфана',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Исфана', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Кара-Кульджа' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Кара-Кульджа',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Кара-Кульджа', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Кара-Суу' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Кара-Суу',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Кара-Суу', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Караван' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Караван',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Караван', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Карамык' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Карамык',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Карамык', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Кок-Янгак' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Кок-Янгак',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Кок-Янгак', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Кызыл-Кия' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Кызыл-Кия',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Кызыл-Кия', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Майли-Сай' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Майли-Сай',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Майли-Сай', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Ош' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Ош',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Ош', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Сары-Таш' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Сары-Таш',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Сары-Таш', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Сопу-Коргон' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Сопу-Коргон',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Сопу-Коргон', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Сулюкта' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Сулюкта',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Сулюкта', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Таш-Кумыр' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Таш-Кумыр',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Таш-Кумыр', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Узген' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Узген',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Узген', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Фрунзе' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Фрунзе',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Фрунзе', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Хайдаркен' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Хайдаркен',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Хайдаркен', 0, 0) end


set @RegionId = (select RegionID from [Customers].[Region] where RegionName = 'Таласская обл.')

if not exists(select CityID From [Customers].[City] where CityName = 'Аманбаево' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Аманбаево',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Аманбаево', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Кара-Куль' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Кара-Куль',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Кара-Куль', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Кировское' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Кировское',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Кировское', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Ленинполь' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Ленинполь',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Ленинполь', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Покровка' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Покровка',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Покровка', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Талас' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Талас',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Талас', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Токтогул' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Токтогул',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Токтогул', 0, 0) end
if not exists(select CityID From [Customers].[City] where CityName = 'Толук' and RegionID = @RegionId) Begin if @RegionId is null Insert  INTO [Customers].[City] (CityName, CitySort, DisplayInPopup) Values ('Толук',  0, 0) else Insert  INTO [Customers].[City] (RegionID, CityName, CitySort, DisplayInPopup) Values (@RegionId, 'Толук', 0, 0) end

GO--

if not exists(select * from [Settings].[MailFormatType] where TypeName = 'Смена комментария заказа пользователем')
	Begin
		Insert  INTO [Settings].[MailFormatType] (TypeName, SortOrder, Comment) Values ('Смена комментария заказа пользователем', 300, 'Письмо при смене комментария пользователем(#ORDER_ID#, #ORDER_USER_COMMENT#, #STORE_NAME#, #NUMBER#)')
	end
	
GO--

if not exists(select * from [Settings].[MailFormat] where [FormatName] = 'Смена комментария заказа пользователем')
	Begin
		Insert  INTO [Settings].[MailFormat] (FormatName, FormatType, SortOrder, Enable, AddDate, ModifyDate, FormatSubject, FormatText) Values ('Смена комментария заказа пользователем',
		(select top 1 MailFormatTypeID from [Settings].[MailFormatType] where TypeName = 'Смена комментария заказа пользователем'), 1000, 1, GETDATE(), GETDATE(),
		'Пользователь изменил свой комментарий к заказу', '<div style="color: #4c4f56; font-family: Arial, Helvetica, sans-serif; font-size: 14px;">
<div class="header" style="border-bottom: 1px solid #ededed; display: table; margin-bottom: 25px; padding-bottom: 25px; width: 100%;">
<div class="logo" style="display: table-cell; text-align: left; vertical-align: middle;">#LOGO#</div>

<div class="phone" style="display: table-cell; text-align: right; vertical-align: middle;">
<div class="tel" style="font-size: 26px; font-weight: bold; line-height: 1; margin-bottom: 5px;">&nbsp;</div>

<div class="inform" style="font-size: 12px;">&nbsp;</div>
</div>
</div>')	
	end
	
GO--

if not exists(select * from [Settings].[Localization] where [ResourceKey] = 'Core.Orders.Order.PaySpend')
	begin
		insert into [Settings].[Localization] (LanguageId,ResourceKey,ResourceValue) 
			Values (1,'Core.Orders.Order.PaySpend','Произведена')
	end

if not exists(select * from [Settings].[Localization] where [ResourceKey] = 'Core.Orders.Order.PayCancel')
	begin
		insert into [Settings].[Localization] (LanguageId,ResourceKey,ResourceValue) 
			Values ( 1, 'Core.Orders.Order.PayCancel','Отменена')
	end
	
GO--

if not exists(select * from [Settings].[MailFormatType] where TypeName = 'Произведение/Отмена оплаты')
	Begin
		Insert  INTO [Settings].[MailFormatType] (TypeName, SortOrder, Comment) Values ('Произведение/Отмена оплаты', 300, 'Письмо при проведении/отмене оплаты(#ORDER_ID#, #PAY_STATUS#, #STORE_NAME#, #NUMBER#, #SUM#)')
	end
	
GO--

if not exists(select * from [Settings].[MailFormat] where [FormatName] = 'Произведение/Отмена оплаты')
	Begin
		Insert  INTO [Settings].[MailFormat] (FormatName, FormatType, SortOrder, Enable, AddDate, ModifyDate, FormatSubject, FormatText) Values ('Произведение/Отмена оплаты',
		(select top 1 MailFormatTypeID from [Settings].[MailFormatType] where TypeName = 'Произведение/Отмена оплаты'), 1000, 1, GETDATE(), GETDATE(),
		'Письмо при смене статуса заказа оплачено/не оплачено', '<div style="color: #4c4f56; font-family: Arial, Helvetica, sans-serif; font-size: 14px;">
<div class="header" style="border-bottom: 1px solid #ededed; display: table; margin-bottom: 25px; padding-bottom: 25px; width: 100%;">
<div class="logo" style="display: table-cell; text-align: left; vertical-align: middle;">#LOGO#</div>

<div class="phone" style="display: table-cell; text-align: right; vertical-align: middle;">
<div class="tel" style="font-size: 26px; font-weight: bold; line-height: 1; margin-bottom: 5px;">&nbsp;</div>

<div class="inform" style="font-size: 12px;">&nbsp;</div>
</div>
</div>

<p>По заказу № #NUMBER# была #PAY_STATUS# оплата на сумму #SUM#.</p>
</div>
')	
	end
	
GO--

CREATE TABLE [CMS].[Attachment](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ObjId] [int] NOT NULL,
	[Type] [nvarchar](50) NOT NULL,
	[FileName] [nvarchar](255) NOT NULL,
	[SortOrder] [int] NOT NULL,
	[FileSize] [int] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateModified] [datetime] NOT NULL,
 CONSTRAINT [PK_Attachment] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO--

CREATE TABLE [Customers].[ViewedTask](
	[TaskId] [int] NOT NULL,
	[ManagerId] [int] NOT NULL,
	[ViewDate] [datetime] NOT NULL,
 CONSTRAINT [PK_ViewedTask] PRIMARY KEY CLUSTERED 
(
	[TaskId] ASC,
	[ManagerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO--
ALTER TABLE [Customers].[ViewedTask] ADD  CONSTRAINT [DF_ViewedTask_ViewDate]  DEFAULT (getdate()) FOR [ViewDate]
GO--
ALTER TABLE [Customers].[ViewedTask]  WITH CHECK ADD  CONSTRAINT [FK_ViewedTask_Managers] FOREIGN KEY([ManagerId])
REFERENCES [Customers].[Managers] ([ManagerId])
ON DELETE CASCADE
GO--
ALTER TABLE [Customers].[ViewedTask] CHECK CONSTRAINT [FK_ViewedTask_Managers]
GO--
ALTER TABLE [Customers].[ViewedTask]  WITH CHECK ADD  CONSTRAINT [FK_ViewedTask_Task] FOREIGN KEY([TaskId])
REFERENCES [Customers].[Task] ([Id])
ON DELETE CASCADE
GO--
ALTER TABLE [Customers].[ViewedTask] CHECK CONSTRAINT [FK_ViewedTask_Task]
GO--

-- уже существующие задачи - просмотренные
INSERT INTO Customers.ViewedTask (TaskId, ManagerId, ViewDate)
SELECT t.Id, m.ManagerId, GETDATE() FROM Customers.Task as t, Customers.Managers as m
WHERE Id NOT IN (SELECT Id FROM Customers.ViewedTask as vt WHERE vt.ManagerId = m.ManagerId AND vt.TaskId = t.Id)
GO--

-- отменена -> закрыта
UPDATE Customers.Task SET [Status] = 1 WHERE [Status] = 2
GO--
UPDATE Customers.Task SET InProgress = (CASE WHEN [Status] = 4 THEN 1 ELSE 0 END)
GO--
-- в процессе -> открыта
UPDATE Customers.Task SET [Status] = 0 WHERE [Status] = 4
GO--


UPDATE [Settings].[InternalSettings] SET [settingValue] = '5.0.20' WHERE [settingKey] = 'db_version'
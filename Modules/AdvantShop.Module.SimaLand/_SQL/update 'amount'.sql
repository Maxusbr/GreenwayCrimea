/****** Скрипт для команды SelectTopNRows из среды SSMS  ******/
update Catalog.Offer 
SET Amount=0 
where 
ProductID in (SELECT Product.ProductID FROM Catalog.Product 
INNER JOIN Catalog.ProductCategories ON ProductCategories.ProductID=Product.ProductId
INNER JOIN catalog.Category ON Category.CategoryID=ProductCategories.CategoryID
WHERE Category.Hidden=1 and Product.ModifiedBy='ProductsSimaLand' and ProductCategories.Main=1)
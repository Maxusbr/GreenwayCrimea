UPDATE Catalog.Offer 
SET Amount = 1000 
WHERE ProductId in (SELECT Product.ProductId 
						FROM Catalog.Product
					INNER JOIN [Module].[SimalandProducts] 
						ON [SimalandProducts].ProductId = Product.ProductId)
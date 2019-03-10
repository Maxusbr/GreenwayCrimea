CREATE TABLE Module.SimalandProducts(
                                ProductId int not null,
                                SlProductId int not null,
                                UpdateDate datetime not null default GETDATE(),
                                CONSTRAINT FK_SimalandProducts FOREIGN KEY (ProductId) REFERENCES Catalog.Product(ProductId) ON DELETE CASCADE)
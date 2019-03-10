CREATE TABLE Module.SimalandParseProduct(
                                ProductId int not null,
                                CONSTRAINT FK_SimalandParseProduct FOREIGN KEY (ProductId) REFERENCES Catalog.Product(ProductId) ON DELETE CASCADE)
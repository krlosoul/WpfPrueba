CREATE DATABASE WpfPrueba

CREATE TABLE Clientes(
Cedula varchar(10) primary key not null, 
Nombre varchar(45),
Apellido varchar(45),
Direccion varchar(45),
Telefono varchar(10),
);

CREATE TABLE Productos(
Codigo varchar(6) primary key not null,
Nombre varchar(45),
Valor int,
Cantidad int
);

CREATE TABLE Cabeceras(
Id int Primary key identity not null,
IdCliente varchar(10),
Fecha datetime,
FOREIGN KEY(IdCliente) REFERENCES Clientes(Cedula)
);

CREATE TABLE Detalles(
Id int primary key identity not null,
IdCabecera int,
IdProducto varchar(6),
Cantidad int,
ValorUnidad int,
ValorTotal int,
FOREIGN KEY(IdCabecera) REFERENCES Cabeceras(Id),
FOREIGN KEY(IdProducto) REFERENCES Productos(Codigo)
);

INSERT INTO Clientes VALUES('1111122222','Carlos','Rodriguez','cll 1 # 2-3','1111111111')
INSERT INTO Clientes VALUES('3333344444','Lina','Suarez','cll 4 # 5-6','2222222222')

INSERT INTO Productos VALUES('MSD001','Micro SD 32 GB',30000,5)
INSERT INTO Productos VALUES('USB001','USB 8 GB',10000,5)

IF OBJECT_ID ( 'dbo.SP_CONS_CLIENTE', 'P' ) IS NOT NULL 
    DROP PROCEDURE dbo.SP_CONS_CLIENTE;
GO
CREATE PROCEDURE SP_CONS_CLIENTE
AS
BEGIN
	SELECT * FROM Clientes
END

IF OBJECT_ID ( 'dbo.SP_CONS_PRODUCTOS', 'P' ) IS NOT NULL 
    DROP PROCEDURE dbo.SP_CONS_PRODUCTOS;
GO
CREATE PROCEDURE SP_CONS_PRODUCTOS
AS
BEGIN
	SELECT * FROM Productos
END

IF OBJECT_ID ( 'dbo.SP_SAVE_CABECERA', 'P' ) IS NOT NULL 
    DROP PROCEDURE dbo.SP_SAVE_CABECERA;
GO
CREATE PROCEDURE SP_SAVE_CABECERA
	@IdCliente varchar(10)
AS
BEGIN
	INSERT INTO Cabeceras (IdCliente,Fecha) VALUES (@IdCliente,GETDATE())
	
	SELECT @@IDENTITY AS IdCabecera 
END

IF OBJECT_ID ( 'dbo.SP_SAVE_DETALLE', 'P' ) IS NOT NULL 
    DROP PROCEDURE dbo.SP_SAVE_DETALLE;
GO
CREATE PROCEDURE SP_SAVE_DETALLE
	@IdCabecera int,
	@IdProducto varchar(6),
	@Cantidad int,
	@ValorUnidad int,
	@ValorTotal int
AS
BEGIN
	INSERT INTO Detalles (IdCabecera,IdProducto,Cantidad,ValorUnidad,ValorTotal) VALUES (@IdCabecera,@IdProducto,@Cantidad,@ValorUnidad,@ValorTotal)
END

IF OBJECT_ID ( 'dbo.TRG_REMOVE_STOCK', 'TR' ) IS NOT NULL 
    DROP TRIGGER dbo.TRG_REMOVE_STOCK;
GO
CREATE TRIGGER TRG_REMOVE_STOCK
ON WpfPrueba.dbo.Detalles
AFTER INSERT
AS
BEGIN
	UPDATE P SET P.Cantidad=P.Cantidad-I.Cantidad
	FROM Productos AS P
	INNER JOIN INSERTED AS I ON I.IdProducto=P.Codigo	 	
END


create database VentasDb

go

use VentasDb

go

create table Menu(
IdMenu int primary key identity(1,1),
Descripcion varchar(30),
IdMenuPadre int references Menu(IdMenu),
Icono varchar(30),
Controlador varchar(30),
PaginaAccion varchar(30),
EsActivo bit,
FechaRegistro datetime default getdate()
)

go

create table Rol(
IdRol int primary key identity(1,1),
Descripcion varchar(30),
EsActivo bit,
FechaRegistro datetime default getdate()
)

go
 
 create table RolMenu(
 IdRolMenu int primary key identity(1,1),
 IdRol int references Rol(IdRol),
 IdMenu int references Menu(IdMenu),
 EsActivo bit,
 FechaRegistro datetime default getdate()
 )

go

create table Usuario(
IdUsuario int primary key identity(1,1),
Nombre varchar(50),
Correo varchar(50),
Telefono varchar(50),
IdRol int references Rol(IdRol),
UrlFoto varchar(500),
NombreFoto varchar(100),
Clave varchar(100),
EsActivo bit,
FechaRegistro datetime default getdate()
)

go

create table Categoria(
IdCategoria int primary key identity(1,1),
Descripcion varchar(50),
EsActivo bit,
FechaRegistro datetime default getdate()
)

go

create table Producto(
IdProducto int primary key identity(1,1),
CodigoBarra varchar(50),
Marca varchar(50),
Descripcion varchar(100),
IdCategoria int references Categoria(IdCategoria),
Stock int,
UrlImagen varchar(500),
NombreImagen varchar(100),
Precio decimal(10,2),
EsActivo bit,
FechaRegistro datetime default getdate()
)

go

create table NumeroCorrelativo(
IdNumeroCorrelativo int primary key identity(1,1),
UltimoNumero int,
CantidadDigitos int,
Gestion varchar(100),
FechaActualizacion datetime
)

go

create table TipoDocumentoVenta(
IdTipoDocumentoVenta int primary key identity(1,1),
Descripcion varchar(50),
EsActivo bit,
FechaRegistro datetime default getdate()
)

go

create table Venta(
IdVenta int primary key identity(1,1),
NumeroVenta varchar(6),
IdTipoDocumentoVenta int references TipoDocumentoVenta(IdTipoDocumentoVenta),
IdUsuario int references Usuario(IdUsuario),
DocumentoCliente varchar(10),
NombreCliente varchar(20),
SubTotal decimal(10,2),
ImpuestoTotal decimal(10,2),
Total decimal(10,2),
FechaRegistro datetime default getdate()
)

go

create table DetalleVenta(
IdDetalleVenta int primary key identity(1,1),
IdVenta int references Venta(IdVenta),
IdProducto int,
MarcaProducto varchar(100),
DescripcionProducto varchar(100),
CategoriaProducto varchar(100),
Cantidad int,
Precio decimal(10,2),
Total decimal(10,2)
)

go

create table Negocio(
IdNegocio int primary key,
UrlLogo varchar(500),
NombreLogo varchar(100),
NumeroDocumento varchar(50),
Nombre varchar(50),
Correo varchar(50),
Direccion varchar(50),
Telefono varchar(50),
PorcentajeImpuesto decimal(10,2),
SimboloMoneda varchar(5)
)

go

create table Configuracion(
Recurso varchar(50),
Propiedad varchar(50),
Valor varchar(60)
)


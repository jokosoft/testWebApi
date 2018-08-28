
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 08/28/2018 23:21:32
-- Generated from EDMX file: C:\Users\jokosoft\OneDrive\MVC_5\testWebApi\ModeloPedidos\Clases\ModeloPedidos.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Pruebas];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_Pedidos_Personas]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Pedidos] DROP CONSTRAINT [FK_Pedidos_Personas];
GO
IF OBJECT_ID(N'[dbo].[FK_Pedidos_Restaurantes]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Pedidos] DROP CONSTRAINT [FK_Pedidos_Restaurantes];
GO
IF OBJECT_ID(N'[dbo].[FK_Productos_Familia]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Productos] DROP CONSTRAINT [FK_Productos_Familia];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductosPedidos_Pedidos]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductosPedidos] DROP CONSTRAINT [FK_ProductosPedidos_Pedidos];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductosPedidos_Productos]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductosPedidos] DROP CONSTRAINT [FK_ProductosPedidos_Productos];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Familia]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Familia];
GO
IF OBJECT_ID(N'[dbo].[Pedidos]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Pedidos];
GO
IF OBJECT_ID(N'[dbo].[Personas]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Personas];
GO
IF OBJECT_ID(N'[dbo].[Productos]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Productos];
GO
IF OBJECT_ID(N'[dbo].[ProductosPedidos]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProductosPedidos];
GO
IF OBJECT_ID(N'[dbo].[Restaurantes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Restaurantes];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Familia'
CREATE TABLE [dbo].[Familia] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nombre] nvarchar(100)  NOT NULL,
    [Descripcion] nvarchar(100)  NULL
);
GO

-- Creating table 'Pedidos'
CREATE TABLE [dbo].[Pedidos] (
    [Id_pedido] int IDENTITY(1,1) NOT NULL,
    [Referencia] nvarchar(150)  NOT NULL,
    [Fecha] datetime  NOT NULL,
    [FIdRestaurante] int  NOT NULL,
    [FIdPersona] int  NOT NULL
);
GO

-- Creating table 'Personas'
CREATE TABLE [dbo].[Personas] (
    [id] int IDENTITY(1,1) NOT NULL,
    [nombre] nvarchar(50)  NOT NULL,
    [edad] int  NOT NULL,
    [apellidos] nvarchar(150)  NULL
);
GO

-- Creating table 'Productos'
CREATE TABLE [dbo].[Productos] (
    [Id_prod] int IDENTITY(1,1) NOT NULL,
    [Nombre_prod] nvarchar(100)  NOT NULL,
    [FidFamilia] int  NOT NULL,
    [Precio] decimal(18,2)  NOT NULL
);
GO

-- Creating table 'ProductosPedidos'
CREATE TABLE [dbo].[ProductosPedidos] (
    [Id_productosPedido] int IDENTITY(1,1) NOT NULL,
    [FIdPedido] int  NOT NULL,
    [FIdProducto] int  NOT NULL,
    [Cantidad] int  NOT NULL
);
GO

-- Creating table 'Restaurantes'
CREATE TABLE [dbo].[Restaurantes] (
    [Id_Restaurante] int IDENTITY(1,1) NOT NULL,
    [Restaurante] nvarchar(150)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Familia'
ALTER TABLE [dbo].[Familia]
ADD CONSTRAINT [PK_Familia]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id_pedido] in table 'Pedidos'
ALTER TABLE [dbo].[Pedidos]
ADD CONSTRAINT [PK_Pedidos]
    PRIMARY KEY CLUSTERED ([Id_pedido] ASC);
GO

-- Creating primary key on [id] in table 'Personas'
ALTER TABLE [dbo].[Personas]
ADD CONSTRAINT [PK_Personas]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [Id_prod] in table 'Productos'
ALTER TABLE [dbo].[Productos]
ADD CONSTRAINT [PK_Productos]
    PRIMARY KEY CLUSTERED ([Id_prod] ASC);
GO

-- Creating primary key on [Id_productosPedido] in table 'ProductosPedidos'
ALTER TABLE [dbo].[ProductosPedidos]
ADD CONSTRAINT [PK_ProductosPedidos]
    PRIMARY KEY CLUSTERED ([Id_productosPedido] ASC);
GO

-- Creating primary key on [Id_Restaurante] in table 'Restaurantes'
ALTER TABLE [dbo].[Restaurantes]
ADD CONSTRAINT [PK_Restaurantes]
    PRIMARY KEY CLUSTERED ([Id_Restaurante] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [FidFamilia] in table 'Productos'
ALTER TABLE [dbo].[Productos]
ADD CONSTRAINT [FK_Productos_Familia]
    FOREIGN KEY ([FidFamilia])
    REFERENCES [dbo].[Familia]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Productos_Familia'
CREATE INDEX [IX_FK_Productos_Familia]
ON [dbo].[Productos]
    ([FidFamilia]);
GO

-- Creating foreign key on [FIdPersona] in table 'Pedidos'
ALTER TABLE [dbo].[Pedidos]
ADD CONSTRAINT [FK_Pedidos_Personas]
    FOREIGN KEY ([FIdPersona])
    REFERENCES [dbo].[Personas]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Pedidos_Personas'
CREATE INDEX [IX_FK_Pedidos_Personas]
ON [dbo].[Pedidos]
    ([FIdPersona]);
GO

-- Creating foreign key on [FIdRestaurante] in table 'Pedidos'
ALTER TABLE [dbo].[Pedidos]
ADD CONSTRAINT [FK_Pedidos_Restaurantes]
    FOREIGN KEY ([FIdRestaurante])
    REFERENCES [dbo].[Restaurantes]
        ([Id_Restaurante])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Pedidos_Restaurantes'
CREATE INDEX [IX_FK_Pedidos_Restaurantes]
ON [dbo].[Pedidos]
    ([FIdRestaurante]);
GO

-- Creating foreign key on [FIdPedido] in table 'ProductosPedidos'
ALTER TABLE [dbo].[ProductosPedidos]
ADD CONSTRAINT [FK_ProductosPedidos_Pedidos]
    FOREIGN KEY ([FIdPedido])
    REFERENCES [dbo].[Pedidos]
        ([Id_pedido])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductosPedidos_Pedidos'
CREATE INDEX [IX_FK_ProductosPedidos_Pedidos]
ON [dbo].[ProductosPedidos]
    ([FIdPedido]);
GO

-- Creating foreign key on [FIdProducto] in table 'ProductosPedidos'
ALTER TABLE [dbo].[ProductosPedidos]
ADD CONSTRAINT [FK_ProductosPedidos_Productos]
    FOREIGN KEY ([FIdProducto])
    REFERENCES [dbo].[Productos]
        ([Id_prod])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductosPedidos_Productos'
CREATE INDEX [IX_FK_ProductosPedidos_Productos]
ON [dbo].[ProductosPedidos]
    ([FIdProducto]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------
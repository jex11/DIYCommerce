
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 02/02/2015 11:41:55
-- Generated from EDMX file: E:\My Workspace\Visual Studio\Development\Razor\ECWebApp\ECWebApp.Domain\ECWebModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [ECWebSec];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[Customer].[FK__Customer__Custom__6D9742D9]', 'F') IS NOT NULL
    ALTER TABLE [Customer].[Customer] DROP CONSTRAINT [FK__Customer__Custom__6D9742D9];
GO
IF OBJECT_ID(N'[Customer].[FK__EmailConf__Custo__62E4AA3C]', 'F') IS NOT NULL
    ALTER TABLE [Customer].[EmailConfirm] DROP CONSTRAINT [FK__EmailConf__Custo__62E4AA3C];
GO
IF OBJECT_ID(N'[Customer].[FK__LastAcces__Custo__2116E6DF]', 'F') IS NOT NULL
    ALTER TABLE [Customer].[LastAccess] DROP CONSTRAINT [FK__LastAcces__Custo__2116E6DF];
GO
IF OBJECT_ID(N'[Customer].[FK_Cart_Customer]', 'F') IS NOT NULL
    ALTER TABLE [Customer].[Cart] DROP CONSTRAINT [FK_Cart_Customer];
GO
IF OBJECT_ID(N'[Customer].[FK_Customer_Customer_Address]', 'F') IS NOT NULL
    ALTER TABLE [Customer].[Address] DROP CONSTRAINT [FK_Customer_Customer_Address];
GO
IF OBJECT_ID(N'[Customer].[FK_Customer_Customer_RoleAssign]', 'F') IS NOT NULL
    ALTER TABLE [Customer].[RoleAssign] DROP CONSTRAINT [FK_Customer_Customer_RoleAssign];
GO
IF OBJECT_ID(N'[Customer].[FK_Product_CartList_Cart]', 'F') IS NOT NULL
    ALTER TABLE [Customer].[CartList] DROP CONSTRAINT [FK_Product_CartList_Cart];
GO
IF OBJECT_ID(N'[Customer].[FK_Product_CartList_Color]', 'F') IS NOT NULL
    ALTER TABLE [Customer].[CartList] DROP CONSTRAINT [FK_Product_CartList_Color];
GO
IF OBJECT_ID(N'[Product].[FK_Product_Color_Product]', 'F') IS NOT NULL
    ALTER TABLE [Product].[Color] DROP CONSTRAINT [FK_Product_Color_Product];
GO
IF OBJECT_ID(N'[Product].[FK_Product_Image_Product]', 'F') IS NOT NULL
    ALTER TABLE [Product].[Image] DROP CONSTRAINT [FK_Product_Image_Product];
GO
IF OBJECT_ID(N'[Product].[FK_Product_Product_Category]', 'F') IS NOT NULL
    ALTER TABLE [Product].[Product] DROP CONSTRAINT [FK_Product_Product_Category];
GO
IF OBJECT_ID(N'[Product].[FK_Product_Promotion]', 'F') IS NOT NULL
    ALTER TABLE [Product].[Product] DROP CONSTRAINT [FK_Product_Promotion];
GO
IF OBJECT_ID(N'[Product].[FK_ProductFolder_Folder]', 'F') IS NOT NULL
    ALTER TABLE [Product].[Product] DROP CONSTRAINT [FK_ProductFolder_Folder];
GO
IF OBJECT_ID(N'[Product].[FK_ProductReview_Customer]', 'F') IS NOT NULL
    ALTER TABLE [Product].[Review] DROP CONSTRAINT [FK_ProductReview_Customer];
GO
IF OBJECT_ID(N'[Product].[FK_ProductReview_Product_MasterProduct]', 'F') IS NOT NULL
    ALTER TABLE [Product].[Review] DROP CONSTRAINT [FK_ProductReview_Product_MasterProduct];
GO
IF OBJECT_ID(N'[Customer].[FK_RoleAssign_Role]', 'F') IS NOT NULL
    ALTER TABLE [Customer].[RoleAssign] DROP CONSTRAINT [FK_RoleAssign_Role];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[Customer].[Address]', 'U') IS NOT NULL
    DROP TABLE [Customer].[Address];
GO
IF OBJECT_ID(N'[Customer].[Cart]', 'U') IS NOT NULL
    DROP TABLE [Customer].[Cart];
GO
IF OBJECT_ID(N'[Customer].[CartList]', 'U') IS NOT NULL
    DROP TABLE [Customer].[CartList];
GO
IF OBJECT_ID(N'[Customer].[Class]', 'U') IS NOT NULL
    DROP TABLE [Customer].[Class];
GO
IF OBJECT_ID(N'[Customer].[Customer]', 'U') IS NOT NULL
    DROP TABLE [Customer].[Customer];
GO
IF OBJECT_ID(N'[Customer].[EmailConfirm]', 'U') IS NOT NULL
    DROP TABLE [Customer].[EmailConfirm];
GO
IF OBJECT_ID(N'[Customer].[LastAccess]', 'U') IS NOT NULL
    DROP TABLE [Customer].[LastAccess];
GO
IF OBJECT_ID(N'[Customer].[Role]', 'U') IS NOT NULL
    DROP TABLE [Customer].[Role];
GO
IF OBJECT_ID(N'[Customer].[RoleAssign]', 'U') IS NOT NULL
    DROP TABLE [Customer].[RoleAssign];
GO
IF OBJECT_ID(N'[dbo].[__MigrationHistory]', 'U') IS NOT NULL
    DROP TABLE [dbo].[__MigrationHistory];
GO
IF OBJECT_ID(N'[dbo].[sysdiagrams]', 'U') IS NOT NULL
    DROP TABLE [dbo].[sysdiagrams];
GO
IF OBJECT_ID(N'[Location].[Country]', 'U') IS NOT NULL
    DROP TABLE [Location].[Country];
GO
IF OBJECT_ID(N'[Location].[Region]', 'U') IS NOT NULL
    DROP TABLE [Location].[Region];
GO
IF OBJECT_ID(N'[Location].[SubRegion]', 'U') IS NOT NULL
    DROP TABLE [Location].[SubRegion];
GO
IF OBJECT_ID(N'[Product].[Category]', 'U') IS NOT NULL
    DROP TABLE [Product].[Category];
GO
IF OBJECT_ID(N'[Product].[Color]', 'U') IS NOT NULL
    DROP TABLE [Product].[Color];
GO
IF OBJECT_ID(N'[Product].[Folder]', 'U') IS NOT NULL
    DROP TABLE [Product].[Folder];
GO
IF OBJECT_ID(N'[Product].[Image]', 'U') IS NOT NULL
    DROP TABLE [Product].[Image];
GO
IF OBJECT_ID(N'[Product].[Product]', 'U') IS NOT NULL
    DROP TABLE [Product].[Product];
GO
IF OBJECT_ID(N'[Product].[Promotion]', 'U') IS NOT NULL
    DROP TABLE [Product].[Promotion];
GO
IF OBJECT_ID(N'[Product].[Review]', 'U') IS NOT NULL
    DROP TABLE [Product].[Review];
GO
IF OBJECT_ID(N'[ECWebAppModelStoreContainer].[vw_CustomerAuthentication]', 'U') IS NOT NULL
    DROP TABLE [ECWebAppModelStoreContainer].[vw_CustomerAuthentication];
GO
IF OBJECT_ID(N'[ECWebAppModelStoreContainer].[vw_OrderHistory]', 'U') IS NOT NULL
    DROP TABLE [ECWebAppModelStoreContainer].[vw_OrderHistory];
GO
IF OBJECT_ID(N'[ECWebAppModelStoreContainer].[vw_ProductList]', 'U') IS NOT NULL
    DROP TABLE [ECWebAppModelStoreContainer].[vw_ProductList];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Addresses'
CREATE TABLE [dbo].[Addresses] (
    [AddressId] uniqueidentifier  NOT NULL,
    [CustomerId] uniqueidentifier  NOT NULL,
    [Name] nvarchar(255)  NOT NULL,
    [Address1] nvarchar(255)  NOT NULL,
    [Postcode] nvarchar(20)  NOT NULL,
    [City] nvarchar(20)  NULL,
    [State] nvarchar(20)  NOT NULL,
    [Country] nvarchar(20)  NOT NULL,
    [Contact] nvarchar(20)  NULL,
    [Type] int  NOT NULL,
    [AddressCreatedOn] datetime  NULL,
    [AddressCreatedBy] nvarchar(255)  NULL,
    [AddressUpdatedOn] datetime  NULL,
    [AddressUpdatedBy] nvarchar(255)  NULL,
    [Status] int  NULL
);
GO

-- Creating table 'Carts'
CREATE TABLE [dbo].[Carts] (
    [CartID] uniqueidentifier  NOT NULL,
    [CustomerID] uniqueidentifier  NOT NULL,
    [CartStatus] int  NOT NULL,
    [PaymentMethod] nvarchar(20)  NULL,
    [PaymentTotal] decimal(19,4)  NULL,
    [PaymentStatus] int  NULL,
    [PaymentCreatedOn] datetime  NULL,
    [PaymentCreatedBy] nvarchar(255)  NULL,
    [CartCreatedOn] datetime  NOT NULL,
    [CartCreatedBy] nvarchar(255)  NOT NULL,
    [CartUpdatedOn] datetime  NULL,
    [CartUpdatedBy] nvarchar(255)  NULL
);
GO

-- Creating table 'CartLists'
CREATE TABLE [dbo].[CartLists] (
    [CartListId] uniqueidentifier  NOT NULL,
    [CartId] uniqueidentifier  NOT NULL,
    [ProductId] uniqueidentifier  NOT NULL,
    [ProductColor] nvarchar(20)  NOT NULL,
    [CartListTotalQuantity] int  NOT NULL,
    [CartListTotalPrice] decimal(19,4)  NOT NULL,
    [CartListTotalWeight] int  NOT NULL,
    [CartListCreatedOn] datetime  NOT NULL,
    [CartListCreatedBy] nvarchar(255)  NOT NULL,
    [CartListUpdatedOn] datetime  NULL,
    [CartListUpdatedBy] nvarchar(255)  NULL
);
GO

-- Creating table 'Classes'
CREATE TABLE [dbo].[Classes] (
    [CustomerCategoryId] uniqueidentifier  NOT NULL,
    [CustomerCategoryName] nvarchar(20)  NOT NULL,
    [CustomerCategoryPointGiven] int  NOT NULL,
    [CustomerCategoryStatus] int  NOT NULL,
    [CustomerCategoryCreatedOn] datetime  NOT NULL,
    [CustomerCategoryCreatedBy] nvarchar(255)  NOT NULL,
    [CustomerCategoryUpdatedOn] datetime  NULL,
    [CustomerCategoryUpdatedBy] nvarchar(255)  NULL
);
GO

-- Creating table 'Customers'
CREATE TABLE [dbo].[Customers] (
    [CustomerID] uniqueidentifier  NOT NULL,
    [CustomerCode] int  NULL,
    [CustomerImg] varbinary(max)  NULL,
    [CustomerPassword] nvarchar(20)  NULL,
    [CustomerFirstName] nvarchar(20)  NULL,
    [CustomerLastName] nvarchar(20)  NULL,
    [CustomerNRIC] nvarchar(20)  NULL,
    [CustomerEmail] nvarchar(255)  NULL,
    [CustomerAddress] nvarchar(255)  NULL,
    [CustomerPostcode] nvarchar(20)  NULL,
    [CustomerCity] nvarchar(20)  NULL,
    [CustomerState] nvarchar(20)  NULL,
    [CustomerCountry] nvarchar(20)  NULL,
    [CustomerContact] nvarchar(20)  NULL,
    [CustomerPoint] nvarchar(20)  NULL,
    [CustomerCategoryID] uniqueidentifier  NOT NULL,
    [CustomerStatus] int  NULL,
    [CustomerCreatedOn] datetime  NULL,
    [CustomerCreatedBy] nvarchar(255)  NULL,
    [CustomerUpdatedOn] datetime  NULL,
    [CustomerUpdatedBy] nvarchar(255)  NULL
);
GO

-- Creating table 'LastAccesses'
CREATE TABLE [dbo].[LastAccesses] (
    [LastAccessId] uniqueidentifier  NOT NULL,
    [LastAccessLocation] nvarchar(255)  NOT NULL,
    [LastAccessDevice] nvarchar(255)  NOT NULL,
    [CustomerId] uniqueidentifier  NOT NULL,
    [LastAccessCreatedOn] datetime  NULL,
    [LastAccessCreatedBy] nvarchar(255)  NULL,
    [LastAccessUpdatedOn] datetime  NULL,
    [LastAccessUpdatedBy] nvarchar(255)  NULL
);
GO

-- Creating table 'Roles'
CREATE TABLE [dbo].[Roles] (
    [RoleId] uniqueidentifier  NOT NULL,
    [RoleName] nvarchar(50)  NOT NULL,
    [RoleDescription] nvarchar(50)  NOT NULL,
    [RoleStatus] int  NOT NULL,
    [RoleCreatedOn] datetime  NOT NULL,
    [RoleCreatedBy] nvarchar(255)  NOT NULL,
    [RoleUpdatedOn] datetime  NULL,
    [RoleUpdatedBy] nvarchar(255)  NULL
);
GO

-- Creating table 'RoleAssigns'
CREATE TABLE [dbo].[RoleAssigns] (
    [RoleAssignId] uniqueidentifier  NOT NULL,
    [CustomerId] uniqueidentifier  NOT NULL,
    [RoleId] uniqueidentifier  NOT NULL,
    [RoleAssignCreatedOn] datetime  NOT NULL,
    [RoleAssignCreatedBy] nvarchar(255)  NOT NULL,
    [RoleAssignUpdatedOn] datetime  NULL,
    [RoleAssignUpdatedBy] nvarchar(255)  NULL
);
GO

-- Creating table 'C__MigrationHistory'
CREATE TABLE [dbo].[C__MigrationHistory] (
    [MigrationID] nvarchar(150)  NOT NULL,
    [ContextKey] nvarchar(300)  NOT NULL,
    [Model] varbinary(max)  NOT NULL,
    [ProductVersion] nvarchar(32)  NOT NULL
);
GO

-- Creating table 'sysdiagrams'
CREATE TABLE [dbo].[sysdiagrams] (
    [name] nvarchar(128)  NOT NULL,
    [principal_id] int  NOT NULL,
    [diagram_id] int IDENTITY(1,1) NOT NULL,
    [version] int  NULL,
    [definition] varbinary(max)  NULL
);
GO

-- Creating table 'Categories'
CREATE TABLE [dbo].[Categories] (
    [ProductCategory] int  NOT NULL,
    [CategoryName] nvarchar(20)  NULL,
    [CategoryDescription] nvarchar(50)  NULL,
    [CategoryStatus] int  NOT NULL,
    [CategoryCreateOn] datetime  NOT NULL,
    [CategoryCreatedBy] nvarchar(255)  NOT NULL,
    [CategoryUpdatedOn] datetime  NULL,
    [CategoryUpdatedBy] nvarchar(255)  NULL
);
GO

-- Creating table 'Colors'
CREATE TABLE [dbo].[Colors] (
    [ProductId] uniqueidentifier  NOT NULL,
    [ProductColor] nvarchar(20)  NOT NULL,
    [ProductQuantity] int  NULL,
    [IsOutOfStock] int  NULL,
    [ProductColorCreatedOn] datetime  NOT NULL,
    [ProductColorCreatedBy] nvarchar(255)  NOT NULL,
    [ProductColorUpdatedOn] datetime  NULL,
    [ProductColorUpdatedBy] nvarchar(255)  NULL
);
GO

-- Creating table 'Folders'
CREATE TABLE [dbo].[Folders] (
    [ProductFolderId] uniqueidentifier  NOT NULL,
    [ProductFolderName] nvarchar(100)  NOT NULL,
    [ProductFolderDescription] nvarchar(100)  NULL,
    [ProductFolderCreatedOn] datetime  NOT NULL,
    [ProductFolderCreatedBy] nvarchar(255)  NOT NULL,
    [ProductFolderUpdatedOn] datetime  NULL,
    [ProductFolderUpdatedBy] nvarchar(255)  NULL,
    [ProductFolderFrom] uniqueidentifier  NULL
);
GO

-- Creating table 'Images'
CREATE TABLE [dbo].[Images] (
    [ProductImgId] uniqueidentifier  NOT NULL,
    [ProductId] uniqueidentifier  NOT NULL,
    [ProductColor] nvarchar(20)  NOT NULL,
    [ProductImg] varbinary(max)  NOT NULL,
    [ProductImageCreatedOn] datetime  NOT NULL,
    [ProductImageCreatedBy] nvarchar(255)  NOT NULL,
    [ProductImageUpdatedOn] datetime  NULL,
    [ProductImageUpdatedBy] nvarchar(255)  NULL
);
GO

-- Creating table 'Products'
CREATE TABLE [dbo].[Products] (
    [ProductId] uniqueidentifier  NOT NULL,
    [ProductCode] nvarchar(20)  NOT NULL,
    [ProductName] nvarchar(20)  NOT NULL,
    [ProductCategory] int  NOT NULL,
    [ProductQuantity] int  NOT NULL,
    [ProductOriginalPrice] decimal(19,4)  NULL,
    [ProductRetailPrice] decimal(19,4)  NULL,
    [IsOutOfStock] int  NULL,
    [ProductDescription] nvarchar(50)  NULL,
    [ProductWeight] int  NULL,
    [ProductWidth] decimal(18,0)  NULL,
    [ProductHeight] decimal(18,0)  NULL,
    [ProductLength] decimal(18,0)  NULL,
    [ProductScale] nvarchar(20)  NULL,
    [PromotionId] uniqueidentifier  NULL,
    [ProductFolderId] uniqueidentifier  NULL,
    [ProductStatus] int  NOT NULL,
    [ProductCreatedOn] datetime  NOT NULL,
    [ProductCreatedBy] nvarchar(255)  NOT NULL,
    [ProductUpdatedOn] datetime  NULL,
    [ProductUpdatedBy] nvarchar(255)  NULL
);
GO

-- Creating table 'Promotions'
CREATE TABLE [dbo].[Promotions] (
    [PromotionId] uniqueidentifier  NOT NULL,
    [PromotionName] nvarchar(20)  NOT NULL,
    [PromotionDescription] nvarchar(50)  NOT NULL,
    [PromotionImage] varbinary(max)  NOT NULL,
    [PromotionBanner] nvarchar(50)  NOT NULL,
    [PromotionDiscountRate] int  NOT NULL,
    [PromotionStart] datetime  NULL,
    [PromotionEnd] datetime  NULL,
    [PromotionStatus] int  NOT NULL,
    [PromotionCreatedOn] datetime  NOT NULL,
    [PromotionCreatedBy] nvarchar(255)  NOT NULL,
    [PromotionUpdatedOn] datetime  NULL,
    [PromotionUpdatedBy] nvarchar(255)  NULL
);
GO

-- Creating table 'Reviews'
CREATE TABLE [dbo].[Reviews] (
    [ProductReviewId] uniqueidentifier  NOT NULL,
    [CustomerID] uniqueidentifier  NOT NULL,
    [ProductID] uniqueidentifier  NOT NULL,
    [ProductReview] nvarchar(100)  NOT NULL,
    [ProductReviewRate] int  NOT NULL,
    [ProductReviewCreatedOn] datetime  NOT NULL,
    [ProductReviewCreatedBy] nvarchar(255)  NOT NULL,
    [ProductReviewUpdatedOn] datetime  NULL,
    [ProductReviewUpdatedBy] nvarchar(255)  NULL
);
GO

-- Creating table 'Countries'
CREATE TABLE [dbo].[Countries] (
    [SCC_FIPS] nvarchar(255)  NOT NULL,
    [CC_ISO] nvarchar(255)  NULL,
    [TLD] nvarchar(255)  NULL,
    [COUNTRY_NAME] nvarchar(255)  NULL
);
GO

-- Creating table 'Regions'
CREATE TABLE [dbo].[Regions] (
    [REGION1] int  NOT NULL,
    [REGION_NAME] nvarchar(255)  NULL
);
GO

-- Creating table 'SubRegions'
CREATE TABLE [dbo].[SubRegions] (
    [SUBREGION1] nvarchar(2)  NOT NULL,
    [SUBREGION_NAME] nvarchar(255)  NULL
);
GO

-- Creating table 'vw_ProductList'
CREATE TABLE [dbo].[vw_ProductList] (
    [ProductId] uniqueidentifier  NOT NULL,
    [ProductCode] nvarchar(20)  NOT NULL,
    [ProductName] nvarchar(20)  NOT NULL,
    [ProductDescription] nvarchar(50)  NULL,
    [ProductColor] nvarchar(20)  NOT NULL,
    [ProductQuantity] int  NULL,
    [IsOutOfStock] int  NULL,
    [ProductImg] varbinary(max)  NOT NULL,
    [CategoryName] nvarchar(20)  NULL,
    [ProductFolderName] nvarchar(100)  NOT NULL
);
GO

-- Creating table 'vw_CustomerAuthentication'
CREATE TABLE [dbo].[vw_CustomerAuthentication] (
    [CustomerID] uniqueidentifier  NOT NULL,
    [RoleName] nvarchar(50)  NOT NULL,
    [CustomerPassword] nvarchar(20)  NULL,
    [CustomerEmail] nvarchar(255)  NULL,
    [CustomerStatus] int  NULL,
    [RoleStatus] int  NOT NULL,
    [LastAccessLocation] nvarchar(255)  NOT NULL,
    [LastAccessDevice] nvarchar(255)  NOT NULL
);
GO

-- Creating table 'vw_OrderHistory'
CREATE TABLE [dbo].[vw_OrderHistory] (
    [CartID] uniqueidentifier  NOT NULL,
    [CustomerID] uniqueidentifier  NOT NULL,
    [CartStatus] int  NOT NULL,
    [PaymentMethod] nvarchar(20)  NULL,
    [PaymentTotal] decimal(19,4)  NULL,
    [PaymentStatus] int  NULL,
    [ProductId] uniqueidentifier  NOT NULL,
    [ProductColor] nvarchar(20)  NOT NULL,
    [CartListTotalQuantity] int  NOT NULL,
    [CartListTotalPrice] decimal(19,4)  NOT NULL,
    [CartListTotalWeight] int  NOT NULL,
    [PaymentCreatedOn] datetime  NULL,
    [PaymentCreatedBy] nvarchar(255)  NULL
);
GO

-- Creating table 'EmailConfirms'
CREATE TABLE [dbo].[EmailConfirms] (
    [EmailConfirmId] uniqueidentifier  NOT NULL,
    [CustomerID] uniqueidentifier  NULL,
    [CustomerEmail] nvarchar(255)  NULL,
    [ConfirmationCode] nvarchar(32)  NULL,
    [ConfirmationStatus] int  NULL,
    [EmailConfirmCreatedOn] datetime  NULL,
    [EmailConfirmEndOn] datetime  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [AddressId] in table 'Addresses'
ALTER TABLE [dbo].[Addresses]
ADD CONSTRAINT [PK_Addresses]
    PRIMARY KEY CLUSTERED ([AddressId] ASC);
GO

-- Creating primary key on [CartID] in table 'Carts'
ALTER TABLE [dbo].[Carts]
ADD CONSTRAINT [PK_Carts]
    PRIMARY KEY CLUSTERED ([CartID] ASC);
GO

-- Creating primary key on [CartListId] in table 'CartLists'
ALTER TABLE [dbo].[CartLists]
ADD CONSTRAINT [PK_CartLists]
    PRIMARY KEY CLUSTERED ([CartListId] ASC);
GO

-- Creating primary key on [CustomerCategoryId] in table 'Classes'
ALTER TABLE [dbo].[Classes]
ADD CONSTRAINT [PK_Classes]
    PRIMARY KEY CLUSTERED ([CustomerCategoryId] ASC);
GO

-- Creating primary key on [CustomerID] in table 'Customers'
ALTER TABLE [dbo].[Customers]
ADD CONSTRAINT [PK_Customers]
    PRIMARY KEY CLUSTERED ([CustomerID] ASC);
GO

-- Creating primary key on [LastAccessId] in table 'LastAccesses'
ALTER TABLE [dbo].[LastAccesses]
ADD CONSTRAINT [PK_LastAccesses]
    PRIMARY KEY CLUSTERED ([LastAccessId] ASC);
GO

-- Creating primary key on [RoleId] in table 'Roles'
ALTER TABLE [dbo].[Roles]
ADD CONSTRAINT [PK_Roles]
    PRIMARY KEY CLUSTERED ([RoleId] ASC);
GO

-- Creating primary key on [RoleAssignId] in table 'RoleAssigns'
ALTER TABLE [dbo].[RoleAssigns]
ADD CONSTRAINT [PK_RoleAssigns]
    PRIMARY KEY CLUSTERED ([RoleAssignId] ASC);
GO

-- Creating primary key on [MigrationID], [ContextKey] in table 'C__MigrationHistory'
ALTER TABLE [dbo].[C__MigrationHistory]
ADD CONSTRAINT [PK_C__MigrationHistory]
    PRIMARY KEY CLUSTERED ([MigrationID], [ContextKey] ASC);
GO

-- Creating primary key on [diagram_id] in table 'sysdiagrams'
ALTER TABLE [dbo].[sysdiagrams]
ADD CONSTRAINT [PK_sysdiagrams]
    PRIMARY KEY CLUSTERED ([diagram_id] ASC);
GO

-- Creating primary key on [ProductCategory] in table 'Categories'
ALTER TABLE [dbo].[Categories]
ADD CONSTRAINT [PK_Categories]
    PRIMARY KEY CLUSTERED ([ProductCategory] ASC);
GO

-- Creating primary key on [ProductId], [ProductColor] in table 'Colors'
ALTER TABLE [dbo].[Colors]
ADD CONSTRAINT [PK_Colors]
    PRIMARY KEY CLUSTERED ([ProductId], [ProductColor] ASC);
GO

-- Creating primary key on [ProductFolderId] in table 'Folders'
ALTER TABLE [dbo].[Folders]
ADD CONSTRAINT [PK_Folders]
    PRIMARY KEY CLUSTERED ([ProductFolderId] ASC);
GO

-- Creating primary key on [ProductImgId] in table 'Images'
ALTER TABLE [dbo].[Images]
ADD CONSTRAINT [PK_Images]
    PRIMARY KEY CLUSTERED ([ProductImgId] ASC);
GO

-- Creating primary key on [ProductId] in table 'Products'
ALTER TABLE [dbo].[Products]
ADD CONSTRAINT [PK_Products]
    PRIMARY KEY CLUSTERED ([ProductId] ASC);
GO

-- Creating primary key on [PromotionId] in table 'Promotions'
ALTER TABLE [dbo].[Promotions]
ADD CONSTRAINT [PK_Promotions]
    PRIMARY KEY CLUSTERED ([PromotionId] ASC);
GO

-- Creating primary key on [ProductReviewId] in table 'Reviews'
ALTER TABLE [dbo].[Reviews]
ADD CONSTRAINT [PK_Reviews]
    PRIMARY KEY CLUSTERED ([ProductReviewId] ASC);
GO

-- Creating primary key on [SCC_FIPS] in table 'Countries'
ALTER TABLE [dbo].[Countries]
ADD CONSTRAINT [PK_Countries]
    PRIMARY KEY CLUSTERED ([SCC_FIPS] ASC);
GO

-- Creating primary key on [REGION1] in table 'Regions'
ALTER TABLE [dbo].[Regions]
ADD CONSTRAINT [PK_Regions]
    PRIMARY KEY CLUSTERED ([REGION1] ASC);
GO

-- Creating primary key on [SUBREGION1] in table 'SubRegions'
ALTER TABLE [dbo].[SubRegions]
ADD CONSTRAINT [PK_SubRegions]
    PRIMARY KEY CLUSTERED ([SUBREGION1] ASC);
GO

-- Creating primary key on [ProductId] in table 'vw_ProductList'
ALTER TABLE [dbo].[vw_ProductList]
ADD CONSTRAINT [PK_vw_ProductList]
    PRIMARY KEY CLUSTERED ([ProductId] ASC);
GO

-- Creating primary key on [CustomerID], [RoleName], [RoleStatus], [LastAccessLocation], [LastAccessDevice] in table 'vw_CustomerAuthentication'
ALTER TABLE [dbo].[vw_CustomerAuthentication]
ADD CONSTRAINT [PK_vw_CustomerAuthentication]
    PRIMARY KEY CLUSTERED ([CustomerID], [RoleName], [RoleStatus], [LastAccessLocation], [LastAccessDevice] ASC);
GO

-- Creating primary key on [CartID], [CustomerID], [CartStatus], [ProductId], [ProductColor], [CartListTotalQuantity], [CartListTotalPrice], [CartListTotalWeight] in table 'vw_OrderHistory'
ALTER TABLE [dbo].[vw_OrderHistory]
ADD CONSTRAINT [PK_vw_OrderHistory]
    PRIMARY KEY CLUSTERED ([CartID], [CustomerID], [CartStatus], [ProductId], [ProductColor], [CartListTotalQuantity], [CartListTotalPrice], [CartListTotalWeight] ASC);
GO

-- Creating primary key on [EmailConfirmId] in table 'EmailConfirms'
ALTER TABLE [dbo].[EmailConfirms]
ADD CONSTRAINT [PK_EmailConfirms]
    PRIMARY KEY CLUSTERED ([EmailConfirmId] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [CustomerId] in table 'Addresses'
ALTER TABLE [dbo].[Addresses]
ADD CONSTRAINT [FK_Customer_Customer_Address]
    FOREIGN KEY ([CustomerId])
    REFERENCES [dbo].[Customers]
        ([CustomerID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Customer_Customer_Address'
CREATE INDEX [IX_FK_Customer_Customer_Address]
ON [dbo].[Addresses]
    ([CustomerId]);
GO

-- Creating foreign key on [CustomerID] in table 'Carts'
ALTER TABLE [dbo].[Carts]
ADD CONSTRAINT [FK_Cart_Customer]
    FOREIGN KEY ([CustomerID])
    REFERENCES [dbo].[Customers]
        ([CustomerID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Cart_Customer'
CREATE INDEX [IX_FK_Cart_Customer]
ON [dbo].[Carts]
    ([CustomerID]);
GO

-- Creating foreign key on [CartId] in table 'CartLists'
ALTER TABLE [dbo].[CartLists]
ADD CONSTRAINT [FK_Product_CartList_Cart]
    FOREIGN KEY ([CartId])
    REFERENCES [dbo].[Carts]
        ([CartID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Product_CartList_Cart'
CREATE INDEX [IX_FK_Product_CartList_Cart]
ON [dbo].[CartLists]
    ([CartId]);
GO

-- Creating foreign key on [ProductId], [ProductColor] in table 'CartLists'
ALTER TABLE [dbo].[CartLists]
ADD CONSTRAINT [FK_Product_CartList_Color]
    FOREIGN KEY ([ProductId], [ProductColor])
    REFERENCES [dbo].[Colors]
        ([ProductId], [ProductColor])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Product_CartList_Color'
CREATE INDEX [IX_FK_Product_CartList_Color]
ON [dbo].[CartLists]
    ([ProductId], [ProductColor]);
GO

-- Creating foreign key on [CustomerCategoryID] in table 'Customers'
ALTER TABLE [dbo].[Customers]
ADD CONSTRAINT [FK__Customer__Custom__6D9742D9]
    FOREIGN KEY ([CustomerCategoryID])
    REFERENCES [dbo].[Classes]
        ([CustomerCategoryId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__Customer__Custom__6D9742D9'
CREATE INDEX [IX_FK__Customer__Custom__6D9742D9]
ON [dbo].[Customers]
    ([CustomerCategoryID]);
GO

-- Creating foreign key on [CustomerId] in table 'LastAccesses'
ALTER TABLE [dbo].[LastAccesses]
ADD CONSTRAINT [FK__LastAcces__Custo__2116E6DF]
    FOREIGN KEY ([CustomerId])
    REFERENCES [dbo].[Customers]
        ([CustomerID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__LastAcces__Custo__2116E6DF'
CREATE INDEX [IX_FK__LastAcces__Custo__2116E6DF]
ON [dbo].[LastAccesses]
    ([CustomerId]);
GO

-- Creating foreign key on [CustomerId] in table 'RoleAssigns'
ALTER TABLE [dbo].[RoleAssigns]
ADD CONSTRAINT [FK_Customer_Customer_RoleAssign]
    FOREIGN KEY ([CustomerId])
    REFERENCES [dbo].[Customers]
        ([CustomerID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Customer_Customer_RoleAssign'
CREATE INDEX [IX_FK_Customer_Customer_RoleAssign]
ON [dbo].[RoleAssigns]
    ([CustomerId]);
GO

-- Creating foreign key on [CustomerID] in table 'Reviews'
ALTER TABLE [dbo].[Reviews]
ADD CONSTRAINT [FK_ProductReview_Customer]
    FOREIGN KEY ([CustomerID])
    REFERENCES [dbo].[Customers]
        ([CustomerID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductReview_Customer'
CREATE INDEX [IX_FK_ProductReview_Customer]
ON [dbo].[Reviews]
    ([CustomerID]);
GO

-- Creating foreign key on [RoleId] in table 'RoleAssigns'
ALTER TABLE [dbo].[RoleAssigns]
ADD CONSTRAINT [FK_RoleAssign_Role]
    FOREIGN KEY ([RoleId])
    REFERENCES [dbo].[Roles]
        ([RoleId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RoleAssign_Role'
CREATE INDEX [IX_FK_RoleAssign_Role]
ON [dbo].[RoleAssigns]
    ([RoleId]);
GO

-- Creating foreign key on [ProductCategory] in table 'Products'
ALTER TABLE [dbo].[Products]
ADD CONSTRAINT [FK_Product_Product_Category]
    FOREIGN KEY ([ProductCategory])
    REFERENCES [dbo].[Categories]
        ([ProductCategory])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Product_Product_Category'
CREATE INDEX [IX_FK_Product_Product_Category]
ON [dbo].[Products]
    ([ProductCategory]);
GO

-- Creating foreign key on [ProductId] in table 'Colors'
ALTER TABLE [dbo].[Colors]
ADD CONSTRAINT [FK_Product_Color_Product]
    FOREIGN KEY ([ProductId])
    REFERENCES [dbo].[Products]
        ([ProductId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [ProductId], [ProductColor] in table 'Images'
ALTER TABLE [dbo].[Images]
ADD CONSTRAINT [FK_Product_Image_Product]
    FOREIGN KEY ([ProductId], [ProductColor])
    REFERENCES [dbo].[Colors]
        ([ProductId], [ProductColor])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Product_Image_Product'
CREATE INDEX [IX_FK_Product_Image_Product]
ON [dbo].[Images]
    ([ProductId], [ProductColor]);
GO

-- Creating foreign key on [ProductFolderId] in table 'Products'
ALTER TABLE [dbo].[Products]
ADD CONSTRAINT [FK_ProductFolder_Folder]
    FOREIGN KEY ([ProductFolderId])
    REFERENCES [dbo].[Folders]
        ([ProductFolderId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductFolder_Folder'
CREATE INDEX [IX_FK_ProductFolder_Folder]
ON [dbo].[Products]
    ([ProductFolderId]);
GO

-- Creating foreign key on [PromotionId] in table 'Products'
ALTER TABLE [dbo].[Products]
ADD CONSTRAINT [FK_Product_Promotion]
    FOREIGN KEY ([PromotionId])
    REFERENCES [dbo].[Promotions]
        ([PromotionId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Product_Promotion'
CREATE INDEX [IX_FK_Product_Promotion]
ON [dbo].[Products]
    ([PromotionId]);
GO

-- Creating foreign key on [ProductID] in table 'Reviews'
ALTER TABLE [dbo].[Reviews]
ADD CONSTRAINT [FK_ProductReview_Product_MasterProduct]
    FOREIGN KEY ([ProductID])
    REFERENCES [dbo].[Products]
        ([ProductId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductReview_Product_MasterProduct'
CREATE INDEX [IX_FK_ProductReview_Product_MasterProduct]
ON [dbo].[Reviews]
    ([ProductID]);
GO

-- Creating foreign key on [CustomerID] in table 'EmailConfirms'
ALTER TABLE [dbo].[EmailConfirms]
ADD CONSTRAINT [FK__EmailConf__Custo__62E4AA3C]
    FOREIGN KEY ([CustomerID])
    REFERENCES [dbo].[Customers]
        ([CustomerID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__EmailConf__Custo__62E4AA3C'
CREATE INDEX [IX_FK__EmailConf__Custo__62E4AA3C]
ON [dbo].[EmailConfirms]
    ([CustomerID]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------
--USE dbQuanLiCuaHangLoa
--GO
--DROP DATABASE dbThuongMaiDienTu
--GO

CREATE DATABASE dbThuongMaiDienTu
GO

USE dbThuongMaiDienTu
GO

CREATE TABLE [dbo].[tb_Brand](
	[BrandID] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Name] [nvarchar](250) NOT NULL,
	[Avatar] [nvarchar](200) NULL 
)
GO

CREATE TABLE [dbo].[tb_Supplier](
	[SupplierID] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Name] [nvarchar](50) NULL,
	[Email] [nvarchar](50) NULL,
	[Phone] [nvarchar](50) NULL,
	[Address] [nvarchar](50) NULL
)
GO

CREATE TABLE [dbo].[tb_Role](
	[RoleID] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Name] [nvarchar](100) NULL
)
GO

CREATE TABLE [dbo].[tb_Customer](
	[CustomerID] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Username] [varchar](50) NULL,
	[Password] [varchar](50) NULL,
	[Name] [nvarchar](50) NULL,
	[Email] [nvarchar](50) NULL,
	[Phone] [nvarchar](50) NULL,
	[Gender] [nvarchar](10) NULL,
	[DayOfBirth] [datetime] DEFAULT (getdate()),
	[Address] [nvarchar](200) NULL,
	[Avatar] [nvarchar](200) NULL,
	[RoleID] [int] NULL DEFAULT ((1))

	FOREIGN KEY ([RoleID]) REFERENCES [dbo].[tb_Role]([RoleID])
		ON UPDATE CASCADE
		ON DELETE CASCADE
)
GO


CREATE TABLE [dbo].[tb_Address](
	[AddressID] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Address] [nvarchar](200) NULL,
	[CustomerID] [int]

	FOREIGN KEY ([CustomerID]) REFERENCES [dbo].[tb_Customer]([CustomerID])
		ON UPDATE CASCADE
		ON DELETE CASCADE
)
GO

CREATE TABLE [dbo].[tb_Shop](
	[ShopID] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Name] [nvarchar](100) NULL,
	[Address] [nvarchar](200) NULL,
	[Email] [nvarchar](50) NULL,
	[Phone] [nvarchar](50) NULL,
	[Avatar] [nvarchar](200) NULL,
	[Description] [nvarchar](250) NULL,
	[Detail] [ntext] NULL,
	[OwnerID] [int] NULL

    FOREIGN KEY ([OwnerID]) REFERENCES [dbo].[tb_Customer]([CustomerID])
		ON UPDATE CASCADE
		ON DELETE CASCADE
)
GO

CREATE TABLE [dbo].[tb_ProductCategory](
	[CateID] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Name] [nvarchar](250) NULL,
	[ParentID] [int] NULL,
	[Image] [nvarchar](200) NULL,
	[CreatedDate] [datetime] NULL DEFAULT (getdate())
)
GO

CREATE TABLE [dbo].[tb_Product](
	[ProductID] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Name] [nvarchar](250) NULL,
	[Image] [nvarchar](250) NULL,
	[Price] [decimal](18, 0) NULL DEFAULT ((0)),
	[PromotionPrice] [decimal](18, 0) NULL,
	[Quantity] [int] NULL DEFAULT ((0)),
	[Description] [nvarchar](500) NULL,
	[Detail] [ntext] NULL,
	[CateID] [int] NULL,
	[ShopID] [int] NOT NULL,
	[BrandID] [int] NULL,
	[SupplierID] [int] NULL,
	[CreatedDate] [datetime] NULL DEFAULT (getdate()),

	FOREIGN KEY ([CateID]) REFERENCES [dbo].[tb_ProductCategory]([CateID])
		ON UPDATE CASCADE
		ON DELETE CASCADE,
	FOREIGN KEY ([ShopID]) REFERENCES [dbo].[tb_Shop]([ShopID])
		ON UPDATE CASCADE
		ON DELETE CASCADE,
    FOREIGN KEY ([BrandID]) REFERENCES [dbo].[tb_Brand]([BrandID])
		ON UPDATE CASCADE
		ON DELETE CASCADE,
    FOREIGN KEY ([SupplierID]) REFERENCES [dbo].[tb_Supplier]([SupplierID])
		ON UPDATE CASCADE
		ON DELETE CASCADE
)
GO

CREATE TABLE [dbo].[tb_ListImage](
	[ImageID] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[SRC] [nvarchar](250) NULL,
	[ProductID] [int]

	FOREIGN KEY ([ProductID]) REFERENCES [dbo].[tb_Product]([ProductID])
		ON UPDATE CASCADE
		ON DELETE CASCADE
)
GO

CREATE TABLE [dbo].[tb_ProductComment](
	[CommentID] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Detail] [nvarchar](500) NULL,
	[Star] [int] CONSTRAINT CK_Star CHECK ([Star] BETWEEN 1 AND 5) DEFAULT 5,
	[ProductID] [int] NOT NULL,
	[CustomerID] [int] NOT NULL,
	[CreatedDate] [datetime] NULL DEFAULT (getdate()),

	FOREIGN KEY ([CustomerID]) REFERENCES [dbo].[tb_Customer]([CustomerID])
		ON UPDATE NO ACTION
		ON DELETE NO ACTION,
	FOREIGN KEY ([ProductID]) REFERENCES [dbo].[tb_Product]([ProductID])
		ON UPDATE CASCADE
		ON DELETE CASCADE
)
GO

CREATE TABLE [dbo].[tb_Cart](
	[CartID] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[CustomerID] [int],
	[ProductID] [int],
	[Image] [nvarchar](200) NULL, 
	[Price] [int] NULL DEFAULT ((0)),
	[Quantity] [int] NULL DEFAULT ((1)),

	FOREIGN KEY ([CustomerID]) REFERENCES [dbo].[tb_Customer]([CustomerID]),
	FOREIGN KEY ([ProductID]) REFERENCES [dbo].[tb_Product]([ProductID])
		ON UPDATE CASCADE
		ON DELETE CASCADE
)
GO

CREATE TABLE [dbo].[tb_Order](
	[OrderID] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[OrderDate] [datetime] NULL DEFAULT (getdate()),
	[Status] [nvarchar](50) NOT NULL DEFAULT (N'Đã đặt'),
	[Address] [nvarchar](200) NULL,
	[DeliveredDate] [datetime] NULL,
	[CustomerID] [int] NULL,
	[ShopID] [int] NULL,
	[Discount] [int] NULL

	FOREIGN KEY ([CustomerID]) REFERENCES [dbo].[tb_Customer]([CustomerID])
		ON UPDATE CASCADE
		ON DELETE CASCADE,
	FOREIGN KEY ([ShopID]) REFERENCES [dbo].[tb_Shop]([ShopID])
)
GO

CREATE TABLE [dbo].[tb_OrderDetail](
    [OrderDetailID] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [OrderID] [int] NULL,
    [ProductID] [int] NULL,
    [Price] [decimal](18, 0) Default (0),
    [Quantity] [int] Default (1),
	[Status] [nvarchar](50) NULL

    FOREIGN KEY ([ProductID]) REFERENCES [dbo].[tb_Product]([ProductID]),
    FOREIGN KEY ([OrderID]) REFERENCES [dbo].[tb_Order]([OrderID])
		ON UPDATE CASCADE
		ON DELETE CASCADE
)
GO


insert into tb_Brand
values
	(N'Sony',			'sony.jpg'), 
	(N'Bose',			'bose.jpg'), 
	(N'JBL',			'jbl.jpg'), 
	(N'Sennheiser',		'sennheiser.jpg'), 
	(N'AKG',			'akg	.jpg'), 
	(N'Audio-Technica', 'audiotechnica.jpg'), 
	(N'Bang & Olufsen', 'bangolufsen.jpg'), 
	(N'Pioneer',		'pioneer.jpg'),
	(N'Jamo',			'jamo.jpg'),
	(N'Numark',			'numark.jpg'),
	(N'Shure',			'shure.jpg'),
	(N'Jays',			'jays.jpg'),
	(N'Marshall',		'marshall.jpg'),
	(N'VietK',			'vietk.jpg'),
	(N'LG',				'lg.jpg'),
	(N'Nordost',		'nordost.jpg'),
	(N'Wharfedale',		'wharfedale.jpg'),
	(N'Nuforce',		'nuforce.jpg'),
	(N'Denon',			'denon.jpg')
GO

insert into tb_Role
values
	(N'Người dùng'), (N'Admin'), (N'Quản lí tài khoản'), (N'Quản lí sản phẩm')
GO

insert into tb_Customer([Username], [Password],	[Name], [Email], [Phone], [Gender], [DayOfBirth], [Address], [Avatar], [RoleID])
values
	('admin',	'123', N'Admin',					null, null, null, null, null, null, 2),
	('thien',	'123', N'Nguyễn Văn Thanh Thiện',	null, null, null, null, null, null, 1),
	('viet',	'123', N'Lê Xuân Việt',				null, null, null, null, null, null, 1),
	('thang',	'123', N'Đỗ Viết Thắng',			null, null, null, null, null, null, 1),
	('quy',		'123', N'Nguyễn Quang Quý',			null, null, null, null, null, null, 1),
	('minh',	'123', N'Đinh Công Minh',			null, null, null, null, null, null, 1)
GO

insert into tb_Shop([Name], [Avatar], [Description], [Detail], [OwnerID])
values
	(N'Loa.Store',			'shop1.jpg',	null, null, 1),
	(N'TDShop',				'shop2.jpg',	null, null, 2),
	(N'Value Choice Shop',	'shop3.jpg',	null, null, 3),
	(N'Poermax.Store',		'shop4.jpg',	null, null, 4)
GO

insert into tb_ProductCategory([Name],[ParentID],[Image])
values
	(N'Loa',		null, 'loa.png'),			
	(N'Tai nghe',	null, 'tainghe.png'),
	(N'Micro',		null, 'micro.png'), 
	(N'Ampli',		null, 'ampli.png'),		
	(N'Máy nghe nhạc', null, 'maynghenhac.png'),		
	(N'Bàn DJ', null, 'bandj.png'),	
	(N'Bàn Mixer',	null, 'mixer.png'),	
	(N'Phụ kiện âm thanh', null, 'phukien.png'),	
	(N'Nhạc cụ',	null, 'nhaccu.png'),
	(N'Loa âm trần',		1, 'loaamtran.png'), 
	(N'Loa bluetooth',		1, 'loabluetooth.png'), 
	(N'Loa treo tường',		1, 'loatreotuong.png'), 
	(N'Loa karaoke',		1, 'loakaraoke.png'), 
	(N'Earphone',			2, 'earphone.png'), 
	(N'Headphone',			2, 'headphone.png'),
	(N'Earphone có dây',	14,	'earphonecoday.png'), 
	(N'Earphone bluetooth', 14, 'earphonebluetooth.png'), 
	(N'Headphone có dây',	15,	'headphonecoday.png'), 
	(N'Headphone bluetooth', 15, 'headphonebluetooth.png'),
	(N'Micro không dây',	3, 'micropin.png'), 
	(N'Micro có dây',		3, 'microcoday.png'),	
	(N'Micro cổ ngổng',		3, 'microcongong.png'), 
	(N'Bàn DJ Pioneer',		6, 'bandjpioneer.png'),	
	(N'Bàn DJ Numark',		6, 'bandjnumark.png'),
	(N'Guitar',				9, 'guitar.png'),	
	(N'Violin',				9, 'violin.png')
GO


insert into tb_Product([Name],[Image],[Price],[PromotionPrice],[Quantity],[Description],[Detail],[CateID],[ShopID],[BrandID],[SupplierID])
values
	(N'Loa JBL Partybox Ultimate',	'1.jpg',	39900000, 33850000, 20, null, null,  1, 1, 3, null), 
	(N'Loa JBL STUDIO 665C',		'2.jpg',	22900000, 22900000, 20, null, null,  1, 1, 3, null), 
	(N'Loa JBL Control 31',			'3.jpg',	19990000, 19990000, 20, null, null,  1, 1, 3, null), 
	(N'Loa JBL Control X',			'4.jpg',	10900000, 10900000, 20, null, null,  1, 1, 3, null), 
	(N'Loa JBL Control 85M',		'5.jpg',	10000000,  8300000, 20, null, null,  1, 1, 3, null),
	(N'Tai nghe DareU EH416',		'6.jpg',	10000000,  8300000, 20, null, null,  2, 1, 3, null),
	(N'Ampli Marantz PM8006',		'7.jpg',	10000000,  8300000, 20, null, null,  4, 1, 3, null),
	(N'Bàn DJ Pioneer XDJ ',		'8.jpg',	10000000,  8300000, 20, null, null, 23, 1, 3, null),
	(N'Đàn Guitar TAYLOR 110E',		'9.jpg',	10000000,  8300000, 20, null, null, 25, 1, 3, null),
	(N'Máy nghe nhạc SONY NW',		'10.jpg',	10000000,  8300000, 20, null, null,  5, 1, 3, null),
	(N'Micro Không Dây JBL',		'11.jpg',	10000000,  8300000, 20, null, null, 20, 1, 3, null),
	(N'Bàn Mixer Yamaha MG12XU',	'12.jpg',	10000000,  8300000, 20, null, null,  7, 1, 3, null),
	(N'Máy nghe nhạc MP3 Wakama',	'13.jpg',	10000000,  8300000, 20, null, null,  5, 1, 3, null),
	(N'Tai nghe RKX 3.5 Gaming',	'14.jpg',	10000000,  8300000, 20, null, null, 18, 1, 3, null),
	(N'Loa JBL Partybox Encore',	'15.jpg',	10000000,  8300000, 20, null, null, 13, 1, 3, null),
	(N'LOA SPEAKER BOSE 101',		'16.jpg',	10000000,  8300000, 20, null, null, 12, 1, 3, null),
	(N'Tai Nghe SONY WH-CH520',		'17.jpg',	10000000,  8300000, 20, null, null, 19, 1, 3, null),
	(N'Bàn DJ Pioneer DDJ-FLX4',	'18.jpg',	10000000,  8300000, 20, null, null,  6, 1, 3, null),
	(N'Micro SENNHEISER E 845-S',	'19.jpg',	10000000,  8300000, 20, null, null, 21, 1, 3, null),
	(N'Ampli Marantz NR 1200',		'20.jpg',	10000000,  8300000, 20, null, null,  4, 1, 3, null)
GO

select * from tb_Brand
select * from tb_ProductCategory

insert into tb_ProductComment([Detail],[Star],[ProductID],[CustomerID])
values
	(N'Được',			default, 1, 1),
	(N'Được được',		default, 1, 1),
	(N'Được đấy',		default, 1, 1),
	(N'Được đấy chứ',	default, 2, 1),
	(N'Cũng được',		default, 2, 1)
GO

insert into tb_Cart([CustomerID],[ProductID],[Image],[Price],[Quantity])
values
	(2, 1, N'' ,33850000, 1),
	(2, 2, N'', 22900000, 1),
	(3, 1, N'', 33850000, 1),
	(3, 1, N'', 33850000, 1),
	(4, 1, N'', 33850000, 1)
GO

insert into tb_Order([Status],[Address],[CustomerID],[ShopID])
values
	(default, N'An Tân, xã Hoà Phong',	 1, 1),
	(default, N'An Tân, xã Hoà Phú',	 1, 1),
	(default, N'An Tân, xã Hoà Nhơn',	 1, 1),
	(default, N'An Tân, xã Hoà Khương',	 1, 1)
GO

insert into tb_OrderDetail([OrderID],[ProductID],[Price],[Quantity])
values
	(1, 1, 10000000,	2),
	(1, 2, 11000000,	1),
	(1, 3, 15000000,	1),
	(2, 1, 30000000,	3),
	(2, 2, 9000000,		5)
GO

-- Proc
-- Thêm sản phẩm
CREATE PROCEDURE procAddProduct
    @Name NVARCHAR(255),
    @Image VARBINARY(MAX),
    @ImageID INT,
    @Price DECIMAL(18, 2),
    @PromotionPrice DECIMAL(18, 2),
    @Quantity INT,
    @Description NVARCHAR(MAX),
    @Detail NVARCHAR(MAX),
    @CateID INT,
    @ShopID INT,
    @BrandID INT,
    @SupplierID INT
AS
BEGIN
    BEGIN TRANSACTION
    IF @Price >= 0 AND @PromotionPrice >= 0 AND @Quantity >= 0
    BEGIN
        INSERT INTO tb_Product([Name], [Image], [Price], [PromotionPrice], [Quantity], [Description], [Detail], [CateID], [ShopID], [BrandID], [SupplierID])
        VALUES (@Name, @Image, @Price, @PromotionPrice, @Quantity, @Description, @Detail, @CateID, @ShopID, @BrandID, @SupplierID)
        
        COMMIT TRANSACTION
    END
    ELSE
        ROLLBACK TRANSACTION
END
GO
--EXEC procAddProduct 
--    @Name = N'Loa JBL Partybox Ultimate',
--    @Image = NULL,
--    @ImageID = NULL,
--    @Price = 39900000,
--    @PromotionPrice = 33850000,
--    @Quantity = 200,
--    @Description = NULL,
--    @Detail = NULL,
--    @CateID = 1,
--    @ShopID = 1,
--    @BrandID = 3,
--    @SupplierID = NULL
--GO
--select * from tb_Product
GO

-- Cập nhật sản phẩm
CREATE PROCEDURE procUpdateProduct
    @ProductID INT,
    @Name NVARCHAR(255),
    @Image VARBINARY(MAX),
    @Price DECIMAL(18, 2),
    @PromotionPrice DECIMAL(18, 2),
    @Quantity INT,
    @Description NVARCHAR(MAX),
    @Detail NVARCHAR(MAX),
    @CateID INT,
    @ShopID INT,
    @BrandID INT,
    @SupplierID INT
AS
BEGIN
    BEGIN TRANSACTION
    IF @Price >= 0 AND @PromotionPrice >= 0 AND @Quantity >= 0
    BEGIN
        UPDATE tb_Product
        SET [Name] = @Name,
            [Image] = @Image,
            [Price] = @Price,
            [PromotionPrice] = @PromotionPrice,
            [Quantity] = @Quantity,
            [Description] = @Description,
            [Detail] = @Detail,
            [CateID] = @CateID,
            [ShopID] = @ShopID,
            [BrandID] = @BrandID,
            [SupplierID] = @SupplierID
        WHERE ProductID = @ProductID

        COMMIT TRANSACTION
    END
    ELSE
        ROLLBACK TRANSACTION
END
--select * from tb_Product where ProductID = 1
--EXEC procUpdateProduct 
--    @ProductID = 1,
--    @Name = N'Loa JBL Partybox Ultimate (Phiên bản mới)',
--    @Image = NULL,
--    @Price = 41900000,
--    @PromotionPrice = 35850000,
--    @Quantity = 25,
--    @Description = N'Loa JBL Partybox Ultimate phiên bản mới có thiết kế hiện đại và âm thanh mạnh mẽ.',
--    @Detail = N'...',
--    @CateID = 1,
--    @ShopID = 1,
--    @BrandID = 3,
--    @SupplierID = NULL
--GO
--select * from tb_Product where ProductID = 1

GO
-- Xoá 1 sản phẩm
CREATE PROCEDURE procRemoveProduct
    @ProductID INT
AS
BEGIN
    BEGIN TRANSACTION;
    DELETE FROM tb_OrderDetail WHERE ProductID = @ProductID
    DELETE FROM tb_Product WHERE ProductID = @ProductID

    COMMIT TRANSACTION;
END
--select * from tb_Product
--select * from tb_OrderDetail
--EXEC procRemoveProduct 
--    @ProductID = 2
--GO
--select * from tb_Product
--select * from tb_OrderDetail

GO

-- Select 1 sản phẩm
CREATE PROCEDURE procGetProduct
    @ProductID INT
AS
BEGIN
    SELECT * FROM tb_Product
    WHERE ProductID = @ProductID
END

--EXEC procGetProduct
--    @ProductID = 3
GO

-- Select tất cả sản phẩm của 1 shop
CREATE PROCEDURE procGetAllProductByShop
    @ShopID INT
AS
BEGIN
    SELECT * 
    FROM tb_Product
    WHERE ShopID = @ShopID
END
GO
--EXEC procGetAllProductByShop 
--    @ShopID = 1
GO

--Thêm bình luận đánh giá
CREATE PROCEDURE procAddProductComment
    @Detail NVARCHAR(MAX),
    @Star INT,
    @ProductID INT,
    @CustomerID INT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO tb_ProductComment ([Detail],[Star], [ProductID], [CustomerID])
    VALUES (@Detail, @Star, @ProductID, @CustomerID);
END
GO
--EXEC dbo.procAddProductComment N'Đẹp', 5, 1, 1
--GO

-- Select tất cả bình luận của 1 sản phẩm
CREATE PROCEDURE procGetProductComment
    @ProductID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT CommentID, [Detail], [Star], [CustomerID]
    FROM tb_ProductComment
    WHERE ProductID = @ProductID;
END
GO
--EXEC procGetProductComment @ProductID = 1
--GO

--DBCC CHECKIDENT ('[tb_Brand]', RESEED, 0)
--DBCC CHECKIDENT ('[tb_Supplier]', RESEED, 0)
--DBCC CHECKIDENT ('[tb_Role]', RESEED, 0)
--DBCC CHECKIDENT ('[tb_Address]', RESEED, 0)
--DBCC CHECKIDENT ('[tb_Customer]', RESEED, 0)
--DBCC CHECKIDENT ('[tb_Shop]', RESEED, 0)
--DBCC CHECKIDENT ('[tb_ProductCategory]', RESEED, 0)
--DBCC CHECKIDENT ('[tb_ListImage]', RESEED, 0)
--DBCC CHECKIDENT ('[tb_Product]', RESEED, 0)
--DBCC CHECKIDENT ('[tb_ProductComment]', RESEED, 0)
--DBCC CHECKIDENT ('[tb_Cart]', RESEED, 0)
--DBCC CHECKIDENT ('[tb_OrderDetail]', RESEED, 0)
--DBCC CHECKIDENT ('[tb_Order]', RESEED, 0)



--Delete Users
--DBCC CHECKIDENT ('Products', NORESEED)   --== Kiểm tra Identity đang số mấy
--DBCC CHECKIDENT ('Users', RESEED, 0) --== Set lại Identity chạy lại từ đầu
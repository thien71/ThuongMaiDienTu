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
	[Username] [varchar](50) NULL Unique,
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
	[OwnerID] [int] NULL,
	[CreatedDate] [datetime] DEFAULT (getdate())

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
	[QuantitySold] [int] NULL DEFAULT ((0)),
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
	(N'Denon',			'denon.jpg'),
	(N'Taylor',			'taylor.jpg')
GO

insert into tb_Role
values
	(N'Người dùng'), (N'Admin'), (N'Quản lí tài khoản'), (N'Quản lí sản phẩm')
GO

insert into tb_Customer([Username], [Password],	[Name], [Email], [Phone], [Gender], [DayOfBirth], [Address], [Avatar], [RoleID])
values
	('admin',	'123', N'Admin',					null, null, null, null, null, 'avatar1.jpg', 2),
	('thien',	'123', N'Nguyễn Văn Thanh Thiện',	null, null, null, null, null, 'avatar2.jpg', 1),
	('viet',	'123', N'Lê Xuân Việt',				null, null, null, null, null, 'avatar3.jpg', 1),
	('thang',	'123', N'Đỗ Viết Thắng',			null, null, null, null, null, 'avatar4.jpg', 1),
	('quy',		'123', N'Nguyễn Quang Quý',			null, null, null, null, null, 'avatar5.jpg', 1),
	('minh',	'123', N'Đinh Công Minh',			null, null, null, null, null, 'avatar6.jpg', 1),
	('an',		'123', N'Nguyễn Văn An',			null, null, null, null, null, 'avatar7.jpg', 1),
	('hoang',	'123', N'Nguyễn Văn Hoàng',			null, null, null, null, null, 'avatar8.jpg', 1),
	('hai',		'123', N'Nguyễn Văn Hải',			null, null, null, null, null, 'avatar9.jpg', 1),
	('vinh',	'123', N'Nguyễn Quốc Vinh',			null, null, null, null, null, 'avatar10.jpg', 1),
	('dung',	'123', N'Nguyễn Thị Dung',			null, null, null, null, null, 'avatar11.jpg', 1),
	('sy',		'123', N'Nguyễn Sỹ',				null, null, null, null, null, 'avatar12.jpg', 1),
	('quoc',	'123', N'Phạm Anh Quốc',			null, null, null, null, null, 'avatar13.jpg', 1)
GO

insert into tb_Shop([Name], [Phone], [Avatar], [Description], [Detail], [OwnerID])
values
	(N'Loa.Store',			'0398231456',	'shop1.jpg',	null, null, 3),
	(N'TDShop',				'0398231456',	'shop2.jpg',	null, null, 4),
	(N'Value Choice Shop',	'0904373232',	'shop3.jpg',	null, null, 5),
	(N'Poermax.Store',		'0962344674',	'shop4.jpg',	null, null, 6),
	(N'KingsnamTechnology',	'0905623671',	'shop5.jpg',	null, null, 7),
	(N'BestStore',			'0943674992',	'shop6.jpg',	null, null, 8),
	(N'Yoroshiko',			'036723344',	'shop7.jpg',	null, null, 9),
	(N'Máy chiếu mini KAW',	'0338442343',	'shop8.jpg',	null, null, 10),
	(N'Shopee Choice Global','0933673344',	'shop9.jpg',	null, null, 11),
	(N'hxbgxb.vn',			'0383624853',	'shop10.jpg',	null, null, 12),
	(N'Shopgiare1234',		'0394366333',	'shop11.jpg',	null, null, 13)
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
select * from tb_ProductCategory

insert into tb_Product([Name],[Image],[Price],[PromotionPrice],[Quantity], [QuantitySold],[Description],[Detail],[CateID],[ShopID],[BrandID],[SupplierID])
values
	(N'Loa JBL STUDIO 665C',			'22.jpg', 22900000, 20, 1000, 20, 
		N'là dòng loa xem phim cao cấp được thiết kế center đẹp cùng chất âm rõ ràng, chi tiết giúp tái hiện rõ ràng chân thực nhất âm thanh nhân vật dù nhỏ nhất như tiếng thì thầm, hay tiếng lá rơi. Loa JBL Studio 665C được sử dụng rất nhiều trong các hệ thống âm thanh phòng chiếu phim, tại Việt nam hiện nay.', 
		N'Model STUDIO 665C.Loại Loa Center cao cấp.Trình điều khiển tần số thấp 4 x 5,25 inch PolyPlas.Công suất Khoảng 170W.Đáp ứng tần số 58Hz - 40kHz.Độ nhạy 90dB @ 1M, 2,83V.Trở kháng danh nghĩa 6 OhmsCrossover Frequencies.600 Hz, 2.2kHz.Kích thước 760 x 230 x 190mm (W x D x H).Trọng lượng 16,12kg',	
		1, 11, 3, null), 
	(N'Loa Âm Trần KACAUDIO 104',		'23.jpg', 500000,	20, 7550, 20, null, null,	1, 1, 3, null),
	(N'Loa Âm Trần JBL 8128',			'24.jpg', 1800000,	30, 1438, 20, null, null,	1, 1, 3, null),
	(N'Loa Âm Trần TOA PC-658R',		'25.jpg', 400000,	22, 3210, 20, null, null,	1, 1, 3, null),
	(N'Loa Âm Trần BOSCH LHM',			'26.jpg', 500000,	29, 3300, 30, null, null,	1, 1, 3, null),
	(N'Loa JBL Partybox Ultimate',		'27.jpg', 39900000, 28, 1000, 80, null, null,	1, 1, 3, null), 
	(N'Loa JBL Control X',				'28.jpg', 10900000, 27, 1000, 20, null, null,	1, 2, 3, null), 
	(N'Loa JBL Control 85M',			'29.jpg', 7000000,  26, 1300, 40, null, null,	1, 2, 3, null),
	(N'Loa Harman Kardon Go Play 3',	'30.jpg', 7900000,  25, 1000, 32, null, null,	1, 2, 3, null),
	(N'Loa B&O Beolit 20',				'31.jpg', 12000000, 24, 1400, 25, null, null,	1, 2, 7, null),
	(N'Loa JBL Control 31',				'32.jpg', 19990000, 23, 1000, 20, null, null,	1, 2, 3, null), 	
	(N'Loa Speaker Bose 101',			'33.jpg', 1200000,  22, 2200, 21, null, null,	1, 3, 2, null),
	(N'Loa Kasen FT-205S',				'34.jpg', 1800000,  21, 1000, 20, null, null,	1, 3, 3, null),
	(N'Loa C-PSOUND CON-10',			'35.jpg', 1500000,  20, 1000, 20, null, null,	1, 3, 3, null),
	(N'Loa Bose FreeSpace FS2SE',		'36.jpg', 10000000, 22, 1000, 20, null, null,	1, 3, 2, null),
	(N'Loa JBL Partybox Encore',		'37.jpg', 1400000,  22, 100, 26, null, null,	1, 3, 3, null),
	(N'Tai nghe Có dây AVA',			'42.jpg', 200000,	36, 1000, 250, null, null,	2, 4, 5, null),
	(N'Tai nghe Có Dây Apple MTJY3',	'43.jpg', 500000,	38, 1200, 220, null, null,	2, 4, 5, null),
	(N'Tai nghe Mozard DS510-WB ',		'44.jpg', 150000,	39, 1200, 240, null, null,	2, 4, 5, null),
	(N'Tai nghe Samsung IA500',			'45.jpg', 250000,	32, 1000, 230, null, null,	2, 4, 15, null),
	(N'Tai nghe OPPO MH320 ',			'46.jpg', 180000,	20, 1700, 420, null, null,	2, 4, 15, null),
	(N'Tai nghe Apple AirPods 2',		'47.jpg', 2500000,	22, 1800, 250, null, null,	2, 5, 15, null),
	(N'Tai nghe Marshall Minor 3',		'48.jpg', 2800000,	41, 900, 240, null, null,	2, 5, 13, null),
	(N'Tai nghe Baseus Bowie WM02',		'49.jpg', 400000,	43, 1000, 620, null, null,	2, 5, 13, null),
	(N'Tai nghe Samsung Galaxy Buds FE','50.jpg', 1400000,	32, 1000, 260, null, null,	2, 5, 15, null),
	(N'Tai nghe JBL Live Pro 2',		'51.jpg', 2700000,	33, 1000, 270, null, null,	2, 5, 3, null),
	(N'Tai nghe DareU EH416',			'52.jpg', 700000,	23, 340, 12, null, null,	2, 6, 13, null),
	(N'Tai nghe RKX 3.5 Gaming',		'53.jpg', 10000000, 20, 600, 72, null, null,	2, 6, 13, null),
	(N'Tai nghe Sony MDR-ZX310AP',		'54.jpg', 600000,	20, 500, 20, null, null,	2, 6, 13, null),
	(N'Tai nghe HyperX Cloud Stinger',	'55.jpg', 800000,	20, 1000, 20, null, null,	2, 6, 13, null),
	(N'Tai nghe Yamaha YH-5000SE',		'56.jpg', 100000000, 20, 1000, 20, null, null,	2, 6, 13, null),
	(N'Tai Nghe SONY WH-CH520',			'57.jpg', 10000000,  20, 1000, 20, null, null,	2, 7, 1, null),
	(N'Tai nghe Soul Ultra Dynamic',	'58.jpg', 700000,	20, 1000, 20, null, null,	2, 7, 13, null),
	(N'Tai nghe Havit H630BT',			'59.jpg', 400000,	20, 1000, 20, null, null,	2, 7, 13, null),
	(N'Tai nghe Edifier W820NB',		'60.jpg', 900000,	20, 1000, 20, null, null,	2, 7, 13, null),
	(N'Tai nghe Sony MDR-ZX110AP',		'61.jpg', 500000,	20, 1000, 20, null, null,	2, 7, 1, null),
	(N'Micro Không Dây JBL',			'62.jpg', 11000000, 20, 1000, 20, null, null,	3, 8, 3, null),
	(N'Micro Toprhyme M300',			'63.jpg', 45000000, 20, 1000, 20, null, null,	3, 8, 11, null),
	(N'Micro Shure SVX288A/PG58',		'64.jpg', 14000000, 20, 1000, 20, null, null,	3, 8, 11, null),
	(N'Micro Shure BLX24A/PG58',		'65.jpg', 10000000, 20, 1000, 20, null, null,	3, 8, 11, null),
	(N'Micro BMB WB-5000',				'66.jpg', 12000000, 20, 1000, 20, null, null,	3, 8, 11, null),
	(N'Micro SENNHEISER E 845-S',		'67.jpg', 10000000, 20, 1000, 20, null, null,	3, 9, 4, null),
	(N'Micro Có Dây BIK Pro-8X',		'68.jpg', 800000,	20, 1000, 20, null, null,	3, 9, 4, null),
	(N'Micro Dây Shure PGA58-LC',		'69.jpg', 1500000,	20, 500, 20, null, null,	3, 9, 11, null),
	(N'Micro Shure SM58-LC',			'70.jpg', 3600000,  20, 3100, 20, null, null,	3, 9, 11, null),
	(N'Micro Dây Guinness BG-68S',		'71.jpg', 500000,	20, 1000, 20, null, null,	3, 9, 4, null),
	(N'Ampli Marantz NR 1200',			'1.jpg', 21000000,	20, 1000, 20, null, null,	4, 10, 19, null),
	(N'Amply Toa A-230',				'2.jpg', 2200000,	10, 1000, 0, null, null,	4, 10, 19, null),
	(N'AMPLY mini LEPY LP-838USB',		'3.jpg', 400000,	21, 1000, 0, null, null,	4, 10, 19, null),
	(N'AMPLY JARGUAR 601A',				'4.jpg', 1000000,   20, 9988, 6, null, null,	4, 10, 19, null),
	(N'Ampli Marantz PM8006',			'5.jpg', 40000000,	24, 3000, 7, null, null,	4, 10, 19, null),
	(N'Máy nghe nhạc MP3 Wakama',		'7.jpg', 10000000,  20, 1000, 20, null, null,	5, 11, 1, null),
	(N'Máy nghe nhạc SONY NW',			'8.jpg', 10000000,  14, 1000, 20, null, null,	5, 11, 1, null),
	(N'Máy nghe nhạc AGPTEK C2',		'9.jpg', 400000,	20, 1000, 20, null, null,	5, 11, 1, null),
	(N'Máy Nghe Nhạc Ruizu H11',		'10.jpg', 1200000,  20, 1000, 20, null, null,	5, 11, 1, null),
	(N'Máy nghe nhạc VIRWIR',			'11.jpg', 300000,	22, 1200, 60, null, null,	5, 11, 1, null),
	(N'Bàn DJ Pioneer XDJ-XZ ',			'77.jpg', 65000000,	27, 1000, 90, null, null,	6, 1, 8, null),
	(N'Bàn Pioneer DJ DDJ-200 ',		'78.jpg', 5400000,  21, 4000, 20, null, null,	6, 1, 8, null),
	(N'Bàn DJ Pioneer OPUS-QUAD ',		'79.jpg', 87000000, 20, 1000, 20, null, null,	6, 1, 8, null),
	(N'Bàn DJ Pioneer DJM-V10 ',		'80.jpg', 90000000, 20, 1000, 20, null, null,	6, 1, 8, null),
	(N'Bàn DJ Pioneer CDJ-3000 ',		'81.jpg', 90000000, 20, 1000, 20, null, null,	6, 1, 8, null),
	(N'Bàn DJ Pioneer DDJ-FLX4',		'82.jpg', 10000000, 20, 1000, 20, null, null,	6, 2, 8, null),
	(N'Bàn DJ Numark Party Mix',		'83.jpg', 3500000,  20, 2000, 20, null, null,   6, 2, 10, null),
	(N'Bàn DJ Numark Mixtrack 3',		'84.jpg', 7000000,  20, 1000, 20, null, null,   6, 2, 10, null),
	(N'Bàn DJ Numark Mixstream Pro',	'85.jpg', 20000000, 20, 5000, 20, null, null,	6, 2, 10, null),
	(N'Bàn DJ Numark Mixtrack Platinum','86.jpg', 8000000,  20, 1000, 40, null, null,	6, 2, 10, null),
	(N'Bàn Mixer Yamaha MG12XU',		'12.jpg', 12000000, 20, 4000, 80, null, null,	7, 3, 17, null),
	(N'Bàn Mixer Yamaha MG10XUF',		'13.jpg', 10000000, 30, 5000, 20, null, null,	7, 3, 17, null),
	(N'Mixer Behringer X AIR XR12',		'14.jpg', 10000000, 20, 5000, 20, null, null,	7, 3, 17, null),
	(N'Bàn Mixer Midas M32R',			'15.jpg', 58000000, 20, 1000, 20, null, null,	7, 3, 17, null),
	(N'Bàn Mixer Midas MR 18',			'16.jpg', 20000000, 32, 100, 20, null, null,	7, 3, 15, null),
	(N'Volume Control VC-201C',			'17.jpg', 300000,	40, 400, 20, null, null,	8, 4, 15, null),
	(N'TD-1032 Stereo Jack TRS 6.3mm',	'18.jpg', 40000,	50, 100, 90, null, null,	8, 4, 15, null),
	(N'TAD-094 Dây micro 5M',			'19.jpg', 50000,	20, 1009, 20, null, null,	8, 4, 15, null),
	(N'Bắp chuối NAKAMICHI',			'20.jpg', 60000,	20, 1340, 20, null, null,	8, 4, 15, null),
	(N'Hộp 4 Pin xạc 3000mhA',			'21.jpg', 300000,	20, 134, 20, null, null,	8, 4, 15, null),
	(N'Đàn Guitar TAYLOR 110E',			'88.jpg', 10000000, 25, 1000, 80, null, null,	9, 5, 20, null),
	(N'Đàn Guitar MS E55E',				'89.jpg', 500000,	24, 1700, 20, null, null,	9, 5, 20, null),	
	(N'Guitar Acoustic E65BLack',		'90.jpg', 600000,	20, 90, 23, null, null,		9, 5, 20, null),	
	(N'Guitar Acoustic E65G',			'91.jpg', 700000,	30, 340, 20, null, null,	9, 5, 20, null),	
	(N'Guitar classic E75OBLACK',		'92.jpg', 900000,	40, 1000, 20, null, null,	9, 5, 20, null),	
	(N'Đàn Violin Omebo RV205',			'93.jpg', 2500000,	50, 3000, 20, null, null,	9, 6, 20, null),
	(N'Đàn Violin V601 1/4',			'94.jpg', 1600000,  27, 1000, 30, null, null,	9, 6, 20, null),
	(N'Đàn Violin Strad Classic ',		'95.jpg', 2500000,	21, 2000, 20, null, null,	9, 6, 20, null),
	(N'Đàn Violin Carlo Giordan',		'96.jpg', 2000000,	30, 1000, 25, null, null,	9, 6, 20, null),
	(N'Kapok Violin V182',				'97.jpg', 3000000,  29, 1000, 5, null, null,	9, 6, 20, null),
	(N'Loa JBL STUDIO 665C',			'22.jpg', 22900000, 20, 1000, 20, null, null,	10, 7, 3, null), 
	(N'Loa Âm Trần KACAUDIO 104',		'23.jpg', 500000,	20, 7550, 20, null, null,	10, 7, 3, null),
	(N'Loa Âm Trần JBL 8128',			'24.jpg', 1800000,	30, 1438, 20, null, null,	10, 7, 3, null),
	(N'Loa Âm Trần TOA PC-658R',		'25.jpg', 400000,	22, 3210, 20, null, null,	10, 7, 3, null),
	(N'Loa Âm Trần BOSCH LHM',			'26.jpg', 500000,	29, 3300, 30, null, null,	10, 7, 3, null),
	(N'Loa JBL Partybox Ultimate',		'27.jpg', 39900000, 28, 1000, 80, null, null,	11, 8, 3, null), 
	(N'Loa JBL Control X',				'28.jpg', 10900000, 27, 1000, 20, null, null,	11, 8, 3, null), 
	(N'Loa JBL Control 85M',			'29.jpg', 7000000,  26, 1300, 40, null, null,	11, 8, 3, null),
	(N'Loa Harman Kardon Go Play 3',	'30.jpg', 7900000,  25, 1000, 32, null, null,	11, 8, 3, null),
	(N'Loa B&O Beolit 20',				'31.jpg', 12000000, 24, 1400, 25, null, null,	11, 8, 7, null),
	(N'Loa JBL Control 31',				'32.jpg', 19990000, 23, 1000, 20, null, null,	12, 9, 3, null), 	
	(N'Loa Speaker Bose 101',			'33.jpg', 1200000,  22, 2200, 21, null, null,	12, 9, 2, null),
	(N'Loa Kasen FT-205S',				'34.jpg', 1800000,  21, 1000, 20, null, null,	12, 9, 3, null),
	(N'Loa C-PSOUND CON-10',			'35.jpg', 1500000,  20, 1000, 20, null, null,	12, 9, 3, null),
	(N'Loa Bose FreeSpace FS2SE',		'36.jpg', 10000000, 22, 1000, 20, null, null,	12, 9, 2, null),
	(N'Loa JBL Partybox Encore',		'37.jpg', 1400000,  22, 100, 26, null, null,	13, 10, 3, null),
	(N'Loa Karaoke Denon DP-C10',		'38.jpg', 6500000,  24, 600, 25, null, null,	13, 10, 19, null),
	(N'Loa Karaoke JBL CV1652T',		'39.jpg', 9000000,  20, 1700, 2, null, null,	13, 10, 3, null),
	(N'Loa Karaoke BIK CS-525',			'40.jpg', 1400000,  20, 3600, 50, null, null,	13, 10, 3, null),
	(N'Loa Karaoke Denon DP-R310',		'41.jpg', 15000000, 34, 5400, 220, null, null,	13, 10, 19, null),
	(N'Tai nghe Có dây AVA',			'42.jpg', 200000,	36, 1000, 250, null, null,	16, 11, 5, null),
	(N'Tai nghe Có Dây Apple MTJY3',	'43.jpg', 500000,	38, 1200, 220, null, null,	16, 11, 5, null),
	(N'Tai nghe Mozard DS510-WB ',		'44.jpg', 150000,	39, 1200, 240, null, null,	16, 11, 5, null),
	(N'Tai nghe Samsung IA500',			'45.jpg', 250000,	32, 1000, 230, null, null,	16, 11, 15, null),
	(N'Tai nghe OPPO MH320 ',			'46.jpg', 180000,	20, 1700, 420, null, null,	16, 11, 15, null),
	(N'Tai nghe Apple AirPods 2',		'47.jpg', 2500000,	22, 1800, 250, null, null,	17, 1, 15, null),
	(N'Tai nghe Marshall Minor 3',		'48.jpg', 2800000,	41, 900, 240, null, null,	17, 1, 13, null),
	(N'Tai nghe Baseus Bowie WM02',		'49.jpg', 400000,	43, 1000, 620, null, null,	17, 1, 13, null),
	(N'Tai nghe Samsung Galaxy Buds FE','50.jpg', 1400000,	32, 1000, 260, null, null,	17, 1, 15, null),
	(N'Tai nghe JBL Live Pro 2',		'51.jpg', 2700000,	33, 1000, 270, null, null,	17, 1, 3, null),
	(N'Tai nghe DareU EH416',			'52.jpg', 700000,	23, 340, 12, null, null,	18, 2, 13, null),
	(N'Tai nghe RKX 3.5 Gaming',		'53.jpg', 10000000, 20, 600, 72, null, null,	18, 2, 13, null),
	(N'Tai nghe Sony MDR-ZX310AP',		'54.jpg', 600000,	20, 500, 20, null, null,	18, 2, 13, null),
	(N'Tai nghe HyperX Cloud Stinger',	'55.jpg', 800000,	20, 1000, 20, null, null,	18, 2, 13, null),
	(N'Tai nghe Yamaha YH-5000SE',		'56.jpg', 100000000, 20, 1000, 20, null, null,	18, 2, 13, null),
	(N'Tai Nghe SONY WH-CH520',			'57.jpg', 10000000,  20, 1000, 20, null, null,	19, 3, 1, null),
	(N'Tai nghe Soul Ultra Dynamic',	'58.jpg', 700000,	20, 1000, 20, null, null,	19, 3, 13, null),
	(N'Tai nghe Havit H630BT',			'59.jpg', 400000,	20, 1000, 20, null, null,	19, 3, 13, null),
	(N'Tai nghe Edifier W820NB',		'60.jpg', 900000,	20, 1000, 20, null, null,	19, 3, 13, null),
	(N'Tai nghe Sony MDR-ZX110AP',		'61.jpg', 500000,	20, 1000, 20, null, null,	19, 3, 1, null),
	(N'Micro Không Dây JBL',			'62.jpg', 11000000, 20, 1000, 20, null, null,	20, 4, 3, null),
	(N'Micro Toprhyme M300',			'63.jpg', 45000000, 20, 1000, 20, null, null,	20, 4, 11, null),
	(N'Micro Shure SVX288A/PG58',		'64.jpg', 14000000, 20, 1000, 20, null, null,	20, 4, 11, null),
	(N'Micro Shure BLX24A/PG58',		'65.jpg', 10000000, 20, 1000, 20, null, null,	20, 4, 11, null),
	(N'Micro BMB WB-5000',				'66.jpg', 12000000, 20, 1000, 20, null, null,	20, 4, 11, null),
	(N'Micro SENNHEISER E 845-S',		'67.jpg', 10000000, 20, 1000, 20, null, null,	21, 4, 4, null),
	(N'Micro Có Dây BIK Pro-8X',		'68.jpg', 800000,	20, 1000, 20, null, null,	21, 4, 4, null),
	(N'Micro Dây Shure PGA58-LC',		'69.jpg', 1500000,	20, 500, 20, null, null,	21, 4, 11, null),
	(N'Micro Shure SM58-LC',			'70.jpg', 3600000,  20, 3100, 20, null, null,	21, 4, 11, null),
	(N'Micro Dây Guinness BG-68S',		'71.jpg', 500000,	20, 1000, 20, null, null,	21, 4, 4, null),
	(N'Micro Baomic Bm 222B',			'72.jpg', 2400000,  20, 8700, 20, null, null,	22, 5, 4, null),
	(N'Micro Baomic Bm 222A ',			'73.jpg', 2400000,	20, 1000, 0, null, null,	22, 5, 4, null),
	(N'Micro MB-T 6400S Dynamic',		'74.jpg', 4200000,  20, 1000, 0, null, null,	22, 5, 4, null),
	(N'Micro ST-5000 Conference',		'75.jpg', 1200000,	21, 2000, 0, null, null,	22, 5, 4, null),
	(N'ST-6000 Micro cổ ngỗng JTS',		'76.jpg', 2700000,  45, 2000, 0, null, null,	22, 5, 4, null),
	(N'Bàn DJ Pioneer XDJ-XZ ',			'77.jpg', 65000000,	27, 1000, 90, null, null,	23, 6, 8, null),
	(N'Bàn Pioneer DJ DDJ-200 ',		'78.jpg', 5400000,  21, 4000, 20, null, null,	23, 6, 8, null),
	(N'Bàn DJ Pioneer OPUS-QUAD ',		'79.jpg', 87000000, 20, 1000, 20, null, null,	23, 6, 8, null),
	(N'Bàn DJ Pioneer DJM-V10 ',		'80.jpg', 90000000, 20, 1000, 20, null, null,	23, 6, 8, null),
	(N'Bàn DJ Pioneer CDJ-3000 ',		'81.jpg', 90000000, 20, 1000, 20, null, null,	23, 6, 8, null),
	(N'Bàn DJ Pioneer DDJ-FLX4',		'82.jpg', 10000000, 20, 1000, 20, null, null,	23, 7, 8, null),
	(N'Bàn DJ Numark Party Mix',		'83.jpg', 3500000,  20, 2000, 20, null, null,   24, 7, 10, null),
	(N'Bàn DJ Numark Mixtrack 3',		'84.jpg', 7000000,  20, 1000, 20, null, null,   24, 7, 10, null),
	(N'Bàn DJ Numark Mixstream Pro',	'85.jpg', 20000000, 20, 5000, 20, null, null,	24, 7, 10, null),
	(N'Bàn DJ Numark Mixtrack Platinum','86.jpg', 8000000,  20, 1000, 40, null, null,	24, 7, 10, null),
	(N'Đàn Guitar TAYLOR 110E',			'88.jpg', 10000000, 25, 1000, 80, null, null,	25, 8, 20, null),
	(N'Đàn Guitar MS E55E',				'89.jpg', 500000,	24, 1700, 20, null, null,	25, 8, 20, null),	
	(N'Guitar Acoustic E65BLack',		'90.jpg', 600000,	20, 90, 23, null, null,		25, 8, 20, null),	
	(N'Guitar Acoustic E65G',			'91.jpg', 700000,	30, 340, 20, null, null,	25, 8, 20, null),	
	(N'Guitar classic E75OBLACK',		'92.jpg', 900000,	40, 1000, 20, null, null,	25, 8, 20, null),	
	(N'Đàn Violin Omebo RV205',			'93.jpg', 2500000,	50, 3000, 20, null, null,	26, 9, 20, null),
	(N'Đàn Violin V601 1/4',			'94.jpg', 1600000,  27, 1000, 30, null, null,	26, 9, 20, null),
	(N'Đàn Violin Strad Classic ',		'95.jpg', 2500000,	21, 2000, 20, null, null,	26, 9, 20, null),
	(N'Đàn Violin Carlo Giordan',		'96.jpg', 2000000,	30, 1000, 25, null, null,	26, 9, 20, null),
	(N'Kapok Violin V182',				'97.jpg', 3000000,  29, 1000, 5, null, null,	26, 9, 20, null)
GO

select * from tb_Brand
select * from tb_ProductCategory
select * from tb_Product

insert into tb_ProductComment([Detail],[Star],[ProductID],[CustomerID])
values
	(N'Được',			default, 1, 3),
	(N'Được được',		default, 1, 4),
	(N'Được đấy',		default, 1, 5),
	(N'Được đấy chứ',	default, 2, 6),
	(N'Cũng được',		default, 2, 7)
GO

insert into tb_Cart([CustomerID],[ProductID],[Image],[Price],[Quantity])
values
	(2, 1, N'1.jpg' ,16800000, 1)
GO

--insert into tb_Order([Status],[Address],[CustomerID],[ShopID])
--values
--	(default, N'An Tân, xã Hoà Phong',	 1, 1),
--	(default, N'An Tân, xã Hoà Phú',	 1, 1),
--	(default, N'An Tân, xã Hoà Nhơn',	 1, 1),
--	(default, N'An Tân, xã Hoà Khương',	 1, 1)
--GO

--insert into tb_OrderDetail([OrderID],[ProductID],[Price],[Quantity])
--values
--	(1, 1, 10000000,	2),
--	(1, 2, 11000000,	1),
--	(1, 3, 15000000,	1),
--	(2, 1, 30000000,	3),
--	(2, 2, 9000000,		5)
--GO

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

--DELETE FROM [tb_Brand]
--DELETE FROM [tb_Supplier]
--DELETE FROM [tb_Role]
--DELETE FROM [tb_Address]
--DELETE FROM [tb_Customer]
--DELETE FROM [tb_Shop]
--DELETE FROM [tb_ProductCategory]
--DELETE FROM [tb_ListImage]
--DELETE FROM [tb_Product]
--DELETE FROM [tb_ProductComment]
--DELETE FROM [tb_Cart]
--DELETE FROM [tb_OrderDetail]
--DELETE FROM [tb_Order]

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

/****** Object:  Table [dbo].[Storages]    Script Date: 01/12/2015 23:03:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Storages](
	[StorageId] [int] IDENTITY(1,1) NOT NULL,
	[StorageName] [nvarchar](128) NOT NULL,
	[Desciption] [nvarchar](128) NULL,
	[IsDeleted] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[StorageId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Storages] ON
INSERT [dbo].[Storages] ([StorageId], [StorageName], [Desciption], [IsDeleted]) VALUES (1, N'HCM', N'HCM', 0)
INSERT [dbo].[Storages] ([StorageId], [StorageName], [Desciption], [IsDeleted]) VALUES (2, N'Đà Nẵng', N'Đà Nẵng', 0)
SET IDENTITY_INSERT [dbo].[Storages] OFF
/****** Object:  Table [dbo].[StaticPages]    Script Date: 01/12/2015 23:03:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StaticPages](
	[StaticPageId] [int] IDENTITY(1,1) NOT NULL,
	[Type] [int] NOT NULL,
	[Description] [nvarchar](128) NULL,
	[Content] [nvarchar](max) NULL,
	[IsDeleted] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[StaticPageId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[StaticPages] ON
INSERT [dbo].[StaticPages] ([StaticPageId], [Type], [Description], [Content], [IsDeleted]) VALUES (1, 1, N'Trang giới thiệu', N'Nhập nội dung vào đây', 0)
INSERT [dbo].[StaticPages] ([StaticPageId], [Type], [Description], [Content], [IsDeleted]) VALUES (2, 2, N'Trang liên hệ', N'Nhập nội dung vào đây', 0)
SET IDENTITY_INSERT [dbo].[StaticPages] OFF
/****** Object:  Table [dbo].[StateProvinces]    Script Date: 01/12/2015 23:03:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StateProvinces](
	[StateProvinceId] [int] IDENTITY(1,1) NOT NULL,
	[StateProvinceName] [nvarchar](128) NOT NULL,
	[StateProvinceCode] [nvarchar](24) NULL,
	[IsDeleted] [bit] NOT NULL,
	[MyOfficeId] [int] NOT NULL,
	[IsMain] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[StateProvinceId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[StateProvinces] ON
INSERT [dbo].[StateProvinces] ([StateProvinceId], [StateProvinceName], [StateProvinceCode], [IsDeleted], [MyOfficeId], [IsMain]) VALUES (1, N'An Giang', N'AGG', 0, 1, 0)
INSERT [dbo].[StateProvinces] ([StateProvinceId], [StateProvinceName], [StateProvinceCode], [IsDeleted], [MyOfficeId], [IsMain]) VALUES (2, N'Bà Rịa-Vũng Tàu', N'BRT', 0, 1, 0)
INSERT [dbo].[StateProvinces] ([StateProvinceId], [StateProvinceName], [StateProvinceCode], [IsDeleted], [MyOfficeId], [IsMain]) VALUES (3, N'Bạc Liêu', N'BLU', 0, 1, 0)
INSERT [dbo].[StateProvinces] ([StateProvinceId], [StateProvinceName], [StateProvinceCode], [IsDeleted], [MyOfficeId], [IsMain]) VALUES (4, N'Bắc Kạn', N'BKN', 0, 2, 0)
INSERT [dbo].[StateProvinces] ([StateProvinceId], [StateProvinceName], [StateProvinceCode], [IsDeleted], [MyOfficeId], [IsMain]) VALUES (5, N'Bắc Giang', N'BGG', 0, 2, 0)
INSERT [dbo].[StateProvinces] ([StateProvinceId], [StateProvinceName], [StateProvinceCode], [IsDeleted], [MyOfficeId], [IsMain]) VALUES (6, N'Bắc Ninh', N'BNH', 0, 2, 0)
INSERT [dbo].[StateProvinces] ([StateProvinceId], [StateProvinceName], [StateProvinceCode], [IsDeleted], [MyOfficeId], [IsMain]) VALUES (7, N'Bến Tre', N'BTE', 0, 1, 0)
INSERT [dbo].[StateProvinces] ([StateProvinceId], [StateProvinceName], [StateProvinceCode], [IsDeleted], [MyOfficeId], [IsMain]) VALUES (8, N'Bình Dương', N'BDG', 0, 1, 0)
INSERT [dbo].[StateProvinces] ([StateProvinceId], [StateProvinceName], [StateProvinceCode], [IsDeleted], [MyOfficeId], [IsMain]) VALUES (9, N'Bình Định', N'BDH', 0, 1, 0)
INSERT [dbo].[StateProvinces] ([StateProvinceId], [StateProvinceName], [StateProvinceCode], [IsDeleted], [MyOfficeId], [IsMain]) VALUES (10, N'Bình Phước', N'BPC', 0, 1, 0)
INSERT [dbo].[StateProvinces] ([StateProvinceId], [StateProvinceName], [StateProvinceCode], [IsDeleted], [MyOfficeId], [IsMain]) VALUES (11, N'Bình Thuận', N'BTNN', 0, 1, 0)
INSERT [dbo].[StateProvinces] ([StateProvinceId], [StateProvinceName], [StateProvinceCode], [IsDeleted], [MyOfficeId], [IsMain]) VALUES (12, N'Cà Mau', N'CMU', 0, 1, 0)
INSERT [dbo].[StateProvinces] ([StateProvinceId], [StateProvinceName], [StateProvinceCode], [IsDeleted], [MyOfficeId], [IsMain]) VALUES (13, N'Cao Bằng', N'CBG', 0, 2, 0)
INSERT [dbo].[StateProvinces] ([StateProvinceId], [StateProvinceName], [StateProvinceCode], [IsDeleted], [MyOfficeId], [IsMain]) VALUES (14, N'Cần Thơ (TP)', N'CTO', 0, 1, 0)
INSERT [dbo].[StateProvinces] ([StateProvinceId], [StateProvinceName], [StateProvinceCode], [IsDeleted], [MyOfficeId], [IsMain]) VALUES (15, N'Đà Nẵng (TP)', N'DNG', 0, 2, 0)
INSERT [dbo].[StateProvinces] ([StateProvinceId], [StateProvinceName], [StateProvinceCode], [IsDeleted], [MyOfficeId], [IsMain]) VALUES (16, N'Đắk Lắk', N'DLK', 0, 1, 0)
INSERT [dbo].[StateProvinces] ([StateProvinceId], [StateProvinceName], [StateProvinceCode], [IsDeleted], [MyOfficeId], [IsMain]) VALUES (17, N'Đắk Nông', N'DNG', 0, 1, 0)
INSERT [dbo].[StateProvinces] ([StateProvinceId], [StateProvinceName], [StateProvinceCode], [IsDeleted], [MyOfficeId], [IsMain]) VALUES (18, N'Điện Biên', N'DBN', 0, 2, 0)
INSERT [dbo].[StateProvinces] ([StateProvinceId], [StateProvinceName], [StateProvinceCode], [IsDeleted], [MyOfficeId], [IsMain]) VALUES (19, N'Đồng Nai', N'DNI', 0, 1, 0)
INSERT [dbo].[StateProvinces] ([StateProvinceId], [StateProvinceName], [StateProvinceCode], [IsDeleted], [MyOfficeId], [IsMain]) VALUES (20, N'Đồng Tháp', N'DTP', 0, 1, 0)
INSERT [dbo].[StateProvinces] ([StateProvinceId], [StateProvinceName], [StateProvinceCode], [IsDeleted], [MyOfficeId], [IsMain]) VALUES (21, N'Gia Lai', N'GLI', 0, 1, 0)
INSERT [dbo].[StateProvinces] ([StateProvinceId], [StateProvinceName], [StateProvinceCode], [IsDeleted], [MyOfficeId], [IsMain]) VALUES (22, N'Hà Giang', N'HGG', 0, 2, 0)
INSERT [dbo].[StateProvinces] ([StateProvinceId], [StateProvinceName], [StateProvinceCode], [IsDeleted], [MyOfficeId], [IsMain]) VALUES (23, N'Hà Nam', N'HNM', 0, 2, 0)
INSERT [dbo].[StateProvinces] ([StateProvinceId], [StateProvinceName], [StateProvinceCode], [IsDeleted], [MyOfficeId], [IsMain]) VALUES (24, N'Hà Nội (TP)', N'HNI', 0, 2, 0)
INSERT [dbo].[StateProvinces] ([StateProvinceId], [StateProvinceName], [StateProvinceCode], [IsDeleted], [MyOfficeId], [IsMain]) VALUES (25, N'Hà Tây', N'HTY', 0, 2, 0)
INSERT [dbo].[StateProvinces] ([StateProvinceId], [StateProvinceName], [StateProvinceCode], [IsDeleted], [MyOfficeId], [IsMain]) VALUES (26, N'Hà Tĩnh', N'HTH', 0, 2, 0)
INSERT [dbo].[StateProvinces] ([StateProvinceId], [StateProvinceName], [StateProvinceCode], [IsDeleted], [MyOfficeId], [IsMain]) VALUES (27, N'Hải Dương', N'HDG', 0, 2, 0)
INSERT [dbo].[StateProvinces] ([StateProvinceId], [StateProvinceName], [StateProvinceCode], [IsDeleted], [MyOfficeId], [IsMain]) VALUES (28, N'Hải Phòng (TP)', N'HPG', 0, 2, 0)
INSERT [dbo].[StateProvinces] ([StateProvinceId], [StateProvinceName], [StateProvinceCode], [IsDeleted], [MyOfficeId], [IsMain]) VALUES (29, N'Hòa Bình', N'HBH', 0, 2, 0)
INSERT [dbo].[StateProvinces] ([StateProvinceId], [StateProvinceName], [StateProvinceCode], [IsDeleted], [MyOfficeId], [IsMain]) VALUES (30, N'Hồ Chí Minh (TP)', N'HCM', 0, 1, 0)
INSERT [dbo].[StateProvinces] ([StateProvinceId], [StateProvinceName], [StateProvinceCode], [IsDeleted], [MyOfficeId], [IsMain]) VALUES (31, N'Hậu Giang', N'HGG', 0, 1, 0)
INSERT [dbo].[StateProvinces] ([StateProvinceId], [StateProvinceName], [StateProvinceCode], [IsDeleted], [MyOfficeId], [IsMain]) VALUES (32, N'Hưng Yên', N'HYN', 0, 2, 0)
INSERT [dbo].[StateProvinces] ([StateProvinceId], [StateProvinceName], [StateProvinceCode], [IsDeleted], [MyOfficeId], [IsMain]) VALUES (33, N'Khánh Hòa', N'KHA', 0, 1, 0)
INSERT [dbo].[StateProvinces] ([StateProvinceId], [StateProvinceName], [StateProvinceCode], [IsDeleted], [MyOfficeId], [IsMain]) VALUES (34, N'Kiên Giang', N'KGG', 0, 1, 0)
INSERT [dbo].[StateProvinces] ([StateProvinceId], [StateProvinceName], [StateProvinceCode], [IsDeleted], [MyOfficeId], [IsMain]) VALUES (35, N'Kon Tum', N'KTM', 0, 1, 0)
INSERT [dbo].[StateProvinces] ([StateProvinceId], [StateProvinceName], [StateProvinceCode], [IsDeleted], [MyOfficeId], [IsMain]) VALUES (36, N'Lai Châu', N'LCU', 0, 2, 0)
INSERT [dbo].[StateProvinces] ([StateProvinceId], [StateProvinceName], [StateProvinceCode], [IsDeleted], [MyOfficeId], [IsMain]) VALUES (37, N'Lào Cai', N'LCI', 0, 2, 0)
INSERT [dbo].[StateProvinces] ([StateProvinceId], [StateProvinceName], [StateProvinceCode], [IsDeleted], [MyOfficeId], [IsMain]) VALUES (38, N'Lạng Sơn', N'LSN', 0, 2, 0)
INSERT [dbo].[StateProvinces] ([StateProvinceId], [StateProvinceName], [StateProvinceCode], [IsDeleted], [MyOfficeId], [IsMain]) VALUES (39, N'Lâm Đồng', N'LDG', 0, 1, 0)
INSERT [dbo].[StateProvinces] ([StateProvinceId], [StateProvinceName], [StateProvinceCode], [IsDeleted], [MyOfficeId], [IsMain]) VALUES (40, N'Long An', N'LAN', 0, 1, 0)
INSERT [dbo].[StateProvinces] ([StateProvinceId], [StateProvinceName], [StateProvinceCode], [IsDeleted], [MyOfficeId], [IsMain]) VALUES (41, N'Nam Định', N'NDH', 0, 2, 0)
INSERT [dbo].[StateProvinces] ([StateProvinceId], [StateProvinceName], [StateProvinceCode], [IsDeleted], [MyOfficeId], [IsMain]) VALUES (42, N'Nghệ An', N'NAN', 0, 2, 0)
INSERT [dbo].[StateProvinces] ([StateProvinceId], [StateProvinceName], [StateProvinceCode], [IsDeleted], [MyOfficeId], [IsMain]) VALUES (43, N'Ninh Bình', N'NBH', 0, 2, 0)
INSERT [dbo].[StateProvinces] ([StateProvinceId], [StateProvinceName], [StateProvinceCode], [IsDeleted], [MyOfficeId], [IsMain]) VALUES (44, N'Ninh Thuận', N'NTN', 0, 1, 0)
INSERT [dbo].[StateProvinces] ([StateProvinceId], [StateProvinceName], [StateProvinceCode], [IsDeleted], [MyOfficeId], [IsMain]) VALUES (45, N'Phú Thọ', N'PTO', 0, 2, 0)
INSERT [dbo].[StateProvinces] ([StateProvinceId], [StateProvinceName], [StateProvinceCode], [IsDeleted], [MyOfficeId], [IsMain]) VALUES (46, N'Phú Yên', N'PYN', 0, 1, 0)
INSERT [dbo].[StateProvinces] ([StateProvinceId], [StateProvinceName], [StateProvinceCode], [IsDeleted], [MyOfficeId], [IsMain]) VALUES (47, N'Quảng Bình', N'QBH', 0, 2, 0)
INSERT [dbo].[StateProvinces] ([StateProvinceId], [StateProvinceName], [StateProvinceCode], [IsDeleted], [MyOfficeId], [IsMain]) VALUES (48, N'Quảng Nam', N'QNM', 0, 2, 0)
INSERT [dbo].[StateProvinces] ([StateProvinceId], [StateProvinceName], [StateProvinceCode], [IsDeleted], [MyOfficeId], [IsMain]) VALUES (49, N'Quảng Ngãi', N'QNI', 0, 2, 0)
INSERT [dbo].[StateProvinces] ([StateProvinceId], [StateProvinceName], [StateProvinceCode], [IsDeleted], [MyOfficeId], [IsMain]) VALUES (50, N'Quảng Ninh', N'QNH', 0, 2, 0)
INSERT [dbo].[StateProvinces] ([StateProvinceId], [StateProvinceName], [StateProvinceCode], [IsDeleted], [MyOfficeId], [IsMain]) VALUES (51, N'Quảng Trị', N'QTI', 0, 2, 0)
INSERT [dbo].[StateProvinces] ([StateProvinceId], [StateProvinceName], [StateProvinceCode], [IsDeleted], [MyOfficeId], [IsMain]) VALUES (52, N'Sóc Trăng', N'STG', 0, 1, 0)
INSERT [dbo].[StateProvinces] ([StateProvinceId], [StateProvinceName], [StateProvinceCode], [IsDeleted], [MyOfficeId], [IsMain]) VALUES (53, N'Sơn La', N'SLA', 0, 2, 0)
INSERT [dbo].[StateProvinces] ([StateProvinceId], [StateProvinceName], [StateProvinceCode], [IsDeleted], [MyOfficeId], [IsMain]) VALUES (54, N'Tây Ninh', N'TNH', 0, 1, 0)
INSERT [dbo].[StateProvinces] ([StateProvinceId], [StateProvinceName], [StateProvinceCode], [IsDeleted], [MyOfficeId], [IsMain]) VALUES (55, N'Thái Bình', N'TBH', 0, 2, 0)
INSERT [dbo].[StateProvinces] ([StateProvinceId], [StateProvinceName], [StateProvinceCode], [IsDeleted], [MyOfficeId], [IsMain]) VALUES (56, N'Thái Nguyên', N'TNN', 0, 2, 0)
INSERT [dbo].[StateProvinces] ([StateProvinceId], [StateProvinceName], [StateProvinceCode], [IsDeleted], [MyOfficeId], [IsMain]) VALUES (57, N'Thanh Hóa', N'THA', 0, 2, 0)
INSERT [dbo].[StateProvinces] ([StateProvinceId], [StateProvinceName], [StateProvinceCode], [IsDeleted], [MyOfficeId], [IsMain]) VALUES (58, N'Thừa Thiên - Huế', N'TTH', 0, 2, 0)
INSERT [dbo].[StateProvinces] ([StateProvinceId], [StateProvinceName], [StateProvinceCode], [IsDeleted], [MyOfficeId], [IsMain]) VALUES (59, N'Tiền Giang', N'TGG', 0, 1, 0)
INSERT [dbo].[StateProvinces] ([StateProvinceId], [StateProvinceName], [StateProvinceCode], [IsDeleted], [MyOfficeId], [IsMain]) VALUES (60, N'Trà Vinh', N'TVH', 0, 1, 0)
INSERT [dbo].[StateProvinces] ([StateProvinceId], [StateProvinceName], [StateProvinceCode], [IsDeleted], [MyOfficeId], [IsMain]) VALUES (61, N'Tuyên Quang', N'TQG', 0, 2, 0)
INSERT [dbo].[StateProvinces] ([StateProvinceId], [StateProvinceName], [StateProvinceCode], [IsDeleted], [MyOfficeId], [IsMain]) VALUES (62, N'Vĩnh Long', N'VLG', 0, 1, 0)
INSERT [dbo].[StateProvinces] ([StateProvinceId], [StateProvinceName], [StateProvinceCode], [IsDeleted], [MyOfficeId], [IsMain]) VALUES (63, N'Vĩnh Phúc', N'VPC', 0, 2, 0)
INSERT [dbo].[StateProvinces] ([StateProvinceId], [StateProvinceName], [StateProvinceCode], [IsDeleted], [MyOfficeId], [IsMain]) VALUES (64, N'Yên Bái', N'YBI', 0, 2, 0)
SET IDENTITY_INSERT [dbo].[StateProvinces] OFF
/****** Object:  Table [dbo].[ShippingServices]    Script Date: 01/12/2015 23:03:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ShippingServices](
	[ShippingServiceId] [int] IDENTITY(1,1) NOT NULL,
	[ShippingServiceName] [nvarchar](128) NULL,
	[Address] [nvarchar](128) NULL,
	[PhoneNumber] [nvarchar](24) NULL,
	[CellPhoneNumber] [nvarchar](24) NULL,
	[IsDeleted] [bit] NOT NULL,
	[MyOfficeId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ShippingServiceId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[ShippingServices] ON
INSERT [dbo].[ShippingServices] ([ShippingServiceId], [ShippingServiceName], [Address], [PhoneNumber], [CellPhoneNumber], [IsDeleted], [MyOfficeId]) VALUES (1, N'TP.HCM', NULL, N'098867868', NULL, 0, 1)
INSERT [dbo].[ShippingServices] ([ShippingServiceId], [ShippingServiceName], [Address], [PhoneNumber], [CellPhoneNumber], [IsDeleted], [MyOfficeId]) VALUES (2, N'KonTum', NULL, N'098867868', NULL, 0, 1)
SET IDENTITY_INSERT [dbo].[ShippingServices] OFF
/****** Object:  Table [dbo].[Roles]    Script Date: 01/12/2015 23:03:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[RoleId] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Roles] ON
INSERT [dbo].[Roles] ([RoleId], [RoleName], [Description]) VALUES (1, N'Admin', N'Admin')
INSERT [dbo].[Roles] ([RoleId], [RoleName], [Description]) VALUES (2, N'Nhap Lieu', N'Nhap Lieu')
INSERT [dbo].[Roles] ([RoleId], [RoleName], [Description]) VALUES (3, N'Ke Toan', N'Ke Toan')
INSERT [dbo].[Roles] ([RoleId], [RoleName], [Description]) VALUES (4, N'Nhan Vien In', N'Nhan Vien In')
INSERT [dbo].[Roles] ([RoleId], [RoleName], [Description]) VALUES (5, N'NV Giao Hang', N'NV Giao Hang')
SET IDENTITY_INSERT [dbo].[Roles] OFF
/****** Object:  Table [dbo].[Users]    Script Date: 01/12/2015 23:03:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](50) NULL,
	[LastName] [nvarchar](50) NULL,
	[EmailAddress] [nvarchar](50) NOT NULL,
	[CompanyName] [nvarchar](max) NULL,
	[ContactNumber] [nvarchar](50) NULL,
	[Address1] [nvarchar](128) NULL,
	[Address2] [nvarchar](128) NULL,
	[TownCity] [nvarchar](50) NULL,
	[PostCodeZip] [nvarchar](50) NULL,
	[CountryRegion] [nvarchar](10) NULL,
	[Password] [nvarchar](50) NULL,
	[Username] [nvarchar](50) NULL,
	[PasswordSalt] [nvarchar](50) NULL,
	[IsApproved] [bit] NOT NULL,
	[IsLockedOut] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[IsOnline] [bit] NOT NULL,
	[NeedChangePassWord] [bit] NOT NULL,
	[NeedRemindChangePassword] [bit] NOT NULL,
	[CreateDate] [datetime] NULL,
	[LastLogInDate] [datetime] NULL,
	[LastActivityDate] [datetime] NULL,
	[LastLockedOutDate] [datetime] NULL,
	[LastPasswordChangedDate] [datetime] NULL,
	[AllowLoginFrom] [datetime] NULL,
	[AllowLoginTo] [datetime] NULL,
	[ProfileImage] [nvarchar](max) NULL,
	[UserImage] [nvarchar](max) NULL,
	[TimeZone] [nvarchar](50) NULL,
	[ColourScheme] [nvarchar](25) NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Users] ON
INSERT [dbo].[Users] ([UserId], [FirstName], [LastName], [EmailAddress], [CompanyName], [ContactNumber], [Address1], [Address2], [TownCity], [PostCodeZip], [CountryRegion], [Password], [Username], [PasswordSalt], [IsApproved], [IsLockedOut], [IsDeleted], [IsOnline], [NeedChangePassWord], [NeedRemindChangePassword], [CreateDate], [LastLogInDate], [LastActivityDate], [LastLockedOutDate], [LastPasswordChangedDate], [AllowLoginFrom], [AllowLoginTo], [ProfileImage], [UserImage], [TimeZone], [ColourScheme]) VALUES (1, N'Le Quang', N'Nguyên', N'lequangnguyenqn@gmail.com', N'Company Name', N'01534 123445', N'12 Road Name', N'', N'Huddersfield', N'HD7 2DS', N'US', N'3zpa/dnPuv0kSNcUvuVCaQ1JaBg=', N'brierlytom@gmail.com', N'eQqP3SNq35h8+e+4AiXp3w==', 1, 0, 0, 1, 0, 0, CAST(0x0000A41E017B4C71 AS DateTime), CAST(0x0000A41E017B933F AS DateTime), CAST(0x0000A41E017B933F AS DateTime), NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Users] ([UserId], [FirstName], [LastName], [EmailAddress], [CompanyName], [ContactNumber], [Address1], [Address2], [TownCity], [PostCodeZip], [CountryRegion], [Password], [Username], [PasswordSalt], [IsApproved], [IsLockedOut], [IsDeleted], [IsOnline], [NeedChangePassWord], [NeedRemindChangePassword], [CreateDate], [LastLogInDate], [LastActivityDate], [LastLockedOutDate], [LastPasswordChangedDate], [AllowLoginFrom], [AllowLoginTo], [ProfileImage], [UserImage], [TimeZone], [ColourScheme]) VALUES (2, N'Kế Toán', N'Trưởng', N'KeToan@gmail.com', N'Company Name', N'01534 123445', N'12 Road Name', N'', N'Huddersfield', N'HD7 2DS', N'US', N'nDFJZo44H4Hw8cwh0XHzE2XZwz8=', N'KeToan@gmail.com', N'rvjtf/9CYOCULgpqXOAkUA==', 1, 0, 0, 0, 0, 0, CAST(0x0000A41E017B4CBF AS DateTime), NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Users] ([UserId], [FirstName], [LastName], [EmailAddress], [CompanyName], [ContactNumber], [Address1], [Address2], [TownCity], [PostCodeZip], [CountryRegion], [Password], [Username], [PasswordSalt], [IsApproved], [IsLockedOut], [IsDeleted], [IsOnline], [NeedChangePassWord], [NeedRemindChangePassword], [CreateDate], [LastLogInDate], [LastActivityDate], [LastLockedOutDate], [LastPasswordChangedDate], [AllowLoginFrom], [AllowLoginTo], [ProfileImage], [UserImage], [TimeZone], [ColourScheme]) VALUES (3, N'Nhan Vien', N'In', N'NhanVienIn@gmail.com', N'Company Name', N'01534 123445', N'12 Road Name', N'', N'Huddersfield', N'HD7 2DS', N'US', N'nDFJZo44H4Hw8cwh0XHzE2XZwz8=', N'NhanVienIn@gmail.com', N'rvjtf/9CYOCULgpqXOAkUA==', 1, 0, 0, 0, 0, 0, CAST(0x0000A41E017B4CC0 AS DateTime), NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Users] ([UserId], [FirstName], [LastName], [EmailAddress], [CompanyName], [ContactNumber], [Address1], [Address2], [TownCity], [PostCodeZip], [CountryRegion], [Password], [Username], [PasswordSalt], [IsApproved], [IsLockedOut], [IsDeleted], [IsOnline], [NeedChangePassWord], [NeedRemindChangePassword], [CreateDate], [LastLogInDate], [LastActivityDate], [LastLockedOutDate], [LastPasswordChangedDate], [AllowLoginFrom], [AllowLoginTo], [ProfileImage], [UserImage], [TimeZone], [ColourScheme]) VALUES (4, N'Nhận', N'Đơn Hàng', N'NhapLieu@gmail.com', N'Company Name', N'01534 123445', N'12 Road Name', N'', N'Huddersfield', N'HD7 2DS', N'US', N'nDFJZo44H4Hw8cwh0XHzE2XZwz8=', N'NhapLieu@gmail.com', N'rvjtf/9CYOCULgpqXOAkUA==', 1, 0, 0, 0, 0, 0, CAST(0x0000A41E017B4CC1 AS DateTime), NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Users] ([UserId], [FirstName], [LastName], [EmailAddress], [CompanyName], [ContactNumber], [Address1], [Address2], [TownCity], [PostCodeZip], [CountryRegion], [Password], [Username], [PasswordSalt], [IsApproved], [IsLockedOut], [IsDeleted], [IsOnline], [NeedChangePassWord], [NeedRemindChangePassword], [CreateDate], [LastLogInDate], [LastActivityDate], [LastLockedOutDate], [LastPasswordChangedDate], [AllowLoginFrom], [AllowLoginTo], [ProfileImage], [UserImage], [TimeZone], [ColourScheme]) VALUES (5, N'Giao', N'Nhận', N'NVGiaoHang@gmail.com', N'Company Name', N'01534 123445', N'12 Road Name', N'', N'Huddersfield', N'HD7 2DS', N'US', N'nDFJZo44H4Hw8cwh0XHzE2XZwz8=', N'NVGiaoHang@gmail.com', N'rvjtf/9CYOCULgpqXOAkUA==', 1, 0, 0, 0, 0, 0, CAST(0x0000A41E017B4CC1 AS DateTime), NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[Users] OFF
/****** Object:  Table [dbo].[Expenses]    Script Date: 01/12/2015 23:03:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Expenses](
	[ExpenseId] [int] IDENTITY(1,1) NOT NULL,
	[ExpenseName] [nvarchar](max) NULL,
	[Cost] [decimal](18, 2) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[Note] [nvarchar](max) NULL,
	[CreateUserId] [int] NOT NULL,
	[MyOfficeId] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ExpenseId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EdmMetadata]    Script Date: 01/12/2015 23:03:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EdmMetadata](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ModelHash] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[EdmMetadata] ON
INSERT [dbo].[EdmMetadata] ([Id], [ModelHash]) VALUES (1, N'DF5D98F6E408D1137B83C865ACDADC8D5EB672483CCF922DC8392708D7F25ACF')
SET IDENTITY_INSERT [dbo].[EdmMetadata] OFF
/****** Object:  Table [dbo].[Customers]    Script Date: 01/12/2015 23:03:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customers](
	[CustomerId] [int] IDENTITY(1,1) NOT NULL,
	[CustomerName] [nvarchar](128) NOT NULL,
	[CustomerShortName] [nvarchar](128) NULL,
	[CustomerCode] [nvarchar](128) NULL,
	[Address] [nvarchar](128) NULL,
	[PhoneNumber] [nvarchar](24) NULL,
	[Fax] [nvarchar](24) NULL,
	[CellPhoneNumber] [nvarchar](24) NULL,
	[Note] [nvarchar](max) NULL,
	[IsDeleted] [bit] NOT NULL,
	[DoNotDelete] [bit] NOT NULL,
	[MyOfficeId] [int] NOT NULL,
	[DiscountPercent] [int] NOT NULL,
	[UseSpecialRateTable] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CustomerId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Customers] ON
INSERT [dbo].[Customers] ([CustomerId], [CustomerName], [CustomerShortName], [CustomerCode], [Address], [PhoneNumber], [Fax], [CellPhoneNumber], [Note], [IsDeleted], [DoNotDelete], [MyOfficeId], [DiscountPercent], [UseSpecialRateTable]) VALUES (1, N'ACB', NULL, N'ACB', NULL, NULL, NULL, N'099787878', NULL, 0, 0, 1, 0, 0)
INSERT [dbo].[Customers] ([CustomerId], [CustomerName], [CustomerShortName], [CustomerCode], [Address], [PhoneNumber], [Fax], [CellPhoneNumber], [Note], [IsDeleted], [DoNotDelete], [MyOfficeId], [DiscountPercent], [UseSpecialRateTable]) VALUES (2, N'ANZ', NULL, N'ANZ', NULL, NULL, NULL, N'099787878', NULL, 0, 0, 1, 0, 0)
INSERT [dbo].[Customers] ([CustomerId], [CustomerName], [CustomerShortName], [CustomerCode], [Address], [PhoneNumber], [Fax], [CellPhoneNumber], [Note], [IsDeleted], [DoNotDelete], [MyOfficeId], [DiscountPercent], [UseSpecialRateTable]) VALUES (3, N'HSBC', NULL, N'HSBC', NULL, NULL, NULL, N'099787878', NULL, 0, 0, 1, 0, 0)
INSERT [dbo].[Customers] ([CustomerId], [CustomerName], [CustomerShortName], [CustomerCode], [Address], [PhoneNumber], [Fax], [CellPhoneNumber], [Note], [IsDeleted], [DoNotDelete], [MyOfficeId], [DiscountPercent], [UseSpecialRateTable]) VALUES (4, N'BIDV', NULL, N'BIDV', NULL, NULL, NULL, N'099787878', NULL, 0, 0, 1, 0, 0)
SET IDENTITY_INSERT [dbo].[Customers] OFF
/****** Object:  Table [dbo].[CustomerLogoes]    Script Date: 01/12/2015 23:03:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomerLogoes](
	[CustomerLogoId] [int] IDENTITY(1,1) NOT NULL,
	[CustomerName] [nvarchar](128) NULL,
	[LogoUrl] [nvarchar](128) NULL,
	[HomePageUrl] [nvarchar](128) NULL,
PRIMARY KEY CLUSTERED 
(
	[CustomerLogoId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Categories]    Script Date: 01/12/2015 23:03:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[CategoryId] [int] IDENTITY(1,1) NOT NULL,
	[CategoryName] [nvarchar](128) NOT NULL,
	[Desciption] [nvarchar](128) NULL,
	[CategoryCode] [nvarchar](128) NULL,
	[IsDeleted] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CategoryId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Categories] ON
INSERT [dbo].[Categories] ([CategoryId], [CategoryName], [Desciption], [CategoryCode], [IsDeleted]) VALUES (1, N'H7', N'H7', N'H7', 0)
INSERT [dbo].[Categories] ([CategoryId], [CategoryName], [Desciption], [CategoryCode], [IsDeleted]) VALUES (2, N'H8', N'H8', N'H8', 0)
INSERT [dbo].[Categories] ([CategoryId], [CategoryName], [Desciption], [CategoryCode], [IsDeleted]) VALUES (3, N'H9', N'H9', N'H9', 0)
SET IDENTITY_INSERT [dbo].[Categories] OFF
/****** Object:  Table [dbo].[MyOffices]    Script Date: 01/12/2015 23:03:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MyOffices](
	[MyOfficeId] [int] IDENTITY(1,1) NOT NULL,
	[OfficeName] [nvarchar](128) NULL,
	[Address] [nvarchar](128) NULL,
	[PhoneNumber] [nvarchar](24) NULL,
	[Fax] [nvarchar](24) NULL,
	[CellPhoneNumber] [nvarchar](24) NULL,
	[IsDeleted] [bit] NOT NULL,
	[IsRetailCustomer] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[MyOfficeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[MyOffices] ON
INSERT [dbo].[MyOffices] ([MyOfficeId], [OfficeName], [Address], [PhoneNumber], [Fax], [CellPhoneNumber], [IsDeleted], [IsRetailCustomer]) VALUES (1, N'Tp.Hồ Chí Minh', N'Tp.Hồ Chí Minh', N'08.3253.6528', NULL, NULL, 0, 0)
INSERT [dbo].[MyOffices] ([MyOfficeId], [OfficeName], [Address], [PhoneNumber], [Fax], [CellPhoneNumber], [IsDeleted], [IsRetailCustomer]) VALUES (2, N'Khách Lẻ - Tp.Hồ Chí Minh', N'Khách Lẻ - Tp.Hồ Chí Minh', N'08.3253.6528', NULL, NULL, 0, 1)
INSERT [dbo].[MyOffices] ([MyOfficeId], [OfficeName], [Address], [PhoneNumber], [Fax], [CellPhoneNumber], [IsDeleted], [IsRetailCustomer]) VALUES (3, N'Hà Nội', N'Hà Nội', N'04.3236.3265', NULL, NULL, 0, 0)
INSERT [dbo].[MyOffices] ([MyOfficeId], [OfficeName], [Address], [PhoneNumber], [Fax], [CellPhoneNumber], [IsDeleted], [IsRetailCustomer]) VALUES (4, N'Khách Lẻ - Hà Nội', N'Khách Lẻ - Hà Nội', N'04.3236.3265', NULL, NULL, 0, 1)
SET IDENTITY_INSERT [dbo].[MyOffices] OFF
/****** Object:  Table [dbo].[ExtraFees]    Script Date: 01/12/2015 23:03:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExtraFees](
	[ExtraFeeId] [int] IDENTITY(1,1) NOT NULL,
	[ExtraFeeName] [nvarchar](max) NULL,
	[AmountFrom] [int] NOT NULL,
	[AmountTo] [int] NOT NULL,
	[Cost] [decimal](18, 2) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ExtraFeeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[ExtraFees] ON
INSERT [dbo].[ExtraFees] ([ExtraFeeId], [ExtraFeeName], [AmountFrom], [AmountTo], [Cost], [IsDeleted]) VALUES (1, N'Phụ thu 30 nghìn cho đơn hàng từ 0 -> 200 thiệp', 0, 200, CAST(30000.00 AS Decimal(18, 2)), 0)
SET IDENTITY_INSERT [dbo].[ExtraFees] OFF
/****** Object:  Table [dbo].[NewsCategoryItems]    Script Date: 01/12/2015 23:03:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NewsCategoryItems](
	[NewsCategoryItemId] [int] IDENTITY(1,1) NOT NULL,
	[CategoryName] [nvarchar](128) NOT NULL,
	[CategoryDescription] [nvarchar](128) NULL,
	[Level] [int] NOT NULL,
	[ParentId] [int] NULL,
	[Active] [bit] NOT NULL,
	[DisplayOrder] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[NewsCategoryItemId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[NewsCategoryItems] ON
INSERT [dbo].[NewsCategoryItems] ([NewsCategoryItemId], [CategoryName], [CategoryDescription], [Level], [ParentId], [Active], [DisplayOrder], [IsDeleted]) VALUES (1, N'Tin tức', N'Tin tức', 0, NULL, 1, 0, 0)
INSERT [dbo].[NewsCategoryItems] ([NewsCategoryItemId], [CategoryName], [CategoryDescription], [Level], [ParentId], [Active], [DisplayOrder], [IsDeleted]) VALUES (2, N'Tuyển dụng', N'Tuyển dụng', 0, NULL, 1, 0, 0)
SET IDENTITY_INSERT [dbo].[NewsCategoryItems] OFF
/****** Object:  Table [dbo].[MyOfficeUsers]    Script Date: 01/12/2015 23:03:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MyOfficeUsers](
	[MyOffice_MyOfficeId] [int] NOT NULL,
	[User_UserId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[MyOffice_MyOfficeId] ASC,
	[User_UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Products]    Script Date: 01/12/2015 23:03:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products](
	[ProductId] [int] IDENTITY(1,1) NOT NULL,
	[ProductName] [nvarchar](128) NOT NULL,
	[Desciption] [nvarchar](128) NULL,
	[ProductCode] [nvarchar](128) NULL,
	[DefaultOriginalPrice] [decimal](18, 2) NOT NULL,
	[DefaultPrice] [decimal](18, 2) NOT NULL,
	[DefaultOriginalPrintingIncludeImagePrice] [decimal](18, 2) NOT NULL,
	[DefaultPrintingIncludeImagePrice] [decimal](18, 2) NOT NULL,
	[DefaultPrintingWithoutImagePrice] [decimal](18, 2) NOT NULL,
	[DefaultOriginalPrintingWithoutImagePrice] [decimal](18, 2) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CategoryId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ProductId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Products] ON
INSERT [dbo].[Products] ([ProductId], [ProductName], [Desciption], [ProductCode], [DefaultOriginalPrice], [DefaultPrice], [DefaultOriginalPrintingIncludeImagePrice], [DefaultPrintingIncludeImagePrice], [DefaultPrintingWithoutImagePrice], [DefaultOriginalPrintingWithoutImagePrice], [IsDeleted], [CategoryId]) VALUES (1, N'H7 BIA', NULL, N'H7BIA', CAST(4000.00 AS Decimal(18, 2)), CAST(5000.00 AS Decimal(18, 2)), CAST(200.00 AS Decimal(18, 2)), CAST(300.00 AS Decimal(18, 2)), CAST(200.00 AS Decimal(18, 2)), CAST(100.00 AS Decimal(18, 2)), 0, 1)
INSERT [dbo].[Products] ([ProductId], [ProductName], [Desciption], [ProductCode], [DefaultOriginalPrice], [DefaultPrice], [DefaultOriginalPrintingIncludeImagePrice], [DefaultPrintingIncludeImagePrice], [DefaultPrintingWithoutImagePrice], [DefaultOriginalPrintingWithoutImagePrice], [IsDeleted], [CategoryId]) VALUES (2, N'H7 RUOT', NULL, N'H7RUOT', CAST(4000.00 AS Decimal(18, 2)), CAST(5000.00 AS Decimal(18, 2)), CAST(200.00 AS Decimal(18, 2)), CAST(300.00 AS Decimal(18, 2)), CAST(200.00 AS Decimal(18, 2)), CAST(100.00 AS Decimal(18, 2)), 0, 1)
INSERT [dbo].[Products] ([ProductId], [ProductName], [Desciption], [ProductCode], [DefaultOriginalPrice], [DefaultPrice], [DefaultOriginalPrintingIncludeImagePrice], [DefaultPrintingIncludeImagePrice], [DefaultPrintingWithoutImagePrice], [DefaultOriginalPrintingWithoutImagePrice], [IsDeleted], [CategoryId]) VALUES (3, N'H8 BIA', NULL, N'H8BIA', CAST(6000.00 AS Decimal(18, 2)), CAST(7000.00 AS Decimal(18, 2)), CAST(200.00 AS Decimal(18, 2)), CAST(300.00 AS Decimal(18, 2)), CAST(200.00 AS Decimal(18, 2)), CAST(100.00 AS Decimal(18, 2)), 0, 2)
INSERT [dbo].[Products] ([ProductId], [ProductName], [Desciption], [ProductCode], [DefaultOriginalPrice], [DefaultPrice], [DefaultOriginalPrintingIncludeImagePrice], [DefaultPrintingIncludeImagePrice], [DefaultPrintingWithoutImagePrice], [DefaultOriginalPrintingWithoutImagePrice], [IsDeleted], [CategoryId]) VALUES (4, N'H8 RUOT', NULL, N'H8RUOT', CAST(6000.00 AS Decimal(18, 2)), CAST(7000.00 AS Decimal(18, 2)), CAST(200.00 AS Decimal(18, 2)), CAST(300.00 AS Decimal(18, 2)), CAST(200.00 AS Decimal(18, 2)), CAST(100.00 AS Decimal(18, 2)), 0, 2)
SET IDENTITY_INSERT [dbo].[Products] OFF
/****** Object:  Table [dbo].[UserRoles]    Script Date: 01/12/2015 23:03:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRoles](
	[User_UserId] [int] NOT NULL,
	[Role_RoleId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[User_UserId] ASC,
	[Role_RoleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[UserRoles] ([User_UserId], [Role_RoleId]) VALUES (1, 1)
INSERT [dbo].[UserRoles] ([User_UserId], [Role_RoleId]) VALUES (2, 3)
INSERT [dbo].[UserRoles] ([User_UserId], [Role_RoleId]) VALUES (3, 4)
INSERT [dbo].[UserRoles] ([User_UserId], [Role_RoleId]) VALUES (4, 2)
INSERT [dbo].[UserRoles] ([User_UserId], [Role_RoleId]) VALUES (5, 5)
/****** Object:  Table [dbo].[StorageUsers]    Script Date: 01/12/2015 23:03:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StorageUsers](
	[Storage_StorageId] [int] NOT NULL,
	[User_UserId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Storage_StorageId] ASC,
	[User_UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NewsItems]    Script Date: 01/12/2015 23:03:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NewsItems](
	[NewsItemId] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](128) NULL,
	[ImageUrl] [nvarchar](128) NULL,
	[Short] [nvarchar](512) NULL,
	[Full] [nvarchar](max) NULL,
	[Published] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[NewsCategoryItemId] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[NewsItemId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Orders]    Script Date: 01/12/2015 23:03:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[OrderId] [int] IDENTITY(1,1) NOT NULL,
	[MyOfficeId] [int] NOT NULL,
	[CustomerId] [int] NOT NULL,
	[ShippingServiceId] [int] NULL,
	[RealShippingServiceId] [int] NULL,
	[CreateDate] [datetime] NOT NULL,
	[FileApproval] [datetime] NULL,
	[PrintDoneAt] [datetime] NULL,
	[PrintByUserId] [int] NULL,
	[DeliveredDate] [datetime] NULL,
	[DeliveredUserId] [int] NULL,
	[Note] [nvarchar](max) NULL,
	[CreateUserId] [int] NOT NULL,
	[UpdateUserId] [int] NULL,
	[IsDeleted] [bit] NOT NULL,
	[ShowOnTop] [bit] NOT NULL,
	[PaymentType] [int] NOT NULL,
	[Status] [int] NOT NULL,
	[Paid] [bit] NOT NULL,
	[WaitForPrint] [bit] NOT NULL,
	[Stopping] [bit] NOT NULL,
	[TotalCost] [decimal](18, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[OrderId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderDetails]    Script Date: 01/12/2015 23:03:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderDetails](
	[OrderDetailId] [int] IDENTITY(1,1) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[FilePath] [nvarchar](max) NULL,
	[Note] [nvarchar](max) NULL,
	[CreateUserId] [int] NOT NULL,
	[UpdateUserId] [int] NULL,
	[IsDeleted] [bit] NOT NULL,
	[TotalCost] [decimal](18, 2) NOT NULL,
	[PrintIncludeImage] [bit] NOT NULL,
	[PrintWithoutImage] [bit] NOT NULL,
	[Amount] [int] NOT NULL,
	[OrderId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[StorageId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[OrderDetailId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RateMappings]    Script Date: 01/12/2015 23:03:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RateMappings](
	[RateMappingId] [int] IDENTITY(1,1) NOT NULL,
	[ProductId] [int] NOT NULL,
	[MyOfficeId] [int] NOT NULL,
	[Price] [decimal](18, 2) NOT NULL,
	[PrintingPrice] [decimal](18, 2) NOT NULL,
	[OriginalPrice] [decimal](18, 2) NOT NULL,
	[OriginalPrintingPrice] [decimal](18, 2) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[RateMappingId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductInStorages]    Script Date: 01/12/2015 23:03:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductInStorages](
	[ProductInStorageId] [int] IDENTITY(1,1) NOT NULL,
	[ProductId] [int] NOT NULL,
	[StorageId] [int] NOT NULL,
	[Amount] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ProductInStorageId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ExportTracks]    Script Date: 01/12/2015 23:03:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExportTracks](
	[ExportTrackId] [int] IDENTITY(1,1) NOT NULL,
	[Quantity] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[Note] [nvarchar](max) NULL,
	[CreateUserId] [int] NOT NULL,
	[ReceivedUserId] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[MyOfficeId] [int] NOT NULL,
	[Amount] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[StorageId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ExportTrackId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ImportTracks]    Script Date: 01/12/2015 23:03:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ImportTracks](
	[ImportTrackId] [int] IDENTITY(1,1) NOT NULL,
	[Amount] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[Note] [nvarchar](max) NULL,
	[CreateUserId] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[ProductId] [int] NOT NULL,
	[StorageId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ImportTrackId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  ForeignKey [NewsCategoryItem_Parent]    Script Date: 01/12/2015 23:03:18 ******/
ALTER TABLE [dbo].[NewsCategoryItems]  WITH CHECK ADD  CONSTRAINT [NewsCategoryItem_Parent] FOREIGN KEY([ParentId])
REFERENCES [dbo].[NewsCategoryItems] ([NewsCategoryItemId])
GO
ALTER TABLE [dbo].[NewsCategoryItems] CHECK CONSTRAINT [NewsCategoryItem_Parent]
GO
/****** Object:  ForeignKey [MyOffice_Users_Source]    Script Date: 01/12/2015 23:03:18 ******/
ALTER TABLE [dbo].[MyOfficeUsers]  WITH CHECK ADD  CONSTRAINT [MyOffice_Users_Source] FOREIGN KEY([MyOffice_MyOfficeId])
REFERENCES [dbo].[MyOffices] ([MyOfficeId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[MyOfficeUsers] CHECK CONSTRAINT [MyOffice_Users_Source]
GO
/****** Object:  ForeignKey [MyOffice_Users_Target]    Script Date: 01/12/2015 23:03:18 ******/
ALTER TABLE [dbo].[MyOfficeUsers]  WITH CHECK ADD  CONSTRAINT [MyOffice_Users_Target] FOREIGN KEY([User_UserId])
REFERENCES [dbo].[Users] ([UserId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[MyOfficeUsers] CHECK CONSTRAINT [MyOffice_Users_Target]
GO
/****** Object:  ForeignKey [Product_Category]    Script Date: 01/12/2015 23:03:18 ******/
ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [Product_Category] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Categories] ([CategoryId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [Product_Category]
GO
/****** Object:  ForeignKey [User_Roles_Source]    Script Date: 01/12/2015 23:03:18 ******/
ALTER TABLE [dbo].[UserRoles]  WITH CHECK ADD  CONSTRAINT [User_Roles_Source] FOREIGN KEY([User_UserId])
REFERENCES [dbo].[Users] ([UserId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserRoles] CHECK CONSTRAINT [User_Roles_Source]
GO
/****** Object:  ForeignKey [User_Roles_Target]    Script Date: 01/12/2015 23:03:18 ******/
ALTER TABLE [dbo].[UserRoles]  WITH CHECK ADD  CONSTRAINT [User_Roles_Target] FOREIGN KEY([Role_RoleId])
REFERENCES [dbo].[Roles] ([RoleId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserRoles] CHECK CONSTRAINT [User_Roles_Target]
GO
/****** Object:  ForeignKey [Storage_Users_Source]    Script Date: 01/12/2015 23:03:18 ******/
ALTER TABLE [dbo].[StorageUsers]  WITH CHECK ADD  CONSTRAINT [Storage_Users_Source] FOREIGN KEY([Storage_StorageId])
REFERENCES [dbo].[Storages] ([StorageId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[StorageUsers] CHECK CONSTRAINT [Storage_Users_Source]
GO
/****** Object:  ForeignKey [Storage_Users_Target]    Script Date: 01/12/2015 23:03:18 ******/
ALTER TABLE [dbo].[StorageUsers]  WITH CHECK ADD  CONSTRAINT [Storage_Users_Target] FOREIGN KEY([User_UserId])
REFERENCES [dbo].[Users] ([UserId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[StorageUsers] CHECK CONSTRAINT [Storage_Users_Target]
GO
/****** Object:  ForeignKey [NewsItem_Category]    Script Date: 01/12/2015 23:03:18 ******/
ALTER TABLE [dbo].[NewsItems]  WITH CHECK ADD  CONSTRAINT [NewsItem_Category] FOREIGN KEY([NewsCategoryItemId])
REFERENCES [dbo].[NewsCategoryItems] ([NewsCategoryItemId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[NewsItems] CHECK CONSTRAINT [NewsItem_Category]
GO
/****** Object:  ForeignKey [Order_Customer]    Script Date: 01/12/2015 23:03:18 ******/
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [Order_Customer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customers] ([CustomerId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [Order_Customer]
GO
/****** Object:  ForeignKey [Order_MyOffice]    Script Date: 01/12/2015 23:03:18 ******/
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [Order_MyOffice] FOREIGN KEY([MyOfficeId])
REFERENCES [dbo].[MyOffices] ([MyOfficeId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [Order_MyOffice]
GO
/****** Object:  ForeignKey [OrderDetail_Order]    Script Date: 01/12/2015 23:03:18 ******/
ALTER TABLE [dbo].[OrderDetails]  WITH CHECK ADD  CONSTRAINT [OrderDetail_Order] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Orders] ([OrderId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderDetails] CHECK CONSTRAINT [OrderDetail_Order]
GO
/****** Object:  ForeignKey [OrderDetail_Product]    Script Date: 01/12/2015 23:03:18 ******/
ALTER TABLE [dbo].[OrderDetails]  WITH CHECK ADD  CONSTRAINT [OrderDetail_Product] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Products] ([ProductId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderDetails] CHECK CONSTRAINT [OrderDetail_Product]
GO
/****** Object:  ForeignKey [RateMapping_MyOffice]    Script Date: 01/12/2015 23:03:18 ******/
ALTER TABLE [dbo].[RateMappings]  WITH CHECK ADD  CONSTRAINT [RateMapping_MyOffice] FOREIGN KEY([MyOfficeId])
REFERENCES [dbo].[MyOffices] ([MyOfficeId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RateMappings] CHECK CONSTRAINT [RateMapping_MyOffice]
GO
/****** Object:  ForeignKey [RateMapping_Product]    Script Date: 01/12/2015 23:03:18 ******/
ALTER TABLE [dbo].[RateMappings]  WITH CHECK ADD  CONSTRAINT [RateMapping_Product] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Products] ([ProductId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RateMappings] CHECK CONSTRAINT [RateMapping_Product]
GO
/****** Object:  ForeignKey [ProductInStorage_Product]    Script Date: 01/12/2015 23:03:18 ******/
ALTER TABLE [dbo].[ProductInStorages]  WITH CHECK ADD  CONSTRAINT [ProductInStorage_Product] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Products] ([ProductId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ProductInStorages] CHECK CONSTRAINT [ProductInStorage_Product]
GO
/****** Object:  ForeignKey [ProductInStorage_Storage]    Script Date: 01/12/2015 23:03:18 ******/
ALTER TABLE [dbo].[ProductInStorages]  WITH CHECK ADD  CONSTRAINT [ProductInStorage_Storage] FOREIGN KEY([StorageId])
REFERENCES [dbo].[Storages] ([StorageId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ProductInStorages] CHECK CONSTRAINT [ProductInStorage_Storage]
GO
/****** Object:  ForeignKey [ExportTrack_Product]    Script Date: 01/12/2015 23:03:18 ******/
ALTER TABLE [dbo].[ExportTracks]  WITH CHECK ADD  CONSTRAINT [ExportTrack_Product] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Products] ([ProductId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ExportTracks] CHECK CONSTRAINT [ExportTrack_Product]
GO
/****** Object:  ForeignKey [ExportTrack_ReceivedUser]    Script Date: 01/12/2015 23:03:18 ******/
ALTER TABLE [dbo].[ExportTracks]  WITH CHECK ADD  CONSTRAINT [ExportTrack_ReceivedUser] FOREIGN KEY([ReceivedUserId])
REFERENCES [dbo].[Users] ([UserId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ExportTracks] CHECK CONSTRAINT [ExportTrack_ReceivedUser]
GO
/****** Object:  ForeignKey [ExportTrack_Storage]    Script Date: 01/12/2015 23:03:18 ******/
ALTER TABLE [dbo].[ExportTracks]  WITH CHECK ADD  CONSTRAINT [ExportTrack_Storage] FOREIGN KEY([StorageId])
REFERENCES [dbo].[Storages] ([StorageId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ExportTracks] CHECK CONSTRAINT [ExportTrack_Storage]
GO
/****** Object:  ForeignKey [ImportTrack_CreateUser]    Script Date: 01/12/2015 23:03:18 ******/
ALTER TABLE [dbo].[ImportTracks]  WITH CHECK ADD  CONSTRAINT [ImportTrack_CreateUser] FOREIGN KEY([CreateUserId])
REFERENCES [dbo].[Users] ([UserId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ImportTracks] CHECK CONSTRAINT [ImportTrack_CreateUser]
GO
/****** Object:  ForeignKey [ImportTrack_Product]    Script Date: 01/12/2015 23:03:18 ******/
ALTER TABLE [dbo].[ImportTracks]  WITH CHECK ADD  CONSTRAINT [ImportTrack_Product] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Products] ([ProductId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ImportTracks] CHECK CONSTRAINT [ImportTrack_Product]
GO
/****** Object:  ForeignKey [ImportTrack_Storage]    Script Date: 01/12/2015 23:03:18 ******/
ALTER TABLE [dbo].[ImportTracks]  WITH CHECK ADD  CONSTRAINT [ImportTrack_Storage] FOREIGN KEY([StorageId])
REFERENCES [dbo].[Storages] ([StorageId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ImportTracks] CHECK CONSTRAINT [ImportTrack_Storage]
GO

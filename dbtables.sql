USE [Daigou]
GO

/****** Object:  Table [dbo].[Customer]    Script Date: 1/8/2018 4:08:30 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Customer](
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[Address1] [nvarchar](50) NOT NULL,
	[Address2] [nvarchar](50) NULL,
	[City] [nvarchar](50) NOT NULL,
	[State] [nvarchar](50) NOT NULL,
	[Country] [nvarchar](50) NOT NULL,
	[Zip] [nvarchar](50) NOT NULL,
	[Phone] [nvarchar](50) NOT NULL,
	[IM] [nvarchar](50) NULL,
	[IdentificationNumber] [nvarchar](50) NULL,
	[CustomerID] [int] IDENTITY(1,1) NOT NULL,
	[CustomerPicture] [image] NULL,
 CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED 
(
	[CustomerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Merchandise]    Script Date: 1/8/2018 4:08:30 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Merchandise](
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Store] [nvarchar](50) NULL,
	[USDPrice] [decimal](18, 2) NULL,
	[weight] [float] NULL,
	[MerchandiseID] [int] IDENTITY(1,1) NOT NULL,
	[Picture] [image] NULL,
 CONSTRAINT [PK_Merchandise] PRIMARY KEY CLUSTERED 
(
	[MerchandiseID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

/****** Object:  Table [dbo].[OrderItem]    Script Date: 1/8/2018 4:08:30 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OrderItem](
	[OrderID] [int] NOT NULL,
	[MerchandiseID] [int] NOT NULL,
	[Number] [int] NOT NULL,
	[DiscountPercent] [decimal](18, 2) NULL,
	[DiscountValue] [decimal](18, 2) NULL,
	[OrderItemID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_OrderItem] PRIMARY KEY CLUSTERED 
(
	[OrderItemID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Orders]    Script Date: 1/8/2018 4:08:30 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Orders](
	[CustomerID] [int] NOT NULL,
	[OrderDate] [datetime] NOT NULL,
	[ShipmentDate] [datetime] NULL,
	[ShipmentNumber] [nvarchar](50) NULL,
	[ShipmentCarrier] [nvarchar](50) NULL,
	[OrderID] [int] IDENTITY(1,1) NOT NULL,
	[OrderStatus] [int] NULL,
	[ShippingCost] [decimal](18, 2) NULL,
	[ChargedPrice] [decimal](18, 2) NULL,
 CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED 
(
	[OrderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[OrderItem]  WITH CHECK ADD  CONSTRAINT [FK_OrderItem_Merchandise] FOREIGN KEY([MerchandiseID])
REFERENCES [dbo].[Merchandise] ([MerchandiseID])
GO

ALTER TABLE [dbo].[OrderItem] CHECK CONSTRAINT [FK_OrderItem_Merchandise]
GO

ALTER TABLE [dbo].[OrderItem]  WITH CHECK ADD  CONSTRAINT [FK_OrderItem_Orders] FOREIGN KEY([OrderID])
REFERENCES [dbo].[Orders] ([OrderID])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[OrderItem] CHECK CONSTRAINT [FK_OrderItem_Orders]
GO

ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_Customer] FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customer] ([CustomerID])
GO

ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_Customer]
GO



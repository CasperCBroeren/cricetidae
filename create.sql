/****** Object:  Table [dbo].[BonusProductsEvents]    Script Date: 10/19/2019 9:19:13 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BonusProductsEvents](
	[Year] [int] NOT NULL,
	[Week] [int] NOT NULL,
	[Id] [bigint] NOT NULL,
	[StartDate] [date] NULL,
	[EndDate] [date] NULL,
	[FromPriceInCents] [int] NULL,
	[ForPriceInCents] [int] NULL,
	[Title] [nvarchar](250) NULL,
	[DiscountText] [nvarchar](250) NULL,
	[UnitSize] [nvarchar](250) NULL,
	[Category] [nvarchar](250) NULL,
	[Link] [nvarchar](250) NULL,
	[ActiveAtNumberOfProducts] [int] NULL,
	[Brand] [nvarchar](90) NULL,
	[Store] [nvarchar](50) NULL
) ON [PRIMARY]
GO


/****** Object:  View [dbo].[vwBonusProductOccurance]    Script Date: 10/19/2019 9:19:22 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE VIEW [dbo].[vwBonusProductOccurance]
AS
SELECT        COUNT(*) AS BonusOccurance, Id as IdOfProduct
FROM            dbo.BonusProductsEvents
GROUP BY Id
GO
/****** Object:  View [dbo].[vwBonusView]    Script Date: 10/19/2019 9:19:26 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[vwBonusView]
AS
SELECT        dbo.BonusProductsEvents.Year, dbo.BonusProductsEvents.Week, dbo.BonusProductsEvents.Id, dbo.BonusProductsEvents.StartDate, dbo.BonusProductsEvents.EndDate, dbo.BonusProductsEvents.FromPriceInCents, 
                         dbo.BonusProductsEvents.ForPriceInCents, dbo.BonusProductsEvents.FromPriceInCents - dbo.BonusProductsEvents.ForPriceInCents AS Delta, dbo.BonusProductsEvents.Title, dbo.BonusProductsEvents.DiscountText, 
                         dbo.BonusProductsEvents.UnitSize, dbo.BonusProductsEvents.Category, dbo.vwBonusProductOccurance.BonusOccurance, dbo.BonusProductsEvents.Link, dbo.BonusProductsEvents.ActiveAtNumberOfProducts, 
                         dbo.BonusProductsEvents.Brand, dbo.BonusProductsEvents.Store
FROM            dbo.BonusProductsEvents INNER JOIN
                         dbo.vwBonusProductOccurance ON dbo.vwBonusProductOccurance.IdOfProduct = dbo.BonusProductsEvents.Id
GO



USE [EventFlowDemo]
GO

/****** Object:  Table [dbo].[ReadModel-Example]    Script Date: 22/02/2019 13:46:26 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ReadModel-ExampleDuplicate](
	[AggregateId] [nvarchar](255) NULL,
	[MagicNumber] [int] NOT NULL,
	[Version] [int] NULL
) ON [PRIMARY]

GO



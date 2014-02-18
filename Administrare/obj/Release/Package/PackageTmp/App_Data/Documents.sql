/****** Object:  Table [dbo].[Documents]    Script Date: 02/09/2013 21:07:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Documents](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[PathPDF] [nvarchar](50) NULL,
	[PathHTML] [nvarchar](50) NULL,
	[AddedBy] [int] NULL,
	[IdCategorie] [int] NULL,
	[IsActive] [bit] NULL,
	[CreationDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
 CONSTRAINT [PK_Documents] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO



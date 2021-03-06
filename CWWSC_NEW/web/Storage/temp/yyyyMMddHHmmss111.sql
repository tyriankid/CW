USE [Mvc_tset]
GO
/****** Object:  Table [dbo].[UsreInfo]    Script Date: 2016/6/15 星期三 上午 10:44:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UsreInfo](
	[Userid] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](50) NULL,
	[UserPass] [nvarchar](50) NULL,
	[UserData] [datetime] NULL,
 CONSTRAINT [PK_UsreInfo] PRIMARY KEY CLUSTERED 
(
	[Userid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[UsreInfo] ADD  CONSTRAINT [DF_UsreInfo_UserData]  DEFAULT ((1)) FOR [UserData]
GO

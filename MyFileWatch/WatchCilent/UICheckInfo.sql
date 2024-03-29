USE [WisoftTestSv]
GO
/****** 对象:  Table [dbo].[UICheckInfo]    脚本日期: 05/21/2012 17:58:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UICheckInfo](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[checkno] [nvarchar](20) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[packageid] [int] NOT NULL CONSTRAINT [DF_UICheckList_packageid]  DEFAULT ((0)),
	[packagename] [nvarchar](255) COLLATE Chinese_PRC_CI_AS NULL,
	[adminid] [int] NOT NULL CONSTRAINT [DF_UICheckInfo_adminid]  DEFAULT ((0)),
	[adminname] [nvarchar](255) COLLATE Chinese_PRC_CI_AS NULL,
	[moduleid] [int] NOT NULL CONSTRAINT [DF_UICheckInfo_moduleid]  DEFAULT ((0)),
	[modulename] [nvarchar](255) COLLATE Chinese_PRC_CI_AS NULL,
	[projectid] [int] NOT NULL CONSTRAINT [DF_UICheckInfo_projectid]  DEFAULT ((0)),
	[projectname] [nvarchar](255) COLLATE Chinese_PRC_CI_AS NULL,
	[createtime] [nvarchar](255) COLLATE Chinese_PRC_CI_AS NULL,
	[checkedtime] [nvarchar](255) COLLATE Chinese_PRC_CI_AS NULL,
	[checkerid] [int] NOT NULL CONSTRAINT [DF_UICheckList_checkerid]  DEFAULT ((0)),
	[checkername] [nvarchar](255) COLLATE Chinese_PRC_CI_AS NULL,
	[state] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[shotimage] [nvarchar](500) COLLATE Chinese_PRC_CI_AS NULL
) ON [PRIMARY]

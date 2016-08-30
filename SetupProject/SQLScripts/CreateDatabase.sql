CREATE DATABASE "OpsLogix.MgmtSvc.TenantAutomation"
GO

USE [OpsLogix.MgmtSvc.TenantAutomation]
GO

/****** Object:  Table [dbo].[Runbooks]    Script Date: 10-2-2016 13:20:01 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Runbooks](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RunbookId] [nvarchar](50) NOT NULL,
	[RunbookName] [nvarchar](50) NOT NULL,
	[RunbookTag] [nvarchar](50) NULL,
	[PlanId] [nvarchar](50) NOT NULL,
	[PlanName] [nvarchar](50) NOT NULL,
	[ParamStringLabel] [nvarchar](100) NOT NULL CONSTRAINT [DF_Runbooks_ParamStringLabel]  DEFAULT (N'String Label'),
	[ParamString] [nvarchar](2) NOT NULL CONSTRAINT [DF_Runbooks_ParamString]  DEFAULT ((0)),
	[ParamIntLabel] [nvarchar](100) NOT NULL CONSTRAINT [DF_Runbooks_ParamIntLabel]  DEFAULT (N'Int Label'),
	[ParamInt] [nvarchar](2) NOT NULL CONSTRAINT [DF_Runbooks_ParamInt]  DEFAULT ((0)),
	[ParamStringArrayLabel] [nvarchar](100) NOT NULL CONSTRAINT [DF_Runbooks_ParamStringArrayLabel]  DEFAULT (N'String Array Label'),
	[ParamStringArray] [nvarchar](2) NOT NULL CONSTRAINT [DF_Runbooks_ParamStringArray]  DEFAULT ((0)),
	[ParamDateLabel] [nvarchar](100) NOT NULL CONSTRAINT [DF_Runbooks_ParamDateLabel]  DEFAULT (N'Date Label'),
	[ParamDate] [nvarchar](2) NOT NULL CONSTRAINT [DF_Runbooks_ParamDate]  DEFAULT ((0)),
	[ParamBoolLabel] [nvarchar](100) NOT NULL CONSTRAINT [DF_Runbooks_ParamBoolLabel]  DEFAULT (N'Bool Label'),
	[ParamBool] [nvarchar](2) NOT NULL CONSTRAINT [DF_Runbooks_ParamBool]  DEFAULT ((0)),
	[ParamVMDropdownLabel] [nvarchar](100) NOT NULL CONSTRAINT [DF_Runbooks_ParamVMDropdownLabel]  DEFAULT (N'Virtual Machines'),
	[ParamVMDropdown] [nvarchar](2) NOT NULL CONSTRAINT [DF_Runbooks_ParamVMDropdown]  DEFAULT ((0)),
 CONSTRAINT [PK_Runbook] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
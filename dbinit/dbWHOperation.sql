USE [dbWHOperation]
GO
/****** Object:  Table [dbo].[sysMaster]    Script Date: 01/26/2015 08:09:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sysMaster](
	[SystemID] [nvarchar](10) NOT NULL,
	[CompanyName] [nvarchar](100) NULL,
	[verNo] [nvarchar](3) NULL,
	[NewVerNo] [nvarchar](5) NULL,
 CONSTRAINT [PK_sysMaster] PRIMARY KEY CLUSTERED 
(
	[SystemID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PIMSMRBException]    Script Date: 01/26/2015 08:09:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PIMSMRBException](
	[TransID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[DNNo] [nvarchar](20) NOT NULL,
	[DNDate] [nvarchar](50) NOT NULL,
	[SupplierID] [nvarchar](50) NULL,
	[MfgrID] [nvarchar](50) NULL,
	[MG] [nvarchar](50) NULL,
	[PIMS] [nvarchar](30) NULL,
	[PartNumber] [nvarchar](50) NULL,
	[ReqMfgrPart] [nvarchar](100) NULL,
	[RecMfgrPart] [nvarchar](100) NULL,
	[CustPart] [nvarchar](50) NULL,
	[RecQty] [numeric](18, 5) NULL,
	[RIRNo] [nvarchar](20) NULL,
	[UpdatedDate] [smalldatetime] NULL,
 CONSTRAINT [PK_PIMSMRBException] PRIMARY KEY CLUSTERED 
(
	[TransID] ASC,
	[DNNo] DESC,
	[DNDate] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PIMLVendorTemplateX]    Script Date: 01/26/2015 08:09:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PIMLVendorTemplateX](
	[VendorID] [nvarchar](20) NOT NULL,
	[TemplateID] [nvarchar](3) NOT NULL,
	[xmlVendorData] [xml] NULL,
	[isDefault] [char](10) NULL,
	[templateImage] [varbinary](max) NULL,
	[TemplateIDN] [nvarchar](5) NULL,
 CONSTRAINT [PK_PIMLVendorTemplate] PRIMARY KEY CLUSTERED 
(
	[VendorID] ASC,
	[TemplateID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PIMLVendorTemplate]    Script Date: 01/26/2015 08:09:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PIMLVendorTemplate](
	[VendorID] [nvarchar](20) NOT NULL,
	[TemplateID] [nvarchar](15) NOT NULL,
	[xmlVendorData] [xml] NULL,
	[isDefault] [char](10) NULL,
	[templateImage] [varbinary](max) NULL,
 CONSTRAINT [PK_PIMLVendorTemplateNew] PRIMARY KEY CLUSTERED 
(
	[VendorID] ASC,
	[TemplateID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PIMLDetail]    Script Date: 01/26/2015 08:09:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PIMLDetail](
	[TransID] [nvarchar](8) NULL,
	[TransLine] [nvarchar](3) NULL,
	[DNNo] [nvarchar](20) NULL,
	[DNDate] [nvarchar](10) NULL,
	[VendorID] [nvarchar](20) NULL,
	[PONo] [nvarchar](10) NULL,
	[POLine] [nvarchar](3) NULL,
	[PartNumber] [nvarchar](50) NULL,
	[LotNo] [nvarchar](100) NULL,
	[RIRNo] [nvarchar](50) NULL,
	[MFGPartNumber] [nvarchar](50) NULL,
	[DeliveryNoteNo] [nvarchar](50) NULL,
	[DateCode] [nvarchar](10) NULL,
	[ExpDate] [nvarchar](10) NULL,
	[t_urg] [nvarchar](20) NULL,
	[t_loc] [nvarchar](20) NULL,
	[t_msd] [nvarchar](20) NULL,
	[t_cust_part] [nvarchar](50) NULL,
	[t_shelf_life] [nvarchar](20) NULL,
	[t_wt] [nvarchar](18) NULL,
	[t_wt_ind] [nvarchar](18) NULL,
	[t_conn] [nvarchar](20) NULL,
	[t_site] [nvarchar](10) NULL,
	[SystemID] [nvarchar](10) NULL,
	[MFGDate] [nvarchar](10) NULL,
	[DNQty] [numeric](18, 0) NULL,
	[LineQty] [numeric](18, 0) NULL,
	[updatedDate] [smalldatetime] NULL,
	[NoOfLabels] [numeric](4, 0) NULL,
	[PIMSNumber] [nvarchar](max) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PI_Print]    Script Date: 01/26/2015 08:09:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PI_Print](
	[PI_Line] [bigint] NOT NULL,
	[PI_NO] [nvarchar](12) NOT NULL,
	[PI_PART] [nvarchar](18) NOT NULL,
	[PI_mpq] [decimal](18, 0) NOT NULL,
	[pi_mfgr_part] [nvarchar](50) NOT NULL,
	[PI_LOT] [nvarchar](12) NOT NULL,
	[PI_PO] [nvarchar](50) NOT NULL,
	[pi_mfgr] [nvarchar](50) NOT NULL,
	[PI_QTY] [decimal](18, 0) NOT NULL,
	[PI_Print_QTY] [decimal](18, 0) NOT NULL,
	[PI_SITE] [nvarchar](6) NULL,
	[PI_PO_price] [decimal](18, 0) NULL,
	[pi_char1] [nvarchar](50) NULL,
	[pi_char2] [nvarchar](50) NULL,
	[pi_char3] [nvarchar](50) NULL,
	[pi_num1] [decimal](18, 7) NULL,
	[pi_num2] [decimal](18, 7) NULL,
	[pi_int1] [int] NULL,
	[pi_int2] [int] NULL,
	[pi_cre_date] [datetime] NULL,
	[pi_cre_userid] [nvarchar](50) NULL,
	[pi_update_date] [datetime] NULL,
	[pi_edituser_id] [nvarchar](50) NULL,
	[pi_user_ip] [nvarchar](50) NULL,
	[pi_remark] [nvarchar](256) NULL,
 CONSTRAINT [PK_PI_Print_1] PRIMARY KEY CLUSTERED 
(
	[PI_Line] DESC,
	[PI_NO] ASC,
	[PI_PART] ASC,
	[pi_mfgr_part] ASC,
	[PI_LOT] ASC,
	[PI_PO] ASC,
	[pi_mfgr] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[vpi_sumPrintQty]    Script Date: 01/26/2015 08:09:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vpi_sumPrintQty]
AS
SELECT     PI_NO, PI_PART, pi_mfgr_part, PI_LOT, PI_PO, pi_mfgr, SUM(PI_Print_QTY) AS sumPrintQty
FROM         dbo.PI_Print
GROUP BY PI_NO, PI_PART, pi_mfgr_part, PI_LOT, PI_PO, pi_mfgr
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "PI_Print"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 125
               Right = 219
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 12
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vpi_sumPrintQty'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vpi_sumPrintQty'
GO
/****** Object:  Default [DF_PI_Print_PI_Print_QTY]    Script Date: 01/26/2015 08:09:24 ******/
ALTER TABLE [dbo].[PI_Print] ADD  CONSTRAINT [DF_PI_Print_PI_Print_QTY]  DEFAULT ((0)) FOR [PI_Print_QTY]
GO
/****** Object:  Default [DF_PIMLDetail_updatedDate]    Script Date: 01/26/2015 08:09:24 ******/
ALTER TABLE [dbo].[PIMLDetail] ADD  CONSTRAINT [DF_PIMLDetail_updatedDate]  DEFAULT (getdate()) FOR [updatedDate]
GO
/****** Object:  Default [DF_PIMSMRBException_UpdatedDate]    Script Date: 01/26/2015 08:09:24 ******/
ALTER TABLE [dbo].[PIMSMRBException] ADD  CONSTRAINT [DF_PIMSMRBException_UpdatedDate]  DEFAULT (getdate()) FOR [UpdatedDate]
GO

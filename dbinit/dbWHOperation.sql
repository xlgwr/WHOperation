USE [dbWHOperation]
GO
/****** Object:  Table [dbo].[sysMaster]    Script Date: 02/05/2015 12:52:49 ******/
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
/****** Object:  Table [dbo].[PIMSMRBException]    Script Date: 02/05/2015 12:52:49 ******/
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
	[PartNumberRec] [nvarchar](50) NULL,
	[ReqMfgrPart] [nvarchar](100) NULL,
	[RecMfgrPart] [nvarchar](100) NULL,
	[CustPart] [nvarchar](50) NULL,
	[RecQty] [numeric](18, 5) NULL,
	[RIRNo] [nvarchar](20) NULL,
	[UpdatedDate] [smalldatetime] NULL,
 CONSTRAINT [PK_PIMSMRBException] PRIMARY KEY CLUSTERED 
(
	[TransID] DESC,
	[DNNo] DESC,
	[DNDate] DESC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PIMLVendorTemplateX]    Script Date: 02/05/2015 12:52:49 ******/
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
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PIMLVendorTemplate]    Script Date: 02/05/2015 12:52:49 ******/
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
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PIMLDetail]    Script Date: 02/05/2015 12:52:49 ******/
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
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PI_Print]    Script Date: 02/05/2015 12:52:49 ******/
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
	[pi_DateCode] [text] NULL,
	[pi_lotNumber] [text] NULL,
	[pi_char1] [nvarchar](50) NULL,
	[pi_char2] [nvarchar](50) NULL,
	[pi_char3] [nvarchar](50) NULL,
	[pi_num1] [decimal](18, 0) NULL,
	[pi_num2] [decimal](18, 0) NULL,
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
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[pi_Det_Remote]    Script Date: 02/05/2015 12:52:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[pi_Det_Remote](
	[pi_Line] [bigint] NOT NULL,
	[pi_NO] [nvarchar](12) NOT NULL,
	[pi_PART] [nvarchar](18) NOT NULL,
	[pi_mfgr_part] [nvarchar](128) NOT NULL,
	[pi_LOT] [nvarchar](12) NOT NULL,
	[pi_WecNumber] [nvarchar](12) NOT NULL,
	[pi_PO] [nvarchar](128) NOT NULL,
	[pi_mfgr] [nvarchar](128) NOT NULL,
	[pi_QTY] [decimal](18, 0) NOT NULL,
	[pi_Print_QTY] [decimal](18, 0) NOT NULL,
	[pi_ttlQTY] [decimal](18, 0) NOT NULL,
	[pi_mpq] [decimal](18, 0) NOT NULL,
	[pi_SITE] [nvarchar](6) NULL,
	[pi_PALLET] [nvarchar](128) NULL,
	[pi_CARTON_NO] [nvarchar](128) NULL,
	[pi_carton_prefix] [nvarchar](50) NULL,
	[pi_carton_from] [int] NULL,
	[pi_carton_to] [int] NULL,
	[pi_DateCode] [nvarchar](128) NULL,
	[pi_LotNumber] [nvarchar](128) NULL,
	[pi_char1] [nvarchar](128) NULL,
	[pi_char2] [nvarchar](128) NULL,
	[pi_char3] [nvarchar](128) NULL,
	[pi_num1] [decimal](18, 7) NULL,
	[pi_num2] [decimal](18, 7) NULL,
	[pi_int1] [int] NULL,
	[pi_int2] [int] NULL,
	[pi_cre_date] [datetime] NULL,
	[pi_cre_userid] [nvarchar](128) NULL,
	[pi_update_date] [datetime] NULL,
	[pi_edituser_id] [nvarchar](128) NULL,
	[pi_user_ip] [nvarchar](128) NULL,
	[pi_remark] [nvarchar](256) NULL,
 CONSTRAINT [PK_pi_det_remote] PRIMARY KEY CLUSTERED 
(
	[pi_Line] DESC,
	[pi_NO] ASC,
	[pi_PART] ASC,
	[pi_mfgr_part] ASC,
	[pi_LOT] ASC,
	[pi_PO] ASC,
	[pi_mfgr] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[vpi_sumPrintQty]    Script Date: 02/05/2015 12:52:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vpi_sumPrintQty]
AS
SELECT     PI_NO, PI_PART, pi_mfgr_part, PI_LOT, PI_PO, pi_mfgr, PI_QTY, SUM(PI_Print_QTY) AS sumPrintQty
FROM         dbo.PI_Print
GROUP BY PI_NO, PI_PART, pi_mfgr_part, PI_LOT, PI_PO, pi_mfgr, PI_QTY
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[25] 4[36] 2[20] 3) )"
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
/****** Object:  StoredProcedure [dbo].[getMRBToEmail]    Script Date: 02/05/2015 12:53:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[getMRBToEmail] 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	declare @site nvarchar(50)
	declare @partNumber nvarchar(50)
	declare @partNumberScan nvarchar(50)
	declare @mfgPart nvarchar(100)
	declare @mfgPartScan nvarchar(100)
	declare @mfgrID nvarchar(50)
	declare @suppliterID nvarchar(50)
	declare @updateDate smalldatetime
	declare @updateDatetmp nvarchar(50)
	
	declare @cMSG varchar(MAX)
	declare @cRecepients char(100)
	declare @cDocFileName char(100)
	declare @cDocFileExt char(4)
	declare @cHasData char(1)
	
    -- Insert statements for procedure here
	DECLARE tmp1 CURSOR FOR select isnull(MG,''),isnull(PartNumber,''),isnull(PartNumberRec,''),isnull(ReqMfgrPart,''),isnull(RecMfgrPart,''),isnull(MfgrID,''), isnull(SupplierID,0) ,isnull(UpdatedDate,0) from dbo.PIMSMRBException where UpdatedDate>GETDATE()-5
	
	OPEN tmp1
	
	set @cMSG='<html><head><meta charset="utf-8" /> <style>table {width: 100%;margin-bottom: 1.6rem;border-spacing: 0px;border-collapse: separate;max-width: 100%;background-color: transparent;empty-cells: show;}td, th {padding: 8px;line-height: 1.42857;vertical-align: top;border-top: 1px solid #DDD;}</style></head><body><table><tr bgcolor=#FFFFFF><td colspan=8 align=center><h1>MRB Test Report - '+convert(varchar,GETDATE()-5,111)+'-'+convert(varchar,GETDATE(),111)+'</h1></td></tr>'
	set @cMSG = ltrim(rtrim(@cMSG))+ '<tr>'+
					'<td>Site</td>'+
					'<td>PartNumber</td>'+
					'<td>PartNumber Scan</td>'+					
					'<td>MfgrPart</td>'+
					'<td>MfgrPart Scan</td>'+
					'<td>MfgrID</td>'+
					'<td>Supplier</td>'+
					'<td>UpdatedDate</td>'+
					'</tr>'
	set @cHasData = 'N'
	FETCH NEXT FROM tmp1 into @site,@partNumber,@partNumberScan,@mfgPart,@mfgPartScan,@mfgrID,@suppliterID,@updateDate
	WHILE @@FETCH_STATUS = 0
	BEGIN
		set @updateDatetmp=CONVERT(nvarchar(50),@updateDate)
		--select @cMSG,@site,@partNumber,@partNumberScan,@mfgPart,@mfgPartScan,@mfgrID,@suppliterID,@updateDate
			set @cMSG = LTRIM(rtrim(@cMSG))+ '<tr><td>'+
						ltrim(rtrim(@site))+'</td><td>'+
						ltrim(rtrim(@partNumber))+'</td><td>'+
						ltrim(rtrim(@partNumberScan))+'</td><td>'+
						ltrim(rtrim(@mfgPart))+'</td><td>'+
						ltrim(rtrim(@mfgPartScan))+'</td><td>'+
						ltrim(rtrim(@mfgrID))+'</td><td>'+
						ltrim(rtrim(@suppliterID))+'</td><td>'+
						ltrim(rtrim(@updateDatetmp))+'</td>'+
						'</tr>'													
		set @cHasData = 'Y'
		FETCH next FROM tmp1 into @site,@partNumber,@partNumberScan,@mfgPart,@mfgPartScan,@mfgrID,@suppliterID,@updateDate
	end
	
	declare @cSubj char(100)
	set @cSubj = 'MRP Test Report '+convert(varchar,GETDATE()-5,111)+'-'+convert(varchar,GETDATE(),111)
	set @cMSG = ltrim(rtrim(@cMSG)) + '</table></body></html>'
		
	if @cHasData='Y'
	Begin
			  EXEC msdb.dbo.sp_send_dbmail 
                    @profile_name = 'DMS', 
                    @body_format = 'HTML', 
                    --@recipients = 'Ling.Xie@wehc.com.cn;',
                    @recipients = 'Michael.Tai@wehc.com.cn;Ling.Xie@wehc.com.cn;Eric.Ting@wehc.com.cn;Paul.Kwan@wehc.com.cn;KL.Siu@wehc.com.cn',
                    @subject = @cSubj, 
                    @body = @cMSG    
    end  
	CLOSE tmp1
	DEALLOCATE tmp1
END
GO
/****** Object:  Default [DF_pi_Det_Remote_pi_Print_QTY]    Script Date: 02/05/2015 12:52:49 ******/
ALTER TABLE [dbo].[pi_Det_Remote] ADD  CONSTRAINT [DF_pi_Det_Remote_pi_Print_QTY]  DEFAULT ((0)) FOR [pi_Print_QTY]
GO
/****** Object:  Default [DF_PI_Print_PI_Print_QTY]    Script Date: 02/05/2015 12:52:49 ******/
ALTER TABLE [dbo].[PI_Print] ADD  CONSTRAINT [DF_PI_Print_PI_Print_QTY]  DEFAULT ((0)) FOR [PI_Print_QTY]
GO
/****** Object:  Default [DF_PIMLDetail_updatedDate]    Script Date: 02/05/2015 12:52:49 ******/
ALTER TABLE [dbo].[PIMLDetail] ADD  CONSTRAINT [DF_PIMLDetail_updatedDate]  DEFAULT (getdate()) FOR [updatedDate]
GO
/****** Object:  Default [DF_PIMSMRBException_UpdatedDate]    Script Date: 02/05/2015 12:52:49 ******/
ALTER TABLE [dbo].[PIMSMRBException] ADD  CONSTRAINT [DF_PIMSMRBException_UpdatedDate]  DEFAULT (getdate()) FOR [UpdatedDate]
GO

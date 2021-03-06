USE [pi_hk]
GO
/****** Object:  StoredProcedure [pi].[daliyGetOneDayPI]    Script Date: 03/12/2015 09:48:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [pi].[daliyGetOneDayPI] 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.	
	SET NOCOUNT ON;
	
	declare @pino nvarchar(50)
    -- Insert statements for procedure here
	declare tmpcur cursor for select PI_NO from piRemote7.pi.dbo.pi_mstr where pi_date < getdate()-2 and pi_date > getdate()-4
	
	open tmpcur
	
	fetch next from tmpcur into @pino
	
	WHILE @@FETCH_STATUS = 0
	begin
		if not exists(select top 1 * from dbo.PI_DET where PI_NO = @pino)
			exec insertPiDetFromHK @pino
		
		fetch next from tmpcur into @pino
	end
	
	close tmpcur
	deallocate tmpcur
END
GO
/****** Object:  Table [dbo].[PI_DET]    Script Date: 03/12/2015 09:48:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PI_DET](
	[PI_NO] [nvarchar](12) NOT NULL,
	[PI_LINE] [bigint] NOT NULL,
	[PI_CARTON_NO] [nvarchar](12) NULL,
	[PI_SITE] [nvarchar](6) NULL,
	[PI_PART] [nvarchar](18) NULL,
	[PI_CO] [nvarchar](10) NULL,
	[PI_DESC] [nvarchar](50) NULL,
	[PI_QTY] [numeric](18, 0) NULL,
	[PI_LOT] [nvarchar](12) NOT NULL,
	[PI_NW] [numeric](18, 4) NULL,
	[PI_K200_NW] [numeric](18, 7) NULL,
	[PI_GW] [numeric](18, 4) NULL,
	[PI_SBU] [nvarchar](4) NULL,
	[PI_REC_NO] [nvarchar](15) NULL,
	[PI_PRICE] [numeric](18, 4) NULL,
	[PI_PALLET] [nvarchar](12) NULL,
	[PI_PO_price] [numeric](18, 0) NULL,
	[PI_CONTRACT] [nvarchar](18) NULL,
	[PI_SEQ] [int] NULL,
	[PI_SEQ_CL] [int] NULL,
	[PI_IQC] [nvarchar](4) NULL,
	[PI_PO] [nvarchar](8) NULL,
	[PI_Taxcode] [nvarchar](16) NULL,
	[PI_ConnCode] [nvarchar](8) NULL,
	[pi_user] [nvarchar](8) NULL,
	[pi_cre_time] [datetime] NULL,
	[pi_ver] [int] NULL,
	[pi_mfgr] [nvarchar](8) NULL,
	[pi_mfgr_part] [nvarchar](100) NULL,
	[pi_mfgr_name] [nvarchar](100) NULL,
	[pi_Lic_req] [nvarchar](3) NULL,
	[pi_ori_PO_price] [numeric](18, 4) NULL,
	[pi_PO_curr] [nvarchar](8) NULL,
	[pi_curr_rate] [numeric](18, 4) NULL,
	[pi_us_rate] [numeric](18, 4) NULL,
	[pi_vend] [nvarchar](50) NULL,
	[PI_Print_QTY] [decimal](18, 0) NULL,
	[pi_dateCode] [text] NULL,
	[pi_lotNumber] [text] NULL,
	[NumOfCarton] [decimal](18, 0) NULL,
 CONSTRAINT [PK_PI_DET] PRIMARY KEY NONCLUSTERED 
(
	[PI_LINE] ASC,
	[PI_NO] ASC,
	[PI_LOT] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  StoredProcedure [pi].[insertPiDetFromHK]    Script Date: 03/12/2015 09:48:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [pi].[insertPiDetFromHK]
	-- Add the parameters for the stored procedure here
	@pi_no as nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
    -- Insert statements for procedure here
	if exists(select top 1 * from dbo.PI_DET where pi_no=@pi_no)
		delete from dbo.PI_DET where pi_no=@pi_no
    
	insert into dbo.PI_DET
	select *,0,'','',1 from piRemote7.pi.dbo.pi_det a where a.pi_no=@pi_no and (a.pi_lot<> NUll or a.pi_lot <>'')
END
GO
/****** Object:  View [pi].[vpi_detWHO]    Script Date: 03/12/2015 09:48:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [pi].[vpi_detWHO]
AS
SELECT     TOP 100 PERCENT RTRIM(a.PI_PART) AS PI_PART, RTRIM(a.pi_mfgr_part) AS pi_mfgr_part, RTRIM(a.PI_LOT) AS PI_LOT, RTRIM(a.PI_PO) AS PI_PO, RTRIM(a.pi_mfgr) AS pi_mfgr, a.PI_QTY, 
                      ISNULL(a.PI_Print_QTY, 0) AS PI_Print_QTY, ISNULL(a.PI_PO_price, 0) AS PI_PO_price, RTRIM(LTRIM(a.PI_PALLET)) AS PI_PALLET, LTRIM(a.PI_CARTON_NO) AS PI_CARTON_NO, a.PI_SITE, 
                      a.pi_cre_time, ISNULL(b.ttlQTY, 0) AS ttlQTY, a.PI_NO, a.PI_LINE, a.pi_dateCode, a.pi_lotNumber, ISNULL(a.NumOfCarton, 1) AS NumOfCarton
FROM         dbo.PI_DET AS a LEFT OUTER JOIN
                          (SELECT     PI_NO AS bPI_NO, PI_LOT AS bPI_LOT, SUM(PI_QTY) AS ttlQTY
                            FROM          dbo.PI_DET AS pi_det_1
                            WHERE      (PI_LOT <> NULL) OR
                                                   (PI_LOT <> '')
                            GROUP BY PI_NO, PI_LOT, PI_PART, pi_mfgr_part) AS b ON a.PI_NO = b.bPI_NO AND a.PI_LOT = b.bPI_LOT
WHERE     (a.PI_LOT <> NULL) OR
                      (a.PI_LOT <> '')
ORDER BY a.PI_NO DESC, a.PI_LINE
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[25] 4[36] 2[21] 3) )"
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
         Begin Table = "a"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 149
               Right = 206
            End
            DisplayFlags = 280
            TopColumn = 35
         End
         Begin Table = "b"
            Begin Extent = 
               Top = 6
               Left = 244
               Bottom = 110
               Right = 386
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
      Begin ColumnWidths = 11
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
' , @level0type=N'SCHEMA',@level0name=N'pi', @level1type=N'VIEW',@level1name=N'vpi_detWHO'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'pi', @level1type=N'VIEW',@level1name=N'vpi_detWHO'
GO
/****** Object:  StoredProcedure [pi].[get_vPiDet]    Script Date: 03/12/2015 09:48:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [pi].[get_vPiDet] 
	-- Add the parameters for the stored procedure here
	@pi_no as nvarchar(50), 
	@pallet as nvarchar(50),	
	@ctn_prefix as nvarchar(50),
	@ctn_no as decimal,
	@allPrint as decimal
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	if not exists(select top 1 * from dbo.PI_DET where PI_NO = @pi_no or PI_LOT = @pi_no)
		begin
			exec insertPiDetFromHK @pi_no
			exec daliyGetOneDayPI
		end
	--if (@allPrint=0)
	--begin
	--   select * from vpi_detWHO where PI_NO = @pi_no order by PI_LINE
	--   --print 'pino:'+@pi_no+','+cast(@pallet as nvarchar(50))+','+@ctn_prefix+','+cast(@ctn_no as nvarchar(50))
	--   return
	--end
	if (@pallet ='' and @ctn_prefix='' and @ctn_no<=0)
	 begin
	   select * from vpi_detWHO where (PI_NO = @pi_no or PI_LOT = @pi_no ) order by pi_mfgr_part,PI_LOT,PI_PART,PI_LINE
	   --print 'pino:'+@pi_no+','+cast(@pallet as nvarchar(50))+','+@ctn_prefix+','+cast(@ctn_no as nvarchar(50))
	   return
	 end
	---Pallet No
	if @pallet <> ''
	 begin
		select * from vpi_detWHO where PI_NO = @pi_no and PI_PALLET=@pallet and PI_LOT<> '' order by pi_mfgr_part,PI_LOT,PI_PART,PI_LINE
		--print 'pallet:'+@pi_no+','+cast(@pallet as nvarchar(50))+','+@ctn_prefix+','+cast(@ctn_no as nvarchar(50))
		return
	 end
		
	----Carton no
	if @ctn_prefix <> ''
	begin	    
		if exists(select top 1 * from vpi_detWHO where PI_NO = @pi_no and rtrim(ltrim(pi_carton_no)) like @ctn_prefix+'%')
		select * from vpi_detWHO where PI_NO = @pi_no 
			and rtrim(ltrim(pi_carton_no)) like @ctn_prefix+'%'  
			and cast((case CHARINDEX('-',PI_CARTON_NO,0) 
		when 0 then rtrim(ltrim(REPLACE(PI_CARTON_NO,@ctn_prefix,''))) 
			else rtrim(ltrim(left(REPLACE(PI_CARTON_NO,@ctn_prefix,''), CHARINDEX('-',REPLACE(PI_CARTON_NO,@ctn_prefix,''),0)-1)))  end) as decimal) <= @ctn_no 
			and  cast((case CHARINDEX('-',REPLACE(PI_CARTON_NO,@ctn_prefix,''),0) 
	    when 0 then rtrim(ltrim(REPLACE(PI_CARTON_NO,@ctn_prefix,''))) else right(REPLACE(PI_CARTON_NO,@ctn_prefix,''), len(REPLACE(PI_CARTON_NO,@ctn_prefix,''))-CHARINDEX('-',REPLACE(PI_CARTON_NO,@ctn_prefix,''),0))  end) as decimal) >= @ctn_no
		order by pi_mfgr_part,PI_LOT,PI_PART,PI_LINE
		else
		select top 1 * from vpi_detWHO where pi_no='zzzzzzzzzz'
		--print 'carton:'+@pi_no+','+cast(@pallet as nvarchar(50))+','+@ctn_prefix+','+cast(@ctn_no as nvarchar(50))
	end
	else
	begin
		select * from vpi_detWHO where PI_NO = @pi_no 
			and pi_carton_no like '[0-9]%'
			and cast((case CHARINDEX('-',PI_CARTON_NO,0)
		when 0 then PI_CARTON_NO
			else left(PI_CARTON_NO,CHARINDEX('-',PI_CARTON_NO,0)-1)  end) as decimal) <= @ctn_no 
			and cast((case CHARINDEX('-',PI_CARTON_NO,0)
		when 0 then PI_CARTON_NO
			else right(PI_CARTON_NO,len(PI_CARTON_NO)-CHARINDEX('-',PI_CARTON_NO,0))  end) as decimal) >= @ctn_no
		order by pi_mfgr_part,PI_LOT,PI_PART,PI_LINE
		--print 'carton num:'+@pi_no+','+cast(@pallet as nvarchar(50))+','+@ctn_prefix+','+cast(@ctn_no as nvarchar(50))
	end
end
GO
/****** Object:  Default [DF_PI_DET_pi_cre_time]    Script Date: 03/12/2015 09:48:08 ******/
ALTER TABLE [dbo].[PI_DET] ADD  CONSTRAINT [DF_PI_DET_pi_cre_time]  DEFAULT (getdate()) FOR [pi_cre_time]
GO
/****** Object:  Default [DF_PI_DET_PI_Print_QTY]    Script Date: 03/12/2015 09:48:08 ******/
ALTER TABLE [dbo].[PI_DET] ADD  CONSTRAINT [DF_PI_DET_PI_Print_QTY]  DEFAULT (0) FOR [PI_Print_QTY]
GO
/****** Object:  Default [DF_PI_DET_NumOfLabel]    Script Date: 03/12/2015 09:48:08 ******/
ALTER TABLE [dbo].[PI_DET] ADD  CONSTRAINT [DF_PI_DET_NumOfLabel]  DEFAULT (1) FOR [NumOfCarton]
GO

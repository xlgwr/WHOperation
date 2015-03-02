def temp-table wsas002
    field wsas002_PIMS      as char format 'x(18)'
    field wsas002_rir       as char format 'x(13)'
    field wsas002_part      as char format 'x(18)'
    field wsas002_site      as char
    field wsas002_loc       as char
    field wsas002_type      as char format 'x(10)'
    field wsas002_ref       as char
    field wsas002_qty_per   like ld_qty_oh
    field wsas002_qty_tot   like ld_qty_oh
    field wsas002_expi_date as date
    field wsas002_expi_type as char format 'x(1)'
    field wsas002_mfgr_part like qpl_mfgr_part
    field wsas002_cust_part like qpl_cust_part
    field wsas002_date_code as char format 'x(20)'
    field wsas002_by        as char format 'x(30)'
    field wsas002_wt        like pt_net_wt
    field wsas002_msd       as char
    field wsas002_nw_po     as char.
  

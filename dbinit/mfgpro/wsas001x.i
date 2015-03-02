def temp-table wsas001x
    field t_dn        like ftn_nbr
    field t_deli_date as date
    field t_rir       as char format 'x(18)'
    field t_part      as char format 'x(18)'
    field t_po        as char 
    field t_site      as char 
    field t_qty       as decimal format '->>>,>>>,>>9.99'
    field t_supp      as char
    field t_mfgr      as char
    field t_mfgr_part as char format 'x(40)'
    field t_id        as integer
    field t_urg       as char format 'x(3)'
    field t_loc       as char format 'x(4)'
    field t_msd       as char format 'x(2)'
    field t_cust_part as char format 'x(30)'
    field t_shelf_life as decimal format '9.99'
    field t_wt        as decimal format '>>>,>>9.9999'
    field t_wt_ind    as char
    field t_conn      as char
    field t_mpq       like ptp_ord_mult
    index v1 t_dn t_part t_mfgr_part t_rir.
 

/*
def var w_para as char format 'x(100)' no-undo.
def var opcount as integer no-undo.
w_para = 'TEST25000001,B1306102798,1323,05/12/12,08/29/12,1,WZ_WU,lot4,706787'.
*/
{d:\mfgpro\wsappsrc\wsas002.i}    

def input parameter w_para  as char format 'x(200)' no-undo.
def output parameter table for wsas002.
def output parameter opcount as int no-undo.

def var t1_today      as char.
def var t1_pims       as char.
def var t1_site       as char.
def var t1_part       as char format 'x(18)'.
def var t1_rir        as char format 'x(18)'.
def var t1_date_code  as char.
def var t1_ref_pims   as char.
def var t1_qty        as decimal format '->>>>>>>>9.99'.
def var t1_qty_per    as decimal format '->>>>>>>>9.99'.
def var t1_qty_tot    as decimal format '->>>>>>>>9.99'.
def var t1_ref        as char.
def var t1_userid     as char.
def var t1_loc        as char.
def var t1_mfgr_lot   as char.
def var t1_lot        as char.
def var t1_mfgr_date  as date.
def var t1_expi_date  as date.
def var t1_mfgr_date1 as date.
def var t1_vend       as char.
def var t1_calc_mfgr  as date.
def var t1_shelf_life as char.
def var t1_ind        as char.
def var t1_shelf      as logical.
def var t1_expi_type  as char format 'x(1)'.
def var t1_u_part     as char format 'x(2)'.
def var t1_type       as char. 
def var t1_by         as char format 'x(30)'.
def var t1_wt_ind     as char format 'x(4)'.
def var t1_msd        as char.
def var t1_wt         like pt_net_wt.
def var t1_yy         as decimal.
def var t1_site_sbu   as char.
def var t1_by_site    as logical.
def var t1_mfgr_part  like qpl_mfgr_part.
def var t2_mfgr_part  like qpl_mfgr_part.
def var t1_in_mfgr_part like qpl_mfgr_part.
def var t1_cust_part  like qpl_cust_part.
def var t1_dc_control as logical.
def var t1_life_time  as integer.
def var t1_nw_po      as char format 'x(1)' init ''.
def var i             as integer.
def var t_mrb         as logical init no.

def temp-table t_prefix
    field  tp_entity as char
    field  tp_prefix as char
    field  tp_year   as decimal
    index tp_key1
          tp_entity
          tp_prefix.

for each t_prefix: delete t_prefix. end.
for each wsas002: delete wsas002. end.
do transaction:
   for each dtab where dtab_x14 begins 'sl' no-lock:
       create t_prefix.
       tp_entity = substring(dtab_x14,3,4).
       tp_prefix = substring(dtab_x14,7,length(dtab_x14) - 6).
       tp_year   = decimal(substring(dtab_x40,1,length(dtab_x40))).
   end.
end.

opcount = 0.
if num-entries(w_para) <> 9 then do:
   opcount = -1.
   leave.
end.

t1_pims       = entry(1,w_para).
t1_rir        = entry(2,w_para).
t1_date_code  = entry(3,w_para).
if entry(4,w_para) = '' then
    t1_mfgr_date = date(entry(4,w_para)).
else
    t1_mfgr_date = ?.
if entry(5,w_para) <> '' then
    t1_expi_date = date(entry(5,w_para)).
else
    t1_expi_date = ?.
t1_qty        = decimal(entry(6,w_para)).
t1_userid     = entry(7,w_para).
t1_lot        = entry(8,w_para).
t1_in_mfgr_part = entry(9,w_para).

/****
t1_vend       = entry(9,w_para).
t1_userid     = entry(10,w_para).
t1_mfgr_lot   = entry(11,w_para).
t1_mfgr_date  = entry(12,w_para).
t1_exp_date   = entry(13,w_para).
t1_recv_date  = entry(14,w_para).
t1_shelf_life = entry(15,w_para).
t1_ind        = entry(16,w_para).
t1_1_3        = entry(17,w_para).
****/

/* get part ,site ,ref ,vend */
find first prh_hist where prh__chr05 = t1_rir no-lock no-error.
if avail prh_hist then do:    
    t1_qty_tot = prh_rcvd.
    t1_part    = prh_part.
    t1_site    = prh_site.
    t1_ref     = prh__chr04.
    t1_vend    = prh_vend.
    if prh_nbr begins '5' or prh_nbr begins 'V5' or prh_nbr begins 'VT5' then t1_nw_po = 'D'.
end.    

/* check tmp_tab cancel by wwz 05/24/2013 
find first tmp_tab where tmp_system = 'wse869a4' and tmp_site = t1_site and tmp_part = t1_part no-lock no-error.
if avail tmp_tab and t1_date_code = '' and t1_lot = '' then do:
    create wsas002.
    opcount = -2.
    wsas002_pims = string(opcount).
    wsas002_site      = t1_site.
    wsas002_part      = t1_part.
    wsas002_rir       = t1_rir.
    wsas002_loc       = ''.
    wsas002_type      = ''.
    wsas002_ref       = ''.
    wsas002_qty_per   = 0.
    wsas002_expi_date = ?.
    wsas002_expi_type = ''.
    wsas002_mfgr_part = ''.
    wsas002_cust_part = ''.
    wsas002_date_code = ''.
    wsas002_by        = ''.
    wsas002_wt        = 0.
    wsas002_msd       = ''.
    opcount = 1.
    leave.
end. 
***/

/* get type ,mfgr_part */
t2_mfgr_part = ''.
t1_mfgr_part = ''.
find first pod_det where pod_nbr = prh_nbr and pod_line = prh_line
and pod_part = prh_part no-lock no-error.
if avail pod_det then do:
    t1_type = pod__chr01.
    do i = 1 to length(pod_vpart):
        if substring(pod_vpart,i,1) = '' then next.
        else do:
            t2_mfgr_part = t2_mfgr_part + substring(pod_vpart,i,1).
        end.
    end.
    t1_mfgr_part = pod_vpart.
    find first vd_mstr where vd_addr = prh_vend no-lock no-error.
    if avail vd_mstr and vd_type = 'SSB' then t1_type = 'IQC'.
end.

/* get Location */
find first code_mstr where code_fldname = 'LocCtrl' 
                       and code_value = t1_site + substring(t1_part,1,2)
                       no-lock no-error.
if not avail code_mstr then do:
    find first code_mstr where code_fldname = 'LocCtrl' 
                           and code_value = t1_site + substring(t1_part,1,1)
                           no-lock no-error.
    if avail code_mstr then do:
        if t1_rir begins 'V' then do:
            if substring(t1_part,length(t1_part),1) = 'F' then
                t1_loc = lc(substring(code_user2,1,8)).
            else 
                t1_loc = lc(substring(code_user2,9,8)).
        end.
        else do:
            if substring(t1_part,length(t1_part),1) = 'F' then
                t1_loc = lc(substring(code_user1,1,8)).
            else
                t1_loc = lc(substring(code_user1,9,8)).
        end.
    end.
end.
find first ptp_det where ptp_site = prh_site 
                     and ptp_part = prh_part no-lock no-error.
if avail ptp_det then do:
    if ptp__chr02 <> '' and not t1_rir begins 'V' then
        t1_loc = ptp__chr02.
end.

/* get msd ,by ,wt */
find first pt_mstr where pt_part = t1_part no-lock no-error.
if avail pt_mstr then do:
    if pt_user1 <> ''  then t1_wt_ind = '*' + substring(pt_user1,7,2) + '*'.
    else t1_wt_ind = ''.
    t1_by = caps(t1_wt_ind) + ' ' + 'RECV' + ' ' + caps(pt_draw) + ' ' + 'ROHS'.
    t1_wt = pt_net_wt.
    if pt_user2 <> '' and pt_user2 <> '0' then do:
        t1_msd = 'MSL' + ' ' + pt_user2.
    end.
    else do:
        t1_msd = ''.
    end.
end.

/* get shelf life */
find first t_prefix where tp_entity = substring(prh_site,3,4) no-lock no-error.
if avail t_prefix then do:
    t1_by_site = yes.
    t1_site_sbu = substring(prh_site,3,4).
end.
else do:
    t1_by_site = no.
    t1_site_sbu = '0003'.
end.
if prh_part = '70634932R3F' or prh_part = '80342190R1F' then do:
    if prh_part = '70634932r3f' then t1_yy = 0.25.
    if prh_part = '80342190r1f' then t1_yy = 0.5.
end.
else do:
    find first t_prefix where tp_prefix = substring(prh_part,1,3)
                          and tp_entity = t1_site_sbu no-lock no-error.
    if avail t_prefix then do:
        t1_yy = tp_year.
    end.
    else do:
        find first t_prefix where tp_prefix = substring(prh_part,1,1)
                              and tp_entity = t1_site_sbu no-lock no-error.
        if avail t_prefix then t1_yy = tp_year.
        else                   t1_yy = 1.
    end.
end.

/* get cust_part */
find first qpl_mstr where qpl_site = t1_site and qpl_part = t1_part
                      and qpl_mfgr = t1_ref no-lock no-error.
if avail qpl_mstr then do:
    t1_cust_part = qpl_cust_part.
    if t1_mfgr_part = '' then t1_mfgr_part = qpl_mfgr_part.
end.
else do:
    t1_cust_part = ''.
end.
if t1_site <> 'mg8013' and t1_site <> 'mg8028' and t1_site <> 'mg8000' then do:
    find first cp_mstr where cp_cust = t1_site 
                         and cp_part = t1_part no-lock no-error.
    if avail cp_mstr then do:
        t1_cust_part = cp_cust_part.
    end.
end.

/* get urgent part information */
t1_u_part = ''.
find first tmp_tab where tmp_system = 'critical' and tmp_part = t1_part
                     and tmp_site   = t1_site no-lock no-error.
if avail tmp_tab then t1_u_part = '-U'.

run wse869d.p(t1_vend,t1_ref,t1_date_code, output t1_calc_mfgr).
t1_mfgr_date1 = t1_calc_mfgr.
if t1_mfgr_date1 = ? then do:
    if not t1_rir begins 'B' and not t1_rir begins 'W' and
       not t1_rir begins 'V' and not t1_rir begins 'Q' then
       t1_mfgr_date1 = today.
    else t1_mfgr_date1 = date(substring(t1_rir,4,2) + '/' +
                              substring(t1_rir,6,2) + '/' +
                              substring(t1_rir,2,2)).
end.
if t1_mfgr_date = ? then t1_mfgr_date = t1_mfgr_date1.

if t1_expi_date <> ? then t1_expi_type = 'E'.
else do:
    if t1_calc_mfgr <> ? then do:
        if t1_mfgr_date = t1_mfgr_date1 then t1_expi_type = 'D'.
                                        else t1_expi_type = 'M'.
    end.
    else do:
        if t1_mfgr_date = today or (t1_rir begins 'B' or t1_rir begins 'W' or
                                    t1_rir begins 'V' or t1_rir begins 'Q')
        then t1_expi_type = 'R'.
        else t1_expi_type = 'M'.
    end.
end.

if t1_date_code = '' and t1_lot <> '' then do:
    t1_dc_control = yes.
end.

if t1_expi_date = ? then t1_expi_date = t1_mfgr_date + round(t1_yy * 365,0).
t1_life_time = t1_expi_date - today.
if t1_life_time < round(t1_yy * 365 / 3,0) then t1_shelf = yes.
if t1_shelf = yes then do:
    if t1_type = 'sts' then t1_type = '*IQC'.
end.
t1_type = t1_type + t1_u_part.

/* Change Type if input mfgr part not matches qpl mfgr part ****
*******                wwz 05/24/2013                       ***/
t_mrb = no.
if t2_mfgr_part <> t1_in_mfgr_part then do:
    find first qpl_mstr where qpl_site = t1_site
                          and qpl_part = t1_part
			  and qpl_mfgr_part = t1_in_mfgr_part
			  and qpl_appr_st = 'A' no-lock no-error.
    if not avail qpl_mstr then do:
        t1_type = t1_type + ' (MRB) '.
        t_mrb = yes.
    end.
end.

do transaction:
    if t1_lot <> '' and t1_date_code = '' then
        t1_date_code = t1_lot.

    create wse869f1.
    wse869f1_pims      = caps(t1_pims). 
    /*for testing until implement*/
    /* wse869f1_pims      = 'TEST' + substring(t1_pims,5). */
    wse869f1_site      = caps(t1_site).
    wse869f1_part      = caps(t1_part).
    wse869f1_rir       = caps(t1_rir).
    wse869f1_date_code = t1_date_code.
    wse869f1_mfgr_lot  = t1_ref_pims.
    wse869f1_qty       = t1_qty.
    wse869f1_ref       = t1_ref.
    wse869f1_date      = today.
    wse869f1_chr01     = t1_userid.
    wse869f1_chr02     = string(time,'>>>>9').
    wse869f1_chr03     = t1_lot.
    wse869f1_dte01     = t1_mfgr_date.
    wse869f1_dte02     = t1_expi_date.
    wse869f1_dte03     = t1_calc_mfgr.
    if t1_dc_control = yes then
        wse869f1_dte04     = today.
    wse869f1_int01     = t1_yy * 100.
    if t1_shelf = yes then wse869f1_chr04 = 'Yes'.
    else                   wse869f1_chr04 = 'No'.
    wse869f1_chr05     = t1_expi_type.
    
    if t1_shelf = yes then do:
        create tmp_tab.
        tmp_system  = 'alert_expi'.
        tmp_site    = t1_site.
        tmp_part    = t1_part.
        tmp_key3    = t1_rir.
        tmp_chr01   = t1_userid.
        tmp_dte01   = today.
        tmp_int01   = time.
        tmp_chr02   = t1_date_code.
        tmp_dte02   = t1_mfgr_date.
        tmp_dte03   = t1_expi_date.
        tmp_dec01   = t1_life_time.
        tmp_dec02   = t1_yy.
    end.
    
    if t_mrb = yes then do:
        create tmp_tab.
        tmp_system = 'pims_mrb'.
        tmp_site   = caps(t1_site).
        tmp_part   = caps(t1_part).
        tmp_key3   = caps(t1_pims).
        tmp_chr01  = caps(t1_rir).
        tmp_dte01  = today.
        tmp_int01  = time.
        tmp_chr02  = t1_mfgr_part.
        tmp_chr03  = t1_in_mfgr_part.
        tmp_chr04  = t1_date_code.
    end.

    create wsas002.
    wsas002_pims      = t1_pims.
    wsas002_site      = t1_site.
    wsas002_part      = t1_part.
    wsas002_rir       = t1_rir.
    wsas002_loc       = t1_loc.
    wsas002_type      = t1_type.
    wsas002_ref       = t1_ref.
    wsas002_qty_per   = t1_qty.
    wsas002_expi_date = t1_expi_date.
    wsas002_expi_type = t1_expi_type.
    wsas002_mfgr_part = t1_mfgr_part.
    wsas002_cust_part = t1_cust_part.
    wsas002_date_code = t1_date_code.
    wsas002_by        = t1_by.
    wsas002_wt        = t1_wt.
    wsas002_msd       = t1_msd.
    wsas002_nw_po     = t1_nw_po.
    opcount = opcount + 1.
end.
/*
release wse869f1.
for each wsas002: 
    disp wsas002 with 2 col.
end.
*/
/*end of program*/


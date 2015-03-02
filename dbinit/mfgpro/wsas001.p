{d:\mfgpro\wsappsrc\wsas001.i}    

def input parameter w_para  as char format 'x(30)' no-undo.
def output parameter table for wsas001.
def output parameter opcount as int no-undo.
/**
def var w_para as char format 'x(30)' init 'yageoe,06/01/13,06/17/13'.
def var opcount as integer.
**/
def var t1_dn         as char format 'x(16)'.
def var t1_deli_datex as char.
def var t1_deli_date  as date.
def var t1_urg        as char format 'x(3)'.
def var t1_conn       as char.
def var t1_wt         as decimal format '>>>,>>9.9999'.
def var t1_wt_ind     as char.
def var t1_msd        as char.
def var t1_cust_part  as char format 'x(30)'.
def var t1_loc        as char.
def var t1_site_sbu   as char.
def var t1_yy         as decimal.
def var t1_vend       as char.
def var t1_date1      as date.
def var t1_date2      as date.
def var t1_vpart      as char format 'x(30)'.
def var t1_mpq        like ptp_ord_mult.
def var i             as integer.

def temp-table t_prefix
    field  tp_entity as char
    field  tp_prefix as char
    field  tp_year   as decimal
    index tp_key1
          tp_entity
          tp_prefix.

opcount = 0.
if num-entries(w_para) <> 3 then do:
   opcount = -1.
   leave.
end.
/***
t1_dn = entry(1,w_para).
t1_deli_datex = entry(2,w_para).
t1_deli_date = date(t1_deli_datex).
***/
t1_vend = entry(1,w_para).
t1_date1 = date(entry(2,w_para)).
t1_date2 = date(entry(3,w_para)).

do transaction:
   for each dtab where dtab_x14 begins 'sl' no-lock:
       create t_prefix.
              tp_entity = substring(dtab_x14,3,4).
              tp_prefix = substring(dtab_x14,7,length(dtab_x14) - 6).
              tp_year   = decimal(substring(dtab_x40,1,length(dtab_x40))).
   end.
end.

for each dnd_det where dnd_vend = t1_vend 
                   and dnd_deli_date >= t1_date1 
                   and dnd_deli_date <= t1_date2 no-lock:
    if dnd_rir = '' then next.
    find first pod_det where pod_nbr = dnd_ponbr and 
                             pod_line = dnd_poline no-lock no-error.
    if not avail pod_det then next.
    find first po_mstr where po_nbr = pod_nbr no-lock no-error.
    if not avail po_mstr then next.
    t1_conn = ''.
    t1_wt   = 0.
    t1_wt_ind = ''.
    t1_msd = ''.
    t1_loc  = ''.

    /* trim the mfgr part blank wwz 07/17/2013 begin */
    t1_vpart = ''.
    do i = 1 to length(pod_vpart):
        if substring(pod_vpart,i,1) = '' then next.
        else do:
            t1_vpart = t1_vpart + substring(pod_vpart,i,1).
        end.
    end.
    /* trim the mfgr part blank wwz 07/17/2013 end */

    find first pt_mstr where pt_part = dnd_part no-lock no-error.
    if avail pt_mstr then do:
        t1_conn = pt_draw.
        t1_wt = pt_net_wt.
        if pt_user1 <> '' then 
            t1_wt_ind = '*' + substring(pt_user1,7,2) + '*'.
        if pt_user2 <> '' and pt_user2 <> '0' then do:
            t1_msd = "MSL " + pt_user2.
        end.
    end.
    t1_mpq = 0.
    find first ptp_det where ptp_site = po_site and
                             ptp_part = dnd_part no-lock no-error.
    if avail ptp_det  then do:
        if ptp__chr02 <> '' and not dnd_rir begins 'V' then 
            t1_loc = ptp__chr02.
        t1_mpq = ptp_ord_mult.
    end.
        
    find first qpl_mstr where qpl_site = po_site and
                              qpl_part = dnd_part and
                              qpl_mfgr = pod__chr03
                              no-lock no-error.
    if avail qpl_mstr then do:
        t1_cust_part = qpl_cust_part.
    end.
    else do:
        t1_cust_part = ''.
    end.   
    if po_site <> 'mg8000' then do:
        find first cp_mstr where cp_cust = po_site and 
                                 cp_part = dnd_part no-lock no-error.
        if avail cp_mstr then do:
            t1_cust_part = cp_cust_part.
        end.                         
    end.   
         
    t1_site_sbu = '0003'.
    if dnd_part = '70634932R3F' or dnd_part = '80342190R1F' then do:
        if dnd_part = '70634932R3F' then  t1_yy = 0.25.
        if dnd_part = '80342190R1F' then  t1_yy = 0.5.
    end.
    else do:   
        find first t_prefix where tp_prefix = substring(dnd_part,1,3)
                              and tp_entity = t1_site_sbu
                              no-lock no-error.
        if avail t_prefix then do:
            t1_yy = tp_year.
        end.
        else do:
            find first t_prefix where tp_prefix = substring(dnd_part,1,2)
                                  and tp_entity = t1_site_sbu
                                  no-lock no-error.
            if avail t_prefix then do:
                t1_yy = tp_year.
            end.
            else do:
                find first t_prefix where tp_prefix = substring(dnd_part,1,1)
                                      and tp_entity = t1_site_sbu
                                      no-lock no-error.
                if avail t_prefix then
                    t1_yy = tp_year.
                else
                    t1_yy = 1.
            end.
        end.
    end.

    find first tmp_tab where tmp_system = 'critical' and 
                             tmp_part = dnd_part     and 
                             tmp_site = po_site 
                             no-lock no-error.
    if avail tmp_tab then t1_urg = 'yes'.
    else                  t1_urg = 'no'.
    opcount = opcount + 1.
    create wsas001.
    t_dn        = dnd_nbr.
    t_deli_date = dnd_deli_date.
    t_rir       = dnd_rir.
    t_part      = dnd_part.
    t_po        = dnd_ponbr.
    t_site      = po_site.
    t_qty       = dnd_qty.
    t_supp      = dnd_vend.
    t_mfgr      = pod__chr03.
    t_mfgr_part = trim(t1_vpart).
    t_id        = opcount.
    t_urg       = t1_urg.
    t_msd       = t1_msd.
    t_wt        = t1_wt.
    t_wt_ind    = t1_wt_ind.
    t_conn      = t1_conn.
    t_cust_part = t1_cust_part.
    t_loc       = t1_loc.
    t_shelf_life = t1_yy.
    t_mpq        = ptp_ord_mult.
end.


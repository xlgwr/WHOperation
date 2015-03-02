{d:\mfgpro\wsappsrc\wsas003.i}    
def input parameter w_para  as char format 'x(20)' no-undo.
def output parameter table for wsas003.
def output parameter opcount as int no-undo.
def var  t1_system   as char.
def var  t1_today    as char.
def var  t1_pims     as char.
if num-entries(w_para) <> 1 then do:
   opcount = -1.
   leave.
end.
t1_system       = entry(1,w_para).
         
opcount = 0.
t1_today = substring(string(year(today),'9999'),3,2) +
           string(month(today),'99') +
           string(day(today),'99').
do transaction:
   find first dtab where dtab_x14 = 'PIMS' + t1_today no-error.
   if not avail dtab then do:
      create dtab.
      dtab_x14='PIMS' + t1_today.
      if t1_system = 'Wellop' then do:
         t1_pims   = '600001'.
         dtab_x40  = '600002'.
      end.
      else do:
         t1_pims   = '000001'.
         dtab_x40  = '000002'.
      end.
   end.
   else do:     
      find first dtab where dtab_x14 = 'PIMS' + t1_today 
                      exclusive-lock.
      t1_pims = substring(dtab_x40,1,6).
      dtab_x40 = string(integer(t1_pims)+ 1,'999999').
   end.
   opcount = 1.
   create wsas003.
   wsas003_pims = t1_today + t1_pims.
   release dtab.              
   
end.

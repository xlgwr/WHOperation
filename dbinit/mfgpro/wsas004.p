{d:\mfgpro\wsappsrc\wsas004.i}    

def input parameter w_para  as char format 'x(20)' no-undo.
def output parameter table for wsas004.
def output parameter opcount as int no-undo.


def var t1_userid     as char.
def var t1_passwd     as char.


opcount = 0.
if num-entries(w_para) <> 2 then do:
   opcount = -1.
   leave.
end.
t1_userid = entry(1,w_para).
t1_passwd = entry(2,w_para).

find first usr_mstr where usr_userid = trim(t1_userid) and 
           encode(trim(t1_passwd)) = usr_passwd no-lock no-error.
if  avail usr_mstr then do:
   opcount = opcount + 1.
   create wsas004.
   wsas004_usrid  = usr_userid.
   wsas004_name   = usr_name.
end.




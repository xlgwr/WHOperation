namespace WHOperation.EF.DW
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class DW_Develop : DbContext
    {
        public DW_Develop()
            : base("data source=142.2.70.209;initial catalog=DW_Develop;user id=qims_user;Password=information;MultipleActiveResultSets=True;App=EntityFramework")//"name=DW_Develop"
        {
        }

        public virtual DbSet<ac_mstr> ac_mstr { get; set; }
        public virtual DbSet<ad_mstr> ad_mstr { get; set; }
        public virtual DbSet<dimDate> dimDate { get; set; }
        public virtual DbSet<Document_Definition_5> Document_Definition_5 { get; set; }
        public virtual DbSet<ftn_mst> ftn_mst { get; set; }
        public virtual DbSet<gmtbom> gmtbom { get; set; }
        public virtual DbSet<po_mstr> po_mstr { get; set; }
        public virtual DbSet<pt_mstr> pt_mstr { get; set; }
        public virtual DbSet<qpl_mstr> qpl_mstr { get; set; }
        public virtual DbSet<si_mstr> si_mstr { get; set; }
        public virtual DbSet<sq_mstr> sq_mstr { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<Tally> Tally { get; set; }
        public virtual DbSet<Time> Time { get; set; }
        public virtual DbSet<tmp_tr_hist> tmp_tr_hist { get; set; }
        public virtual DbSet<tr_hist> tr_hist { get; set; }
        public virtual DbSet<tr_hist1> tr_hist1 { get; set; }
        public virtual DbSet<fact_invbal> fact_invbal { get; set; }
        public virtual DbSet<gmtFG> gmtFG { get; set; }
        public virtual DbSet<gmtRM> gmtRM { get; set; }
        public virtual DbSet<MyTable> MyTable { get; set; }
        public virtual DbSet<ospo> ospo { get; set; }
        public virtual DbSet<pbom> pbom { get; set; }
        public virtual DbSet<tblprolog> tblprolog { get; set; }
        public virtual DbSet<tr_hist_test> tr_hist_test { get; set; }
        public virtual DbSet<wst055f2> wst055f2 { get; set; }
        public virtual DbSet<v_receiving> v_receiving { get; set; }
        public virtual DbSet<v_receiving_2> v_receiving_2 { get; set; }
        public virtual DbSet<v_receiving3> v_receiving3 { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ac_mstr>()
                .Property(e => e.ac_code)
                .IsFixedLength();

            modelBuilder.Entity<ac_mstr>()
                .Property(e => e.ac_desc)
                .IsFixedLength();

            modelBuilder.Entity<ac_mstr>()
                .Property(e => e.ac_desc2)
                .IsFixedLength();

            modelBuilder.Entity<ac_mstr>()
                .Property(e => e.ac_curr)
                .IsFixedLength();

            modelBuilder.Entity<ac_mstr>()
                .Property(e => e.ac_type)
                .IsFixedLength();

            modelBuilder.Entity<ac_mstr>()
                .Property(e => e.ac_active)
                .IsFixedLength();

            modelBuilder.Entity<ad_mstr>()
                .Property(e => e.ad_addr)
                .IsUnicode(false);

            modelBuilder.Entity<ad_mstr>()
                .Property(e => e.ad_name)
                .IsUnicode(false);

            modelBuilder.Entity<ad_mstr>()
                .Property(e => e.ad_line1)
                .IsUnicode(false);

            modelBuilder.Entity<ad_mstr>()
                .Property(e => e.ad_line2)
                .IsUnicode(false);

            modelBuilder.Entity<ad_mstr>()
                .Property(e => e.ad_line3)
                .IsUnicode(false);

            modelBuilder.Entity<ad_mstr>()
                .Property(e => e.ad_city)
                .IsUnicode(false);

            modelBuilder.Entity<ad_mstr>()
                .Property(e => e.ad_state)
                .IsUnicode(false);

            modelBuilder.Entity<ad_mstr>()
                .Property(e => e.ad_zip)
                .IsUnicode(false);

            modelBuilder.Entity<ad_mstr>()
                .Property(e => e.ad_type)
                .IsUnicode(false);

            modelBuilder.Entity<ad_mstr>()
                .Property(e => e.ad_attn)
                .IsUnicode(false);

            modelBuilder.Entity<ad_mstr>()
                .Property(e => e.ad_phone)
                .IsUnicode(false);

            modelBuilder.Entity<ad_mstr>()
                .Property(e => e.ad_ext)
                .IsUnicode(false);

            modelBuilder.Entity<ad_mstr>()
                .Property(e => e.ad_ref)
                .IsUnicode(false);

            modelBuilder.Entity<ad_mstr>()
                .Property(e => e.ad_sort)
                .IsUnicode(false);

            modelBuilder.Entity<ad_mstr>()
                .Property(e => e.ad_country)
                .IsUnicode(false);

            modelBuilder.Entity<ad_mstr>()
                .Property(e => e.ad_user1)
                .IsUnicode(false);

            modelBuilder.Entity<ad_mstr>()
                .Property(e => e.ad_user2)
                .IsUnicode(false);

            modelBuilder.Entity<ad_mstr>()
                .Property(e => e.ad_userid)
                .IsUnicode(false);

            modelBuilder.Entity<dimDate>()
                .Property(e => e.EnglishMonthName)
                .IsFixedLength();

            modelBuilder.Entity<Document_Definition_5>()
                .Property(e => e.invno)
                .IsFixedLength();

            modelBuilder.Entity<ftn_mst>()
                .Property(e => e.ftn_comp)
                .IsUnicode(false);

            modelBuilder.Entity<ftn_mst>()
                .Property(e => e.ftn_nbr)
                .IsUnicode(false);

            modelBuilder.Entity<ftn_mst>()
                .Property(e => e.ftn_site)
                .IsUnicode(false);

            modelBuilder.Entity<ftn_mst>()
                .Property(e => e.ftn_part)
                .IsUnicode(false);

            modelBuilder.Entity<ftn_mst>()
                .Property(e => e.ftn_wo)
                .IsUnicode(false);

            modelBuilder.Entity<ftn_mst>()
                .Property(e => e.ftn_lot)
                .IsUnicode(false);

            modelBuilder.Entity<ftn_mst>()
                .Property(e => e.ftn_lot5)
                .IsUnicode(false);

            modelBuilder.Entity<ftn_mst>()
                .Property(e => e.ftn_cre_user)
                .IsUnicode(false);

            modelBuilder.Entity<ftn_mst>()
                .Property(e => e.ftn_con_user)
                .IsUnicode(false);

            modelBuilder.Entity<ftn_mst>()
                .Property(e => e.ftn_apr_user)
                .IsUnicode(false);

            modelBuilder.Entity<ftn_mst>()
                .Property(e => e.ftn_pas_user)
                .IsUnicode(false);

            modelBuilder.Entity<ftn_mst>()
                .Property(e => e.ftn_rej_user)
                .IsUnicode(false);

            modelBuilder.Entity<ftn_mst>()
                .Property(e => e.ftn_loc)
                .IsUnicode(false);

            modelBuilder.Entity<ftn_mst>()
                .Property(e => e.ftn_po)
                .IsUnicode(false);

            modelBuilder.Entity<ftn_mst>()
                .Property(e => e.ftn_status)
                .IsUnicode(false);

            modelBuilder.Entity<gmtbom>()
                .Property(e => e.Usage)
                .HasPrecision(15, 9);

            modelBuilder.Entity<po_mstr>()
                .Property(e => e.po_comp)
                .IsUnicode(false);

            modelBuilder.Entity<po_mstr>()
                .Property(e => e.po_nbr)
                .IsUnicode(false);

            modelBuilder.Entity<po_mstr>()
                .Property(e => e.po_vend)
                .IsUnicode(false);

            modelBuilder.Entity<po_mstr>()
                .Property(e => e.po_ship)
                .IsUnicode(false);

            modelBuilder.Entity<po_mstr>()
                .Property(e => e.po_rmks)
                .IsUnicode(false);

            modelBuilder.Entity<po_mstr>()
                .Property(e => e.po_cr_terms)
                .IsUnicode(false);

            modelBuilder.Entity<po_mstr>()
                .Property(e => e.po_buyer)
                .IsUnicode(false);

            modelBuilder.Entity<po_mstr>()
                .Property(e => e.po_shipvia)
                .IsUnicode(false);

            modelBuilder.Entity<po_mstr>()
                .Property(e => e.po_fob)
                .IsUnicode(false);

            modelBuilder.Entity<po_mstr>()
                .Property(e => e.po_bill)
                .IsUnicode(false);

            modelBuilder.Entity<po_mstr>()
                .Property(e => e.po_confirm)
                .IsUnicode(false);

            modelBuilder.Entity<po_mstr>()
                .Property(e => e.po_curr)
                .IsUnicode(false);

            modelBuilder.Entity<po_mstr>()
                .Property(e => e.po_ex_rate)
                .HasPrecision(23, 10);

            modelBuilder.Entity<po_mstr>()
                .Property(e => e.po_site)
                .IsUnicode(false);

            modelBuilder.Entity<po_mstr>()
                .Property(e => e.po_type)
                .IsUnicode(false);

            modelBuilder.Entity<po_mstr>()
                .Property(e => e.po_user_id)
                .IsUnicode(false);

            modelBuilder.Entity<po_mstr>()
                .Property(e => e.po_ent_ex)
                .HasPrecision(23, 10);

            modelBuilder.Entity<po_mstr>()
                .Property(e => e.po__chr07)
                .IsUnicode(false);

            modelBuilder.Entity<po_mstr>()
                .Property(e => e.po__chr08)
                .IsUnicode(false);

            modelBuilder.Entity<po_mstr>()
                .Property(e => e.po__chr09)
                .IsUnicode(false);

            modelBuilder.Entity<po_mstr>()
                .Property(e => e.po__chr10)
                .IsUnicode(false);

            modelBuilder.Entity<pt_mstr>()
                .Property(e => e.pt_part)
                .IsUnicode(false);

            modelBuilder.Entity<pt_mstr>()
                .Property(e => e.pt_desc1)
                .IsUnicode(false);

            modelBuilder.Entity<pt_mstr>()
                .Property(e => e.pt_desc2)
                .IsUnicode(false);

            modelBuilder.Entity<pt_mstr>()
                .Property(e => e.pt_part_type)
                .IsUnicode(false);

            modelBuilder.Entity<pt_mstr>()
                .Property(e => e.pt_um)
                .IsUnicode(false);

            modelBuilder.Entity<pt_mstr>()
                .Property(e => e.pt_conn)
                .IsUnicode(false);

            modelBuilder.Entity<pt_mstr>()
                .Property(e => e.pt_net_wt)
                .HasPrecision(28, 4);

            modelBuilder.Entity<qpl_mstr>()
                .Property(e => e.qpl_key)
                .IsUnicode(false);

            modelBuilder.Entity<qpl_mstr>()
                .Property(e => e.qpl_part)
                .IsUnicode(false);

            modelBuilder.Entity<qpl_mstr>()
                .Property(e => e.qpl_site)
                .IsUnicode(false);

            modelBuilder.Entity<qpl_mstr>()
                .Property(e => e.qpl_mfgr)
                .IsUnicode(false);

            modelBuilder.Entity<qpl_mstr>()
                .Property(e => e.qpl_mfgr_part)
                .IsUnicode(false);

            modelBuilder.Entity<qpl_mstr>()
                .Property(e => e.qpl_appr_st)
                .IsUnicode(false);

            modelBuilder.Entity<qpl_mstr>()
                .Property(e => e.qpl_rmk)
                .IsUnicode(false);

            modelBuilder.Entity<qpl_mstr>()
                .Property(e => e.qpl_rec_type)
                .IsUnicode(false);

            modelBuilder.Entity<qpl_mstr>()
                .Property(e => e.qpl_cust_part)
                .IsUnicode(false);

            modelBuilder.Entity<si_mstr>()
                .Property(e => e.si_site)
                .IsUnicode(false);

            modelBuilder.Entity<si_mstr>()
                .Property(e => e.si_desc)
                .IsUnicode(false);

            modelBuilder.Entity<si_mstr>()
                .Property(e => e.si_entity)
                .IsUnicode(false);

            modelBuilder.Entity<si_mstr>()
                .Property(e => e.si_last_fcid)
                .IsUnicode(false);

            modelBuilder.Entity<si_mstr>()
                .Property(e => e.si_comp)
                .IsUnicode(false);

            modelBuilder.Entity<si_mstr>()
                .Property(e => e.si__cac)
                .IsUnicode(false);

            modelBuilder.Entity<si_mstr>()
                .Property(e => e.si__fcid_cs)
                .IsUnicode(false);

            modelBuilder.Entity<si_mstr>()
                .Property(e => e.si__do)
                .IsUnicode(false);

            modelBuilder.Entity<si_mstr>()
                .Property(e => e.si__no_fc)
                .IsUnicode(false);

            modelBuilder.Entity<si_mstr>()
                .Property(e => e.si__show_cust_pn)
                .IsUnicode(false);

            modelBuilder.Entity<sq_mstr>()
                .Property(e => e.sq_type)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<sq_mstr>()
                .Property(e => e.sq_name)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<sq_mstr>()
                .Property(e => e.sq_unit)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<sq_mstr>()
                .Property(e => e.sq_unit_id)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<sq_mstr>()
                .Property(e => e.sq_spec)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tmp_tr_hist>()
                .Property(e => e.tr_comp)
                .IsUnicode(false);

            modelBuilder.Entity<tmp_tr_hist>()
                .Property(e => e.tr_type)
                .IsUnicode(false);

            modelBuilder.Entity<tmp_tr_hist>()
                .Property(e => e.tr_site)
                .IsUnicode(false);

            modelBuilder.Entity<tmp_tr_hist>()
                .Property(e => e.tr_part)
                .IsUnicode(false);

            modelBuilder.Entity<tmp_tr_hist>()
                .Property(e => e.tr_serial)
                .IsUnicode(false);

            modelBuilder.Entity<tmp_tr_hist>()
                .Property(e => e.tr_loc)
                .IsUnicode(false);

            modelBuilder.Entity<tmp_tr_hist>()
                .Property(e => e.tr_ref)
                .IsUnicode(false);

            modelBuilder.Entity<tmp_tr_hist>()
                .Property(e => e.tr_qty_loc)
                .HasPrecision(18, 6);

            modelBuilder.Entity<tmp_tr_hist>()
                .Property(e => e.tr_nbr)
                .IsUnicode(false);

            modelBuilder.Entity<tmp_tr_hist>()
                .Property(e => e.tr_lot)
                .IsUnicode(false);

            modelBuilder.Entity<tmp_tr_hist>()
                .Property(e => e.tr_rmks)
                .IsUnicode(false);

            modelBuilder.Entity<tmp_tr_hist>()
                .Property(e => e.tr_program)
                .IsUnicode(false);

            modelBuilder.Entity<tmp_tr_hist>()
                .Property(e => e.tr_userid)
                .IsUnicode(false);

            modelBuilder.Entity<tr_hist>()
                .Property(e => e.tr_comp)
                .IsUnicode(false);

            modelBuilder.Entity<tr_hist>()
                .Property(e => e.tr_type)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tr_hist>()
                .Property(e => e.tr_site)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tr_hist>()
                .Property(e => e.tr_part)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tr_hist>()
                .Property(e => e.tr_serial)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tr_hist>()
                .Property(e => e.tr_loc)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tr_hist>()
                .Property(e => e.tr_ref)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tr_hist>()
                .Property(e => e.tr_qty_loc)
                .HasPrecision(18, 6);

            modelBuilder.Entity<tr_hist>()
                .Property(e => e.tr_nbr)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tr_hist>()
                .Property(e => e.tr_lot)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tr_hist>()
                .Property(e => e.tr_rmks)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tr_hist>()
                .Property(e => e.tr_program)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tr_hist>()
                .Property(e => e.tr_userid)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tr_hist1>()
                .Property(e => e.tr_type)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tr_hist1>()
                .Property(e => e.tr_site)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tr_hist1>()
                .Property(e => e.tr_part)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tr_hist1>()
                .Property(e => e.tr_serial)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tr_hist1>()
                .Property(e => e.tr_loc)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tr_hist1>()
                .Property(e => e.tr_ref)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tr_hist1>()
                .Property(e => e.tr_qty_loc)
                .HasPrecision(18, 6);

            modelBuilder.Entity<tr_hist1>()
                .Property(e => e.tr_nbr)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tr_hist1>()
                .Property(e => e.tr_lot)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tr_hist1>()
                .Property(e => e.tr_rmks)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tr_hist1>()
                .Property(e => e.tr_program)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tr_hist1>()
                .Property(e => e.tr_userid)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tr_hist1>()
                .Property(e => e.tr_comp)
                .IsUnicode(false);

            modelBuilder.Entity<fact_invbal>()
                .Property(e => e.dw_site)
                .IsUnicode(false);

            modelBuilder.Entity<fact_invbal>()
                .Property(e => e.dw_part)
                .IsUnicode(false);

            modelBuilder.Entity<fact_invbal>()
                .Property(e => e.dw_type)
                .IsUnicode(false);

            modelBuilder.Entity<fact_invbal>()
                .Property(e => e.dw_qty)
                .HasPrecision(18, 4);

            modelBuilder.Entity<fact_invbal>()
                .Property(e => e.dw_model)
                .IsUnicode(false);

            modelBuilder.Entity<fact_invbal>()
                .Property(e => e.dw_usage)
                .HasPrecision(18, 6);

            modelBuilder.Entity<fact_invbal>()
                .Property(e => e.dw_lot)
                .IsUnicode(false);

            modelBuilder.Entity<fact_invbal>()
                .Property(e => e.dw_loc)
                .IsUnicode(false);

            modelBuilder.Entity<gmtFG>()
                .Property(e => e.Znamec)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<gmtFG>()
                .Property(e => e.zspecial)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<gmtFG>()
                .Property(e => e.zprice)
                .HasPrecision(15, 6);

            modelBuilder.Entity<gmtFG>()
                .Property(e => e.Zunit)
                .IsUnicode(false);

            modelBuilder.Entity<gmtFG>()
                .Property(e => e.Zcountry)
                .IsUnicode(false);

            modelBuilder.Entity<gmtRM>()
                .Property(e => e.Znamec)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<gmtRM>()
                .Property(e => e.zspecial)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<gmtRM>()
                .Property(e => e.zprice)
                .HasPrecision(15, 6);

            modelBuilder.Entity<gmtRM>()
                .Property(e => e.zplanqty)
                .HasPrecision(15, 4);

            modelBuilder.Entity<gmtRM>()
                .Property(e => e.Zunit)
                .IsUnicode(false);

            modelBuilder.Entity<gmtRM>()
                .Property(e => e.Zcountry)
                .IsUnicode(false);

            modelBuilder.Entity<ospo>()
                .Property(e => e.Company)
                .IsUnicode(false);

            modelBuilder.Entity<ospo>()
                .Property(e => e.Buyer)
                .IsUnicode(false);

            modelBuilder.Entity<ospo>()
                .Property(e => e.Supplier)
                .IsUnicode(false);

            modelBuilder.Entity<ospo>()
                .Property(e => e.Order)
                .IsUnicode(false);

            modelBuilder.Entity<ospo>()
                .Property(e => e.PO_FOB)
                .IsUnicode(false);

            modelBuilder.Entity<ospo>()
                .Property(e => e.MPO)
                .IsUnicode(false);

            modelBuilder.Entity<ospo>()
                .Property(e => e.Buyer_Name)
                .IsUnicode(false);

            modelBuilder.Entity<ospo>()
                .Property(e => e.PO_level1)
                .IsUnicode(false);

            modelBuilder.Entity<ospo>()
                .Property(e => e.PO_level2)
                .IsUnicode(false);

            modelBuilder.Entity<ospo>()
                .Property(e => e.PO_level3)
                .IsUnicode(false);

            modelBuilder.Entity<ospo>()
                .Property(e => e.DO)
                .IsUnicode(false);

            modelBuilder.Entity<ospo>()
                .Property(e => e.Site)
                .IsUnicode(false);

            modelBuilder.Entity<ospo>()
                .Property(e => e.Item_no)
                .IsUnicode(false);

            modelBuilder.Entity<ospo>()
                .Property(e => e.ABC)
                .IsUnicode(false);

            modelBuilder.Entity<ospo>()
                .Property(e => e.Desc1)
                .IsUnicode(false);

            modelBuilder.Entity<ospo>()
                .Property(e => e.Desc2)
                .IsUnicode(false);

            modelBuilder.Entity<ospo>()
                .Property(e => e.MFGR)
                .IsUnicode(false);

            modelBuilder.Entity<ospo>()
                .Property(e => e.MFGR_Name)
                .IsUnicode(false);

            modelBuilder.Entity<ospo>()
                .Property(e => e.PO_QPL_PART)
                .IsUnicode(false);

            modelBuilder.Entity<ospo>()
                .Property(e => e.CUST_PART)
                .IsUnicode(false);

            modelBuilder.Entity<ospo>()
                .Property(e => e.VMI)
                .IsUnicode(false);

            modelBuilder.Entity<ospo>()
                .Property(e => e.WO_ID)
                .IsUnicode(false);

            modelBuilder.Entity<ospo>()
                .Property(e => e.SDC)
                .IsUnicode(false);

            modelBuilder.Entity<ospo>()
                .Property(e => e.WELLOP_PR)
                .IsUnicode(false);

            modelBuilder.Entity<ospo>()
                .Property(e => e.Message_For_Alert)
                .IsUnicode(false);

            modelBuilder.Entity<ospo>()
                .Property(e => e.ROHS)
                .IsUnicode(false);

            modelBuilder.Entity<ospo>()
                .Property(e => e.MRP_OAR)
                .IsUnicode(false);

            modelBuilder.Entity<ospo>()
                .Property(e => e.De_expdeite)
                .IsUnicode(false);

            modelBuilder.Entity<ospo>()
                .Property(e => e.Expedite)
                .IsUnicode(false);

            modelBuilder.Entity<ospo>()
                .Property(e => e.Cancel)
                .IsUnicode(false);

            modelBuilder.Entity<ospo>()
                .Property(e => e.UnChange)
                .IsUnicode(false);

            modelBuilder.Entity<ospo>()
                .Property(e => e.ACO_Hist)
                .IsUnicode(false);

            modelBuilder.Entity<pbom>()
                .Property(e => e.Model)
                .IsUnicode(false);

            modelBuilder.Entity<pbom>()
                .Property(e => e.Comp)
                .IsUnicode(false);

            modelBuilder.Entity<pbom>()
                .Property(e => e.Usage)
                .HasPrecision(28, 6);

            modelBuilder.Entity<pbom>()
                .Property(e => e.Column_3)
                .IsUnicode(false);

            modelBuilder.Entity<tblprolog>()
                .Property(e => e.Site)
                .IsFixedLength();

            modelBuilder.Entity<tblprolog>()
                .Property(e => e.process)
                .IsFixedLength();

            modelBuilder.Entity<tr_hist_test>()
                .Property(e => e.tr_comp)
                .IsUnicode(false);

            modelBuilder.Entity<tr_hist_test>()
                .Property(e => e.tr_type)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tr_hist_test>()
                .Property(e => e.tr_site)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tr_hist_test>()
                .Property(e => e.tr_part)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tr_hist_test>()
                .Property(e => e.tr_serial)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tr_hist_test>()
                .Property(e => e.tr_loc)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tr_hist_test>()
                .Property(e => e.tr_ref)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tr_hist_test>()
                .Property(e => e.tr_qty_loc)
                .HasPrecision(18, 6);

            modelBuilder.Entity<tr_hist_test>()
                .Property(e => e.tr_nbr)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tr_hist_test>()
                .Property(e => e.tr_lot)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tr_hist_test>()
                .Property(e => e.tr_rmks)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tr_hist_test>()
                .Property(e => e.tr_program)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tr_hist_test>()
                .Property(e => e.tr_userid)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<wst055f2>()
                .Property(e => e.wst055f2_comp)
                .IsUnicode(false);

            modelBuilder.Entity<v_receiving>()
                .Property(e => e.prh_nbr)
                .IsUnicode(false);

            modelBuilder.Entity<v_receiving>()
                .Property(e => e.prh_receiver)
                .IsUnicode(false);

            modelBuilder.Entity<v_receiving>()
                .Property(e => e.prh_rcp_date)
                .IsUnicode(false);

            modelBuilder.Entity<v_receiving>()
                .Property(e => e.prh_site)
                .IsUnicode(false);

            modelBuilder.Entity<v_receiving>()
                .Property(e => e.prh_part)
                .IsUnicode(false);

            modelBuilder.Entity<v_receiving>()
                .Property(e => e.Qty)
                .HasPrecision(23, 10);

            modelBuilder.Entity<v_receiving>()
                .Property(e => e.HKAmt)
                .HasPrecision(38, 11);

            modelBuilder.Entity<v_receiving_2>()
                .Property(e => e.si_site)
                .IsUnicode(false);

            modelBuilder.Entity<v_receiving_2>()
                .Property(e => e.si__do)
                .IsUnicode(false);

            modelBuilder.Entity<v_receiving_2>()
                .Property(e => e.si__cac)
                .IsUnicode(false);

            modelBuilder.Entity<v_receiving_2>()
                .Property(e => e.prh_rcp_date)
                .IsUnicode(false);

            modelBuilder.Entity<v_receiving_2>()
                .Property(e => e.Qty)
                .HasPrecision(23, 10);

            modelBuilder.Entity<v_receiving_2>()
                .Property(e => e.HKAmt)
                .HasPrecision(38, 11);

            modelBuilder.Entity<v_receiving_2>()
                .Property(e => e.prh_part)
                .IsUnicode(false);

            modelBuilder.Entity<v_receiving3>()
                .Property(e => e.prh_rcp_date)
                .IsUnicode(false);

            modelBuilder.Entity<v_receiving3>()
                .Property(e => e.prh_nbr)
                .IsUnicode(false);

            modelBuilder.Entity<v_receiving3>()
                .Property(e => e.si__do)
                .IsUnicode(false);

            modelBuilder.Entity<v_receiving3>()
                .Property(e => e.si__cac)
                .IsUnicode(false);

            modelBuilder.Entity<v_receiving3>()
                .Property(e => e.si_site)
                .IsUnicode(false);

            modelBuilder.Entity<v_receiving3>()
                .Property(e => e.pt_part_type)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<v_receiving3>()
                .Property(e => e.pt_conn)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<v_receiving3>()
                .Property(e => e.pt_netwt)
                .HasPrecision(18, 4);

            modelBuilder.Entity<v_receiving3>()
                .Property(e => e.pt_desc1)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<v_receiving3>()
                .Property(e => e.Qty)
                .HasPrecision(23, 10);

            modelBuilder.Entity<v_receiving3>()
                .Property(e => e.HKAmt)
                .HasPrecision(38, 11);

            modelBuilder.Entity<v_receiving3>()
                .Property(e => e.prh_part)
                .IsUnicode(false);

            modelBuilder.Entity<v_receiving3>()
                .Property(e => e.po_vend)
                .IsUnicode(false);

            modelBuilder.Entity<v_receiving3>()
                .Property(e => e.po_ord_date)
                .IsUnicode(false);

            modelBuilder.Entity<v_receiving3>()
                .Property(e => e.po_ship)
                .IsUnicode(false);

            modelBuilder.Entity<v_receiving3>()
                .Property(e => e.po_cr_terms)
                .IsUnicode(false);

            modelBuilder.Entity<v_receiving3>()
                .Property(e => e.po_buyer)
                .IsUnicode(false);

            modelBuilder.Entity<v_receiving3>()
                .Property(e => e.po_shipvia)
                .IsUnicode(false);

            modelBuilder.Entity<v_receiving3>()
                .Property(e => e.po_fob)
                .IsUnicode(false);

            modelBuilder.Entity<v_receiving3>()
                .Property(e => e.po_curr)
                .IsUnicode(false);

            modelBuilder.Entity<v_receiving3>()
                .Property(e => e.po_type)
                .IsUnicode(false);
        }
    }
}

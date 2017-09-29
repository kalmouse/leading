using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using CommenClass;
namespace LeadingClass
{
    public class Transfer
    {
        public int Id { get; set; }
        public int OutBranchId { get; set; }
        public int InBranchId { get; set; }
        public int OutStoreId { get; set; }
        public int OutStoreUserId { get; set; }
        public int InStoreId { get; set; }
        public int InStoreUserId { get; set; }
        public string Memo { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime OutStoreDate { get; set; }
        public DateTime InStoreDate { get; set; }
        public string Status { get; set; }
        public int IsInner { get; set; }
        private DBOperate m_dbo;

        public Transfer()
        {
            Id = 0;
            OutBranchId = 0;
            InBranchId = 0;
            OutStoreId = 0;
            OutStoreUserId = 0;
            InStoreId = 0;
            InStoreUserId = 0;
            Memo = "";
            UpdateDate = DateTime.Now;
            OutStoreDate = new DateTime(1900, 1, 1);
            InStoreDate = new DateTime(1900, 1, 1);
            Status = "";
            IsInner = 0;
            m_dbo = new DBOperate();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", Id));
            }
            arrayList.Add(new SqlParameter("@OutBranchId", OutBranchId));
            arrayList.Add(new SqlParameter("@InBranchId", InBranchId));
            arrayList.Add(new SqlParameter("@OutStoreId", OutStoreId));
            arrayList.Add(new SqlParameter("@OutStoreUserId", OutStoreUserId));
            arrayList.Add(new SqlParameter("@InStoreId", InStoreId));
            arrayList.Add(new SqlParameter("@InStoreUserId", InStoreUserId));
            arrayList.Add(new SqlParameter("@Memo", Memo));
            arrayList.Add(new SqlParameter("@UpdateDate", UpdateDate));
            arrayList.Add(new SqlParameter("@OutStoreDate", OutStoreDate));
            arrayList.Add(new SqlParameter("@InStoreDate", InStoreDate));
            arrayList.Add(new SqlParameter("@Status", Status));
            arrayList.Add(new SqlParameter("@IsInner", IsInner));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("Transfer", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("Transfer", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from Transfer where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                OutBranchId = DBTool.GetIntFromRow(row, "OutBranchId", 0);
                InBranchId = DBTool.GetIntFromRow(row, "InBranchId", 0);
                OutStoreId = DBTool.GetIntFromRow(row, "OutStoreId", 0);
                OutStoreUserId = DBTool.GetIntFromRow(row, "OutStoreUserId", 0);
                InStoreId = DBTool.GetIntFromRow(row, "InStoreId", 0);
                InStoreUserId = DBTool.GetIntFromRow(row, "InStoreUserId", 0);
                Memo = DBTool.GetStringFromRow(row, "Memo", "");
                UpdateDate = DBTool.GetDateTimeFromRow(row, "UpdateDate");
                OutStoreDate = DBTool.GetDateTimeFromRow(row, "OutStoreDate");
                InStoreDate = DBTool.GetDateTimeFromRow(row, "InStoreDate");
                Status = DBTool.GetStringFromRow(row, "Status", "");
                IsInner = DBTool.GetIntFromRow(row, "IsInner", 0);
                return true;
            }
            return false;
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from Transfer where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }
    }

    public class Transferdetail
    {
        public int Id { get; set; }
        public int TransferId { get; set; }
        public int GoodsId { get; set; }
        public string Model { get; set; }
        public string Unit { get; set; }
        public double AC { get; set; }
        public int Num { get; set; }
        public double Amount { get; set; }
        public string Memo { get; set; }     
        public DateTime UpdateDate { get; set; }
        private DBOperate m_dbo;

        public Transferdetail()
        {
            Id = 0;
            TransferId = 0;
            GoodsId = 0;
            Model = "";
            Unit = "";
            AC = 0;
            Num = 0;
            Amount = 0;
            Memo = "";
            UpdateDate = DateTime.Now;
            m_dbo = new DBOperate();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", Id));
            }
            arrayList.Add(new SqlParameter("@TransferId", TransferId));
            arrayList.Add(new SqlParameter("@GoodsId", GoodsId));
            arrayList.Add(new SqlParameter("@Model", Model));
            arrayList.Add(new SqlParameter("@Unit", Unit));
            arrayList.Add(new SqlParameter("@AC", AC));
            arrayList.Add(new SqlParameter("@Num", Num));
            arrayList.Add(new SqlParameter("@Amount", Amount));
            arrayList.Add(new SqlParameter("@Memo", Memo));
            arrayList.Add(new SqlParameter("@UpdateDate", UpdateDate));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("TransferDetail", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("TransferDetail", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from TransferDetail where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                TransferId = DBTool.GetIntFromRow(row, "TransferId", 0);
                GoodsId = DBTool.GetIntFromRow(row, "GoodsId", 0);
                Model = DBTool.GetStringFromRow(row, "Model", "");
                Unit = DBTool.GetStringFromRow(row, "Unit", "");
                AC = DBTool.GetDoubleFromRow(row, "AC", 0);
                Num = DBTool.GetIntFromRow(row, "Num", 0);
                Amount = DBTool.GetDoubleFromRow(row, "Amount", 0);
                Memo = DBTool.GetStringFromRow(row, "Memo", "");
                UpdateDate = DBTool.GetDateTimeFromRow(row, "UpdateDate");
                return true;
            }
            return false;
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from TransferDetail where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }

    }

    public class TransferOption
    {
        public int Id { get; set; }
        public int OutBranchId { get; set; }
        public int InBranchId { get; set; }
        public int OutStoreId { get; set; }
        public int OutStoreUserId { get; set; }
        public int InStoreId { get; set; }
        public int InStoreUserId { get; set; }
        public string Memo { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime OutStoreDateS { get; set; }
        public DateTime OutStoreDateE { get; set; }
        public DateTime InStoreDateS { get; set; }
        public DateTime InStoreDateE { get; set; }
        public string Status { get; set; }

        public TransferOption()
        {
            Id = 0;
            OutBranchId = 0;
            InBranchId = 0;
            OutStoreId = 0;
            OutStoreUserId = 0;
            InStoreId = 0;
            InStoreUserId = 0;
            Memo = "";
            UpdateDate = DateTime.Now;
            OutStoreDateS = new DateTime(1900, 1, 1);
            OutStoreDateE = new DateTime(1900, 1, 1);
            InStoreDateS = new DateTime(1900, 1, 1);
            InStoreDateE = new DateTime(1900, 1, 1);
            Status = "";
        }
    }
    public class TransferManager
    {
        private TransferOption m_option;
        public TransferOption option { get { return m_option; } set { m_option = value; } }
        private DBOperate m_dbo;
        public TransferManager()
        {
            m_dbo = new DBOperate();
        }
        public DataSet ReadTransferDetail(int TId)
        {
            string sql = string.Format("select * from View_TransferDetail where TransferId ={0}", TId);
            return m_dbo.GetDataSet(sql);
        }
        public int AddTransfer(Transfer Tf, Transferdetail[] tfd)
        {
            Transfer Transfer = new Transfer();
            Transfer.Id = Tf.Id;
            Transfer.InBranchId = Tf.InBranchId;
            Transfer.InStoreId = Tf.InStoreId;
            Transfer.InStoreUserId = Tf.InStoreUserId;
            Transfer.IsInner = Tf.IsInner;
            Transfer.Memo = Tf.Memo;
            Transfer.OutBranchId = Tf.OutBranchId;
            Transfer.OutStoreId = Tf.OutStoreId;
            Transfer.OutStoreUserId = Tf.OutStoreUserId;
            Transfer.Status = Tf.Status;
            int TransferId = Transfer.Save();
            if (TransferId > 0)
            {
                foreach (Transferdetail t in tfd)
                {
                    Transferdetail tfdetail = new Transferdetail();
                    tfdetail.TransferId = TransferId;
                    tfdetail.GoodsId = t.GoodsId;
                    tfdetail.Model = t.Model;
                    tfdetail.Unit = t.Unit;
                    tfdetail.AC = t.AC;
                    tfdetail.Num = t.Num;
                    tfdetail.Amount = t.Amount;
                    tfdetail.Memo = t.Memo;
                    tfdetail.UpdateDate = t.UpdateDate;
                    tfdetail.Save();
                }
            }
            return TransferId;
        }

        public bool ModifyTransfer(Transfer Tf, Transferdetail[] tfd)
        {
            Transfer Transfer = new Transfer();
            Transfer.Id = Tf.Id;
            Transfer.Load();
            Transfer.InBranchId = Tf.InBranchId;
            Transfer.InStoreId = Tf.InStoreId;
            Transfer.InStoreUserId = Tf.InStoreUserId;
            Transfer.IsInner = Tf.IsInner;
            Transfer.Memo = Tf.Memo;
            Transfer.OutStoreId = Tf.OutStoreId;
            Transfer.OutStoreUserId = Tf.OutStoreUserId;
            Transfer.Status = Tf.Status;
            Transfer.UpdateDate = DateTime.Now;
            int TransferId = Transfer.Save();
            if (TransferId > 0)
            {
                DataTable dtOld = this.ReadTransferDetail(TransferId).Tables[0];
                for (int i = 0; i < tfd.Length; i++)
                {
                    DataRow[] rows = dtOld.Select(string.Format(" GoodsId={0} ", tfd[i].GoodsId));
                    if (rows.Length == 1)//有这个商品
                    {
                        int oldNum = DBTool.GetIntFromRow(rows[0], "num", 0);
                        string oldMemo = DBTool.GetStringFromRow(rows[0], "Memo", "");
                        if (tfd[i].Num != oldNum || tfd[i].Memo != oldMemo)//有变化需要修改
                        {
                            Transferdetail od = new Transferdetail();
                            od.Id = DBTool.GetIntFromRow(rows[0], "Id", 0);
                            od.Load();
                            od.Num = tfd[i].Num;
                            od.Amount = tfd[i].Num * tfd[i].AC;
                            od.Memo = tfd[i].Memo.Replace(" ", "").Replace("　　", "");
                            od.Save();
                        }
                    }
                    else //新增商品
                    {
                        Transferdetail od = new Transferdetail();
                        od.GoodsId = tfd[i].GoodsId;
                        od.TransferId = TransferId;
                        od.Model = tfd[i].Model;
                        od.Unit = tfd[i].Unit;
                        od.AC = tfd[i].AC;
                        od.Num = tfd[i].Num;
                        od.Amount = tfd[i].Amount;
                        od.Memo = tfd[i].Memo;
                        od.UpdateDate = DateTime.Now;
                        od.Save();
                    }
                }

                //循环旧表，查找新表中没有的项。删除，记录明细
                foreach (DataRow row in dtOld.Select(""))
                {
                    int goodsId = DBTool.GetIntFromRow(row, "goodsId", 0);
                    int oldnum = DBTool.GetIntFromRow(row, "num", 0);
                    bool isExsist = false;
                    for (int i = 0; i < tfd.Length; i++)
                    {
                        if (tfd[i].GoodsId == goodsId)
                        {
                            isExsist = true;
                            break;
                        }
                    }
                    if (isExsist == false)
                    {
                        //新订单中无 此项
                        int odId = DBTool.GetIntFromRow(row, "Id", 0);
                        Transferdetail od = new Transferdetail();
                        od.Id = odId;
                        od.GoodsId = goodsId;
                        od.Delete();
                    }
                }
            }
            return true;
        }

        public DataSet ReadTransfer()
        {
            StringBuilder stb = new StringBuilder();
            stb.Append(" select * from View_Transfer where IsInner=1 ");
            if (option.Id > 0)
            {
                stb.Append(string.Format(" and Id={0} ", option.Id));
            }
            if (option.OutBranchId > 0)
            {
                stb.Append(string.Format(" and OutBranchId={0}  ", option.OutBranchId));
            }
            if (option.InBranchId > 0)
            {
                stb.Append(string.Format(" and InBranchId={0} ", option.InBranchId));
            }
            if (option.OutStoreId > 0)
            {
                stb.Append(string.Format(" and OutStoreId={0} ", option.OutStoreId));
            }
            if (option.InStoreId > 0)
            {
                stb.Append(string.Format(" and InStoreId={0} ", option.InStoreId));
            }
            if (option.Status != "")
            {
                string[] status = option.Status.Split('|');
                if (status.Length == 1)
                {
                    stb.Append(string.Format(" and Status in ('{0}')", option.Status));
                }
                else
                {
                    stb.Append(" and Status in ( ");
                    for (int i = 0; i < status.Length; i++)
                    {
                        if (i == 0)
                        {
                            stb.Append(string.Format("'{0}'", status[i]));
                        }
                        else
                            stb.Append(string.Format(",'{0}'", status[i]));
                    }
                    stb.Append(" )");
                }
            }

            if (option.OutStoreDateS > new DateTime(1900, 1, 1))
            {
                stb.Append(string.Format(" and  OutStoreDate> '{0}' ", option.OutStoreDateS.AddDays(1).ToShortDateString()));
            }
            if (option.OutStoreDateE > new DateTime(1900, 1, 1))
            {
                stb.Append(string.Format(" and  OutStoreDate< '{0}' ", option.OutStoreDateE.ToShortDateString()));
            }
            if (option.InStoreDateS > new DateTime(1900, 1, 1))
            {
                stb.Append(string.Format(" and  InStoreDate> '{0}' ", option.InStoreDateS.AddDays(1).ToShortDateString()));
            }
            if (option.InStoreDateE > new DateTime(1900, 1, 1))
            {
                stb.Append(string.Format(" and  InStoreDate< '{0}' ", option.InStoreDateE.ToShortDateString()));
            }
            stb.Append(" order by Id");
            return m_dbo.GetDataSet(stb.ToString());

        }

        /// <summary>
        /// 调拨出库
        /// </summary>
        /// <param name="TransferId"></param>
        /// <param name="storeId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public bool OutBoundT(int TransferId, int storeId, int UserId)
        {
            if (!IsTransferCanOutBound(TransferId, storeId))
            {
                return false;
            }
          //  修改订单状态

            string sql = string.Format(" update Transfer set Status='调拨出库',OutStoreDate='{0}' where Id={1} ", DateTime.Now.ToString(), TransferId);
            if (m_dbo.ExecuteNonQuery(sql))
            {
                GoodsStoreManager gsm = new GoodsStoreManager();
                DataSet ds = ReadTransferDetail(TransferId);
                //读取商品明细
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    //循环单个商品
                    int goodsId = DBTool.GetIntFromRow(row, "GoodsId", 0);
                    int num = DBTool.GetIntFromRow(row, "Num", 0);
                    int Id = DBTool.GetIntFromRow(row, "Id", 0);
                    GoodsStoreDetail gsd = new GoodsStoreDetail();
                    gsd.GoodsId = goodsId;
                    gsd.Num = num;
                    gsd.Operate = CommenClass.TransferStatus.调拨出库.ToString();
                    gsd.RelationId = TransferId;
                    gsd.StoreId = storeId;
                    gsd.UserId = UserId;                   
                    double AC = gsm.OutBoundGoods(gsd);
                    if (AC > 0)
                    {
                        Transferdetail tfd = new Transferdetail();
                        tfd.Id = Id;
                        tfd.Load();
                        tfd.AC = AC;
                        tfd.Save();
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 调拨入库，撤销入库
        /// </summary>
        /// <param name="TransferId"></param>
        /// <param name="storeId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public  bool InBoundT(int TransferId,int storeId, int UserId)
        {
            Transfer t=new Transfer();
            t.Id=TransferId;
            t.Load();
            bool storevalid = false;
            if (t.InStoreId == storeId)
            {
                t.InStoreDate = DateTime.Now;
                t.Status = CommenClass.TransferStatus.调拨入库.ToString();
                t.InStoreUserId = UserId;
                t.Save();
                storevalid = true;
            }
            else if (t.OutStoreId == storeId)
            {
                t.UpdateDate = DateTime.Now;
                t.Status = CommenClass.TransferStatus.待出库.ToString();
                t.OutStoreDate = new DateTime(1900, 1, 1);
                t.Save();
                storevalid = true;
            }
            else
            {
                return false;
            }
            if (storevalid)
            {
                DataSet ds = ReadTransferDetail(TransferId);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    int goodsId = DBTool.GetIntFromRow(row, "GoodsId", 0);
                    int num = DBTool.GetIntFromRow(row, "Num", 0);
                    int Id = DBTool.GetIntFromRow(row, "Id", 0);
                    double AC = DBTool.GetDoubleFromRow(row, "AC", 0);
                    GoodsStore gs = new GoodsStore();
                    gs.GoodsId = goodsId;
                    gs.StoreId = storeId;
                    gs.Load(storeId, goodsId);
                    //修改库存 成本和数量,如果是固定成本的 直接赋值
                    GoodsAC ga = new GoodsAC();
                    if (ga.Load(t.InBranchId, goodsId))
                    {
                        gs.AC = ga.Goods_AC;
                    }
                    else
                    {
                        if ((gs.Num + num) != 0)
                        {
                            gs.AC = (gs.AC * gs.Num + num * AC) / (gs.Num + num);
                        }
                        else
                        {
                            gs.AC = AC;
                        }
                    }
                    gs.Num = gs.Num + num;
                    gs.UpdateTime = DateTime.Now;
                    gs.Save();
                    GoodsStoreDetail gsd = new GoodsStoreDetail();
                    gsd.GoodsId = goodsId;
                    gsd.Id = 0;
                    gsd.NewNum = gs.Num;
                    gsd.Num = num;
                    gsd.OldNum = gs.Num - num;
                    gsd.Operate = t.Status;
                    gsd.RelationId = TransferId;
                    gsd.StoreId = storeId;
                    gsd.UpdateTime = DateTime.Now;
                    gsd.UserId = UserId;
                    gsd.Save();
                }
            }
            return true;
        }

        private bool IsTransferCanOutBound(int TransferId, int StoreId)
        {
            GoodsStoreManager gsm = new GoodsStoreManager();
            //读取IsCalc=1 的商品汇总明细
            string sql = string.Format(@"select GoodsId,SUM(Num) as Num,(select SUM(Num)from view_GoodsStore where GoodsId=a.GoodsId and StoreId={0} group by GoodsId)  as  StoreNum from View_TransferDetail a where TransferId ={1} group by GoodsId  ", StoreId, TransferId);

            DataSet ds = m_dbo.GetDataSet(sql);
            //读取商品明细
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                //循环单个商品
                int num = DBTool.GetIntFromRow(row, "Num", 0);
                int storeNum = DBTool.GetIntFromRow(row, "StoreNum", 0);
                if (storeNum < num)
                {
                    return false;
                }
            }
            return true;
        }
    }
}

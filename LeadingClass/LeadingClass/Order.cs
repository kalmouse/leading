using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace LeadingClass
{
    public class Order
    {
        public int Id { get; set; }
        public int ComId { get; set; }
        public int MemberId { get; set; }
        public DateTime OrderTime { get; set; }
        public DateTime PlanDate { get; set; }
        public DateTime FinishDate { get; set; }
        public double SumMoney { get; set; }
        public double GrossProfit { get; set; }
        public double Point { get; set; }
        public string RealName { get; set; }
        public string Mobile { get; set; }
        public string Telphone { get; set; }
        public string Email { get; set; }
        public string Company { get; set; }
        public string Address { get; set; }
        public string DeptName { get; set; }
        public string Invoice { get; set; }
        public string Invoice_Name { get; set; }
        public string Invoice_Type { get; set; }
        public string Invoice_Content { get; set; }
        public string Pay_method { get; set; }
        public string Memo { get; set; }
        public int UserId { get; set; }
        public int SaveNum { get; set; }
        public int PrintNum { get; set; }
        public DateTime PrintDateTime { get; set; }
        public string OrderType { get; set; }
        public string PayStatus { get; set; }
        public int IsInner { get; set; }
        public int BranchId { get; set; }
        public int RowNum { get; set; }
        public int IsCopied { get; set; }
        public int RawOrderId { get; set; }
        public DateTime UpdateTime { get; set; }
        public int Emergency { get; set; }
        public double ChargeOff { get; set; }
        public double PaidMoney { get; set; }
        public int TPI_OrderId { get; set; }//根据接口写进订单 add by luochunhui 07-16
        public int SalesId { get; set; }//业务员Id
        public int SecrecyId { get; set; }
        public int DeptId { get; set; }//客户订单当时所在的部门Id
        public int ApplyId { get; set; }//申请单转成订单的，记录当时申请单的Id
        public string TPI_Name { get; set; }
        public string GUID { get; set; }
        public int IsStorage { get; set; }
        public int IsDelete { get; set; }//是否删除订单 add by Luochunhui
        public double Tax { get; set; }
        public double TaxRate { get; set; }
        public double TaxGrossProfit { get; set; }
        private DBOperate m_dbo;

        public Order()
        {
            Id = 0;
            ComId = 0;
            MemberId = 0;
            OrderTime = DateTime.Now;
            PlanDate = DateTime.Now.AddDays(1).Date;
            FinishDate = new DateTime(1900, 1, 1);
            SumMoney = 0;
            GrossProfit = 0;
            Point = 0;
            RealName = "";
            Mobile = "";
            Telphone = "";
            Email = "";
            Company = "";
            Address = "";
            DeptName = "";
            Invoice = CommenClass.InvoiceStatus.未开票.ToString();
            Invoice_Name = "";
            Invoice_Type = "";
            Invoice_Content = "";
            Pay_method = "";
            Memo = "";
            UserId = 0;
            SaveNum = 0;
            PrintNum = 0;
            PrintDateTime = DateTime.Now;
            OrderType = "";
            PayStatus = CommenClass.PayStatus.未付款.ToString();
            IsInner = 0;
            BranchId = 0;
            RowNum = 0;
            IsCopied = 0;
            RawOrderId = 0;
            UpdateTime = DateTime.Now;
            Emergency = 0;
            ChargeOff = 0;
            PaidMoney = 0;
            TPI_OrderId = 0;
            SalesId = 0;
            SecrecyId = 0;
            DeptId = 0;
            ApplyId = 0;
            TPI_Name = "";
            GUID = "";
            IsStorage = 0;
            IsDelete = 0;
            Tax = 0;
            TaxRate = 0;
            TaxGrossProfit = 0;
            m_dbo = new DBOperate();
        }
        public Order(int Id)
            : this()
        {
            this.Id = Id;
            this.Load();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", Id));
            }
            arrayList.Add(new SqlParameter("@ComId", ComId));
            arrayList.Add(new SqlParameter("@MemberId", MemberId));
            arrayList.Add(new SqlParameter("@OrderTime", OrderTime));
            arrayList.Add(new SqlParameter("@PlanDate", PlanDate));
            arrayList.Add(new SqlParameter("@FinishDate", FinishDate));
            arrayList.Add(new SqlParameter("@SumMoney", SumMoney));
            arrayList.Add(new SqlParameter("@GrossProfit", GrossProfit));
            arrayList.Add(new SqlParameter("@Point", Point));
            arrayList.Add(new SqlParameter("@RealName", RealName));
            arrayList.Add(new SqlParameter("@Mobile", Mobile));
            arrayList.Add(new SqlParameter("@Telphone", Telphone));
            arrayList.Add(new SqlParameter("@Email", Email));
            arrayList.Add(new SqlParameter("@Company", Company));
            arrayList.Add(new SqlParameter("@Address", Address));
            arrayList.Add(new SqlParameter("@DeptName", DeptName));
            arrayList.Add(new SqlParameter("@Invoice", Invoice));
            arrayList.Add(new SqlParameter("@Invoice_Name", Invoice_Name));
            arrayList.Add(new SqlParameter("@Invoice_Type", Invoice_Type));
            arrayList.Add(new SqlParameter("@Invoice_Content", Invoice_Content));
            arrayList.Add(new SqlParameter("@Pay_method", Pay_method));
            arrayList.Add(new SqlParameter("@Memo", Memo));
            arrayList.Add(new SqlParameter("@UserId", UserId));
            arrayList.Add(new SqlParameter("@SaveNum", SaveNum));
            arrayList.Add(new SqlParameter("@PrintNum", PrintNum));
            arrayList.Add(new SqlParameter("@PrintDateTime", PrintDateTime));
            arrayList.Add(new SqlParameter("@OrderType", OrderType));
            arrayList.Add(new SqlParameter("@PayStatus", PayStatus));
            arrayList.Add(new SqlParameter("@IsInner", IsInner));
            arrayList.Add(new SqlParameter("@BranchId", BranchId));
            arrayList.Add(new SqlParameter("@RowNum", RowNum));
            arrayList.Add(new SqlParameter("@IsCopied", IsCopied));
            arrayList.Add(new SqlParameter("@RawOrderId", RawOrderId));
            arrayList.Add(new SqlParameter("@UpdateTime", DateTime.Now));
            arrayList.Add(new SqlParameter("@Emergency", Emergency));
            arrayList.Add(new SqlParameter("@ChargeOff", ChargeOff));
            arrayList.Add(new SqlParameter("@PaidMoney", PaidMoney));
            arrayList.Add(new SqlParameter("@TPI_OrderId", TPI_OrderId));
            arrayList.Add(new SqlParameter("@SalesId", SalesId));
            arrayList.Add(new SqlParameter("@SecrecyId", SecrecyId));
            arrayList.Add(new SqlParameter("@DeptId", DeptId));
            arrayList.Add(new SqlParameter("@ApplyId", ApplyId));
            arrayList.Add(new SqlParameter("@TPI_Name", TPI_Name));
            arrayList.Add(new SqlParameter("@GUID", GUID));
            arrayList.Add(new SqlParameter("@IsStorage", IsStorage));
            arrayList.Add(new SqlParameter("@IsDelete", IsDelete));
            arrayList.Add(new SqlParameter("@Tax", Tax));
            arrayList.Add(new SqlParameter("@TaxRate", TaxRate));
            arrayList.Add(new SqlParameter("@TaxGrossProfit", TaxGrossProfit));
            if (this.Id > 0)
            { 
                m_dbo.UpdateData("[Order]", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                if (m_dbo.InsertData("[Order]", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter))) > 0)
                {
                    this.Id = GetIdByGUID(this.GUID);
                }
                //当comId>0检查 customer：AddTime（首单时间）是否是1900-1-1，是：改为当前时间
                if (this.ComId > 0)
                {
                    Customer customer = new Customer();
                    customer.Load(this.ComId);
                    if (customer.AddTime == new DateTime(1900, 1, 1))
                    {
                        customer.AddTime = DateTime.Now;
                        customer.Save();
                    }
                }
                //自动添加orderstatus
                if (this.Id > 0)
                {
                    OrderStatus os = new OrderStatus();
                    os.OrderId = this.Id;
                    //如果是手工单，客服状态默认是 已完成
                    if (OrderType == CommenClass.OrderType.销售开单.ToString() || OrderType == CommenClass.OrderType.销售退单.ToString())
                    {
                        os.ServiceId = UserId;
                        os.ServiceStatus = CommenClass.Order_Status.已完成.ToString();
                        os.ServiceFinishTime = DateTime.Now;
                    }
                    os.Save();
                }
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from [Order] where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                ComId = DBTool.GetIntFromRow(row, "ComId", 0);
                MemberId = DBTool.GetIntFromRow(row, "MemberId", 0);
                OrderTime = DBTool.GetDateTimeFromRow(row, "OrderTime");
                PlanDate = DBTool.GetDateTimeFromRow(row, "PlanDate");
                FinishDate = DBTool.GetDateTimeFromRow(row, "FinishDate");
                SumMoney = DBTool.GetDoubleFromRow(row, "SumMoney", 0);
                GrossProfit = DBTool.GetDoubleFromRow(row, "GrossProfit", 0);
                Point = DBTool.GetDoubleFromRow(row, "Point", 0);
                RealName = DBTool.GetStringFromRow(row, "RealName", "");
                Mobile = DBTool.GetStringFromRow(row, "Mobile", "");
                Telphone = DBTool.GetStringFromRow(row, "Telphone", "");
                Email = DBTool.GetStringFromRow(row, "Email", "");
                Company = DBTool.GetStringFromRow(row, "Company", "");
                Address = DBTool.GetStringFromRow(row, "Address", "");
                DeptName = DBTool.GetStringFromRow(row, "DeptName", "");
                Invoice = DBTool.GetStringFromRow(row, "Invoice", "");
                Invoice_Name = DBTool.GetStringFromRow(row, "Invoice_Name", "");
                Invoice_Type = DBTool.GetStringFromRow(row, "Invoice_Type", "");
                Invoice_Content = DBTool.GetStringFromRow(row, "Invoice_Content", "");
                Pay_method = DBTool.GetStringFromRow(row, "Pay_method", "");
                Memo = DBTool.GetStringFromRow(row, "Memo", "");
                UserId = DBTool.GetIntFromRow(row, "UserId", 0);
                SaveNum = DBTool.GetIntFromRow(row, "SaveNum", 0);
                PrintNum = DBTool.GetIntFromRow(row, "PrintNum", 0);
                PrintDateTime = DBTool.GetDateTimeFromRow(row, "PrintDateTime");
                OrderType = DBTool.GetStringFromRow(row, "OrderType", "");
                PayStatus = DBTool.GetStringFromRow(row, "PayStatus", "");
                IsInner = DBTool.GetIntFromRow(row, "IsInner", 0);
                BranchId = DBTool.GetIntFromRow(row, "BranchId", 0);
                RowNum = DBTool.GetIntFromRow(row, "RowNum", 0);
                IsCopied = DBTool.GetIntFromRow(row, "IsCopied", 0);
                RawOrderId = DBTool.GetIntFromRow(row, "RawOrderId", 0);
                UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                Emergency = DBTool.GetIntFromRow(row, "Emergency", 0);
                ChargeOff = DBTool.GetDoubleFromRow(row, "ChargeOff", 0);
                PaidMoney = DBTool.GetDoubleFromRow(row, "PaidMoney", 0);
                TPI_OrderId = DBTool.GetIntFromRow(row, "TPI_OrderId", 0);
                SalesId = DBTool.GetIntFromRow(row, "SalesId", 0);
                SecrecyId = DBTool.GetIntFromRow(row, "SecrecyId", 0);
                DeptId = DBTool.GetIntFromRow(row, "DeptId", 0);
                ApplyId = DBTool.GetIntFromRow(row, "ApplyId", 0);
                TPI_Name = DBTool.GetStringFromRow(row, "TPI_Name","");
                GUID = DBTool.GetStringFromRow(row, "GUID", "");
                IsStorage = DBTool.GetIntFromRow(row, "IsStorage", 0);
                IsDelete = DBTool.GetIntFromRow(row, "IsDelete", 0);
                Tax = DBTool.GetDoubleFromRow(row, "Tax", 0);
                TaxRate = DBTool.GetDoubleFromRow(row, "TaxRate", 0);
                TaxGrossProfit = DBTool.GetDoubleFromRow(row, "TaxGrossProfit", 0); 
                return true;
            }
            return false;
        }
        public bool Load(int TPI_OrderId)
        {
            string sql = string.Format("select * from [Order] where TPI_OrderId={0}", TPI_OrderId);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                ComId = DBTool.GetIntFromRow(row, "ComId", 0);
                MemberId = DBTool.GetIntFromRow(row, "MemberId", 0);
                OrderTime = DBTool.GetDateTimeFromRow(row, "OrderTime");
                PlanDate = DBTool.GetDateTimeFromRow(row, "PlanDate");
                FinishDate = DBTool.GetDateTimeFromRow(row, "FinishDate");
                SumMoney = DBTool.GetDoubleFromRow(row, "SumMoney", 0);
                GrossProfit = DBTool.GetDoubleFromRow(row, "GrossProfit", 0);
                Point = DBTool.GetDoubleFromRow(row, "Point", 0);
                RealName = DBTool.GetStringFromRow(row, "RealName", "");
                Mobile = DBTool.GetStringFromRow(row, "Mobile", "");
                Telphone = DBTool.GetStringFromRow(row, "Telphone", "");
                Email = DBTool.GetStringFromRow(row, "Email", "");
                Company = DBTool.GetStringFromRow(row, "Company", "");
                Address = DBTool.GetStringFromRow(row, "Address", "");
                DeptName = DBTool.GetStringFromRow(row, "DeptName", "");
                Invoice = DBTool.GetStringFromRow(row, "Invoice", "");
                Invoice_Name = DBTool.GetStringFromRow(row, "Invoice_Name", "");
                Invoice_Type = DBTool.GetStringFromRow(row, "Invoice_Type", "");
                Invoice_Content = DBTool.GetStringFromRow(row, "Invoice_Content", "");
                Pay_method = DBTool.GetStringFromRow(row, "Pay_method", "");
                Memo = DBTool.GetStringFromRow(row, "Memo", "");
                UserId = DBTool.GetIntFromRow(row, "UserId", 0);
                SaveNum = DBTool.GetIntFromRow(row, "SaveNum", 0);
                PrintNum = DBTool.GetIntFromRow(row, "PrintNum", 0);
                PrintDateTime = DBTool.GetDateTimeFromRow(row, "PrintDateTime");
                OrderType = DBTool.GetStringFromRow(row, "OrderType", "");
                PayStatus = DBTool.GetStringFromRow(row, "PayStatus", "");
                IsInner = DBTool.GetIntFromRow(row, "IsInner", 0);
                BranchId = DBTool.GetIntFromRow(row, "BranchId", 0);
                RowNum = DBTool.GetIntFromRow(row, "RowNum", 0);
                IsCopied = DBTool.GetIntFromRow(row, "IsCopied", 0);
                RawOrderId = DBTool.GetIntFromRow(row, "RawOrderId", 0);
                UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                Emergency = DBTool.GetIntFromRow(row, "Emergency", 0);
                ChargeOff = DBTool.GetDoubleFromRow(row, "ChargeOff", 0);
                PaidMoney = DBTool.GetDoubleFromRow(row, "PaidMoney", 0);
                TPI_OrderId = DBTool.GetIntFromRow(row, "TPI_OrderId", 0);
                SalesId = DBTool.GetIntFromRow(row, "SalesId", 0);
                SecrecyId = DBTool.GetIntFromRow(row, "SecrecyId", 0);
                DeptId = DBTool.GetIntFromRow(row, "DeptId", 0);
                ApplyId = DBTool.GetIntFromRow(row, "ApplyId", 0);
                TPI_Name = DBTool.GetStringFromRow(row, "TPI_Name", "");
                GUID = DBTool.GetStringFromRow(row, "GUID", "");
                IsStorage = DBTool.GetIntFromRow(row, "IsStorage", 0);
                IsDelete = DBTool.GetIntFromRow(row, "IsDelete", 0);
                Tax = DBTool.GetDoubleFromRow(row, "Tax", 0);
                TaxRate = DBTool.GetDoubleFromRow(row, "TaxRate", 0);
                TaxGrossProfit = DBTool.GetDoubleFromRow(row, "TaxGrossProfit", 0);
                return true;
            }
            return false;
        }

        public int GetIdByGUID(string guid)
        {
            string sql = string.Format(" select Id from [Order] where GUID='{0}' ", guid);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count == 1)
            {
                return DBTool.GetIntFromRow(ds.Tables[0].Rows[0], "Id", 0);
            }
            else return 0;
        }
        public bool Delete()
        {
            //记录订单删除记录，然后删除订单
            string sql = string.Format("update [order] set IsDelete=1,UserId={1} where Id={0};", this.Id, this.UserId);
            return m_dbo.ExecuteNonQuery(sql);
        }

        ///// <summary>
        ///// 更改订单修改时间
        ///// </summary>
        ///// <param name="updatetime"></param>
        ///// <returns></returns>
        //public bool ModifyUpdateTime(string updatetime)
        //{
        //    string sql = string.Format(" update [order] set updateTime = '{0}' where Id={1} ", updatetime,this.Id);
        //    return m_dbo.ExecuteNonQuery(sql);
        //}

        /// <summary>
        /// 批量设置打印数量
        /// </summary>
        /// <param name="orderIds"></param>
        /// <returns></returns>
        public bool SetOrderPrin(int[] orderIds, int PrintNum)
        {
            string sql = string.Format("update [Order] set PrintNum={0} where  ", PrintNum);
            if (orderIds.Length == 1)
            {
                sql += string.Format(" Id ={0} ", orderIds[0]);
            }
            else
            {
                string ids = "  Id in ( ";
                for (int i = 0; i < orderIds.Length; i++)
                {
                    if (i == 0)
                    {
                        ids += orderIds[i].ToString();
                    }
                    else ids += "," + orderIds[i].ToString();
                }
                ids += " ) ";
                sql += ids;
            }
            return m_dbo.ExecuteNonQuery(sql);


        }

        /// <summary>
        /// 重新计算订单毛利 //该方法位置有问题
        /// </summary>
        /// <returns></returns>
        public bool CalcGrossProfit()
        {
            string sql = "";
            this.Load();
            OrderManager om = new OrderManager();
            DataSet details = om.ReadOrderDetail(this.Id);
            foreach (DataRow row in details.Tables[0].Rows)
            {
                //循环订单明细
                int goodsId = DBTool.GetIntFromRow(row, "GoodsId", 0);
                sql = string.Format("select AC, TaxAC from View_GoodsStore where BranchId={0} and GoodsId={1} and IsDefault=1  order by IsAvalible desc ", this.BranchId, goodsId);

                //更新商品成本
                DataSet ds = m_dbo.GetDataSet(sql);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    double AC = DBTool.GetDoubleFromRow(ds.Tables[0].Rows[0], "AC", 0);
                    double TaxAC = DBTool.GetDoubleFromRow(ds.Tables[0].Rows[0], "TaxAC", 0);
                    sql = string.Format(@"update OrderDetail set AC={0} ,TaxAC={3} where OrderId={1} and GoodsId={2} ", AC, this.Id, goodsId,TaxAC);
                    m_dbo.ExecuteNonQuery(sql);
                }
            }

            //更新订单毛利
            sql = string.Format(@"update [Order] set GrossProfit = SumMoney/(1+TaxRate)-(select isnull(SUM(AC*Num),0) from OrderDetail where OrderId ={0}),
                                                     TaxGrossProfit = SumMoney - (select isnull(SUM(TaxAC*Num),0) from OrderDetail where OrderId ={0}) where Id ={0}", this.Id);
            return m_dbo.ExecuteNonQuery(sql);
        }
        ///// <summary>
        ///// 判断是否被确认成订单
        ///// </summary>
        ///// <param name="OrderId"></param>
        ///// <returns></returns>
        //public int CheckIsOrder()
        //{
        //    string sql = string.Format("select * from [Order] where Id={0}", Id);
        //    DataSet ds = m_dbo.GetDataSet(sql);
        //    if (ds.Tables[0].Rows.Count > 0)
        //    {
        //        return 1;
        //    }
        //    return 0;
        //}
        /// <summary>
        /// 保密单位用/以后部署项目会用到该方法
        /// </summary>
        /// <param name="secrecyId"></param>
        /// <returns></returns>
        public bool SecrecyLoad(int secrecyId)
        {
            string sql = string.Format("select * from [Order] where SecrecyId={0}", secrecyId);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                SecrecyId = DBTool.GetIntFromRow(row, "SecrecyId", 0);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 更新订单支付状态  待查看和平台一致的方法
        /// </summary>
        /// <returns></returns>
        public bool UpdateOrderPayStatus(int orderId)
        {
            string sql = string.Format("update [order] set PayStatus='已付款' where Id={0}",orderId);
            return m_dbo.ExecuteNonQuery(sql);
        }
        /// <summary>
        /// 修改订单支付方式
        /// </summary>
        /// <param name="OrderId">订单号</param>
        /// <param name="Paymethod">支付方式</param>
        /// <returns></returns>
        public bool UpdateOrderPaymethod(int OrderId, string Paymethod)
        {
            string sql = string.Format("update [order] set pay_method='{0}' where Id={1}", Paymethod, OrderId);
            return m_dbo.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 订单的总价///修改订单总价 对应的毛利也得修改
        /// </summary>
        public bool UpdateOrderSumMoney(int OrderId)
        {
            string sql = string.Format(@"update [Order] set SumMoney=(select isnull(sum(Amount),0)  from OrderDetail  where OrderId= {0} and IsShow=1 ),
                                                            RowNum=isnull((select COUNT(OrderId) from OrderDetail  where OrderId= {0}),0),
                                                            Tax=(select isnull(sum(Amount),0)  from OrderDetail  where OrderId= {0} and IsShow=1 )/(1+TaxRate)*TaxRate  where Id={0}", OrderId);
            return m_dbo.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 生成包装箱，如果已经有这个ID了，就增加打印次数  放在orderBox类库更合适
        /// </summary>
        /// <param name="BoxNum"></param>
        /// <returns></returns>
        public void CreateBox(int BoxNum)
        {
            DeleteBox(BoxNum);//删除多余的包装

            for (int i = 1; i < BoxNum + 1; i++)
            {
                OrderBox ob = new OrderBox();
                if (ob.Load(this.Id, i))
                {
                    ob.AddPrintNum();
                }
                else
                {
                    ob.BoxId = i;
                    ob.OrderId = this.Id;
                    ob.PrintNum = 1;
                    ob.Status = CommenClass.OrderBoxStatus.已打印.ToString();
                    ob.StoreZone = CommenClass.SpecialStoreZone.发货区.ToString();
                    ob.Save();
                }
            }
        }
        /// <summary>
        /// 删除多余的包裹
        /// </summary>
        /// <returns></returns>
        public bool DeleteBox(int maxBoxId)
        {
            string sql = string.Format(" delete from orderbox where orderid={0} and BoxId >{1} ", this.Id, maxBoxId);
            return m_dbo.ExecuteNonQuery(sql);
        }
//        /// <summary>
//        /// 读取扬州订单
//        /// </summary>
//        /// <returns></returns>
//        public DataSet Readorder( int comId) 
//        {
//            string sql = string.Format(@"select  [Order].*,OrderDetail.*,Goods.TypeId,Brand.Name as BrandName ,Member.Province,Member.City,Member.Area ,Customer .Invoice_Name from   [Order] inner  join OrderDetail on [order].Id = OrderDetail.OrderId 	
//left join Member on [order].MemberId= Member.Id 
//left  join Customer on [Order] .ComId= Customer.ComId
//	left join Goods on OrderDetail.GoodsId = Goods.ID
//	left  join Brand on Goods.BrandId= Brand.Id  where  OrderType='网上订单' and IsDelete =0 and RawOrderId = 0 and [Order].ComId={0}", comId);
//            return m_dbo.GetDataSet(sql);
//        }
        /// <summary>
        /// 拆单
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public DataSet ReadSplitOrder(int orderId)
        {
            string sql = string.Format(@"select * from [Order] 
inner  join OrderStatus on [order].Id =OrderStatus.OrderId where 1=1 and [Order].IsDelete <>1 and  ([Order].Id = {0} or Memo like '%{0}拆单%' );select * from OrderDetail where OrderId={0} ", orderId);

            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 根据tpiorderId查询orderId
        /// </summary>
        /// <returns></returns>
        public int ReadorderByTPIOrderId(int tpiorderId)
        {
            string sql = string.Format("select * from  [Order]  where  TPI_OrderId='{0}'", tpiorderId);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DBTool.GetIntFromRow(ds.Tables[0].Rows[0], "IsDelete", 0);
            }
            else return -1;
        }
        /// <summary>
        /// 读取多条订单
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public DataSet ReadOrder(List<int> orderId) 
        {
            string sql = string.Format("select [Order].Id as orderId,GoodsId,SkuName from [Order]  inner join OrderDetail on [order].Id=OrderDetail.OrderId  where  1=1 ");
            if (orderId != null)
            {
                sql += "and  [Order].Id in (";
                for (int i = 0; i < orderId.Count; i++)
                {
                    if (i == 0)
                    {
                        sql += string.Format(" '{0}' ", orderId[0]);
                    }
                    else sql += string.Format(" ,'{0}' ", orderId[i]);
                }
                sql += " ) ";
            }
            return m_dbo.GetDataSet(sql);
        }
    }
}

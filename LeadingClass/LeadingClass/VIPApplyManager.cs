using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;

namespace LeadingClass
{
    public class VIPApplyOption
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public int ComId { get; set; }
        public int DeptId { get; set; }
        public int MemberId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int IsPass { get; set; }//是否越级，查看该部门下的所有级别的申请单 add by quxiaoshan 2015-5-6
        public string Code { get; set; }//部门对应的申请单 add by quxiaoshan 2015-5-7
        public int IsAdmin { get; set; }//是否是管理员，查看申请单的sql quxiaoshan 2015-5-13
        public int RoleId { get; set; }//判断角色 add by luochunhui 2015-5-25
        public string Mobile { get; set; }//找对应接收短信人的手机号 add by luochunhui 2015-5-25
        public int ConfirmLevel { get; set; }
        public int IsBudget { get; set; }

        public VIPApplyOption()
        {
            Id = 0;
            Status = "";
            ComId = 0;
            DeptId = 0;
            MemberId = 0;
            StartDate = new DateTime(1900, 1, 1);
            EndDate = new DateTime(1900, 1, 1);
            Code = "";
            IsAdmin = 0;
            ConfirmLevel = 0;
            RoleId = 4;
            Mobile = "";
            IsBudget = 0;
        }
    }
    public class VIPApplyManager
    {
        private DBOperate m_dbo;
        public VIPApplyManager()
        {
            m_dbo = new DBOperate();
        }


        #region SaveVIPApply
        /// <summary>
        /// 保存VIPApply 的通用方法
        /// </summary>
        /// <param name="apply"></param>
        /// <param name="details"></param>
        /// <returns></returns>
        public int SaveVIPApply(VIPApply apply, List<VIPApplyDetail> details, int OperaterId)
        {
            if (apply.Id == 0)
            {
                return saveVIPApply(apply, details);
            }
            else
            {
                return modifyVIPApply(apply, details, OperaterId);
            }
        }

        private int saveVIPApply(VIPApply apply, List<VIPApplyDetail> details)
        {
            int Id = apply.Save();
            if (Id > 0)
            {
                foreach (VIPApplyDetail detail in details)
                {
                    detail.ApplyId = Id;
                    detail.Save();
                }
            }
            return Id;
        }

        private int modifyVIPApply(VIPApply apply, List<VIPApplyDetail> details, int OperaterId)
        {
            if (apply.Save() > 0)
            {
                //记录修改明细
                //循环新表处理 修改数量的商品 和 新增商品
                VIPApplyDetail vad = new VIPApplyDetail();
                vad.ApplyId = apply.Id;
                DataTable dtOld = vad.GetApplyDetailByApplyId().Tables[0];
                for (int i = 0; i < details.Count; i++)
                {
                    DataRow[] rows = dtOld.Select(string.Format(" GoodsId={0} ", details[i].GoodsId));
                    if (rows.Length == 1)//有这个商品
                    {
                        int oldNum = DBTool.GetIntFromRow(rows[0], "Num", 0);
                        if (details[i].Num != oldNum)//数量改变需要修改
                        {
                            vad.Id = DBTool.GetIntFromRow(rows[0], "Id", 0);
                            vad.Load();
                            vad.Num = details[i].Num;
                            if (vad.Save() > 0)
                            {
                                //按专柜中没有组合商品处理
                                //修改明细记录
                                VIPApplyModify vam = new VIPApplyModify();
                                vam.ApplyId = apply.Id;
                                vam.GoodsId = vad.GoodsId;
                                vam.OldNum = oldNum;
                                vam.NewNum = vad.Num;
                                vam.OperaterId = OperaterId;//记录操作人
                                vam.UpdateTime = DateTime.Now;
                                vam.Save();
                            }
                        }
                    }
                    else //新增商品
                    {
                        VIPApplyDetail svad = new VIPApplyDetail();
                        svad.ApplyId = apply.Id;
                        svad.GoodsId = details[i].GoodsId;
                        svad.Num = details[i].Num;
                        svad.VIPPrice = details[i].VIPPrice;
                        if (svad.Save() > 0)
                        {
                            //修改明细记录
                            VIPApplyModify vam = new VIPApplyModify();
                            vam.ApplyId = apply.Id;
                            vam.GoodsId = svad.GoodsId;
                            vam.OldNum = 0;
                            vam.NewNum = svad.Num;
                            vam.OperaterId = OperaterId;//记录操作人
                            vam.UpdateTime = DateTime.Now;
                            vam.Save();
                        }
                    }
                }
                //循环旧表找到删除的商品 记录明细
                foreach (DataRow row in dtOld.Rows)
                {
                    int goodsId = DBTool.GetIntFromRow(row, "goodsId", 0);
                    int oldnum = DBTool.GetIntFromRow(row, "num", 0);
                    bool isExsist = false;
                    for (int i = 0; i < details.Count; i++)
                    {
                        if (details[i].GoodsId == goodsId)
                        {
                            isExsist = true;
                            break;
                        }
                    }
                    if (isExsist == false)
                    {
                        VIPApplyDetail dvad = new VIPApplyDetail();
                        //新申请单中无 此项
                        dvad.Id = DBTool.GetIntFromRow(row, "Id", 0);
                        if (dvad.Delete())//删除申请单明细中的某一商品记录
                        {
                            //记录申请单删除某一商品
                            VIPApplyModify vam = new VIPApplyModify();
                            vam.ApplyId = apply.Id;
                            vam.GoodsId = goodsId;
                            vam.OldNum = oldnum;
                            vam.NewNum = 0;
                            vam.OperaterId = OperaterId;//记录操作人
                            vam.UpdateTime = DateTime.Now;
                            vam.Save();
                        }
                    }
                }
            }
            return apply.Id;
        }
        #endregion

        #region 读取部门和人员， 通过部门和人员等条件读取申请单列表 add by quxiaoshan 2015-5-7

        /// <summary>
        /// 读取待审核的申请单 add by luochunhui 
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public DataSet ReadApply(VIPApplyOption option)
        {
            string sql = "select * from View_VIPApply where 1=1";
            if (option.Id > 0)
            {
                sql += string.Format(" and Id={0} ", option.Id);
            }
            if (option.ComId > 0)
            {
                sql += string.Format(" and ComId={0}", option.ComId);
            }
            if (option.Status != "")
            {
                string[] status = option.Status.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                if (status.Length == 1)
                {
                    sql += string.Format(" and Status='{0}' ", status[0]);
                }
                else
                {
                    sql += " and Status in ( ";
                    for (int i = 0; i < status.Length; i++)
                    {
                        if (i == 0)
                        {
                            sql += string.Format(" '{0}' ", status[0]);
                        }
                        else sql += string.Format(" ,'{0}' ", status[i]);
                    }
                    sql += " ) ";
                }
            }
            if (option.ConfirmLevel >= 0)
            {
                if (option.IsPass == 1)
                {
                    sql += string.Format(" and ApplyConfirmLevel>={0}", option.ConfirmLevel);
                }
                else
                {
                    sql += string.Format(" and ApplyConfirmLevel={0}", option.ConfirmLevel);
                }
            }
            if (option.Code != "")
            {
                sql += string.Format(" and Code like '{0}%'", option.Code);
            }
            if (option.IsBudget > 0 && option.MemberId > 0)
            {
                sql += string.Format(" and ( MemberId={0} ) ", option.MemberId);
            }
            else if (option.MemberId > 0)
            {
                sql += string.Format(" and ( MemberId={0} or OperatorId={0} ) ", option.MemberId);
            }
            if (option.DeptId > 0)
            {
                sql += string.Format("and DeptId={0} ", option.DeptId, option.Code);
            }
            if (option.StartDate != new DateTime(1900, 1, 1))
            {
                sql += string.Format(" and UpdateTime >='{0}' ", option.StartDate.ToShortDateString());
            }
            if (option.EndDate != new DateTime(1900, 1, 1))
            {
                sql += string.Format(" and UpdateTime <'{0}' ", option.EndDate.AddDays(1).ToShortDateString());
            }
            sql += "  order by UpdateTime desc ; select @@ROWCOUNT as rownum ";
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 找到最近的管理员
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public DataSet GetAdmin(VIPApplyOption option)
        {
            string sql = "select top 1 mr.MemberId,mr.RoleId,m.ComId,d.Code,d.Level from Member m inner join Dept d on m.DeptId=d.Id inner join MemberRole mr on m.Id=mr.MemberId where 1=1";
            if (option.ComId > 0)
            {
                sql += string.Format(" and m.ComId={0}", option.ComId);
            }
            if (option.Code != "")
            {
                DeptCode dc = new DeptCode();
                List<string> code = dc.GetAllCode(option.Code);
                sql += " and ( ";
                for (int i = 0; i < code.Count; i++)
                {
                    sql += "  d.Code = '" + code[i] + "' ";
                    if (i < code.Count - 1)
                    {
                        sql += " or ";
                    }
                }
                sql += " ) ";
            }
            if (option.RoleId > 0)
            {
                sql += string.Format(" and RoleId={0}", option.RoleId);
            }
            if (option.MemberId > 0)//传memberId是为了找到当前提交申请单的是成本中心管理员 把他排除 找到上一级管理员
            {
                sql += string.Format(" and m.Id <> {0}", option.MemberId);
            }
            sql += "  and IsVisible=1 order by Code desc";
            return m_dbo.GetDataSet(sql);
        }

        #endregion

        #region 提交申请到上一级，申请单转订单，add by quxiaoshan 2015-5-8

        /// <summary>
        /// 批量将申请单提交到上一级 
        /// </summary>
        /// <param name="applyIds"></param>
        /// <param name="comId"></param>
        /// <returns></returns>
        public int VIPApplysToHigherLevel(int[] applyIds, int confirmLevel, int operatorId, string memo, int isPass)
        {
            int oknum = 0;
            for (int i = 0; i < applyIds.Length; i++)
            {
                int higherLevel = VIPApplyToHigherLevel(applyIds[i], confirmLevel, operatorId, memo, isPass);

                if (higherLevel > 0)
                {
                    oknum += 1;
                }
            }
            return oknum;
        }

        /// <summary>
        /// 将一条申请单提交到上一级
        /// </summary>
        /// <param name="applyId"></param>
        /// <param name="comId"></param>
        /// <returns></returns>
        private int VIPApplyToHigherLevel(int applyId, int confirmLevel, int operatorId, string memo, int isPass)
        {
            VIPApply apply = new VIPApply();
            apply.Id = applyId;
            if (apply.Load())
            {
                apply.ConfirmLevel = confirmLevel;
                apply.OperatorId = operatorId; //这里处理添加操作员的ID，相当于登录人的memberId
                apply.Save();
                //审核通过一级时给上一级再发送
                LeadingEmail le = new LeadingEmail();
                le.SendEmail(applyId, isPass);

                //保存申请单审核记录表
                Confirmprocess process = new Confirmprocess();
                process.ApplyId = applyId;
                process.MemberId = operatorId;
                process.Memo = memo;
                process.Status = apply.Status;
                process.ConfirmLevel = confirmLevel;
                process.Amount = apply.SumMoney;
                process.UpdateTime = DateTime.Now;
                process.Save();
                return apply.Id;
            }
            return 0;
        }

        /// <summary>
        /// 批量将 申请转换为正式订单
        /// </summary>
        /// <param name="ApplyIds"></param>
        /// <returns>转换成功的数量</returns>
        public int VIPApplyToOrder(int[] ApplyIds, int OperatorId, int RoleId, string memo, Customer customer)
        {
            int OKCount = 0;
            for (int i = 0; i < ApplyIds.Length; i++)
            {
                if (ConvertApplyToOrder(ApplyIds[i], OperatorId, RoleId, memo, customer))
                {
                    OKCount += 1;
                }
            }
            return OKCount;
        }
        /// <summary>
        /// 将单条申请单转换为订单
        /// </summary>
        /// <param name="ApplyId"></param>
        /// <param name="OperatorId"></param>
        /// <param name="RoleId"></param>
        /// <param name="Memo"></param>
        /// <returns></returns>
        private bool ConvertApplyToOrder(int ApplyId, int OperatorId, int RoleId, string Memo, Customer customer)
        {
            ViewVIPApply View_Apply = new ViewVIPApply(ApplyId);
            if (View_Apply.Status != CommenClass.VIPApplyStatus.待审核.ToString())
                return false;
            VIPApply apply = new VIPApply(ApplyId);
            DataTable dt = apply.ReadDetail().Tables[0];
            Order order = GetOrderInfoByApply(View_Apply, dt.Rows.Count, customer);
            OrderDetail[] od = GetOrderDetail(dt);//初始化订单明细数据
            OrderManager om = new OrderManager();
            int orderId = om.AddOrder(order, od);
            if (orderId > 0)
            {
                //保存申请单审核记录表
                Confirmprocess process = new Confirmprocess();
                process.ApplyId = ApplyId;
                process.MemberId = OperatorId;
                process.Memo = apply.Memo;
                process.Status = CommenClass.VIPApplyStatus.已审核.ToString();
                process.Amount = apply.SumMoney;
                process.UpdateTime = DateTime.Now;

                apply.Status = CommenClass.VIPApplyStatus.已审核.ToString();
                apply.OperatorId = OperatorId;
                apply.UpdateTime = process.UpdateTime;
                if (RoleId == 1)//系统管理员审核通过的
                {
                    apply.ConfirmLevel = 0;
                    process.ConfirmLevel = 0;
                }
                else//成本中心管理员通过的
                {
                    Member m = new Member(OperatorId);
                    Dept dept = new Dept(m.DeptId);
                    apply.ConfirmLevel = dept.Level;//同时修改一下申请单的confirmLevel
                    process.ConfirmLevel = dept.Level;
                }
                if (apply.Save() > 0)//申请单状态更改了
                {
                    //发邮件审核通过的通知
                    LeadingEmail email = new LeadingEmail();
                    email.EmailToPass(ApplyId, apply.MemberId);
                }
                process.Save();
                return true;
            }
            else
            {
                return false;
            }
        }
        private Order GetOrderInfoByApply(ViewVIPApply apply, int RowNum, Customer customer)
        {
            Order order = new Order();
            order.Address = apply.Address;
            order.ComId = apply.ComId;
            order.Company = apply.CompanyName;

            VIPApply va = new VIPApply(apply.Id);
            if (va.NewDeptId != 0)
            {
                order.DeptId = va.NewDeptId;
                Dept dept = new Dept(va.NewDeptId);
                order.DeptName = dept.Name;
            }
            else
            {
                order.DeptId = apply.DeptId;
                order.DeptName = apply.DeptName;
            }
            order.GrossProfit = 0;
            order.MemberId = apply.MemberId;
            order.Memo = apply.Memo;
            order.Mobile = apply.Mobile;
            order.OrderTime = DateTime.Now;
            order.OrderType = CommenClass.OrderType.网上订单.ToString();
            order.PayStatus = CommenClass.PayStatus.未付款.ToString();
            order.PlanDate = DateTime.Now.AddDays(1);
            order.Point = 0;
            order.PrintNum = 0;
            order.RealName = apply.RealName;
            order.RowNum = RowNum;
            order.SaveNum = 0;
            order.SumMoney = apply.SumMoney;
            order.Telphone = apply.Telphone;
            order.UpdateTime = DateTime.Now;
            order.UserId = 0;
            order.ApplyId = apply.Id;

            MemberAddress address = new MemberAddress();
            address.Id = apply.MemberAddressId;
            address.Load();
            string city = address.City;
            order.BranchId = GetVIPBranchId(city, order.ComId);//按默认地址的branchId走,没有的是0。add by luochunhui
            order.TaxRate = customer.TaxRate;
            order.Invoice_Name = customer.Invoice_Name;
            order.Invoice_Type = customer.InvoiceType;
            order.Invoice_Content = customer.Invoice_Content;
            return order;
        }
        private OrderDetail[] GetOrderDetail(DataTable dt)
        {
            OrderDetail[] od = new OrderDetail[dt.Rows.Count];

            for (int j = 0; j < dt.Rows.Count; j++)
            {
                DataRow row = dt.Rows[j];
                od[j] = new OrderDetail();
                od[j].SalePrice = DBTool.GetDoubleFromRow(row, "VIPPrice", 0);
                od[j].AC = DBTool.GetDoubleFromRow(row, "InPrice", od[j].SalePrice);
                od[j].GoodsId = DBTool.GetIntFromRow(row, "GoodsId", 0);
                od[j].InPrice = DBTool.GetDoubleFromRow(row, "InPrice", 0);
                od[j].Model = DBTool.GetStringFromRow(row, "Model", "");
                od[j].Num = DBTool.GetIntFromRow(row, "Num", 0);
                od[j].Price = DBTool.GetIntFromRow(row, "Price", 0);
                od[j].TaxInPrice = DBTool.GetDoubleFromRow(row, "TaxInPrice", 0);
                od[j].Amount = od[j].Num * od[j].SalePrice;
                od[j].IsShow = 1;
                od[j].IsCalc = 1;
            }
            return od;
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdList"></param>
        /// <returns>table0:申请单列表，table1:申请明细表</returns>
        public DataSet ReadVIPApplyDetailsByIds(List<int> IdList)
        {
            string wheresql = "( ";
            for (int i = 0; i < IdList.Count; i++)
            {
                if (i == 0)
                {
                    wheresql += IdList[i].ToString();
                }
                else
                {
                    wheresql += "," + IdList[i].ToString();
                }
            }
            wheresql += ") ";
            string sql = string.Format(@"select * from view_VIPApply where Id in{0} order by Id;
                         select * from View_VIPApplyDetail where ApplyId in{0} order by ApplyId,DisplayName;", wheresql);
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 读取单条审核单明细
        /// </summary>
        /// <param name="ApplyId"></param>
        /// <returns></returns>
        public DataSet ReadVIPApplyDetailsByIds(int ApplyId)
        {
            string sql = " select GoodsId,model,Num,VIPPrice as SalePrice,(VIPPrice*Num) as Amount,Unit,DisplayName  from View_VIPApplyDetail where 1=1";
            if (ApplyId > 0)
            {
                sql += string.Format(" and ApplyId = {0}", ApplyId);
            }
            sql += " order by ApplyId,DisplayName";
            return m_dbo.GetDataSet(sql);
        }
        public DataSet ReadVIPApplyDetailsByIds(List<int> IdList, int CounterId)
        {
            string wheresql = "( ";
            for (int i = 0; i < IdList.Count; i++)
            {
                if (i == 0)
                {
                    wheresql += IdList[i].ToString();
                }
                else
                {
                    wheresql += "," + IdList[i].ToString();
                }
            }
            wheresql += ") ";
            string sql = string.Format(@"select * from view_VIPApply where Id in{0} order by Id; select *,(select Remark from VIPCounterDetail vd  where CounterId={1} and vd.GoodsId=vvd.GoodsId) as Remark from View_VIPApplyDetail vvd where ApplyId in{0} order by ApplyId,DisplayName;", wheresql, CounterId);
            return m_dbo.GetDataSet(sql);
        }

        #region vip客户branchId的分配 add by quxiaoshan 2015-4-28
        /// <summary>
        /// 获取vip客户的branchId
        /// </summary>
        /// <param name="comId"></param>
        /// <returns></returns>
        public int GetVIPBranchId(string City, int ComId)
        {
            Customer customer = new Customer(ComId);
            int branchId = 0;//暂时这么处理
            if (customer.IsNational == 1)
            {
                Sys_Branch branch = new Sys_Branch();
                if (branch.LoadByCity(City))
                {
                    if (branch.IsUnable == 1)
                    {
                        branchId = branch.Id;
                    }
                }
            }
            else
            {
                branchId = customer.BranchId;
            }
            return branchId;
        }

        #endregion

        /// <summary>
        /// 管理员驳回申请
        /// </summary>
        /// <param name="ApplyId"></param>
        /// 
        /// <returns></returns>
        public bool RejectVIPApply(int ApplyId)
        {
            string sql = string.Format(" update VIPApply set Status='{0}' , ConfirmLevel=0 where Id={1} ", CommenClass.VIPApplyStatus.已驳回.ToString(), ApplyId);
            return m_dbo.ExecuteNonQuery(sql);
        }
        /// <summary>
        /// 查找被驳回的申请单
        /// </summary>
        /// <param name="ApplyId"></param>
        /// <returns></returns>
        public DataSet ReadApply(int ApplyId)
        {
            string sql = string.Format("select * from View_VIPApply where Id ={0}", ApplyId);
            return m_dbo.GetDataSet(sql);
        }

        /// <summary>
        /// 读取申请单明细（人保导出订单用）
        /// </summary>
        /// <returns></returns>
        public DataSet GetVipApplyDetail(int ApplyId)
        {
            if (ApplyId > 0)
            {
                string sql = string.Format(@"select vad.ApplyId, g.ID as GoodsId,g.TypeId,gt.TypeName,g.BrandId,b.Name as BrandName,g.DisplayName,vad.VIPPrice,g.MarketPrice,vad.Num,g.Unit from VIPApplyDetail vad join Goods g on vad.GoodsId = g.ID join GoodsType gt on gt.ID=g.TypeId join Brand b on b.Id=g.BrandId where ApplyId={0}", ApplyId);
                return m_dbo.GetDataSet(sql);
            }
            return null;
        }

    }
}

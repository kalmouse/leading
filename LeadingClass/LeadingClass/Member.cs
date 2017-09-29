using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace LeadingClass
{
    public class Member
    {
        public int Id { get; set; }
        public string LoginName { get; set; }
        public string PassWord { get; set; }
        public string RealName { get; set; }
        public string Telphone { get; set; }
        public string Mobile { get; set; }
        public int IsCheckMobile { get; set; }
        public int Status { get; set; }
        public int LoginNumber { get; set; }
        public DateTime RegisterDate { get; set; }
        public double Point { get; set; }
        public int ComId { get; set; }
        public int DeptId { get; set; }
        public int IsMainContact { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
        public string Street { get; set; }
        public string Mansion { get; set; }
        public string Room { get; set; }
        public string Address { get; set; }
        public string PostCode { get; set; }
        public DateTime UpdateTime { get; set; }
        public int IsThirdLogin { get; set; }
        public string Email { get; set; }
        public int IsCheckEmail { get; set; }
        public string RegisterMethod { get; set; }
        public int IsAdmin { get; set; }
        public int BranchId { get; set; }//指定VIP客户对应的加盟商 add by quxiaoshan 2015-4-23
        public int ConfirmLevel { get; set; }//管理员的审核级别 add by quxiaoshan 2015-5-7
        public string SecrecyId { get; set; }
        public DateTime ExportDate { get; set; }//保密用户的用于标示是否数据已经同步
        public int IsOrderAuthority { get; set; }//是否有下单权限 1:有  0:没有
        public int IsVisible { get; set; }//是否是有效的人，作废的人不能删掉（因为有下单记录），所以添加该标示，默认是1是有效，0是无效，不显示
        public int GoodsVisibleLevel { get; set; }
        public string  ValidateCode { get; set; }
        private DBOperate m_dbo;

        public Member()
        {
            Id = 0;
            LoginName = "";
            PassWord = "";
            RealName = "";
            Telphone = "";
            Mobile = "";
            IsCheckMobile = 0;
            Status = 0;
            LoginNumber = 0;
            RegisterDate = DateTime.Now;
            ComId = 0;
            DeptId = 0;
            IsMainContact = 0;
            Province = "";
            City = "";
            Area = "";
            Street = "";
            Mansion = "";
            Room = "";
            Address = "";
            PostCode = "";
            UpdateTime = DateTime.Now;
            IsThirdLogin = 0;
            Email = "";
            IsCheckEmail = 0;
            RegisterMethod = "";
            IsAdmin = 0;
            BranchId = 0;
            ConfirmLevel = 0;
            ExportDate = Convert.ToDateTime("1900-01-01 00:00:00.000");
            SecrecyId = "";
            IsOrderAuthority = 0;//默认是0 没有下单权限
            IsVisible = 1;
            GoodsVisibleLevel = 1;
            m_dbo = new DBOperate();
        }
        public Member(int Id)
        {
            m_dbo = new DBOperate();
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
            arrayList.Add(new SqlParameter("@LoginName", LoginName));
            arrayList.Add(new SqlParameter("@PassWord", PassWord));
            arrayList.Add(new SqlParameter("@RealName", RealName));
            arrayList.Add(new SqlParameter("@Telphone", Telphone));
            arrayList.Add(new SqlParameter("@Mobile", Mobile));
            arrayList.Add(new SqlParameter("@IsCheckMobile", IsCheckMobile));
            arrayList.Add(new SqlParameter("@Status", Status));
            arrayList.Add(new SqlParameter("@LoginNumber", LoginNumber));
            arrayList.Add(new SqlParameter("@RegisterDate", RegisterDate));
            //arrayList.Add(new SqlParameter("@Point", Point)); //积分通过 pointdetail 的balance 来读取
            arrayList.Add(new SqlParameter("@ComId", ComId));
            arrayList.Add(new SqlParameter("@DeptId", DeptId));
            arrayList.Add(new SqlParameter("@IsMainContact", IsMainContact));
            arrayList.Add(new SqlParameter("@Province", Province));
            arrayList.Add(new SqlParameter("@City", City));
            arrayList.Add(new SqlParameter("@Area", Area));
            arrayList.Add(new SqlParameter("@Street", Street));
            arrayList.Add(new SqlParameter("@Mansion", Mansion));
            arrayList.Add(new SqlParameter("@Room", Room));
            arrayList.Add(new SqlParameter("@Address", GetAddress()));
            arrayList.Add(new SqlParameter("@PostCode", PostCode));
            arrayList.Add(new SqlParameter("@UpdateTime", UpdateTime));
            arrayList.Add(new SqlParameter("@IsThirdLogin", IsThirdLogin));
            arrayList.Add(new SqlParameter("@Email", Email));
            arrayList.Add(new SqlParameter("@IsCheckEmail", IsCheckEmail));
            arrayList.Add(new SqlParameter("@RegisterMethod", RegisterMethod));
            arrayList.Add(new SqlParameter("@IsAdmin", IsAdmin));
            arrayList.Add(new SqlParameter("@BranchId", BranchId));
            arrayList.Add(new SqlParameter("@ConfirmLevel", ConfirmLevel));
            arrayList.Add(new SqlParameter("@ExportDate", ExportDate));
            arrayList.Add(new SqlParameter("@SecrecyId", SecrecyId));
            arrayList.Add(new SqlParameter("@IsOrderAuthority", IsOrderAuthority));
            arrayList.Add(new SqlParameter("@IsVisible", IsVisible));
            arrayList.Add(new SqlParameter("@GoodsVisibleLevel", GoodsVisibleLevel));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("Member", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("Member", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
                if (this.Id > 0)
                {
                    MemberAddress Maddress = new MemberAddress();
                    Maddress.MemberId = Id;
                    Maddress.RealName = RealName;
                    Maddress.Address = Address;
                    Maddress.Province = Province;
                    Maddress.City = City;
                    Maddress.Area = Area;
                    Maddress.Street = Street; ;
                    Maddress.Mansion = Mansion;
                    Maddress.Room = Room;
                    Maddress.TelPhone = Telphone;
                    Maddress.Mobile = Mobile;
                    Maddress.Email = Email;
                    Maddress.IsDefault = 1;
                    Maddress.Save();

                }
            }
            if (this.Id > 0 && Email != "")
            {
                Memberinfocfg Minfocfg = new Memberinfocfg();
                Minfocfg.MemberId = Id;
                Minfocfg.Load();
                Minfocfg.Type = CommenClass.MemberEmailTimely.实时发送.ToString();
                Minfocfg.UpdateTime = DateTime.Now;
                Minfocfg.Value = IsCheckEmail;
                Minfocfg.Save();
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from Member where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count ==1)
            {
                DataRow row = ds.Tables[0].Rows[0];
                LoadFromRow(row);
                return true;
            }
            return false;
        }
        public bool Load(int Id, int ComId)
        {
            string sql = string.Format("select * from Member where Id={0} and ComId={1}", Id, ComId);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count == 1)
            {
                DataRow row = ds.Tables[0].Rows[0];
                LoadFromRow(row);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 获取联系人地址
        /// </summary>
        /// <returns></returns>
        private string GetAddress()
        {
            string result = "";
            result = AppendString(result, Province);
            if (City != Province && City != "")
            {
                result = AppendString(result, City);
            }
            result = AppendString(result, Area);
            result = AppendString(result, Street);
            //result = AppendString(result, Mansion);
            //result = AppendString(result, Room);
            return result;
        }
        private string AppendString(string result, string part)
        {
            if (part == "")
                return result;
            if (result != "")
                return result + "." + part;
            else return part;
        }
        public bool LoadFromLoginName(string LoginName)
        {
            string sql = string.Format("select * from Member where LoginName='{0}'", LoginName);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count >0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                LoadFromRow(row);
                return true;
            }
            return false;
        }
        public bool LoadFromSecrecyId(string SecrecyId)
        {
            string sql = string.Format("select * from Member where SecrecyId='{0}'", SecrecyId);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                LoadFromRow(row);
                return true;
            }
            return false;
        }

        private void LoadFromRow(DataRow row)
        {
            Id = DBTool.GetIntFromRow(row, "Id", 0);
            LoginName = DBTool.GetStringFromRow(row, "LoginName", "");
            PassWord = DBTool.GetStringFromRow(row, "PassWord", "");
            RealName = DBTool.GetStringFromRow(row, "RealName", "");
            Telphone = DBTool.GetStringFromRow(row, "Telphone", "");
            Mobile = DBTool.GetStringFromRow(row, "Mobile", "");
            IsCheckMobile = DBTool.GetIntFromRow(row, "IsCheckMobile",0);
            Status = DBTool.GetIntFromRow(row, "Status", 0);
            LoginNumber = DBTool.GetIntFromRow(row, "LoginNumber", 0);
            RegisterDate = DBTool.GetDateTimeFromRow(row, "RegisterDate");
            Point = ReadPointBalance();//读取balance赋值point
            ComId = DBTool.GetIntFromRow(row, "ComId", 0);
            DeptId = DBTool.GetIntFromRow(row, "DeptId", 0);
            IsMainContact = DBTool.GetIntFromRow(row, "IsMainContact", 0);
            Province = DBTool.GetStringFromRow(row, "Province", "");
            City = DBTool.GetStringFromRow(row, "City", "");
            Area = DBTool.GetStringFromRow(row, "Area", "");
            Street = DBTool.GetStringFromRow(row, "Street", "");
            Mansion = DBTool.GetStringFromRow(row, "Mansion", "");
            Room = DBTool.GetStringFromRow(row, "Room", "");
            Address = DBTool.GetStringFromRow(row, "Address", "");
            PostCode = DBTool.GetStringFromRow(row, "PostCode", "");
            UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
            IsThirdLogin = DBTool.GetIntFromRow(row, "IsThirdLogin", 0);
            Email = DBTool.GetStringFromRow(row, "Email", "");
            IsCheckEmail = DBTool.GetIntFromRow(row, "IsCheckEmail", 0);
            RegisterMethod = DBTool.GetStringFromRow(row, "RegisterMethod", "");
            IsAdmin = DBTool.GetIntFromRow(row, "IsAdmin", 0);
            BranchId = DBTool.GetIntFromRow(row, "BranchId", 0);
            ConfirmLevel = DBTool.GetIntFromRow(row, "ConfirmLevel", 0);
            ExportDate = DBTool.GetDateTimeFromRow(row, "ExportDate");
            SecrecyId = DBTool.GetStringFromRow(row, "SecrecyId", "");
            IsOrderAuthority = DBTool.GetIntFromRow(row, "IsOrderAuthority", 0);
            IsVisible = DBTool.GetIntFromRow(row, "IsVisible", 0);
            GoodsVisibleLevel = DBTool.GetIntFromRow(row, "GoodsVisibleLevel", 0);
        }

        /// <summary>
        /// 读取已购商品
        /// </summary>
        /// <returns></returns>
        public DataSet ReadBought(string keywords, PageModel pageModel)
        {
            string sql = string.Format(@"select GoodsId,DisplayName,Unit,g.Price,g.HomeImage,package, count(num) as BuyNum, sum(num) as Count,SUM(Amount) as SumMoney 
                                        from Goods g inner join OrderDetail od on g.ID=od.GoodsId inner join [Order] o on od.OrderId = o.Id
                                        where memberId={0} ", Id);
            if (keywords != "")
            {
                sql += string.Format(" and displayname like '%{0}%' ", keywords);
            }
            sql += "group by GoodsId,DisplayName,Unit,g.Price,g.HomeImage,package order by BuyNum desc,SumMoney desc;select @@rowcount";
            DataSet boughtDataSet = new DataSet();
            boughtDataSet = m_dbo.GetDataSet(sql, (pageModel.CurrentPage - 1) * pageModel.PageSize, pageModel.PageSize);
            return boughtDataSet;
        }
        /// <summary>
        /// 读取积分明细
        /// </summary>
        /// <returns></returns>
        public DataSet ReadPoint()
        {
            string sql = string.Format(" select * from PointDetail where memberId={0} order by UpdateTime desc ", this.Id);
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 读取最新积分
        /// </summary>
        /// <returns></returns>
        public double ReadPointBalance()
        {
            string sql = string.Format(" select top 1 Balance from PointDetail where memberId={0} order by UpdateTime desc ", this.Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if(ds.Tables[0].Rows.Count ==1)
            {
                return DBTool.GetDoubleFromRow(ds.Tables[0].Rows[0], "Balance", 0);
            }
            else return 0;
        }

        /// <summary>
        /// 获取用户角色
        /// </summary>
        /// <returns></returns>
        public DataSet ReadRoles()
        {
            string sql = string.Format(" select RoleId,RoleName from view_MemberRole where MemberId={0} order by RoleId", this.Id);
            return m_dbo.GetDataSet(sql);
        }

        /// <summary>
        /// 读取
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public DataSet ReadMemberOrRole(int memberId)
        {
            string sql = string.Format(@"select * from view_MemberRole where MemberId={0} order by RoleId", memberId);
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 读取要同步的人员  add by luochunhui 09-09
        /// </summary>
        /// <returns></returns>
        public DataSet ExportDeptOrMember(DateTime NowTime,DateTime LastTime)
        {
            string sql = string.Format("update Member set ExportDate ='{0}' where ExportDate ='{1}';",NowTime,LastTime);
            sql += string.Format("select * from Member where ExportDate >='{0}'",NowTime);
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 获取上次同步人员的时间 add by luochunhui 09-09
        /// </summary>
        /// <returns></returns>
        public DataSet GetLastExportData(DateTime LastTime)
        {
            string sql = string.Format("select * from Member where ExportDate ='{0}'", LastTime);
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 保密单位用
        /// </summary>
        /// <param name="secrecyId"></param>
        /// <returns></returns>
        public bool SecrecyLoad(string secrecyId)
        {
            string sql = string.Format("select * from Member where SecrecyId={0}", secrecyId);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                SecrecyId = DBTool.GetStringFromRow(row, "SecrecyId", "");
                return true;
            }
            return false;
        }
        /// <summary>
        /// 根据部门Id找到对应的人
        /// </summary>
        /// <returns></returns>
        public DataSet GetMemberByDeptId()
        {
            string sql = string.Format("select * from Member where DeptId={0} and ComId={1}",DeptId,ComId);
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 根据memberId找到对应的公司部门
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public DataSet GetDeptComIdByMemberId(int memberId)
        {
            string sql = string.Format("select * from View_Member where Id={0}", memberId);
            return m_dbo.GetDataSet(sql);
        }
    }

}

//2017-4-18 yanghaiyang   需求008 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace LeadingClass
{
    public class Company
    {
        private int m_Id;
        private string m_name;
        private string m_ShortName;
        private int m_HasSub;
        private string m_ComPath;
        private int m_ParentId;
        private int m_TypeId;
        private string m_Status;
        private string m_EmployeesNum;
        private string m_Province;
        private string m_City;
        private string m_Area;
        private string m_Street;
        private string m_Mansion;
        private string m_Room;
        private string m_Address;
        private string m_PostCode;
        private DateTime m_AddTime;
        private DateTime m_UpdateTime;
        private int m_InnerUserId;
        private string m_Telphone;
        private string m_Fax;
        private string m_WebSite;
        private string m_RegisterMethod;
        private string m_PY;

        private DBOperate m_dbo;
        public int Id { get { return m_Id; } set { m_Id = value; } }
        public string name { get { return m_name; } set { m_name = value; } }
        public string ShortName { get { return m_ShortName; } set { m_ShortName = value; } }
        public int HasSub { get { return m_HasSub; } set { m_HasSub = value; } }
        public string ComPath { get { return m_ComPath; } set { m_ComPath = value; } }
        public int ParentId { get { return m_ParentId; } set { m_ParentId = value; } }
        public int TypeId { get { return m_TypeId; } set { m_TypeId = value; } }
        public string Status { get { return m_Status; } set { m_Status = value; } }
        public string EmployeesNum { get { return m_EmployeesNum; } set { m_EmployeesNum = value; } }
        public string Province { get { return m_Province; } set { m_Province = value; } }
        public string City { get { return m_City; } set { m_City = value; } }
        public string Area { get { return m_Area; } set { m_Area = value; } }
        public string Street { get { return m_Street; } set { m_Street = value; } }
        public string Mansion { get { return m_Mansion; } set { m_Mansion = value; } }
        public string Room { get { return m_Room; } set { m_Room = value; } }
        public string Address { get { return m_Address; } set { m_Address = value; } }
        public string PostCode { get { return m_PostCode; } set { m_PostCode = value; } }
        public DateTime AddTime { get { return m_AddTime; } set { m_AddTime = value; } }
        public DateTime UpdateTime { get { return m_UpdateTime; } set { m_UpdateTime = value; } }
        public int InnerUserId { get { return m_InnerUserId; } set { m_InnerUserId = value; } }
        public string Telphone { get { return m_Telphone; } set { m_Telphone = value; } }
        public string Fax { get { return m_Fax; } set { m_Fax = value; } }
        public string WebSite { get { return m_WebSite; } set { m_WebSite = value; } }
        public string RegisterMethod { get { return m_RegisterMethod; } set { m_RegisterMethod = value; } }
        public string PY { get { return m_PY; } set { m_PY = value; } }
        
        public Company()
        {
            m_Id = 0;
            m_name = "";
            m_ShortName = "";
            m_HasSub = 0;
            m_ComPath = "";
            m_ParentId = 0;
            m_TypeId = 0;
            m_Status = "";
            m_EmployeesNum = "";
            m_Province = "";
            m_City = "";
            m_Area = "";
            m_Street = "";
            m_Mansion = "";
            m_Room = "";
            m_Address = "";
            m_PostCode = "";
            m_AddTime = DateTime.Now;
            m_UpdateTime = DateTime.Now;
            m_InnerUserId = 0;
            m_Telphone = "";
            m_Fax = "";
            m_WebSite = "";
            m_RegisterMethod = "";
            m_PY="";
            m_dbo = new DBOperate();
        }
        public Company(int Id)
        {
            m_dbo = new DBOperate();
            this.Id = Id;
            this.Load();
        }
        public int Save()
        {
            m_name = m_name.Replace("(", "（").Replace(")", "）");//将公司名称中的　半角括号 替换成 全角括号
            if (m_Id == 0)
            {
                //读取公司名称，如果已经存在 就返回 Id
                if (this.LoadByName(m_name))
                {
                    return this.Id;
                }

            }

            ArrayList arrayList = new ArrayList();
            if (m_Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", m_Id));
            }
            arrayList.Add(new SqlParameter("@name", m_name));
            arrayList.Add(new SqlParameter("@ShortName", m_ShortName));

            arrayList.Add(new SqlParameter("@HasSub", m_HasSub));
            arrayList.Add(new SqlParameter("@ParentId", m_ParentId));
            arrayList.Add(new SqlParameter("@TypeId", m_TypeId));
            arrayList.Add(new SqlParameter("@Status", m_Status));
            arrayList.Add(new SqlParameter("@EmployeesNum", m_EmployeesNum));
            arrayList.Add(new SqlParameter("@Province", m_Province));
            arrayList.Add(new SqlParameter("@City", m_City));
            arrayList.Add(new SqlParameter("@Area", m_Area));
            arrayList.Add(new SqlParameter("@Street", m_Street));
            arrayList.Add(new SqlParameter("@Mansion", m_Mansion));
            arrayList.Add(new SqlParameter("@Room", m_Room));
            arrayList.Add(new SqlParameter("@Address", GetAddress()));//自动生成的地址格式
            arrayList.Add(new SqlParameter("@PostCode", m_PostCode));
            arrayList.Add(new SqlParameter("@AddTime", m_AddTime));
            arrayList.Add(new SqlParameter("@UpdateTime", m_UpdateTime));
            arrayList.Add(new SqlParameter("@InnerUserId", m_InnerUserId));
            arrayList.Add(new SqlParameter("@Telphone", m_Telphone));
            arrayList.Add(new SqlParameter("@Fax", m_Fax));
            arrayList.Add(new SqlParameter("@WebSite", m_WebSite));
            arrayList.Add(new SqlParameter("@RegisterMethod", m_RegisterMethod));
            arrayList.Add(new SqlParameter("@PY", CommenClass.StringTools.GetFirstPYLetter("("+m_ShortName+")"+m_name)));
            //生成compath
            string compath = "";
            arrayList.Add(new SqlParameter("@Compath", compath));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("Company", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("Company", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));

                //添加默认的 customer
                Customer customer = new Customer();
                customer.ComId = this.Id;
                customer.ModelId = 2;
                customer.Save();

                //如果 parentId >0 ,修改parent HasSub =1
                if (this.Id > 0)
                {
                    string sql = string.Format("update company set HasSub =1 where Id={0} ", this.ParentId);
                    m_dbo.ExecuteNonQuery(sql);
                }
            }
            ///修改公司路径
            if (this.Id > 0)
            {
                string companypath = "";
                if (this.ParentId > 0)
                {
                    Company com = new Company();
                    com.Id = this.ParentId;
                    com.Load();
                    companypath = com.ComPath + this.Id.ToString() + "-";
                }
                else
                {
                    companypath = this.Id.ToString() + "-";
                }
                UpdateComPath(companypath);
            }

            return this.Id;
        }
        /// <summary>
        /// ERP FCompany.cs 中调用
        ///     FInvoiceList.cs 中调用
        ///     FOrderReceiveMoney.cs 中调用
        ///     FOrderReceiveMoney.cs 中调用
        /// </summary>
        /// <returns></returns>
        public bool Load()
        {
            string sql = string.Format("select * from Company where id={0}", m_Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                m_Id = DBTool.GetIntFromRow(row, "Id", 0);
                m_name = DBTool.GetStringFromRow(row, "name", "");
                m_ShortName = DBTool.GetStringFromRow(row, "ShortName", "");
                m_HasSub = DBTool.GetIntFromRow(row, "HasSub", 0);
                m_ComPath = DBTool.GetStringFromRow(row, "ComPath", "");
                m_ParentId = DBTool.GetIntFromRow(row, "ParentId", 0);
                m_TypeId = DBTool.GetIntFromRow(row, "TypeId", 0);
                m_Status = DBTool.GetStringFromRow(row, "Status", "");
                m_EmployeesNum = DBTool.GetStringFromRow(row, "EmployeesNum", "");
                m_Province = DBTool.GetStringFromRow(row, "Province", "");
                m_City = DBTool.GetStringFromRow(row, "City", "");
                m_Area = DBTool.GetStringFromRow(row, "Area", "");
                m_Street = DBTool.GetStringFromRow(row, "Street", "");
                m_Mansion = DBTool.GetStringFromRow(row, "Mansion", "");
                m_Room = DBTool.GetStringFromRow(row, "Room", "");
                m_Address = DBTool.GetStringFromRow(row, "Address", "");
                m_PostCode = DBTool.GetStringFromRow(row, "PostCode", "");
                m_AddTime = DBTool.GetDateTimeFromRow(row, "AddTime");
                m_UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                m_InnerUserId = DBTool.GetIntFromRow(row, "InnerUserId", 0);
                m_Telphone = DBTool.GetStringFromRow(row, "Telphone", "");
                m_Fax = DBTool.GetStringFromRow(row, "Fax", "");
                m_WebSite = DBTool.GetStringFromRow(row, "WebSite", "");
                m_RegisterMethod = DBTool.GetStringFromRow(row, "RegisterMethod", "");
                m_PY = DBTool.GetStringFromRow(row, "PY", "");
                return true;
            }
            return false;
        }
        /// <summary>
        /// 根据名称Load  addBy_WangPengliang
        /// </summary>
        /// <returns></returns>
        public bool LoadByName(string name)
        {
            string sql = string.Format("select * from Company where name='{0}'", name);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                m_Id = DBTool.GetIntFromRow(row, "Id", 0);
                m_name = DBTool.GetStringFromRow(row, "name", "");
                m_ShortName = DBTool.GetStringFromRow(row, "ShortName", "");
                m_HasSub = DBTool.GetIntFromRow(row, "HasSub", 0);
                m_ComPath = DBTool.GetStringFromRow(row, "ComPath", "");
                m_ParentId = DBTool.GetIntFromRow(row, "ParentId", 0);
                m_TypeId = DBTool.GetIntFromRow(row, "TypeId", 0);
                m_Status = DBTool.GetStringFromRow(row, "Status", "");
                m_EmployeesNum = DBTool.GetStringFromRow(row, "EmployeesNum", "");
                m_Province = DBTool.GetStringFromRow(row, "Province", "");
                m_City = DBTool.GetStringFromRow(row, "City", "");
                m_Area = DBTool.GetStringFromRow(row, "Area", "");
                m_Street = DBTool.GetStringFromRow(row, "Street", "");
                m_Mansion = DBTool.GetStringFromRow(row, "Mansion", "");
                m_Room = DBTool.GetStringFromRow(row, "Room", "");
                m_Address = DBTool.GetStringFromRow(row, "Address", "");
                m_PostCode = DBTool.GetStringFromRow(row, "PostCode", "");
                m_AddTime = DBTool.GetDateTimeFromRow(row, "AddTime");
                m_UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                m_InnerUserId = DBTool.GetIntFromRow(row, "InnerUserId", 0);
                m_Telphone = DBTool.GetStringFromRow(row, "Telphone", "");
                m_Fax = DBTool.GetStringFromRow(row, "Fax", "");
                m_WebSite = DBTool.GetStringFromRow(row, "WebSite", "");
                m_RegisterMethod = DBTool.GetStringFromRow(row, "RegisterMethod", "");
                return true;
            }
            return false;
        }
        /// <summary>
        /// 获取详细地址
        /// </summary>
        /// <returns></returns>
        private string GetAddress()
        {
            string result = "";
            result = AppendString(result, m_Province);
            if (m_City != m_Province && m_City != "")
            {
                result = AppendString(result, m_City);
            }
            result = AppendString(result, m_Area);
            result = AppendString(result, m_Street);
            result = AppendString(result, m_Mansion);
            result = AppendString(result, m_Room);
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

        public bool UpdateComPath(string comPath)
        {
            string sql = string.Format(" update Company Set comPath='{0}' where Id={1} ", comPath, m_Id);
            return m_dbo.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 公司读取部门
        /// 调用：FOrderStatement.cs
        /// ERP   FWSearchOrder.cs 中调用
        /// </summary>
        /// <returns></returns>
        public DataSet ReadDept()
        {
        string sql = string.Format(@"select dbo.Dept.Id,
       dbo.Dept.Name,
       dbo.dept.ComId,
       dbo.Dept.IsCalc,
       dbo.Dept.Code,
       dbo.dept.PCode,
       dbo.dept.Level,
       COUNT(dbo.Member.Id) as MemberNum 
from dbo.Dept left join dbo.Member on dbo.Dept.Id=dbo.Member.DeptId 
where dbo.dept.ComId={0}
group by dept.Id,dbo.Dept.Name,dbo.dept.ComId,dbo.dept.Code,dbo.dept.IsCalc,dbo.dept.PCode,dbo.dept.Level
order by Code;
select @@ROWCOUNT", this.Id);
            return m_dbo.GetDataSet(sql);

        }

        /// <summary>
        /// 获取当前登录人下面的部门
        /// </summary>
        /// <param name="comId"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public DataSet ReadDept(string code)
        {
            string sql = string.Format("select * from Dept where ComId={0} ",Id);
            if (code != "")
            {
                sql += string.Format(" and Code like '{0}%'",code);
            }
            sql += "  order by code";
            return m_dbo.GetDataSet(sql);
        }

        /// <summary>
        /// 获取公司ComID
        /// </summary>
        /// <param name="comId"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public DataSet ReadCompanyId(string comName)
        {
            string sql = string.Format("select * from Company where name='{0}'", comName);
            return m_dbo.GetDataSet(sql);
        }

        public DataTable CheckCompanyInfo(string comName)
        {
            string sql = string.Format("select com.Id as ComId, com.Name as ComName,ISNULL(d.Id,0) as DeptId,d.Name from Company com left join Dept d on com.Id=D.ComId where  com.Name='{0}'", comName);
            return m_dbo.GetDataSet(sql).Tables[0];
        }
        /// <summary>
        /// 获取当前登录人下面的部门--StoreDept表   add by hjy 2016-6-26
        /// </summary>
        /// <param name="comId"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public DataSet ReadStoreDept(string code)
        {
            string sql = string.Format("select * from StoreDept where SComId={0} ", Id);
            if (code != "")
            {
                sql += string.Format(" and SCode like '{0}%'", code);
            }
            sql += "  order by Scode";
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 获取当前部门的下一级子部门
        /// </summary>
        /// <param name="comId"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public DataSet ReadDept(int level, string code)
        {
            string sql = string.Format("select * from Dept where Code like '{0}%' and ComId={1} and (Level = {2} or level ={2}+1)", code,this.m_Id,level);
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 公司读取 会员,status = -1,禁用
        /// 调用：ERP：FSearchOrder.cs
        /// ERP   FWSearchOrder.cs 中调用
        /// </summary>
        /// <returns></returns>
        public DataSet ReadMember()
        {
            string sql = string.Format(" select * from view_Member where ComId={0}  order by RealName ", this.m_Id);
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 返回公司专柜各类商品的数量 IsVisible >=0;
        /// </summary>
        /// <returns></returns>
        public DataSet ReadVIPGoodsTypes()
        {
            string sql = string.Format("select TypeName1,TypeId1,PPCode,TypeName2,TypeId2,PCode,TypeName3,TypeId3,Code,Count(Code) as GoodsNum from view_ComGoods  where comId = {0} and IsVisible >=0 group by TypeName1,TypeId1,PPCode,TypeName2,TypeId2,Pcode,TypeName3,TypeId3,Code order by Code", m_Id);
            return m_dbo.GetDataSet(sql);

        }
        /// <summary>
        /// 读取公司专柜商品;IsVisible >=0;
        /// </summary>
        /// <returns></returns>
        public int ReadVIPGoodsNum(string keyWords, string code)
        {
            string sql = string.Format(" select count(*) as num from view_comgoods where  comId={0} and IsVisible >=0  ", m_Id);
            if (keyWords != "")
            {
                sql += string.Format(" and (SN like '%{0}%' or GoodsName like '%{0}%' or Feature like '%{0}%' ) ", keyWords);

            }
            if (code != "")
            {
                sql += string.Format(" and code like '{0}%' ", code);
            }
            return DBTool.GetIntFromRow(m_dbo.GetDataSet(sql).Tables[0].Rows[0], "num", 0);
        }
        /// <summary>
        /// 读取公司专柜商品;IsVisible >=0;
        /// </summary>
        /// <returns></returns>
        public DataSet ReadVIPGoods(string keyWords, string code)
        {
            string sql = string.Format(" select * from view_comgoods where  comId={0} and IsVisible >=0  ", m_Id);
            if (keyWords != "")
            {
                if (keyWords.Length > 2)
                {
                    keyWords = CommenClass.StringTools.SplitKeyWords(keyWords);
                }
                char[] sep = { ' ' };
                string[] kws = keyWords.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                if (kws.Length == 1)
                {
                    if (kws[0].Length <= 2)//关键字太短的时候，只从 SN和GoodsName中筛选
                    {
                        sql += string.Format(" and ((SN like '%{0}%') or  (PY like '%{0}%')  or (goodsName like '%{0}%')) ", kws[0]);
                    }
                    else
                    {
                        sql += string.Format(" and ((SN like '%{0}%')  or (goodsName like '%{0}%') or  (PY like '%{0}%')  or (Feature like '%{0}%')) ", kws[0]);
                    }
                }
                else
                {
                    for (int i = 0; i < kws.Length; i++)
                    {
                        sql += string.Format(" and ((SN like '%{0}%')  or (goodsName like '%{0}%') or  (PY like '%{0}%')  or (Feature like '%{0}%')) ", kws[i]);
                    }
                }
            }
            if (code != "")
            {
                sql += string.Format(" and code like '{0}%' ", code);
            }
            sql += "  order by code,recommend desc ";
            return m_dbo.GetDataSet(sql);
        }
        public DataSet ReadVIPGoods(string keyWords, string code, RecordControl rc)
        {
            string sql = string.Format(" select * from view_comgoods where  comId={0} and IsVisible >=0  ", m_Id);
            if (keyWords != "")
            {
                if (keyWords.Length > 2)
                {
                    keyWords = CommenClass.StringTools.SplitKeyWords(keyWords);
                }
                char[] sep = { ' ' };
                string[] kws = keyWords.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                if (kws.Length == 1)
                {
                    if (kws[0].Length <= 2)//关键字太短的时候，只从 SN和GoodsName中筛选
                    {
                        sql += string.Format(" and ((SN like '%{0}%') or  (PY like '%{0}%')  or (goodsName like '%{0}%')) ", kws[0]);
                    }
                    else
                    {
                        sql += string.Format(" and ((SN like '%{0}%')  or (goodsName like '%{0}%') or  (PY like '%{0}%')  or (Feature like '%{0}%')) ", kws[0]);
                    }
                }
                else
                {
                    for (int i = 0; i < kws.Length; i++)
                    {
                        sql += string.Format(" and ((SN like '%{0}%')  or (goodsName like '%{0}%') or  (PY like '%{0}%')  or (Feature like '%{0}%')) ", kws[i]);
                    }
                }
            }
            if (code != "")
            {
                sql += string.Format(" and code like '{0}%' ", code);
            }
            sql += "  order by recommend desc,Code  ";
            return m_dbo.GetDataSet(sql, rc.StartRecord, rc.PageSize);
        }
        /// <summary>
        /// 返回商品的 VIP专柜价格
        /// </summary>
        /// <param name="GoodsId"></param>
        /// <returns></returns>
        public double ReadVIPPrice(int GoodsId)
        {
            string sql = string.Format("select max(VIPPrice) as VIPPrice  from view_comgoods where GoodsId = {0} and comId= {1} ", GoodsId, m_Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            return DBTool.GetDoubleFromRow(ds.Tables[0].Rows[0], "VIPPrice", 0);
        }
    }
}

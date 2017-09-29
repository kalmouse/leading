using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
namespace LeadingClass
{
    public class Dept
    {
        private int m_Id;
        private int m_ComId;
        private string m_Name;
        private string m_Code;
        private string m_PCode;
        private int m_Level;
        private DateTime m_ExportDate;//保密用户的用于标示是否数据已经同步
        /// <summary>
        /// 是否独立结算
        /// </summary>
        private int m_IsCalc;
        private DateTime m_UpdateTime;
        private int m_BranchId;
        private int m_IsSepareOrder;
        private int m_SecrecyId;
        private DBOperate m_dbo;
        public int Id { get { return m_Id; } set { m_Id = value; } }
        public int ComId { get { return m_ComId; } set { m_ComId = value; } }
        public string Name { get { return m_Name; } set { m_Name = value; } }
        public string Code { get { return m_Code; } set { m_Code = value; } }
        public string PCode { get { return m_PCode; } set { m_PCode = value; } }
        public int Level { get { return m_Level; } set { m_Level = value; } }
        public int IsCalc { get { return m_IsCalc; } set { m_IsCalc = value; } }
        public DateTime UpdateTime { get { return m_UpdateTime; } set { m_UpdateTime = value; } }
        public int BranchId { get { return m_BranchId; } set { m_BranchId = value; } }
        public int SecrecyId { get { return m_SecrecyId; } set { m_SecrecyId = value; } }
        public DateTime ExportDate { get { return m_ExportDate; } set { m_ExportDate = value; } }//保密用户的用于标示是否数据已经同步
        /// <summary>
        /// 是否单独下单 
        /// </summary>
        public int IsSepareOrder { get { return m_IsSepareOrder; } set { m_IsSepareOrder = value; } }//quxiaoshan 2015-5-21
        public Dept()
        {
            m_Id = 0;
            m_ComId = 0;
            m_Name = "";
            m_Code = "";
            m_PCode = "";
            m_Level = 0;
            m_IsCalc = 0;
            m_UpdateTime = DateTime.Now;
            m_BranchId = 1;
            m_IsSepareOrder = 0;
            ExportDate = Convert.ToDateTime("1900-01-01 00:00:00.000");
            m_SecrecyId = 0;
            m_dbo = new DBOperate();
        }
        public Dept(int Id)
        {
            m_dbo = new DBOperate();
            this.Id = Id;
            this.Load();
        }
        public int Save()
        {
            ///如果该部门存在，直接返回部门id
            int id = IsExsist(m_Name, m_ComId);
            if (id > 0)
            {
                return id;
            }

            ArrayList arrayList = new ArrayList();
            if (m_Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", m_Id));
            }
            arrayList.Add(new SqlParameter("@ComId", m_ComId));
            arrayList.Add(new SqlParameter("@Name", m_Name));
            arrayList.Add(new SqlParameter("@Code", m_Code));
            arrayList.Add(new SqlParameter("@PCode", m_PCode));
            arrayList.Add(new SqlParameter("@Level", m_Level));
            arrayList.Add(new SqlParameter("@IsCalc", m_IsCalc));
            arrayList.Add(new SqlParameter("@UpdateTime", m_UpdateTime));
            arrayList.Add(new SqlParameter("@BranchId", m_BranchId));
            arrayList.Add(new SqlParameter("@IsSepareOrder", m_IsSepareOrder));
            arrayList.Add(new SqlParameter("@ExportDate", ExportDate));
            arrayList.Add(new SqlParameter("@SecrecyId", m_SecrecyId));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("Dept", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("Dept", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public int UpdateSave()
        {
            ArrayList arrayList = new ArrayList();
            if (m_Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", m_Id));
            }
            arrayList.Add(new SqlParameter("@ComId", m_ComId));
            arrayList.Add(new SqlParameter("@Name", m_Name));
            arrayList.Add(new SqlParameter("@Code", m_Code));
            arrayList.Add(new SqlParameter("@PCode", m_PCode));
            arrayList.Add(new SqlParameter("@Level", m_Level));
            arrayList.Add(new SqlParameter("@IsCalc", m_IsCalc));
            arrayList.Add(new SqlParameter("@UpdateTime", m_UpdateTime));
            arrayList.Add(new SqlParameter("@BranchId", m_BranchId));
            arrayList.Add(new SqlParameter("@IsSepareOrder", m_IsSepareOrder));
            arrayList.Add(new SqlParameter("@ExportDate", ExportDate));
            arrayList.Add(new SqlParameter("@SecrecyId", m_SecrecyId));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("Dept", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("Dept", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from Dept where id={0}", m_Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                m_Id = DBTool.GetIntFromRow(row, "Id", 0);
                m_ComId = DBTool.GetIntFromRow(row, "ComId", 0);
                m_Name = DBTool.GetStringFromRow(row, "Name", "");
                m_Code = DBTool.GetStringFromRow(row, "Code", "");
                m_PCode = DBTool.GetStringFromRow(row, "PCode", "");
                m_Level = DBTool.GetIntFromRow(row, "Level", 0);
                m_IsCalc = DBTool.GetIntFromRow(row, "IsCalc", 0);
                m_UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                m_BranchId = DBTool.GetIntFromRow(row, "BranchId", 0);
                m_IsSepareOrder = DBTool.GetIntFromRow(row, "IsSepareOrder", 0);
                m_SecrecyId = DBTool.GetIntFromRow(row, "SecrecyId", 0);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 保密单位用
        /// </summary>
        /// <param name="secrecyId"></param>
        /// <returns></returns>
        public bool SecrecyLoad(int secrecyId)
        {
            string sql = string.Format("select * from Dept where SecrecyId={0}", secrecyId);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                m_SecrecyId = DBTool.GetIntFromRow(row, "SecrecyId", 0);
                return true;
            }
            return false;
        }

        public bool LoadByName(string deptName)
        {
            string sql = string.Format("select * from Dept where name='{0}' ", deptName);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                return true;
            }
            return false;
        }

        public int IsExsist(string deptName, int comId)
        {
            string sql = string.Format("select * from Dept where ComId={0} and Name='{1}' ", comId, deptName);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DBTool.GetIntFromRow(ds.Tables[0].Rows[0], "Id", 0);
            }
            else return 0;
        }
        public bool Delete()
        {
            string sql = string.Format(" delete from Dept where Id={0} ", m_Id);
            return m_dbo.ExecuteNonQuery(sql);
        }
        //获取各级部门的code
        public DataSet GetLevel1Dept()
        {
            string sql = "select * from Dept where 1=1";
            if (m_ComId != 0)
            {
                sql += string.Format(" and ComId={0}",m_ComId);
            }
            if (m_Level != 0)
            {
                sql += string.Format(" and Level = {0}",m_Level);
            }
            if (m_Code != null)
            {
                sql += string.Format(" and Code like '{0}%'",m_Code);
            }
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 获取某公司的部门级别
        /// </summary>
        /// <returns></returns>
        public int GetLevelCountByComId()
        {
            string sql = "";
            if (m_ComId != 0)
            {
                if (m_Code != "")
                {
                    sql = string.Format("declare @Rank int set @Rank = (select MAX(Level) from Dept where ComId={0} and Code like '{1}%')-(select Level from Dept where Id={2});select @Rank as RankLevel;", m_ComId, m_Code, m_Id);
                }
                else
                {
                    sql += string.Format("select MAX(Level) as RankLevel from Dept where ComId={0}", m_ComId);
                }
            }
            DataSet ds = m_dbo.GetDataSet(sql);
            return Convert.ToInt32(ds.Tables[0].Rows[0]["RankLevel"]);
        }
        /*读取部门下的人员*/
        public DataSet GetDeptMember()
        {
            string sql = string.Format("select * from view_Member where ComId={0}", m_ComId);
            if (m_Id != 0)
            {
                sql += string.Format("and  DeptId={0}", m_Id);
            }
            sql += " and IsVisible=1 Order by RealName;";//select * from view_Member where ComId=1 and IsVisible=1 Order by RealName;select @@ROWCOUNT
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 检查某一deptID是否某一部门的子部门
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool IsHasDeptId(int ID)
        {
            string sql = string.Format(" select * from Dept where ComId ={0} and Code like '{1}%' and Id ={2}",m_ComId,m_Code,ID);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
                return true;
            else return false;
        }
        /// <summary>
        /// 读取公司对应的部门信息  add by quxiaoshan 2015-3-23
        /// </summary>
        /// <returns></returns>
        public DataSet ReadDeptByComId()
        {
            string sql = string.Format(@"select * from dbo.Dept where ComId={0} order by Code", m_ComId);
            return m_dbo.GetDataSet(sql);
        }

        public DataSet GetAllDept(int memberId)
        {
            string sql = string.Format("select Dept.Id,Dept.Name from dbo.Dept where ComId =(select ComId from dbo.Member where Id={0});", memberId);
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 部门查找
        /// </summary>
        /// <returns></returns>
        public bool GetDeptByComIdDeptName()
        {
            string sql = string.Format(@"select * from Dept where ComId={0} and Name='{1}'", m_ComId, m_Name);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                m_Id = DBTool.GetIntFromRow(row, "Id", 0);
                m_ComId = DBTool.GetIntFromRow(row, "ComId", 0);
                m_Name = DBTool.GetStringFromRow(row, "Name", "");
                m_Code = DBTool.GetStringFromRow(row, "Code", "");
                m_PCode = DBTool.GetStringFromRow(row, "PCode", "");
                m_Level = DBTool.GetIntFromRow(row, "Level", 0);
                m_IsCalc = DBTool.GetIntFromRow(row, "IsCalc", 0);
                m_UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                m_BranchId = DBTool.GetIntFromRow(row, "BranchId", 0);
                m_IsSepareOrder = DBTool.GetIntFromRow(row, "IsSepareOrder", 0);
                m_SecrecyId = DBTool.GetIntFromRow(row, "SecrecyId", 0);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 读取部门数量和人员数量
        /// </summary>
        /// <returns></returns>
        public DataSet ReadDeptCountOrMemberCount()
        {
            string sql = string.Format(@"select COUNT(*) as DeptNum from dbo.Dept where ComId={0};
	select COUNT(*) as MemberNum from dbo.Member where ComId ={0}", m_ComId);
            return m_dbo.GetDataSet(sql);
        }

        /// <summary>
        /// 读取要同步的部门  add by luochunhui 09-09
        /// </summary>
        /// <returns></returns>
        public DataSet ExportDeptOrMember(DateTime NowTime, DateTime LastTime)
        {
            string sql = string.Format("update Dept set ExportDate ='{0}' where ExportDate ='{1}';", NowTime, LastTime);
            sql += string.Format("select * from Dept where ExportDate >='{0}'", NowTime);
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 获取他们添加的部门 add by luochunhui 09-09
        /// </summary>
        /// <returns></returns>
        public DataSet GetLastExportData(DateTime LastTime)
        {
            string sql = string.Format("select * from Dept where ExportDate ='{0}'", LastTime);
            return m_dbo.GetDataSet(sql);
    }


        /// <summary>
        /// 修改部门
        /// </summary>
        /// <param name="DeptId"></param>
        /// <param name="Code"></param>
        /// <param name="PCode"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public bool UpdateDeptCode(int DeptId, string Code, string PCode, int level)
        {
            string sql = string.Format("update Dept set Code='{1}',PCode='{2}',Level='{3}' where Id='{0}'", DeptId, Code, PCode, level);
            return m_dbo.ExecuteNonQuery(sql);
        }
        /// <summary>
        /// 根据部门ID获取此部门对应的所有子部门
        /// </summary>
        /// <param name="deptId"></param>
        /// <returns></returns>
        public List<int> GetAllChildDept(int deptId)
        {
            List<int> listdept = new List<int>();
            if (deptId > 0)
            {
                string sql = string.Format("select a.Id from Dept a inner join (select Code,comid from Dept where Id={0})b on  a.code like b.code+'%' and a.ComId=b.ComId", deptId);
                DataSet ds = m_dbo.GetDataSet(sql);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        listdept.Add(Convert.ToInt32(row["Id"]));
                    }
                }
            }
            return listdept;
        }

    }
    public class DeptOption
    {
        public int ComId { get; set; }
        public int MemberId { get; set; }
        public int DeptId { get; set; }
        public DBOperate dbo { get; set; }
        public DeptOption()
        {
            MemberId = 0;
            DeptId = 0;
            dbo = new DBOperate();
        }
    }
    public class DeptManager
    {
        public DeptOption deptOption { get; set; }
        public DeptManager()
        {
            deptOption = new DeptOption();
        }
        public DataSet ReadVipBought(PageModel pageModel)
        {
            string sql = string.Format(@"select GoodsId,DisplayName,Unit, dbo.Goods.Price,dbo.Goods.HomeImage,package, count(num) as BuyNum, sum(num) as Count,SUM(amount) as SumMoney  from dbo.[Order]  join dbo.OrderDetail on dbo.[Order].Id=dbo.OrderDetail.OrderId join dbo.Member  
on dbo.[Order].MemberId =dbo.Member.Id join dbo.Goods on dbo.OrderDetail.GoodsId=dbo.Goods.ID
where  dbo.[Order].ComId =(select ComId from dbo.Member where Member.Id={0}) ", deptOption.MemberId);

            if (deptOption.DeptId > 0)
            {
                sql += string.Format(" and [order].DeptId={0}  ", deptOption.DeptId);
            }
            sql += string.Format(" group by GoodsId,DisplayName,Unit,dbo.Goods.Price,dbo.Goods.HomeImage,package order by BuyNum desc,SumMoney desc;select @@rowcount ");
            DataSet boughtDataSet = new DataSet();
            boughtDataSet = deptOption.dbo.GetDataSet(sql, (pageModel.CurrentPage - 1) * pageModel.PageSize, pageModel.PageSize);
            return boughtDataSet;
        }
        public string GetMaxCodeByPCode(string PCode, int ComId)
        {
            string sql = string.Format("select MAX(Code) from Dept where 1=1 and PCode='{0}' and ComId={1} ", PCode, ComId);
            DataSet ds = deptOption.dbo.GetDataSet(sql);
            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return (ds.Tables[0].Rows[0][0] != null) ? ds.Tables[0].Rows[0][0].ToString() : "";
                }
            }
            return "";
        }

        public DataSet ReadSubDept()
        {

            return null;
        }
    }

    /// <summary>
    /// Dpet表中 Code 增加结点等操作 调用此类中的方法
    /// </summary>
    public class DeptCode
    {
        public string Code { get; set; }
        public string PCode { get; set; }
        public List<string> Path { get; set; }
        public int Level { get; set; }

        public DeptCode()
        {
            Code = "";
            PCode = "";
            Path = new List<string>();
            Level = 0;
        }

        public DeptCode(string code)
        {
            Code = code;
            Path = GetPath(code);//获取当前Code的路径

            //获取PCode
            if (Path.Count > 1)
            {
                PCode = Path[Path.Count - 2];
            }
            else PCode = "";

            Level = Path.Count;
        }
        /// <summary>
        /// 根据目前最大的 子Code，获取下一个 子Code 的代码， MaxChildCode 由 Dept 类 提供
        /// </summary>
        /// <param name="MaxChildCode"></param>
        /// <returns></returns>
        public string GetAChildCode(string MaxChildCode)
        {
            string result = "";
            List<string> childpath = GetPath(MaxChildCode);
            if (childpath.Count > 0)
            {
                try
                {
                    string newId = (int.Parse(childpath[childpath.Count - 1]) + 1).ToString();
                    switch (newId.Length)
                    {
                        case 1:
                            newId = "00" + newId;
                            break;
                        case 2:
                            newId = "0" + newId;
                            break;
                        case 3:
                            break;
                        default:
                            break;
                    }

                    for (int i = 0; i < childpath.Count - 1; i++)
                    {
                        result += childpath[i] + "|";
                    }
                    result += newId;

                }
                catch
                {
                }
            }
            else
            {
                result = "001";
            }
            return result;
        }

        /// <summary>
        /// 当自己是 上级部门 中 Code 最大的部门时，获取 下一个 同级 部门的 Code
        /// </summary>
        /// <returns></returns>
        public string GetNextCode()
        {
            return GetAChildCode(this.Code);
        }

        /// <summary>
        /// 根据“|”拆分Code路径
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private List<string> GetPath(string code)
        {
            string[] path = code.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> result = new List<string>();
            for (int i = 0; i < path.Length; i++)
            {
                result.Add(path[i]);
            }
            return result;
        }

        /// <summary>
        /// 根据部门的code获取全部code路径
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public List<string> GetAllCode(string code)
        {
            List<string> result = new List<string>();
            string[] path = code.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < path.Length; i++)
            {
                string tmp = "";
                for (int j = 0; j < i + 1; j++)
                {
                    if (j > 0)
                    {
                        tmp += "|";
                    }
                    tmp += path[j];
                }
                result.Add(tmp);
            }
            return result;
        }
    }
}

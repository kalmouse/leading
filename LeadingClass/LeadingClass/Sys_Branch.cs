using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Data.SqlTypes;
using System.Collections;

namespace LeadingClass
{
    public class Sys_Branch
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Telphone { get; set; }
        public string LinkMan { get; set; }
        public string Bank { get; set; }
        public string Account { get; set; }
        public string Complain_hotline { get; set; }
        public string Print_title { get; set; }
        public string Province { get; set; }
        public int IsUnable { get; set; }
        public int ProvinceSort { get; set; }
        public int CitySort { get; set; }
        public int IsUseSiteGoods { get; set; }
        public int CityId { get; set; }
        public int level { get; set; }
        private DBOperate m_dbo;

        public Sys_Branch()
        {
            Id = 0;
            Name = "";
            ShortName = "";
            City = "";
            Address = "";
            Telphone = "";
            LinkMan = "";
            Bank = "";
            Account = "";
            Complain_hotline = "";
            Print_title = "";
            Province = "";
            IsUnable = 0;
            ProvinceSort = 0;
            CitySort = 0;
            IsUseSiteGoods = 0;
            CityId = 0;
            level = 0;
            m_dbo = new DBOperate();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", Id));
                if (Id == 1)
                {
                    ProvinceSort = 1;
                }
            }
            arrayList.Add(new SqlParameter("@Name", Name));
            arrayList.Add(new SqlParameter("@ShortName", ShortName));
            arrayList.Add(new SqlParameter("@City", City));
            arrayList.Add(new SqlParameter("@Address", Address));
            arrayList.Add(new SqlParameter("@Telphone", Telphone));
            arrayList.Add(new SqlParameter("@LinkMan", LinkMan));
            arrayList.Add(new SqlParameter("@Bank", Bank));
            arrayList.Add(new SqlParameter("@Account", Account));
            arrayList.Add(new SqlParameter("@Complain_hotline", Complain_hotline));
            arrayList.Add(new SqlParameter("@Print_title", Print_title));
            arrayList.Add(new SqlParameter("@Province", Province));
            arrayList.Add(new SqlParameter("@IsUnable", IsUnable));
            arrayList.Add(new SqlParameter("@ProvinceSort", ProvinceSort));
            arrayList.Add(new SqlParameter("@CitySort", CitySort));
            arrayList.Add(new SqlParameter("@IsUseSiteGoods", IsUseSiteGoods));
            arrayList.Add(new SqlParameter("@CityId", CityId));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("Sys_Branch", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("Sys_Branch", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
                //初始化分支机构数据
                InitData(this.Id);
            }
            return this.Id;
        }

        private void InitData(int branchId)
        {
            //添加默认的仓库
            Store store = new Store();
            store.BranchId = branchId;
            store.IsAvalible = 1;
            store.IsDefault = 1;
            store.Name = "主仓库(" + this.Name + ")";
            store.Place = "同公司";
            store.UpdateTime = DateTime.Now;
            store.Save();
            //添加默认的收款账户
            BankAccount ba = new BankAccount();
            ba.BranchId = branchId;
            ba.Account = "0";
            ba.Bank = "";
            ba.Company = this.Name;
            ba.Name = "主账户" + this.Name;
            ba.Save();
            //添加默认的部门
            string[] depts = new string[] { "总经办", "销售部", "客服部", "物流部", "仓储部", "采购部", "财务部", "行政部", "技术部" };
            for (int i = 0; i < depts.Length; i++)
            {
                Sys_Dept dept = new Sys_Dept();
                dept.BranchId = branchId;
                dept.Code = "0" + (i + 1).ToString();
                dept.Name = depts[i];
                dept.Memo = "";
                dept.Save();
            }
        }

        public bool Load()
        {
            string sql = string.Format("select * from Sys_Branch where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                Name = DBTool.GetStringFromRow(row, "Name", "");
                ShortName = DBTool.GetStringFromRow(row, "ShortName", "");
                City = DBTool.GetStringFromRow(row, "City", "");
                Address = DBTool.GetStringFromRow(row, "Address", "");
                Telphone = DBTool.GetStringFromRow(row, "Telphone", "");
                LinkMan = DBTool.GetStringFromRow(row, "LinkMan", "");
                Bank = DBTool.GetStringFromRow(row, "Bank", "");
                Account = DBTool.GetStringFromRow(row, "Account", "");
                Complain_hotline = DBTool.GetStringFromRow(row, "Complain_hotline", "");
                Print_title = DBTool.GetStringFromRow(row, "Print_title", "");
                Province = DBTool.GetStringFromRow(row, "Province", "");
                IsUnable = DBTool.GetIntFromRow(row, "IsUnable", 0);
                ProvinceSort = DBTool.GetIntFromRow(row, "ProvinceSort", 0);
                CitySort = DBTool.GetIntFromRow(row, "CitySort", 0);
                IsUseSiteGoods = DBTool.GetIntFromRow(row, "IsUseSiteGoods", 0);
                CityId = DBTool.GetIntFromRow(row, "CityId", 0); 
                return true;
            }
            return false;
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from Sys_Branch where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }
        public string GetAddress()
        {
            string result = "";
            if (Province != City)
            {
                result += Province + " ▪ " + City;
            }
            else result += City;
            result += " ▪ " + Address;
            return result;
        }

        
        public DataSet  ReadDeptofBranch(int branchId)
        {
            string sql = "select * from Sys_Dept where 1=1 ";
            sql += string.Format(" and BranchId = {0} ", branchId);
            sql += " order by Code,Name ";
            return m_dbo.GetDataSet(sql);
        }

        /// <summary>
        /// 获得该省份对应的加盟商城市 add by quxiaoshan 2015-4-15
        /// </summary>
        /// <returns></returns>
        public bool LoadByCity(string city)
        {
            string sql = string.Format(" select * from dbo.Sys_Branch where IsUnable=1");
            if (city != "")
            {
                sql += string.Format(" and City ='{0}' ", city);

                DataSet ds = m_dbo.GetDataSet(sql);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    Id = DBTool.GetIntFromRow(row, "Id", 0);
                    Name = DBTool.GetStringFromRow(row, "Name", "");
                    City = DBTool.GetStringFromRow(row, "City", "");
                    Address = DBTool.GetStringFromRow(row, "Address", "");
                    Telphone = DBTool.GetStringFromRow(row, "Telphone", "");
                    LinkMan = DBTool.GetStringFromRow(row, "LinkMan", "");
                    Bank = DBTool.GetStringFromRow(row, "Bank", "");
                    Account = DBTool.GetStringFromRow(row, "Account", "");
                    Print_title = DBTool.GetStringFromRow(row, "Print_title", "");
                    Complain_hotline = DBTool.GetStringFromRow(row, "Complain_hotline", "");
                    Province = DBTool.GetStringFromRow(row, "Province", "");
                    IsUnable = DBTool.GetIntFromRow(row, "IsUnable", 0);
                    ProvinceSort = DBTool.GetIntFromRow(row, "ProvinceSort", 0);
                    CitySort = DBTool.GetIntFromRow(row, "CitySort", 0);
                    IsUseSiteGoods = DBTool.GetIntFromRow(row, "IsUseSiteGoods", 0);
                    CityId = DBTool.GetIntFromRow(row, "CityId", 0);
                    return true;
                }
                else return false;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 读取图片
        /// </summary>
        /// <returns></returns>
        public DataSet ReadPhotos(int topNum,string type)
        {
            string sql = " select ";
            if (topNum > 0)
            {
                sql += string.Format(" top {0} ", topNum);
            }
            sql += string.Format(" * from Sys_BranchPhoto where BranchId={0} and Type='{1}'  order by Sort", this.Id,type);

            return m_dbo.GetDataSet(sql);
        }
    }
    public class BranchOption
    {
        public string Name { get; set; }
        public int IsUnable { get; set; }
        public BranchOption()
        {
            Name = "";
            IsUnable = -1;
        }

    }
    public class Sys_BranchManager
    {
        private DBOperate m_dbo;
        public BranchOption option;

        public Sys_BranchManager()
        {
            option = new BranchOption();
            m_dbo = new DBOperate();
        }
        /// <summary>
        /// 原版显示省市区，调用地方太多，暂不修改--平台使用
        ///  调用：ERP FGoodsAllow.cs
        /// </summary>
        /// <returns></returns>
        public DataSet ReadBranch()
        {
            string sql = "select b.*,p.Code,p.level from Sys_Branch b join Province p on b.CityId = p.Id  where 1=1 ";
            if (option.IsUnable > -1)
            {
                sql += string.Format(" and IsUnable = {0} ", option.IsUnable);
            }
            if (option.Name != "")
            {
                sql += string.Format(" and b.Name like '%{0}%' ", option.Name);
            }
            sql += " order by Code,CitySort,City ";
            return m_dbo.GetDataSet(sql);
        }
        //显示市--网站使用
        public DataSet ReadBranchCity(string code)
        {
            string sql = @"select distinct city,p.Code,p.level,p.previouslevel from Sys_Branch b join Province p on b.CityId = p.Id  where 1=1 and level =2 and IsUnable=1";
            sql += " and code like '" + code + "%'  union all select Name,Code,level,previouslevel from Province where level =2 and previouslevel=1 and code like '"+code+"%' ";
            return m_dbo.GetDataSet(sql);
        }
        //显示区--网站使用
        public DataSet ReadBranchCountys(string code)
        {
            string sql = "select distinct city,b.*,p.Code,p.level from Sys_Branch b join Province p on b.CityId = p.Id  where 1=1 and level=3 and IsUnable=1"; 
            sql += " and code like '" + code + "%' order by Code,CitySort,b.City ";
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 通过选择的地址放到sys_branch表里找到对应的branchId
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="areaId"></param>
        /// <returns></returns>
        public int GetBranchId(int cityId, int areaId)
        {
            int branchId = 0;
            string sql = string.Format("select Id from Sys_Branch where CityId in ('{0}','{1}') and IsUnable=1 order by CityId desc", cityId, areaId);
            DataTable dt = m_dbo.GetDataSet(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                branchId = Convert.ToInt32(dt.Rows[0]["Id"]);
            }
            return branchId;
        }
    }
}

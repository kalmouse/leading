using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using LeadingClass;
using CommenClass;

namespace LeadingClass
{
    public class ProvinceOption
    {
        private int m_level;
        private string m_PCode;
        private bool m_AllSubs;
        public int level { get { return m_level; } set { m_level = value; } }
        public string PCode { get { return m_PCode; } set { m_PCode = value; } }
        public bool AllSubs { get { return m_AllSubs; } set { m_AllSubs = value; } }
        public ProvinceOption()
        {
            m_AllSubs = false;
            m_level = 0;
            m_PCode = "";
        }
    }
    public class Province
    {
        private DBOperate m_dbo;
        private ProvinceOption m_option;
        public int Id { get; set; }
        public string Code { get; set; }
        public string PCode { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public int previouslevel { get; set; }

        public ProvinceOption option { get { return m_option; } set { m_option = value; } }
        public Province()
        {
            m_dbo = new DBOperate();
            m_option = new ProvinceOption();
        }
        public bool Load()
        {
            string sql = string.Format("select * from Province where Id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count == 1)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                Name = DBTool.GetStringFromRow(row, "Name", "");
                Code = DBTool.GetStringFromRow(row, "Code", "");
                PCode = DBTool.GetStringFromRow(row, "PCode", "");
                Level = DBTool.GetIntFromRow(row, "Level", 0);
                previouslevel = DBTool.GetIntFromRow(row, "Level", 0);
                return true;
            }
            else return false;
        }        
        public DataSet ReadProvince()
        {
            string sql = " select * from Province where 1=1 ";
            if (option.level != 0)
            {
                sql += string.Format(" and level={0}", option.level);
            }
            if (option.PCode != "")
            {
                if (option.AllSubs)
                {
                    sql += string.Format(" and PCode like '{0}%' ", option.PCode);
                }              
                else sql += string.Format(" and PCode = '{0}' ", option.PCode);
            }
            sql += string.Format("order by Code");
            return m_dbo.GetDataSet(sql);
        }
        public string GetCodeByName(string Province)
        {
            string sql = string.Format("select Code from  Province where Name='{0}'", Province);
            DataRowCollection rows = m_dbo.GetDataSet(sql).Tables[0].Rows;
            if (rows.Count > 0)
            {
                DataRow row = rows[0];
                Code = DBTool.GetStringFromRow(row, "Code", "");
            }
            return Code;
        }
        public string GetNameByCode(string Code)
        {
            string sql = string.Format("select Name from  Province where Code='{0}'", Code);
            DataRowCollection rows = m_dbo.GetDataSet(sql).Tables[0].Rows;
            if (rows.Count > 0)
            {
                DataRow row = rows[0];
                Name = DBTool.GetStringFromRow(row, "Name", "");
            }
            return Name;
        }

        //通过sys_branch表cityId获取省市县
        public DataSet GetIdCity(int Id)
        {
            string sql = string.Format(@"declare @Code varchar(50);
                                  select @Code=Code from Province where Id={0}
                                  if(len(@Code)>4)
                                  select (select Name from Province where Code=Substring(@Code, 0, 3)) as Province , (select Name from Province where Code=Substring(@Code, 0, 5)) as City ,                                    (select Name from Province where Code=@Code ) as Area from Province where Id={0}
                                  else
                                  select (select Name from Province where Code=Substring(@Code, 0, 3)) as Province , (select Name from Province where Code=Substring(@Code, 0, 5)) as City                                      from Province where Id={0}", Id);
            return m_dbo.GetDataSet(sql);
        }
        //获取全国30个省 update by hjy 2016-09-26
        public DataSet showProvince()
        {
            string sql = @"with showProvince as
                    (
                    select distinct b.Province from Sys_Branch b join Province p on b.CityId = p.Id  where 1=1 and IsUnable=1
                    )
                    select Code,Name,level,previouslevel from Province p join showProvince pp on level=1 and p.Name=pp.Province";
            return m_dbo.GetDataSet(sql);
        }
    }
}

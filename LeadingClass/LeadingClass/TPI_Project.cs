using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace LeadingClass
{
    public class TPI_Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime UpdateTime { get; set; }
        public int IsUseTypeCompare { get; set; }
        public int IsUseBrandCompare { get; set; }
        public int BranchId { get; set; }
        public string Developers { get; set; }
        public int PortNumber { get; set; }
        public string Note { get; set; }
        public string Url { get; set; }
        public int PushType { get; set; }
        public int IsDiscount { get; set; }
        private DBOperate m_dbo;

        public TPI_Project()
        {
            Id = 0;
            Name = "";
            UpdateTime = DateTime.Now;
            IsUseTypeCompare = 0;
            IsUseBrandCompare = 0;
            BranchId = 1;
            Developers = "";
            PortNumber = 0;
            Note = "";
            Url = "";
            PushType = 0;
            IsDiscount = 0;
            m_dbo = new DBOperate();
        }
        public TPI_Project(int Id)
        {
            m_dbo = new DBOperate();
            this.Id = Id;
            this.Load();
        }
        /// <summary>
        /// 通过ProjectId获取项目信息
        /// </summary>
        /// <returns>bool 返回单条记录的对象</returns>
        public bool Load()
        {
            string sql = string.Format("select * from TPI_Project where Id={0}",Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                Name = DBTool.GetStringFromRow(row, "Name", "");
                UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                IsUseTypeCompare = DBTool.GetIntFromRow(row, "IsUseTypeCompare", 0);
                IsUseBrandCompare = DBTool.GetIntFromRow(row, "IsUseBrandCompare", 0);
                BranchId = DBTool.GetIntFromRow(row, "BranchId", 0);

                Developers = DBTool.GetStringFromRow(row, "Developers", "");
                PortNumber = DBTool.GetIntFromRow(row, "PortNumber", 0);
                Note = DBTool.GetStringFromRow(row, "Note", "");
                Url = DBTool.GetStringFromRow(row, "Url", "");
                PushType = DBTool.GetIntFromRow(row, "PushType", 0);
                IsDiscount = DBTool.GetIntFromRow(row, "IsDiscount", 0);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 通过ProjectId获取项目信息
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable LoadById()
        {
            string sql = string.Format("select * from TPI_Project where id={0}", Id);
            return m_dbo.GetDataSet(sql).Tables[0];
        }
        
        /// <summary>
        /// 读取自己可用的项目
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataSet ReadMyProject(int userId)
        {
            string sql = string.Format("select * from dbo.View_TPIAuthority where UserId={0}", userId);
            return m_dbo.GetDataSet(sql);

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data.SqlClient;
using System.Data;
namespace LeadingClass
{
    /// <summary>
    /// GoodsParam备份
    /// </summary>
    //public class GoodsParam
    //{
    //    public int Id { get; set; }
    //    public int TypeId { get; set; }
    //    public string Name { get; set; }
    //    public int Sort { get; set; }
    //    public int IsGroup { get; set; }
    //    private DBOperate m_dbo;

    //    public GoodsParam()
    //    {
    //        Id = 0;
    //        TypeId = 0;
    //        Name = "";
    //        Sort = 0;
    //        IsGroup = 0;
    //        m_dbo = new DBOperate();
    //    }
    //    public GoodsParam(int Id)
    //        : this()
    //    {
    //        this.Id = Id;
    //        this.Load();
    //    }
    //    public int Save()
    //    {
    //        ArrayList arrayList = new ArrayList();
    //        if (Id > 0)
    //        {
    //            arrayList.Add(new SqlParameter("@Id", Id));
    //        }
    //        arrayList.Add(new SqlParameter("@TypeId", TypeId));
    //        arrayList.Add(new SqlParameter("@Name", Name));
    //        arrayList.Add(new SqlParameter("@Sort", Sort));
    //        arrayList.Add(new SqlParameter("@IsGroup", IsGroup));
    //        if (this.Id > 0)
    //        {
    //            m_dbo.UpdateData("GoodsParam", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
    //        }
    //        else
    //        {
    //            this.Id = m_dbo.InsertData("GoodsParam", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
    //        }
    //        return this.Id;
    //    }
    //    public bool Load()
    //    {
    //        string sql = string.Format("select * from GoodsParam where id={0}", Id);
    //        DataSet ds = m_dbo.GetDataSet(sql);
    //        if (ds.Tables[0].Rows.Count == 1)
    //        {
    //            DataRow row = ds.Tables[0].Rows[0];
    //            Id = DBTool.GetIntFromRow(row, "Id", 0);
    //            TypeId = DBTool.GetIntFromRow(row, "TypeId", 0);
    //            Name = DBTool.GetStringFromRow(row, "Name", "");
    //            Sort = DBTool.GetIntFromRow(row, "Sort", 0);
    //            IsGroup = DBTool.GetIntFromRow(row, "IsGroup", 0);
    //            return true;
    //        }
    //        return false;
    //    }
    //    public bool Delete()
    //    {
    //        string sql = string.Format("Delete from GoodsParam where id={0} ", Id);
    //        return m_dbo.ExecuteNonQuery(sql);
    //    }

    //    public DataSet ReadGoodsParam(int TypeId)
    //    {
    //        string sql = string.Format("select * from GoodsParam where typeId={0} order by Sort", TypeId);
    //        return m_dbo.GetDataSet(sql);
    //    }

    //    /// <summary>
    //    /// 批量导入数据
    //    /// </summary>
    //    /// <param name="TypeId"></param>
    //    /// <param name="dtParams"></param>
    //    public void SaveTypeParams(int TypeId, DataTable dtParams)
    //    {
    //        string sql = string.Format(" delete from GoodsParam where typeId={0} ", TypeId);
    //        if (m_dbo.ExecuteNonQuery(sql))
    //        {
    //            foreach (DataRow row in dtParams.Rows)
    //            {
    //                GoodsParam gp = new GoodsParam();
    //                gp.IsGroup = DBTool.GetIntFromRow(row, "IsGroup", 0);
    //                gp.Name = DBTool.GetStringFromRow(row, "Name", "");
    //                gp.Sort = DBTool.GetIntFromRow(row, "Sort", 0);
    //                gp.TypeId = TypeId;
    //                gp.Save();
    //            }
    //        }
    //    }
    //}

    /// <summary>
    /// GoodsParam实体类
    /// 添加了GoodsParamGroupId和UserId和UpdateTime字段
    /// </summary>
    public class GoodsParam
    {
        public int Id { get; set; }
        public int TypeId { get; set; }
        public string Name { get; set; }
        public int Sort { get; set; }
        public int IsGroup { get; set; }
        public int GoodsParamGroupId { get; set; }
        public int UserId { get; set; }
        public DateTime UpdateTime { get; set; }
        private DBOperate m_dbo;

        public GoodsParam()
        {
            Id = 0;
            TypeId = 0;
            Name = "";
            Sort = 0;
            IsGroup = 0;
            GoodsParamGroupId = 0;
            UserId = 0;
            UpdateTime = DateTime.Now;
            m_dbo = new DBOperate();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", Id));
            }
            arrayList.Add(new SqlParameter("@TypeId", TypeId));
            arrayList.Add(new SqlParameter("@Name", Name));
            arrayList.Add(new SqlParameter("@Sort", Sort));
            arrayList.Add(new SqlParameter("@IsGroup", IsGroup));
            arrayList.Add(new SqlParameter("@GoodsParamGroupId", GoodsParamGroupId));
            arrayList.Add(new SqlParameter("@UserId", UserId));
            arrayList.Add(new SqlParameter("@UpdateTime", UpdateTime));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("GoodsParam", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("GoodsParam", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from GoodsParam where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                TypeId = DBTool.GetIntFromRow(row, "TypeId", 0);
                Name = DBTool.GetStringFromRow(row, "Name", "");
                Sort = DBTool.GetIntFromRow(row, "Sort", 0);
                IsGroup = DBTool.GetIntFromRow(row, "IsGroup", 0);
                GoodsParamGroupId = DBTool.GetIntFromRow(row, "GoodsParamGroupId", 0);
                UserId = DBTool.GetIntFromRow(row, "UserId", 0);
                UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                return true;
            }
            return false;
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from GoodsParam where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }
        /// <summary>
        /// 调用：ERP：GoodsParam.cs
        /// ERP:FGoodsParam.cs
        /// </summary>
        /// <param name="TypeId"></param>
        /// <returns></returns>
        public DataSet ReadGoodsParam(int TypeId)
        {
            string sql = string.Format("select * from GoodsParam where typeId={0} order by Sort", TypeId);
            return m_dbo.GetDataSet(sql);
        }

        /// <summary>
        /// 批量导入数据
        /// </summary>
        /// <param name="TypeId"></param>
        /// <param name="dtParams"></param>
        public void SaveTypeParams(int TypeId, DataTable dtParams)
        {
            string sql = string.Format(" delete from GoodsParam where typeId={0} ", TypeId);
            if (m_dbo.ExecuteNonQuery(sql))
            {
                foreach (DataRow row in dtParams.Rows)
                {
                    GoodsParam gp = new GoodsParam();
                    gp.IsGroup = DBTool.GetIntFromRow(row, "IsGroup", 0);
                    gp.Name = DBTool.GetStringFromRow(row, "Name", "");
                    gp.Sort = DBTool.GetIntFromRow(row, "Sort", 0);
                    gp.TypeId = TypeId;
                    gp.Save();
                }
            }
        }

        /// <summary>
        /// 删除Goodsparam
        /// </summary>
        /// <param name="GoodsParamId"></param>
        /// <returns></returns>
        public bool DeleteGoodsParam(int GoodsParamId)
        {
            GoodsParam gp = new GoodsParam();
            gp.Id = GoodsParamId;
            return gp.Delete();
        }
    }
}

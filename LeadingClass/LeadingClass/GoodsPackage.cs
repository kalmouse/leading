using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom;
using System.Collections;
using System.Data.SqlClient;
using System.Data;

namespace LeadingClass
{

    public class GoodsPackage
    {
        public int Id { get; set; }
        public int GoodsId { get; set; }
        public string Type { get; set; }
        public int Num { get; set; }
        public string Name { get; set; }
        public int IsDefault { get; set; }
        public string BarCode { get; set; }
        public int BarCodeUserId { get; set; }
        public DateTime UpdateTime { get; set; }
        private DBOperate m_dbo;

        public GoodsPackage()
        {
            Id = 0;
            GoodsId = 0;
            Type = "";
            Num = 0;
            Name = "";
            IsDefault = 0;
            BarCode = "";
            BarCodeUserId = 0;
            UpdateTime = DateTime.Now;
            m_dbo = new DBOperate();
        }
        /// <summary>
        /// 调用: ERP:FGoods.cs
        /// </summary>
        /// <returns></returns>
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", Id));
            }
            arrayList.Add(new SqlParameter("@GoodsId", GoodsId));
            arrayList.Add(new SqlParameter("@Type", Type));
            arrayList.Add(new SqlParameter("@Num", Num));
            arrayList.Add(new SqlParameter("@Name", Name));
            arrayList.Add(new SqlParameter("@IsDefault", IsDefault));
            arrayList.Add(new SqlParameter("@BarCode", BarCode));
            arrayList.Add(new SqlParameter("@BarCodeUserId",BarCodeUserId));
            arrayList.Add(new SqlParameter("@UpdateTime", UpdateTime));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("GoodsPackage", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("GoodsPackage", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;

        }
        /// <summary>
        /// ERP:FGoods.cs
        /// </summary>
        /// <param name="GoodsId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool Load(int GoodsId,string type)
        {
            string sql = string.Format("select * from GoodsPackage where goodsId={0} and Type='{1}' ", GoodsId, type);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count==1)
            {
                DataRow row = ds.Tables[0].Rows[0];
                LoadFromRow(row);
                return true;
            }
            return false;
        }
        public bool Load(string BarCode)
        {
            string sql = string.Format("select * from GoodsPackage where BarCode='{0}' ", BarCode);
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
        /// 仅用于扫码开单
        /// </summary>
        /// <param name="BarCode"></param>
        /// <returns></returns>
        public DataSet GoodsInfo(string BarCode, int BranchId)
        {
            string sql = string.Format(@"select ID,DisplayName,Unit,model,Price,Num,(select SUM(Num) from GoodsStore where StoreId=1 and GoodsId =gp.GoodsId group by GoodsId) as StoreNum,ParentId,(Num*Price) as Amount 
                                       from Goods  
                                        join 
                                       (select GoodsId,Num from GoodsPackage where BarCode='{0}')
                                        gp on gp.GoodsId=Goods.ID
                                        where  (IsPublic=1 or ( IsPublic=0 and BranchId={1}))  and IsVisible>-1 and ParentId<>2 and IsShelves=1", BarCode, BranchId);
            DataSet ds = m_dbo.GetDataSet(sql);
            return ds;
        }
        public void LoadFromRow(DataRow row)
        {
            Id = DBTool.GetIntFromRow(row, "Id", 0);
            GoodsId = DBTool.GetIntFromRow(row, "GoodsId", 0);
            Type = DBTool.GetStringFromRow(row, "Type", "");
            Num = DBTool.GetIntFromRow(row, "Num", 0);
            Name = DBTool.GetStringFromRow(row, "Name", "");
            IsDefault = DBTool.GetIntFromRow(row, "IsDefault", 0);
            BarCode = DBTool.GetStringFromRow(row, "BarCode", "");
            BarCodeUserId = DBTool.GetIntFromRow(row, "BarCodeUserId", 0);
            UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
        }

        public bool Delete(int GoodsId,string type)
        {
            string sql = string.Format("Delete from GoodsPackage  where goodsId={0} and Type='{1}' ", GoodsId, type);
            return m_dbo.ExecuteNonQuery(sql);
        }

        public bool SaveBarCode(string BarCode)
        {
            this.BarCode = BarCode;
            if (this.Save() > 0)
                return true;
            else return false;
        }
        
    }

    public class Goodspackagelog
    {
        public int Id { get; set; }
        public int GoodsId { get; set; }
        public string Type { get; set; }
        public int OldNum { get; set; }
        public int Num { get; set; }
        public string OldName { get; set; }
        public string Name { get; set; }
        public int IsDefault { get; set; }
        public string OldBarCode { get; set; }
        public string BarCode { get; set; }
        public int UserId { get; set; }
        public DateTime UpdateTime { get; set; }
        private DBOperate m_dbo;

        public Goodspackagelog()
        {
            Id = 0;
            GoodsId = 0;
            Type = "";
            OldNum = 0;
            Num = 0;
            OldName = "";
            Name = "";
            IsDefault = 0;
            OldBarCode = "";
            BarCode = "";
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
            arrayList.Add(new SqlParameter("@GoodsId", GoodsId));
            arrayList.Add(new SqlParameter("@Type", Type));
            arrayList.Add(new SqlParameter("@OldNum", OldNum));
            arrayList.Add(new SqlParameter("@Num", Num));
            arrayList.Add(new SqlParameter("@OldName", OldName));
            arrayList.Add(new SqlParameter("@Name", Name));
            arrayList.Add(new SqlParameter("@IsDefault", IsDefault));
            arrayList.Add(new SqlParameter("@OldBarCode", OldBarCode));
            arrayList.Add(new SqlParameter("@BarCode", BarCode));
            arrayList.Add(new SqlParameter("@UserId", UserId));
            arrayList.Add(new SqlParameter("@UpdateTime", UpdateTime));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("GoodsPackageLog", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("GoodsPackageLog", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from GoodsPackageLog where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                GoodsId = DBTool.GetIntFromRow(row, "GoodsId", 0);
                Type = DBTool.GetStringFromRow(row, "Type", "");
                OldNum = DBTool.GetIntFromRow(row, "OldNum", 0);
                Num = DBTool.GetIntFromRow(row, "Num", 0);
                OldName = DBTool.GetStringFromRow(row, "OldName", "");
                Name = DBTool.GetStringFromRow(row, "Name", "");
                IsDefault = DBTool.GetIntFromRow(row, "IsDefault", 0);
                OldBarCode = DBTool.GetStringFromRow(row, "OldBarCode", "");
                BarCode = DBTool.GetStringFromRow(row, "BarCode", "");
                UserId = DBTool.GetIntFromRow(row, "UserId", 0);
                UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                return true;
            }
            return false;
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from GoodsPackageLog where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }
    }
}


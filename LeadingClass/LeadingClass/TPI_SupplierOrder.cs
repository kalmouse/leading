using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LeadingClass
{
    public class TPI_Supplierorder
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string RealName { get; set; }
        public string Address { get; set; }
        public string DetailStreet { get; set; }
        public string ZipCode { get; set; }
        public string Telephone { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Memo { get; set; }
        public double Amount { get; set; }
        public double ShipFree { get; set; }
        public string GoodsList { get; set; }
        public DateTime UpdateTime { get; set; }
        public int UsersId { get; set; }
       // public int Distribution { get; set; }
        private DBOperate m_dbo;

        public TPI_Supplierorder()
        {
            Id = 0;
            OrderId = 0;
            RealName = "";
            Address = "";
            DetailStreet = "";
            ZipCode = "";
            Telephone = "";
            Mobile = "";
            Email = "";
            Memo = "";
            Amount = 0;
            ShipFree = 0;
            GoodsList = "";
            UpdateTime = DateTime.Now;
            UsersId = 0;
           // Distribution = 0;
            m_dbo = new DBOperate();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", Id));
            }
            arrayList.Add(new SqlParameter("@OrderId", OrderId));
            arrayList.Add(new SqlParameter("@RealName", RealName));
            arrayList.Add(new SqlParameter("@Address", Address));
            arrayList.Add(new SqlParameter("@DetailStreet", DetailStreet));
            arrayList.Add(new SqlParameter("@ZipCode", ZipCode));
            arrayList.Add(new SqlParameter("@Telephone", Telephone));
            arrayList.Add(new SqlParameter("@Mobile", Mobile));
            arrayList.Add(new SqlParameter("@Email", Email));
            arrayList.Add(new SqlParameter("@Memo", Memo));
            arrayList.Add(new SqlParameter("@Amount", Amount));
            arrayList.Add(new SqlParameter("@ShipFree", ShipFree));
            arrayList.Add(new SqlParameter("@GoodsList", GoodsList));
            arrayList.Add(new SqlParameter("@UpdateTime", UpdateTime));
            arrayList.Add(new SqlParameter("@UsersId", UsersId));
            //arrayList.Add(new SqlParameter("@Distribution", Distribution));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("TPI_SupplierOrder", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("TPI_SupplierOrder", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from TPI_SupplierOrder where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                OrderId = DBTool.GetIntFromRow(row, "OrderId", 0);
                RealName = DBTool.GetStringFromRow(row, "RealName", "");
                Address = DBTool.GetStringFromRow(row, "Address", "");
                DetailStreet = DBTool.GetStringFromRow(row, "DetailStreet", "");
                ZipCode = DBTool.GetStringFromRow(row, "ZipCode", "");
                Telephone = DBTool.GetStringFromRow(row, "Telephone", "");
                Mobile = DBTool.GetStringFromRow(row, "Mobile", "");
                Email = DBTool.GetStringFromRow(row, "Email", "");
                Memo = DBTool.GetStringFromRow(row, "Memo", "");
                Amount = DBTool.GetDoubleFromRow(row, "Amount", 0);
                ShipFree = DBTool.GetDoubleFromRow(row, "ShipFree", 0);
                GoodsList = DBTool.GetStringFromRow(row, "GoodsList", "");
                UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                UsersId = DBTool.GetIntFromRow(row, "UsersId", 0);
                //Distribution = DBTool.GetIntFromRow(row, "Distribution", 0);
                return true;
            }
            return false;
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from TPI_SupplierOrder where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }
        //
        public DataSet ReadTPI_SupplierOrder()
        {
            string sql = string.Format("select * from TPI_SupplierOrder where OrderId={0}", OrderId);
            return m_dbo.GetDataSet(sql);

        }
        /// <summary>
        /// 读取天津的订单
        /// </summary>
        /// <returns></returns>
        public DataSet ReadOrder()
        {
            string sql = string.Format("select * from [Order] where TPI_OrderId in(select Id from TPI_Order where ProjectId=12)");
            return m_dbo.GetDataSet(sql);
        }
    }
}

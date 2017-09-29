using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data.SqlClient;
using System.Data;

namespace LeadingClass
{
    public class Sys_Car
    {
        public int Id { get; set; }
        public int BranchId { get; set; }
        public string Plate { get; set; }
        public int IsEnable { get; set; }
        public string Memo { get; set; }
        public DateTime UpdateTime { get; set; }
        private DBOperate m_dbo;

        public Sys_Car()
        {
            Id = 0;
            BranchId = 0;
            Plate = "";
            IsEnable = 0;
            Memo = "";
            UpdateTime = DateTime.Now;
            m_dbo = new DBOperate();
        }
        public Sys_Car(int Id)
            : this()
        {
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
            arrayList.Add(new SqlParameter("@BranchId", BranchId));
            arrayList.Add(new SqlParameter("@Plate", Plate));
            arrayList.Add(new SqlParameter("@IsEnable", IsEnable));
            arrayList.Add(new SqlParameter("@Memo", Memo));
            arrayList.Add(new SqlParameter("@UpdateTime", UpdateTime));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("Sys_Car", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("Sys_Car", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        } 
        public Sys_Car(string plate)
            : this()
        {
            this.Load(plate);
        }

        public bool Load()
        {
            string sql = string.Format("select * from Sys_Car where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count == 1)
            {
                DataRow row = ds.Tables[0].Rows[0];
                LoadFromRow(row);
                return true;
            }
            return false;
        }

        public bool Load(string plate)
        {
            string sql = string.Format("select * from Sys_Car where Plate='{0}'", plate);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count == 1)
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
            BranchId = DBTool.GetIntFromRow(row, "BranchId", 0);
            Plate = DBTool.GetStringFromRow(row, "Plate", "");
            IsEnable = DBTool.GetIntFromRow(row, "IsEnable", 0);
            Memo = DBTool.GetStringFromRow(row, "Memo", "");
            UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
        }

        public bool Delete()
        {
            string sql = string.Format("Delete from Sys_Car where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }

        public bool Load(int branchId, string plate)
        {
            string sql = string.Format("select * from Sys_Car where BranchId={0}  and Plate='{1}'", branchId, plate);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count == 1)
            {
                DataRow row = ds.Tables[0].Rows[0];
                LoadFromRow(row);
                return true;
            }
            return false;
        }
    }

    public class Sys_CarLog
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public double Kil { get; set; }
        public double Oil { get; set; }
        public double OilMoney { get; set; }
        public int UserId { get; set; }
        public string Memo { get; set; }
        public DateTime UpdateTime { get; set; }
        private DBOperate m_dbo;

        public Sys_CarLog()
        {
            Id = 0;
            CarId = 0;
            Kil = 0;
            Oil = 0;
            OilMoney = 0;
            UserId = 0;
            Memo = "";
            UpdateTime = DateTime.Now;
            m_dbo = new DBOperate();
        }
        public Sys_CarLog(int Id)
            : this()
        {
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
            arrayList.Add(new SqlParameter("@CarId", CarId));
            arrayList.Add(new SqlParameter("@Kil", Kil));
            arrayList.Add(new SqlParameter("@Oil", Oil));
            arrayList.Add(new SqlParameter("@OilMoney", OilMoney));
            arrayList.Add(new SqlParameter("@UserId", UserId));
            arrayList.Add(new SqlParameter("@Memo", Memo));
            arrayList.Add(new SqlParameter("@UpdateTime", UpdateTime));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("Sys_CarLog", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("Sys_CarLog", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from Sys_CarLog where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count == 1)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                CarId = DBTool.GetIntFromRow(row, "CarId", 0);
                Kil = DBTool.GetDoubleFromRow(row, "Kil", 0);
                Oil = DBTool.GetDoubleFromRow(row, "Oil", 0);
                OilMoney = DBTool.GetDoubleFromRow(row, "OilMoney", 0);
                UserId = DBTool.GetIntFromRow(row, "UserId", 0);
                Memo = DBTool.GetStringFromRow(row, "Memo", "");
                UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                return true;
            }
            return false;
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from Sys_CarLog where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }
    }

    public class Sys_CarUser
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public int UserId { get; set; }
        public DateTime UpdateTime { get; set; }
        private DBOperate m_dbo;

        public Sys_CarUser()
        {
            Id = 0;
            CarId = 0;
            UserId = 0;
            UpdateTime = DateTime.Now;
            m_dbo = new DBOperate();
        }
        public Sys_CarUser(int Id)
            : this()
        {
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
            arrayList.Add(new SqlParameter("@CarId", CarId));
            arrayList.Add(new SqlParameter("@UserId", UserId));
            arrayList.Add(new SqlParameter("@UpdateTime", UpdateTime));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("Sys_CarUser", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("Sys_CarUser", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from Sys_CarUser where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count == 1)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                CarId = DBTool.GetIntFromRow(row, "CarId", 0);
                UserId = DBTool.GetIntFromRow(row, "UserId", 0);
                UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                return true;
            }
            return false;
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from Sys_CarUser where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }
    }

    public class Sys_CarManager
    {
        private DBOperate m_dbo;

        public Sys_CarManager()
        {
            m_dbo = new DBOperate();
        }

        /// <summary>
        /// 更换车辆
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="carId"></param>
        /// <returns></returns>
        public bool ChangeCar(int userId, int carId)
        {
            Sys_CarUser carUser = new Sys_CarUser();
            carUser.CarId = carId;
            carUser.UserId = userId;
            if (carUser.Save() > 0)
            {
                return true;
            }
            else return false;
        }

        /// <summary>
        /// 更换车辆
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="carId"></param>
        /// <returns></returns>
        public bool ChangeCar(int userId, string CarPlate)
        {
            Sys_Car car = new Sys_Car(CarPlate);
            return ChangeCar(userId, car.Id);
        }

        /// <summary>
        /// 获取车辆当前驾驶员
        /// </summary>
        /// <param name="CarId"></param>
        /// <returns></returns>
        public Sys_Users GetCarUser(int CarId)
        {
            string sql = string.Format(" select top 1 * from sys_caruser where CarId={0} order by updatetime desc", CarId);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count == 1)
            {
                Sys_Users su = new Sys_Users(DBTool.GetIntFromRow(ds.Tables[0].Rows[0], "UserId", 0));
                return su;
            }
            else return null;
        }
        /// <summary>
        /// 获取车辆当前驾驶员
        /// </summary>
        /// <param name="plate"></param>
        /// <returns></returns>
        public Sys_Users GetCarUser(string plate)
        {
            Sys_Car car = new Sys_Car(plate);
            return GetCarUser(car.Id);
        }
        /// <summary>
        /// 获取车辆特定时间的驾驶员
        /// </summary>
        /// <param name="plate"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public Sys_Users GetCarUser(string plate, DateTime date)
        {
            Sys_Car car = new Sys_Car(plate);
            string sql = string.Format(" select top 1 * from sys_caruser where CarId={0} and updatetime<'{1}' order by updatetime desc", car.Id, date);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count == 1)
            {
                Sys_Users su = new Sys_Users(DBTool.GetIntFromRow(ds.Tables[0].Rows[0], "UserId", 0));
                return su;
            }
            else return null;

        }
    }
}

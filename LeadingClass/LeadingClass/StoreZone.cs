using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

using CommenClass;

namespace LeadingClass
{
    public class StoreZone
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string PCode { get; set; }
        public string Memo { get; set; }
        public DateTime AddTime { get; set; }
        public DateTime UpdateTime { get; set; }
        private DBOperate m_dbo;

        public StoreZone()
        {
            Id = 0;
            StoreId = 0;
            Type = "";
            Name = "";
            Code = "";
            PCode = "";
            Memo = "";
            AddTime = DateTime.Now;
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
            arrayList.Add(new SqlParameter("@StoreId", StoreId));
            arrayList.Add(new SqlParameter("@Type", Type));
            arrayList.Add(new SqlParameter("@Name", Name));
            arrayList.Add(new SqlParameter("@Code", Code));
            arrayList.Add(new SqlParameter("@PCode", PCode));
            arrayList.Add(new SqlParameter("@Memo", Memo));
            arrayList.Add(new SqlParameter("@AddTime", AddTime));
            arrayList.Add(new SqlParameter("@UpdateTime", UpdateTime));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("StoreZone", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("StoreZone", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from StoreZone where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count ==1)
            {
                LoadFromRow(ds.Tables[0].Rows[0]);
                return true;
            }
            return false;
        }
        public bool Load(string code)
        {
            string sql = string.Format("select * from StoreZone where Code='{0}' ", code);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count == 1)
            {
                LoadFromRow(ds.Tables[0].Rows[0]);
                return true;
            }
            return false;
        }

        private void LoadFromRow(DataRow row)
        {
            Id = DBTool.GetIntFromRow(row, "Id", 0);
            StoreId = DBTool.GetIntFromRow(row, "StoreId", 0);
            Type = DBTool.GetStringFromRow(row, "Type", "");
            Name = DBTool.GetStringFromRow(row, "Name", "");
            Code = DBTool.GetStringFromRow(row, "Code", "");
            PCode = DBTool.GetStringFromRow(row, "PCode", "");
            Memo = DBTool.GetStringFromRow(row, "Memo", "");
            AddTime = DBTool.GetDateTimeFromRow(row, "AddTime");
            UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from StoreZone where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 读取全部子区位
        /// </summary>
        /// <returns></returns>
        public DataSet ReadSubZone(string zonetype)
        {
            string sql = "select * from storezone where 1=1 ";
            sql += string.Format(" and StoreId={0}", this.StoreId);
            if (this.Code != "")
            {
                sql += string.Format("  and Code like '{0}-%' ", this.Code);
            }

            sql += string.Format(" and Type = '{0}' ", zonetype);
            sql += " order by Code ";
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 创建子区域
        /// </summary>
        /// <returns></returns>
        public int CreateSubZone(int maxNum)
        {
            int OK = 0;
            string type = GetSubType();
            for (int i = 1; i <= maxNum; i++)
            {
                StoreZone sz = new StoreZone();
                sz.StoreId = this.StoreId;
                sz.Type = type;
                sz.Name = this.Name + "-" + i.ToString();
                sz.Code = this.Code + "-" + i.ToString();
                sz.PCode = this.Code;
                if (sz.Save() > 0)
                    OK += 1;
            }
            return OK;
        }

        private string GetSubType()
        {
            switch (this.Type)
            {
                case "区":
                    return StoreZoneType.排.ToString();
                case "排":
                    return StoreZoneType.架.ToString();
                case "架":
                    return StoreZoneType.层.ToString();
                case "层":
                    return StoreZoneType.位.ToString();
                default:
                    return "";
            }
        }

        /// <summary>
        /// 读取货位上的商品
        /// </summary>
        /// <returns></returns>
        public DataSet ReadGoods()
        {
            string sql = "";
            if (this.Type == CommenClass.StoreZoneType.位.ToString())
            {
                sql = string.Format(" select * from View_GoodsStore where StoreZone ='{0}' ", this.Code);
            }
            else
             sql = string.Format(" select * from View_GoodsStore where StoreZone like '{0}-%' ", this.Code);
            return m_dbo.GetDataSet(sql);
        }
    }

    public class ZoneOption
    {
        public int RowNum { get; set; }
        public int SheelfNum { get; set; }
        public int LayerNum { get; set; }
        public int PositionNum { get; set; }

        public ZoneOption()
        {
            RowNum = 0;
            SheelfNum = 0;
            LayerNum = 0;
            PositionNum = 0;
        }
    }

    public class StoreZoneManager
    {
        private DBOperate m_dbo;
        public StoreZoneManager()
        {
            m_dbo = new DBOperate();
        }
        /// <summary>
        /// 给仓库创建大区
        /// </summary>
        /// <param name="StoreId"></param>
        /// <param name="NewZoneNum"></param>
        /// <returns>返回生成的大区数量</returns>
        public int CreateStoreZone(int StoreId, int NewZoneNum)
        {
            int curZoneNum = ReadStoreZone(StoreId).Tables[0].Rows.Count;
            LetterNumber ln = new LetterNumber(curZoneNum);
            List<string> newZone = ln.Add(NewZoneNum);
            int OK = 0;
            for (int i = 0; i < newZone.Count; i++)
            {
                //生成新的大区
                StoreZone sz = new StoreZone();
                sz.StoreId = StoreId;
                sz.Code = StoreId + newZone[i];
                sz.Name = newZone[i];
                sz.Type = CommenClass.StoreZoneType.区.ToString();
                if (sz.Save() > 0)
                    OK += 1;
            }
            return OK;
        }

         /// <summary>
        /// 读取仓库大区
        /// </summary>
        /// <returns></returns>
        public DataSet ReadStoreZone(int StoreId)
        {
            string sql = string.Format("select * from storezone where  StoreId={0} and Type='{1}' order by Code", StoreId, CommenClass.StoreZoneType.区.ToString());
            return m_dbo.GetDataSet(sql);
        }

        /// <summary>
        /// 给大区创建 库位码
        /// </summary>
        /// <returns></returns>
        public int CreateStoreZoneCode(StoreZone bigZone,ZoneOption option)
        {
            if (bigZone.Type != CommenClass.StoreZoneType.区.ToString())
            {
                return 0;
            }
            int num = 0;
            if (option.RowNum > 0)
            {
                bigZone.CreateSubZone(option.RowNum);//创建排
            }
            if (option.SheelfNum > 0)//需要创建 架
            {
                DataSet ds = bigZone.ReadSubZone(CommenClass.StoreZoneType.排.ToString());//读取排
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    StoreZone zone = new StoreZone();
                    zone.Id = DBTool.GetIntFromRow(row, "Id", 0);
                    zone.Load();
                    zone.CreateSubZone(option.SheelfNum);//创建架
                }
            }
            if (option.LayerNum > 0)//需要创建层
            {
                DataSet ds = bigZone.ReadSubZone(CommenClass.StoreZoneType.架.ToString());//读取架
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    StoreZone zone = new StoreZone();
                    zone.Id = DBTool.GetIntFromRow(row, "Id", 0);
                    zone.Load();
                    zone.CreateSubZone(option.LayerNum);//创建架
                }
            }
            if (option.PositionNum > 0)//创建位
            {
                DataSet ds = bigZone.ReadSubZone(CommenClass.StoreZoneType.层.ToString());//读取层
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    StoreZone zone = new StoreZone();
                    zone.Id = DBTool.GetIntFromRow(row, "Id", 0);
                    zone.Load();
                    num = zone.CreateSubZone(option.PositionNum);//创建位
                }
            }
            return num;

        }
    }
}

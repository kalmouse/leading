using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data.SqlClient;
using System.Data;
using LeadingClass;

namespace LeadingClass
{
    public class MemberAddress
    {
        private int m_Id;
        private int m_MemberId;
        private string m_RealName;
        private string m_Address;
        private string m_Province;
        private string m_City;
        private string m_Area;
        private string m_Street;
        private string m_Mansion;
        private string m_Room;
        private string m_TelPhone;
        private string m_Mobile;
        private string m_Email;
        private int m_IsDefault;
        private string m_IdCard;//身份证号  
        private DBOperate m_dbo;
        public int IsDefault { get { return m_IsDefault; } set { m_IsDefault = value; } }
        public int Id { get { return m_Id; } set { m_Id = value; } }
        public int MemberId { get { return m_MemberId; } set { m_MemberId = value; } }
        public string RealName { get { return m_RealName; } set { m_RealName = value; } }
        public string Address { get { return m_Address; } set { m_Address = value; } }
        public string Province { get { return m_Province; } set { m_Province = value; } }
        public string City { get { return m_City; } set { m_City = value; } }
        public string Area { get { return m_Area; } set { m_Area = value; } }
        public string Street { get { return m_Street; } set { m_Street = value; } }
        public string Mansion { get { return m_Mansion; } set { m_Mansion = value; } }
        public string Room { get { return m_Room; } set { m_Room = value; } }
        public string TelPhone { get { return m_TelPhone; } set { m_TelPhone = value; } }
        public string Mobile { get { return m_Mobile; } set { m_Mobile = value; } }
        public string Email { get { return m_Email; } set { m_Email = value; } }
        public string IdCard { get { return m_IdCard; } set { m_IdCard = value; } }
        public MemberAddress()
        {
            m_Id = 0;
            m_MemberId = 0;
            m_RealName = "";
            m_Address = "";
            m_Province = "";
            m_City = "";
            m_Area = "";
            m_Street = "";
            m_Mansion = "";
            m_Room = "";
            m_TelPhone = "";
            m_Mobile = "";
            m_Email = "";
            m_IdCard = "";
            m_dbo = new DBOperate();
            m_IsDefault = 0;
        }

        public MemberAddress(int Id)
        {
            m_dbo = new DBOperate();
            this.Id = Id;
            this.Load();
        }

        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (m_Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", m_Id));
            }
            arrayList.Add(new SqlParameter("@MemberId", m_MemberId));
            arrayList.Add(new SqlParameter("@RealName", m_RealName));
            arrayList.Add(new SqlParameter("@Address", GetAddress()));
            arrayList.Add(new SqlParameter("@Province", m_Province));
            arrayList.Add(new SqlParameter("@City", m_City));
            arrayList.Add(new SqlParameter("@Area", m_Area));
            arrayList.Add(new SqlParameter("@Street", m_Street));
            arrayList.Add(new SqlParameter("@Mansion", m_Mansion));
            arrayList.Add(new SqlParameter("@Room", m_Room));
            arrayList.Add(new SqlParameter("@TelPhone", m_TelPhone));
            arrayList.Add(new SqlParameter("@Mobile", m_Mobile));
            arrayList.Add(new SqlParameter("@Email", m_Email));
            arrayList.Add(new SqlParameter("@IsDefault", m_IsDefault));
            arrayList.Add(new SqlParameter("@IdCard", m_IdCard));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("MemberAddress", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("MemberAddress", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        /// <summary>
        /// 获取联系人地址
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
        public bool Load()
        {
            string sql = string.Format("select * from MemberAddress where id={0}", m_Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                m_Id = DBTool.GetIntFromRow(row, "Id", 0);
                m_MemberId = DBTool.GetIntFromRow(row, "MemberId", 0);
                m_RealName = DBTool.GetStringFromRow(row, "RealName", "");
                m_Address = DBTool.GetStringFromRow(row, "Address", "");
                m_Province = DBTool.GetStringFromRow(row, "Province", "");
                m_City = DBTool.GetStringFromRow(row, "City", "");
                m_Area = DBTool.GetStringFromRow(row, "Area", "");
                m_Street = DBTool.GetStringFromRow(row, "Street", "");
                m_Mansion = DBTool.GetStringFromRow(row, "Mansion", "");
                m_Room = DBTool.GetStringFromRow(row, "Room", "");
                m_TelPhone = DBTool.GetStringFromRow(row, "TelPhone", "");
                m_Mobile = DBTool.GetStringFromRow(row, "Mobile", "");
                m_Email = DBTool.GetStringFromRow(row, "Email", "");
                m_IsDefault = DBTool.GetIntFromRow(row, "IsDefault", 0);
                m_IdCard = DBTool.GetStringFromRow(row, "IdCard", "");
                return true;
            }
            return false;
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from MemberAddress where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }

        public bool SetDefaultAddress(int memberId, int Id)
        {
            string sql = string.Format(@" update MemberAddress set IsDefault=0 where MemberId={0};  
                                          update MemberAddress set IsDefault=1 where Id={1};", memberId, Id);
            return m_dbo.ExecuteNonQuery(sql);
        }
        /// <summary>
        /// 读取member的默认地址
        /// </summary>
        /// <param name="MemberId"></param>
        /// <returns></returns>
        public MemberAddress ReadDefault(int MemberId)
        {
            //MemberAddress ma = new MemberAddress();
            string sql = string.Format("select * from dbo.MemberAddress where MemberId ={0} and IsDefault=1", MemberId);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                this.Id = DBTool.GetIntFromRow(ds.Tables[0].Rows[0], "Id", 0);
                this.Load();
                return this;
            }
            else return null;
        }

        public DataSet ReadMemberAddressByMemberId()
        {
            string sql = string.Format("select * from dbo.MemberAddress where MemberId ={0} order by IsDefault desc,Id", m_MemberId);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count == 0)
            {
                InitMemberAddress(m_MemberId);
                ds = m_dbo.GetDataSet(sql);
            }
            return ds;

        }

        private void InitMemberAddress(int MemberId)
        {
            Member member = new Member();
            member.Id = MemberId;
            member.Load();
            MemberAddress Maddress = new MemberAddress();
            Maddress.MemberId = MemberId;
            Maddress.RealName = member.RealName;
            Maddress.Address = member.Address;
            Maddress.Province = member.Province;
            Maddress.City = member.City;
            Maddress.Area = member.Area;
            Maddress.Street = member.Street; ;
            Maddress.Mansion = member.Mansion;
            Maddress.Room = member.Room;
            Maddress.TelPhone = member.Telphone;
            Maddress.Mobile = member.Mobile;
            Maddress.Email = member.Email;
            Maddress.IsDefault = 1;
            Maddress.Save();
        }
        //用户修改个人信息时，获取MemberAddressId最小的地址
        public int GetMinMemberMinAddressId(int memberId)
        {
            string sql = string.Format("select Min(Id) as MinId from MemberAddress where MemberId={0}", memberId);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                return DBTool.GetIntFromRow(row, "MinId", 0);
            }
            return 0;
        }

    }

}

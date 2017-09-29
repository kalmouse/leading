using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
namespace LeadingClass
{
    public class MemberManager
    {
        private DBOperate m_dbo;

        public MemberManager()
        {
            m_dbo = new DBOperate();

        }
        public int SaveMemberAndDetail(Member m, MemberAddress[] ma)
        {
            int MemberId = 0;
            int result = 0;
            Member member = new Member();
            member.LoginName = m.LoginName;
            member.PassWord = m.PassWord;
            //
            MemberId = member.Save();
            if (MemberId > 0)
            {
                for (int i = 0; i < ma.Length; i++)
                {
                    MemberAddress memberAddress = new MemberAddress();
                    memberAddress.MemberId = ma[i].MemberId;
                    //
                    if (memberAddress.Save() > 0)
                    {
                        result += 1;
                    }
                }
            }
            return result;
        }
    }
}

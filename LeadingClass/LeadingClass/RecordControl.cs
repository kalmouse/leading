using System;
using System.Collections.Generic;
using System.Web;

namespace LeadingClass
{
    public class RecordControl
    {
        private int m_StartRecord;
        private int m_PageSize;

        public int StartRecord { get { return m_StartRecord; } set { m_StartRecord = value; } }
        public int PageSize { get { return m_PageSize; } set { m_PageSize = value; } }

        public RecordControl()
        {
            m_StartRecord = 0;
            m_PageSize = 20;
        }
    }
}
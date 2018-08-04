using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic
{
    public class ListItem
    {
        private int m_ID;
        private string m_Value;

        public ListItem(int id, string value)
        {
            m_ID = id;
            m_Value = value;

        }

        public ListItem()
        {
            m_ID = 0;
            m_Value = "";

        }

        public int ID
        {
            get
            {
                return m_ID;
            }
            set
            {
                m_ID = value;
            }
        }
        public string Value
        {
            get
            {
                return m_Value;
            }
            set
            {
                m_Value = value;
            }
        }

        public override string ToString()
        {
            return m_Value;
        }

    }
}


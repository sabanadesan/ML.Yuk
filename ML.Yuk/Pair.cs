using System;
using System.Collections.Generic;
using System.Text;

namespace ML.Yuk
{
    public class Pair
    {
        private KeyValuePair<String, dynamic> m_Pair;

        public Pair(String key, dynamic value)
        {
            m_Pair = new KeyValuePair<String, dynamic>(key, value);
        }

        public dynamic Value()
        {
            return m_Pair.Value;
        }

        public String Key()
        {
            return m_Pair.Key;
        }
    }
}

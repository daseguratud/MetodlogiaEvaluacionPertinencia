using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LifaeTools
{
    public class DataTools
    {
        public static DataTable GetDataTable<T>(List<T> comparedList)
        {
            DataTable dt = new DataTable();
            PropertyInfo[] propertiesInfo = typeof(T).GetProperties();
            foreach (PropertyInfo field in propertiesInfo)
            {
                dt.Columns.Add(field.Name);
            }

            foreach (var item in comparedList)
            {
                var values = propertiesInfo.Select(p => p.GetValue(item, null)).ToArray();
                dt.Rows.Add(values);
            }

            return dt;
        }
    }
}

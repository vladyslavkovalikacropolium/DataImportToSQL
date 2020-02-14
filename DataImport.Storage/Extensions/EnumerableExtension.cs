using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

namespace DataImport.Storage.Extensions
{
    public static class EnumerableExtension
    {
        public static DataTable ToDataTable<T>(this IEnumerable<T> data)
        {
            var props = TypeDescriptor.GetProperties(typeof(T));
            var table = new DataTable();

            foreach (PropertyDescriptor descriptor in props)
            {
                table.Columns.Add(descriptor.Name, descriptor.PropertyType);
            }
            
           
            var values = new object[props.Count];
            foreach (var item in data)
            {
                for (var i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;        
        }
    }
}
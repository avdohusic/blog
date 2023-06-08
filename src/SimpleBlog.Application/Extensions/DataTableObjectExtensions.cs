using System.ComponentModel;
using System.Data;

namespace SimpleBlog.Application.Extensions;
public static class DataTableObjectExtensions
{
    /// <summary>
    /// Converts a List of object to a datatable
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <param name="tableName"></param>
    /// <returns></returns>
    public static DataTable ToDataTable<T>(this IEnumerable<T> data, string tableName)
    {
        PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
        DataTable dt = new DataTable();
        for (int i = 0; i < properties.Count; i++)
        {
            PropertyDescriptor property = properties[i];
            dt.Columns.Add(property.DisplayName, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);
        }
        object[] values = new object[properties.Count];
        foreach (T item in data)
        {
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = properties[i].GetValue(item);
            }
            dt.Rows.Add(values);
        }
        dt.TableName = tableName;
        return dt;
    }
}

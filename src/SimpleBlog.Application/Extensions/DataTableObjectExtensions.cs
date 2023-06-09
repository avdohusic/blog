using OfficeOpenXml;
using SimpleBlog.Application.Common.Exceptions;
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
    /// <returns>DataTable</returns>
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

    /// <summary>
    /// Read from stream to List of T object
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="stream"></param>
    /// <param name="sheetName"></param>
    /// <returns>DataTable</returns>
    public static IEnumerable<T> ReadDataFromExcelFile<T>(this Stream stream, string sheetName)
    {
        using (ExcelPackage package = new ExcelPackage(stream))
        {
            ExcelWorksheet worksheet = package.Workbook.Worksheets[sheetName];

            if (worksheet == null)
            {
                throw new ResourceProhibitedException($"File doesn't contain '{sheetName}' sheet");
            }
            // Check if the worksheet is completely empty
            if (worksheet.Dimension == null)
            {
                throw new ResourceProhibitedException($"Sheet '{sheetName}' is empty");
            }

            List<T> list = new List<T>();
            //first row is for knowing the properties of object
            var columnInfo = Enumerable.Range(1, worksheet.Dimension.Columns).ToList().Select(n =>
                new { Index = n, ColumnName = worksheet.Cells[1, n].Value.ToString().ToLower().Replace(" ", "") }
            );

            for (int row = 2; row < worksheet.Dimension.Rows; row++)
            {
                T obj = (T)Activator.CreateInstance(typeof(T));//generic object
                foreach (var prop in typeof(T).GetProperties())
                {
                    int col = columnInfo.SingleOrDefault(c => c.ColumnName == prop.Name.ToLower()).Index;
                    var val = worksheet.Cells[row, col].Value;
                    var propType = prop.PropertyType;

                    if (propType == typeof(Guid?) || propType == typeof(Guid))
                    {
                        if (val != null)
                        {
                            prop.SetValue(obj, Guid.Parse(val.ToString()));
                        }
                    }
                    else
                    {
                        prop.SetValue(obj, Convert.ChangeType(val.ToString(), propType));
                    }
                }
                list.Add(obj);
            }

            return list;
        }
    }
}

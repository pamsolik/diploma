using System.Reflection;
using System.Text;

namespace Services.Extensions;

public static class CustomListExtensions
{
    /// <summary>
    /// Convert a list of Type T to a CSV
    /// </summary>
    /// <typeparam name="T">The type of the object held in the list</typeparam>
    /// <param name="items">The list of items to process</param>
    /// <param name="delimiter">Specify the delimiter, default is ,</param>
    /// <returns></returns>
    public static string ToCsv<T>(this List<T> items, string delimiter = ",")
    {
        Type itemType = typeof(T);
        var props = itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance).OrderBy(p => p.Name);

        var csv = new StringBuilder();

        // Write Headers
        csv.AppendLine(string.Join(delimiter, props.Select(p => p.Name)));

        // Write Rows
        foreach (var item in items)
        {
            // Write Fields
            csv.AppendLine(string.Join(delimiter, props.Select(p => GetCsvFieldasedOnValue(p, item))));
        }

        return csv.ToString();
    }

    /// <summary>
    /// Provide generic and specific handling of fields
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="p"></param>
    /// <param name="item"></param>
    /// <returns></returns>
    private static object GetCsvFieldasedOnValue<T>(PropertyInfo p, T item)
    {
        string? value = "";

        try
        {
            value = p.GetValue(item, null)?.ToString();
            if (value == null) return "NULL";  // Deal with nulls
            if (value.Trim().Length == 0) return ""; // Deal with spaces and blanks

            // Guard strings with "s, they may contain the delimiter!
            if (p.PropertyType == typeof(string))
            {
                value = string.Format("\"{0}\"", value);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return value;
    }
}

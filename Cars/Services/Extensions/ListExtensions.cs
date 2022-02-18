using System.Reflection;
using System.Text;
using Core.DataModels;
using Core.ViewModels;

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
    public static string ToCsv<T>(this List<T> items, string delimiter = ";")
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

    public static string ToCsv(this List<ProjectView> items, string delimiter = ";")
    {
        Type itemType = typeof(ProjectView);
        Type itemType2 = typeof(CodeQualityAssessment);
        var props = itemType.GetProperties();
        var subProps = itemType2.GetProperties().Where(p => !p.Name.Equals("Id"));
        var csv = new StringBuilder();

        csv.AppendLine(string.Join(delimiter, 
            props.Select(p => p.Name)
            .Union(subProps.Select(p => p.Name))
        ));

        foreach (var item in items)
        {
            var pro = props.Select(p => GetCsvFieldasedOnValue(p, item));
            var sub = subProps.Select(p => GetCsvFieldasedOnValue(p, item.CodeQualityAssessment));
            var all = pro.Concat(sub).ToList();
            csv.AppendLine(string.Join(delimiter, all));
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
            if (item == null) return "NULL";
            value = p.GetValue(item, null)?.ToString();
            if (value == null) return "NULL";  // Deal with nulls
            if (value.Trim().Length == 0) return ""; // Deal with spaces and blanks

            // Guard strings with "s, they may contain the delimiter!
            if (p.PropertyType == typeof(string))
            {
                value = string.Format("\"{0}\"", value);
            }
            if (p.PropertyType == typeof(float) || p.PropertyType == typeof(double)){
                value = value.Replace(',', '.');
            }
        }
        catch (Exception)
        {
            value = "";
        }
        return value;
    }
}

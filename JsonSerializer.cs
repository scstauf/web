using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Script.Serialization;

public static class JsonSerializer
{
    public static T DeserializeJson<T>(string json)
    {
        var serializer = new JavaScriptSerializer();
        return serializer.Deserialize<T>(json);
    }

    public static string DataTableToJson(DataTable dt)
    {
        var json = string.Empty;

        var serializer = new JavaScriptSerializer();
        var rows = new List<Dictionary<string, object>>();
        Dictionary<string, object> row = null;
        foreach (DataRow dr in dt.Rows)
        {
            row = new Dictionary<string, object>();
            foreach (DataColumn col in dt.Columns)
                row.Add(col.ColumnName, dr[col]);
            rows.Add(row);
        }

        return serializer.Serialize(rows);
    }

    public static string DataSetToJson(DataSet ds, params string[] tableNames)
    {
        var json = string.Empty;

        var serializer = new JavaScriptSerializer();
        var tables = new List<Dictionary<string, List<Dictionary<string, object>>>>();

        int index = 0;
        foreach (DataTable dt in ds.Tables)
        {
            var table = new Dictionary<string, List<Dictionary<string, object>>>();
            var rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = null;
            foreach (DataRow dr in dt.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                    row.Add(col.ColumnName, dr[col]);
                rows.Add(row);
            }
            table.Add(
                index < ds.Tables.Count && index < tableNames.Length 
                ? tableNames[index] 
                : dt.TableName, rows
            );
            tables.Add(table);
            index++;
        }

        return serializer.Serialize(tables);
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Telerik.Web.UI.ExportInfrastructure;

/// <summary>
/// Wrapper for the Telerik Export Infrastructure
/// Contains definitions for generating Excel files from datasources
/// </summary>
public class ExportInfrastructure
{
    /// <summary>
    /// Exports the columns and rows from a DataTable into an XLSX byte array
    /// </summary>
    /// <param name="dt">The source of the data to generate the byte array</param>
    /// <returns>An XLSX byte array containing the data from the DataTable</returns>
    public static byte[] ExportFromDataTable(DataTable dt)
    {
        var bytes = new List<byte>();

        try
        {
            if (dt != null)
            {
                ExportStructure expStruct = new ExportStructure();
                XlsxRenderer xlsx = null;

                Table tbl = new Table();
                var columns = new List<Column>();
                var columnNames = new List<string>();

                foreach (DataColumn col in dt.Columns)
                {
                    columns.Add(new Column(tbl));
                    columnNames.Add(col.ColumnName);
                }

                for (int i = 0; i < columnNames.Count; i++)
                {
                    tbl.Cells[i, 0].Value = columnNames[i];
                    tbl.Cells[i, 0].Style.Font.Bold = true;
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < columnNames.Count; j++)
                    {
                        tbl.Cells[j, i + 2].Value = dt.Rows[i][j].ToString();
                    }
                }

                expStruct.Tables.Add(tbl);
                xlsx = new XlsxRenderer(expStruct);

                bytes.AddRange(xlsx.Render());
            }
        }
        catch 
        {
            // log it?
        }

        return bytes.ToArray();
    }

    /// <summary>
    /// Sends an XLSX byte array to the browser to be downloaded by an end-user.
    /// </summary>
    /// <param name="response">HttpResponse to use</param>
    /// <param name="fileName">Name of the file to be downloaded</param>
    /// <param name="dt">Source of data to generate the xlsx file</param>
    public static void SendXlsxToBrowser(HttpResponse response, string fileName, DataTable dt)
    {
        byte[] xlsx = ExportInfrastructure.ExportFromDataTable(dt);

        response.Clear();
        response.Buffer = true;
        response.CacheControl = "Private";
        response.AddHeader("Content-Type", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        response.AddHeader("Content-Length", xlsx.Length.ToString());
        response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", fileName));
        response.Charset = "";
        response.BinaryWrite(xlsx);
        response.Flush();
        response.Close();
        response.Clear();
        response.End();
    }
}

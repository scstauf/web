using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace LWO_Dev.Helpers
{
    public class DataHelper
    {
        #region Data Conversion

        /// <summary>
        /// Extracts the base64 string from a DataUrl string
        /// </summary>
        /// <param name="dataUrl">The DataUrl string</param>
        /// <returns>A base64 string containing the DataUrl content</returns>
        public static string DataUrlToBase64(string dataUrl)
        {
            return Regex.Match(dataUrl, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;
        }

        /// <summary>
        /// Converts a DataUrl to a byte[]
        /// </summary>
        /// <param name="dataUrl">The DataUrl string</param>
        /// <returns>byte[] containing the DataUrl content</returns>
        public static byte[] DataUrlToBytes(string dataUrl)
        {
            return Convert.FromBase64String(DataUrlToBase64(dataUrl));
        }

        /// <summary>
        /// Converts a DataTable object to a JSON string
        /// </summary>
        /// <param name="dt">The DataTable to convert</param>
        /// <returns>A JSON string containing the data from a DataTable object</returns>
        public static string DataTableToJson(DataTable dt)
        {
            var rows = new List<Dictionary<string, object>>();

            foreach (DataRow dr in dt.Rows)
            {
                var row = new Dictionary<string, object>();

                foreach (DataColumn col in dt.Columns)
                    row[col.ColumnName] = dr[col];

                rows.Add(row);
            }

            return new JavaScriptSerializer().Serialize(rows);
        }

        /// <summary>
        /// Converts a DataSet object to a JSON string
        /// </summary>
        /// <param name="ds">The DataSet to convert</param>
        /// <returns>A JSON string containing the data from a DataSet object</returns>
        public static string DataSetToJson(DataSet ds)
        {
            var tables = new Dictionary<string, List<Dictionary<string, object>>>();

            foreach (DataTable dt in ds.Tables)
            {
                var rows = new List<Dictionary<string, object>>();

                foreach (DataRow dr in dt.Rows)
                {
                    var row = new Dictionary<string, object>();

                    foreach (DataColumn col in dt.Columns)
                        row[col.ColumnName] = dr[col];

                    rows.Add(row);
                }
                
                tables.Add(dt.TableName, rows);
            }

            return new JavaScriptSerializer().Serialize(tables);
        }

        #endregion

        #region Data Access Layer

        /// <summary>
        /// The key to the connection string in the configuration file
        /// </summary>
        private const string Dbkey = "LWODBConnection";

        /// <summary>
        /// Returns an empty list for building SqlParameter objects
        /// </summary>
        /// <returns></returns>
        public static List<SqlParameter> BuildSqlParamList() => new List<SqlParameter>();

        /// <summary>
        /// A generic SqlParameter factory
        /// </summary>
        /// <typeparam name="T">The datatype of the SqlParameter value</typeparam>
        /// <param name="parameterName">The name of the SqlParameter</param>
        /// <param name="dbType">The SqlDbType of the SqlParameter</param>
        /// <param name="value">The value of the SqlParameter</param>
        /// <returns>A new SqlParameter</returns>
        public static SqlParameter BuildParam<T>(string parameterName, SqlDbType dbType, T value)
        {
            var param = new SqlParameter(parameterName, dbType) { Value = (T)value };
            return param;
        }

        /// <summary>
        /// Builds a nullable generic SqlParameter for adding to a SqlParameter collection
        /// Can only work with value types.
        /// </summary>
        /// <typeparam name="T">Specify the datatype to create</typeparam>
        /// <param name="parameterName">Specify the parameter name</param>
        /// <param name="dbType">Specify the SqlDbType</param>
        /// <param name="value">Specify the generic value</param>
        /// <returns>Returns a SqlParameter that can contain DBNull.Value</returns>
        public SqlParameter BuildNullableParam<T>(string parameterName, SqlDbType dbType, T? value)
            where T: struct
        {
            SqlParameter sqlParameter = new SqlParameter();

            sqlParameter.ParameterName = parameterName;
            sqlParameter.SqlDbType = dbType;

            if (value.HasValue)
            {
                sqlParameter.Value = value.Value;
            }
            else
            {
                sqlParameter.Value = DBNull.Value;
            }

            return sqlParameter;
        }
        
        /// <summary>
        /// A generic OUTPUT SqlParameter factory
        /// </summary>
        /// <typeparam name="T">The datatype of the SqlParameter value</typeparam>
        /// <param name="parameterName">The name of the SqlParameter</param>
        /// <param name="dbType">The SqlDbType of the SqlParameter</param>
        /// <param name="value">The value of the SqlParameter</param>
        /// <returns>A new OUTPUT SqlParameter</returns>
        public static SqlParameter BuildOutputParam<T>(string parameterName, SqlDbType dbType, T value)
        {
            var param = BuildParam<T>(parameterName, dbType, value);
            param.Direction = ParameterDirection.Output;
            return param;
        }

        public static bool IsDBNull(DataRow row, string columnName)
        {
            var isDbNull = true;

            try
            {
                if (row != null)
                    isDbNull = row.Table.Columns.Contains(columnName) && row.IsNull(columnName);
            }
            catch { }

            return isDbNull;
        }

        public static T GetValue<T>(DataRow row, string columnName)
        {
            if (row == null)
                return default(T);
            
            if (!row.Table.Columns.Contains(columnName))
                return default(T);

            if (row.IsNull(columnName))
                return default(T);

            return (T)row[columnName];
        }

        public static T GetValue<T>(DataRow row, string columnName, T defaultValue)
        {
            if (row == null)
                return defaultValue;
            
            if (!row.Table.Columns.Contains(columnName))
                return defaultValue;

            if (row.IsNull(columnName))
                return defaultValue;

            return (T)row[columnName];
        }

        /// <summary>
        /// Returns a DataTable object containing the result set of a SQL query
        /// </summary>
        /// <param name="sql">SQL to execute</param>
        /// <param name="session">The Session object of the current controller</param>
        /// <param name="parameters">A list of parameters to pass to the SQL query</param>
        /// <returns>A DataTable object containing the result set of a SQL query</returns>
        public static DataTable GetDataTable(string sql, HttpSessionStateBase session, List<SqlParameter> parameters = null)
        {
            return GetDataTable(sql, (int) session["userID"], parameters);
        }

        /// <summary>
        /// Returns a DataTable object containing the result set of a SQL query
        /// </summary>
        /// <param name="sql">SQL to execute</param>
        /// <param name="userId">The userId of the current session</param>
        /// <param name="parameters">A list of parameters to pass to the SQL query</param>
        /// <returns>A DataTable object containing the result set of a SQL query</returns>
        public static DataTable GetDataTable(string sql, int userId, List<SqlParameter> parameters = null)
        {
            SqlConnection conn = null;
            DataTable dt = null;
            try
            {
                conn = new SqlConnection(ConfigurationManager.ConnectionStrings[Dbkey].ToString());
                conn.Open();

                using (var cmd = new SqlCommand(sql, conn))
                {
                    if (parameters != null)
                        foreach (var param in parameters)
                            cmd.Parameters.Add(param);
                     

                    using (var adapter = new SqlDataAdapter(cmd))
                    {
                        dt = new DataTable();
                        adapter.Fill(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHelper.Log(ex, userId);
            }
            finally
            {
                conn?.Close();
            }

            return dt;
        }

        /// <summary>
        /// Returns a DataTable object containing the result set of a stored procedure
        /// </summary>
        /// <param name="procName">The name of the stored procedure</param>
        /// <param name="session">The Session object of the controller</param>
        /// <param name="parameters">A list of parameters to pass to the stored procedure</param>
        /// <returns>A DataTable object containing the result set of a stored procedure</returns>
        public static DataTable GetDataTableFromStoredProcedure(string procName, HttpSessionStateBase session, List<SqlParameter> parameters = null)
        {
            return GetDataTableFromStoredProcedure(procName, (int) session["userID"], parameters);
        }

        /// <summary>
        /// Returns a DataTable object containing the result set of a stored procedure
        /// </summary>
        /// <param name="procName">The name of the stored procedure</param>
        /// <param name="userId">The userId of the current session</param>
        /// <param name="parameters">A list of parameters to pass to the stored procedure</param>
        /// <returns>A DataTable object containing the result set of a stored procedure</returns>
        public static DataTable GetDataTableFromStoredProcedure(string procName, int userId, List<SqlParameter> parameters = null)
        {
            SqlConnection conn = null;
            DataTable dt = null;
            try
            {
                conn = new SqlConnection(ConfigurationManager.ConnectionStrings[Dbkey].ToString());
                conn.Open();

                using (var cmd = new SqlCommand(procName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (parameters != null)
                        foreach (var param in parameters)
                            cmd.Parameters.Add(param);


                    using (var adapter = new SqlDataAdapter(cmd))
                    {
                        dt = new DataTable();
                        adapter.Fill(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHelper.Log(ex, userId);
            }
            finally
            {
                conn?.Close();
            }

            return dt;
        }

        /// <summary>
        /// Returns a DataSet object containing the result set of a SQL query
        /// </summary>
        /// <param name="sql">SQL to execute</param>
        /// <param name="session">The Session object of the current controller</param>
        /// <param name="parameters">A list of parameters to pass to the SQL query</param>
        /// <returns>A DataSet object containing the result set of a SQL query</returns>
        public static DataSet GetDataSet(string sql, HttpSessionStateBase session, List<SqlParameter> parameters = null)
        {
            return GetDataSet(sql, (int)session["userID"], parameters);
        }

        /// <summary>
        /// Returns a DataSet object containing the result set of a SQL query
        /// </summary>
        /// <param name="sql">SQL to execute</param>
        /// <param name="userId">The userId of the current session</param>
        /// <param name="parameters">A list of parameters to pass to the SQL query</param>
        /// <returns>A DataSet object containing the result set of a SQL query</returns>
        public static DataSet GetDataSet(string sql, int userId, List<SqlParameter> parameters = null)
        {
            SqlConnection conn = null;
            DataSet ds = null;
            try
            {
                conn = new SqlConnection(ConfigurationManager.ConnectionStrings[Dbkey].ToString());
                conn.Open();

                using (var cmd = new SqlCommand(sql, conn))
                {
                    if (parameters != null)
                        foreach (var param in parameters)
                            cmd.Parameters.Add(param);


                    using (var adapter = new SqlDataAdapter(cmd))
                    {
                        ds = new DataSet();
                        adapter.Fill(ds);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHelper.Log(ex, userId);
            }
            finally
            {
                conn?.Close();
            }

            return ds;
        }

        /// <summary>
        /// Returns a DataSet object containing the result set of a stored procedure
        /// </summary>
        /// <param name="procName">The name of the stored procedure</param>
        /// <param name="session">The Session object of the controller</param>
        /// <param name="parameters">A list of parameters to pass to the stored procedure</param>
        /// <returns>A DataSet object containing the result set of a stored procedure</returns>
        public static DataSet GetDataSetFromStoredProcedure(string procName, HttpSessionStateBase session, List<SqlParameter> parameters = null)
        {
            return GetDataSetFromStoredProcedure(procName, (int)session["userID"], parameters);
        }

        /// <summary>
        /// Returns a DataSet object containing the result set of a stored procedure
        /// </summary>
        /// <param name="procName">The name of the stored procedure</param>
        /// <param name="userId">The userId of the current session</param>
        /// <param name="parameters">A list of parameters to pass to the stored procedure</param>
        /// <returns>A DataSet object containing the result set of a stored procedure</returns>
        public static DataSet GetDataSetFromStoredProcedure(string procName, int userId, List<SqlParameter> parameters = null)
        {
            SqlConnection conn = null;
            DataSet ds = null;
            try
            {
                conn = new SqlConnection(ConfigurationManager.ConnectionStrings[Dbkey].ToString());
                conn.Open();

                using (var cmd = new SqlCommand(procName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (parameters != null)
                        foreach (var param in parameters)
                            cmd.Parameters.Add(param);


                    using (var adapter = new SqlDataAdapter(cmd))
                    {
                        ds = new DataSet();
                        adapter.Fill(ds);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHelper.Log(ex, userId);
            }
            finally
            {
                conn?.Close();
            }

            return ds;
        }

        /// <summary>
        /// Returns the first value returned by a stored procedure
        /// </summary>
        /// <param name="procName">The name of the stored procedure</param>
        /// <param name="userId">The userId of the current session</param>
        /// <param name="parameters">A list of parameters to pass to the stored procedure</param>
        /// <returns>The first value returned by a stored procedure</returns>
        public static T GetScalarFromStoredProcedure<T>(string procName, int userId, List<SqlParameter> parameters = null)
        {
            SqlConnection conn = null;
            T value = default(T);
            try
            {
                conn = new SqlConnection(ConfigurationManager.ConnectionStrings[Dbkey].ToString());
                conn.Open();

                using (var cmd = new SqlCommand(procName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (parameters != null)
                        foreach (var param in parameters)
                            cmd.Parameters.Add(param);

                    value = (T)cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                ErrorHelper.Log(ex, userId);
            }
            finally
            {
                conn?.Close();
            }

            return (T)value;
        }

        /// <summary>
        /// Executes stored procedure directly and returns number of rows affected
        /// </summary>
        /// <param name="procName">Name of stored procedure</param>
        /// <param name="session">The Session object of the controller</param>
        /// <param name="parameters">A list of parameters to pass to the SQL</param>
        /// <returns>Number of rows affected</returns>
        public static int ExecuteStoredProcedure(string procName, HttpSessionStateBase session, List<SqlParameter> parameters = null)
        {
            return ExecuteStoredProcedure(procName, (int)session["userID"], parameters);
        }

        /// <summary>
        /// Executes stored procedure directly and returns number of rows affected
        /// </summary>
        /// <param name="procName">Name of stored procedure</param>
        /// <param name="userId">The userId of the current session</param>
        /// <param name="parameters">A list of parameters to pass to the SQL</param>
        /// <returns>Number of rows affected</returns>
        public static int ExecuteStoredProcedure(string procName, int userId, List<SqlParameter> parameters = null)
        {
            SqlConnection conn = null;
            var rowsAffected = 0;

            try
            {
                conn = new SqlConnection(ConfigurationManager.ConnectionStrings[Dbkey].ToString());
                conn.Open();

                using (var cmd = new SqlCommand(procName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    
                    if (parameters != null)
                        foreach (var param in parameters)
                            cmd.Parameters.Add(param);

                    rowsAffected = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                ErrorHelper.Log(ex, userId);
            }
            finally
            {
                conn?.Close();
            }

            return rowsAffected;
        }

        /// <summary>
        /// Executes SQL statements directly and returns number of rows affected
        /// </summary>
        /// <param name="sql">SQL to execute</param>
        /// <param name="session">The Session object of the controller</param>
        /// <param name="parameters">A list of parameters to pass to the SQL</param>
        /// <returns>Number of rows affected</returns>
        public static int ExecuteNonQuery(string sql, HttpSessionStateBase session, List<SqlParameter> parameters = null)
        {
            return ExecuteNonQuery(sql, (int) session["userID"], parameters);
        }

        /// <summary>
        /// Executes SQL statements directly and returns number of rows affected
        /// </summary>
        /// <param name="sql">SQL to execute</param>
        /// <param name="userId">The userId of the current session</param>
        /// <param name="parameters">A list of parameters to pass to the SQL</param>
        /// <returns>Number of rows affected</returns>
        public static int ExecuteNonQuery(string sql, int userId, List<SqlParameter> parameters = null)
        {
            SqlConnection conn = null;
            var rowsAffected = 0;

            try
            {
                conn = new SqlConnection(ConfigurationManager.ConnectionStrings[Dbkey].ToString());
                conn.Open();

                using (var cmd = new SqlCommand(sql, conn))
                {
                    if (parameters != null)
                        foreach (var param in parameters)
                            cmd.Parameters.Add(param);

                    rowsAffected = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                ErrorHelper.Log(ex, userId);
            }
            finally
            {
                conn?.Close();
            }

            return rowsAffected;
        }
        
        /// <summary>
        /// Asynchronously returns a DataTable object containing the result set of a stored procedure
        /// </summary>
        /// <param name="procName">The name of the stored procedure</param>
        /// <param name="session">The Session object of the controller</param>
        /// <param name="parameters">A list of parameters to pass to the stored procedure</param>
        /// <returns>A DataTable object containing the result set of a stored procedure</returns>
        public static async Task<DataTable> GetDataTableFromStoredProcedureAsync(string procName, HttpSessionStateBase session, List<SqlParameter> parameters = null)
        {
            return await GetDataTableFromStoredProcedure(procName, (int) session["userID"], parameters);
        }
        
        /// <summary>
        /// Asynchronously returns a DataTable object containing the result set of a stored procedure
        /// </summary>
        /// <param name="procName">The name of the stored procedure</param>
        /// <param name="userId">The userId of the current session</param>
        /// <param name="parameters">A list of parameters to pass to the stored procedure</param>
        /// <returns>A DataTable object containing the result set of a stored procedure</returns>
        public async Task<DataTable> GetDataTableFromStoredProcedureAsync(string procName, int userId, List<SqlParameter> parameters = null)
        {
            SqlConnection conn = null;
            DataTable dt = null;

            try 
            {
                conn = new SqlConnection(ConfigurationManager.ConnectionStrings[Dbkey].ToString());
                await conn.OpenAsync();

                using (var cmd = new SqlCommand(procName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (parameters != null)
                        foreach (var param in parameters)
                            cmd.Parameters.Add(param);

                    var reader = await cmd.ExecuteReaderAsync();
                    dt = new DataTable();

                    _GetColumns(ref dt, reader);
                    _GetRows(dt, reader);
                }
            }
            catch (Exception ex) 
            {
                ErrorHelper.Log(ex, userId);
            }
            finally 
            {
                conn?.Close();
            }

            return dt;
        }
        
        private void _GetColumns(ref DataTable table, SqlDataReader reader)
        {
            try
            {
                if (!reader.IsClosed)
                {
                    var columns = new List<DataColumn>();

                    for (var i = 0; i < reader.FieldCount; i++)
                        columns.Add(
                            new DataColumn(
                                reader.GetName(i),
                                reader.GetFieldType(i)
                            )
                        );

                    table.Columns.AddRange(columns.ToArray());
                }
            }
            catch {}
        }

        private async void _GetRows(DataTable table, SqlDataReader reader)
        {
            try
            {
                if (!reader.IsClosed)
                {
                    while (await reader.ReadAsync())
                    {
                        var values = new List<object>();

                        for (var i = 0; i < reader.FieldCount; i++)
                        {
                            var value = reader[i];
                            values.Add(value);
                        }

                        table.Rows.Add(values.ToArray());
                    }
                }
            }
            catch {}
        }

        #endregion
    }
}

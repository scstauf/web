
public class Sql
{
    private string connectionString = string.Empty;
    private List<SqlParameter> _Params;
    private const int SqlTimeout = 60000;

    public Sql(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public List<SqlParameter> BuildParamList()
    {
        return new List<SqlParameter>();
    }

    public List<SqlParameter> Params
    {
        get
        {
            if (this._Params == null)
            {
                this._Params = this.BuildParamList();
            }

            return this._Params;
        }
    }

    private void SwapParameters(List<SqlParameter> parameters = null) {            
        if (sqlParameters == null && this.Params.Count > 0)
            sqlParameters = this.Params;
    }

    /// <summary>
    /// Builds a generic SqlParameter for adding to a SqlParameter collection
    /// </summary>
    /// <typeparam name="T">Specify the datatype to create</typeparam>
    /// <param name="parameterName">Specify the parameter name</param>
    /// <param name="dbType">Specify the SqlDbType</param>
    /// <param name="value">Specify the generic value</param>
    /// <returns>Returns a SqlParameter</returns>
    public SqlParameter BuildParam<T>(string parameterName, SqlDbType dbType, T value)
    {
        SqlParameter sqlParameter = new SqlParameter();

        sqlParameter.ParameterName = parameterName;
        sqlParameter.SqlDbType = dbType;
        sqlParameter.Value = value;

        this.Params.Add(sqlParameter);

        return sqlParameter;
    }

    /// <summary>
    /// Builds a non-generic nullable SqlParameter for adding to a SqlParameter collection.
    /// </summary>
    /// <param name="parameterName">Specify the parameter name</param>
    /// <param name="dbType">Specify the SqlDbType</param>
    /// <param name="value">Specify the generic value</param>
    /// <returns>Returns a SqlParameter that can contain DBNull.Value</returns>
    public SqlParameter BuildNullableParam(string parameterName, SqlDbType dbType, string value)
    {
        var sqlParameter = new SqlParameter
        {
            ParameterName = parameterName,
            SqlDbType = dbType
        };

        if (value == null)
        {
            sqlParameter.Value = DBNull.Value;
        }
        else
        {
            sqlParameter.Value = value;
        }

        this.Params.Add(sqlParameter);

        return sqlParameter;
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

        this.Params.Add(sqlParameter);

        return sqlParameter;
    }

    /// <summary>
    /// Gets a DataSet from a stored procedure
    /// </summary>
    /// <param name="storedProc">The name of the stored procedure</param>
    /// <param name="sqlParameters">A collection of SqlParameter</param>
    /// <returns>Returns a DataSet from the stored procedure</returns>
    public DataSet GetDataSetFromStoredProc(string storedProc, List<SqlParameter> sqlParameters = null)
    {
        SqlConnection conn = null;
        DataSet ds = null;

        try
        {
            SwapParameters(parameters);

            conn = new SqlConnection(connectionString);
            conn.Open();

            using (var cmd = new SqlCommand(storedProc, conn))
            {
                cmd.CommandTimeout = SqlTimeout;
                cmd.CommandType = CommandType.StoredProcedure;

                if (sqlParameters != null)
                    foreach (var param in sqlParameters)
                        cmd.Parameters.Add(param);

                using (var adapter = new SqlDataAdapter(cmd))
                {
                    ds = new DataSet();
                    adapter.Fill(ds);
                }
            }
        }
        finally
        {
            if (conn != null)
                conn.Close();

            this.Params.Clear();
        }

        return ds;
    }

    /// <summary>
    /// Gets a DataTable from a stored procedure
    /// </summary>
    /// <param name="storedProc">The name of the stored procedure</param>
    /// <param name="sqlParameters">A collection of SqlParameter</param>
    /// <returns>Returns a DataTable from the stored procedure</returns>
    public DataTable GetDataTableFromStoredProc(string storedProc, List<SqlParameter> sqlParameters = null, int sqlTimeout = 120000)
    {
        SqlConnection conn = null;
        DataTable dt = null;

        try
        {
            SwapParameters(parameters);

            conn = new SqlConnection(connectionString);
            conn.Open();

            using (var cmd = new SqlCommand(storedProc, conn))
            {
                cmd.CommandTimeout = sqlTimeout;
                cmd.CommandType = CommandType.StoredProcedure;

                if (sqlParameters != null)
                    foreach (var param in sqlParameters)
                        cmd.Parameters.Add(param);

                using (var adapter = new SqlDataAdapter(cmd))
                {
                    dt = new DataTable();
                    adapter.Fill(dt);
                }
            }
        }
        finally
        {
            if (conn != null)
                conn.Close();

            this.Params.Clear();
        }

        return dt;
    }

    public void ExecStoredProc(string storedProc, List<SqlParameter> sqlParameters = null)
    {
        SwapParameters(parameters);

        GetDataTableFromStoredProc(storedProc, sqlParameters);
    }

    /// <summary>
    /// Generic wrapper for ExecuteScalar()
    /// </summary>
    /// <typeparam name="T">The datatype of the scalar value</typeparam>
    /// <param name="storedProc">Name of stored procedure</param>
    /// <param name="sqlParameters">List of parameters to pass into stored procedure. Null by default</param>
    /// <returns>Generic scalar value</returns>
    public T GetScalarFromStoredProc<T>(string storedProc, List<SqlParameter> sqlParameters = null, T defaultValue = default(T))
    {
        object scalar = null;

        SqlConnection conn = null;
        try
        {
            SwapParameters(parameters);

            conn = new SqlConnection(connectionString);
            conn.Open();

            using (var cmd = new SqlCommand(storedProc, conn))
            {
                cmd.CommandTimeout = SqlTimeout;
                cmd.CommandType = CommandType.StoredProcedure;

                if (sqlParameters != null)
                    foreach (var param in sqlParameters)
                        cmd.Parameters.Add(param);

                scalar = cmd.ExecuteScalar();
                if (scalar == null || scalar == DBNull.Value)
                    scalar = defaultValue;
            }
        }
        finally
        {
            if (conn != null)
                conn.Close();

            this.Params.Clear();
        }

        return (T)scalar;
    }
}

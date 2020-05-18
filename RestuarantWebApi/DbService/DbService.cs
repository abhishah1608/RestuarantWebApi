using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;

namespace RestuarantWebApi.DbService
{
    public class DbServicecls : IDisposable
    {

        private IConfiguration Configuration { get; }

        private string sqlConnection = null;

        private SqlConnection _connection = null;



        public DbServicecls(IConfiguration configuration)
        {
            Configuration = configuration;
            this.sqlConnection = Configuration["DbConnection"];
            this._connection = new SqlConnection(this.sqlConnection);
        }

        /// <summary>
        /// Get List of Records.
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public DataSet GetAllRecords(SqlCommand cmd)
        {
            DataSet ds = new DataSet();
            if (this._connection != null)
            {
                SqlDataAdapter adapter = new SqlDataAdapter();
                cmd.Connection = this._connection;
                adapter.SelectCommand = cmd;
                adapter.Fill(ds);
            }
            return ds;
        }

        /// <summary>
        /// Abhi shah : Perform Transaction of Records.
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public DataSet Transaction(SqlCommand cmd)
        {
            DataSet ds = new DataSet();
            if (this._connection != null)
            {
                SqlDataAdapter adapter = new SqlDataAdapter();
                cmd.Connection = this._connection;
                adapter.SelectCommand = cmd; 
                adapter.Fill(ds);
            }
            return ds;
        }

        public object GetScalerValue(SqlCommand cmd)
        {
            cmd.Connection = this._connection;
            this._connection.Open();
            object obj = cmd.ExecuteScalar();
            this._connection.Close();
            return obj;
        }

        public void Dispose()
        {
            if (this._connection != null && this._connection.State == ConnectionState.Open)
            {
                this._connection.Close();
            }
        }

        
    }
}

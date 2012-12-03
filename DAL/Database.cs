using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading;

namespace DAL
{
    public class Database
    {
        DbProviderFactory _factory;
        DbConnection _con;
        DbCommand _com;

        public void Open()
        {
            while (IsConnectionBusy)
            {
                Thread.Sleep(5);
            }
            _con.Open();
        }

        public void Close()
        {
            _con.Close();
        }

        public bool IsConnectionBusy
        {
            get
            {
                if (_con.State != ConnectionState.Closed)
                    return true;
                else
                    return false;
            }
        }

        public DbDataReader ExcecuteReader(string sqlString, CommandType cmdType, params DbParameter[] parameters)
        {
            FillCommand(sqlString, cmdType, parameters);
            return _com.ExecuteReader();
        }

        public object ExecuteScalar(string sqlString, CommandType cmdType, params DbParameter[] parameters)
        {
            FillCommand(sqlString, cmdType, parameters);
            return _com.ExecuteScalar();
        }

        public int ExecuteNonQuery(string sqlString, CommandType cmdType, params DbParameter[] parameters)
        {
            FillCommand(sqlString, cmdType, parameters);
            return _com.ExecuteNonQuery();
        }

        private void FillCommand(string sqlString, CommandType cmdType, DbParameter[] parameters)
        {
            _com.Parameters.Clear();
            _com.CommandText = sqlString;
            _com.CommandType = cmdType;
            if (parameters != null)
                _com.Parameters.AddRange(parameters);
        }

        public DbParameter CreateParameter(string name, object value)
        {
            DbParameter parameter = _factory.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value;

            return parameter;
        }

        public DbParameter CreateParameter(string name, DbType dbType)
        {
            DbParameter parameter = _factory.CreateParameter();
            parameter.ParameterName = name;
            parameter.DbType = dbType;

            return parameter;
        }

        public Database(string connectionString, string providerName)
        {
            _factory = DbProviderFactories.GetFactory(providerName);
            _com = _factory.CreateCommand();
            _con = _factory.CreateConnection();
            _con.ConnectionString = connectionString;
            _com.Connection = _con;
        }
    }
}

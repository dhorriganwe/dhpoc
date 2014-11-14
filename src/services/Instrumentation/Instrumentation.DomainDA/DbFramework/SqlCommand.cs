using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Instrumentation.DomainDA.Helpers;
using Npgsql;

namespace Instrumentation.DomainDA.DbFramework
{
    /// <summary>
    /// DAL layer for Postgre
    /// </summary>
    public class SqlCommand : ISqlCommand
    {
        private readonly EfDatabaseContext _dbContext = EfDatabaseContext.CreateContext();
        private NpgsqlConnection _npgsqlConnection = new NpgsqlConnection(Configurations.GetConnectionString("rtAudit"));
        private string _schema = null;
        //private readonly NpgsqlConnection _npgsqlConnection = new NpgsqlConnection(Constants.PostgresConnectionString);

        #region ctors
        public SqlCommand()
        { }

        public SqlCommand(string dbKey, string schema)
        {
            _schema = schema;
            _npgsqlConnection = new NpgsqlConnection(Configurations.GetConnectionString(dbKey));
        }

        public SqlCommand(EfDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        #endregion ctors
        
        #region Create NpgsqlCommand
        private NpgsqlCommand CreateCommandWithParameters(
            string procName, 
            IEnumerable<KeyValuePair<string, object>> parameters)
        {
            var command = new NpgsqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                Connection = _npgsqlConnection,
                CommandText = _schema + "." + procName
            };
            foreach (var parameter in parameters)
            {
                command.Parameters.Add(new NpgsqlParameter { ParameterName = parameter.Key, Value = parameter.Value });
            }
            return command;
        }

        private NpgsqlCommand CreateCommandWithParameters(
            string procName, 
            IEnumerable<DbParameter> parameters)
        {
            var command = new NpgsqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                Connection = _npgsqlConnection,
                CommandText = _schema + "." + procName
            };
            foreach (var parameter in parameters)
            {
                command.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = parameter.ParameterName, 
                    DbType = parameter.DbType, 
                    Value = parameter.Value
                });
            }
            return command;
        }

        private NpgsqlCommand CreateCommandForSqlStatement(
            string sqlStatement, 
            IEnumerable<KeyValuePair<string, object>> parameters)
        {
            var command = new NpgsqlCommand
            {
                CommandType = CommandType.Text,
                Connection = _npgsqlConnection,
                CommandText = sqlStatement
            };
            foreach (var parameter in parameters)
            {
                command.Parameters.Add(new NpgsqlParameter { ParameterName = parameter.Key, Value = parameter.Value });
            }
            return command;
        }
        #endregion Create NpgsqlCommand

        #region Execute Methods
        public DataSet ExecuteStoredProc(
            string procName, 
            IDictionary<string, object> parameters)
        {
            using (var command = CreateCommandWithParameters(procName, parameters))
            {
                _npgsqlConnection.Open();
                var dataAdapter = new NpgsqlDataAdapter(command);
                _npgsqlConnection.Close();
                var dataSet = new DataSet();
                dataAdapter.Fill(dataSet);
                return dataSet;
            }
        }

        /// <summary>
        /// Executes the reader.
        /// </summary>
        /// <param name="procName">Name of the proc.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>DataReader</returns>
        public IDataReader ExecuteReader(string procName, IDictionary<string, object> parameters)
        {
            var command = CreateCommandWithParameters(procName, parameters);
            if (_npgsqlConnection.State != ConnectionState.Open)
            {
                _npgsqlConnection.Open();
            }
            return command.ExecuteReader();
        }

        public IDataReader ExecuteReaderSqlStatement(string sqlStatement, IDictionary<string, object> parameters)
        {
            var command = CreateCommandForSqlStatement(sqlStatement, parameters);
            if (_npgsqlConnection.State != ConnectionState.Open)
            {
                _npgsqlConnection.Open();
            }
            return command.ExecuteReader();
        }

        public IDataReader ExecuteReaderSqlStatement(string sqlStatement, IDictionary<string, object> parameters, Action<string, string> onError)
        {
            try
            {
                var command = CreateCommandForSqlStatement(sqlStatement, parameters);
                if (_npgsqlConnection.State != ConnectionState.Open)
                {
                    _npgsqlConnection.Open();
                }
                return command.ExecuteReader();
            }
            catch (NpgsqlException npgsqlException)
            {
                if (onError != null)
                {
                    onError(npgsqlException.Code, npgsqlException.Message);
                }
                return null;
            }
            
        }

        /// <summary>
        /// Executes the scalar.
        /// </summary>
        /// <param name="procName">Name of the proc.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Scalar Object</returns>
        public object ExecuteScalar(string procName, IDictionary<string, object> parameters)
        {
            using (var command = CreateCommandWithParameters(procName, parameters))
            {
                if (_npgsqlConnection.State != ConnectionState.Open)
                {
                    _npgsqlConnection.Open();
                }
                return command.ExecuteScalar();
            }
        }

        /// <summary>
        /// Executes the non query.
        /// </summary>
        /// <param name="procName">Name of the proc.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        /// The number of rows affected if known; -1 otherwise.
        /// </returns>
        public int ExecuteNonQuery(string procName, IDictionary<string, object> parameters)
        {
            using (var command = CreateCommandWithParameters(procName, parameters))
            {
                if (_npgsqlConnection.State != ConnectionState.Open)
                {
                    _npgsqlConnection.Open();
                }
                return command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Executes the scalar.
        /// </summary>
        /// <param name="procName">Name of the proc.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        /// Scalar Object
        /// </returns>
        public object ExecuteScalar(string procName, IEnumerable<DbParameter> parameters)
        {
            using (var command = CreateCommandWithParameters(procName, parameters))
            {
                if (_npgsqlConnection.State != ConnectionState.Open)
                {
                    _npgsqlConnection.Open();
                }
                return command.ExecuteScalar();
            }
        }

        /// <summary>
        /// Executes the non query.
        /// </summary>
        /// <param name="procName">Name of the proc.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        /// The number of rows affected if known; -1 otherwise.
        /// </returns>
        public int ExecuteNonQuery(string procName, IEnumerable<DbParameter> parameters)
        {
            using (var command = CreateCommandWithParameters(procName, parameters))
            {
                if (_npgsqlConnection.State != ConnectionState.Open)
                {
                    _npgsqlConnection.Open();
                }
                return command.ExecuteNonQuery();
            }
        }
        #endregion Execute Methods

        #region Implementing IDisposable interface
        /// <summary>
        /// Implementing IDisposable interface
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Adding all the resources here which needs to be disposed of by IDisposable
        /// </summary>
        /// <param name="isDisposable"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool isDisposable)
        {
            if (!isDisposable) return;

            _dbContext.Dispose();
            _npgsqlConnection.Close();
        }
        #endregion
    }
}
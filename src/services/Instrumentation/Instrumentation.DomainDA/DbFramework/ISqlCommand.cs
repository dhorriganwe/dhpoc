using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Instrumentation.DomainDA.DbFramework
{
    /// <summary>
    /// DAL layer interface which will be exposed to bussiness layer
    /// </summary>
    public interface ISqlCommand : IDisposable
    {
        DataSet ExecuteStoredProc(string procName, IDictionary<string, object> parameters);

        IDataReader ExecuteReader(string procName, IDictionary<string, object> parameters);

        /// <returns>Scalar Object</returns>
        object ExecuteScalar(string procName, IDictionary<string, object> parameters);

        /// <returns>Scalar Object</returns>
        object ExecuteScalar(string procName, IEnumerable<DbParameter> parameters);

        /// <returns> The number of rows affected if known; -1 otherwise. </returns>
        int ExecuteNonQuery(string procName, IDictionary<string, object> parameters);

        /// <returns> The number of rows affected if known; -1 otherwise. </returns>
        int ExecuteNonQuery(string procName, IEnumerable<DbParameter> parameters);
    }
}
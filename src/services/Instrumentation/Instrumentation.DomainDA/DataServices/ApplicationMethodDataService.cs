using System.Collections.Generic;
using System.Data;
using Instrumentation.DomainDA.DbFramework;
using Instrumentation.DomainDA.Helpers;
using Instrumentation.DomainDA.Models;

namespace Instrumentation.DomainDA.DataServices
{
    public interface IApplicationMethodDataService
    {
        IList<ApplicationMethod> GetAllApplicationMethods_sproc();
    }

    public class ApplicationMethodDataService : IApplicationMethodDataService
    {
        #region ApplicationMethod

        private const string GETALLAPPLICATIONMETHODS = "getallapplicationmethods";
        private const string DBKEY = "RisingTide";
        private const string DBSCHEMA = "rt";

        public IList<ApplicationMethod> GetAllApplicationMethods_sproc()
        {
            return GetApplicationMethods(GETALLAPPLICATIONMETHODS, new Dictionary<string, object>());
        }

        private static IList<ApplicationMethod> GetApplicationMethods(
            string storedProcedureName,
            IDictionary<string, object> parameters)
        {
            var operationBoundaries = new List<ApplicationMethod>();

            using (var dbContext = new SqlCommand(DBKEY, DBSCHEMA))
            {
                using (var reader = dbContext.ExecuteReader(storedProcedureName, parameters))
                {
                    while (!reader.IsClosed && reader.Read())
                    {
                        var location = ToApplicationMethod(reader);
                        operationBoundaries.Add(location);
                    }
                }
            }

            return operationBoundaries;
        }

        private static ApplicationMethod ToApplicationMethod(
            IDataReader reader)
        {
            return new ApplicationMethod()
            {
                Id = reader["id"].ReturnDefaultOrValue<string>(),
                Title = reader["title"].ReturnDefaultOrValue<string>(),
            };
        }

        #endregion
    }
}

using System.Data.Common;
using System.Data.Entity;
using Instrumentation.DomainDA.Helpers;
using Npgsql;

namespace Instrumentation.DomainDA.DbFramework
{
    /// <summary>
    /// Entity framework context
    /// </summary>
    public class EfDatabaseContext : DbContext
    {
        public EfDatabaseContext(DbConnection connection)
            : base(connection, true)
        {
            Database.SetInitializer(new ContextInitializer());
        }

        public static EfDatabaseContext CreateContext()
        {
            var conn = new NpgsqlConnection(Constants.PostgresConnectionString);
            return new EfDatabaseContext(conn);
        }
    }

    #region Context Initializer

    internal class ContextInitializer : IDatabaseInitializer<EfDatabaseContext>
    {
        public void InitializeDatabase(EfDatabaseContext context)
        {
            if (!context.Database.Exists())
            {
                context.Database.Create();
                Seed(context);
                context.SaveChanges();
            }
        }

        private void Seed(EfDatabaseContext context)
        {
            //context.Organization.Add();
        }
    }

    #endregion
}

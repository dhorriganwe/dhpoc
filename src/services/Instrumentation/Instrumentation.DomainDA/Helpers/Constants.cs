
namespace Instrumentation.DomainDA.Helpers
{
    public class Constants
    {
        public const string PostgresConnectionString = "server=127.0.0.1;port=5432;database=RisingTide;user id=postgres;password=Aditi01*;enlist=true;pooling=false;minpoolsize=1;maxpoolsize=100;timeout=50;";
        public const string Schema = "rt";
        public const string AuditLog = "AuditLog";


        public const string ConfigKey_LogsPath = "LogsPath";
        public const string ConfigKey_CdmsRequestLoggingEnabled = "CdmsRequestLoggingEnabled";
    }
}
using Serilog;

namespace WalkProject.Loggers
{
    public class LoggerServices
    {
        public static WebApplicationBuilder AppBuilder(WebApplicationBuilder builder)
        {
            // Logger
            var logger = new LoggerConfiguration()
                .WriteTo.Console()
                // .WriteTo.File("Loggers/NzWalks_Log.txt", rollingInterval: RollingInterval.Minute)
                .MinimumLevel.Warning()
                .CreateLogger();

            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog(logger);

            return builder;
        }
    }
}

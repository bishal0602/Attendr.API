using Attendr.API;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .CreateLogger();

try
{
    Log.Information("Starting Attendr API...");

    var builder = WebApplication.CreateBuilder(args);
    var app = builder
        .ConfigureServices()
        .ConfigurePipeline();

    app.Run();
}
// https://github.com/dotnet/runtime/issues/60600
catch (Exception ex) when (ex.GetType().Name is not "HostAbortedException")
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}
using DDS.Logs.Extensions;
using DDS.Logs.Middlewares;
using Serilog;
using System.Diagnostics;
using TinyMais.Application.Abstractions.AppServices;
using TinyMais.WebAPI.Configurations;

var webApplicationOptions = new WebApplicationOptions()
{
    ContentRootPath = AppContext.BaseDirectory,
    Args = args,
    ApplicationName = Process.GetCurrentProcess().ProcessName
};

var builder = WebApplication.CreateBuilder(webApplicationOptions);

builder.Host
    .ConfigureLogs(options =>
    {
        options.UseMongoDB = false;

        options.UseTextFile = true;
        options.TextFileRollingInterval = RollingInterval.Day;
        options.TextFilePath = $"{AppContext.BaseDirectory}\\Logs\\log.txt";
    })
    .UseWindowsService();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInjecaoDependenciasConfig(builder.Configuration);

var app = builder.Build();
try
{

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.UseMiddleware<RequestLoggingMiddleware>();

    if (args.Length == 2)
    {
        var dataInicial = Convert.ToDateTime(args[0]);
        var dataFinal = Convert.ToDateTime(args[1]);

        using var sp = builder?.Services?.BuildServiceProvider();
        using var scope = sp?.CreateScope();
        var baixarRecebiveisService = scope?.ServiceProvider.GetRequiredService<IBaixarRecebiveisAppService>();

        await baixarRecebiveisService?.BaixarAsync(dataInicial, dataFinal);
    }

    app.Run();

}
catch (Exception exception)
{
    Serilog.Log.Fatal("Erro fatal. Exception: {exception}", exception);
}
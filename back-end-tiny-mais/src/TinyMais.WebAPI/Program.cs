using DDS.Logs.Extensions;
using DDS.Logs.Middlewares;
using Serilog;
using TinyMais.Application.Abstractions.AppServices;
using TinyMais.WebAPI.Configurations;

var builder = WebApplication.CreateBuilder(args);
builder.Host.ConfigureLogs(options =>
{
    options.UseMongoDB = false;

    options.UseTextFile = true;
    options.TextFileRollingInterval = RollingInterval.Day;
    options.TextFilePath = "Logs/log.txt";
});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInjecaoDependenciasConfig(builder.Configuration);

var app = builder.Build();

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

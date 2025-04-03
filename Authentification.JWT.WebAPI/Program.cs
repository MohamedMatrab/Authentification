using Authentification.JWT.Service;
using Authentification.JWT.Service.Mappers;
using NLog;
using NLog.Extensions.Logging;
using NLog.Web;


var builder = WebApplication.CreateBuilder(args);

//var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");
builder.Logging.ClearProviders();
builder.Host.UseNLog();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.ClearProviders();
    loggingBuilder.AddNLog();
});

builder.Services.AddAutoMapper(typeof(UserToDtoMapper));

builder.Services
    .AddService(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
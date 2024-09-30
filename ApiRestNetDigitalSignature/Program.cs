using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using ApiRestNetDigitalSignature.Application.Service;
using ApiRestNetDigitalSignature.Application.Service.Cypher;
using ApiRestNetDigitalSignature.Application.Service.User.Validator;
using ApiRestNetDigitalSignature.Dominio.Port;
using ApiRestNetDigitalSignature.Infraestructure.ApiRestDigitalSignature.Exceptions;
using ApiRestNetDigitalSignature.Infraestructure.ApiRestDigitalSignature.Log;
using ApiRestNetDigitalSignature.Infraestructure.Persistence;
using ApiRestNetDigitalSignature.Infraestructure.Persistence.Repository;
using ApiRestNetDigitalSignature.Infraestructure.Service;
using log4net;
using log4net.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Log4Net.AspNetCore;

[assembly: ApiController]

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddScoped<IAppLogger, LoggerImpl>();
builder.Services.AddScoped<IMultiLanguageMessagesService, MultiLanguageMessagesService>();

builder.Services.AddScoped<IDsUserRepository, DsUserRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<CreateDsUserService>();
builder.Services.AddScoped<CreateDsUserValidator>();
builder.Services.AddScoped<CreateCertAndPairKeyUseCase>();

var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

Host.CreateDefaultBuilder(args)
          .ConfigureLogging(logging =>
          {
              logging.ClearProviders();
              logging.AddLog4Net(); // Esto es necesario si estás usando la integración con Microsoft.Extensions.Logging
          })
          .ConfigureWebHostDefaults(webBuilder =>
          {
              webBuilder.UseStartup<Program>();
          });

builder.Services.AddControllers(options =>
   {
       options.Filters.Add<CustomGlobalExceptionHandler>();
   });

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Aquí puedes modificar las opciones de serialización
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;  // O null para no cambiar nombres a camelCase
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;  // Ignorar propiedades nulas
    });

builder.Services.AddDbContext<AppDbContext>(opt =>
    //opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
    opt.UseInMemoryDatabase("MyBdMemory"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<LoggingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
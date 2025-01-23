using MyMiddleWareExceptions;
using WebAPIProject.Interface;
using WebAPIProject.Service;
using WebAPIProject.services;
using Serilog;
using MyLoggerMiddleWare;


var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, config) =>
{
    config
        .WriteTo.Console() // כתיבה ל-Console
        .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day) // כתיבה לקובץ, מתחדש כל יום
        .MinimumLevel.Debug(); // רמת לוג מינימלית
});
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddJobFinderService();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMyLoggerMiddleWare();
app.useMyMiddleWareExceptions();

//app.use(())

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("index.html");

app.Run();
  
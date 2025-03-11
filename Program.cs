using MyMiddleWareExceptions;
using Serilog;
using MyLoggerMiddleWare;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using WebAPIProject.Services;
using WebAPIProject.Interface;
using WebAPIProject.services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTokenService();
builder.Services.AddControllers();
builder.Services.AddJobsService();
builder.Services.AddUsersService();
builder.Host.UseSerilog((context, config) =>
{
    config
        .WriteTo.Console()
        .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day) // כתיבה לקובץ, מתחדש כל יום
        .MinimumLevel.Debug();
});

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(cfg =>
    {
        using var provider = builder.Services.BuildServiceProvider();
        var tokenService = provider.GetRequiredService<ITokenService>();
        
        var tokenParams = tokenService.GetTokenValidationParameters();
        tokenParams.RoleClaimType = "role";
        cfg.TokenValidationParameters = tokenParams;
    });

builder.Services.AddAuthorization(cfg =>
    {
        cfg.AddPolicy("SuperAdmin", policy => policy.RequireClaim("type", "SuperAdmin"));
        cfg.AddPolicy("Admin", policy => policy.RequireClaim("type", "Admin","SuperAdmin"));
        cfg.AddPolicy("User", policy => policy.RequireClaim("type", "User","Admin","SuperAdmin"));
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "WebAPIProject",
        Version = "v1",
        Description = "API for Job Finder"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                { new OpenApiSecurityScheme
                        {
                         Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer"}
                        },
                    new string[] {}
                }
                });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(3000);
    options.ListenLocalhost(3001, listenOptions => { listenOptions.UseHttps(); });
});

var app = builder.Build();

app.UseMyLoggerMiddleWare();
app.useMyMiddleWareExceptions();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowAll");

app.MapControllers();
app.MapFallbackToFile("Html/index.html");
app.Run();
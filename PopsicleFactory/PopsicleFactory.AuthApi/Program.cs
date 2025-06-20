using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PopsicleFactory.AuthApi.Entities;
using PopsicleFactory.AuthApi.Entities.Context;
using PopsicleFactory.AuthApi.Services;
using PopsicleFactory.Infrastructure.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) =>
    loggerConfig.ReadFrom.Configuration(context.Configuration));
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme() {
        Name = "Authorization", In = ParameterLocation.Header, Type = SecuritySchemeType.Http, Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement{
        { new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }}, Array.Empty<string>()}
    });
});

builder.Services.AddDbContext<UserContext>(opt => opt.UseInMemoryDatabase("InMem"));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["AppSettings:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["AppSettings:Audience"],
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:Token"]!)),
            ValidateIssuerSigningKey = true
        };
    });

builder.Services.AddScoped<IAuthService, AuthService>();


builder.Services.AddHealthChecks()
.AddCheck("default", () => HealthCheckResult.Healthy());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(ui =>
    {
        ui.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
        ui.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.UseCustomMiddlewares();

app.MapCustomHealthChecks();

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health");

UserDb.PrepPopulation(app);

app.Run();

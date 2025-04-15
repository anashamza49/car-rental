using Authentification.JWT.DAL.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Authentification.JWT.Service.Interfaces;
using Authentification.JWT.Service.Services;
using Authentification.JWT.Service.Mappings;
using Authentification.JWT.DAL.Repositories;
using NLog.Web;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Logging.ClearProviders();
builder.Host.UseNLog();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorOrigin",
        policy => policy.WithOrigins("https://localhost:7085")
            .AllowAnyHeader()
            .AllowAnyMethod());
});



builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        x => x.MigrationsAssembly("Authentification.JWT.DAL")));


builder.Services.AddScoped<IUserRepository, UserRepository>();


builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IJwtService, JwtService>();

builder.Services.AddScoped<ICarRepository, CarRepository>();
builder.Services.AddScoped<ICarService, CarService>();


builder.Services.AddAutoMapper(typeof(AuthProfile));


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("EmployeeOrAdmin", policy => policy.RequireRole("Admin", "Employee"));
});


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "JWT API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        In = ParameterLocation.Header,
        Description = "JWT Authorization: Bearer {token}"
    });

    c.AddSecurityRequirement(new()
    {
        {
            new() { Reference = new() { Type = ReferenceType.SecurityScheme, Id = "Bearer" } },
            Array.Empty<string>()
        }
    });
});

var logDir = Path.Combine(Directory.GetCurrentDirectory(), "logs");
Directory.CreateDirectory(Path.Combine(logDir, "all"));
Directory.CreateDirectory(Path.Combine(logDir, "errors"));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorApp",
        policy => policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var app = builder.Build();

NLog.Common.InternalLogger.LogToConsole = true;


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowBlazorApp");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();

app.MapControllers();

app.Run();

using AspNetCoreRateLimit;
using ContaVida.MVC.DataAccess.DataAccess;
using ContaVida.MVC.EmailSender;
using ContaVida.MVC.Models.Email;
using ContaVida.MVC.Server;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvc()
        .AddSessionStateTempDataProvider();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession();


builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();

var emailConfig = builder.Configuration
        .GetSection("EmailConfiguration")
        .Get<EmailConfigurationModel>();
builder.Services.AddScoped<IEmailSender, EmailSender>();

builder.Services.AddSingleton(emailConfig);

// Add services to the container.
builder.Services.InjectServices();

builder.Services.AddHttpContextAccessor();

builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();


//this is now obsolete in .NET 10
//builder.Services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
builder.Services.AddMemoryCache();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "ContaVida API " + builder.Environment.EnvironmentName, Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
});

var securityKey = builder.Configuration["security:JWT_PrivateKey"];
var encoding = Encoding.UTF8.GetBytes(securityKey);
var mySecurityKey = new SymmetricSecurityKey(encoding);
var issuer = builder.Configuration["security:issuer"];
var audience = builder.Configuration["security:audience"];

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        RequireExpirationTime = true,
        ValidIssuer = builder.Configuration["security:issuer"],
        ValidAudience = builder.Configuration["security:audience"],
        IssuerSigningKey = mySecurityKey,
        //https://stackoverflow.com/questions/43045035/jwt-token-authentication-expired-tokens-still-working-net-core-web-api
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddDbContext<ContaVidaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("dbConnection")));

var app = builder.Build();

app.UseIpRateLimiting();

app.UseDefaultFiles();
app.MapStaticAssets();
app.UseSwagger();
app.UseSwaggerUI();

app.UseSession();
// Configure the HTTP request pipeline.
app.UseAuthentication();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();

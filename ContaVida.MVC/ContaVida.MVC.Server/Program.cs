using ContaVida.MVC.DataAccess.DataAccess;
using ContaVida.MVC.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.InjectServices();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "ContaVida API " + builder.Environment.EnvironmentName, Version = "v1" });
});

builder.Services.AddDbContext<ContaVidaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("dbConnection")));

var app = builder.Build();

app.UseDefaultFiles();
app.MapStaticAssets();
app.UseSwagger();
app.UseSwaggerUI();

// Configure the HTTP request pipeline.
app.MapOpenApi();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();

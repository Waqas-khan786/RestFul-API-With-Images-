using DataAccessLayer.DatabaseContext;
using DataAccessLayer.IServices;
using DataAccessLayer.Services;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var IWebHostEnvironment = builder.Environment;
var configuration = builder.Configuration;
string Jwtkey = builder.Configuration.GetSection("Jwt").GetSection("Key").Value;

// Add services to the container.

builder.Services.AddDbContext<AppDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DBConnection"));
});

builder.Services.AddScoped<IEmployee, EmployeeService>();
builder.Services.AddScoped<IOrders, OrdersService>();
builder.Services.AddScoped<IUsers, UsersService>();
builder.Services.AddSingleton<Helper>(new Helper(IWebHostEnvironment, configuration));
builder.Services.AddSingleton<AuthenticationHelper>(new AuthenticationHelper(configuration));

builder.Services.AddControllers();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(Jwtkey)),
    };
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

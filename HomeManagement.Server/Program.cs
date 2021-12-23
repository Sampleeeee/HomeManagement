using System.Text;
using HomeManagement.Server.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder( args );
var services = builder.Services;

// Add services to the container.

services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddCors( options =>
{
    options.AddPolicy( "EnableCORS", builder =>
    {
        builder.AllowAnyOrigin();
        builder.AllowAnyHeader();
        builder.AllowAnyMethod();
    } );
} );

services.AddAuthentication( options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
} ).AddJwtBearer( options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = "https://localhost:5001",
        ValidAudience = "https://localhost:5001",

        // TODO Replace key
        IssuerSigningKey = new SymmetricSecurityKey( Encoding.UTF8.GetBytes( "fake ass key but apparently it needs to be a lot longer for it to work" ) )
    };
} );

string connectionString = builder.Environment.IsDevelopment()
        ? builder.Configuration.GetConnectionString( "Development" )
        : builder.Configuration.GetConnectionString( "Production" );

Console.WriteLine( connectionString );

services.AddDbContext<DatabaseContext>( options => options.UseSqlServer( connectionString ) );

var app = builder.Build();

// Configure the HTTP request pipeline.
if ( app.Environment.IsDevelopment() )
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors( "EnableCORS" );

app.MapControllers();

app.Run();
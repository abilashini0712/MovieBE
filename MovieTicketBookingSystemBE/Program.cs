
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MovieTicketBookingSystemBE.Data;
using MovieTicketBookingSystemBE.Services;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database configuration
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Email service
builder.Services.Configure<EmailSetting>(
    builder.Configuration.GetSection("EmailSettings"));

builder.Services.AddScoped<IEmailService, EmailService>();

//// JWT service
//builder.Services.AddScoped<JwtService>();

//// JWT settings
//var jwtSettings =
//    builder.Configuration.GetSection("JwtSettings");

//var jwtKey = jwtSettings["Key"]
//    ?? throw new InvalidOperationException(
//        "JWT Key is missing from configuration.");

//builder.Services
//    .AddAuthentication(options =>
//    {
//        options.DefaultAuthenticateScheme =
//            JwtBearerDefaults.AuthenticationScheme;

//        options.DefaultChallengeScheme =
//            JwtBearerDefaults.AuthenticationScheme;
//    })
//    .AddJwtBearer(options =>
//    {
//        options.TokenValidationParameters =
//            new TokenValidationParameters
//            {
//                ValidateIssuer = true,
//                ValidIssuer = jwtSettings["Issuer"],

//                ValidateAudience = true,
//                ValidAudience = jwtSettings["Audience"],

//                ValidateIssuerSigningKey = true,
//                IssuerSigningKey = new SymmetricSecurityKey(
//                    Encoding.UTF8.GetBytes(jwtKey)),

//                ValidateLifetime = true,
//                ClockSkew = TimeSpan.FromMinutes(1),

//                RoleClaimType = ClaimTypes.Role,
//                NameClaimType = ClaimTypes.NameIdentifier
//            };
//    });

//// Require authentication for all endpoints by default
//builder.Services.AddAuthorization(options =>
//{
//    options.FallbackPolicy =
//        new AuthorizationPolicyBuilder()
//            .RequireAuthenticatedUser()
//            .Build();
//});

var app = builder.Build();

// HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Authentication MUST come before authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

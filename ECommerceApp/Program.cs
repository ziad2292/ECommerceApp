using Application.Settings;
using Domain.IdentityEntities;
using ECommerceApp.Extensions;
using Infrastructure;
using Infrastructure.Persistence._Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();



//Configure JWT Settings
var secretKey = Environment.GetEnvironmentVariable("SECRET_KEY");
builder.Services.Configure<JWTSettings>(options =>
{
    builder.Configuration.GetSection("JwtSettings").Bind(options);

    // Override the secret with environment variable
    options.Secret = secretKey;
});
var jwtSettings = builder.Configuration
    .GetSection("JwtSettings")
    .Get<JWTSettings>();
if(jwtSettings != null) jwtSettings.Secret = secretKey;



// Dependency Injection for Solution Layers
builder.Services.AddInfrastructure(builder.Configuration);



//Enable Identity
builder.Services.AddIdentity<User, Role>(options =>
{
    options.User.RequireUniqueEmail = true;

    //Password Complexity Configuration
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequiredUniqueChars = 1;
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders() //Generate Token for reset password, change email, etc
    .AddUserStore<UserStore<User, Role, AppDbContext, Guid>>()
    .AddRoleStore<RoleStore<Role, AppDbContext, Guid>>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; //How Asp.Net will authenticate the user
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; //How will Asp.Net will respond when authentication fails
})
    .AddJwtBearer(op =>
    {
        op.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,             // Check that token's issuer matches our expected Issuer
            ValidateAudience = true,           // Check that token's audience matches our expected Audience
            ValidateLifetime = true,           // Ensure token hasn't expired
            ValidIssuer = jwtSettings!.Issuer, // Our configured issuer (from appsettings.json or env vars)
            ValidAudience = jwtSettings.Audience, // Our configured audience
            ValidateIssuerSigningKey = true,   // Check token signature
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret!)) // Use our secret key to verify signature
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build(); //Require User to be Authenticcated for all endpoints by default
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

app.UseHsts(); //Forces the browser to use HTTPS for all requests and responses
app.UseHttpsRedirection();

//Order matters
app.UseRouting(); //Identifying action method based on route
app.UseAuthentication(); //Enable Authentication Middleware
app.UseAuthorization(); //Enable Authorization Middleware
app.MapControllers(); //Execute the filter pipeline (action + filters)


await app.InitializeDbAsync();

app.Run();

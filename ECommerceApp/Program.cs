using Domain.IdentityEntities;
using ECommerceApp.Extensions;
using Infrastructure;
using Infrastructure.Persistence._Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Dependency Injection for Solution Layers
builder.Services.AddInfrastructure(builder.Configuration);

//Enable Identity
builder.Services.AddIdentity<User, Role>(options =>
{
    //Password Complexity Configuration
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequiredUniqueChars = 1;
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders()
    .AddUserStore<UserStore<User, Role, AppDbContext, Guid>>()
    .AddRoleStore<RoleStore<Role, AppDbContext, Guid>>();


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

app.UseAuthentication(); //Reading Identity Cookie
app.UseRouting(); //Identifying action method based on route
app.MapControllers(); //Execute the filter pipeline (action + filters)

await app.InitializeDbAsync();

app.Run();

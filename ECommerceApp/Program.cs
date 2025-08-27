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
builder.Services.AddIdentity<User, Role>()
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

app.UseRouting();

app.MapControllers();

await app.InitializeDbAsync();

app.Run();

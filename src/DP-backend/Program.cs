using System.Reflection;
using DP_backend;
using DP_backend.Configurations;
using DP_backend.Database;
using DP_backend.Domain.Identity;
using DP_backend.FileStorage;
using DP_backend.Services.Initialization;
using Mapster;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwaggerGen();

services.InitInternalServices(configuration);

builder.AddDb<ApplicationDbContext>("DbConnection");
services.AddDefaultIdentity<User>(options =>
    {
        options.User.AllowedUserNameCharacters = string.Empty; 
    })
    .AddRoles<Role>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager<SignInManager<User>>()
    .AddUserManager<UserManager<User>>()
    .AddRoleManager<RoleManager<Role>>();
//services.AddFileStorage(configuration); закоментил, пока нету секретного ключа

builder.ConfigureJwtAuthentication();
builder.ConfigureClaimAuthorization();

builder.Services.AddMapster();
TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());

var allowAnyCorsPolicy = "AllowAnyCorsPolicy";
builder.Services.AddCors(options => { options.AddPolicy(name: allowAnyCorsPolicy, configurePolicy: builder => { builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); }); });

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); }
);

app.MigrateDBWhenNecessary<ApplicationDbContext>();
app.UseStaticFiles();
app.UseRouting();
app.UseCors(allowAnyCorsPolicy);
app.UseHttpsRedirection();

app.UseExceptionMiddleware();

app.UseAuthentication();
app.UseAuthorization();
RoleInitializer.Initialize(app.Services, configuration);
app.MapControllers();

app.Run();

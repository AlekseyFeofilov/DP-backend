using System.Reflection;
using DP_backend;
using DP_backend.Configurations;
using DP_backend.Configurators;
using DP_backend.Domain.Identity;
using DP_backend.Services.Initialization;
using DP_backend.Swagger;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
    options.EnableAnnotations();
    options.OperationFilter<SecurityRequirementsOperationFilter>();
    options.OperationFilter<DefaultResponseOperationFilter>();
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    
});

services.InitInternalServices(configuration);
builder.AddDb<ApplicationDbContext>("DbConnection");
services.AddDefaultIdentity<User>()
    .AddRoles<Role>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager<SignInManager<User>>()
    .AddUserManager<UserManager<User>>()
    .AddRoleManager<RoleManager<Role>>();

builder.ConfigureJwtAuthentication();
builder.ConfigureClaimAuthorization();

builder.Services.AddMapster();
TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());
var allowAnyCorsPolicy = "AllowAnyCorsPolicy";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: allowAnyCorsPolicy, configurePolicy: builder =>
    {
        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
}
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
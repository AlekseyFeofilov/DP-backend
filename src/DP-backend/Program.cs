using System.Reflection;
using DP_backend.Configurations;
using DP_backend.Database;
using DP_backend.Domain.Identity;
using DP_backend.Domain.Templating.Employment;
using DP_backend.FileStorage;
using DP_backend.Services.Initialization;
using DP_backend.Templating;
using Mapster;
using Microsoft.AspNetCore.Identity;using TemplateEngine.Docx;


var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwaggerGen();

services.InitInternalServices(configuration);

builder.AddDb<ApplicationDbContext>("DbConnection");
services.AddDefaultIdentity<User>(options => { options.User.AllowedUserNameCharacters = string.Empty; })
    .AddRoles<Role>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager<SignInManager<User>>()
    .AddUserManager<UserManager<User>>()
    .AddRoleManager<RoleManager<Role>>();
services.AddFileStorage(configuration);
services.AddLazyCache();

builder.ConfigureJwtAuthentication();
builder.ConfigureClaimAuthorization();

builder.Services.AddMapster();
TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());

var allowAnyCorsPolicy = "AllowAnyCorsPolicy";
builder.Services.AddCors(options => { options.AddPolicy(name: allowAnyCorsPolicy, configurePolicy: builder => { builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); }); });


var app = builder.Build();

app.MigrateDBWhenNecessary<ApplicationDbContext>();
await RoleInitializer.Initialize(app.Services, configuration);
await DbDictionariesInitializer.Initialize(app.Services, configuration);


// var serviceScope = app.Services.CreateScope();
//
// var documentTemplatesService = serviceScope.ServiceProvider.GetRequiredService<DocumentTemplatesService>();
// var storageService = serviceScope.ServiceProvider.GetRequiredService<IObjectStorageService>();
//
// InternshipDiaryTemplate.CreateFor5Semester()
//
// documentTemplatesService.AddDocumentTemplate()


// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); });

app.UseStaticFiles();
app.UseRouting();
app.UseCors(allowAnyCorsPolicy);
app.UseHttpsRedirection();

app.UseExceptionMiddleware();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
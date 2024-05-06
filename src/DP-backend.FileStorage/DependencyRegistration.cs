using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;

namespace DP_backend.FileStorage;

public static class DependencyRegistration
{
    public static IServiceCollection AddFileStorage(this IServiceCollection services, IConfiguration configuration)
    {
        var minioSection = configuration.GetSection("Minio");
        services.AddMinio(client =>
        {
            var url = new Uri(minioSection["ServiceUrl"]!);
            client.WithEndpoint(url)
                .WithSSL(secure: false)
                .WithCredentials(minioSection["AccessKey"], minioSection["SecretKey"]);
        });

        services.Configure<FileStorageConfiguration>(configuration.GetSection("FileStorage"));

        services.AddScoped<IObjectStorageService, MinioStorageService>();
        services.AddScoped<IFileLinkService, FileLinkService>();
        
        // todo : всё-таки заставить это работать 
        // var awsOptions = configuration.GetAWSOptions("AWS");
        // awsOptions.Credentials = new BasicAWSCredentials(configuration["AWS:AccessKey"], configuration["AWS:SecretKey"]);
        // services.AddDefaultAWSOptions(awsOptions);
        // services.AddAWSService<IAmazonS3>();

        return services;
    }
}
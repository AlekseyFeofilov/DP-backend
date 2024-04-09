using DP_backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DP_backend.Configurations
{
    public static class AuthenticationConfiguration
    {
        public static void ConfigureJwtAuthentication(this WebApplicationBuilder? builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            var jwtSection = builder.Configuration.GetSection("JwtSettings");
            builder.Services.Configure<JwtConfigurations>(jwtSection);
            var jwtSettings = jwtSection.Get<JwtConfigurations>();
            if (jwtSettings == null)
            {
                throw new ArgumentException("Section Jwt was not found in application settings");
            }

            builder.ConfigureCommonAuthentication();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = jwtSettings.Issuer,
                    ValidateIssuer = true,

                    ValidAudience = jwtSettings.Audience,
                    ValidateAudience = true,

                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
                    ValidateIssuerSigningKey = true,

                    ValidateLifetime = true,
                    LifetimeValidator = (before, expires, token, parameters) =>
                    {
                        var utcNow = DateTime.UtcNow;
                        return before <= utcNow && utcNow < expires;
                    }
                };
            });
        }



        private static void ConfigureCommonAuthentication(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IJwtAuthService, JwtAuthService>();
        }
    }
}

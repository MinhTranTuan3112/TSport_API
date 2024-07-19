using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Filters;
using TSport.Api.Repositories;
using TSport.Api.Shared;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text.RegularExpressions;
using TSport.Api.Attributes;

namespace TSport.Api.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddApiDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllersWithConfigurations()
                    .AddRedisCacheConfigurations(configuration)
                    .AddDbContextWithConfigurations(configuration)
                    .AddSupabaseAuthenticationConfigurations(configuration)
                    .AddSwaggerConfigurations()
                    .AddCorsConfigurations();

            return services;
        }

        private static IServiceCollection AddRedisCacheConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration["Redis:DockerConnection"]!;
                options.InstanceName = configuration["Redis:InstanceName"]!;
            });
            
            return services;
        }

        private static IServiceCollection AddDbContextWithConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection")!;
            services.AddDbContext<TsportDbContext>(options =>
            options.UseSqlServer(connectionString));
            return services;
        }

        private static IServiceCollection AddSwaggerConfigurations(this IServiceCollection services)
        {

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme()
                {
                    Description =
                        @"JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below. Example: 'Bearer 12345example'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });



                options.OperationFilter<SecurityRequirementsOperationFilter>();
                options.SchemaFilter<ApplyKebabCaseNamingConvention>();
            });

            return services;
        }

        private static IServiceCollection AddAuthenticationServicesWithConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = configuration["JwtAuth:Issuer"],
                    ValidAudience = configuration["JwtAuth:Audience"],
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtAuth:Key"]!))
                };
            });
            return services;
        }

        private static IServiceCollection AddSupabaseAuthenticationConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Supabase:SecretKey"]!)),
                    ValidAudience = configuration["Supabase:Audience"],
                    ValidIssuer = configuration["Supabase:Issuer"],
                };
            });
            return services;
        }

        private static IServiceCollection AddControllersWithConfigurations(this IServiceCollection services)
        {
            services.AddControllers(
                options => options.ModelBinderProviders.Insert(0, new CurrentAccountModelBinderProvider())
            ).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                options.SerializerSettings.Converters.Add(new DateOnlyJsonConverter());
                options.SerializerSettings.ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new KebabCaseNamingStrategy()
                };
            });
            return services;
        }

        private static IServiceCollection AddCorsConfigurations(this IServiceCollection services)
        {
            services.AddCors(options =>
                options.AddPolicy("AllowAll", b => b.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod()));
            return services;
        }

    }

    internal class ApplyKebabCaseNamingConvention : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema?.Properties?.Count == 0)
            {
                return;
            }

            if (schema is null)
            {
                return;
            }

            var properties = schema.Properties;
            schema.Properties = new Dictionary<string, OpenApiSchema>();

            foreach (var property in properties)
            {
                var kebabCaseProperty = ToKebabCase(property.Key);
                schema.Properties.Add(kebabCaseProperty, property.Value);
            }
        }

        private string ToKebabCase(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            return Regex.Replace(value, "(?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z])", "-$1").ToLower();
        }
    }
}
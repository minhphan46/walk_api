﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using WalkProject.API.RestFul.Repositories.Implements;
using WalkProject.API.RestFul.Repositories.Interfaces;
using WalkProject.DataModels.DbContexts;
using System.Text.Json.Serialization;

namespace WalkProject.API.RestFul.AppServices
{
    public class RestfulAppServices
    {
        public static WebApplicationBuilder AppBuilder(WebApplicationBuilder builder)
        {
            // Configuration
            builder.Services.AddControllers();
            builder.Services.AddControllers().AddJsonOptions(x =>
                            x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "NZ Walks API", Version = "v1" });
                options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = JwtBearerDefaults.AuthenticationScheme
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            },
                            Scheme = "Oauth2",
                            Name = JwtBearerDefaults.AuthenticationScheme,
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
            });

            // DBContext
            builder.Services.AddScoped<NZWalksDbContext>(p =>
                p.GetRequiredService<IDbContextFactory<NZWalksDbContext>>().CreateDbContext()
            );

            // Repositories
            builder.Services.AddScoped<IRegionRepository, SQLRegionRepository>();
            builder.Services.AddScoped<IWalkRepository, SQLWalkRepository>();
            builder.Services.AddScoped<ITokenRepository, TokenRepository>();
            builder.Services.AddScoped<IImageRepository, LocalImageRepository>();
            builder.Services.AddScoped<IDifficultyRepository, SQLDifficultyRepository>();
            builder.Services.AddScoped<ICategoryRepository, SQLCategoryRepository>();

            return builder;
        }
    }
}

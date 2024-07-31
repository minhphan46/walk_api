using FirebaseAdmin;
using FirebaseAdminAuthentication.DependencyInjection.Extensions;
using Google.Apis.Auth.OAuth2;
using WalkProject.Authentication.Authorization.Rules;
using WalkProject.Authentication.Authorization;
using WalkProject.Utils;
using Microsoft.AspNetCore.Authorization;

namespace WalkProject.Authentication.AppServices
{
    public class AuthAppServices
    {
        public static WebApplicationBuilder AppBuilder(WebApplicationBuilder builder)
        {
            // Authorization
            builder.Services.AddSingleton<IAuthorizationHandler, IsAdminHandler>();

            // Authentication
            var firebaseConfigPath = builder.Configuration.GetValue<string>("FIREBASE_CONFIG");

            builder.Services.AddSingleton(FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile(firebaseConfigPath)
            }));
            builder.Services.AddFirebaseAuthentication();
            builder.Services.AddAuthorization(
                o => o.AddPolicy("IsAdmin", p => p.AddRequirements(
                        new IsAllowDeleted()
                    ))
            );

            builder.Services.AddHttpClient<JwtProvider>((sp, HttpClient) =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();

                HttpClient.BaseAddress = new Uri(configuration["Authentication:TokenUri"]);
            });


            // Authentication
            //builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //.AddJwtBearer(options =>
            //options.TokenValidationParameters = new TokenValidationParameters
            //{
            //    ValidateIssuer = true,
            //    ValidateAudience = true,
            //    ValidateLifetime = true,
            //    ValidateIssuerSigningKey = true,
            //    ValidIssuer = builder.Configuration["Jwt:Issuer"],
            //    ValidAudience = builder.Configuration["Jwt:Audience"],
            //    IssuerSigningKey = new SymmetricSecurityKey(
            //        Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
            //});


            return builder;
        }
    }
}

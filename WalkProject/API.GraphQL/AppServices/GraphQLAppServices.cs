using AppAny.HotChocolate.FluentValidation;
using FirebaseAdmin;
using FirebaseAdminAuthentication.DependencyInjection.Extensions;
using FirebaseAdminAuthentication.DependencyInjection.Models;
using FluentValidation.AspNetCore;
using Google.Apis.Auth.OAuth2;
using WalkProject.API.GraphQL.DataLoaders;
using WalkProject.API.GraphQL.DTOs.Categories;
using WalkProject.API.GraphQL.DTOs.Difficulties;
using WalkProject.API.GraphQL.DTOs.Regions;
using WalkProject.API.GraphQL.DTOs.Walks;
using WalkProject.API.GraphQL.Resolvers;
using WalkProject.API.GraphQL.Schemas.Mutations;
using WalkProject.API.GraphQL.Schemas.Queries;
using WalkProject.API.GraphQL.Schemas.Subscriptions;
using WalkProject.API.GraphQL.Validators;
using WalkProject.Utils;
using NZWalks.GraphQL.DataLoaders;

namespace WalkProject.API.GraphQL.AppServices
{
    public class GraphQLAppServices
    {
        public static WebApplicationBuilder AppBuilder(WebApplicationBuilder builder)
        {
            // Resolver
            builder.Services.AddScoped<CategoriesResolver>();
            builder.Services.AddScoped<DifficultiesResolver>();
            builder.Services.AddScoped<RegionsResolver>();
            builder.Services.AddScoped<WalksResolver>();
            builder.Services.AddScoped<WalkCategoriesResolver>();
            builder.Services.AddScoped<AuthenticationResolver>();

            // DataLoader
            builder.Services.AddScoped<DifficultyDataLoader>();
            builder.Services.AddScoped<RegionDataLoader>();
            builder.Services.AddScoped<WalkDataLoader>();
            builder.Services.AddScoped<WalkCategorieDataLoader>();

            // validation
            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddTransient<CategoryInputValidator>();

            // QueryType
            builder.Services
                .AddGraphQLServer()
                .AddType<WalkResponse>()
                .AddType<DifficultyResponse>()
                .AddType<RegionResponse>()
                .AddType<CategoryResponse>()
                .AddQueryType(q => q.Name("Query"))
                .AddType<CategoriesQuery>()
                .AddType<DifficultyQuery>()
                .AddType<RegionQuery>()
                .AddType<WalkQuery>()
                .AddType<SearchQuery>()
                .AddType<UserQuery>()
                .AddMutationType(q => q.Name("Mutation"))
                .AddMutationConventions()
                .AddType<CategoriesMutation>()
                .AddType<AuthenticationMutation>()
                .AddSubscriptionType(s => s.Name("Subscription"))
                .AddType<CategoriesSubscription>()
                .AddFiltering()
                .AddSorting()
                .AddInMemorySubscriptions()
                .AddAuthorization()
                .AddFluentValidation(o =>
                {
                    o.UseDefaultErrorMapper();
                });

            // Authentication
            var firebaseConfigPath = builder.Configuration.GetValue<string>("FIREBASE_CONFIG");

            builder.Services.AddSingleton(FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile(firebaseConfigPath)
            }));
            builder.Services.AddFirebaseAuthentication();
            builder.Services.AddAuthorization(
                o => o.AddPolicy("IsAdmin", p => p.RequireClaim(FirebaseUserClaimType.EMAIL, "test@test.com"))
            );

            builder.Services.AddHttpClient<JwtProvider>((sp, HttpClient) =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();

                HttpClient.BaseAddress = new Uri(configuration["Authentication:TokenUri"]);
            });

            return builder;
        }
    }
}

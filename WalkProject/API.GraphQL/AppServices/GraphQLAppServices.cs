﻿using AppAny.HotChocolate.FluentValidation;
using FluentValidation.AspNetCore;
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
            builder.Services.AddSingleton<UsersResolver>();

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
                .AddType<DifficultyMutation>()
                .AddType<RegionMutation>()
                .AddType<WalkMutation>()
                .AddType<AuthenticationMutation>()
                .AddType<UserMutation>()
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


            return builder;
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using WalkProject.API.GraphQL.AppServices;
using WalkProject.API.RestFul.AppServices;
using WalkProject.DataModels.DbContexts;
using WalkProject.Loggers;
using WalkProject.Middlewares;
using WalkProject.Utils;

var builder = WebApplication.CreateBuilder(args);

// Database
builder.Services.AddPooledDbContextFactory<NZWalksDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString")).LogTo(Console.WriteLine)
);

builder.Services.AddHttpContextAccessor();

// AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

// builder 
builder = LoggerServices.AppBuilder(builder);

builder = RestfulAppServices.AppBuilder(builder);

builder = GraphQLAppServices.AppBuilder(builder);


// App builder
var app = builder.Build();

//if (app.Environment.IsDevelopment()){}
app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseWebSockets();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "./Resources/Images")),
    RequestPath = "/Resources/Images"
});

app.MapControllers();

app.MapGraphQL();

app.Run();
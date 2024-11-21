using HC14Test.Authorization;
using HC14Test.Models;
using HotChocolate.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPooledDbContextFactory<AdventureWorks2022Context>(
    options => options
    .UseSqlServer("Data Source=DESKTOP-92K3UB0;" +
    "Initial Catalog=AdventureWorks2022;" +
    "Integrated Security=SSPI;Encrypt=True;TrustServerCertificate=True"));

builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();


builder.Services.AddSingleton<IAuthorizationHandler, ReadHandler>();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();


builder.Services
    .AddGraphQLServer()
    .AddAuthorization()
    .AddHttpRequestInterceptor<HttpRequestInterceptor>()
    //.RegisterDbContext<AdventureWorks2022Context>(DbContextKind.Pooled)
    .AddProjections()
    .AddFiltering()
    .AddSorting()
    .AddQueryType<Query>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapGraphQL().WithOptions(new GraphQLServerOptions()
{
    Tool = { ServeMode = GraphQLToolServeMode.Embedded }
});
app.Run();

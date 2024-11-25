using HC14Test.Authorization;
using HC14Test.Models;
using HotChocolate.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;


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
    .AddOptions()
    .AddGraphQLServer()
    .DisableIntrospection(config.GetValue("DisableIntrospection", true))
    //adding this throws  "message": "The query request contains no document or no document id.",
    //.UseCostAnalyzer()
    .AddAuthorization()
    .AddHttpRequestInterceptor<HttpRequestInterceptor>()  
    .AddProjections()
    .AddFiltering()
    .AddSorting()
    //adding ModifyPagingOptions throws ' "message": "The maximum allowed type cost was exceeded.",'
    .ModifyPagingOptions(opt =>
    {
        opt.DefaultPageSize = config.GetValue("PagingOptions:DefaultPageSize", 10);
        opt.MaxPageSize = config.GetValue("PagingOptions:MaxPageSize", 100);
        opt.IncludeTotalCount = config.GetValue("PagingOptions:IncludeTotalCount", true);
        opt.AllowBackwardPagination = config.GetValue("PagingOptions:AllowBackwardPagination", true);
        opt.RequirePagingBoundaries = config.GetValue("PagingOptions:RequirePagingBoundaries", true);
    })
    .AddQueryType<Query>();

var app = builder.Build();

//app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapGraphQL().WithOptions(new GraphQLServerOptions()
{
    Tool = { ServeMode = GraphQLToolServeMode.Embedded }
});
app.Run();

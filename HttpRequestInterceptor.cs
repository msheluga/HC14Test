using HotChocolate.AspNetCore;
using HotChocolate.Execution;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace HC14Test
{
    public class HttpRequestInterceptor : DefaultHttpRequestInterceptor
    {
        private readonly IPolicyEvaluator _policyEvaluator;
        private readonly IAuthorizationPolicyProvider _authorizationPolicyProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;
       



        public HttpRequestInterceptor(
            IHttpContextAccessor httpContextAccessor,
            IPolicyEvaluator policyEvaluator,
            IAuthorizationPolicyProvider authorizationPolicyProvider
            )
        {
            _httpContextAccessor = httpContextAccessor;
            _policyEvaluator = policyEvaluator;
            _authorizationPolicyProvider = authorizationPolicyProvider;
        }

        public override async ValueTask OnCreateAsync(HttpContext context, IRequestExecutor requestExecutor, OperationRequestBuilder requestBuilder, CancellationToken cancellationToken)
        {
           // var queryRequest = requestBuilder.Create();
            //inspeact the context and look at the 
            var identity = new ClaimsIdentity();
            //go get the claims 
            if (context.Request.Headers.TryGetValue("Authorization", out var authValue))
            {
                //validate the claim against the value
                if (!String.IsNullOrEmpty(authValue.ToString()))
                {
                    PopulateClaims(context, requestExecutor, requestBuilder);
                }
            }
            await _policyEvaluator.AuthenticateAsync(await _authorizationPolicyProvider.GetDefaultPolicyAsync(), context);
            await base.OnCreateAsync(context, requestExecutor, requestBuilder, cancellationToken);
        }

        private void PopulateClaims(HttpContext context, IRequestExecutor requestExecutor, OperationRequestBuilder requestBuilder)
        {
            var appId = context.Request.Headers["Authorization"];
            if (appId.ToString() == null)
            {
                return;
            }
            var userIds = new List<string>
            {
                "C8104FC7-8643-4908-B88E-61AA6A3FCF36",
                "4E74C96B-D428-4A76-A518-E412C770D951",
                "7D4F3DEA-92AA-4B98-B5EB-887A246FA566"
            };
            if (!userIds.Contains(appId.ToString(), StringComparer.OrdinalIgnoreCase))
            {
                return;
            }

            var principal = context.User.Identity as ClaimsIdentity;
            var claims = new List<Claim>()
            {
                new Claim (ClaimTypes.Name,appId.ToString()),
                new Claim("AdventureWorks2022.Person.Address.Read", "AddressID, City, PostalCode, rowguid")
            };
            principal.AddClaims(claims);
        }
    }
}

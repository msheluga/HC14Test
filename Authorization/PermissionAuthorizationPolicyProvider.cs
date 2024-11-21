using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace HC14Test.Authorization
{
    public class PermissionAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        public PermissionAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options) : base(options) { }

        public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            if (!policyName.StartsWith("AdventureWorks2022"))
            {
                return await base.GetPolicyAsync(policyName);
            }

            var requirement = new ReadRequirement(policyName);

            return new AuthorizationPolicyBuilder().AddRequirements(requirement).Build();
        }
    }
}

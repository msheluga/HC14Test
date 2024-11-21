using Microsoft.AspNetCore.Authorization;

namespace HC14Test.Authorization
{
    public class ReadHandler :AuthorizationHandler<ReadRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ReadRequirement requirement)
        {
            string[] claimParts = requirement.Claim.Split(".");
            string databaseName = claimParts[0];
            string schemaName = claimParts[1];
            string tblName = claimParts[2];
            string fieldName = claimParts[3];
            string perm = claimParts[4];
            string claimType = $"{databaseName}.{schemaName}.{tblName}.{perm}";

            var readClaim = context.User.Claims.FirstOrDefault(x => x.Type == claimType);

            if (readClaim is null)
            {
                return Task.CompletedTask;
            }

            if (readClaim.Value.Contains(fieldName))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}

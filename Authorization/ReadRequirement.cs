using Microsoft.AspNetCore.Authorization;

namespace HC14Test.Authorization
{
    public class ReadRequirement : IAuthorizationRequirement
    {
        public string Claim { get; }

        public ReadRequirement(string claim)
        {
            if (claim.Length == 0)
                throw new ArgumentException("Claim is required", nameof(claim));

            Claim = claim;
        }
    }
}

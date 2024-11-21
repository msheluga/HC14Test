using HotChocolate.Authorization;

namespace HC14Test.Authorization
{
    public class PermissionAuthorizeAttribute : AuthorizeAttribute
    {
        public PermissionAuthorizeAttribute(string policy)
        {
            Policy = policy;
        }
    }
}

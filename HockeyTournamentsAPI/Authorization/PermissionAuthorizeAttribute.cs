using HockeyTournamentsAPI.Core.Models;
using Microsoft.AspNetCore.Authorization;

namespace HockeyTournamentsAPI.Authorization
{
    public class PermissionAuthorizeAttribute : AuthorizeAttribute
    {
        public PermissionAuthorizeAttribute(RolePermissions rolePermissions)
        {
            Roles = rolePermissions.ToString().Replace(" ", string.Empty);
        }
    }
}

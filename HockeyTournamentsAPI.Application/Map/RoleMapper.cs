using System.ComponentModel;
using HockeyTournamentsAPI.Application.Contracts.Roles;
using HockeyTournamentsAPI.Core.Models;

namespace HockeyTournamentsAPI.Application.Map
{
    public static class RoleMapper
    {
        public static string Description(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attributes = (DescriptionAttribute[])field
                .GetCustomAttributes(typeof(DescriptionAttribute), false);
            return (attributes.Length > 0)
                ? attributes[0].Description
                : value.ToString();
        }

        public static RoleResponse ToResponse(this Role role)
        {
            var response = new RoleResponse(
                (int)role, role.Description());
            return response;
        }
    }
}

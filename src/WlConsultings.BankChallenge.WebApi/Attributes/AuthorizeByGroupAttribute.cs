using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

public class AuthorizeByGroupAttribute : AuthorizeAttribute, IAuthorizationFilter
{
    private readonly string _group;

    public AuthorizeByGroupAttribute(string group)
    {
        _group = group;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;

        if (!user.Identity.IsAuthenticated)
        {
            context.Result = new Microsoft.AspNetCore.Mvc.UnauthorizedResult();
            return;
        }

        var groupClaim = user.Claims.FirstOrDefault(c => c.Type == "Group")?.Value;

        if (groupClaim == null || groupClaim != _group)
        {
            context.Result = new Microsoft.AspNetCore.Mvc.ForbidResult();
        }
    }
}
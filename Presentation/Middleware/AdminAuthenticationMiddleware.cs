using Microsoft.AspNetCore.Authorization;

namespace Presentation.Middleware
{
    public class AdminAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public AdminAuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {

            var endpoint = context.GetEndpoint();

            if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
            {
                // Allow access to pages that don't have the [Authorize] attribute
                await _next(context);
                return;
            }

            var path = context.Request.Path.Value;

            if (!context.User.Identity.IsAuthenticated && path.StartsWith("/Admin") && !path.StartsWith("/Admin/Authentication/Login"))
            {
                // User is not authenticated and trying to access a page with [Authorize]
                context.Response.Redirect("/Admin/Authentication/Login");
                return;
            }

            await _next(context);
        }
    }
}

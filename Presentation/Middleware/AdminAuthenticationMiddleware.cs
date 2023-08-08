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

        public async Task Invoke(HttpContext context)
        {
            var endpoint = context.GetEndpoint();

            if (endpoint != null && RequiresAuthorization(endpoint))
            {
                if (!context.User.Identity.IsAuthenticated)
                {
                    context.Response.Redirect("/Admin/Authentication/Login");
                    return;
                }
                else
                {
                    // User is authenticated, proceed to the requested page
                    await _next(context);
                    return;
                }
            }

            await _next(context);
        }

        private bool RequiresAuthorization(Endpoint endpoint)
        {
            // Check if the endpoint metadata contains IAuthorizeData
            return endpoint.Metadata.GetMetadata<IAuthorizeData>() != null;
        }
    }
}

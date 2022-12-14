namespace Animato.Messaging.WebApi.Services;

using System.Security.Claims;
using Animato.Messaging.Application.Common.Interfaces;

public class HttpContextCurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor httpContextAccessor;

    public HttpContextCurrentUserService(IHttpContextAccessor httpContextAccessor) => this.httpContextAccessor = httpContextAccessor;

    public ClaimsPrincipal GetUser() => httpContextAccessor.HttpContext?.User;
    public void SetUser(ClaimsPrincipal user)
    {
        if (httpContextAccessor.HttpContext != null)
        {
            httpContextAccessor.HttpContext.User = user;
        }
    }
}


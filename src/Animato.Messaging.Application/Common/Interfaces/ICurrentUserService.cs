namespace Animato.Messaging.Application.Common.Interfaces;
using System.Security.Claims;

public interface ICurrentUserService
{
    ClaimsPrincipal GetUser();
    void SetUser(ClaimsPrincipal user);
}

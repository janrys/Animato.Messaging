namespace Animato.Messaging.Application.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Animato.Messaging.Application.Common;
using Animato.Messaging.Application.Common.Interfaces;
using Animato.Sso.Domain.Entities;
using Animato.Sso.Domain.Enums;
using SecurityClaims = System.Security.Claims;

public class ClaimFactory : IClaimFactory
{

    public IEnumerable<Claim> GenerateClaims(User user, AuthorizationMethod authorizationMethod, params ApplicationRole[] roles)
        => GenerateClaims(user, new AuthorizationMethod[] { authorizationMethod }, roles);

    public IEnumerable<Claim> GenerateClaims(User user, IEnumerable<AuthorizationMethod> authorizationMethods, params ApplicationRole[] roles)
    {
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Login),
            new Claim(ClaimTypes.Name, user.Login),
            new Claim("name", user.Name),
            new Claim("full_name", user.FullName),
            new Claim(ClaimTypes.Sid, user.Id.Value.ToString()),
            new Claim("last_changed", user.LastChanged.ToUniversalTime().ToString(GlobalOptions.DatePattern, GlobalOptions.Culture))
        };

        if (roles is not null && roles.Any())
        {
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r.Name)));
        }

        if (authorizationMethods is not null && authorizationMethods.Any())
        {
            claims.AddRange(authorizationMethods.Select(r => new Claim(ClaimTypes.AuthenticationMethod, r.Name)));
        }

        return claims;
    }
}

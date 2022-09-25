namespace Animato.Messaging.WebApi.Common;

public interface ITokenCommandBuilder
{
    Task RevokeToken(string token);
    Task RevokeAllTokens();
}

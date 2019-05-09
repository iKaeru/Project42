using System.Threading;
using System.Threading.Tasks;

namespace MemoryCardsAPI.Auth
{
    public interface IAuthenticator
    {
        Task<SessionState> AuthenticateAsync(string login, string password, CancellationToken cancellationToken);

        Task<SessionState> GetSessionAsync(string sessionId, CancellationToken cancellationToken);
    }
}
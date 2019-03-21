using WebApi.Models;

namespace WebApi.Repositiories
{
    public interface IAuthTokenRepository
    {
        AuthToken FindAuthToken(string id, string pin);

        AuthToken FindUnused(string email);

        void Create(AuthToken authToken);

        void Update(AuthToken authToken);
    }
}
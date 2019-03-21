using WebApi.Models;

namespace WebApi.Services
{
    public interface IEmailService
    {
        void Send(string email, string pin);
    }
}
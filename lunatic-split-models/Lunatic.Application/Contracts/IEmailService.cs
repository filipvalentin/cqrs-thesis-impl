
using Lunatic.Application.Models;


namespace Lunatic.Application.Contracts {
    public interface IEmailService {
        Task<bool> SendEmailAsync(Mail email);
    }
}

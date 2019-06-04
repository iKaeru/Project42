using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Models.User;

namespace Models.Token.Repositories
{
    public interface ITokensRepository
    {
        Task<PasswordResetToken> GetPasswordResetTokenAsync(string tokenValue);
        Task<bool> AddAsync(PasswordResetToken token);
        Task<bool> DeleteTokenAsync(PasswordResetToken token);
    }
}

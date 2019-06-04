using Models.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Models.Token.Services
{
    public interface ITokenService
    {
        Task<PasswordResetToken> GetPasswordResetTokenAsync(string tokenValue);
        Task<bool> AddPasswordResetTokenAsync(PasswordResetToken token);
    }
}

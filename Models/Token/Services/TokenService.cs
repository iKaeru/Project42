using System;
using System.Threading.Tasks;
using Models.Errors;
using Models.Token.Repositories;
using Models.User;

namespace Models.Token.Services
{
    public class TokenService : ITokenService
    {
        private readonly ITokensRepository repository;

        public TokenService(ITokensRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> AddPasswordResetTokenAsync(PasswordResetToken token)
        {
            try
            {
                await repository.AddAsync(token);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public async Task<PasswordResetToken> GetPasswordResetTokenAsync(string tokenValue)
        {
            var token = await repository.GetPasswordResetTokenAsync(tokenValue);
            if (token == null) throw new AppException("Такого токена нет");

            await repository.DeleteTokenAsync(token);

            if (token.validUntil < DateTime.Now)
            {
                throw new AppException("Время действия токена вышло");
            }

            return token;
        }
    }
}

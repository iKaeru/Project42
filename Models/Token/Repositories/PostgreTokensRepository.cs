using Models.Data;
using System;
using System.Threading.Tasks;

namespace Models.Token.Repositories
{
    public class PostgreTokensRepository : ITokensRepository
    {
        private InMemoryContext context;

        public PostgreTokensRepository(InMemoryContext context)
        {
            this.context = context;
        }

        public async Task<bool> AddAsync(PasswordResetToken token)
        {
            try
            {
                await context.PasswordResetTokens.AddAsync(token);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        public async Task<bool> DeleteTokenAsync(PasswordResetToken token)
        {
            if (token != null)
            {
                context.PasswordResetTokens.Remove(token);
                await context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<PasswordResetToken> GetPasswordResetTokenAsync(string tokenValue)
        {
            var token = await context.PasswordResetTokens.FindAsync(tokenValue);
            return token;
        }
    }
}

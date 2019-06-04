using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Data;
using Models.User;

namespace Models.Token.Repositories
{
    public class InMemoryTokensRepository : ITokensRepository
    {
        private InMemoryContext context;

        public InMemoryTokensRepository(InMemoryContext context)
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
            //return await context.PasswordResetTokens.FindAsync(tokenGuid);
            return context.PasswordResetTokens.FirstOrDefault(p => p.token == tokenValue);
        }
    }
}

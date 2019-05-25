using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models.CardItem;
using Models.Data;
using Models.Errors;

namespace Models.User.Repositories
{
    public class InMemoryUsersRepository : IUsersRepository
    {
        private InMemoryContext context;

        public InMemoryUsersRepository(InMemoryContext context)
        {
            this.context = context;
        }

        public async Task<UserInfo> CreateAsync(User userToAdd, CancellationToken cancellationToken)
        {
            var id = Guid.NewGuid();
            var user = userToAdd;
            user.Id = id;

            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            return user;
        }

        public async Task<User> SearchUserAsync(string username)
        {
            return await context.Users.SingleOrDefaultAsync(x => x.Login == username);
        }

        public async Task<User> GetUserAsync(Guid id)
        {
            return await context.Users.FindAsync(id);
        }
        
        public async Task<bool> FindLoginAsync(string login)
        {
            return await context.Users.AnyAsync(x => x.Login == login);
        }

        public async Task<bool> FindMailAsync(string mailAddress)
        {
            return await context.Users.AnyAsync(x => x.EmailAdress == mailAddress);
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            var user = await context.Users.FindAsync(id);
            if (user != null)
            {
                context.Users.Remove(user);
                context.SaveChanges();
                return true;
            }

            return false;
        }

        public Task<IReadOnlyList<CardItemInfo>> SearchAsync(UserSearchInfo query, CancellationToken cancelltionToken)
        {
            throw new NotImplementedException();
        }

        public async Task<User> UpdateAsync(User updateInfo)
        {
            var user = context.Users.Update(updateInfo);
            await context.SaveChangesAsync();
            return user.Entity;
        }
    }
}
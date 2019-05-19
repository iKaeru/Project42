using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Models.CardItem;

namespace Models.User.Repositories
{
    public class PostgreUsersRepository : IUsersRepository
    {
        public Task<UserInfo> CreateAsync(User userToCreate, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<User> SearchUserAsync(string username)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUserAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteUserAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<CardItemInfo>> SearchAsync(UserSearchInfo query, CancellationToken cancelltionToken)
        {
            throw new NotImplementedException();
        }

        public Task<UserInfo> GetAsync(Guid userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<User> UpdateAsync(User updateInfo)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(Guid userId, CancellationToken cancelltionToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> FindLoginAsync(string login)
        {
            throw new NotImplementedException();
        }

        public Task<bool> FindMailAsync(string mailAddress)
        {
            throw new NotImplementedException();
        }
    }
}
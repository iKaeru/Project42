using System;
using System.Collections.Generic;
using Model = Models.User;
using View = Client.Models.User;

namespace MemoryCardsAPI.Services
{
    public interface IUserService
    {
        Model.User Authenticate(string username, string password);
        IEnumerable<Model.User> GetAll();
        Model.User GetById(Guid id);
        Model.User Create(Model.User user, string password);
        void Update(Model.User user, string password = null);
        void Delete(int id);
    }
}
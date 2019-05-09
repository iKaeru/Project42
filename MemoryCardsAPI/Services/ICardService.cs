using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Model = Models.CardItem;
using View = Client.Models.CardItem;

namespace MemoryCardsAPI.Services
{
    public interface ICardService
    {
        IEnumerable<Model.CardItem> GetAll();
        Model.CardItem GetById(Guid id);
        Model.CardItem Create(Model.CardItem card);
        void Update(Model.CardItem card);
        void Delete(int id);
        Task AddAsync(Model.CardItem card);
        Task SaveChangesAsync();
    }
}
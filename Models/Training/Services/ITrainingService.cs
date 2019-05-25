using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Client.Models.Training;
using Models.CardItem;

namespace Models.Training.Services
{
    public interface ITrainingService
    {
        Training CreateTraining(Guid userId, Guid cardId);
        Training UpdateTraining(Training training, MemorizationBoxes box);
        Task<Training> AddToRepositoryAsync(Training training);
        Task<Training> GetTrainingAsync(CardItem.CardItem card, Guid userId);
        Task<Training> GetTrainingByIdAsync(Guid trainId, Guid userId);
        Task<List<Guid>> GetDateTrainingAsync(DateTime date, Guid uId);
        Task<int> GetCardsFromBoxAsync(MemorizationBoxes box, Guid uId);
        Task<bool> Delete(Guid id);
    }
}
